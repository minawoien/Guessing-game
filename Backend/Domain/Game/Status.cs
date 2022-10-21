namespace Backend.Domain.Game
{
    public enum Status
    {
        Started,
        WaitingOnFragment,
        WaitingOnGuess,
        FinishedWithWinner,
        Ended,
        GameNotFound
    }
}