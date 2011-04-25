using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailer;
using CommandLine;
using System.IO;

namespace ConsoleMailer {
    class Program {
        static void Main(string[] args) {
            ClientConfiguration configuration;
            Options options = Options.Create(args, new CommandLineParserSettings(Console.Out));
            Mail smtpMail;

            if (!String.IsNullOrEmpty(options.SampleConfigurationFilePath)) {
                configuration = ClientConfiguration.CreateSampleConfiguration();
                configuration.Save(options.SampleConfigurationFilePath);
                Console.WriteLine("Sample configuration file generated at {0}", Path.GetFullPath(options.SampleConfigurationFilePath));
                return;
            }

            ErrorList optionErrors = options.Validate();
            if (optionErrors.HasErrors) {
                PrintErrors(optionErrors);
                return;
            }

            configuration = ClientConfiguration.Create(options.ConfigurationFile);
            ErrorList configurationErrors = options.Validate();
            if (configurationErrors.HasErrors) {
                PrintErrors(configurationErrors);
                return;
            }

            try {
                smtpMail = new Mail(options, new NativeSmtpClient(configuration));
                smtpMail.Send();
            }
            catch (Exception ex) {
                Console.WriteLine("Unable to send the email reason: {0}", ex.Message);
            }
        }

        static void PrintErrors(ErrorList errors) {
            foreach (ErrorInfo error in errors) {
                Console.WriteLine(error.ToString());
            }
        }
    }
}
