using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Packages
{
    public class UpgradePackageRequestDto
    {
        [Required(ErrorMessage = "شناسه کاربر الزامی است.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "شناسه پکیج فعلی خریداری شده الزامی است.")]
        public int PackageId { get; set; } // پکیجی که کاربر در حال حاضر دارد

        [Required(ErrorMessage = "شناسه پکیج جدید الزامی است.")]
        public int NewPackageId { get; set; } // پکیجی که کاربر قصد ارتقا به آن را دارد

        [Required(ErrorMessage = "شناسه سفارش الزامی است.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "شناسه پرداخت کننده الزامی است.")]
        public string PayerId { get; set; }
    }
}