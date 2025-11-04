namespace HouseTakes21.Core
{
    /// <summary>
    /// Tracks mutable resources for the current run.
    /// </summary>
    public sealed class GameResources
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameResources"/> class.
        /// </summary>
        public GameResources()
        {
            Reset();
        }

        /// <summary>
        /// Gets or sets the player's hit points.
        /// </summary>
        public int HitPoints { get; set; }

        /// <summary>
        /// Gets or sets the player's sanity.
        /// </summary>
        public int Sanity { get; set; }

        /// <summary>
        /// Gets or sets the player's chip count.
        /// </summary>
        public int Chips { get; set; }

        /// <summary>
        /// Gets or sets the player's favor.
        /// </summary>
        public int Favor { get; set; }

        /// <summary>
        /// Resets the resources to their starting values.
        /// </summary>
        public void Reset()
        {
            HitPoints = 5;
            Sanity = 5;
            Chips = 10;
            Favor = 0;
        }
    }
}
