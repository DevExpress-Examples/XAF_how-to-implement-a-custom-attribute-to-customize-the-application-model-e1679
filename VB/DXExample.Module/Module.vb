Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic

Imports DevExpress.ExpressApp
Imports System.Reflection
Imports DevExpress.ExpressApp.DC
Imports DevExpress.ExpressApp.InfoGenerators
Imports DevExpress.ExpressApp.NodeWrappers
Imports DevExpress.ExpressApp.Model.Core
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Model.NodeGenerators
Imports System.ComponentModel


Namespace DXExample.Module
	Public Interface IRemovedFromViewInfo
		<DefaultValue(False)> _
		ReadOnly Property IsRemovedFromViewInfo() As Boolean
	End Interface

	Public Class BOModelNodesGeneratorUpdater
		Inherits ModelNodesGeneratorUpdater(Of ModelBOModelClassNodesGenerator)
		Public Overrides Sub UpdateNode(ByVal node As ModelNode)
			For Each modelClass As IModelClass In CType(node, IModelBOModel)
				For Each [property] As IModelMember In modelClass.AllMembers
					Dim attr As RemoveFromViewInfoAttribute = modelClass.TypeInfo.FindMember([property].Name).FindAttribute(Of RemoveFromViewInfoAttribute)()
					If attr IsNot Nothing Then
						[property].SetValue(Of Boolean)("IsRemovedFromViewInfo", attr.IsPropertyRemoved)
					End If
				Next [property]
			Next modelClass
		End Sub
	End Class
	Public Class ViewsNodesGeneratorUpdater
		Inherits ModelNodesGeneratorUpdater(Of ModelViewsNodesGenerator)
		Public Overrides Sub UpdateNode(ByVal node As ModelNode)
			For Each view As IModelView In CType(node, IModelViews)
				Dim propertiesToRemove As String = String.Empty
				For Each modelMember As DevExpress.ExpressApp.Model.IModelMember In view.ModelClass.AllMembers
					If (CType(modelMember, IRemovedFromViewInfo)).IsRemovedFromViewInfo Then
						propertiesToRemove = String.Concat(propertiesToRemove, modelMember.Name & ";")
					End If
				Next modelMember
				If (Not String.IsNullOrEmpty(propertiesToRemove)) Then
					If TypeOf view Is IModelDetailView Then
						For Each propertyName As String In propertiesToRemove.Split(New Char() { ";"c }, StringSplitOptions.RemoveEmptyEntries)
                            Dim items As IModelList(Of IModelDetailViewItem) = (CType(view, IModelDetailView)).Items
                            Dim viewItem As IModelDetailViewItem = items(propertyName)
							If viewItem IsNot Nothing Then
								CType(view, IModelDetailView).Items.Remove(viewItem)
							End If
						Next propertyName
                        Dim layoutModel As IModelDetailViewLayout = (CType(view, IModelDetailView)).Layout
                        Dim layoutItems As IModelList(Of IModelDetailViewLayoutElement) = layoutModel
                        layoutModel.Remove(layoutItems(0))
						CType(New ModelDetailViewLayoutNodesGenerator(), ModelDetailViewLayoutNodesGenerator).GenerateNodes(CType(layoutModel, ModelNode))
					End If
					If TypeOf view Is IModelListView Then
						For Each propertyName As String In propertiesToRemove.Split(New Char() { ";"c }, StringSplitOptions.RemoveEmptyEntries)
							Dim columns As IModelList(Of IModelColumn) = (CType(view, IModelListView)).Columns
							Dim column As IModelColumn = columns(propertyName)
							If column IsNot Nothing Then
								CType(view, IModelListView).Columns.Remove(column)
							End If
						Next propertyName
					End If
				End If
			Next view
		End Sub
	End Class

	Public NotInheritable Partial Class DXExampleModule
		Inherits ModuleBase
		Public Sub New()
			DevExpress.ExpressApp.Demos.InMemoryDataStoreProvider.Register()
			InitializeComponent()
		End Sub
		Public Overrides Sub ExtendModelInterfaces(ByVal extenders As DevExpress.ExpressApp.Model.ModelInterfaceExtenders)
			MyBase.ExtendModelInterfaces(extenders)
			extenders.Add(Of IModelMember, IRemovedFromViewInfo)()
		End Sub
		Public Overrides Sub AddGeneratorUpdaters(ByVal updaters As ModelNodesGeneratorUpdaters)
			MyBase.AddGeneratorUpdaters(updaters)
			updaters.Add(New BOModelNodesGeneratorUpdater())
			updaters.Add(New ViewsNodesGeneratorUpdater())
		End Sub

	End Class
End Namespace
