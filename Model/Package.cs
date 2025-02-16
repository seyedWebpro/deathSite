using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using deathSite.Model;

namespace api.Model
{
public class Package
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Duration { get; set; } // مدت زمان

        public decimal Price { get; set; }

        public decimal RenewalFee { get; set; }

        public string ValidityPeriod { get; set; } // مدت اعتبار

        // محدودیت‌های پکیج
        public int ImageCount { get; set; }
        public int VideoCount { get; set; }
        public int NotificationCount { get; set; }
        public int AudioFileLimit { get; set; }

        // امکانات پکیج
        public bool BarcodeEnabled { get; set; }
        public bool DisplayEnabled { get; set; }
        public bool TemplateSelectionEnabled { get; set; }
        public bool CondolenceMessageEnabled { get; set; }
        public bool VisitorContentSubmissionEnabled { get; set; }
        public bool LocationAndNavigationEnabled { get; set; }
        public bool SharingEnabled { get; set; }
        public bool File360DegreeEnabled { get; set; }
        public bool UpdateCapabilityEnabled { get; set; }

        // روابط
        public ICollection<UserPackage> UserPackages { get; set; }
    }

}