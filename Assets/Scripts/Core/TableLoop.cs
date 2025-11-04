using System;
using System.Collections.Generic;
using HouseTakes21.Blackjack;
using HouseTakes21.Items;
using HouseTakes21.Trinkets;

namespace HouseTakes21.Core
{
    /// <summary>
    /// Coordinates the gameplay loop for tables and hands.
    /// </summary>
    public sealed class TableLoop
    {
        private readonly GameStateMachine stateMachine;
        private readonly BlackjackEngine blackjackEngine;
        private readonly ItemController itemController;
        private readonly TrinketBus trinketBus;
        private readonly GameResources resources;
        private readonly RNGService rngService;
        private readonly DealerSO dealer;
        private readonly ShoeConfig shoeConfig;

        private int handsPlayedInTable;
        private bool awaitingPlayerInput;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableLoop"/> class.
        /// </summary>
        public TableLoop(
            GameStateMachine stateMachine,
            BlackjackEngine blackjackEngine,
            ItemController itemController,
            TrinketBus trinketBus,
            GameResources resources,
            RNGService rngService,
            DealerSO dealer,
            ShoeConfig shoeConfig)
        {
            this.stateMachine = stateMachine;
            this.blackjackEngine = blackjackEngine;
            this.itemController = itemController;
            this.trinketBus = trinketBus;
            this.resources = resources;
            this.rngService = rngService;
            this.dealer = dealer;
            this.shoeConfig = shoeConfig;
        }

        /// <summary>
        /// Starts a new run with default values.
        /// </summary>
        public void StartNewRun()
        {
            resources.Reset();
            blackjackEngine.ConfigureDealer(dealer);
            blackjackEngine.ConfigureShoe(shoeConfig, rngService);
            trinketBus.Reset();
            handsPlayedInTable = 0;
            StartHand();
        }

        /// <summary>
        /// Processes per-frame updates.
        /// </summary>
        public void Tick()
        {
            blackjackEngine.Tick();
        }

        /// <summary>
        /// Requests a hit for the active hand.
        /// </summary>
        public void PlayerHit()
        {
            if (!awaitingPlayerInput)
            {
                return;
            }

            blackjackEngine.PlayerHit();
            EvaluatePostAction();
        }

        /// <summary>
        /// Requests a stand for the active hand.
        /// </summary>
        public void PlayerStand()
        {
            if (!awaitingPlayerInput)
            {
                return;
            }

            blackjackEngine.PlayerStand();
            MoveToDealerTurn();
        }

        /// <summary>
        /// Requests a double for the active hand.
        /// </summary>
        public void PlayerDouble()
        {
            if (!awaitingPlayerInput)
            {
                return;
            }

            if (blackjackEngine.PlayerDouble())
            {
                MoveToDealerTurn();
            }
        }

        /// <summary>
        /// Requests a split for the active hand.
        /// </summary>
        public void PlayerSplit()
        {
            if (!awaitingPlayerInput)
            {
                return;
            }

            if (blackjackEngine.PlayerSplit())
            {
                EvaluatePostAction();
            }
        }

        private void StartHand()
        {
            handsPlayedInTable++;
            if (handsPlayedInTable == 1)
            {
                trinketBus.Broadcast((t, e) => t.OnTableStart(e), blackjackEngine);
            }

            blackjackEngine.StartHand(handsPlayedInTable);
            awaitingPlayerInput = true;
            stateMachine.SetState(GameStateMachine.GameState.PlayerTurn);
        }

        private void EvaluatePostAction()
        {
            if (blackjackEngine.IsHandComplete)
            {
                MoveToDealerTurn();
            }
        }

        private void MoveToDealerTurn()
        {
            awaitingPlayerInput = false;
            stateMachine.SetState(GameStateMachine.GameState.DealerTurn);
            blackjackEngine.ResolveDealerHand();
            ResolveHand();
        }

        private void ResolveHand()
        {
            stateMachine.SetState(GameStateMachine.GameState.Resolve);
            HandOutcome outcome = blackjackEngine.ResolveOutcome();

            switch (outcome)
            {
                case HandOutcome.PlayerBust:
                case HandOutcome.DealerWin:
                    resources.HitPoints = Math.Max(0, resources.HitPoints - 1);
                    if (blackjackEngine.RefundBustHp)
                    {
                        resources.HitPoints += 1;
                        blackjackEngine.RefundBustHp = false;
                    }
                    break;
                case HandOutcome.PlayerBlackjack:
                    resources.Chips += 2;
                    break;
                case HandOutcome.PlayerWin:
                    resources.Chips += blackjackEngine.CurrentPayout;
                    break;
            }

            if (blackjackEngine.GrantedFavor > 0)
            {
                resources.Favor += blackjackEngine.GrantedFavor;
            }

            if (resources.HitPoints <= 0)
            {
                stateMachine.SetState(GameStateMachine.GameState.Death);
                return;
            }

            if (handsPlayedInTable >= 5)
            {
                stateMachine.SetState(GameStateMachine.GameState.TableEnd);
                trinketBus.Broadcast((t, e) => t.OnTableEnd(e), blackjackEngine);
                blackjackEngine.ResetHands();
                handsPlayedInTable = 0;
                stateMachine.SetState(GameStateMachine.GameState.Intermission);
                return;
            }

            StartHand();
        }
    }
}
