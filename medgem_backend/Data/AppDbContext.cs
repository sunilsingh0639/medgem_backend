using Microsoft.EntityFrameworkCore;
using medgem_backend.Models;

namespace medgem_backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<Treatment> Treatments => Set<Treatment>();
    public DbSet<Hospital> Hospitals => Set<Hospital>();
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<Enquiry> Enquiries => Set<Enquiry>();
    public DbSet<Faq> Faqs => Set<Faq>();
    public DbSet<HomePageContent> HomePageContent => Set<HomePageContent>();
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();
    public DbSet<MediaAsset> MediaAssets => Set<MediaAsset>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Treatment>().HasIndex(t => t.Slug).IsUnique();
        modelBuilder.Entity<Hospital>().HasIndex(h => h.Slug).IsUnique();
        modelBuilder.Entity<Destination>().HasIndex(d => d.Slug).IsUnique();
        modelBuilder.Entity<Doctor>().HasIndex(d => d.Slug).IsUnique();
        modelBuilder.Entity<AdminUser>().HasIndex(a => a.Email).IsUnique();
        modelBuilder.Entity<Hospital>().Property(h => h.Rating).HasPrecision(3, 2);
        modelBuilder.Entity<HomePageContent>()
    .HasData(new HomePageContent
    {
        Id = 1,
        UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    });

        modelBuilder.Entity<SiteSettings>()
            .HasData(new SiteSettings
            {
                Id = 1,
                UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
    }
}
