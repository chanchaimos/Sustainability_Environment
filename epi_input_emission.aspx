<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_EPI_FORMS.master" AutoEventWireup="true" CodeFile="epi_input_emission.aspx.cs" Inherits="epi_input_emission" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .flat-green:not(.radio) label {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

        .flat-green-custom label {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

        div#divHasPRD table > tbody > tr > td > input, table#tbVOC > tbody > tr > td > input {
            text-align: right !important;
        }

        hr {
            margin-top: 20px !important;
            margin-bottom: 20px !important;
            border: 0 !important;
            border-top: 3px solid #eee !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divContent" style="display: none;">
        <div class="col-xs-12 text-right-lg text-right-md text-left-sm" style="margin-bottom: 5px;">
            <button type="button" onclick="ShowDeviate();" class="btn btn-info" title="Deviate History"><i class="fas fa-comments"></i></button>
            <button type="button" onclick="ShowHistory();" class="btn btn-info" title="Workflow History"><i class="fas fa-comment-alt"></i></button>
            <asp:LinkButton ID="lnkExport" runat="server" CssClass="btn btn-success" OnClick="lnkExport_Click">Export</asp:LinkButton>
        </div>
        <div class="clearfix"></div>
        <div class="col-xs-12">
            <div id="divHasPRD">
                <div class="form-group">
                    <a style="font-size: 24px;" title="Helper NOx emission" href="Helper_Indicator.aspx?ind=4&&prd=160" target="_blank"><i class="fas fa-question-circle"></i></a>
                    <a style="font-size: 24px;" title="Helper SO2 emission" href="Helper_Indicator.aspx?ind=4&&prd=162" target="_blank"><i class="fas fa-question-circle"></i></a>
                    <a style="font-size: 24px;" title="Helper TSP emission" href="Helper_Indicator.aspx?ind=4&&prd=164" target="_blank"><i class="fas fa-question-circle"></i></a>
                </div>
                <div class="form-group">
                    <div class="panel panel-primary col">
                        <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divCombustion">
                            Combustion
                        </div>
                        <div class="panel-body panel-collapse collapse in" id="divCombustion">
                            <div class="table-responsive">
                                <table id="tbCombustion" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="panel panel-primary col">
                        <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divNonCombustion">
                            Non-Combustion
                        </div>
                        <div class="panel-body panel-collapse collapse in" id="divNonCombustion">
                            <div class="table-responsive">
                                <table id="tbNonCombustion" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="panel panel-primary col">
                        <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divCEM">
                            CEM
                        </div>
                        <div class="panel-body panel-collapse collapse in" id="divCEM">
                            <div class="table-responsive">
                                <table id="tbCEM" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="panel panel-primary col">
                        <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divAdditionalCombustion">
                            Additional Combustion
                        </div>
                        <div class="panel-body panel-collapse collapse in" id="divAdditionalCombustion">
                            <div class="table-responsive">
                                <table id="tbAdditionalCombustion" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="panel panel-primary col">
                        <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divAdditionalNonCombustion">
                            Additional Non-Combustion
                        </div>
                        <div class="panel-body panel-collapse collapse in" id="divAdditionalNonCombustion">
                            <div class="table-responsive">
                                <table id="tbAdditionalNonCombustion" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="divAddStack">
                    <div class="form-group">
                        <button type="button" id="btnAddStack" onclick="addStack()" class="btn btn-primary btn-sm text-left NoPRMS">Add Stack</button>
                    </div>
                    <div class="form-group">
                        <div class="table-responsive">
                            <table id="tbStack" class="table dataTable table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th class="text-center" style="width: 10%">No.</th>
                                        <th class="text-center" style="width: 55%">Stack Name</th>
                                        <th class="text-center" style="width: 20%">Stack Type</th>
                                        <th class="text-center" style="width: 15%"></th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="divTbAddStack" style="display: none;">
                    <div class="panel panel-info col">
                        <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divCreate">
                            <i class="fas fa-edit"></i>Create / Update
                        </div>
                        <div class="panel-body panel-collapse collapse in" id="divCreate">
                            <div class="form-group">
                                <label for="txtStackName" class="control-label col-xs-12 col-md-2 col-lg-2">Stack Name<span class="text-red">*</span> :</label>
                                <div class="col-xs-12 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <input class="form-control input-sm input-sm str" id="txtStackName" name="txtStackName" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-md-6 col-lg-6">
                                    <div class="form-group">
                                        <asp:RadioButtonList runat="server" ID="rblTypeStack" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Combustion" class="flat-green" Value="CMS" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Non-Combustion" class="flat-green" Value="NCS"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="form-group">
                                <div class="table-responsive">
                                    <table id="tbAddStack" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                    </table>
                                </div>
                            </div>
                            <div class="form-group">
                                <button type="button" id="btnAddAdditional" onclick="addAdditional()" class="btn btn-primary btn-sm text-left NoPRMS">Add (Additional)</button>
                            </div>
                            <hr />
                            <div class="form-group">
                                <div class="table-responsive">
                                    <table id="tbAddStackCEM" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                    </table>
                                </div>
                            </div>
                            <div class="well">
                                <div class="form-group">
                                    <label class="control-label">Remark Stack</label>
                                    <textarea id="txtRemarkStack" runat="server" class="form-control" rows="4" style="resize: vertical"></textarea>
                                </div>
                            </div>
                            <div class="form-group" id="divButtonStack">
                                <div class="col-xs-12 text-center">
                                    <button type="button" id="btnSaveStack" class="btn btn-primary NoPRMS" onclick="saveStack()">Save this Stack</button>
                                    <button type="button" id="btnCancelStack" class="btn btn-default" onclick="CancelAdd()">Cancel this Stack</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div id="divVOC">
                <div class="form-group">
                    <a style="font-size: 24px;" title="Helper VOC emission" href="Helper_Indicator.aspx?ind=4&&prd=193" target="_blank"><i class="fas fa-question-circle"></i></a>
                </div>
                <div class="form-group">
                    <div class="panel panel-primary col">
                        <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divVOCEmission">
                            VOC Emission
                        </div>
                        <div class="panel-body panel-collapse collapse in" id="divVOCEmission">
                            <div class="form-group form-inline">
                                <span class="control-label">VOC Emission Data Available</span>&nbsp;
                                <asp:DropDownList runat="server" ID="ddlVOCType" CssClass="form-control input-sm">
                                    <asp:ListItem Text="Monthly" Value="M"></asp:ListItem>
                                    <asp:ListItem Text="Yearly" Value="Y"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="clearfix"></div>
                            <div class="table-responsive form-group">
                                <table id="tbVOC" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                </table>
                            </div>
                            <div class="well">
                                <div class="form-group">
                                    <label class="control-label">Remark VOC Emission</label>
                                    <textarea id="txtRemarkVOC" runat="server" class="form-control" rows="4" style="resize: vertical"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div id="divUploadFile">
                <div class="form-group">
                    <div class="panel panel-info">
                        <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" data-parent="#divContent1" href="#divBoxOtherFile">
                            <i class="glyphicon glyphicon-file"></i>&nbsp;Attach File
                        </div>
                        <div id="divBoxOtherFile" class="panel-collapse collapse in">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-6 NoPRMS">
                                        <input type="file" id="fulOther" name="fulOther" />
                                    </div>
                                    <div class="col-xs-6 NoPRMS">
                                        <span class="text-red">Maximum size 10MB / File, Allowed File Type: .jpg, .jpeg, .png, .xls, .xlsx, .pdf, .txt, .doc, .docx, .ppt, .pptx, .rar, .zip</span>
                                    </div>
                                    <div class="col-xs-12">
                                        <div id="divGridOtherFile">
                                            <table id="tblOtherFile" class="table dataTable table-responsive table-hover table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th class="dt-head-center" style="width: 10%">No.</th>
                                                        <th class="dt-head-center" style="width: 50%">File Name</th>
                                                        <th class="dt-head-center" style="width: 30%">File Description</th>
                                                        <th class="dt-head-center NoPRMS" style="width: 10%">Action</th>
                                                    </tr>
                                                </thead>
                                                <tbody></tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdfIndID" />
    <asp:HiddenField runat="server" ID="hdfOprtID" />
    <asp:HiddenField runat="server" ID="hdfManageID" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        var arrShortMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var arrFullMonth = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var $divContent = $("div[id$=divContent]");
        var $divHasPRD = $("div[id$=divHasPRD]");
        var $divVOC = $("div[id$=divVOC]");
        var $divAddStack = $("div[id$=divAddStack]");
        var $divTbAddStack = $("div[id$=divTbAddStack]");
        var $divUploadFile = $("div[id$=divUploadFile]");

        var $tbCombustion = $("table[id$=tbCombustion]");
        var $tbNonCombustion = $("table[id$=tbNonCombustion]");
        var $tbCEM = $("table[id$=tbCEM]");
        var $tbAdditionalCombustion = $("table[id$=tbAdditionalCombustion]");
        var $tbAdditionalNonCombustion = $("table[id$=tbAdditionalNonCombustion]");
        var $tbStack = $("table[id$=tbStack]");
        var $tbAddStack = $("table[id$=tbAddStack]");
        var $tbAddStackCEM = $("table[id$=tbAddStackCEM]");
        var $tbVOC = $("table[id$=tbVOC]");
        var IsFullMonth = true;

        var lstCombustion = [];
        var lstNonCombustion = [];
        var lstCEM = [];
        var lstAdditional = [];
        var lstAdditionalNonCombustion = [];
        var lstStack = [];
        var lstVOC = [];
        var lstAddStack = [];
        var lstAddStackCEM = [];
        var lstAddAdditional = [];
        var lstOtherPrd = [];
        var lstDataAddStack = [];
        var lstCEMProductID = [241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252];

        var $dataFileOther = [];

        var $btnAddStack = $("button[id$=btnAddStack]");

        var sTitleOption1 = "data available from CEM or Periodic Sampling (figure from CEM is prefered) Monthly total emission load";
        var sTitleOption2 = "data (based on normal condition i.e. 25 C @ 1 atm or 760 mmHg and dry basis) available from available from CEM or Periodic Sampling (figure from CEM is prefered)";
        $(function () {
            //$.each($("div[id$=divHasPRD],div[id$=divVOC]").find("div.table-responsive table:not(table[id$=tbStack])"), function (i, el) {
            //    el = $(el);
            //    ArrInputFromTableID.push(el.attr("id"));
            //    el.tableHeadFixer({ "left": 1 }, { "head": true })
            //})
            SetFileUploadOther();
            EventRadioInTableAddStack();
            EventInput();
        })

        function LoadData() {
            var param = {
                sIndID: Select("ddlIndicator").val(),
                sOprtID: Select("ddlOperationType").val(),
                sFacID: Select("ddlFacility").val(),
                sYear: Select("Year").val()
            }
            LoaddinProcess();
            AjaxCallWebMethod("LoadData", function (response) {

                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                } else {
                    lstCombustion = [];
                    lstNonCombustion = [];
                    lstCEM = [];
                    lstAdditional = [];
                    lstAdditionalNonCombustion = [];
                    lstStack = [];
                    lstVOC = [];
                    lstAddStack = [];
                    lstAddStackCEM = [];
                    lstAddAdditional = [];
                    lstOtherPrd = [];
                    $dataFileOther = [];
                    ArrInputFromTableID = [];
                    $btnAddStack.prop("disabled", false);
                    $("div[id$=dvbtn]").find("button").prop("disabled", false);
                    $tbCombustion.find("input[type=checkbox]").not("[class$=submited]").prop("disabled", false);
                    $tbStack.find("button").prop("disabled", false);
                    SetValueTextArea("txtRemarkStack", "");
                    Input("txtStackName").val("");
                    Input("hdfManageID").val("");
                    $divTbAddStack.hide();
                    var objData = response.d;
                    var objDataEmission = response.d.objDataEmission;
                    lstCombustion = objDataEmission.lstCombustion;
                    lstNonCombustion = objDataEmission.lstNonCombustion;
                    lstCEM = objDataEmission.lstCEM;
                    lstAdditional = objDataEmission.lstAdditional;
                    lstAdditionalNonCombustion = objDataEmission.lstAdditionalNonCombustion;
                    lstStack = objDataEmission.lstStack;
                    lstVOC = objData.lstVOC;
                    lstAddStack = objData.lstAddStack;
                    lstAddStackCEM = objData.lstAddStackCEM;
                    lstAddAdditional = objData.lstAddAdditional;
                    lstOtherPrd = objData.lstOtherPrd;
                    $dataFileOther = objData.lstFile;
                    lstStatus = objData.lstStatus;

                    IsFullMonth = true;
                    $.each(lstStatus, function (i, el) {
                        if (el.nStatusID == 0) {
                            IsFullMonth = false;
                        }
                    })
                    bindDataTable($tbCombustion, lstCombustion, true, false, false, false);
                    bindDataTable($tbNonCombustion, lstNonCombustion, false, false, false, false);
                    bindDataTable($tbCEM, lstCEM, false, false, false, false);
                    bindDataTable($tbAdditionalCombustion, lstAdditional, false, false, true, false);
                    bindDataTable($tbAdditionalNonCombustion, lstAdditionalNonCombustion, false, false, true, false);
                    var IsVOCHasCheckbox = false;
                    IsVOCHasCheckbox = lstVOC.length > 0 ? (lstVOC[0].sOption == "Y" ? false : true) : false;
                    SetValueDropDown("ddlVOCType", IsVOCHasCheckbox ? "M" : "Y");
                    bindDataTable($tbVOC, lstVOC, IsVOCHasCheckbox, true, false, false);
                    SetValueTextArea("txtRemarkVOC", objData.sRemarkVOC);
                    bindDataStack();
                    BindTableFileOther();
                }
            }, function () {
                $('.flat-green-custom').iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_square-green' //'iradio_flat-green'
                });
                if ($ddlOperationType.val() != "13") {
                    $divHasPRD.show();
                } else {
                    $divHasPRD.hide();
                }
                if (IsFullMonth) {
                    if (($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") || $hdfIsAdmin.val() == "Y") {
                        $(".NoPRMS").show();
                        $("div[id$=divContent]").find("input.target,select,textarea").prop("disabled", false);
                        $("textarea:not([id$=txtsComment])").prop("disabled", false);
                        $divUploadFile.find("input").prop("disabled", false);
                        Input("txtStackName").prop("disabled", false);
                        $btnAddStack.show()
                    } else {
                        /// 30-01-2020 เพิ่มเช็ค L2 แก้ไขได้ทุกอย่างเหมือน superAdmin
                        if ($hdfIsAdmin.val() != "Y" && $hdfRole.val() != "4") {
                            $(".NoPRMS").hide();
                            $("div[id$=divContent]").find("input.target,select,textarea").prop("disabled", true);
                            $("textarea:not([id$=txtsComment])").prop("disabled", true);
                            $divUploadFile.find("input").prop("disabled", true);
                            Input("txtStackName").prop("disabled", true);
                            $btnAddStack.hide();
                        }
                    }
                } else {
                    $(".NoPRMS").show();
                    $("div[id$=divContent]").find("input.target,select,textarea").prop("disabled", false);
                    $("textarea:not([id$=txtsComment])").prop("disabled", false);
                    $divUploadFile.find("input").prop("disabled", false);
                    Input("txtStackName").prop("disabled", false);
                    $btnAddStack.show();
                }
                $.each($("div[id$=divHasPRD],div[id$=divVOC]").find("div.table-responsive table:not(table[id$=tbStack])"), function (i, el) {
                    el = $(el);
                    ArrInputFromTableID.push(el.attr("id"));
                    //if (el.find("tbody tr").length == 1 && el.find("tbody tr td:eq(0)").has("colspan")) {
                    //    $("table#" + el.attr("id")).tableHeadFixer({ head: true });
                    //} else {
                    //    $("table#" + el.attr("id")).tableHeadFixer({ "left": 1, head: true });
                    //}
                })
                var IsVOCHasCheckbox = false;
                IsVOCHasCheckbox = lstVOC.length > 0 ? (lstVOC[0].sOption == "Y" ? false : true) : false;
                if (!IsVOCHasCheckbox) {
                    ArrInputFromTableID = $.grep(ArrInputFromTableID, function (value) {
                        return value != $tbVOC.attr("id");
                    })
                    $tbVOC.width(0);
                }
                CheckEventButton();
                CheckboxQuarterChanged();
                HideLoadding();
            }, { param: param });
            $divContent.show();
        }

        function bindDataTable(table, dataValue, HasCheckbox, IsVOC, IsAdditional, IsAddEditStack) {
            table.empty();
            var sHtml = "";
            var data = Enumerable.From(dataValue).ToArray();
            var IsTableCEM = table.attr("id") == "tbAddStackCEM";
            if (IsAdditional) {
                data = Enumerable.From(dataValue).Where(function (w) { return w.sType == "OTH" }).ToArray();
            } else {
                data = Enumerable.From(dataValue).Where(function (w) { return w.sType != "OTH" }).ToArray();
            }
            var lstDataCheck = [];
            if (data.length > 0) {
                lstDataCheck = [data[0].IsCheckM1, data[0].IsCheckM2, data[0].IsCheckM3, data[0].IsCheckM4, data[0].IsCheckM5, data[0].IsCheckM6, data[0].IsCheckM7, data[0].IsCheckM8, data[0].IsCheckM9, data[0].IsCheckM10, data[0].IsCheckM11, data[0].IsCheckM12];
            } else {
                lstDataCheck = [null, null, null, null, null, null, null, null, null, null, null, null];
            }
            sHtml += "<thead>";
            sHtml += "  <tr>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthIndicator + "px;'><label>Indicator</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'><label>Unit</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;'><label>Target</label></th>";
            if (IsVOC && !HasCheckbox) {
                sHtml += "<th class='text-center' style='vertical-align: middle;width:120px'>Total</th>";
            } else {
                if (IsAddEditStack) {
                    sHtml += "<th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;'><label>1<sup>st</sup></br><button type='button' class='btn btn-info btn-sm' tableID='" + table.attr("id") + "' amountMonth='1_6'>Confirm</button></label></th>";
                    sHtml += "<th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;'><label>2<sup>sd</sup></br><button type='button' class='btn btn-info btn-sm' tableID='" + table.attr("id") + "' amountMonth='7_12'>Confirm</button></label></th>";
                }
                sHtml += bindColumnQ(true, null, lstDataCheck, HasCheckbox, false, true, null, null, null, null, null, false);
            }
            sHtml += "  </tr>";
            sHtml += "</thead>";
            sHtml += "<tbody>";
            if (Enumerable.From(data).Where(function (w) { return w.sType == "2" || w.sType == "2H" }).ToArray().length > 0 && Enumerable.From(data).Where(function (w) { return w.sType == "CEM" }).ToArray().length == 0) {
                sHtml += "<tr style='background-color:#EFEFEF'>";
                sHtml += "<td class='text-left'style='font-weight: 700;' >General Information</td>";
                sHtml += "<td class='text-left' style='vertical-align: middle;'></td>";
                sHtml += "<td class='text-left cTarget' style='vertical-align: middle;'></td>";
                sHtml += "<td class='text-left' style='vertical-align: middle;'></td>";
                sHtml += "<td class='text-left' style='vertical-align: middle;'></td>";
                sHtml += bindColumnQ(false, data, null, false, true, true, null, null, null, null, null, false);
                sHtml += "</tr>";
            }
            $.each(data, function (i, el) {
                var lstDataMonth = [el.M1, el.M2, el.M3, el.M4, el.M5, el.M6, el.M7, el.M8, el.M9, el.M10, el.M11, el.M12]
                var sProductName = IsVOC && el.cTotal != "Y" ? "<input type='checkbox' ProductID='" + el.ProductID + "' class='flat-green-custom'/><label style='padding-left:5px;font-weight:400;'>" + el.ProductName + "</label>" : el.ProductName;
                var sUnitName = el.sUnit == null ? "" : el.sUnit;
                var sSelectOption1 = el.nOptionProduct == "1" ? "checked" : "";
                var sSelectOption2 = el.nOptionProduct == "2" || el.nOptionProduct == null ? "checked" : "";
                sHtml += "  <tr " + (el.sType == "SUM" || (el.sType == "VOC" && el.cTotal == "Y") || el.UnitID == 2 ? "style='background-color:#dbea97'" :
                    (el.sType == "2H" ? "style='background-color:#EFEFEF'" : "")) + " " + (el.sType == "2" ? "nGroupCal ='" + el.nGroupCalc + "' " + "nOption='" + el.nOption + "'" : "") + ">";
                sHtml += "      <td class='text-left' style='vertical-align: middle;" + (el.sType == "2H" ? "font-weight: 700;" : "") + "'>" + (el.sType == "SUM" ? sProductName : (el.sType == "SUM2" || el.UnitID == "68" ? "" : sProductName));
                if (el.sType == "2H" && !IsTableCEM) {
                    sHtml += "</br></br>&nbsp;<input type='radio'  id='optionStack_" + el.nGroupCalc + "1' name='optionStack_" + el.nGroupCalc + "' ProductID='" + el.ProductID + "' class='flat-green-custom' value ='1' " + sSelectOption1 + "><span title='" + sTitleOption1 + "'> Option 1: Emission Load </span>";
                    sHtml += "</br></br>&nbsp;<input type='radio'  id='optionStack_" + el.nGroupCalc + "' name='optionStack_" + el.nGroupCalc + "' ProductID='" + el.ProductID + "' class='flat-green-custom' value ='2' " + sSelectOption2 + "> <span title='" + sTitleOption2 + "'> Option 2: Emission Concentration </span> ";
                }
                sHtml += "    </td>";
                sHtml += "      <td class='text-center' style='vertical-align: middle;'>" + sUnitName + "</td>";
                sHtml += "      <td class='text-center cTarget' style='vertical-align: middle;'>" + (el.sType == "2H" ? "" : "<input class='form-control input-sm text-right target' tableID ='" + table.attr("id") + "' ProductID ='" + el.ProductID + "' UnitID ='" + el.UnitID + "'  value ='" + CheckTextInput(el.Target == null ? "" : el.Target + "") + "' maxlength='20'/>") + "</td>";

                if (IsVOC && !HasCheckbox) {
                    sHtml += "<td class='text-left' style='vertical-align:middle;'><input class='form-control input-sm total' " + (el.cTotal == "Y" || IsFullMonth ? "disabled" : "") + " cTotal = '" + el.cTotal + "' ProductID = '" + el.ProductID + "' value='" + el.nTotal + "'/></td>";
                } else {
                    if (IsAddEditStack) {
                        var sDisableInputConfirm = el.cTotal == "Y" ? "disabled" : "";
                        var sShowInputFirst = el.sType == "2H" ? "" : "<input class='form-control input-sm confirm' amountMonth='1_6' cTotal = '" + el.cTotal + "'  ProductID = '" + el.ProductID + "' " + sDisableInputConfirm + " />";
                        var sShowInputSecond = el.sType == "2H" ? "" : "<input class='form-control input-sm confirm' amountMonth='7_12' cTotal = '" + el.cTotal + "'  ProductID = '" + el.ProductID + "' " + sDisableInputConfirm + " />";
                        sHtml += "<td class='text-center' style='vertical-align: middle;'>" + sShowInputFirst + "</td>";
                        sHtml += "<td class='text-center' style='vertical-align: middle;'>" + sShowInputSecond + "</td>";
                    }
                    sHtml += bindColumnQ(false, lstDataMonth, lstDataCheck, false, el.sType == "2H", (el.sType == "2" || el.sType == "CEM") && el.cTotal != "Y" ? false : (el.sType == "VOC" && el.cTotal != "Y" ? false : true), el.ProductID, (IsAdditional ? "Y" : el.cTotal), el.nGroupCalc, null, el.nOption, IsVOC);
                }
                sHtml += "  </tr>";
            })
            sHtml += "</tbody>";
            table.append(sHtml);

            if (data.length == 0) {
                SetRowNoData(table.attr("id"), 15)
                table.find("input[type=checkbox]").prop("disabled", true);
            }
        }

        function bindColumnQ(IsTHead, lstDataMonth, dataCheck, HasCheckbox, Is2H, IsDisabled, prdID, cTotalCheck, nGroupCal, nOther, nOption, IsVoc) {
            var dataMonth = lstDataMonth == null ? [null, null, null, null, null, null, null, null, null, null, null, null] : lstDataMonth;
            var cTotal = cTotalCheck == null || cTotalCheck == undefined ? "" : cTotalCheck;
            var sHtml = "";
            var strForDisabled = "";
            var IsCheck = false;
            if (IsTHead) {
                for (var i = 1 ; i <= 12; i++) {
                    strForDisabled = "";
                    if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                        if (lstStatus[i - 1].nStatusID != "2") {
                            strForDisabled = "disabled";
                        }
                    } else {
                        if (lstStatus[i - 1].nStatusID != "0") {
                            strForDisabled = "disabled";
                        }
                    }
                    if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                        strForDisabled = "";
                    }

                    var IsCheck = dataCheck[i - 1] == "Y" ? "checked" : "";
                    sHtml += "<th style='vertical-align: middle;' class='text-center M_" + i + " QHead_" + getQrt(i) + "'>" + (HasCheckbox ? "<input type='checkbox' nMonth ='" + i + "' class='flat-green-custom " + (strForDisabled != "" ? "submited" : "") + "' " + IsCheck + " " + strForDisabled + " />&nbsp;" : "") + "<label>Q" + getQrt(i) + " : " + arrShortMonth[i - 1] + "</label></th>";
                }
            } else {
                for (var i = 1 ; i <= 12; i++) {
                    strForDisabled = "";
                    if (IsVoc && dataCheck[i - 1] == "Y") {
                        strForDisabled = "disabled";
                    } else {
                        strForDisabled = IsDisabled ? "disabled" : "";
                    }
                    if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                        if (lstStatus[i - 1].nStatusID != "2") {
                            strForDisabled = "disabled";
                        }
                    } else {
                        if (lstStatus[i - 1].nStatusID != "0") {
                            strForDisabled = "disabled";
                        }
                    }
                    if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                        strForDisabled = "";
                    }

                    IsCheck = $tbCombustion.find("input[type=checkbox][nMonth=" + i + "]").is(":checked") ? true : false;
                    if (IsCheck && !IsVoc) {
                        if ($("input[id*=rblTypeStack]:checked").val() == "CMS") {
                            strForDisabled = "disabled";
                        }
                    }
                    var nDataVal = dataCheck != null ? dataCheck[i - 1] == "Y" ? "0" : dataMonth[i - 1] : dataMonth[i - 1];
                    var nVal = cTotal == "Y" ? (nDataVal == null ? "" : CheckTextOutput(nDataVal + "")) : (nDataVal == null ? "" : CheckTextInput(nDataVal + ""));
                    var nTrueVal = cTotal == "Y" ? "nTrueVal='" + (nDataVal == null ? "" : nDataVal) + "'" : "";
                    sHtml += "<td class='text-center M_" + i + " QHead_" + getQrt(i) + " '>" + (Is2H ? "" : "<input class='form-control input-sm " + (nGroupCal == "999" ? "otherPrd" : "") + "' " + (nGroupCal != null ? "nGroupCal='" + nGroupCal + (nOther != null ? "_" + nOther : "") + "'" : "") + " " + (nOption != null ? "nOption ='" + nOption + "'" : "") + " cTotal='" + cTotal + "' ProductID = '" + prdID + "' nMonth ='" + i + "' value ='" + nVal + "' " + strForDisabled + " " + nTrueVal + "  maxlength='20'/>") + "</td>";
                }
            }
            return sHtml;
        }

        function getQrt(nColumn) {
            var nQ = 0;
            if (nColumn >= 1 && nColumn <= 3) {
                nQ = 1;
            }
            else if (nColumn >= 4 && nColumn <= 6) {
                nQ = 2;
            }
            else if (nColumn >= 7 && nColumn <= 9) {
                nQ = 3;
            }
            else if (nColumn >= 9 && nColumn <= 12) {
                nQ = 4;
            }
            return nQ;
        }

        function bindDataStack() {
            var data = lstStack;
            $tbStack.find("tbody tr").remove();
            var sHtml = "";
            var IsSubmitSomeMonth = false;
            $.each(lstStatus, function (i, el) {
                if (el.nStatusID != 0) {
                    IsSubmitSomeMonth = true;
                }
            })

            if (data.length > 0) {
                $.each(data, function (i, el) {
                    var sTitle = "";
                    var sClass = "";
                    var sIcon = "";
                    var sStackType = el.sStackType == "CMS" ? "Combusion" : "Non-Combussion";
                    if (IsFullMonth || $hdfPRMS.val() == "1") {
                        sIcon = "search";
                        sTitle = "View";
                        sClass = "btn-info";
                    } else {
                        sIcon = "edit";
                        sTitle = "Edit";
                        sClass = "btn-warning";
                    }

                    //var sBtnEdit = "<button type='button' title='" + sTitle + "' style='margin-bottom:2px;' class='btn " + sClass + " btn-sm' value='" + el.nStackID + "' onclick='EditStack(" + el.nStackID + ")'><i class='fa fa-" + sIcon + "'></i></button>";
                    var sBtnEdit = "<button type='button' style='margin-bottom:2px;' class='btn " + sClass + " btn-sm' value='" + el.nStackID + "' onclick='EditStack(" + el.nStackID + ")'><i class='fa fa-" + sIcon + "'></i></button>";
                    var sBtnDel = "<button type='button' title='Delete' style='margin-bottom:2px;' class='btn btn-danger btn-sm' value='" + el.nStackID + "' onclick='DelStack(" + el.nStackID + ")'><i class='fa fa-trash'></i></button>";
                    sHtml += "<tr>";
                    sHtml += "  <td class='text-center'>" + (i + 1) + "</td>";
                    sHtml += "  <td class='text-left'>" + el.sStackName + "</td>";
                    sHtml += "  <td class='text-center'>" + sStackType + "</td>";
                    if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") || ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27")) {
                        sHtml += "  <td class='text-center'>" + sBtnEdit + sBtnDel + "</td>";
                    } else {
                        sHtml += "  <td class='text-center'>" + sBtnEdit + (IsFullMonth || (IsSubmitSomeMonth && el.IsSubmited) || $hdfPRMS.val() == "1" ? "" : "&nbsp;" + sBtnDel) + "</td>";
                    }
                    sHtml += "</tr>";
                })
                $tbStack.find("tbody").append(sHtml);
            } else {
                SetRowNoData("tbStack", 4);
            }
            SetTootip();
        }

        function addStack() {
            $tbAddStack.find("tbody").remove();
            $tbAddStackCEM.find("tbody").remove();
            bindDataTable($tbAddStack, lstAddStack, false, false, false, true);
            bindDataTable($tbAddStackCEM, lstAddStackCEM, false, false, false, true);
            $tbAddStack.find("input:not(input[name*=optionStack_],input[id*=rblTypeStack_])").val("");
            $tbAddStackCEM.find("input").val("");
            $("input[name*=optionStack_][value=2]").iCheck("check");
            $("input[id*=rblTypeStack_]").iCheck("enable");
            $("input[id*=rblTypeStack_]:first").iCheck("check");
            $.each($("input[name*=optionStack_]:checked"), function () {
                el = $(this);
                var attrName = el.attr("name");
                var nGroupCal = el.attr("id").split("_")[1];
                var nOption = el.val();

                var nGroupCalForHide = $("input[name=" + attrName + "]:not(input[id=" + el.attr("id") + "])").attr("id").split("_")[1];
                $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOption + "]").show();
                $("tr[nGroupCal=" + nGroupCalForHide + "]").hide();
            })
            $btnAddStack.prop("disabled", true);
            $("div[id$=dvbtn]").find("button").prop("disabled", true);
            $tbStack.find("button").prop("disabled", true);
            $tbCombustion.find("input[type=checkbox]").not("[class$=submited]").prop("disabled", true);
            $divTbAddStack.show();
            bindValidateFormInput("txtStackName", "Stack Name", objControl.txtbox);
            UpdateStatusValidateControl("divTbAddStack", "txtStackName", "NOT_VALIDATED");
            ScrollTopToElementsTo("divTbAddStack", 85);

            var MaxID = lstStack.length > 0 ? Enumerable.From(lstStack).Max(function (m) { return m.nStackID }) + 1 : 1;
            var DataAddStack = [];
            var DataAddStackCEM = [];
            DataAddStack = JSON.parse(JSON.stringify(lstAddStack));
            DataAddStackCEM = JSON.parse(JSON.stringify(lstAddStackCEM));
            $.each(DataAddStack, function (i, el) {
                el.nStackID = MaxID;
            })
            $.each(DataAddStackCEM, function (i, el) {
                el.nStackID = MaxID;
            })
            lstStack.push({
                nStackID: MaxID,
                sStackName: Input("txtStackName").val(),
                sRemark: GetValTextArea("txtRemarkStack"),
                sStackType: $("input[id*=rblTypeStack]:checked").val(),
                lstDataStack: DataAddStack,
                lstDataStackCEM: DataAddStackCEM,
                IsSaved: false,
                IsSumited: false,
            });
            Input("hdfManageID").val(MaxID);
            $('.flat-green-custom').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_square-green' //'iradio_flat-green'
            });
            if (IsFullMonth || $hdfPRMS.val() == "1") {
                /// 30-01-2020 เพิ่มเช็ค L2 แก้ไขได้ทุกอย่างเหมือน superAdmin
                if ($hdfIsAdmin.val() != "Y" && $hdfRole.val() != "4") {
                    $tbAddStack.find("input,select").prop("disabled", true);
                    $tbAddStackCEM.find("input,select").prop("disabled", true);
                }
            }
            SetTootip();
            CheckboxQuarterChangedCustom($tbAddStack.attr("id"));
            CheckboxQuarterChangedCustom($tbAddStackCEM.attr("id"));

            //$tbAddStack.tableHeadFixer({ "left": 1 }, { "head": true })
        }

        function CancelAdd() {
            $btnAddStack.prop("disabled", false);
            $("div[id$=dvbtn]").find("button").prop("disabled", false);
            $tbCombustion.find("input[type=checkbox]").not("[class$=submited]").prop("disabled", false);
            $tbStack.find("button").prop("disabled", false);
            SetValueTextArea("txtRemarkStack", "");
            Input("txtStackName").val("");
            UpdateStatusValidateControl("divTbAddStack", "txtStackName", "NOT_VALIDATED");
            var dataAdditionalCur = Enumerable.From(lstStack).FirstOrDefault(null, function (w) { return w.nStackID == Input("hdfManageID").val() });
            if (dataAdditionalCur != null) {
                dataAdditionalCur.lstDataStack = Enumerable.From(dataAdditionalCur.lstDataStack).Where(function (w) { return w.IsSaved == true }).ToArray();
            }
            lstStack = Enumerable.From(lstStack).Where(function (w) { return w.IsSaved }).ToArray();
            Input("hdfManageID").val("");
            $divTbAddStack.hide();
            ScrollTopToElementsTo("divAddStack", 85);
        }

        function EditStack(nStackID) {
            var dataStack = Enumerable.From(lstStack).FirstOrDefault(null, function (w) { return w.nStackID == nStackID });
            if (dataStack != null) {
                $("input[id*=rblTypeStack_]").iCheck("enable");
                Input("hdfManageID").val(dataStack.nStackID);
                Input("txtStackName").val(dataStack.sStackName);
                SetValueTextArea("txtRemarkStack", dataStack.sRemark);
                $("input[id*=rblTypeStack_][value=" + dataStack.sStackType + "]").iCheck("check");
                var IsSubmitSomeMonth = false;
                bindValidateFormInput("txtStackName", "Stack Name", objControl.txtbox);
                UpdateStatusValidateControl("divTbAddStack", "txtStackName", "NOT_VALIDATED");
                $tbAddStack.find("tbody").remove();
                bindDataTable($tbAddStack, dataStack.lstDataStack, false, false, false, true);
                bindDataTable($tbAddStackCEM, dataStack.lstDataStackCEM, false, false, false, true);

                var dataOther = Enumerable.From(dataStack.lstDataStack).Where(function (w) { return w.sType == "OTH" }).ToArray();
                var nMaxnOther = dataOther.length > 0 ? Enumerable.From(dataOther).Max(function (m) { return m.nID }) : 0;
                for (var i = 0; i <= nMaxnOther; i++) {
                    var dataForBind = Enumerable.From(dataOther).Where(function (w) { return w.nID == i }).ToArray();
                    if (dataForBind.length > 0) {
                        bindDataAdditional(i, dataForBind, dataForBind[0].IsSubmited);
                    }
                }
                $divTbAddStack.show();
                $('.flat-green-custom').iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_square-green' //'iradio_flat-green'
                });
                $.each(lstStatus, function (i, el) {
                    if (el.nStatusID != 0) {
                        IsSubmitSomeMonth = true;
                    }
                })
                if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") || ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27")) {
                    $("input[id*=rblTypeStack_]").prop("disabled", false);
                } else {
                    if ((IsFullMonth || (IsSubmitSomeMonth && dataStack.IsSubmited) || $hdfPRMS.val() == "1")) {
                        $("input[id*=rblTypeStack_]").prop("disabled", true);

                    } else {
                        $("input[id*=rblTypeStack_]").prop("disabled", false);
                    }
                }
                $.each($("input[name*=optionStack_]:checked"), function () {
                    el = $(this);
                    var attrName = el.attr("name");
                    var nGroupCal = el.attr("id").split("_")[1];
                    var nOption = el.val();

                    var nGroupCalForHide = $("input[name=" + attrName + "]:not(input[id=" + el.attr("id") + "])").attr("id").split("_")[1];
                    $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOption + "]").show();
                    $("tr[nGroupCal=" + nGroupCalForHide + "]").hide();
                })
                $.each($("input[name*=optionStack_][class*=otherPrd]:checked"), function () {
                    el = $(this);
                    var attrName = "";
                    var nGroupCal = "";
                    var nOption = "";
                    if (el.is(":checked")) {
                        attrName = el.attr("name");
                        nGroupCal = el.attr("name").split("_")[1] + "_" + el.attr("name").split("_")[2];
                        nOption = el.val();
                        var nOptionForHide = $("input[name=" + attrName + "]:not(input[name=" + attrName + "][value=" + nOption + "])").val();
                        $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOption + "]").show();
                        $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOptionForHide + "]").hide();
                    }
                })
                $.each($("select[name*=ddlOther_]"), function (i, el) {
                    el = $(el);
                    $("#divTbAddStack").formValidation('addField', el.attr("name"), addValidate_notEmpty(DialogMsg.Specify));
                })
                $btnAddStack.prop("disabled", true);
                $("div[id$=dvbtn]").find("button").prop("disabled", true);
                $tbStack.find("button").prop("disabled", true);
                $tbCombustion.find("input[type=checkbox]").not("[class$=submited]").prop("disabled", true);
                if (IsFullMonth || $hdfPRMS.val() == "1") {
                    /// 30-01-2020 เพิ่มเช็ค L2 แก้ไขได้ทุกอย่างเหมือน superAdmin
                    if ($hdfIsAdmin.val() != "Y" && $hdfRole.val() != "4") {
                        $tbAddStack.find("select,input").prop("disabled", true);
                        $tbAddStackCEM.find("select,input").prop("disabled", true);
                    }
                }
                ScrollTopToElementsTo("divTbAddStack", 85);
                SetTootip();
                CheckboxQuarterChangedCustom($tbAddStack.attr("id"));
                CheckboxQuarterChangedCustom($tbAddStackCEM.attr("id"));
                //$tbAddStack.tableHeadFixer({ "left": 1 }, { "head": true })
            }
        }

        function DelStack(nStackID) {
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                lstStack = Enumerable.From(lstStack).Where(function (w) { return w.nStackID != nStackID }).ToArray();
                var param = {
                    sIndID: Select("ddlIndicator").val(),
                    sOprtID: Select("ddlOperationType").val(),
                    sFacID: Select("ddlFacility").val(),
                    sYear: Select("Year").val()
                }
                ReCalculateAll(function () {
                    bindDataStack();
                });
            })
        }

        function SetFileUploadOther() {
            var filupload1 = $('input[id$="fulOther"]').fileuploader({
                enableApi: true,
                limit: null,
                fileMaxSize: 10,
                dialogs: SysFileUpload.dialogs,
                captions: SysFileUpload.captions,
                extensions: SysFileUpload.arrFileType,
                thumbnails: null,
                upload: {
                    url: AshxSysFunc.UrlFileUpload,
                    data: { funcname: "UPLOAD", savetopath: 'Materials/Temp/', savetoname: '' },
                    type: 'POST',
                    enctype: 'multipart/form-data',
                    start: true,
                    synchron: true,
                    beforeSend: function (item, listEl, parentEl, newInputEl, inputEl) {
                        return true;
                    },
                    onProgress: SysFileUpload.onProgress,
                    onSuccess: SysFileUpload.onSuccess,
                    onError: SysFileUpload.onError,
                    onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                        var arrFile = apiFile1.getFiles();
                        for (var i = 0; i < arrFile.length; i++) {
                            var item = arrFile[i];
                            $dataFileOther.push(item.data);
                        }

                        //Update ID
                        var qMaxID = $dataFileOther.length > 0 ? Enumerable.From($dataFileOther).Max(function (x) { return x.ID }) + 1 : 1;
                        for (var i = 0; i < $dataFileOther.length; i++) {
                            var item = $dataFileOther[i];
                            if (item.ID == 0) {
                                item.ID = qMaxID;
                                qMaxID++;
                            }
                        }

                        apiFile1.reset();
                        HideLoadding();

                        //call function render file
                        BindTableFileOther();
                    }
                }
            });

            var apiFile1 = $.fileuploader.getInstance(filupload1);
        }

        function BindTableFileOther() {
            var sTableID = "tblOtherFile";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var lstData = Enumerable.From($dataFileOther).Where(function (x) { return x.sDelete == "N" }).ToArray();
            if (lstData != null && lstData.length > 0) {
                var htmlTD = '<tr><td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-left"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-center NoPRMS"></td>';
                htmlTD += '</tr>';

                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                $("table[id$=" + sTableID + "] tbody tr").remove();

                for (var i = 0; i < lstData.length; i++) {
                    var item = lstData[i];

                    $("td", row).eq(0).html((i + 1) + ".");
                    $("td", row).eq(1).html('<a href="' + item.url + '" target="_blank">' + item.FileName + '</a>');
                    $("td", row).eq(2).html('<input id="txtFile_' + item.ID + '" class="form-control input-sm" />');
                    $("td", row).eq(3).html('<button type="button" class="btn btn-danger btn-sm" title="Delete" onclick="DeleteFileOther(' + item.ID + ')"><i class="glyphicon glyphicon-trash"></i></button>');

                    $("table[id$=" + sTableID + "] tbody").append(row);
                    row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                }

                for (var x = 0; x < lstData.length; x++) {
                    var item = lstData[x];
                    $("#txtFile_" + item.ID + "").val(item.sDescription);
                }
                SetTootip();
            } else {
                SetRowNoData(sTableID, 4);
            }
            HideLoadding();
        }

        function DeleteFileOther(fileid) {
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                var item = Enumerable.From($dataFileOther).FirstOrDefault(null, function (x) { return x.ID == fileid });
                if (item != null) {
                    item.sDelete = "Y";
                    if (Boolean(item.IsNewFile)) {
                        $.ajax({
                            type: "POST",
                            url: AshxSysFunc.UrlFileUpload,
                            data: { funcname: "DEL", delpath: item.SaveToPath, delfilename: item.SaveToFileName },
                            success: function (response) {
                                if (Boolean(response.IsCompleted)) {
                                    BindTableFileOther();
                                } else {
                                    HideLoadding();
                                }
                            },
                            complete: function (jqXHR, status) {//finaly
                                //HideLoadding();
                            }
                        });
                    } else {
                        BindTableFileOther();
                    }
                } else {
                    DialogWarning(DialogHeader.Warning, "Not found !");
                }
            }, "");
        }

        function EventRadioInTableAddStack() {
            $tbAddStack.delegate("input[name*=optionStack_]", "ifChanged", function () {
                el = $(this);
                var attrName = "";
                var nGroupCal = "";
                var nOption = "";
                if (el.is(":checked")) {
                    if (el.hasClass("otherPrd")) {
                        attrName = el.attr("name");
                        nGroupCal = el.attr("name").split("_")[1] + "_" + el.attr("name").split("_")[2];
                        nOption = el.val();
                        var nOptionForHide = $("input[name=" + attrName + "]:not(input[name=" + attrName + "][value=" + nOption + "])").val();
                        $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOption + "]").show();
                        $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOptionForHide + "]").hide();
                    } else {
                        attrName = el.attr("name");
                        nGroupCal = el.attr("id").split("_")[1];
                        nOption = el.val();
                        var nGroupCalForHide = $("input[name=" + attrName + "]:not(input[id=" + el.attr("id") + "])").attr("id").split("_")[1];
                        $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOption + "]").show();
                        $("tr[nGroupCal=" + nGroupCalForHide + "]").hide();
                    }
                }
            });
        }

        function addAdditional() {
            var nOther = 1;
            var nStackID = Input("hdfManageID").val();
            var dataStack = Enumerable.From(lstStack).FirstOrDefault(null, function (w) { return w.nStackID == nStackID });

            $tbAddStack.find("tbody tr[nOther]").each(function () {
                var value = parseInt($(this).attr('nOther')) + 1;
                nOther = (value > nOther) ? value : nOther;
            });
            bindDataAdditional(nOther, lstAddAdditional, false);
            $.each($("input[name*=optionStack_][class*=otherPrd]:checked"), function () {
                el = $(this);
                var attrName = "";
                var nGroupCal = "";
                var nOption = "";
                if (el.is(":checked")) {
                    attrName = el.attr("name");
                    nGroupCal = el.attr("name").split("_")[1] + "_" + el.attr("name").split("_")[2];
                    nOption = el.val();
                    var nOptionForHide = $("input[name=" + attrName + "]:not(input[name=" + attrName + "][value=" + nOption + "])").val();
                    $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOption + "]").show();
                    $("tr[nGroupCal=" + nGroupCal + "][nOption=" + nOptionForHide + "]").hide();
                }
            })
            SetTootip();
            $("#divTbAddStack").formValidation('addField', "ddlOther_" + nOther, addValidate_notEmpty(DialogMsg.Specify));
            if (dataStack != null) {
                var dataAdditional = JSON.parse(JSON.stringify(lstAddAdditional));
                $.each(dataAdditional, function (i, el) {
                    el.nID = nOther;
                    el.nStackID = nStackID;
                })
                dataStack.lstDataStack = dataStack.lstDataStack.concat(dataAdditional);
            }
            CheckboxQuarterChangedCustom($tbAddStack.attr("id"));
            //$tbAddStack.tableHeadFixer({ "left": 1 }, { "head": true })
        }

        function bindDataAdditional(nOther, dataValue, IsSubmited) {
            var sHtml = "";
            sHtml += "  <tr style='background-color:#EFEFEF;font-weight:700;' nOther = '" + nOther + "'>";
            sHtml += "      <td class='text-left' style='vertical-align: middle;'>" + bindDDLAdditionalAndCb(dataValue[0].nOtherProductID, nOther, false, dataValue.length == 0 ? false : dataValue[0].cIsVOCs, dataValue[0].nOptionProduct, IsSubmited) + "</td>";
            sHtml += "      <td class='text-center' style='vertical-align: middle;'></td>";
            sHtml += "      <td class='text-center cTarget' style='vertical-align: middle;'></td>";
            sHtml += "      <td class='text-center' style='vertical-align: middle;'></td>";
            sHtml += "      <td class='text-center' style='vertical-align: middle;'></td>";
            sHtml += bindColumnQ(false, null, null, false, true, false, null, null, null, null, null, false);
            sHtml += "  </tr>";
            $.each(dataValue, function (i, el) {
                var lstDataMonth = [el.M1, el.M2, el.M3, el.M4, el.M5, el.M6, el.M7, el.M8, el.M9, el.M10, el.M11, el.M12]
                sHtml += "  <tr " + "nGroupCal ='" + el.nGroupCalc + "_" + nOther + "' " + "nOption='" + el.nOption + "'>";
                sHtml += "      <td class='text-left' style='vertical-align: middle;'>" + el.ProductName + "</td>";
                sHtml += "      <td class='text-center' style='vertical-align: middle;'>" + el.sUnit + "</td>";
                sHtml += "      <td class='text-center cTarget' style='vertical-align: middle;'><input class='form-control input-sm text-right target otherPrd' tableId ='" + $tbAddStack.attr("id") + "' nGroupCal='" + el.nGroupCalc + (nOther != null ? "_" + nOther : "") + "' ProductID ='" + el.ProductID + "'  value ='" + (el.Target != null ? CheckTextInput(el.Target + "") : "") + "' maxlength='20'/></td>";

                var sDisableInputConfirm = el.cTotal == "Y" ? "disabled" : "";
                sHtml += "      <td class='text-center' style='vertical-align: middle;'><input class='form-control input-sm confirm otherPrd' nGroupCal ='" + el.nGroupCalc + "_" + nOther + "' amountMonth='1_6' ProductID ='" + el.ProductID + "' " + sDisableInputConfirm + " cTotal='" + el.cTotal + "' /></td>";
                sHtml += "      <td class='text-center' style='vertical-align: middle;'><input class='form-control input-sm confirm otherPrd' nGroupCal ='" + el.nGroupCalc + "_" + nOther + "' amountMonth='7_12' ProductID ='" + el.ProductID + "' " + sDisableInputConfirm + " cTotal='" + el.cTotal + "'/></td>";
                sHtml += bindColumnQ(false, lstDataMonth, null, false, false, el.cTotal == "Y" ? true : false, el.ProductID, el.cTotal, el.nGroupCalc, nOther, el.nOption, false);
                sHtml += "  </tr>";
            })
            $tbAddStack.find("tbody").append(sHtml);
            $('.flat-green-custom').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_square-green' //'iradio_flat-green'
            });
        }

        function DelAdditional(nOther) {
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                var dataAdditionalCur = Enumerable.From(lstStack).FirstOrDefault(null, function (w) { return w.nStackID == Input("hdfManageID").val() });
                if (dataAdditionalCur != null) {
                    $.each(Enumerable.From(dataAdditionalCur.lstDataStack).Where(function (w) { return w.nID == nOther }).ToArray(), function (i, el) {
                        el.IsDel = true;
                    })

                }
                $tbAddStack.find("tr[nOther=" + nOther + "]").remove();
                $tbAddStack.find("tr[nGroupCal=999_" + nOther + "]").remove();
                HideLoadding()
            })
        }

        function bindDDLAdditionalAndCb(nProductID, nOther, IsDisabled, IsCheck, OptionProductValue, IsSubmited) {
            var ProductID = nProductID == null ? 0 : nProductID;
            var sCheckOption1 = OptionProductValue == "1" ? "checked" : "";
            var sCheckOption2 = OptionProductValue == "2" || OptionProductValue == null ? "checked" : "";
            var sDisabled = IsDisabled ? "disabled" : "";
            var sBtnDel = "<button type='button' title='Delete'  class='btn btn-danger btn-sm' value='" + "" + "' onclick='DelAdditional(" + nOther + ")'><i class='fa fa-trash'></i></button>";

            var sHtml = "";
            var IsSubmitSomeMonth = false;
            $.each(lstStatus, function (i, el) {
                if (el.nStatusID != 0) {
                    IsSubmitSomeMonth = true;
                }
            })
            sHtml += "<div class='form-group col-xs-12 col-sm-9 col-md-9 col-lg-9'>";
            sHtml += "<select class='form-control input-sm' id='ddlOther_" + nOther + "' name='ddlOther_" + nOther + "' " + ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") ? "" : sDisabled) + ">";
            sHtml += "<option value=''>- Please select -</option>";
            $.each(lstOtherPrd, function (i, el) {
                sHtml += "<option value='" + el.nProductID + "' " + (ProductID == el.nProductID ? "selected" : "") + ">" + el.sName + "</option>";
            })
            sHtml += "</select>";
            sHtml += "</div>";
            sHtml += "<div class='form-group col-xs-12 col-sm-3 col-md-3 col-lg-3'>";
            sHtml += "<input type='checkbox' class='flat-green-custom' id='cbOther_" + nOther + "' name='cbOther_" + nOther + "'  " + (IsCheck == "Y" ? "checked" : "") + " /><label style='padding-left:1px'>VOCs</label>";
            sHtml += "</div>";
            sHtml += "<div class='form-group col-xs-12'>";
            sHtml += "&nbsp;<input type='radio'  id='optionStack_1' name='optionStack_999_" + nOther + "' ProductID='" + ProductID + "' class='flat-green-custom otherPrd' value ='1' " + sCheckOption1 + "> <span title='" + sTitleOption1 + "'>Option 1: Emission Load</span> ";
            sHtml += "</br></br>&nbsp;<input type='radio' id='optionStack_2' name='optionStack_999_" + nOther + "' ProductID='" + ProductID + "' class='flat-green-custom otherPrd' value ='2' " + sCheckOption2 + "> <span  title='" + sTitleOption2 + "'>Option 2: Emission Concentration</span>  ";
            sHtml += "</div>"
            sHtml += "<div class='col-xs-12'>";
            if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") || ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27")) {
                sHtml += sBtnDel;
            } else {
                sHtml += (IsFullMonth || (IsSubmitSomeMonth && IsSubmited) || $hdfPRMS.val() == "1") ? "" : sBtnDel;
            }
            sHtml += "</div>"
            return sHtml;
        }

        function EventInput() {
            $divContent.delegate("input.target", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
                var nVal = $(this).val().replace(/,/g, '');
                var tableID = $(this).attr("tableID");
                var ProductID = $(this).attr("ProductID");
                var UnitID = $(this).attr("UnitID");
                var IsAdditional = false;
                var lstData = [];
                switch (tableID) {
                    case $tbCombustion.attr("id"):
                        data = lstCombustion;
                        break;
                    case $tbNonCombustion.attr("id"):
                        data = lstNonCombustion;
                        break;
                    case $tbCEM.attr("id"):
                        data = lstCEM;
                        break;
                    case $tbAdditionalCombustion.attr("id"):
                        data = lstAdditional;
                        IsAdditional = true;
                        break;
                    case $tbAdditionalNonCombustion.attr("id"):
                        data = lstAdditionalNonCombustion;
                        IsAdditional = true;
                        break;
                    case $tbVOC.attr("id"):
                        data = lstVOC;
                        break;
                }
                if (tableID != $tbAddStack.attr("id")) {
                    var item = Enumerable.From(data).FirstOrDefault(null, function (w) { return (IsAdditional ? w.ProductID == ProductID && w.UnitID == UnitID : w.ProductID == ProductID) });
                    if (item != null) {
                        item.Target = nVal;
                    }
                }
            });

            //////////////////////////////////////////////////////////// FOR DIV VOC ////////////////////////////////////////////////////////////
            $tbVOC.delegate("input:not(input[type=checkbox],input.target)", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
                var nMonth = $(this).attr("nMonth");
                var ProductID = $(this).attr("ProductID");
                var nValTotal = "";
                if ($(this).hasClass("total")) {
                    $.each($tbVOC.find("input[class*=total][cTotal=N]"), function (i, el) {
                        el = $(el);
                        var nVal = el.val().replace(/,/g, '');
                        if (CheckTextInput(nVal + "") == "" || CheckTextInput(nVal + "").toLowerCase() == "n/a") {
                            if (nValTotal.toString() == "") {
                                nValTotal = "";
                            }
                        } else {
                            if (nValTotal.toString() == "") {
                                nValTotal = +nVal;
                            } else {
                                nValTotal += +nVal;
                            }
                        }
                    })
                    $tbVOC.find("input[cTotal=Y][class*=total]").val(CheckTextOutput(nValTotal + ""));
                    if (nValTotal.toString() != "") {
                        nValTotal = (Math.round((nValTotal) * 10000000000) / 10000000000)
                    }
                    var dataTotalVOC = Enumerable.From(lstVOC).FirstOrDefault(null, function (w) { return w.cTotal == "Y" });
                    if (dataTotalVOC != null) {
                        dataTotalVOC.nTotal = nValTotal;
                    }
                    var dataVOC = Enumerable.From(lstVOC).FirstOrDefault(null, function (w) { return w.ProductID == ProductID });
                    if (dataVOC != null) {
                        dataVOC.nTotal = $(this).val().replace(/,/g, '');
                    }
                } else {
                    $.each($tbVOC.find("input:not(input[type=checkbox],input.target)[nMonth=" + nMonth + "][cTotal=N]"), function (i, el) {
                        el = $(el);
                        var nVal = el.val().replace(/,/g, '');
                        if (CheckTextInput(nVal + "") == "" || CheckTextInput(nVal + "").toLowerCase() == "n/a") {
                            if (nValTotal.toString() == "") {
                                nValTotal = "";
                            }
                        } else {
                            if (nValTotal.toString() == "") {
                                nValTotal = +nVal;
                            } else {
                                nValTotal += +nVal;
                            }
                        }
                    })
                    $tbVOC.find("input[cTotal=Y][nMonth=" + nMonth + "]").val(CheckTextOutput(nValTotal + ""));
                    if (nValTotal.toString() != "") {
                        nValTotal = (Math.round((nValTotal) * 10000000000) / 10000000000)
                    }
                    var dataTotalVOC = Enumerable.From(lstVOC).FirstOrDefault(null, function (w) { return w.cTotal == "Y" });
                    if (dataTotalVOC != null) {
                        nValTotal = nValTotal.toString().replace(/,/g, '');
                        switch (+nMonth) {
                            case 1: dataTotalVOC.M1 = nValTotal; break;
                            case 2: dataTotalVOC.M2 = nValTotal; break;
                            case 3: dataTotalVOC.M3 = nValTotal; break;
                            case 4: dataTotalVOC.M4 = nValTotal; break;
                            case 5: dataTotalVOC.M5 = nValTotal; break;
                            case 6: dataTotalVOC.M6 = nValTotal; break;
                            case 7: dataTotalVOC.M7 = nValTotal; break;
                            case 8: dataTotalVOC.M8 = nValTotal; break;
                            case 9: dataTotalVOC.M9 = nValTotal; break;
                            case 10: dataTotalVOC.M10 = nValTotal; break;
                            case 11: dataTotalVOC.M11 = nValTotal; break;
                            case 12: dataTotalVOC.M12 = nValTotal; break;
                        }
                    }

                    var dataVOC = Enumerable.From(lstVOC).FirstOrDefault(null, function (w) { return w.ProductID == ProductID });
                    if (dataVOC != null) {
                        switch (+nMonth) {
                            case 1: dataVOC.M1 = $(this).val().replace(/,/g, ''); break;
                            case 2: dataVOC.M2 = $(this).val().replace(/,/g, ''); break;
                            case 3: dataVOC.M3 = $(this).val().replace(/,/g, ''); break;
                            case 4: dataVOC.M4 = $(this).val().replace(/,/g, ''); break;
                            case 5: dataVOC.M5 = $(this).val().replace(/,/g, ''); break;
                            case 6: dataVOC.M6 = $(this).val().replace(/,/g, ''); break;
                            case 7: dataVOC.M7 = $(this).val().replace(/,/g, ''); break;
                            case 8: dataVOC.M8 = $(this).val().replace(/,/g, ''); break;
                            case 9: dataVOC.M9 = $(this).val().replace(/,/g, ''); break;
                            case 10: dataVOC.M10 = $(this).val().replace(/,/g, ''); break;
                            case 11: dataVOC.M11 = $(this).val().replace(/,/g, ''); break;
                            case 12: dataVOC.M12 = $(this).val().replace(/,/g, ''); break;
                        }
                    }
                }
            });

            $tbVOC.delegate("input[type=checkbox]", "ifChanged", function () {
                var curEl = $(this);
                var nValForAttr = "";
                var sAttr = "";
                var sTxtAction = "";
                var IsActionInRow = false;
                if (curEl.attr("nMonth")) {
                    nValForAttr = +curEl.attr("nMonth");
                    sAttr = "nMonth";
                    sTxtAction = arrFullMonth[nValForAttr - 1];
                } else {
                    IsActionInRow = true;
                    nValForAttr = +curEl.attr("ProductID");
                    sAttr = "ProductID";
                    var dataVOC = Enumerable.From(lstVOC).FirstOrDefault(null, function (w) { return w.ProductID == nValForAttr });
                    sTxtAction = dataVOC != null ? dataVOC.ProductName : "";
                }
                if (curEl.is(":checked")) {
                    var sComfirm = "The action comes into effect in " + sTxtAction + ", all data within this period will be automatically " + (IsActionInRow ? "reset to zero all month" : "reset to zero in month") + ".<br/> Do you want save data ?";
                    DialogConfirmCloseButton(DialogHeader.Confirm, sComfirm, function () {
                        $.each($tbVOC.find("input:not(input[type=checkbox],input.target)[" + sAttr + "=" + nValForAttr + "]" + (IsActionInRow ? ":enabled" : "")), function (i, el) {
                            el = $(el);
                            var IscTotal = el.attr("cTotal") == "Y" ? true : false;
                            el.val(IscTotal ? CheckTextOutput("0") : (CheckTextInput("0")));
                            if (IsActionInRow) {
                                if (el.attr("cTotal") != "Y") el.change();
                            }
                        })

                        if (!IsActionInRow) {
                            $tbVOC.find("input:not(input[type=checkbox],input.target)[" + sAttr + "=" + nValForAttr + "][cTotal=N]").prop("disabled", true);
                            var IsCheck = curEl.is(":checked") ? "Y" : "N";
                            $.each(lstVOC, function (i, incData) {
                                switch (nValForAttr) {
                                    case 1:
                                        incData.IsCheckM1 = IsCheck;
                                        incData.M1 = "0";
                                        break;
                                    case 2:
                                        incData.IsCheckM2 = IsCheck;
                                        incData.M2 = "0";
                                        break;
                                    case 3:
                                        incData.IsCheckM3 = IsCheck;
                                        incData.M3 = "0";
                                        break;
                                    case 4:
                                        incData.IsCheckM4 = IsCheck;
                                        incData.M4 = "0";
                                        break;
                                    case 5:
                                        incData.IsCheckM5 = IsCheck;
                                        incData.M5 = "0";
                                        break;
                                    case 6:
                                        incData.IsCheckM6 = IsCheck;
                                        incData.M6 = "0";
                                        break;
                                    case 7:
                                        incData.IsCheckM7 = IsCheck;
                                        incData.M7 = "0";
                                        break;
                                    case 8:
                                        incData.IsCheckM8 = IsCheck;
                                        incData.M8 = "0";
                                        break;
                                    case 9:
                                        incData.IsCheckM9 = IsCheck;
                                        incData.M9 = "0";
                                        break;
                                    case 10:
                                        incData.IsCheckM10 = IsCheck;
                                        incData.M10 = "0";
                                        break;
                                    case 11:
                                        incData.IsCheckM11 = IsCheck;
                                        incData.M11 = "0";
                                        break;
                                    case 12:
                                        incData.IsCheckM12 = IsCheck;
                                        incData.M12 = "0";
                                        break;
                                }
                            })
                        }
                        HideLoadding();
                    }, function () {
                        curEl.prop("checked", false);
                        curEl.iCheck('update');
                    });
                } else {
                    $.each($tbVOC.find("input:not(input[type=checkbox],input.target)[" + sAttr + "=" + nValForAttr + "]" + (IsActionInRow ? ":enabled" : "")), function (i, el) {
                        el = $(el);
                        el.val("");
                        if (IsActionInRow) {
                            if (el.attr("cTotal") != "Y") el.change();
                        }
                    })
                    if (!IsActionInRow) {
                        $tbVOC.find("input:not(input[type=checkbox],input.target)[" + sAttr + "=" + nValForAttr + "][cTotal=N]").prop("disabled", false);
                    }

                    var IsCheck = curEl.is(":checked") ? "Y" : "N";
                    $.each(lstVOC, function (i, incData) {
                        switch (nValForAttr) {
                            case 1:
                                incData.IsCheckM1 = IsCheck;
                                incData.M1 = "";
                                break;
                            case 2:
                                incData.IsCheckM2 = IsCheck;
                                incData.M2 = "";
                                break;
                            case 3:
                                incData.IsCheckM3 = IsCheck;
                                incData.M3 = "";
                                break;
                            case 4:
                                incData.IsCheckM4 = IsCheck;
                                incData.M4 = "";
                                break;
                            case 5:
                                incData.IsCheckM5 = IsCheck;
                                incData.M5 = "";
                                break;
                            case 6:
                                incData.IsCheckM6 = IsCheck;
                                incData.M6 = "";
                                break;
                            case 7:
                                incData.IsCheckM7 = IsCheck;
                                incData.M7 = "";
                                break;
                            case 8:
                                incData.IsCheckM8 = IsCheck;
                                incData.M8 = "";
                                break;
                            case 9:
                                incData.IsCheckM9 = IsCheck;
                                incData.M9 = "";
                                break;
                            case 10:
                                incData.IsCheckM10 = IsCheck;
                                incData.M10 = "";
                                break;
                            case 11:
                                incData.IsCheckM11 = IsCheck;
                                incData.M11 = "";
                                break;
                            case 12:
                                incData.IsCheckM12 = IsCheck;
                                incData.M12 = "";
                                break;
                        }
                    })
                }
            });

            $("select[id$=ddlVOCType]").on("change", function () {
                el = $(this);
                if (el.val() == "Y") {
                    ArrInputFromTableID = $.grep(ArrInputFromTableID, function (value) {
                        return value != $tbVOC.attr("id");
                    })
                    bindDataTable($tbVOC, lstVOC, false, true, false, false);
                    $tbVOC.width(0);
                } else {
                    ArrInputFromTableID.push($tbVOC.attr("id"));
                    bindDataTable($tbVOC, lstVOC, true, true, false, false);
                    CheckboxQuarterChangedCustom($tbVOC.attr("id"));
                }
                $.map(lstVOC, function (n) {
                    n.sOption = el.val();
                })
                $('.flat-green-custom').iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_square-green' //'iradio_flat-green'
                });
            })
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////// FOR DIV ADD STACK ////////////////////////////////////////////////////////////
            $tbAddStack.delegate("input:not(input[type=checkbox],input.target,input[class*=confirm])", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
                if ($(this).attr("ProductID") == "174" && (+$(this).val().replace(/,/g, '') > 20.9 || $(this).val().replace(/,/g, '').toLowerCase() == "n/a")) {
                    $(this).val("");
                    DialogWarning(DialogHeader.Warning, "Oxygen content must be less then 20.9%");
                }
                CalculateDataStack(this);
            })

            $("div[id$=divCreate]").delegate("input[id*=rblTypeStack]", "ifChanged", function () {
                el = $(this);
                if (el.is(":checked")) {
                    var lstCheckInMonthCombustion = $.map($tbCombustion.find("input[type=checkbox]"), function (item) {
                        return $(item).is(":checked") ? "Y" : "N";
                    });
                    if (el.val() == "CMS") {
                        $tbAddStackCEM.show();
                        for (var i = 1 ; i <= 12; i++) {
                            if (lstStatus[i - 1].nStatusID == 0) {
                                if (lstCheckInMonthCombustion[i - 1] == "Y") {
                                    $tbAddStack.find("input[nMonth=" + i + "]:not(input[type=checkbox])").prop("disabled", true);
                                } else {
                                    $tbAddStack.find("input[nMonth=" + i + "]:not(input[type=checkbox],input[cTotal=Y])").prop("disabled", false);
                                }
                            }
                        }
                    } else {
                        $tbAddStackCEM.hide();
                        for (var i = 1 ; i <= 12; i++) {
                            if (lstStatus[i - 1].nStatusID == 0) {
                                $tbAddStack.find("input[nMonth=" + i + "]:not(input[type=checkbox],input[cTotal=Y])").prop("disabled", false);
                            }
                        }
                    }
                }
            })

            $("div[id$=divCreate]").delegate("th button", "click", function () {
                elButton = $(this);
                DialogConfirm(DialogHeader.Confirm, "Do you want to change value ?", function () {
                    var table = $("table[id$=" + elButton.attr("tableID") + "]");
                    var amountMonth = elButton.attr("amountMonth");
                    var StartMonth = +amountMonth.split("_")[0];
                    var EndMonth = +amountMonth.split("_")[1];
                    var arrInputConfirm = table.find("input[amountMonth=" + amountMonth + "][cTotal=N]");
                    $.each(arrInputConfirm, function (i, el) {
                        el = $(el);
                        var ProductID = el.attr("ProductID");
                        var nVal = el.val().replace(/,/g, '') + "";
                        var IsOther = el.hasClass("otherPrd");
                        for (var month = StartMonth ; month <= EndMonth ; month++) {
                            if (IsOther) {
                                var nGroupCal = el.attr("nGroupCal");
                                table.find("input[ProductID=" + ProductID + "][nGroupCal=" + nGroupCal + "][cTotal=N][nMonth=" + month + "]:enabled").val(CheckTextInput(nVal)).change();
                            } else {
                                table.find("input[ProductID=" + ProductID + "][cTotal=N][nMonth=" + month + "]:enabled").val(CheckTextInput(nVal)).change();
                            }
                        }
                    })
                    HideLoadding();
                }, function () { HideLoadding(); })
            })

            $("div[id$=divCreate]").delegate("input[class*=confirm]", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
                if ($(this).attr("ProductID") == "174" && (+$(this).val().replace(/,/g, '') > 20.9 || $(this).val().replace(/,/g, '').toLowerCase() == "n/a")) {
                    $(this).val("");
                    DialogWarning(DialogHeader.Warning, "Oxygen content must be less then 20.9%");
                }
            })
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////// FOR DIV Combustion ////////////////////////////////////////////////////////////
            $tbCombustion.delegate("input[type=checkbox]", "ifChanged", function () {
                var el = $(this);
                var nMonth = +el.attr("nMonth");
                if (el.is(":checked")) {
                    var sComfirm = "The action comes into effect in " + arrFullMonth[nMonth - 1] + ", all data within this period will be automatically reset to zero in month.<br/> Do you want save data ?";
                    DialogConfirmCloseButton(DialogHeader.Confirm, sComfirm, function () {
                        $.each(lstCombustion, function (i, incData) {
                            var IsCheck = el.is(":checked") ? "Y" : "N";
                            switch (nMonth) {
                                case 1:
                                    incData.IsCheckM1 = IsCheck;
                                    incData.M1 = "0";
                                    break;
                                case 2:
                                    incData.IsCheckM2 = IsCheck;
                                    incData.M2 = "0";
                                    break;
                                case 3:
                                    incData.IsCheckM3 = IsCheck;
                                    incData.M3 = "0";
                                    break;
                                case 4:
                                    incData.IsCheckM4 = IsCheck;
                                    incData.M4 = "0";
                                    break;
                                case 5:
                                    incData.IsCheckM5 = IsCheck;
                                    incData.M5 = "0";
                                    break;
                                case 6:
                                    incData.IsCheckM6 = IsCheck;
                                    incData.M6 = "0";
                                    break;
                                case 7:
                                    incData.IsCheckM7 = IsCheck;
                                    incData.M7 = "0";
                                    break;
                                case 8:
                                    incData.IsCheckM8 = IsCheck;
                                    incData.M8 = "0";
                                    break;
                                case 9:
                                    incData.IsCheckM9 = IsCheck;
                                    incData.M9 = "0";
                                    break;
                                case 10:
                                    incData.IsCheckM10 = IsCheck;
                                    incData.M10 = "0";
                                    break;
                                case 11:
                                    incData.IsCheckM11 = IsCheck;
                                    incData.M11 = "0";
                                    break;
                                case 12:
                                    incData.IsCheckM12 = IsCheck;
                                    incData.M12 = "0";
                                    break;
                            }
                        })
                        $.each(Enumerable.From(lstStack).Where(function (w) { return w.sStackType == "CMS" }).ToArray(), function (i, el) {
                            $.each(el.lstDataStack, function (i2, incData) {
                                switch (nMonth) {
                                    case 1:
                                        incData.M1 = "0";
                                        break;
                                    case 2:
                                        incData.M2 = "0";
                                        break;
                                    case 3:
                                        incData.M3 = "0";
                                        break;
                                    case 4:
                                        incData.M4 = "0";
                                        break;
                                    case 5:
                                        incData.M5 = "0"
                                    case 6:
                                        incData.M6 = "0";
                                        break;
                                    case 7:
                                        incData.M7 = "0";
                                        break;
                                    case 8:
                                        incData.M8 = "0";
                                        break;
                                    case 9:
                                        incData.M9 = "0";
                                        break;
                                    case 10:
                                        incData.M10 = "0";
                                        break;
                                    case 11:
                                        incData.M11 = "0";
                                        break;
                                    case 12:
                                        incData.M12 = "0";
                                        break;
                                }
                            })
                            $.each(el.lstDataStackCEM, function (i2, incData) {
                                switch (nMonth) {
                                    case 1:
                                        incData.M1 = "0";
                                        break;
                                    case 2:
                                        incData.M2 = "0";
                                        break;
                                    case 3:
                                        incData.M3 = "0";
                                        break;
                                    case 4:
                                        incData.M4 = "0";
                                        break;
                                    case 5:
                                        incData.M5 = "0"
                                    case 6:
                                        incData.M6 = "0";
                                        break;
                                    case 7:
                                        incData.M7 = "0";
                                        break;
                                    case 8:
                                        incData.M8 = "0";
                                        break;
                                    case 9:
                                        incData.M9 = "0";
                                        break;
                                    case 10:
                                        incData.M10 = "0";
                                        break;
                                    case 11:
                                        incData.M11 = "0";
                                        break;
                                    case 12:
                                        incData.M12 = "0";
                                        break;
                                }
                            })
                        })
                        $.each($tbCombustion.find("input[nMonth=" + nMonth + "]"), function (i, el) {
                            $(el).val(CheckTextOutput("0"));
                        })
                        $.each($tbAdditionalCombustion.find("input[nMonth=" + nMonth + "]"), function (i, el) {
                            $(el).val(CheckTextOutput("0"));
                        })
                        ReCalculateAll();
                    }, function () {
                        el.prop("checked", false);
                        el.iCheck('update');
                    })
                } else {
                    var nMonth = +el.attr("nMonth");
                    var IsCheck = el.is(":checked") ? "Y" : "N";
                    $.each(lstCombustion, function (i, incData) {
                        switch (nMonth) {
                            case 1:
                                incData.IsCheckM1 = IsCheck;
                                incData.M1 = "";
                                break;
                            case 2:
                                incData.IsCheckM2 = IsCheck;
                                incData.M2 = "";
                                break;
                            case 3:
                                incData.IsCheckM3 = IsCheck;
                                incData.M3 = "";
                                break;
                            case 4:
                                incData.IsCheckM4 = IsCheck;
                                incData.M4 = "";
                                break;
                            case 5:
                                incData.IsCheckM5 = IsCheck;
                                incData.M5 = "";
                                break;
                            case 6:
                                incData.IsCheckM6 = IsCheck;
                                incData.M6 = "";
                                break;
                            case 7:
                                incData.IsCheckM7 = IsCheck;
                                incData.M7 = "";
                                break;
                            case 8:
                                incData.IsCheckM8 = IsCheck;
                                incData.M8 = "";
                                break;
                            case 9:
                                incData.IsCheckM9 = IsCheck;
                                incData.M9 = "";
                                break;
                            case 10:
                                incData.IsCheckM10 = IsCheck;
                                incData.M10 = "";
                                break;
                            case 11:
                                incData.IsCheckM11 = IsCheck;
                                incData.M11 = "";
                                break;
                            case 12:
                                incData.IsCheckM12 = IsCheck;
                                incData.M12 = "";
                                break;
                        }
                    })
                    $.each(Enumerable.From(lstStack).Where(function (w) { return w.sStackType == "CMS" }).ToArray(), function (i, el) {
                        $.each(el.lstDataStack, function (i2, incData) {
                            switch (nMonth) {
                                case 1:
                                    incData.M1 = "";
                                    break;
                                case 2:
                                    incData.M2 = "";
                                    break;
                                case 3:
                                    incData.M3 = "";
                                    break;
                                case 4:
                                    incData.M4 = "";
                                    break;
                                case 5:
                                    incData.M5 = ""
                                case 6:
                                    incData.M6 = "";
                                    break;
                                case 7:
                                    incData.M7 = "";
                                    break;
                                case 8:
                                    incData.M8 = "";
                                    break;
                                case 9:
                                    incData.M9 = "";
                                    break;
                                case 10:
                                    incData.M10 = "";
                                    break;
                                case 11:
                                    incData.M11 = "";
                                    break;
                                case 12:
                                    incData.M12 = "";
                                    break;
                            }
                        })
                        $.each(el.lstDataStackCEM, function (i2, incData) {
                            switch (nMonth) {
                                case 1:
                                    incData.M1 = "";
                                    break;
                                case 2:
                                    incData.M2 = "";
                                    break;
                                case 3:
                                    incData.M3 = "";
                                    break;
                                case 4:
                                    incData.M4 = "";
                                    break;
                                case 5:
                                    incData.M5 = ""
                                case 6:
                                    incData.M6 = "";
                                    break;
                                case 7:
                                    incData.M7 = "";
                                    break;
                                case 8:
                                    incData.M8 = "";
                                    break;
                                case 9:
                                    incData.M9 = "";
                                    break;
                                case 10:
                                    incData.M10 = "";
                                    break;
                                case 11:
                                    incData.M11 = "";
                                    break;
                                case 12:
                                    incData.M12 = "";
                                    break;
                            }
                        })
                    })
                    $.each($tbCombustion.find("input[nMonth=" + nMonth + "]"), function (i, el) {
                        $(el).val(CheckTextOutput(""));
                    })
                    $.each($tbAdditionalCombustion.find("input[nMonth=" + nMonth + "]"), function (i, el) {
                        $(el).val(CheckTextOutput(""));
                    })
                    ReCalculateAll();
                    var param = {
                        sIndID: Select("ddlIndicator").val(),
                        sOprtID: Select("ddlOperationType").val(),
                        sFacID: Select("ddlFacility").val(),
                        sYear: Select("Year").val()
                    }
                    ReCalculateAll();
                }

            })
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }

        function CalculateDataStack(element) {
            var el = $(element);
            var ProductID = el.attr("ProductID");
            var nOption = el.attr("nOption");
            var cTotal = el.attr("cTotal");
            var nGroupCal = el.attr("nGroupCal");
            var nMonth = el.attr("nMonth");
            var nVal = el.val().replace(/,/g, '');
            var IsOther = el.hasClass("otherPrd");
            var actuaPercentlO2 = $("input[ProductID=174][nMonth=" + nMonth + "]").val().replace(/,/g, '');
            if (ProductID != "172" && ProductID != "173" && ProductID != "174" && cTotal == "N") {
                //if (nOption == "2") {
                var SumValue = CalculateEmissionConcentration7Percent(actuaPercentlO2, nVal) + "";
                if (nVal.toLowerCase() == "n/a") {
                    SumValue = "";
                }
                $("input[nGroupCal=" + nGroupCal + "][nMonth=" + nMonth + "][cTotal=Y][nOption=" + nOption + "]").val(CheckTextOutput(SumValue)).attr("nTrueVal", SumValue);
                //}
            } else if (ProductID == "174") {
                $.each($tbAddStack.find("input:not(input[type=checkbox],input.target)[nMonth=" + nMonth + "][nOption=2][cTotal=N]"), function (i, el) {
                    el = $(el);
                    el.change();
                })
            }
        }

        //Option 2 : Emission concentration (at 7% O2 content)
        function CalculateEmissionConcentration7Percent(actuaPercentlO2, actualO2Content) {
            // <param name="actuaPercentlO2">General Info : Actual %O2 content at measurement</param>
            // <param name="actualO2Content">Option 2 : Emission concentration (at actual O2 content)</param>
            // สูตร
            //var result = actualO2Content * ((20.9m - 7m) / (20.9m - actuaPercentlO2));

            var result = "";
            var v1 = parseFloat("20.9");
            var v2 = parseFloat("7");
            if (actuaPercentlO2 != "" && actualO2Content != "") {
                var nactuaPercentlO2 = parseFloat(actuaPercentlO2);
                var nactualO2Content = parseFloat(actualO2Content);
                result = nactualO2Content * ((v1 - v2) / (v1 - nactuaPercentlO2));
            }
            return result;
        }

        function bindValidateFormInput(elementName, sSpecify, objControl) {
            var objValidate = {};
            objValidate[GetElementName(elementName, objControl)] = addValidate_notEmpty(DialogMsg.Specify + " " + sSpecify);
            BindValidate("divTbAddStack", objValidate);
        }

        function saveStack() {
            if (CheckValidateCustomForAddStack("divTbAddStack")) {
                var IsDuplicate = false;
                var arrCheckDup = $.map($tbAddStack.find("select[name*=ddlOther]"), function (el) {
                    el = $(el);
                    return el.val();
                })
                for (var key in arrCheckDup) {
                    var exitsValue = $.map(arrCheckDup, function (n, i) {
                        if (n == arrCheckDup[key]) {
                            return i;
                        }
                    }).length;
                    if (exitsValue > 1) {
                        IsDuplicate = true;
                    }
                }
                if (!IsDuplicate) {
                    LoaddinProcess();
                    var nStackID = Input("hdfManageID").val();
                    var dataStack = Enumerable.From(lstStack).FirstOrDefault(null, function (w) { return w.nStackID == nStackID });
                    if (dataStack != null) {
                        dataStack.sStackName = Input("txtStackName").val(),
                        dataStack.sRemark = GetValTextArea("txtRemarkStack"),
                        dataStack.sStackType = $("input[id*=rblTypeStack]:checked").val(),
                        dataStack.IsSaved = true;
                        $.each($tbAddStack.find("tbody input:not(input[type=radio],input[type=checkbox],input[type=radio][class*=otherPrd],input[class*=confirm]),input[type=radio]:checked"), function (i, el) {
                            el = $(el);
                            var nProductID = el.attr("ProductID");
                            var nMonth = el.attr("nMonth");
                            var IsCheck = false;
                            if ($("input[id*=rblTypeStack]:checked").val() == "CMS") {
                                IsCheck = $tbCombustion.find("input[type=checkbox][nMonth=" + nMonth + "]").is(":checked") ? true : false;
                            }
                            var nVal = el.is(":visible") ? (IsCheck ? "0" : el.val().replace(/,/g, '')) + "" : "";
                            var data = Enumerable.From(dataStack.lstDataStack).FirstOrDefault(null, function (w) { return w.ProductID == nProductID });
                            if (el.attr("type") == "radio") {
                                if (data != null) {
                                    data.IsSaved = true;
                                    data.nOptionProduct = nVal;
                                }
                            } else {
                                if (data != null) {
                                    data.IsSaved = true;
                                    if (el.hasClass("otherPrd")) {
                                        var nOther = el.attr("nGroupCal").split("_")[1];
                                        var nOtherProductID = $("select[id$=ddlOther_" + nOther + "]").val();
                                        var cIsVOCs = $("input[id$=cbOther_" + nOther + "]").is(":checked") ? "Y" : "N";
                                        var nOptionProduct = $("input[name*=optionStack_999_" + nOther + "]:checked").val();
                                        data = Enumerable.From(dataStack.lstDataStack).FirstOrDefault(null, function (w) { return w.ProductID == nProductID && w.nID == nOther });
                                        if (data != null) {
                                            data.IsSaved = true;
                                            if (el.hasClass("target")) {
                                                data.Target = nVal;
                                            }
                                            data.nOtherProductID = nOtherProductID;
                                            data.cIsVOCs = cIsVOCs;
                                            data.nOptionProduct = nOptionProduct;
                                        }
                                    }
                                    if (el.hasClass("target") && !el.hasClass("otherPrd")) {
                                        data.IsSaved = true;
                                        data.Target = nVal;
                                    } else {
                                        var nOptionProduct = 0;
                                        var IsHasHead = false;
                                        if (data.cSetCode != null) {
                                            if (data.cSetCode.split("-").length > 1) {
                                                IsHasHead = true;
                                            }
                                        }
                                        if (IsHasHead) {
                                            var dataHead = Enumerable.From(dataStack.lstDataStack).FirstOrDefault(null, function (w) { return w.cSetCode == data.cSetCode.split("-")[0] });
                                            if (dataHead != null) {
                                                data.nOptionProduct = dataHead.nOptionProduct;
                                            }
                                        }
                                        if (el.attr("cTotal") == "Y") {
                                            nVal = el.is(":visible") ? (IsCheck ? "0" : el.attr("nTrueVal")) : "";
                                        }
                                        switch (+nMonth) {
                                            case 1:
                                                data.M1 = nVal;
                                                break;
                                            case 2:
                                                data.M2 = nVal;
                                                break;
                                            case 3:
                                                data.M3 = nVal;
                                                break;
                                            case 4:
                                                data.M4 = nVal;
                                                break;
                                            case 5:
                                                data.M5 = nVal;
                                                break;
                                            case 6:
                                                data.M6 = nVal;
                                                break;
                                            case 7:
                                                data.M7 = nVal;
                                                break;
                                            case 8:
                                                data.M8 = nVal;
                                                break;
                                            case 9:
                                                data.M9 = nVal;
                                                break;
                                            case 10:
                                                data.M10 = nVal;
                                                break;
                                            case 11:
                                                data.M11 = nVal;
                                                break;
                                            case 12:
                                                data.M12 = nVal;
                                                break;
                                        }
                                    }
                                }
                            }
                        })
                        if ($("input[id*=rblTypeStack]:checked").val() == "CMS") {
                            $.each($tbAddStackCEM.find("tbody input:not(input[class*=confirm])"), function (i, el) {
                                el = $(el);
                                var nProductID = el.attr("ProductID");
                                var nMonth = el.attr("nMonth");
                                var IsCheck = false;
                                if ($("input[id*=rblTypeStack]:checked").val() == "CMS") {
                                    IsCheck = $tbCombustion.find("input[type=checkbox][nMonth=" + nMonth + "]").is(":checked") ? true : false;
                                }
                                var nVal = el.is(":visible") ? (IsCheck ? "0" : el.val().replace(/,/g, '')) + "" : "";
                                var data = Enumerable.From(dataStack.lstDataStackCEM).FirstOrDefault(null, function (w) { return w.ProductID == nProductID });
                                if (data != null) {
                                    data.IsSaved = true;
                                    if (el.hasClass("target")) {
                                        data.IsSaved = true;
                                        data.Target = nVal;
                                    } else {
                                        var nOptionProduct = 0;
                                        var IsHasHead = false;
                                        if (data.cSetCode != null) {
                                            if (data.cSetCode.split("-").length > 1) {
                                                IsHasHead = true;
                                            }
                                        }
                                        if (IsHasHead) {
                                            var dataHead = Enumerable.From(dataStack.lstDataStackCEM).FirstOrDefault(null, function (w) { return w.cSetCode == data.cSetCode.split("-")[0] });
                                            if (dataHead != null) {
                                                dataHead.IsSaved = true;
                                                data.nOptionProduct = dataHead.nOptionProduct;
                                            }
                                        }
                                        if (el.attr("cTotal") == "Y") {
                                            nVal = el.is(":visible") ? (IsCheck ? "0" : el.attr("nTrueVal")) : "";
                                        }
                                        switch (+nMonth) {
                                            case 1:
                                                data.M1 = nVal;
                                                break;
                                            case 2:
                                                data.M2 = nVal;
                                                break;
                                            case 3:
                                                data.M3 = nVal;
                                                break;
                                            case 4:
                                                data.M4 = nVal;
                                                break;
                                            case 5:
                                                data.M5 = nVal;
                                                break;
                                            case 6:
                                                data.M6 = nVal;
                                                break;
                                            case 7:
                                                data.M7 = nVal;
                                                break;
                                            case 8:
                                                data.M8 = nVal;
                                                break;
                                            case 9:
                                                data.M9 = nVal;
                                                break;
                                            case 10:
                                                data.M10 = nVal;
                                                break;
                                            case 11:
                                                data.M11 = nVal;
                                                break;
                                            case 12:
                                                data.M12 = nVal;
                                                break;
                                        }
                                    }
                                }
                            })
                            dataStack.lstDataStackCEM = Enumerable.From(dataStack.lstDataStackCEM).Where(function (w) { return w.IsSaved == true && w.IsDel != true }).ToArray();
                        } else {
                            dataStack.lstDataStackCEM = JSON.parse(JSON.stringify(lstAddStackCEM));
                            $.each(dataStack.lstDataStackCEM, function (i, el) {
                                el.nStackID = Input("hdfManageID").val();
                            })
                        }
                        dataStack.lstDataStack = Enumerable.From(dataStack.lstDataStack).Where(function (w) { return w.IsSaved == true && w.IsDel != true }).ToArray();
                    }
                    ReCalculateAll(function () {
                        $("input[id*=rblTypeStack_]:first").iCheck("check");
                        Input("txtStackName").val("");
                        SetValueTextArea("txtRemarkStack", "");
                        UpdateStatusValidateControl("divTbAddStack", "txtStackName", "NOT_VALIDATED");
                        $btnAddStack.prop("disabled", false);
                        $("div[id$=dvbtn]").find("button").prop("disabled", false);
                        $tbCombustion.find("input[type=checkbox]").not("[class$=submited]").prop("disabled", false);
                        bindDataStack();
                        $divTbAddStack.hide();
                        Input("hdfManageID").val("");
                        ScrollTopToElementsTo("divAddStack", 85);
                    });
                } else {
                    DialogWarning(DialogHeader.Warning, "Duplicate additional.");
                }
            }
        }

        function ReCalculateAll(fnComplete) {
            fnComplete = fnComplete != null && fnComplete != undefined ? fnComplete : function () { };
            var param = {
                sIndID: Select("ddlIndicator").val(),
                sOprtID: Select("ddlOperationType").val(),
                sFacID: Select("ddlFacility").val(),
                sYear: Select("Year").val()
            }
            AjaxCallWebMethod("CalculateToTable", function (response) {
                lstCombustion = [];
                lstNonCombustion = [];
                lstCEM = [];
                lstAdditional = [];
                lstAdditionalNonCombustion = [];
                var objData = response.d;
                var objDataEmission = response.d.objDataEmission;
                lstCombustion = objDataEmission.lstCombustion;
                lstNonCombustion = objDataEmission.lstNonCombustion;
                lstCEM = objDataEmission.lstCEM;
                lstAdditional = objDataEmission.lstAdditional;
                lstAdditionalNonCombustion = objDataEmission.lstAdditionalNonCombustion;
                bindDataTable($tbCombustion, lstCombustion, true, false, false, false);
                bindDataTable($tbNonCombustion, lstNonCombustion, false, false, false, false);
                bindDataTable($tbCEM, lstCEM, false, false, false, false);
                bindDataTable($tbAdditionalCombustion, lstAdditional, false, false, true, false);
                bindDataTable($tbAdditionalNonCombustion, lstAdditionalNonCombustion, false, false, true, false);
            }, function () {
                $('.flat-green-custom').iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_square-green' //'iradio_flat-green'
                });
                fnComplete();
                CheckboxQuarterChanged();
                HideLoadding();
            }, {
                param: param,
                lstStack: lstStack,
                lstDataCombustion: lstCombustion,
                lstDataNonCombustion: lstNonCombustion,
                lstDataCEM: lstCEM,
                lstDataAdditional: lstAdditional,
                lstDataAdditionalNonCombustion: lstAdditionalNonCombustion,
            })
        }

        function SaveData(nStatus) {
            var IsPass = true;
            var sMsg = "";
            var sMsgComfirmAlert = DialogMsg.ConfirmSave;
            var sMsgComplete = DialogMsg.SaveComplete;
            switch (+nStatus) {
                case 0: sMsgComfirmAlert = DialogMsg.ConfirmSaveDraft;
                    var sMsgComplete = DialogMsg.SaveDraftComplete;
                    break;
                case 1: sMsgComfirmAlert = DialogMsg.ConfirmSubmit;
                    var sMsgComplete = DialogMsg.SubmitComplete;
                    break;
                case 24: sMsgComfirmAlert = DialogMsg.ConfirmRecall;
                    var sMsgComplete = DialogMsg.RecallComplete;
                    break;
                case 9999: sMsgComfirmAlert = DialogMsg.ConfirmSave;
                    var sMsgComplete = DialogMsg.SaveComplete;
                    break;
                case 2: sMsgComfirmAlert = DialogMsg.ConfirmRequest;
                    var sMsgComplete = DialogMsg.RequestComplete;
                    break;
                case 27: sMsgComfirmAlert = DialogMsg.ConfirmApprove;
                    var sMsgComplete = DialogMsg.ApproveComplete;
                    break;
            }

            var arrIncDataProductID = [160, 162, 164];
            var arrIncCheck = $.map($tbCombustion.find("input[type=checkbox]"), function (el) {
                return $(el).is(":checked") ? "Y" : "N";
            });
            var IsCheckValueThisMonth = false;
            if (nStatus == 0 || nStatus == 9999) {
                IsPass = false;
                for (var i = 0; i < 12 ; i++) {

                    if ($hdfsddlOperationType.val() == "13") {
                        //IsPass = false;
                        $.each($tbVOC.find("tbody input[cTotal=Y][nMonth=" + (i + 1) + "]:not(input.target)"), function (i, el) {
                            el = $(el);
                            if (el.val() != "") {
                                IsPass = true;
                            }
                        })
                    } else {
                        IsCheckValueThisMonth = false;
                        $.each($tbCombustion.find("input:not(input[type=checkbox],input.target)[nMonth=" + (i + 1) + "]"), function (i, el) {
                            el = $(el);
                            if (arrIncDataProductID.indexOf(+el.attr("ProductID")) > -1 && el.val() != "") {
                                IsCheckValueThisMonth = true
                            }
                        })
                        if (arrIncCheck[i] == "Y" || IsCheckValueThisMonth) {
                            IsPass = true;
                        }
                    }
                }
                if (!IsPass) {
                    DialogWarning(DialogHeader.Warning, "Please specify data");
                    return false;
                }
            } else if (nStatus == 24 || nStatus == 2) {
                IsPass = true;
            } else {
                for (var i = 0; i < 12 ; i++) {
                    var IsThisMonth = arrMonth.indexOf((i + 1) + "") > -1;
                    if (IsThisMonth) {

                        if ($hdfsddlOperationType.val() == "13") {
                            //IsPass = false;
                            $.each($tbVOC.find("tbody input[cTotal=Y][nMonth=" + (i + 1) + "]:not(input.target)"), function (index, el) {
                                el = $(el);
                                if (el.val() != "") {
                                    IsPass = true;
                                } else {
                                    sMsg += "<br/>- Please specify " + arrFullMonth[i];
                                }
                            })
                        } else {

                            IsCheckValueThisMonth = false;
                            $.each($tbCombustion.find("input:not(input[type=checkbox],input.target)[nMonth=" + (i + 1) + "]"), function (i, el) {
                                el = $(el);
                                if (arrIncDataProductID.indexOf(+el.attr("ProductID")) > -1 && el.val() != "") {
                                    IsCheckValueThisMonth = true
                                }
                            })
                            if (arrIncCheck[i] != "Y" && !IsCheckValueThisMonth) {
                                IsPass = false
                                sMsg += "<br/>- Please specify " + arrFullMonth[i];
                            }
                        }
                    }
                }
            }

            var IsFilePass = true;
            if ($("table[id$=tblOtherFile] tbody tr").length > 0) {
                $.each($("table[id$=tblOtherFile] tbody tr").find("td:eq(2) input"), function (i, el) {
                    el = $(el);
                    if (el.val().trim() == "") {
                        IsPass = false;
                        IsFilePass = false;
                    }
                })
                if (!IsFilePass) {
                    sMsg += "<br/><br/>&bull; File";
                    sMsg += "<br/>&nbsp;- Please specify file description";
                }
            }
            if (IsPass) {
                $.each($dataFileOther, function (e, n) {
                    var ID = n.ID;
                    var sVal = $("input[id$=txtFile_" + ID + "]").val();
                    n.sDescription = sVal;
                });
                var arrData = {
                    nIndicatorID: $ddlIndicator.val(),
                    nOperationID: $ddlOperationType.val(),
                    nFacilityID: $ddlFacility.val(),
                    sYear: $ddlYear.val(),
                    lstFile: $dataFileOther,
                    lstStack: lstStack,
                    lstDataCombustion: lstCombustion,
                    lstDataNonCombustion: lstNonCombustion,
                    lstDataCEM: lstCEM,
                    lstDataAdditional: lstAdditional,
                    lstDataAdditionalNonCombustion: lstAdditionalNonCombustion,
                    objVOC: { lstVOC: lstVOC, sRemarkVOC: GetValTextArea("txtRemarkVOC") },
                    lstMonthSubmit: arrMonth,
                    nStatus: +nStatus,
                    sRemarkRequestEdit: $("textarea[id$=txtsComment]").val()
                }
                if (nStatus != 1) {
                    DialogConfirm(DialogHeader.Confirm, sMsgComfirmAlert, function () {
                        LoaddinProcess();
                        AjaxCallWebMethod("saveToDB", function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupLogin();
                            } else if (response.d.Status == SysProcess.Success) {
                                if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                                    DialogSuccessRedirect(DialogHeader.Info, sMsgComplete, "epi_mytask.aspx");
                                } else {
                                    DialogSuccess(DialogHeader.Info, sMsgComplete);
                                    LoadDataCheckddl();
                                }
                            } else {
                                DialogWarning(DialogHeader.Warning, response.d.Msg);
                            }
                        }, function () { HideLoadding(); }, { arrData: arrData })
                    }, function () { HideLoadding(); })
                } else {
                    if (!IsDeviatePass) {
                        arrData.nStatus = 0;
                        DialogConfirm(DialogHeader.Confirm, sMsgComfirmAlert, function () {
                            LoaddinProcess();
                            AjaxCallWebMethod("saveToDB", function (response) {
                                if (response.d.Status == SysProcess.SessionExpired) {
                                    PopupLogin();
                                } else if (response.d.Status == SysProcess.Success) {
                                    LoadData();
                                } else {
                                    DialogWarning(DialogHeader.Warning, response.d.Msg);
                                }
                            }, function () {
                                var lstPrdDeviate = [];
                                if ($hdfsddlOperationType.val() == "13") {
                                    lstPrdDeviate = Enumerable.From(lstVOC).Where(function (w) { return w.ProductID == 193 }).ToArray();
                                } else {
                                    lstPrdDeviate = Enumerable.From(lstCombustion).Where(function (w) { return w.ProductID == 160 || w.ProductID == 162 || w.ProductID == 164 }).ToArray();
                                    lstPrdDeviate = lstPrdDeviate.concat((Enumerable.From(lstVOC).Where(function (w) { return w.ProductID == 193 }).ToArray()));
                                }
                                lstPrdDeviate = $.map(lstPrdDeviate, function (item) {
                                    return [{
                                        ProductID: item.ProductID,
                                        ProductName: "",
                                        M1: item.M1,
                                        M2: item.M2,
                                        M3: item.M3,
                                        M4: item.M4,
                                        M5: item.M5,
                                        M6: item.M6,
                                        M7: item.M7,
                                        M8: item.M8,
                                        M9: item.M9,
                                        M10: item.M10,
                                        M11: item.M11,
                                        M12: item.M12,
                                    }];

                                });
                                Deviate(lstPrdDeviate, nStatus);
                            }, { arrData: arrData })
                        }, function () { HideLoadding(); })
                    } else {
                        LoaddinProcess();
                        AjaxCallWebMethod("saveToDB", function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupLogin();
                            } else if (response.d.Status == SysProcess.Success) {
                                if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                                    DialogSuccessRedirect(DialogHeader.Info, sMsgComplete, "epi_mytask.aspx");
                                } else {
                                    DialogSuccess(DialogHeader.Info, sMsgComplete);
                                    LoadDataCheckddl();
                                }
                            } else {
                                DialogWarning(DialogHeader.Warning, response.d.Msg);
                            }
                        }, function () { HideLoadding(); }, { arrData: arrData })
                    }
                }
            } else {
                DialogWarning(DialogHeader.Warning, (sMsg.length > 0 ? sMsg.substr(5) : ""));
            }
        }

        function CheckValidateCustomForAddStack(sContainer) {
            var isValid = $("#" + sContainer).data('formValidation').validate().isValid();
            if (!isValid) {
                var IsDdlEmpty = false;
                $.each($tbAddStack.find("select[name*=ddlOther]"), function () {
                    if ($(this).val() == "") {
                        IsDdlEmpty = true;
                    }
                })
                if (IsDdlEmpty) DialogWarning(DialogHeader.Warning, "Please select additional.");
                ScrollTopToElementsTo(sContainer, 85);//$("div#" + sContainer).data('formValidation').$invalidFields[0].focus();
            }
            return isValid;
        }
        function CheckboxQuarterChangedCustom(sTableID) {
            $(".QHead_1").hide();
            $(".QHead_2").hide();
            $(".QHead_3").hide();
            $(".QHead_4").hide();

            nWidthTB = 0;
            nWidthTB = (nWidthTD * 2) + nWidthIndicator;
            if (arrrdoQ.length == 0) {
                nWidthTB = 2030;
            }
            $.each(arrrdoQ, function (e, n) {
                if (n == 1) {
                    $(".QHead_1").show();
                    nWidthTB = nWidthTB + (nWidthTD * 3);
                }
                if (n == 2) {
                    $(".QHead_2").show();
                    nWidthTB = nWidthTB + (nWidthTD * 3);
                }
                if (n == 3) {
                    $(".QHead_3").show();
                    nWidthTB = nWidthTB + (nWidthTD * 3);
                }
                if (n == 4) {
                    $(".QHead_4").show();
                    nWidthTB = nWidthTB + (nWidthTD * 3);
                }
            });

            var el = $("table[id$=" + sTableID + "]");
            el.width((sTableID == "tbAddStack" || sTableID == "tbAddStackCEM" ? nWidthTB + (nWidthTD * 3) : nWidthTB) + "px");
            if (el.find("tbody tr").length == 1 && el.find("tbody tr td:eq(0)").hasClass("NoFix")) {
                $("table#" + el.attr("id")).tableHeadFixer({ head: true });
            } else {
                $("table#" + el.attr("id")).tableHeadFixer({ "left": 1, head: true });
            }
        }
    </script>
</asp:Content>

