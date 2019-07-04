using Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sample.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            return View();
        }
        public ActionResult GridViewPartial(bool? resizing) {
            ViewData["resizing"] = resizing;
            return PartialView();
        }
    }

    public static class GridGroupRowContentTemplate {
        const string
        MainTableCssClassName = "summaryTable",
        SummaryTextContainerCssClassName = "summaryTextContainer",
        SummaryCellCssClassNameFormat = "summaryCell_{0}",
        GroupTextFormat = "{0}: {1}";

        public static GridViewGroupRowTemplateContainer Container { get; set; }
        private static ASPxGridView Grid { get { return Container.Grid; } }

        private static Table MainTable { get; set; }
        private static TableRow GroupTextRow { get; set; }
        private static TableRow SummaryTextRow { get; set; }

        private static int IndentCount { get { return Grid.GroupCount - GroupLevel - 1; } }
        private static int GroupLevel { get { return Grid.DataBoundProxy.GetRowLevel(Container.VisibleIndex); } }
        private static List<GridViewColumn> VisibleColumns { get { return Grid.VisibleColumns.Except(Grid.GetGroupedColumns()).ToList(); } }

        public static void CreateGroupRowTable() {
            MainTable = new Table();

            GroupTextRow = CreateRow();
            SummaryTextRow = CreateRow();

            CreateGroupTextCell();
            CreateIndentCells();
            foreach (var column in VisibleColumns)
                CreateSummaryTextCell(column);

            ApplyStyles();
        }

        public static string GetTemplateHtml() {
            CreateGroupRowTable();
            string html = "";
            using (StringWriter sw = new StringWriter()) {
                MainTable.RenderControl(new HtmlTextWriter(sw));
                html = sw.ToString();
            }
            return html;
        }

        private static void CreateGroupTextCell() {
            var cell = CreateCell(GroupTextRow);
            cell.Text = string.Format(GroupTextFormat, Container.Column, Container.GroupText);
            cell.ColumnSpan = VisibleColumns.Count + IndentCount;
        }

        private static void CreateSummaryTextCell(GridViewColumn column) {
            if (column is GridViewBandColumn)
                return;
            var cell = CreateCell(SummaryTextRow);
            var dataColumn = column as GridViewDataColumn;
            if (dataColumn == null)
                return;
            var summaryItems = FindSummaryItems(dataColumn);
            if (summaryItems.Count == 0)
                return;

            var div = new WebControl(HtmlTextWriterTag.Div) { CssClass = SummaryTextContainerCssClassName };
            cell.Controls.Add(div);

            var text = GetGroupSummaryText(summaryItems, column);
            div.Controls.Add(new LiteralControl(text));
        }

        private static string GetGroupSummaryText(List<ASPxSummaryItem> items, GridViewColumn column) {
            var sb = new StringBuilder();
            for (var i = 0; i < items.Count; i++) {
                if (i > 0) sb.Append("<br />");
                var item = items[i];
                var summaryValue = Grid.GetGroupSummaryValue(Container.VisibleIndex, item);
                sb.Append(item.GetGroupRowDisplayText(column, summaryValue));
            }
            return sb.ToString();
        }

        private static void ApplyStyles() {
            MainTable.CssClass = MainTableCssClassName;
            var startIndex = GroupLevel + 1;
            for (var i = 0; i < SummaryTextRow.Cells.Count; i++)
                SummaryTextRow.Cells[i].CssClass = string.Format(SummaryCellCssClassNameFormat, i + startIndex);
        }

        private static void CreateIndentCells() {
            for (var i = 0; i < IndentCount; i++)
                CreateCell(SummaryTextRow);
        }
        private static List<ASPxSummaryItem> FindSummaryItems(GridViewDataColumn column) {
            return Grid.GroupSummary.Where(i => i.FieldName == column.FieldName).ToList();
        }
        private static TableRow CreateRow() {
            var row = new TableRow();
            MainTable.Rows.Add(row);
            return row;
        }
        private static TableCell CreateCell(TableRow row) {
            var cell = new TableCell();
            row.Cells.Add(cell);
            return cell;
        }
    }
}