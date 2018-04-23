using System;
using System.Collections.Generic;
using System.Text;

namespace DXExample.Module {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RemoveFromViewModelAttribute : Attribute {
        private bool _IsPropertyRemoved;
        public RemoveFromViewModelAttribute(bool value) {
            _IsPropertyRemoved = value;
        }
        public RemoveFromViewModelAttribute() {
            _IsPropertyRemoved = true;
        }
        public bool IsPropertyRemoved {
            get { return _IsPropertyRemoved; }
        }
    }
}
