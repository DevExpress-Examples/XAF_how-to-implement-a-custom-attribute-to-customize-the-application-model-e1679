using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using System.Reflection;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.InfoGenerators;
using DevExpress.ExpressApp.NodeWrappers;


namespace DXExample.Module {
    public sealed partial class DXExampleModule : ModuleBase {
        public DXExampleModule() {
            InitializeComponent();
        }
        public override Schema GetSchema() {
            const string newSchema =
            @"<Element Name=""Application"">
                <Element Name=""BOModel"">
                    <Element Name=""Class"">
                        <Element Name=""Member"">
                            <Attribute Name=""IsRemovedFromViewInfo"" Choice=""True,False"" IsReadOnly=""True""/>
                        </Element>
                    </Element>
                </Element>
            </Element>";
            return new Schema(new DictionaryXmlReader().ReadFromString(newSchema));
        }
        public override void UpdateModel(Dictionary model) {
            base.UpdateModel(model);
            ApplicationNodeWrapper applicationNodeWrapper = new ApplicationNodeWrapper(model);
            foreach (ClassInfoNodeWrapper classInfoWrapper in applicationNodeWrapper.BOModel.Classes) {
                string propertiesToRemove = SetIsRemovedFromViewInfoAttribute(classInfoWrapper);
                RemovePropertiesFromViews(propertiesToRemove, classInfoWrapper, applicationNodeWrapper);
            }
        }
        private string SetIsRemovedFromViewInfoAttribute(ClassInfoNodeWrapper classInfoWrapper) {
            string propertiesToRemove = String.Empty;
            foreach (PropertyInfoNodeWrapper property in classInfoWrapper.Properties) {
                RemoveFromViewInfoAttribute attr = classInfoWrapper.ClassTypeInfo.FindMember(property.Name).FindAttribute<RemoveFromViewInfoAttribute>();
                if (attr != null) {
                    property.Node.SetAttribute("IsRemovedFromViewInfo", attr.IsPropertyRemoved);
                    if (attr.IsPropertyRemoved) {
                        propertiesToRemove = String.Concat(propertiesToRemove, property.Name + ";");
                    }
                }  else {
                    property.Node.SetAttribute("IsRemovedFromViewInfo", false);
                }
            }
            return propertiesToRemove;
        }
        private void RemovePropertiesFromViews(string propertiesToRemove, ClassInfoNodeWrapper classInfoWrapper, ApplicationNodeWrapper applicationNodeWrapper) {
            if (!String.IsNullOrEmpty(propertiesToRemove)) {
                foreach (DetailViewInfoNodeWrapper detailView in applicationNodeWrapper.Views.GetDetailViews(classInfoWrapper)) {
                    foreach (string propertyName in propertiesToRemove.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) {
                        detailView.Editors.RemoveEditor(propertyName);
                    }
                    DictionaryNode layoutNode = detailView.Node.FindChildNode("Layout");
                    layoutNode.RemoveChildNodes();
                    layoutNode.AddChildNode(DetailViewLayoutNodeGenerator.Generate(classInfoWrapper, detailView.Editors));
                }
                foreach (ListViewInfoNodeWrapper listView in applicationNodeWrapper.Views.GetListViews(classInfoWrapper)) {
                    foreach (string propertyName in propertiesToRemove.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) {
                        ColumnInfoNodeWrapper column = listView.Columns.FindColumnInfo(propertyName);
                        if (column != null) {
                            listView.Columns.Node.RemoveChildNode(column.Node);
                        }
                    }
                }
            }
        }
    }
}
