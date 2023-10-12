using Emte.Core.Models.Request;

namespace Emte.Core.API
{
    public interface IEmailService
    {
        Task SendMail(SendMailRequest sendMailRequest, CancellationToken cancellationToken);
    }
}