using HouseTakes21.Blackjack;
using HouseTakes21.Core;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Provides hooks for reacting to blackjack events.
    /// </summary>
    public interface ITrinket
    {
        /// <summary>
        /// Gets the scriptable object backing this trinket.
        /// </summary>
        TrinketSO Definition { get; }

        /// <summary>
        /// Called when a new table begins.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnTableStart(BlackjackEngine engine);

        /// <summary>
        /// Called when a new hand begins.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnHandStart(BlackjackEngine engine);

        /// <summary>
        /// Called when the initial deal occurs.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnInitialDeal(BlackjackEngine engine);

        /// <summary>
        /// Called before the player hits.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnBeforeHit(BlackjackEngine engine);

        /// <summary>
        /// Called after the player hits.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnAfterHit(BlackjackEngine engine);

        /// <summary>
        /// Called when the player stands.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnStand(BlackjackEngine engine);

        /// <summary>
        /// Called when the player doubles.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnDouble(BlackjackEngine engine);

        /// <summary>
        /// Called when the player splits.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnSplit(BlackjackEngine engine);

        /// <summary>
        /// Called when the player busts.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnBust(BlackjackEngine engine);

        /// <summary>
        /// Called when the player wins the hand.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnWin(BlackjackEngine engine);

        /// <summary>
        /// Called when the player loses the hand.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnLoss(BlackjackEngine engine);

        /// <summary>
        /// Called when the player hits exactly 21.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnExact21(BlackjackEngine engine);

        /// <summary>
        /// Called when payouts are resolved.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnPayout(BlackjackEngine engine);

        /// <summary>
        /// Called when a table ends.
        /// </summary>
        /// <param name="engine">Active engine.</param>
        void OnTableEnd(BlackjackEngine engine);
    }
}
