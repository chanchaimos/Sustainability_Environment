<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="epi_transfertoptt_lst.aspx.cs" Inherits="epi_transfertoptt_lst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;Transfer to PTT</div>
        <div class="panel-body" id="divContent">
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-12 col-md-3 col-lg-2">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-xs-12 col-md-3 col-lg-3">
                                <asp:DropDownList ID="ddlGroupIndicator" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-xs-12 col-md-4 col-lg-2">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-xs-12 col-md-4 col-lg-3">
                                <asp:DropDownList ID="ddlFacility" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-xs-12 col-md-2 col-lg-2">
                                <button type="button" class="btn btn-info btn-block" onclick="SearchData()"><i class="fa fa-search"></i>&nbsp;Search</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <table id="tblData" class="table dataTable table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th class="dt-head-center dissort" style="width: 5%;">No.</th>
                                    <th class="dt-head-center sorting" style="width: 10%;">Year</th>
                                    <th class="dt-head-center sorting" style="width: 10%;">Quarter</th>
                                    <th class="dt-head-center sorting" style="width: 30%;">Facility</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Group Indicator</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Status</th>
                                    <th class="dt-head-center dissort" style="width: 5%;"></th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="cSet1">
                        <div class="cSet2">
                            <div class="dataTables_length">
                                <label>
                                    List
                                    <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                    Items
                                </label>
                            </div>
                        </div>
                        <div class="cSet2">
                            <div class="dataTables_info">
                            </div>
                        </div>
                        <div class="cSet2">
                            <div class="dataTables_paginate paging_simple_numbers">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        $(function () {
            SetEventTableOnDocReady("divContent", "tblData", "ddlPageSize", function (sIndexCol, sOrderBy, sPageIndex, sMode) { LoadData(sIndexCol, sOrderBy, sPageIndex, sMode); });
            LoadData("", "", "", GridEvent.BIND, true);
        });

        function SearchData() {
            LoadData("", "", "", GridEvent.BIND, false);
        }

        function LoadData(sIndexCol, sOrderBy, sPageIndex, sMode, IsPageLoad) {
            LoaddinProcess();
            var divContiner = "divContent";
            var sTableID = "tblData";

            //alway load data
            if (sMode != GridEvent.sort) {
                var dataSort = GetDataColumSort(sTableID);
                sIndexCol = dataSort.colindex;
                sOrderBy = dataSort.orderby;
            }

            var Param = {
                sIndexCol: sIndexCol + "",
                sOrderBy: sOrderBy + "",
                sPageSize: GetValDropdown("ddlPageSize"),
                sPageIndex: sPageIndex,
                sMode: sMode,

                sFacID: GetValDropdown("ddlFacility"),
                sYear: GetValDropdown("ddlYear"),
                sIndicatorID: GetValDropdown("ddlGroupIndicator"),
                sStatus: GetValDropdown("ddlStatus"),
            };

            AjaxCallWebMethod("LoadData", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    if (!Boolean(IsPageLoad))
                        PopupLogin();
                }
                else {
                    $("table[id$=" + sTableID + "] tbody tr").remove();
                    if (response.d.lstData != null && response.d.lstData.length > 0) {
                        var htmlTD = '<tr><td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '</tr>';

                        $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                        var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                        $("table[id$=" + sTableID + "] tbody tr").remove();
                        var nStartDataIndex = parseInt(response.d.nStartItemIndex);

                        var prms = Input("hdfPrmsMenu").val();
                        $.each(response.d.lstData, function (indx, item) {
                            $("td", row).eq(0).html(nStartDataIndex + '.');
                            $("td", row).eq(1).html(item.nYear);
                            $("td", row).eq(2).html(item.nQuarter);
                            $("td", row).eq(3).html(item.sGCFacName + '&nbsp;<i class="text-info glyphicon glyphicon-info-sign" title="' + item.sPTTFacilityName + '" style="font-size: 18px;"></i>');
                            $("td", row).eq(4).html(item.sIndicatorName);
                            $("td", row).eq(5).html(item.sStatusName + '&nbsp;<button type="button" class="btn btn-info btn-sm" title="View History" onclick="ViewHistory(' + item.nGCFacID + ',' + item.nYear + ',' + item.nIndicatorID + ',' + item.nQuarter + ')"><i class="fas fa-comment-alt"></i></button>');
                            if (item.sWarningPTTCode == "N") {
                                if (item.sLink != "")
                                    $("td", row).eq(6).html('<a href="' + item.sLink + '" class="' + item.sBtnClass + '"><i class="fa fa-search"></i>&nbsp;' + item.sBtnText + '</a>');
                                else
                                    $("td", row).eq(6).html('');
                            }
                            else {
                                $("td", row).eq(6).html('<i class="text-warning glyphicon glyphicon-warning-sign" title="Not Mapping PTT Code" style="font-size: 20px;"></i>');
                            }
                            $("table[id$=" + sTableID + "] tbody").append(row);
                            row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                            nStartDataIndex++;
                        });

                        SetHoverRowColor(sTableID);
                    }
                    else {
                        SetRowNoData(sTableID, 7);
                    }
                    SetTootip();
                    SetEvenTableAfterBind(response, divContiner, function (sIndexCol, sOrderBy, activeIndex, pageindex) { LoadData(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex); }, sTableID, "", "");
                    HideLoadding();
                }

            }, "", { itemSearch: Param });
        }

        function ViewHistory(nfacid, nyear, nindid, nquarter) {
            AjaxCallWebMethod("ViewHistory", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                } else if (response.d.Status == SysProcess.Success) {
                    var html = '';
                    if (response.d.lstData != null && response.d.lstData.length > 0) {
                        html = '<table class="table dataTable table-bordered table-hover">';
                        html += '<thead><tr><th class="dt-head-center dissort" style="width:15%">Date</th><th class="dt-head-center dissort" style="width:15%">Status</th><th class="dt-head-center dissort" style="width:20%">Action By</th><th class="dt-head-center dissort" style="width:50%">Comment</th></tr></thead>';
                        html += '<tbody>';
                        $.each(response.d.lstData, function (indx, item) {
                            html += '<tr>';
                            html += '<td class="dt-body-center">' + item.sDate + '</td>';
                            html += '<td class="dt-body-left">' + item.sStatus + '</td>';
                            html += '<td class="dt-body-left">' + item.sActionBy + '</td>';
                            html += '<td class="dt-body-left">' + item.sComment + '</td>';
                            html += '</tr>';
                        });
                        html += '</tbody>';
                        html += '</table>';
                    } else {
                        html = '<table class="table dataTable table-bordered table-hover">';
                        html += '<thead><tr><th class="dt-head-center dissort" style="width:15%">Date</th><th class="dt-head-center dissort" style="width:15%">Status</th><th class="dt-head-center dissort" style="width:20%">Action By</th><th class="dt-head-center dissort" style="width:50%">Comment</th></tr></thead>';
                        html += '<tbody>';
                        html += '<tr><td colspan="4" class="dt-body-center NoFix" style="background-color:#f6f6f6;vertical-align: middle;"><span class="text-red">No Data.</span></td></tr>'
                        html += '</tbody>';
                        html += '</table>';                        
                    }
                    DialogInfo("History", html);
                } else {
                    DialogWarning(DialogHeader.Warning, response.d.Msg);
                }
            }, "", { nFacID: nfacid, nYear: nyear, nIndID: nindid, nQuarter: nquarter });
        }
    </script>
</asp:Content>
