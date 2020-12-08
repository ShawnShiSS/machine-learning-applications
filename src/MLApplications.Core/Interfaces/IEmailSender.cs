using System.Threading.Tasks;

namespace MLApplications.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string fromEmail, string toEmail, string subject, string message);
    }
}
