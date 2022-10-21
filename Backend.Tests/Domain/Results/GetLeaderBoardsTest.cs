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
    public class GetLeaderBoardsTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public GetLeaderBoardsTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }

        [Fact]
        public void GetLeaderboardTest()
        {
            // get no leaderboard
            var context = _serviceScope.ServiceProvider.GetService<GameContext>();

            var request = new GetLeaderboard.Request();
            var handler = new GetLeaderboard.Handler(context);
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            Assert.False(response.Success);
    
            // get leaderboard
            var gameResult = new GameResult("TestName", 5);

            context.Results.Add(gameResult);
            context.SaveChanges();

            var newRequest = new GetLeaderboard.Request();
            var newHandler = new GetLeaderboard.Handler(context);
            var newResponse = newHandler.Handle(newRequest, CancellationToken.None).GetAwaiter().GetResult();

            Assert.True(newResponse.Success);
        }

    }
}