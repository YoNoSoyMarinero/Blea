using server.Models;

namespace server.Interfaces
{
    public interface IMailUtility
    {
        Task<bool> SendEmailAsync(MailData mailData, CancellationToken ct);
    }
}
