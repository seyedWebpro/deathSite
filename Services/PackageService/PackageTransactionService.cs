using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Model;
using deathSite.Model;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Services.PackageService
{
    public class PackageTransactionService : IPackageTransactionService
    {
        private readonly apiContext _dbContext;

        public PackageTransactionService(apiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleNewPackageRegistration(Factors factor, Package package)
        {
            var userPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.UserId == factor.UserId && up.PackageId == factor.PackageId.Value);

            if (userPackage == null)
            {
                var newUserPackage = new UserPackage
                {
                    UserId = factor.UserId,
                    PackageId = package.Id,
                    PurchaseDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(package.ValidityPeriod)),
                    IsActive = true,
                    UsedImageCount = 0,
                    UsedVideoCount = 0,
                    UsedNotificationCount = 0,
                    UsedAudioFileCount = 0
                };

                _dbContext.UserPackages.Add(newUserPackage);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task HandlePackageRenewal(Factors factor, Package package)
        {
            var userPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.Id == factor.UserPackageId);

            if (userPackage != null)
            {
                var daysToExpiry = (userPackage.ExpiryDate - DateTime.UtcNow).Days;
                if (daysToExpiry <= 30)
                {
                    if (userPackage.ExpiryDate < DateTime.UtcNow)
                    {
                        // در صورتی که بسته منقضی شده باشد، تمدید از تاریخ جاری انجام می‌شود
                        userPackage.ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(package.ValidityPeriod));
                    }
                    else
                    {
                        // در صورتی که بسته هنوز منقضی نشده، مدت تمدید به تاریخ انقضا اضافه می‌شود
                        userPackage.ExpiryDate = userPackage.ExpiryDate.AddDays(int.Parse(package.ValidityPeriod));
                    }
                }
                else
                {
                    // اگر فاصله زمان باقی‌مانده بیش از ۳۰ روز باشد، تاریخ انقضا به همان صورت تمدید می‌شود
                    userPackage.ExpiryDate = userPackage.ExpiryDate.AddDays(int.Parse(package.ValidityPeriod));
                }

                userPackage.IsActive = true;
                _dbContext.UserPackages.Update(userPackage);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task HandlePackageUpgrade(Factors factor, Package newPackage)
        {
            var currentUserPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.Id == factor.UserPackageId);

            if (currentUserPackage != null)
            {
                // غیرفعال کردن بسته فعلی
                currentUserPackage.IsActive = false;
                _dbContext.UserPackages.Update(currentUserPackage);

                // ایجاد بسته جدید با ویژگی‌های ارتقاء یافته
                var newUserPackage = new UserPackage
                {
                    UserId = factor.UserId,
                    PackageId = newPackage.Id,
                    PurchaseDate = currentUserPackage.PurchaseDate,
                    ExpiryDate = currentUserPackage.ExpiryDate,
                    IsActive = true,
                    UsedImageCount = currentUserPackage.UsedImageCount,
                    UsedVideoCount = currentUserPackage.UsedVideoCount,
                    UsedNotificationCount = currentUserPackage.UsedNotificationCount,
                    UsedAudioFileCount = currentUserPackage.UsedAudioFileCount
                };

                _dbContext.UserPackages.Add(newUserPackage);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}