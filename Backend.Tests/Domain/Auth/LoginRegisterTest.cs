using System;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Backend.Controllers.Auth;
using Backend.Data;
using Backend.Domain.Auth;
using Backend.Domain.Auth.Pipelines;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Auth
{

    public class LogRegTests :
        IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private readonly CustomWebApplicationFactory<Startup>
            _factory;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public LogRegTests(
            CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }

        [Fact]
        public void GetGameContext()
        {
            var service = _serviceScope.ServiceProvider.GetService<GameContext>();
            Assert.NotNull(service);
        }
        [Fact]
        public void GetIdentity()
        {
            var userManager = _serviceScope.ServiceProvider.GetService<UserManager<User>>();
            var signInManager = _serviceScope.ServiceProvider.GetService<SignInManager<User>>();
            Assert.NotNull(userManager);
            Assert.NotNull(signInManager);
                
        }
        [Fact]
        public async Task RegisterTesting()
        {
            var userManager = _serviceScope.ServiceProvider.GetService<UserManager<User>>();
            var data = new LogRegData("nils","#¤1234eeeRRFFFFGG");
            var request = new RegisterUser.Request(data);
            var handler = new RegisterUser.Handler(userManager);
            var response = await handler.Handle(request, new CancellationToken());
            Assert.True(response.Success);
            var test = await userManager.FindByNameAsync("nils");
            Assert.Equal("nils",test.UserName);
        }
        //[Fact]
        public async Task LoginTesting()
        {
            //create user
            var userManager = _serviceScope.ServiceProvider.GetService<UserManager<User>>();
            var signInManager = _serviceScope.ServiceProvider.GetService<SignInManager<User>>();
            var test = _serviceScope.ServiceProvider.GetService<IHttpContextAccessor>();
            signInManager.Context = new DefaultHttpContext();
            signInManager.Context.User = new GenericPrincipal(
                new GenericIdentity(String.Empty),
                new string[0]
            );
            signInManager.Context.Request.Path = "/test";
            signInManager.Context.Request.Host = new HostString("test");

            
            var data = new LogRegData("ole","#¤1234exeRRF/&FGG");
            var registerRequest = new RegisterUser.Request(data);
            var registerHandler = new RegisterUser.Handler(userManager);
            await registerHandler.Handle(registerRequest, new CancellationToken());
            
            //login user
            var loginRequest = new LoginUser.Request(data);
            var loginHandler = new LoginUser.Handler(userManager, signInManager);
            var loginResponse = await loginHandler.Handle(loginRequest, new CancellationToken());
            Assert.True(loginResponse.Success);


        }
    }
}