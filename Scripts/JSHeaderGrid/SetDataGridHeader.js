function MoveFirstRowToHeaderTag(tableID) {
    var table = document.getElementById(tableID);
    if (table != null) {
        var head = document.createElement("thead"); // TABLE HEAD : <thead></thead>
        head.appendChild(table.rows[0]);
        table.insertBefore(head, table.childNodes[0]);
    }
}

function SetFirstRowTDtoTH(tableID) {
    $("table[id$='" + tableID + "'] tr:first-child td").each(function () {
        var thisTD = this;
        var newElement = $("<th></th>");
        $.each(this.attributes, function (index) {
            $(newElement).attr(thisTD.attributes[index].name, thisTD.attributes[index].value);
        });
        $(newElement).html($(this).html());
        $(this).after(newElement).remove();
    });
}

function SetSortingFunction(tableID) {
    $("table[id$='" + tableID + "'] tr:first-child th").each(function () {
        var tag_a = $(this).find("a");
        if (tag_a != null) {
            $(this).attr("onclick", $(tag_a).attr("href"));
            $(this).html($(tag_a).html());
        }
    });
}

function SetDataGridHeader(dgdID) {
    SetFirstRowTDtoTH(dgdID);
    MoveFirstRowToHeaderTag($("table[id$='" + dgdID + "']").attr("id"));
    //SetSortingFunction(dgdID);
}