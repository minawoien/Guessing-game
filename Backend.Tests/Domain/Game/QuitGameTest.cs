using System.Net.Http;
using System.Threading;
using Backend.Data;
using Backend.Domain.Game;
using Backend.Domain.Game.Pipelines;
using Backend.Domain.Game.Services;
using Backend.Domain.Images;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Game
{
    public class QuitGameTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public QuitGameTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }
        [Fact]
        public async void QuitGame()
        {
            var context = _serviceScope.ServiceProvider.GetService<GameContext>();

            var game = new Backend.Domain.Game.Game
            {
                Players = new()
            };

            game.AddPlayers(4, "test", Role.Guesser);

            var player = game.Players[0];
            game.AddGuess("guess", player);

            game.Oracle = new Oracle(5);

            if (context != null)
            {
                context.Games.Add(game);
                await context.SaveChangesAsync();

                var image = new Image("testImage");

                context.Images.Add(image);
                await context.SaveChangesAsync();

                game.Image = new ImageInfo(image.Id, image.Label.Value);

                await context.SaveChangesAsync();

                var gameService = _serviceScope.ServiceProvider.GetRequiredService<IGameService>();

                var request = new QuitGame.Request(4);
                var handler = new QuitGame.Handler(gameService);
                _ = await handler.Handle(request, CancellationToken.None);

                Assert.Empty(game.Players);

                var deletedGame = await context.Games.FirstOrDefaultAsync(p => p.Id == game.Id);
                Assert.Null(deletedGame);
            }
        }
    }
}