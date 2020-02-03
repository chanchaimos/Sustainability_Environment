
$(function () {
    SetInputMask();
    SetTootip();
    SetICheck();
});

function LoaddinProcess() {
    BlockUI();
    //$("#loading-div-background").show();
}

function HideLoadding() {
    UnblockUI();
    //$("#loading-div-background").hide();
}

function MiniLoaddingProcess(divID) {
    $("#" + divID).addClass("overlay").append('<i class="fa fa-refresh fa-spin"></i>');
}

function HideMiniLoadding(divID) {
    $("#" + divID).removeClass("overlay").html("");
}

function PopupLogin() {
    HideLoadding();
    DialogAlertLogin(DialogHeader.Info, DialogMsg.Login, "AD/loginAD.aspx");
}

function FuncLogout() {
    DialogConfirmInfo(DialogHeader.Info, DialogMsg.ConfirmLogout, function () { window.location = "logout.aspx"; }, "");
}

/****Function Master Page****/
function ChangeRole() {
    LoaddinProcess();
    $.ajax({
        dataType: "html",
        type: AjaxCall.type,
        url: AshxSysFunc.url,
        data: { funcName: AshxSysFunc.FuncChangeRole },
        success: function (response) {
            var ItemData = JSON.parse(response);
            if (ItemData.Status == SysProcess.SessionExpired) {
                HideLoadding();
                PopupLogin();
            }
            else {
                HideLoadding();
                var ItemRole = ItemData.lstData;
                var divData = '<table class="table dataTable table-bordered table-responsive table-hover">';
                divData += '<thead><tr><th class="dt-head-center">Role Name</th></tr></thead>';
                divData += '<tbody>'
                var nRow = 1;
                for (var i = 0; i < ItemRole.length; i++) {
                    divData += '<tr style="cursor:pointer" onclick="MPcallRedirect(' + ItemRole[i].nRoleID + ')"><td class="dt-body-left">' + nRow + '. ' + ItemRole[i].sRoleName + '</td></tr>';
                    nRow++;
                }
                divData += '</tbody></table>';

                $("#divMPPopContent").html(divData);
                $("#MPhTitle").html("Role");
                $("#MPPopContent").modal();
                $('#MPPopContent').on('hidden.bs.modal', function (e) {
                    $("#divMPPopContent").html("");
                });
            }

        },
        error: AjaxCall.error,
        complete: function (jqXHR, status) {//finaly

        }
    });
}

function MPcallRedirect(sroleid) {
    $('#MPPopContent').modal('toggle');
    LoaddinProcess();
    $.ajax({
        dataType: "html",
        type: AjaxCall.type,
        url: AshxSysFunc.url,
        data: { funcName: "selectedrole", param1: sroleid },
        success: function (response) {
            var ItemData = JSON.parse(response);
            if (ItemData.Status == SysProcess.SessionExpired) {
                HideLoadding();
                PopupLogin();
            }
            else if (ItemData.Status == SysProcess.Success) {
                LoaddinProcess();
                window.location = ItemData.Content;
            }
            else {
                HideLoadding();
                DialogWarning(DialogHeader.Warning, ItemData.Msg);
            }
        },
        error: AjaxCall.error,
        complete: function (jqXHR, status) {//finaly

        }
    });
}
/****End Function Master Page****/


//process code behind return
var SysProcess = {
    FileOversize: "OverSize",
    FileInvalidType: "InvalidType",
    Failed: "Failed",
    Success: "Success",
    SessionExpired: "SSEXP",
    LogonSharePathFailed: "LogonSPFailed"
}

var GridEvent = {
    sort: "SORT",
    pageindex: "PAGINDEX",
    pagesize: "PAGESIZE",
    BIND: "BIND",
    DOCREADY: "DOCREADY"
}

var AshxSysFunc = {
    url: "Ashx/SystemFunction.ashx",
    FuncEncrypt: "encrypt",
    FuncDecrypt: "decrypt",
    FuncEncodeForJavaDecode: "encrypt_decodejava",
    FuncChangeRole: "changerole",
    FuncHistoryWorkflow: "historywf",
    UrlFileUpload: "Fileuploader.ashx"
}

var AshxSysFuncParam = {
    funcName: "funcName",
    param1: "param1"
}

var SysFileUpload = {
    arrFileType: ['jpg', 'jpeg', 'png', 'xls', 'xlsx', 'pdf', 'txt', 'doc', 'docx', 'ppt', 'pptx', 'rar', 'zip'],
    dialogs: {
        alert: function (text) {
            return DialogWarning(DialogHeader.Warning, text);
        },
        confirm: function (text, callback) {
            DialogConfirm(DialogHeader.Confirm, text, function () { callback(); });
        }
    },
    captions: {
        button: function (options) { return '<i class="glyphicon glyphicon-paperclip"></i> Choose ' + (options.limit == 1 ? 'File' : 'Files'); },
        errors: {
            filesType: 'Only ${extensions} files are allowed to be uploaded.',
            fileSize: '${name} is too large! Please choose a file up to ${fileMaxSize}MB.'
        }
    },
    onProgress: function (data, item, listEl, parentEl, newInputEl, inputEl) {
        LoaddinProcess();
    },
    onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
        item.data = data;
    },
    onError: function (item, listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus, errorThrown) {
        HideLoadding();
    },
    //fnSucc = function(response){}
    DeleteFile: function (path, filename, fnSucc, objfile) {
        $.ajax({
            type: "POST",
            url: AshxSysFunc.UrlFileUpload,
            data: { funcname: "DEL", delpath: path, delfilename: filename },
            success: function (response) {
                if (fnSucc != undefined && fnSucc != "")
                    fnSucc(response, objfile);
            },
            complete: function (jqXHR, status) {//finaly
                //HideLoadding();
            }
        });
    }
}

