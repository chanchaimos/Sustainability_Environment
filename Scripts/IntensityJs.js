var arrCt = [];
var arrrdoQ = [];
var arrProduct = [];
var arrByProduct = [];
var arrDetail = [];
var nRowAll = 1;
var nRowAllHead = 1;
var nIDAll = 1;
var arrM = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
var $dvContent = $("div#dvContent");
var $dvRemark = $("div#dvRemark");
var $dvFile = $("div#dvFile");
var $dataFileOther = [];
var arrMonthName = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
var $hdfPermission = $("input[id$=hdfPermission]");
$tbTotal = $("table#tbTotal");
$tbProduct = $("table#tbProduct");
$tbByProduct = $("table#tbByProduct");
var nProductAll = 1;
var nProductAllHead = 1;
var $hdfsRoleID = $("input[id$=hdfsRoleID]");
$(function () {
    $("div#divExport").hide();
    nStatus = -1;
    CheckEventButton();
    EventFrom();
    SetFileUploadOther();
    $("button#btnExport").click(function () {
        AjaxCallWebMethod('CreateLinkExport', function (data) {

            window.location = data.d.sPath;
        }, function () {
            UnblockUI();
        }, { 'sFacility': $ddlFacility.val(), 'sYear': $ddlYear.val() });

    });
});

function LoadData() {
    arrCt = [];
    BlockUI();
    $dvContent.empty();
    $dvRemark.empty();
    $dvFile.hide();
    $("div#divExport").hide();
    $btnSubmit.hide();
    $btnSaveDraft.hide();
    arrMonthRecall = [];
    AjaxCallWebMethod('LoadData', function (data) {
        nStatus = 0;
        if ($ddlFacility.val() != "" && $ddlYear != "") {

            arrCt = data.d;
            $dvFile.show();
            $dataFileOther = arrCt.lstDataFile;
            BindTableFileOther();
            lstStatus = arrCt.lstwf;
            $("div#divExport").show();
            $hdfPRMS.val(arrCt.hdfPRMS);
        }

        CreateTable()


    }, function () {
        UnblockUI();

        CheckboxQuarterChanged();
        CheckEventButton();
        if ($hdfIsAdmin.val() == "Y" || $hdfRole.val() == "4" && $hdfsStatus.val() == "") {
            $("input[id^='txtDetail_']").prop('disabled', false);
        }

        
    }, { 'sFacility': $ddlFacility.val(), 'sYear': $ddlYear.val() });

}
//#region CreateTable


