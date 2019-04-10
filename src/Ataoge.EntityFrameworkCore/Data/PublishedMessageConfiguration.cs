using Ataoge.EntityFrameworkCore;
using Ataoge.EventBus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ataoge.Data
{
    public class PublishedMessageConfiguration : AtaogeEntityTypeConfiguration<PublishedMessage>
    {
        public PublishedMessageConfiguration(IAtaogeDbContext dbContext) : base(dbContext)
        {
        }

        public override void Configure(EntityTypeBuilder<PublishedMessage> builder)
        {
            
            builder.ToTable(base.ConvertName("Msg_Published"));

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).HasColumnName(base.ConvertName(nameof(PublishedMessage.Name)));
            builder.Property(t => t.Content).HasColumnName(base.ConvertName(nameof(PublishedMessage.Content)));
            builder.Property(t => t.Added).HasColumnName(base.ConvertName(nameof(PublishedMessage.Added)));
            builder.Property(t => t.Id).HasColumnName(base.ConvertName(nameof(PublishedMessage.Id)));
            builder.Property(t => t.ExpiresAt).HasColumnName(base.ConvertName(nameof(PublishedMessage.ExpiresAt)));
            builder.Property(t => t.Retries).HasColumnName(base.ConvertName(nameof(PublishedMessage.Retries)));
            builder.Property(t => t.StatusName).HasColumnName(base.ConvertName(nameof(PublishedMessage.StatusName)));

            builder.Property<string>(base.ConvertName("Version"));

        }
    }
}