<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="user_profile_update.aspx.cs" Inherits="user_profile_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <i class="fa fa-table"></i>&nbsp;Profile / Edit
        </div>
        <div class="panel-body" id="divContent">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">User Type :</label>
                        <div class="col-xs-7 col-md-4">
                            <asp:RadioButtonList ID="rblUsertype" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0" Text="GC Employee" Selected="True" class="flat-green radio radio-inline"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Contract" class="flat-green radio radio-inline"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div id="DivContract" runat="server">
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Name <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Surname <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Org <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtOrg" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">E-mail <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Username <span class="text-red">*</span> :</label>
                        <div class="col-xs-7 col-md-4">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <%--                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Old Password <span class="text-red">*</span> :</label>
                        <div class="col-xs-7 col-md-4">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtOldPass" TextMode="Password" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                    </div>--%>
                </div>
                <div id="DivPass">
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">New Password :</label>
                        <div class="col-xs-7 col-md-4">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtNewpass" TextMode="Password" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Confirm Password :</label>
                        <div class="col-xs-7 col-md-4">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtConfirmPass" TextMode="Password" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <%--                    <div class="form-group">
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
                                    <button type="button" class="btn btn-primary" onclick="ResetPass()">&nbsp;Reset</button>
                                </div>
                            </div>
                        </div>
                    </div>--%>

                <div id="DivGc" runat="server">
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Name <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <%--<asp:TextBox ID="txtNameGc" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>--%>

                                <asp:TextBox runat="server" ID="txtNameGc" CssClass="form-control" placeholder="Search from EmployeeID,EmployeeName"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdfEmployeeName" />
                                <asp:HiddenField runat="server" ID="hdfEmployeeID" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">Org <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtOrgGc" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-xs-12 col-md-3 text-left-sm">E-mail <span class="text-red">*</span> :</label>
                        <div class="col-xs-12 col-md-9">
                            <div class="input-group">
                                <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                <asp:TextBox ID="txtEmailGc" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <%--<div class="registrationFormAlert" id="divCheckPasswordMatch"></div>--%>
                </div>
            </div>
        </div>
        <div class="panel-footer text-center">
            <button type="button" class="btn btn-primary btnInput" onclick="SaveToDB()"><i class="fa fa-save"></i>&nbsp;Save</button>
            <a class="btn btn-default btnInput" href="epi_mytask.aspx">Cancel</a>
        </div>
    </div>
    <asp:HiddenField ID="hidUserID" runat="server" />
    <asp:HiddenField ID="hidEncryptUserID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $txtNewpass = $('input[id$=txtNewpass]');
        var $txtConfirmPass = $('input[id$=txtConfirmPass]');
        $(function () {
            var objValidate = {}; //Validate Employee_Other
            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Name");
            objValidate[GetElementName("txtSurname", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Surname");
            objValidate[GetElementName("txtOrg", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Org");
            objValidate[GetElementName("txtEmail", objControl.txtbox)] = addValidateEmail_notEmpty(DialogMsg.Specify + " Email");
            objValidate[GetElementName("txtUsername", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Username");
            BindValidate_Custom($('div[id$=DivContract]'), objValidate);
            var objValidate2 = {}; //Validate Employee_GC
            objValidate2[GetElementName("txtNameGc", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Name");
            objValidate2[GetElementName("txtOrgGc", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Org");
            objValidate2[GetElementName("txtEmailGc", objControl.txtbox)] = addValidateEmail_notEmpty(DialogMsg.Specify + " Email");
            BindValidate("DivGc", objValidate2);
            var objValidate3 = {}; //Validate Password
            objValidate3[GetElementName("txtNewpass", objControl.txtbox)] = addValidate_Password_notEmpty_Length(20, "");
            objValidate3[GetElementName("txtConfirmPass", objControl.txtbox)] = addValidatePassword_notEmpty_Confirm('txtNewpass');
            BindValidate("DivPass", objValidate3);

            if ($('input[id$=hidUserID]').val() != "") { //Disabled_Radio_Type
                $("input[id*=rblUsertype]").prop('disabled', true);
            }


            $txtNewpass.change(function () {
                if ($txtNewpass.val() == "") {
                    $('#DivPass').formValidation('updateStatus', $txtNewpass, 'NOT_VALIDATED');
                } else {
                    ReValidateFieldControl("DivPass", $('input[id$=txtConfirmPass]'));
                }
            }).blur(function () {
                if ($txtNewpass.val() == "") {
                    $('#DivPass').formValidation('updateStatus', $txtNewpass, 'NOT_VALIDATED');
                }

            });
            $txtConfirmPass.change(function () {
                if ($txtConfirmPass.val() == "") {
                    $('#DivPass').formValidation('updateStatus', $txtConfirmPass, 'NOT_VALIDATED');
                }
            }).blur(function () {
                if ($txtConfirmPass.val() == "") {
                    $('#DivPass').formValidation('updateStatus', $txtConfirmPass, 'NOT_VALIDATED');
                }
            });
        });
        function SaveToDB() {
            var IspassPassword = false;
            var sUserType = GetValRadioListICheck('rblUsertype');
            if (GetValTextBox('txtNewpass') != "" && GetValTextBox('txtConfirmPass') != "") {
                IspassPassword = CheckValidate('DivPass');
            } else if (GetValTextBox('txtNewpass') == "" && GetValTextBox('txtConfirmPass') == "") {
                IspassPassword = true;
            } else {
                if (GetValTextBox('txtNewpass') != "" && GetValTextBox('txtConfirmPass') == "") {
                    $('#DivPass').formValidation('updateStatus', $txtConfirmPass, 'INVALID');
                }
            }
            if (sUserType == "1") {
                var dataValue = {
                    //sUserType: GetValRadioListICheck('rblUsertype'),
                    sName: GetValTextBox('txtName'),
                    sSurName: GetValTextBox('txtSurname'),
                    sOrg: GetValTextBox('txtOrg'),
                    sEmail: GetValTextBox('txtEmail'),
                    sUsername: GetValTextBox('txtUsername'),
                    sPassword: GetValTextBox('txtNewpass'),
                    //sStatus: GetValRadioListICheck('rblStatus'),
                    //lstData_Role: arrData,
                    sUserID: GetValTextBox('hidUserID'),
                };
                IsPass = (CheckValidate_custom($('div[id$=DivContract]')));
            } else {
                var dataValue = {
                    //sUserType: GetValRadioListICheck('rblUsertype'),
                    sUserCode: GetValTextBox('hdfEmployeeID'),
                    sName: $("input[id$=hdfEmployeeName]").val(),
                    sOrg: GetValTextBox('txtOrgGc'),
                    sEmail: GetValTextBox('txtEmailGc'),
                    //sStatus: GetValRadioListICheck('rblStatus'),
                    //lstData_Role: arrData,
                    sUserID: GetValTextBox('hidUserID'),
                };
                IsPass = GetValTextBox('hidEncryptUserID') != "";
            }
            if (IsPass && IspassPassword) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    LoaddinProcess();
                    AjaxCallWebMethod("SaveData", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "epi_mytask.aspx");
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", {
                        data: dataValue,
                    });
                }, "");
            }
        }

    </script>

    <script type="text/javascript">
        function addValidate_Password_notEmpty_Length(maxLength, msgEmpty) {
            return {
                validators: {
                    regexp: {
                        regexp: "^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.*[!@#\$%\^&\*])(?=.{8,})",
                        message: "Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols."//Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols.
                    },
                    callback: {
                        //message: (msgEmpty != undefined && msgEmpty != "") ? msgEmpty : "กรุณาระบุ รหัสผ่าน",
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
        function addValidatePassword_notEmpty_Confirm(txt1) {
            return {
                validators: {
                    callback: {
                        callback: function (value, validator, $field) {
                            var value = $field.val();
                            if (value === '') {
                                return {
                                    valid: false,
                                    //message: 'ระบุ รหัสผ่าน'
                                };
                            }

                            // Check the password strength
                            if (value.length < 8) {
                                return {
                                    valid: false,
                                    message: 'The password confirmation does not match'
                                    // message: 'ระบุ รหัสผ่านอย่างน้อย 8 ตัวอักษร'
                                    //message: 'Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols.'
                                };
                            }

                            //// The password doesn't contain any uppercase character
                            //if (value === value.toLowerCase()) {
                            //    return {
                            //        valid: false,
                            //        message: 'ระบุ ตัวอักษรตัวพิมพ์ใหญ่อย่างน้อย 1 ตัวอักษร'
                            //    }
                            //}

                            // The password doesn't contain any uppercase character
                            if (value === value.toUpperCase()) {
                                return {
                                    valid: false,
                                    message: 'The password confirmation does not match'
                                    //message: 'Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols.'
                                    //message: 'ระบุ ตัวอักษรตัวพิมพ์เล็กอย่างน้อย 1 ตัวอักษร'
                                }
                            }

                            // The password doesn't contain any digit
                            if (value.search(/[0-9]/) < 0) {
                                return {
                                    valid: false,
                                    message: 'The password confirmation does not match'
                                    //message: 'Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols.'
                                    //message: 'ระบุ ตัวเลขอย่างน้อย 1 ตัว'
                                }
                            }

                            // The password doesn't contain any digit
                            if (value != $("input[name$=" + txt1 + "]").val()) {
                                return {
                                    valid: false,
                                    //message: 'ระบุ รหัสผ่านไม่ตรงกัน'
                                    message: 'The password confirmation does not match'
                                }
                            }
                            return true;
                        }
                    }
                }
            };
        }
        function CheckValidate_custom(sContainer) {
            var isValid = $(sContainer).data('formValidation').validate().isValid();
            if (!isValid) {
                ScrollTopToElements($($("div#" + sContainer).data('formValidation').$invalidFields[0]).attr("id"));//$("div#" + sContainer).data('formValidation').$invalidFields[0].focus();
            }
            return isValid;
        }
        function BindValidate_Custom(sContainer, objValidate) {
            $(sContainer).formValidation({
                framework: 'bootstrap',
                err: {
                    //container: 'tooltip'
                },
                icon: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: objValidate,
                autoFocus: true
            }).on('err.validator.fv', function (e, data) {
                data.element
                       .data('fv.messages')
                       // Hide all the messages
                       .find('.help-block[data-fv-for="' + data.field + '"]').hide()
                       // Show only message associated with current validator
                       .filter('[data-fv-validator="' + data.validator + '"]').show();
            }).on('err.field.fv', function (e, data) {
                data.fv.disableSubmitButtons(false);
            }).on('success.field.fv', function (e, data) {
                data.fv.disableSubmitButtons(false);
            });
        }
    </script>
</asp:Content>