//use >> sysEncrypt(empid, function (data) { sysDecrypt(data, function (data) { alert(data); }); });
function sysEncrypt(str, funcSuccess) {
    $.ajax({
        dataType: "html",
        type: AjaxCall.type,
        url: AshxSysFunc.url,
        data: { funcName: AshxSysFunc.FuncEncrypt, param1: str },
        success: function (response) {
            funcSuccess(response);
        },
        error: AjaxCall.error,
        complete: function (jqXHR, status) {//finaly

        }
    });
}

function sysEncryptForJava(str, funcSuccess) {
    $.ajax({
        dataType: "html",
        type: AjaxCall.type,
        url: AshxSysFunc.url,
        data: { funcName: AshxSysFunc.FuncEncodeForJavaDecode, param1: str },
        success: function (response) {
            funcSuccess(response);
        },
        error: AjaxCall.error,
        complete: function (jqXHR, status) {//finaly

        }
    });
}

//Use in java only
function sysDecrypt(str, funcSuccess) {
    $.ajax({
        dataType: "html",
        type: AjaxCall.type,
        url: AshxSysFunc.url,
        data: { funcName: AshxSysFunc.FuncDecrypt, param1: str },
        success: function (response) {
            funcSuccess(response);
        },
        error: AjaxCall.error,
        complete: function (jqXHR, status) {//finaly
        }
    });
}


function SetICheck() {
    //iCheck for checkbox and radio inputs
    $('.minimal input[type="checkbox"],.minimal input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });
    $('.minimal-red input[type="checkbox"],.minimal-red input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    });
    $('.flat-red input[type="checkbox"],.flat-red input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });
    $('.minimal-green input[type="checkbox"],.minimal-green input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_minimal-green',
        radioClass: 'iradio_minimal-green'
    });
    $('.flat-green input[type="checkbox"],.flat-green input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_square-green' //'iradio_flat-green'
    });

    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal,input[type="radio"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });
    $('input[type="checkbox"].minimal-red,input[type="radio"].minimal-red').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    });
    $('input[type="checkbox"].flat-red,input[type="radio"].flat-red').iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });
    $('input[type="checkbox"].minimal-green,input[type="radio"].minimal-green').iCheck({
        checkboxClass: 'icheckbox_minimal-green',
        radioClass: 'iradio_minimal-green'
    });
    $('input[type="checkbox"].flat-green,input[type="radio"].flat-green').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_square-green'//'iradio_flat-green'
    });
}

//tooltip
function SetTootip() {
    $('[data-toggle="tooltip"],[title]').tooltip();
}

//set mask format textbox
function SetInputMask() {
    $("[data-mask]").inputmask();
}

//Ex. >> SetEventKeypressOnEnter(Input("txtSearch"), function () { SearchData() });
function SetEventKeypressOnEnter(obj, func) {
    $(obj).on('keydown', function (e) {
        if (e.which == 13) {
            e.preventDefault();
            func();
        }
    });
}

//integerDigits : จำนวนหลักของของค่าจำนวนเต็ม,digits : จำนวนหลักของค่าทศนิยม
function InputMaskDecimal(objCtrl, integerDigits, digits, allowPlus, allowMinus) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: digits, //จำนวนหลักของค่าทศนิยม
        radixPoint: '.', //จุดทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
        allowPlus: Boolean(allowPlus), //อนุญาตใส่เครื่องหมายบวก
        allowMinus: Boolean(allowMinus) //อนุญาตใส่เครื่องหมายลบ
    });
}

function SetCheckBoxSelectRowInGrid(tableID, ckbHeadID, ckbListID) {
    $("table[id$=" + tableID + "] div.icheckbox_flat-green input[id$=" + ckbHeadID + "]").on("ifClicked", function (event) { checkboxSelectOrUnSelectAll(tableID, ckbListID, event); });
    $("table[id$=" + tableID + "] div.icheckbox_flat-green input[id*=" + ckbListID + "]").on("ifClicked", function (event) { checkUnCheckHeadCheckbox(tableID, ckbHeadID, ckbListID, event); });
}

function checkboxSelectOrUnSelectAll(containID, elementBodyID, event) {
    if (!event.currentTarget.checked) {
        $("table[id$=" + containID + "] div.icheckbox_flat-green input[id*=" + elementBodyID + "]").iCheck('check');
    }
    else {
        $("table[id$=" + containID + "] div.icheckbox_flat-green input[id*=" + elementBodyID + "]").iCheck('uncheck');
    }
}

