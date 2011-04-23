using Mailer;
using NUnit.Framework;
using NUnit.Mocks;
using System.IO;
using System;
using System.Net.Mail;

namespace MailerTest {

    [TestFixture]
    class MailerTest {

        [Test]
        public void ShouldCreateMailByPassingOptionsAndConfiguration() {
            string[] args = new string[] { };
            ClientConfiguration configuration = new ClientConfiguration();
            Mail mail = new Mail(Options.Create(args), configuration);
        }

        [Test]
        public void ShouldBeInValidIfInValidOptionsAndValidConfigurationAreNotPassed() {
            string[] args = new string[] { };
            ClientConfiguration configuration = new ClientConfiguration() { UserName = "user", Password = "password", Host = "smtp.google.com", Port = 445, Ssl = true };
            Mail mail = new Mail(Options.Create(args), configuration);
            ErrorList errors = mail.Validate();

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Options can't be invalid", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInValidIfValidOptionsAndInValidConfigurationAreNotPassed() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody", "-lclient.conf" };
            ClientConfiguration configuration = new ClientConfiguration();
            Mail mail = new Mail(Options.Create(args), configuration);
            ErrorList errors = mail.Validate();

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("ClientConfiguration can't be invalid", errors[0].ToString());
        }

        [Test]
        public void ShouldBeValidIfValidOptionsAndConfigurationArePassed() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody", "-lclient.conf" };
            ClientConfiguration configuration = new ClientConfiguration() { UserName = "user", Password = "password", Host = "smtp.google.com", Port = 445, Ssl = true };
            Mail mail = new Mail(Options.Create(args), configuration);
            ErrorList errors = mail.Validate();

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void MailMessageShouldHaveTheFromAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.AreEqual("from@someone.com", message.From.Address);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "From can't be blank")]
        public void ShouldThrowInvalidOperationExceptionWhenFromAddressIsNotPassed() {
            string[] args = new string[] { "-tsomeone@someone.com", "-sTest", "-mBody" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.IsNull(message.From);
        }

        [Test]
        public void MailMessageShouldHaveTheToAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someoneelse@someone.com", "-sTest", "-mBody" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.AreEqual(2, message.To.Count);
            Assert.AreEqual("someone@someone.com", message.To[0].Address);
            Assert.AreEqual("someoneelse@someone.com", message.To[1].Address);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "To can't be blank")]
        public void ShouldThrowInvalidOperationExceptionWhenToAddressIsNotPassed() {
            string[] args = new string[] { "-ffrom@someone.com", "-sTest", "-mBody" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.IsEmpty(message.To);
        }

        [Test]
        public void MailMessageShouldHaveSubject() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.AreEqual("Test", message.Subject);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Subject can't be blank")]
        public void ShouldThrowInvalidOperationExceptionWhenSubjectIsNotPassed() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-mBody" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.IsNullOrEmpty(message.Subject);
        }

        [Test]
        public void MailMessageShouldHaveBody() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.AreEqual("Body", message.Body);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Body can't be blank")]
        public void ShouldThrowInvalidOperationExceptionWhenBodyIsNotPassed() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.IsNullOrEmpty(message.Body);
        }

        [Test]
        public void MailMessageShouldHaveTheCcAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someonelse@someone.com", "-sTest", "-mBody", "-ccc1@someone.com,cc2@someone.com" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.AreEqual(2, message.CC.Count);
            Assert.AreEqual("cc1@someone.com", message.CC[0].Address);
            Assert.AreEqual("cc2@someone.com", message.CC[1].Address);
        }

        [Test]
        public void MailMessageShouldHaveEmptyCcAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someonelse@someone.com", "-sTest", "-mBody", "-c" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.IsEmpty(message.CC);
        }

        [Test]
        public void MailMessageShouldHaveTheBccAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someonelse@someone.com", "-sTest", "-mBody", "-bbcc1@someone.com,bcc2@someone.com" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.AreEqual(2, message.Bcc.Count);
            Assert.AreEqual("bcc1@someone.com", message.Bcc[0].Address);
            Assert.AreEqual("bcc2@someone.com", message.Bcc[1].Address);
        }

        [Test]
        public void MailMessageShouldHaveEmptyBCcAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someonelse@someone.com", "-sTest", "-mBody", "-b" };
            Mail mail = new Mail(Options.Create(args), new ClientConfiguration());

            MailMessage message = mail.GetMessageFromOptions();
            Assert.IsEmpty(message.Bcc);
        }
    }
}