<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="epi_monitoring.aspx.cs" Inherits="epi_monitoring" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .cHeadIndicatorTable {
            vertical-align: middle;
            width: 120px;
        }

        .cHeadAllTable {
            vertical-align: middle;
            width: 50px;
        }

        .btn-circle {
            display: inline-block;
            /*background-color: #666;*/
            color: white;
            width: 30px;
            line-height: 30px;
            border-radius: 50%;
        }

        .modal-backdrop {
            display: none !important;
        }

        .cModal {
            background-color: #4eb3f0;
            color: white;
        }

        .cExStyle {
            text-align: right;
            margin-bottom: 10px;
        }

        .cbtnShow {
            padding-left: 8px;
        }

        .cbtnDefalt {
            background-color: #ccc;
        }

        .cPointer {
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;EPI MONITORING</div>
        <div class="panel-body" id="divContent">
            <div class="col-xs-12">
                <div class="form-group col-xs-12 col-md-3">
                    <%--<asp:DropDownList ID="ddlIndicator" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                    <select class="form-control" id="ddlIndicator"></select>
                </div>
                <div class="form-group col-xs-12 col-md-3 ">
                    <%-- <asp:DropDownList ID="ddlOperationType" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                    <select class="form-control" id="ddlOperationType">
                    </select>
                </div>
                <div class="form-group col-xs-12 col-md-3 ">
                    <%--  <asp:DropDownList ID="ddlFacility" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                    <select class="form-control" id="ddlFacility">
                    </select>
                </div>
                <div class="form-group col-xs-12 col-md-2">
                    <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server" />
                </div>
                <div class="form-group col-xs-12 col-md-1">
                    <button type="button" class="btn btn-info" onclick="SearchData()"><i class="fa fa-search"></i>&nbsp;search</button>
                </div>
            </div>
            <%-- Export --%>
            <div class="row cExStyle">
                <div class="col-xs-12 " id="divExport">
                    <button type="button" onclick="ExportData();" class="btn btn-success">Export</button>
                    <asp:Button ID="btnEx" runat="server" CssClass="hidden" OnClick="btnEx_Click" />
                </div>
            </div>
            <%-- TABLE --%>
            <div class="row">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <table id="tblData" class="table dataTable table-bordered table-hover" style="width: 1010px; min-width: 100%">
                            <thead>
                                <tr>
                                    <th class="dt-head-center dissort cHeadAllTable">No.</th>
                                    <th class="dt-head-center sorting cHeadIndicatorTable">Operation Type</th>
                                    <th class="dt-head-center sorting cHeadIndicatorTable">Facility</th>
                                    <th class="dt-head-center sorting cHeadIndicatorTable">Indicator</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_1">Jan</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_1">Feb</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_1">Mar</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_2">Apr</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_2">May</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_2">Jun</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_3">Jul</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_3">Aug</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_3">Sep</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_4">Oct</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_4">Nov</th>
                                    <th class="dt-head-center dissort cHeadAllTable QHead_4">Dec</th>
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
                                    Items</label>
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
            <%-- สัญลักษณ์ --%>
            <div class="row">
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle cbtnDefalt cbtnShow"><i class='fas'></i></a>
                        <label class="control-label">No Action</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-info cbtnShow"><i class='fas'></i></a>
                        <label class="control-label">Save Draft (L0)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-info cbtnShow"><i class='fas fa-hourglass'></i></a>
                        <label class="control-label">Submitted by Operational User (L0)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-warning cbtnShow"><i class='fas fa-retweet'></i></a>
                        <label class="control-label">Edit Requested by Operational User (L0)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-success cbtnShow"><i class="fas fa-user-check"></i></a>
                        <label class="control-label">Approved by Manager (L1)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-success cbtnShow"><i class="fas fa-check"></i></a>
                        <label class="control-label">Approved by ENVI Corporate (L2)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-success cbtnShow"><i class="fas"></i></a>
                        <label class="control-label">Completed (L3)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-danger cbtnShow"><i class="fas fa-reply"></i></a>
                        <label class="control-label">Rejected by Manager (L1)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-danger cbtnShow"><i class="fas fa-reply-all"></i></a>
                        <label class="control-label">Rejected by ENVI Corporate (L2)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-success cbtnShow"><i class="fas fa-check-double"></i></a>
                        <label class="control-label">Accept Edit Request by ENVI Corporate(L2)</label>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6">
                    <div class="form-group">
                        <a class="btn-circle btn-info cbtnShow"><i class="fas fa-sync"></i></a>
                        <label class="control-label">Recalled by Operational User (L0)</label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidYear" runat="server" />
    <asp:HiddenField ID="hidIndicator" runat="server" />
    <asp:HiddenField ID="hidOperationType" runat="server" />
    <asp:HiddenField ID="hidFacility" runat="server" />

    <asp:HiddenField ID="hidPms" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <!-- Modal -->
    <div id="popDetail" class="modal fade col-xs-12" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
        <div class="modal-dialog" role="document" style="width: 60%;">
            <div class="modal-content">
                <div class="modal-header cModal">
                    <h4 class="modal-title">History</h4>
                </div>
                <div class="modal-body">
                    <div id="divPopContentDetail">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tbDetail" class="table dataTable table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th class="dt-head-center dissort" style="width: 20%;">Date</th>
                                                <th class="dt-head-center dissort" style="width: 15%;">Status</th>
                                                <th class="dt-head-center dissort" style="width: 30%;">Remark</th>
                                                <th class="dt-head-center dissort" style="width: 15%;">Action By</th>
                                                <th class="dt-head-center dissort" style="width: 15%;">Next Step By</th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="dvModelDetail" class="text-right">
                        <button class="btn" type="button" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        var $ddlIndicator = $("select#ddlIndicator");
        var $ddlOperationType = $("select#ddlOperationType");
        var $ddlFacility = $("select#ddlFacility");
        var $hdfsIndicator = $("input[id$=hdfsIndicator]");
        var $hdfsOperationType = $("input[id$=hdfsOperationType]");
        var $hdfsFacility = $("input[id$=hdfsFacility]");
        var $ddlYear = $("select[id$=ddlYear]");

        $(function () {
            SetDataMaster(true);
            SetEventTableOnDocReady("divContent", "tblData", "ddlPageSize", function (sIndexCol, sOrderBy, sPageIndex, sMode) { LoadData(sIndexCol, sOrderBy, sPageIndex, sMode); });
        });

        function SearchData() {
            LoadData("", "", "", GridEvent.BIND, false);
        }
        function LoadData(sIndexCol, sOrderBy, sPageIndex, sMode, IsPageload) {
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
                sPms: $("input[id$=hidPms]").val(),

                sOperationTypeID: GetValDropdown("ddlOperationType"),
                sIDIndicator: GetValDropdown("ddlIndicator"),
                sFacilityID: GetValDropdown("ddlFacility"),
                sYear: GetValDropdown("ddlYear"),
            };

            AjaxCallWebMethod("LoadData", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    if (!Boolean(IsPageload)) {
                        PopupLogin();
                    }
                }
                else {
                    $("table[id$=" + sTableID + "] tbody tr").remove();
                    if (response.d.lstData != null && response.d.lstData.length > 0) {
                        var htmlTD = '<tr>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '</tr>';

                        $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                        var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                        $("table[id$=" + sTableID + "] tbody tr").remove();
                        var nStartDataIndex = parseInt(response.d.nStartItemIndex);

                        $.each(response.d.lstData, function (indx, item) {
                            $("td", row).eq(0).html(nStartDataIndex + ".");
                            $("td", row).eq(1).html(item.sOperationTypeName);
                            $("td", row).eq(2).html(item.sFacilityName);
                            $("td", row).eq(3).html(item.sIndicator);
                            $("td", row).eq(4).html(item.sM1);
                            $("td", row).eq(5).html(item.sM2);
                            $("td", row).eq(6).html(item.sM3);
                            $("td", row).eq(7).html(item.sM4);
                            $("td", row).eq(8).html(item.sM5);
                            $("td", row).eq(9).html(item.sM6);
                            $("td", row).eq(10).html(item.sM7);
                            $("td", row).eq(11).html(item.sM8);
                            $("td", row).eq(12).html(item.sM9);
                            $("td", row).eq(13).html(item.sM10);
                            $("td", row).eq(14).html(item.sM11);
                            $("td", row).eq(15).html(item.sM12);

                            $("table[id$=" + sTableID + "] tbody").append(row);
                            row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                            nStartDataIndex++;
                        });
                        SetTootip();
                        SetHoverRowColor(sTableID);
                        HideLoadding();
                    }
                    else {
                        SetRowNoData(sTableID, 16);
                        HideLoadding();
                    }
                    SetEvenTableAfterBind(response, divContiner, function (sIndexCol, sOrderBy, activeIndex, pageindex) { LoadData(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex); }, sTableID, "ckbAll", "ckbRow");

                }

            }, "", { itemSearch: Param, sStatus: "S" });
        }

        function DetailData(nFormID, nMonth, nReportID, nFac, nIDt) {
            // $("#popDetail").modal('toggle');

            var divContiner = "divPopContentDetail";
            var sTableID = "tbDetail";
            var dataSort = GetDataColumSort(sTableID);
            sIndexCol = dataSort.colindex;
            sOrderBy = dataSort.orderby;


            var Param = {
                nFormID: nFormID,
                nReportID: nReportID,
                nMonth: nMonth,
                nFac: nFac,
                nIDt: nIDt,
            };

            AjaxCallWebMethod("ListDetail", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    PopupLogin();
                }
                else {
                    $("table[id$=" + sTableID + "] tbody tr").remove();
                    if (response.d.lstData != null && response.d.lstData.length > 0) {
                        var htmlTD = '<tr>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '</tr>';

                        $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                        var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                        $("table[id$=" + sTableID + "] tbody tr").remove();
                        var nStartDataIndex = 1;

                        var prms = Input("hdfPrmsMenu").val();
                        $.each(response.d.lstData, function (indx, item) {
                            $("td", row).eq(0).html(item.sDate);
                            $("td", row).eq(1).html(item.sStatusName);
                            $("td", row).eq(2).html(item.sRemark);
                            $("td", row).eq(3).html(item.sActionBy);
                            $("td", row).eq(4).html(item.sNextTo);

                            $("table[id$=" + sTableID + "] tbody").append(row);
                            row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                            nStartDataIndex++;
                        });

                        SetICheck();
                        SetTootip();
                        SetHoverRowColor(sTableID);
                        $("#popDetail").modal('toggle');
                    }
                    else {
                        SetRowNoData(sTableID, 7);
                        $("#popDetail").modal('toggle');
                    }

                    HideLoadding();
                    SetEvenTableAfterBind(response, divContiner, function (sIndexCol, sOrderBy, activeIndex, pageindex) { LoadData(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex); }, sTableID, "ckbAll", "ckbRow");
                }

            }, "", { itemSearch: Param });
        }

        /////////************ EXPORT *************\\\\\\\\\\\\
        function ExportData() {
            $("input[id$=hidYear]").val(GetValDropdown("ddlYear"));
            $("input[id$=hidIndicator]").val(GetValDropdown("ddlIndicator"));
            $("input[id$=hidOperationType]").val(GetValDropdown("ddlOperationType"));
            $("input[id$=hidFacility]").val(GetValDropdown("ddlFacility"));
            $("input[id$=btnEx]").click();
        }

        /////////************ EXPORT *************\\\\\\\\\\\\
        function SetDataMaster(IsPageLoad) {
            BlockUI();
            $.ajax({
                type: 'POST',
                url: './Ashx/GenIndicator.ashx', //fileName.aspx/FunctionName
                data: JSON.stringify({}), //Variable in function
                beforeSend: function () {
                    BlockUI();
                },
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    arrlst = data;

                    if (data.sMsg == "") {
                        var start_year = new Date().getFullYear();

                        $ddlIndicator.append($("<option/>").text("- All Indicator -").val(""))
                        $ddlOperationType.append($("<option/>").text("- All OperationType -").val(""))
                        $ddlFacility.append($("<option/>").text("- All Sub-facility -").val(""))

                        var lstTemp = [];
                        var lstTemp2 = [];
                        $.each(arrlst.lstIn, function () {
                            $ddlIndicator.append($("<option/>").text(this.Indicator).val(this.ID))

                            $.each(this.lstOperationType, function () {
                                var nID = this.ID;
                                var tp = Enumerable.From(lstTemp).Where(function (w) { return w.ID == nID }).FirstOrDefault();
                                if (tp == null) {

                                    lstTemp.push({
                                        Name: this.Name,
                                        ID: nID,
                                        nOrder: this.nOrder
                                    });

                                }


                                $.each(this.lstFacility, function () {
                                    var nID2 = this.ID;
                                    var tp = Enumerable.From(lstTemp2).Where(function (w) { return w.ID == nID2 }).FirstOrDefault();
                                    
                                    if (tp == null) {
                                        lstTemp2.push({
                                            Name: this.Name,
                                            ID: nID2,
                                            nOrder: this.nOrder
                                        });

                                    }
                                });

                            });

                        });
                        lstTemp = Enumerable.From(lstTemp).OrderBy(function (w) { return w.nOrder }).ToArray();
                        lstTemp2 = Enumerable.From(lstTemp2).OrderBy(function (w) { return w.nOrder }).ToArray();
                        $.each(lstTemp, function () {
                            $ddlOperationType.append($("<option/>").text(this.Name).val(this.ID))
                        });
                        $.each(lstTemp2, function () {
                            $ddlFacility.append($("<option/>").text(this.Name).val(this.ID))
                        });
                        $("select#ddlYear").val(start_year)
                        $ddlIndicator.change(function () {
                            var lstTemp = [];
                            var lstTemp2 = [];
                            $ddlOperationType.find("option").remove();
                            $ddlFacility.find("option").remove();
                            var nVal = $(this).val();
                            if ($ddlIndicator.val() != "") {
                                $.each(arrlst.lstIn, function () {
                                    if (nVal == this.ID) {
                                        $ddlOperationType.append($("<option/>").text("- All OperationType -").val(""))
                                        if (this.lstOperationType.length > 0) {
                                            $.each(this.lstOperationType, function () {
                                                var nOrderOperationType = 1;
                                                $ddlOperationType.append($("<option/>").text(this.Name).val(this.ID))
                                            });
                                            $ddlOperationType.change();
                                        }

                                    }

                                });
                            }
                            else {
                                $ddlOperationType.find("option").remove();
                                $ddlFacility.find("option").remove();
                                $ddlOperationType.append($("<option/>").text("- All OperationType -").val(""))
                                $ddlFacility.append($("<option/>").text("- All Sub-facility -").val(""));
                                $.each(arrlst.lstIn, function () {

                                    $.each(this.lstOperationType, function () {
                                        var nID = this.ID;
                                        var tp = Enumerable.From(lstTemp).Where(function (w) { return w.ID == nID }).FirstOrDefault();
                                        if (tp == null) {

                                            lstTemp.push({
                                                Name: this.Name,
                                                ID: nID,
                                                nOrder: this.nOrder
                                            });

                                        }


                                        $.each(this.lstFacility, function () {
                                            var nID2 = this.ID;
                                            var tp = Enumerable.From(lstTemp2).Where(function (w) { return w.ID == nID2 }).FirstOrDefault();
                                            if (tp == null) {
                                                lstTemp2.push({
                                                    Name: this.Name,
                                                    ID: nID2,
                                                    nOrder: this.nOrder
                                                });

                                            }
                                        });

                                    });
                                });
                                lstTemp = Enumerable.From(lstTemp).OrderBy(function (w) { return w.nOrder }).ToArray();
                                lstTemp2 = Enumerable.From(lstTemp2).OrderBy(function (w) { return w.nOrder }).ToArray();

                                $.each(lstTemp, function () {
                                    $ddlOperationType.append($("<option/>").text(this.Name).val(this.ID))
                                });
                                $.each(lstTemp2, function () {
                                    $ddlFacility.append($("<option/>").text(this.Name).val(this.ID))
                                });
                                $ddlOperationType.change();
                            }

                        });
                        $ddlOperationType.change(function () {
                            
                            $ddlFacility.find("option").remove();
                            var nVal = $(this).val();
                            var lstTemp2 = [];
                            $ddlFacility.append($("<option/>").text("- All Sub-facility -").val(""))
                            $.each(arrlst.lstIn, function () {
                                if ($ddlIndicator.val() != "") {
                                    if ($ddlIndicator.val() == this.ID) {
                                        if (nVal != "") {
                                            $.each(this.lstOperationType, function () {
                                                if (nVal == this.ID) {

                                                    if (this.lstFacility.length > 0) {
                                                        $ddlFacility.attr("disabled", false);
                                                        $.each(this.lstFacility, function () {
                                                            $ddlFacility.append($("<option/>").text(this.Name).val(this.ID))
                                                        });

                                                    }
                                                    else {
                                                        $ddlFacility.attr("disabled", true);
                                                    }
                                                }

                                            });
                                        }
                                        else {
                                            $.each(this.lstOperationType, function () {
                                                $.each(this.lstFacility, function () {
                                                    var nID2 = this.ID;

                                                    var tp = Enumerable.From(lstTemp2).Where(function (w) { return w.ID == nID2 }).FirstOrDefault();

                                                    if (tp == null) {
                                                        lstTemp2.push({
                                                            Name: this.Name,
                                                            ID: nID2,
                                                            nOrder: this.nOrder
                                                        });

                                                    }
                                                });

                                            });
                                        }
                                    }
                                }
                                else {
                                    $.each(this.lstOperationType, function () {
                                        
                                        if (nVal != "")
                                        {
                                            if (nVal == this.ID) {
                                                $.each(this.lstFacility, function () {
                                                    var nID2 = this.ID;
                                                    var tp = Enumerable.From(lstTemp2).Where(function (w) { return w.ID == nID2 }).FirstOrDefault();

                                                    if (tp == null) {
                                                        lstTemp2.push({
                                                            Name: this.Name,
                                                            ID: nID2,
                                                            nOrder: this.nOrder
                                                        });

                                                    }
                                                });
                                            }
                                        }
                                        else {
                                            $.each(this.lstFacility, function () {
                                                var nID2 = this.ID;
                                                
                                                var tp = Enumerable.From(lstTemp2).Where(function (w) { return w.ID == nID2 }).FirstOrDefault();

                                                if (tp == null) {
                                                    lstTemp2.push({
                                                        Name: this.Name,
                                                        ID: nID2,
                                                        nOrder: this.nOrder
                                                    });

                                                }
                                            });
                                        }
                    
                                    });
                                }


                            });
                            lstTemp2 = Enumerable.From(lstTemp2).OrderBy(function (w) { return w.nOrder }).ToArray();
                            $.each(lstTemp2, function () {
                                $ddlFacility.append($("<option/>").text(this.Name).val(this.ID))
                            });
                        });

                        if (Boolean(IsPageLoad)) {
                            LoadData("", "", "", GridEvent.BIND, true)
                        }
                    }
                    else {
                        if (SysProcess.SessionExpired == data.sStatus) {
                            PopupLogin();
                        }
                        else {
                            DialogWarning(DialogHeader.Warning, data.sMsg);
                        }
                    }
                },
                complete: function (data) {
                    if (!Boolean(IsPageLoad)) {
                        UnblockUI();
                    }
                    if ($hdfsIndicator.val() != "") {
                        $ddlIndicator.val($hdfsIndicator.val());
                    }
                    if ($hdfsOperationType.val() != "") {
                        if ($hdfsOperationType.val() == "0") {
                            $hdfsOperationType.val("");
                        }
                        $ddlOperationType.val($hdfsOperationType.val());
                        var nVal = $hdfsOperationType.val();
                        //$ddlFacility.find("option").remove();
                        //$ddlFacility.append($("<option/>").text("- Select Sub-facility -").val(""))
                        //$.each(arrlst.lstIn, function () {
                        //    if ($ddlIndicator.val() == this.ID) {

                        //        $.each(this.lstOperationType, function () {
                        //            if (nVal == this.ID) {

                        //                if (this.lstFacility.length > 0) {
                        //                    $.each(this.lstFacility, function () {
                        //                        $ddlFacility.append($("<option/>").text(this.Name).val(this.ID))
                        //                    });

                        //                }

                        //            }
                        //        });
                        //    }

                        //});
                        //$ddlOperationType.attr("disabled", false);
                    }
                }
            });
        }
    </script>
</asp:Content>

