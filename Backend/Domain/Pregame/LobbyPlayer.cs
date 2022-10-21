namespace Backend.Domain.Pregame
{
    public class LobbyPlayer
    {
        public LobbyPlayer(int userId, int role, string username)
        {
            UserId = userId;
            MapRole(role);
            Username = username;
        }

        public LobbyPlayer()
        {
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public Role Role { get; set; }
        public string Username { get; set; }

        private void MapRole(int role)
        {
            Role = role != 0 ? Role.Proposer : Role.Guesser;
        }
    }
}