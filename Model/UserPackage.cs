using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class UserPackage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PackageId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        // آمار استفاده از پکیج
        public int UsedImageCount { get; set; }
        public int UsedVideoCount { get; set; }
        public int UsedNotificationCount { get; set; }
        public int UsedAudioFileCount { get; set; }

        // روابط
        public User User { get; set; }
        public Package Package { get; set; }
        public ICollection<PaymentInvoice> PaymentInvoices { get; set; }
    }
}