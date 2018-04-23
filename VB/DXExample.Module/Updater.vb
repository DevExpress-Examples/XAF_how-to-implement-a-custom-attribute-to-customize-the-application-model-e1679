Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl

Namespace DXExample.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal session As Session, ByVal currentDBVersion As Version)
			MyBase.New(session, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Dim testDO1 As DomainObject1 = Session.FindObject(Of DomainObject1)(New BinaryOperator("Name", "Test"))
			If testDO1 Is Nothing Then
				testDO1 = New DomainObject1(Session)
				testDO1.Name = "Test"
				testDO1.Save()
			End If
		End Sub
	End Class
End Namespace
