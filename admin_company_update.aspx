<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_company_update.aspx.cs" Inherits="admin_company_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-edit"></i>&nbsp;Create / Edit</div>
        <div class="panel-body" id="divContent1">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Company Name <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">SAP Code <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-2">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="3"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Description :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
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
            <a class="btn btn-default" href="admin_company_lst.aspx">Cancel</a>
        </div>
    </div>
    <asp:HiddenField ID="hdfEncryptCompanyID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        $(function () {
            var objValidate = {};
            objValidate[GetElementName("txtCompanyName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Company Name");
            objValidate[GetElementName("txtCode", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " SAP Code");
            objValidate[GetElementName("txtRemark", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Remark");
            BindValidate("divContent1", objValidate);

            $('input[id*=rblStatus]').on('ifChecked', function (event) {
                if ($(this).val() == "N") {
                    Input("txtRemark").prop("disabled", false);
                }
                else {
                    UpdateStatusValidateControl("divContent1", GetElementName("txtRemark", objControl.txtbox), ValidateProp.Status_NOT_VALIDATED);
                    Input("txtRemark").prop("disabled", true);
                }
            });
        });

        function SaveData() {
            if (CheckValidate("divContent1")) {
                var dataValue = {
                    sCompID: Input("hdfEncryptCompanyID").val(),
                    sCompName: GetValTextBox("txtCompanyName"),
                    sCode: GetValTextBox("txtCode"),
                    sDesc: GetValTextArea("txtDesc"),
                    sStatus: GetValRadioListICheck("rblStatus"),
                    sRemark: GetValTextBox("txtRemark")
                };
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    AjaxCallWebMethod("SaveData", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "admin_company_lst.aspx");
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", { data: dataValue });
                }, "");
            }
        }
    </script>
</asp:Content>

