using HashCraft.API.Tools.Sha1HashGenerator;
using HashCraft.IntegrationTests.Fixtures;
using HashCraft.Storage.Services;

namespace HashCraft.IntegrationTests
{
    public class HashStorageServiceTests : IClassFixture<DbContextFactoryFixture>, IDisposable
    {
        private DbContextFactoryFixture _dbContextFactoryFixture;
        public HashStorageServiceTests(DbContextFactoryFixture fixture)
        {
            _dbContextFactoryFixture = fixture;
        }

        [Fact]
        public async Task SaveData_ConcurrentThreads_ShouldUpdateCounterWithoutRaceConditions()
        {
            // Arrange & Act
            var hashStorageService = new HashStorageService(_dbContextFactoryFixture.DbContextFactory);

            var tasks = new List<Task>();

            for (int i = 0; i < 50; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    string[] hashes = new string[10]
                        .Select(x => new Sha1Generator().GenerateHash(EncodeStyle.Hex))
                        .ToArray();

                    await hashStorageService.SaveHashesAsync(hashes);
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            using var dbContext = _dbContextFactoryFixture.DbContextFactory.CreateDbContext();
            var hashStats = dbContext.HashStats.ToList();
            
            hashStats.ForEach(hashStat =>
            {
                var hashes = dbContext.Hashes.Where(x => x.Date.Equals(hashStat.Date));
                Assert.Equal(hashStat.Count, hashes.Count());
            });
        }

        public void Dispose()
        {
            _dbContextFactoryFixture.Dispose();
        }
    }
}
