using UnityEngine;

namespace HouseTakes21.Blackjack
{
    /// <summary>
    /// Describes the size of the shoe for a run.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/ShoeConfig", fileName = "ShoeConfig")]
    public sealed class ShoeConfig : ScriptableObject
    {
        [SerializeField]
        private int numDecks = 4;

        /// <summary>
        /// Gets the number of decks to include in the shoe.
        /// </summary>
        public int NumDecks => numDecks;
    }
}
