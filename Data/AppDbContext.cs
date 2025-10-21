using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        // ===== Authors
        model.Entity<Author>(e =>
        {
            e.ToTable("Authors", "dbo");
            e.HasKey(a => a.Id);
            e.Property(a => a.Name).IsRequired().HasMaxLength(200);
            e.Property(a => a.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasMany(a => a.Books)
             .WithOne(b => b.Author)
             .HasForeignKey(b => b.AuthorId)
             .OnDelete(DeleteBehavior.Restrict); // safer than cascade for demos
        });

        // ===== Publishers
        model.Entity<Publisher>(e =>
        {
            e.ToTable("Publishers", "dbo");
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasMany(p => p.Books)
             .WithOne(b => b.Publisher)
             .HasForeignKey(b => b.PublisherId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== Genres
        model.Entity<Genre>(e =>
        {
            e.ToTable("Genres", "dbo");
            e.HasKey(g => g.Id);
            e.Property(g => g.Name).IsRequired().HasMaxLength(200);
            e.Property(g => g.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // ===== Books
        model.Entity<Book>(e =>
        {
            e.ToTable("Books", "dbo");
            e.HasKey(b => b.Id);
            e.Property(b => b.Title).IsRequired().HasMaxLength(300);
            e.Property(b => b.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

            // 1:N Author -> Books (set also on Author above; keep here for clarity)
            e.HasOne(b => b.Author)
             .WithMany(a => a.Books)
             .HasForeignKey(b => b.AuthorId)
             .OnDelete(DeleteBehavior.Restrict);

            // 1:N Publisher -> Books
            e.HasOne(b => b.Publisher)
             .WithMany(p => p.Books)
             .HasForeignKey(b => b.PublisherId)
             .OnDelete(DeleteBehavior.Restrict);

            // M:N Book <-> Genre via join table
            e.HasMany(b => b.Genres)
             .WithMany(g => g.Books)
             .UsingEntity<Dictionary<string, object>>(
                 "BookGenres",
                 j => j.HasOne<Genre>()
                       .WithMany()
                       .HasForeignKey("GenreId")
                       .OnDelete(DeleteBehavior.Cascade),
                 j => j.HasOne<Book>()
                       .WithMany()
                       .HasForeignKey("BookId")
                       .OnDelete(DeleteBehavior.Cascade),
                 j =>
                 {
                     j.ToTable("BookGenres", "dbo");
                     j.HasKey("BookId", "GenreId");
                 });
        });

        // ===== Members
        model.Entity<Member>(e =>
        {
            e.ToTable("Members", "dbo");
            e.HasKey(m => m.Id);
            e.Property(m => m.FullName).IsRequired().HasMaxLength(200);
            e.Property(m => m.Email).IsRequired().HasMaxLength(255);
            e.Property(m => m.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });
    }
}
