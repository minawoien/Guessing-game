using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Images;
using Backend.Domain.Pregame;
using Backend.Domain.Pregame.Pipelines;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Pregame
{
    public class StartGameTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public StartGameTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }
        //[Fact]
        public async Task StartGame()
        {
            var context = _serviceScope.ServiceProvider.GetService<GameContext>();
            await context.Database.EnsureDeletedAsync();
            var request = new StartGame.Request(1);
            var handler = new StartGame.Handler(context);
            var response = await handler.Handle(request, CancellationToken.None);
            Assert.False(response.Success);

            var userId = 3;
            var lobby = new Lobby(0);
            lobby.AddUsers(userId, 0, "test");

            context.Lobbies.Add(lobby);
            await context.SaveChangesAsync();

            var image = new Image("TestLabel");
            for (int i = 0; i < 10; i++)
            {
                image.Fragments.Add(new ImageFragment(new byte[10], "fragment.png", "image/png"));
            }
            var j = 0;
            foreach (var fra in image.Fragments)
            {
                fra.FileName = $"img{j}";
                j++;
            }
            context.Images.Add(image);
            await context.SaveChangesAsync();
        
            var newRequest = new StartGame.Request(userId);
            var newHandler = new StartGame.Handler(context);
            var newResponse = await newHandler.Handle(newRequest, CancellationToken.None);

            Assert.True(newResponse.Success);

        }

        
    }
}