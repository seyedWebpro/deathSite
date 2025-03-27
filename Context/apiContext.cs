using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;
using api.Model.AdminModel;
using deathSite.Model;
using deathSite.Model.AdminModel;
using Microsoft.EntityFrameworkCore;

namespace api.Context
{
    public class apiContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<SmsTemplate> smsTemplates { get; set; }
        public DbSet<Package> packages { get; set; }
        public DbSet<CondolenceMessage> condolenceMessages { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<Shahid> shahids { get; set; }
        public DbSet<UserToken> userTokens { get; set; }
        public DbSet<ShahidTag> ShahidTags { get; set; }
        public DbSet<Blog> blogs { get; set; }
        public DbSet<LogoSiteSettings> logoSiteSettings { get; set; }
        public DbSet<MenuSiteSettings> MenuSiteSettings { get; set; }
        public DbSet<Surah> Surahs { get; set; }
        public DbSet<Deceased> Deceaseds { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ContactMeForm> ContactMeForms { get; set; }
        public DbSet<Factors> Factors { get; set; }
        public DbSet<PaymentSettings> PaymentSettings { get; set; }
        public DbSet<Poster> posters { get; set; }
        public DbSet<DeceasedLocation> DeceasedLocations { get; set; }
        public DbSet<SavedDeceased> SavedDeceaseds { get; set; }
        public DbSet<LikeDeceased> LikeDeceaseds { get; set; }
        public DbSet<ShahidViewCount> ShahidViewCounts { get; set; }
        public DbSet<DeathViewCount> DeathViewCounts { get; set; }
        public DbSet<Elamieh> Elamiehs { get; set; }
        public DbSet<CondolenceReply> CondolenceReplies { get; set; }
        public DbSet<DeceasedPackage> DeceasedPackages { get; set; }
        public DbSet<AboutUs> aboutUs { get; set; }
        public DbSet<ShahidUpdateRequest> ShahidUpdateRequests { get; set; }
        public DbSet<GolestanShohadaSection> GolestanShohadaSections { get; set; }


        public apiContext(DbContextOptions<apiContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // 1. تنظیم رابطه یک به یک بین Deceased و Sarbarg (اختیاری)
            modelBuilder.Entity<Deceased>()
                .HasOne(d => d.Sarbarg)
                .WithOne(s => s.Deceased)
                .HasForeignKey<Deceased>(d => d.SarbargId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. تنظیم روابط مربوط به DeathViewCount
            modelBuilder.Entity<DeathViewCount>()
                .HasOne(dvc => dvc.Deceased)
                .WithMany(d => d.DeathViews)
                .HasForeignKey(dvc => dvc.DeceasedId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeathViewCount>()
                .HasOne(dvc => dvc.User)
                .WithMany(u => u.DeathViews)
                .HasForeignKey(dvc => dvc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. تنظیم روابط مربوط به ShahidViewCount
            modelBuilder.Entity<ShahidViewCount>()
                .HasOne(svc => svc.Shahid)
                .WithMany(s => s.ShahidViews)
                .HasForeignKey(svc => svc.ShahidId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShahidViewCount>()
                .HasOne(svc => svc.User)
                .WithMany(u => u.ShahidViews)
                .HasForeignKey(svc => svc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 4. تنظیم کلید مرکب و روابط مربوط به LikeDeceased
            modelBuilder.Entity<LikeDeceased>()
                .HasKey(ld => new { ld.UserId, ld.DeceasedId });

            modelBuilder.Entity<LikeDeceased>()
                .HasOne(ld => ld.User)
                .WithMany(u => u.LikedDeceaseds)
                .HasForeignKey(ld => ld.UserId);

            modelBuilder.Entity<LikeDeceased>()
                .HasOne(ld => ld.Deceased)
                .WithMany(d => d.LikedByUsers)
                .HasForeignKey(ld => ld.DeceasedId);

            // 5. تنظیم کلید مرکب و روابط مربوط به SavedDeceased
            modelBuilder.Entity<SavedDeceased>()
                .HasKey(sd => new { sd.UserId, sd.DeceasedId });

            modelBuilder.Entity<SavedDeceased>()
                .HasOne(sd => sd.User)
                .WithMany(u => u.SavedDeceaseds)
                .HasForeignKey(sd => sd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedDeceased>()
                .HasOne(sd => sd.Deceased)
                .WithMany(d => d.SavedByUsers)
                .HasForeignKey(sd => sd.DeceasedId)
                .OnDelete(DeleteBehavior.Cascade);

            // 6. تنظیم روابط DeceasedPackage (جایگزین UserPackage)
            modelBuilder.Entity<DeceasedPackage>()
                .HasOne(dp => dp.Deceased)
                .WithMany(d => d.Packages)
                .HasForeignKey(dp => dp.DeceasedId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeceasedPackage>()
                .HasOne(dp => dp.Package)
                .WithMany(p => p.DeceasedPackages)
                .HasForeignKey(dp => dp.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // 7. تنظیم رابطه بین DeceasedPackage و Factor (اختیاری)
            modelBuilder.Entity<DeceasedPackage>()
                .HasOne(dp => dp.Factor)
                .WithMany(f => f.DeceasedPackages)
                .HasForeignKey(dp => dp.FactorId)
                .IsRequired(false) // چون FactorId می‌تواند null باشد (برای پکیج رایگان)
                .OnDelete(DeleteBehavior.Restrict);

            // 8. تنظیم رابطه بین Package و DeceasedPackage
            modelBuilder.Entity<Package>()
                .HasMany(p => p.DeceasedPackages)
                .WithOne(dp => dp.Package)
                .HasForeignKey(dp => dp.PackageId);

            // 9. تنظیم کلید مرکب برای ShahidTag و روابط مربوط به آن
            modelBuilder.Entity<ShahidTag>()
                .HasKey(st => new { st.ShahidId, st.TagId });

            modelBuilder.Entity<ShahidTag>()
                .HasOne(st => st.Shahid)
                .WithMany(s => s.ShahidTags)
                .HasForeignKey(st => st.ShahidId);

            modelBuilder.Entity<ShahidTag>()
                .HasOne(st => st.Tag)
                .WithMany(t => t.ShahidTags)
                .HasForeignKey(st => st.TagId);

             // 10. تنظیم رابطه بین Deceased و User
            modelBuilder.Entity<Deceased>()
                .HasOne(d => d.User)
                .WithMany(u => u.Deceaseds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 11. تنظیم روابط مربوط به CondolenceMessage
            modelBuilder.Entity<CondolenceMessage>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.CondolenceMessages)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CondolenceMessage>()
                .HasOne(cm => cm.Deceased)
                .WithMany(d => d.CondolenceMessages)
                .HasForeignKey(cm => cm.DeceasedId)
                .OnDelete(DeleteBehavior.Cascade);

            // 12. تنظیم روابط مربوط به Factors
            modelBuilder.Entity<Factors>()
                .HasOne(f => f.User)
                .WithMany(u => u.Factors)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factors>()
                .HasOne(f => f.Package)
                .WithMany(p => p.Factors)
                .HasForeignKey(f => f.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // 13. تنظیم رابطه بین Shahid و User
            modelBuilder.Entity<Shahid>()
                .HasOne(s => s.User)
                .WithMany(u => u.Shahids)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 14. تنظیم رابطه یک به یک بین DeceasedLocation و Deceased
            modelBuilder.Entity<DeceasedLocation>()
                .HasOne(dl => dl.Deceased)
                .WithOne(d => d.DeceasedLocation)
                .HasForeignKey<DeceasedLocation>(dl => dl.DeceasedId)
                .OnDelete(DeleteBehavior.Cascade);

            // 15. تنظیم روابط مربوط به Elamieh
            modelBuilder.Entity<Elamieh>()
                .HasOne(e => e.Deceased)
                .WithMany(d => d.Elamiehs)
                .HasForeignKey(e => e.DeceasedId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Elamieh>()
                .HasOne(e => e.User)
                .WithMany(u => u.Elamiehs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 16. تنظیم رابطه بین CondolenceReply و CondolenceMessage
            modelBuilder.Entity<CondolenceReply>()
                .HasOne(cr => cr.CondolenceMessage)
                .WithMany(cm => cm.Replies)
                .HasForeignKey(cr => cr.CondolenceMessageId)
                .OnDelete(DeleteBehavior.Cascade);

            // 17. اضافه کردن پروپرتی Factors به مدل User
            modelBuilder.Entity<User>()
                .HasMany(u => u.Factors)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);

            // 18. اضافه کردن پروپرتی Factors به مدل Package
            modelBuilder.Entity<Package>()
                .HasMany(p => p.Factors)
                .WithOne(f => f.Package)
                .HasForeignKey(f => f.PackageId);

            // 19. اضافه کردن پروپرتی DeceasedPackages به مدل Package
            modelBuilder.Entity<Package>()
                .HasMany(p => p.DeceasedPackages)
                .WithOne(dp => dp.Package)
                .HasForeignKey(dp => dp.PackageId);

                  // تنظیم رابطه یک به چند بین Shahid و ShahidUpdateRequest
    modelBuilder.Entity<ShahidUpdateRequest>()
        .HasOne(ur => ur.Shahid)
        .WithMany(s => s.UpdateRequests)
        .HasForeignKey(ur => ur.ShahidId)
        .OnDelete(DeleteBehavior.Cascade);

    // تنظیم رابطه یک به چند بین User و ShahidUpdateRequest
    modelBuilder.Entity<ShahidUpdateRequest>()
        .HasOne(ur => ur.User)
        .WithMany(u => u.ShahidUpdateRequests)
        .HasForeignKey(ur => ur.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            // سایر تنظیمات و کانفیگ‌های اضافی در صورت نیاز...
        }

    }
}