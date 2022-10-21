using System.Collections.Generic;
using Backend.Domain.Game;
using Backend.SharedKernel;

namespace Backend.Domain.Pregame
{
    public class Lobby : BaseEntity
    {
        public Lobby(int type)
        {
            Players = new List<LobbyPlayer>();
            MapType(type);
        }

        public Lobby()
        {
            Players = new List<LobbyPlayer>();
        }

        public int Id { get; set; }
        public Type Type { get; set; }
        public int GameId { get; set; }

        public List<LobbyPlayer> Players { get; set; }

        public void AddUsers(int userId, int role, string username)
        {
            var lobbyPlayer = new LobbyPlayer(userId, role, username);
            Players.Add(lobbyPlayer);
        }


        private void MapType(int type)
        {
            if (type is < 3 and >= 0)
            {
                Type = (Type) type;
            }
            else
            {
                Type = Type.SinglePlayer;
            }
        }
    }
}