using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public interface ISmsService
    {
        Task SendAuthenticationSmsAsync(string phoneNumber);
        Task SendPasswordResetSmsAsync(string phoneNumber);
        Task SendOrderSuccessSmsAsync(string phoneNumber);
        Task SendRenewalSuccessSmsAsync(string phoneNumber);
        Task SendUpgradeSuccessSmsAsync(string phoneNumber);
        Task SendRenewalReminderSmsAsync(string phoneNumber);
        
    }
}