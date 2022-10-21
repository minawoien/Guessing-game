namespace Backend.Domain.Game
{
    public static class ScoreCalculator
    {
        public static int Calculate(int tot, int unlocked, int guessInRound, int maxGuess)
        {
            return (tot * maxGuess) - (unlocked * maxGuess) + (maxGuess - guessInRound);
        }
    }
}