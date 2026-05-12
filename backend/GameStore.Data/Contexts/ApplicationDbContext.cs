using GameStore.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data.Contexts;

public sealed class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("dbo");

        ApplyGameConfiguration(builder);

        ApplyGenreConfiguration(builder);

        ApplyReviewConfiguration(builder);

        ApplyWishlistConfiguration(builder);
    }

    private static void ApplyGameConfiguration(ModelBuilder builder)
    {
        builder.Entity<Game>(entity =>
        {
            entity.Property(g => g.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(g => g.Description)
                .HasMaxLength(4000)
                .IsRequired();

            entity.Property(g => g.Price)
                .HasColumnType("decimal(18,2)");

            entity.Property(g => g.CoverImageUrl)
                .HasMaxLength(500);

            entity.HasIndex(g => g.Title);

            entity.HasOne(g => g.Genre)
                .WithMany(g => g.Games)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ApplyGenreConfiguration(ModelBuilder builder)
    {
        builder.Entity<Genre>(entity =>
        {
            entity.Property(g => g.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(g => g.Name)
                .IsUnique();
        });
    }

    private static void ApplyReviewConfiguration(ModelBuilder builder)
    {
        builder.Entity<Review>(entity =>
        {
            entity.Property(r => r.Comment)
                .HasMaxLength(2000);

            entity.HasIndex(r => new
            {
                r.GameId,
                r.UserId
            }).IsUnique();

            entity.HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ApplyWishlistConfiguration(ModelBuilder builder)
    {
        builder.Entity<WishlistItem>(entity =>
        {
            entity.HasIndex(w => new
            {
                w.GameId,
                w.UserId
            }).IsUnique();

            entity.HasOne(w => w.Game)
                .WithMany(g => g.WishlistItems)
                .HasForeignKey(w => w.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(w => w.User)
                .WithMany(u => u.WishlistItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}