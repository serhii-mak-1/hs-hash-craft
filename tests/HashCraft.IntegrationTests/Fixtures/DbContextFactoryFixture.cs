using HashCraft.Storage.DAL;
using Microsoft.Extensions.Configuration;

namespace HashCraft.IntegrationTests.Fixtures
{
    public class DbContextFactoryFixture : IDisposable
    {
        private static int counter = 0;
        private static string ConnectionString => $"Server=localhost;Port=3307;Database=hashCraftDb-test-{GetSafeCounter()};User=root;Password=root;";

        public DbContextFactory DbContextFactory { get; private set; }

        public DbContextFactoryFixture()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnection", ConnectionString }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            DbContextFactory = new DbContextFactory(configuration);

            var dbContext = DbContextFactory.CreateDbContext();
            dbContext.Database.EnsureCreated();
        }

        private static int GetSafeCounter()
        {
            return Interlocked.Increment(ref counter);
        }

        public void Dispose()
        {
            var dbContext = DbContextFactory.CreateDbContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
    }
}
