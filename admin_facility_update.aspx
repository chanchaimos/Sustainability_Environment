<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_facility_update.aspx.cs" Inherits="admin_facility_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .flat-green:not(.radio) label {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

        @media (min-width: 424px ) and (max-width:767px) {
            i[data-fv-icon-for="cblOperationTypeGC"] {
                display: none !important;
            }
        }
        /*@media (min-width: 486px) {
            i[data-fv-icon-for="cblOperationTypeGC"] {
                left: 280px;
                top: -40px;
            }
        }*/
        @media (min-width: 768px) {
            i[data-fv-icon-for="cblOperationTypeGC"] {
                left: 540px;
                top: -40px;
            }
        }

        @media (min-width: 992px) {
            i[data-fv-icon-for="cblOperationTypeGC"] {
                left: 540px;
                top: -40px;
            }
        }

        @media (min-width: 1200px) {
            i[data-fv-icon-for="cblOperationTypeGC"] {
                left: 680px;
                top: -40px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp<asp:Literal runat="server" ID="ltrHeader"></asp:Literal></div>
        <div class="panel-body" id="divContent">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Facility Name <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtFacilityName" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group contentGC">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Reference PTT Facility :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:DropDownList runat="server" ID="ddlFacilityPTT" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Reference PTT Operation Type <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:DropDownList runat="server" ID="ddlOperationType" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group cRowMappingPTTCode">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Mapping PTT Code <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtMapPTTCode" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <%--                <div class="form-group contentGC">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">GC Operation Type <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:CheckBox runat="server" ID="cbAll" CssClass="flat-green" Text="All" />
                        <div class="row">
                            <asp:CheckBoxList runat="server" ID="cblOperationTypeGC" CssClass="flat-green" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
                        </div>

                    </div>
                </div>--%>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Description :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Satus <span class="text-red">*</span> :</label>
                    <div class="col-xs-6 col-md-3">
                        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Y" Text="Active" Selected="True" class="flat-green radio radio-inline"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Inactive" class="flat-green radio radio-inline"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-xs-6 col-md-6">
                        <div class="form-group">
                            <div class="col-xs-12">
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" MaxLength="150" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer text-center">
            <button type="button" class="btn btn-primary" onclick="saveToDB()"><i class="fa fa-save"></i>&nbsp;Save</button>
            <a class="btn btn-default" id="aCancel">Cancel</a>
        </div>
    </div>
    <%--ตัวแปรเก็บ Encrypt ID สำหรับ Redirect กลับไปหน้า list--%>
    <asp:HiddenField runat="server" ID="hdfReturnStr" />
    <%--ใช้เช็คว่าเป็นของ GC หรือ PTT--%>
    <asp:HiddenField runat="server" ID="hdfComType" />
    <%--ID Company--%>
    <asp:HiddenField runat="server" ID="hdfComID" />
    <%--ID Facility--%>
    <asp:HiddenField runat="server" ID="hdfFacilityID" />
    <%--Check Mode--%>
    <asp:HiddenField runat="server" ID="hdfIsNew" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        $(function () {
            $("a[id$=aCancel]").attr("href", "admin_facility_lst.aspx?" + $("input[id$=hdfReturnStr]").val());
            if ($("input[id$=hdfComType]").val() == "GC") {
                if ($("select[id$=ddlFacilityPTT]").val() != "") {
                    $("select[id$=ddlOperationType]").prop("disabled", true);
                } else {
                    $("select[id$=ddlOperationType]").prop("disabled", false);
                }

                $(".contentGC").show();
                $(".cRowMappingPTTCode").hide();
            } else {
                $(".contentGC").hide();
                $(".cRowMappingPTTCode").show();
                $("select[id$=ddlOperationType]").prop("disabled", false);
            }
            setValidate();
            setEventCtrl();
        })

        function setValidate() {
            //$("input[id*=cblOperationTypeGC]").attr("name", "cblOperationTypeGC");
            var objValidate = {};
            objValidate[GetElementName("txtFacilityName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Facility Name");
            objValidate[GetElementName("txtRemark", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Remark");
            objValidate[GetElementName("txtMapPTTCode", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Mapping PTT Code");
            objValidate[GetElementName("ddlOperationType", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Operation Type");
            if ($("input[id$=hdfComType]").val() == "GC") {
                // objValidate[GetElementName("ddlFacilityPTT", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Operation Type");
                //objValidate["cblOperationTypeGC"] = addValidateCheckbox_notEmpty(DialogMsg.Specify + " GC Operation Type");
            }
            BindValidate("divContent", objValidate);
            //$('i[data-fv-icon-for="cblOperationTypeGC"]').css('left', '700px').css('top', '-20px');

            $('input[id*=rblStatus]').on('ifChecked', function (event) {
                if ($(this).val() == "N") {
                    Input("txtRemark").prop("disabled", false);
                }
                else {
                    UpdateStatusValidateControl("divContent", GetElementName("txtRemark", objControl.txtbox), ValidateProp.Status_NOT_VALIDATED);
                    Input("txtRemark").prop("disabled", true);
                }
            });
        }
        function setEventCtrl() {
            // ---- set Event GC Operation Type checkobox
            //var nCheckLength = $("input[id*=cblOperationTypeGC]:checked").length;
            //var nUnCheckLength = $("input[id*=cblOperationTypeGC]").length;
            //if (nCheckLength == nUnCheckLength) {
            //    $("input[id$=cbAll]").iCheck("check");
            //} else {
            //    $("input[id$=cbAll]").iCheck("uncheck");
            //}
            //$("input[id$=cbAll]").on("ifClicked", function (e) {
            //    if (e.currentTarget.checked) {
            //        $("input[id*=cblOperationTypeGC]").iCheck("uncheck");
            //    } else {
            //        $("input[id*=cblOperationTypeGC]").iCheck("check");
            //    }
            //})
            //$("input[id*=cblOperationTypeGC]").on("ifChanged", function () {
            //    var nCheckLength = $("input[id*=cblOperationTypeGC]:checked").length;
            //    var nUnCheckLength = $("input[id*=cblOperationTypeGC]").length;
            //    if (nCheckLength == nUnCheckLength) {
            //        $("input[id$=cbAll]").iCheck("check");
            //    } else {
            //        $("input[id$=cbAll]").iCheck("uncheck");
            //    }
            //    ReValidateFieldControl("divContent", GetElementNameICheck("cblOperationTypeGC"));
            //})

            // ---- set Event PTT Facility dropdownlist
            $("select[id$=ddlFacilityPTT]").on("change", function () {
                if ($(this).val() != "") {
                    AjaxCallWebMethod("getOperationTypePTT", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            $("select[id$=ddlOperationType]").val(response.d.Content);
                            $("select[id$=ddlOperationType]").prop("disabled", true);
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", { sFacilityPTT_ID: $(this).val() });
                } else {
                    $("select[id$=ddlOperationType]").val("");
                    $("select[id$=ddlOperationType]").prop("disabled", false);
                }
            });

        }
        function addValidateCheckbox_notEmpty(msg) {
            return {
                validators: {
                    choice: {
                        min: 1,
                        message: msg
                    }
                }
            };
        }
        function saveToDB() {

            if (CheckValidate("divContent")) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    //var lstOprtGC_ID = $.map($("input[id*=cblOperationTypeGC]:checked"), function (el, i) {
                    //    return el.value;
                    //})
                    var dataValue = {
                        sComID: GetValTextBox("hdfComID"),
                        sFacilityID: GetValTextBox("hdfFacilityID"),
                        sFacilityName: GetValTextBox("txtFacilityName").trim(),
                        sFacilityPTT_ID: GetValDropdown("ddlFacilityPTT"),
                        sOperationTypePTT_ID: GetValDropdown("ddlOperationType"),
                        //lstOperationTypeGC_ID: lstOprtGC_ID,
                        sDescription: GetValTextArea("txtDesc"),
                        sActive: GetValRadioListICheck("rblStatus"),
                        sRemark: GetValTextBox("txtRemark"),
                        sComType: GetValTextBox("hdfComType"),
                        IsNew: GetValTextBox("hdfIsNew") == "C",
                        sMapPTTCode: GetValTextBox("txtMapPTTCode")
                    };
                    AjaxCallWebMethod("saveToDB", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, $("a[id$=aCancel]").attr("href"));
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", { dataValue: dataValue });
                }, "");
            }
        }
    </script>
</asp:Content>