function checkUnCheckHeadCheckbox(containID, elementHeadID, elementBodyID, event) {
    var nLengthAll = $("table[id$=" + containID + "] div.icheckbox_flat-green input[id*=" + elementBodyID + "]").length;
    var nLengthCheck = $("table[id$=" + containID + "] div.icheckbox_flat-green[aria-checked=true] input[id*=" + elementBodyID + "]").length;
    if (!event.currentTarget.checked) {//event ifClicked value checked is berfor click
        nLengthCheck = nLengthCheck + 1;
    }
    else {
        nLengthCheck = nLengthCheck - 1;
    }

    if (nLengthAll == 0 || nLengthAll != nLengthCheck) {
        $("input[id$=" + elementHeadID + "]").iCheck('uncheck');
    }
    else {
        $("input[id$=" + elementHeadID + "]").iCheck('check');
    }
}

function UnCheckCheckBoxHeader(elementHeadID) {
    $("input[id$=" + elementHeadID + "]").iCheck('uncheck');
}

function CheckDeleteDataInGrid(containID, elementBodyID, funcYes, funcNo) {
    var nLengthCheck = $("table[id$=" + containID + "] div.icheckbox_flat-green[aria-checked=true] input[id*=" + elementBodyID + "]").length;
    if (nLengthCheck == 0) {
        DialogWarning(DialogHeader.Warning, DialogMsg.AlertDel);
    }
    else {
        DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, funcYes, funcNo);
    }
}

/*********** Jquery Datatable  ***********/
var DataLengthMenu = [10, 20, 30, 50, 100];
var objJSTableDefaul = {
    retrieve: true,
    bPaginate: true,
    bLengthChange: true,
    aLengthMenu: DataLengthMenu,
    bFilter: false,
    bSort: true,
    bInfo: true,
    bAutoWidth: false,
    bProcessing: true,
    sDom: '<r>t<"cSet1"<"cSet2"l><"cSet2"i><"cSet2"p>>',
    sDomBtnDel: '<r>t<"cSet1"<"cBtnDel"><"cSet2"l><"cSet2"i><"cSet2"p>>',
    oLanguage: {
        "sInfoEmpty": "",
        "sEmptyTable": "No data.",
        "sInfo": "Showing _START_ to _END_ (_TOTAL_ item(s))",
        "sLengthMenu": "List _MENU_ items"
    }
};

function SetCommonPropertyTable(obj) {
    obj.bPaginate = objJSTableDefaul.bPaginate;
    obj.bLengthChange = objJSTableDefaul.bLengthChange;
    obj.aLengthMenu = objJSTableDefaul.aLengthMenu;
    $(obj).bFilter = objJSTableDefaul.bFilter;
    $(obj).bSort = objJSTableDefaul.bSort;
    $(obj).bInfo = objJSTableDefaul.bInfo;
    $(obj).bAutoWidth = objJSTableDefaul.bAutoWidth;
    $(obj).bProcessing = objJSTableDefaul.bProcessing;
    obj.sDom = objJSTableDefaul.sDom;
    obj.oLanguage = objJSTableDefaul.oLanguage;
    /*
    "bPaginate": objJSTableDefaul.bPaginate,
                "bLengthChange": objJSTableDefaul.bLengthChange,
                "": objJSTableDefaul.aLengthMenu,
                "": objJSTableDefaul.bFilter,
                "": objJSTableDefaul.bSort,
                "": objJSTableDefaul.bInfo,
                "": objJSTableDefaul.bAutoWidth,
                "": objJSTableDefaul.bProcessing,
                "": objJSTableDefaul.sDom,
                "": objJSTableDefaul.oLanguage
    */
}
/*********** End Jquery Datatable  ***********/


