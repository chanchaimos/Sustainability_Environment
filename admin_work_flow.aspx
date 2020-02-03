<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_work_flow.aspx.cs" Inherits="admin_work_flow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <script src="Scripts/bootstrap-multiselect/js/bootstrap-multiselect.js"></script>
    <link href="Scripts/bootstrap-multiselect/css/bootstrap-multiselect.css" rel="stylesheet" />
    <script type="text/javascript">
        var multiselect_template = {
            button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown">' +
                         '<table width="100%">' +
                             '<tr>' +
                                 '<td class="text-left"><span class="multiselect-selected-text"></span></td>' +
                                 '<td class="text-right"><b class="caret"></b></td>' +
                             '</tr>' +
                         '</table>' +
                     '</button>',
            li: '<li><a tabindex="0"><label class="cb-dropdown"><input type="checkbox" /></label></a></li>',
        };
    </script>
    <style type="text/css">
        label {
            color: #262626 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary" id="DivCreate_Workflow" runat="server">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;WorkFlow</div>
        <div class="panel-body" id="divContent">

            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Operation type <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:DropDownList ID="ddlOperationType" runat="server" CssClass="form-control" multiple="multiple" Width="75%">
                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtOperationType" CssClass="hidden"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Sub-facility <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:DropDownList runat="server" ID="ddlFacility" CssClass="form-control" multiple="multiple" Width="75%"></asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtFacility" CssClass="hidden"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Group indicator <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:DropDownList runat="server" ID="ddlGroupIndicator" CssClass="form-control" multiple="multiple" Width="75%"></asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtGroupIndicator" CssClass="hidden"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Manager name (L1) <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:DropDownList runat="server" ID="ddlManager" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">ENVI Corporate name (L2) <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:DropDownList runat="server" ID="ddlEnviron" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer text-center">
            <button type="button" class="btn btn-primary btnInput" onclick="Add_Operation()"><i class="fa fa-save"></i>&nbsp;Save</button>
            <a class="btn btn-default btnInput" href="admin_work_flow.aspx">Cancel</a>
        </div>
    </div>

    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;WorkFlow</div>
        <div class="panel-body" id="divContent1">
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-group">
                        <div class="row" style="padding: 10px">
                            <div class="col-xs-12 col-md-3 col-lg-3">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Firstname,Lastname"></asp:TextBox>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-9">
                                <div class="row">
                                    <div class="col-xs-12 col-md-4">
                                        <asp:DropDownList runat="server" ID="ddlOperationSearch" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-12 col-md-3">
                                        <asp:DropDownList runat="server" ID="ddlFacilitySearch" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-12 col-md-3">
                                        <asp:DropDownList runat="server" ID="ddlIndicatorSearch" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-12 col-md-2">
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
                                    <th class="dt-head-center sorting" style="width: 20%;">Operation type</th>
                                    <th class="dt-head-center sorting" style="width: 15%;">Sub-facility</th>
                                    <th class="dt-head-center sorting" style="width: 15%;">Group indicator</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">L1</th>
                                    <th class="dt-head-center sorting" style="width: 20%;">L2</th>
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
    <asp:HiddenField ID="hdfPrmsMenu" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var arrOperationType = []; //arrMuti
        var arrFacility = []; //arrMuti

        var arrData = []; //
        var $ddlOperationType = $('select[id$=ddlOperationType]');
        var $txtOperationType = $('input[id$=txtOperationType]');

        var $ddlOperationSearch = $('select[id$=ddlOperationSearch]');
        var isView = GetValTextBox('hdfPrmsMenu') == 1;
        $(function () {

            var objValidate = {};
            objValidate[GetElementName('ddlOperationType', objControl.dropdown)] = {
                validators: {
                    callback: {
                        message: "Please select operation type",
                        callback: function (value, validator, $field) {
                            return GetMultiSeletValue("ddlOperationType").length > 0;
                        }
                    }
                }
            }
            objValidate[GetElementName("ddlFacility", objControl.dropdown)] = {
                validators: {
                    callback: {
                        message: "Please select Sub-facility",
                        callback: function (value, validator, $field) {
                            return GetMultiSeletValue("ddlFacility").length > 0;
                        }
                    }
                }
            }
            objValidate[GetElementName("ddlGroupIndicator", objControl.dropdown)] = {
                validators: {
                    callback: {
                        message: "Please select group indicator",
                        callback: function (value, validator, $field) {
                            return GetMultiSeletValue("ddlGroupIndicator").length > 0;
                        }
                    }
                }
            }
            objValidate[GetElementName("ddlManager", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Manager name");
            objValidate[GetElementName("ddlEnviron", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Environment name");
            BindValidateExcluded("divContent", objValidate);

            SETCONTROL();
        });
        function SETCONTROL() {
            $("select[id$=ddlFacilitySearch]").append($("<option/>").text("- search from sub-facility -").val(""));
            $("select[id$=ddlFacilitySearch]").prop("disabled", true).trigger("chosen:updated");
            SetMulitiselect();
            $("select[id$=ddlFacility]").multiselect('disable');
            SetEventTableOnDocReady("divContent1", "tblData", "ddlPageSize", function (sIndexCol, sOrderBy, sPageIndex, sMode) { LoadData(sIndexCol, sOrderBy, sPageIndex, sMode); });
            SetEventKeypressOnEnter(Input("txtSearch"), function () { SearchData() });
            SearchData();

            $ddlOperationType.change(function () {
                $("select[id$=ddlFacility]").html('').multiselect('rebuild');
                GET_Facility();
                //alert(arrOperationType);
            });
            $ddlOperationSearch.change(function () {
                GET_Facility_Seach();
            });
        }
        function GET_Facility_Seach() {
            arrFacility = [];
            if ($ddlOperationSearch.val() != "" || $ddlOperationSearch.val() != null) {
                LoaddinProcess();
                //arrOperationType = $ddlOperationSearch.val() || [];
                AjaxCallWebMethod("Get_Facility_Seach", function (response) {
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

        function GET_Facility() {
            if ($txtOperationType.val() != "" || $txtOperationType.val() != null) {
                LoaddinProcess();
                arrOperationType = $ddlOperationType.val() || [];
                AjaxCallWebMethod("Get_Facility", function (response) {
                    HideLoadding();
                    if (response.d.Status == SysProcess.SessionExpired) {
                        HideLoadding();
                        PopupLogin();
                    }
                    else {
                        arrFacility = [];
                        if (response.d.lstData_Facility.length > 0 && response.d.lstData_Facility.length != null) {
                            arrFacility = response.d.lstData_Facility;
                            $.each(arrFacility, function (i, el) {
                                var optFac = $('<option />', {
                                    value: el.nFacilityID,
                                    text: el.sFacilityName,
                                });
                                $("select[id$=ddlFacility]").append(optFac);
                                $("select[id$=ddlFacility]").multiselect('rebuild');
                            });
                            $("select[id$=ddlFacility]").multiselect('enable');
                        } else {
                            $('#divContent').formValidation('updateStatus', $('select[id$=ddlFacility]'), 'INVALID');
                            $("select[id$=ddlFacility]").multiselect('disable');
                        }
                    }
                }, "", { lst: arrOperationType });
            } else {
                $('#divContent').formValidation('updateStatus', $('select[id$=ddlFacility]'), 'INVALID');
                $("select[id$=ddlFacility]").multiselect('disable');
            }
        }

        function SearchData() {
            LoadData("", "", "", GridEvent.BIND);
        }

        function LoadData(sIndexCol, sOrderBy, sPageIndex, sMode) {
            LoaddinProcess();
            var divContiner = "divContent1";
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

                sOperationID: GetValDropdown('ddlOperationSearch'),
                sIndicatorID: GetValDropdown('ddlIndicatorSearch'),
                sFacilityID: GetValDropdown('ddlFacilitySearch'),
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
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
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
                                $("td", row).eq(0).html('<input type="checkbox" id="ckbRow" class="flat-green cNoPrms" /> <input type="text" id="txtid" class="hidden" value="' + item.IDFac + ',' + item.IDIndicator + '">' + nStartDataIndex + ".");
                            }

                            $("td", row).eq(1).html(item.sOperationName);
                            $("td", row).eq(2).html(item.sFacilityName);
                            $("td", row).eq(3).html(item.sGroupName);
                            $("td", row).eq(4).html(item.sManagerName);
                            $("td", row).eq(5).html(item.sEnvironName);

                            $("table[id$=" + sTableID + "] tbody").append(row);
                            row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                            nStartDataIndex++;
                        });

                        SetICheck();
                        SetTootip();
                        SetHoverRowColor(sTableID);
                    }
                    else {
                        SetRowNoData(sTableID, 6);
                    }
                    SetCheckBoxSelectRowInGrid
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

        function SetMulitiselect() {
            $('select[id$=ddlOperationType]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                //sModeSearch: true,
                templates: multiselect_template,
                maxHeight: 350,
                buttonWidth: '100%',
                nonSelectedText: '- Search from operation type -',
                numberDisplayed: 0,
                onChange: function (element, checked) {
                    ReValidateFieldControl("divContent", GetElementName("ddlOperationType", objControl.dropdown));
                }
            });
            if (!isView) {
                $('select[id$=ddlOperationType]').multiselect('select', $('input[id$=txtOperationType]').val().split(','));
            }

            $('select[id$=ddlFacility]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                //sModeSearch: true,
                templates: multiselect_template,
                maxHeight: 350,
                buttonWidth: '100%',
                nonSelectedText: '- Search from sub facility -',
                numberDisplayed: 0,
                onChange: function (element, checked) {
                    ReValidateFieldControl("divContent", GetElementName("ddlFacility", objControl.dropdown));
                }
            });
            if (!isView) {
                $('select[id$=ddlFacility]').multiselect('select', $('input[id$=txtFacility]').val().split(','));
            }


            $('select[id$=ddlGroupIndicator]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                //sModeSearch: true,
                templates: multiselect_template,
                maxHeight: 350,
                buttonWidth: '100%',
                nonSelectedText: '- Search from group indicator -',
                numberDisplayed: 0,
                onChange: function (element, checked) {
                    ReValidateFieldControl("divContent", GetElementName("ddlGroupIndicator", objControl.dropdown));
                }
            });
            if (!isView) {
                $('select[id$=ddlGroupIndicator]').multiselect('select', $('input[id$=txtGroupIndicator]').val().split(','));
            }
        }

        function Add_Operation() {
            var IsPass = (CheckValidate("divContent"));
            if (IsPass) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    LoaddinProcess();
                    AjaxCallWebMethod("AddOperation", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            HideLoadding();
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            arrData = response.d.lstMapping;
                            //DialogSuccess(DialogHeader.Info, DialogMsg.SaveComplete);
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "admin_work_flow.aspx");
                        } else if (response.d.Status == SysProcess.Failed) {
                            DialogConfirm_Duplicate(DialogHeader.Warning, response.d.Msg + DialogMsg.ConfirmSave, function () {
                                if (response.d.lstDuplicate.length > 0 && response.d.lstDuplicate != null) {
                                    var data = response.d.lstDuplicate;
                                    var dataSave = response.d.lstToSave;
                                    LoaddinProcess();
                                    AjaxCallWebMethod("ConfirmData", function (response) {
                                        HideLoadding();
                                        if (response.d.Status == SysProcess.SessionExpired) {
                                            HideLoadding();
                                            PopupLogin();
                                        } else if (response.d.Status == SysProcess.Success) {
                                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "admin_work_flow.aspx");
                                        } else {
                                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                                        }
                                    }, "", { lstData: data, lstData_Save: dataSave });
                                }
                            }, "");
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", {
                        sManagerID: GetValDropdown('ddlManager'),
                        sEnvironID: GetValDropdown('ddlEnviron'),
                        lst_Operation: GetValDropdown('ddlOperationType'),
                        lst_Facility: GetValDropdown('ddlFacility'),
                        lst_GroupIndicator: GetValDropdown('ddlGroupIndicator'),
                    });
                }, "");
            }
        }

        function DialogConfirm_Duplicate(head, msg, funcYes, funcNo) {
            BootstrapDialog.show({
                title: head,
                message: msg,
                type: BootstrapDialog.TYPE_WARNING,
                closable: true,
                draggable: true,
                buttons: [{
                    id: 'btn-ok',
                    //icon: 'glyphicon glyphicon-check',
                    label: btnOKText,
                    cssClass: 'btn btn-warning',
                    autospin: false,
                    action: function (dialogRef) {
                        dialogRef.close();
                        LoaddinProcess();
                        funcYes();
                    }
                },
                {
                    id: 'btn-cancel',
                    //icon: 'glyphicon glyphicon-remove',
                    label: btnCancelText,
                    cssClass: 'btn btn-default',
                    autospin: false,
                    action: function (dialogRef) {
                        dialogRef.close();
                        if (funcNo != null && funcNo != undefined && funcNo != "") {
                            funcNo();
                        }
                    }
                }
                ]
            });
        }
    </script>
</asp:Content>

