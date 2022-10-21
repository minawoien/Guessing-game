using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Game;
using Backend.Domain.Game.Pipelines;
using Backend.Domain.Game.Services;
using Backend.Domain.Images;
using Backend.Domain.Pregame;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Role = Backend.Domain.Game.Role;

namespace Backend.Tests.Domain.Game
{
    public class GetGameTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public GetGameTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }
        //[Fact]
        public async Task GetGame()
        {
            var gameService = _serviceScope.ServiceProvider.GetRequiredService<IGameService>();

            var context = _serviceScope.ServiceProvider.GetService<GameContext>();
            await context.Database.EnsureDeletedAsync();
            var userId = 1;

            var lobby = new Lobby(0);
            lobby.AddUsers(userId, 0, "test");

            context.Lobbies.Add(lobby);
            await context.SaveChangesAsync();

            var game = new Backend.Domain.Game.Game
            {
                Players = new List<Player>()
            };
            game.AddPlayers(userId, "test", Role.Guesser);
            game.UseOracle = true;
            var image = new Image("myLabel");
            for (int i = 0; i < 10; i++)
            {
                image.Fragments.Add(new ImageFragment(new byte[10],"test","image/png"));
            }

            context.Images.Add(image);
            game.Image = new ImageInfo(image.Id, image.Label.Value);
            await context.SaveChangesAsync();
            
            context.Games.Add(game);
            await context.SaveChangesAsync();
           
            var newRequest = new GetGame.Request(userId);
            var newHandler = new GetGame.Handler(gameService);
            var newResponse = await newHandler.Handle(newRequest, CancellationToken.None);

            Assert.Equal(game.Id, newResponse.Id);
        }
    }
}