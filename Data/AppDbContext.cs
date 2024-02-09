using signup.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace signup.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options) {
    public DbSet<Product> Product { get; set; }
    public DbSet<Cart> Cart { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .Ignore(u => u.EmailConfirmed)
            .Ignore(u => u.PhoneNumber)
            .Ignore(u => u.PhoneNumberConfirmed)
            .Ignore(u => u.TwoFactorEnabled)
            .Ignore(u => u.LockoutEnabled)
            .Ignore(u => u.LockoutEnd)
            .Ignore(u => u.AccessFailedCount)
            .Property(e => e.Id).HasColumnName("UserId");

        modelBuilder.Entity<AppUser>().ToTable("User");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
        modelBuilder.Entity<IdentityRole>().ToTable("Role");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
    }
}

public class Product {
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public string? Description { get; set; }

    // Navigation Properties
    public List<Cart> Cart { get; set; } = [];
}

public class Cart {
    public int CartId { get; set; }
    [ForeignKey("User")]
    public string? UserId { get; set; }
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public AppUser User { get; set; } = new(); 
    public Product Product { get; set; } = new();
}

public class AppUser : IdentityUser {
    [StringLength(100)]
    [MaxLength(100)]
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Surname { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public Role Role { get; set; }

    // Navigation Properties
    public List<Cart> Cart { get; set; } = [];
}