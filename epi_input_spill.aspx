<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_EPI_FORMS.master" AutoEventWireup="true" CodeFile="epi_input_spill.aspx.cs" Inherits="epi_input_spill" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .flat-green:not(.radio) label {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

        table#tbDataMonth > tbody > tr > td > input {
            text-align: right !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divContent" style="display: none;">
        <div class="col-xs-12 col-md-6 text-left" style="margin-bottom: 5px;">
            <a style="font-size: 24px;" title="Helper Spill to environment" href="Helper_Indicator.aspx?ind=9&&prd=0" target="_blank"><i class="fas fa-question-circle"></i></a>
        </div>
        <div class="col-xs-12 col-md-6 text-right-lg text-right-md text-left-sm" style="margin-bottom: 5px;">
            <button type="button" onclick="ShowDeviate();" class="btn btn-info" title="Deviate History"><i class="fas fa-comments"></i></button>
            <button type="button" onclick="ShowHistory();" class="btn btn-info" title="Workflow History"><i class="fas fa-comment-alt"></i></button>
            <asp:LinkButton ID="lnkExport" runat="server" CssClass="btn btn-success" OnClick="lnkExport_Click">Export</asp:LinkButton>
        </div>
        <div class="col-xs-12">
            <div class="table-responsive">
                <table id="tbDataMonth" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                </table>
            </div>
        </div>
        <div class="form-group col-xs-12" style="margin-top: 20px">
            <button type="button" id="btnAddSpill" onclick="AddSpill()" class="btn btn-primary btn-sm text-left NoPRMS">Add New Spill</button>
            <span class="pull-right"><span class="text-red">*</span>&nbsp; Spill to sensitive area</span>
        </div>
        <div class="col-xs-12">
            <div class="table-responsive">
                <table id="tbDataList" class="table dataTable table-bordered table-hover">
                    <thead>
                        <tr>
                            <th class="text-center" style="vertical-align: middle; width: 7%;">No</th>
                            <th class="text-center" style="vertical-align: middle; width: 25%;">Primary reason for loss of containment</th>
                            <th class="text-center" style="vertical-align: middle; width: 10%;">Spill Type</th>
                            <th class="text-center" style="vertical-align: middle; width: 10%;">Spill of</th>
                            <th class="text-center" style="vertical-align: middle; width: 16%;">Spill to</th>
                            <th class="text-center" style="vertical-align: middle; width: 12%;">Volume</th>
                            <th class="text-center" style="vertical-align: middle; width: 10%;">Spill Date</th>
                            <th class="text-center" style="vertical-align: middle; width: 18%;"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-xs-12" id="divCreateSpill" style="display: none">
            <div class="panel panel-info col">
                <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divCreate">
                    <i class="fas fa-edit"></i>Create / Update
                </div>
                <div class="panel-body panel-collapse collapse in" id="divCreate">
                    <div id="divInput">
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Cause of Spill <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-6 col-lg-6">
                                <div class="form-inline">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="addOnCauseOfSpill"><i class="fa fa-list"></i></span>
                                        <asp:DropDownList runat="server" ID="ddlCauseOfSPill" CssClass="form-control input-sm HasOther" txtID="txtOtherCauseOfSpill" aria-describedby="addOnCauseOfSpill"></asp:DropDownList>
                                    </div>
                                    <input id="txtOtherCauseOfSpill" class="form-control input-sm txtOther" name="txtOtherCauseOfSpill" maxlength="250" style="display: none;" placeholder="Specify" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Incidence No <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-6 col-lg-3">
                                <div class="input-group">
                                    <span class="input-group-addon" id="addOnIncidenceNo"><i class="fa fa-pencil-alt"></i></span>
                                    <input id="txtIncidenceNo" class="form-control input-sm" name="txtIncidenceNo" placeholder="Incidence number" maxlength="250" aria-describedby="addOnIncidenceNo" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Spill Type <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-4 col-lg-3">
                                <div class="input-group">
                                    <span class="input-group-addon" id="addOnSpillType"><i class="fa fa-list"></i></span>
                                    <asp:DropDownList runat="server" ID="ddlSpillType" CssClass="form-control input-sm" aria-describedby="addOnSpillType"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Spill of <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-6 col-lg-6">
                                <div class="form-inline">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="addOnSpillOf"><i class="fa fa-list"></i></span>
                                        <asp:DropDownList runat="server" ID="ddlSpillOf" CssClass="form-control input-sm HasOther" txtID="txtOtherSpillOf" aria-describedby="addOnSpillOf"></asp:DropDownList>
                                    </div>
                                    <input id="txtOtherSpillOf" class="form-control input-sm txtOther" name="txtOtherSpillOf" maxlength="250" style="display: none;" placeholder="Specify" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Substance name <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-6 col-lg-6">
                                <div class="input-group">
                                    <span class="input-group-addon" id="addOnSubstanceName"><i class="fa fa-pencil-alt"></i></span>
                                    <input id="txtSubstanceName" class="form-control input-sm" name="txtSubstanceName" placeholder="Substance name" maxlength="1000" aria-describedby="addOnSubstanceName" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Volume <span class="text-red ">*</span>  <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-8 col-lg-9">
                                <div class="form-inline">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="addOnVolume"><i class="fa fa-pencil-alt"></i></span>
                                        <input id="txtVolume" class="form-control input-sm text-right" name="txtVolume" maxlength="15" placeholder="Spill Volume" aria-describedby="addOnVolume" />
                                    </div>
                                    <input id="txtDensity" class="form-control input-sm text-right" name="txtDensity" maxlength="15" placeholder="Density" />
                                    <asp:DropDownList runat="server" ID="ddlVolume" txtID="txtDensity" CssClass="form-control input-sm">
                                    </asp:DropDownList>
                                    <span id="lbCalculateM3" class="control-label"></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                LOPC Tier <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-4 col-lg-3">
                                <div class="form-inline">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="addOnLOPCTier"><i class="fa fa-list"></i></span>
                                        <asp:DropDownList runat="server" ID="ddlLOPCTier" CssClass="form-control input-sm" aria-describedby="addOnLOPCTier"></asp:DropDownList>
                                    </div>
                                    <a id="aHelperTier" class="control-label" style="font-size: 24px;" title="Helper Spill LOPC Tier"><i class="fas fa-question-circle"></i></a>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Spill to <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-6 col-lg-6">
                                <div class="form-inline">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="addOnSpillTo"><i class="fa fa-list"></i></span>
                                        <asp:DropDownList runat="server" ID="ddlSpillTo" CssClass="form-control input-sm HasOther" txtID="txtOtherSpillTo" aria-describedby="addOnSpillTo"></asp:DropDownList>
                                    </div>
                                    <input id="txtOtherSpillTo" class="form-control input-sm txtOther" name="txtOtherSpillTo" maxlength="250" style="display: none;" placeholder="Specify" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Spill by <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-6 col-lg-6">
                                <div class="form-inline">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="addOnSpillBy"><i class="fa fa-list"></i></span>
                                        <asp:DropDownList runat="server" ID="ddlSpillBy" CssClass="form-control HasOther input-sm" txtID="txtOtherSpillBy" aria-describedby="addOnSpillBy"></asp:DropDownList>
                                    </div>
                                    <input id="txtOtherSpillBy" class="form-control input-sm txtOther" name="txtOtherSpillBy" maxlength="250" style="display: none;" placeholder="Specify" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                            </div>
                            <div class="col-xs-12 col-md-4 col-lg-3">
                                <asp:CheckBox runat="server" ID="cbSpillSensitiveArea" CssClass="flat-green" Text="Spill to sensitive area" />
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Date Of Spill <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-4 col-lg-3">
                                <div class="input-group">
                                    <span class="input-group-addon" id="addOnDateOfSpill"><i class="fa fa-calendar-alt"></i></span>
                                    <input class="form-control input-sm" id="txtDateOfSpill" name="txtDateOfSpill" placeholder="--/--/----" aria-describedby="addOnDateOfSpill" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Description <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-9">
                                <textarea class="form-control" rows="4" style="resize: vertical;" id="txtDescription" name="txtDescription"></textarea>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Intermediate Action <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-9">
                                <textarea class="form-control" rows="4" style="resize: vertical;" id="txtIntermediateAction" name="txtIntermediateAction"></textarea>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg-3 text-left-xs text-right-lg text-right-md">
                                Recovery Action <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-9">
                                <textarea class="form-control" rows="4" style="resize: vertical;" id="txtRecoveryAction" name="txtIntermediateAction"></textarea>
                            </div>
                        </div>
                    </div>
                    <div id="divUploadFile">
                        <div class="col-xs-12">
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
                    <div id="divButtonSpill">
                        <div class="col-xs-12 text-center">
                            <button type="button" id="btnSaveSpill" class="btn btn-primary" onclick="saveSpill()">Save this Spill</button>
                            <button type="button" class="btn btn-default" onclick="CancelAdd()">Cancel this Spill</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdfIndID" />
    <asp:HiddenField runat="server" ID="hdfManageID" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        var arrShortMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var arrFullMonth = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var $divContent = $("div[id$=divContent]");
        var $tbDataMonth = $("table[id$=tbDataMonth]");
        var $tbDataList = $("table[id$=tbDataList]");
        var $divCreateSpill = $("div[id$=divCreateSpill]");
        var $btnAddSpill = $("button[id$=btnAddSpill]");
        var $hdfManageID = $("input[id$=hdfManageID]");
        var lstIncData = [];
        var lstSpill = []
        var IsFullMonth = true;
        var lstOther = [20, 27, 30, 75, 93];
        $(function () {
            ArrInputFromTableID.push("tbDataMonth");

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
            setDatePicker();

            AjaxCallWebMethod("LoadData", function (response) {

                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                } else {
                    lstIncData = [];
                    lstSpill = [];
                    lstIncData = response.d.lstIncData;
                    lstSpill = response.d.lstSpill;
                    lstStatus = response.d.lstStatus;
                    $tbDataMonth.empty();
                    bindDataMonth(response.d.lstIncData);
                    bindDataSpill(response.d.lstSpill);
                    CalculateData();
                    ClearDataInput();
                    IsFullMonth = true;
                    $.each(lstStatus, function (i, el) {
                        if (el.nStatusID == 0) {
                            IsFullMonth = false;
                        }
                    })
                    if (IsFullMonth) {
                        if (($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") || $hdfIsAdmin.val() == "Y") {
                            $btnAddSpill.show();
                            $("input.target").prop("disabled", false);
                        } else {
                            $btnAddSpill.hide();
                            $("input.target").prop("disabled", true);
                        }
                    } else {
                        $btnAddSpill.show();
                        $("input.target").prop("disabled", false);
                    }
                }
            }, function () {
                $('.flat-green-custom').iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_square-green' //'iradio_flat-green'
                });
                CheckEventButton();
                CheckboxQuarterChanged();
                $tbDataMonth.tableHeadFixer({ "left": 1 }, { "head": true })
                $tbDataList.tableHeadFixer({ "head": true })
                HideLoadding();
            }, { param: param });
            $divContent.show();
        }

        function bindDataMonth(data) {
            var sHtml = "";
            var lstDataCheck = [data[0].IsCheckM1, data[0].IsCheckM2, data[0].IsCheckM3, data[0].IsCheckM4, data[0].IsCheckM5, data[0].IsCheckM6, data[0].IsCheckM7, data[0].IsCheckM8, data[0].IsCheckM9, data[0].IsCheckM10, data[0].IsCheckM11, data[0].IsCheckM12]

            sHtml += "<thead>";
            sHtml += "  <tr>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthIndicator + "px;'><label>Indicator</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'><label>Unit</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;'><label>Target</label></th>";
            sHtml += bindColumnQ(true, null, lstDataCheck, false, "");
            sHtml += "  </tr>";
            sHtml += "</thead>";
            sHtml += "<tbody>";
            $.each(data, function (i, el) {
                var lstDataMonth = [el.M1, el.M2, el.M3, el.M4, el.M5, el.M6, el.M7, el.M8, el.M9, el.M10, el.M11, el.M12]
                sHtml += "  <tr>";
                sHtml += "      <td class='text-left' style='vertical-align: middle;'>" + el.ProductName + "</td>";
                sHtml += "      <td class='text-center' style='vertical-align: middle;'>" + el.sUnit + "</td>";
                sHtml += "      <td class='text-center cTarget' style='vertical-align: middle;'><input class='form-control input-sm text-right target' ProductID ='" + el.ProductID + "'  value ='" + CheckTextInput(el.Target) + "' maxlength='20'/></td>";
                sHtml += bindColumnQ(false, lstDataMonth, null, el.ProductID == 209, (el.ProductID == 210 ? "2Liter" : "2M3"));
                sHtml += "  </tr>";
            })
            sHtml += "</tbody>";
            $tbDataMonth.append(sHtml);
        }

        function bindColumnQ(IsTHead, dataMonth, dataCheck, IsCountMonth, sClassNotCountMonth) {
            var sHtml = "";
            if (IsTHead) {
                for (var i = 1 ; i <= 12; i++) {
                    var strForDisabled = "";
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
                    sHtml += "<th class='text-center M_" + i + " QHead_" + getQrt(i) + "'><input type='checkbox' nMonth ='" + i + "' class='flat-green-custom " + (strForDisabled != "" ? "submited" : "") + "' " + IsCheck + " " + strForDisabled + " />&nbsp;<label>Q" + getQrt(i) + " : " + arrShortMonth[i - 1] + "</label></th>";
                }
            } else {
                for (var i = 1 ; i <= 12; i++) {
                    sHtml += "<td class='text-center M_" + i + " QHead_" + getQrt(i) + " '><input class='form-control input-sm " + (IsCountMonth ? "CountMonth" : sClassNotCountMonth) + "' nMonth ='" + i + "' value ='" + CheckTextInput(dataMonth[i - 1]) + "' disabled/></td>";
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

        function bindDataSpill(lstSpill) {
            var sHtml = "";
            $tbDataList.find("tbody tr").remove();
            lstSpill = Enumerable.From(lstSpill).Where(function (w) { return !w.IsDel && w.IsShow }).ToArray();
            if (lstSpill.length > 0) {
                sHtml += "<tr>";
                $.each(lstSpill, function (i, el) {
                    var IsSubmited = false;
                    var sTitle = "";
                    var sClass = "";
                    var sIcon = "";
                    if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                        if (lstStatus[el.nMonth - 1].nStatusID != "2") {
                            IsSubmited = true;
                        }
                    } else {
                        if (lstStatus[el.nMonth - 1].nStatusID != "0") {
                            IsSubmited = true;
                        }
                    }
                    if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                        IsSubmited = false;
                    }
                    if ($hdfRole.val() == "3") {
                        IsSubmited = true;
                    }
                    if (IsSubmited) {
                        sIcon = "search";
                        sTitle = "View";
                        sClass = "btn-info";
                    } else {
                        sIcon = "edit";
                        sTitle = "Edit";
                        sClass = "btn-warning";
                    }
                    var sBtnEdit = "<button type='button' title='" + sTitle + "' style='margin-bottom:2px;' class='btn " + sClass + " btn-sm' value='" + el.nSpillID + "' onclick='EditSpill(" + el.nSpillID + "," + IsSubmited + ")'><i class='fa fa-" + sIcon + "'></i></button>";
                    var sBtnDel = "<button type='button' title='Delete' style='margin-bottom:2px;' class='btn btn-danger btn-sm' value='" + el.nSpillID + "' onclick='DelSpill(" + el.nSpillID + ")'><i class='fa fa-trash'></i></button>";
                    sHtml += "  <td class='text-center'>" + (i + 1) + "</td>";
                    sHtml += "  <td class='text-left'>" + (lstOther.indexOf(+el.PrimaryReasonID) > -1 ? el.sOtherPrimary : el.sPrimaryReason) + setIconSpill(el) + "</td>";
                    sHtml += "  <td class='text-center'>" + el.sSpillTypeName + "</td>";
                    sHtml += "  <td class='text-left'>" + (lstOther.indexOf(+el.SpillOfID) > -1 ? el.sOtherSpillOf : el.sSpillOfName) + "</td>";
                    sHtml += "  <td class='text-left'>" + (lstOther.indexOf(+el.SpillToID) > -1 ? el.sOtherSpillTo : el.sSpillToName) + "</td>";
                    sHtml += "  <td class='text-right'>" + (CheckTextInput(el.Volume) + " " + el.sUnitName) + (el.sIsSensitiveArea == "Y" ? "&nbsp;<span class='text-red'>*</span>" : "") + "</td>";
                    sHtml += "  <td class='text-center'>" + el.sSpillDate + "</td>";
                    sHtml += "  <td class='text-center'>" + sBtnEdit + (!IsSubmited ? "&nbsp;" + sBtnDel : "") + "</td>";
                    sHtml += "</tr>";
                })
                $tbDataList.find("tbody").append(sHtml);

                SetTootip();
            } else {
                SetRowNoData($tbDataList.attr("id"), 8);
            }
        }

        function setIconSpill(el) {
            var sHtml = "";
            var sClassIcon = "";
            var sTitle = "";
            var sColor = "";
            var sClassIconSpill = "fas fa-fill-drip";
            var sTitleSpill = "Spill";
            var sColorSpill = "color:green;";
            var sClassIconSignificantSpill = "fas fa-fill-drip";
            var sTitleSignificantSpill = "Significant Spill";
            var sColorSignificantSpill = "color:red;";
            var sClassIconNoneSpill = "fas fa-ban";
            var sTitleNoneSpill = "None";
            var nBarrel = 0;
            var nDivideToBarrel = 158.9873;

            // กรณีที่ nBarrel = null แสดงว่า Volumn = "N/A"
            switch (el.UnitVolumeID + "") {
                case "63":
                    nBarrel = +ConvertLiterToBarrel(el.Volume + "");
                    break;
                case "64":
                    nBarrel = +el.Volume;
                    break;
                case "65":
                    nBarrel = +ConvertM3ToBarrel(el.Volume + "");
                    break;
                case "2":
                    var nVolumn = +el.Volume;
                    var nDensity = +el.Density;
                    nBarrel = +ConvertLiterToBarrel(nVolumn * nDensity);
            }
            if (el.PrimaryReasonID == "18") {
                if (el.SpillByID == "32") {
                    if (el.sIsSensitiveArea == "Y") { //Significant Spill
                        sClassIcon = sClassIconSignificantSpill;
                        sTitle = sTitleSignificantSpill;
                        sColor = sColorSignificantSpill;
                    } else {
                        if (nBarrel >= 1 && nBarrel < 100) { // Spill
                            sClassIcon = sClassIconSpill;
                            sTitle = sTitleSpill;
                            sColor = sColorSpill;
                        } else if (nBarrel >= 100) { // Significant Spill
                            sClassIcon = sClassIconSignificantSpill;
                            sTitle = sTitleSignificantSpill;
                            sColor = sColorSignificantSpill;
                        }
                        else { // None
                            sClassIcon = sClassIconNoneSpill;
                            sTitle = sTitleNoneSpill;
                        }
                    }
                } else { // None
                    sClassIcon = sClassIconNoneSpill;
                    sTitle = sTitleNoneSpill;
                }
            } else {
                if (el.SpillByID != "32") {
                    if (el.sIsSensitiveArea == "Y") { //Significant Spill
                        sClassIcon = sClassIconSignificantSpill;
                        sTitle = sTitleSignificantSpill;
                        sColor = sColorSignificantSpill;
                    } else {
                        if (nBarrel >= 1 && nBarrel < 100) { // Spill
                            sClassIcon = sClassIconSpill;
                            sTitle = sTitleSpill;
                            sColor = sColorSpill;
                        } else if (nBarrel >= 100) { // Significant Spill
                            sClassIcon = sClassIconSignificantSpill;
                            sTitle = sTitleSignificantSpill;
                            sColor = sColorSignificantSpill;
                        }
                        else { // None
                            sClassIcon = sClassIconNoneSpill;
                            sTitle = sTitleNoneSpill;
                        }
                    }
                } else { // None
                    sClassIcon = sClassIconNoneSpill;
                    sTitle = sTitleNoneSpill;
                }
            }
            sHtml += "&nbsp;<i style='font-size:20px;" + sColor + "' class='" + sClassIcon + "' title='" + sTitle + "'></i>";
            return sHtml;
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
                    data: { funcname: "UPLOAD", savetopath: 'Spill/Temp/', savetoname: '' },
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
                        var ItemData = Enumerable.From(lstSpill).FirstOrDefault(null, function (w) { return w.nSpillID == $hdfManageID.val() });
                        var lstFile = ItemData.lstFile;
                        var arrFile = apiFile1.getFiles();
                        for (var i = 0; i < arrFile.length; i++) {
                            var item = arrFile[i];
                            item.data.IsCompleted = false;
                            ItemData.lstFile.push(item.data);
                        }

                        //Update ID
                        var qMaxID = lstFile.length > 0 ? Enumerable.From(lstFile).Max(function (x) { return x.ID }) + 1 : 1;
                        for (var i = 0; i < lstFile.length; i++) {
                            var item = lstFile[i];
                            if (item.ID == 0) {
                                item.ID = qMaxID;
                                qMaxID++;
                            }
                        }

                        apiFile1.reset();
                        HideLoadding();

                        //call function render file
                        BindTableFileOther(lstFile);
                    }
                }
            });

            var apiFile1 = $.fileuploader.getInstance(filupload1);
        }

        function BindTableFileOther(lstFile) {
            var sTableID = "tblOtherFile";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var lstData = Enumerable.From(lstFile).Where(function (x) { return x.sDelete == "N" }).ToArray();
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
                    $("td", row).eq(2).html('<input id="txtFile_' + item.ID + '" class="form-control input-sm " />');
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
            var ItemData = Enumerable.From(lstSpill).FirstOrDefault(null, function (w) { return w.nSpillID == $hdfManageID.val() });
            var lstFile = ItemData.lstFile;
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                var item = Enumerable.From(lstFile).FirstOrDefault(null, function (x) { return x.ID == fileid });
                if (item != null) {
                    item.sDelete = "Y";
                    if (Boolean(item.IsNewFile)) {
                        $.ajax({
                            type: "POST",
                            url: AshxSysFunc.UrlFileUpload,
                            data: { funcname: "DEL", delpath: item.SaveToPath, delfilename: item.SaveToFileName },
                            success: function (response) {
                                if (Boolean(response.IsCompleted)) {
                                    BindTableFileOther(lstFile);
                                } else {
                                    HideLoadding();
                                }
                            },
                            complete: function (jqXHR, status) {//finaly
                                //HideLoadding();
                            }
                        });
                    } else {
                        BindTableFileOther(lstFile);
                    }
                } else {
                    HideLoadding();
                    DialogWarning(DialogHeader.Warning, "Not found !");
                }
            }, "");
        }

        function ClearDataInput() {
            var arrObj = [];
            $divCreateSpill.find("input,textarea,select").prop("disabled", false);
            $("div[id$=divInput]").find("input,textarea,select").val("").change().prop("disabled", false);
            $("select[id$=ddlVolume]").val($("select[id$=ddlVolume]").find("option:first").val());

            $("div[id$=dvbtn]").find("button").prop("disabled", false);
            $("input[id*=cbSpillSensitiveArea]").iCheck("Uncheck");
            $.each($("div[id$=divInput]").find("input,textarea,select"), function () {
                arrObj.push($(this).attr("name"));
            })
            $("div[id$=divCreate]").find(".NoPRMS").show();
            $("button[id$=btnSaveSpill]").show();
            UpdateStatusValidate("divInput", arrObj);
            //$("div[id$=divInput]").find("input.txtOther").prop("disabled", true);
            $btnAddSpill.prop("disabled", false);
            $tbDataList.find("button").prop("disabled", false);
            $tbDataMonth.find("input[type=checkbox]").not("[class$=submited]").prop("disabled", false);
            $divCreateSpill.hide();
            $hdfManageID.val("");
            CalculateData();
            bindDataSpill(lstSpill);
        }

        function setDatePicker(ctrl) {
            var sDefaultDate = "";
            if (ctrl != "" && ctrl != null && ctrl != undefined) {
                ctrl = $(ctrl);
                sDefaultDate = ctrl.val();
            }
            $("input[id$=txtDateOfSpill]").datepicker("remove");
            $("input[id$=txtDateOfSpill]").datepicker({
                setDate: sDefaultDate,
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: new Date("01/01/" + $ddlYear.val()),
                endDate: new Date("12/31/" + $ddlYear.val()),
            });
        }

        function AddSpill() {
            $btnAddSpill.prop("disabled", true);
            $("table[id$=tbDataList]").find("button").prop("disabled", true);
            $tbDataMonth.find("input[type=checkbox]").prop("disabled", true);
            $("div[id$=dvbtn]").find("button").prop("disabled", true);
            $("span[id$=lbCalculateM3]").hide();
            var nMaxID = lstSpill.length > 0 ? Enumerable.From(lstSpill).Max(function (m) { return m.nSpillID }) + 1 : 1;
            lstSpill.push({
                nSpillID: nMaxID,
                PrimaryReasonID: "",
                sPrimaryReason: "",
                sOtherPrimary: "",
                SpillType: "",
                sSpillTypeName: "",
                SpillOfID: "",
                sSpillOfName: "",
                sOtherSpillOf: "",
                Volume: "",
                UnitVolumeID: "",
                Density: "",
                sUnitName: "",
                SpillToID: "",
                sSpillToName: "",
                sOtherSpillTo: "",
                SpillByID: "",
                sOtherSpillBy: "",
                sSpillDate: "",
                sDescription: "",
                IncidentDescription: "",
                RecoveryAction: "",
                sIsSensitiveArea: "",
                sIncidenceNo: "",
                sSubstanceName: "",
                LOPCTierID: "",
                nMonth: "",
                IsDel: false,
                IsNew: true,
                IsSubmited: false,
                IsShow: true,
                lstFile: [],
            })
            $hdfManageID.val(nMaxID);
            var dataManage = Enumerable.From(lstSpill).FirstOrDefault(null, function (w) { return w.nSpillID == nMaxID });
            bindValidateFormInput();
            BindTableFileOther(dataManage.lstFile);
            setDatePicker();
            $divCreateSpill.show();
            ScrollTopToElementsTo("divCreateSpill", 70);
        }

        function CancelAdd() {
            var data = Enumerable.From(lstSpill).FirstOrDefault(null, function (w) { return w.nSpillID == $hdfManageID.val() });
            $.each(data.lstFile, function (i, el) {
                if (el.sDelete == "Y" && el.IsCompleted == true) {
                    el.sDelete = "N";
                }
            })
            data.lstFile = Enumerable.From(data.lstFile).Where(function (w) { return w.IsCompleted == true }).ToArray();
            if (!data.IsSubmited) {
                lstSpill = Enumerable.From(lstSpill).Where(function (w) { return w.nSpillID != $hdfManageID.val() }).ToArray();
            }
            ClearDataInput();
        }

        function bindValidateFormInput() {
            var objValidate = {};
            objValidate[GetElementName("ddlCauseOfSPill", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Cause of spill");
            objValidate[GetElementName("txtOtherCauseOfSpill", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify);
            objValidate[GetElementName("ddlSpillType", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Spill type");
            objValidate[GetElementName("ddlSpillOf", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Spill of ");
            objValidate[GetElementName("txtOtherSpillOf", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify);
            objValidate[GetElementName("txtVolume", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Volume");
            objValidate[GetElementName("txtDensity", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Density");
            objValidate[GetElementName("ddlSpillTo", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Spill to");
            objValidate[GetElementName("txtOtherSpillTo", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify);
            objValidate[GetElementName("ddlSpillBy", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Spill by");
            objValidate[GetElementName("txtOtherSpillBy", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify);
            objValidate[GetElementName("txtDateOfSpill", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Date of spill");
            objValidate[GetElementName("txtSubstanceName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Substance name");
            objValidate[GetElementName("ddlLOPCTier", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " LOPC Tier");
            //objValidate[GetElementName("txtDescription", objControl.txtarea)] = addValidate_notEmpty(DialogMsg.Specify + " Description");
            //objValidate[GetElementName("txtIntermediateAction", objControl.txtarea)] = addValidate_notEmpty(DialogMsg.Specify + " Intermediate action");
            //objValidate[GetElementName("txtRecoveryAction", objControl.txtarea)] = addValidate_notEmpty(DialogMsg.Specify + " Recovery action");
            BindValidate("divInput", objValidate);
        }

        function saveSpill() {
            if (CheckValidate("divInput")) {

                var nMonth = +Input("txtDateOfSpill").val().split("/")[1];

                var IsPass = true;
                if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                    if (lstStatus[nMonth - 1].nStatusID != "2") {
                        IsPass = false;
                    }
                } else {
                    if (lstStatus[nMonth - 1].nStatusID != "0") {
                        IsPass = false;
                    }
                }
                if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                    IsPass = true;
                }
                if (!IsPass) {
                    DialogWarning(DialogHeader.Warning, "Cannot save data in " + arrFullMonth[nMonth - 1]);
                } else {
                    if ($tbDataMonth.find("input[type=checkbox][nMonth=" + nMonth + "]").is(":checked")) {
                        DialogConfirm(DialogHeader.Confirm, "This month is checked for ignore ,Do you want to save?", function () {
                            var DataInMonth = Enumerable.From(lstSpill).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray();
                            $.each(DataInMonth, function (i, el) {
                                el.IsShow = false;
                                el.IsDel = true;
                            })
                            var data = Enumerable.From(lstSpill).FirstOrDefault(null, function (w) { return w.nSpillID == $hdfManageID.val() });
                            $.each(data.lstFile, function (i, el) {
                                if (el.sDelete == "Y") {
                                    el.IsCompleted = false;
                                } else {
                                    el.IsCompleted = true;
                                }
                                var ID = el.ID;
                                var sVal = $("input[id$=txtFile_" + ID + "]").val();
                                el.sDescription = sVal;
                            })
                            data.PrimaryReasonID = GetValDropdown("ddlCauseOfSPill");
                            data.sPrimaryReason = $("select[id$=ddlCauseOfSPill] option[value=" + GetValDropdown("ddlCauseOfSPill") + "]").text();
                            data.sOtherPrimary = Input("txtOtherCauseOfSpill").val();
                            data.SpillType = GetValDropdown("ddlSpillType");
                            data.sSpillTypeName = $("select[id$=ddlSpillType] option[value=" + GetValDropdown("ddlSpillType") + "]").text();
                            data.SpillOfID = GetValDropdown("ddlSpillOf");
                            data.sSpillOfName = $("select[id$=ddlSpillOf] option[value=" + GetValDropdown("ddlSpillOf") + "]").text();
                            data.sOtherSpillOf = Input("txtOtherSpillOf").val();
                            data.Volume = Input("txtVolume").val().replace(/,/g, "");
                            data.Density = Input("txtDensity").val().replace(/,/g, ""),
                            data.UnitVolumeID = GetValDropdown("ddlVolume");
                            data.sUnitName = $("select[id$=ddlVolume] option[value=" + GetValDropdown("ddlVolume") + "]").text();
                            data.SpillToID = GetValDropdown("ddlSpillTo");
                            data.sSpillToName = $("select[id$=ddlSpillTo] option[value=" + GetValDropdown("ddlSpillTo") + "]").text();
                            data.sOtherSpillTo = Input("txtOtherSpillTo").val();
                            data.SpillByID = GetValDropdown("ddlSpillBy");
                            data.sOtherSpillBy = Input("txtOtherSpillBy").val();
                            data.sSpillDate = Input("txtDateOfSpill").val();
                            data.sDescription = GetValTextArea("txtDescription");
                            data.IncidentDescription = GetValTextArea("txtIntermediateAction");
                            data.RecoveryAction = GetValTextArea("txtRecoveryAction");
                            data.sIsSensitiveArea = Input("cbSpillSensitiveArea").is(":checked") ? "Y" : "N";
                            data.sIncidenceNo = Input("txtIncidenceNo").val();
                            data.sSubstanceName = Input("txtSubstanceName").val();
                            data.LOPCTierID = GetValDropdown("ddlLOPCTier");
                            data.nMonth = nMonth;
                            data.IsSubmited = true;
                            ClearDataInput();
                            $tbDataMonth.find("input[type=checkbox][nMonth=" + nMonth + "]").iCheck("Uncheck");
                            HideLoadding();
                        });
                    } else {
                        var data = Enumerable.From(lstSpill).FirstOrDefault(null, function (w) { return w.nSpillID == $hdfManageID.val() });
                        $.each(data.lstFile, function (i, el) {
                            if (el.sDelete == "Y") {
                                el.IsCompleted = false;
                            } else {
                                el.IsCompleted = true;
                            }
                            var ID = el.ID;
                            var sVal = $("input[id$=txtFile_" + ID + "]").val();
                            el.sDescription = sVal;
                        })
                        data.PrimaryReasonID = GetValDropdown("ddlCauseOfSPill");
                        data.sPrimaryReason = $("select[id$=ddlCauseOfSPill] option[value=" + GetValDropdown("ddlCauseOfSPill") + "]").text();
                        data.sOtherPrimary = Input("txtOtherCauseOfSpill").val();
                        data.SpillType = GetValDropdown("ddlSpillType");
                        data.sSpillTypeName = $("select[id$=ddlSpillType] option[value=" + GetValDropdown("ddlSpillType") + "]").text();
                        data.SpillOfID = GetValDropdown("ddlSpillOf");
                        data.sSpillOfName = $("select[id$=ddlSpillOf] option[value=" + GetValDropdown("ddlSpillOf") + "]").text();
                        data.sOtherSpillOf = Input("txtOtherSpillOf").val();
                        data.Volume = Input("txtVolume").val().replace(/,/g, "");
                        data.Density = Input("txtDensity").val().replace(/,/g, ""),
                        data.UnitVolumeID = GetValDropdown("ddlVolume");
                        data.sUnitName = $("select[id$=ddlVolume] option[value=" + GetValDropdown("ddlVolume") + "]").text();
                        data.SpillToID = GetValDropdown("ddlSpillTo");
                        data.sSpillToName = $("select[id$=ddlSpillTo] option[value=" + GetValDropdown("ddlSpillTo") + "]").text();
                        data.sOtherSpillTo = Input("txtOtherSpillTo").val();
                        data.SpillByID = GetValDropdown("ddlSpillBy");
                        data.sOtherSpillBy = Input("txtOtherSpillBy").val();
                        data.sSpillDate = Input("txtDateOfSpill").val();
                        data.sDescription = GetValTextArea("txtDescription");
                        data.IncidentDescription = GetValTextArea("txtIntermediateAction");
                        data.RecoveryAction = GetValTextArea("txtRecoveryAction");
                        data.sIsSensitiveArea = Input("cbSpillSensitiveArea").is(":checked") ? "Y" : "N";
                        data.sIncidenceNo = Input("txtIncidenceNo").val();
                        data.sSubstanceName = Input("txtSubstanceName").val();
                        data.LOPCTierID = GetValDropdown("ddlLOPCTier");
                        data.nMonth = nMonth;
                        data.IsSubmited = true;
                        ClearDataInput();
                    }
                }
            }
        }

        function EditSpill(nSpillID, IsSubmited) {
            $btnAddSpill.prop("disabled", true);
            $tbDataList.find("button").prop("disabled", true);
            $tbDataMonth.find("input[type=checkbox]").prop("disabled", true);
            $("div[id$=dvbtn]").find("button").prop("disabled", true);
            $hdfManageID.val(nSpillID);
            var data = Enumerable.From(lstSpill).FirstOrDefault(null, function (w) { return w.nSpillID == $hdfManageID.val() });
            BindTableFileOther(data.lstFile);
            SetValueDropDown("ddlCauseOfSPill", data.PrimaryReasonID);
            Input("txtOtherCauseOfSpill").val(data.sOtherPrimary);
            SetValueDropDown("ddlSpillType", data.SpillType);
            AjaxCallWebMethod("ddlChange", function (response) {
                LoaddinProcess();
                $("select[id$=ddlSpillOf]").find("option").remove();
                var sHtml = "<option value=''>- Select -</option>";
                var data = response.d;
                $.each(data, function (i, el) {
                    sHtml += "<option value='" + el.Value + "'>" + el.Text + "</option>";
                });
                $("select[id$=ddlSpillOf]").append(sHtml);
            }, function () {
                $divCreateSpill.show();
                SetValueDropDown("ddlSpillOf", data.SpillOfID);
                Input("txtOtherSpillOf").val(data.sOtherSpillOf);
                Input("txtVolume").val(CheckTextInput(data.Volume));
                Input("txtDensity").val(CheckTextInput(data.Density));
                SetValueDropDown("ddlVolume", data.UnitVolumeID);
                SetValueDropDown("ddlSpillTo", data.SpillToID);
                Input("txtOtherSpillTo").val(data.sOtherSpillTo);
                SetValueDropDown("ddlSpillBy", data.SpillByID);
                Input("txtOtherSpillBy").val(data.sOtherSpillBy);
                Input("txtDateOfSpill").val(data.sSpillDate);
                SetValueDropDown("ddlLOPCTier", data.LOPCTierID);
                Input("txtIncidenceNo").val(data.sIncidenceNo);
                Input("txtSubstanceName").val(data.sSubstanceName);
                SetValueTextArea("txtDescription", data.sDescription);
                SetValueTextArea("txtIntermediateAction", data.IncidentDescription);
                SetValueTextArea("txtRecoveryAction", data.RecoveryAction);
                Input("cbSpillSensitiveArea").iCheck(data.sIsSensitiveArea == "Y" ? "Check" : "Uncheck");
                if (IsSubmited) {
                    $divCreateSpill.find("input,textarea,select").prop("disabled", true);
                    $("div[id$=divCreate]").find(".NoPRMS").hide();
                    $("button[id$=btnSaveSpill]").hide();
                }
                if (lstOther.indexOf(+GetValDropdown("ddlCauseOfSPill")) > -1) {
                    $("div#divInput select[id$=ddlCauseOfSPill]").change();
                    Input("txtOtherCauseOfSpill").show();
                    UpdateStatusValidateControl("divInput", GetElementName("txtOtherCauseOfSpill", objControl.txtbox), "NOT_VALIDATED")
                } else {
                    UpdateStatusValidateControl("divInput", GetElementName("ddlCauseOfSPill", objControl.txtbox), "NOT_VALIDATED")
                }
                if (lstOther.indexOf(+GetValDropdown("ddlSpillOf")) > -1) {
                    $("div#divInput select[id$=ddlSpillOf]").change();
                    Input("txtOtherSpillOf").show();
                    UpdateStatusValidateControl("divInput", GetElementName("txtOtherSpillOf", objControl.txtbox), "NOT_VALIDATED")
                } else {
                    UpdateStatusValidateControl("divInput", GetElementName("ddlSpillOf", objControl.txtbox), "NOT_VALIDATED")
                }
                if (lstOther.indexOf(+GetValDropdown("ddlSpillTo")) > -1) {
                    $("div#divInput select[id$=ddlSpillTo]").change();
                    Input("txtOtherSpillTo").show();
                    UpdateStatusValidateControl("divInput", GetElementName("txtOtherSpillTo", objControl.txtbox), "NOT_VALIDATED")
                } else {
                    UpdateStatusValidateControl("divInput", GetElementName("ddlSpillTo", objControl.txtbox), "NOT_VALIDATED")
                }
                if (lstOther.indexOf(+GetValDropdown("ddlSpillBy")) > -1) {
                    $("div#divInput select[id$=ddlSpillBy]").change();
                    Input("txtOtherSpillBy").show();
                    UpdateStatusValidateControl("divInput", GetElementName("txtOtherSpillBy", objControl.txtbox), "NOT_VALIDATED")
                } else {
                    UpdateStatusValidateControl("divInput", GetElementName("ddlSpillBy", objControl.txtbox), "NOT_VALIDATED")
                }
                $("div#divInput select[id$=ddlVolume]").change();
                $("div#divInput select[id$=ddlLOPCTier]").change();
                UpdateStatusValidateControl("divInput", GetElementName("txtVolume", objControl.txtbox), "NOT_VALIDATED")
                setDatePicker(Input("txtDateOfSpill"));
                bindValidateFormInput();
                ScrollTopToElementsTo("divCreateSpill", 70);
                HideLoadding();
            }, { ID: +data.SpillType });
        }

        function DelSpill(nSpillID) {
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                var data = Enumerable.From(lstSpill).FirstOrDefault(null, function (w) { return w.nSpillID == nSpillID });
                if (!data.IsNew) {
                    data.IsDel = true;
                    data.IsShow = false;
                } else {
                    lstSpill = Enumerable.From(lstSpill).Where(function (w) { return w.nSpillID != nSpillID }).ToArray();
                }
                CalculateData();
                bindDataSpill(lstSpill);
                HideLoadding();
            });
        }

        function CalculateData() {
            $.each(lstIncData, function (i, incData) {
                //var arrIncData = [incData.M1, incData.M2, incData.M3, incData.M4, incData.M5, incData.M6, incData.M7, incData.M8, incData.M9, incData.M10, incData.M11, incData.M12]
                var arrIncCheck = [incData.IsCheckM1, incData.IsCheckM2, incData.IsCheckM3, incData.IsCheckM4, incData.IsCheckM5, incData.IsCheckM6, incData.IsCheckM7, incData.IsCheckM8, incData.IsCheckM9, incData.IsCheckM10, incData.IsCheckM11, incData.IsCheckM12]
                for (var i = 0 ; i < 12 ; i++) {
                    var nMonth = i + 1;
                    var nDataInMonth = 0;
                    if (incData.ProductID == 209) {
                        nDataInMonth = Enumerable.From(lstSpill).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray().length;
                    } else {
                        var DataVolumeInMonth = Enumerable.From(lstSpill).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray();
                        if (incData.ProductID == 210) {// Covert data to Liter
                            $.each(DataVolumeInMonth, function (i, el) {
                                var nMultiple = 0;
                                switch (el.UnitVolumeID + "") {
                                    case "63":
                                        nMultiple = 1;
                                        break;
                                    case "64":
                                        nMultiple = 158.9873;
                                        break;
                                    case "65":
                                        nMultiple = 1000;
                                        break;
                                    case "2":
                                        nMultiple = +el.Density;
                                        break;
                                }
                                if ($.isNumeric(el.Volume)) {
                                    nDataInMonth += (+el.Volume * nMultiple);
                                }
                            })
                        } else {// Covert data to M3
                            $.each(DataVolumeInMonth, function (i, el) {
                                var nVal = el.Volume;
                                switch (el.UnitVolumeID + "") {
                                    case "63":
                                        nVal = ConvertLiterToM3(nVal + "");
                                        break;
                                    case "64":
                                        nVal = ConvertBarrelToM3(nVal + "");
                                        break;
                                    case "65":
                                        nVal = nVal;
                                        break;
                                    case "2":
                                        var nVolumn = +el.Volume;
                                        var nDensity = +el.Density;
                                        nVal = nVolumn * nDensity;
                                        nVal = ConvertLiterToM3(nVal + "");
                                        break;
                                }
                                if ($.isNumeric(nVal)) {
                                    nDataInMonth += (+nVal);
                                }
                            })
                        }
                    }
                    if (arrIncCheck[i] == "Y") {
                        nDataInMonth = 0;
                    } else {
                        if (nDataInMonth == 0) {
                            nDataInMonth = "";
                        }
                    }
                    switch (i) {
                        case 0:
                            incData.M1 = nDataInMonth;
                            break;
                        case 1:
                            incData.M2 = nDataInMonth;
                            break;
                        case 2:
                            incData.M3 = nDataInMonth;
                            break;
                        case 3:
                            incData.M4 = nDataInMonth;
                            break;
                        case 4:
                            incData.M5 = nDataInMonth;
                            break;
                        case 5:
                            incData.M6 = nDataInMonth;
                            break;
                        case 6:
                            incData.M7 = nDataInMonth;
                            break;
                        case 7:
                            incData.M8 = nDataInMonth;
                            break;
                        case 8:
                            incData.M9 = nDataInMonth;
                            break;
                        case 9:
                            incData.M10 = nDataInMonth;
                            break;
                        case 10:
                            incData.M11 = nDataInMonth;
                            break;
                        case 11:
                            incData.M12 = nDataInMonth;
                            break;
                    }
                    nDataInMonth = incData.ProductID == 209 ? CheckTextInput(nDataInMonth + "") : CheckTextOutput(nDataInMonth + "");
                    $tbDataMonth.find("input:not([type=checkbox])[nMonth=" + nMonth + "]" + (incData.ProductID == 209 ? "[class*=CountMonth]" : (incData.ProductID == 210 ? "[class*=2Liter]" : "[class*=2M3]"))).val(nDataInMonth);
                }
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

            var arrIncData = [lstIncData[0].M1, lstIncData[0].M2, lstIncData[0].M3, lstIncData[0].M4, lstIncData[0].M5, lstIncData[0].M6, lstIncData[0].M7, lstIncData[0].M8, lstIncData[0].M9, lstIncData[0].M10, lstIncData[0].M11, lstIncData[0].M12]
            var arrIncCheck = [lstIncData[0].IsCheckM1, lstIncData[0].IsCheckM2, lstIncData[0].IsCheckM3, lstIncData[0].IsCheckM4, lstIncData[0].IsCheckM5, lstIncData[0].IsCheckM6, lstIncData[0].IsCheckM7, lstIncData[0].IsCheckM8, lstIncData[0].IsCheckM9, lstIncData[0].IsCheckM10, lstIncData[0].IsCheckM11, lstIncData[0].IsCheckM12]

            if (nStatus == 0 || nStatus == 9999) {
                IsPass = false;
                for (var i = 0; i < 12 ; i++) {
                    if (arrIncCheck[i] == "Y" || arrIncData[i] != "") {
                        IsPass = true;
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
                        if (arrIncCheck[i] != "Y" && arrIncData[i] == "") {
                            IsPass = false
                            sMsg += "<br/>&nbsp;- Please specify " + arrFullMonth[i];
                        }
                    }
                }
            }

            if (IsPass) {
                var arrData = {
                    nIndicatorID: $ddlIndicator.val(),
                    nOperationID: $ddlOperationType.val(),
                    nFacilityID: $ddlFacility.val(),
                    sYear: $ddlYear.val(),
                    incData: lstIncData,
                    lstSpill: lstSpill,
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
                                lstPrdDeviate.push(Enumerable.From(lstIncData).Where(function (w) { return w.ProductID != 256 }).ToArray());
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
                DialogWarning(DialogHeader.Warning, "&bull; " + lstIncData[0].ProductName + sMsg);
            }
        }

        function EventInput() {
            $("input[id$=txtDateOfSpill]").on("change", function () {
                ReValidateFieldControl("divInput", $(this).attr("name"));
            });

            $tbDataMonth.delegate("input.target", "change", function () {
                var ProductID = $(this).attr("ProductID");
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
                var nVal = $(this).val();
                if (nVal != "") {
                    nVal = nVal.replace(/,/g, '');
                }
                Enumerable.From(lstIncData).FirstOrDefault(null, function (w) { return w.ProductID == ProductID }).Target = nVal;
            });

            $tbDataMonth.delegate("input[type=checkbox]", "ifChanged", function () {
                var el = $(this);
                var nMonth = +el.attr("nMonth");
                if (el.is(":checked")) {
                    var sComfirm = "The action comes into effect in " + arrFullMonth[nMonth - 1] + ", all data within this period will be automatically removed in result.<br/> Do you want save data ?";
                    DialogConfirmCloseButton(DialogHeader.Confirm, sComfirm, function () {
                        $.each(lstIncData, function (i, incData) {
                            var IsCheck = el.is(":checked") ? "Y" : "N";
                            switch (nMonth) {
                                case 1:
                                    incData.IsCheckM1 = IsCheck;
                                    break;
                                case 2:
                                    incData.IsCheckM2 = IsCheck;
                                    break;
                                case 3:
                                    incData.IsCheckM3 = IsCheck;
                                    break;
                                case 4:
                                    incData.IsCheckM4 = IsCheck;
                                    break;
                                case 5:
                                    incData.IsCheckM5 = IsCheck;
                                    break;
                                case 6:
                                    incData.IsCheckM6 = IsCheck;
                                    break;
                                case 7:
                                    incData.IsCheckM7 = IsCheck;
                                    break;
                                case 8:
                                    incData.IsCheckM8 = IsCheck;
                                    break;
                                case 9:
                                    incData.IsCheckM9 = IsCheck;
                                    break;
                                case 10:
                                    incData.IsCheckM10 = IsCheck;
                                    break;
                                case 11:
                                    incData.IsCheckM11 = IsCheck;
                                    break;
                                case 12:
                                    incData.IsCheckM12 = IsCheck;
                                    break;
                            }
                            CalculateData();
                            var DataInMonth = Enumerable.From(lstSpill).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray();
                            $.each(DataInMonth, function (i, el) {
                                el.IsShow = false;
                            })
                        })
                        bindDataSpill(lstSpill);
                        HideLoadding();
                    }, function () {
                        el.iCheck('Uncheck');
                    })
                } else {
                    var nMonth = +el.attr("nMonth");
                    var IsCheck = el.is(":checked") ? "Y" : "N";
                    $.each(lstIncData, function (i, incData) {
                        switch (nMonth) {
                            case 1:
                                incData.IsCheckM1 = IsCheck;
                                break;
                            case 2:
                                incData.IsCheckM2 = IsCheck;
                                break;
                            case 3:
                                incData.IsCheckM3 = IsCheck;
                                break;
                            case 4:
                                incData.IsCheckM4 = IsCheck;
                                break;
                            case 5:
                                incData.IsCheckM5 = IsCheck;
                                break;
                            case 6:
                                incData.IsCheckM6 = IsCheck;
                                break;
                            case 7:
                                incData.IsCheckM7 = IsCheck;
                                break;
                            case 8:
                                incData.IsCheckM8 = IsCheck;
                                break;
                            case 9:
                                incData.IsCheckM9 = IsCheck;
                                break;
                            case 10:
                                incData.IsCheckM10 = IsCheck;
                                break;
                            case 11:
                                incData.IsCheckM11 = IsCheck;
                                break;
                            case 12:
                                incData.IsCheckM12 = IsCheck;
                                break;
                        }
                        CalculateData();
                        var DataInMonth = Enumerable.From(lstSpill).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray();
                        $.each(DataInMonth, function (i, el) {
                            el.IsShow = true;
                        })
                    })
                    bindDataSpill(lstSpill);
                }
            })

            $("div#divInput select.HasOther,select[id$=ddlVolume]").on("change", function () {
                var sTxtID = $(this).attr("txtID");
                var IsCheckVal = false;
                UpdateStatusValidateControl("divInput", GetElementName(sTxtID, objControl.txtbox), "NOT_VALIDATED")
                if (sTxtID == "txtDensity") {
                    IsCheckVal = $("select[id$=ddlVolume]").val() == "2";
                } else {
                    IsCheckVal = lstOther.indexOf(+$(this).val()) > -1;
                }
                if (IsCheckVal) {
                    $("input[id$=" + sTxtID + "]").show();
                    ReValidateFieldControl("divInput", sTxtID);
                } else {
                    $("input[id$=" + sTxtID + "]").hide();
                    $("input[id$=" + sTxtID + "]").val("");
                    ReValidateFieldControl("divInput", GetElementName($(this).attr("id"), objControl.dropdown));
                }
                if ($(this).attr("id") == $("select[id$=ddlVolume]").attr("id")) {
                    if ($(this).val() == "2") {
                        $("input[id$=txtDensity]").change()
                    } else {
                        $("input[id$=txtVolume]").change();
                    }
                }
            })

            $("select[id$=ddlSpillType]").on("change", function () {
                AjaxCallWebMethod("ddlChange", function (response) {
                    LoaddinProcess();
                    $("select[id$=ddlSpillOf]").find("option").remove();
                    var sHtml = "<option value=''>- Select -</option>";
                    var data = response.d;
                    $.each(data, function (i, el) {
                        sHtml += "<option value='" + el.Value + "'>" + el.Text + "</option>";
                    });
                    $("select[id$=ddlSpillOf]").append(sHtml);
                }, function () {
                    $("select[id$=ddlSpillOf]").change();
                    HideLoadding();
                }, { ID: +$(this).val() });
            })

            $("input[id$=txtVolume],input[id$=txtDensity]").on("change", function () {
                $(this).val(CheckTextInput($(this).val()));
                var nCalculate2M3 = 0;
                var nVal = $(this).val().replace(/,/g, '');
                switch ($("select[id$=ddlVolume]").val() + "") {
                    case "63":
                        nCalculate2M3 = ConvertLiterToM3(nVal + "");
                        break;
                    case "64":
                        nCalculate2M3 = ConvertBarrelToM3(nVal + "");
                        break;
                    case "65":
                        nCalculate2M3 = nVal;
                        break;
                    case "2":
                        if (($("input[id$=txtVolume]").val().replace(/,/g, '') != "" && $("input[id$=txtVolume]").val().replace(/,/g, '') != "N/A")
                            && ($("input[id$=txtDensity]").val() != "" && $("input[id$=txtDensity]").val() != "N/A")) {
                            var nVolumn = +$("input[id$=txtVolume]").val().replace(/,/g, '');
                            var nDensity = +$("input[id$=txtDensity]").val().replace(/,/g, '');
                            nVal = nVolumn * nDensity;
                            nCalculate2M3 = ConvertLiterToM3(nVal + "");
                        } else {
                            nCalculate2M3 = "";
                        }
                        break;
                }
                if (IsNumberic(nCalculate2M3)) {
                    $("span[id$=lbCalculateM3]").show();
                    $("span[id$=lbCalculateM3]").html(CheckTextOutput(nCalculate2M3 + "") + "&nbsp;m<sup>3<sup>");
                } else {
                    $("span[id$=lbCalculateM3]").hide();
                }
                ReValidateFieldControl("divInput", "txtVolume");
            })

            $("select[id$=ddlLOPCTier]").on("change", function () {
                if ($(this).val() != "") {
                    $("a[id$=aHelperTier]").attr("href", "Helper_Indicator.aspx?ind=9&&prd=" + $(this).val());
                    $("a[id$=aHelperTier]").attr("target", "_blank");
                    $("a[id$=aHelperTier]").show();
                } else {
                    $("a[id$=aHelperTier]").removeAttr("href");
                    $("a[id$=aHelperTier]").hide();
                }
            })

            SetFileUploadOther();

            bindValidateFormInput();
        }

    </script>
</asp:Content>

