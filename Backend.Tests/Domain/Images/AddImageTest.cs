using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Backend.Data;
using Backend.Domain.Images;
using Backend.Tests.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Domain.Images
{
	public class AddItemTests : DbTest
	{
		public AddItemTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void AddNewItem_AddsOneItem()
		{
			//adds empty images to a mock db
			using (var context = new GameContext(ContextOptions, null))
			{
				context.Database.Migrate();
				var image = new Image("testLabel");
				image.Fragments = new List<ImageFragment>();
				for (int i = 0; i < 10; i++)
				{
					image.Fragments.Add(new ImageFragment(new byte[10], "file.png", "image/png"));
				}
				context.Images.Add(image);
				context.SaveChanges();
			}
			//checks that the number of images and fragments are correct
			using (var context = new GameContext(ContextOptions, null))
			{
				context.Images.Count().ShouldBe(1);
				context.Images
					.Include(i => i.Fragments)
					.FirstOrDefault()
					?.Fragments
					.Count
					.ShouldBe(10);

			}
		}

	}
}