<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_EPI_FORMS.master" AutoEventWireup="true" CodeFile="epi_input_complaint.aspx.cs" Inherits="epi_input_complaint" %>

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
            <a style="font-size: 24px;" title="Helper Validated environmental complaint Used" href="Helper_Indicator.aspx?ind=1&&prd=0" target="_blank"><i class="fas fa-question-circle"></i></a>
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
            <button type="button" id="btnAddNon_Complaint" onclick="AddNon_Complaint()" class="btn btn-primary btn-sm text-left NoPRMS">Add New Complaint</button>
        </div>
        <div class="col-xs-12">
            <div class="table-responsive">
                <table id="tbDataList" class="table dataTable table-bordered table-hover">
                    <thead>
                        <tr>
                            <th class="text-center" style="vertical-align: middle; width: 10%;">No</th>
                            <th class="text-center" style="vertical-align: middle; width: 15%;">Issue Date</th>
                            <th class="text-center" style="vertical-align: middle; width: 20%;">Issued by</th>
                            <th class="text-center" style="vertical-align: middle; width: 40%;">Subject</th>
                            <th class="text-center" style="vertical-align: middle; width: 15%;"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-xs-12" id="divCreateComplaint" style="display: none">
            <div class="panel panel-info col">
                <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divCreate">
                    <i class="fas fa-edit"></i>Create / Update
                </div>
                <div class="panel-body panel-collapse collapse in" id="divCreate">
                    <div id="divInput">
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Complaint Type <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-4 col-lg-3">
                                <div class="input-group">
                                    <span class="input-group-addon" id="addOnComplaintType"><i class="fa fa-list"></i></span>
                                    <asp:DropDownList runat="server" ID="ddlComplaintType" CssClass="form-control input-sm" aria-describedby="addOnComplaintType"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Impact type <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-9">
                                <div class="form-inline">
                                    <asp:CheckBoxList runat="server" ID="cblImpactType" CssClass="flat-green" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    </asp:CheckBoxList>
                                    <input id="txtOther" name="txtOther" class="form-control input-sm" disabled maxlength="1000" placeholder="Specify name" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Issue Date <span class="text-red">*</span> <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-4 col-lg-3">
                                <div class="input-group">
                                    <span class="input-group-addon" id="addOnIssueDate"><i class="fa fa-calendar-alt"></i></span>
                                    <input class="form-control input-sm" id="txtIssueDate" name="txtIssueDate" placeholder="--/--/----" aria-describedby="addOnIssueDate" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Issued by <span class="text-red ">*</span>  <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-6 col-lg-6">
                                <div class="input-group">
                                    <span class="input-group-addon" id="addOnIssueBy"><i class="fa fa-pencil-alt"></i></span>
                                    <input class="form-control input-sm" id="txtIssueBy" name="txtIssueBy" maxlength="250" aria-describedby="addOnIssueBy" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Subject <span class="text-red ">*</span>  <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-6 col-lg-6">
                                <div class="input-group">
                                    <span class="input-group-addon" id="addOnSubject"><i class="fa fa-pencil-alt"></i></span>
                                    <input class="form-control input-sm" id="txtSubject" name="txtSubject" maxlength="250" aria-describedby="addOnSubject" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Complaint by <span class="text-red ">*</span>  <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-8 col-lg-8">
                                <div class="form-inline">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="addOnComplaintBy"><i class="fa fa-list"></i></span>
                                        <asp:DropDownList runat="server" ID="ddlComplaintBy" CssClass="form-control input-sm" aria-describedby="addOnComplaintBy">
                                        </asp:DropDownList>
                                    </div>
                                    <input id="txtComplaintBy" class="form-control input-sm" name="txtComplaintBy" maxlength="1000" style="display: none;" placeholder="Specify name" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Detail <span class="text-red ">*</span>  <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-9">
                                <textarea class="form-control" rows="4" style="resize: vertical;" id="txtDetail" name="txtDetail"></textarea>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Corrective action <span class="text-red ">*</span>  <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-9">
                                <textarea class="form-control" rows="4" style="resize: vertical;" id="txtCorectiveAction" name="txtCorectiveAction"></textarea>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 col-md-3 col-lg3 text-left-xs text-right-lg text-right-md">
                                Status <span class="text-red ">*</span>  <span class="hidden-xs">:</span>
                            </div>
                            <div class="col-xs-12 col-md-9 col-lg-9">
                                <asp:RadioButtonList runat="server" ID="rblStatus" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="C" Text="Closed" Selected="True" class="flat-green radio radio-inline"></asp:ListItem>
                                    <asp:ListItem Value="F" Text="Follow up" class="flat-green radio radio-inline"></asp:ListItem>
                                </asp:RadioButtonList>
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
                    <div id="divButtonComplaint">
                        <div class="col-xs-12 text-center">
                            <button type="button" id="btnSaveNonComplaint" class="btn btn-primary" onclick="saveNon_Complaint()">Save this Complaint</button>
                            <button type="button" class="btn btn-default" onclick="CancelAdd()">Cancel this Complaint</button>
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
        var $divCreateComplaint = $("div[id$=divCreateComplaint]");
        var $btnAddNon_Complaint = $("button[id$=btnAddNon_Complaint]");
        var $hdfManageID = $("input[id$=hdfManageID]");
        var incData = {};
        var lstComplaint = []
        var IsFullMonth = true;
        $(function () {
            ArrInputFromTableID.push("tbDataMonth");
            $("input[id$=txtIssueDate]").on("change", function () {
                ReValidateFieldControl("divInput", $(this).attr("name"));
            });

            $tbDataMonth.delegate("input.target", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
                var nVal = $(this).val();
                if (nVal != "") {
                    nVal = nVal.replace(/,/g, '');
                }
                incData.sTarget = nVal;
            });

            $tbDataMonth.delegate("input[type=checkbox]", "ifChanged", function () {
                var el = $(this);
                var nMonth = +el.attr("nMonth");
                if (el.is(":checked")) {
                    var sComfirm = "The action comes into effect in " + arrFullMonth[nMonth - 1] + ", all data within this period will be automatically removed in result.<br/> Do you want save data ?";
                    DialogConfirmCloseButton(DialogHeader.Confirm, sComfirm, function () {
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
                        var DataInMonth = Enumerable.From(lstComplaint).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray();
                        $.each(DataInMonth, function (i, el) {
                            el.IsShow = false;
                        })
                        bindDataComplaint(lstComplaint);
                        HideLoadding();
                    }, function () {
                        el.iCheck('Uncheck');
                    })
                } else {
                    var nMonth = +el.attr("nMonth");
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
                    var DataInMonth = Enumerable.From(lstComplaint).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray();
                    $.each(DataInMonth, function (i, el) {
                        el.IsShow = true;
                    })
                    bindDataComplaint(lstComplaint);
                }
            })

            $("input[id*=cblImpactType][value=8]").on("ifChanged", function () {
                UpdateStatusValidateControl("divInput", GetElementName("txtOther", objControl.txtbox), "NOT_VALIDATED")
                if ($(this).is(":checked")) {
                    $("input[id$=txtOther]").prop("disabled", false);
                    ReValidateFieldControl("divInput", GetElementName("txtOther", objControl.txtbox));
                } else {
                    $("input[id*=cblImpactType]").not("[value=8]").change()
                    $("input[id$=txtOther]").val("").prop("disabled", true);
                }
            })

            $("select[id$=ddlComplaintBy]").on("change", function () {
                UpdateStatusValidateControl("divInput", GetElementName("txtComplaintBy", objControl.txtbox), "NOT_VALIDATED")
                if ($(this).val() == 12) {
                    $("input[id$=txtComplaintBy]").show();
                    ReValidateFieldControl("divInput", "txtComplaintBy");
                } else {
                    $("input[id$=txtComplaintBy]").hide();
                    $("input[id$=txtComplaintBy]").val("");
                    ReValidateFieldControl("divInput", GetElementName("ddlComplaintBy", objControl.dropdown));
                }
            })

            SetFileUploadOther();

            $("input[id*=cblImpactType]").on("ifChanged", function () {
                ReValidateFieldControl("divInput", GetElementNameICheck("cblImpactType"));
            })
            bindValidateFormInput();
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
                    incData = {};
                    lstComplaint = [];
                    incData = response.d.incData;
                    lstComplaint = response.d.lstComplaint;
                    lstStatus = response.d.lstStatus;
                    $tbDataMonth.empty();
                    bindDataMonth(response.d.incData);
                    bindDataComplaint(response.d.lstComplaint);
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
                            $btnAddNon_Complaint.show();
                            $("input.target").prop("disabled", false);
                        } else {
                            $btnAddNon_Complaint.hide();
                            $("input.target").prop("disabled", true);
                        }
                    } else {
                        $btnAddNon_Complaint.show();
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

        function bindDataMonth(el) {
            var sHtml = "";
            var lstDataMonth = [el.M1, el.M2, el.M3, el.M4, el.M5, el.M6, el.M7, el.M8, el.M9, el.M10, el.M11, el.M12]
            var lstDataCheck = [el.IsCheckM1, el.IsCheckM2, el.IsCheckM3, el.IsCheckM4, el.IsCheckM5, el.IsCheckM6, el.IsCheckM7, el.IsCheckM8, el.IsCheckM9, el.IsCheckM10, el.IsCheckM11, el.IsCheckM12]
            sHtml += "<thead>";
            sHtml += "  <tr>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthIndicator + "px;'><label>Indicator</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'><label>Unit</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;'><label>Target</label></th>";
            sHtml += bindColumnQ(true, null, lstDataCheck);
            sHtml += "  </tr>";
            sHtml += "</thead>";
            sHtml += "<tbody>";
            sHtml += "  <tr>";
            sHtml += "      <td class='text-left' style='vertical-align: middle;'>" + el.sProductName + "</td>";
            sHtml += "      <td class='text-center' style='vertical-align: middle;'>" + el.sUnit + "</td>";
            sHtml += "      <td class='text-center cTarget' style='vertical-align: middle;'><input class='form-control input-sm text-right target'  value ='" + CheckTextInput(el.sTarget) + "' maxlength='20'/></td>";
            sHtml += bindColumnQ(false, lstDataMonth, null);
            sHtml += "  </tr>";
            sHtml += "</tbody>";
            $tbDataMonth.append(sHtml);
        }

        function bindColumnQ(IsTHead, dataMonth, dataCheck) {
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
                    sHtml += "<td class='text-center M_" + i + " QHead_" + getQrt(i) + "'><input class='form-control input-sm' nMonth ='" + i + "' value ='" + CheckTextInput(dataMonth[i - 1]) + "' disabled/></td>";
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

        function bindDataComplaint(lstComplaint) {
            var sHtml = "";
            $tbDataList.find("tbody tr").remove();
            lstComplaint = Enumerable.From(lstComplaint).Where(function (w) { return !w.IsDel && w.IsShow }).ToArray();
            if (lstComplaint.length > 0) {
                sHtml += "<tr>";
                $.each(lstComplaint, function (i, el) {
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
                    //var sBtnEdit = "<button type='button' title='" + sTitle + "'  class='btn " + sClass + " btn-sm' value='" + el.nComplaintID + "' onclick='EditNon_Complaint(" + el.nComplaintID + "," + IsSubmited + ")'><i class='fa fa-" + sIcon + "'></i></button>";
                    //var sBtnDel = "<button type='button' title='Delete' class='btn btn-danger btn-sm' value='" + el.nComplaintID + "' onclick='DelNon_Complaint(" + el.nComplaintID + ")'><i class='fa fa-trash'></i></button>";
                    var sBtnEdit = "<button type='button'  class='btn " + sClass + " btn-sm' value='" + el.nComplaintID + "' onclick='EditNon_Complaint(" + el.nComplaintID + "," + IsSubmited + ")'><i class='fa fa-" + sIcon + "'></i></button>";
                    var sBtnDel = "<button type='button'  class='btn btn-danger btn-sm' value='" + el.nComplaintID + "' onclick='DelNon_Complaint(" + el.nComplaintID + ")'><i class='fa fa-trash'></i></button>";
                    sHtml += "  <td class='text-center'>" + (i + 1) + "</td>";
                    sHtml += "  <td class='text-center'>" + el.sIssueDate + "</td>";
                    sHtml += "  <td class='text-center'>" + el.sIssueBy + "</td>";
                    sHtml += "  <td class='text-left'>" + el.sSubject + "</td>";
                    sHtml += "  <td class='text-center'>" + sBtnEdit + (!IsSubmited ? "&nbsp;" + sBtnDel : "") + "</td>";
                    sHtml += "</tr>";
                })
                $tbDataList.find("tbody").append(sHtml);

                SetTootip();
            } else {
                SetRowNoData($tbDataList.attr("id"), 6);
            }
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
                    data: { funcname: "UPLOAD", savetopath: 'Complaint/Temp/', savetoname: '' },
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
                        var ItemData = Enumerable.From(lstComplaint).FirstOrDefault(null, function (w) { return w.nComplaintID == $hdfManageID.val() });
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
            var lstData = Enumerable.From(lstFile).Where(function (x) { return x.sDelete == "N"}).ToArray();
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
            var ItemData = Enumerable.From(lstComplaint).FirstOrDefault(null, function (w) { return w.nComplaintID == $hdfManageID.val() });
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

        function bindValidateFormInput() {
            var objValidate = {};
            objValidate[GetElementName("txtIssueDate", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Issue date");
            objValidate[GetElementName("txtIssueBy", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Issued by");
            objValidate[GetElementName("txtSubject", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Subject ");
            objValidate[GetElementName("txtComplaintBy", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify);
            objValidate[GetElementName("txtOther", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify);
            objValidate[GetElementName("txtCorectiveAction", objControl.txtarea)] = addValidate_notEmpty(DialogMsg.Specify + " Corrective action");
            objValidate[GetElementName("txtDetail", objControl.txtarea)] = addValidate_notEmpty(DialogMsg.Specify + " Detail");
            objValidate[GetElementName("ddlComplaintType", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Complaint type");
            objValidate[GetElementName("ddlComplaintBy", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Complaint by");
            $("input[id*=cblImpactType]").attr("name", "cblImpactType");
            objValidate["cblImpactType"] = addValidateCheckbox_notEmpty(DialogMsg.Specify + " Impact type");
            BindValidate("divInput", objValidate);
        }

        function AddNon_Complaint() {
            $btnAddNon_Complaint.prop("disabled", true);
            $("table[id$=tbDataList]").find("button").prop("disabled", true);
            $tbDataMonth.find("input[type=checkbox]").prop("disabled", true);
            $("div[id$=dvbtn]").find("button").prop("disabled", true);
            var nMaxID = lstComplaint.length > 0 ? Enumerable.From(lstComplaint).Max(function (m) { return m.nComplaintID }) + 1 : 1;
            lstComplaint.push({
                nComplaintID: nMaxID,
                nComplaintTypeID: "",
                lstImpact: [],
                sIssueDate: "",
                sIssueBy: "",
                sSubject: "",
                nComplaintByID: "",
                sComplaintByOther: "",
                sDetail: "",
                sCorrectiveAction: "",
                nMonth: "",
                lstFile: [],
                IsDel: false,
                IsNew: true,
                IsSubmited: false,
                IsShow: true,
            })
            $hdfManageID.val(nMaxID);
            var dataManage = Enumerable.From(lstComplaint).FirstOrDefault(null, function (w) { return w.nComplaintID == nMaxID });
            bindValidateFormInput();
            BindTableFileOther(dataManage.lstFile);
            setDatePicker();
            $divCreateComplaint.show();
            ScrollTopToElementsTo("divCreateComplaint", 70);
        }

        function EditNon_Complaint(nComplaintID, IsSubmited) {
            $btnAddNon_Complaint.prop("disabled", true);
            $tbDataList.find("button").prop("disabled", true);
            $tbDataMonth.find("input[type=checkbox]").prop("disabled", true);
            $("div[id$=dvbtn]").find("button").prop("disabled", true);
            $hdfManageID.val(nComplaintID);
            var data = Enumerable.From(lstComplaint).FirstOrDefault(null, function (w) { return w.nComplaintID == $hdfManageID.val() });
            BindTableFileOther(data.lstFile);
            $("select[id$=ddlComplaintType]").val(data.nComplaintTypeID);
            $.each(data.lstImpact, function (i, el) {
                if (el.nImpactTypeID == "8") {
                    Input("txtOther").prop("disabled", false);
                    Input("txtOther").val(el.sOther);
                }
                $("input[id*=cblImpactType][value=" + el.nImpactTypeID + "]").iCheck("check")
            })
            Input("txtIssueDate").val(data.sIssueDate);
            Input("txtIssueBy").val(data.sIssueBy);
            Input("txtSubject").val(data.sSubject);
            $("select[id$=ddlComplaintBy]").val(data.nComplaintByID);
            if (data.nComplaintByID == "12") {
                Input("txtComplaintBy").show();
            }
            Input("txtComplaintBy").val(data.sComplaintByOther);
            SetValueTextArea("txtDetail", data.sDetail);
            SetValueTextArea("txtCorectiveAction", data.sCorrectiveAction);
            if (IsSubmited) {
                $divCreateComplaint.find("input,textarea,select").prop("disabled", true);
                $("div[id$=divCreate]").find(".NoPRMS").hide();
                $("button[id$=btnSaveNonComplaint]").hide();
            }
            $("input[id*=rblStatus][value=" + data.sStatus + "]").iCheck("check");
            setDatePicker(Input("txtIssueDate"));
            $divCreateComplaint.show();
            bindValidateFormInput();
            ScrollTopToElementsTo("divCreateComplaint", 70);
        }

        function DelNon_Complaint(nComplaintID) {
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                var data = Enumerable.From(lstComplaint).FirstOrDefault(null, function (w) { return w.nComplaintID == nComplaintID });
                if (!data.IsNew) {
                    data.IsDel = true;
                    data.IsShow = false;
                } else {
                    lstComplaint = Enumerable.From(lstComplaint).Where(function (w) { return w.nComplaintID != nComplaintID }).ToArray();
                }
                CalculateData();
                bindDataComplaint(lstComplaint);
                HideLoadding();
            });
        }

        function CancelAdd() {
            var data = Enumerable.From(lstComplaint).FirstOrDefault(null, function (w) { return w.nComplaintID == $hdfManageID.val() });
            $.each(data.lstFile, function (i, el) {
                if (el.sDelete == "Y" && el.IsCompleted == true) {
                    el.sDelete = "N";
                }
            })
            data.lstFile = Enumerable.From(data.lstFile).Where(function (w) { return w.IsCompleted == true }).ToArray();
            if (!data.IsSubmited) {
                lstComplaint = Enumerable.From(lstComplaint).Where(function (w) { return w.nComplaintID != $hdfManageID.val() }).ToArray();
            }
            ClearDataInput();
        }

        function saveNon_Complaint() {
            if (CheckValidate("divInput")) {

                var nMonth = +Input("txtIssueDate").val().split("/")[1];

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
                            var DataInMonth = Enumerable.From(lstComplaint).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray();
                            $.each(DataInMonth, function (i, el) {
                                el.IsShow = false;
                                el.IsDel = true;
                            })
                            var data = Enumerable.From(lstComplaint).FirstOrDefault(null, function (w) { return w.nComplaintID == $hdfManageID.val() });
                            data.nComplaintTypeID = GetValDropdown("ddlComplaintType");
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
                            var lstAddImpact = [];
                            $.each($("input[id*=cblImpactType]:checked"), function (i, el) {
                                var sOther = "";
                                el = $(el);
                                if (el.val() == "8") {
                                    sOther = Input("txtOther").val();
                                }
                                lstAddImpact.push({
                                    nImpactTypeID: el.val(),
                                    sOther: sOther
                                });
                            })
                            data.lstImpact = lstAddImpact;
                            data.sIssueDate = Input("txtIssueDate").val();
                            data.sIssueBy = Input("txtIssueBy").val();
                            data.sSubject = Input("txtSubject").val();
                            data.nComplaintByID = GetValDropdown("ddlComplaintBy");
                            data.sComplaintByOther = Input("txtComplaintBy").val();
                            data.sDetail = GetValTextArea("txtDetail");
                            data.sCorrectiveAction = GetValTextArea("txtCorectiveAction");
                            data.nMonth = nMonth;
                            data.sStatus = GetValRadioListICheck("rblStatus");
                            data.IsSubmited = true;
                            ClearDataInput();
                            $tbDataMonth.find("input[type=checkbox][nMonth=" + nMonth + "]").iCheck("Uncheck");
                            HideLoadding();
                        });
                    } else {
                        var data = Enumerable.From(lstComplaint).FirstOrDefault(null, function (w) { return w.nComplaintID == $hdfManageID.val() });
                        data.nComplaintTypeID = GetValDropdown("ddlComplaintType");
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
                        var lstAddImpact = [];
                        $.each($("input[id*=cblImpactType]:checked"), function (i, el) {
                            var sOther = "";
                            el = $(el);
                            if (el.val() == "8") {
                                sOther = Input("txtOther").val();
                            }
                            lstAddImpact.push({
                                nImpactTypeID: el.val(),
                                sOther: sOther
                            });
                        })
                        data.lstImpact = lstAddImpact;
                        data.sIssueDate = Input("txtIssueDate").val();
                        data.sIssueBy = Input("txtIssueBy").val();
                        data.sSubject = Input("txtSubject").val();
                        data.nComplaintByID = GetValDropdown("ddlComplaintBy");
                        data.sComplaintByOther = Input("txtComplaintBy").val();
                        data.sDetail = GetValTextArea("txtDetail");
                        data.sCorrectiveAction = GetValTextArea("txtCorectiveAction");
                        data.nMonth = nMonth;
                        data.IsSubmited = true;
                        data.sStatus = GetValRadioListICheck("rblStatus");
                        ClearDataInput();
                    }
                }
            }
        }

        function ClearDataInput() {
            var arrObj = [];
            $divCreateComplaint.find("input,textarea,select").prop("disabled", false);
            $("div[id$=divInput]").find("input:not([id$=txtOther],[id*=cblImpactType],[id*=rblStatus]),textarea,select").val("").change().prop("disabled", false);
            $("input[id*=rblStatus]:eq(0)").iCheck("check")

            $("div[id$=dvbtn]").find("button").prop("disabled", false);
            $("input[id*=cblImpactType]").iCheck("Uncheck");
            $.each($("div[id$=divInput]").find("input,textarea,select"), function () {
                arrObj.push($(this).attr("name"));
            })
            $("div[id$=divCreate]").find(".NoPRMS").show();
            $("button[id$=btnSaveNonComplaint]").show();
            UpdateStatusValidate("divInput", arrObj);
            $("div[id$=divInput]").find("input[id$=txtOther]").prop("disabled", true);
            $btnAddNon_Complaint.prop("disabled", false);
            $tbDataList.find("button").prop("disabled", false);
            $tbDataMonth.find("input[type=checkbox]").not("[class$=submited]").prop("disabled", false);
            $divCreateComplaint.hide();
            $hdfManageID.val("");
            CalculateData();
            bindDataComplaint(lstComplaint);
        }

        function CalculateData() {
            var arrIncData = [incData.M1, incData.M2, incData.M3, incData.M4, incData.M5, incData.M6, incData.M7, incData.M8, incData.M9, incData.M10, incData.M11, incData.M12]
            var arrIncCheck = [incData.IsCheckM1, incData.IsCheckM2, incData.IsCheckM3, incData.IsCheckM4, incData.IsCheckM5, incData.IsCheckM6, incData.IsCheckM7, incData.IsCheckM8, incData.IsCheckM9, incData.IsCheckM10, incData.IsCheckM11, incData.IsCheckM12]
            for (var i = 0 ; i < 12 ; i++) {
                var nMonth = i + 1;
                var nDataInMonth = Enumerable.From(lstComplaint).Where(function (w) { return w.nMonth == nMonth && !w.IsDel }).ToArray().length;
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
                $tbDataMonth.find("input[nMonth=" + nMonth + "]").val(nDataInMonth);
            }
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

            var arrIncData = [incData.M1, incData.M2, incData.M3, incData.M4, incData.M5, incData.M6, incData.M7, incData.M8, incData.M9, incData.M10, incData.M11, incData.M12]
            var arrIncCheck = [incData.IsCheckM1, incData.IsCheckM2, incData.IsCheckM3, incData.IsCheckM4, incData.IsCheckM5, incData.IsCheckM6, incData.IsCheckM7, incData.IsCheckM8, incData.IsCheckM9, incData.IsCheckM10, incData.IsCheckM11, incData.IsCheckM12]

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
                    incData: incData,
                    lstComplaint: lstComplaint,
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
                                lstPrdDeviate.push(incData);
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
                //DialogConfirm(DialogHeader.Confirm, sMsgComfirmAlert, function () {
                //    AjaxCallWebMethod("SaveToDB", function (response) {
                //        if (response.d.Status == SysProcess.SessionExpired) {
                //            PopupLogin();
                //        } else if (response.d.Status == SysProcess.Success) {
                //            if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                //                DialogSuccessRedirect(DialogHeader.Info, sMsgComplete, "epi_mytask.aspx");
                //            } else {
                //                DialogSuccess(DialogHeader.Info, sMsgComplete);
                //                LoadDataCheckddl();
                //            }
                //        } else {
                //            DialogWarning(DialogHeader.Warning, response.d.Msg);
                //        }
                //    }, function () { ClearDataInput(); HideLoadding(); }, { arrData: arrData, })
                //})
            } else {
                DialogWarning(DialogHeader.Warning, "&bull; " + incData.sProductName + sMsg);
            }
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

        function setDatePicker(ctrl) {
            var sDefaultDate = "";
            if (ctrl != "" && ctrl != null && ctrl != undefined) {
                ctrl = $(ctrl);
                sDefaultDate = ctrl.val();
            }
            $("input[id$=txtIssueDate]").datepicker("remove");
            $("input[id$=txtIssueDate]").datepicker({
                setDate: sDefaultDate,
                format: 'dd/mm/yyyy',
                autoclose: true,
                startDate: new Date("01/01/" + $ddlYear.val()),
                endDate: new Date("12/31/" + $ddlYear.val()),
            });
        }
    </script>
</asp:Content>

