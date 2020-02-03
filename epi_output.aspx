<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_EPI_FORMS_Output.master" AutoEventWireup="true" CodeFile="epi_output.aspx.cs" Inherits="epi_output" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        table.dataTable thead th.thOther {
            background-color: #fdb813 !important;
            color: #fff !important;
        }

        #divTable table {
            margin-top: -1px !important;
        }

        .cHeadIndicatorTable {
            vertical-align: middle;
            width: 350px;
        }

        .cHeadAllTable {
            vertical-align: middle;
            width: 120px;
        }

        .cOrange {
            background-color: #fabd4f !important;
        }

        .cGreen {
            background-color: #dbea97 !important;
        }

        .cExStyle {
            text-align: right;
            margin-bottom: 10px;
        }

        .table-responsive {
            max-height: 400px !important;
            overflow: auto !important;
            margin-bottom: 10px !important;
        }

        table thead th:not(:nth-child(1)) {
            z-index: 1 !important;
        }

        table thead th:nth-child(1) {
            z-index: 3 !important;
        }

        table tbody td:nth-child(1) {
            z-index: 2 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:HiddenField ID="hidYear" runat="server" />
    <asp:HiddenField ID="hidIndicator" runat="server" />
    <asp:HiddenField ID="hidOperationType" runat="server" />
    <asp:HiddenField ID="hidFacility" runat="server" />

    <div class="row" id="divEditFrom">
        <%-- Export --%>
        <div class="col-xs-12 cExStyle" id="divExport">
            <div class="form-group">
                <div class="col-xs-12">
                    <button type="button" onclick="ExportData();" class="btn btn-success">Export</button>
                    <asp:Button ID="btnEx" runat="server" CssClass="hidden" OnClick="btnEx_Click" />
                </div>
            </div>
        </div>
        <%-- Total Area --%>
        <div class="col-xs-12 cExStyle" id="divTotalArea">
            <div class="form-group">
                <div class="col-xs-12">
                    <label class="control-label col-xs-10 text-right">Total Area</label>
                    <span class="col-xs-2" id="spTotalArea" style="text-align: left;"></span>
                </div>
            </div>
        </div>
        <%-- TABLE --%>
        <div class="col-xs-12" id="divTable">
            <div class="form-group">
                <div class="col-xs-12">
                    <label id="lbForCombusion" class="headOther">Combusion</label>
                    <div class="table-responsive">
                        <table id="tbData" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                            <thead>
                                <tr>
                                    <th class="text-center cHeadIndicatorTable">Indicator</th>
                                    <th class="text-center cHeadAllTable">Unit</th>
                                    <th class="text-center cHeadAllTable">YTD</th>
                                    <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                    <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                    <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                    <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                    <th class="text-center cHeadAllTable QHead_2">May</th>
                                    <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                    <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                    <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                    <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                    <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                    <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                    <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="form-group" id="divTableSpill">
                <div class="col-xs-12">
                    <label class="headOther">Spill</label>
                    <div class="table-responsive">
                        <table id="tbDataSpill" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                            <thead>
                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">Spill</th>
                                </tr>--%>
                                <tr>
                                    <th class="text-center cHeadIndicatorTable">Indicator</th>
                                    <th class="text-center cHeadAllTable">Unit</th>
                                    <th class="text-center cHeadAllTable">YTD</th>
                                    <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                    <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                    <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                    <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                    <th class="text-center cHeadAllTable QHead_2">May</th>
                                    <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                    <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                    <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                    <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                    <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                    <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                    <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="form-group" id="divTableSignificantSpill">
                <div class="col-xs-12">
                    <label class="headOther">Significant Spill</label>
                    <div class="table-responsive">
                        <table id="tbDataSignificantSpill" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                            <thead>
                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">Significant Spill</th>
                                </tr>--%>
                                <tr>
                                    <th class="text-center cHeadIndicatorTable">Indicator</th>
                                    <th class="text-center cHeadAllTable">Unit</th>
                                    <th class="text-center cHeadAllTable">YTD</th>
                                    <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                    <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                    <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                    <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                    <th class="text-center cHeadAllTable QHead_2">May</th>
                                    <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                    <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                    <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                    <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                    <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                    <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                    <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="form-group" id="divNonCombustion">
                <div class="col-xs-12">
                    <label class="headOther">Non-Combustion</label>
                    <div class="table-responsive">
                        <table id="tbDataNonCombution" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                            <thead>
                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">Non-Combustion</th>
                                </tr>--%>
                                <tr>
                                    <th class="text-center cHeadIndicatorTable">Indicator</th>
                                    <th class="text-center cHeadAllTable">Unit</th>
                                    <th class="text-center cHeadAllTable">YTD</th>
                                    <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                    <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                    <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                    <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                    <th class="text-center cHeadAllTable QHead_2">May</th>
                                    <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                    <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                    <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                    <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                    <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                    <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                    <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="form-group" id="divCEM">
                <div class="col-xs-12">
                    <label class="headOther">CEM</label>
                    <div class="table-responsive">
                        <table id="tbDataCEM" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                            <thead>
                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">CEM</th>
                                </tr>--%>
                                <tr>
                                    <th class="text-center cHeadIndicatorTable">Indicator</th>
                                    <th class="text-center cHeadAllTable">Unit</th>
                                    <th class="text-center cHeadAllTable">YTD</th>
                                    <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                    <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                    <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                    <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                    <th class="text-center cHeadAllTable QHead_2">May</th>
                                    <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                    <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                    <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                    <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                    <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                    <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                    <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="form-group" id="divAdditionalCombustion">
                <div class="col-xs-12">
                    <label class="headOther">Additional Combustion</label>
                    <div class="table-responsive">
                        <table id="tbDataAdditionalCombustion" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                            <thead>
                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">Additional Combustion</th>
                                </tr>--%>
                                <tr>
                                    <th class="text-center cHeadIndicatorTable">Indicator</th>
                                    <th class="text-center cHeadAllTable">Unit</th>
                                    <th class="text-center cHeadAllTable">YTD</th>
                                    <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                    <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                    <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                    <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                    <th class="text-center cHeadAllTable QHead_2">May</th>
                                    <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                    <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                    <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                    <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                    <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                    <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                    <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="form-group" id="divAdditionalNonCombustion">
                <div class="col-xs-12">

                    <label class="headOther">Additional Non-Combustion</label>
                    <div class="table-responsive">
                        <table id="tbDataAdditionalNonCombustion" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                            <thead>
                                <%-- <tr>
                                    <th colspan="15" class="text-left thOther">Additional Non-Combustion</th>
                                </tr>--%>
                                <tr>
                                    <th class="text-center cHeadIndicatorTable">Indicator</th>
                                    <th class="text-center cHeadAllTable">Unit</th>
                                    <th class="text-center cHeadAllTable">YTD</th>
                                    <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                    <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                    <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                    <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                    <th class="text-center cHeadAllTable QHead_2">May</th>
                                    <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                    <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                    <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                    <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                    <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                    <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                    <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="form-group" id="divVOC">
                <div class="col-xs-12">

                    <label class="headOther">VOC</label>
                    <div class="table-responsive">
                        <table id="tbDataVOC" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                            <thead>
                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">VOC</th>
                                </tr>--%>
                                <tr>
                                    <th class="text-center cHeadIndicatorTable">Indicator</th>
                                    <th class="text-center cHeadAllTable">Unit</th>
                                    <th class="text-center cHeadAllTable">YTD</th>
                                    <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                    <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                    <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                    <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                    <th class="text-center cHeadAllTable QHead_2">May</th>
                                    <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                    <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                    <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                    <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                    <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                    <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                    <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidTotalArea" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var arrData = [];
        var arrDataNonCombustion = [];
        var arrDataCEM = [];
        var arrDataAdditionalCombustion = [];
        var arrDataAdditionalNonCombustion = [];
        var arrDataVOC = [];
        $(function () {
            $("#divTable").hide();
            $("#divExport").hide(); $("#divTotalArea").hide();
            $.each($("div[id$=divTable]").find("table"), function (i, el) {
                el = $(el);
                ArrInputFromTableID.push(el.attr("id"));
            })
        });

        function LoadData() {
            BlockUI();
            var item = {
                nIndicator: +GetValDropdown("ddlIndicator"),
                nOperationType: +GetValDropdown("ddlOperationType"),
                nFacility: +GetValDropdown("ddlFacility"),
                sYear: GetValDropdown("ddlYear"),
            };

            AjaxCallWebMethod("LoadData", function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                }
                else if (data.d.Status == SysProcess.Success) {
                    arrData = data.d.lstData;
                    arrDataNonCombustion = data.d.lstDataNonCombustion;
                    arrDataCEM = data.d.lstDataCEM;
                    arrDataAdditionalCombustion = data.d.lstDataAdditionalCombustion;
                    arrDataAdditionalNonCombustion = data.d.lstDataAdditionalNonCombustion;
                    arrDataVOC = data.d.lstDataVOC;
                    if ($ddlIndicator.val() == "4") {
                        $("label[id$=lbForCombusion]").show();
                        if ($ddlOperationType.val() != 13) {
                            BindTableOther(arrDataNonCombustion, "tbDataNonCombution");
                            BindTableOther(arrDataCEM, "tbDataCEM");
                            BindTableOther(arrDataAdditionalCombustion, "tbDataAdditionalCombustion");
                            BindTableOther(arrDataAdditionalNonCombustion, "tbDataAdditionalNonCombustion");
                            $("div[id$=divNonCombustion]").show();
                            $("div[id$=divCEM]").show();
                            $("div[id$=divAdditionalCombustion]").show();
                            $("div[id$=divAdditionalNonCombustion]").show();
                        } else {
                            $("div[id$=divNonCombustion]").hide();
                            $("div[id$=divCEM]").hide();
                            $("div[id$=divAdditionalCombustion]").hide();
                            $("div[id$=divAdditionalNonCombustion]").hide();
                        }
                        BindTableOther(arrDataVOC, "tbDataVOC");
                        $("div[id$=divVOC]").show();
                    } else {
                        $("label[id$=lbForCombusion]").hide();
                        $("div[id$=divNonCombustion]").hide();
                        $("div[id$=divCEM]").hide();
                        $("div[id$=divAdditionalCombustion]").hide();
                        $("div[id$=divAdditionalNonCombustion]").hide();
                        $("div[id$=divVOC]").hide();
                    }
                    BindTable();
                    CheckboxQuarterChanged();
                    $("#divTable").show();
                    if (arrData.length > 0) {
                        $("#divExport").show();
                    }
                }

            }, function () {
                $.each($("div[id$=divTable]").find("table"), function (i, el) {
                    el = $(el);
                    if (el.find("tbody tr").length == 1 && el.find("tbody tr td:eq(0)").has("colspan")) {
                        $("table#" + el.attr("id")).tableHeadFixer({ head: true });
                    } else {
                        $("table#" + el.attr("id")).tableHeadFixer({ "left": 1, head: true });
                    }
                })
                UnblockUI();
            }
           , { item: item });
        }

        function BindTable() {
            var sTableID = "tbData";
            if (arrData.length > 0) {
                var nOperationType = +GetValDropdown("ddlOperationType");
                var nIndicator = +GetValDropdown("ddlIndicator");
                $("table[id$=" + sTableID + "] tbody tr").remove();
                var htmlTD = "";
                if (nIndicator == 6 && nOperationType == 14) {
                    $("#divTotalArea").show();
                }
                else {
                    $("#divTotalArea").hide();
                }
                if (Enumerable.From(arrData).Where(function (w) { return w.IDIndicator == 9 }).ToArray().length > 0) {
                    BindTableOther(Enumerable.From(arrData).Where(function (w) { return w.sMakeField2 == "1" || w.IDIndicator == "0" }).ToArray(), "tbDataSpill");
                    BindTableOther(Enumerable.From(arrData).Where(function (w) { return w.sMakeField2 == "2" || w.IDIndicator == "0" }).ToArray(), "tbDataSignificantSpill");
                    $("#divTableSpill").show();
                    $("#divTableSignificantSpill").show();
                    arrData = Enumerable.From(arrData).Where(function (w) { return w.sMakeField2 == "0" }).ToArray();
                } else {
                    $("#divTableSpill").hide();
                    $("#divTableSignificantSpill").hide();
                }
                $.each(arrData, function (indx, item) {
                    if (item.IDIndicator == 6 && item.OperationtypeID == 14 && item.ProductID == 86 && item.sType == "TotalArea") {
                        $("#spTotalArea").html(item.sTotalArea);
                        $("input[id$=hidTotalArea]").val(item.sTotalArea);
                    }
                    else {
                        var sClass = item.sType == "Head" ? "cOrange" : item.sType == "Group" ? "cGreen" : item.sType == "Sub" ? item.nHeadID : "";
                        var btn = item.sType == "Head" ? '' + (item.isSub == true ? '<a id="a_' + item.ProductID + '" class="btn btn-default"'
                            + 'onclick="DetailSub(' + item.ProductID + ');"><i id="i_' + item.ProductID + '" class="fas fa-chevron-up"></i></a>&nbsp;' : '') + '' : '';

                        htmlTD += '<tr class="' + sClass + '">';
                        htmlTD += '<td class="dt-body-left">' + btn + item.ProductName + '</td>';
                        htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                        htmlTD += '<td class="dt-body-right">' + item.sTotal + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM1 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM2 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM3 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM4 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM5 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM6 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM7 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM8 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM9 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM10 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM11 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM12 + '</td>';
                        htmlTD += '</tr>';
                    }
                });

                $("table[id$=" + sTableID + "] tbody").append(htmlTD);

            }
            else {
                SetRowNoData(sTableID, 15);
            }
        }

        function DetailSub(sid) {
            if ($("." + sid + "").is(":visible")) {
                $("." + sid + "").hide();
                $("#i_" + sid + "").addClass("fa-chevron-down");
                $("#i_" + sid + "").removeClass("fa-chevron-up");
            }
            else {
                $("." + sid + "").show();
                $("#i_" + sid + "").removeClass("fa-chevron-down");
                $("#i_" + sid + "").addClass("fa-chevron-up");
            }
        }

        function BindTableOther(lstData, sTableID) {
            var htmlTD = "";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            if (lstData.length > 0) {
                $.each(lstData, function (indx, item) {
                    var sStyle = "";
                    var sProductName = "";
                    if (item.sType == "SUM" || item.UnitID == "2" || item.ProductID == "193" || item.nUnitID == "0") {
                        sProductName = item.ProductName;
                        sStyle = "style='background-color:#dbea97;'";
                    } else if (item.sType == "SUM2" || item.UnitID == "68") {
                        sProductName = "";
                        sStyle = "";
                    } else {
                        sProductName = item.ProductName;
                        sStyle = "";
                    }
                    htmlTD += '<tr ' + sStyle + '>';
                    htmlTD += '<td class="dt-body-left">' + sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    if ($ddlIndicator.val() == "4") {
                        htmlTD += '<td class="dt-body-right">' + item.nTotal + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.M1 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.M2 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.M3 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.M4 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.M5 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.M6 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.M7 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.M8 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.M9 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.M10 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.M11 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.M12 + '</td>';
                    } else {
                        htmlTD += '<td class="dt-body-right">' + item.sTotal + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM1 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM2 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM3 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM4 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM5 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM6 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM7 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM8 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM9 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM10 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM11 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM12 + '</td>';
                    }
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                SetRowNoData(sTableID, 15);
            }
        }

        /////////************ EXPORT *************\\\\\\\\\\\\
        function ExportData() {
            $("input[id$=hidYear]").val(GetValDropdown("ddlYear"));
            $("input[id$=hidIndicator]").val(GetValDropdown("ddlIndicator"));
            $("input[id$=hidOperationType]").val(GetValDropdown("ddlOperationType"));
            $("input[id$=hidFacility]").val(GetValDropdown("ddlFacility"));
            $("input[id$=btnEx]").click();
        }
    </script>
</asp:Content>

