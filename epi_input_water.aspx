<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_EPI_FORMS.master" AutoEventWireup="true" CodeFile="epi_input_water.aspx.cs" Inherits="epi_input_water" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        table#tbData > tbody > tr > td > input:not(.str), input.Density {
            text-align: right !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divContent" style="display: none;">
        <div class="col-xs-12 col-md-6 text-left" style="margin-bottom: 5px;">
            <a style="font-size: 24px;" title="Helper Water Withdrawal" href="Helper_Indicator.aspx?ind=11&&prd=91" target="_blank"><i class="fas fa-question-circle"></i></a>
            <a style="font-size: 24px;" title="Helper Water Recycled & Reused" href="Helper_Indicator.aspx?ind=11&&prd=101" target="_blank"><i class="fas fa-question-circle"></i></a>
        </div>
        <div class="col-xs-12 col-md-6 text-right-lg text-right-md text-left-sm" style="margin-bottom: 5px;">
            <button type="button" onclick="ShowDeviate();" class="btn btn-info" title="Deviate History"><i class="fas fa-comments"></i></button>
            <button type="button" onclick="ShowHistory();" class="btn btn-info" title="Workflow History"><i class="fas fa-comment-alt"></i></button>
            <asp:LinkButton ID="lnkExport" runat="server" CssClass="btn btn-success" OnClick="lnkExport_Click">Export</asp:LinkButton>
        </div>
        <div class="col-xs-12">
            <div class="table-responsive">
                <table id="tbData" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                </table>
            </div>
        </div>
    </div>
    <div id="divRemark" style="display: none;">
        <div class="col-xs-12 col-md-6" style="margin-top: 20px;">
            <div class="well">
                <div class="form-group">
                    <label class="control-label">Remark (Total Water Withdrawal)<span class="text-red">*</span></label>
                    <textarea id="txtRemarkWW" runat="server" class="form-control" rows="4" style="resize: vertical"></textarea>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-6" style="margin-top: 20px;">
            <div class="well">
                <div class="form-group">
                    <label class="control-label col-xs-12">Remark (Total Water Recycled & Reused)<span class="text-red">*</span></label>
                    <textarea id="txtRemarkWR" runat="server" class="form-control" rows="4" style="resize: vertical"></textarea>
                </div>
            </div>
        </div>
    </div>
    <div id="divUploadFile" style="display: none;">
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
    <asp:HiddenField runat="server" ID="hdfIndID" />
    <asp:HiddenField runat="server" ID="hdfRemarkWW" />
    <asp:HiddenField runat="server" ID="hdfRemarkWR" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        var arrShortMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var arrFullMonth = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var $tbData = $("table[id$=tbData]");
        var $divContent = $("div[id$=divContent]");
        var $divRemark = $("div[id$=divRemark]");
        var $divUploadFile = $("div[id$=divUploadFile]");
        var lstUnit = [];
        var lstProduct = [];
        var IsFullMonth = true;
        /////// File /////////
        var $dataFileOther = [];
        $(function () {
            ArrInputFromTableID.push("tbData");
            $tbData.delegate("input:not(input.str,input.target)", "change", function () {
                var HeadID = parseInt($(this).attr("under"), 0);
                var Month = $(this).attr("month");
                var TrueVal = $(this).val();
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
                var nValMultiply = 1;
                var nValTotal = "";
                if ($(this).hasClass("Density")) {
                    $(this).parent().parent().parent().find("td[class*=M_] input").change();
                    return true;
                } else {
                    $.each($("input[under=" + HeadID + "][month=" + Month + "]"), function (i, el) {
                        var nVal = $(el).val().replace(/,/g, '');
                        if (CheckTextInput(nVal + "") == "" || CheckTextInput(nVal + "").toLowerCase() == "n/a") {
                            if (nValTotal.toString() == "") {
                                nValTotal = "";
                            }
                        } else {
                            if (nValTotal.toString() == "") {
                                if ($(el).parent().parent().find("select").val() == "3") {
                                    nValTotal = +nVal / (nValMultiply);
                                } else {
                                    nValTotal = +nVal * (nValMultiply);
                                }
                            } else {
                                if ($(el).parent().parent().find("select").val() == "3") {
                                    nValTotal = ((+nVal / (nValMultiply)) + (+nValTotal)) + "";
                                } else {
                                    nValTotal = ((+nVal * (nValMultiply)) + (+nValTotal)) + "";
                                }
                            }
                        }
                    })
                    //if ($(this).attr("prdid") != "100") {
                    $tbData.find("input[prdID=" + HeadID + "][month=" + Month + "]").val(CheckTextOutput(nValTotal));
                    if (nValTotal.toString() != "") {
                        nValTotal = (Math.round((nValTotal) * 10000000000) / 10000000000)
                    }
                    var itemPrdLV3 = Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == HeadID + "" });
                    setDataArr(itemPrdLV3, Month, nValTotal);
                    var lstPrdLV2 = Enumerable.From(lstProduct).Where(function (w) { return w.nHeaderID == itemPrdLV3.nHeaderID + "" }).ToArray();
                    var nTotalLevel2 = HeaderTotal(lstPrdLV2, Month);
                    $tbData.find("input[prdID=" + itemPrdLV3.nHeaderID + "][month=" + Month + "]").val(CheckTextOutput(nTotalLevel2));
                    if (nTotalLevel2.toString() != "") {
                        nTotalLevel2 = (Math.round((nTotalLevel2) * 10000000000) / 10000000000)
                    }
                    var itemHeadLv2 = Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == lstPrdLV2[0].nHeaderID + "" })
                    setDataArr(itemHeadLv2, Month, nTotalLevel2);
                    if (itemHeadLv2.nHeaderID != null && itemHeadLv2.nHeaderID != "" && itemHeadLv2.nHeaderID != undefined) {
                        var nTotalLevel1 = "";
                        var lstPrdLv1 = Enumerable.From(lstProduct).Where(function (w) { return w.nHeaderID == itemHeadLv2.nHeaderID }).ToArray();
                        var nTotalLevel1 = HeaderTotal(lstPrdLv1, Month);
                        if (nTotalLevel1.toString() != "") {
                            nTotalLevel1 = (Math.round((nTotalLevel1) * 10000000000) / 10000000000)
                        }
                        setDataArr(Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == itemHeadLv2.nHeaderID + "" }), Month, nTotalLevel1);
                        $tbData.find("input[prdID=" + itemHeadLv2.nHeaderID + "][month=" + Month + "]").val(CheckTextOutput(nTotalLevel1));
                    }
                    //} else {
                    //    var PrdID = +$(this).attr("prdid");
                    //    var itemPrdLV3 = Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == PrdID });
                    //    setDataArr(itemPrdLV3, Month, TrueVal);
                    //    var lstPrdLV2 = Enumerable.From(lstProduct).Where(function (w) { return w.nHeaderID == HeadID + "" }).ToArray();
                    //    var nTotalLevel2 = HeaderTotal(lstPrdLV2, Month);
                    //    $tbData.find("input[prdID=" + HeadID + "][month=" + Month + "]").val(CheckTextOutput(nTotalLevel2));
                    //    var itemHeadLv2 = Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == lstPrdLV2[0].nHeaderID + "" })
                    //    setDataArr(itemHeadLv2, Month, nTotalLevel2);
                    //}
                }

            });
            $tbData.delegate("input.target", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
            });
            SetFileUploadOther();
        })
        function LoadData() {
            var param = {
                sIndID: Select("ddlIndicator").val(),
                sOprtID: Select("ddlOperationType").val(),
                sFacID: Select("ddlFacility").val(),
                sYear: Select("Year").val()
            }

            $("textarea,input:not([type=checkbox])").prop("disabled", false);
            $tbData.find("input.str,input.Density,select").prop("disabled", false);
            IsFullMonth = true;
            LoaddinProcess();
            $tbData.empty();
            AjaxCallWebMethod("LoadData", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                } else {
                    lstStatus.length = 0;
                    lstStatus = response.d.lstStatus;
                    lstUnit = response.d.lstUnit;
                    lstProduct = response.d.lstProduct;
                    $dataFileOther = response.d.lstFile;
                    nStatus = response.d.nStatus;
                    $hdfPRMS.val(response.d.hdfPRMS);
                    SetValueTextBox("hdfRemarkWW", response.d.sRemarkTotalWithdrawal);
                    SetValueTextBox("hdfRemarkWR", response.d.sRemarkTotalRecycled);
                    SetValueTextArea("txtRemarkWW", response.d.sRemarkTotalWithdrawal)
                    SetValueTextArea("txtRemarkWR", response.d.sRemarkTotalRecycled)
                    $.each(lstStatus, function (i, el) {
                        if (el.nStatusID == 0) {
                            IsFullMonth = false;
                        }
                    })
                    bindData(response.d);
                    BindTableFileOther();
                }
            }, function () {
                setPRMS();
                $divRemark.show();
                $divUploadFile.show();
                $divContent.show();
                CheckEventButton();
                CheckboxQuarterChanged();
                $tbData.tableHeadFixer({ "left": 1 }, { "head": true })
                if (IsFullMonth) {
                    if ($hdfIsAdmin.val() != "Y" && $hdfRole.val() != "4") {
                        $(".NoPRMS").hide();
                        $("textarea:not([id$=txtsComment]),input.target").prop("disabled", true);
                        $tbData.find("tr.trAdd").remove();
                        $tbData.find("input.str,input.Density,select").prop("disabled", true);
                    }
                }
                SetTootip();
                HideLoadding();
            }, { param: param });
        }
        function setPRMS() {
            if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                $tbData.find("tr[class*=txtInput] td[class*=M_] input").prop("disabled", false);
            }
        }
        function bindData(lst) {
            var sHtml = "";
            sHtml += "<thead>";
            sHtml += "  <tr>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthIndicator + "px;'><label>Indicator</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'><label>Unit</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;'><label>Target</label></th>";
            sHtml += bindColumnQ(true);
            sHtml += "  </tr>";
            sHtml += "</thead>";
            if (lst.lstProduct.length > 0) {
                sHtml += "<tbody>";
                //Leve 1  cTotal ="Y" , cTotalAll = "Y"
                $.each(Enumerable.From(lst.lstProduct).Where(function (w) { return w.nLevel == 1 }).ToArray(), function (i, el) {
                    sHtml += "<tr class='cTotalYY' id='" + el.ProductID + "'>";
                    sHtml += " <td class='text-left'>" + el.ProductName + setTooltipProduct(el.sTooltip) + "</td>";
                    sHtml += " <td class='text-center'>" + el.sUnit + "</td>";
                    sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' ProductID='" + el.ProductID + "' value='" + (el.Target != null ? CheckTextInput(el.Target) : "") + "' maxlength='20'></td>";
                    sHtml += bindColumnQ(false, "disabled", el.ProductID, "", el, true);
                    sHtml += "</tr>";
                })
                //Leve 2 cTotal ="Y" , cTotalAll = "N"
                $.each(Enumerable.From(lst.lstProduct).Where(function (w) { return w.nLevel == 2 }).ToArray(), function (i, el) {
                    sHtml += "<tr style='background-color:#e7bb5b' id='" + el.ProductID + "'>";
                    sHtml += " <td class='text-left'>&nbsp;&nbsp;" + el.ProductName + setTooltipProduct(el.sTooltip) + "</td>";
                    sHtml += " <td class='text-center'>" + el.sUnit + "</td>";
                    sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' ProductID='" + el.ProductID + "' value='" + (el.Target != null ? CheckTextInput(el.Target) : "") + "' " + (el.sOption == "0" ? "disabled" : "") + " maxlength='20'></td>";
                    sHtml += bindColumnQ(false, "disabled", el.ProductID, el.nHeaderID, el, true);
                    if (el.ProductID == "100") {
                        $.each(Enumerable.From(lst.lstProduct).Where(function (w) { return w.nHeaderID == el.ProductID && w.nLevel == 4 }).ToArray(), function (i3, el3) {
                            sHtml += "<tr class='tr_under_" + el.ProductID + " tr_under_ txtInput'>";
                            sHtml += " <td class='text-left'>";
                            if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                                sHtml += "        <div class='input-group'>";
                                sHtml += "          <span onclick='DelRow($(this))' class='input-group-addon' style='background-color:#ffdfda;cursor:pointer;' id='basic-addon1'><i class='fas fa-trash-alt' style='color: red;'></i></span>";
                                sHtml += "          <input type='text' class='form-control input-sm str' aria-describedby='basic-addon1' value='" + el3.ProductName + "'>";
                                sHtml += "        </div>";
                                sHtml += "</td>";
                                sHtml += " <td class='text-center'>" + bindDDLUnit(el3.sUnit, "") + "</td>";
                            } else {
                                if (nStatus != 0 || $hdfPRMS.val() == "1") {
                                    sHtml += "          <input type='text' class='form-control input-sm str' aria-describedby='basic-addon1' value='" + el3.ProductName + "' >";
                                    sHtml += "</td>";
                                    sHtml += " <td class='text-center'>" + bindDDLUnit(el3.sUnit, "disabled") + "</td>";
                                } else {
                                    if (!IsFullMonth) {
                                        sHtml += "        <div class='input-group'>";
                                        sHtml += "          <span onclick='DelRow($(this))' class='input-group-addon' style='background-color:#ffdfda;cursor:pointer;' id='basic-addon1'><i class='fas fa-trash-alt' style='color: red;'></i></span>";
                                        sHtml += "          <input type='text' class='form-control input-sm str' aria-describedby='basic-addon1' value='" + el3.ProductName + "'>";
                                        sHtml += "        </div>";
                                        sHtml += "</td>";
                                        sHtml += " <td class='text-center'>" + bindDDLUnit(el3.sUnit, "") + "</td>";
                                    } else {
                                        sHtml += "          <input type='text' class='form-control input-sm str' aria-describedby='basic-addon1' value='" + el3.ProductName + "' >";
                                    }
                                }
                            }
                            sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' value='" + el3.Target + "' maxlength='20'></td>";
                            sHtml += bindColumnQ(false, "", "", el.ProductID, el3, false);
                            sHtml += "</tr>";
                        })
                        if ($hdfPRMS.val() != "1") {
                            sHtml += "<tr class='tr_under_" + el.ProductID + " trAdd ' " + (el.sOption == "0" ? "style='display:none;'" : "") + ">";
                            sHtml += " <td class='text-left' colspan='3'><a class='btn btn-info btn-sm ' onclick='Addrow(\"" + el.ProductID + "\",\"\")'>ADD</a></td>";
                            sHtml += " <td class='text-left' colspan='12'></td>";
                            sHtml += "</tr>";
                        }
                    }
                    sHtml += "</tr>";
                    //Leve 3 cTotal ="Y" , cTotalAll = "N"
                    $.each(Enumerable.From(lst.lstProduct).Where(function (w) { return w.nHeaderID == el.ProductID && w.nLevel == 3 }).ToArray(), function (i2, el2) {
                        sHtml += "<tr class='cTotalYN tr_under_" + el.ProductID + "' id='" + el2.ProductID + "' " + (el.sOption == "0" ? "style='display:none;'" : "") + ">";
                        sHtml += " <td class='text-left'><a class='btn btn-sm btn-default' onclick='btnChevron(\"tr_under_" + el2.ProductID + "\")'><i id='tr_under_" + el2.ProductID + "' class='fas fa-chevron-" + (Enumerable.From(lst.lstProduct).Where(function (w) { return w.nHeaderID == el2.ProductID && w.nLevel == 4 }).ToArray().length > 0 ? "up" : "down") + " tr_under_" + el.ProductID + "' ></i></a>&nbsp;" + el2.ProductName + setTooltipProduct(el2.sTooltip) + "</td>";
                        sHtml += " <td class='text-center'>" + el2.sUnit + "</td>";
                        sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' maxlength='20' ProductID='" + el2.ProductID + "' value='" + (el2.Target != null ? CheckTextInput(el2.Target) : "") + "' ></td>";
                        sHtml += bindColumnQ(false, "disabled", el2.ProductID, el.ProductID, el2, true);
                        $.each(Enumerable.From(lst.lstProduct).Where(function (w) { return w.nHeaderID == el2.ProductID && w.nLevel == 4 }).ToArray(), function (i3, el3) {
                            sHtml += "<tr class='tr_under_" + el2.ProductID + " tr_under_" + el3.ProductID + " txtInput'>";
                            sHtml += " <td class='text-left'>";
                            if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                                sHtml += "        <div class='input-group'>";
                                sHtml += "          <span onclick='DelRow($(this))' class='input-group-addon' style='background-color:#ffdfda;cursor:pointer;' id='basic-addon1'><i class='fas fa-trash-alt' style='color: red;'></i></span>";
                                sHtml += "          <input type='text' class='form-control input-sm str' aria-describedby='basic-addon1' value='" + el3.ProductName + "'  maxlength='300'/>";
                                sHtml += "        </div>";
                                sHtml += "</td>";
                                sHtml += " <td class='text-center'>" + bindDDLUnit(el3.sUnit, "") + "</td>";
                            } else {
                                if (nStatus != 0 || $hdfPRMS.val() == "1") {
                                    sHtml += "          <input type='text' class='form-control input-sm str' aria-describedby='basic-addon1'  value='" + el3.ProductName + "' maxlength='300' />";
                                    sHtml += "</td>";
                                    sHtml += " <td class='text-center'>" + bindDDLUnit(el3.sUnit, "disabled") + "</td>";
                                } else {
                                    if (!IsFullMonth) {
                                        sHtml += "        <div class='input-group'>";
                                        sHtml += "          <span onclick='DelRow($(this))' class='input-group-addon' style='background-color:#ffdfda;cursor:pointer;' id='basic-addon1'><i class='fas fa-trash-alt' style='color: red;'></i></span>";
                                        sHtml += "          <input type='text' class='form-control input-sm str' aria-describedby='basic-addon1' value='" + el3.ProductName + "' maxlength='300'/>";
                                        sHtml += "        </div>";
                                        sHtml += "</td>";
                                        sHtml += " <td class='text-center'>" + bindDDLUnit(el3.sUnit, "") + "</td>";
                                    } else {
                                        sHtml += "          <input type='text' class='form-control input-sm str' aria-describedby='basic-addon1'  value='" + el3.ProductName + "' maxlength='300' />";
                                    }
                                }
                            }
                            sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' value='" + el3.Target + "' maxlength='20'></td>";
                            sHtml += bindColumnQ(false, "", "", el2.ProductID, el3, false);
                            sHtml += "</tr>";
                        })
                        sHtml += "</tr>";
                        if ($hdfPRMS.val() != "1") {
                            sHtml += "<tr class='tr_under_" + el2.ProductID + " tr_under_" + el.ProductID + " trAdd ' " + "style='" + (Enumerable.From(lst.lstProduct).Where(function (w) { return w.nHeaderID == el2.ProductID && w.nLevel == 4 }).ToArray().length > 0 ? "" : "display:none;") + "'>";
                            sHtml += " <td class='text-left' colspan='3'><a class='btn btn-info btn-sm ' onclick='Addrow(\"" + el2.ProductID + "\",\"" + el.ProductID + "\")'>ADD</a></td>";
                            sHtml += " <td class='text-left' colspan='12'></td>";
                            sHtml += "</tr>";
                        }
                    })
                })
                sHtml += "</tbody>";
            }
            $tbData.append(sHtml);
            $tbData.find("tbody tr[class*=txtInput]").change();
            lstProduct = Enumerable.From(lstProduct).Where(function (w) { return w.nLevel < 4 }).ToArray();
        }
        function bindColumnQ(IsHead, Disabled, PrdID, HeadID, el, IsHeadVal) {
            var sHtml = "";
            if (IsHead) {
                for (var i = 1 ; i <= 12; i++) {
                    sHtml += "<th class='text-center M_" + i + " QHead_" + getQrt(i) + "'><label>Q" + getQrt(i) + " : " + arrShortMonth[i - 1] + "</label></th>";
                }
            } else {
                if (el != "") {
                    var arrDataM = [el.M1, el.M2, el.M3, el.M4, el.M5, el.M6, el.M7, el.M8, el.M9, el.M10, el.M11, el.M12, ];
                    for (var i = 1 ; i <= 12; i++) {
                        var strForDisabled = "";
                        if (Disabled == "") {
                            if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                                if (lstStatus[i - 1].nStatusID != "2") {
                                    strForDisabled = "disabled";
                                }
                            } else {
                                if (lstStatus[i - 1].nStatusID != "0") {
                                    strForDisabled = "disabled";
                                }
                            }
                        } else {
                            strForDisabled = Disabled;
                        }
                        var nValOutput = "";
                        if (IsHeadVal) {
                            nValOutput = CheckTextOutput(arrDataM[i - 1])
                        } else {
                            nValOutput = CheckTextInput(arrDataM[i - 1])
                        }
                        sHtml += "<td class='text-center  M_" + i + " QHead_" + getQrt(i) + "'><input id='' PrdID='" + PrdID + "' month='" + i + "' under='" + HeadID + "' class='form-control input-sm " + (IsHeadVal ? "NoDis" : "") + "' value='" + nValOutput + "' " + strForDisabled + " maxlength='20' /></td>";
                    }
                } else {
                    for (var i = 1 ; i <= 12; i++) {
                        var strForDisabled = "";
                        if (Disabled == "") {
                            if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                                if (lstStatus[i - 1].nStatusID != "2") {
                                    strForDisabled = "disabled";
                                }
                            } else {
                                if (lstStatus[i - 1].nStatusID != "0") {
                                    strForDisabled = "disabled";
                                }
                            }
                        } else {
                            strForDisabled = Disabled;
                        }
                        sHtml += "<td class='text-center M_" + i + " QHead_" + getQrt(i) + "'><input id='' PrdID='" + PrdID + "' month='" + i + "' under='" + HeadID + "' class='form-control input-sm' value='' " + strForDisabled + " maxlength='20'/></td>";
                    }
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
        function btnChevron(id) {
            if ($("i[id$=" + id + "]").hasClass("fas fa-chevron-down")) {
                $("." + id).show();
                $("i[id$=" + id + "]").removeClass("fas fa-chevron-down")
                $("i[id$=" + id + "]").addClass("fas fa-chevron-up")
            } else {
                $("." + id).hide();
                $("i[id$=" + id + "]").removeClass("fas fa-chevron-up")
                $("i[id$=" + id + "]").addClass("fas fa-chevron-down")
            }
        }
        function Addrow(rowID, rowHeadID) {
            var cur = $(this);
            var sDefaultName = "";
            var data = Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == rowID });
            if (data != null) {
                sDefaultName = data.ProductName;
            }
            var sHtml = "";
            sHtml += "<tr class='tr_under_" + rowID + " tr_under_" + rowHeadID + " txtInput'>";
            sHtml += " <td class='text-left'>";
            sHtml += "        <div class='input-group'>";
            sHtml += "          <span onclick='DelRow($(this))' class='input-group-addon' style='background-color:#ffdfda;cursor:pointer;' id='basic-addon1'><i class='fas fa-trash-alt' style='color: red;'></i></span>";
            sHtml += "          <input type='text' class='form-control input-sm str' value ='" + sDefaultName + "' aria-describedby='basic-addon1' maxlength='300' />";
            sHtml += "        </div>";
            sHtml += "</td>";
            sHtml += " <td class='text-center'>" + bindDDLUnit("", "") + "</td>";
            sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' value='' maxlength='20'></td>";
            sHtml += bindColumnQ(false, "", "", rowID, "", "");
            sHtml += "</tr>";
            $tbData.find("tr[class*=tr_under_" + rowID + "]:last").before(sHtml);
            SetTootip();
            setPRMS();
            $tbData.tableHeadFixer({ "left": 1 }, { "head": true })
            CheckboxQuarterChanged();
        }
        function DelRow(cur) {
            DialogConfirm(DialogHeader.Comfirm, DialogMsg.ConfirmDel, function () {
                cur.parent().parent().parent().find("td[class*=M_] input").val("").change();
                cur.parent().parent().parent().remove();
                HideLoadding();
            }, function () { HideLoadding(); });
        }
        function bindDDLUnit(sUnitID, sDisable) {
            var sHtml = "";
            //sHtml += "<select class='form-control input-sm' " + ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") ? "" : sDisable) + " onchange='ddlChange($(this))'>";
            //$.each(lstUnit, function (i, el) {
            //    sHtml += "<option value='" + el.nUnitID + "' " + (sUnitID == el.nUnitID ? "selected" : "") + ">" + el.sUnitName + "</option>";
            //})
            //sHtml += "</select>";
            sHtml += "m<sup>3</sup>";
            return sHtml;
        }
        function SetFormatNumberToarr(nNumber, nDecimal) {
            if ($.isNumeric(nNumber)) {
                nNumber = +nNumber;
                if ($.isNumeric(nDecimal))
                    return nNumber.toFixed(nDecimal);
                else
                    return nNumber;
            }
            else {
                return "";
            }
        }
        function setDataArr(itemPrd, Month, nValtotal) {
            var nVal = nValtotal + "";
            switch (Month) {
                case "1":
                    itemPrd.M1 = nVal;
                    break;
                case "2":
                    itemPrd.M2 = nVal;
                    break;
                case "3":
                    itemPrd.M3 = nVal;
                    break;
                case "4":
                    itemPrd.M4 = nVal;
                    break;
                case "5":
                    itemPrd.M5 = nVal;
                    break;
                case "6":
                    itemPrd.M6 = nVal;
                    break;
                case "7":
                    itemPrd.M7 = nVal;
                    break;
                case "8":
                    itemPrd.M8 = nVal;
                    break;
                case "9":
                    itemPrd.M9 = nVal;
                    break;
                case "10":
                    itemPrd.M10 = nVal;
                    break;
                case "11":
                    itemPrd.M11 = nVal;
                    break;
                case "12":
                    itemPrd.M12 = nVal;
                    break;
            }
        }
        function getDataInArr(itemPrd, nMonth) {
            var nVal = "";
            var Month = nMonth;
            switch (Month + "") {
                case "1":
                    nVal = itemPrd.M1;
                    break;
                case "2":
                    nVal = itemPrd.M2;
                    break;
                case "3":
                    nVal = itemPrd.M3;
                    break;
                case "4":
                    nVal = itemPrd.M4;
                    break;
                case "5":
                    nVal = itemPrd.M5;
                    break;
                case "6":
                    nVal = itemPrd.M6;
                    break;
                case "7":
                    nVal = itemPrd.M7;
                    break;
                case "8":
                    nVal = itemPrd.M8;
                    break;
                case "9":
                    nVal = itemPrd.M9;
                    break;
                case "10":
                    nVal = itemPrd.M10;
                    break;
                case "11":
                    nVal = itemPrd.M11;
                    break;
                case "12":
                    nVal = itemPrd.M12;
                    break;
            }
            return nVal;
        }
        function HeaderTotal(lstData, nMonth) {
            var nTotal = "";
            $.each(lstData, function (i, el) {
                var Month = nMonth
                var nVal = getDataInArr(el, Month);
                if (nTotal == "") {
                    if (nVal != "") {
                        nTotal += +nVal;
                    }
                } else {
                    if (nVal != "") {
                        nTotal = +nTotal + (+nVal);
                    }
                }
            })
            return nTotal;
        }

        /////////////*********** SAVE ************/////////////
        function SaveData(nStatus) {
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
            var IsPass = true;
            var sMsg = "";
            var arrIndForCheck = ["93", "94", "95", "96", "98", "99", "100", "101", "102"];
            if (nStatus == "0" || nStatus == 9999) { // Draft
                //$.each(lstStatus, function (i, el) {
                //    if (el.nStatus != 0) {
                //        arrMonth.push(el.nMonth);
                //    }
                //})
                IsPass = false;
                $.each($tbData.find("tbody tr[id$=91] td[class*=M_] input"), function (i, el) {
                    el = $(el);
                    if (el.val().trim() != "") {
                        IsPass = true;
                    }
                })
                if (!IsPass) {
                    DialogWarning(DialogHeader.Warning, "Invalid Data");
                    return false;
                }
            } else if (nStatus == 24 || nStatus == 2) {
                IsPass = true;
            }
            else {
                IsPass = true;
                if ($tbData.find("tbody tr[class*=txtInput]").length > 0) {
                    $.each(arrIndForCheck, function (index, IndID) {
                        if ($tbData.find("tbody tr[class*=tr_under_" + IndID + "][class*=txtInput]").length > 0) {
                            var IsSubPass = true;
                            var SubMsg = "";
                            $.each($tbData.find("tbody tr[class*=tr_under_" + IndID + "][class*=txtInput]"), function (i, el) {
                                el = $(el);
                                if (el.find("input.str").val() == "") {
                                    IsSubPass = false;
                                }
                            })
                            var IsMonthPass = true;
                            var MonthMsg = "";
                            var sMsgHeader = "";
                            var IsHeadPass = true;
                            $.each(arrMonth, function (i, el) {
                                if (getDataInArr(Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == IndID }), el).trim() == "") {
                                    IsHeadPass = false;
                                    //sMsgHeader += "<br/>&nbsp;- " + arrFullMonth[+el - 1] + " invalid data";
                                }
                                $.each($tbData.find("tbody tr[class*=tr_under_" + IndID + "][class*=txtInput]").find("td[class*=M_]:eq(" + (+el - 1) + ") input:not(:disabled)"), function (i2, el2) {
                                    el2 = $(el2);
                                    if (el2.val() == "") {
                                        IsMonthPass = false;
                                    }
                                })
                                if (!IsMonthPass || !IsHeadPass) {
                                    MonthMsg += "<br/>&nbsp;- Please specify " + arrFullMonth[+el - 1];
                                }
                            })
                            if (!IsSubPass || !IsMonthPass || !IsHeadPass) {
                                sMsg += "</br><br/>&bull; " + Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == IndID }).ProductName;
                            }
                            if (!IsSubPass) {
                                sMsg += "<br/>&nbsp;- Please specify indicator";
                                IsPass = false;
                            }
                            if (!IsMonthPass || !IsHeadPass) {
                                sMsg += MonthMsg;
                                IsPass = false;
                            }
                        }
                        //}
                    })
                } else {
                    IsPass = false;
                    DialogWarning(DialogHeader.Warning, "Plsease specify data");
                    return false;
                }
                //if (GetValTextBox("hdfRemarkTMU") != "") {
                if (GetValTextArea("txtRemarkWW").trim() == "") {
                    IsPass = false;
                    sMsg += "</br></br>&bull; Total Water Withdrawal";
                    sMsg += "</br>&nbsp;- Please specify Remark ";
                }
                //}
                //if (GetValTextBox("hdfRemarkDMU") != "") {
                if (GetValTextArea("txtRemarkWR").trim() == "") {
                    IsPass = false;
                    sMsg += "</br></br>&bull; Total Water Recycled & Reused";
                    sMsg += "</br>&nbsp;- Please specify Remark ";
                }
                //}
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
                        sMsg += "<br/></br>&bull; File";
                        sMsg += "<br/>&nbsp;- Please specify file description";
                    }
                }

            }
            if (IsPass) {
                lstProduct = Enumerable.From(lstProduct).Where(function (w) { return w.nLevel < 4 }).ToArray();
                var tableData = $tbData.find("tbody tr").not("tr[class*=trAdd]");
                $.each(tableData, function (i, el) {
                    el = $(el);
                    var ID = el.attr("id");
                    if (!el.hasClass("txtInput")) {
                        var itemData = Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.ProductID == ID });
                        itemData.Target = el.find("input[productid$=" + ID + "]").val().replace(/,/g, '');
                    } else {
                        var HeadID = el.attr("class").split(" ")[0].split("_")[2];
                        var ProductName = el.find("input.str").val();
                        var sTarget = el.find("td.cTarget input").val();
                        lstProduct.push({
                            nHeaderID: +HeadID,
                            nLevel: 4,
                            sOption: "",
                            ProductID: null,
                            ProductName: ProductName,
                            cTotal: "",
                            cTotalAll: "",
                            Target: sTarget.replace(/,/g, ''),
                            M1: el.find("input[month$=1]").val().replace(/,/g, ''),
                            M2: el.find("input[month$=2]").val().replace(/,/g, ''),
                            M3: el.find("input[month$=3]").val().replace(/,/g, ''),
                            M4: el.find("input[month$=4]").val().replace(/,/g, ''),
                            M5: el.find("input[month$=5]").val().replace(/,/g, ''),
                            M6: el.find("input[month$=6]").val().replace(/,/g, ''),
                            M7: el.find("input[month$=7]").val().replace(/,/g, ''),
                            M8: el.find("input[month$=8]").val().replace(/,/g, ''),
                            M9: el.find("input[month$=9]").val().replace(/,/g, ''),
                            M10: el.find("input[month$=10]").val().replace(/,/g, ''),
                            M11: el.find("input[month$=11]").val().replace(/,/g, ''),
                            M12: el.find("input[month$=12]").val().replace(/,/g, ''),
                        });
                    }
                })
                $.each($dataFileOther, function (e, n) {
                    var ID = n.ID;
                    var sVal = $("input[id$=txtFile_" + ID + "]").val();
                    n.sDescription = sVal;
                });
                var arrData = {
                    sRemarkTotalWithdrawal: GetValTextArea("txtRemarkWW"),
                    sRemarkTotalRecycled: GetValTextArea("txtRemarkWR"),
                    nIndicatorID: $ddlIndicator.val(),
                    nOperationID: $ddlOperationType.val(),
                    nFacilityID: $ddlFacility.val(),
                    sYear: $ddlYear.val(),
                    lstProduct: lstProduct,
                    lstFile: $dataFileOther,
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
                                HideLoadding();
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
                                    //DialogSuccess(DialogHeader.Info, DialogMsg.SaveComplete);
                                } else {
                                    HideLoadding();
                                    DialogWarning(DialogHeader.Warning, response.d.Msg);
                                }
                            }, function () {
                                var lstPrdDeviate = Enumerable.From(lstProduct).Where(function (w) { return w.ProductID == 91 }).ToArray();
                                Deviate(lstPrdDeviate, nStatus);
                            }, { arrData: arrData })
                            //var lstPrdDeviate = Enumerable.From(lstProduct).Where(function (w) { return w.ProductID == 34 || w.ProductID == 37 || w.ProductID == 41 }).ToArray();
                            //Deviate(lstPrdDeviate, nStatus);
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
                                HideLoadding();
                                DialogWarning(DialogHeader.Warning, response.d.Msg);
                            }
                        }, function () { HideLoadding(); }, { arrData: arrData })
                    }
                }

            } else {
                $("div#divPopContentValidate").empty();
                $("div#divPopContentValidate").append(sMsg.substr(10));
                $("#popValidate").modal('toggle');
            }
        }

        //////////************** FILE *****************\\\\\\\\\
        var $dataFileOther = [];

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
                    $("td", row).eq(2).html('<input id="txtFile_' + item.ID + '" class="form-control " />');
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
        function setTooltipProduct(sTooltip) {
            var sHtml = '';
            if (sTooltip != "" && sTooltip != null) {//<i class="far fa-question-circle"></i>
                sHtml += '&nbsp;<i class="fas fa-question-circle" title="' + sTooltip + '" style="font-size:16px"></i>';
            }
            return sHtml;
        }
    </script>
</asp:Content>

