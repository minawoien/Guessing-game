using System;

namespace Backend.Domain.Game
{
    public class Player
    {
        public Player(int userId, Role role, string userName)
        {
            UserId = userId;
            Role = role;
            PlayerStatus = PlayerStatus.New;
            UserName = userName;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public int GuessCount { get; set; }
        public Role Role { get; set; }
        public PlayerStatus PlayerStatus { get; set; }

        public void PostGame(int score, DateTime startTime, DateTime endTime)
        {
            Score = score;
        }
    }
}