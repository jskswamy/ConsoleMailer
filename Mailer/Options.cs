using System;
using System.Reflection;
using CommandLine;
using System.Collections.Generic;

namespace Mailer {

    public class Options {

        [Option("f", "from", Required = true, HelpText = "From address")]
        public string From = string.Empty;

        [OptionList("t", "to", Separator = ',', Required = true, HelpText = "To address seperated by comma")]
        public List<string> To = null;

        [OptionList("c", "cc", Separator = ',', Required = false, HelpText = "Cc address seperated by comma")]
        public List<string> Cc = null;

        [OptionList("b", "bcc", Separator = ',', Required = false, HelpText = "Bcc address seperated by comma")]
        public List<string> Bcc = null;

        [Option("s", "subject", Required = true, HelpText = "Subject for the email")]
        public string Subject = string.Empty;

        [Option("m", "body", Required = true, HelpText = "Email body")]
        public string Body = string.Empty;

        [OptionList("a", "attachment", Separator = ',', Required = false, HelpText = "Files to be attached seperated by comma")]
        public List<string> Attachment = null;

        [Option("l", "configration", Required = true, HelpText = "Mail client configuration file")]
        public string ConfigurationFile = string.Empty;

        private Options() {
        }

        public static Options Create(string[] args) {
            return Create(args, new CommandLineParserSettings(false));
        }

        public static Options Create(string[] args, CommandLineParserSettings settings) {
            Options mailOptions = new Options();
            ICommandLineParser parser = new CommandLineParser(settings);

            parser.ParseArguments(args, mailOptions);
            return mailOptions;
        }

        public ErrorList Validate() {
            Type optionsType = Assembly.GetExecutingAssembly().GetType("Mailer.Options");
            ErrorList errors = new ErrorList();

            foreach (FieldInfo field in optionsType.GetFields()) {
                foreach (Attribute attr in Attribute.GetCustomAttributes(field)) {
                    Type attributeType = attr.GetType();
                    if (attributeType.IsSubclassOf(typeof(BaseOptionAttribute))) {
                        if (((BaseOptionAttribute)attr).Required) {
                            object fieldValue = field.GetValue(this);
                            bool blank = fieldValue == null ? true : (fieldValue.GetType().Equals(typeof(string)) ? string.IsNullOrEmpty(((string)fieldValue)) : false);
                            Console.WriteLine("{0}: {1}", field.Name, blank);
                            if (blank) { errors.Add(new ErrorInfo(field.Name, "Can't be blank")); }
                        }
                    }
                }
            }

            return errors;
        }
    }
}
