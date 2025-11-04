namespace HouseTakes21.Blackjack
{
    /// <summary>
    /// Represents the outcome of a resolved hand.
    /// </summary>
    public enum HandOutcome
    {
        Pending,
        PlayerWin,
        PlayerBlackjack,
        DealerWin,
        PlayerBust,
        Push,
    }
}
