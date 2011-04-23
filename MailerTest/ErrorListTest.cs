using Mailer;
using NUnit.Framework;

namespace MailerTest {

    [TestFixture]
    class ErrorListTest {

        [Test]
        public void ShouldBeAbleToAddErrorInfo() {
            ErrorList errors = new ErrorList();
            errors.Add(new ErrorInfo("Name"));
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void HasErrorShouldBeTrue() {
            ErrorList errors = new ErrorList();
            errors.Add(new ErrorInfo("Name"));
            Assert.IsTrue(errors.HasErrors);
        }

        [Test]
        public void HasErrorShouldBeFalse() {
            ErrorList errors = new ErrorList();
            Assert.IsFalse(errors.HasErrors);
        }

        [Test]
        public void ShouldBeAbleToGetErrorInfoByFieldName() {
            ErrorList errors = new ErrorList();
            errors.Add(new ErrorInfo("Name", "Can't be blank"));
            Assert.AreEqual("Name can't be blank", errors["Name"].ToString());
        }

        [Test]
        public void ShouldReturnNullIfFieldNameDoesntExists() {
            ErrorList errors = new ErrorList();
            errors.Add(new ErrorInfo("Name", "Can't be blank"));
            Assert.IsNull(errors["Age"]);
        }
    }
}
