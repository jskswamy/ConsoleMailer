using System.Net.Mail;

namespace Mailer {

    public interface ISmtpClient {
        ClientConfiguration Configuration { get; }
        void Send(MailMessage message);
    }

}