using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using System.ComponentModel;
using System.Collections;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;


namespace DXExample.Module {
    public sealed partial class DXExampleModule : ModuleBase {
        public DXExampleModule() {
            InMemoryDataStoreProvider.Register();
            InitializeComponent();
        }
        public override void ExtendModelInterfaces(DevExpress.ExpressApp.Model.ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelMember, IRemovedFromViewModel>();
        }
        public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters) {
            base.AddGeneratorUpdaters(updaters);
            updaters.Add(new ViewsNodesGeneratorUpdater());
        }
        public override void CustomizeLogics(CustomLogics customLogics) {
            base.CustomizeLogics(customLogics);
            customLogics.RegisterLogic(typeof(IRemovedFromViewModel), typeof(RemovedFromViewInfoLogic));
        }
    }
    [DomainLogic(typeof(RemovedFromViewInfoLogic))]
    public interface IRemovedFromViewModel {
        bool IsRemovedFromViewModel { get; }
    }
    public class RemovedFromViewInfoLogic {
        public static bool Get_IsRemovedFromViewModel(IRemovedFromViewModel instance) {
            RemoveFromViewModelAttribute attr = ((IModelMember)instance).MemberInfo.FindAttribute<RemoveFromViewModelAttribute>();
            if (attr != null) {
                return attr.IsPropertyRemoved;
            } else {
                return false;
            }
        }
    }
    public class ViewsNodesGeneratorUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator> {
        public override void UpdateNode(ModelNode node) {
            foreach (IModelView view in (IModelViews)node) {
                ArrayList itemsToRemove = new ArrayList();
                if (view is IModelDetailView) {
                    foreach (IModelViewItem item in ((IModelDetailView)view).Items) {
                        if (item is IModelMemberViewItem) {
                            IRemovedFromViewModel member = ((IModelMemberViewItem)item).ModelMember as IRemovedFromViewModel;
                            if (member != null && member.IsRemovedFromViewModel) {
                                itemsToRemove.Add(item);
                            }
                        }
                    }
                }
                if (view is IModelListView) {
                    foreach (IModelColumn column in ((IModelListView)view).Columns) {
                        IRemovedFromViewModel member = column.ModelMember as IRemovedFromViewModel;
                        if (member != null && member.IsRemovedFromViewModel) {
                            itemsToRemove.Add(column);
                        }
                    }
                }
                foreach (IModelNode item in itemsToRemove) {
                    item.Remove();
                }
                if (view is IModelDetailView && itemsToRemove.Count > 0) {
                    IModelViewLayout layoutModel = ((IModelDetailView)view).Layout;
                    layoutModel[0].Remove();
                    new ModelDetailViewLayoutNodesGenerator().GenerateNodes((ModelNode)layoutModel);
                }
            }
        }
    }
}
