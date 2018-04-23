Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace DXExample.Module
	<AttributeUsage(AttributeTargets.Property, AllowMultiple := False)> _
	Public Class RemoveFromViewModelAttribute
		Inherits Attribute
		Private _IsPropertyRemoved As Boolean
		Public Sub New(ByVal value As Boolean)
			_IsPropertyRemoved = value
		End Sub
		Public Sub New()
			_IsPropertyRemoved = True
		End Sub
		Public ReadOnly Property IsPropertyRemoved() As Boolean
			Get
				Return _IsPropertyRemoved
			End Get
		End Property
	End Class
End Namespace
