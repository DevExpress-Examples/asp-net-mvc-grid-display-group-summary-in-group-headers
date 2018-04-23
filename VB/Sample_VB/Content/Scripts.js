window.addEventListener("resize", function() { AdjustSummaryTable(); }, true);

function Grid_Init(s, e) {
    AdjustSummaryTable();
}

function Grid_EndCallback(s, e) {
    AdjustSummaryTable();
}

function Grid_ColumnResized(s, e) {
    AdjustSummaryTable();
}

function AdjustSummaryTable() {
    RemoveCustomStyleElement();

    var styleRules = [ ];
    var headerRow = GetGridHeaderRow(grid);
    var headerCells = headerRow.getElementsByClassName("gridHeader");
    var totalWidth = 0;
    for(var i = 0; i < headerCells.length; i++)
        styleRules.push(CreateStyleRule(headerCells[i], i));
    AppendStyleToHeader(styleRules);
}

function CreateStyleRule(headerCell, headerIndex) {
    var width = headerCell.offsetWidth;
    var cellRule = ".summaryCell_" + headerIndex;
    return cellRule + ", " + cellRule + " .summaryTextContainer" + "{ width:" + width + "px; }";
}

function GetGridHeaderRow(grid) {
    var headers = grid.GetMainElement().getElementsByClassName("gridVisibleColumn");
    if(headers.length > 0)
        return headers[0].parentNode;
}

var customStyleElement = null;
function AppendStyleToHeader(styleRules) {
    var container = document.createElement("DIV");
    container.innerHTML = "<style type='text/css'>" + styleRules.join("") + "</style>";
        
    var head = document.getElementsByTagName("HEAD")[0];
    customStyleElement = container.getElementsByTagName("STYLE")[0];
            
    head.appendChild(customStyleElement);
}
function RemoveCustomStyleElement() {
    if(customStyleElement) {
        customStyleElement.parentNode.removeChild(customStyleElement);
        customStyleElement = null;
    }
}