//#region CreateHead
function CreateHead() {
    var tr = "", td1 = "", td2 = "", td3 = "", tdHead = "";
    td1 = "<th class='text-center' style='vertical-align: middle; width:" + nWidthIndicator + "px;'>Indicator</th>";
    td2 = "<th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'>Unit</th>";
    td3 = "<th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'>Target</th>";
    for (var i = 1; i <= 12 ; i++) {
        tdHead += "<th class='text-center QHead_" + getQrt(i) + "' style='width:" + nWidthTD + "px;'>Q" + getQrt(i) + " : " + arrM[i - 1] + "</th>";
    }
    tr = "<thead><tr>" + td1 + td2 + td3 + tdHead + "</tr></thead>";
    return tr;
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
//#endregion

//#region CreateTotal
function CreateRowTotal(ProductName, nTb, nRow, nProductID, nUnder, sCC, sUnit) {
    //nTb, nRow, nProductID, nUnder, IsNew, IsHead
    var tr = "", td1 = "", td2 = "", td3 = "", tdTotal = "", disabled = "", cDisabled = "";
    td1 = "<td class='text-left'>" + ProductName + "</td>";
    td2 = "<td class='text-center'>" + sUnit + "</td>";
    td3 = "<td class='text-right cTarget'><input tyle='text' id='txtTargetDetail_" + nTb + "' name='txtTargetDetail_" + nTb + "' data-nProductID='" + nProductID + "' data-nUnder = '" + nProductID + "'  class='form-control input-sm text-right' maxlength='20'></td>";
    disabled = '';
    if ($hdfPRMS.val() == "1" && ($hdfsRoleID.val() != "3" && $hdfsRoleID.val() != "4")) {
        disabled = 'disabled="disabled"';
    }
    if (nProductID == 88) {
        td3 = "<td class='text-right cTarget'><input tyle='text' id='txtTargetDetail_" + nTb + "' name='txtTargetDetail_" + nTb + "' data-nProductID='" + nProductID + "' data-nUnder = '" + nProductID + "'  class='form-control input-sm text-right' maxlength='200'  " + disabled + "></td>";
    }
    else if (nProductID == 89) {

        td3 = "<td class='text-right cTarget'><input tyle='text' id='txtTargetDetail_" + nTb + "_1' name='txtTargetDetail_" + nTb + "_1' data-nProductID='" + nProductID + "' data-nUnder = '" + nProductID + "'  class='form-control input-sm text-right' maxlength='200'  " + disabled + "></td>";
    }
    else if (nProductID == 90) {

        td3 = "<td class='text-right cTarget'><input tyle='text' id='txtTargetDetail_" + nTb + "_2' name='txtTargetDetail_" + nTb + "_2' data-nProductID='" + nProductID + "' data-nUnder = '" + nProductID + "'  class='form-control input-sm text-right' maxlength='200'  " + disabled + "></td>";
    }
    if (nProductID != 89 && nProductID != 90 && nProductID != 79 && nProductID != 87) {
        disabled = 'disabled="disabled"';
        for (var i = 1; i <= 12 ; i++) {
            tdTotal += "<td class='text-right QHead_" + getQrt(i) + "'><input tyle='text' id='txtTotal_" + nTb + "_M" + i + "'  name='txtTotal_" + nTb + "_M" + i + "'  data-nProductID='" + nProductID + "'  class='form-control input-sm text-right NoDis' " + disabled + " maxlength='20'></td>";;
        }

    }
    else {
        for (var i = 1; i <= 12 ; i++) {
            var nStatusWF = 0;
            cDisabled = "";
            var wf = Enumerable.From(arrCt.lstwf).Where(function (w) { return w.nMonth == i }).FirstOrDefault();
            if (wf != null) {
                nStatusWF = wf.nStatusID;
            }
            if (nStatusWF > 0) {
                cDisabled = "disabled"
            }
            tdTotal += "<td class='text-right QHead_" + getQrt(i) + "'><input tyle='text' id='txtDetail_" + nTb + "_" + nRow + "_M" + i + "'  name='txtDetail_" + nTb + "_" + nRow + "_M" + i + "'  data-nProductID='" + nProductID + "' data-nUnder = '" + nUnder + "' data-month='" + i + "' class='form-control input-sm text-right' " + cDisabled + " maxlength='20'></td>";;
        }
    }


    tr = "<tr id='trTotal_" + nTb + "' class='" + sCC + "'>" + td1 + td2 + td3 + tdTotal + "</tr>";
    return tr;
}

//#endregion

//#region Detail
function CreateRowDetail(nTb, nRow, nProductID, nUnder, IsNew, IsHead, sUnit) {
    var tr = "", td1 = "", td2 = "", td3 = "", tdDetail = "", cDisabled = "";
    var nStatusWF = 0;
    cDisabled = "";
    var wf = Enumerable.From(arrCt.lstwf).Where(function (w) { return w.nStatusID > 0 }).FirstOrDefault();
    if (((wf != null && IsNew == false) || $hdfPRMS.val() == "1") && ($hdfsRoleID.val() != "3" && $hdfsRoleID.val() != "4")) {

        td1 = "<td class='text-center'><input tyle='text' id='txtIndicatorDetail_" + nTb + "_" + nRow + "' name='txtIndicatorDetail_" + nTb + "_" + nRow + "' data-nProductID='" + nProductID + "' data-nUnder = '" + nUnder + "' class='form-control sm' maxlength='200'/></td>";
    }
    else {
        td1 = "<td class='text-center'><div class='input-group'><div class='input-group-addon' style='background-color: #ffdfda;cursor: pointer;' onclick='DeleteDetail(" + nUnder + "," + nProductID + "," + nRow + "," + nTb + ")'><i class='fas fa-trash-alt' style='color: red;'></i></div><input tyle='text' id='txtIndicatorDetail_" + nTb + "_" + nRow + "' name='txtIndicatorDetail_" + nTb + "_" + nRow + "' data-nProductID='" + nProductID + "' data-nUnder = '" + nUnder + "' class='form-control sm' maxlength='200'></td>";
    }

    td2 = "<td class='text-center'>" + sUnit + "</td>";
    td3 = "<td class='text-right cTarget'><input tyle='text' id='txtTargetDetail_" + nTb + "_" + nRow + "' name='txtTargetDetail_" + nTb + "_" + nRow + "' data-nProductID='" + nProductID + "' data-nUnder = '" + nUnder + "' class='form-control input-sm text-right' maxlength='20'></td>";
    for (var i = 1; i <= 12 ; i++) {
        var nStatusWF = 0;
        cDisabled = "";
        
        var wf = Enumerable.From(arrCt.lstwf).Where(function (w) { return w.nMonth == i }).FirstOrDefault();
        if (wf != null) {
            nStatusWF = wf.nStatusID;
        }
        if (nStatusWF > 0 && $hdfsRoleID.val() != "4") {
            cDisabled = "disabled";
        }
        if (nStatusWF == 2) {
            cDisabled = "";
        }
        if ($hdfsStatus.val() != "" && nStatusWF == 0 && $hdfsRoleID.val() != "4")
        {
            cDisabled = "disabled";
        }
        if ($hdfPRMS.val() == "1" && ($hdfsRoleID.val() != "3" && $hdfsRoleID.val() != "4")) {
            
            cDisabled = "disabled";
        }
        tdDetail += "<td class='text-right QHead_" + getQrt(i) + "'><input tyle='text' id='txtDetail_" + nTb + "_" + nRow + "_M" + i + "'  name='txtDetail_" + nTb + "_" + nRow + "_M" + i + "'  data-nProductID='" + nProductID + "' data-nUnder = '" + nUnder + "' data-month='" + i + "' class='form-control input-sm text-right' " + cDisabled + " maxlength='20'></td>";;
    }
    tr = "<tr id='trDetail_" + nTb + "_" + nRow + "'>" + td1 + td2 + td3 + tdDetail + "</tr>";
    return tr;
};
//#endregion
//#endregion 


//#region Event

function AddDetail(UnderProductID, tb, nTb) {
    var nID = (UnderProductID + 1), nRow = 1, nAll = 1;

    if (arrCt.lstIn != null) {
        var lst = Enumerable.From(arrCt.lstIn).Where(function (w) { return w.ProductID == UnderProductID }).FirstOrDefault();
        if (lst != null) {
            var sUnit = lst.sUnit;
            if (Enumerable.From(lst.lstarrDetail).Count() > 0) {
                nID = Enumerable.From(lst.lstarrDetail).Max(function (m) { return m.nProductID }) + 1;
                nRow = Enumerable.From(lst.lstarrDetail).Count() + 1;
                nAll = Enumerable.From(lst.lstarrDetail).Where(function (w) { return w.IsActive == true }).Count() + 1;
            }
            Enumerable.From(arrCt.lstIn).Where(function (w) { return w.ProductID == UnderProductID }).ForEach(function (f) {
                var FormID = f.FormID;
                f.lstarrDetail.push({
                    UnderProductID: UnderProductID,
                    nProductID: nID,
                    sIndicator: "",
                    sTarget: "",
                    cTotal: "",
                    cTotalAll: "",
                    FormID: FormID,
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
                    IsActive: true,

                });
                return f;
            })

        }
    }
    $("input#txtContextAdd_" + nTb).val(nAll);
    var tr = CreateRowDetail(nTb, nRow, nID, UnderProductID, true, false, sUnit);
    $("table#" + tb + " tr:last").after(tr);
    CheckboxQuarterChanged();
}
function DeleteDetail(nUnder, nProductID, nRow, nTb) {

    DialogConfirmCloseButton(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
        if (arrCt.lstIn != null) {
            var nAll = "", nAllTotal = "";
            var i = 1;
            Enumerable.From(arrCt.lstIn).ForEach(function (f) {
                if (f.ProductID == nUnder) {
                    if (Enumerable.From(f.lstarrDetail).Where(function (w) { return w.IsActive == true }).Count() > 0) {

                        Enumerable.From(f.lstarrDetail).Where(function (w2) { return w2.IsActive == true && w2.nProductID == nProductID }).ForEach(function (f2) {
                            f2.IsActive = false;
                            $("tr#trDetail_" + nTb + "_" + nRow).remove();

                            return f2;
                        });
                        nAll = Enumerable.From(f.lstarrDetail).Where(function (w) { return w.IsActive == true }).Count();
                    }
                    if (nAll == 0) {
                        nAll = "";
                        for (var k = 1; k <= 12 ; k++) {
                            $("input#txtTotal_" + nTb + "_M" + k).val("");
                            switch (k) {
                                case 1: f.M1 = nAll;
                                    break;
                                case 2: f.M2 = nAll;
                                    break;
                                case 3: f.M3 = nAll;
                                    break;
                                case 4: f.M4 = nAll;
                                    break
                                case 5: f.M5 = nAll;
                                    break;
                                case 6: f.M6 = nAll;
                                    break;
                                case 7: f.M7 = nAll;
                                    break;
                                case 8: f.M8 = nAll;
                                    break;
                                case 9: f.M9 = nAll;
                                    break;
                                case 10: f.M10 = nAll;
                                    break;
                                case 11: f.M11 = nAll;
                                    break;
                                case 12: f.M12 = nAll;
                                    break;
                            }

                        }
                    }

                    $("input#txtContextAdd_" + nTb).val(nAll);
                }
                nAllTotal = (Enumerable.From(f.lstarrDetail).Where(function (w) { return w.IsActive == true }).Count() + parseInt(nAllTotal));

                return f;
            });
            if (nAllTotal == 0) {

                nAllTotal = "";
                for (var k = 1; k <= 12 ; k++) {
                    $("input#txtTotal_" + nRowAll + "_M" + k).val("");
                    Enumerable.From(arrCt.lstIn).Where(function (w) { return w.ProductID == nProductAll }).ForEach(function (f) {
                        switch (nM) {
                            case 1: f.M1 = nAllTotal;
                                break;
                            case 2: f.M2 = nAllTotal;
                                break;
                            case 3: f.M3 = nAllTotal;
                                break;
                            case 4: f.M4 = nAllTotal;
                                break
                            case 5: f.M5 = nAllTotal;
                                break;
                            case 6: f.M6 = nAllTotal;
                                break;
                            case 7: f.M7 = nAllTotal;
                                break;
                            case 8: f.M8 = nAllTotal;
                                break;
                            case 9: f.M9 = nAllTotal;
                                break;
                            case 10: f.M10 = nAllTotal;
                                break;
                            case 11: f.M11 = nAllTotal;
                                break;
                            case 12: f.M12 = nAllTotal;
                                break;
                        }
                        return f;
                        //#endregion

                    });
                }
            }
            $("input[id^='txtDetail_']").blur()
        }
        UnblockUI();
    })
}
//#endregion
function SaveData(sMode) {
    var IsPass = CheckDataSave(sMode);
    if (IsPass) {
        $.each($dataFileOther, function (e, n) {
            var ID = n.ID;
            var sVal = $("input[id$=txtFile_" + ID + "]").val();
            n.sDescription = sVal;
        });
        if (sMode != 1) {
            Senttodb(sMode);
        }
        else {
            if (!IsDeviatePass) {
                SenttodbBeforeDeviate(sMode)
            }
            else {
                SenttodbNotCon(sMode);
            }
        }

    }

}
function SenttodbNotCon(sMode) {

    var TRetunrLoadData = [];
    TRetunrLoadData = {
        lstIn: arrCt.lstIn,
        nStatusWF: sMode,
        lstDataFile: $dataFileOther,
        lstMonth: arrMonth,
        sComment: $("textarea#txtsComment").val()
    };
    AjaxCallWebMethod("SaveData", function (data) {
        HideLoadding();

        if (data.d.sStatus == SysProcess.SessionExpired) {
            PopupLogin();
        } else if (data.d.sStatus == SysProcess.Success) {
             var sMgSS = DialogMsg.SaveComplete;
                    switch (+sMode) {
                        case 0: sMgSS = DialogMsg.SaveDraftComplete;
                            break;
                        case 1: sMgSS = DialogMsg.SubmitComplete;
                            break;
                        case 24: sMgSS = DialogMsg.RecallComplete;
                            break;
                        case 9999: sMgSS = DialogMsg.SaveComplete;
                            break;
                        case 2: sMgSS = DialogMsg.RequestComplete;
                            break;
                        case 27: sMgSS = DialogMsg.ApproveComplete;
                            break;
                    }
                    DialogSuccess(DialogHeader.Success, sMgSS);
            LoadDataCheckddl()

        } else {
            DialogWarning(DialogHeader.Warning, data.d.sMsg);
        }
    }, "", { lst: TRetunrLoadData, sFacility: $ddlFacility.val(), sYear: $ddlYear.val() });
}
function Senttodb(sMode) {



    var sMg = DialogMsg.ConfirmSave;
    switch (+sMode)
    {
        case 0: sMg = DialogMsg.ConfirmSaveDraft;
            break;
        case 1: sMg = DialogMsg.ConfirmSubmit;
            break;
        case 24: sMg = DialogMsg.ConfirmRecall;
            break;
        case 9999: sMg = DialogMsg.ConfirmSave;
            break;
        case 2: sMg = DialogMsg.ConfirmRequest;
            break;
        case 27: sMg = DialogMsg.ConfirmApprove;
            break;
    }

    var TRetunrLoadData = [];
    TRetunrLoadData = {
        lstIn: arrCt.lstIn,
        nStatusWF: sMode,
        lstDataFile: $dataFileOther,
        lstMonth: arrMonth,
        sComment :$("textarea#txtsComment").val()
    }; 
    DialogConfirm(DialogHeader.Confirm, sMg, function () {
        AjaxCallWebMethod("SaveData", function (data) {
            HideLoadding();

            if (data.d.sStatus == SysProcess.SessionExpired) {
                PopupLogin();
            } else if (data.d.sStatus == SysProcess.Success) {
                if (sMode != 27)
                {
                    var sMgSS = DialogMsg.SaveComplete;
                    switch (+sMode) {
                        case 0: sMgSS = DialogMsg.SaveDraftComplete;
                            break;
                        case 1: sMgSS = DialogMsg.SubmitComplete;
                            break;
                        case 24: sMgSS = DialogMsg.RecallComplete;
                            break;
                        case 9999: sMgSS = DialogMsg.SaveComplete;
                            break;
                        case 2: sMgSS = DialogMsg.RequestComplete;
                            break;
                        case 27: sMgSS = DialogMsg.ApproveComplete;
                            break;
                    }
                    DialogSuccess(DialogHeader.Success, sMgSS);

                    LoadDataCheckddl()
                }
                else
                {
                    DialogSuccessRedirect(DialogHeader.Success, DialogMsg.SaveComplete, "epi_mytask.aspx");
                }


            } else {
                DialogWarning(DialogHeader.Warning, data.d.sMsg);
            }
        }, "", { lst: TRetunrLoadData, sFacility: $ddlFacility.val(), sYear: $ddlYear.val() });
    }, "");

}
function SenttodbBeforeDeviate(sMode) {

    var TRetunrLoadData = [];
    TRetunrLoadData = {
        lstIn: arrCt.lstIn,
        nStatusWF: 0,
        lstDataFile: $dataFileOther,
        lstMonth: arrMonth,
        sComment: $("textarea#txtsComment").val()
    };
    var sMg = DialogMsg.ConfirmSave;
    switch (+sMode) {
        case 0: sMg = DialogMsg.ConfirmSaveDraft;
            break;
        case 1: sMg = DialogMsg.ConfirmSubmit;
            break;
        case 24: sMg = DialogMsg.ConfirmRecall;
            break;
        case 9999: sMg = DialogMsg.ConfirmSave;
            break;
        case 2: sMg = DialogMsg.ConfirmRequest;
            break;
        case 27: sMg = DialogMsg.ConfirmApprove;
            break;
    }
    DialogConfirm(DialogHeader.Confirm,sMg, function () {
        AjaxCallWebMethod("SaveData", function (data) {
            HideLoadding();
            $dataFileOther
            debugger
            Enumerable.From($dataFileOther).Where(function (w) { return w.sDelete == "N"}).ForEach(function (f) {
                f.IsNewFile = false;
                return f;
            })
            if (data.d.sStatus == SysProcess.SessionExpired) {
                PopupLogin();
            } else if (data.d.sStatus == SysProcess.Success) {

                var lstPrdDeviate = Enumerable.From(arrCt.lstIn).Where(function (w) { return w.ProductID == 88 || w.ProductID == 239 || w.ProductID == 84 || w.ProductID == 85 || w.ProductID == 79 || w.ProductID == 87 }).ToArray();
                Deviate(lstPrdDeviate, sMode);

            } else {
                DialogWarning(DialogHeader.Warning, data.d.sMsg);
            }
        }, "", { lst: TRetunrLoadData, sFacility: $ddlFacility.val(), sYear: $ddlYear.val() });
    }, "");

}
function SumValue(nTotal, nVal) {
    var result = "";

    nVal = nVal != "" ? nVal.replace(/,/g, '') : "";
    var CheckTotal = nTotal != "" ? nTotal : 0;

    if (IsNumberic(CheckTotal)) {
        result = IsNumberic(nVal) ? (parseFloat(CheckTotal) + parseFloat(nVal)) : nTotal;
    }
    else {
        result = "";
    }
    if (IsNumberic(result)) {
        if (result > 0) {
            result = (Math.round((result) * 10000000000) / 10000000000);
        }
    }
    return result + "";
}
//#region File
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
            data: { funcname: "UPLOAD", savetopath: 'intensity_2/Temp/', savetoname: '' },
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
            $("td", row).eq(2).html('<input id="txtFile_' + item.ID + '" class="form-control"type="text" maxlength="1000"/>');
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
//#endregion