using System;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace Mailer {
    public class ClientConfiguration {

        public string UserName { get; set; }

        public string Host { get; set; }

        public string Password { get; set; }

        public Nullable<int> Port { get; set; }

        public Nullable<bool> Ssl { get; set; }

        public static ClientConfiguration Create(string fileName) {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientConfiguration));
            using (StreamReader reader = new StreamReader(fileName)) {
                return (ClientConfiguration)serializer.Deserialize(reader);
            }
        }

        public virtual void Save(string fileName) {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientConfiguration));
            using (StreamWriter writer = new StreamWriter(fileName)) {
                serializer.Serialize(writer, this);
            }
        }

        public ErrorList Validate() {
            Type clientConfigurationType = Assembly.GetExecutingAssembly().GetType("Mailer.ClientConfiguration");
            ErrorList errors = new ErrorList();
            foreach (PropertyInfo property in clientConfigurationType.GetProperties()) {
                if (property.GetValue(this, null) == null)
                    errors.Add(new ErrorInfo(property.Name, "Can't be blank"));
            }
            return errors;
        }

        public static ClientConfiguration CreateSampleConfiguration() {
            return new ClientConfiguration() {
                UserName = "username",
                Password = "password",
                Host = "smtp.gmail.com",
                Port = 465,
                Ssl = true
            };
        }
    }
}