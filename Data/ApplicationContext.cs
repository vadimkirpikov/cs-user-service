using CsApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CsApi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Subscription>()
            .HasKey(s => new { s.SubscriberId, s.SubscribedUserId });

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Subscriber)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.SubscriberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.SubscribedUser)
            .WithMany(u => u.Subscribers)
            .HasForeignKey(s => s.SubscribedUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
