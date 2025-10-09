using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book>   Books   => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        // Author
        model.Entity<Author>(e =>
        {
            e.ToTable("Authors", "dbo");
            e.HasKey(a => a.Id);
            e.Property(a => a.Name).IsRequired().HasMaxLength(200);
            e.Property(a => a.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Book
        model.Entity<Book>(e =>
        {
            e.ToTable("Books", "dbo");
            e.HasKey(b => b.Id);
            e.Property(b => b.Title).IsRequired().HasMaxLength(300);
            e.Property(b => b.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

            // 1:N relation: Author has many Books
            e.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        model.Entity<Genre>(e =>
        {
            e.ToTable("Genres", "dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        model.Entity<Publisher>(e =>
        {
            e.ToTable("Publishers", "dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        model.Entity<Member>(e =>
        {
            e.ToTable("Members", "dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            e.Property(x => x.Email).IsRequired().HasMaxLength(255);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

    }
}