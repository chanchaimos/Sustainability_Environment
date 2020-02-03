<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="epi_mytask.aspx.cs" Inherits="epi_mytask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;My task</div>
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
                                <%--  <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Facility"></asp:TextBox>--%>
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
                                    <th class="dt-head-center dissort" style="width: 10%;">
                                        <asp:CheckBox ID="ckbAll" runat="server" CssClass="flat-green checkbox-inline cNoPrms" />&nbsp;No.</th>
                                    <th class="dt-head-center sorting" style="width: 10%;">Year</th>
                                    <th class="dt-head-center sorting" style="width: 10%;">Month</th>
                                    <th class="dt-head-center sorting" style="width: 25%;">Facility</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Group Indicator</th>
                                    <th class="dt-head-center dissort" style="width: 20%;">Status</th>
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
                        <div class="cSet3 cNoPrms">
                            <button type="button" id="btnAppr" runat="server" class="btn btn-success btn-sm"><i class="glyphicon glyphicon-check"></i>&nbsp;Approve</button>
                        </div>
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
    <asp:HiddenField ID="hdfPrmsMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        $(function () {
            $('button[id$=btnAppr]').on('click', function () {
                ApproveAll();
            });
            SetEventTableOnDocReady("divContent", "tblData", "ddlPageSize", function (sIndexCol, sOrderBy, sPageIndex, sMode) { LoadData(sIndexCol, sOrderBy, sPageIndex, sMode); });
            //  SetEventKeypressOnEnter(Select("ddlFacility"), function () { SearchData() });
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

                // sSearch: GetValTextBox("txtSearch"),
                sFacID: GetValDropdown("ddlFacility"),
                nYear: GetValDropdown("ddlYear"),
                nGroupIndID: GetValDropdown("ddlGroupIndicator"),
                sPrms: Input("hdfPrmsMenu").val(),
                nStatus: GetValDropdown("ddlStatus"),
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
                            if (prms == "2") {
                                $("td", row).eq(0).html('<input type="checkbox" id="ckbRow" class="flat-green" /><input type="text" id="txtid" class="hidden" value="' + item.nFormID + '" data-month="' + item.nMonth + '"> ' + nStartDataIndex + ".");
                            } else {
                                $("td", row).eq(0).html('<input type="text" id="txtid" class="hidden" value="' + item.nFormID + '" data-month="' + item.nMonth + '">' + nStartDataIndex + ".");
                            }
                            $("td", row).eq(1).html(item.sYear);
                            $("td", row).eq(2).html(item.sMonth);
                            $("td", row).eq(3).html(item.sNameFacilities);
                            $("td", row).eq(4).html(item.sNameIndicator);
                            $("td", row).eq(5).html(item.sStatus);
                            $("td", row).eq(6).html(item.sBtn);

                            $("table[id$=" + sTableID + "] tbody").append(row);
                            row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                            nStartDataIndex++;
                        });

                        SetICheck();
                        SetTootip();
                        SetHoverRowColor(sTableID);

                        if (prms == "2") {
                            $(".cNoPrms").show();
                        } else {
                            $(".cNoPrms").hide();
                        }
                    }
                    else {
                        SetRowNoData(sTableID, 7);
                        $(".cNoPrms").hide();
                    }

                    HideLoadding();
                    SetEvenTableAfterBind(response, divContiner, function (sIndexCol, sOrderBy, activeIndex, pageindex) { LoadData(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex); }, sTableID, "ckbAll", "ckbRow");
                }

            }, "", { itemSearch: Param });
        }

        function ApproveAll() {
            var arrDataApprove = [];
            var arrTR = $("table[id$=tblData] tbody tr");
            for (var i = 0; i < arrTR.length; i++) {
                if ($(arrTR[i]).find("div.icheckbox_flat-green").attr("aria-checked") + "" == "true") {
                    var sFormID = $(arrTR[i]).find("input[id$=txtid]").attr("value");
                    var sMonth = $(arrTR[i]).find("input[id$=txtid]").attr("data-month");
                    arrDataApprove.push({
                        nMonth: +sMonth || 0,
                        nFormID: +sFormID || 0
                    });
                }
            }

            if (arrDataApprove.length > 0) {
                DialogConfirm(DialogHeader.Confirm, "Do you want approve data ?", function () {
                    AjaxCallWebMethod("ApproveAll", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        }
                        else {
                            DialogSuccess(DialogHeader.Info, "Already approved.");
                            if ($("input[id$=ckbAll]").is(":checked")) {
                                $("input[id$=ckbAll]").iCheck("uncheck");
                            }
                            SearchData();
                        }
                    }, "", { arrValue: arrDataApprove });
                });
            } else {
                DialogWarning(DialogHeader.Warning, "Please select data !");
            }
        }
    </script>
</asp:Content>

