using System;
using Mailer;
using NUnit.Framework;

namespace MailerTest {

    [TestFixture]
    class OptionsTest {

        [Test]
        public void ShouldBeAbleToCreateNewOptionFromArgs() {
            string[] args = new string[] { };
            Options option = Options.Create(args);
        }

        [Test]
        public void ShouldAssignToField() {
            string[] args = new string[] { "-tsomeone@org.com,someoneelse@org.com" };
            Options option = Options.Create(args);
            Assert.AreEqual("someone@org.com,someoneelse@org.com", option.To);
        }

        [Test]
        public void ShouldAssignCcField() {
            string[] args = new string[] { "-csomeone@org.com,someoneelse@org.com" };
            Options option = Options.Create(args);
            Assert.AreEqual("someone@org.com,someoneelse@org.com", option.Cc);
        }

        [Test]
        public void ShouldAssignBccField() {
            string[] args = new string[] { "-bsomeone@org.com,someoneelse@org.com" };
            Options option = Options.Create(args);
            Assert.AreEqual("someone@org.com,someoneelse@org.com", option.Bcc);
        }

        [Test]
        public void ShouldAssignSubjectField() {
            string[] args = new string[] { "-sSubject" };
            Options option = Options.Create(args);
            Assert.AreEqual("Subject", option.Subject);
        }

        [Test]
        public void ShouldAssignBodytField() {
            string[] args = new string[] { "-mHi how are you" };
            Options option = Options.Create(args);
            Assert.AreEqual("Hi how are you", option.Body);
        }

        [Test]
        public void ShouldAssignAttachment() {
            string[] args = new String[] { "-afile.txt,file1.txt" };
            Options option = Options.Create(args);
            Assert.AreEqual("file.txt,file1.txt", option.Attachment);
        }

        [Test]
        public void ShouldAssignConfigrationFile() {
            string[] args = new string[] { "-lmail_client.conf" };
            Options option = Options.Create(args);
            Assert.AreEqual("mail_client.conf", option.ConfigurationFile);
        }

        [Test]
        public void ShouldBeValidWhenToSubjectBodyAndClientArePresent() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody", "-lclient.conf" };
            Options option = Options.Create(args);
            ErrorList errors = option.Validate();
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void ShouldBeInvalidWhenFromIsNotPresent() {
            string[] args = new string[] { "-tsomeone@someone.com", "-sTest", "-mBody", "-lclient.conf" };
            Options option = Options.Create(args);
            ErrorList errors = option.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("From can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInvalidWhenToIsNotPresent() {
            string[] args = new string[] { "-ffrom@someone.com", "-sTest", "-mBody", "-lclient.conf" };
            Options option = Options.Create(args);
            ErrorList errors = option.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("To can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInvalidWhenSubjectIsNotPresent() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-mBody", "-lclient.conf" };
            Options option = Options.Create(args);
            ErrorList errors = option.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Subject can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInvalidWhenBodyIsNotPresent() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-lclient.conf" };
            Options option = Options.Create(args);
            ErrorList errors = option.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Body can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInvalidWhenConfigurationFileIsNotPresent() {
            string[] args = new string[] { "-ffrom@someone.com", "-tsomeone@someone.com", "-sTest", "-mBody" };
            Options option = Options.Create(args);
            ErrorList errors = option.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("ConfigurationFile can't be blank", errors[0].ToString());
        }
    }
}