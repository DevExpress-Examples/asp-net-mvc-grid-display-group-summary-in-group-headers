Imports DevExpress.Web
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace Sample_VB.App_Code
    Public NotInheritable Class GridGroupRowContentTemplate

        Private Sub New()
        End Sub
        Private Const MainTableCssClassName As String = "summaryTable", SummaryTextContainerCssClassName As String = "summaryTextContainer", SummaryCellCssClassNameFormat As String = "summaryCell_{0}", GroupTextFormat As String = "{0}: {1}"

        Public Shared Property Container() As GridViewGroupRowTemplateContainer
        Private Shared ReadOnly Property Grid() As ASPxGridView
            Get
                Return Container.Grid
            End Get
        End Property

        Private Shared Property MainTable() As Table
        Private Shared Property GroupTextRow() As TableRow
        Private Shared Property SummaryTextRow() As TableRow

        Private Shared ReadOnly Property IndentCount() As Integer
            Get
                Return Grid.GroupCount - GroupLevel - 1
            End Get
        End Property
        Private Shared ReadOnly Property GroupLevel() As Integer
            Get
                Return Grid.DataBoundProxy.GetRowLevel(Container.VisibleIndex)
            End Get
        End Property
        Private Shared ReadOnly Property VisibleColumns() As List(Of GridViewColumn)
            Get
                Return Grid.VisibleColumns.Except(Grid.GetGroupedColumns()).ToList()
            End Get
        End Property

        Public Shared Sub CreateGroupRowTable()
            MainTable = New Table()

            GroupTextRow = CreateRow()
            SummaryTextRow = CreateRow()

            CreateGroupTextCell()
            CreateIndentCells()
            For Each column In VisibleColumns
                CreateSummaryTextCell(column)
            Next column

            ApplyStyles()
        End Sub

        Public Shared Function GetTemplateHtml() As String
            CreateGroupRowTable()
            Dim html As String = ""
            Using sw As New StringWriter()
                MainTable.RenderControl(New HtmlTextWriter(sw))
                html = sw.ToString()
            End Using
            Return html
        End Function

        Private Shared Sub CreateGroupTextCell()
            Dim cell = CreateCell(GroupTextRow)
            cell.Text = String.Format(GroupTextFormat, Container.Column, Container.GroupText)
            cell.ColumnSpan = VisibleColumns.Count + IndentCount
        End Sub

        Private Shared Sub CreateSummaryTextCell(ByVal column As GridViewColumn)
            If TypeOf column Is GridViewBandColumn Then Return
            Dim cell = CreateCell(SummaryTextRow)
            Dim dataColumn = TryCast(column, GridViewDataColumn)
            If dataColumn Is Nothing Then
                Return
            End If
            Dim summaryItems = FindSummaryItems(dataColumn)
            If summaryItems.Count = 0 Then
                Return
            End If

            Dim div = New WebControl(HtmlTextWriterTag.Div) With {.CssClass = SummaryTextContainerCssClassName}
            cell.Controls.Add(div)

            Dim text = GetGroupSummaryText(summaryItems, column)
            div.Controls.Add(New LiteralControl(text))
        End Sub

        Private Shared Function GetGroupSummaryText(ByVal items As List(Of ASPxSummaryItem), ByVal column As GridViewColumn) As String
            Dim sb = New StringBuilder()
            For i = 0 To items.Count - 1
                If i > 0 Then
                    sb.Append("<br />")
                End If
                Dim item = items(i)
                Dim summaryValue = Grid.GetGroupSummaryValue(Container.VisibleIndex, item)
                sb.Append(item.GetGroupRowDisplayText(column, summaryValue))
            Next i
            Return sb.ToString()
        End Function

        Private Shared Sub ApplyStyles()
            MainTable.CssClass = MainTableCssClassName
            Dim startIndex = GroupLevel + 1
            For i = 0 To SummaryTextRow.Cells.Count - 1
                SummaryTextRow.Cells(i).CssClass = String.Format(SummaryCellCssClassNameFormat, i + startIndex)
            Next i
        End Sub

        Private Shared Sub CreateIndentCells()
            For i = 0 To IndentCount - 1
                CreateCell(SummaryTextRow)
            Next i
        End Sub
        Private Shared Function FindSummaryItems(ByVal column As GridViewDataColumn) As List(Of ASPxSummaryItem)
            Return Grid.GroupSummary.Where(Function(i) i.FieldName = column.FieldName).ToList()
        End Function
        Private Shared Function CreateRow() As TableRow
            Dim row = New TableRow()
            MainTable.Rows.Add(row)
            Return row
        End Function
        Private Shared Function CreateCell(ByVal row As TableRow) As TableCell
            Dim cell = New TableCell()
            row.Cells.Add(cell)
            Return cell
        End Function
    End Class
End Namespace