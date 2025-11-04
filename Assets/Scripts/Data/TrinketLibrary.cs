using System.Collections.Generic;
using UnityEngine;
using HouseTakes21.Trinkets;

namespace HouseTakes21.Data
{
    /// <summary>
    /// Provides lookup and weighted access for trinkets.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Libraries/TrinketLibrary", fileName = "TrinketLibrary")]
    public sealed class TrinketLibrary : ScriptableObject
    {
        [SerializeField]
        private List<TrinketSO> trinkets = new();

        private readonly Dictionary<string, TrinketSO> lookup = new();

        private void OnEnable()
        {
            BuildLookup();
        }

        /// <summary>
        /// Resolves a trinket by id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Trinket definition or null.</returns>
        public TrinketSO? Resolve(string id)
        {
            if (!lookup.TryGetValue(id, out TrinketSO trinket))
            {
                return null;
            }

            return trinket;
        }

        private void BuildLookup()
        {
            lookup.Clear();
            foreach (TrinketSO trinket in trinkets)
            {
                if (trinket != null && !lookup.ContainsKey(trinket.TrinketId))
                {
                    lookup.Add(trinket.TrinketId, trinket);
                }
            }
        }
    }
}
