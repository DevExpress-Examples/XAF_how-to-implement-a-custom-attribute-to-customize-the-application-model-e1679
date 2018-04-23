Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.Xpo.DB
Imports System.Data
Imports DevExpress.Xpo.DB.Helpers

Namespace DXExample.Module
	Public Class CodeCentralExampleInMemoryDataStoreProvider
		Public Const XpoProviderTypeString As String = "CodeCentralExampleInMemoryDataSet"
		Public Const ConnectionString As String = "XpoProvider=CodeCentralExampleInMemoryDataSet"
		Private Shared dataSet As DataSet

		Shared Sub New()
			Try
				dataSet = New DataSet()
				DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, New DataStoreCreationFromStringDelegate(AddressOf CreateProviderFromString))
			Catch
				Throw New Exception(String.Format("Cannot register the {0}", GetType(CodeCentralExampleInMemoryDataStoreProvider).Name))
			End Try
		End Sub
		Public Shared Function CreateProviderFromString(ByVal connectionString As String, ByVal autoCreateOption As AutoCreateOption, <System.Runtime.InteropServices.Out()> ByRef objectsToDisposeOnDisconnect() As IDisposable) As IDataStore
			Dim result As New InMemoryDataStore(dataSet, autoCreateOption)
			objectsToDisposeOnDisconnect = New IDisposable() { }
			Return result
		End Function
		Public Shared Sub Register()
		End Sub
	End Class

End Namespace
