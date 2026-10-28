using medgem_backend.Data;
using medgem_backend.Models;

namespace medgem_backend.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!db.AdminUsers.Any())
        {
            db.AdminUsers.Add(new AdminUser
            {
                Email = "admin@medgem.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123!"),
                FullName = "Super Admin",
                Role = "SuperAdmin",
                IsActive = true
            });
        }

        if (!db.Treatments.Any())
        {
            db.Treatments.AddRange(
                new Treatment { Title = "Cardiac Surgery", Slug = "cardiac-surgery", Category = "Cardiology", Description = "Advanced heart procedures including bypass surgery, valve replacement and minimally invasive cardiac interventions.", Image = "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?w=600&q=80", Icon = "Heart", Duration = "7–14 days", Destinations = "[\"India\",\"Germany\",\"Thailand\"]", Tags = "[\"Heart\",\"Surgery\",\"Cardiology\"]", DisplayOrder = 1 },
                new Treatment { Title = "Knee Replacement", Slug = "knee-replacement", Category = "Orthopaedics", Description = "Total and partial knee replacement surgery using advanced implants for long-lasting mobility restoration.", Image = "https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?w=600&q=80", Icon = "Activity", Duration = "10–14 days", Destinations = "[\"India\",\"Thailand\",\"Malaysia\"]", Tags = "[\"Orthopaedics\",\"Joint\",\"Surgery\"]", DisplayOrder = 2 },
                new Treatment { Title = "Cancer Treatment", Slug = "cancer-treatment", Category = "Oncology", Description = "Comprehensive oncology care including chemotherapy, radiation therapy, immunotherapy and surgical oncology.", Image = "https://images.unsplash.com/photo-1631815588090-d4bfec5b1ccb?w=600&q=80", Icon = "Shield", Duration = "21–60 days", Destinations = "[\"Germany\",\"Singapore\",\"India\"]", Tags = "[\"Cancer\",\"Oncology\",\"Therapy\"]", DisplayOrder = 3 },
                new Treatment { Title = "Neurosurgery", Slug = "neurosurgery", Category = "Neurology", Description = "Brain and spine surgeries including tumour removal, spinal fusion and deep brain stimulation procedures.", Image = "https://images.unsplash.com/photo-1559757175-0eb30cd8c063?w=600&q=80", Icon = "Brain", Duration = "14–21 days", Destinations = "[\"Germany\",\"Singapore\",\"UAE\"]", Tags = "[\"Brain\",\"Spine\",\"Neurology\"]", DisplayOrder = 4 },
                new Treatment { Title = "Cosmetic Surgery", Slug = "cosmetic-surgery", Category = "Cosmetic & Plastic Surgery", Description = "Aesthetic and reconstructive procedures including rhinoplasty, liposuction, facelifts and body contouring.", Image = "https://images.unsplash.com/photo-1612349317150-e413f6a5b16d?w=600&q=80", Icon = "Sparkles", Duration = "5–10 days", Destinations = "[\"Thailand\",\"Turkey\",\"UAE\"]", Tags = "[\"Cosmetic\",\"Aesthetic\",\"Plastic\"]", DisplayOrder = 5 },
                new Treatment { Title = "Dental Implants", Slug = "dental-implants", Category = "Dental Care", Description = "Full-mouth dental restoration, implants, veneers and advanced cosmetic dentistry procedures.", Image = "https://images.unsplash.com/photo-1606811841689-23dfddce3e95?w=600&q=80", Icon = "Smile", Duration = "5–14 days", Destinations = "[\"Turkey\",\"Thailand\",\"India\"]", Tags = "[\"Dental\",\"Implants\",\"Cosmetic\"]", DisplayOrder = 6 },
                new Treatment { Title = "IVF & Fertility", Slug = "ivf-fertility", Category = "Fertility & IVF", Description = "Assisted reproductive technologies including IVF, ICSI, egg freezing and fertility preservation.", Image = "https://images.unsplash.com/photo-1584820927498-cfe5211fd8bf?w=600&q=80", Icon = "Baby", Duration = "14–21 days", Destinations = "[\"India\",\"Thailand\",\"Malaysia\"]", Tags = "[\"Fertility\",\"IVF\",\"Reproductive\"]", DisplayOrder = 7 },
                new Treatment { Title = "Liver Transplant", Slug = "liver-transplant", Category = "Organ Transplant", Description = "Living and deceased donor liver transplantation with comprehensive pre and post-operative care.", Image = "https://images.unsplash.com/photo-1551190822-a9333d879b1f?w=600&q=80", Icon = "Stethoscope", Duration = "30–45 days", Destinations = "[\"India\",\"Singapore\",\"Germany\"]", Tags = "[\"Transplant\",\"Liver\",\"Surgery\"]", DisplayOrder = 8 },
                new Treatment { Title = "Bariatric Surgery", Slug = "bariatric-surgery", Category = "Bariatric Surgery", Description = "Weight loss surgeries including gastric bypass, sleeve gastrectomy and gastric banding procedures.", Image = "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=600&q=80", Icon = "TrendingDown", Duration = "7–10 days", Destinations = "[\"India\",\"Turkey\",\"Thailand\"]", Tags = "[\"Weight Loss\",\"Bariatric\",\"Surgery\"]", DisplayOrder = 9 },
                new Treatment { Title = "LASIK Eye Surgery", Slug = "lasik-eye-surgery", Category = "Eye Care", Description = "Laser vision correction, cataract surgery and advanced retinal treatments for improved eyesight.", Image = "https://images.unsplash.com/photo-1582750433449-648ed127bb54?w=600&q=80", Icon = "Eye", Duration = "3–5 days", Destinations = "[\"India\",\"Thailand\",\"Singapore\"]", Tags = "[\"Eye\",\"LASIK\",\"Vision\"]", DisplayOrder = 10 },
                new Treatment { Title = "Hair Transplant", Slug = "hair-transplant", Category = "Hair Transplant", Description = "FUE and FUT hair restoration techniques for natural-looking, permanent hair regrowth.", Image = "https://images.unsplash.com/photo-1522337360788-8b13dee7a37e?w=600&q=80", Icon = "User", Duration = "3–5 days", Destinations = "[\"Turkey\",\"India\",\"Thailand\"]", Tags = "[\"Hair\",\"Restoration\",\"Cosmetic\"]", DisplayOrder = 11 },
                new Treatment { Title = "Health Checkup", Slug = "health-checkup", Category = "Health Checkups", Description = "Comprehensive preventive health screening packages with advanced diagnostics and specialist consultations.", Image = "https://images.unsplash.com/photo-1505751172876-fa1923c5c528?w=600&q=80", Icon = "ClipboardList", Duration = "2–3 days", Destinations = "[\"Singapore\",\"India\",\"Germany\"]", Tags = "[\"Preventive\",\"Screening\",\"Wellness\"]", DisplayOrder = 12 }
            );
        }

        if (!db.Destinations.Any())
        {
            db.Destinations.AddRange(
                new Destination { Country = "India", City = "Multiple Cities", Slug = "india", Tagline = "World-Class Care at Accessible Costs", Description = "India is a leading medical tourism destination offering advanced treatments across cardiology, oncology, orthopaedics and more, with internationally trained specialists.", Image = "https://images.unsplash.com/photo-1524492412937-b28074a5d7da?w=700&q=80", Treatments = "[\"Cardiac Surgery\",\"Organ Transplant\",\"Oncology\",\"Orthopaedics\"]", Hospitals = 45, Specialists = 200, Flag = "🇮🇳", Highlights = "[\"JCI Accredited Hospitals\",\"English-Speaking Specialists\",\"Advanced Technology\"]", DisplayOrder = 1 },
                new Destination { Country = "Thailand", City = "Bangkok & Phuket", Slug = "thailand", Tagline = "Excellence in Healthcare and Hospitality", Description = "Thailand combines world-class medical facilities with exceptional hospitality, making it a top choice for cosmetic surgery, dental care and wellness treatments.", Image = "https://images.unsplash.com/photo-1508009603885-50cf7c579365?w=700&q=80", Treatments = "[\"Cosmetic Surgery\",\"Dental Care\",\"Cardiology\",\"Wellness\"]", Hospitals = 32, Specialists = 150, Flag = "🇹🇭", Highlights = "[\"International Hospitals\",\"Wellness Retreats\",\"Affordable Excellence\"]", DisplayOrder = 2 },
                new Destination { Country = "Singapore", City = "Singapore City", Slug = "singapore", Tagline = "Asia's Premier Medical Hub", Description = "Singapore offers cutting-edge medical technology, rigorous standards and a seamless patient experience, particularly for complex oncology and neurology cases.", Image = "https://images.unsplash.com/photo-1525625293386-3f8f99389edd?w=700&q=80", Treatments = "[\"Oncology\",\"Neurology\",\"Cardiology\",\"Health Checkups\"]", Hospitals = 18, Specialists = 90, Flag = "🇸🇬", Highlights = "[\"Highest Medical Standards\",\"Research Excellence\",\"Multilingual Care\"]", DisplayOrder = 3 },
                new Destination { Country = "Germany", City = "Berlin & Munich", Slug = "germany", Tagline = "European Medical Excellence", Description = "Germany is renowned for its precision medicine, advanced research and specialist expertise in neurology, oncology and complex surgical procedures.", Image = "https://images.unsplash.com/photo-1467269204594-9661b134dd2b?w=700&q=80", Treatments = "[\"Neurology\",\"Oncology\",\"Cardiology\",\"Orthopaedics\"]", Hospitals = 22, Specialists = 110, Flag = "🇩🇪", Highlights = "[\"Research Hospitals\",\"Precision Medicine\",\"Specialist Expertise\"]", DisplayOrder = 4 },
                new Destination { Country = "United Arab Emirates", City = "Dubai & Abu Dhabi", Slug = "uae", Tagline = "Modern Healthcare in the Heart of the Gulf", Description = "The UAE offers state-of-the-art medical facilities with internationally trained specialists, serving patients from across the Middle East and beyond.", Image = "https://images.unsplash.com/photo-1512453979798-5ea266f8880c?w=700&q=80", Treatments = "[\"Cardiology\",\"Oncology\",\"Fertility\",\"Wellness\"]", Hospitals = 28, Specialists = 130, Flag = "🇦🇪", Highlights = "[\"World-Class Facilities\",\"Multilingual Support\",\"Luxury Healthcare\"]", DisplayOrder = 5 },
                new Destination { Country = "Malaysia", City = "Kuala Lumpur & Penang", Slug = "malaysia", Tagline = "Quality Healthcare in Southeast Asia", Description = "Malaysia offers high-quality medical care at competitive costs, with internationally accredited hospitals and a welcoming environment for international patients.", Image = "https://images.unsplash.com/photo-1596422846543-75c6fc197f07?w=700&q=80", Treatments = "[\"Fertility\",\"Cardiology\",\"Orthopaedics\",\"Wellness\"]", Hospitals = 20, Specialists = 95, Flag = "🇲🇾", Highlights = "[\"Affordable Quality\",\"English-Speaking\",\"Cultural Diversity\"]", DisplayOrder = 6 },
                new Destination { Country = "Turkey", City = "Istanbul & Ankara", Slug = "turkey", Tagline = "Where East Meets West in Healthcare", Description = "Turkey has emerged as a leading destination for hair transplants, dental care, cosmetic surgery and oncology with modern facilities and experienced specialists.", Image = "https://images.unsplash.com/photo-1541432901042-2d8bd64b4a9b?w=700&q=80", Treatments = "[\"Hair Transplant\",\"Dental Care\",\"Cosmetic Surgery\",\"Oncology\"]", Hospitals = 35, Specialists = 160, Flag = "🇹🇷", Highlights = "[\"Hair Restoration Hub\",\"Dental Excellence\",\"Modern Facilities\"]", DisplayOrder = 7 },
                new Destination { Country = "United Kingdom", City = "London & Manchester", Slug = "united-kingdom", Tagline = "Prestigious British Medical Expertise", Description = "The UK offers access to some of the world's most respected medical specialists and prestigious private hospitals with a long tradition of clinical excellence.", Image = "https://images.unsplash.com/photo-1513635269975-59663e0ac1ad?w=700&q=80", Treatments = "[\"Oncology\",\"Cardiology\",\"Neurology\",\"Orthopaedics\"]", Hospitals = 15, Specialists = 80, Flag = "🇬🇧", Highlights = "[\"Prestigious Specialists\",\"Research Excellence\",\"Private Healthcare\"]", DisplayOrder = 8 }
            );
        }

        if (!db.Hospitals.Any())
        {
            db.Hospitals.AddRange(
                new Hospital { Name = "Apollo Hospitals", Slug = "apollo-hospitals", City = "Chennai", Country = "India", Image = "https://images.unsplash.com/photo-1586773860418-d37222d8fce3?w=600&q=80", Specialties = "[\"Cardiology\",\"Oncology\",\"Neurology\",\"Orthopaedics\"]", Accreditation = "JCI Accredited (Demo)", Description = "A leading multi-specialty hospital offering advanced medical care across 70+ specialties with state-of-the-art technology.", Beds = 700, Established = 1983, Rating = 4.8m, DisplayOrder = 1 },
                new Hospital { Name = "Bumrungrad International", Slug = "bumrungrad-international", City = "Bangkok", Country = "Thailand", Image = "https://images.unsplash.com/photo-1519494026892-80bbd2d6fd0d?w=600&q=80", Specialties = "[\"Cosmetic Surgery\",\"Cardiology\",\"Orthopaedics\",\"Dental\"]", Accreditation = "JCI Accredited (Demo)", Description = "One of Asia's premier international hospitals serving patients from over 190 countries with world-class facilities.", Beds = 580, Established = 1980, Rating = 4.9m, DisplayOrder = 2 },
                new Hospital { Name = "Gleneagles Hospital", Slug = "gleneagles-hospital", City = "Singapore", Country = "Singapore", Image = "https://images.unsplash.com/photo-1538108149393-fbbd81895907?w=600&q=80", Specialties = "[\"Oncology\",\"Neurology\",\"Cardiology\",\"Transplant\"]", Accreditation = "JCI Accredited (Demo)", Description = "A premier tertiary care hospital in Singapore known for excellence in complex medical procedures and patient care.", Beds = 380, Established = 1957, Rating = 4.7m, DisplayOrder = 3 },
                new Hospital { Name = "Charité Medical Centre", Slug = "charite-medical-centre", City = "Berlin", Country = "Germany", Image = "https://images.unsplash.com/photo-1587351021759-3e566b6af7cc?w=600&q=80", Specialties = "[\"Neurology\",\"Oncology\",\"Cardiology\",\"Research\"]", Accreditation = "ISO Certified (Demo)", Description = "One of Europe's largest university hospitals with cutting-edge research facilities and specialist medical expertise.", Beds = 3000, Established = 1710, Rating = 4.9m, DisplayOrder = 4 },
                new Hospital { Name = "Cleveland Clinic Abu Dhabi", Slug = "cleveland-clinic-abu-dhabi", City = "Abu Dhabi", Country = "UAE", Image = "https://images.unsplash.com/photo-1516549655169-df83a0774514?w=600&q=80", Specialties = "[\"Cardiology\",\"Neurology\",\"Oncology\",\"Orthopaedics\"]", Accreditation = "JCI Accredited (Demo)", Description = "A world-class multispecialty hospital bringing Cleveland Clinic's renowned expertise to the Middle East.", Beds = 364, Established = 2015, Rating = 4.8m, DisplayOrder = 5 },
                new Hospital { Name = "Prince Court Medical Centre", Slug = "prince-court-medical-centre", City = "Kuala Lumpur", Country = "Malaysia", Image = "https://images.unsplash.com/photo-1551076805-e1869033e561?w=600&q=80", Specialties = "[\"Fertility\",\"Cardiology\",\"Orthopaedics\",\"Wellness\"]", Accreditation = "ISO Certified (Demo)", Description = "A premium private hospital offering personalised healthcare with a focus on patient comfort and clinical excellence.", Beds = 257, Established = 2008, Rating = 4.7m, DisplayOrder = 6 },
                new Hospital { Name = "Acibadem Healthcare", Slug = "acibadem-healthcare", City = "Istanbul", Country = "Turkey", Image = "https://images.unsplash.com/photo-1504439468489-c8920d796a29?w=600&q=80", Specialties = "[\"Oncology\",\"Cardiology\",\"Hair Transplant\",\"Dental\"]", Accreditation = "JCI Accredited (Demo)", Description = "Turkey's leading healthcare group offering advanced medical treatments with internationally trained specialists.", Beds = 450, Established = 1991, Rating = 4.6m, DisplayOrder = 7 },
                new Hospital { Name = "The London Clinic", Slug = "the-london-clinic", City = "London", Country = "United Kingdom", Image = "https://images.unsplash.com/photo-1580281657702-257584239a55?w=600&q=80", Specialties = "[\"Oncology\",\"Cardiology\",\"Neurology\",\"Orthopaedics\"]", Accreditation = "CQC Registered (Demo)", Description = "A prestigious independent hospital in central London offering exceptional specialist care and personalised treatment.", Beds = 234, Established = 1932, Rating = 4.8m, DisplayOrder = 8 }
            );
        }

        if (!db.Doctors.Any())
        {
            db.Doctors.AddRange(
                new Doctor { Name = "Dr. Arjun Mehta", Slug = "dr-arjun-mehta", Specialty = "Interventional Cardiology", Hospital = "Apollo Hospitals, Chennai", Country = "India", Experience = "22+ years (Sample)", Languages = "[\"English\",\"Hindi\",\"Tamil\"]", Image = "https://images.unsplash.com/photo-1612349317150-e413f6a5b16d?w=400&q=80", Education = "MBBS, MD Cardiology, Fellowship – USA (Sample)", About = "A highly experienced interventional cardiologist specialising in complex coronary interventions and structural heart disease.", Tags = "[\"Cardiology\",\"Heart Surgery\",\"Angioplasty\"]", DisplayOrder = 1 },
                new Doctor { Name = "Dr. Priya Nair", Slug = "dr-priya-nair", Specialty = "Oncology & Haematology", Hospital = "Gleneagles Hospital, Singapore", Country = "Singapore", Experience = "18+ years (Sample)", Languages = "[\"English\",\"Malay\",\"Tamil\"]", Image = "https://images.unsplash.com/photo-1559839734-2b71ea197ec2?w=400&q=80", Education = "MBBS, MD Oncology, Fellowship – UK (Sample)", About = "A dedicated oncologist with expertise in haematological malignancies, breast cancer and targeted therapy protocols.", Tags = "[\"Oncology\",\"Cancer\",\"Haematology\"]", DisplayOrder = 2 },
                new Doctor { Name = "Dr. Klaus Weber", Slug = "dr-klaus-weber", Specialty = "Neurosurgery", Hospital = "Charité Medical Centre, Berlin", Country = "Germany", Experience = "25+ years (Sample)", Languages = "[\"German\",\"English\",\"French\"]", Image = "https://images.unsplash.com/photo-1622253692010-333f2da6031d?w=400&q=80", Education = "MD, PhD Neurosurgery – Heidelberg (Sample)", About = "A pioneering neurosurgeon specialising in minimally invasive brain tumour surgery and complex spinal procedures.", Tags = "[\"Neurosurgery\",\"Brain\",\"Spine\"]", DisplayOrder = 3 },
                new Doctor { Name = "Dr. Siriwan Charoenwong", Slug = "dr-siriwan-charoenwong", Specialty = "Cosmetic & Plastic Surgery", Hospital = "Bumrungrad International, Bangkok", Country = "Thailand", Experience = "16+ years (Sample)", Languages = "[\"Thai\",\"English\",\"Japanese\"]", Image = "https://images.unsplash.com/photo-1594824476967-48c8b964273f?w=400&q=80", Education = "MD, Fellowship Plastic Surgery – USA (Sample)", About = "A renowned plastic surgeon known for natural-looking aesthetic results and advanced reconstructive techniques.", Tags = "[\"Cosmetic\",\"Plastic Surgery\",\"Aesthetic\"]", DisplayOrder = 4 },
                new Doctor { Name = "Dr. Fatima Al-Rashid", Slug = "dr-fatima-al-rashid", Specialty = "Fertility & Reproductive Medicine", Hospital = "Cleveland Clinic Abu Dhabi", Country = "UAE", Experience = "14+ years (Sample)", Languages = "[\"Arabic\",\"English\",\"French\"]", Image = "https://images.unsplash.com/photo-1651008376811-b90baee60c1f?w=400&q=80", Education = "MBBS, MD Reproductive Medicine – UK (Sample)", About = "A compassionate fertility specialist with expertise in IVF, ICSI and advanced reproductive technologies.", Tags = "[\"Fertility\",\"IVF\",\"Reproductive\"]", DisplayOrder = 5 },
                new Doctor { Name = "Dr. Rajesh Kumar", Slug = "dr-rajesh-kumar", Specialty = "Orthopaedic Surgery", Hospital = "Apollo Hospitals, Chennai", Country = "India", Experience = "20+ years (Sample)", Languages = "[\"English\",\"Hindi\",\"Telugu\"]", Image = "https://images.unsplash.com/photo-1537368910025-700350fe46c7?w=400&q=80", Education = "MBBS, MS Orthopaedics, Fellowship – Germany (Sample)", About = "A leading orthopaedic surgeon specialising in joint replacement, sports injuries and complex spinal surgeries.", Tags = "[\"Orthopaedics\",\"Joint Replacement\",\"Sports Medicine\"]", DisplayOrder = 6 },
                new Doctor { Name = "Dr. Mehmet Yilmaz", Slug = "dr-mehmet-yilmaz", Specialty = "Hair Restoration & Dermatology", Hospital = "Acibadem Healthcare, Istanbul", Country = "Turkey", Experience = "12+ years (Sample)", Languages = "[\"Turkish\",\"English\",\"Arabic\"]", Image = "https://images.unsplash.com/photo-1582750433449-648ed127bb54?w=400&q=80", Education = "MD Dermatology, Fellowship Hair Restoration (Sample)", About = "A specialist in advanced FUE hair transplantation and dermatological treatments with thousands of successful procedures.", Tags = "[\"Hair Transplant\",\"Dermatology\",\"FUE\"]", DisplayOrder = 7 },
                new Doctor { Name = "Dr. Sarah Thompson", Slug = "dr-sarah-thompson", Specialty = "Cardiothoracic Surgery", Hospital = "The London Clinic, London", Country = "United Kingdom", Experience = "19+ years (Sample)", Languages = "[\"English\",\"Spanish\"]", Image = "https://images.unsplash.com/photo-1527613426441-4da17471b66d?w=400&q=80", Education = "MBBS, FRCS Cardiothoracic Surgery – London (Sample)", About = "A distinguished cardiothoracic surgeon with expertise in minimally invasive heart surgery and lung procedures.", Tags = "[\"Cardiothoracic\",\"Heart Surgery\",\"Thoracic\"]", DisplayOrder = 8 }
            );
        }

        if (!db.Testimonials.Any())
        {
            db.Testimonials.AddRange(
                new Testimonial { Name = "James Okonkwo", Country = "Nigeria", Treatment = "Cardiac Surgery", Avatar = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=200&q=80", Text = "The assistance I received in coordinating my cardiac procedure was exceptional. Every step was handled professionally and I felt supported throughout my entire journey.", Rating = 5, Note = "(Illustrative demo testimonial)", DisplayOrder = 1 },
                new Testimonial { Name = "Amira Hassan", Country = "Egypt", Treatment = "IVF Treatment", Avatar = "https://images.pexels.com/photos/6129197/pexels-photo-6129197.jpeg", Text = "Finding the right fertility specialist felt overwhelming until I connected with Medgem. The team helped me navigate my options and I felt genuinely cared for.", Rating = 5, Note = "(Illustrative demo testimonial)", DisplayOrder = 2 },
                new Testimonial { Name = "Robert Chen", Country = "Canada", Treatment = "Knee Replacement", Avatar = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=200&q=80", Text = "The coordination of my knee replacement in India was seamless. The hospital was world-class and the support before and after my procedure was outstanding.", Rating = 5, Note = "(Illustrative demo testimonial)", DisplayOrder = 3 },
                new Testimonial { Name = "Fatou Diallo", Country = "Senegal", Treatment = "Cancer Treatment", Avatar = "https://images.unsplash.com/photo-1531746020798-e6953c6e8e04?w=200&q=80", Text = "Medgem helped me access specialist oncology care that was not available locally. The team was compassionate and guided me through a very difficult time.", Rating = 5, Note = "(Illustrative demo testimonial)", DisplayOrder = 4 },
                new Testimonial { Name = "Mohammed Al-Farsi", Country = "Oman", Treatment = "Neurosurgery", Avatar = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=200&q=80", Text = "The second opinion service helped me make a more informed decision about my treatment. I am grateful for the professional and caring support I received.", Rating = 5, Note = "(Illustrative demo testimonial)", DisplayOrder = 5 },
                new Testimonial { Name = "Elena Petrova", Country = "Russia", Treatment = "Cosmetic Surgery", Avatar = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=200&q=80", Text = "From the initial consultation to my recovery, the entire experience was handled with professionalism and warmth. I would highly recommend Medgem to anyone.", Rating = 5, Note = "(Illustrative demo testimonial)", DisplayOrder = 6 }
            );
        }

        if (!db.Faqs.Any())
        {
            db.Faqs.AddRange(
                new Faq { Question = "How can I explore treatment options with Medgem?", Answer = "You can browse our treatments section to discover a wide range of medical specialties and procedures. Use the search and filter tools to find treatments that match your needs, then submit an enquiry for personalised assistance.", DisplayOrder = 1 },
                new Faq { Question = "How do I request a consultation?", Answer = "Click the \"Get Free Consultation\" button in the header or visit our Consultation page. Fill in your details and treatment preferences, and our team will be in touch to discuss your requirements.", DisplayOrder = 2 },
                new Faq { Question = "Can I explore hospitals in different countries?", Answer = "Yes. Our Hospitals section features listings from multiple international destinations including India, Thailand, Singapore, Germany, UAE and more. You can filter by country and specialty.", DisplayOrder = 3 },
                new Faq { Question = "What is a medical second opinion?", Answer = "A medical second opinion involves seeking an independent assessment of your diagnosis or treatment plan from another qualified specialist. This can help you gain confidence and clarity before proceeding with a major medical decision.", DisplayOrder = 4 },
                new Faq { Question = "Can I request medical travel assistance?", Answer = "Medgem can help coordinate various aspects of your medical journey including appointment scheduling, travel planning support and accommodation assistance. Contact us to discuss your specific requirements.", DisplayOrder = 5 },
                new Faq { Question = "How can I contact the Medgem team?", Answer = "You can reach us through our Contact page, by submitting a consultation request, or via the WhatsApp button on our website. Our team is available to assist with your healthcare enquiries.", DisplayOrder = 6 }
            );
        }

        // Seed navigation items
        if (!db.NavItems.Any())
        {
            var home = new NavItem { Label = "Home", Path = "/", DisplayOrder = 1 };
            var treatments = new NavItem { Label = "Treatments", Path = "/treatments", DisplayOrder = 2 };
            var hospitals = new NavItem { Label = "Hospitals", Path = "/hospitals", DisplayOrder = 3 };
            var doctors = new NavItem { Label = "Doctors", Path = "/doctors", DisplayOrder = 4 };
            var destinations = new NavItem { Label = "Destinations", Path = "/destinations", DisplayOrder = 5 };
            var wellness = new NavItem { Label = "Wellness", Path = "/wellness", DisplayOrder = 6 };
            var about = new NavItem { Label = "About Us", Path = "/about", DisplayOrder = 7 };
            var contact = new NavItem { Label = "Contact", Path = "/contact", DisplayOrder = 8 };
            db.NavItems.AddRange(home, treatments, hospitals, doctors, destinations, wellness, about, contact);
            await db.SaveChangesAsync();

            // Treatment dropdown children
            db.NavItems.AddRange(
                new NavItem { Label = "Cardiology", Path = "/treatments?cat=Cardiology", ParentId = treatments.Id, DisplayOrder = 1 },
                new NavItem { Label = "Orthopaedics", Path = "/treatments?cat=Orthopaedics", ParentId = treatments.Id, DisplayOrder = 2 },
                new NavItem { Label = "Oncology", Path = "/treatments?cat=Oncology", ParentId = treatments.Id, DisplayOrder = 3 },
                new NavItem { Label = "Neurology", Path = "/treatments?cat=Neurology", ParentId = treatments.Id, DisplayOrder = 4 },
                new NavItem { Label = "Cosmetic Surgery", Path = "/treatments?cat=Cosmetic", ParentId = treatments.Id, DisplayOrder = 5 },
                new NavItem { Label = "Fertility & IVF", Path = "/treatments?cat=Fertility", ParentId = treatments.Id, DisplayOrder = 6 },
                new NavItem { Label = "View All Treatments", Path = "/treatments", IsHighlight = true, ParentId = treatments.Id, DisplayOrder = 7 }
            );
            // Destination dropdown children
            db.NavItems.AddRange(
                new NavItem { Label = "🇮🇳 India", Path = "/destinations/india", ParentId = destinations.Id, DisplayOrder = 1 },
                new NavItem { Label = "🇹🇭 Thailand", Path = "/destinations/thailand", ParentId = destinations.Id, DisplayOrder = 2 },
                new NavItem { Label = "🇸🇬 Singapore", Path = "/destinations/singapore", ParentId = destinations.Id, DisplayOrder = 3 },
                new NavItem { Label = "🇩🇪 Germany", Path = "/destinations/germany", ParentId = destinations.Id, DisplayOrder = 4 },
                new NavItem { Label = "🇦🇪 UAE", Path = "/destinations/uae", ParentId = destinations.Id, DisplayOrder = 5 },
                new NavItem { Label = "🇹🇷 Turkey", Path = "/destinations/turkey", ParentId = destinations.Id, DisplayOrder = 6 },
                new NavItem { Label = "All Destinations", Path = "/destinations", IsHighlight = true, ParentId = destinations.Id, DisplayOrder = 7 }
            );
        }

        // Seed footer columns and links
        if (!db.FooterColumns.Any())
        {
            var quickLinks = new FooterColumn { Heading = "Quick Links", DisplayOrder = 1 };
            var treatmentsCol = new FooterColumn { Heading = "Treatments", DisplayOrder = 2 };
            var destinationsCol = new FooterColumn { Heading = "Destinations", DisplayOrder = 3 };
            db.FooterColumns.AddRange(quickLinks, treatmentsCol, destinationsCol);
            await db.SaveChangesAsync();

            db.FooterLinks.AddRange(
                new FooterLink { FooterColumnId = quickLinks.Id, Label = "Home", Url = "/", DisplayOrder = 1 },
                new FooterLink { FooterColumnId = quickLinks.Id, Label = "About Us", Url = "/about", DisplayOrder = 2 },
                new FooterLink { FooterColumnId = quickLinks.Id, Label = "Treatments", Url = "/treatments", DisplayOrder = 3 },
                new FooterLink { FooterColumnId = quickLinks.Id, Label = "Hospitals", Url = "/hospitals", DisplayOrder = 4 },
                new FooterLink { FooterColumnId = quickLinks.Id, Label = "Doctors", Url = "/doctors", DisplayOrder = 5 },
                new FooterLink { FooterColumnId = quickLinks.Id, Label = "Wellness", Url = "/wellness", DisplayOrder = 6 },
                new FooterLink { FooterColumnId = quickLinks.Id, Label = "Contact", Url = "/contact", DisplayOrder = 7 },
                new FooterLink { FooterColumnId = treatmentsCol.Id, Label = "Cardiology", Url = "/treatments?cat=Cardiology", DisplayOrder = 1 },
                new FooterLink { FooterColumnId = treatmentsCol.Id, Label = "Orthopaedics", Url = "/treatments?cat=Orthopaedics", DisplayOrder = 2 },
                new FooterLink { FooterColumnId = treatmentsCol.Id, Label = "Oncology", Url = "/treatments?cat=Oncology", DisplayOrder = 3 },
                new FooterLink { FooterColumnId = treatmentsCol.Id, Label = "Neurology", Url = "/treatments?cat=Neurology", DisplayOrder = 4 },
                new FooterLink { FooterColumnId = treatmentsCol.Id, Label = "Cosmetic Surgery", Url = "/treatments?cat=Cosmetic+Surgery", DisplayOrder = 5 },
                new FooterLink { FooterColumnId = treatmentsCol.Id, Label = "Fertility & IVF", Url = "/treatments?cat=Fertility", DisplayOrder = 6 },
                new FooterLink { FooterColumnId = treatmentsCol.Id, Label = "Dental Care", Url = "/treatments?cat=Dental+Care", DisplayOrder = 7 },
                new FooterLink { FooterColumnId = treatmentsCol.Id, Label = "Hair Transplant", Url = "/treatments?cat=Hair+Transplant", DisplayOrder = 8 },
                new FooterLink { FooterColumnId = destinationsCol.Id, Label = "India", Url = "/destinations/india", DisplayOrder = 1 },
                new FooterLink { FooterColumnId = destinationsCol.Id, Label = "Thailand", Url = "/destinations/thailand", DisplayOrder = 2 },
                new FooterLink { FooterColumnId = destinationsCol.Id, Label = "Singapore", Url = "/destinations/singapore", DisplayOrder = 3 },
                new FooterLink { FooterColumnId = destinationsCol.Id, Label = "Germany", Url = "/destinations/germany", DisplayOrder = 4 },
                new FooterLink { FooterColumnId = destinationsCol.Id, Label = "UAE", Url = "/destinations/uae", DisplayOrder = 5 },
                new FooterLink { FooterColumnId = destinationsCol.Id, Label = "Malaysia", Url = "/destinations/malaysia", DisplayOrder = 6 },
                new FooterLink { FooterColumnId = destinationsCol.Id, Label = "Turkey", Url = "/destinations/turkey", DisplayOrder = 7 },
                new FooterLink { FooterColumnId = destinationsCol.Id, Label = "United Kingdom", Url = "/destinations/united-kingdom", DisplayOrder = 8 }
            );
        }

        await db.SaveChangesAsync();
    }
}
