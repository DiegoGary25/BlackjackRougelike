using System.Collections.Generic;
using HouseTakes21.Blackjack;
using HouseTakes21.Core;
using HouseTakes21.Trinkets;

namespace HouseTakes21.Items
{
    /// <summary>
    /// Manages the player's active items.
    /// </summary>
    public sealed class ItemController
    {
        private readonly GameResources resources;
        private readonly RNGService rng;
        private readonly BlackjackEngine engine;
        private readonly TrinketBus trinketBus;
        private readonly List<ItemSO> items = new(2);

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemController"/> class.
        /// </summary>
        public ItemController(GameResources resources, RNGService rng, BlackjackEngine engine, TrinketBus trinketBus)
        {
            this.resources = resources;
            this.rng = rng;
            this.engine = engine;
            this.trinketBus = trinketBus;
        }

        /// <summary>
        /// Gets the items in quick slots.
        /// </summary>
        public IReadOnlyList<ItemSO> Items => items;

        /// <summary>
        /// Attempts to add an item to the quick slots.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <returns>True if added.</returns>
        public bool TryAddItem(ItemSO item)
        {
            if (items.Count >= 2)
            {
                return false;
            }

            items.Add(item);
            return true;
        }

        /// <summary>
        /// Replaces an item at the specified index.
        /// </summary>
        /// <param name="index">Slot index.</param>
        /// <param name="item">New item.</param>
        public void ReplaceItem(int index, ItemSO item)
        {
            if (index < 0 || index >= items.Count)
            {
                return;
            }

            items[index] = item;
        }

        /// <summary>
        /// Activates the item in the provided slot.
        /// </summary>
        /// <param name="slot">Slot index.</param>
        public void ActivateItem(int slot)
        {
            if (slot < 0 || slot >= items.Count)
            {
                return;
            }

            ItemSO item = items[slot];
            if (item.Activate(engine, resources, trinketBus))
            {
                items.RemoveAt(slot);
            }
        }
    }
}
