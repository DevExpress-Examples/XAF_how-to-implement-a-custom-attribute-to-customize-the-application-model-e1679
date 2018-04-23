Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Model.Core
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Model.NodeGenerators
Imports System.ComponentModel
Imports System.Collections
Imports DevExpress.ExpressApp.DC


Namespace DXExample.Module
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
			updaters.Add(New ViewsNodesGeneratorUpdater())
		End Sub
		Public Overrides Sub CustomizeLogics(ByVal customLogics As CustomLogics)
			MyBase.CustomizeLogics(customLogics)
			customLogics.RegisterLogic(GetType(IRemovedFromViewInfo), GetType(RemovedFromViewInfoLogic))
		End Sub
	End Class
	<DomainLogic(GetType(RemovedFromViewInfoLogic))> _
	Public Interface IRemovedFromViewInfo
		ReadOnly Property IsRemovedFromViewInfo() As Boolean
	End Interface
	Public Class RemovedFromViewInfoLogic
		Public Shared Function Get_IsRemovedFromViewInfo(ByVal instance As IRemovedFromViewInfo) As Boolean
			Dim attr As RemoveFromViewInfoAttribute = (CType(instance, IModelMember)).MemberInfo.FindAttribute(Of RemoveFromViewInfoAttribute)()
			If attr IsNot Nothing Then
				Return attr.IsPropertyRemoved
			Else
				Return False
			End If
		End Function
	End Class
	Public Class ViewsNodesGeneratorUpdater
		Inherits ModelNodesGeneratorUpdater(Of ModelViewsNodesGenerator)
		Public Overrides Sub UpdateNode(ByVal node As ModelNode)
			For Each view As IModelView In CType(node, IModelViews)
				Dim itemsToRemove As New ArrayList()
				If TypeOf view Is IModelDetailView Then
					For Each item As IModelViewItem In (CType(view, IModelDetailView)).Items
						If TypeOf item Is IModelMemberViewItem Then
							Dim member As IRemovedFromViewInfo = TryCast((CType(item, IModelMemberViewItem)).ModelMember, IRemovedFromViewInfo)
							If member IsNot Nothing AndAlso member.IsRemovedFromViewInfo Then
								itemsToRemove.Add(item)
							End If
						End If
					Next item
				End If
				If TypeOf view Is IModelListView Then
					For Each column As IModelColumn In (CType(view, IModelListView)).Columns
						Dim member As IRemovedFromViewInfo = TryCast(column.ModelMember, IRemovedFromViewInfo)
						If member IsNot Nothing AndAlso member.IsRemovedFromViewInfo Then
							itemsToRemove.Add(column)
						End If
					Next column
				End If
				For Each item As IModelNode In itemsToRemove
					item.Remove()
				Next item
				If TypeOf view Is IModelDetailView AndAlso itemsToRemove.Count > 0 Then
					Dim layoutModel As IModelViewLayout = (CType(view, IModelDetailView)).Layout
                    layoutModel.Item(0).Remove()
					CType(New ModelDetailViewLayoutNodesGenerator(), ModelDetailViewLayoutNodesGenerator).GenerateNodes(CType(layoutModel, ModelNode))
				End If
			Next view
		End Sub
	End Class
End Namespace
