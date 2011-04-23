using System;
using System.Net.Mail;
using Mailer;
using NUnit.Framework;

namespace MailerTest {

    [TestFixture]
    public class NativeSmtpClientTest {

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowInvalidOperationExceptionForInvalidConfiguration() {
            ClientConfiguration configuration = new ClientConfiguration() { Host = "smtp.google.com", Port = 445, Ssl = false };
            NativeSmtpClient client = new NativeSmtpClient(configuration);

            client.Send(new MailMessage());
        }
    }

}
