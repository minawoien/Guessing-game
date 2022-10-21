namespace Backend.Domain.Game
{
    public enum PlayerStatus
    {
        New,
        Joined,
        AwaitingGuess,
        HaveGuessed,
        Left
    }
}