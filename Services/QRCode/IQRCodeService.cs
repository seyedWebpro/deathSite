using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.Services.QRCode
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string url);
    }
}