var AjaxCall = {
    dataType: "json",
    type: "POST",
    contentType: "application/json; charset=utf-8",
    error: function (XMLHttpRequest, textStatus, errorThrown) {
        HideLoadding();
        $.ajax({
            dataType: AjaxCall.dataType,
            type: AjaxCall.type,
            contentType: AjaxCall.contentType,
            url: "Ashx/SystemFunction.ashx?funcName=addlogerror&param1=" + XMLHttpRequest.responseJSON.Message + " " + XMLHttpRequest.responseJSON.ExceptionType + "&param2=" + XMLHttpRequest.responseJSON.StackTrace,
            success: function (response) {

            }
        });
        DialogError(DialogHeader.Error, "Message: " + XMLHttpRequest.responseJSON.Message + "<br/>ExceptionType: " + XMLHttpRequest.responseJSON.ExceptionType + "<br/>StackTrace: " + XMLHttpRequest.responseJSON.StackTrace);
    },
    errorMiniLoadding: function (XMLHttpRequest, textStatus, errorThrown) {
        $("div.overlay").removeClass("overlay").html("");
        DialogError(DialogHeader.Error, "Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
    },
    complete: function (jqXHR, status) {//finaly
    }
};

/************Get Data *************/
function GetValTextBox(txtID) {
    return $("input[id$=" + txtID + "]").val();
}
function GetValDropdown(ddlID) {
    return $("select[id$=" + ddlID + "]").val();
}
function GetValRadioListICheck(rblID) {
    return $("input[id*=" + rblID + "]:checked").val();
}
function GetValTextArea(txtID) {
    return $("textarea[id$=" + txtID + "]").val();
}
function GetValueAttrValue(txtID) {
    return $("input[id$=" + txtID + "]").attr("value");
}
function GetIsCheckRadioICheck(tableID, rblID) {
    return $("table[id$=" + tableID + "] input[id$=" + rblID + "]").is(":checked");
}
function Input(ctrlID) {
    return $("input[id$=" + ctrlID + "]");
}
function Select(ctrlID) {
    return $("select[id$=" + ctrlID + "]");
}
function IsNumberic(sVal) {
    sVal = (sVal + "").replace(/,/g, '');
    return $.isNumeric(sVal);
}
function GetValueCheckBoxiCheck(ckbID) {
    return Input(ckbID).prop('checked');
}

function sysParseFloat(sVal) {
    if (IsNumberic(sVal)) {
        return parseFloat((sVal + "").replace(/,/g, ''));
    }
    else {
        return null;
    }
}

//Using jquery.number.js
function NumbericFormat(nVal, nDigit) {
    var result = "";
    if (nVal != null && (nVal + "") != "" && nVal != undefined) {
        result = $.number(nVal, nDigit)
    }
    return result;
}

/******Set Data*********/
function SetValueTextBox(txtID, sval) {
    $("input[id$=" + txtID + "]").val(sval);
}

function SetValueLable(lblID, sval) {
    $("span[id$=" + lblID + "]").text(sval);
}

function SetValueTextArea(txtID, sval) {
    $("textarea[id$=" + txtID + "]").val(sval);
}

function SetValueDropDown(ddlID, sval) {
    $("select[id$=" + ddlID + "]").val(sval);
}

/*********** Dialog  ***********/
var DialogHeader = {
    Info: "Information",
    Error: "Error",
    Confirm: "Confirm",
    Warning: "Warning"
}

var DialogMsg = {
    ConfirmDel: "Do you want to delete data ?",
    ConfirmSave: "Do you want to save data ?",

    ConfirmSaveDraft: "Do you want to save draft data ?",
    ConfirmSubmit: "Do you want to submit data ?",
    ConfirmRecall: "Do you want to recall ?",
    ConfirmRequest: "Do you want to request edit ?",
    ConfirmApprove: "Do you want to approve with edit content edit ?",

    ConfirmLogout: "Do you want to logout ?",
    AlertDel: "Please select data to delete.",
    SaveComplete: "Already saved data.",

    SaveDraftComplete: "Already saved draft data.",
    //SubmitComplete: "Already submit data.",
    //RecallComplete: "Already recall.",
    //RequestComplete: "Already request edit.",
    SubmitComplete: "Already sent data.",
    RecallComplete: "Already sent data.",
    RequestComplete: "Already sent data.",
    ApproveComplete: "Already sent data.",

    DelComplete: "Already Deleted data.",
    Login: "Please Login.",
    Specify: "Please Specify"
}
var btnOKText = "OK";
var btnCancelText = "Cancel";

function DialogInfo(head, msg) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_INFO,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-info',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}

function DialogInfoRedirect(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_INFO,
        closable: false,
        closeByBackdrop: false,
        closeByKeyboard: false,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-info',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}

function DialogError(head, msg) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_DANGER,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-danger',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}

function DialogErrorRedirect(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_DANGER,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-danger',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}

function DialogWarning(head, msg) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_WARNING,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-warning',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}

function DialogWarningFunc(head, msg, func) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_WARNING,
        closable: true,
        draggable: true,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-warning',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                func();
            }
        }
        ]
    });
}

function DialogWarningRedirect(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_WARNING,
        closable: false,
        closeByBackdrop: false,
        closeByKeyboard: false,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-warning',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}

function DialogConfirm(head, msg, funcYes, funcNo) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_PRIMARY,
        closable: true,
        draggable: true,
        buttons: [{
            id: 'btn-ok',
            //icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-primary',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                LoaddinProcess();
                funcYes();
            }
        },
        {
            id: 'btn-cancel',
            //icon: 'glyphicon glyphicon-remove',
            label: btnCancelText,
            cssClass: 'btn btn-default',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                if (funcNo != null && funcNo != undefined && funcNo != "") {
                    funcNo();
                }
            }
        }
        ]
    });
}

function DialogConfirmInfo(head, msg, funcYes, funcNo) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_INFO,
        closable: true,
        draggable: true,
        buttons: [{
            id: 'btn-ok',
            //icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-info',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                LoaddinProcess();
                funcYes();
            }
        },
        {
            id: 'btn-cancel',
            //icon: 'glyphicon glyphicon-remove',
            label: btnCancelText,
            cssClass: 'btn btn-default',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                if (funcNo != null && funcNo != undefined && funcNo != "") {
                    funcNo();
                }
            }
        }
        ]
    });
}

