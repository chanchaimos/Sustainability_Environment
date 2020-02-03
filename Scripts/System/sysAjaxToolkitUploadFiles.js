
function AjaxFileUpload_change_text() {

    //Here you can write whatever you want = "..."

    Sys.Extended.UI.Resources.AjaxFileUpload_SelectFile = "Select File";
    Sys.Extended.UI.Resources.AjaxFileUpload_DropFiles = "Drop files here";
    Sys.Extended.UI.Resources.AjaxFileUpload_Pending = "pending";
    Sys.Extended.UI.Resources.AjaxFileUpload_Remove = "Remove";
    Sys.Extended.UI.Resources.AjaxFileUpload_Upload = "Upload";
    Sys.Extended.UI.Resources.AjaxFileUpload_Uploaded = "Uploaded";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadedPercentage = "uploaded {0} %";
    Sys.Extended.UI.Resources.AjaxFileUpload_Uploading = "Uploading";
    Sys.Extended.UI.Resources.AjaxFileUpload_FileInQueue = "{0} file(s) in queue.";
    Sys.Extended.UI.Resources.AjaxFileUpload_AllFilesUploaded = "All Files Uploaded.";
    Sys.Extended.UI.Resources.AjaxFileUpload_FileList = "List of Uploaded files:";
    Sys.Extended.UI.Resources.AjaxFileUpload_SelectFileToUpload = "Please select file(s) to upload.";
    Sys.Extended.UI.Resources.AjaxFileUpload_Cancelling = "Cancelling...";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadError = "An Error occured during file upload.";
    Sys.Extended.UI.Resources.AjaxFileUpload_CancellingUpload = "Cancelling upload...";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadingInputFile = "Uploading file: {0}.";
    Sys.Extended.UI.Resources.AjaxFileUpload_Cancel = "Cancel";
    Sys.Extended.UI.Resources.AjaxFileUpload_Canceled = "cancelled";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadCanceled = "File upload cancelled";
    Sys.Extended.UI.Resources.AjaxFileUpload_DefaultError = "File upload error";
    Sys.Extended.UI.Resources.AjaxFileUpload_UploadingHtml5File = "Uploading file: {0} of size {1} bytes.";
    Sys.Extended.UI.Resources.AjaxFileUpload_error = "error";
}

function SetEventAjaxFileUpload_SingelFile(CtrlID) {
    $("input[id$=" + CtrlID + "_Html5InputFile]").removeAttr("multiple");
    $("div[id$=" + CtrlID + "_Html5DropZone]").on("click", function () { $("input[id$=" + CtrlID + "_Html5InputFile]").click(); });
    $("span[id$=" + CtrlID + "_SelectFileContainer]").on("click", function () {
        $("input[id$=" + CtrlID + "_Html5InputFile]").val("");
    });
    Sys.Extended.UI.Resources.AjaxFileUpload_DropFiles = "";
    Sys.Extended.UI.Resources.AjaxFileUpload_SelectFile = "<i class='glyphicon glyphicon-upload'></i><span> Select</span>";
}

function SetEventAjaxFileUpload_MulltiFile(CtrlID) {
    $("div[id$=" + CtrlID + "_Html5DropZone]").on("click", function () {
        $("input[id$=" + CtrlID + "_Html5InputFile]").val("");
        $("input[id$=" + CtrlID + "_Html5InputFile]").click();
    });

    $("span[id$=" + CtrlID + "_SelectFileContainer]").on("click", function () {
        $("input[id$=" + CtrlID + "_Html5InputFile]").val("");
    });

    Sys.Extended.UI.Resources.AjaxFileUpload_DropFiles = "";
    Sys.Extended.UI.Resources.AjaxFileUpload_SelectFile = "<i class='glyphicon glyphicon-upload'></i><span> Select</span>";
}
