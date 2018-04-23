using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp;

namespace DXExample.Win {
    public partial class DXExampleWindowsFormsApplication : WinApplication {
        public DXExampleWindowsFormsApplication() {
            InitializeComponent();
        }

        private void DXExampleWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
    }
}
