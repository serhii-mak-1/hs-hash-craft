using HashCraft.Storage.DAL;
using HashCraft.Storage.DAL.Entities;
using HashCraft.Storage.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HashCraft.Storage.Services
{
    public class HashStorageService
    {
        private readonly DbContextFactory _dbContextFactory;

        public HashStorageService(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public virtual async Task SaveHashesAsync(string[] hashes, CancellationToken ct = default)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            using var transaction = await dbContext.Database.BeginTransactionAsync(ct);
            
            try
            {
                var date = DateOnly.FromDateTime(DateTime.UtcNow);

                await dbContext.Hashes.AddRangeAsync(hashes.Select(x => new Hash()
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    Sha1 = x
                }), ct);

                int insertedRows = await dbContext.SaveChangesAsync(ct);

                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    @$"INSERT INTO HashStats (date, count) VALUES ({date:yyyy-MM-dd}, {insertedRows}) 
                        ON DUPLICATE KEY UPDATE count = count + {insertedRows};",
                    ct);

                await transaction.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                throw new SaveDataException(ex);
            }          
        }

        public virtual List<Hash> GetHashes(int take = 100, int skip = 0)
        {
            try
            {
                using var dbContext = _dbContextFactory.CreateDbContext();
                return dbContext.Hashes.Skip(skip).Take(take).ToList();
            }
            catch (Exception ex)
            {
                throw new FetchDataException(ex);
            }
        }

        public virtual List<HashStat> GetStatistics(int take = 10, int skip = 0)
        {
            try
            {
                using var dbContext = _dbContextFactory.CreateDbContext();
                return dbContext.HashStats.Skip(skip).Take(take).ToList();
            }
            catch (Exception ex)
            {
                throw new FetchDataException(ex);
            }
        }
    }
}
