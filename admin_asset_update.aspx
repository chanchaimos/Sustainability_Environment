<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_asset_update.aspx.cs" Inherits="admin_asset_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .flat-green:not(.radio) label {
            padding-left: 5px !important;
            padding-right: 5px !important;
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
                            <asp:TextBox ID="txtFacilityName" runat="server" Enabled="false" CssClass="form-control" MaxLength="1000"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Option <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:RadioButtonList ID="rblOption" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="P" Text="SAP Master" Selected="True" class="flat-green radio radio-inline"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Manual" class="flat-green radio radio-inline"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="form-group cSAPOption">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Sub-facility Name <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-search"></i></div>
                            <asp:TextBox ID="txtSAPFacilitySearch" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="txtSAPFacilityCode" runat="server" CssClass="hidden"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%-- Manual Option --%>
                <div class="form-group cManualOption">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Category <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-6 col-lg-4">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-list"></i></div>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text="- Select -" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="S" Text="Group of storage location (S)"></asp:ListItem>
                                <asp:ListItem Value="P" Text="Plant of subsidiary (P)"></asp:ListItem>
                                <asp:ListItem Value="O" Text="Office (Z)"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group cManualOption">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Sub-facility Name <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtSubFacManual" runat="server" CssClass="form-control" MaxLength="1000"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group cManualOption">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Reference SAP Code(Optional) :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-search"></i></div>
                            <asp:TextBox ID="txtRefSAPCodeSearch" runat="server" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:TextBox ID="txtRefSAPCodeValue" runat="server" CssClass="hidden"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group cManualOption">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Internal Code :</label>
                    <div class="col-xs-12 col-md-4 col-lg-3">
                        <div class="input-group">
                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtInternalCode" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%--<div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">GC Operation Type <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:CheckBox runat="server" ID="cbAll" CssClass="flat-green" Text="All" />
                        <asp:CheckBoxList runat="server" ID="cblOperationTypeGC" CssClass="flat-green" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
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
    <%--ID Asset--%>
    <asp:HiddenField runat="server" ID="hdfAssetID" />
    <%--ID Facility--%>
    <asp:HiddenField runat="server" ID="hdfFacilityID" />
    <%--Check Mode--%>
    <asp:HiddenField runat="server" ID="hdfIsNew" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        $(function () {
            $("a[id$=aCancel]").attr("href", "admin_asset_lst.aspx?" + $("input[id$=hdfReturnStr]").val());
            //$("input[id*=cblOperationTypeGC]").iCheck("disable");
            //$('input[id*=cbAll]').iCheck("disable");
            setValidate();
            //setEventCtrl();

            if (GetValRadioListICheck("rblOption") == "P") {//SAP
                $(".cSAPOption").show();
                $(".cManualOption").hide();
            }
            else {
                $(".cSAPOption").hide();
                $(".cManualOption").show();
            }
        })

        function setValidate() {
            // $("input[id*=cblOperationTypeGC]").attr("name", "cblOperationTypeGC");
            var objValidate = {};
            objValidate[GetElementName("txtSAPFacilitySearch", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Sub-facility Name");
            objValidate[GetElementName("ddlCategory", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Category");
            objValidate[GetElementName("txtSubFacManual", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Sub-facility Name");

            objValidate[GetElementName("txtRemark", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Sub-facility Name");
            objValidate[GetElementName("ddlOperationType", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Operation Type");

            //objValidate[GetElementName("txtAssetName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Sub-facility Name");
            //objValidate[GetElementName("txtCodeSap", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " SAP Code");
            //objValidate[GetElementName("txtNameSap", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " SAP Name");
            // objValidate["cblOperationTypeGC"] = addValidateCheckbox_notEmpty(DialogMsg.Specify + " GC Operation Type");

            BindValidate("divContent", objValidate);
            // $('i[data-fv-icon-for="cblOperationTypeGC"]').css('left', '700px').css('top', '-20px');

            $('input[id*=rblOption]').on('ifChecked', function (event) {
                if ($(this).val() == "P") {//SAP
                    $(".cSAPOption").show();
                    $(".cManualOption").hide();
                }
                else {
                    $(".cSAPOption").hide();
                    $(".cManualOption").show();
                }
            });

            $('input[id*=rblStatus]').on('ifChecked', function (event) {
                if ($(this).val() == "N") {
                    Input("txtRemark").prop("disabled", false);
                }
                else {
                    UpdateStatusValidateControl("divContent", GetElementName("txtRemark", objControl.txtbox), ValidateProp.Status_NOT_VALIDATED);
                    Input("txtRemark").prop("disabled", true);
                }
            });

            SetAutocomSAPFacility();
            SetAutocomRefSAPFacility();
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

        }

        function UrlSearchFacility() {
            LoaddinProcess();
            return location.pathname + "/SearchSAPFacility";
        }

        var IsSelectedtxtSAPFacility = false;
        function SetAutocomSAPFacility() {
            var $txtAutocomplete = Input("txtSAPFacilitySearch");
            $txtAutocomplete.on("change", function () {
                if (!IsSelectedtxtSAPFacility || !IsBrowserFirefox()) {
                    $("input[id$=txtSAPFacilityCode]").val("");
                    $('#divContent').formValidation('updateStatus', GetElementName("txtSAPFacilitySearch", objControl.txtbox), 'INVALID');
                }
                else if ($("input[id$=txtSAPFacilityCode]").val() == "") {
                    $('#divContent').formValidation('updateStatus', GetElementName("txtSAPFacilitySearch", objControl.txtbox), 'INVALID');
                }
            }).focus(function () {
                IsSelectedtxtSAPFacility = false;
            });

            $txtAutocomplete.autocomplete({
                source: function (request, response) {
                    IsSelectedtxtSAPFacility = false;
                    $.ajax({
                        url: UrlSearchFacility(),
                        data: JSON.stringify({ 'sSearch': request.term, 'sFacilityID': Input("hdfFacilityID").val(), 'sAssesstID': Input("hdfAssetID").val() }),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            HideLoadding();
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.sName,
                                    label: item.sName,
                                    sCode: item.sCode
                                }
                            }));
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    IsSelectedtxtSAPFacility = true;
                    SetValueTextBox("txtSAPFacilityCode", ui.item.sCode);

                    if (IsBrowserFirefox()) {
                        $txtAutocomplete.blur();
                    }
                },
                complete: function (jqXHR, status) {//finaly
                    HideLoadding();
                }
            });
        }

        var IsSelectedtxtRefSAPFacility = false;
        function SetAutocomRefSAPFacility() {
            var $txtAutocomplete = Input("txtRefSAPCodeSearch");
            $txtAutocomplete.on("change", function () {
                if (!IsSelectedtxtRefSAPFacility || !IsBrowserFirefox()) {
                    $("input[id$=txtRefSAPCodeValue]").val("");
                    $("input[id$=txtRefSAPCodeSearch]").val("");
                    $('#divContent').formValidation('updateStatus', GetElementName("txtRefSAPCodeSearch", objControl.txtbox), 'INVALID');
                }
                else if ($("input[id$=txtRefSAPCodeValue]").val() == "") {
                    $('#divContent').formValidation('updateStatus', GetElementName("txtRefSAPCodeSearch", objControl.txtbox), 'INVALID');
                }
            }).focus(function () {
                IsSelectedtxtRefSAPFacility = false;
            });

            $txtAutocomplete.autocomplete({
                source: function (request, response) {
                    IsSelectedtxtRefSAPFacility = false;
                    $.ajax({
                        url: UrlSearchFacility(),
                        data: JSON.stringify({ 'sSearch': request.term, 'sFacilityID': Input("hdfFacilityID").val(), 'sAssesstID': Input("hdfAssetID").val() }),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            HideLoadding();
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.sName,
                                    label: item.sName,
                                    sCode: item.sCode
                                }
                            }));
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    IsSelectedtxtRefSAPFacility = true;
                    SetValueTextBox("txtRefSAPCodeValue", ui.item.sCode);

                    if (IsBrowserFirefox()) {
                        $txtAutocomplete.blur();
                    }
                },
                complete: function (jqXHR, status) {//finaly
                    HideLoadding();
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
                        sAssetID: GetValTextBox("hdfAssetID"),
                        sFacilityID: GetValTextBox("hdfFacilityID"),
                        sRefFacType: GetValRadioListICheck("rblOption"),
                        sRefFacSubType: GetValDropdown("ddlCategory"),
                        sAssetName: GetValTextBox("txtSubFacManual"),
                        sRefFacCode: GetValRadioListICheck("rblOption") == "P" ? GetValTextBox("txtSAPFacilityCode") : GetValTextBox("txtRefSAPCodeValue"),

                        //sCodeSAP: GetValTextBox("txtCodeSap"),
                        //sNameSAP: GetValTextBox("txtNameSap"),
                        //sAssetName: GetValTextBox("txtAssetName"),

                        sDescription: GetValTextArea("txtDesc"),
                        sActive: GetValRadioListICheck("rblStatus"),
                        sRemark: GetValTextBox("txtRemark"),
                        IsNew: GetValTextBox("hdfIsNew") == "C",
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

