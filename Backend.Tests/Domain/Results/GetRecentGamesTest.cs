using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Backend.Data;
using Backend.Domain.Result;
using Backend.Domain.Result.Pipelines;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Results
{
    public class GetRecentGamesTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public GetRecentGamesTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }


        [Fact]
        public void GetRecentGames()
        {
            // Get no recent games
            var context = _serviceScope.ServiceProvider.GetService<GameContext>();

            var request = new GetRecentGames.Request();
            var handler = new GetRecentGames.Handler(context);
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            Assert.False(response.Success);
    
            // get recent games

            var players = new List<string>(); 
            players.Add("TestPlayer");

            var recentGames = new RecentGame(Backend.Domain.Game.Type.SinglePlayer, "20");

            context.RecentGames.Add(recentGames);
            context.SaveChanges();

            var newRequest = new GetRecentGames.Request();
            var newHandler = new GetRecentGames.Handler(context);
            var newResponse = newHandler.Handle(newRequest, CancellationToken.None).GetAwaiter().GetResult();

            Assert.True(newResponse.Success);
        }
    }
}