using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Images;
using Backend.Domain.Images.Pipelines;
using Backend.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Images
{
    public class ImageTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IServiceScope _serviceScope;

        public ImageTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
            });
            _serviceScope = _factory.Services.CreateScope();
        }

        [Fact]
        public async Task GetImageFragmentTest()
        {
            var image = new Image("myImage");
            image.Fragments = new List<ImageFragment>();
            for (int i = 0; i < 10; i++)
            {
                image.Fragments.Add(new ImageFragment(new byte[10], "Test", "image/png"));
            }

            var context = _serviceScope.ServiceProvider.GetService<GameContext>();

            context.Images.Add(image);
            context.SaveChanges();

            var request = new GetImageFragment.Request(image.Id, "Test");
            var handler = new GetImageFragment.Handler(context);
            var response = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(image.Fragments[0].FileName, response.FileName);
        }
    }
}