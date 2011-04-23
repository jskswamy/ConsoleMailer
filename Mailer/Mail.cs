using System;
using System.Net.Mail;

namespace Mailer {

    public class Mail {
        private Options _options;
        private ClientConfiguration _clientConfiguration;

        public MailMessage GetMessageFromOptions() {
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

        public Mail(Options options, ClientConfiguration configuration) {
            this._options = options;
            this._clientConfiguration = configuration;
        }

        public ErrorList Validate() {
            ErrorList errors = new ErrorList();
            if (this.ValidateOptions().HasErrors) {
                errors.Add(new ErrorInfo("Options", "Can't be invalid"));
            }
            if (this.ValidateClientConfiguration().HasErrors) {
                errors.Add(new ErrorInfo("ClientConfiguration", "Can't be invalid"));
            }
            return errors;
        }

        public void Send() {
            MailMessage message = GetMessageFromOptions();
        }

        private void ThrowInvalidOperationIfErrorOnField(string fieldName) {
            ErrorInfo error = ValidateOptions()[fieldName];
            if (error != null)
                throw new InvalidOperationException(error.ToString());
        }

        private ErrorList ValidateOptions() {
            return this._options.Validate();
        }

        private ErrorList ValidateClientConfiguration() {
            return this._clientConfiguration.Validate();
        }

        private void ConvertStringToMailAddressCollection(MailAddressCollection addresses, string addressSeperatedByCommas) {
            foreach (string address in addressSeperatedByCommas.Split(',')) {
                if (!String.IsNullOrEmpty(address))
                    addresses.Add(address);
            }
        }
    }
}