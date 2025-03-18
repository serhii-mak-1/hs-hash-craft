using HashCraft.Storage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HashCraft.Storage.DAL.Configurations
{
    public class HashStatConfiguration : IEntityTypeConfiguration<Hash>
    {
        public void Configure(EntityTypeBuilder<Hash> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
