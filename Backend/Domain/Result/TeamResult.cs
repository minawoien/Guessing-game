using System.Collections.Generic;

namespace Backend.Domain.Result
{
    public class TeamResult
    {
        public TeamResult(int score)
        {
            Score = score;
            Players = new List<ResultPlayer>();
        }

        public int Id { get; protected set; }
        public List<ResultPlayer> Players { get; set; }
        public int Score { get; set; }

        public void AddUsers(string username)
        {
            var player = new ResultPlayer(username);
            Players.Add(player);
        }
    }
}