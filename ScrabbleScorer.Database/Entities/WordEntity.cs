using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScrabbleScorer.Database.Entities;

public class WordEntity
{
    public int Id { get; set; }
    public required string Word { get; set; }
    public required string WordSorted { get; set; }

    public class WordEntityTypeConfiguration : IEntityTypeConfiguration<WordEntity>
    {
        public void Configure(EntityTypeBuilder<WordEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Word).IsUnique();

            builder.HasIndex(e => e.WordSorted);
        }
    }
}
