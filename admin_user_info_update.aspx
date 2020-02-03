<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_user_info_update.aspx.cs" Inherits="admin_user_info_update" %>

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
    <%--    <style>
        input[type="radio"] {
            display: inline-block;
            width: 19px;
            height: 19px;
            margin: -2px 10px 0 0;
            vertical-align: middle;
            cursor: pointer;
        }
    </style>--%>
    <style type="text/css">
        label {
            color: #262626 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <i class="fa fa-table"></i>&nbsp;User Info
        </div>
        <div class="panel-body" id="divContent">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">User type <span class="text-red">*</span> :</label>
                    <div class="col-xs-7 col-md-4">
                        <asp:RadioButtonList ID="rblUsertype" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0" Text="GC Employee" Selected="True" class="flat-green radio radio-inline"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Contract" class="flat-green radio radio-inline"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div id="DivContract" runat="server">
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Name <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Surname <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Org <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtOrg" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">E-mail <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Username <span class="text-red">*</span> :</label>
                        <div class="col-xs-7 col-md-4">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Password <span class="text-red">*</span> :</label>
                        <div class="col-xs-6 col-md-4">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-6 col-md-4">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <button type="button" runat="server" id="btnResetmail" class="btn btn-primary" onclick="ResetPass()">&nbsp;Reset</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="DivGc" runat="server">
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Name <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-search"></i></div>
                                <%--<asp:TextBox ID="txtNameGc" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>--%>

                                <asp:TextBox runat="server" ID="txtNameGc" CssClass="form-control" placeholder="Search from EmployeeID,EmployeeName"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdfEmployeeName" />
                                <asp:HiddenField runat="server" ID="hdfEmployeeLastName" />
                                <asp:HiddenField runat="server" ID="hdfEmployeeID" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Org <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtOrgGc" runat="server" CssClass="form-control" MaxLength="90"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">E-mail <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtEmailGc" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Status <span class="text-red">*</span> :</label>
                    <div class="col-xs-7 col-md-4">
                        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Y" Text="Active" Selected="True" class="flat-green radio radio-inline"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Inactive" class="flat-green radio radio-inline"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div id="Operation">
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Role <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-4">
                            <asp:DropDownList runat="server" ID="ddlUserRole" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div id="divRoleAdmin" style="display: none">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tblData_admin" class="table dataTable table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <%--<th class="dt-head-center dissort" style="width: 10%;">
                                                <asp:CheckBox ID="CheckBox1" runat="server" CssClass="flat-green checkbox-inline" />&nbsp;No.</th>--%>
                                                <th class="dt-head-center" style="width: 10%;">No.</th>
                                                <th class="dt-head-center" style="width: 40%;">Menu</th>
                                                <th class="dt-head-center" style="width: 25%;">Enable</th>
                                                <th class="dt-head-center" style="width: 25%;">Disable</th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="cSet1" style="display: none">
                                    <div class="cSet3 cNoPrms">
                                        <%--<button type="button" id="Button1" runat="server" class="btn btn-danger btn-sm" onclick="DeleteDataInTable()"><i class="glyphicon glyphicon-trash"></i>&nbsp;Delete</button>--%>
                                    </div>
                                    <div class="cSet2">
                                        <div class="dataTables_length">
                                            <label>
                                                List
                                            <asp:DropDownList ID="ddlPageSize_RoleAdmin" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
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

                <div id="Div_AddOperation_Permission">

                    <div id="addOperation">
                        <div class="form-group">
                            <label class="control-label col-xs-12 col-md-3 text-left-sm">Operation type <span class="text-red">*</span> :</label>
                            <div class="col-xs-12 col-md-5">
                                <asp:DropDownList ID="ddlOperationType" runat="server" CssClass="form-control" multiple="multiple" Width="75%">
                                </asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtOperationType" CssClass="hidden"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-xs-12 col-md-3 text-left-sm">Sub-facility  <span class="text-red">*</span> :</label>
                            <div class="col-xs-12 col-md-5">
                                <asp:DropDownList runat="server" ID="ddlFacility" CssClass="form-control" multiple="multiple" Width="75%"></asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtFacility" CssClass="hidden"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-xs-12 col-md-3 text-left-sm">Group indicator <span class="text-red">*</span> :</label>
                            <div class="col-xs-12 col-md-5">
                                <asp:DropDownList runat="server" ID="ddlGroupIndicator" CssClass="form-control" multiple="multiple" Width="75%"></asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtGroupIndicator" CssClass="hidden"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-xs-12 col-md-3 text-left-sm">Permission <span class="text-red">*</span> :</label>
                            <div class="col-xs-3 col-md-2">
                                <asp:DropDownList runat="server" ID="ddlPermission" CssClass="form-control">
                                    <asp:ListItem Value="2" Text="Read / Write"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Read"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-xs-3 col-md-2">
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <button type="button" runat="server" id="btnAddOperation" class="btn btn-primary" onclick="Add_Operation()">&nbsp;Add Operation</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="divOperation">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tblData" class="table dataTable table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th class="dt-head-center dissort" style="width: 10%;">
                                                    <asp:CheckBox ID="ckbAll" runat="server" CssClass="flat-green checkbox-inline" />&nbsp;No.</th>
                                                <th class="dt-head-center sorting" style="width: 35%;">Operation type</th>
                                                <th class="dt-head-center sorting" style="width: 20%;">Sub-facility</th>
                                                <th class="dt-head-center sorting" style="width: 15%;">Group indicator</th>
                                                <th class="dt-head-center sorting" style="width: 20%;">Permission</th>
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



                <div class="form-group">
                    <div class="col-xs-12 col-lg-offset-5 col-md-9">
                        <button type="button" runat="server" id="AddData_Role" class="btn btn-primary" onclick="AddData()">&nbsp;Add Role</button>
                        <button type="button" runat="server" id="btnCancel_Role" class="btn btn-default" onclick="ClearData()">&nbsp;Cancel</button>
                        <%--<a class="btn btn-default" onclick="ClearData()">Cancel</a>--%>
                    </div>
                </div>

                <div class="row" id="divContent_Main">
                    <div class="col-xs-12">
                        <div class="table-responsive">
                            <table id="tblData_Main" class="table dataTable table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th class="dt-head-center dissort" style="width: 10%;">No.</th>
                                        <th class="dt-head-center dissort" style="width: 70%;">Role</th>
                                        <th class="dt-head-center dissort" style="width: 20%;"></th>
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
                                <%--<button type="button" id="Button1" runat="server" class="btn btn-danger btn-sm" onclick="DeleteDataInTable()"><i class="glyphicon glyphicon-trash"></i>&nbsp;Delete</button>--%>
                            </div>
                            <div class="cSet2">
                                <div class="dataTables_length hidden">
                                    <label>
                                        List
                                            <asp:DropDownList ID="ddlPage_Main" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
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
        <div class="panel-footer text-center">
            <button type="button" runat="server" id="btnSave_Main" class="btn btn-primary btnInput" onclick="SaveToDB()"><i class="fa fa-save"></i>&nbsp;Save</button>
            <%--<a class="btn btn-default btnInput" href="admin_user_info_lst.aspx">Cancel</a>--%>
            <button type="button" runat="server" id="btnCancel_Main" class="btn btn-default btnInput" onclick="Cancel()">&nbsp;Cancel</button>
        </div>
    </div>
    <asp:HiddenField ID="hdfPrmsMenu" runat="server" />
    <asp:HiddenField ID="hidCheckDup" runat="server" />
    <asp:HiddenField ID="hidUserrole" runat="server" />
    <asp:HiddenField ID="hidUserID" runat="server" />
    <asp:HiddenField ID="hidEncryptUserID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var isView = GetValTextBox('hdfPrmsMenu') == 1;
        var $ddlUserRole = $('select[id$=ddlUserRole]');
        var $ddlPermission = $('select[id$=ddlPermission]');
        var lst_Menu = []; // Menu_Admin
        var arrOperation = []; // arr_Operation
        var arrData = []; //arr_MainData
        var arrMenu = []; // arr_Menu

        var arrDelData = []; // del_Data

        var arrOperationAll = [];
        var arrOperationType = []; //arrMuti
        var arrFacility = []; //arrMuti
        var $ddlOperationType = $('select[id$=ddlOperationType]');
        var $txtOperationType = $('input[id$=txtOperationType]');

        var $ddlFacility = $('select[id$=ddlFacility]');
        var $ddlGroupIndicator = $('select[id$=ddlGroupIndicator]');

        var $hdfEmployeeName = $('input[id$=hdfEmployeeName]');
        var $hdfEmployeeLastName = $('input[id$=hdfEmployeeLastName]');
        var $hdfEmployeeID = $('input[id$=hdfEmployeeID]');
        var $txtNameGc = $('input[id$=txtNameGc]');
        var $txtEmailGc = $('input[id$=txtEmailGc]');
        var $txtOrgGc = $('input[id$=txtOrgGc]');

        var $btnResetmail = $('button[id$=btnResetmail]');
        $(function () {
            if ($('input[id$=hidUserID]').val() == "") {
                $btnResetmail.hide();
            }
            if (isView) {
                $("input").prop("disabled", true);
            }

            //$("select[id$=ddlFacility]").empty(); //remove all child nodes               
            //$("select[id$=ddlFacility]").append($("<option/>").text("-- Search from Facility --").val("0"));
            //$("select[id$=ddlFacility]").prop("disabled", true).trigger("chosen:updated");
            //$("select[id$=ddlFacility]").trigger("chosen:updated");

            var objValidate = {};
            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Name");
            objValidate[GetElementName("txtSurname", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Surname");
            objValidate[GetElementName("txtOrg", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Org");
            objValidate[GetElementName("txtEmail", objControl.txtbox)] = addValidateEmail_notEmpty(DialogMsg.Specify + " Email");
            objValidate[GetElementName("txtUsername", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Username");
            objValidate[GetElementName("txtPassword", objControl.txtbox)] = addValidate_Password_notEmpty_Length(20, "Please specify password");
            BindValidate("divContent", objValidate);

            var objValidate1 = {};
            objValidate1[GetElementName("ddlUserRole", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " User role");
            BindValidate("Operation", objValidate1);

            var objValidate2 = {};
            //objValidate2[GetElementName("txtNameGc", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Name");
            objValidate2[GetElementName('txtNameGc', objControl.txtbox)] = addValidate_textAutocomplete('Please specify employee name/code', $hdfEmployeeID);
            objValidate2[GetElementName("txtOrgGc", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Org");
            objValidate2[GetElementName("txtEmailGc", objControl.txtbox)] = addValidateEmail_notEmpty(DialogMsg.Specify + " Email");
            BindValidate($("div[id$=DivGc]").attr("id"), objValidate2);

            var objValidate3 = {};
            //objValidate2[GetElementName("txtNameGc", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Name");
            objValidate3[GetElementName('ddlOperationType', objControl.dropdown)] = {
                validators: {
                    callback: {
                        message: "Please select operation type",
                        callback: function (value, validator, $field) {
                            return GetMultiSeletValue("ddlOperationType").length > 0;
                        }
                    }
                }
            }
            objValidate3[GetElementName("ddlFacility", objControl.dropdown)] = {
                validators: {
                    callback: {
                        message: "Please select sub-facility",
                        callback: function (value, validator, $field) {
                            return GetMultiSeletValue("ddlFacility").length > 0;
                        }
                    }
                }
            }
            objValidate3[GetElementName("ddlGroupIndicator", objControl.dropdown)] = {
                validators: {
                    callback: {
                        message: "Please select group indicator",
                        callback: function (value, validator, $field) {
                            return GetMultiSeletValue("ddlGroupIndicator").length > 0;
                        }
                    }
                }
            }
            objValidate3[GetElementName("ddlPermission", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Permission");
            BindValidateExcluded("addOperation", objValidate3);

            SETCONTROL();
        });

        function Cancel() {
            window.location = "admin_user_info_lst.aspx";
        }
        function SETCONTROL() {
            SetMulitiselect();

            SetEventTableOnDocReady("divRoleAdmin", "tblData_admin", "ddlPageSize_RoleAdmin", function (sIndexCol, sOrderBy, sPageIndex, sMode) { LoadData(sIndexCol, sOrderBy, sPageIndex, sMode, ''); });
            SetEventKeypressOnEnter(Input("txtSearch"), function () { SearchData() });
            SetEventTableOnDocReady("divOperation", "tblData", "ddlPageSize", function (sIndexCol, sOrderBy, sPageIndex, sMode) { Add_Operation(sIndexCol, sOrderBy, sPageIndex, sMode, 'E'); });
            SetEventTableOnDocReady("divContent_Main", "tblData_Main", "ddlPage_Main", function (sIndexCol, sOrderBy, sPageIndex, sMode) { LoadData_Main(sIndexCol, sOrderBy, sPageIndex, sMode, ''); });
            //SetEventKeypressOnEnter(Input("txtSearch"), function () { Add_Operation() });
            SearchData();

            $ddlUserRole.change(function () {
                UpdateStatusValidateControl("Operation", $("select[id$=ddlUserRole]"), "NOT_VALIDATED");
                if (arrOperation.length > 0 && arrOperation != null && arrOperation != undefined) {
                    //DialogWarning(DialogHeader.Confirm, "Do you want change user role ?");
                    DialogConfirm(DialogHeader.Confirm, "Do you want change user role ?", function () {
                        arrOperation.length = 0;
                        Add_Operation("", "", "", GridEvent.BIND, 'C');
                        $ddlUserRole.change();
                    }, function () {
                        $ddlUserRole.val($('input[id$=hidUserrole]').val());
                    });
                } else {
                    var IsAdmin_role = $ddlUserRole.val() == 1;
                    var IsCompanyAdmin_role = $ddlUserRole.val() == 6;

                    if (IsAdmin_role) {
                        LoadData("", "", "", GridEvent.BIND, '1'); //Load_menuAdmin
                        $('select[id$=ddlPermission]').val('1').prop("disabled", true);
                        $('#divRoleAdmin').show();

                        //if ($ddlUserRole.val() != '2') {
                        //    $('select[id$=ddlPermission]').val('1').prop("disabled", true);
                        //} else {
                        //    $('select[id$=ddlPermission]').val('2').prop("disabled", false);
                        //}


                    } else if (IsCompanyAdmin_role) {

                        LoadData("", "", "", GridEvent.BIND, '6'); //Load_menuAdmin
                        $('select[id$=ddlPermission]').val('1').prop("disabled", true);
                        $('#divRoleAdmin').show();

                    } else {
                        $('#divRoleAdmin').hide();
                        if ($ddlUserRole.val() != "") {
                            if ($ddlUserRole.val() == '3' || $ddlUserRole.val() == '4') {
                                $('div[id$=Div_AddOperation_Permission]').hide();
                            } else if ($ddlUserRole.val() != '2') {
                                $('div[id$=Div_AddOperation_Permission]').show();
                                $('select[id$=ddlPermission]').val('1').prop("disabled", true);
                            } else {
                                $('div[id$=Div_AddOperation_Permission]').show();
                                $('select[id$=ddlPermission]').val('2').prop("disabled", false);
                            }
                        } else {
                            $('div[id$=Div_AddOperation_Permission]').show();
                            $('select[id$=ddlPermission]').val('2').prop("disabled", false);
                        }
                    }
                    $('input[id$=hidUserrole]').val($ddlUserRole.val());
                }
                //$ddlUserRole.val('1').prop("disabled", false);

            });

            $ddlOperationType.change(function () {

                $("select[id$=ddlFacility]").html('').multiselect('rebuild');
                GET_Facility();
                //ReValidateFieldControl("addOperation", $("select[id$=ddlFacility]"));
                //alert(arrOperationType);
            });
            //$("input[id*=rblUsertype]").trigger('ifClicked');
            $("div#divContent").delegate("input[id*=rblUsertype]", 'ifClicked', function () {
                if ($(this).val() == "0") { //Value 0 = GC
                    $('div[id$=DivContract]').hide();
                    $('div[id$=DivGc]').show();
                }
                else {
                    $('div[id$=DivContract]').show();
                    $('div[id$=DivGc]').hide();

                    $('input[id$=txtNameGc]').val('').prop("disabled", false);
                    $('input[id$=txtOrgGc]').val('').prop("disabled", false);
                    $('input[id$=txtEmailGc]').val('').prop("disabled", false);

                    $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $("input[id$=txtNameGc]").attr("name"), "VALIDATED");
                    $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $("input[id$=txtOrgGc]").attr("name"), "VALIDATED");
                    $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $("input[id$=txtEmailGc]").attr("name"), "VALIDATED");

                    $('input[id$=hdfEmployeeID]').val('');
                    $('input[id$=hdfEmployeeName]').val('');
                    $('input[id$=hdfEmployeeLastName]').val('');
                }
            });
        }

        function Reset_Dropdown() {
            UpdateStatusValidateControl("addOperation", $('select[id$=ddlOperationType]'), 'NOT_VALIDATED');
            UpdateStatusValidateControl("addOperation", $('select[id$=ddlFacility]'), 'NOT_VALIDATED');
            UpdateStatusValidateControl("addOperation", $('select[id$=ddlGroupIndicator]'), 'NOT_VALIDATED');
            UpdateStatusValidateControl("addOperation", $('select[id$=ddlPermission]'), 'NOT_VALIDATED');
            $("select[id$=ddlOperationType]").multiselect("clearSelection");
            $("select[id$=ddlFacility]").multiselect("clearSelection");
            $("select[id$=ddlGroupIndicator]").multiselect("clearSelection");

            $('input[id$=txtOperationType]').val('');
            $('input[id$=txtFacility]').val('');
            $('input[id$=txtGroupIndicator]').val('');
        }

        function SearchData() {
            LoadData_Main("", "", "", GridEvent.BIND, ''); //UserData_Role 
            // LoadData("", "", "", GridEvent.BIND, ''); //Load_menuAdmin
            Add_Operation("", "", "", GridEvent.BIND, '');
        }
        function LoadData(sIndexCol, sOrderBy, sPageIndex, sMode, sEditdata) {
            LoaddinProcess();
            var divContiner = "divRoleAdmin";
            var sTableID = "tblData_admin";

            //alway load data
            if (sMode != GridEvent.sort) {
                var dataSort = GetDataColumSort(sTableID);
                sIndexCol = dataSort.colindex;
                sOrderBy = dataSort.orderby;
            }

            var Param = {
                sIndexCol: sIndexCol + "",
                sOrderBy: sOrderBy + "",
                sPageSize: GetValDropdown("ddlPageSize_RoleAdmin"),
                sPageIndex: sPageIndex,
                sMode: sMode,
                sEditdata: sEditdata,

                sSearch: GetValTextBox("txtSearch"),
                sPrms: Input("hdfPrmsMenu").val()
            };

            AjaxCallWebMethod("Get_MenuAdmin", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    PopupLogin();
                }
                else {
                    if (response.d.lst_Menu.length > 0 && response.d.lst_Menu.length != null) {
                        lst_Menu = [];
                        lst_Menu = response.d.lst_Menu;
                    }
                    BlindTB_RoleAdmin();
                }

            }, "", {
                itemSearch: Param,
                lst: arrMenu,
            });
        }

        function BlindTB_RoleAdmin() {
            $("table[id$=tblData_admin] tbody tr").remove();
            if (lst_Menu.length > 0 && lst_Menu.length != null) {
                var htmlTD = '<tr><td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-left"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '</tr>';

                $("table[id$=tblData_admin] tbody").append(htmlTD);
                var row = $("table[id$=tblData_admin] tbody").find("tr").last().clone(true);
                $("table[id$=tblData_admin] tbody tr").remove();
                //var nStartDataIndex = parseInt(response.d.nStartItemIndex);

                var prms = Input("hdfPrmsMenu").val();
                var checked = "";
                $.each(lst_Menu, function (indx, item) {
                    var rdoPRMS_Enabled = '<input class="flat-green radio" id="rdoPRMS2_' + item.nMenuID + '" type="radio" data-menu="' + item.nMenuID + '" name="rdoPRMS_' + item.nMenuID + '" value="2" ' + (item.sPermission == "2" || item.sPermission == "" ? "checked" : "") + '><label for="rdoPRMS2_' + item.nMenuID + '">&nbsp;</label>';
                    var rdoPRMS_Disabled = '<input class="flat-green radio" id="rdoPRMS_' + item.nMenuID + '" type="radio" data-menu="' + item.nMenuID + '" name="rdoPRMS_' + item.nMenuID + '" value="0" ' + (item.sPermission == "0" ? "checked" : "") + '><label for="rdoPRMS_' + item.nMenuID + '">&nbsp;</label>';

                    $("td", row).eq(0).html('<input type="hidden" id="hidMenuID_' + item.nMenuID + '" data-menu="' + item.nMenuID + '"/>' + (indx + 1));
                    $("td", row).eq(1).html(item.sMenuName);
                    $("td", row).eq(2).html(rdoPRMS_Enabled);
                    $("td", row).eq(3).html(rdoPRMS_Disabled);
                    //$("td", row).eq(4).html('');
                    //$("td", row).eq(5).html(item.sLink);

                    $("table[id$=tblData_admin] tbody").append(row);
                    row = $("table[id$=tblData_admin] tbody").find("tr").last().clone(true);
                    indx++;
                });

                $('input[id*="rdoPRMS2_"].flat-green,input[type="radio"].flat-green').iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_square-green'//'iradio_flat-green'
                });
                SetTootip();
                SetHoverRowColor("tblData_admin");
            } else {
                SetRowNoData("tblData_admin", 5);
            }
            HideLoadding();
            //SetEvenTableAfterBind(response, divContiner, function (sIndexCol, sOrderBy, activeIndex, pageindex) { LoadData(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex); }, "tblData_admin", "ckbAll", "ckbRow");
        }

        function SetMulitiselect() {
            $('select[id$=ddlOperationType]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                //sModeSearch: true,
                maxHeight: 350,
                templates: multiselect_template,
                buttonWidth: '100%',

                nonSelectedText: '- Search from operation type -',
                numberDisplayed: 0,
                onChange: function (element, checked) {
                    ReValidateFieldControl("addOperation", GetElementName("ddlOperationType", objControl.dropdown));
                }
            });
            $('select[id$=ddlOperationType]').multiselect('select', $('input[id$=txtOperationType]').val().split(','));
            //$("select[id$=ddlOperationType]").multiselect('select', $('input[id$=txtOperationType]').val().split(',')).change();

            $('select[id$=ddlFacility]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                //sModeSearch: true,
                maxHeight: 350,
                templates: multiselect_template,
                buttonWidth: '100%',
                nonSelectedText: '- Search from sub-facility -',
                numberDisplayed: 0,
                onChange: function (element, checked) {
                    ReValidateFieldControl("addOperation", GetElementName("ddlFacility", objControl.dropdown));
                }
            });
            $('select[id$=ddlFacility]').multiselect('select', $('input[id$=txtFacility]').val().split(','));

            $('select[id$=ddlGroupIndicator]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                //sModeSearch: true,
                templates: multiselect_template,
                buttonWidth: '100%',
                maxHeight: 350,
                nonSelectedText: '- Search from group indicator -',
                numberDisplayed: 0,
                onChange: function (element, checked) {
                    ReValidateFieldControl("addOperation", GetElementName("ddlGroupIndicator", objControl.dropdown));
                }
            });
            $('select[id$=ddlGroupIndicator]').multiselect('select', $('input[id$=txtGroupIndicator]').val().split(','));
        }

        function GET_Facility(lst) {
            arrFacility = [];
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
                        $("select[id$=ddlFacility]").html('');

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
                            if (lst != "" && lst != null && lst != undefined) {
                                $("select[id$=ddlFacility]").multiselect('select', lst);
                            }
                        } else {
                            $('#addOperation').formValidation('updateStatus', $('select[id$=ddlFacility]'), 'INVALID');
                            $("select[id$=ddlFacility]").multiselect('disable');
                        }
                    }
                }, "", { lst: arrOperationType });
            } else {
                $('#addOperation').formValidation('updateStatus', $('select[id$=ddlFacility]'), 'INVALID');
                $("select[id$=ddlFacility]").multiselect('disable');
            }
        }

        function Add_Operation(sIndexCol, sOrderBy, sPageIndex, sMode, sEditdata) {
            var Ispass = false;
            if (sEditdata == "" || sEditdata == "C" || sEditdata == "E" || sEditdata == "Y") {
                Ispass = true;
            } else {
                Ispass = (CheckValidate("addOperation"));
            }
            var divOperation = "divOperation";
            var sTableOperationID = "tblData";

            //alway load data
            if (sMode != GridEvent.sort) {
                var dataSort = GetDataColumSort(sTableOperationID);
                sIndexCol = dataSort.colindex;
                sOrderBy = dataSort.orderby;
            }

            var Param = {
                sIndexCol: sIndexCol + "",
                sOrderBy: sOrderBy + "",
                sPageSize: GetValDropdown("ddlPageSize"),
                sPageIndex: sPageIndex,
                sMode: sMode,
                sEditdata: sEditdata,
                sSearch: GetValTextBox("txtSearch"),
                sPrms: Input("hdfPrmsMenu").val()
            };
            if (Ispass) {
                LoaddinProcess();
                AjaxCallWebMethod("Add_Operation", function (response) {
                    if (response.d.Status == SysProcess.SessionExpired) {
                        HideLoadding();
                        PopupLogin();
                    }
                    else {
                        //arrOperation.length = 0;
                        //arrOperationAll.length = 0;
                        arrOperation = [];
                        arrOperationAll = [];
                        $("table[id$=tblData] tbody tr").remove();
                        if (response.d.lstData.length > 0 && response.d.lstData != null) {
                            //arrOperation.length = 0;
                            //arrOperationAll.length = 0;

                            var lstOperationTypeID = [];
                            var lstFacility = [];
                            var lstGroupIndicator = [];

                            arrOperationAll = response.d.lstData_All;
                            if (arrOperationAll.length > 0 && arrOperationAll != null && arrOperationAll != undefined) {
                                $.each(arrOperationAll, function (indx, item) {
                                    lstOperationTypeID.push(item.nOperationTypeID);
                                    lstFacility.push(item.nFacilityID);
                                    lstGroupIndicator.push(item.nGroupIndicatorID);
                                });
                                lstOperationTypeID = lstOperationTypeID.filter(onlyUnique);
                                lstFacility = lstFacility.filter(onlyUnique);
                                lstGroupIndicator = lstGroupIndicator.filter(onlyUnique);
                                $("select[id$=ddlOperationType]").multiselect('select', lstOperationTypeID);
                                GET_Facility(lstFacility);
                                $("select[id$=ddlGroupIndicator]").multiselect('select', lstGroupIndicator);
                            }
                            arrOperation = response.d.lstData;
                            var htmlTD = '<tr><td class="dt-body-center"></td>';
                            htmlTD += '<td class="dt-body-left"></td>';
                            htmlTD += '<td class="dt-body-center"></td>';
                            htmlTD += '<td class="dt-body-center"></td>';
                            htmlTD += '<td class="dt-body-center"></td>';
                            //htmlTD += '<td class="dt-body-center"></td>';
                            htmlTD += '</tr>';

                            $("table[id$=tblData] tbody").append(htmlTD);
                            var row = $("table[id$=tblData] tbody").find("tr").last().clone(true);
                            $("table[id$=tblData] tbody tr").remove();
                            var nStartDataIndex = parseInt(response.d.nStartItemIndex);

                            var prms = Input("hdfPrmsMenu").val();
                            var checked = "";
                            $.each(arrOperation, function (indx, item) {
                                //var rdoPRMS_Enabled = '<span class="radio radio-success"><input id="rdoPRMS2_' + item.nMenuID + '" type="radio" name="rdoPRMS_' + item.nMenuID + '" value="2" ' + (item.nPermission == "2" || item.nPermission == "" ? "checked" : "") + '><label for="rdoPRMS2_' + item.nMenuID + '">&nbsp;</label></span>';
                                //var rdoPRMS_Disabled = '<span class="radio radio-success"><input id="rdoPRMS_' + item.nMenuID + '" type="radio" name="rdoPRMS_' + item.nMenuID + '" value="1" ' + (item.nPermission == "1" ? "checked" : "") + '><label for="rdoPRMS_' + item.nMenuID + '">&nbsp;</label></span>';
                                var sPrmsName = item.sPermission == "2" ? "Read / Write" : "Read";
                                if (isView) {
                                    $("td", row).eq(0).html(nStartDataIndex + ".");
                                } else {
                                    $("td", row).eq(0).html('<input type="checkbox" id="ckbRow_' + indx + '" class="flat-green cNoPrms" data-row="' + indx + '" /> <input type="text" id="txtid_' + indx + '" data-row="' + indx + '" class="hidden" value="' + item.nFacilityID + ',' + item.nGroupIndicatorID + '">' + nStartDataIndex + ".");
                                }
                                $("td", row).eq(1).html(item.sOperationName);
                                $("td", row).eq(2).html(item.sFacilityName);
                                $("td", row).eq(3).html(item.sGroupIndicatorName);
                                $("td", row).eq(4).html(sPrmsName);
                                //$("td", row).eq(5).html(item.sLink);

                                $("table[id$=tblData] tbody").append(row);
                                row = $("table[id$=tblData] tbody").find("tr").last().clone(true);
                                indx++;
                                nStartDataIndex++;
                            });
                            //SetICheck();
                            $('input[id*="ckbRow_"].flat-green,input[type="radio"].flat-green').iCheck({
                                checkboxClass: 'icheckbox_flat-green',
                                radioClass: 'iradio_square-green'//'iradio_flat-green'
                            });
                            //SetTootip();
                            SetHoverRowColor("tblData");
                            //Reset_Dropdown();
                        } else {
                            SetRowNoData("tblData", 5);
                        }
                        HideLoadding();
                        SetEvenTableAfterBind(response, divOperation, function (sIndexCol, sOrderBy, activeIndex, pageindex) { Add_Operation(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex, 'E'); }, sTableOperationID, "ckbAll", "ckbRow");
                        //BlindTB_Operation();
                    }
                }, "", {
                    itemSearch: Param,
                    lst_Operation: GetValDropdown('ddlOperationType'),
                    lst_Facility: GetValDropdown('ddlFacility'),
                    lst_GroupIndicator: GetValDropdown('ddlGroupIndicator'),
                    sPermission: GetValDropdown('ddlPermission'),
                    arrDel: arrDelData,
                    lstOperation_Data: arrOperation,
                    lstOperation_DataAll: arrOperationAll,

                });
                Reset_Dropdown();
            }

        }
        function LoadData_Main(sIndexCol, sOrderBy, sPageIndex, sMode, sEditdata) {
            LoaddinProcess();

            var divContiner = "divContent_Main";
            var sTableID = "tblData_Main";

            //alway load data
            if (sMode != GridEvent.sort) {
                var dataSort = GetDataColumSort(sTableID);
                sIndexCol = dataSort.colindex;
                sOrderBy = dataSort.orderby;
            }

            var Param = {
                sIndexCol: sIndexCol + "",
                sOrderBy: sOrderBy + "",
                sPageSize: GetValDropdown("ddlPage_Main"),
                sPageIndex: sPageIndex,
                sMode: sMode,
                sEditdata: sEditdata,

                //sSearch: GetValTextBox("txtSearch"),
                sPrms: Input("hdfPrmsMenu").val()
            };

            AjaxCallWebMethod("LoadData_Main", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    PopupLogin();
                }
                else {
                    $("table[id$=" + sTableID + "] tbody tr").remove();
                    if (response.d.lstData_Role != null && response.d.lstData_Role.length > 0) {
                        arrData.length = 0;
                        arrData = response.d.lstData_Role;
                        var htmlTD = '<tr><td class="dt-body-center"></td>';
                        htmlTD += '<td class="dt-body-left"></td>';
                        htmlTD += '<td class="dt-body-center"></td>';
                        //htmlTD += '<td class="dt-body-center"></td>';
                        //htmlTD += '<td class="dt-body-center"></td>';
                        //htmlTD += '<td class="dt-body-center"></td>';
                        htmlTD += '</tr>';

                        $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                        var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                        $("table[id$=" + sTableID + "] tbody tr").remove();
                        var nStartDataIndex = parseInt(response.d.nStartItemIndex);

                        var prms = Input("hdfPrmsMenu").val();
                        $.each(response.d.lstData_Role, function (indx, item) {
                            $("td", row).eq(0).html(nStartDataIndex);
                            $("td", row).eq(1).html(item.sRoleName);
                            if (isView) {
                                $("td", row).eq(2).html(item.sLink);
                            } else {
                                $("td", row).eq(2).html(item.sLink + ' ' + item.sLinkDel);
                            }
                            //$("td", row).eq(3).html(item.sRoleName);
                            //$("td", row).eq(4).html('');
                            //$("td", row).eq(5).html(item.sLink);

                            $("table[id$=" + sTableID + "] tbody").append(row);
                            row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                            nStartDataIndex++;
                        });

                        //SetICheck();
                        //$('input[type="checkbox"].flat-green,input[type="radio"].flat-green').iCheck({
                        //    checkboxClass: 'icheckbox_flat-green',
                        //    radioClass: 'iradio_square-green'//'iradio_flat-green'
                        //});
                        SetTootip();
                        SetHoverRowColor(sTableID);

                        if ($('input[id$=hidUserID]').val() != "") {
                            $("input[id*=rblUsertype]").prop('disabled', true);
                        }
                    }
                    else {
                        SetRowNoData(sTableID, 5);
                    }

                    HideLoadding();
                    SetEvenTableAfterBind(response, divContiner, function (sIndexCol, sOrderBy, activeIndex, pageindex) { LoadData_Main(sIndexCol, sOrderBy, activeIndex, GridEvent.pageindex, ''); }, sTableID, "", "");
                }

            }, "", {
                itemSearch: Param,
                lstData_Role: arrData,
                sUserID: $('input[id$=hidUserID]').val(),
                sMode: $('input[id$=hidCheckDup]').val(),
            });
        }

        function DeleteDataInTable() {
            var lstDel = $('div#divOperation table tbody tr td input[id*=ckbRow_]:checked');
            if (lstDel.length > 0) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                    arrDelData = [];
                    $('div#divOperation table tbody tr td input[id*=ckbRow_]:checked').each(function () {
                        var row = $(this).attr("data-row");
                        var sVal = (GetValTextBox('txtid_' + row + ''));
                        //var sVal = $(arrTR[i]).find("input[id$=" + txtID_Data + "]").attr("value");
                        arrDelData.push(sVal);
                    });
                    Add_Operation("", "", "", GridEvent.BIND, 'Y');
                    if ($("input[id$=ckbAll]").is(":checked")) {
                        $("input[id$=ckbAll]").iCheck("uncheck");
                    }
                }, "");
            } else {
                DialogWarning(DialogHeader.Warning, DialogMsg.AlertDel);
            }


            //DeleteData("tblData", "ckbRow", "txtid", function (arrID) {
            //    arrDelData = arrID;
            //    Add_Operation("", "", "", GridEvent.BIND, 'Y');
            //    if ($("input[id$=ckbAll]").is(":checked")) {
            //        $("input[id$=ckbAll]").iCheck("uncheck");
            //    }
            //    //Add_Operation();
            //});
        }

        function AddData() {
            if (CheckValidate("Operation")) {
                var sMode = $('input[id$=hidCheckDup]').val();
                var CheckDup = Enumerable.From(arrData).Where(function (w) { return (sMode ? (w.nRoleID != sMode) : true) && w.nRoleID == $ddlUserRole.val() }).ToArray();
                if (CheckDup.length > 0) {
                    DialogWarning(DialogHeader.Warning, "Duplicate Data Role");
                    //DialogError(DialogHeader.Error, "Duplicate Data Role");
                } else {
                    var Ispass = true;
                    var arrPRMS = [];
                    if ($ddlUserRole.val() == 1) { //Type Admin
                        $('table#tblData_admin > tbody > tr > td input[id^=hidMenuID_]').each(function () {
                            var menuID = $(this).attr("data-menu");
                            objPRMS = {
                                nMenuID: menuID,
                                //sMenuName:$('input[name^=rdoPRMS_' + menuID + ']:checked').val(),
                                //sPRMS: GetValRadioListICheck('rdoPRMS_' + menuID + ''),
                                sPermission: $('input[name^=rdoPRMS_' + menuID + ']:checked').val(),
                            }
                            arrPRMS.push(objPRMS);
                        });
                        var arrParam = {
                            nRoleID: $ddlUserRole.val(),
                            sRoleName: $("select[id$=ddlUserRole] option:selected").text(),
                            lstData_Operation: arrOperationAll,
                            lst_Menu: arrPRMS,
                        };
                        if (sMode != "") {
                            var Data = Enumerable.From(arrData).Where(function (w) { return w.nRoleID == sMode }).ToArray();
                            if (Data.length > 0) {
                                Data[0].nRoleID = $ddlUserRole.val();
                                Data[0].sRoleName = $("select[id$=ddlUserRole] option:selected").text();
                                Data[0].lstData_Operation = arrOperationAll;
                                Data[0].lst_Menu = arrPRMS;
                            }
                        } else {
                            arrData.push(arrParam);
                        }

                    } else if ($ddlUserRole.val() == 3 || $ddlUserRole.val() == 4) {// L1 AND L2
                        arrOperationAll = [];
                        arrPRMS = [];
                        var arrParam = {
                            nRoleID: $ddlUserRole.val(),
                            sRoleName: $("select[id$=ddlUserRole] option:selected").text(),
                            lstData_Operation: arrOperationAll,
                            lst_Menu: arrPRMS,
                        };
                        //arrData.push(arrParam);
                        if (sMode != "") {
                            var Data = Enumerable.From(arrData).Where(function (w) { return w.nRoleID == sMode }).ToArray();
                            if (Data.length > 0) {
                                Data[0].nRoleID = $ddlUserRole.val();
                                Data[0].sRoleName = $("select[id$=ddlUserRole] option:selected").text();
                                Data[0].lstData_Operation = arrOperationAll;
                                Data[0].lst_Menu = arrPRMS;
                            }
                        } else {
                            arrData.push(arrParam);
                        }
                    }
                    else { //Type_Other IS Have Operation Data
                        if (arrOperation.length > 0 && arrOperation != null && arrOperation != undefined) {

                            if ($ddlUserRole.val() == 6) { //Is Have Menu _ Admin 
                                $('table#tblData_admin > tbody > tr > td input[id^=hidMenuID_]').each(function () {
                                    var menuID = $(this).attr("data-menu");
                                    objPRMS = {
                                        nMenuID: menuID,
                                        //sMenuName:$('input[name^=rdoPRMS_' + menuID + ']:checked').val(),
                                        //sPRMS: GetValRadioListICheck('rdoPRMS_' + menuID + ''),
                                        sPermission: $('input[name^=rdoPRMS_' + menuID + ']:checked').val(),
                                    }
                                    arrPRMS.push(objPRMS);
                                });
                            }

                            var arrParam = {
                                nRoleID: $ddlUserRole.val(),
                                sRoleName: $("select[id$=ddlUserRole] option:selected").text(),
                                lstData_Operation: arrOperationAll,
                                lst_Menu: arrPRMS,
                            };
                            //arrData.push(arrParam);
                            if (sMode != "") {
                                var Data = Enumerable.From(arrData).Where(function (w) { return w.nRoleID == sMode }).ToArray();
                                if (Data.length > 0) {
                                    Data[0].nRoleID = $ddlUserRole.val();
                                    Data[0].sRoleName = $("select[id$=ddlUserRole] option:selected").text();
                                    Data[0].lstData_Operation = arrOperationAll;
                                    Data[0].lst_Menu = arrPRMS;
                                }
                            } else {
                                arrData.push(arrParam);
                            }


                        } else {
                            DialogWarning(DialogHeader.Warning, "Please select sub-facility");
                            Ispass = false;
                            //DialogError(DialogHeader.Error, "Selected Operation Type");
                        }
                    }
                    if (Ispass) {
                        LoadData_Main("", "", "", GridEvent.BIND, "E");


                        //$("select[id$=ddlOperationType]").html('').multiselect('refresh');
                        arrOperation.length = 0;
                        arrOperationAll.length = 0;
                        $ddlUserRole.val('').prop("disabled", false).change();
                        Add_Operation("", "", "", GridEvent.BIND, 'E');
                        LoadData("", "", "", GridEvent.BIND, '');
                        $('input[id$=hidCheckDup]').val('');
                    }

                }
                UpdateStatusValidate("Operation", "ddlUserRole");
                $(".btnInput").attr("disabled", false);
            } else {
                DialogWarning(DialogHeader.Warning, "Please choose user role");
            }
        }

        function EditData(nID) {
            arrOperation.length = 0;
            arrOperationAll.length = 0;
            $('input[id$=hidCheckDup]').val(nID);
            //$("select[id$=ddlOperationType]").multiselect('selectAll', true).change();
            //$("select[id$=ddlOperationType]").multiselect("clearSelection");
            var lst = Enumerable.From(arrData).Where(function (w) { return w.nRoleID == nID }).ToArray();
            if (nID == 1 || nID == 6) {
                if (lst.length > 0) {
                    arrOperation = lst[0].lstData_Operation;
                    arrMenu = lst[0].lst_Menu;
                    $ddlUserRole.val(nID).prop("disabled", true);
                    $('#divRoleAdmin').show();
                    if (arrOperation.length > 0 && arrOperation != null && arrOperation != undefined) {
                        $ddlPermission.val(arrOperation[0].sPermission).prop("disabled", true);
                    } else {
                        $ddlPermission.val('1').prop("disabled", true);
                    }
                    LoadData("", "", "", GridEvent.BIND, 'Y');
                }
            } else if (nID == 2) {
                if (lst.length > 0) {
                    arrOperation = lst[0].lstData_Operation;
                    $ddlUserRole.val(lst[0].nRoleID).prop("disabled", true);
                    $ddlPermission.val(arrOperation[0].sPermission).prop("disabled", false);
                }
            } else if (nID == 3 || nID == 4) {
                if (lst.length > 0) {
                    $('div[id$=Div_AddOperation_Permission]').hide();
                    arrOperation = lst[0].lstData_Operation;
                    $ddlUserRole.val(lst[0].nRoleID).prop("disabled", true);
                    $ddlPermission.val('1').prop("disabled", true);
                }
            }
            else {
                if (lst.length > 0) {
                    arrOperation = lst[0].lstData_Operation;
                    $ddlUserRole.val(lst[0].nRoleID).prop("disabled", true);
                    $ddlPermission.val('1').prop("disabled", true);
                }
            }
            //$("select[id$=ddlOperationType]").multiselect('select', ['14', '11', '4']);
            $(".btnInput").attr("disabled", true);
            Add_Operation("", "", "", GridEvent.BIND, 'E');
        }

        function SaveToDB() {
            var IsRoleData = arrData.length > 0;
            var IsPass = false;
            var IsPassEmailGC = false;
            var sUserType = GetValRadioListICheck('rblUsertype');
            if (sUserType == "1") {
                var dataValue = {
                    sUserType: GetValRadioListICheck('rblUsertype'),
                    sName: GetValTextBox('txtName'),
                    sSurName: GetValTextBox('txtSurname'),
                    sOrg: GetValTextBox('txtOrg'),
                    sEmail: GetValTextBox('txtEmail'),
                    sUsername: GetValTextBox('txtUsername'),
                    sPassword: GetValTextBox('txtPassword'),
                    sStatus: GetValRadioListICheck('rblStatus'),
                    lstData_Role: arrData,
                    sUserID: GetValTextBox('hidUserID'),
                };
                IsPass = (CheckValidate("divContent"));
                IsPassEmailGC = true;
            } else {
                var dataValue = {
                    sUserType: GetValRadioListICheck('rblUsertype'),
                    sUserCode: GetValTextBox('hdfEmployeeID'),
                    sName: $("input[id$=hdfEmployeeName]").val(),
                    sSurName: $("input[id$=hdfEmployeeLastName]").val(),
                    sOrg: GetValTextBox('txtOrgGc'),
                    sEmail: GetValTextBox('txtEmailGc'),
                    sStatus: GetValRadioListICheck('rblStatus'),
                    lstData_Role: arrData,
                    sUserID: GetValTextBox('hidUserID'),
                };
                IsPass = (CheckValidate($("div[id$=DivGc]").attr("id")));
                IsPassEmailGC = GetValTextBox('txtEmailGc') != "";
                //IsPass = GetValTextBox('hdfEmployeeID') != "";
            }
            if (IsPass && IsRoleData && IsPassEmailGC) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    AjaxCallWebMethod("SaveData", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "admin_user_info_lst.aspx");
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", { data: dataValue });
                }, "");
            } else {
                if (IsPass) {
                    if (!IsPassEmailGC) {
                        DialogWarning(DialogHeader.Warning, "Employee don't have E-mail");
                        //DialogError(DialogHeader.Error, "Employee Don't have E-mail");
                    } else if (!IsRoleData) {
                        DialogWarning(DialogHeader.Warning, "Please add data role");
                        //DialogError(DialogHeader.Error, "Please Add Data Role");
                    }
                }

            }
        }

        function ClearData() {
            UpdateStatusValidateControl("addOperation", $('select[id$=ddlOperationType]'), 'NOT_VALIDATED');
            UpdateStatusValidateControl("addOperation", $('select[id$=ddlFacility]'), 'NOT_VALIDATED');
            UpdateStatusValidateControl("addOperation", $('select[id$=ddlGroupIndicator]'), 'NOT_VALIDATED');
            UpdateStatusValidateControl("addOperation", $('select[id$=ddlPermission]'), 'NOT_VALIDATED');
            $(".btnInput").attr("disabled", false);
            arrOperation.length = 0;
            arrOperationAll.length = 0;
            $ddlUserRole.val('').prop("disabled", false).change();
            $('select[id$=ddlPermission]').val('2').prop("disabled", false);
            $('input[id$=hidCheckDup]').val('');
            //$("select[id$=ddlOperationType]").html('').multiselect('refresh');

            Add_Operation("", "", "", GridEvent.BIND, 'C');
            LoadData("", "", "", GridEvent.BIND, '');
            LoadData_Main("", "", "", GridEvent.BIND, 'E');
        }

        function ResetPass() {
            DialogConfirm(DialogHeader.Confirm, "Do you want to reset password ?", function () {
                LoaddinProcess();
                AjaxCallWebMethod("ResetPass", (function (response) {
                    HideLoadding();
                    if (response.d.Status == SysProcess.SessionExpired) {
                        PopupLogin();
                    } else if (response.d.Status == SysProcess.Success) {
                        DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, response.d.Content);
                    } else {
                        DialogWarning(DialogHeader.Warning, response.d.Msg);
                    }
                }), "", {
                    sUserID: GetValTextBox('hidUserID'),
                });
            });
        }

        function DelData(nID) {
            //LoaddinProcess();
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                var lst_Data = Enumerable.From(arrData).Where(function (w) { return w.nRoleID == nID }).ToArray();
                //var lst_Data = Enumerable(arrData).Where(function (w) { w.nRoleID == nID }).ToArray();
                if (lst_Data.length > 0) {
                    arrData = Enumerable.From(arrData).Where(function (w) { return w.nRoleID != nID }).ToArray();
                }
                LoadData_Main("", "", "", GridEvent.BIND, 'E');
                //DialogSuccess(DialogHeader.Info, DialogMsg.DelComplete);
            });
        }

        function addValidate_Password_notEmpty_Length(maxLength, msgEmpty) {
            return {
                validators: {
                    regexp: {
                        regexp: "^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.*[!@#\$%\^&\*])(?=.{8,})",
                        message: "Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols."//Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols.
                    },
                    callback: {
                        message: (msgEmpty != undefined && msgEmpty != "") ? msgEmpty : "กรุณาระบุ รหัสผ่าน",
                        callback: function (value, validator, $field) {
                            return !(value + "" == "" || value == null || value == undefined);
                        }
                    },
                    stringLength: {
                        max: maxLength,
                        message: 'ขนาดไม่เกิน ' + maxLength + ' อักษร'//msgmaxLength
                    }
                }
            };
        }

        var clearEleCtrl = function () {

            $txtNameGc.val('').keyup();
            //$txtName.val('').keyup();
            $hdfEmployeeName.val('').keyup();
            $hdfEmployeeID.val('').keyup();
            $hdfEmployeeLastName.val('').keyup();
            //$txtUserName.val('').keyup();
            //$txtEmail.val('').keyup();
            //$txtPhone.val('').keyup();
            //$ddlRole.val('');
            //$ddlComp.val('');
            //$cbActivity.prop('checked', false);
        }

        var TextBox_AutoComplete = function (sMethodName, $txt, $hidID, $hidName, $hidLastName, nMinLength, $btn) {
            $txt = $($txt);
            $hidID = $($hidID);
            $hidName = $($hidName);
            $hidLastName = $($hidLastName);
            nMinLength = nMinLength || 0;
            $btn = $($btn);

            var arrData = [];
            $txt
                .autocomplete({
                    source: function (request, response) {
                        LoaddinProcess();
                        AjaxCallWebMethod(sMethodName, function (data) {
                            HideLoadding();
                            arrData = data.d.d.results;
                            arrData = $.map(data.d.d.results, function (item) {
                                return {
                                    value: item.EmployeeID + ' - ' + item.Name,
                                    label: item.EmployeeID + ' - ' + item.Name,
                                    nID: item.EmployeeID,
                                    sTitle: item.ENTitle,
                                    sFristname: item.ENFirstName,
                                    sLastname: item.ENLastName,
                                    sEmail: item.EmailAddress,
                                    sCompany: item.CompanyName,
                                }
                            });
                            response(arrData);
                        }, "", {
                            'sSearch': request.term
                        });
                    },
                    minLength: nMinLength,
                    select: function (event, ui) {
                        //$txt.val(ui.item.sName).keyup();
                        $("input[id$=txtNameGc]").val(ui.item.label).change();
                        $hidID.val(ui.item.nID);
                        $hidName.val(ui.item.sFristname);
                        $hidLastName.val(ui.item.sLastname);
                        //$txtName.val(ui.item.sFullName).keyup();
                        //$txtEmail.val(ui.item.sEmail).keyup();
                        //$txtPhone.val(ui.item.sPhone).keyup();
                        //$txtUserName.val(ui.item.nID).keyup();

                        //$txtEmailGc.val(ui.item.sEmail).keyup();
                        //$txtOrgGc.val(ui.item.sCompany).keyup();

                        //ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtNameGc", objControl.txtbox));
                        //ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtOrgGc", objControl.txtbox));
                        //ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtEmailGc", objControl.txtbox));

                        //ReValidateFieldControl("DivGc", GetElementName("txtNameGc", objControl.txtbox));
                        //$("#DivGc").formValidation('revalidateField', '' + $txtNameGc.attr("name") + '');
                        $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtNameGc]'), "VALIDATED");
                        $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtOrgGc]'), "VALIDATED");
                        $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtEmailGc]'), "VALIDATED");
                    }
                })
                //.focus(function () {
                //    var thisVal = $(this).val();
                //    if (thisVal.length >= nMinLength) { BlockUI(); $(this).autocomplete('search', thisVal) };
                //    $("#DivGc").formValidation('revalidateField', '' + $txtNameGc.attr("name") + '');
                //})
                .change(function () {
                    var thisVal = $(this).val();
                    var d = Enumerable.From(arrData).FirstOrDefault(null, '$.value=="' + thisVal + '"');
                    if (d != null) {
                        //$txt.val(d.sName).keyup();
                        $hidID.val(d.nID).keyup();
                        $hidName.val(d.sFristname).keyup();
                        $hidLastName.val(d.sLastname).keyup();

                        $("input[id$=txtNameGc]").val(d.label);
                        $("input[id$=txtOrgGc]").val(d.sCompany).prop("disabled", true);
                        $("input[id$=txtEmailGc]").val(d.sEmail).prop("disabled", true);
                        //$("input[id$=txtPhone]").val(d.sPhone).keyup();
                        //$txtUserName.val(d.sName).keyup();

                        //$("input[id$=txtEmailGc]").val(d.sEmail).keyup();

                        //ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtNameGc", objControl.txtbox));
                        //ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtOrgGc", objControl.txtbox));
                        // ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtEmailGc", objControl.txtbox));

                        $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtNameGc]'), "VALIDATED");
                        $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtOrgGc]'), "VALIDATED");
                        $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtEmailGc]'), "VALIDATED");

                        //$("input[id$=txtOrgGc]").keyup().prop("disabled", true);
                        //$("input[id$=txtEmailGc]").keyup().prop("disabled", true);

                        //$("#DivGc").formValidation('revalidateField', '' + $txtNameGc.attr("name") + '');
                    }
                    else {
                        $txt.val('');
                        $hidID.val('');
                        $hidName.val('');
                        $hidLastName.val('');
                        //$("input[id$=txtName]").val('').keyup();
                        //$("input[id$=txtEmail]").val('').keyup();
                        //$("input[id$=txtPhone]").val('').keyup();

                        $("input[id$=txtOrgGc]").val('').prop("disabled", false);
                        $("input[id$=txtEmailGc]").val('').prop("disabled", false);
                        $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtNameGc]'), "VALIDATED");
                    }
                });

            //$btn.on('click', function () {
            //    $txt.focus();

            //    $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtNameGc]'), "VALIDATED");
            //    $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtOrgGc]'), "VALIDATED");
            //    $('#' + $("div[id$=DivGc]").attr("id")).formValidation('updateStatus', $('input[id$=txtEmailGc]'), "VALIDATED");

            //    //ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtNameGc", objControl.txtbox));
            //    //ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtOrgGc", objControl.txtbox));
            //    //ReValidateFieldControl($("div[id$=DivGc]").attr("id"), GetElementName("txtEmailGc", objControl.txtbox));
            //});
            //$("#DivGc").formValidation('revalidateField', '' + $txtNameGc.attr("name") + ''); 
        }

        TextBox_AutoComplete("getEmployees", $("input[id$=txtNameGc]"), $("input[id$=hdfEmployeeID]"), $("input[id$=hdfEmployeeName]"), $("input[id$=hdfEmployeeLastName]"), 3, "");

        function addValidate_textAutocomplete(sMsg, sHidTxtID) {
            //sHidTxtID = ตัวที่เก็บค่าเวลาที่เลือกใน ul
            return {
                validators: {
                    callback: {
                        callback: function (value, validator, $field) {
                            var hidValue = sHidTxtID.val();
                            var value = $field.val();
                            var sum = 0;

                            if (value === '') {
                                return {
                                    valid: false,
                                    message: sMsg,
                                };
                            } else if (hidValue == "") {
                                return {
                                    valid: false,
                                    message: sMsg,
                                };
                            }

                            return true;
                        }
                    }
                }
            }
        }

        function onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }

    </script>
</asp:Content>

