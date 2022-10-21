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
    public class CreateLobbyTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public CreateLobbyTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }
        //[Fact]
        public async Task CreateLobby()
        {
            var context = _serviceScope.ServiceProvider.GetService<GameContext>();

            var data = new LobbyData(0, 0, 0);
            var request = new CreateLobby.Request(1, "TestName", data);
            var handler = new CreateLobby.Handler(context);
            var response = await handler.Handle(request, CancellationToken.None);

            Assert.True(response.Success);

        }

        
    }
}