using System.Collections.Generic;
using UnityEngine;
using HouseTakes21.Core;
using HouseTakes21.Blackjack;
using HouseTakes21.Items;
using HouseTakes21.Trinkets;

namespace HouseTakes21.UI
{
    /// <summary>
    /// Minimal UI controller stub to support runtime wiring.
    /// </summary>
    public sealed class RunUIController : MonoBehaviour
    {
        /// <summary>
        /// Initializes the UI bindings.
        /// </summary>
        public void Initialize(GameResources resources, BlackjackEngine engine, ItemController items, TrinketBus trinkets, GameStateMachine stateMachine)
        {
        }

        /// <summary>
        /// Refreshes the trinket list display.
        /// </summary>
        /// <param name="trinkets">Active trinkets.</param>
        public void RefreshTrinkets(IReadOnlyList<ITrinket> trinkets)
        {
        }

        /// <summary>
        /// Refreshes the item quick slots.
        /// </summary>
        /// <param name="items">Active items.</param>
        public void RefreshItems(IReadOnlyList<ItemSO> items)
        {
        }
    }
}
