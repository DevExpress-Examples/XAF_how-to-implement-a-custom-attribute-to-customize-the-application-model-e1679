using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using System.Reflection;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.InfoGenerators;
using DevExpress.ExpressApp.NodeWrappers;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using System.ComponentModel;


namespace DXExample.Module {
	public interface IRemovedFromViewInfo {
		[DefaultValue(false)]
		bool IsRemovedFromViewInfo { get; }
	}

	public class BOModelNodesGeneratorUpdater : ModelNodesGeneratorUpdater<ModelBOModelClassNodesGenerator> {
		public override void UpdateNode(ModelNode node) {
			foreach(IModelClass modelClass in (IModelBOModel)node) {
				foreach(IModelMember property in modelClass.AllMembers) {
					RemoveFromViewInfoAttribute attr = modelClass.TypeInfo.FindMember(property.Name).FindAttribute<RemoveFromViewInfoAttribute>();
					if(attr != null) {
						property.SetValue<bool>("IsRemovedFromViewInfo", attr.IsPropertyRemoved);
					}
				}
			}
		}
	}
	public class ViewsNodesGeneratorUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator> {
		public override void UpdateNode(ModelNode node) {
			foreach(IModelView view in (IModelViews)node) {
				string propertiesToRemove = String.Empty;
                IModelObjectView objectView = view as IModelObjectView;
                if(objectView != null) {
                    foreach(DevExpress.ExpressApp.Model.IModelMember modelMember in objectView.ModelClass.AllMembers) {
                        if(((IRemovedFromViewInfo)modelMember).IsRemovedFromViewInfo) {
                            propertiesToRemove = String.Concat(propertiesToRemove, modelMember.Name + ";");
                        }
                    }
                }
				if(!string.IsNullOrEmpty(propertiesToRemove)) {
					if(view is IModelDetailView) {
						foreach(string propertyName in propertiesToRemove.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) {
							IModelViewItem viewItem = ((IModelDetailView)view).Items[propertyName];
							if(viewItem != null) {
								viewItem.Remove();
							}
						}
						IModelViewLayout layoutModel = ((IModelDetailView)view).Layout;
						layoutModel[0].Remove();
						new ModelDetailViewLayoutNodesGenerator().GenerateNodes((ModelNode)layoutModel);
					}
					if(view is IModelListView) {
						foreach(string propertyName in propertiesToRemove.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) {
							IModelColumn column = ((IModelListView)view).Columns[propertyName];
							if(column != null) {
								column.Remove();
							}
						}
					}
				}
			}
		}
	}

	public sealed partial class DXExampleModule : ModuleBase {
        public DXExampleModule() {
			DevExpress.ExpressApp.Demos.InMemoryDataStoreProvider.Register();
            InitializeComponent();
        }
		public override void ExtendModelInterfaces(DevExpress.ExpressApp.Model.ModelInterfaceExtenders extenders) {
			base.ExtendModelInterfaces(extenders);
			extenders.Add<IModelMember, IRemovedFromViewInfo>();
		}
		public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters) {
			base.AddGeneratorUpdaters(updaters);
			updaters.Add(new BOModelNodesGeneratorUpdater());
			updaters.Add(new ViewsNodesGeneratorUpdater());
		}

    }
}
