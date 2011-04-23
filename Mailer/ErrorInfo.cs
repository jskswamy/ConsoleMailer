using System;
using System.Text;
using System.Collections.Generic;

namespace Mailer {

    public class ErrorInfo {
        private string _fieldName;
        private List<string> _errors;
        
        public ErrorInfo(string fieldName) {
            this._fieldName = fieldName;
            _errors = new List<string>();
        }

        public ErrorInfo(string fieldName, params string[] errors)
            : this(fieldName) {
            this._errors.AddRange(errors);
        }

        public string FieldName { get { return _fieldName; } }

        public string[] Errors { get { return _errors.ToArray(); } }

        public override string ToString() {
            StringBuilder humanReadbleText = new StringBuilder(this.FieldName);
            for (int errorIndex = 0; errorIndex < _errors.Count; errorIndex++) {
                string error = _errors[errorIndex];
                humanReadbleText.AppendFormat(" {0} {1}", error.ToLower(), (errorIndex + 1) == _errors.Count ? String.Empty : "and");
            }
            return humanReadbleText.ToString().Trim(' ');
        }

        public void Add(string error) {
            _errors.Add(error);
        }
    }
}
