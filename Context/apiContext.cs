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
        public DbSet<User> users {get; set;}
        public DbSet<SmsTemplate> smsTemplates {get; set;}
        public DbSet<Package> packages {get; set;}
        public DbSet<CondolenceMessage> condolenceMessages {get; set;}
        public DbSet<Tag> tags {get; set;}
        public DbSet<Shahid> shahids {get; set;}
        public DbSet<UserToken> userTokens {get; set;}
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

            // سایر تنظیمات مدل‌ها در اینجا
        }
        
    }
}