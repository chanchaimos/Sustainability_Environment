<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_user_info_lst.aspx.cs" Inherits="admin_user_info_lst" %>

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
                        <div class="row" style="padding: 10px">
                            <div class="col-xs-12 col-md-3 col-lg-2">
                                <button id="btnCreate" type="button" runat="server" class="btn btn-primary btn-block" onclick="CreateData()"><i class="fa fa-plus"></i>&nbsp;Create User</button>
                                <%--<a class="btn btn-primary btn-sm btn-block" href="admin_user_info_update.aspx"><i class="fa fa-plus"></i>&nbsp;Create User</a>--%>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-10">
                                <div class="row">
                                    <div class="col-xs-12 col-md-2">
                                    </div>
                                    <div class="col-xs-12 col-md-10">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Username,Firstname,Lastname"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="padding: 10px">
                            <div class="col-xs-12 col-md-3 col-lg-2">
                                <asp:DropDownList runat="server" ID="ddlUserRole" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-10">
                                <div class="row">
                                    <div class="col-xs-12 col-md-4">
                                        <asp:DropDownList runat="server" ID="ddlOperationSearch" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-12 col-md-3">
                                        <asp:DropDownList runat="server" ID="ddlFacilitySearch" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-12 col-md-2">
                                        <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control">
                                            <asp:ListItem Value="" Text="- Status -"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Active"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Inactive"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-12 col-md-3">
                                        <button type="button" class="btn btn-info btn-block" onclick="SearchData()"><i class="fa fa-search"></i>&nbsp;search</button>
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
                                    <th class="dt-head-center sorting" style="width: 10%;">Username</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Name</th>
                                    <th class="dt-head-center sorting" style="width: 15%;">Org</th>
                                    <th class="dt-head-center sorting" style="width: 10%;">User role</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">Email</th>
                                    <th class="dt-head-center sorting" style="width: 10%;">Status</th>
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
    <!-- Modal -->
    <div id="MPPopContent_User" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header csetpop">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="MPhTitle_User"></h4>
                </div>
                <div class="modal-body">
                    <div id="divMPPopContent_User"></div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdfPrmsMenu" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">

    <script type="text/javascript">
        var isView = GetValTextBox('hdfPrmsMenu') == 1;
        var $ddlOperationSearch = $('select[id$=ddlOperationSearch]');

        $(function () {
            SetEventTableOnDocReady("divContent", "tblData", "ddlPageSize", function (sIndexCol, sOrderBy, sPageIndex, sMode) { LoadData(sIndexCol, sOrderBy, sPageIndex, sMode); });
            SetEventKeypressOnEnter(Input("txtSearch"), function () { SearchData() });
            SearchData();
            $("select[id$=ddlFacilitySearch]").append($("<option/>").text("- Search from sub-facility -").val(""));
            $("select[id$=ddlFacilitySearch]").prop("disabled", true).trigger("chosen:updated");
            $ddlOperationSearch.change(function () {
                GET_Facility();
            });
        });
        function GET_Facility() {
            arrFacility = [];
            if ($ddlOperationSearch.val() != "" || $ddlOperationSearch.val() != null) {
                LoaddinProcess();
                //arrOperationType = $ddlOperationSearch.val() || [];
                AjaxCallWebMethod("Get_Facility", function (response) {
                    HideLoadding();
                    if (response.d.Status == SysProcess.SessionExpired) {
                        HideLoadding();
                        PopupLogin();
                    }
                    else {
                        $("select[id$=ddlFacilitySearch]").html('');

                        if (response.d.lstData_Facility.length > 0 && response.d.lstData_Facility.length != null) {
                            $("select[id$=ddlFacilitySearch]").append($("<option/>").text("- search from sub-facility -").val(""));
                            arrFacility = response.d.lstData_Facility;
                            $.each(arrFacility, function (i, el) {
                                var optFac = $('<option />', {
                                    value: el.nFacilityID,
                                    text: el.sFacilityName,
                                });
                                $("select[id$=ddlFacilitySearch]").append(optFac);
                            });
                            $("select[id$=ddlFacilitySearch]").prop("disabled", false).trigger("chosen:updated");
                        } else {
                            $("select[id$=ddlFacilitySearch]").append($("<option/>").text("- search from sub-facility -").val(""));
                            $("select[id$=ddlFacilitySearch]").prop("disabled", true).trigger("chosen:updated");
                        }
                    }
                }, "", { operationID: $ddlOperationSearch.val() });
            } else {
                $("select[id$=ddlFacilitySearch]").append($("<option/>").text("- search from sub-facility -").val(""));
                $("select[id$=ddlFacilitySearch]").prop("disabled", true).trigger("chosen:updated");
            }
        }
        function CreateData() {
            window.location = "admin_user_info_update.aspx";
        }
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

                sUserRole: GetValDropdown('ddlUserRole'),
                sOperationSearch: GetValDropdown('ddlOperationSearch'),
                sFacilitySearch: GetValDropdown('ddlFacilitySearch'),
                sStatus: GetValDropdown('ddlStatus'),
                sSearch: GetValTextBox("txtSearch"),

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
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '</tr>';

                        $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                        var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                        $("table[id$=" + sTableID + "] tbody tr").remove();
                        var nStartDataIndex = parseInt(response.d.nStartItemIndex);

                        var prms = Input("hdfPrmsMenu").val();
                        $.each(response.d.lstData, function (indx, item) {
                            if (isView) {
                                $("td", row).eq(0).html(nStartDataIndex + ".");
                            } else {
                                $("td", row).eq(0).html('<input type="checkbox" id="ckbRow" class="flat-green cNoPrms" /> <input type="text" id="txtid" class="hidden" value="' + item.nID + '">' + nStartDataIndex + ".");
                            }

                            $("td", row).eq(1).html(item.Username);
                            $("td", row).eq(2).html(item.Firstname + ' ' + item.Lastname);
                            $("td", row).eq(3).html(item.Company);
                            $("td", row).eq(4).html('<span style="cursor:pointer"><a onclick="UserRole(' + item.nID + ')">' + item.nCountRole + '<i class="fa fa-search"></i></a></span>');
                            $("td", row).eq(5).html(item.Email);
                            $("td", row).eq(6).html(item.sStatus);
                            $("td", row).eq(7).html(item.sLink);

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

        function UserRole(nID) {
            LoaddinProcess();
            AjaxCallWebMethod("GetRole", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    PopupLogin();
                } else {
                    arrDataRole = [];
                    arrDataRole = response.d.lstDataRole;

                    var divData = '<table class="table dataTable table-bordered table-responsive table-hover">';
                    divData += '<thead><tr><th class="dt-head-center">Role Name</th></tr></thead>';
                    divData += '<tbody>'
                    var nRow = 1;
                    if (arrDataRole.length > 0) {
                        for (var i = 0; i < arrDataRole.length; i++) {
                            divData += '<tr><td class="dt-body-left">' + nRow + '. ' + arrDataRole[i].sRoleName + '</td></tr>';
                            nRow++;
                        }
                    } else {
                        divData += '<tr><td class="dt-body-center" style="color:red;">NoData</td></tr>';
                    }

                    divData += '</tbody></table>';

                    $("#divMPPopContent_User").html(divData);
                    $("#MPhTitle_User").html("Role");
                    $("#MPPopContent_User").modal();
                    $('#MPPopContent_User').on('hidden.bs.modal', function (e) {
                        ("#divMPPopContent_User").html("");
                    });
                    HideLoadding();
                }
            }, "", {
                sUserID: nID,
            });
        }
    </script>
</asp:Content>

