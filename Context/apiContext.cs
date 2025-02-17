using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;
using api.Model.AdminModel;
using deathSite.Model;
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
        public DbSet<UserPackage> UserPackages { get; set; }
        public DbSet<PaymentSettings> PaymentSettings { get; set; }

        public apiContext(DbContextOptions<apiContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. تعریف رابطه بین UserPackage و User
            modelBuilder.Entity<UserPackage>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPackages)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. تعریف رابطه بین UserPackage و Package
            modelBuilder.Entity<UserPackage>()
                .HasOne(up => up.Package)
                .WithMany(p => p.UserPackages)
                .HasForeignKey(up => up.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. تعریف کلید مرکب برای ShahidTag
            modelBuilder.Entity<ShahidTag>()
                .HasKey(st => new { st.ShahidId, st.TagId });

            // 4. تعریف رابطه بین Shahid و ShahidTag
            modelBuilder.Entity<ShahidTag>()
                .HasOne(st => st.Shahid)
                .WithMany(s => s.ShahidTags)
                .HasForeignKey(st => st.ShahidId);

            // 5. تعریف رابطه بین Tag و ShahidTag
            modelBuilder.Entity<ShahidTag>()
                .HasOne(st => st.Tag)
                .WithMany(t => t.ShahidTags)
                .HasForeignKey(st => st.TagId);

            // 6. تعریف رابطه بین Deceased و User
            modelBuilder.Entity<Deceased>()
                .HasOne(d => d.User)
                .WithMany(u => u.Deceaseds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 7. تعریف رابطه بین CondolenceMessage و User
            modelBuilder.Entity<CondolenceMessage>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.CondolenceMessages)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 8. تعریف رابطه بین CondolenceMessage و Deceased
            modelBuilder.Entity<CondolenceMessage>()
                .HasOne(cm => cm.Deceased)
                .WithMany(d => d.CondolenceMessages)
                .HasForeignKey(cm => cm.DeceasedId)
                .OnDelete(DeleteBehavior.Cascade);

            // 9. تعریف رابطه بین Factors و User
            modelBuilder.Entity<Factors>()
                .HasOne(pi => pi.User)
                .WithMany(u => u.Factors)
                .HasForeignKey(pi => pi.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 10. تعریف رابطه بین Factors و UserPackage (اختیاری)
            modelBuilder.Entity<Factors>()
                .HasOne(pi => pi.UserPackage)
                .WithMany(up => up.Factors)
                .HasForeignKey(pi => pi.UserPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // 11. تعریف رابطه بین Shahid و User
            modelBuilder.Entity<Shahid>()
                .HasOne(s => s.User)
                .WithMany(u => u.Shahids)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}