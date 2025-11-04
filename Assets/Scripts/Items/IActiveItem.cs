using HouseTakes21.Blackjack;
using HouseTakes21.Core;
using HouseTakes21.Trinkets;

namespace HouseTakes21.Items
{
    /// <summary>
    /// Interface for active-use items.
    /// </summary>
    public interface IActiveItem
    {
        /// <summary>
        /// Gets the scriptable object backing the item.
        /// </summary>
        ItemSO Definition { get; }

        /// <summary>
        /// Attempts to activate the item.
        /// </summary>
        /// <param name="engine">Current blackjack engine.</param>
        /// <param name="resources">Resource tracker.</param>
        /// <param name="bus">Trinket bus.</param>
        /// <returns>True if consumed, otherwise false.</returns>
        bool Activate(BlackjackEngine engine, GameResources resources, TrinketBus bus);
    }
}
