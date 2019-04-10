using Ataoge.EntityFrameworkCore;
using Ataoge.EventBus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ataoge.Data
{
    public class ReceivedMessageConfiguration : AtaogeEntityTypeConfiguration<ReceivedMessage>
    {
        public ReceivedMessageConfiguration(IAtaogeDbContext dbContext) : base(dbContext)
        {
        }

        public override void Configure(EntityTypeBuilder<ReceivedMessage> builder)
        {
            throw new System.NotImplementedException();
        }
    }
}