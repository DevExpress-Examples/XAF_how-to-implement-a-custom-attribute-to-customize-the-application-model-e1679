using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;

namespace DXExample.Module {
    public class Updater : ModuleUpdater {
        public Updater(ObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            DomainObject1 testDO1 = ObjectSpace.FindObject<DomainObject1>(new BinaryOperator("Name", "Test"));
            if (testDO1 == null) {
                testDO1 = ObjectSpace.CreateObject<DomainObject1>();
                testDO1.Name = "Test";
                testDO1.Save();
            }
        }
    }
}
