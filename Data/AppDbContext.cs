using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.Models.Domain;
namespace Backend.Data;
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Learning> Learnings { get; set; }
    public DbSet<LearningImage> LearningImages { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<LearningTag> LearningTags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure many-to-many relationship
        builder.Entity<LearningTag>()
            .HasKey(lt => new { lt.LearningId, lt.TagId });

        builder.Entity<LearningTag>()
            .HasOne(lt => lt.Learning)
            .WithMany(l => l.LearningTags)
            .HasForeignKey(lt => lt.LearningId);

        builder.Entity<LearningTag>()
            .HasOne(lt => lt.Tag)
            .WithMany(t => t.LearningTags)
            .HasForeignKey(lt => lt.TagId);
    }
}