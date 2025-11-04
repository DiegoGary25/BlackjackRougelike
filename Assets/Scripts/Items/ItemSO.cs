using UnityEngine;

namespace HouseTakes21.Items
{
    /// <summary>
    /// Base ScriptableObject for active items.
    /// </summary>
    public abstract class ItemSO : ScriptableObject, IActiveItem
    {
        [SerializeField]
        private string itemId = "";

        [SerializeField]
        private string displayName = "";

        [SerializeField]
        [TextArea]
        private string description = "";

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string ItemId => itemId;

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName => displayName;

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description => description;

        /// <inheritdoc />
        public ItemSO Definition => this;

        /// <inheritdoc />
        public abstract bool Activate(HouseTakes21.Blackjack.BlackjackEngine engine, HouseTakes21.Core.GameResources resources, HouseTakes21.Trinkets.TrinketBus bus);
    }
}
