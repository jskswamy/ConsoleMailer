using Mailer;
using NUnit.Framework;

namespace MailerTest {

    [TestFixture]
    class ErrorInfoTest {

        [Test]
        public void ShouldBeAbleToCreateErrorInfoWithNameAndErrorText() {
            ErrorInfo info = new ErrorInfo("Name");
            info.Add("Can't be blank");
            Assert.AreEqual("Name", info.FieldName);
            Assert.AreEqual("Can't be blank", info.Errors[0]);
        }

        [Test]
        public void ShouldBeAbleToAddMoreThanOneError() {
            ErrorInfo info = new ErrorInfo("Name");
            info.Add("Can't be blank");
            info.Add("Can't be less than 10");
            Assert.AreEqual("Name can't be blank and can't be less than 10", info.ToString());
        }

        [Test]
        public void ToStringShouldCombineFieldNameAndErrorText() {
            ErrorInfo info = new ErrorInfo("Name");
            info.Add("Can't be blank");
            Assert.AreEqual("Name can't be blank", info.ToString());
        }

        [Test]
        public void ShouldBeAbleToPassErrosAreParams() {
            ErrorInfo info = new ErrorInfo("Name", "Can't be blank", "Can't be less than 10");
            Assert.AreEqual("Name can't be blank and can't be less than 10", info.ToString());
        }
    }
}
