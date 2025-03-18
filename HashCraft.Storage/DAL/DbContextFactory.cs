using Microsoft.EntityFrameworkCore;

namespace HashCraft.Storage.DAL
{
    public class DbContextFactory
    {
        private readonly IConfiguration _config;
        public DbContextFactory(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(
                _config.GetSection("ConnectionStrings")["DefaultConnection"],
                new MySqlServerVersion(new Version(10, 5)));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
