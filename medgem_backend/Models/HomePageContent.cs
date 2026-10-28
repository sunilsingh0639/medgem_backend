namespace medgem_backend.Models;

public class HomePageContent
{
    public int Id { get; set; }
    // Hero
    public string HeroHeading { get; set; } = "Your Health. Our Global Commitment.";
    public string HeroSubheading { get; set; } = "Discover trusted healthcare and personalised medical travel assistance.";
    public string HeroBadgeText { get; set; } = "Personalised Healthcare Assistance";
    public string HeroVideoUrl { get; set; } = "https://videos.pexels.com/video-files/3195394/3195394-hd_1920_1080_25fps.mp4";
    public string HeroPosterUrl { get; set; } = "https://images.unsplash.com/photo-1519494026892-80bbd2d6fd0d?w=1400&q=80";
    public string HeroCtaPrimaryLabel { get; set; } = "Explore Treatments";
    public string HeroCtaPrimaryUrl { get; set; } = "/treatments";
    public string HeroCtaSecondaryLabel { get; set; } = "Get Free Consultation";
    public string HeroCtaSecondaryUrl { get; set; } = "/consultation";
    // Stats
    public int StatPatients { get; set; } = 10000;
    public int StatPartners { get; set; } = 200;
    public int StatCountries { get; set; } = 40;
    public int StatSpecialties { get; set; } = 70;
    // Why Choose Us heading
    public string WhyHeading { get; set; } = "Your Trusted Healthcare Navigation Partner";
    public string WhySubheading { get; set; } = "We are dedicated to helping patients access quality healthcare with confidence, clarity and compassionate support at every step.";
    // Second Opinion section
    public string SecondOpinionHeading { get; set; } = "A Second Opinion Can Help You Make an Informed Decision";
    public string SecondOpinionSubheading { get; set; } = "Before proceeding with a major medical procedure, gaining an independent specialist perspective can provide valuable clarity and confidence in your treatment path.";
    public string SecondOpinionImage { get; set; } = "https://images.unsplash.com/photo-1576091160550-2173dba999ef?w=600&q=80";
    // Contact Banner
    public string ContactBannerHeading { get; set; } = "Start Your Healthcare Journey Today";
    public string ContactBannerSubheading { get; set; } = "Share your requirements and our team will help you explore suitable healthcare options and coordinate your medical journey.";
   // public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}
