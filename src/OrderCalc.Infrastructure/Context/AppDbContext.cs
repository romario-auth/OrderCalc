using Microsoft.EntityFrameworkCore;
using OrderCalc.Domain.Entities;

namespace OrderCalc.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<Order> Order { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity
                .HasMany(typeof(OrderItem), "_items")
                .WithOne()
                .HasForeignKey("OrderId")
                .IsRequired();

            entity.Ignore(o => o.Items);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);
        });
    }
}