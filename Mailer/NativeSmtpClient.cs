using System;
using System.Net.Mail;
using System.Net;

namespace Mailer {
    public class NativeSmtpClient : ISmtpClient {

        private ClientConfiguration _configuration;

        public NativeSmtpClient(ClientConfiguration configuration) {
            this._configuration = configuration;
        }

        public ClientConfiguration Configuration {
            get { return _configuration; }
        }

        public void Send(MailMessage message) {
            if (Configuration.Validate().HasErrors) {
                ErrorInfo error = new ErrorInfo("Configuration", "is invalid");
                throw new InvalidOperationException(error.ToString());
            }
            SmtpClient client = new SmtpClient(Configuration.Host, Configuration.Port.Value);
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(Configuration.UserName, Configuration.Password);
            client.EnableSsl = Configuration.Ssl.Value;
            client.Send(message);
        }
    }
}