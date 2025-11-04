using System.Collections.Generic;
using HouseTakes21.Core;

namespace HouseTakes21.Blackjack
{
    /// <summary>
    /// Represents the shoe of cards for the run.
    /// </summary>
    public sealed class Shoe
    {
        private readonly List<CardSO> catalog;
        private readonly List<CardInstance> shoeCards = new();
        private readonly RNGService rng;
        private readonly ShoeConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shoe"/> class.
        /// </summary>
        /// <param name="catalog">The source cards.</param>
        /// <param name="config">Configuration data.</param>
        /// <param name="rng">Random service.</param>
        public Shoe(List<CardSO> catalog, ShoeConfig config, RNGService rng)
        {
            this.catalog = catalog;
            this.config = config;
            this.rng = rng;
            BuildShoe();
        }

        /// <summary>
        /// Gets the number of cards remaining in the shoe.
        /// </summary>
        public int Count => shoeCards.Count;

        /// <summary>
        /// Draws the top card of the shoe.
        /// </summary>
        /// <returns>Card instance.</returns>
        public CardInstance Draw()
        {
            if (shoeCards.Count == 0)
            {
                BuildShoe();
            }

            CardInstance card = shoeCards[^1];
            shoeCards.RemoveAt(shoeCards.Count - 1);
            return card;
        }

        /// <summary>
        /// Peeks the next card without removing it.
        /// </summary>
        /// <returns>Card instance or null.</returns>
        public CardInstance? Peek()
        {
            if (shoeCards.Count == 0)
            {
                return null;
            }

            return shoeCards[^1];
        }

        private void BuildShoe()
        {
            shoeCards.Clear();
            for (int i = 0; i < config.NumDecks; i++)
            {
                foreach (CardSO card in catalog)
                {
                    shoeCards.Add(new CardInstance(card));
                }
            }

            rng.Shuffle(shoeCards);
        }
    }
}
