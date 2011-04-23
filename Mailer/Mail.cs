using System;
using System.Net.Mail;

namespace Mailer {

    public class Mail {
        private Options _options;
        private ISmtpClient _client;

        public Mail(Options options, ISmtpClient client) {
            this._options = options;
            this._client = client;
        }

        public ErrorList Validate() {
            ErrorList errors = new ErrorList();
            if (this.ValidateOptions().HasErrors) {
                errors.Add(new ErrorInfo("Options", "Can't be invalid"));
            }
            return errors;
        }

        public void Send() {
            MailMessage message = GetMessageFromOptions();
            _client.Send(message);
        }

        private MailMessage GetMessageFromOptions() {
            MailMessage message = new MailMessage();
            string[] requiredFields = new string[] { "From", "To", "Subject", "Body" };
            foreach (string requiredField in requiredFields) {
                ThrowInvalidOperationIfErrorOnField(requiredField);
            }
            message.From = new MailAddress(this._options.From);
            message.Subject = this._options.Subject;
            message.Body = this._options.Body;
            ConvertStringToMailAddressCollection(message.To, this._options.To);
            ConvertStringToMailAddressCollection(message.CC, this._options.Cc);
            ConvertStringToMailAddressCollection(message.Bcc, this._options.Bcc);
            return message;
        }

        private void ThrowInvalidOperationIfErrorOnField(string fieldName) {
            ErrorInfo error = ValidateOptions()[fieldName];
            if (error != null)
                throw new InvalidOperationException(error.ToString());
        }

        private ErrorList ValidateOptions() {
            return this._options.Validate();
        }

        private void ConvertStringToMailAddressCollection(MailAddressCollection addresses, string addressSeperatedByCommas) {
            foreach (string address in addressSeperatedByCommas.Split(',')) {
                if (!String.IsNullOrEmpty(address))
                    addresses.Add(address);
            }
        }
    }
}