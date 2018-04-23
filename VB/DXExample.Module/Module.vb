Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic

Imports DevExpress.ExpressApp
Imports System.Reflection
Imports DevExpress.ExpressApp.DC
Imports DevExpress.ExpressApp.InfoGenerators
Imports DevExpress.ExpressApp.NodeWrappers


Namespace DXExample.Module
	Public NotInheritable Partial Class DXExampleModule
		Inherits ModuleBase
		Public Sub New()
			InitializeComponent()
		End Sub
		Public Overrides Function GetSchema() As Schema
			Const newSchema As String = "<Element Name=""Application"">" & ControlChars.CrLf & "                <Element Name=""BOModel"">" & ControlChars.CrLf & "                    <Element Name=""Class"">" & ControlChars.CrLf & "                        <Element Name=""Member"">" & ControlChars.CrLf & "                            <Attribute Name=""IsRemovedFromViewInfo"" Choice=""True,False"" IsReadOnly=""True""/>" & ControlChars.CrLf & "                        </Element>" & ControlChars.CrLf & "                    </Element>" & ControlChars.CrLf & "                </Element>" & ControlChars.CrLf & "            </Element>"
			Return New Schema(New DictionaryXmlReader().ReadFromString(newSchema))
		End Function
		Public Overrides Sub UpdateModel(ByVal model As Dictionary)
			MyBase.UpdateModel(model)
			Dim applicationNodeWrapper As New ApplicationNodeWrapper(model)
			For Each classInfoWrapper As ClassInfoNodeWrapper In applicationNodeWrapper.BOModel.Classes
				Dim propertiesToRemove As String = SetIsRemovedFromViewInfoAttribute(classInfoWrapper)
				RemovePropertiesFromViews(propertiesToRemove, classInfoWrapper, applicationNodeWrapper)
			Next classInfoWrapper
		End Sub
		Private Function SetIsRemovedFromViewInfoAttribute(ByVal classInfoWrapper As ClassInfoNodeWrapper) As String
			Dim propertiesToRemove As String = String.Empty
			For Each [property] As PropertyInfoNodeWrapper In classInfoWrapper.Properties
				Dim attr As RemoveFromViewInfoAttribute = classInfoWrapper.ClassTypeInfo.FindMember([property].Name).FindAttribute(Of RemoveFromViewInfoAttribute)()
				If attr IsNot Nothing Then
					[property].Node.SetAttribute("IsRemovedFromViewInfo", attr.IsPropertyRemoved)
					If attr.IsPropertyRemoved Then
						propertiesToRemove = String.Concat(propertiesToRemove, [property].Name & ";")
					End If
				Else
					[property].Node.SetAttribute("IsRemovedFromViewInfo", False)
				End If
			Next [property]
			Return propertiesToRemove
		End Function
		Private Sub RemovePropertiesFromViews(ByVal propertiesToRemove As String, ByVal classInfoWrapper As ClassInfoNodeWrapper, ByVal applicationNodeWrapper As ApplicationNodeWrapper)
			If (Not String.IsNullOrEmpty(propertiesToRemove)) Then
				For Each detailView As DetailViewInfoNodeWrapper In applicationNodeWrapper.Views.GetDetailViews(classInfoWrapper)
					For Each propertyName As String In propertiesToRemove.Split(New Char() { ";"c }, StringSplitOptions.RemoveEmptyEntries)
						detailView.Editors.RemoveEditor(propertyName)
					Next propertyName
					Dim layoutNode As DictionaryNode = detailView.Node.FindChildNode("Layout")
					layoutNode.RemoveChildNodes()
					layoutNode.AddChildNode(DetailViewLayoutNodeGenerator.Generate(classInfoWrapper, detailView.Editors))
				Next detailView
				For Each listView As ListViewInfoNodeWrapper In applicationNodeWrapper.Views.GetListViews(classInfoWrapper)
					For Each propertyName As String In propertiesToRemove.Split(New Char() { ";"c }, StringSplitOptions.RemoveEmptyEntries)
						Dim column As ColumnInfoNodeWrapper = listView.Columns.FindColumnInfo(propertyName)
						If column IsNot Nothing Then
							listView.Columns.Node.RemoveChildNode(column.Node)
						End If
					Next propertyName
				Next listView
			End If
		End Sub
	End Class
End Namespace
