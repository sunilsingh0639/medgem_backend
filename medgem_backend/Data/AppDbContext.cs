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
    public DbSet<NavItem> NavItems => Set<NavItem>();
    public DbSet<FooterConfig> FooterConfigs => Set<FooterConfig>();
    public DbSet<FooterColumn> FooterColumns => Set<FooterColumn>();
    public DbSet<FooterLink> FooterLinks => Set<FooterLink>();
    public DbSet<PatientGallery> PatientGalleries => Set<PatientGallery>();
    public DbSet<PatientGalleryImage> PatientGalleryImages => Set<PatientGalleryImage>();
    public DbSet<PatientStory> PatientStories => Set<PatientStory>();
    public DbSet<PatientStoryPhoto> PatientStoryPhotos => Set<PatientStoryPhoto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Treatment>().HasIndex(t => t.Slug).IsUnique();
        modelBuilder.Entity<Hospital>().HasIndex(h => h.Slug).IsUnique();
        modelBuilder.Entity<Destination>().HasIndex(d => d.Slug).IsUnique();
        modelBuilder.Entity<Doctor>().HasIndex(d => d.Slug).IsUnique();
        modelBuilder.Entity<AdminUser>().HasIndex(a => a.Email).IsUnique();
        modelBuilder.Entity<Hospital>().Property(h => h.Rating).HasPrecision(3, 2);

        // NavItem self-referencing
        modelBuilder.Entity<NavItem>()
            .HasOne(n => n.Parent)
            .WithMany(n => n.Children)
            .HasForeignKey(n => n.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // FooterLink → FooterColumn
        modelBuilder.Entity<FooterLink>()
            .HasOne(fl => fl.Column)
            .WithMany(fc => fc.Links)
            .HasForeignKey(fl => fl.FooterColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        // PatientGallery → PatientGalleryImage
        modelBuilder.Entity<PatientGalleryImage>()
            .HasOne(i => i.Gallery)
            .WithMany(g => g.Images)
            .HasForeignKey(i => i.PatientGalleryId)
            .OnDelete(DeleteBehavior.Cascade);

        // PatientStory → PatientStoryPhoto
        modelBuilder.Entity<PatientStoryPhoto>()
            .HasOne(p => p.Story)
            .WithMany(s => s.Photos)
            .HasForeignKey(p => p.PatientStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<HomePageContent>().HasData(new HomePageContent { Id = 1, UpdatedAt = seedDate });
        modelBuilder.Entity<SiteSettings>().HasData(new SiteSettings { Id = 1, UpdatedAt = seedDate });
        modelBuilder.Entity<FooterConfig>().HasData(new FooterConfig { Id = 1, UpdatedAt = seedDate });
    }
}
