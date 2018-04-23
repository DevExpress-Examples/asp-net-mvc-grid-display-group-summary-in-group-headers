Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.Linq
Imports System.Web

Public Class ConnectionRepository
    Public Shared Function GetDataConnection() As OleDbConnection
        Dim connection As New OleDbConnection()
        connection.ConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", HttpContext.Current.Server.MapPath("~/App_Data/Northwind.mdb"))
        Return connection
    End Function
End Class
Public Class NorthwindDataModel
    Public Shared Function GetProducts() As DataTable
        Dim dataTableProducts As New DataTable()
        Using connection As OleDbConnection = ConnectionRepository.GetDataConnection()
            Dim adapter As New OleDbDataAdapter(String.Empty, connection)
            adapter.SelectCommand.CommandText = "SELECT * FROM [Products]"
            adapter.Fill(dataTableProducts)
        End Using
        Return dataTableProducts
    End Function

    Public Shared Function GetCategories() As DataTable
        Dim dataTableCategories As New DataTable()
        Using connection As OleDbConnection = ConnectionRepository.GetDataConnection()
            Dim adapter As New OleDbDataAdapter(String.Empty, connection)
            adapter.SelectCommand.CommandText = "SELECT * FROM [Categories]"
            adapter.Fill(dataTableCategories)
        End Using
        Return dataTableCategories
    End Function
End Class