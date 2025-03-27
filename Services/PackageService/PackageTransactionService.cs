using System;
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

        /// <summary>
        /// خرید پکیج برای یک متوفی خاص (در صورت وجود پکیج قبلی، جایگزین می‌شود)
        /// </summary>
        public async Task<DeceasedPackage> HandleNewPackageRegistration(Factors factor, Package package)
        {
            // بررسی وجود پکیج قبلی برای همین متوفی
            var existingPackage = await _dbContext.DeceasedPackages
                .FirstOrDefaultAsync(dp => dp.DeceasedId == factor.DeceasedId.Value && dp.IsActive);

            if (existingPackage != null && existingPackage.IsFreePackage)
            {
                // غیرفعال کردن پکیج رایگان قبلی
                existingPackage.IsActive = false;
                _dbContext.DeceasedPackages.Update(existingPackage);
            }


            // ثبت پکیج جدید
            var newDeceasedPackage = new DeceasedPackage
            {
                DeceasedId = factor.DeceasedId.Value,
                PackageId = package.Id,
                ActivationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(int.Parse(package.ValidityPeriod)), // فرض بر این است که ValidityPeriod یک عدد صحیح است
                IsActive = true,
                IsFreePackage = false, // در صورتی که این پکیج رایگان نیست
                FactorId = factor.Id // ارتباط با تراکنش پرداخت
            };

            _dbContext.DeceasedPackages.Add(newDeceasedPackage);
            await _dbContext.SaveChangesAsync();

            return newDeceasedPackage;
        }

        /// <summary>
        /// تمدید پکیج برای یک متوفی مشخص
        /// </summary>
        public async Task HandlePackageRenewal(Factors factor, Package package)
        {
            var deceasedPackage = await _dbContext.DeceasedPackages
                .FirstOrDefaultAsync(dp => dp.DeceasedId == factor.DeceasedId.Value && dp.IsActive);

            if (deceasedPackage != null)
            {
                // بررسی اینکه آیا تاریخ انقضا گذشته است یا نه
                if (deceasedPackage.ExpirationDate.HasValue)
                {
                    if (DateTime.UtcNow > deceasedPackage.ExpirationDate.Value)
                    {
                        // اگر تاریخ فعلی از تاریخ انقضا گذشته بود، یک سال از تاریخ فعلی تمدید می‌شود
                        deceasedPackage.ExpirationDate = DateTime.UtcNow.AddYears(1);
                    }
                    else
                    {
                        // اگر تاریخ فعلی هنوز به تاریخ انقضا نرسیده بود، یک سال از تاریخ انقضا قبلی تمدید می‌شود
                        deceasedPackage.ExpirationDate = deceasedPackage.ExpirationDate.Value.AddYears(1);
                    }
                }
                else
                {
                    throw new InvalidOperationException("تاریخ انقضا معتبر برای این پکیج وجود ندارد.");
                }

                // ذخیره تغییرات
                _dbContext.DeceasedPackages.Update(deceasedPackage);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("پکیج فعال برای این متوفی یافت نشد.");
            }
        }


        /// <summary>
        /// ارتقاء پکیج برای متوفی مشخص
        /// </summary>
        public async Task HandlePackageUpgrade(Factors factor, Package newPackage)
        {
            var deceasedPackage = await _dbContext.DeceasedPackages
                .FirstOrDefaultAsync(dp => dp.DeceasedId == factor.DeceasedId.Value && dp.IsActive);

            if (deceasedPackage != null)
            {
                // غیرفعال کردن پکیج قبلی
                deceasedPackage.IsActive = false;
                _dbContext.DeceasedPackages.Update(deceasedPackage);

                // ثبت پکیج جدید با تاریخ انقضای پکیج قبلی
                var upgradedDeceasedPackage = new DeceasedPackage
                {
                    DeceasedId = factor.DeceasedId.Value,
                    PackageId = newPackage.Id,
                    ActivationDate = DateTime.UtcNow,
                    ExpirationDate = deceasedPackage.ExpirationDate, // تاریخ انقضا از پکیج قبلی حفظ می‌شود
                    IsActive = true,
                    IsFreePackage = false, // در صورت ارتقاء به پکیج غیر رایگان
                    FactorId = factor.Id
                };

                _dbContext.DeceasedPackages.Add(upgradedDeceasedPackage);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("پکیج فعال برای این متوفی یافت نشد.");
            }
        }

    }
}
