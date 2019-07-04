@Imports Sample_VB
@Imports DevExpress.Data
@Imports Sample_VB.Controllers

@Html.DevExpress().GridView(
    Sub(settings)
        settings.Name = "grid"
        settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "GridViewPartial"}

        settings.Columns.Add("ProductID")
        settings.Columns.Add("ProductName")
        settings.Columns.Add(Sub(column)
            column.FieldName = "CategoryID"
            column.Caption = "Category"
            column.EditorProperties().ComboBox(Sub(p)
                p.TextField = "CategoryName"
                p.ValueField = "CategoryID"
                p.ValueType = GetType(Integer)
                p.DataSource = NorthwindDataModel.GetCategories()
            End Sub)
            column.GroupIndex = 0
        End Sub)

        settings.Columns.Add("SupplierID").GroupIndex = 1
        settings.Columns.Add("QuantityPerUnit")
        settings.Columns.Add("UnitPrice")
        settings.Columns.Add("UnitsOnOrder")
        settings.Columns.Add("ReorderLevel")
        settings.Columns.Add("Discontinued", MVCxGridViewColumnType.CheckBox)

        settings.GroupSummary.Add(SummaryItemType.Min, "UnitPrice")
        settings.GroupSummary.Add(SummaryItemType.Max, "UnitPrice")
        settings.GroupSummary.Add(SummaryItemType.Max, "UnitsOnOrder")
        settings.GroupSummary.Add(SummaryItemType.Count, "ProductID")

        settings.Width = Unit.Percentage(100)
        settings.Settings.ShowGroupPanel = True
        settings.ClientSideEvents.Init = "Grid_Init"
        settings.ClientSideEvents.EndCallback = "Grid_EndCallback"
        settings.ClientSideEvents.ColumnResized = "Grid_ColumnResized"

        settings.Styles.Header.CssClass = "gridHeader"
        settings.Styles.GroupRow.CssClass = "gridGroupRow"

        If ViewData("resizing") IsNot Nothing Then
            If Convert.ToBoolean(ViewData("resizing")) = True Then
                settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.NextColumn
            Else
                settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Disabled
            End If
        End If

        settings.SetGroupRowContentTemplateContent(Sub(c)
            GridGroupRowContentTemplate.Container = c
            ViewContext.Writer.Write(GridGroupRowContentTemplate.GetTemplateHtml())
        End Sub)

        settings.Init = Sub(sender, e) TryCast(sender, MVCxGridView).VisibleColumns(0).HeaderStyle.CssClass = "gridVisibleColumn"

        settings.PreRender = Sub(sender, e)
            Dim grid As MVCxGridView = TryCast(sender, MVCxGridView)
            grid.ExpandRow(0)
            grid.ExpandRow(1)
        End Sub
End Sub).Bind(NorthwindDataModel.GetProducts()).GetHtml()
