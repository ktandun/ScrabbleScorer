using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScrabbleScorer.Database.Entities;

public class LetterHashEntity
{
    public int Id { get; set; }
    public required char Letter { get; set; }
    public required string Hash { get; set; }

    public class LetterHashEntityTypeConfiguration : IEntityTypeConfiguration<LetterHashEntity>
    {
        public void Configure(EntityTypeBuilder<LetterHashEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Letter).IsUnique();
        }
    }
}
