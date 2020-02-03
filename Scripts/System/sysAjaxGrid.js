
function SetEventPageIndex(divContiner) {
    $("li.paginate_button").on("click", function () {
        var indx = $(this).find("a").attr("data-dt-idx");
        var activeIndex = SetPageIndexActive(this, indx, divContiner);
        if (activeIndex != "" && activeIndex != null && activeIndex != undefined) {
            LoadData("", "", activeIndex, GridEvent.pageindex);
        }
    });
}

function SetEvetnPageSize(ddlPageSizeID, divContiner) {
    $("select[id$=" + ddlPageSizeID + "]").on("change", function () {
        LoadData("", "", GetPageIndex(divContiner), GridEvent.pagesize);
    });
}

function SetEventPageIndexFunc(divContiner, Func) {
    $("li.paginate_button").on("click", function () {
        var indx = $(this).find("a").attr("data-dt-idx");
        var activeIndex = SetPageIndexActive(this, indx, divContiner);
        var pageindex = GridEvent.pageindex;
        if (activeIndex != "" && activeIndex != null && activeIndex != undefined) {
            Func(activeIndex, pageindex);
        }
    });
}

function SetEvetnPageSizeFunc(ddlPageSizeID, divContiner, Func) {
    $("select[id$=" + ddlPageSizeID + "]").on("change", function () {
        var activeIndex = GetPageIndex(divContiner);
        var pageindex = GridEvent.pageindex;
        Func(activeIndex, pageindex);
    });
}

function SetPageIndexActive(objThis, indx, sContiner) {
    var now = $("div[id$=" + sContiner + "] div.dataTables_paginate").find("ul").find("li.active");
    var pageindex = "";
    if (indx === "-1") {//prev
        if (!$(objThis).hasClass("disabled")) {
            var prev = now.prev();
            now.removeClass("active");
            $(prev).addClass("active");
            if (prev.prev().is($("div[id$=" + sContiner + "] div.dataTables_paginate").find("ul").find("li").first())) {
                prev.prev().addClass("disabled");
            }

            //เปิด next  
            $("div[id$=" + sContiner + "] div.dataTables_paginate").find("ul").find("li.next").removeClass("disabled");

            pageindex = GetPageIndex(sContiner);
        }
    }
    else if (indx === "+1") {//next
        if (!$(objThis).hasClass("disabled")) {
            var next = now.next();
            $(now).removeClass("active");
            $(next).addClass("active");
            if (next.next().is($("div[id$=" + sContiner + "] div.dataTables_paginate").find("ul").find("li").last())) {
                next.next().addClass("disabled");
            }

            //เปิด prev  
            $("div[id$=" + sContiner + "] div.dataTables_paginate").find("ul").find("li.previous").removeClass("disabled");

            pageindex = GetPageIndex(sContiner);
        }
    }
    else {//Number
        var nowindex = GetPageIndex(sContiner);

        $(now).removeClass("active");
        $(objThis).addClass("active");
        var objPagenation = $("div[id$=" + sContiner + "] div.dataTables_paginate").find("ul").find("li")

        //ปุ่ม prev
        if (!$(objThis).prev().is(objPagenation.first())) {
            objPagenation.first().removeClass("disabled");//เปิด
        }
        else {
            objPagenation.first().addClass("disabled");//ปิด
        }

        //ปุ่ม next
        if (!$(objThis).next().is(objPagenation.last())) {
            objPagenation.last().removeClass("disabled");//เปิด
        }
        else {
            objPagenation.last().addClass("disabled");//ปิด
        }

        //not action on click if old index
        var newIndex = GetPageIndex(sContiner);
        pageindex = newIndex == nowindex ? "" : newIndex;
    }

    return pageindex;
}

function GetPageIndex(sContiner) {
    var index = $("div[id$=" + sContiner + "] div.dataTables_paginate").find("ul").find("li.active").find("a").attr("data-dt-idx");
    return index == undefined ? "" : index;
}

function BindPageIndex(divContiner, sHtml) {
    $("div[id$=" + divContiner + "] div.dataTables_paginate").html(sHtml);
}

function BindPageInfo(divContiner, sHtml) {
    $("div[id$=" + divContiner + "] div.dataTables_info").html(sHtml);
}

//funcOnSort >> ({ orderby: sorderby, colindex: colindex }), ex. call by >> function (data) { TEST(data); })
function SetSortData(sGridID, funcOnSort) {
    $("table[id$=" + sGridID + "] thead tr th.sorting").on("click", function (e) {
        $("table[id$=" + sGridID + "] thead tr th").removeClass("sorting sorting_asc sorting_desc");
        $("table[id$=" + sGridID + "] thead tr th").addClass("sorting");
        $("table[id$=" + sGridID + "] thead tr th.dissort").removeClass("sorting");
        $(this).removeClass("sorting");

        if ($(this).find("span").length == 0) {
            $(this).append('<span class="hidden">asc</span>');
            $(this).addClass("sorting_asc");
        }
        else {
            if ($(this).find("span").text() == "asc") {
                $(this).addClass("sorting_desc");
                $(this).find("span").text("desc");//order
            }
            else {
                $(this).addClass("sorting_asc");
                $(this).find("span").text("asc");//order
            }
        }

        var sorderby = $(this).find("span").text();
        var colindex = e.currentTarget.cellIndex;	 //e.toElement.cellIndex;
        var result = { orderby: sorderby, colindex: colindex };
        funcOnSort(result);
    });
}

