using Ataoge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ataoge.EntityFrameworkCore
{
    public class SequencesRuleMapper : AtaogeEntityTypeConfiguration<SequencesRule>
    {
        public SequencesRuleMapper(IAtaogeDbContext dbContext) : base(dbContext)
        {
           
        }

        public override void Configure(EntityTypeBuilder<SequencesRule> builder)
        {
            
                builder.HasKey(t => t.PatternName);
                builder.Property(t => t.PatternName)
                    .HasColumnName(ConvertName(nameof(SequencesRule.PatternName)));
                builder.Property(t => t.MinValue)
                    .HasColumnName(ConvertName(nameof(SequencesRule.MinValue)));
                builder.Property(t => t.MaxValue)
                    .HasColumnName(ConvertName(nameof(SequencesRule.MaxValue)));
                builder.Property(t => t.NextValue)
                    .HasColumnName(ConvertName(nameof(SequencesRule.NextValue)));
                builder.Property(t => t.Continuum)
                    .HasColumnName(ConvertName(nameof(SequencesRule.Continuum)));
                builder.Property(t => t.TableField)
                    .HasColumnName(ConvertName(nameof(SequencesRule.TableField)));
                builder.Property(t => t.PreservedCount)
                    .HasColumnName(ConvertName(nameof(SequencesRule.PreservedCount)));
                builder.Property(t => t.Step)
                    .HasColumnName(ConvertName(nameof(SequencesRule.Step)));
                builder.Property( t => t.UpdateTime)
                    .HasColumnName(ConvertName(nameof(SequencesRule.UpdateTime)))
                    .ValueGeneratedOnAdd()
                    .IsConcurrencyToken();
             
                 builder.ToTable(ConvertName(nameof(SequencesRule)));
            
        }
    }
}