function DialogConfirmCloseButton(head, msg, funcYes, funcNo) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_PRIMARY,
        closable: false,
        closeByBackdrop: false,
        closeByKeyboard: false,
        draggable: true,
        buttons: [{
            id: 'btn-ok',
            //icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-primary',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                LoaddinProcess();
                funcYes();
            }
        },
        {
            id: 'btn-cancel',
            //icon: 'glyphicon glyphicon-remove',
            label: btnCancelText,
            cssClass: 'btn btn-default',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                if (funcNo != null && funcNo != undefined && funcNo != "") {
                    funcNo();
                }
            }
        }
        ]
    });
}

function DialogSuccess(head, msg) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_SUCCESS,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-success',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}

function DialogSuccessRedirect(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_SUCCESS,
        closable: false,
        closeByBackdrop: false,
        closeByKeyboard: false,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-success',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}

function DialogAlertLogin(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_INFO,
        closable: false,
        closeByBackdrop: false,
        closeByKeyboard: false,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-info',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}
function DialogConfirmDup(head, msg, funcYes, funcNo) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_WARNING,
        closable: true,
        draggable: true,
        buttons: [{
            id: 'btn-ok',
            //icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-primary',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                LoaddinProcess();
                funcYes();
            }
        },
        {
            id: 'btn-cancel',
            //icon: 'glyphicon glyphicon-remove',
            label: btnCancelText,
            cssClass: 'btn btn-default',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                if (funcNo != null && funcNo != undefined && funcNo != "") {
                    funcNo();
                }
            }
        }
        ]
    });
}
/***********End Dialog  ***********/

/*********** Form Validation ************/

function BindValidate(sContainer, objValidate) {
    $("#" + sContainer).formValidation({
        framework: 'bootstrap',
        err: {
            //container: 'tooltip'
        },
        //icon: {
        //    valid: 'glyphicon glyphicon-ok',
        //    invalid: 'glyphicon glyphicon-remove',
        //    validating: 'glyphicon glyphicon-refresh'
        //},
        fields: objValidate,
        autoFocus: true
    }).on('err.validator.fv', function (e, data) {
        data.element
               .data('fv.messages')
               // Hide all the messages
               .find('.help-block[data-fv-for="' + data.field + '"]').hide()
               // Show only message associated with current validator
               .filter('[data-fv-validator="' + data.validator + '"]').show();
    }).on('err.field.fv', function (e, data) {
        data.fv.disableSubmitButtons(false);
    }).on('success.field.fv', function (e, data) {
        data.fv.disableSubmitButtons(false);
    });
}

//for excluded: ':disabled' >> check control on hide and disable
function BindValidateExcluded(sContainer, objValidate) {
    $("#" + sContainer).formValidation({
        framework: 'bootstrap',
        err: {
            //container: 'tooltip'
        },
        excluded: ':disabled',
        icon: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: objValidate,
        autoFocus: true
    }).on('err.validator.fv', function (e, data) {
        data.element
               .data('fv.messages')
               // Hide all the messages
               .find('.help-block[data-fv-for="' + data.field + '"]').hide()
               // Show only message associated with current validator
               .filter('[data-fv-validator="' + data.validator + '"]').show();
    }).on('err.field.fv', function (e, data) {
        data.fv.disableSubmitButtons(false);
    }).on('success.field.fv', function (e, data) {
        data.fv.disableSubmitButtons(false);
    });
}

function CheckValidate(sContainer) {
    var isValid = $("#" + sContainer).data('formValidation').validate().isValid();
    if (!isValid) {
        ScrollTopToElements($($("div#" + sContainer).data('formValidation').$invalidFields[0]).attr("id"));//$("div#" + sContainer).data('formValidation').$invalidFields[0].focus();
    }
    return isValid;
}

function ScrollTopToElements(sElementID) {
    $('html, body').animate({ scrollTop: $("#" + sElementID).offset().top - 40 }, 'fast');
}

//top-(40 + nToTopValue)
function ScrollTopToElementsTo(sElementID, nToTopValue) {
    $('html, body').animate({ scrollTop: $("#" + sElementID).offset().top - (40 + nToTopValue) }, 'fast');
}

var ValidateProp = {
    Status_INVALID: "INVALID",
    Status_NOT_VALIDATED: "NOT_VALIDATED",
    UpdateStatut: "updateStatus",
    DateFormat_DDMMYYYY: "DD/MM/YYYY",
}

//arrObj >> Ex. arrObj.push(GetElementName("txtTest", objControl.txtbox))
function ReValidateField(sContainer, arrObj) {
    for (var i = 0; i < arrObj.length; i++) {
        $('#' + sContainer).formValidation('revalidateField', arrObj[i]);
    }
}

function ReValidateFieldControl(sContainer, ctrlName) {
    $('#' + sContainer).formValidation('revalidateField', ctrlName);
}

