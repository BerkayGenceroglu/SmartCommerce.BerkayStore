using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationWorker.Services
{
    public interface IMailService
    {
        Task SendOrderConfirmationAsync(string toEmail, string fullName, Guid orderId, decimal totalAmount);

    }
}
