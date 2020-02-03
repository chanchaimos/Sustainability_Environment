<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_asset_lst.aspx.cs" Inherits="admin_asset_lst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;<asp:Literal runat="server" ID="ltrHeader"></asp:Literal></div>
        <div class="panel-body" id="divContent">
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-12 col-md-3 col-lg-2">
                                <asp:Literal runat="server" ID="ltrCreate"></asp:Literal>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-10">
                                <div class="row">
                                    <div class="col-xs-12 col-md-8">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Sub-facility Name"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-12 col-md-2">
                                        <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control">
                                            <asp:ListItem Value="" Text="- Status -"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Active"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Inactive"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-12 col-md-2">
                                        <button type="button" class="btn btn-info btn-block" onclick="SearchData()"><i class="fa fa-search"></i>&nbsp;Search</button>
                                    </div>
                                </div>
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
                                        <asp:CheckBox ID="ckbAll" runat="server" CssClass="flat-green checkbox-inline" />&nbsp;No.</th>
                                    <th class="dt-head-center sorting" style="width: 35%;">Sub-facility Name</th>
                                    <th class="dt-head-center sorting" style="width: 15%;">Status</th>
                                    <th class="dt-head-center sorting" style="width: 15%;">Last Update</th>
                                    <th class="dt-head-center dissort" style="width: 10%;"></th>
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
                            <button type="button" id="btnDel" runat="server" class="btn btn-danger btn-sm" onclick="DeleteDataInTable()"><i class="glyphicon glyphicon-trash"></i>&nbsp;Delete</button>
                        </div>
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
    <asp:HiddenField ID="hdfFacID" runat="server" />
    <asp:HiddenField ID="hdfPrmsMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        $(function () {
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
                sIndexCol: sIndexCol + "",
                sOrderBy: sOrderBy + "",
                sPageSize: GetValDropdown("ddlPageSize"),
                sPageIndex: sPageIndex,
                sMode: sMode,

                sSearch: GetValTextBox("txtSearch"),
                sStatus: GetValDropdown('ddlStatus'),
                sFacID: GetValTextBox("hdfFacID"),
                sPrms: Input("hdfPrmsMenu").val()
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
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '</tr>';

                        $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                        var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                        $("table[id$=" + sTableID + "] tbody tr").remove();
                        var nStartDataIndex = parseInt(response.d.nStartItemIndex);

                        var prms = Input("hdfPrmsMenu").val();
                        $.each(response.d.lstData, function (indx, item) {
                            $("td", row).eq(0).html('<input type="checkbox" id="ckbRow" class="flat-green cNoPrms" /> <input type="text" id="txtid" class="hidden" value="' + item.nAssetID + '">' + nStartDataIndex + ".");
                            $("td", row).eq(1).html(item.sAssetName);
                            $("td", row).eq(2).html(item.sStatus);
                            $("td", row).eq(3).html(item.sUpdate);
                            $("td", row).eq(4).html(item.sLink);

                            $("table[id$=" + sTableID + "] tbody").append(row);
                            row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                            nStartDataIndex++;
                        });

                        SetICheck();
                        SetTootip();
                        SetHoverRowColor(sTableID);
                    }
                    else {
                        SetRowNoData(sTableID, 5);
                    }

                    HideLoadding();
                    SetEvenTableAfterBind(response, divContiner, function (sIndexCol, sOrderBy, activeIndex, pageindex) { LoadData(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex); }, sTableID, "ckbAll", "ckbRow");
                }

            }, "", { itemSearch: Param });
        }

        function DeleteDataInTable() {
            DeleteData("tblData", "ckbRow", "txtid", function (arrID) {
                AjaxCallWebMethod("DeleteData", function (response) {
                    HideLoadding();
                    if (response.d.Status == SysProcess.SessionExpired) {
                        PopupLogin();
                    }
                    else {
                        DialogSuccess(DialogHeader.Info, DialogMsg.DelComplete);
                        if ($("input[id$=ckbAll]").is(":checked")) {
                            $("input[id$=ckbAll]").iCheck("uncheck");
                        }
                        SearchData();
                    }
                }, "", { arrValue: arrID });
            });
        }
    </script>
</asp:Content>