//arrObj >> Ex. arrObj.push(GetElementName("txtTest", objControl.txtbox))
function UpdateStatusValidate(sContainer, arrObj) {
    for (var i = 0; i < arrObj.length; i++) {
        $('#' + sContainer).formValidation('updateStatus', arrObj[i], 'NOT_VALIDATED');
    }
}

function UpdateStatusValidateControl(sContainer, ctrlName, Status) {
    $('#' + sContainer).formValidation('updateStatus', ctrlName, Status);
}

function addValidateEmail_notEmpty() {//กรณีที่มีการกำหนด data-inputmask เนื่องจากไม่สามารถใช้ notEmpty ได้
    return {
        validators: {
            regexp: {
                regexp: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
                //message: "รูปแบบอีเมล์ ไม่ถูกต้อง",
                message: "Invalid E-mail Format."
            },
            callback: {
                //message: "กรุณาระบุ อีเมล์",
                message: "Please Enter E-mail.",
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }
    };
}

function addValidate_Email() {
    return {
        validators: {
            regexp: {
                regexp: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
                message: "รูปแบบอีเมล์ ไม่ถูกต้อง"
            }
        }
    };
}

//กรณีที่มีการกำหนด data-inputmask เนื่องจากไม่สามารถใช้ notEmpty ได้
function addValidate_CustomNotEmpty(msg) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }
    };
}

function addValidate_rblICheckNotEmpty(msg) {
    return {
        validators: {
            choice: {
                min: 1,
                message: msg
            }
        }
    };
}

function addValidate_TextArea(msg, minLength, maxLength) {
    return {
        validators: {
            stringLength: {
                min: minLength,
                max: maxLength,
                message: msg
            }
        }
    };
}

function addValidate_TextAreaMin(msg, minLength) {
    return {
        validators: {
            stringLength: {
                min: minLength,
                message: msg
            }
        }
    };
}

function addValidate_TextAreaMax(msg, minLength, maxLength) {
    return {
        validators: {
            stringLength: {
                max: maxLength,
                message: msg
            }
        }
    };
}

function addValidate_notEmpty(msg) {
    return {
        validators: {
            notEmpty: {
                message: msg
            }
        }
    };
}

function addValidateMask_notEmpty(msg) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }
    };
}

