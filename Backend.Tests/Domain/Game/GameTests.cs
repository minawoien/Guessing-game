using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Backend.Data;
using Backend.Domain.Game;
using Backend.Domain.Game.Pipelines;
using Backend.Domain.Game.Services;
using Backend.Domain.Images;
using Backend.Domain.Pregame;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Game
{
    public class GameTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public GameTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }

        
        


        //[Fact]
        public void RegisterGuessTest()
        {
          var gameService = _serviceScope.ServiceProvider.GetRequiredService<IGameService>();
          var request = new RegisterGuess.Request("test",1);
          var handler = new RegisterGuess.Handler(gameService);
          var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
          Assert.False(response.Success);

            
          var context = _serviceScope.ServiceProvider.GetService<GameContext>();
          
          var userId = 1;
          var game = new Backend.Domain.Game.Game();
          game.Players = new();
          game.AddPlayers(userId, "test", Backend.Domain.Game.Role.Guesser);

          context.Games.Add(game);
          context.SaveChangesAsync();

          var image = new Image("imageLabel");
          for (int i = 0; i < 10; i++)
          {
              image.Fragments.Add(new ImageFragment(new byte[10], "fragment.png", "image/png"));
          }
          context.Images.Add(image);
          context.SaveChanges();

          var fragmentList = new List<string>();
          foreach (var file in image.Fragments)
          {
              fragmentList.Add($"/ImageFragment/{image.Id}/{file.FileName}");
          }

          game.RevealedFragments = new List<RevealedFragment>();
          foreach (var item in fragmentList)
          {
              game.RevealedFragments.Add(new RevealedFragment(item));
          }

          context.SaveChanges();

        }

        [Fact]
        public void GetGameIdTestFail()
        {
            var gameService = _serviceScope.ServiceProvider.GetRequiredService<IGameService>();
            var request = new GetGameId.Request(5);
            var handler = new GetGameId.Handler(gameService);
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            Assert.Equal(0, response);

            var context = _serviceScope.ServiceProvider.GetService<GameContext>();
            var game = new Backend.Domain.Game.Game();
            game.Players = new();
            game.AddPlayers(2, "Test", Backend.Domain.Game.Role.Proposer);

            if (context != null)
            {
                context.Games.Add(game);
                context.SaveChangesAsync();

                var newRequest = new GetGameId.Request(2);
                var newHandler = new GetGameId.Handler(gameService);
                var newResponse = newHandler.Handle(newRequest, CancellationToken.None).GetAwaiter().GetResult();
                Assert.Equal(game.Id, newResponse);

                context.Remove(game);
            }
        }

         [Fact]
        public void SetWaitingStatus()
        {
            var context = _serviceScope.ServiceProvider.GetService<GameContext>();

            var game = new Backend.Domain.Game.Game();

            if (context != null)
            {
                context.Games.Add(game);
                context.SaveChangesAsync();

                game.Status = Status.Started;

                context.SaveChangesAsync();

                var gameService = _serviceScope.ServiceProvider.GetRequiredService<IGameService>();
                var request = new SetWaitingStatus.Request(game.Id);
                var handler = new SetWaitingStatus.Handler(gameService);
                var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
                Assert.Equal(Status.WaitingOnFragment, game.Status);

                context.Remove(game);
            }
        }




    }
}
