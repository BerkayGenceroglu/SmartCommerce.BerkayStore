using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NotificationWorker.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailService> _logger;
        public MailService(IConfiguration configuration, ILogger<MailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task SendOrderConfirmationAsync(string toEmail, string fullName, Guid orderId, decimal totalAmount)
        {
            try
            {
                var orderIdShort = orderId.ToString().Substring(0, 8).ToUpper();
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["Mail:From"]));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = $"Siparişiniz Alındı! #{orderIdShort}";
                email.Body = new TextPart("html")
                {
                    Text = $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
  <meta charset='UTF-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1.0' />
  <title>Sipariş Onayı</title>
</head>
<body style='margin:0;padding:0;background:#f4f4f0;font-family:Georgia,serif;'>

  <!-- WRAPPER -->
  <table width='100%' cellpadding='0' cellspacing='0' style='background:#f4f4f0;padding:40px 0;'>
    <tr>
      <td align='center'>
        <table width='600' cellpadding='0' cellspacing='0' style='background:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.08);'>

          <!-- HEADER -->
          <tr>
            <td style='background:#1a6b3c;padding:36px 40px;text-align:center;'>
              <h1 style='margin:0;color:#ffffff;font-family:Georgia,serif;font-size:28px;font-weight:700;letter-spacing:0.05em;'>Berkay Store</h1>
              <p style='margin:8px 0 0;color:rgba(255,255,255,0.75);font-size:13px;letter-spacing:0.08em;text-transform:uppercase;font-family:Arial,sans-serif;'>Sipariş Onayı</p>
            </td>
          </tr>

          <!-- HERO -->
          <tr>
            <td style='padding:40px 40px 24px;text-align:center;border-bottom:1px solid #f0ede8;'>
              <div style='width:64px;height:64px;background:#f0f9f4;border-radius:50%;display:inline-flex;align-items:center;justify-content:center;margin-bottom:16px;'>
                <span style='font-size:28px;'>✅</span>
              </div>
              <h2 style='margin:0 0 8px;color:#1a1a1a;font-family:Georgia,serif;font-size:22px;font-weight:700;'>Siparişiniz Alındı!</h2>
              <p style='margin:0;color:#666;font-family:Arial,sans-serif;font-size:14px;line-height:1.6;'>Merhaba <strong style='color:#1a1a1a;'>{fullName}</strong>, siparişiniz başarıyla oluşturuldu.<br/>En kısa sürede hazırlanmaya başlanacak.</p>
            </td>
          </tr>

          <!-- ORDER DETAILS -->
          <tr>
            <td style='padding:24px 40px;'>
              <table width='100%' cellpadding='0' cellspacing='0' style='background:#f9f7f4;border-radius:10px;overflow:hidden;'>
                <tr>
                  <td style='padding:20px 24px;border-bottom:1px solid #ede9e3;'>
                    <table width='100%' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='font-family:Arial,sans-serif;font-size:12px;color:#999;text-transform:uppercase;letter-spacing:0.08em;'>Sipariş Numarası</td>
                        <td align='right' style='font-family:Georgia,serif;font-size:15px;font-weight:700;color:#1a6b3c;'>#{orderIdShort}</td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td style='padding:20px 24px;border-bottom:1px solid #ede9e3;'>
                    <table width='100%' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='font-family:Arial,sans-serif;font-size:12px;color:#999;text-transform:uppercase;letter-spacing:0.08em;'>Sipariş Tarihi</td>
                        <td align='right' style='font-family:Arial,sans-serif;font-size:14px;color:#333;font-weight:500;'>{DateTime.Now.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("tr-TR"))}</td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td style='padding:20px 24px;'>
                    <table width='100%' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='font-family:Arial,sans-serif;font-size:12px;color:#999;text-transform:uppercase;letter-spacing:0.08em;'>Toplam Tutar</td>
                        <td align='right' style='font-family:Georgia,serif;font-size:20px;font-weight:700;color:#1a6b3c;'>{totalAmount.ToString("N0", new System.Globalization.CultureInfo("tr-TR"))}₺</td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- STATUS STEPS -->
          <!-- INFO BOX -->
          <tr>
            <td style='padding:0 40px 32px;'>
              <table width='100%' cellpadding='0' cellspacing='0' style='background:#f0f9f4;border:1px solid #c8e6d4;border-radius:8px;'>
                <tr>
                  <td style='padding:16px 20px;'>
                    <p style='margin:0;font-family:Arial,sans-serif;font-size:13px;color:#1a6b3c;line-height:1.6;'>
                      🚚 <strong>Ücretsiz kargo</strong> ile siparişiniz en geç 3-5 iş günü içinde teslim edilecektir.<br/>
                      📦 Kargo takip numaranız hazırlandığında ayrıca bildirilecektir.
                    </p>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- FOOTER -->
          <tr>
            <td style='background:#f9f7f4;padding:24px 40px;text-align:center;border-top:1px solid #ede9e3;'>
              <p style='margin:0 0 8px;font-family:Georgia,serif;font-size:15px;font-weight:700;color:#1a1a1a;'>Berkay Store.</p>
              <p style='margin:0;font-family:Arial,sans-serif;font-size:12px;color:#999;line-height:1.6;'>
                Modern alışverişin yeni adresi.<br/>
                Bu mail otomatik olarak gönderilmiştir, lütfen yanıtlamayınız.
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>

</body>
</html>"
                };
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(
                    _configuration["Mail:Host"],
                    int.Parse(_configuration["Mail:Port"]!),
                    SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(
                    _configuration["Mail:Username"],
                    _configuration["Mail:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Mail gönderildi. To: {Email}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError("Mail gönderilemedi. Hata: {Error}", ex.Message);
            }
        }
    }
}