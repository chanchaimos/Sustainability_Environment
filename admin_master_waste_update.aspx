<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_master_waste_update.aspx.cs" Inherits="admin_master_waste_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-edit"></i>&nbsp;Create / Edit</div>
        <div class="panel-body" id="divContent">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Disposal Method <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-6">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:DropDownList runat="server" ID="ddlDisposal" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Disposal Code <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-2">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Disposal Name <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="500"></asp:TextBox>
                        </div>
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
                </div>
            </div>
        </div>
        <div class="panel-footer text-center">
            <button type="button" class="btn btn-primary" onclick="SaveData()"><i class="fa fa-save"></i>&nbsp;Save</button>
            <a class="btn btn-default" href="admin_master_waste_lst.aspx">Cancel</a>
        </div>
    </div>
    <asp:HiddenField ID="hdfPrmsMenu" runat="server" />
    <asp:HiddenField ID="hdfWasteID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        $(function () {

            var objValidate = {};
            objValidate[GetElementName("ddlDisposal", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Disposal Type");
            objValidate[GetElementName("txtCode", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Disposal Code");
            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Disposal Name");
            BindValidate("divContent", objValidate);
        });

        function SaveData() {
            var Ispass = CheckValidate("divContent");
            if (Ispass) {
                var dataValue = {
                    sDisposalType: GetValDropdown('ddlDisposal'),
                    sCode: GetValTextBox('txtCode'),
                    sName: GetValTextBox('txtName'),
                    sStatus: GetValRadioListICheck('rblStatus'),
                    sWasteID: GetValTextBox('hdfWasteID'),
                };
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    AjaxCallWebMethod("SaveData", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "admin_master_waste_lst.aspx");
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", { data: dataValue });
                }, "");
            }
        }
    </script>
</asp:Content>

