using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DXExample.Module;

namespace DXExample.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            DXExampleWindowsFormsApplication winApplication = new DXExampleWindowsFormsApplication();
            CodeCentralExampleInMemoryDataStoreProvider.Register();
            winApplication.ConnectionString = CodeCentralExampleInMemoryDataStoreProvider.ConnectionString;
            try {
                winApplication.Setup();
                winApplication.Start();
            } catch (Exception e) {
                winApplication.HandleException(e);
            }
        }
    }
}
