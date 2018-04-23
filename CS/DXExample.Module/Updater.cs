using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace DXExample.Module {
    public class Updater : ModuleUpdater {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            DomainObject1 testDO1 = Session.FindObject<DomainObject1>(new BinaryOperator("Name", "Test"));
            if (testDO1 == null) {
                testDO1 = new DomainObject1(Session);
                testDO1.Name = "Test";
                testDO1.Save();
            }
        }
    }
}
