using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Backend.Data;
using Backend.Domain.Game;
using Backend.Domain.Game.Pipelines;
using Backend.Domain.Game.Services;
using Backend.Domain.Images;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Game
{
    public class GetProposerGameTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public GetProposerGameTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }
        [Fact]
        public void GetProposerGame()
        {
            var gameService = _serviceScope.ServiceProvider.GetRequiredService<IGameService>();
            var request = new GetProposerGame.Request(1);
            var handler = new GetProposerGame.Handler(gameService);
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            Assert.False(response.Success);

            var userId = 1;

            var context = _serviceScope.ServiceProvider.GetService<GameContext>();

            var game = new Backend.Domain.Game.Game();
            game.Players = new();
            game.AddPlayers(userId, "test", Role.Proposer);

            context.Games.Add(game);
            context.SaveChangesAsync();

            var image = new Image("test");
            for (int i = 0; i < 10; i++)
            {
                image.Fragments.Add(new ImageFragment(new byte[10], "testing", "image/png"));
            }
            context.Images.Add(image);
            context.SaveChanges();

            game.Image = new ImageInfo(image.Id, image.Label.Value);
            context.SaveChangesAsync();

            var newRequest = new GetProposerGame.Request(userId);
            var newHandler = new GetProposerGame.Handler(gameService);
            var newResponse = newHandler.Handle(newRequest, CancellationToken.None).GetAwaiter().GetResult();

            Assert.False(newResponse.Success);

            context.Remove(image);
            context.Remove(game);

        }
    }
}