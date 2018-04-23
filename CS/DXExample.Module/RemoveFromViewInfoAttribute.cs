using System;
using System.Collections.Generic;
using System.Text;

namespace DXExample.Module {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RemoveFromViewInfoAttribute : Attribute {
        private bool _IsPropertyRemoved;
        public RemoveFromViewInfoAttribute(bool value) {
            _IsPropertyRemoved = value;
        }
        public RemoveFromViewInfoAttribute() {
            _IsPropertyRemoved = true;
        }
        public bool IsPropertyRemoved {
            get { return _IsPropertyRemoved; }
        }
    }
}
