using System;
using System.Reflection;
using CommandLine;

namespace Mailer {

    public class Options {

        [Option("f", "from", Required = true, HelpText = "From address")]
        public string From = string.Empty;

        [Option("t", "to", Required = true, HelpText = "To address seperated by comma")]
        public string To = string.Empty;

        [Option("c", "cc", Required = false, HelpText = "Cc address seperated by comma")]
        public string Cc = string.Empty;

        [Option("b", "bcc", Required = false, HelpText = "Bcc address seperated by comma")]
        public string Bcc = string.Empty;

        [Option("s", "subject", Required = true, HelpText = "Subject for the email")]
        public string Subject = string.Empty;

        [Option("m", "body", Required = true, HelpText = "Email body")]
        public string Body = string.Empty;

        [Option("a", "attachment", Required = false, HelpText = "Files to be attached seperated by comma")]
        public string Attachment = string.Empty;

        [Option("l", "configration", Required = true, HelpText = "Mail client configuration file")]
        public string ConfigurationFile = string.Empty;

        private Options() {
        }

        public static Options Create(string[] args) {
            Options mailOptions = new Options();
            ICommandLineParser parser = new CommandLineParser();

            parser.ParseArguments(args, mailOptions);
            return mailOptions;
        }

        public ErrorList Validate() {
            Type optionsType = Assembly.GetExecutingAssembly().GetType("Mailer.Options");
            ErrorList errors = new ErrorList();

            foreach (FieldInfo field in optionsType.GetFields()) {
                foreach (Attribute attr in Attribute.GetCustomAttributes(field)) {
                    if (attr.GetType() == typeof(OptionAttribute)) {
                        if (((OptionAttribute)attr).Required) {
                            if (string.IsNullOrEmpty(((string)field.GetValue(this)))) {
                                errors.Add(new ErrorInfo(field.Name, "Can't be blank"));
                            }
                        }
                    }
                }
            }

            return errors;
        }
    }
}
