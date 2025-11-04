using UnityEngine;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Discounts shop prices.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/BonePurse", fileName = "BonePurse")]
    public sealed class BonePurseTrinketSO : TrinketSO
    {
        /// <summary>
        /// Applies the price discount to the provided value.
        /// </summary>
        /// <param name="price">Original price.</param>
        /// <returns>Discounted price.</returns>
        public int ApplyDiscount(int price)
        {
            return Mathf.Max(1, price - 1);
        }
    }
}
