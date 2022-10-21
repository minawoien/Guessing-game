using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domain.Pregame;

namespace Backend.Domain.Game.Services
{
    public interface IGameService
    {
        public Task<GameServiceResponse> StartGame(int lobbyId, List<LobbyPlayer> players, Type type);

        //public Task EndGame(int gameId);
        public Task<GameDTO> GetGame(int userId);
        public Task<int> GetGameId(int userId);

        public Task<GameServiceDataResponse<string>> RegisterGuess(string guess, int userId);
        public Task<ImagePathResponse> GetFragments(int gameId, int userId);
        public Task InsertFragmentList(int gameId, int imageId, string label, List<string> fragmentList);
        public Task<GameServiceDataResponse<string>> UnlockFragmentWithOracle(int gameId, bool firstRound);
        public Task<GameServiceResponse> UnlockFragmentWithProposer(int gameId, string fragment);
        public Task<GameServiceDataResponse<(int GameId, int ImageId)>> GetProposerGame(int userId);
        public Task SetWaitingStatus(int gameId);
        public Task QuitGame(int userId);
    }
}