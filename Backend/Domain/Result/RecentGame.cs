using System.Collections.Generic;
using Backend.Domain.Game;

namespace Backend.Domain.Result
{
    public class RecentGame
    {
        private RecentGame()
        {
        }

        public RecentGame(Type gameType, string startTime)
        {
            GameType = gameType;
            StartTime = startTime;
            Players = new List<ResultPlayer>();
            MapType();
        }

        public int Id { get; set; }
        public List<ResultPlayer> Players { get; set; }
        public Type GameType { get; set; } // Waiting for Game Domain
        public string Type { get; set; }
        public string StartTime { get; set; }

        public void AddUsers(string username)
        {
            var player = new ResultPlayer(username);
            Players.Add(player);
        }

        private void MapType()
        {
            if (GameType == Game.Type.SinglePlayer)
            {
                Type = "Single player";
            }
            else if (GameType == Game.Type.TwoPlayer)
            {
                Type = "Two player";
            }
            else
            {
                Type = "Multiplayer";
            }
        }
    }
}