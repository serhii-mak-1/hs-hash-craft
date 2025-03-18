using HashCraft.Storage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HashCraft.Storage.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hash> Hashes { get; set; }
        public DbSet<HashStat> HashStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
