using System.Security.Claims;
using BookDemo.Core.Entities;
using BookDemo.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;



namespace BookDemo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.Status)
                .HasConversion<int>(); 

            modelBuilder.Entity<Category>()
                .Property(c => c.Status)
                .HasConversion<int>(); 
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.UserCratedId = Convert.ToInt32(userId);
                    }
                    else if (entry.State == EntityState.Modified)

                    {
                        entry.Entity.UserCratedId = Convert.ToInt32(userId);
                    }

                    else if (entry.State == EntityState.Deleted)
                    {
                        entry.Entity.UserCratedId = Convert.ToInt32(userId);
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }  
}
