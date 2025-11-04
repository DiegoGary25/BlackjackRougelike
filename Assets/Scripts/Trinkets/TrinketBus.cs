using System.Collections.Generic;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Manages trinket registration and event dispatch.
    /// </summary>
    public sealed class TrinketBus
    {
        private readonly List<ITrinket> trinkets = new();

        /// <summary>
        /// Gets an enumeration of active trinkets.
        /// </summary>
        public IReadOnlyList<ITrinket> ActiveTrinkets => trinkets;

        /// <summary>
        /// Registers a trinket for the current run.
        /// </summary>
        /// <param name="trinket">Trinket to add.</param>
        public void Register(ITrinket trinket)
        {
            if (!trinkets.Contains(trinket))
            {
                trinkets.Add(trinket);
            }
        }

        /// <summary>
        /// Clears all active trinkets.
        /// </summary>
        public void Reset()
        {
            trinkets.Clear();
        }

        /// <summary>
        /// Broadcasts an event to all trinkets.
        /// </summary>
        /// <param name="callback">Invocation delegate.</param>
        public void Broadcast(System.Action<ITrinket> callback)
        {
            for (int i = 0; i < trinkets.Count; i++)
            {
                callback(trinkets[i]);
            }
        }

        /// <summary>
        /// Broadcasts an event with engine context.
        /// </summary>
        /// <param name="callback">Invocation delegate.</param>
        /// <param name="engine">Current engine.</param>
        public void Broadcast(System.Action<ITrinket, BlackjackEngine> callback, BlackjackEngine engine)
        {
            for (int i = 0; i < trinkets.Count; i++)
            {
                callback(trinkets[i], engine);
            }
        }
    }
}
