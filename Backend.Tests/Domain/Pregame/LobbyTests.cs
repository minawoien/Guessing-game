using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Backend.Data;
using Backend.Domain.Images;
using Backend.Domain.Pregame;
using Backend.Domain.Pregame.Pipelines;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Pregame
{
    public class LobbiesTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public LobbiesTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }

        
        [Fact ]
        public async void GetLobbyByIdOrTypeTest()
        {
            var lobby = new Lobby(1);

            var context = _serviceScope.ServiceProvider.GetService<GameContext>();

            lobby.AddUsers(1, 0, "test");

            context.Lobbies.Add(lobby);
            await context.SaveChangesAsync();

            // Testing GetLobbyById
            var request = new GetLobbyById.Request(1);
            var handler = new GetLobbyById.Handler(context);
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            Assert.True(response.Success);

            // Testing GetLobbyByType
            var typeRequest = new GetLobbyByType.Request(1);
            var typeHandler = new GetLobbyByType.Handler(context);
            var typeResponse = typeHandler.Handle(typeRequest, CancellationToken.None).GetAwaiter().GetResult();

            Assert.True(typeResponse.Success);

            var newTypeRequest = new GetLobbyByType.Request(3);
            var newTypeHandler = new GetLobbyByType.Handler(context);
            var newResponse = newTypeHandler.Handle(newTypeRequest, CancellationToken.None).GetAwaiter().GetResult();

            Assert.False(newResponse.Success);

            _testOutputHelper.WriteLine(lobby.Id.ToString());
            var quitRequest = new QuitLobby.Request(1);
            var quitHandler = new QuitLobby.Handler(context);
            _ = await quitHandler.Handle(quitRequest, CancellationToken.None);
            Assert.Empty(lobby.Players);

            var deletedLobby = await context.Lobbies.FirstOrDefaultAsync(p => p.Id == lobby.Id);
            Assert.Null(deletedLobby);
        }


        
    }
}