using System;
using Mailer;
using NUnit.Framework;
using CommandLine;
using System.IO;
using System.Text;

namespace MailerTest {

    [TestFixture]
    class OptionsTest {

        [Test]
        public void ShouldBeAbleToCreateNewOptionFromArgs() {
            string[] args = new string[] { };

            Options option = Options.Create(args);
        }

        [Test]
        public void ByDefaultOptionsShouldBeCaseInsensitive() {
            string[] args = new string[] { "-tsomeone@org.com", "-Ffrom@someone.org" };

            Options option = Options.Create(args);
            Assert.AreEqual("someone@org.com", option.To[0]);
            Assert.AreEqual("from@someone.org", option.From);
        }

        [Test]
        public void ShouldBeAbleToCreateNewOptionsFromArgsAndSpecifyingCommandLineParserSettings() {
            string[] args = new string[] { "-h" };
            StringWriter writer = new StringWriter();
            CommandLineParserSettings settings = new CommandLineParserSettings(writer);
            StringBuilder expectedHelpText = new StringBuilder();
            expectedHelpText.AppendLine("CommandLine Utility to send email using SMTP protocol");
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "f", "from".PadRight(15, ' '), "From address"));
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "t", "to".PadRight(15, ' '), "To address seperated by comma"));
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "c", "cc".PadRight(15, ' '), "Cc address seperated by comma"));
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "b", "bcc".PadRight(15, ' '), "Bcc address seperated by comma"));
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "s", "subject".PadRight(15, ' '), "Subject for the email"));
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "m", "body".PadRight(15, ' '), "Email body"));
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "a", "attachment".PadRight(15, ' '), "Files to be attached seperated by comma"));
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "l", "configration".PadRight(15, ' '), "Mail client configuration file"));
            expectedHelpText.AppendLine(String.Format("-{0} -{1}{2}", "g", "generate".PadRight(15, ' '), "Generates sample configuration file"));

            Options option = Options.Create(args, settings);
            Assert.AreEqual(expectedHelpText.ToString(), writer.ToString());
        }

        [Test]
        public void ShouldAssignToField() {
            string[] args = new string[] { "-tsomeone@org.com,someoneelse@org.com" };

            Options option = Options.Create(args);
            Assert.AreEqual("someone@org.com", option.To[0]);
            Assert.AreEqual("someoneelse@org.com", option.To[1]);
        }

        [Test]
        public void ShouldAssignCcField() {
            string[] args = new string[] { "-csomeone@org.com,someoneelse@org.com" };

            Options option = Options.Create(args);
            Assert.AreEqual("someone@org.com", option.Cc[0]);
            Assert.AreEqual("someoneelse@org.com", option.Cc[1]);
        }

        [Test]
        public void ShouldAssignBccField() {
            string[] args = new string[] { "-bsomeone@org.com,someoneelse@org.com" };

            Options option = Options.Create(args);
            Assert.AreEqual("someone@org.com", option.Bcc[0]);
            Assert.AreEqual("someoneelse@org.com", option.Bcc[1]);
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
            Assert.AreEqual("file.txt", option.Attachment[0]);
            Assert.AreEqual("file1.txt", option.Attachment[1]);
        }

        [Test]
        public void ShouldAssignConfigrationFile() {
            string[] args = new string[] { "-lmail_client.conf" };

            Options option = Options.Create(args);
            Assert.AreEqual("mail_client.conf", option.ConfigurationFile);
        }

        [Test]
        public void ShouldAssignSampleConfigurationFilePath() {
            string[] args = new String[] { "-gsample.txt", "-lmail_client.conf" };

            Options option = Options.Create(args);
            Assert.AreEqual("sample.txt", option.SampleConfigurationFilePath);
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