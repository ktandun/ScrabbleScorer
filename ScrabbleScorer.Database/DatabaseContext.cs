using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ScrabbleScorer.Database.Entities;

namespace ScrabbleScorer.Database;

public class DatabaseContext : DbContext
{
    public DbSet<WordEntity> Words { get; set; } = null!;

    private string DbPath { get; }

    public DatabaseContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "words.db");
    }

    public bool IsExist()
    {
        return Path.Exists(DbPath);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
