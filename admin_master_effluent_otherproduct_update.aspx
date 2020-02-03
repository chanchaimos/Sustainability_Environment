<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_master_effluent_otherproduct_update.aspx.cs" Inherits="admin_master_effluent_otherproduct_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-edit"></i>&nbsp;Create / Edit</div>
        <div class="panel-body" id="divContent">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Product Name <span class="text-red">*</span> :</label>
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
            <a class="btn btn-default" href="admin_master_effluent_otherproduct_lst.aspx">Cancel</a>
        </div>
    </div>
    <asp:HiddenField ID="hdfPrmsMenu" runat="server" />
    <asp:HiddenField ID="hdfProductID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        $(function () {

            var objValidate = {};
            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Product Name");
            BindValidate("divContent", objValidate);
        });

        function SaveData() {
            var Ispass = CheckValidate("divContent");
            if (Ispass) {
                var dataValue = {
                    sName: GetValTextBox('txtName'),
                    sStatus: GetValRadioListICheck('rblStatus'),
                    sProductID: GetValTextBox('hdfProductID'),
                };
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    AjaxCallWebMethod("SaveData", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "admin_master_effluent_otherproduct_lst.aspx");
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", { data: dataValue });
                }, "");
            }
        }
    </script>
</asp:Content>

