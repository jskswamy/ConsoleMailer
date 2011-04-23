using System;
using System.Net.Mail;
using Mailer;
using NUnit.Framework;
using Moq;

namespace MailerTest {

    [TestFixture]
    class MailerTest {

        [Test]
        public void ShouldCreateMailByPassingOptionsAndISmtpClient() {
            string[] args = new string[] { };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
        }

        [Test]
        public void ShouldBeInValidIfInValidOptionsIsPassed() {
            string[] args = new string[] { };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            ErrorList errors = mail.Validate();

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Options can't be invalid", errors[0].ToString());
        }

        [Test]
        public void MailMessageShouldHaveTheFromAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => Assert.AreEqual("from@someone.com", message.From.Address));

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "From can't be blank")]
        public void ShouldThrowInvalidOperationExceptionWhenFromAddressIsNotPassed() {
            string[] args = new string[] { "-tsomeone@someone.com", "-sTest", "-mBody" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => Assert.IsNull(message.From));

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        public void MailMessageShouldHaveTheToAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someoneelse@someone.com", "-sTest", "-mBody" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => {
                Assert.AreEqual(2, message.To.Count);
                Assert.AreEqual("someone@someone.com", message.To[0].Address);
                Assert.AreEqual("someoneelse@someone.com", message.To[1].Address);
            });

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "To can't be blank")]
        public void ShouldThrowInvalidOperationExceptionWhenToAddressIsNotPassed() {
            string[] args = new string[] { "-ffrom@someone.com", "-sTest", "-mBody" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>()));

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        public void MailMessageShouldHaveSubject() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => Assert.AreEqual("Test", message.Subject));

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Subject can't be blank")]
        public void ShouldThrowInvalidOperationExceptionWhenSubjectIsNotPassed() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-mBody" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>()));

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        public void MailMessageShouldHaveBody() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => Assert.AreEqual("Body", message.Body));

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Body can't be blank")]
        public void ShouldThrowInvalidOperationExceptionWhenBodyIsNotPassed() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>()));

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        public void MailMessageShouldHaveTheCcAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someonelse@someone.com", "-sTest", "-mBody", "-ccc1@someone.com,cc2@someone.com" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => {
                Assert.AreEqual(2, message.CC.Count);
                Assert.AreEqual("cc1@someone.com", message.CC[0].Address);
                Assert.AreEqual("cc2@someone.com", message.CC[1].Address);
            });

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        public void MailMessageShouldHaveEmptyCcAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someonelse@someone.com", "-sTest", "-mBody", "-c" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => Assert.IsEmpty(message.CC));

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        public void MailMessageShouldHaveTheBccAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someonelse@someone.com", "-sTest", "-mBody", "-bbcc1@someone.com,bcc2@someone.com" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => {
                Assert.AreEqual(2, message.Bcc.Count);
                Assert.AreEqual("bcc1@someone.com", message.Bcc[0].Address);
                Assert.AreEqual("bcc2@someone.com", message.Bcc[1].Address);
            });

            mail.Send();
            mock.VerifyAll();
        }

        [Test]
        public void MailMessageShouldHaveEmptyBCcAddress() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com,someonelse@someone.com", "-sTest", "-mBody", "-b" };
            var mock = new Mock<ISmtpClient>();
            Mail mail = new Mail(Options.Create(args), mock.Object);
            mock.Setup(client => client.Send(It.IsAny<MailMessage>())).Callback((MailMessage message) => Assert.IsEmpty(message.Bcc));

            mail.Send();
            mock.VerifyAll();
        }
    }
}