//ge column index on sort
function GetDataColumSort(sGridID) {
    var result = { orderby: "", colindex: "" };
    var arrTH = $("table[id$=" + sGridID + "]").find("thead").find("th");
    for (var i = 0; i < arrTH.length; i++) {
        if ($(arrTH[i]).hasClass("sorting_desc") || $(arrTH[i]).hasClass("sorting_asc")) {
            var sorderby = $(arrTH[i]).find("span").text();
            var colindex = i;
            result = { orderby: sorderby, colindex: colindex };
        }
    }
    return result;
}

//colname = "c1,c2,c3,c4" 
//colwidht(%) = "10,20,30,40", heigthRowNodata
//ความสูงของแถวที่แสดงข้อความ(px)
function SetTableNoData(colname, colwidht, heigthRowNodata, isShowHead) {

    var arrCol = colname.split(',');
    var arrWidht = colwidht.split(',');
    var setrowheigth = heigthRowNodata != undefined && heigthRowNodata != "" ? "height: " + heigthRowNodata + "px;" : "";

    var data = '<table id="tblNoData" class="table table-bordered table-hover dataTable tblNoData">';
    if (Boolean(isShowHead)) {
        data += '<thead><tr>';
        for (var i = 0; i < arrCol.length; i++) {
            data += '<th class="dt-head-center" width="' + arrWidht[i] + '%">' + arrCol[i] + '</th>';
        }
        data += '</tr></thead>';
    }
    data += '<tbody><tr><td colspan="' + arrCol.length + '" class="dt-body-center" style="background-color:#f6f6f6;vertical-align: middle;' + setrowheigth + '"><span class="text-red">No Data.</span></td></tr></tbody>';
    data += '</table>';
    return data;
}

function GetRowNoData(nColSpan, heigthRowNodata) {
    var setrowheigth = heigthRowNodata != undefined && heigthRowNodata != "" ? "height: " + heigthRowNodata + "px;" : "";
    return '<tr><td colspan="' + nColSpan + '" class="dt-body-center" style="background-color:#f6f6f6;vertical-align: middle;' + setrowheigth + '"><span class="text-red">No Data.</span></td></tr>'
}

//Delete data in the table list
function DeleteData(sTableID, ckbRowID, txtID, funcDel) {
    CheckDeleteDataInGrid(sTableID, ckbRowID, function () {
        var arrCheckID = GetSelectDeleteRowInGrid(sTableID, txtID);
        if (arrCheckID.length > 0) {
            funcDel(arrCheckID);
        }
    }, "");
}

//ex. funcLoadData = function (sIndexCol, sOrderBy, sPageIndex, sMode){ LoadData(sIndexCol, sOrderBy, sPageIndex, sMode); }
function SetEventTableOnDocReady(divContainerID, TableID, ddlPageSizeID, funcLoadData) {
    //set sort
    SetSortData(TableID, function (data) { gvwDataSort(data); });
    //set page index
    SetEventPageIndexFunc(divContainerID, function (activeIndex, pageindex) { funcLoadData("", "", activeIndex, pageindex); });
    //set page size
    SetEvetnPageSizeFunc(ddlPageSizeID, divContainerID, function (activeIndex, pageindex) { "", "", funcLoadData("", "", activeIndex, GridEvent.pagesize); });

    //function for call load data on click sort in grid
    function gvwDataSort(data) {
        funcLoadData(data.colindex, data.orderby, GetPageIndex(divContainerID), GridEvent.sort);
    }
}

//ex. funcLoadData = function (activeIndex, pageindex) { LoadDataT1("", "", activeIndex, GridEvent.pageindex) },sTableID & ckbHeadID & ckbRowID > for event delete if none event not blank all
function SetEvenTableAfterBind(response, divContiner, funcLoadData, sTableID, ckbHeadID, ckbRowID) {
    //set pageinfo
    BindPageInfo(divContiner, response.d.sPageInfo)

    //set page index
    BindPageIndex(divContiner, response.d.sContentPageIndex);
    SetEventPageIndexFunc(divContiner, function (activeIndex, pageindex) { funcLoadData("", "", activeIndex, pageindex); });

    //set checkbox
    //if (sTableID != "" && ckbHeadID != "" && ckbRowID != "")
    //    SetCheckBoxSelectRowInGrid(sTableID, ckbHeadID, ckbRowID);
    if (sTableID != "" && ckbHeadID != "" && ckbRowID != "") {
        SetCheckBoxSelectRowInGrid(sTableID, ckbHeadID, ckbRowID);
        if ($("table[id$=" + sTableID + "] input[id$=" + ckbHeadID + "]").is(":checked")) {
            $("table[id$=" + sTableID + "] input[id$=" + ckbHeadID + "]").iCheck("uncheck");
            $("table[id$=" + sTableID + "] input[id$=" + ckbRowID + "]").iCheck("uncheck");
        }
    }
}

function SetHoverRowColor(sTableID) {
    $("table[id$=" + sTableID + "] tbody tr").addClass("cSetHoverTR");
}