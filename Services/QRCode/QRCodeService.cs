using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;

namespace deathSite.Services.QRCode
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQRCode(string url)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                // ایجاد دیتا برای QR Code با استفاده از URL مقصد
                var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                // تولید تصویر QR Code به صورت بایت آرایه
                return qrCode.GetGraphic(20);
            }
        }
    }
}