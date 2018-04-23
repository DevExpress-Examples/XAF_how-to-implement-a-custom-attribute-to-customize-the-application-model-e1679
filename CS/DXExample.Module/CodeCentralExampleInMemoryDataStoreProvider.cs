using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Xpo.DB;
using System.Data;
using DevExpress.Xpo.DB.Helpers;

namespace DXExample.Module {
    public class CodeCentralExampleInMemoryDataStoreProvider {
        public const string XpoProviderTypeString = "CodeCentralExampleInMemoryDataSet";
        public const string ConnectionString = "XpoProvider=CodeCentralExampleInMemoryDataSet";
        private static DataSet dataSet;

        static CodeCentralExampleInMemoryDataStoreProvider() {
            try {
                dataSet = new DataSet();
                DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, new DataStoreCreationFromStringDelegate(CreateProviderFromString));
            } catch {
                throw new Exception(string.Format("Cannot register the {0}", typeof(CodeCentralExampleInMemoryDataStoreProvider).Name));
            }
        }
        public static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
            InMemoryDataStore result = new InMemoryDataStore(dataSet, autoCreateOption);
            objectsToDisposeOnDisconnect = new IDisposable[] { };
            return result;
        }
        public static void Register() { }
    }

}