function addValidate_Password_notEmpty(msgEmpty) {
    return {
        validators: {
            regexp: {
                regexp: "^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.*[!@#\$%\^&\*])(?=.{8,})",
                message: "Passwords must have at least 8 characters and contain at least three of the following: uppercase letters, lowercase letters, numbers, and symbols."//จะต้องมี ตัวพิมพ์ใหญ่ หรือ พิมพ์เล็ก หรือ ตัวเลข และ อักขระพิเศษ
            },
            callback: {
                message: (msgEmpty != undefined && msgEmpty != "") ? msgEmpty : "กรุณาระบุ รหัสผ่าน",
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }
    };
}

function addValidate_ConfirmPassword(msgEmpty, CompareCtrlID, msgInvalidPassword) {
    return {
        validators: {
            notEmpty: {
                message: msgEmpty
            },
            callback: {
                message: msgInvalidPassword,
                callback: function (value, validator, $field) {
                    var sValCheck = $("input[id$=" + CompareCtrlID + "]").val();
                    return (value == sValCheck);
                }
            }
        }
    };
}

function addValidate_DateNotEmpty(msgEmpty, msgFormat) {
    return {
        validators: {
            notEmpty: {
                message: msgEmpty
            },
            date: {
                format: 'DD/MM/YYYY',
                message: msgFormat == "" ? 'The date is not a valid' : msgFormat
            }
        }
    }
}

//check date range picker : please include moment.js, default format = DD/MM/YYYY
function addValidate_DateRangeNotEmpty(msgEmpty, msgFormat, format) {
    format = format == null || format == undefined || format + "" == "" ? ValidateProp.DateFormat_DDMMYYYY : format
    return {
        validators: {
            notEmpty: {
                message: msgEmpty
            },
            callback: {
                message: msgFormat == "" ? 'The date is not a valid' : msgFormat,
                callback: function (value, validator, $field) {
                    var arrValue = (value + "").split('-');
                    if (arrValue.length == 2) {
                        var start = arrValue[0].replace(/ /g, '');
                        var end = arrValue[1].replace(/ /g, '');
                        var c1 = moment(start, format, true).isValid();
                        var c2 = moment(end, format, true).isValid();
                        if (Boolean(c1) && Boolean(c2)) {
                            if (moment(start, format, true).valueOf() > moment(end, format, true).valueOf()) {
                                return false;
                            }
                            else {
                                return true;
                            }
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        return false;
                    }
                }
            }
        }
    };
}


function GetElementName(sElement, objType) {
    return $(objType + "[id$=" + sElement + "]").attr("name");
}

function GetElementNameICheck(sElement) {
    return $("input[name$=" + sElement + "").attr("name");
}

function GetElementID(sElement, objType) {
    return $(objType + "[id$=" + sElement + "]").attr("id");
}

var objControl = {
    txtbox: "input",
    txtarea: "textarea",
    dropdown: "select",
    div: "div",
    span: "span",
    rblICheck: "input",
    btn: "input"
};

/***********End Form Validation ************/

//get value selected checkbox in gridview
//txtID_Data = control textbox
function GetSelectDeleteRowInGrid(sGridID, txtID_Data) {
    var arrTR = $("table[id$=" + sGridID + "] tbody tr");
    var arrCheckID = [];
    for (var i = 0; i < arrTR.length; i++) {
        if ($(arrTR[i]).find("div.icheckbox_flat-green").attr("aria-checked") + "" == "true") {
            var sVal = $(arrTR[i]).find("input[id$=" + txtID_Data + "]").attr("value");
            arrCheckID.push(sVal);
        }
    }
    return arrCheckID;
}

function DisableItemDropdown(ddlID, value, isDisable) {
    var $obj = $("select[id$=" + ddlID + "] option[value='" + value + "']");
    if (Boolean(isDisable)) {
        $obj.prop('disabled', true).addClass("itemDisable");
    }
    else {
        $obj.prop('disabled', false).removeClass("itemDisable");
    }
}

function NullToBlank(val) {
    return val == null || val == undefined ? "" : val;
}

//jsonListData = { Value : "",Text : "" }
function BindDropdown(objControl, jsonListData, lableText, labelValue) {
    var $control = $(objControl);
    $control.html("");
    if (lableText != undefined && lableText != null) {
        $control.append('<option value="' + labelValue + '">' + lableText + '</option>');
    }
    $.each(jsonListData, function () {
        var item = $(this);
        $control.append('<option value="' + item[0].Value + '">' + item[0].Text + '</option>');
    });
}

function GetRowIndex(ctrlID) {
    var arrID = $("#" + ctrlID).attr("id").split('_');
    var id = arrID[arrID.length - 1];
    return id;
}

function GetRowIndexObj(objThis) {
    var arrID = $(objThis).attr("id").split('_');
    var id = arrID[arrID.length - 1];
    return id;
}

function GetMultiSeletValue(ctrlID) {
    var arrControl = $('select[id$=' + ctrlID + '] option:selected');
    var selected = [];
    $(arrControl).each(function (index, item) {
        selected.push($(this).val());
    });
    return selected;
}

function IsBrowserFirefox() {
    var mybrowser = navigator.userAgent;
    if (mybrowser.indexOf('Firefox') > 0 || mybrowser.indexOf('Chrome') > 0) {
        return true;
    }
    else
        return false;
}

//WebMethodName = WebMethod Name, FuncSuccess = Function On Success >> function(response){}, FuncComplete = Function On Complete >> function(){}, objData = format data { dataID: dataID }
function AjaxCallWebMethod(WebMethodName, FuncSuccess, FuncComplete, objData) {
    if (objData == undefined || objData == null) {
        $.ajax({
            dataType: AjaxCall.dataType,
            type: AjaxCall.type,
            contentType: AjaxCall.contentType,
            url: location.pathname + "/" + WebMethodName,
            success: function (response) {
                if (FuncSuccess != undefined && FuncSuccess != null && FuncSuccess != "") {
                    FuncSuccess(response);
                }
            },
            error: AjaxCall.error,
            complete: function (jqXHR, status) {//finaly
                if (FuncComplete != undefined && FuncComplete != null && FuncComplete != "") {
                    FuncComplete();
                }
            }
        });
    }
    else {
        $.ajax({
            dataType: AjaxCall.dataType,
            type: AjaxCall.type,
            contentType: AjaxCall.contentType,
            url: location.pathname + "/" + WebMethodName,
            data: JSON.stringify(objData),
            success: function (response) {
                if (FuncSuccess != undefined && FuncSuccess != null && FuncSuccess != "") {
                    FuncSuccess(response);
                }
            },
            error: AjaxCall.error,
            complete: function (jqXHR, status) {//finaly
                if (FuncComplete != undefined && FuncComplete != null && FuncComplete != "") {
                    FuncComplete();
                }
            }
        });
    }
}

//return Value:'',Text:'', value != '' only
function GetDataSourceDropdown(ddlID) {
    var data = [];
    var arr = $("select[id$=" + ddlID + "] option[value != '']");
    for (var i = 0; i < arr.length; i++) {
        data.push({
            Value: $(arr[i]).val(),
            Text: $(arr[i]).text()
        });
    }
    return data;
}

function GetSourceTextFromDropdown(ddlID, val) {
    return $("select[id$=" + ddlID + "] option[value='" + val + "']").text();
}

function SetRowNoData(sTableID, Colspan) {
    Colspan = Colspan == undefined || Colspan == null ? 1 : Colspan;

    $("table[id$=" + sTableID + "] tbody tr").remove();
    $("table[id$=" + sTableID + "] tbody").append('<tr><td colspan="' + Colspan + '" class="dt-body-center NoFix" style="background-color:#f6f6f6;vertical-align: middle;"><span class="text-red">No Data.</span></td></tr>');
}
function CheckTextInput(nVal) {

    if (nVal != "") {
        nVal = nVal.replace(/,/g, '');
        if (IsNumberic(nVal)) {
            var nCheck = parseFloat(nVal);
            if (nCheck > 0) {
                nVal = addCommas(nCheck);
            }
            else if (nCheck < 0) {
                nVal = "";
            }
            else {
                nVal = nCheck;
            }
        }
        else {
            if (nVal.toLowerCase() == "na" || nVal.toLowerCase() == "n/a") {
                nVal = "N/A";
            } else {
                nVal = "";
            }
        }
    }
    nVal = nVal + "";
    return nVal + "";
}
function CheckTextOutput(nValue) {
    var nVal = nValue + "";
    if (nVal != "") {
        if (IsNumberic(nVal)) {
            nVal = nVal.replace(/,/g, '');
            if (nVal.toLowerCase() != "na" && nVal.toLowerCase() != "n/a") {
                var nDecimal = 3;
                var sEmpty = "-";
                if (IsNumberic(nVal)) {

                    var sv2 = parseFloat(nVal);
                    var arrValue = nVal.split('.');
                    if (sv2 >= 1 || sv2 == 0) { // 1 ถึง Infinity
                        nVal = SetFormatNumber(sv2, nDecimal, sEmpty)
                    }
                    else if (sv2 > 0 && sv2 < 1 && arrValue.length == 2) {
                        if (arrValue[1].length > 4) {
                            nVal = (sv2.toExponential(3)).replace(/e/g, 'E');
                        }
                    }
                    else if (sv2 <= -1) {
                        nVal = SetFormatNumber(sv2, nDecimal, sEmpty)
                    }
                }
            }
            else {
                nVal = "N/A";
            }
        }
        else {
            nVal = "";
        }

    }

    nVal = nVal + "";
    return nVal + "";
}
function SetFormatNumber(nNumber, nDecimal, sEmpty) {
    if ($.isNumeric(nNumber)) {
        if ($.isNumeric(nDecimal))
            return addCommas(nNumber.toFixed(nDecimal));
        else
            return addCommas(nNumber);
    }
    else {
        return IsEmpty(nNumber) ? (sEmpty === undefined ? "" : sEmpty) : nNumber;
    }
}
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

// FancyBox Bank เพิ่ม
var Extension = {
    Image: ['jpg', 'jpeg', 'png', 'gif'],
    Video: ['mov', 'wmv', 'avi', 'mp4'],
    Document: ['doc', 'docx', 'xls', 'xlsx', 'pdf'],
    Other: ['rar', 'zip'],
    GetAll: function () {
        var arrExt = [];
        for (var key in this) {
            if (key != 'GetAll') arrExt = arrExt.concat(this[key]); //arrExt.push(this[key]);
        }
        return arrExt;
    }
};
function FancyBox_ViewFile(sFileURL) {
    var sFileExt = sFileURL.split('.').pop(); //File Extension
    var isImage = $.inArray(sFileExt, Extension.Image) > -1; //Extension.Image.indexOf(sFileExt) > -1;
    var isCorrectType = isImage || sFileExt == 'pdf';
    if (isCorrectType) {
        $.fancybox.open({
            href: sFileURL,
            type: isImage ? 'image' : 'iframe',
            iframe: { preload: false }, // fixes issue with iframe and IE
            padding: 5,
            openEffect: 'elastic',
            openSpeed: 150,
            closeEffect: 'elastic',
            closeSpeed: 150,
            closeClick: true
        });
    } else {
        window.location = sFileURL;
    }
    return isCorrectType;
}

///////////Function Convert////////////////
function ConvertLiterToBarrel(sVal) {
    var nReturn = null;
    if ((sVal != null && sVal != "") && $.isNumeric(sVal)) {
        var nVal = +sVal;
        nReturn = nVal / 158.9873;
    }
    return nReturn;
}

function ConvertM3ToBarrel(sVal) {
    var nReturn = null;
    if ((sVal != null && sVal != "") && $.isNumeric(sVal)) {
        {
            nReturn = ConvertLiterToBarrel(ConvertM3ToLiter(sVal));
        }
    }
    return nReturn;
}

function ConvertM3ToLiter(sVal) {
    var nReturn = null;
    if ((sVal != null && sVal != "") && $.isNumeric(sVal)) {
        var nVal = +sVal;
        nReturn = nVal * 1000;
    }
    return nReturn;
}

function ConvertBarrelToLiter(sVal) {
    var nReturn = null;
    if ((sVal != null && sVal != "") && $.isNumeric(sVal)) {
        var nVal = +sVal;
        nReturn = nVal * 158.9873;
    }
    return nReturn;
}

function ConvertBarrelToM3(sVal) {
    var nReturn = null;
    if ((sVal != null && sVal != "") && $.isNumeric(sVal)) {
        var nVal = +sVal;
        nReturn = ConvertLiterToM3((ConvertBarrelToLiter(sVal) + ""));
    }
    return nReturn;
}

function ConvertLiterToM3(sVal) {
    var nReturn = null;
    if ((sVal != null && sVal != "") && $.isNumeric(sVal)) {
        var nVal = +sVal;
        nReturn = nVal / 1000;
    }
    return nReturn;
}