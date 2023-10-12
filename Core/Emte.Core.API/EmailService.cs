using System.Net;
using System.Net.Mail;
using Emte.Core.Models.Request;

namespace Emte.Core.API
{
    public class GmailService : IEmailService
    {
        private readonly IAppEmailConfig _emailConfig;
        public GmailService(IAppEmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public async Task SendMail(SendMailRequest sendMailRequest, CancellationToken cancellationToken)
        {
            Console.WriteLine($"sending mail from {_emailConfig.Email} to {sendMailRequest.ToAddress}");
            var fromAddress = new MailAddress(_emailConfig.Email!, "Connect2Save");
            var toAddress = new MailAddress(sendMailRequest.ToAddress!, sendMailRequest.ToAddress);
            string fromPassword = _emailConfig.Password!;
            string subject = sendMailRequest.Subject!;
            string body = sendMailRequest.Body!;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            }) { smtp.Send(message); }
        }
    }
}