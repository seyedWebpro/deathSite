using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;
using api.Model.AdminModel;
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

        public apiContext(DbContextOptions<apiContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تعریف رابطه بین User و Package
            modelBuilder.Entity<User>()
                .HasOne(u => u.Package) // هر کاربر یک پکیج دارد
                .WithMany(p => p.Users) // هر پکیج می‌تواند به چندین کاربر تعلق داشته باشد
                .HasForeignKey(u => u.PackageId); // کلید خارجی در User

            // تعریف کلید مرکب برای ShahidTag
            modelBuilder.Entity<ShahidTag>()
                .HasKey(st => new { st.ShahidId, st.TagId });

            // تعریف رابطه بین Shahid و ShahidTag
            modelBuilder.Entity<ShahidTag>()
                .HasOne(st => st.Shahid)
                .WithMany(s => s.ShahidTags)
                .HasForeignKey(st => st.ShahidId);

            // تعریف رابطه بین Tag و ShahidTag
            modelBuilder.Entity<ShahidTag>()
                .HasOne(st => st.Tag)
                .WithMany(t => t.ShahidTags)
                .HasForeignKey(st => st.TagId);

           // رابطه بین Deceased و User
    modelBuilder.Entity<Deceased>()
        .HasOne(d => d.Owner)
        .WithMany(u => u.Deceaseds)
        .HasForeignKey(d => d.OwnerId)
        .OnDelete(DeleteBehavior.Restrict); // برای جلوگیری از حذف خودکار متوفی در صورت حذف کاربر

     // رابطه بین CondolenceMessage و User
    modelBuilder.Entity<CondolenceMessage>()
        .HasOne(cm => cm.User)
        .WithMany(u => u.CondolenceMessages)
        .HasForeignKey(cm => cm.UserId)
        .OnDelete(DeleteBehavior.Restrict);  // تغییر رفتار در صورت حذف کاربر

    // رابطه بین CondolenceMessage و Deceased
    modelBuilder.Entity<CondolenceMessage>()
        .HasOne(cm => cm.Deceased)
        .WithMany(d => d.CondolenceMessages)
        .HasForeignKey(cm => cm.DeceasedId)
        .OnDelete(DeleteBehavior.Cascade);  // حذف خودکار پیام‌ها هنگام حذف متوفی
        }

    }
}