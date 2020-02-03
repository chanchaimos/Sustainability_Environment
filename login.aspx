<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=no">

    <%-- Font for Icon --%>
    <link href="Styles/fontawesome-5.3.1/css/all.min.css" rel="stylesheet" />

    <!-- UI -->
    <link href="Styles/ui/jquery-ui%20v.1.10.3.css" rel="stylesheet" />
    <!-- Bootstrap dialog -->
    <link href="Scripts/bootstrap3-dialog-master/css/bootstrap-dialog.css" rel="stylesheet" />
    <!-- iCheck 1.0.1 -->
    <link href="Scripts/iCheck/all.css" rel="stylesheet" />
    <!-- Form Validation -->
    <link href="Scripts/FormValidation/formValidation.min.css" rel="stylesheet" />
    <!-- Datatable -->
    <link href="Scripts/datatables/dataTables.bootstrap.css" rel="stylesheet" />

    <%-- Site CSS --%>
    <link href="Styles/easy.css" rel="stylesheet" />

    <!-- Custom -->
    <link href="Styles/css/Custom/jquery-table-custom.css" rel="stylesheet" />
    <link href="Styles/css/Custom/custom.css" rel="stylesheet" />
    <link href="Styles/css/Custom/AjaxFileupload.css" rel="stylesheet" />
    <link type="text/css" rel="shortcut icon" href="Images/pttg-logo.png" />
    <link href="Styles/master_bootstrap-add.css" rel="stylesheet" />
    <link href="Styles/master_bootstrap-fix.css" rel="stylesheet" />
    <link href="Styles/master_bootstrap-flat.css" rel="stylesheet" />

    <%-- JQuery Core --%>
    <script src="Scripts/jquery-1.11.1.min.js"></script>

    <script src="Scripts/jquery.reDefaultTools.js"></script>

    <%-- Boostrap Core --%>
    <link href="Styles/bootstrap-3.3.7-dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="Styles/bootstrap-3.3.7-dist/js/bootstrap.min.js"></script>
    <script src="Styles/bootstrap-3.3.7-dist/html5shiv.min.js"></script>
    <script src="Styles/bootstrap-3.3.7-dist/respond.min.js"></script>

    <!-- jQuery UI -->
    <script src="Scripts/ui/jquery-ui.min.js"></script>
    <!-- bootstrap dialog -->
    <script src="Scripts/bootstrap3-dialog-master/js/bootstrap-dialog.js"></script>
    <!-- iCheck 1.0.1 -->
    <script src="Scripts/iCheck/icheck.js"></script>
    <!-- Form Validation -->
    <script src="Scripts/FormValidation/formValidation.min.js"></script>
    <script src="Scripts/FormValidation/bootstrap.min.js"></script>
    <!-- InputMask -->
    <script src="Scripts/inputmask/inputmask.js"></script>
    <script src="Scripts/inputmask/inputmask.extensions.js"></script>
    <script src="Scripts/inputmask/inputmask.date.extensions.js"></script>
    <script src="Scripts/inputmask/inputmask.numeric.extensions.js"></script>
    <script src="Scripts/inputmask/inputmask.regex.extensions.js"></script>
    <script src="Scripts/inputmask/jquery.inputmask.js"></script>
    <!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
    <script>
        $.widget.bridge('uibutton', $.ui.button);
    </script>

    <!-- bootstrap-multiselect -->
    <link href="Scripts/bootstrap-multiselect/css/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="Scripts/bootstrap-multiselect/js/bootstrap-multiselect.js"></script>

    <!-- JS LINQ -->
    <script src="Scripts/JSLINQ-master/linq.js"></script>

    <!-- System -->
    <script src="Scripts/System/sysFunction.js"></script>
    <script src="Scripts/System/sysAjaxToolkitUploadFiles.js"></script>
    <script src="Scripts/System/sysAjaxGrid.js"></script>
    <script src="Scripts/JSHeaderGrid/SetDataGridHeader.js"></script>

    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="Images/ico/ptt_weblogo-144.png" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="Images/ico/ptt_weblogo-114.png" />
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="Images/ico/ptt_weblogo-72.png" />
    <link rel="apple-touch-icon-precomposed" sizes="57x57" href="Images/ico/ptt_weblogo-57.png" />
    <link rel="shortcut icon" href="Images/ico/ptt_weblogo-favicon.png" />

    <link href="Styles/_MP_Front.css" rel="stylesheet" />
    <script src="Scripts/_MP_Front.js"></script>

    <%--#SITE_OVERLAY--%>
    <link href="Styles/lds-ring.css" rel="stylesheet" />

    <%--Fixed _MP_Front.css--%>
    <style type="text/css">
        #SITE_CONTENT {
            padding-top: 0;
            padding-bottom: 200px;
            background-image: url(Images/page-login/bg-body.png);
            background-size: auto;
            background-position: center top;
        }

        #FOOT {
            height: 200px;
            background-image: url(Images/page-login/bg-footer.png);
        }
    </style>

    <%--.box-login--%>
    <style type="text/css">
        .box-login {
            display: block;
            padding: 9vh 0;
        }

            .box-login .box-icon, .box-login .box-form {
                float: left;
                display: block;
            }

            .box-login .box-icon {
                width: 48%;
                padding-right: 20px;
                text-align: center;
            }

                .box-login .box-icon > img {
                    width: 421px;
                    transition: width .3s;
                }

            .box-login .box-form {
                width: 52%;
                padding: 50px 35px;
                transition: all .3s;
            }

                .box-login .box-form > .box-title {
                    display: flex;
                    line-height: 1;
                    padding-top: 25px;
                    padding-bottom: 5px;
                    font-size: 32px;
                    font-weight: 500;
                    transition: font-size .3s;
                }

                    .box-login .box-form > .box-title > .title-main {
                        display: block;
                        color: #252675;
                    }

                    .box-login .box-form > .box-title > .title-sub {
                        margin-left: 0.5em;
                        display: block;
                        color: #169cd8;
                    }

                .box-login .box-form > .box-input {
                    margin-top: 40px;
                }

                    .box-login .box-form > .box-input > .box-input-field {
                        display: flex;
                    }

                        .box-login .box-form > .box-input > .box-input-field > .form-group {
                            margin-left: 5px;
                            margin-right: 5px;
                            flex: 1;
                        }

                            .box-login .box-form > .box-input > .box-input-field > .form-group:first-of-type {
                                margin-left: 0;
                            }

                            .box-login .box-form > .box-input > .box-input-field > .form-group:last-of-type {
                                margin-right: 0;
                            }

                    .box-login .box-form > .box-input > .box-btn {
                        position: relative;
                    }

                        .box-login .box-form > .box-input > .box-btn > button {
                            padding: 5px 10px;
                            background-color: transparent;
                            border: 2px solid transparent;
                            border-radius: 5px;
                            margin-bottom: 5px;
                            transition: all .3s;
                        }

                            .box-login .box-form > .box-input > .box-btn > button.btn-login {
                                color: #248ebc;
                                border-color: #44a3d4;
                            }

                                .box-login .box-form > .box-input > .box-btn > button.btn-login:hover {
                                    background-color: rgba(68, 163, 212, 0.25);
                                }

                            .box-login .box-form > .box-input > .box-btn > button.btn-register {
                                color: #656262;
                                border-color: #a4b3bc;
                            }

                                .box-login .box-form > .box-input > .box-btn > button.btn-register:hover {
                                    background-color: rgba(164, 179, 188, 0.25);
                                }

                            .box-login .box-form > .box-input > .box-btn > button:focus {
                                color: #398439;
                                border-color: #5cb85c;
                                outline: 0;
                                -webkit-box-shadow: 0px 0px 10px 2px rgba(92,184,92,0.5);
                                -moz-box-shadow: 0px 0px 10px 2px rgba(92,184,92,0.5);
                                box-shadow: 0px 0px 10px 2px rgba(92,184,92,0.5);
                            }

                                .box-login .box-form > .box-input > .box-btn > button:focus:hover {
                                    background-color: rgba(92, 184, 92, 0.25);
                                }

                        .box-login .box-form > .box-input > .box-btn > .box-btn-link {
                            float: right;
                            padding: 5px;
                        }

                            .box-login .box-form > .box-input > .box-btn > .box-btn-link > a {
                                display: block;
                                color: #ffffff;
                                font-size: 12px;
                                line-height: 1.1;
                                text-shadow: 0px 0px 2px #333333;
                            }

        @media (max-width:991px) {
            .box-login .box-icon > img {
                width: 345px;
            }

            .box-login .box-form {
                padding: 40px 20px;
            }
        }

        @media (max-width:767px) {
            .box-login {
                text-align: center;
            }

                .box-login .box-icon {
                    display: none;
                }

                .box-login .box-form {
                    float: none;
                    display: inline-block;
                    width: auto;
                    max-width: 100%;
                    background-color: rgba(255,255,255,0.5);
                }

                    .box-login .box-form > .box-title {
                        display: inline-flex;
                        font-size: 28px;
                        text-align: left;
                    }

                    .box-login .box-form > .box-input > .box-input-field {
                        display: block;
                    }

                        .box-login .box-form > .box-input > .box-input-field > .form-group {
                            margin-left: 0;
                            margin-right: 0;
                        }

                    .box-login .box-form > .box-input > .box-btn {
                        text-align: left;
                    }

                        .box-login .box-form > .box-input > .box-btn > .box-btn-link {
                            padding-right: 0;
                            text-align: left;
                        }

                            .box-login .box-form > .box-input > .box-btn > .box-btn-link > a {
                                color: #337ab7;
                                text-shadow: none;
                            }
        }
    </style>

    <script type="text/javascript">
        var $txtEmailForgot = $("input[id$=txtEmailForgot]");
        var arrDataRole = [];
        $(function () {
            var objValidate = {};
            objValidate[GetElementName("txtUsername", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Username");
            objValidate[GetElementName("txtPassword", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Password");
            BindValidate("DivLogin", objValidate);

            var objValidate1 = {};
            objValidate1[GetElementName("txtEmail", objControl.txtbox)] = addValidateEmail_notEmpty(DialogMsg.Specify + " Email");
            objValidate1[GetElementName("txtUsernameForget", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Username");
            BindValidate("exampleModalCenter", objValidate1);

            var UserAD = GetValTextBox('hdfUserAD');
            if (UserAD != "") {
                Login('AD');
            }
            SetEventKeypressOnEnter("body", function () { Login(''); })
            $('input[id$=txtUsername]').keydown(function (e) {
                if (e.which == 13) {
                    $('input[id$=txtPassword]').focus();
                    return false;
                }
                else if (e.which == 220) return false;
            });

            $('input[id$=txtPassword]').keydown(function (e) {
                if (e.which == 13) {
                    $('button#btnLogin').click();
                    return false;
                }
                else if (e.which == 220) return false;
            });
        });

        function Login(sMode) {
            BootstrapDialog.closeAll();
            var sUsername = GetValTextBox('txtUsername') != "" ? GetValTextBox('txtUsername') : GetValTextBox('hdfUserAD');
            if ((sMode == "" ? CheckValidate("DivLogin") : true)) {
                LoaddinProcess();
                AjaxCallWebMethod("Login", function (response) {
                    if (response.d.Status == SysProcess.Success) {
                        window.location = "epi_mytask.aspx";
                    } else if (response.d.Status == SysProcess.Failed) {
                        if (response.d.Msg == "muti") {
                            arrDataRole = response.d.TDataRole;
                            //var ItemRole = respone.d.lstData;
                            var divData = '<table class="table dataTable table-bordered table-responsive table-hover">';
                            divData += '<thead><tr><th class="dt-head-center">Role Name</th></tr></thead>';
                            divData += '<tbody>'
                            var nRow = 1;
                            for (var i = 0; i < arrDataRole.length; i++) {
                                divData += '<tr style="cursor:pointer" onclick="callRedirect(' + response.d.nUserID + ',' + arrDataRole[i].nRoleID + ')"><td class="dt-body-left">' + nRow + '. ' + arrDataRole[i].sRoleName + '</td></tr>';
                                nRow++;
                            }
                            divData += '</tbody></table>';

                            $("#divMPPopContent").html(divData);
                            $("#MPhTitle").html("Role");
                            $("#MPPopContent").modal();
                            $('#MPPopContent').on('hidden.bs.modal', function (e) {
                                $("#divMPPopContent").html("");
                            });

                            //DialogError(DialogHeader.Warning, response.d.Msg);
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                        HideLoadding();
                    }
                }, "", { sUserName: sUsername, sPassword: GetValTextBox("txtPassword"), sMode: sMode });
            }

            //} else {
            //    DialogError('Error', 'Please specify Employee Username.');
            // }
        }

        function handleEnter(field, e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                SubmitUsername();
            }
            else
                return false;
        }

        function callRedirect(nUserID, nRoleID) {
            $('#PopSetContent').modal('toggle');
            LoaddinProcess();
            AjaxCallWebMethod("SelectedRole", function (response) {
                HideLoadding();
                if (response.d.Status == SysProcess.SessionExpired) {
                    HideLoadding();
                    PopupLogin();
                }
                else if (response.d.Status == SysProcess.Success) {
                    HideLoadding();
                    window.location = response.d.Content;
                }
                else {
                    HideLoadding();
                    DialogWarning(DialogHeader.Warning, response.d.Msg);
                }
            }, "", {
                sUserID: nUserID,
                sRoleID: nRoleID,
            });
        }

        function ForgotPassword() {
            var Ispass = CheckValidate("exampleModalCenter");
            if (Ispass) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    LoaddinProcess();
                    AjaxCallWebMethod("ForgetPassword", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.Success) {
                            DialogSuccessRedirect(DialogHeader.Confirm, DialogMsg.SaveComplete, "login.aspx");
                            // window.location = "login.aspx";
                        }
                        else {
                            HideLoadding();
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", {
                        sEmail: GetValTextBox('txtEmail'),
                        sUsername: GetValTextBox('txtUsernameForget'),
                    });
                }, "");
            }

        }

    </script>

</head>
<body id="bodyMain" runat="server">
    <form id="form1" runat="server">
        <!-- Modal -->
        <div id="MPPopContent" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header csetpop">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="MPhTitle"></h4>
                    </div>
                    <div class="modal-body">
                        <div id="divMPPopContent"></div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header ">
                        <div class="text-center col-12">
                            <h3><i class="fa fa-lock fa-4x"></i></h3>
                            <h2 class="text-center">Forgot Password?</h2>
                            <p>You can reset your password here.</p>
                        </div>

                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="control-label col-xs-12 col-md-3 text-left-sm">E-mail <span class="text-red">*</span> :</label>
                            <div class="col-xs-12 col-md-9" style="padding-bottom: 7px">
                                <div class="input-group">
                                    <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-xs-12 col-md-3 text-left-sm">Username <span class="text-red">*</span> :</label>
                            <div class="col-xs-12 col-md-9" style="padding-bottom: 7px">
                                <div class="input-group">
                                    <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                                    <asp:TextBox ID="txtUsernameForget" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <button class="btn btn-lg btn-primary btn-block" type="button" onclick="ForgotPassword()" id="btnForgot">Send My Password</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- The Modal -->
        </div>


        <asp:HiddenField runat="server" ID="hdfUserAD" />
        <div id="SITE_CONTENT">
            <div class="box-login">
                <div class="container">
                    <div class="box-icon">
                        <img src="Images/page-login/icon-page.png" />
                    </div>
                    <div class="box-form">
                        <div class="box-logo-pttgc">
                            <img src="Images/page-login/PTTGC-logo-home.png" style="width: 192px; height: 69px" />
                        </div>
                        <div class="box-title">
                            <div class="title-main">Environmental Performance</div>
                            <div class="title-sub">System</div>
                        </div>
                        <div class="box-input" id="DivLogin">
                            <div class="box-input-field">
                                <div class="form-group">
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password"></asp:TextBox>
                                </div>
                            </div>
                            <div class="box-btn">
                                <button id="btnLogin" type="button" class="btn-login" onclick="Login('')">
                                    <i class="fa fa-sign-in-alt"></i>&nbsp;Login
                                </button>
                                <%--                                <button id="btnRegister" type="button" class="btn-register">
                                    <i class="fa fa-user-plus"></i>&nbsp;Register
                                </button>--%>
                                <div class="box-btn-link">
                                    <a style="cursor: pointer" href="#exampleModalCenter" data-toggle="modal" data-target="#exampleModalCenter"><i class="fa fa-caret-right"></i>&nbsp;Forget password</a>
                                    <%--<a><i class="fa fa-caret-right"></i>&nbsp;Contact us</a>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="FOOT">
                <div class="container">
                    <div class="footer-area">
                        <div class="footer-pttgc">
                            <div class="footer-pttgc-icon">
                                <img src="Images/logo/logo-pttgc-new.png" />
                            </div>
                            <div class="footer-pttgc-info">
                                <div class="footer-pttgc-copyright">
                                    Copyright &copy; 2018,<br />
                                    PTT Global Chemical Public Company Limited All rights reserved
                                </div>
                                <div class="footer-pttgc-about">
                                    <div class="footer-pttgc-about-title">PTT Global Chemical Public Company Limited</div>
                                    <div class="footer-pttgc-about-address">
                                        555/1 Energy Complex Building A, 14th - 18th Floor, Vibhavadi Rangsit Road, Chatuchak, Chatuchak, Bangkok 10900 Thailand.
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="footer-icon">
                            <img src="Images/icon-sshe.png" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="SITE_OVERLAY">
            <div class="loader">
                <div class="lds-ring">
                    <div></div>
                    <div></div>
                    <div></div>
                    <div></div>
                </div>
                <div class="loader-text">Loading</div>
            </div>
        </div>
    </form>
</body>
</html>
