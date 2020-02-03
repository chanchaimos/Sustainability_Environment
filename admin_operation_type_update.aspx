<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_operation_type_update.aspx.cs" Inherits="admin_operation_type_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        input[type="radio"] {
            display: inline-block;
            width: 19px;
            height: 19px;
            margin: -2px 10px 0 0;
            vertical-align: middle;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-edit"></i>&nbsp;Create / Edit</div>
        <div class="panel-body" id="divContent">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Operation type name <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtOperationName" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Description :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12">
                        <div class="table-responsive">
                            <table id="tblData" class="table dataTable table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th class="dt-head-center" style="width: 10%;">No.</th>
                                        <th class="dt-head-center" style="width: 60%;">Facility name</th>
                                        <%--                                        <th class="dt-head-center dissort" style="width: 15%;">Read</th>
                                        <th class="dt-head-center dissort" style="width: 15%;">Write</th>--%>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="row" style="display: none">
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

                <div class="form-group" style="padding-top: 15px">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Status <span class="text-red">*</span> :</label>
                    <div class="col-xs-6 col-md-3">
                        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Y" Text="Active" Selected="True" class="flat-green radio radio-inline"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Inactive" class="flat-green radio radio-inline"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-xs-6 col-md-6">
                        <div class="form-group">
                            <div class="col-xs-12">
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" MaxLength="1000" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="panel-footer text-center">
            <button type="button" class="btn btn-primary" onclick="SaveData()"><i class="fa fa-save"></i>&nbsp;Save</button>
            <a class="btn btn-default" href="admin_operation_type_lst.aspx">Cancel</a>
        </div>
    </div>
    <asp:HiddenField ID="hdfEncryptOperationID" runat="server" />
    <asp:HiddenField ID="hdfOperationID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $txtOperationName = $('input[id$=txtOperationName]');
        var $txtDesc = $('textarea[id$=txtDesc]');
        var $txtRemark = $('input[id$=txtRemark]');

        $(function () {
            var objValidate = {};
            objValidate[GetElementName("txtOperationName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Operation type name");
            objValidate[GetElementName("txtRemark", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Remark");
            BindValidate("divContent", objValidate);

            $("div#divContent").delegate("input[id*=rblStatus]", 'ifClicked', function () {
                if ($(this).val() == "N") {
                    Input("txtRemark").prop("disabled", false);
                }
                else {
                    UpdateStatusValidateControl("divContent", GetElementName("txtRemark", objControl.txtbox), ValidateProp.Status_NOT_VALIDATED);
                    Input("txtRemark").prop("disabled", true);
                }
            });
            //$('input[id*=rblStatus]').on('ifChecked', function (event) {
            //    if ($(this).val() == "N") {
            //        Input("txtRemark").prop("disabled", false);
            //    }
            //    else {
            //        UpdateStatusValidateControl("divContent", GetElementName("txtRemark", objControl.txtbox), ValidateProp.Status_NOT_VALIDATED);
            //        Input("txtRemark").prop("disabled", true);
            //    }
            //});

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
                sPrms: Input("hdfPrmsMenu").val()
            };

            AjaxCallWebMethod("LoadData_Facility", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    PopupLogin();
                }
                else {
                    $("table[id$=" + sTableID + "] tbody tr").remove();
                    if (response.d.lstDataFacility != null && response.d.lstDataFacility.length > 0) {
                        var htmlTD = '<tr><td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        //htmlTD += '<td class="dt-body-center"></td>';
                        //htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '</tr>';

                        $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                        var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                        $("table[id$=" + sTableID + "] tbody tr").remove();
                        var nStartDataIndex = parseInt(response.d.nStartItemIndex);

                        var prms = Input("hdfPrmsMenu").val();
                        $.each(response.d.lstDataFacility, function (indx, item) {

                            //var rdoPRMS_Enabled = '<span class="radio radio-success"><input id="rdoPRMS2_' + item.nID + '" type="radio" data-menu="' + item.nID + '" name="rdoPRMS_' + item.nID + '" value="2" ' + (item.sPermission == "2" || item.sPermission == "" ? "checked" : "") + '><label for="rdoPRMS2_' + item.nID + '">&nbsp;</label></span>';
                            //var rdoPRMS_Disabled = '<span class="radio radio-success"><input id="rdoPRMS_' + item.nID + '" type="radio" data-menu="' + item.nID + '" name="rdoPRMS_' + item.nID + '" value="1" ' + (item.sPermission == "1" ? "checked" : "") + '><label for="rdoPRMS_' + item.nID + '">&nbsp;</label></span>';
                            //$("td", row).eq(0).html('<input type="checkbox" id="ckbRow" class="flat-green cNoPrms" /> <input type="text" id="txtid" class="hidden" value="' + item.nID + '">' + nStartDataIndex + ".");
                            $("td", row).eq(0).html(nStartDataIndex);
                            $("td", row).eq(1).html(item.sName);
                            //$("td", row).eq(2).html(rdoPRMS_Enabled);
                            //$("td", row).eq(3).html(rdoPRMS_Disabled);
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

            }, "", { itemSearch: Param, sID: GetValTextBox('hdfEncryptOperationID') });
        }

        function SaveData() {
            var Ispass = (CheckValidate("divContent"));
            if (Ispass) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    var arrParam = {
                        sOperationName: $txtOperationName.val(),
                        sDesc: $txtDesc.val(),
                        sRemark: $txtRemark.val(),
                        sStatus: GetValRadioListICheck('rblStatus'),
                        sID: GetValTextBox('hdfEncryptOperationID'),
                    };
                    LoaddinProcess();
                    AjaxCallWebMethod("SaveData", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            //arrData = response.d.lstMapping;
                            //DialogSuccess(DialogHeader.Info, DialogMsg.SaveComplete);
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "admin_operation_type_lst.aspx");
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", {
                        itemSave: arrParam,
                    });
                }, "");

            }
        }
    </script>
</asp:Content>

