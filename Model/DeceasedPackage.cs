using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class DeceasedPackage
    {
        public int Id { get; set; }
        
        public int DeceasedId { get; set; }
        public Deceased Deceased { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }

        public DateTime ActivationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        
        // افزودن فیلد برای مشخص کردن پکیج رایگان
        public bool IsFreePackage { get; set; }
        
        // افزودن فیلد برای ارتباط با تراکنش خرید (در صورت پکیج پولی)
        public int? FactorId { get; set; }
        public Factors Factor { get; set; }
    }
}