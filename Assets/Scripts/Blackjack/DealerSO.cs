using UnityEngine;

namespace HouseTakes21.Blackjack
{
    /// <summary>
    /// Defines the configuration for a dealer archetype.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Dealer", fileName = "Dealer")]
    public sealed class DealerSO : ScriptableObject
    {
        [SerializeField]
        private string dealerId = "default";

        [SerializeField]
        private string displayName = "Default Dealer";

        [SerializeField]
        private int standOn = 17;

        /// <summary>
        /// Gets the dealer identifier.
        /// </summary>
        public string DealerId => dealerId;

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName => displayName;

        /// <summary>
        /// Gets the point total where the dealer stands.
        /// </summary>
        public int StandOn => standOn;
    }
}
