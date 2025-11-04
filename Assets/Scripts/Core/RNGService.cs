using System;
using System.Collections.Generic;

namespace HouseTakes21.Core
{
    /// <summary>
    /// Provides deterministic random number generation services scoped to a single run.
    /// </summary>
    public sealed class RNGService
    {
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RNGService"/> class with the specified seed.
        /// </summary>
        /// <param name="seed">The seed to use for deterministic results.</param>
        public RNGService(int seed)
        {
            Seed = seed;
            random = new Random(seed);
        }

        /// <summary>
        /// Gets the seed used to create this service.
        /// </summary>
        public int Seed { get; }

        /// <summary>
        /// Rolls a chance against the provided probability.
        /// </summary>
        /// <param name="probability">Probability between 0 and 1.</param>
        /// <returns>True if the roll succeeds, otherwise false.</returns>
        public bool RollChance(float probability)
        {
            if (probability <= 0f)
            {
                return false;
            }

            if (probability >= 1f)
            {
                return true;
            }

            return random.NextDouble() < probability;
        }

        /// <summary>
        /// Shuffles the provided list in-place using Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="items">List to shuffle.</param>
        public void Shuffle<T>(IList<T> items)
        {
            for (int i = items.Count - 1; i > 0; i--)
            {
                int swapIndex = random.Next(i + 1);
                (items[i], items[swapIndex]) = (items[swapIndex], items[i]);
            }
        }

        /// <summary>
        /// Gets the next integer in range.
        /// </summary>
        /// <param name="min">Inclusive minimum.</param>
        /// <param name="max">Exclusive maximum.</param>
        /// <returns>Random integer.</returns>
        public int Next(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Gets a random float in range 0..1.
        /// </summary>
        /// <returns>Random float.</returns>
        public float NextFloat()
        {
            return (float)random.NextDouble();
        }
    }
}
