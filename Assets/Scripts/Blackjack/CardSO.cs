using UnityEngine;

namespace HouseTakes21.Blackjack
{
    /// <summary>
    /// Scriptable representation of a playing card.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Card", fileName = "Card")]
    public sealed class CardSO : ScriptableObject
    {
        [SerializeField]
        private string cardId = "";

        [SerializeField]
        private string displayName = "";

        [SerializeField]
        private int baseValue;

        [SerializeField]
        private bool isFace;

        [SerializeField]
        private bool isAce;

        [SerializeField]
        private string suit = "";

        /// <summary>
        /// Gets the identifier of the card.
        /// </summary>
        public string CardId => cardId;

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName => displayName;

        /// <summary>
        /// Gets the base value.
        /// </summary>
        public int BaseValue => baseValue;

        /// <summary>
        /// Gets a value indicating whether the card is a face card.
        /// </summary>
        public bool IsFace => isFace;

        /// <summary>
        /// Gets a value indicating whether the card is an ace.
        /// </summary>
        public bool IsAce => isAce;

        /// <summary>
        /// Gets the suit name.
        /// </summary>
        public string Suit => suit;

        /// <summary>
        /// Configures the card runtime data. Used by bootstrap when assets are created dynamically.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Display name.</param>
        /// <param name="value">Base value.</param>
        /// <param name="face">Face card flag.</param>
        /// <param name="ace">Ace flag.</param>
        /// <param name="suitName">Suit value.</param>
        public void Configure(string id, string name, int value, bool face, bool ace, string suitName)
        {
            cardId = id;
            displayName = name;
            baseValue = value;
            isFace = face;
            isAce = ace;
            suit = suitName;
        }
    }
}
