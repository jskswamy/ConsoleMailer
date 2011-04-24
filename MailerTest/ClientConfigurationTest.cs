using System.IO;
using Mailer;
using Moq;
using NUnit.Framework;
using System.Xml.Serialization;

namespace MailerTest {

    [TestFixture]
    class ClientConfigurationTest {

        private const string configurationFileName = "Sample.xml";

        [Test]
        public void ShouldBeAbleToSaveTheConfigurationToFile() {
            ClientConfiguration configuration = new ClientConfiguration();
            configuration.Save(configurationFileName);
            Assert.IsTrue(File.Exists(configurationFileName));
            File.Delete(configurationFileName);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ShouldThrowFileNotFoundException() {
            ClientConfiguration configuration = ClientConfiguration.Create(configurationFileName);
        }

        [Test]
        public void ShouldBeAbleToLoadThePropertiesBackFromXml() {
            ClientConfiguration expected_configuration = new ClientConfiguration() { UserName = "user", Password = "password", Host = "smtp.google.com", Port = 445, Ssl = true };
            expected_configuration.Save(configurationFileName);
            ClientConfiguration configuration = ClientConfiguration.Create(configurationFileName);

            Assert.AreEqual(expected_configuration.UserName, configuration.UserName);
            Assert.AreEqual(expected_configuration.Password, configuration.Password);
            Assert.AreEqual(expected_configuration.Host, configuration.Host);
            Assert.AreEqual(expected_configuration.Port, configuration.Port);
            Assert.AreEqual(expected_configuration.Ssl, configuration.Ssl);
            File.Delete(configurationFileName);
        }

        [Test]
        public void ShouldBeInvalidIfUserNameIsNull() {
            ClientConfiguration configuration = new ClientConfiguration() { Password = "password", Host = "smtp.google.com", Port = 445, Ssl = true };
            ErrorList errors = configuration.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("UserName can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInvalidIfPasswordIsNull() {
            ClientConfiguration configuration = new ClientConfiguration() { UserName = "user", Host = "smtp.google.com", Port = 445, Ssl = true };
            ErrorList errors = configuration.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Password can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInvalidIfHostIsNull() {
            ClientConfiguration configuration = new ClientConfiguration() { UserName = "user", Password = "password", Port = 445, Ssl = true };
            ErrorList errors = configuration.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Host can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInvalidIfPortIsNull() {
            ClientConfiguration configuration = new ClientConfiguration() { UserName = "user", Password = "password", Host = "smtp.google.com", Ssl = true };
            ErrorList errors = configuration.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Port can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldBeInvalidIfSslIsNull() {
            ClientConfiguration configuration = new ClientConfiguration() { UserName = "user", Password = "password", Host = "smtp.google.com", Port = 445 };
            ErrorList errors = configuration.Validate();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Ssl can't be blank", errors[0].ToString());
        }

        [Test]
        public void ShouldReturnDummyGmailConfiguration() {
            ClientConfiguration configuration = ClientConfiguration.CreateSampleConfiguration();

            Assert.AreEqual("smtp.gmail.com", configuration.Host);
            Assert.AreEqual(465, configuration.Port.Value);
            Assert.AreEqual("username", configuration.UserName);
            Assert.AreEqual("password", configuration.Password);
            Assert.AreEqual(true, configuration.Ssl);
        }
    }
}