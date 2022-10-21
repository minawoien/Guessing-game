namespace Backend.Domain.Result
{
    public class GameResult
    {
        public GameResult(string userName, int score)
        {
            UserName = userName;
            Score = score;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }

        public void UpdateScore(int newScore)
        {
            Score += newScore;
        }
    }
}