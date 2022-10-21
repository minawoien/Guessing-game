using System.Data.Common;
using Backend.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit.Abstractions;

namespace Backend.Tests.Helpers
{
	public class DbTest
	{
		//private readonly ShopContext _context;
		private readonly DbContextOptions<GameContext> _contextOptions;
		private readonly DbConnection _connection;
		private readonly ITestOutputHelper _output;

		public DbTest(ITestOutputHelper output)

		{
			_output = output;

			_contextOptions = new DbContextOptionsBuilder<GameContext>()
					.UseSqlite(CreateInMemoryDatabase())
					.LogTo(_output.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
					.EnableSensitiveDataLogging()
					.Options;
			_connection = RelationalOptionsExtension.Extract(_contextOptions).Connection;
			//_context = new ShopContext(_contextOptions);
		}

		public DbContextOptions<GameContext> ContextOptions => _contextOptions;

		//public ShopContext Context => _context;

		private static DbConnection CreateInMemoryDatabase()
		{
			var connection = new SqliteConnection("Filename=:memory:");

			connection.Open();

			return connection;
		}

		public void Dispose() => _connection.Dispose();
	}
}