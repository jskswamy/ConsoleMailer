using System.Collections.ObjectModel;

namespace Mailer {

    public class ErrorList : Collection<ErrorInfo> {
        public bool HasErrors { get { return this.Count > 0; } }

        public ErrorInfo this[string fieldName] {
            get {
                foreach (ErrorInfo info in this) {
                    if (info.FieldName.CompareTo(fieldName) == 0)
                        return info;
                }
                return null;
            }
        }

    }
}
