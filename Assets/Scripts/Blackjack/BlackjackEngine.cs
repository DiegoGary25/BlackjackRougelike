using System;
using System.Collections.Generic;
using System.Linq;
using HouseTakes21.Core;
using HouseTakes21.Data;
using HouseTakes21.Trinkets;

namespace HouseTakes21.Blackjack
{
    /// <summary>
    /// Core blackjack rules engine responsible for sequencing hands and payouts.
    /// </summary>
    public sealed class BlackjackEngine
    {
        private readonly GameResources resources;
        private readonly RNGService rng;
        private readonly List<Hand> playerHands = new() { new Hand(), new Hand() };
        private readonly Hand dealerHand = new();
        private readonly Dictionary<string, int> counters = new();

        private TrinketBus? trinketBus;
        private DealerSO? dealer;
        private Shoe? shoe;
        private ShoeConfig? shoeConfig;

        private int activeHandIndex;
        private bool autoPeek;
        private bool tableFirstDoubleConsumed;
        private bool tableColdPalmTriggered;
        private int winsThisTable;
        private int bustsThisTable;
        private int handsThisTable;
        private bool bustPreventedThisTurn;
        private int lastDealerTotal;
        private bool redThreadPending;
        private bool refundBustHp;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlackjackEngine"/> class.
        /// </summary>
        /// <param name="resources">Mutable resource tracker.</param>
        /// <param name="rng">Deterministic random service.</param>
        public BlackjackEngine(GameResources resources, RNGService rng)
        {
            this.resources = resources;
            this.rng = rng;
        }

        /// <summary>
        /// Gets the active player hand.
        /// </summary>
        public Hand CurrentHand => playerHands[activeHandIndex];

        /// <summary>
        /// Gets the dealer hand.
        /// </summary>
        public Hand DealerHand => dealerHand;

        /// <summary>
        /// Gets the player hands collection.
        /// </summary>
        public IReadOnlyList<Hand> PlayerHands => playerHands;

        /// <summary>
        /// Gets the current payout pending.
        /// </summary>
        public int CurrentPayout { get; private set; }

