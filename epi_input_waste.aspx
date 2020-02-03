<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_EPI_FORMS.master" AutoEventWireup="true" CodeFile="epi_input_waste.aspx.cs" Inherits="epi_input_waste" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .ctxtRight {
            text-align: right;
        }

        .cStyleDivTrash {
            background-color: #ffdfda !important;
            cursor: pointer;
        }

        .cExStyle {
            text-align: right;
            margin-bottom: 10px;
        }

        .modal-backdrop {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="row" id="divEditFrom">
        <%-- Export --%>
        <div class="col-xs-12 col-md-6 text-left" style="margin-bottom: 5px;">
            <a style="font-size: 24px;" title="Helper Hazardous Waste" href="Helper_Indicator.aspx?ind=10&&prd=1" target="_blank"><i class="fas fa-question-circle"></i></a>
        </div>
        <div class="col-xs-12 col-md-6 text-right-lg text-right-md text-left-sm" style="margin-bottom: 5px;" id="divExport">
            <button type="button" onclick="ShowDeviate();" class="btn btn-info" title="Deviate History"><i class="fas fa-comments"></i></button>
            <button type="button" onclick="ShowHistory();" class="btn btn-info" title="Workflow History"><i class="fas fa-comment-alt"></i></button>
            <%-- <button type="button" onclick="ExportData();" class="btn btn-success">Export</button>         
              <asp:Button ID="btnEx" runat="server" CssClass="hidden" OnClick="btnEx_Click" />--%>
            <asp:LinkButton runat="server" ID="btnEx" CssClass="btn btn-success" OnClick="btnEx_Click">Export</asp:LinkButton>
        </div>
        <%-- HZD --%>
        <div class="col-xs-12">
            <div id="divHZD">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#HZD" data-toggle="collapse" style="cursor: pointer;">Hazardous Waste</div>
                        <div id="HZD" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive" style="margin-top: 20px;">
                                        <table id="tbHZD" class="table dataTable table-bordered table-hover" style="width: 2150px; min-width: 100%; margin-top: -1px !important;">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <%-- Remark --%>
                            <div id="divRemarkHZD">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12">
            <a style="font-size: 24px;" title="Helper Hazardous Waste" href="Helper_Indicator.aspx?ind=10&&prd=16" target="_blank"><i class="fas fa-question-circle"></i></a>
        </div>
        <%-- NHZD --%>
        <div class="col-xs-12">
            <div id="divNHZD">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#NHZD" data-toggle="collapse" style="cursor: pointer;">Non Hazardous Waste</div>
                        <div id="NHZD" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive" style="margin-top: 20px;">
                                        <table id="tbNHZD" class="table dataTable table-bordered table-hover" style="width: 2150px; min-width: 100%; margin-top: -1px !important;">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <%-- Remark --%>
                            <div id="divRemarkNHZD">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- municipal --%>
        <div class="col-xs-12">
            <div id="divMul">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#Mul" data-toggle="collapse" style="cursor: pointer;">Other municipal waste</div>
                        <div id="Mul" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive" style="margin-top: 20px;">
                                        <table id="tbMul" class="table dataTable table-bordered table-hover" style="width: 2150px; min-width: 100%; margin-top: -1px !important;">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <%-- Remark --%>
                            <div id="divRemarkMul">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- File --%>
        <div class="col-xs-12" id="divFile">
            <%--  <label class="control-label col-xs-12 text-left-sm">Attach File :</label>--%>
            <div class="col-xds-12">
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
    <asp:HiddenField ID="hidYear" runat="server" />
    <asp:HiddenField ID="hidIndicator" runat="server" />
    <asp:HiddenField ID="hidOperationType" runat="server" />
    <asp:HiddenField ID="hidFacility" runat="server" />
    <asp:HiddenField ID="hidFromID" runat="server" />
    <%-- Status --%>
    <asp:HiddenField ID="hidStatusWF" runat="server" />
    <asp:HiddenField ID="hdfIsAdmin" runat="server" />
    <asp:HiddenField ID="hdfRole" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="Scripts/Fileupload/src/jquery.fileuploader.js"></script>
    <link href="Scripts/Fileupload/src/jquery.fileuploader.css" rel="stylesheet" />

    <script>
        var arrHZD = [];
        var arrQ = ["1", "2", "3", "4"];
        var arrSub = [];
        var arrNHZD = [];
        var arrMask = [];
        var arrDel = [];
        var arrSumit = [];
        var arrCheckMonthNoData = [];
        var arrMul = [];

        var $hidFromID = $("input[id$=hidFromID]");
        var $hidStatusWF = $("input[id$=hidStatusWF]");
        var $hdfIsAdmin = $("input[id$=hdfIsAdmin]");

        $(document).ready(function () {
            ArrInputFromTableID.push("tbHZD");
            ArrInputFromTableID.push("tbNHZD");
            ArrInputFromTableID.push("tbMul");
            $("#divEditFrom").hide();
            SetFileUploadOther();
        });

        function LoadData() {
            BlockUI();
            var item = {
                nIndicator: +GetValDropdown("ddlIndicator"),
                nOperationType: +GetValDropdown("ddlOperationType"),
                nFacility: +GetValDropdown("ddlFacility"),
                sYear: GetValDropdown("ddlYear"),
            };

            AjaxCallWebMethod("ListData", function (data) {

                if (data.d.Status == SysProcess.Success) {
                    arrHZD = data.d.lstHZD;
                    arrSub = data.d.lstSub;
                    arrMask = data.d.lstMarsk;
                    arrNHZD = data.d.lstNHZD;
                    lstStatus = data.d.lstMonth;
                    arrSumit = data.d.lstMonthCheck;
                    arrMonthRecall = data.d.lstRecall;
                    $hidStatusWF.val(data.d.nStatusWF);
                    $hdfPRMS.val(data.d.hidPrms);

                    arrMul = data.d.lstMul;

                    nStatus = data.d.nStatusWF;
                    if (nStatus == 1) {
                        IsRecall = true;
                    }

                    $("input[id$=hidFromID]").val(data.d.sFormID);
                    if (arrHZD.length > 0) {
                        BindTable(arrHZD, "HZD");
                        $("#divHZD").show();
                    }
                    if (arrNHZD.length > 0) {
                        BindTable(arrNHZD, "NHZD");
                        $("#divNHZD").show();
                    }

                    if (arrHZD.length > 0 || arrNHZD.length > 0) {
                        $("#divEditFrom").show();
                        LoadDataFileOther();
                    }
                    else {
                        nStatus = -1;
                    }

                    BindTableMunicipal(arrMul, "MUL");

                    CheckboxQuarterChanged();
                    CheckEventButton();
                }
                else if (data.d.Status == SysProcess.Failed) {
                    DialogError(DialogHeader.Error, data.d.Msg);
                    //DialogWarning(DialogHeader.Warning, response.d.Msg);
                }
                else if (response.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                }
                UnblockUI();

            }, UnblockUI, { item: item });

        }
        function BindTable(arr, sWasteType) {
            if (arr.length > 0) {
                var sTableName = "#tb" + sWasteType;
                var sDivMarsk = "#divRemark" + sWasteType;
                var sRiskName = ""; var sTable = ""; var tr = ""; var Icon = "";
                var nQ = (arrQ.length * 3) + 1;
                var divMarsk = "";
                var nYearNow = +GetValDropdown("ddlYear");
                var nYearOld = (nYearNow - 1);

                $(sTableName).empty();
                $(sDivMarsk).empty();
                sTable += '<thead>'
               + '<tr>'
                + '<th class="text-center" style="vertical-align: middle; width:400px;"><label>Indicator</label></th>'
                + '<th class="text-center" style="vertical-align: middle; width:120px;"><label>Disposal Code</label></th>'
                + '<th class="text-center" style="vertical-align: middle; width:120px;"><label>Unit</label></th>'
                + '<th class="text-center" style="vertical-align: middle;width:120px;"><label>Target</label></th>';
                sTable += CreateQtr(arrQ, 0, "", "");
                sTable += '</tr>'
                + '</thead>';

                ////////////// *********** BODY  ************ \\\\\\\\\\\\\\\\\

                for (var i = 0; i < arr.length; i++) {
                    var arrP = arr[i].lstProduct;
                    var isProduct = arrP.length > 0 ? true : false;
                    if (arr[i].nGroupCalc == 99) {
                        var sIDOld = "txt" + sWasteType + "_Old_" + arr[i].ProductID;
                        var sIDNew = "txt" + sWasteType + "_New_" + arr[i].ProductID;

                        tr += '<tr style="background-color: #fabd4f;">'
                           + '<td class="dt-body-left">' + arr[i].sSetHtml + arr[i].ProductName + setTooltipProduct(arr[i].sTooltip) + '</td>'
                           + '<td class="dt-body-center"></td>'
                           + '<td class="dt-body-center">' + arr[i].sUnit + '</td>'
                           + '<td class="dt-body-center">Previous year<br>( ' + nYearOld + ' )'
                           + '<input id="' + sIDOld + '" maxlength="20" onchange="Cal($(this),\'O\',\'' + sWasteType + '\');" class="form-control ctxtRight input-sm" value="' + (isProduct ? arrP[0].PreviousYear : "") + '">'
                           + '</td>'
                           + '<td class="dt-body-center">Reporting year<br>( ' + nYearNow + ' )'
                           + '<input id="' + sIDNew + '" maxlength="20" onchange="Cal($(this),\'N\',\'' + sWasteType + '\');"  class="form-control ctxtRight input-sm" value="' + (isProduct ? arrP[0].ReportingYear : "") + '"></td>'
                        + '</tr>';

                        var sMaskID = "txtRemarsk_" + arr[i].ProductID;
                        divMarsk += '<div class="col-xs-12 col-md-4" style="margin-top: 20px;">'
                        + '<div class="well">'
                        + '<div class="form-group">'
                        + '<label class="control-label">Remark ( ' + arr[i].ProductName + ')<span class="text-red">*</span></label>'
                        + '<textarea id="' + sMaskID + '" class="form-control" rows="4"></textarea>'
                        + ' </div></div></div>';
                    }
                    else if (arr[i].nGroupCalc == 12) {
                        var sVal = isProduct ? arrP[0].nTotal != 0 ? CheckTextOutput(arrP[0].nTotal) : "" : "";
                        tr += '<tr style="background-color: #fabd4f;">'
                           + '<td class="dt-body-left">' + arr[i].sSetHtml + arr[i].ProductName + setTooltipProduct(arr[i].sTooltip) + '</td>'
                           + '<td class="dt-body-center"></td>'
                           + '<td class="dt-body-center">' + arr[i].sUnit + '</td>'
                           + '<td class="dt-body-center"><input id="txt' + sWasteType + '_Total_' + arr[i].ProductID + '" maxlength="20"  class="form-control ctxtRight input-sm "  value="' + (sVal) + '" disabled></td>'
                           + '</tr>';
                    }
                    else if (arr[i].cTotal == "Y") {

                        if (arr[i].cTotal == "Y" && arr[i].cTotalAll == "Y") {
                            var sNameID = "txt" + sWasteType + "_A";
                            tr += '<tr id="tr_' + arr[i].ProductID + '" style="background-color: #dbea97;">'
                                   + '<td class="dt-body-left">' + arr[i].sSetHtml + arr[i].ProductName + setTooltipProduct(arr[i].sTooltip) + '</td>'
                                   + '<td class="dt-body-center"></td>'
                                   + '<td class="dt-body-center">' + arr[i].sUnit + '</td>'
                                   + '<td class="dt-body-center cTarget"><input id="' + sNameID + '_Target_' + arr[i].ProductID + '" maxlength="20"  onchange="Chang(this,\'A\',\'' + sWasteType + '\');"   class="form-control ctxtRight input-sm" value="' + (isProduct ? CheckTextInput(arrP[0].Target) : "") + '"></td>';
                            /// หัวใหญ่สุด                           
                            tr += '' + CreateVal(arrQ, arrP[0], 0, sNameID, arr[i].ProductID, "A", isProduct, sWasteType) + '';
                        }
                        else if (arr[i].cTotal == "Y" && arr[i].cTotalAll == "N") {
                            var sNameID = "txt" + sWasteType + "_G";
                            var sTargetID = sNameID + "_Target_" + arr[i].nGroupCalc + "_" + arr[i].ProductID;
                            tr += '<tr id="tr_' + arr[i].ProductID + '" style="background-color: #fabd4f;">'
                                   + '<td class="dt-body-left"><a id="a_' + arr[i].ProductID + '" class="btn btn-default" onclick="DetailSub(' + arr[i].ProductID + ');"><i id="i_' + arr[i].ProductID + '" class="fas fa-chevron-up"></i></a>' + arr[i].sSetHtml + arr[i].ProductName + setTooltipProduct(arr[i].sTooltip) + '</td>'
                                   + '<td class="dt-body-center"></td>'
                                   + '<td class="dt-body-center">' + arr[i].sUnit + '</td>'
                                   + '<td class="dt-body-center cTarget">'
                                   + '<input id="' + sTargetID + '" maxlength="20" onchange="Chang(this,\'G\',\'' + sWasteType + '\');" '
                            + 'class="form-control ctxtRight input-sm" value="' + (isProduct ? CheckTextInput(arrP[0].Target) : "") + '"></td>';
                            /// หัวกลุ่ม                           
                            tr += '' + CreateVal(arrQ, arrP[0], 0, sNameID, arr[i].nGroupCalc + "_" + arr[i].ProductID, "G", isProduct, sWasteType) + '';

                            var sMaskID = "txtRemarsk_" + arr[i].ProductID;
                            divMarsk += '<div class="col-xs-12 col-md-4" style="margin-top: 20px;">'
                            + '<div class="well">'
                            + '<div class="form-group">'
                            + '<label class="control-label">Remark ( ' + arr[i].ProductName + ')<span class="text-red">*</span></label>'
                            + '<textarea id="' + sMaskID + '" class="form-control" rows="4"></textarea>'
                            + ' </div></div></div>';
                        }
                        tr += '</tr>';
                    }
                    else {
                        var sNameID = "txt" + sWasteType + "_H_" + arr[i].nGroupCalc;
                        tr += '<tr id="tr_' + arr[i].ProductID + '" class="' + arr[i].nOption + '" style="background-color: rgb(255, 237, 196); position: relative; left: 0px;">'
                               + '<td class="dt-body-left"><a id="a_' + arr[i].ProductID + '" class="btn btn-default" onclick="DetailSub(' + arr[i].ProductID + ');"><i id="i_' + arr[i].ProductID + '" class="fas fa-chevron-down"></i></a>' + arr[i].sSetHtml + arr[i].ProductName + '&nbsp;</td>'
                               + '<td class="dt-body-center"></td>'
                               + '<td class="dt-body-center">' + arr[i].sUnit + '</td>'
                               + '<td class="dt-body-center cTarget"><input id="' + sNameID + '_Target_' + arr[i].ProductID + '" maxlength="20"  onchange="Chang(this,\'H\',\'' + sWasteType + '\');"   class="form-control ctxtRight input-sm" value="' + (isProduct ? CheckTextInput(arrP[0].Target) : "") + '"></td>'
                               + '' + CreateVal(arrQ, arrP[0], 0, sNameID, arr[i].ProductID, "H", isProduct, sWasteType) + ''
                            + '</tr>';
                        if ($hdfPRMS.val() != "1" && arrSumit.length < 12) {
                            tr += '<tr id="trAdd_' + arr[i].ProductID + '" class="' + arr[i].ProductID + ' ' + arr[i].nOption + '" style="background-color: #f5f5f5; display:none;">'
                                + '<td colspan="3"><button id="btnAdd_' + arr[i].ProductID + '" onclick="Add(\'' + sTableName + '\',\'#trAdd_' + arr[i].ProductID + '\',\'' + arr[i].ProductID + '\',\'' + arr[i].nGroupCalc + '\',\'' + sWasteType + '\',' + arr[i].nOption + ');" type="button" class="btn btn-sm btn-info">Add</button></td>'
                                + '<td colspan="' + nQ + '"></td>'
                                + '</tr>';
                        }
                    }
                }
                sTable += '<tbody>' + tr + '</tbody>';

                $(sTableName).append(sTable);
                var arrSubHead = Enumerable.From(arrSub).Where(function (w) { return w.sType == sWasteType && w.sStatus == "Y" }).ToArray();
                if (arrSubHead.length > 0) {
                    BindSub(arrSubHead, sTableName, sWasteType);
                }

                $(sDivMarsk).append(divMarsk);

                for (var i = 0; i < arrMask.length; i++) {
                    $("#txtRemarsk_" + arrMask[i].nProductID + "").val(arrMask[i].sText);
                }

                SetDisableMonth();
            }
        }
        function DetailSub(sid) {

            if ($("." + sid + "").is(":visible")) {
                $("." + sid + "").hide();
                $("#i_" + sid + "").addClass("fa-chevron-down");
                $("#i_" + sid + "").removeClass("fa-chevron-up");
            }
            else {
                $("." + sid + "").show();
                $("#i_" + sid + "").removeClass("fa-chevron-down");
                $("#i_" + sid + "").addClass("fa-chevron-up");
            }

        }
        function CreateQtr(arrQ, nMode, txt) {
            var tr = "", td1 = "", td2 = "", td3 = "", td4 = "", td5 = "", td6 = "", td7 = "", td8 = "", td9 = "", td10 = "", td11 = "", td12 = "", tdB = "", tdAf = "";
            $.each(arrQ, function (e, n) {
                var txtInput = "";

                if (n == 1) {
                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_1' style='width:120px;'><label>Q1 : Jan</label></th>";
                    }
                    td1 = txtInput;
                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_1' style='width:120px;'><label>Q1 : Feb</label></th>";
                    }
                    td2 = txtInput;
                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_1' style='width:120px;'><label>Q1 : Mar</label></th>";
                    }
                    td3 = txtInput;
                }
                else if (n == 2) {
                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_2' style='width:120px;'><label>Q2 : Apr</label></th>";
                    }
                    td4 = txtInput;

                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_2' style='width:120px;'><label>Q2 : May</label></th>";
                    }
                    td5 = txtInput;

                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_2' style='width:120px;'><label>Q2 : Jun</label></th>";
                    }
                    td6 = txtInput;
                }
                else if (n == 3) {

                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_3' style='width:120px;'><label>Q3 : Jul</label></th>";
                    }
                    td7 = txtInput;

                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_3' style='width:120px;'><label>Q3 : Aug</label></th>";
                    }
                    td8 = txtInput;

                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_3' style='width:120px;'><label>Q3 : Sep</label></th>";
                    }
                    td9 = txtInput;
                }
                else if (n == 4) {

                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_4' style='width:120px;'><label>Q4 : Oct</label></th>";
                    }
                    td10 = txtInput;

                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_4' style='width:120px;'><label>Q4 : Nov</label></th>";
                    }
                    td11 = txtInput;

                    if (nMode == 0) {
                        txtInput = "<th class='text-center  QHead_4' style='width:120px;'><label>Q4 : Dec</label></th>";
                    }
                    td12 = txtInput;
                }
            });
            tr = td1 + td2 + td3 + td4 + td5 + td6 + td7 + td8 + td9 + td10 + td11 + td12;
            return tr;
        }
        function CreateVal(arrQ, arrP, nMode, txt, ID, sType, isProduct, sWasteType) {
            var tr = "", td1 = "", td2 = "", td3 = "", td4 = "", td5 = "", td6 = "", td7 = "", td8 = "", td9 = "", td10 = "", td11 = "", td12 = "", tdB = "", tdAf = "";
            $.each(arrQ, function (e, n) {
                var txtInput = "";
                var sQ = "QHead_" + n;
                tdB = '<td class="text-center ' + sQ + '">';
                tdAf = "</td>";
                var dis = ""

                if (nMode == 0) {
                    dis = " disabled";
                }

                if (n == 1) {
                    var sValM1 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M1 + "") : CheckTextOutput(arrP.M1) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M1_' + ID + '" name="' + txt + '_M1' + '" value="' + sValM1 + '" ' + dis + '/>';

                    }
                    else if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M1_' + ID + '" name="' + txt + '_M1_' + ID + '" value="' + sValM1 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');" ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M1_' + ID + '" name="' + txt + '_M1_' + ID + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');" ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M1_' + ID + '" name="' + txt + '_M1_' + ID + '"  value="' + sValM1 + '" ' + dis + '/>';

                    }
                    td1 = tdB + txtInput + tdAf;

                    var sValM2 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M2 + "") : CheckTextOutput(arrP.M2) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M2_' + ID + '" name="' + txt + '_M2' + '" value="' + sValM2 + '" ' + dis + '/>';

                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M2_' + ID + '" name="' + txt + '_M2_' + ID + '" value="' + sValM2 + '"  onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');" ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M2_' + ID + '" name="' + txt + '_M2_' + ID + '"  onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');" ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M2_' + ID + '" name="' + txt + '_M2_' + ID + '" value="' + sValM2 + '" ' + dis + '/>';
                    }

                    td2 = tdB + txtInput + tdAf;

                    var sValM3 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M3 + "") : CheckTextOutput(arrP.M3) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M3_' + ID + '" name="' + txt + '_M3' + '" value="' + sValM3 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M3_' + ID + '" name="' + txt + '_M3_' + ID + '" value="' + sValM3 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M3_' + ID + '" name="' + txt + '_M3_' + ID + '"  onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M3_' + ID + '" name="' + txt + '_M3_' + ID + '" value="' + sValM3 + '" ' + dis + '/>';
                    }
                    td3 = tdB + txtInput + tdAf;
                }
                else if (n == 2) {

                    var sValM4 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M4 + "") : CheckTextOutput(arrP.M4) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M4_' + ID + '" name="' + txt + '_M4' + '" value="' + sValM4 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M4_' + ID + '" name="' + txt + '_M4_' + ID + '" value="' + sValM4 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M4_' + ID + '" name="' + txt + '_M4_' + ID + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M4_' + ID + '" name="' + txt + '_M4_' + ID + '" value="' + sValM4 + '" ' + dis + '/>';
                    }
                    td4 = tdB + txtInput + tdAf;

                    var sValM5 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M5 + "") : CheckTextOutput(arrP.M5) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M5_' + ID + '" name="' + txt + '_M5' + '" value="' + sValM5 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M5_' + ID + '" name="' + txt + '_M5_' + ID + '" value="' + sValM5 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M5_' + ID + '" name="' + txt + '_M5_' + ID + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M5_' + ID + '" name="' + txt + '_M5_' + ID + '" value="' + sValM5 + '" ' + dis + '/>';
                    }
                    td5 = tdB + txtInput + tdAf;

                    var sValM6 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M6 + "") : CheckTextOutput(arrP.M6) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M6_' + ID + '" name="' + txt + '_M6' + '" value="' + sValM6 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M6_' + ID + '" name="' + txt + '_M6_' + ID + '" value="' + sValM6 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M6_' + ID + '" name="' + txt + '_M6_' + ID + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M6_' + ID + '" name="' + txt + '_M6_' + ID + '" value="' + sValM6 + '" ' + dis + '/>';
                    }
                    td6 = tdB + txtInput + tdAf;
                }
                else if (n == 3) {
                    var sValM7 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M7 + "") : CheckTextOutput(arrP.M7) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M7_' + ID + '" name="' + txt + '_M7' + '" value="' + sValM7 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M7_' + ID + '" name="' + txt + '_M7_' + ID + '" value="' + sValM7 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M7_' + ID + '" name="' + txt + '_M7_' + ID + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " id="' + txt + '_M7_' + ID + '" name="' + txt + '_M7_' + ID + '" value="' + sValM7 + '" ' + dis + '/>';
                    }
                    td7 = tdB + txtInput + tdAf;

                    var sValM8 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M8 + "") : CheckTextOutput(arrP.M8) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M8_' + ID + '" name="' + txt + '_M8' + '" value="' + sValM8 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M8_' + ID + '" name="' + txt + '_M8_' + ID + '" value="' + sValM8 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"   ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M8_' + ID + '" name="' + txt + '_M8_' + ID + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"   ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M8_' + ID + '" name="' + txt + '_M8_' + ID + '" value="' + sValM8 + '" ' + dis + '/>';
                    }
                    td8 = tdB + txtInput + tdAf;

                    var sValM9 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M9 + "") : CheckTextOutput(arrP.M9) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M9_' + ID + '" name="' + txt + '_M9' + '" value="' + sValM9 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M9_' + ID + '" name="' + txt + '_M9_' + ID + '" value="' + sValM9 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M9_' + ID + '" name="' + txt + '_M9_' + ID + '"  onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M9_' + ID + '" name="' + txt + '_M9_' + ID + '"  value="' + sValM9 + '" ' + dis + '/>';
                    }
                    td9 = tdB + txtInput + tdAf;
                }
                else if (n == 4) {
                    var sValM10 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M10 + "") : CheckTextOutput(arrP.M10) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M10_' + ID + '" name="' + txt + '_M10" value="' + sValM10 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M10_' + ID + '" name="' + txt + '_M10_' + ID + '" value="' + sValM10 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M10_' + ID + '" name="' + txt + '_M10_' + ID + '"  onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M10_' + ID + '" name="' + txt + '_M10_' + ID + '" value="' + sValM10 + '" ' + dis + '/>';
                    }
                    td10 = tdB + txtInput + tdAf;

                    var sValM11 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M11 + "") : CheckTextOutput(arrP.M11) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M11_' + ID + '" name="' + txt + '_M11" value="' + sValM11 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M11_' + ID + '" name="' + txt + '_M11_' + ID + '" value="' + sValM11 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M11_' + ID + '" name="' + txt + '_M11_' + ID + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M11_' + ID + '" name="' + txt + '_M11_' + ID + '" value="' + sValM11 + '" ' + dis + '/>';
                    }
                    td11 = tdB + txtInput + tdAf;

                    var sValM12 = (isProduct ? sType == "S" ? CheckTextInput(arrP.M12 + "") : CheckTextOutput(arrP.M12) : "");
                    if (sType == "A") {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M12_' + ID + '" name="' + txt + '_M12" value="' + sValM12 + '" ' + dis + '/>';
                    }
                    if (sType == "S") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M12_' + ID + '" name="' + txt + '_M12_' + ID + '" value="' + sValM12 + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else if (sType == "SS") {
                        txtInput = '<input  class="form-control ctxtRight input-sm" maxlength="20" id="' + txt + '_M12_' + ID + '" name="' + txt + '_M12_' + ID + '" onchange="Chang(this,\'Cal\',\'' + sWasteType + '\');"  ' + dis + '/>';
                    }
                    else {
                        txtInput = '<input  class="form-control ctxtRight input-sm " maxlength="20" id="' + txt + '_M12_' + ID + '" name="' + txt + '_M12_' + ID + '" value="' + sValM12 + '" ' + dis + '/>';
                    }
                    td12 = tdB + txtInput + tdAf;
                }
            });
            tr = td1 + td2 + td3 + td4 + td5 + td6 + td7 + td8 + td9 + td10 + td11 + td12;
            return tr;
        }

        function SetDisableMonth() {
            //$('input[type=checkbox][name=rdoM]').filter('[value=1],[value=2],[value=3],[value=4],[value=5],[value=6],[value=7],[value=8],[value=9],[value=10],[value=11],[value=12]').iCheck("uncheck");
            //$('input[type=checkbox][name=rdoM]').filter('[value=1],[value=2],[value=3],[value=4],[value=5],[value=6],[value=7],[value=8],[value=9],[value=10],[value=11],[value=12]').prop('disabled', false);
            $.each(arrSumit, function (e, n) {
                if (n == "1") $("input[id*=_M1_]").prop("disabled", true);
                if (n == "2") $("input[id*=_M2]").prop("disabled", true);
                if (n == "3") $("input[id*=_M3]").prop("disabled", true);
                if (n == "4") $("input[id*=_M4]").prop("disabled", true);
                if (n == "5") $("input[id*=_M5]").prop("disabled", true);
                if (n == "6") $("input[id*=_M6]").prop("disabled", true);
                if (n == "7") $("input[id*=_M7]").prop("disabled", true);
                if (n == "8") $("input[id*=_M8]").prop("disabled", true);
                if (n == "9") $("input[id*=_M9]").prop("disabled", true);
                if (n == "10") $("input[id*=_M10]").prop("disabled", true);
                if (n == "11") $("input[id*=_M11]").prop("disabled", true);
                if (n == "12") $("input[id*=_M12]").prop("disabled", true);

                //$('input[type=checkbox][name=rdoM]').filter('[value=' + n + ']').iCheck("check");
                //$('input[type=checkbox][name=rdoM]:checked').prop('disabled', true);
            });

            if ($hidStatusWF.val() == "1") {
                $("select[id*=_Unit]").prop('disabled', true);
            }
            else {
                $("select[id*=_Unit]").prop('disabled', false);
            }

            if ($hidStatusWF.val() == "1") {
                $("select[id*=_Disposal]").prop('disabled', true);
            }
            else {
                $("select[id*=_Disposal]").prop('disabled', false);
            }

            if (arrSumit.length == 12 || $hdfPRMS.val() == 1) {
                $("textarea[id*=txtRemarsk_]").prop('disabled', true);
                $("input[id*=_Target_]").prop('disabled', true);
                $("input[id*=_Name_]").prop('disabled', true);
                $("input[id*=Old_]").prop('disabled', true);
                $("input[id*=New_]").prop('disabled', true);
                $("input[id*=_M]").prop("disabled", true); //เพิ่มวันที่ 26/04/2562
            }

            if (lstStatus.length > 0) {
                for (var i = 1; i <= 12; i++) {
                    var wf = Enumerable.From(lstStatus).Where(function (w)
                    { return w.nMonth == i }).FirstOrDefault();

                    if (wf != null) {
                        nStatusWF = wf.nStatusID;
                    }

                    //// No Save draf
                    if (nStatusWF > 0) {
                        $("input[id*=_M" + i + "_]").prop("disabled", true);
                    }

                    ////เข้ามาappove eidth eidt content
                    if ($hdfsStatus.val() != "" && nStatusWF == 2) {
                        //$("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', false);
                        $("input[id*=_M" + i + "_]").prop("disabled", false);
                    }

                    ///สั่งปิดที่ไม่ได้อีดิทคอนเทนมา
                    if (nStatusWF == 0 && $hdfsStatus.val() != "") {
                        //$("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', true);
                        $("input[id*=_M" + i + "_]").prop("disabled", true);
                    }
                }
                if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") {
                    $("textarea[id*=txtRemarsk_]").prop('disabled', false);
                    $("input[id*=_Target_]").prop('disabled', false);
                    $("input[id*=_Name_]").prop('disabled', false);
                    $("input[id*=Old_]").prop('disabled', false);
                    $("input[id*=New_]").prop('disabled', false);
                    $("input[id*=M_]").prop("disabled", false);

                    $("input[id*=_A_M]").prop("disabled", true);
                    $("input[id*=_G_M]").prop("disabled", true);
                    $("input[id*=_H_1_M]").prop("disabled", true);
                    $("input[id*=_H_2_M]").prop("disabled", true);
                    /// เปิด ข้อย่อย
                    $("input[id*=_S]").prop("disabled", false);
                    $("select[id*=_Unit]").prop("disabled", false);
                    $("select[id*=_Disposal]").prop("disabled", false);
                }

                if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() != "") {
                    //$("textarea[id*=txtRemarsk_]").prop('disabled', true);
                    //$("input[id*=_Target_]").prop('disabled', true);
                    //$("input[id*=_Name_]").prop('disabled', true);
                    //$("input[id*=Old_]").prop('disabled', true);
                    //$("input[id*=New_]").prop('disabled', true);
                    $("input[id*=M_]").prop("disabled", true);

                    var arr = Enumerable.From(lstStatus).Where(function (w)
                    { return w.nStatusID == 2 }).ToArray();
                    if (arr.length > 0) {
                        for (var x = 0; x < arr.length; x++) {
                            $("input[id*=_M" + arr[x].nMonth + "_]").prop("disabled", false);
                        }
                    }

                    $("input[id*=_A_M]").prop("disabled", true);
                    $("input[id*=_G_M]").prop("disabled", true);
                    $("input[id*=_H_1_M]").prop("disabled", true);
                    $("input[id*=_H_2_M]").prop("disabled", true);
                }
            }
            //else if ($hidStatusWF.val() == "4") {
            //    $("input[id*=_M1_]").prop("disabled", true);
            //    $("input[id*=_M2]").prop("disabled", true);
            //    $("input[id*=_M3]").prop("disabled", true);
            //    $("input[id*=_M4]").prop("disabled", true);
            //    $("input[id*=_M5]").prop("disabled", true);
            //    $("input[id*=_M6]").prop("disabled", true);
            //    $("input[id*=_M7]").prop("disabled", true);
            //    $("input[id*=_M8]").prop("disabled", true);
            //    $("input[id*=_M9]").prop("disabled", true);
            //    $("input[id*=_M10]").prop("disabled", true);
            //    $("input[id*=_M11]").prop("disabled", true);
            //    $("input[id*=_M12]").prop("disabled", true);
            //}
            CheckEventButton();
        }
        /////////************ Other municipal waste *************\\\\\\\\\\\\
        function BindTableMunicipal(arr, sWasteType) {
            //sWasteType = MUL
            if (arr.length > 0) {
                var sTableName = "#tbMul";
                var sDivMarsk = "#divRemarkMul";
                var sRiskName = ""; var sTable = ""; var tr = ""; var Icon = "";
                var nQ = (arrQ.length * 3) + 1;
                var divMarsk = "";

                $(sTableName).empty();
                $(sDivMarsk).empty();
                sTable += '<thead>'
               + '<tr>'
                + '<th class="text-center" style="vertical-align: middle; width:400px;"><label>Indicator</label></th>'
                + '<th class="text-center" style="vertical-align: middle; width:120px;"><label>Disposal Code</label></th>'
                + '<th class="text-center" style="vertical-align: middle; width:120px;"><label>Unit</label></th>'
                + '<th class="text-center" style="vertical-align: middle;width:120px;"><label>Target</label></th>';
                sTable += CreateQtr(arrQ, 0, "", "");
                sTable += '</tr>'
                + '</thead>';

                ////////////// *********** BODY  ************ \\\\\\\\\\\\\\\\\

                for (var i = 0; i < arr.length; i++) {
                    var arrP = arr[i].lstProduct;
                    var isProduct = arrP.length > 0 ? true : false;

                    var sNameID = "txt" + sWasteType + "_A";
                    tr += '<tr id="tr_' + arr[i].ProductID + '"  style="background-color: #dbea97;">'
                           + '<td class="dt-body-left">' + arr[i].ProductName + '&nbsp;</td>'
                           + '<td class="dt-body-center"></td>'
                           + '<td class="dt-body-center">' + arr[i].sUnit + '</td>'
                           + '<td class="dt-body-center cTarget"><input id="' + sNameID + '_Target_' + arr[i].ProductID + '" maxlength="20"  onchange="Chang(this,\'H\',\'' + sWasteType + '\');"   class="form-control ctxtRight input-sm" value="' + (isProduct ? CheckTextInput(arrP[0].Target) : "") + '"></td>'
                           + '' + CreateVal(arrQ, arrP[0], 0, sNameID, arr[i].ProductID, "A", isProduct, sWasteType) + ''
                        + '</tr>';

                    if ($hdfPRMS.val() != "1" && arrSumit.length < 12) {
                        tr += '<tr id="trAdd_' + arr[i].ProductID + '" class="' + arr[i].ProductID + '" style="background-color: #f5f5f5;">'
                            + '<td colspan="3"><button id="btnAdd_' + arr[i].ProductID + '" onclick="Add(\'' + sTableName + '\',\'#trAdd_' + arr[i].ProductID + '\',\'' + arr[i].ProductID + '\',\'0\',\'' + sWasteType + '\',\'0\');" type="button" class="btn btn-sm btn-info">Add</button></td>'
                            + '<td colspan="' + nQ + '"></td>'
                            + '</tr>';
                    }

                    var sMaskID = "txtRemarsk_" + arr[i].ProductID;
                    divMarsk += '<div class="col-xs-12 col-md-4" style="margin-top: 20px;">'
                    + '<div class="well">'
                    + '<div class="form-group">'
                    + '<label class="control-label">Remark ( ' + arr[i].ProductName + ')<span class="text-red">*</span></label>'
                    + '<textarea id="' + sMaskID + '" class="form-control" rows="4"></textarea>'
                    + ' </div></div></div>';
                }
                sTable += '<tbody>' + tr + '</tbody>';

                $(sTableName).append(sTable);

                var arrSubHead = Enumerable.From(arrSub).Where(function (w) { return w.sType == sWasteType && w.sStatus == "Y" }).ToArray();
                if (arrSubHead.length > 0) {
                    BindSub(arrSubHead, sTableName, sWasteType);
                }

                $(sDivMarsk).append(divMarsk);

                for (var i = 0; i < arrMask.length; i++) {
                    $("#txtRemarsk_" + arrMask[i].nProductID + "").val(arrMask[i].sText);
                }

                SetDisableMonth();
            }
        }

        //////////------------ SUB ------------- \\\\\\\\\\
        function Add(sTableID, sTrID, HeadID, nGroupCalc, sWasteType, OptionHead) {
            ///// nGroupCalc - 1=Routine  2=none Routine
            var tr = "";
            var nSubID = arrSub.length > 0 ? Enumerable.From(arrSub).Max(function (m) { return m.nSubID }) + 1 : 1;

            var obj = {
                nHeadID: +HeadID,
                nSubID: nSubID,
                nGroupCalc: +nGroupCalc,
                sName: "",
                sType: sWasteType,
                Target: "",
                sUnit: "0",
                FromID: 0,
                M1: "",
                M2: "",
                M3: "",
                M4: "",
                M5: "",
                M6: "",
                M7: "",
                M8: "",
                M9: "",
                M10: "",
                M11: "",
                M12: "",
                sStatus: "Y",
                sDisposal: "",
                nOptionHead: OptionHead,
            };
            arrSub.push(obj);
            var trID = "trSub_" + HeadID + "_" + nSubID;
            var sNameID = "txt" + sWasteType + "_S_" + nGroupCalc + "_" + HeadID;
            tr += '<tr id="' + trID + '" class="' + HeadID + ' ' + OptionHead + '">'
                 + '<td>'
                 + '<div class="input-group">'
                 + '<div class="input-group-addon cStyleDivTrash" onclick="DeletSub(\'' + trID + '\',' + nSubID + ',' + HeadID + ',' + nGroupCalc + ',\'' + sWasteType + '\');">'
                 + '<i class="fas fa-trash-alt" style="color: red;"></i></div>'
                 + '<input id="' + sNameID + '_' + 'Name_' + nSubID + '" maxlength="200"  class="form-control input-sm" onchange="Chang(this,\'NS\',\'' + sWasteType + '\');"></div></td>'
                  + '<td><select class="form-control input-sm" id="' + sNameID + '_' + 'Disposal_' + nSubID + '" onchange="Chang(this,\'DS\',\'' + sWasteType + '\');"></select></td>'
                 + '<td><select class="form-control input-sm" id="' + sNameID + '_' + 'Unit_' + nSubID + '" onchange="Chang(this,\'S\',\'' + sWasteType + '\');"></select></td>'
                 + '<td class="cTarget"><input id="' + sNameID + '_' + 'Target_' + nSubID + '" maxlength="20" onchange="Chang(this,\'TS\',\'' + sWasteType + '\');"  class="form-control ctxtRight input-sm"></td>';
            tr += CreateVal(arrQ, arrSub, 1, sNameID, nSubID, "SS", true, sWasteType);
            tr += '</tr>';
            var sID = sNameID + "_Unit_" + nSubID;
            var sDisposalID = sNameID + "_Disposal_" + nSubID;
            Gen_ddl_TM_DATA(sID, "0");

            Gen_ddl_Disposal_DATA(sDisposalID, "", "N", HeadID, nSubID);
            $(sTableID).find(sTrID).before(tr);

            CheckboxQuarterChanged();

            //// ปิดเพราะไม่อยากเช็คอีกรอบ ตอนกดแอด L2 มันไปเช็คแล้วปิดหมด 26/04/62
            //if (lstStatus.length > 0) {
            //    for (var i = 1; i <= 12; i++) {
            //        var wf = Enumerable.From(lstStatus).Where(function (w)
            //        { return w.nMonth == i }).FirstOrDefault();

            //        if (wf != null) {
            //            nStatusWF = wf.nStatusID;
            //        }

            //        //// No Save draf
            //        if (nStatusWF > 0) {
            //            $("input[id*=_M" + i + "_]").prop("disabled", true);
            //        }

            //        ////เข้ามาappove eidth eidt content
            //        if ($hdfsStatus.val() != "" && nStatusWF == 2) {
            //            //$("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', false);
            //            $("input[id*=_M" + i + "_]").prop("disabled", false);
            //        }

            //        ///สั่งปิดที่ไม่ได้อีดิทคอนเทนมา
            //        if (nStatusWF == 0 && $hdfsStatus.val() != "") {
            //            //$("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', true);
            //            $("input[id*=_M" + i + "_]").prop("disabled", true);
            //        }
            //    }
            //    if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") {
            //        $("textarea[id*=txtRemarsk_]").prop('disabled', false);
            //        $("input[id*=_Target_]").prop('disabled', false);
            //        $("input[id*=_Name_]").prop('disabled', false);
            //        $("input[id*=Old_]").prop('disabled', false);
            //        $("input[id*=New_]").prop('disabled', false);
            //        $("input[id*=M_]").prop("disabled", false);

            //        $("input[id*=_A_M]").prop("disabled", true);
            //        $("input[id*=_G_M]").prop("disabled", true);
            //        $("input[id*=_H_1_M]").prop("disabled", true);
            //        $("input[id*=_H_2_M]").prop("disabled", true);

            //        ///// เปิด ข้อย่อย เพิ่มวันที่ 26/04/2562
            //        //$("input[id*=_S]").prop("disabled", false);
            //        //$("select[id*=_Unit]").prop("disabled", false);
            //        //$("select[id*=_Disposal]").prop("disabled", false);
            //    }

            //    if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() != "") {
            //        //$("textarea[id*=txtRemarsk_]").prop('disabled', true);
            //        //$("input[id*=_Target_]").prop('disabled', true);
            //        //$("input[id*=_Name_]").prop('disabled', true);
            //        //$("input[id*=Old_]").prop('disabled', true);
            //        //$("input[id*=New_]").prop('disabled', true);
            //        $("input[id*=M_]").prop("disabled", true);

            //        var arr = Enumerable.From(lstStatus).Where(function (w)
            //        { return w.nStatusID == 2 }).ToArray();
            //        if (arr.length > 0) {
            //            for (var x = 0; x < arr.length; x++) {
            //                $("input[id*=_M" + arr[x].nMonth + "_]").prop("disabled", false);
            //            }
            //        }

            //        $("input[id*=_A_M]").prop("disabled", true);
            //        $("input[id*=_G_M]").prop("disabled", true);
            //        $("input[id*=_H_1_M]").prop("disabled", true);
            //        $("input[id*=_H_2_M]").prop("disabled", true);
            //    }
            //}

            //// 04/03/62  ถ้าเลือก na ปิดหมด
            //if ($hdfRole.val() != "4") {
            for (var m1 = 1; m1 <= 12; m1++) {
                $("input[id$=" + sNameID + "_" + "M" + m1 + "_" + nSubID + "]").prop('disabled', true);
            }
            //}
            SetDisableMonth();
        }
        function BindSub(arrSub, sTableID, sWasteType) {
            arrSub = Enumerable.From(arrSub).Where(function (w) { return w.sStatus == "Y" }).ToArray();
            var isSubmit = $hidStatusWF.val() != "" ? $hidStatusWF.val() == "1" ? true : false : false;
            var isTrash = true;
            var IsFullMonth = arrSumit.length == 12 ? true : false;

            if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                isTrash = true;
            }
            else {
                if ($hidStatusWF.val() != "0" || $hdfPRMS.val() == "1") {
                    isTrash = false;
                }
                else {
                    if (!IsFullMonth) { /// น้อยกว่า 12 ด. ต้องมีถังขยะ
                        isTrash = true;
                    }
                    else { /// เท่ากับ 12 ด.
                        isTrash = false;
                    }
                }
            }

            $.each(arrSub, function (e, n) {
                var tr = "";
                var sTrID = "";
                if ($hdfPRMS.val() != "1" && arrSumit.length < 12) {
                    sTrID = "#trAdd_" + n.nHeadID;
                }
                else {
                    sTrID = "#tr_" + n.nHeadID;
                }

                var as = n.nSubID;
                var sNameID = "txt" + n.sType + "_S_" + n.nGroupCalc + "_" + n.nHeadID;
                var trID = "trSub_" + n.nHeadID + "_" + n.nSubID;
                tr += '<tr id="' + trID + '" class="' + n.nHeadID + ' ' + n.nOptionHead + '" ' + (sWasteType == "MUL" ? '' : 'style="display: none;"') + '>'
                     + '<td>';

                if (!isTrash) { //isSubmit
                    tr += '<input id="' + sNameID + '_' + 'Name_' + n.nSubID + '"  class="form-control input-sm" onchange="Chang(this,\'NS\',\'' + n.sType + '\');"></div></td>';
                }
                else {
                    tr += '<div class="input-group">'
                       + '<div class="input-group-addon cStyleDivTrash" onclick="DeletSub(\'' + trID + '\',' + n.nSubID + ',' + n.nHeadID + ',' + n.nGroupCalc + ',\'' + sWasteType + '\');">'
                       + '<i class="fas fa-trash-alt" style="color: red;"></i></div>'
                       + '<input id="' + sNameID + '_' + 'Name_' + n.nSubID + '" maxlength="200"  class="form-control input-sm" onchange="Chang(this,\'NS\',\'' + n.sType + '\');"></div></td>';
                }

                tr += '<td class="dt-body-center"><select class="form-control input-sm" id="' + sNameID + '_' + 'Disposal_' + n.nSubID + '" onchange="Chang(this,\'DS\',\'' + n.sType + '\');"></select></td>'
                tr += '<td><select class="form-control input-sm" id="' + sNameID + '_' + 'Unit_' + n.nSubID + '" onchange="Chang(this,\'S\',\'' + n.sType + '\');"></select></td>'
                     + '<td class="cTarget"><input id="' + sNameID + '_' + 'Target_' + n.nSubID + '" maxlength="20" onchange="Chang(this,\'TS\',\'' + n.sType + '\');"  class="form-control ctxtRight input-sm"></td>';
                tr += CreateVal(arrQ, n, 1, sNameID, n.nSubID, "S", true, n.sType);
                tr += '</tr>';
                var sID = sNameID + "_Unit_" + n.nSubID;
                Gen_ddl_TM_DATA(sID, n.sUnit);

                var sDisposalID = sNameID + "_Disposal_" + n.nSubID;
                Gen_ddl_Disposal_DATA(sDisposalID, n.sDisposal, "O", n.nHeadID, n.nSubID);

                if ($hdfPRMS.val() != "1" && arrSumit.length < 12) {
                    $(sTableID).find(sTrID).before(tr);
                }
                else {
                    $(sTableID).find(sTrID).after(tr);
                }

                $("input[id$=" + sNameID + "_" + "Name_" + n.nSubID + "]").val(n.sName);
                $("input[id$=" + sNameID + "_" + "Target_" + n.nSubID + "]").val(CheckTextInput(n.Target));
                //// 04/03/62  ถ้าเลือก na ปิดหมด
                if (n.sUnit == "0") { //&& $hdfRole.val() != "4"
                    for (var m1 = 1; m1 <= 12; m1++) {
                        $("input[id$=" + sNameID + "_" + "M" + m1 + "_" + n.nSubID + "]").prop('disabled', true);
                    }
                }
                else {
                    $.each(lstStatus, function (e1, n1) {
                        for (var m = 1; m <= 12; m++) {
                            if (n1.nMonth == m) {
                                if (n1.nStatusID == 0) {
                                    $("input[id$=" + sNameID + "_" + "M" + m + "]").prop('disabled', false);
                                }
                            }
                        }
                    });
                }

            });

            if (sWasteType != "MUL") {
                //// Open Sub
                var nID = 0;
                $.each(arrSub, function (e, n) {
                    if (nID != n.nHeadID) {
                        $("#a_" + n.nHeadID + "").click();
                        nID = n.nHeadID;
                    }
                });
            }
        }
        function Gen_ddl_TM_DATA(sName, sValue) {
            $('select[id$=' + sName + ']>option').not(':first').remove();
            AjaxCallWebMethod("GetTM_Data", function (data) {
                var data = data.d;
                for (var i = 0; i < data.length; i++) {
                    $('select[id$=' + sName + ']').append("<option value=" + data[i].Value + ">" + data[i].sText + "</option>");
                }
            }, function () { $('select[id$=' + sName + ']').val(sValue).trigger("chosen:updated"); }, {});
        }
        function Gen_ddl_Disposal_DATA(sName, sValue, sMode, ProductID, nSubID) {
            // sMode - N = NEw , O = Old
            $('select[id$=' + sName + ']>option').not(':first').remove();
            AjaxCallWebMethod("GetDisposal_Data", function (data) {
                var data = data.d;
                $('select[id$=' + sName + ']').append("<option value=''>- select Disposal-</option>");
                for (var i = 0; i < data.length; i++) {
                    $('select[id$=' + sName + ']').append("<option value=" + data[i].Value + ">" + data[i].sText + "</option>");
                }
            }, function () { $('select[id$=' + sName + ']').val(sValue).trigger("chosen:updated"); }
            , { sMode: sMode, ProductID: ProductID, nSubID: nSubID, sFromID: $("input[id$=hidFromID]").val() });
        }
        function DeletSub(trID, nID, nHeadID, nGroupCalc, sWasteType) {
            DialogConfirmCloseButton(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {

                Enumerable.From(arrSub).Where(function (w) { return w.nSubID == nID && w.nHeadID == nHeadID }).ForEach(function (f) {
                    f.sStatus = "N";
                    return f;
                });

                for (var i = 1; i <= 12; i++) {
                    var sID = "input#txt" + sWasteType + "_S_" + nGroupCalc + "_" + nHeadID + "_M" + i + "_" + nID;
                    $("" + sID + "").val(0);
                    Chang(sID, "Cal", sWasteType);
                }

                $("#" + trID + "").remove();
                //var arr = sWasteType == "HZD" ? arrHZD : arrNHZD;
                //BindTable(arr, sWasteType);
                UnblockUI();
            });
        }

        function Chang(ctl, Type, sWasteType) {
            var sVal = $(ctl).val();
            var sID = $(ctl).attr("id");
            if (Type == "Cal") {
                var sINput = CheckTextInput(sVal);
                if (sINput == "N/A") {
                    if (sWasteType == "MUL") {
                        Cal($(ctl), "MS", sWasteType, sINput);
                    }
                    else {
                        Cal($(ctl), "S", sWasteType, sINput);
                    }

                }
                else {
                    if (sWasteType == "MUL") {
                        Cal($(ctl), "MS", sWasteType, "");
                    }
                    else {
                        Cal($(ctl), "S", sWasteType, "");
                    }
                }
            }
            else if (Type == "TS") {
                /// TargetSUB
                var sIN = CheckTextInput(sVal);
                sVal = sIN != "" ? sIN.replace(/,/g, "") : "";
                AddToArray('', sID, sVal, "S", sWasteType);
                $("input[id$=" + sID + "]").val(sIN);
            }
            else if (Type == "T") {
                AddToArray('', sID, sVal, "T", sWasteType);
            }
            else if (Type == "A") {
                var sIN = CheckTextInput(sVal);
                sVal = sIN != "" ? sIN.replace(/,/g, "") : "";
                AddToArray('', sID, sVal, "A", sWasteType);
                $("input[id$=" + sID + "]").val(sIN);
            }
            else if (Type == "H") {
                var sIN = CheckTextInput(sVal);
                sVal = sIN != "" ? sIN.replace(/,/g, "") : "";
                AddToArray('', sID, sVal, "H", sWasteType);
                $("input[id$=" + sID + "]").val(sIN);
            }
            else if (Type == "G") {
                var sIN = CheckTextInput(sVal);
                sVal = sIN != "" ? sIN.replace(/,/g, "") : "";
                AddToArray('', sID, sVal, "G", sWasteType);
                $("input[id$=" + sID + "]").val(sIN);
            }
            else if (Type == "S") {
                CheckUnitChang(sID, sVal, sWasteType);
            }
            else if (Type == "NS") {
                AddToArray('', sID, sVal, "S", sWasteType);
            }
            else if (Type == "DS") {
                AddToArray('', sID, sVal, "S", sWasteType);
            }
        }

        function Cal(sCtr, cType, sWasteType, sNA) {
            var arr = sWasteType == "HZD" ? arrHZD : sWasteType == "NHZD" ? arrNHZD : arrMul;
            var nVal = sCtr.val();
            var sAddID = sCtr.attr("id");
            var sID1 = [];
            sID1 = sAddID.split("_");
            var sGroupCalc = ""; var sHeadID = ""; var sMonth = "";
            var nCountVal = "";
            var sValOut = "";
            if (cType == "S") {
                /// SUB
                sGroupCalc = sID1[2];
                sHeadID = sID1[3];
                sMonth = sID1[4];
                var sID = "txt" + sWasteType + "_H_" + sGroupCalc + "_" + sMonth + "_" + sHeadID;
                var sSubGroupID = sID1[0] + "_" + sID1[1] + "_" + sGroupCalc + "_" + sHeadID + "_" + sMonth;
                var sUnitID = sID1[0] + "_" + sID1[1] + "_" + sID1[2] + "_" + sID1[3] + "_Unit_" + sID1[5];
                var sValUnit = $("select[id$=" + sUnitID + "]").val();

                if (sNA) {
                    sValOut = sNA;
                    nVal = sNA;
                }
                else {
                    var sINput = CheckTextInput(nVal + "");
                    if (sINput == "") {
                        nVal = "";
                        sValOut = "";
                    }
                    else {
                        sValOut = CheckTextInput(nVal);
                    }
                }

                var sVal = sValOut != "" ? (sValOut).replace(/,/g, '') : "";
                AddToArray('', sAddID, sVal, "S", sWasteType);
                $("input[id$=" + sAddID + "]").val(sValOut);

                var arrS = Enumerable.From(arrSub).Where(function (w) { return w.sType == sWasteType && w.nHeadID == +sHeadID && w.sStatus == "Y" && w.sUnit != "0" }).ToArray();
                for (var i = 0; i < arrS.length; i++) {
                    var nValMonth = "";
                    if (sMonth == "M1") nValMonth = arrS[i].M1 == "N/A" ? "" : arrS[i].M1;
                    else if (sMonth == "M2") nValMonth = arrS[i].M2 == "N/A" ? "" : arrS[i].M2;
                    else if (sMonth == "M3") nValMonth = arrS[i].M3 == "N/A" ? "" : arrS[i].M3;
                    else if (sMonth == "M4") nValMonth = arrS[i].M4 == "N/A" ? "" : arrS[i].M4;
                    else if (sMonth == "M5") nValMonth = arrS[i].M5 == "N/A" ? "" : arrS[i].M5;
                    else if (sMonth == "M6") nValMonth = arrS[i].M6 == "N/A" ? "" : arrS[i].M6;
                    else if (sMonth == "M7") nValMonth = arrS[i].M7 == "N/A" ? "" : arrS[i].M7;
                    else if (sMonth == "M8") nValMonth = arrS[i].M8 == "N/A" ? "" : arrS[i].M8;
                    else if (sMonth == "M9") nValMonth = arrS[i].M9 == "N/A" ? "" : arrS[i].M9;
                    else if (sMonth == "M10") nValMonth = arrS[i].M10 == "N/A" ? "" : arrS[i].M10;
                    else if (sMonth == "M11") nValMonth = arrS[i].M11 == "N/A" ? "" : arrS[i].M11;
                    else if (sMonth == "M12") nValMonth = arrS[i].M12 == "N/A" ? "" : arrS[i].M12;
                    if (arrS[i].sUnit == "3") { // KG
                        nValMonth = nValMonth != "" ? nValMonth != "0" ? (+nValMonth / 1000) : "0" : "";
                    }
                    else if (arrS[i].sUnit == "0") {
                        nValMonth = "";
                    }
                    nCountVal = (nCountVal != "" ? +nCountVal : "") + (nValMonth != "" ? +nValMonth : "");
                }
                //// 04/03/62  ถ้าเลือก na ปิดหมด
                if (sValUnit == "0") { //&& $hdfRole.val() != "4"
                    $("input[id$=" + sAddID + "]").prop('disabled', true);
                }
                else {
                    if (lstStatus.length > 0) {
                        $.each(lstStatus, function (e, n) {

                            var mon = 0;
                            switch (sMonth) {
                                case "M1": mon = 1;
                                    break;
                                case "M2": mon = 2;
                                    break;
                                case "M3": mon = 3;
                                    break;
                                case "M4": mon = 4;
                                    break;
                                case "M5": mon = 5;
                                    break;
                                case "M6": mon = 6;
                                    break;
                                case "M7": mon = 7;
                                    break;
                                case "M8": mon = 8;
                                    break;
                                case "M9": mon = 9;
                                    break;
                                case "M10": mon = 10;
                                    break;
                                case "M11": mon = 11;
                                    break;
                                case "M12": mon = 12;
                                    break;
                            }

                            if (n.nMonth == mon) {
                                if (n.nStatusID == 0) {
                                    $("input[id$=" + sAddID + "]").prop('disabled', false);
                                }
                            }
                        });
                    }
                    else {
                        $("input[id$=" + sAddID + "]").prop('disabled', false);
                    }
                }

                var sValOutSub = "";
                sValOutSub = CheckTextOutput(nCountVal) + "";

                var sVal = sValOutSub != "" ? (sValOutSub).replace(/,/g, '') : "";
                AddToArray(sHeadID, sID, sVal, "H", sWasteType);
                $("input[id$=" + sID + "]").val(sValOutSub);

                Cal($("input[id$=" + sID + "]"), "H", sWasteType, "");
            }
            else if (cType == "H") {
                sGroupCalc = sID1[2];
                sMonth = sID1[3];
                var sID = "txt" + sWasteType + "_G_" + sMonth + "_" + sGroupCalc;
                var sSubGroupID = sID1[0] + "_" + sID1[1] + "_" + sGroupCalc + "_" + sMonth;

                var arrSum = Enumerable.From(arr).Where(function (w) { return w.nGroupCalc == +sGroupCalc && w.cTotal != "Y" }).ToArray();
                for (var i = 0; i < arrSum.length; i++) {
                    var arrS = arrSum[i].lstProduct;
                    for (var x = 0; x < arrS.length; x++) {
                        var nValMonth = "";
                        if (sMonth == "M1") nValMonth = arrS[x].M1 == "N/A" ? "" : arrS[x].M1;
                        else if (sMonth == "M2") nValMonth = arrS[x].M2 == "N/A" ? "" : arrS[x].M2;
                        else if (sMonth == "M3") nValMonth = arrS[x].M3 == "N/A" ? "" : arrS[x].M3;
                        else if (sMonth == "M4") nValMonth = arrS[x].M4 == "N/A" ? "" : arrS[x].M4;
                        else if (sMonth == "M5") nValMonth = arrS[x].M5 == "N/A" ? "" : arrS[x].M5;
                        else if (sMonth == "M6") nValMonth = arrS[x].M6 == "N/A" ? "" : arrS[x].M6;
                        else if (sMonth == "M7") nValMonth = arrS[x].M7 == "N/A" ? "" : arrS[x].M7;
                        else if (sMonth == "M8") nValMonth = arrS[x].M8 == "N/A" ? "" : arrS[x].M8;
                        else if (sMonth == "M9") nValMonth = arrS[x].M9 == "N/A" ? "" : arrS[x].M9;
                        else if (sMonth == "M10") nValMonth = arrS[x].M10 == "N/A" ? "" : arrS[x].M10;
                        else if (sMonth == "M11") nValMonth = arrS[x].M11 == "N/A" ? "" : arrS[x].M11;
                        else if (sMonth == "M12") nValMonth = arrS[x].M12 == "N/A" ? "" : arrS[x].M12;

                        nCountVal = (nCountVal != "" ? +nCountVal : "") + (nValMonth != "" ? +nValMonth : "");
                    }
                }

                var ctr = $("input[id*=" + sID + "]");
                var sID = ctr.attr("id");
                var sValOut = "";
                sValOut = CheckTextOutput(nCountVal);

                var sVal = sValOut != "" ? (sValOut).replace(/,/g, '') : "";
                AddToArray("", sID, sVal, "G", sWasteType);
                $("input[id*=" + sID + "]").val(sValOut);

                Cal($("input[id*=" + sID + "]"), "G", sWasteType, "");
            }
            else if (cType == "G") {
                sGroupCalc = sID1[3];
                sMonth = sID1[2];
                var sNameID = "txt" + sWasteType + "_A_" + sMonth + "_";
                var sSubGroupID = sID1[0] + "_" + sID1[1] + "_" + sID1[2];

                var arrSum = Enumerable.From(arr).Where(function (w) { return w.cTotal == "Y" && w.cTotalAll == "N" }).ToArray();
                for (var i = 0; i < arrSum.length; i++) {
                    var arrS = arrSum[i].lstProduct;
                    for (var x = 0; x < arrS.length; x++) {
                        var nValMonth = "";
                        if (sMonth == "M1") nValMonth = arrS[x].M1 == "N/A" ? "" : arrS[x].M1;
                        else if (sMonth == "M2") nValMonth = arrS[x].M2 == "N/A" ? "" : arrS[x].M2;
                        else if (sMonth == "M3") nValMonth = arrS[x].M3 == "N/A" ? "" : arrS[x].M3;
                        else if (sMonth == "M4") nValMonth = arrS[x].M4 == "N/A" ? "" : arrS[x].M4;
                        else if (sMonth == "M5") nValMonth = arrS[x].M5 == "N/A" ? "" : arrS[x].M5;
                        else if (sMonth == "M6") nValMonth = arrS[x].M6 == "N/A" ? "" : arrS[x].M6;
                        else if (sMonth == "M7") nValMonth = arrS[x].M7 == "N/A" ? "" : arrS[x].M7;
                        else if (sMonth == "M8") nValMonth = arrS[x].M8 == "N/A" ? "" : arrS[x].M8;
                        else if (sMonth == "M9") nValMonth = arrS[x].M9 == "N/A" ? "" : arrS[x].M9;
                        else if (sMonth == "M10") nValMonth = arrS[x].M10 == "N/A" ? "" : arrS[x].M10;
                        else if (sMonth == "M11") nValMonth = arrS[x].M11 == "N/A" ? "" : arrS[x].M11;
                        else if (sMonth == "M12") nValMonth = arrS[x].M12 == "N/A" ? "" : arrS[x].M12;
                        nCountVal = (nCountVal != "" ? +nCountVal : "") + (nValMonth != "" ? +nValMonth : "");
                    }
                }

                var ctr = $("input[id*=" + sNameID + "]");
                var sID = ctr.attr("id");
                var sValOut = "";

                sValOut = CheckTextOutput(nCountVal);

                var sVal = sValOut != "" ? (sValOut).replace(/,/g, '') : "";
                AddToArray("", sID, sVal, "A", sWasteType);
                $("input[id*=" + sID + "]").val(sValOut);

                /////// Total รวมทั้งหมด
                var sTotalID = sID1[0] + "_Total";
                var ctr = $("input[id*=" + sTotalID + "]");
                var sID = ctr.attr("id");
                CalTotal(sID, sWasteType);
            }
            else if (cType == "O") {
                var sINput = CheckTextInput(nVal + "") + "";
                if (sINput != "") {
                    if (sINput == "N/A") {
                        var sVal = sINput != "" ? (sINput).replace(/,/g, '') : "";
                        AddToArray("", sAddID, sVal, "O", sWasteType);
                    }
                    else {
                        var sVal = sINput != "" ? (sINput).replace(/,/g, '') : "";
                        AddToArray("", sAddID, sVal, "O", sWasteType);
                    }
                }
                else {
                    AddToArray("", sAddID, "", "O", sWasteType);
                }
                $("input[id$=" + sAddID + "]").val(sINput);


                //// คำนวณ TOTAL
                var sTotalID = sID1[0] + "_Total";
                var ctr = $("input[id*=" + sTotalID + "]");
                var sID = ctr.attr("id");
                CalTotal(sID, sWasteType);
            }
            else if (cType == "N") {
                //var sINput = CheckTextInput(nVal) + "";
                //AddToArray("", sAddID, nVal, "N", sWasteType);
                //$("input[id$=" + sAddID + "]").val(sINput);
                var sINput = CheckTextInput(nVal + "") + "";
                if (sINput != "") {
                    if (sINput == "N/A") {
                        var sVal = sINput != "" ? (sINput).replace(/,/g, '') : "";
                        AddToArray("", sAddID, sVal, "N", sWasteType);
                    }
                    else {
                        var sVal = sINput != "" ? (sINput).replace(/,/g, '') : "";
                        AddToArray("", sAddID, sVal, "N", sWasteType);
                    }
                }
                else {
                    AddToArray("", sAddID, "", "N", sWasteType);
                }
                $("input[id$=" + sAddID + "]").val(sINput);

                //// คำนวณ TOTAL
                var sTotalID = sID1[0] + "_Total";
                var ctr = $("input[id*=" + sTotalID + "]");
                var sID = ctr.attr("id");
                CalTotal(sID, sWasteType);
            }
            else if (cType == "MS") {
                /// SUB
                sGroupCalc = sID1[2];
                sHeadID = sID1[3];
                sMonth = sID1[4];
                var sID = "txt" + sWasteType + "_A_" + sMonth + "_" + sHeadID;
                var sUnitID = sID1[0] + "_" + sID1[1] + "_" + sID1[2] + "_" + sID1[3] + "_Unit_" + sID1[5];
                var sValUnit = $("select[id$=" + sUnitID + "]").val();

                if (nVal == -1) {
                    nVal = "";
                    sValOut = "";
                }
                else {
                    if (sNA) {
                        sValOut = sNA;
                        nVal = sNA;
                    }
                    else {
                        sValOut = CheckTextInput(nVal + "");
                    }
                }

                var sVal = sValOut != "" ? (sValOut).replace(/,/g, '') : "";
                AddToArray('', sAddID, sVal, "S", sWasteType);
                $("input[id$=" + sAddID + "]").val(sValOut);

                var arrS = Enumerable.From(arrSub).Where(function (w) { return w.sType == sWasteType && w.nHeadID == +sHeadID && w.sStatus == "Y" && w.sUnit != "0" }).ToArray();
                for (var i = 0; i < arrS.length; i++) {
                    var nValMonth = "";
                    if (sMonth == "M1") nValMonth = arrS[i].M1 == "N/A" ? "" : arrS[i].M1;
                    else if (sMonth == "M2") nValMonth = arrS[i].M2 == "N/A" ? "" : arrS[i].M2;
                    else if (sMonth == "M3") nValMonth = arrS[i].M3 == "N/A" ? "" : arrS[i].M3;
                    else if (sMonth == "M4") nValMonth = arrS[i].M4 == "N/A" ? "" : arrS[i].M4;
                    else if (sMonth == "M5") nValMonth = arrS[i].M5 == "N/A" ? "" : arrS[i].M5;
                    else if (sMonth == "M6") nValMonth = arrS[i].M6 == "N/A" ? "" : arrS[i].M6;
                    else if (sMonth == "M7") nValMonth = arrS[i].M7 == "N/A" ? "" : arrS[i].M7;
                    else if (sMonth == "M8") nValMonth = arrS[i].M8 == "N/A" ? "" : arrS[i].M8;
                    else if (sMonth == "M9") nValMonth = arrS[i].M9 == "N/A" ? "" : arrS[i].M9;
                    else if (sMonth == "M10") nValMonth = arrS[i].M10 == "N/A" ? "" : arrS[i].M10;
                    else if (sMonth == "M11") nValMonth = arrS[i].M11 == "N/A" ? "" : arrS[i].M11;
                    else if (sMonth == "M12") nValMonth = arrS[i].M12 == "N/A" ? "" : arrS[i].M12;
                    if (arrS[i].sUnit == "3") { // KG
                        nValMonth = nValMonth != "" ? nValMonth != "0" ? (+nValMonth / 1000) : "0" : "";
                    }
                    else if (arrS[i].sUnit == "0") {
                        nValMonth = "";
                    }
                    nCountVal = (nCountVal != "" ? +nCountVal : "") + (nValMonth != "" ? +nValMonth : "");
                }

                var sValOutSub = "";
                sValOutSub = CheckTextOutput(nCountVal) + "";

                var sVal = sValOutSub != "" ? (sValOutSub).replace(/,/g, '') : "";
                AddToArray(sHeadID, sID, sVal, "A", sWasteType);
                $("input[id$=" + sID + "]").val(sValOutSub);
            }
        }
        function CheckUnitChang(sId, sVal, sWasteType) {
            var sID = sId.split("_");
            var id = sID[5];
            var nGroupCalc = sID[2];
            var nHeadID = sID[3];
            var arr = Enumerable.From(arrSub).Where(function (w) { return w.nSubID == id }).ToArray();
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].sUnit == sVal) {
                    var sVal1 = sVal != "" ? (sVal).replace(/,/g, '') : "";
                    AddToArray('', sID, sVal1, "S", sWasteType);
                    //var sIN = CheckTextInput(sVal);
                    //$("input[id$=" + sId + "]").val(sIN);
                }
                else {
                    var sVal1 = sVal != "" ? (sVal).replace(/,/g, '') : "";
                    AddToArray('', sId, sVal1, "S", sWasteType);
                    //var sIN = CheckTextInput(sVal);
                    //$("input[id$=" + sId + "]").val(sIN);
                    for (var i = 1; i < 13; i++) {
                        var sMonthID = "input#txt" + sWasteType + "_S_" + nGroupCalc + "_" + nHeadID + "_M" + i + "_" + id;
                        if (sVal1 == "0") {
                            $(sMonthID).prop('disabled', true);
                        }
                        else {
                            if (lstStatus.length > 0) {
                                $.each(lstStatus, function (e, n) {
                                    if (n.nMonth == i) {
                                        if (n.nStatusID == 0) {
                                            $(sMonthID).prop('disabled', false);
                                        }
                                    }
                                });
                                if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") { $(sMonthID).prop('disabled', false); }
                                if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() != "") {
                                    var arr = Enumerable.From(lstStatus).Where(function (w)
                                    { return w.nStatusID == 2 }).ToArray();
                                    if (arr.length > 0) {
                                        for (var x = 0; x < arr.length; x++) {
                                            $("input[id*=_M" + arr[x].nMonth + "_]").prop("disabled", false);
                                        }
                                    }
                                }
                            }
                            else {
                                $(sMonthID).prop('disabled', false);
                            }
                        }

                        Chang(sMonthID, "Cal", sWasteType);
                    }
                    //}
                }
            }
        }

        function CalTotal(sTotalID, sWasteType) {
            var arr = sWasteType == "HZD" ? arrHZD : arrNHZD;
            var sID = sTotalID.split("_");
            /////// Total รวมทั้งหมด
            var sAllID = sID[0] + "_A_M";
            var nTotalAll = 0;
            //$("input[id*=" + sAllID + "]").each(function (i, item) {
            //    var nVal = $(this).val() != "" ? +$(this).val() : 0;
            //    nTotalAll = nTotalAll + nVal;
            //});
            var nM1 = 0; var nM2 = 0; var nM3 = 0; var nM4 = 0; var nM5 = 0; var nM6 = 0; var nM7 = 0; var nM8 = 0; var nM9 = 0; var nM10 = 0; var nM11 = 0; var nM12 = 0;
            var arrSum = Enumerable.From(arr).Where(function (w) { return w.cTotal == "Y" && w.cTotalAll == "Y" && w.nGroupCalc == 0 }).ToArray();
            for (var i = 0; i < arrSum.length; i++) {
                var arrS = arrSum[i].lstProduct;
                for (var x = 0; x < arrS.length; x++) {
                    nM1 = arrS[x].M1 != "" ? +arrS[x].M1 : 0;
                    nM2 = arrS[x].M2 != "" ? +arrS[x].M2 : 0;
                    nM3 = arrS[x].M3 != "" ? +arrS[x].M3 : 0;
                    nM4 = arrS[x].M4 != "" ? +arrS[x].M4 : 0;
                    nM5 = arrS[x].M5 != "" ? +arrS[x].M5 : 0;
                    nM6 = arrS[x].M6 != "" ? +arrS[x].M6 : 0;
                    nM7 = arrS[x].M7 != "" ? +arrS[x].M7 : 0;
                    nM8 = arrS[x].M8 != "" ? +arrS[x].M8 : 0;
                    nM9 = arrS[x].M9 != "" ? +arrS[x].M9 : 0;
                    nM10 = arrS[x].M10 != "" ? +arrS[x].M10 : 0;
                    nM11 = arrS[x].M11 != "" ? +arrS[x].M11 : 0;
                    nM12 = arrS[x].M12 != "" ? +arrS[x].M12 : 0;
                    nTotalAll = nM1 + nM2 + nM3 + nM4 + nM5 + nM6 + nM7 + nM8 + nM9 + nM10 + nM11 + nM12;
                }

            }

            var nNew = 0; var nOld = 0;
            var arrSumYear = Enumerable.From(arr).Where(function (w) { return w.nGroupCalc == 99 }).ToArray();
            if (arrSumYear.length > 0) {
                for (var q = 0; q < arrSumYear.length; q++) {
                    var arrS = arrSumYear[q].lstProduct;
                    for (var y = 0; y < arrS.length; y++) {
                        nNew = arrS[y].ReportingYear != "" ? +arrS[y].ReportingYear : 0;
                        nOld = arrS[y].PreviousYear != "" ? +arrS[y].PreviousYear : 0;
                    }
                }
            }
            //var sNew = $("input[id*=" + sID[0] + "_New_" + "]").val();
            //var nNew = sNew != "" ? +sNew : 0;
            //var sOld = $("input[id*=" + sID[0] + "_Old_" + "]").val();
            //var nOld = sOld != "" ? +sOld : 0;
            //var nTotal = 0;

            nTotal = Math.abs((nNew + nTotalAll) - nOld);
            var sValOut = "";
            if (nTotal == 0) {
                //nTotal = "";
                sValOut = CheckTextOutput(nTotal);
            }
            else {
                sValOut = CheckTextOutput(nTotal);
            }
            AddToArray("", sTotalID, nTotal, "Total", sWasteType);
            $("input[id$=" + sTotalID + "]").val(sValOut);
        }
        function AddToArray(id, sID, val, sType, sWasteType) {
            var arr = sWasteType == "HZD" ? arrHZD : sWasteType == "NHZD" ? arrNHZD : arrMul;
            var str = sID.split("_");
            var sMonth = sType == "H" ? str[3] : sType == "G" ? str[2] : sType == "A" ? str[2] : sType == "T" ? str[2] : sType == "S" ?
                         str[4] : sType == "Total" ? str[1] : sType == "O" ? str[1] : sType == "N" ? str[1] : "";
            id = sType == "H" ? str[4] : sType == "G" ? str[4] : sType == "A" ? str[3] : sType == "T" ? str[4] : sType == "S" ? str[5] : sType == "Total" ? str[2] : sType == "O" ? str[2] : sType == "N" ? str[2] : id;

            if (sType == "S") {
                var HeadID = +str[3];
                var arrS = Enumerable.From(arrSub).Where(function (w) { return w.sType == sWasteType && w.sStatus == "Y" && w.nHeadID == HeadID }).ToArray();
                for (var i = 0; i < arrS.length; i++) {
                    if (arrS[i].nSubID == id) {
                        if (sMonth == "M1") arrS[i].M1 = val + "";
                        else if (sMonth == "M2") arrS[i].M2 = val + "";
                        else if (sMonth == "M3") arrS[i].M3 = val + "";
                        else if (sMonth == "M4") arrS[i].M4 = val + "";
                        else if (sMonth == "M5") arrS[i].M5 = val + "";
                        else if (sMonth == "M6") arrS[i].M6 = val + "";
                        else if (sMonth == "M7") arrS[i].M7 = val + "";
                        else if (sMonth == "M8") arrS[i].M8 = val + "";
                        else if (sMonth == "M9") arrS[i].M9 = val + "";
                        else if (sMonth == "M10") arrS[i].M10 = val + "";
                        else if (sMonth == "M11") arrS[i].M11 = val + "";
                        else if (sMonth == "M12") arrS[i].M12 = val + "";
                        else if (sMonth == "Target") arrS[i].Target = val + "";
                        else if (sMonth == "Name") arrS[i].sName = val + "";
                        else if (sMonth == "Unit") arrS[i].sUnit = val + "";
                        else if (sMonth == "Disposal") arrS[i].sDisposal = val + "";
                    }
                }
            }
            else {
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].ProductID == id) {
                        var arrP = arr[i].lstProduct;

                        if (arrP.length > 0) {
                            for (var x = 0; x < arrP.length; x++) {
                                if (sMonth == "M1") arrP[x].M1 = val + "";
                                else if (sMonth == "M2") arrP[x].M2 = val + "";
                                else if (sMonth == "M3") arrP[x].M3 = val + "";
                                else if (sMonth == "M4") arrP[x].M4 = val + "";
                                else if (sMonth == "M5") arrP[x].M5 = val + "";
                                else if (sMonth == "M6") arrP[x].M6 = val + "";
                                else if (sMonth == "M7") arrP[x].M7 = val + "";
                                else if (sMonth == "M8") arrP[x].M8 = val + "";
                                else if (sMonth == "M9") arrP[x].M9 = val + "";
                                else if (sMonth == "M10") arrP[x].M10 = val + "";
                                else if (sMonth == "M11") arrP[x].M11 = val + "";
                                else if (sMonth == "M12") arrP[x].M12 = val + "";
                                else if (sMonth == "Target") arrP[x].Target = val + "";
                                else if (sMonth == "Old") arrP[x].PreviousYear = val + "";
                                else if (sMonth == "New") arrP[x].ReportingYear = val + "";
                                else if (sMonth == "Total") arrP[x].nTotal = val + "";
                            }
                        }
                        else {
                            /// ไม่มีข้อมูลมาก่อน
                            var obj = {
                                ProductID: id,
                                FormID: "",
                                M1: sMonth == "M1" ? val + "" : "",
                                M2: sMonth == "M2" ? val + "" : "",
                                M3: sMonth == "M3" ? val + "" : "",
                                M4: sMonth == "M4" ? val + "" : "",
                                M5: sMonth == "M5" ? val + "" : "",
                                M6: sMonth == "M6" ? val + "" : "",
                                M7: sMonth == "M7" ? val + "" : "",
                                M8: sMonth == "M8" ? val + "" : "",
                                M9: sMonth == "M9" ? val + "" : "",
                                M10: sMonth == "M10" ? val + "" : "",
                                M11: sMonth == "M11" ? val + "" : "",
                                M12: sMonth == "M12" ? val + "" : "",
                                nTotal: sMonth == "Total" ? val + "" : "",
                                PreviousYear: sMonth == "Old" ? val + "" : "",
                                ReportingYear: sMonth == "New" ? val + "" : "",
                                Target: sMonth == "Target" ? val + "" : "",
                                nTotal: sMonth == "Total" ? val + "" : "",
                            };
                            arr[i].lstProduct.push(obj);
                        }
                    }
                }
            }
        }

        function setTooltipProduct(sTooltip) {
            var sHtml = '';
            if (sTooltip != "" && sTooltip != null) {//<i class="far fa-question-circle"></i>
                sHtml += '&nbsp;<i class="fas fa-question-circle text-primary" title="' + sTooltip + '" style="font-size:16px"></i>';
            }
            return sHtml;
        }

        /////////************ SAVE *************\\\\\\\\\\\\
        function SaveData(Status) {

            arrMask = [];
            $("textarea[id*=txtRemarsk]").each(function (i, item) {
                var sID = $(this).attr("id");
                sID = sID.split("_");
                var sVal = $(this).val();
                if (sVal) {
                    var obj = {
                        nProductID: +sID[1],
                        sText: sVal,
                    }
                    arrMask.push(obj);
                }
            });

            var isPass = false;
            if (Status == 0) {
                var str = CheckData(0);
                if (str == "") {
                    isPass = true;
                }
                else {
                    setTimeout(function () {
                        $("div#divPopContentValidate").empty();
                        $("div#divPopContentValidate").append(str);
                        $("#popValidate").modal('toggle');
                    }, 500);
                    //DialogWarning(DialogHeader.Warning, str);

                }
            }
            else if (Status == 1) {
                var str = CheckData(1);
                if (str == "") {
                    isPass = true;
                }
                else {
                    //  DialogWarning(DialogHeader.Warning, str);
                    setTimeout(function () {
                        $("div#divPopContentValidate").empty();
                        $("div#divPopContentValidate").append(str);
                        $("#popValidate").modal('toggle');
                    }, 500);
                }
            }
            else if (Status == 27) {
                var str = CheckData(1);
                if (str == "") {
                    isPass = true;
                }
                else {
                    //  DialogWarning(DialogHeader.Warning, str);
                    setTimeout(function () {
                        $("div#divPopContentValidate").empty();
                        $("div#divPopContentValidate").append(str);
                        $("#popValidate").modal('toggle');
                    }, 500);
                }
            }
            else {
                isPass = true;
            }
            if (isPass) {
                $.each($dataFileOther, function (e, n) {
                    var ID = n.ID;
                    var sVal = $("input[id$=txtFile_" + ID + "]").val();
                    n.sDescription = sVal;
                });
                var item = {
                    lstHZD: arrHZD,
                    lstSub: arrSub,
                    lstMask: arrMask,
                    lstNHZD: arrNHZD,
                    lstDataFile: $dataFileOther,
                    lstMonth: arrMonth,
                    lstRecall: arrMonthRecall,
                    lstMul: arrMul,
                    sComment: $("textarea#txtsComment").val()
                };
                var itemSeach = {
                    nIndicator: +GetValDropdown("ddlIndicator"),
                    nOperationType: +GetValDropdown("ddlOperationType"),
                    nFacility: +GetValDropdown("ddlFacility"),
                    sYear: GetValDropdown("ddlYear"),
                };

                var sMg = DialogMsg.ConfirmSave;
                switch (Status + "") {
                    case "0": sMg = DialogMsg.ConfirmSaveDraft;
                        break;
                    case "1": sMg = DialogMsg.ConfirmSubmit;
                        break;
                    case "24": sMg = DialogMsg.ConfirmRecall;
                        break;
                    case "9999": sMg = DialogMsg.ConfirmSave;
                        break;
                    case "2": sMg = DialogMsg.ConfirmRequest;
                        break;
                    case "27": sMg = DialogMsg.ConfirmApprove;
                        break;
                }

                if (Status != "1") {
                    DialogConfirm(DialogHeader.Confirm, sMg, function () {
                        LoaddinProcess();
                        AjaxCallWebMethod("saveToDB", function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupLogin();
                            } else if (response.d.Status == SysProcess.Success) {

                                if (Status == "27") {
                                    HideLoadding();
                                    DialogSuccessRedirect(DialogHeader.Success, DialogMsg.ApproveComplete, "epi_mytask.aspx");
                                }
                                else {
                                    var sMg = DialogMsg.SaveComplete;
                                    switch (Status + "") {
                                        case "0": sMg = DialogMsg.SaveDraftComplete;
                                            break;
                                        case "1": sMg = DialogMsg.SubmitComplete;
                                            break;
                                        case "24": sMg = DialogMsg.RecallComplete;
                                            break;
                                        case "9999": sMg = DialogMsg.SaveComplete;
                                            break;
                                        case "2": sMg = DialogMsg.RequestComplete;
                                            break;
                                        case "27": sMg = DialogMsg.ApproveComplete;
                                            break;
                                    }
                                    DialogSuccess(DialogHeader.Success, sMg);
                                    LoadDataCheckddl();
                                }
                            } else {
                                DialogWarning(DialogHeader.Warning, response.d.Msg);
                            }
                        }, "", {
                            sFormID: $("input[id$=hidFromID]").val(),
                            itemSeach: itemSeach,
                            item: item,
                            Status: Status + ""
                        });
                    }, function () { HideLoadding(); });
                }
                else {
                    if (!IsDeviatePass) {
                        DialogConfirm(DialogHeader.Confirm, sMg, function () {
                            LoaddinProcess();
                            AjaxCallWebMethod("saveToDB", function (response) {
                                if (response.d.Status == SysProcess.SessionExpired) {
                                    PopupLogin();
                                } else if (response.d.Status == SysProcess.Success) {
                                    var lstProduct = [];
                                    var arrH = Enumerable.From(arrHZD).Where(function (w) { return w.ProductID == 2 || w.ProductID == 8 }).ToArray();
                                    for (var i = 0; i < arrH.length; i++) {
                                        var lstP = arrH[i].lstProduct;
                                        for (var x = 0; x < lstP.length; x++) {
                                            var obj = {
                                                ProductID: arrH[i].ProductID,
                                                ProductName: arrH[i].ProductName,
                                                M1: lstP[x].M1,
                                                M2: lstP[x].M2,
                                                M3: lstP[x].M3,
                                                M4: lstP[x].M4,
                                                M5: lstP[x].M5,
                                                M6: lstP[x].M6,
                                                M7: lstP[x].M7,
                                                M8: lstP[x].M8,
                                                M9: lstP[x].M9,
                                                M10: lstP[x].M10,
                                                M11: lstP[x].M11,
                                                M12: lstP[x].M12,
                                            };
                                            lstProduct.push(obj);
                                        }
                                    }
                                    var arrNH = Enumerable.From(arrNHZD).Where(function (w) { return w.ProductID == 17 || w.ProductID == 24 }).ToArray();
                                    for (var a = 0; a < arrNH.length; a++) {
                                        var lstP = arrNH[a].lstProduct;
                                        for (var x = 0; x < lstP.length; x++) {
                                            var obj = {
                                                ProductID: arrNH[a].ProductID,
                                                ProductName: arrNH[a].ProductName,
                                                M1: lstP[x].M1,
                                                M2: lstP[x].M2,
                                                M3: lstP[x].M3,
                                                M4: lstP[x].M4,
                                                M5: lstP[x].M5,
                                                M6: lstP[x].M6,
                                                M7: lstP[x].M7,
                                                M8: lstP[x].M8,
                                                M9: lstP[x].M9,
                                                M10: lstP[x].M10,
                                                M11: lstP[x].M11,
                                                M12: lstP[x].M12,
                                            };
                                            lstProduct.push(obj);
                                        }
                                    }
                                    LoadData();
                                    Deviate(lstProduct, Status);

                                } else {
                                    DialogWarning(DialogHeader.Warning, response.d.Msg);
                                }
                            }, function () { HideLoadding(); }, {
                                sFormID: $("input[id$=hidFromID]").val(),
                                itemSeach: itemSeach,
                                item: item,
                                Status: "0"
                            });
                        }, function () { HideLoadding(); });
                    }
                    else {
                        LoaddinProcess();
                        AjaxCallWebMethod("saveToDB", function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupLogin();
                            } else if (response.d.Status == SysProcess.Success) {
                                var sMg = DialogMsg.SaveComplete;
                                switch (Status + "") {
                                    case "0": sMg = DialogMsg.SaveDraftComplete;
                                        break;
                                    case "1": sMg = DialogMsg.SubmitComplete;
                                        break;
                                    case "24": sMg = DialogMsg.RecallComplete;
                                        break;
                                    case "9999": sMg = DialogMsg.SaveComplete;
                                        break;
                                    case "2": sMg = DialogMsg.RequestComplete;
                                        break;
                                    case "27": sMg = DialogMsg.ApproveComplete;
                                        break;
                                }
                                DialogSuccess(DialogHeader.Success, sMg);
                                LoadDataCheckddl();
                            } else {
                                DialogWarning(DialogHeader.Warning, response.d.Msg);
                            }
                        }, function () { HideLoadding(); }, {
                            sFormID: $("input[id$=hidFromID]").val(),
                            itemSeach: itemSeach,
                            item: item,
                            Status: Status + ""
                        });
                    }
                }
            }
        }

        function CheckData(nStatus) {
            var str = "";
            var Msg = "";
            if (nStatus == 1) {
                var sMsgHZD = "";
                var sMsgNHZD = "";
                var sTotalHZD = "";
                var sTotalNHZD = "";
                var sMsgMul = "";

                //arrCheckMonthNoData = Enumerable.From(arrMonth).Where(function (w) { return w != n }).ToArray();

                var isDataHZD = true; // false คือ ไม่มีข้อมูล true = มีข้อมูล
                for (var h = 0; h < arrHZD.length; h++) {
                    Msg = ""; var arrP = arrHZD[h].lstProduct; var sMsgHead = "";
                    var arrC = [];

                    ///// TOTAL 
                    if (arrHZD[h].cTotal == "Y" && arrHZD[h].cTotalAll == "Y" && arrHZD[h].nGroupCalc == 0) {
                        if (arrP.length > 0) {
                            $.each(arrMonth, function (e, n) {
                                if (n == 1) {
                                    if (arrP[0].M1 == "") {
                                        Msg += "<br/>- Please specify January";
                                        isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 2) {
                                    if (arrP[0].M2 == "") {
                                        Msg += "<br/>- Please specify February"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 3) {
                                    if (arrP[0].M3 == "") {
                                        Msg += "<br/>- Please specify March"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 4) {
                                    if (arrP[0].M4 == "") {
                                        Msg += "<br/>- Please specify April"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 5) {
                                    if (arrP[0].M5 == "") {
                                        Msg += "<br/>- Please specify May"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 6) {
                                    if (arrP[0].M6 == "") {
                                        Msg += "<br/>- Please specify June"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 7) {
                                    if (arrP[0].M7 == "") {
                                        Msg += "<br/>- Please specify July"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 8) {
                                    if (arrP[0].M8 == "") {
                                        Msg += "<br/>- Please specify August"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 9) {
                                    if (arrP[0].M9 == "") {
                                        Msg += "<br/>- Please specify September"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 10) {
                                    if (arrP[0].M10 == "") {
                                        Msg += "<br/>- Please specify October"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 11) {
                                    if (arrP[0].M11 == "") {
                                        Msg += "<br/>- Please specify November"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 12) {
                                    if (arrP[0].M12 == "") {
                                        Msg += "<br/>- Please specify December"; isDataHZD = false;
                                        arrC.push(1);
                                    }
                                }
                            });
                        }
                        else {
                            // Msg += "<br/>- Please specify data";
                            isDataHZD = false;
                        }

                        if (arrC.length == arrMonth.length) {
                            Msg = "";
                        }

                        if (Msg != "") {
                            sTotalHZD = "&bull; " + arrHZD[h].ProductName + Msg;
                            Msg = "";
                        }
                    }
                    else if (arrHZD[h].cTotal == "N" && arrHZD[h].cTotalAll == "N" && arrHZD[h].nGroupCalc == 99) {
                        ///// ON-SITE
                        if (isDataHZD) { /// ถ้ามีข้อมูล total ถึงเช็ค
                            if (arrP.length > 0) {
                                if (arrP[0].PreviousYear == "") {
                                    Msg += "<br/>- Please specify PreviousYear";
                                }

                                if (arrP[0].ReportingYear == "") {
                                    Msg += "<br/>- Please specify ReportingYear";
                                }
                            }
                            else {
                                Msg += "<br/>- Please specify data";
                            }
                        }

                        var arrM = Enumerable.From(arrMask).Where(function (w) { return w.nProductID == arrHZD[h].ProductID }).ToArray();
                        if (arrM.length > 0) {
                            var sMark = $("textarea[id$=txtRemarsk_" + arrHZD[h].ProductID + "]").val();
                            if (sMark == "") {
                                Msg += "<br/>- Please specify Remark ";
                            }
                        }
                        else {
                            Msg += "<br/>- Please specify Remark ";
                        }
                    }
                    else if (arrHZD[h].cTotal == "Y" && arrHZD[h].cTotalAll == "N") {
                        //// Non Routine - Routine Check Remark HZD
                        var arrM = Enumerable.From(arrMask).Where(function (w) { return w.nProductID == arrHZD[h].ProductID }).ToArray();
                        if (arrM.length > 0) {
                            var sMark = $("textarea[id$=txtRemarsk_" + arrHZD[h].ProductID + "]").val();
                            if (sMark == "") {
                                Msg += "<br/>- Please specify Remark ";
                            }
                        }
                        else {
                            Msg += "<br/>- Please specify Remark ";
                        }
                    }
                    else if (arrHZD[h].cTotal == "N" && arrHZD[h].cTotalAll == "N") {
                        var arrS = Enumerable.From(arrSub).Where(function (w) { return w.nHeadID == arrHZD[h].ProductID && w.sStatus == "Y" && w.sType == "HZD" }).ToArray();
                        if (arrS.length > 0) {
                            var isName = true; var isM1 = true; var isM2 = true; var isM3 = true; var isM4 = true; var isM5 = true; var isM6 = true;
                            var isM7 = true; var isM8 = true; var isM9 = true; var isM10 = true; var isM11 = true; var isM12 = true; var isDisposal = true;
                            for (var s = 0; s < arrS.length; s++) {
                                if (arrS[s].sName == "") isName = false;
                                if (arrS[s].sDisposal == "") isDisposal = false;
                                $.each(arrMonth, function (e, n) {
                                    if (n == 1) {
                                        if (arrS[s].M1 == "") isM1 = false;
                                    }
                                    else if (n == 2) {
                                        if (arrS[s].M2 == "") isM2 = false;
                                    }
                                    else if (n == 3) {
                                        if (arrS[s].M3 == "") isM3 = false;
                                    }
                                    else if (n == 4) {
                                        if (arrS[s].M4 == "") isM4 = false;
                                    }
                                    else if (n == 5) {
                                        if (arrS[s].M5 == "") isM5 = false;
                                    }
                                    else if (n == 6) {
                                        if (arrS[s].M6 == "") isM6 = false;
                                    }
                                    else if (n == 7) {
                                        if (arrS[s].M7 == "") isM7 = false;
                                    }
                                    else if (n == 8) {
                                        if (arrS[s].M8 == "") isM8 = false;
                                    }
                                    else if (n == 9) {
                                        if (arrS[s].M9 == "") isM9 = false;
                                    }
                                    else if (n == 10) {
                                        if (arrS[s].M10 == "") isM10 = false;
                                    }
                                    else if (n == 11) {
                                        if (arrS[s].M11 == "") isM11 = false;
                                    }
                                    else if (n == 12) {
                                        if (arrS[s].M12 == "") isM12 = false;
                                    }
                                });
                            }
                            if (!isName) Msg += "<br/>- Please specify Indicator";
                            if (!isDisposal) Msg += "<br/>- Please select Disposal Code";
                            if (!isM1) Msg += "<br/>- Please specify January";
                            if (!isM2) Msg += "<br/>- Please specify February";
                            if (!isM3) Msg += "<br/>- Please specify March";
                            if (!isM4) Msg += "<br/>- Please specify April";
                            if (!isM5) Msg += "<br/>- Please specify May";
                            if (!isM6) Msg += "<br/>- Please specify June";
                            if (!isM7) Msg += "<br/>- Please specify July";
                            if (!isM8) Msg += "<br/>- Please specify August";
                            if (!isM9) Msg += "<br/>- Please specify September";
                            if (!isM10) Msg += "<br/>- Please specify October";
                            if (!isM11) Msg += "<br/>- Please specify November";
                            if (!isM12) Msg += "<br/>- Please specify December";
                        }
                    }

                    if (Msg != "") {
                        if (arrHZD[h].cTotal == "N" && arrHZD[h].cTotalAll == "N" && arrHZD[h].nGroupCalc != 99) {
                            sMsgHZD += sMsgHZD != "" ? "<br/><br/>&bull; " + arrHZD[h].ProductName + " (Hazardous Waste)" : "&bull; " + arrHZD[h].ProductName + " (Hazardous Waste)";
                        }
                        else {
                            sMsgHZD += sMsgHZD != "" ? "<br/><br/>&bull; " + arrHZD[h].ProductName : "&bull; " + arrHZD[h].ProductName;
                        }
                        sMsgHZD += Msg;
                    }
                }

                var isDataNHZD = true; // false คือ ไม่มีข้อมูล true = มีข้อมูล
                for (var n = 0; n < arrNHZD.length; n++) {
                    Msg = ""; var arrP = arrNHZD[n].lstProduct; var sMsgHead = "";
                    var arrC = [];

                    if (arrNHZD[n].cTotal == "Y" && arrNHZD[n].cTotalAll == "Y" && arrNHZD[n].nGroupCalc == 0) {
                        /// TOTAL NHZD
                        if (arrP.length > 0) {
                            $.each(arrMonth, function (e, n) {
                                if (n == 1) {
                                    if (arrP[0].M1 == "") {
                                        Msg += "<br/>- Please specify January"; isDataNHZD = false;
                                        arrC.push(1);
                                    }

                                }
                                else if (n == 2) {
                                    if (arrP[0].M2 == "") {
                                        Msg += "<br/>- Please specify February"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 3) {
                                    if (arrP[0].M3 == "") {
                                        Msg += "<br/>- Please specify March"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 4) {
                                    if (arrP[0].M4 == "") {
                                        Msg += "<br/>- Please specify April"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 5) {
                                    if (arrP[0].M5 == "") {
                                        Msg += "<br/>- Please specify May"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 6) {
                                    if (arrP[0].M6 == "") {
                                        Msg += "<br/>- Please specify June"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 7) {
                                    if (arrP[0].M7 == "") {
                                        Msg += "<br/>- Please specify July"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 8) {
                                    if (arrP[0].M8 == "") {
                                        Msg += "<br/>- Please specify August"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 9) {
                                    if (arrP[0].M9 == "") {
                                        Msg += "<br/>- Please specify September"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 10) {
                                    if (arrP[0].M10 == "") {
                                        Msg += "<br/>- Please specify October"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 11) {
                                    if (arrP[0].M11 == "") {
                                        Msg += "<br/>- Please specify November"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                                else if (n == 12) {
                                    if (arrP[0].M12 == "") {
                                        Msg += "<br/>- Please specify December"; isDataNHZD = false;
                                        arrC.push(1);
                                    }
                                }
                            });
                        }
                        else {
                            // Msg += "<br/>- Please specify data";
                            isDataNHZD = false;
                        }

                        if (arrC.length == arrMonth.length) {
                            Msg = "";
                        }

                        if (Msg != "") {
                            sTotalNHZD = "<br/>&bull; " + arrNHZD[n].ProductName + Msg;
                            Msg = "";
                        }
                    }
                    else if (arrNHZD[n].cTotal == "N" && arrNHZD[n].cTotalAll == "N" && arrNHZD[n].nGroupCalc == 99) {
                        ///ON_SITE NHZD
                        if (isDataNHZD) { /// ถ้ามีข้อมูล total ถึงเช็ค
                            if (arrP.length > 0) {
                                if (arrP[0].PreviousYear == "") {
                                    Msg += "<br/>- Please specify PreviousYear";
                                }

                                if (arrP[0].ReportingYear == "") {
                                    Msg += "<br/>- Please specify ReportingYear";
                                }
                            }
                            else {
                                Msg += "<br/>- Please specify data";
                            }
                        }

                        var arrM = Enumerable.From(arrMask).Where(function (w) { return w.nProductID == arrNHZD[n].ProductID }).ToArray();
                        if (arrM.length > 0) {
                            var sMark = $("textarea[id$=txtRemarsk_" + arrNHZD[n].ProductID + "]").val();
                            if (sMark == "") {
                                Msg += "<br/>- Please specify Remark ";
                            }
                        }
                        else {
                            Msg += "<br/>- Please specify Remark ";
                        }
                    }
                    else if (arrNHZD[n].cTotal == "Y" && arrNHZD[n].cTotalAll == "N") {
                        /////  Routine - Non  Routine NHZD
                        var arrM = Enumerable.From(arrMask).Where(function (w) { return w.nProductID == arrNHZD[n].ProductID }).ToArray();
                        if (arrM.length > 0) {
                            var sMark = $("textarea[id$=txtRemarsk_" + arrNHZD[n].ProductID + "]").val();
                            if (sMark == "") {
                                Msg += "<br/>- Please specify Remark ";
                            }
                        }
                        else {
                            Msg += "<br/>- Please specify Remark ";
                        }
                    }
                    else if (arrNHZD[h].cTotal == "N" && arrNHZD[h].cTotalAll == "N") {
                        var arrS = Enumerable.From(arrSub).Where(function (w) { return w.nHeadID == arrNHZD[n].ProductID && w.sStatus == "Y" && w.sType == "NHZD" }).ToArray();
                        if (arrS.length > 0) {
                            var isName = true; var isM1 = true; var isM2 = true; var isM3 = true; var isM4 = true; var isM5 = true; var isM6 = true;
                            var isM7 = true; var isM8 = true; var isM9 = true; var isM10 = true; var isM11 = true; var isM12 = true; var isDisposal = true;
                            for (var s = 0; s < arrS.length; s++) {
                                if (arrS[s].sName == "") isName = false;
                                if (arrS[s].sDisposal == "") isDisposal = false;
                                $.each(arrMonth, function (e, n) {
                                    if (n == 1) {
                                        if (arrS[s].M1 == "") isM1 = false;
                                    }
                                    else if (n == 2) {
                                        if (arrS[s].M2 == "") isM2 = false;
                                    }
                                    else if (n == 3) {
                                        if (arrS[s].M3 == "") isM3 = false;
                                    }
                                    else if (n == 4) {
                                        if (arrS[s].M4 == "") isM4 = false;
                                    }
                                    else if (n == 5) {
                                        if (arrS[s].M5 == "") isM5 = false;
                                    }
                                    else if (n == 6) {
                                        if (arrS[s].M6 == "") isM6 = false;
                                    }
                                    else if (n == 7) {
                                        if (arrS[s].M7 == "") isM7 = false;
                                    }
                                    else if (n == 8) {
                                        if (arrS[s].M8 == "") isM8 = false;
                                    }
                                    else if (n == 9) {
                                        if (arrS[s].M9 == "") isM9 = false;
                                    }
                                    else if (n == 10) {
                                        if (arrS[s].M10 == "") isM10 = false;
                                    }
                                    else if (n == 11) {
                                        if (arrS[s].M11 == "") isM11 = false;
                                    }
                                    else if (n == 12) {
                                        if (arrS[s].M12 == "") isM12 = false;
                                    }
                                });
                            }

                            if (!isName) Msg += "<br/>- Please specify Indicator";
                            if (!isDisposal) Msg += "<br/>- Please select Disposal Code";
                            if (!isM1) Msg += "<br/>- Please specify January";
                            if (!isM2) Msg += "<br/>- Please specify February";
                            if (!isM3) Msg += "<br/>- Please specify March";
                            if (!isM4) Msg += "<br/>- Please specify April";
                            if (!isM5) Msg += "<br/>- Please specify May";
                            if (!isM6) Msg += "<br/>- Please specify June";
                            if (!isM7) Msg += "<br/>- Please specify July";
                            if (!isM8) Msg += "<br/>- Please specify August";
                            if (!isM9) Msg += "<br/>- Please specify September";
                            if (!isM10) Msg += "<br/>- Please specify October";
                            if (!isM11) Msg += "<br/>- Please specify November";
                            if (!isM12) Msg += "<br/>- Please specify December";
                        }
                    }

                    if (Msg != "") {
                        if (arrNHZD[n].cTotal == "N" && arrNHZD[n].cTotalAll == "N" && arrNHZD[n].nGroupCalc != 99) {
                            sMsgNHZD += sMsgNHZD != "" ? "<br/><br/>&bull; " + arrNHZD[n].ProductName + " (Non Hazardous Waste)" : "<br/>&bull; " + arrNHZD[n].ProductName + " (Non Hazardous Waste)";
                        }
                        else {
                            sMsgNHZD += sMsgNHZD != "" ? "<br/><br/>&bull; " + arrNHZD[n].ProductName : "<br/><br/>&bull; " + arrNHZD[n].ProductName;
                        }
                        sMsgNHZD += Msg;
                    }
                }

                var arrM = Enumerable.From(arrMask).Where(function (w) { return w.nProductID == 240 }).ToArray();
                if (arrM.length > 0) {
                    var sMark = $("textarea[id$=txtRemarsk_240]").val();
                    if (sMark == "") {
                        sMsgMul += "<br/><br/>&bull;Total Other municipal";
                        sMsgMul += "<br/>- Please specify Remark ";
                    }
                }
                else {
                    sMsgMul += "<br/><br/>&bull;Total Other municipal";
                    sMsgMul += "<br/>- Please specify Remark ";
                }

                var arrSubMul = Enumerable.From(arrSub).Where(function (w) { return w.sType == "MUL" && w.sStatus == "Y" }).ToArray();
                if (arrSubMul.length > 0) {
                    var isDataNMul = false;
                    Msg = "";
                    $.each(arrMul, function (e, n) {
                        var arrP = n.lstProduct;
                        if (arrP.length > 0) {
                            $.each(arrMonth, function (e, n) {
                                if (n == 1) {
                                    if (arrP[0].M1 == "") {
                                        Msg += "<br/>- Please specify January"; isDataNMul = false;
                                    }

                                }
                                else if (n == 2) {
                                    if (arrP[0].M2 == "") {
                                        Msg += "<br/>- Please specify February"; isDataNMul = false;
                                    }
                                }
                                else if (n == 3) {
                                    if (arrP[0].M3 == "") {
                                        Msg += "<br/>- Please specify March"; isDataNMul = false;
                                    }
                                }
                                else if (n == 4) {
                                    if (arrP[0].M4 == "") {
                                        Msg += "<br/>- Please specify April"; isDataNMul = false;
                                    }
                                }
                                else if (n == 5) {
                                    if (arrP[0].M5 == "") {
                                        Msg += "<br/>- Please specify May"; isDataNMul = false;
                                    }
                                }
                                else if (n == 6) {
                                    if (arrP[0].M6 == "") {
                                        Msg += "<br/>- Please specify June"; isDataNMul = false;
                                    }
                                }
                                else if (n == 7) {
                                    if (arrP[0].M7 == "") {
                                        Msg += "<br/>- Please specify July"; isDataNMul = false;
                                    }
                                }
                                else if (n == 8) {
                                    if (arrP[0].M8 == "") {
                                        Msg += "<br/>- Please specify August"; isDataNMul = false;
                                    }
                                }
                                else if (n == 9) {
                                    if (arrP[0].M9 == "") {
                                        Msg += "<br/>- Please specify September"; isDataNMul = false;
                                    }
                                }
                                else if (n == 10) {
                                    if (arrP[0].M10 == "") {
                                        Msg += "<br/>- Please specify October"; isDataNMul = false;
                                    }
                                }
                                else if (n == 11) {
                                    if (arrP[0].M11 == "") {
                                        Msg += "<br/>- Please specify November"; isDataNMul = false;
                                    }
                                }
                                else if (n == 12) {
                                    if (arrP[0].M12 == "") {
                                        Msg += "<br/>- Please specify December"; isDataNMul = false;
                                    }
                                }
                            });
                        }
                        else {
                            isDataNMul = false;
                        }
                    });

                    /// หัวไม่มี แต่มีลูก
                    if (!isDataNMul) {
                        if (Msg != "") {
                            sMsgMul += sMsgMul != "" ? "<br/><br/>&bull;Total Other municipal" : "&bull;Total Other municipal";
                            sMsgMul += Msg;
                        }
                    }
                    else {
                        /// หัวมีข้อมูล เช็ดลูกที่เหลือ
                        var isName = true; var isM1 = true; var isM2 = true; var isM3 = true; var isM4 = true; var isM5 = true; var isM6 = true;
                        var isM7 = true; var isM8 = true; var isM9 = true; var isM10 = true; var isM11 = true; var isM12 = true;
                        Msg = "";
                        for (var i = 0; i < arrSubMul.length; i++) {
                            if (arrSubMul[i].sName == "") isName = false;
                            $.each(arrMonth, function (e, n) {
                                if (n == 1) {
                                    if (arrSubMul[i].M1 == "") isM1 = false;
                                }
                                else if (n == 2) {
                                    if (arrSubMul[i].M2 == "") isM2 = false;
                                }
                                else if (n == 3) {
                                    if (arrSubMul[i].M3 == "") isM3 = false;
                                }
                                else if (n == 4) {
                                    if (arrSubMul[i].M4 == "") isM4 = false;
                                }
                                else if (n == 5) {
                                    if (arrSubMul[i].M5 == "") isM5 = false;
                                }
                                else if (n == 6) {
                                    if (arrSubMul[i].M6 == "") isM6 = false;
                                }
                                else if (n == 7) {
                                    if (arrSubMul[i].M7 == "") isM7 = false;
                                }
                                else if (n == 8) {
                                    if (arrSubMul[i].M8 == "") isM8 = false;
                                }
                                else if (n == 9) {
                                    if (arrSubMul[i].M9 == "") isM9 = false;
                                }
                                else if (n == 10) {
                                    if (arrSubMul[i].M10 == "") isM10 = false;
                                }
                                else if (n == 11) {
                                    if (arrSubMul[i].M11 == "") isM11 = false;
                                }
                                else if (n == 12) {
                                    if (arrSubMul[i].M12 == "") isM12 = false;
                                }
                            });
                        }
                        if (!isName) Msg += "<br/>- Please specify Indicator";
                        if (!isM1) Msg += "<br/>- Please specify January";
                        if (!isM2) Msg += "<br/>- Please specify February";
                        if (!isM3) Msg += "<br/>- Please specify March";
                        if (!isM4) Msg += "<br/>- Please specify April";
                        if (!isM5) Msg += "<br/>- Please specify May";
                        if (!isM6) Msg += "<br/>- Please specify June";
                        if (!isM7) Msg += "<br/>- Please specify July";
                        if (!isM8) Msg += "<br/>- Please specify August";
                        if (!isM9) Msg += "<br/>- Please specify September";
                        if (!isM10) Msg += "<br/>- Please specify October";
                        if (!isM11) Msg += "<br/>- Please specify November";
                        if (!isM12) Msg += "<br/>- Please specify December";

                        if (Msg != "") {
                            sMsgMul += sMsgMul != "" ? "<br/><br/>&bull; Sub Total Other municipal" : "&bull; Sub Total Other municipal";
                            sMsgMul += Msg;
                        }
                    }
                }

                if (!isDataHZD && !isDataNHZD) { //&& !isDataNHZD !isDataHZD
                    //str += sTotalHZD + sTotalHZD != "" ? "<br>" + sTotalNHZD : sTotalNHZD;
                    if (sTotalHZD != "" || sTotalNHZD != "") {
                        str += str == "" ? sTotalHZD : "<br/>" + sTotalHZD;
                        str += str == "" ? sTotalNHZD : "<br/>" + sTotalNHZD;
                    }
                    else {
                        str = "Please specify data";
                    }

                }
                else if ((isDataHZD || isDataNHZD) && (sMsgMul != "" || sMsgHZD != "" || sMsgNHZD != "")) { //(isDataHZD || isDataNHZD)
                    str += sMsgHZD + sMsgNHZD + sMsgMul;
                }

            }
            else if (nStatus == 0) {
                var isNotValid = true;
                $.each(arrHZD, function (e, n) {
                    $.each(n.lstProduct, function (e, n) {
                        if (n.M1) isNotValid = false;
                        if (n.M2) isNotValid = false;
                        if (n.M3) isNotValid = false;
                        if (n.M4) isNotValid = false;
                        if (n.M5) isNotValid = false;
                        if (n.M6) isNotValid = false;
                        if (n.M7) isNotValid = false;
                        if (n.M8) isNotValid = false;
                        if (n.M9) isNotValid = false;
                        if (n.M10) isNotValid = false;
                        if (n.M11) isNotValid = false;
                        if (n.M12) isNotValid = false;
                    });
                });
                $.each(arrNHZD, function (e, n) {
                    $.each(n.lstProduct, function (e, n) {
                        if (n.M1) isNotValid = false;
                        if (n.M2) isNotValid = false;
                        if (n.M3) isNotValid = false;
                        if (n.M4) isNotValid = false;
                        if (n.M5) isNotValid = false;
                        if (n.M6) isNotValid = false;
                        if (n.M7) isNotValid = false;
                        if (n.M8) isNotValid = false;
                        if (n.M9) isNotValid = false;
                        if (n.M10) isNotValid = false;
                        if (n.M11) isNotValid = false;
                        if (n.M12) isNotValid = false;
                    });
                });
                $.each(arrMul, function (e, n) {
                    $.each(n.lstProduct, function (e, n) {
                        if (n.M1) isNotValid = false;
                        if (n.M2) isNotValid = false;
                        if (n.M3) isNotValid = false;
                        if (n.M4) isNotValid = false;
                        if (n.M5) isNotValid = false;
                        if (n.M6) isNotValid = false;
                        if (n.M7) isNotValid = false;
                        if (n.M8) isNotValid = false;
                        if (n.M9) isNotValid = false;
                        if (n.M10) isNotValid = false;
                        if (n.M11) isNotValid = false;
                        if (n.M12) isNotValid = false;
                    });
                });

                if (isNotValid) {
                    str = "Please specify data";
                }
            }

            return str;
        }

        /////////************ EXPORT *************\\\\\\\\\\\\
        //function ExportData() {
        //    
        //    $("input[id$=hidYear]").val(GetValDropdown("ddlYear"));
        //    $("input[id$=hidIndicator]").val(GetValDropdown("ddlIndicator"));
        //    $("input[id$=hidOperationType]").val(GetValDropdown("ddlOperationType"));
        //    $("input[id$=hidFacility]").val(GetValDropdown("ddlFacility"));
        //    $("input[id$=btnEx]").click();
        //}

        //////////************** FILE *****************\\\\\\\\\
        var $dataFileOther = [];
        function LoadDataFileOther() {
            AjaxCallWebMethod("LoadDataFileOther", function (response) {
                if (response.d.Status == SysProcess.Success) {
                    $dataFileOther = response.d.lstData;
                }
                BindTableFileOther();
            }, "", { sFormID: $hidFromID.val() });
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
                    data: { funcname: "UPLOAD", savetopath: 'Waste/Temp/', savetoname: '' },
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
                    $("td", row).eq(2).html('<input id="txtFile_' + item.ID + '" class="form-control" maxlength="1000" />');
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
            CheckEventButton();
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
    </script>
</asp:Content>

