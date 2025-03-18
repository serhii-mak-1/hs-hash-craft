using HashCraft.Storage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HashCraft.Storage.DAL.Configurations
{
    public class HashConfiguration : IEntityTypeConfiguration<HashStat>
    {
        public void Configure(EntityTypeBuilder<HashStat> builder)
        {
            {
                builder.HasKey(x => x.Date);
            }
        }
    }
}