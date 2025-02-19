using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Packages
{
     public class RenewPackageRequestDto
    {
        [Required(ErrorMessage = "شناسه کاربر الزامی است.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "شناسه پکیج خریداری شده الزامی است.")]
        public int PackageId { get; set; }

        [Required(ErrorMessage = "شناسه سفارش الزامی است.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "شناسه پرداخت کننده الزامی است.")]
        public string PayerId { get; set; }
    }
}