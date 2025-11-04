using System.Collections.Generic;
using UnityEngine;
using HouseTakes21.Items;

namespace HouseTakes21.Data
{
    /// <summary>
    /// Provides lookup for item definitions.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Libraries/ItemLibrary", fileName = "ItemLibrary")]
    public sealed class ItemLibrary : ScriptableObject
    {
        [SerializeField]
        private List<ItemSO> items = new();

        private readonly Dictionary<string, ItemSO> lookup = new();

        private void OnEnable()
        {
            BuildLookup();
        }

        /// <summary>
        /// Resolves an item by identifier.
        /// </summary>
        /// <param name="itemId">Item id.</param>
        /// <returns>Item definition or null.</returns>
        public ItemSO? Resolve(string itemId)
        {
            if (!lookup.TryGetValue(itemId, out ItemSO item))
            {
                return null;
            }

            return item;
        }

        private void BuildLookup()
        {
            lookup.Clear();
            foreach (ItemSO item in items)
            {
                if (item != null && !lookup.ContainsKey(item.ItemId))
                {
                    lookup.Add(item.ItemId, item);
                }
            }
        }
    }
}
