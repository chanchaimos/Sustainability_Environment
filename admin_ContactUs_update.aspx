<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_ContactUs_update.aspx.cs" Inherits="admin_ContactUs_update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-edit"></i>&nbsp;Contact Us</div>
        <div class="panel-body" id="divContent">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Name :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <asp:Label runat="server" CssClass="form-control-static" ID="lbName"></asp:Label>
                            <%--<asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="240"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">E-mail :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <asp:Label runat="server" ID="lbEmail"></asp:Label>
                            <%--                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Tel :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <asp:Label runat="server" ID="lbTel"></asp:Label>
                            <%--                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtTel" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Subject :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <asp:Label runat="server" ID="lbSubject"></asp:Label>
                            <%--                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">User Description :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:Label runat="server" ID="lbUserDes"></asp:Label>
                        <%--<asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>--%>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Add Date :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <asp:Label runat="server" ID="lbAddDate"></asp:Label>
                            <%--                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Status :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <asp:Label runat="server" ID="lbStatus"></asp:Label>
                            <%--                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">User Request File:</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <asp:Label runat="server" ID="lbUrlFileUser"></asp:Label>
                            <%--                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group" runat="server" id="DivShowFileAdmin">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Admin Response File:</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="input-group">
                            <asp:Label runat="server" ID="lbUrlFileAdmin"></asp:Label>
                            <%--                            <div class="input-group-addon"><i class="fa fa-pencil-alt"></i></div>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Admin Response <span class="text-red">*</span> :</label>
                    <div class="col-xs-12 col-md-9">
                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </div>
                </div>
                <%-- File --%>
                <div class="form-group" id="DivFileContactUS" runat="server">
                    <label class="control-label col-xs-12 col-md-3 text-left-sm">Attach File :</label>
                    <div class="col-xs-12 col-md-9">
                        <div class="row">
                            <div class="col-xs-6">
                                <input type="file" id="fulOther" name="fulOther" />
                            </div>
                            <div id="divFileBtnContactUs" class="col-sm-6 hide">
                                <button id="btnViewFileContactUs" type="button" class="btn btn-info" onclick=""><span class="glyphicon glyphicon-zoom-in"></span></button>
                                <button id="btnDelFileContactUs" type="button" class="btn btn-danger" onclick=""><span class="glyphicon glyphicon-trash"></span></button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
                                <span class="text-red">ระบบอนุญาตให้อัพโหลดได้เฉพาะไฟล์ pdf, MS-Office, jpg, gif, png เท่านั่น ขนาดไฟล์ไม่เกิน 10 MB.</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer text-center">
            <button type="button" id="btnSave" class="btn btn-primary" onclick="SaveData()"><i class="fa fa-save"></i>&nbsp;Save</button>
            <a class="btn btn-default" href="admin_ContactUs_lst.aspx">Cancel</a>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidnID" />
    <asp:HiddenField runat="server" ID="hidEncyptID" />
    <asp:HiddenField runat="server" ID="hidsStatus" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="Scripts/Fileupload/src/jquery.fileuploader.js"></script>
    <link href="Scripts/Fileupload/src/jquery.fileuploader.css" rel="stylesheet" />
    <script type="text/javascript">

        $(function () {
            SetFileUploadOther();
            var Status = GetValTextBox('hidsStatus');
            if (Status == "2") {
                $('button[id$=btnSave]').hide();
            }
            var objValidate = {};
            objValidate[GetElementName("txtDesc", objControl.txtarea)] = addValidate_notEmpty(DialogMsg.Specify + " Description");
            BindValidate("divContent", objValidate);
        });
        function SaveData() {
            var IsPass = (CheckValidate("divContent"));
            if (IsPass) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                    LoaddinProcess();
                    AjaxCallWebMethod("SaveToDB", function (response) {
                        HideLoadding();
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "admin_ContactUs_lst.aspx");
                        } else {
                            DialogWarning(DialogHeader.Warning, response.d.Msg);
                        }
                    }, "", {
                        sDesc: GetValTextArea('txtDesc'),
                        sID: GetValTextBox('hidEncyptID'),
                        objFile: $dataFileOther,
                    });
                }, "");
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
                    data: { funcname: "UPLOAD", savetopath: 'ContactUs/Temp/', savetoname: '' },
                    type: 'POST',
                    enctype: 'multipart/form-data',
                    start: true,
                    synchron: true,
                    beforeSend: function (item, listEl, parentEl, newInputEl, inputEl) {
                        return true;
                    },
                    onProgress: SysFileUpload.onProgress,
                    onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
                        //if (item.size <= 5242880) {
                        AddFileContactUs(data);
                        //} else {
                        // DialogWarning('File size limit up to 5MB.');
                        //}
                        //arrData_item.push({ item: item, nType: elementID })
                        RemoveFile(item);
                    },
                    onError: SysFileUpload.onError,
                    // Callback fired after all files were uploaded
                    onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                        var $dataFileOther = apiFile1.getFiles();
                        BindFileContact();
                    }
                }
            });

            var apiFile1 = $.fileuploader.getInstance(filupload1);

            function RemoveFile(item) {
                apiFile1.remove(item);
            }
            BindFileContact();
        }
        function AddFileContactUs(item) {
            var nID = $dataFileOther.length > 0 ? Enumerable.From($dataFileOther).Max('$.nID') + 1 : 1;
            $dataFileOther.push({
                nID: nID,
                sPath: item.SaveToPath,
                sSysFileName: item.SaveToFileName,
                sFileName: item.FileName,
            });
        }
        function BindFileContact() {
            LoaddinProcess();
            var arrDataFile = Enumerable.From($dataFileOther).ToArray();

            if (arrDataFile.length > 0) {
                $('#divFileBtnContactUs').removeClass('hide');
                //var isHideDel = GetValTextBox('hddStatus') == "Y";
                var isHideDel = "N" == "Y";
                if (isHideDel) { $('#btnDelFileContactUs').remove(); }

                var qThis = Enumerable.From(arrDataFile).FirstOrDefault();

                if (qThis !== undefined) {
                    var sFileURL = qThis.sPath.replace("../", "") + qThis.sSysFileName;
                    var onclick = "FancyBox_ViewFile('" + sFileURL + "')";
                    $('#btnViewFileContactUs').attr('onclick', onclick);

                    if (!isHideDel) {
                        $('#btnDelFileContactUs').attr('onclick', 'DelFileContactUs(' + qThis.nID + ')');
                    }

                    if (qThis.Freeze == "Y") {
                        $('#btnDelFileContactUs').hide();
                    } else {
                        $('#btnDelFileContactUs').show();
                    }

                    $("input[id$=fulOther]").parent().addClass('fileuploader-disabled');
                    //$("#txtFileFinancial").parent().find(".fileuploader-input > .fileuploader-input-caption > span").html("1  file was chosen")
                    $("#fulOther").parent().find(".fileuploader-input > .fileuploader-input-caption > span").html(qThis.sFileName);
                } else {
                    $("input[id$=fulOther]").parent().removeClass('fileuploader-disabled');
                    $("#fulOther").parent().find(".fileuploader-input > .fileuploader-input-caption > span").html("Choose file to upload")
                }
            } else {
                $("input[id$=fulOther]").parent().removeClass('fileuploader-disabled');
                $('#divFileBtnContactUs').addClass('hide');
                $("#fulOther").parent().find(".fileuploader-input > .fileuploader-input-caption > span").html("Choose file to upload")
            }
            HideLoadding();
        }
        function DelFileContactUs(nID) {
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                LoaddinProcess();
                var lstFile = Enumerable.From($dataFileOther).Where(function (w) { return w.nID == nID }).ToArray();
                //var lst_Data = Enumerable(arrData).Where(function (w) { w.nRoleID == nID }).ToArray();
                if (lstFile.length > 0) {
                    $dataFileOther = Enumerable.From($dataFileOther).Where(function (w) { return w.nID != nID }).ToArray();
                }

                //$dataFileOther = $.grep($dataFileOther, function (a) {
                //    if (a.nID == nID) {
                //        a.cDel = "Y";
                //    }
                //    return a;
                //});
                $('#divFileBtnContactUs').addClass('hide');
                HideLoadding();
                BindFileContact();
            }, "");
        }
    </script>
</asp:Content>

