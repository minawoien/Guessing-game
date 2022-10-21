using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Game.Events;
using Backend.Domain.Pregame;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Game.Services
{
    public class GameService : IGameService
    {
        private readonly GameContext _db;
        private readonly IMediator _mediator;

        public GameService(GameContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<GameServiceResponse> StartGame(int lobbyId, List<LobbyPlayer> players, Type type)
        {
            var game = new Game(type);

            if (type is Type.MultiPlayer or Type.TwoPlayer)
            {
                var player = players.FirstOrDefault(p => p.Role == Pregame.Role.Proposer);
                if (player is not null)
                {
                    game.AddPlayers(player.UserId, player.Username, Role.Proposer);
                }
                else
                {
                    game.UseOracle = true;
                }
            }
            else
            {
                game.UseOracle = true;
            }

            foreach (var player in players.Where(p => p.Role == Pregame.Role.Guesser))
            {
                game.AddPlayers(player.UserId, player.Username, Role.Guesser);
            }

            _db.Games.Add(game);
            await _db.SaveChangesAsync();
            game.Events.Add(new GameCreated(game.Id, lobbyId));
            await _db.RunEvents();
            if (game.UseOracle)
            {
                await UnlockFragmentWithOracle(game.Id, true);
                game.Status = Status.WaitingOnGuess;
            }
            else
            {
                game.Status = Status.WaitingOnFragment;
            }

            await _db.SaveChangesAsync();

            return new GameServiceResponse(true, game.Id);
        }

        public async Task<GameServiceDataResponse<string>> RegisterGuess(string guess, int userId)
        {
            var gameId = await GetGameId(userId);
            var game = await _db.Games
                .Include(g => g.RevealedFragments)
                .Include(p => p.Players)
                .Include(g => g.Image)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            if (game is null)
            {
                return new GameServiceDataResponse<string>(false, "", new[] {"game not found"});
            }

            if (game.Status is Status.Ended or Status.FinishedWithWinner)
            {
                return new GameServiceDataResponse<string>(false, "", new[] {"Game has ended"});
            }

            var player = game.Players.FirstOrDefault(p => p.UserId == userId);
            if (player.PlayerStatus == PlayerStatus.HaveGuessed)
            {
                return new GameServiceDataResponse<string>(false, "", new[] {"Not allowed to guess"});
            }

            game.AddGuess(guess, player);
            player.PlayerStatus = PlayerStatus.HaveGuessed;

            if (guess.ToLower() == game.Image.Label)
            {
                game.Status = Status.FinishedWithWinner;
                await EndGame(gameId, player.Id);
                await _db.SaveChangesAsync();
                return new GameServiceDataResponse<string>(true, "", Array.Empty<string>());
            }

            //single and two-player
            if (game.Type < Type.MultiPlayer)
            {
                if (player.GuessCount < 3)
                {
                    game.Status = Status.WaitingOnGuess;
                    player.PlayerStatus = PlayerStatus.AwaitingGuess;
                    await _db.SaveChangesAsync();
                    return new GameServiceDataResponse<string>(true, "", Array.Empty<string>());
                }

                game.Status = Status.WaitingOnFragment;

                if (game.UseOracle)
                {
                    await UnlockFragmentWithOracle(gameId, false);
                }

                await _db.SaveChangesAsync();
                return new GameServiceDataResponse<string>(true, "", Array.Empty<string>());
            }


            //multiplayer

            var numGuessers = game.Players.Where(p => p.Role == Role.Guesser).ToList().Count;
            var guessCount = game.Players.Where(p => p.PlayerStatus == PlayerStatus.HaveGuessed).ToList().Count;

            if (numGuessers == guessCount)
            {
                game.Status = Status.WaitingOnFragment;
                if (game.UseOracle)
                {
                    await UnlockFragmentWithOracle(gameId, false);
                }
            }

            await _db.SaveChangesAsync();
            return new GameServiceDataResponse<string>(true, "", Array.Empty<string>());
        }

        private async Task ResetGuessCounts(IEnumerable<Player> players)
        {
            foreach (var player in players.Where(p => p.Role == Role.Guesser))
            {
                player.PlayerStatus = PlayerStatus.AwaitingGuess;
                player.GuessCount = 0;
            }

            await _db.SaveChangesAsync();
        }

        private async Task EndGame(int gameId, int playerId)
        {
            var game = await _db.Games
                .Include(p => p.Players)
                .Include(g => g.RevealedFragments)
                .FirstOrDefaultAsync(c => c.Id == gameId);

            var players = game.Players.Select(p => p.UserName).ToList();

            var unlockedInGame = game.UnlockedFragments;
            for (var i = 0; i < game.RevealedFragments.Count; i++)
            {
                game.RevealedFragments[i].Unlocked = true;
            }

            game.UnlockedFragments = game.RevealedFragments.Count;

            if (game.Status == Status.Ended)
            {
                game.Winner = "No winner";
                var result = new ResultDTO(0, game.StartTime, game.Type, players, game.Winner);
                game.Events.Add(new EndGame(result));
                await _db.SaveChangesAsync();
                await _db.RunEvents();
                return;
            }

            var player = game.Players.FirstOrDefault(p => p.Id == playerId);

            if (game.Status == Status.FinishedWithWinner)
            {
                if (game.Type < Type.MultiPlayer)
                {
                    player.Score = ScoreCalculator.Calculate(
                        game.RevealedFragments.Count,
                        unlockedInGame,
                        player.GuessCount,
                        3);
                }
                else
                {
                    player.Score = ScoreCalculator.Calculate(
                        game.RevealedFragments.Count,
                        unlockedInGame,
                        player.GuessCount,
                        1);
                }

                if (game.Type == Type.TwoPlayer)
                {
                    game.Winner = "Team";
                }
                else
                {
                    game.Winner = player.UserName;
                }

                var result = new ResultDTO(player.Score, game.StartTime, game.Type, players, game.Winner);
                game.Events.Add(new EndGame(result));
            }

            await _db.SaveChangesAsync();
            await _db.RunEvents();
        }


        public async Task<ImagePathResponse> GetFragments(int gameId, int userId)
        {
            var game = await _db.Games.Include(i => i.RevealedFragments)
                .Include(g => g.Players)
                .FirstOrDefaultAsync(i => i.Id == gameId);
            if (game is not null)
            {
                var player = game.Players.FirstOrDefault(p => p.UserId == userId);
                RevealedFragment[] fragments;
                if (player != null && player.Role == Role.Proposer)
                {
                    fragments = game.RevealedFragments.ToArray();
                }
                else
                {
                    fragments = game.RevealedFragments.Where(f => f.Unlocked).ToArray();
                }

                FragmentDTO[] fragmentDTOs = new FragmentDTO[fragments.Length];
                for (int i = 0; i < fragments.Length; i++)
                {
                    fragmentDTOs[i] = new FragmentDTO(fragments[i].FileName, fragments[i].Unlocked);
                }

                return new ImagePathResponse(true, fragmentDTOs);
            }

            return new ImagePathResponse(false, Array.Empty<FragmentDTO>());
        }

        public async Task InsertFragmentList(int gameId, int imageId, string label, List<string> fragmentList)
        {
            var game = await _db.Games
                .FirstOrDefaultAsync(g => g.Id == gameId);

            game.CreateRevealedFragments(fragmentList);
            //creates oracle based on number of fragments
            game.AddOracle(fragmentList.Count);
            game.AddImage(imageId, label);
            await _db.SaveChangesAsync();
        }

        public async Task<GameServiceResponse> UnlockFragmentWithProposer(int gameId, string fragment)
        {
            var game = await _db.Games
                .Include(p => p.Players)
                .Include(g => g.RevealedFragments)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game.UnlockedFragments == game.RevealedFragments.Count)
            {
                game.Status = Status.Ended;
                await _db.SaveChangesAsync();
                await EndGame(gameId, 0);
                return new GameServiceResponse(false, gameId);
            }

            foreach (var f in game.RevealedFragments)
            {
                if (f.FileName.Contains(fragment) && !f.Unlocked)
                {
                    f.Unlocked = true;
                    game.UnlockedFragments++;
                    game.Status = Status.WaitingOnGuess;
                    await ResetGuessCounts(game.Players);
                    await _db.SaveChangesAsync();
                    return new GameServiceResponse(true, gameId);
                }
            }

            return new GameServiceResponse(false, gameId);
        }

        public async Task<GameServiceDataResponse<string>> UnlockFragmentWithOracle(int gameId, bool firstRound)
        {
            var game = await _db.Games
                .Include(p => p.Players)
                .Include(g => g.RevealedFragments)
                .Include(g => g.Oracle)
                .FirstOrDefaultAsync(g => g.Id == gameId);


            if (!game.UseOracle)
            {
                return new GameServiceDataResponse<string>(false, "", new[] {"game is not played with oracle"});
            }

            var nextOracleValue = game.Oracle.GetNextIndex();

            if (game.UnlockedFragments == game.RevealedFragments.Count())
            {
                game.Status = Status.Ended;
                await _db.SaveChangesAsync();
                await EndGame(gameId, 0);
                return new GameServiceDataResponse<string>(false, "", new[] {"game is finished"});
            }

            if (!nextOracleValue.Item2)
            {
                return new GameServiceDataResponse<string>(false, "", new[] {"no more fragments"});
            }

            game.RevealedFragments[nextOracleValue.Item1].Unlocked = true;
            game.UnlockedFragments++;
            game.Status = Status.WaitingOnGuess;
            if (!firstRound)
            {
                await ResetGuessCounts(game.Players);
            }

            await _db.SaveChangesAsync();
            return new GameServiceDataResponse<string>(true, "", Array.Empty<string>());
        }

        public async Task SetWaitingStatus(int gameId)
        {
            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == gameId);
            if (game.Status < Status.FinishedWithWinner)
            {
                game.Status = Status.WaitingOnFragment;
            }

            await _db.SaveChangesAsync();
        }

        private async Task<int> GetGameIdForProposer(int userId)
        {
            var game = await _db.Games
                .FirstOrDefaultAsync(g => g.Players.Any(p => p.UserId == userId && p.Role == Role.Proposer));
            if (game is null)
            {
                return 0;
            }

            return game.Id;
        }

        public async Task<int> GetGameId(int userId)
        {
            var game = await _db.Games
                .FirstOrDefaultAsync(g => g.Players.Any(p => p.UserId == userId));
            if (game is null)
            {
                return 0;
            }

            return game.Id;
        }

        private async Task RemoveGame(int gameId)
        {
            var game = await _db.Games
                .Include(g => g.Guesses)
                .ThenInclude(g => g.Player)
                .Include(g => g.Image)
                .Include(g => g.RevealedFragments)
                .Include(g => g.Oracle)
                .Include(g => g.Players)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            foreach (var item in game.RevealedFragments)
            {
                _db.Remove(item);
            }

            foreach (var item in game.Guesses)
            {
                _db.Remove(item);
            }

            _db.Remove(game.Image);
            _db.Remove(game.Oracle);
            _db.Remove(game);
            await _db.SaveChangesAsync();
        }

        private async Task RemovePlayer(Player player, Game game)
        {
            _db.Remove(player);
            game.Players.Remove(player);
            if (game.Players.Count == 0)
            {
                await RemoveGame(game.Id);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<GameDTO> GetGame(int userId)
        {
            var gameId = await GetGameId(userId);
            var game = await _db.Games.Include(g => g.Guesses)
                .Include(g => g.Players)
                .Include(g => g.Image)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game is not null)
            {
                var guesses = game.Guesses.Select(g => g.Value).ToList();
                var player = game.Players.FirstOrDefault(p => p.UserId == userId);

                //adds an event that removes the player from the lobby
                if (player is not null && player.PlayerStatus == PlayerStatus.New)
                {
                    player.PlayerStatus = PlayerStatus.Joined;
                    await _db.SaveChangesAsync();
                    game.Events.Add(new JoinedGame(userId));
                    await _db.RunEvents();
                }


                var role = player.Role;
                string imageLabel;

                if (role == Role.Proposer || (int) game.Status >= (int) Status.FinishedWithWinner)
                {
                    imageLabel = game.Image.Label;
                }
                else
                {
                    imageLabel = null;
                }

                return new GameDTO(game.Id, (int) game.Status, game.UnlockedFragments, guesses, (int) role, imageLabel,
                    (int) game.Type, game.Winner);
            }

            return new GameDTO(0, 0, 0, null, 0, null, 0, "");
        }

        // only used for proposeroute
        public async Task<GameServiceDataResponse<(int GameId, int ImageId)>> GetProposerGame(int userId)
        {
            var gameId = await GetGameIdForProposer(userId);
            if (gameId == 0)
            {
                return new GameServiceDataResponse<(int GameId, int ImageId)>(false, (0, 0),
                    new[] {"Not proposer in game"});
            }

            var game = await _db.Games
                .Include(g => g.Image)
                .Where(g => g.Id == gameId)
                .FirstOrDefaultAsync(g => g.Status == Status.WaitingOnFragment);

            if (game is null)
            {
                return new GameServiceDataResponse<(int GameId, int ImageId)>(false, (gameId, 0),
                    new[] {"Waiting on guesser"});
            }

            return new GameServiceDataResponse<(int GameId, int ImageId)>(true, (gameId, game.Image.ImageId),
                Array.Empty<string>());
        }


        public async Task QuitGame(int userId)
        {
            var gameId = await GetGameId(userId);
            var game = await _db.Games
                .Include(g => g.Players)
                .Include(p => p.Guesses)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            var player = game.Players.FirstOrDefault(p => p.UserId == userId);
            var guesses = game.Guesses.ToArray();
            foreach (var guess in guesses)
            {
                _db.Remove(guess);
                game.Guesses.Remove(guess);
            }

            await RemovePlayer(player, game);
        }
    }
}