        /// <summary>
        /// Gets the granted favor for the current hand.
        /// </summary>
        public int GrantedFavor { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the player has resolved all hands.
        /// </summary>
        public bool IsHandComplete => playerHands.All(h => h.IsComplete);

        /// <summary>
        /// Gets a value indicating whether an auto-peek is active.
        /// </summary>
        public bool IsPeekActive { get; private set; }

        /// <summary>
        /// Gets the active resource tracker.
        /// </summary>
        public GameResources Resources => resources;

        /// <summary>
        /// Gets the dealer total from the last resolution.
        /// </summary>
        public int LastDealerTotal => lastDealerTotal;

        /// <summary>
        /// Gets or sets a value indicating whether the next initial draw should enforce odd/even parity.
        /// </summary>
        public bool RedThreadPending
        {
            get => redThreadPending;
            set => redThreadPending = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the next bust should refund health.
        /// </summary>
        public bool RefundBustHp
        {
            get => refundBustHp;
            set => refundBustHp = value;
        }

        /// <summary>
        /// Assigns the trinket bus for event dispatching.
        /// </summary>
        /// <param name="bus">Bus instance.</param>
        public void SetTrinketBus(TrinketBus bus)
        {
            trinketBus = bus;
        }

        /// <summary>
        /// Configures the dealer definition.
        /// </summary>
        /// <param name="dealer">Dealer data.</param>
        public void ConfigureDealer(DealerSO dealer)
        {
            this.dealer = dealer;
        }

        /// <summary>
        /// Configures the shoe for the run.
        /// </summary>
        /// <param name="config">Shoe configuration.</param>
        /// <param name="rngService">Random service.</param>
        public void ConfigureShoe(ShoeConfig config, RNGService rngService)
        {
            shoeConfig = config;
            List<CardSO> cards = StandardDeckBuilder.Create();
            shoe = new Shoe(cards, config, rngService);
        }

        /// <summary>
        /// Advances per frame logic.
        /// </summary>
        public void Tick()
        {
        }

        /// <summary>
        /// Resets hands after a table completes.
        /// </summary>
        public void ResetHands()
        {
            foreach (Hand hand in playerHands)
            {
                hand.Clear();
            }

            dealerHand.Clear();
            activeHandIndex = 0;
            tableFirstDoubleConsumed = false;
            tableColdPalmTriggered = false;
            winsThisTable = 0;
            bustsThisTable = 0;
            handsThisTable = 0;
        }

        /// <summary>
        /// Starts a new hand.
        /// </summary>
        /// <param name="handNumber">Hand number within the table.</param>
        public void StartHand(int handNumber)
        {
            if (shoe == null || dealer == null)
            {
                throw new InvalidOperationException("Engine not configured");
            }

            foreach (Hand hand in playerHands)
            {
                hand.Clear();
            }

            dealerHand.Clear();
            activeHandIndex = 0;
            CurrentPayout = 0;
            GrantedFavor = 0;
            IsPeekActive = false;
            bustPreventedThisTurn = false;
            handsThisTable++;

            trinketBus?.Broadcast((t, e) => t.OnHandStart(e), this);

            DealInitialCards();
            trinketBus?.Broadcast((t, e) => t.OnInitialDeal(e), this);
            EvaluateInitialTotals();
        }

        private void DealInitialCards()
        {
            if (shoe == null)
            {
                return;
            }

            EnsureShoe();

            // Player first two cards
            for (int i = 0; i < 2; i++)
            {
                CurrentHand.AddCard(shoe.Draw());
            }

            if (redThreadPending)
            {
                redThreadPending = false;
                EnforceRedThreadParity(CurrentHand);
            }

            // Dealer cards
            dealerHand.AddCard(shoe.Draw());
            dealerHand.AddCard(shoe.Draw());
        }

        private void EnsureShoe()
        {
            if (shoe == null || shoeConfig == null)
            {
                return;
            }

            int deckSize = 52 * shoeConfig.NumDecks;
            if (shoe.Count < deckSize * 0.25f)
            {
                ConfigureShoe(shoeConfig, rng);
            }
        }

        private void EnforceRedThreadParity(Hand hand)
        {
            if (shoe == null || hand.Cards.Count < 2)
            {
                return;
            }

            CardInstance first = hand.Cards[0];
            CardInstance second = hand.Cards[1];
            bool firstOdd = first.ValueOverride % 2 != 0;
            bool secondOdd = second.ValueOverride % 2 != 0;
            if (firstOdd != secondOdd)
            {
                return;
            }

            hand.RemoveCard(second);
            CardInstance replacement = shoe.Draw();
            hand.AddCard(replacement);

            bool replacementOdd = replacement.ValueOverride % 2 != 0;
            if (replacementOdd == firstOdd)
            {
                EnforceRedThreadParity(hand);
            }
        }

        private void EvaluateInitialTotals()
        {
            if (GetHandValue(CurrentHand) == 21)
            {
                GrantedFavor += 1;
                trinketBus?.Broadcast((t, e) => t.OnExact21(e), this);
                CurrentHand.IsComplete = true;
            }
        }

        /// <summary>
        /// Requests a hit on the active hand.
        /// </summary>
        public void PlayerHit()
        {
            if (CurrentHand.IsComplete)
            {
                return;
            }

            trinketBus?.Broadcast((t, e) => t.OnBeforeHit(e), this);

            DrawCardToHand(CurrentHand);

            trinketBus?.Broadcast((t, e) => t.OnAfterHit(e), this);

            int value = GetHandValue(CurrentHand);
            if (value > 21)
            {
                HandleBust(CurrentHand);
            }
            else if (value == 21)
            {
                GrantedFavor += 1;
                trinketBus?.Broadcast((t, e) => t.OnExact21(e), this);
                CurrentHand.IsComplete = true;
            }
        }

        /// <summary>
        /// Requests the active hand to stand.
        /// </summary>
        public void PlayerStand()
        {
            if (CurrentHand.IsComplete)
            {
                return;
            }

            trinketBus?.Broadcast((t, e) => t.OnStand(e), this);
            CurrentHand.IsComplete = true;
            AdvanceToNextHand();
        }

        /// <summary>
        /// Attempts to double the active hand.
        /// </summary>
        /// <returns>True when the double is allowed.</returns>
        public bool PlayerDouble()
        {
            if (CurrentHand.IsComplete)
            {
                return false;
            }

            int cost = 1;
            bool hasHollowRib = trinketBus?.ActiveTrinkets.Any(t => t.Definition is HollowRibTrinketSO) ?? false;
            if (!tableFirstDoubleConsumed && hasHollowRib)
            {
                cost = 0;
            }

            if (resources.Chips < cost)
            {
                return false;
            }

            resources.Chips -= cost;
            tableFirstDoubleConsumed = true;
            CurrentHand.IsDouble = true;
            trinketBus?.Broadcast((t, e) => t.OnDouble(e), this);
            DrawCardToHand(CurrentHand);
            CurrentHand.IsComplete = true;
            return true;
        }

        /// <summary>
        /// Attempts to split the active hand.
        /// </summary>
        /// <returns>True if split occurred.</returns>
        public bool PlayerSplit()
        {
            if (activeHandIndex != 0)
            {
                return false;
            }

            if (playerHands[1].Cards.Count > 0)
            {
                return false;
            }

            if (CurrentHand.Cards.Count != 2)
            {
                return false;
            }

            CardInstance first = CurrentHand.Cards[0];
            CardInstance second = CurrentHand.Cards[1];
            if (first.Card.BaseValue != second.Card.BaseValue)
            {
                return false;
            }

            playerHands[1].Clear();
            playerHands[1].AddCard(second);
            playerHands[1].IsRightSplit = true;
            CurrentHand.RemoveCard(second);
            DrawCardToHand(CurrentHand);
            DrawCardToHand(playerHands[1]);
            trinketBus?.Broadcast((t, e) => t.OnSplit(e), this);
            return true;
        }

        private void DrawCardToHand(Hand hand)
        {
            EnsureShoe();
            if (shoe == null)
            {
                return;
            }

            CardInstance card = shoe.Draw();

            ApplyGoldenChance(card);

            hand.AddCard(card);
            ApplyAceRules(hand);

            if (card.IsGolden && trinketBus != null)
            {
                foreach (ITrinket trinket in trinketBus.ActiveTrinkets)
                {
                    if (trinket is LuckyCindersTrinketSO lucky)
                    {
                        lucky.OnGoldenCard(this);
                    }
                }
            }
        }

        private void ApplyGoldenChance(CardInstance card)
        {
            if (card.Card.BaseValue == 10 && trinketBus != null)
            {
                bool hasGildedTithe = trinketBus.ActiveTrinkets.Any(t => t.Definition is GildedTitheTrinketSO);
                if (hasGildedTithe && rng.RollChance(0.25f))
                {
                    card.IsGolden = true;
                    IncrementCounter("GoldenTrigger");
                }

                bool hasSaintOfTens = trinketBus.ActiveTrinkets.Any(t => t.Definition is SaintOfTensTrinketSO);
                if (hasSaintOfTens && rng.RollChance(0.1f))
                {
                    // Duplicate 10
                    Hand target = CurrentHand;
                    if (target != null)
                    {
                        CardInstance duplicate = new CardInstance(card.Card);
                        target.AddCard(duplicate);
                        ApplyAceRules(target);
                        if (GetHandValue(target) > 21)
                        {
                            resources.Sanity = 0;
                            int total = GetHandValue(target) - 10;
                            IncrementCounter("SaintOfTensSaved");
                            while (GetHandValue(target) > total && target.Cards.Count > 0)
                            {
                                // degrade first ten-value card by 10
                                foreach (CardInstance inst in target.Cards)
                                {
                                    if (inst.Card.BaseValue == 10)
                                    {
                                        inst.ValueOverride = Math.Max(1, inst.ValueOverride - 10);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ApplyAceRules(Hand hand)
        {
            int total = 0;
            int aces = 0;
            foreach (CardInstance card in hand.Cards)
            {
                int value = card.ValueOverride;
                if (card.Card.IsAce)
                {
                    aces++;
                    value = 11;
                }

                total += value;
            }

            while (total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }
        }

        private void HandleBust(Hand hand)
        {
            bustsThisTable++;
            trinketBus?.Broadcast((t, e) => t.OnBust(e), this);

            bool prevented = false;
            if (trinketBus != null)
            {
                foreach (ITrinket trinket in trinketBus.ActiveTrinkets)
                {
                    if (trinket is PitTokenTrinketSO pit)
                    {
                        prevented = pit.TryPreventBust(this, hand);
                        if (prevented)
                        {
                            bustPreventedThisTurn = true;
                            break;
                        }
                    }
                }
            }

            if (!prevented)
            {
                hand.IsComplete = true;
            }
        }

        private void AdvanceToNextHand()
        {
            if (activeHandIndex == 0 && playerHands[1].Cards.Count > 0)
            {
                activeHandIndex = 1;
                IsPeekActive = false;
                return;
            }

            activeHandIndex = 0;
        }

        /// <summary>
        /// Resolves the dealer turn based on the dealer archetype.
        /// </summary>
        public void ResolveDealerHand()
        {
            if (dealer == null)
            {
                return;
            }

            int dealerTotal = GetHandValue(dealerHand);
            while (dealerTotal < dealer.StandOn)
            {
                DrawCardToHand(dealerHand);
                dealerTotal = GetHandValue(dealerHand);
            }
        }

        /// <summary>
        /// Calculates the outcome after dealer resolution.
        /// </summary>
        /// <returns>Outcome value.</returns>
        public HandOutcome ResolveOutcome()
        {
            int bestPlayer = playerHands.Max(GetHandValue);
            int dealerTotal = GetHandValue(dealerHand);
            lastDealerTotal = dealerTotal;

            if (bestPlayer > 21)
            {
                trinketBus?.Broadcast((t, e) => t.OnLoss(e), this);
                return HandOutcome.PlayerBust;
            }

            if (dealerTotal > 21 || bestPlayer > dealerTotal)
            {
                winsThisTable++;
                int payout = 1;
                if (bestPlayer == 21 && playerHands[0].Cards.Count == 2)
                {
                    payout = 2;
                    trinketBus?.Broadcast((t, e) => t.OnExact21(e), this);
                }

                if (playerHands.Any(h => h.IsDouble))
                {
                    payout += 1;
                }

                int golden = GetCounter("GoldenTrigger");
                if (golden > 0 && trinketBus != null && trinketBus.ActiveTrinkets.Any(t => t.Definition is GildedTitheTrinketSO))
                {
                    payout = (int)Math.Ceiling(payout * 1.25f);
                    counters["GoldenTrigger"] = 0;
                }

                CurrentPayout = payout;
                resources.Chips += payout;
                trinketBus?.Broadcast((t, e) => t.OnWin(e), this);
                trinketBus?.Broadcast((t, e) => t.OnPayout(e), this);
                return bestPlayer == 21 ? HandOutcome.PlayerBlackjack : HandOutcome.PlayerWin;
            }

            trinketBus?.Broadcast((t, e) => t.OnLoss(e), this);
            return HandOutcome.DealerWin;
        }

        /// <summary>
        /// Requests a peek at the next card.
        /// </summary>
        public void RequestPeek()
        {
            IsPeekActive = true;
        }

        /// <summary>
        /// Requests a purge of a card from the current hand.
        /// </summary>
        /// <returns>True if the purge is allowed and executed.</returns>
        public bool RequestPurge()
        {
            Hand hand = CurrentHand;
            if (hand.Cards.Count <= 1)
            {
                return false;
            }

            CardInstance card = hand.Cards[hand.Cards.Count - 1];
            hand.RemoveCard(card);
            DrawCardToHand(hand);
            return true;
        }

        /// <summary>
        /// Gets the total value of a hand including modifiers.
        /// </summary>
        /// <param name="hand">Hand reference.</param>
        /// <returns>Total value.</returns>
        public int GetHandValue(Hand hand)
        {
            int total = 0;
            int aces = 0;
            foreach (CardInstance card in hand.Cards)
            {
                int value = card.ValueOverride;
                if (card.Card.IsAce)
                {
                    aces++;
                    value = 11;
                }

                if (card.Card.IsFace && resources.Sanity <= 3 && trinketBus != null && trinketBus.ActiveTrinkets.Any(t => t.Definition is BrokenEyeTrinketSO))
                {
                    value = 9;
                }

                if (card.Card.CardId.StartsWith("J") && resources.Sanity <= 2 && trinketBus != null && trinketBus.ActiveTrinkets.Any(t => t.Definition is GrinningJackTrinketSO))
                {
                    value = 11;
                }

                total += value;
            }

            while (total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }

            total += hand.HiddenModifier;
            return total;
        }

        private void IncrementCounter(string key)
        {
            counters.TryGetValue(key, out int current);
            counters[key] = current + 1;
        }

        private int GetCounter(string key)
        {
            counters.TryGetValue(key, out int value);
            return value;
        }

        /// <summary>
        /// Gets the dealer up card.
        /// </summary>
        /// <returns>Card or null.</returns>
        public CardInstance? GetDealerUpCard()
        {
            if (dealerHand.Cards.Count == 0)
            {
                return null;
            }

            return dealerHand.Cards[0];
        }
    }
}
