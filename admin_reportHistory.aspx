<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_reportHistory.aspx.cs" Inherits="admin_reportHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .modal-backdrop {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;User Info</div>
        <div class="panel-body" id="divContent">
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4">
                                <asp:LinkButton runat="server" ID="lnkExport" CssClass="btn btn-success btnTable btn-sm" data-toggle="tooltip" title="Export To Excel" OnClick="ExportExcel_Click"><i class="far fa-file-excel"></i>&nbsp;Export To Excel</asp:LinkButton>
                            </div>
                            <div class="input-daterange col-lg-6" id="datepicker">
                                <div class="input-group" style="width: 100%;">
                                    <input type="text" class="input-small form-control" name="start" id="txtStartDate" />
                                    <span class="input-group-addon">to</span>
                                    <input type="text" class="input-small form-control" name="end" id="txtEndDate" />
                                </div>
                            </div>

                            <div class="col-lg-2">
                                <button type="button" class="btn btn-info btn-block" onclick="SearchData()" id="btnSearch"><i class="fa fa-search"></i>&nbsp;search</button>
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
                                    <th class="dt-head-center dissort" style="width: 10%;">No.</th>
                                    <th class="dt-head-center sorting" style="width: 10%;">Username</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Full Name</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Head Menu Name</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Menu Name</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Action Date</th>
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
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $txtStartDate = $("input[id$=txtStartDate]");
        var $txtEndDate = $("input[id$=txtEndDate]");
        $(function () {
            SetDateRangePicker($txtStartDate, $txtEndDate);
            $txtStartDate.datepicker("setDate", new Date());
            $txtEndDate.datepicker("setDate", (new Date()));
            $txtStartDate.change(function () {
                if ($(this).val() != "" && $txtEndDate.val() != "") {
                    $("button#btnSearch").prop('disabled', false);
                }
                else {
                    $("button#btnSearch").prop('disabled', true);
                }

            });
            $txtEndDate.change(function () {
                if ($(this).val() != "" && $txtStartDate.val() != "") {
                    $("button#btnSearch").prop('disabled', false);
                }
                else {
                    $("button#btnSearch").prop('disabled', true);
                }

            });
            SetEventTableOnDocReady("divContent", "tblData", "ddlPageSize", function (sIndexCol, sOrderBy, sPageIndex, sMode) { LoadData(sIndexCol, sOrderBy, sPageIndex, sMode); });
            SetEventKeypressOnEnter(Input("txtSearch"), function () { SearchData() });
            SearchData();
        });

        function SearchData() {
            LoadData("", "", "", GridEvent.BIND);
        }

        function LoadData(sIndexCol, sOrderBy, sPageIndex, sMode) {
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
                sStartDate: $txtStartDate.val(),
                sEndDate: $txtEndDate.val(),
                sIndexCol: sIndexCol + "",
                sOrderBy: sOrderBy + "",
                sPageSize: GetValDropdown("ddlPageSize"),
                sPageIndex: sPageIndex,
                sMode: sMode,
            };

            AjaxCallWebMethod("LoadData", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    PopupLogin();
                }
                else {
                    $("table[id$=" + sTableID + "] tbody tr").remove();
                    if (response.d.lstData != null && response.d.lstData.length > 0) {
                        var htmlTD = '<tr><td class="dt-body-center"></td>';
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

                        $.each(response.d.lstData, function (indx, item) {
                            $("td", row).eq(0).html(nStartDataIndex + ".");
                            $("td", row).eq(1).html(item.Username);
                            $("td", row).eq(2).html(item.Firstname + ' ' + item.Lastname);
                            $("td", row).eq(3).html(item.sMenuHead);
                            $("td", row).eq(4).html(item.sMenu);
                            $("td", row).eq(5).html(item.sAction);

                            $("table[id$=" + sTableID + "] tbody").append(row);
                            row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                            nStartDataIndex++;
                        });

                        SetICheck();
                        SetTootip();
                        SetHoverRowColor(sTableID);
                    }
                    else {
                        SetRowNoData(sTableID, 8);
                    }

                    HideLoadding();
                    SetEvenTableAfterBind(response, divContiner, function (sIndexCol, sOrderBy, activeIndex, pageindex) { LoadData(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex); }, sTableID, "ckbAll", "ckbRow");
                }

            }, "", { itemSearch: Param });
        }
        function SetDateRangePicker($txtDateBEGIN, $txtDateEND) {
            if ($txtDateBEGIN != "" && $txtDateEND != "") {
                SetDatePicker($txtDateBEGIN);
                SetDatePicker($txtDateEND);

                $txtDateBEGIN
                    .change(function () {
                        var thisVal = $(this).val();
                        if (thisVal != '') $txtDateEND.datepicker('setStartDate', thisVal);
                        else SetDatePicker($txtDateEND.datepicker('remove'));

                    })
                    .keydown(function (e) {
                        //if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                        //    return false;
                        //}
                    });

                $txtDateEND
                    .change(function () {
                        var thisVal = $(this).val();
                        if (thisVal != '') $txtDateBEGIN.datepicker('setEndDate', thisVal);
                        else SetDatePicker($txtDateBEGIN.datepicker('remove'));

                    })
                    .keydown(function (e) {
                        //if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                        //    return false;
                        //}
                    });
            } else if ($txtDateEND == "") {
                SetDatePicker($txtDateBEGIN);

                $txtDateBEGIN
                    .keydown(function (e) {
                        if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                            return false;
                        }
                    });
            }
        }
        function SetDatePicker($txtDate) {
            $txtDate
                .datepicker({
                    format: 'dd/mm/yyyy',
                    autoclose: true
                })
                .keydown(function (e) {
                    if ($(this).is('[readonly]')) {
                        if (e.which == 8 || e.which == 46) {
                            $(this).val('').change();
                            return false;
                        }
                    }
                });
            $txtDate.prev().click(function () { $txtDate.focus(); });
        }
    </script>
</asp:Content>


