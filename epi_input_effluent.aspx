<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_EPI_FORMS.master" AutoEventWireup="true" CodeFile="epi_input_effluent.aspx.cs" Inherits="epi_input_effluent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        table#tbDataAdd > tbody > tr > td > input:not(.str), input.Density {
            text-align: right !important;
        }

        table#tbDataAddPoint > tbody > tr > td > input:not(.str), input.Density {
            text-align: right !important;
        }

        table#tbData > tbody > tr > td > input:not(.str), input.Density {
            text-align: right !important;
        }

        table#tbDataAddOther > tbody > tr > td > input:not(.str), input.Density {
            text-align: right !important;
        }

        table#tbOperting > tbody > tr > td > input:not(.str), input.Density {
            text-align: right !important;
        }

        .cMarTop10 {
            margin-top: 10px;
        }

        .cMiddle {
            vertical-align: middle !important;
        }

        .modal-backdrop {
            display: none !important;
        }

        .cModal {
            background-color: #4eb3f0;
            color: white;
        }

        .cbtnStyle {
            text-align: center;
            margin: 20px 0px 20px 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="divContent" style="display: none;">
        <div class="col-xs-12 col-md-6 text-left" style="margin-bottom: 5px;">
        </div>
        <div class="col-xs-12 col-md-6 text-right-lg text-right-md text-left-sm" style="margin-bottom: 5px;">
            <button type="button" onclick="ShowDeviate();" class="btn btn-info" title="Deviate History"><i class="fas fa-comments"></i></button>
            <button type="button" onclick="ShowHistory();" class="btn btn-info" title="Workflow History"><i class="fas fa-comment-alt"></i></button>
            <asp:LinkButton runat="server" ID="btnEx" CssClass="btn btn-success" OnClick="btnEx_Click">Export</asp:LinkButton>
        </div>
        <div class="col-xs-12">
            <div class="table-responsive">
                <table id="tbData" class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                </table>
            </div>
        </div>
        <%-- divRemark --%>
        <div class="col-xs-12 col-md-6" style="margin-top: 20px;">
            <div id="divRemark" style="display: none;">
                <div class="well">
                    <div class="form-group">
                        <label class="control-label col-xs-12">Remark Effluent<span class="text-red">*</span></label>
                        <textarea id="txtRemarkEff" runat="server" class="form-control" rows="4" style="resize: vertical"></textarea>
                    </div>
                </div>
            </div>
        </div>
        <%-- ADD Manually Input --%>
        <div class="col-xs-12" id="divPointManual">
            <div class="panel">
                <div class="panel panel-primary">
                    <div class="panel-heading" href="#PointManual" data-toggle="collapse" style="cursor: pointer;">Manually Input</div>
                    <div id="PointManual" class="panel-body pad-no collapse in">
                        <div class="form-group cMarTop10 col-xs-12">
                            <div class="col-xs-12">
                                <button type="button" id="btnAddManual" class="btn btn-primary">Add New Point(Manually Input)</button>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tbDataAddPointManual" class="table dataTable table-bordered table-hover" style="min-width: 100%; width: 952px;">
                                        <thead>
                                            <tr>
                                                <th class="text-center" width="10%">No.</th>
                                                <th class="text-center" width="35%">Point Name</th>
                                                <th class="text-center" width="15%">Treament Method</th>
                                                <th class="text-center" width="15%">Discharge to</th>
                                                <th class="text-center" width="15%">Area</th>
                                                <th class="text-center" width="5%"></th>
                                                <th class="text-center" width="5%"></th>
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
        <%-- ADD Calculated --%>
        <div class="col-xs-12" id="divPointCalculated">
            <div class="panel">
                <div class="panel panel-primary">
                    <div class="panel-heading" href="#PointCalculated" data-toggle="collapse" style="cursor: pointer;">Calculated</div>
                    <div id="PointCalculated" class="panel-body pad-no collapse in">
                        <div class="form-group cMarTop10 col-md-8 col-sm-12 col-xs-12">
                            <label class="control-label col-xs-12 col-sm-3">Number of point</label>
                            <div class="col-xs-12 col-sm-3">
                                <asp:TextBox ID="txtNumPoint" runat="server" CssClass="form-control text-right"></asp:TextBox>
                            </div>
                            <div class="col-xs-12 col-sm-3">
                                <asp:TextBox ID="txtWater" runat="server" CssClass="form-control text-right"></asp:TextBox>
                            </div>
                            <label class="control-label col-xs-12 col-sm-3">% of water withdraw</label>
                        </div>
                        <div class="form-group cMarTop10 col-md-4 col-sm-12 col-xs-12">
                            <div class="col-xs-12">
                                <button type="button" id="btnConfirmPointCal" class="btn btn-primary" style="display: none; white-space: normal !important;">Confirm Number of point and  % of water withdraw</button>
                                <button type="button" id="btnEditPointCal" class="btn btn-primary" style="white-space: normal !important;">Edit Number of point and  % of water withdraw</button>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12">
                                <button type="button" id="btnAddCalculated" class="btn btn-primary">Add New Point(Calculated)</button>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tbDataAddPointCalculated" class="table dataTable table-bordered table-hover" style="min-width: 100%; width: 952px;">
                                        <thead>
                                            <tr>
                                                <th class="text-center" width="10%">No.</th>
                                                <th class="text-center" width="35%">Point Name</th>
                                                <th class="text-center" width="15%">Treament Method</th>
                                                <th class="text-center" width="15%">Discharge to</th>
                                                <th class="text-center" width="15%">Area</th>
                                                <th class="text-center" width="5%"></th>
                                                <th class="text-center" width="5%"></th>
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
        <%-- POINT OTHER  --%>
        <div class="col-xs-12" id="divEditPoint" style="display: none;">
            <div class="panel">
                <div class="panel panel-info">
                    <div class="panel-heading" href="#Point" data-toggle="collapse" style="cursor: pointer;"><i class="fas fa-edit"></i>Create / Update</div>
                    <div id="Point" class="panel-body pad-no collapse in">
                        <div class="row">
                            <div class="form-group col-xs-12 col-md-6">
                                <label class="control-label col-xs-12 col-md-4 cMarTop10">Point Name<span class="text-red">*</span></label>
                                <div class="col-xs-12 col-md-8 cMarTop10">
                                    <asp:TextBox ID="txtPointName" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-xs-12 col-md-6">
                                <label class="control-label col-xs-12 col-md-4 cMarTop10">Discharge to<span class="text-red">*</span></label>
                                <div class="col-xs-12 col-md-8 cMarTop10">
                                    <asp:DropDownList ID="ddlDischarge" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 col-md-6">
                                <label class="control-label col-xs-12 col-md-4 cMarTop10">Treatment method<span class="text-red">*</span></label>
                                <div class="col-xs-12 col-md-8 cMarTop10">
                                    <asp:DropDownList ID="ddlTreatment" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group col-xs-12 col-md-6">
                                <label class="control-label col-xs-12 col-md-4 cMarTop10">Area<span class="text-red">*</span></label>
                                <div class="col-xs-12 col-md-8 cMarTop10">
                                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-xs-12 col-md-6" id="divTreatmentOther" style="display: none;">
                                <label class="control-label col-xs-12 col-md-4 cMarTop10"></label>
                                <div class="col-xs-12 col-md-8 cMarTop10">
                                    <asp:TextBox ID="txtOther" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <%-- TABLE ADD POINT --%>
                        <div class="form-group col-xs-12 ">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tbDataAddPoint" class="table dataTable table-bordered table-hover" style="min-width: 100%; width: 2032px;">
                                    </table>
                                </div>
                            </div>
                        </div>
                        <%-- Rmark Point --%>
                        <div class="form-group col-xs-12 col-md-6" style="margin-top: 20px;">
                            <div id="divRemarkOther">
                                <div class="well">
                                    <div class="form-group">
                                        <label class="control-label col-xs-12 col-md-6 cMarTop10">Remark Point</label>
                                        <textarea id="txtRemarkPoint" runat="server" class="form-control cMarTop10" rows="4" style="resize: vertical"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- OTHER --%>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12" style="margin-top: 20px">
                                <button type="button" id="btnAddOther" onclick="AddOther();" class="btn btn-primary btn-sm text-left">Add Other</button>
                            </div>
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tbDataAddOther" class="table dataTable table-bordered table-hover" style="min-width: 100%; width: 2032px;">
                                    </table>
                                </div>
                            </div>
                        </div>
                        <%-- BTN SAVE --%>
                        <div class="form-group col-xs-12 ">
                            <div class="col-xs-12 cbtnStyle">
                                <button type="button" id="btnSavePoint" class="btn btn-success">Save this point</button>
                                <button type="button" id="btnCancelPoint" class="btn btn-default ">Cancel this point</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="popOperting" class="modal fade col-xs-12" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false" style="margin-top: 120px;">
        <div class="modal-dialog" role="document" style="width: 80%;">
            <div class="modal-content">
                <div class="modal-header cModal">
                    <h4 class="modal-title">Data Operating hours</h4>
                </div>
                <div class="modal-body">
                    <div id="divPopOperting">
                        <div class="row">
                            <div class="col-xs-12">
                                <label class="control-label">New data operating hours on change number of point :</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tbOperting" class="table dataTable table-bordered table-hover" style="min-width: 100%; width: 2200px;">
                                        <thead>
                                            <tr>
                                                <th class="dt-head-center" style="width: 400px">Point Name</th>
                                                <th class="dt-head-center" style="width: 120px;">Indicator</th>
                                                <th class="dt-head-center" style="width: 120px">Unit</th>
                                                <th class="dt-head-center" style="width: 120px">Target</th>
                                                <th class="dt-head-center" style="width: 120px">Q1:Jan</th>
                                                <th class="dt-head-center" style="width: 120px">Q1:Feb</th>
                                                <th class="dt-head-center" style="width: 120px">Q1:Mar</th>
                                                <th class="dt-head-center" style="width: 120px">Q2:Apr</th>
                                                <th class="dt-head-center" style="width: 120px">Q2:May</th>
                                                <th class="dt-head-center" style="width: 120px">Q2:Jun</th>
                                                <th class="dt-head-center" style="width: 120px">Q3:Jul</th>
                                                <th class="dt-head-center" style="width: 120px">Q3:Aug</th>
                                                <th class="dt-head-center" style="width: 120px">Q3:Sep</th>
                                                <th class="dt-head-center" style="width: 120px">Q4:Oct</th>
                                                <th class="dt-head-center" style="width: 120px">Q4:Nov</th>
                                                <th class="dt-head-center" style="width: 120px">Q4:Dec</th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="dvModelDetail" class="text-right">
                        <button class="btn btn-success" type="button" onclick="SaveOperating();">Confirm this change</button>
                        <button class="btn" type="button" onclick="CancelModal();">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidTypePoint" runat="server" />
    <asp:HiddenField ID="hidOrderPoint" runat="server" />
    <asp:HiddenField ID="hidOperhours" runat="server" />
    <asp:HiddenField ID="hidNumPoint" runat="server" />
    <asp:HiddenField ID="hidWaterPoint" runat="server" />

    <asp:HiddenField ID="hdfsStatus" runat="server" />
    <asp:HiddenField ID="hidClickCHeck" runat="server" />
    <asp:HiddenField ID="hidOpenPoint" runat="server" />
    <%-- ตอนเปิด พ้อยมาแล้ว เปลี่ยนฟังก์ชั่น --%>
    <asp:HiddenField ID="hidClick" runat="server" />
    <asp:HiddenField ID="hidTypeClick" runat="server" />
    <asp:HiddenField ID="hidPointClick" runat="server" />
    <%-- Status --%>
    <asp:HiddenField ID="hidStatusWF" runat="server" />
    <asp:HiddenField ID="hdfIsAdmin" runat="server" />
    <asp:HiddenField ID="hdfRole" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var arrShortMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var arrFullMonth = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var $hidWaterPoint = $("input[id$=hidWaterPoint]");
        var $hidOperhours = $("input[id$=hidOperhours]");
        var $hidNumPoint = $("input[id$=hidNumPoint]");
        var $hidOrderPoint = $("input[id$=hidOrderPoint]");
        var $divContent = $("div#divContent");
        var $divEditPoint = $("div#divEditPoint");
        var $divRemark = $("div#divRemark");
        var $divOther = $("div#divOther");
        var $tbOperting = $("table[id$=tbOperting]");
        var $hidOpenPoint = $("input[id$=hidOpenPoint]");
        var $hidClick = $("input[id$=hidClick]");
        var $hidTypeClick = $("input[id$=hidTypeClick]");
        var $hidPointClick = $("input[id$=hidPointClick]");

        var $tbData = $("table[id$=tbData]");
        var $tbDataAddOther = $("table[id$=tbDataAddOther]");
        var $tbDataAddPoint = $("table[id$=tbDataAddPoint]");
        var arrOther = [];
        var $txtRemarkEff = $("textarea[id$=vtxtRemarkEff]");
        ////// เพิ่มคะแนน        
        var $hidTypePoint = $("input[id$=hidTypePoint]");
        var $txtPointName = $("input[id$=txtPointName]");
        var $ddlDischarge = $("select[id$=ddlDischarge]");
        var $ddlTreatment = $("select[id$=ddlTreatment]");
        var $ddlArea = $("select[id$=ddlArea]");
        var $txtRemarkPoint = $("textarea[id$=txtRemarkPoint]");
        var $btnSavePoint = $("button[id$=btnSavePoint]");
        var $btnCancelPoint = $("button[id$=btnCancelPoint]");
        var arrPoint = [];
        var lstPointInput = [];
        var $btnAddManual = $("button[id$=btnAddManual]");
        var $tbDataAddPointManual = $("table[id$=tbDataAddPointManual]");
        var $btnAddCalculated = $("button[id$=btnAddCalculated]");
        var $tbDataAddPointCalculated = $("table[id$=tbDataAddPointCalculated]");
        var $txtNumPoint = $("input[id$=txtNumPoint]");
        var $txtWater = $("input[id$=txtWater]");
        var $btnConfirmPointCal = $("button[id$=btnConfirmPointCal]");
        var $btnEditPointCal = $("button[id$=btnEditPointCal]");

        var lstIndicatorOther = [];
        var lstUnitOther = [];

        var $hdfRole = $("input[id$=hdfRole]");
        var $hdfIsAdmin = $("input[id$=hdfIsAdmin]");
        var lstStatus = [];
        $(document).ready(function () {
            ArrInputFromTableID.push("tbDataAddOther");
            ArrInputFromTableID.push("tbData");
            ArrInputFromTableID.push("tbDataAddPoint");
            bindDataPoint("M");
            bindDataPoint("C");
            SetControl();
        });
        function SetControl() {
            $ddlDischarge.change(function () {
                var val = $ddlDischarge.val();
                if (val == "3") {
                    $ddlArea.val(14);
                    $ddlArea.prop('disabled', true);
                }
                else {
                    $ddlArea.prop('disabled', false);
                }
            });
            $btnAddCalculated.click(function () {
                $hidOpenPoint.val('Y');
                $hidOrderPoint.val('');
                var num = $txtNumPoint.val() == "" ? 0 : +$txtNumPoint.val();
                var lst = Enumerable.From(lstPointInput).Where(function (w) { return w.cStatus == "Y" && w.sTypePoint == "C" }).ToArray();
                if (num != 0 && lst.length != num) {
                    ClearPoint();
                    SetWater();
                    bindDataOther([]);
                    $hidTypePoint.val("C");
                    $("input[id$=chk_126_1]").removeAttr('checked');
                    $("input[id$=chk_126_2]").prop('checked', true);
                    $("input[id$=chk_126_2]").iCheck('update');
                    $("input[id$=chk_126_1]").iCheck('update');
                    $("tr[id$=127]").hide();
                    $("tr[id$=128]").show();
                    $divEditPoint.show();
                    var objValidate = {};
                    objValidate[GetElementName("txtPointName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Point Name");
                    objValidate[GetElementName("ddlDischarge", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Discharge to");
                    objValidate[GetElementName("ddlTreatment", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Treatment method");
                    objValidate[GetElementName("ddlArea", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Area");
                    BindValidate("Point", objValidate);
                    ScrollTopToElementsTo("divEditPoint", 80);
                    $("button[btnID = 124]").show();
                }
                else {
                    DialogWarning(DialogHeader.Warning, "Over number of point.");
                }
            });
            $btnAddManual.click(function () {
                $hidOpenPoint.val('Y');
                $hidOrderPoint.val('');
                var objValidate = {};
                objValidate[GetElementName("txtPointName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Point Name");
                objValidate[GetElementName("ddlDischarge", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Discharge to");
                objValidate[GetElementName("ddlTreatment", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Treatment method");
                objValidate[GetElementName("ddlArea", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Area");
                BindValidate("Point", objValidate);
                ClearPoint();
                bindDataOther([]);
                for (var i = 1; i <= 12; i++) {
                    var data = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == i });
                    var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
                    if (data.nStatusID == 0) {
                        if ($("input[id$=Chk_" + i + "]").is(':checked')) {
                            for (var x = 0; x < lst.length; x++) {
                                $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").val('0');
                                $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', true);
                            }
                        }
                        else {
                            for (var x = 0; x < lst.length; x++) {
                                $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', false);
                            }
                        }
                    }
                    else {
                        for (var x = 0; x < lst.length; x++) {
                            $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', true);
                        }
                    }
                }
                $hidTypePoint.val("M");
                $("input[id$=chk_126_1]").removeAttr('checked');
                $("input[id$=chk_126_2]").prop('checked', true);
                $("input[id$=chk_126_2]").iCheck('update');
                $("input[id$=chk_126_1]").iCheck('update');
                $("tr[id$=127]").hide();
                $("tr[id$=128]").show();
                $divEditPoint.show();
                ScrollTopToElementsTo("divEditPoint", 80);
                $("button[btnID = 124]").hide();
            });
            $btnCancelPoint.click(function () {
                $hidOpenPoint.val('');
                UpdateStatusValidateControl("Point", $txtPointName, 'NOT_VALIDATED');
                UpdateStatusValidateControl("Point", $ddlDischarge, 'NOT_VALIDATED');
                UpdateStatusValidateControl("Point", $ddlArea, 'NOT_VALIDATED');
                UpdateStatusValidateControl("Point", $ddlTreatment, 'NOT_VALIDATED');
                ClearPoint();
                $divEditPoint.hide();
                DelArry();
            });
            $tbDataAddPoint.delegate("input:not(input.str,input.target)", "change", function () {
                var sVal = $(this).val();
                var Month = $(this).attr("nmonth");
                var ProductID = $(this).attr("productid");
                var sValIN = CheckTextInput(sVal);
                $("input[productid=" + ProductID + "][nmonth=" + Month + "]").val(sValIN);
            });
            $tbDataAddPoint.delegate("input.target", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
            });
            $tbDataAddOther.delegate("input:not(input.str,input.target)", "change", function () {
                var sVal = $(this).val();
                var Month = $(this).attr("nmonth");
                var OtherID = $(this).attr("notherid");
                var sValIN = CheckTextInput(sVal);
                $("input[notherid=" + OtherID + "][nmonth=" + Month + "]").val(sValIN);
            });
            $tbDataAddOther.delegate("input.target", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
            });
            $btnSavePoint.click(function () {
                SavePoint('');
            });
            $txtNumPoint.prop('disabled', true);
            $txtWater.prop('disabled', true);
            $btnEditPointCal.click(function () {
                var objValidate = {};
                objValidate[GetElementName("txtNumPoint", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Number of point");
                objValidate[GetElementName("txtWater", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " % of water withdraw");
                BindValidate("divPointCalculated", objValidate);
                $txtNumPoint.prop('disabled', false);
                $txtWater.prop('disabled', false);
                if ($txtNumPoint.val() != "") {
                    var n = +$txtNumPoint.val();
                    var val = 1 / n;
                    $hidOperhours.val(val);
                }
                $btnEditPointCal.hide();
                $btnConfirmPointCal.show();
                $btnAddCalculated.hide();
                $btnAddManual.hide();
                $("button.btnEdit").prop('disabled', true);
            });
            $btnConfirmPointCal.click(function () {
                var IsPass = CheckValidate("divPointCalculated");
                if (IsPass) {
                    UpdateStatusValidateControl("divPointCalculated", $txtNumPoint, 'NOT_VALIDATED');
                    UpdateStatusValidateControl("divPointCalculated", $txtWater, 'NOT_VALIDATED');
                    var lstData = Enumerable.From(lstPointInput).Where(function (w) { return w.sTypePoint == "C" && w.cStatus == "Y" }).ToArray();
                    if (lstData.length > 0 && $hidNumPoint.val() != "") {
                        if ($hidNumPoint.val() != $txtNumPoint.val()) {
                            var nNew = 1 / (+$txtNumPoint.val());
                            var sOld = $hidOperhours.val();
                            var sHtml = "";
                            $("table[id$=tbOperting] tbody tr").remove();
                            $.each(Enumerable.From(lstPointInput).Where(function (w) { return w.cStatus == "Y" && w.sTypePoint == "C" }).ToArray(), function (i, el) {
                                sHtml += "<tr id='" + el.nPointID + "'>";
                                sHtml += " <td class='text-left'>" + el.sPointName + "</td>"; //setTooltipProduct(el.sTooltip) +
                                sHtml += " <td class='text-left'>Operating hours</td>";
                                sHtml += " <td class='text-left'>hour/month</td>";
                                var data = Enumerable.From(arrPoint).FirstOrDefault(null, function (s) { return s.nPointID == el.nPointID && s.sTypePoint == "C" && s.ProductID == 125 });
                                sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' pointID='" + el.nPointID + "' value='" + (data.Target != null ? CheckTextInput(data.Target) : "") + "' maxlength='20'></td>";
                                sHtml += "<td class='text-center M_1'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='1' value ='' /></td>";
                                sHtml += "<td class='text-center M_2'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='2' value ='' /></td>";
                                sHtml += "<td class='text-center M_3'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='3' value ='' /></td>";
                                sHtml += "<td class='text-center M_4'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='4' value ='' /></td>";
                                sHtml += "<td class='text-center M_5'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='5' value ='' /></td>";
                                sHtml += "<td class='text-center M_6'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='6' value ='' /></td>";
                                sHtml += "<td class='text-center M_7'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='7' value ='' /></td>";
                                sHtml += "<td class='text-center M_8'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='8' value ='' /></td>";
                                sHtml += "<td class='text-center M_9'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='9' value ='' /></td>";
                                sHtml += "<td class='text-center M_10'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='10' value ='' /></td>";
                                sHtml += "<td class='text-center M_11'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='11' value ='' /></td>";
                                sHtml += "<td class='text-center M_12'><input ProductID='" + data.ProductID + "' pointID='" + el.nPointID + "' class='form-control' nMonth ='12' value ='' /></td>";
                                sHtml += "</tr>";
                            });
                            $("table[id$=tbOperting] tbody").append(sHtml);

                            $.each(Enumerable.From(lstPointInput).Where(function (w) { return w.cStatus == "Y" && w.sTypePoint == "C" }).ToArray(), function (i, el) {
                                var data = Enumerable.From(arrPoint).FirstOrDefault(null, function (s) { return s.nPointID == el.nPointID && s.sTypePoint == "C" && s.ProductID == 125 });
                                $("input[pointID=" + el.nPointID + "].target").val(data.Target);
                                for (var j = 1; j <= 12; j++) {
                                    var status = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == j });
                                    if (status.nStatusID == 0) {
                                        var val = getDataInArr(data, j);
                                        if (val == sOld) val = nNew;
                                        else val = CheckTextInput(val);
                                        if ($("input[id$=Chk_" + j + "]").is(':checked')) {
                                            for (var x = 0; x < lst.length; x++) {
                                                $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =" + j + "]").val('0');
                                                $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =" + j + "]").prop('disabled', true);
                                            }
                                        }
                                        else {
                                            $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =" + j + "]").prop('disabled', false);
                                            $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =" + j + "]").val(val);
                                        }
                                    }
                                    else {
                                        var val = getDataInArr(data, j);
                                        val = CheckTextInput(val);
                                        $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =" + j + "]").prop('disabled', true);
                                        $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =" + j + "]").val(val);
                                    }
                                }
                            });
                            $("#popOperting").modal('show');
                        }
                        else {
                            var val = $txtNumPoint.val();
                            var valWater = $txtWater.val();
                            $hidNumPoint.val(val);
                            $hidWaterPoint.val(valWater);
                            $txtNumPoint.prop('disabled', true);
                            $txtWater.prop('disabled', true);
                            $btnEditPointCal.show();
                            $btnConfirmPointCal.hide();
                            ClearPoint(); $divEditPoint.hide();
                        }
                    }
                    else {
                        var val = $txtNumPoint.val();
                        var valWater = $txtWater.val();
                        $hidNumPoint.val(val);
                        $hidWaterPoint.val(valWater);
                        $txtNumPoint.prop('disabled', true);
                        $txtWater.prop('disabled', true);
                        $btnEditPointCal.show();
                        $btnConfirmPointCal.hide();
                        ClearPoint(); $divEditPoint.hide();
                    }
                    $btnAddCalculated.show();
                    $btnAddManual.show();
                    $("button.btnEdit").prop('disabled', false);
                }
            });
            $tbData.delegate("input.target", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
                var ProductID = $(this).attr("productid");
                var nVal = $(this).val();
                if (nVal != "") {
                    nVal = nVal.replace(/,/g, '');
                }
                var val = CheckTextInput(nVal);
                $("input[productid=" + ProductID + "].target").val(val);
                var d = Enumerable.From(lstProduct).FirstOrDefault(null, function (s) { return s.ProductID == ProductID });
                d.Target = nVal;
            });
            $tbData.delegate("input[type=checkbox]", "ifChanged", function () {
                var el = $(this);
                var nMonth = +el.attr("nMonth");
                if (el.is(":checked")) {
                    var sComfirm = "The action comes into effect in February, all data within this period will be automatically removed in result.Do you want save data ?";
                    DialogConfirmCloseButton(DialogHeader.Confirm, sComfirm, function () {
                        var IsCheck = el.is(":checked") ? "Y" : "N";
                        var lstHead = Enumerable.From(lstProduct).Where(function (w) { return w.cTotal == "Y" }).ToArray();
                        for (var i = 0; i < lstHead.length; i++) {
                            switch (nMonth) {
                                case 1:
                                    lstHead[i].M1 = "0";;
                                    lstHead[i].IsCheckM1 = IsCheck;
                                    break;
                                case 2:
                                    lstHead[i].M2 = "0";;
                                    lstHead[i].IsCheckM2 = IsCheck;
                                    break;
                                case 3:
                                    lstHead[i].M3 = "0";;
                                    lstHead[i].IsCheckM3 = IsCheck;
                                    break;
                                case 4:
                                    lstHead[i].M4 = "0";;
                                    lstHead[i].IsCheckM4 = IsCheck;
                                    break;
                                case 5:
                                    lstHead[i].M5 = "0";;
                                    lstHead[i].IsCheckM5 = IsCheck;
                                    break;
                                case 6:
                                    lstHead[i].M6 = "0";;
                                    lstHead[i].IsCheckM6 = IsCheck;
                                    break;
                                case 7:
                                    lstHead[i].M7 = "0";;
                                    lstHead[i].IsCheckM7 = IsCheck;
                                    break;
                                case 8:
                                    lstHead[i].M8 = "0";;
                                    lstHead[i].IsCheckM8 = IsCheck;
                                    break;
                                case 9:
                                    lstHead[i].M9 = "0";;
                                    lstHead[i].IsCheckM9 = IsCheck;
                                    break;
                                case 10:
                                    lstHead[i].M10 = "0";;
                                    lstHead[i].IsCheckM10 = IsCheck;
                                    break;
                                case 11:
                                    lstHead[i].M11 = "0";;
                                    lstHead[i].IsCheckM11 = IsCheck;
                                    break;
                                case 12:
                                    lstHead[i].M12 = "0";;
                                    lstHead[i].IsCheckM12 = IsCheck;
                                    break;
                            }
                        }
                        for (var g = 0; g < arrPoint.length; g++) {
                            switch (nMonth) {
                                case 1:
                                    arrPoint[g].M1 = "0";
                                    arrPoint[g].IsCheckM1 = IsCheck;
                                    break;
                                case 2:
                                    arrPoint[g].M2 = "0";
                                    arrPoint[g].IsCheckM2 = IsCheck;
                                    break;
                                case 3:
                                    arrPoint[g].M3 = "0";
                                    arrPoint[g].IsCheckM3 = IsCheck;
                                    break;
                                case 4:
                                    arrPoint[g].M4 = "0";
                                    arrPoint[g].IsCheckM4 = IsCheck;
                                    break;
                                case 5:
                                    arrPoint[g].M5 = "0";
                                    arrPoint[g].IsCheckM5 = IsCheck;
                                    break;
                                case 6:
                                    arrPoint[g].M6 = "0";
                                    arrPoint[g].IsCheckM6 = IsCheck;
                                    break;
                                case 7:
                                    arrPoint[g].M7 = "0";
                                    arrPoint[g].IsCheckM7 = IsCheck;
                                    break;
                                case 8:
                                    arrPoint[g].M8 = "0";
                                    arrPoint[g].IsCheckM8 = IsCheck;
                                    break;
                                case 9:
                                    arrPoint[g].M9 = "0";
                                    arrPoint[g].IsCheckM9 = IsCheck;
                                    break;
                                case 10:
                                    arrPoint[g].M10 = "0";
                                    arrPoint[g].IsCheckM10 = IsCheck;
                                    break;
                                case 11:
                                    arrPoint[g].M11 = "0";
                                    arrPoint[g].IsCheckM11 = IsCheck;
                                    break;
                                case 12:
                                    arrPoint[g].M12 = "0";
                                    arrPoint[g].IsCheckM12 = IsCheck;
                                    break;
                            }
                            //setDataInArr(arrPoint[g], nMonth, "0", false);
                        }
                        for (var i = 1; i <= 12; i++) {
                            if ($("input[id$=Chk_" + i + "]").is(':checked')) {
                                var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
                                for (var x = 0; x < lst.length; x++) {
                                    $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").val('0');
                                }
                            }
                        }
                        CalPointToHead();
                        if ($hidOpenPoint.val() != "") $divEditPoint.hide(); $hidOpenPoint.val('');

                        HideLoadding();
                    }, function () {
                        $("input[id$=hidClickCHeck]").val('cancel');
                        el.iCheck('Uncheck');
                    });
                } else {
                    if ($("input[id$=hidClickCHeck]").val() == "cancel") {
                        $("input[id$=hidClickCHeck]").val('');
                    }
                    else {
                        var nMonth = +el.attr("nMonth");
                        var IsCheck = el.is(":checked") ? "Y" : "N";
                        var lstHead = Enumerable.From(lstProduct).Where(function (w) { return w.cTotal == "Y" }).ToArray();
                        for (var i = 0; i < lstHead.length; i++) {
                            switch (nMonth) {
                                case 1:
                                    lstHead[i].M1 = "";
                                    lstHead[i].IsCheckM1 = IsCheck;
                                    break;
                                case 2:
                                    lstHead[i].M2 = "";
                                    lstHead[i].IsCheckM2 = IsCheck;
                                    break;
                                case 3:
                                    lstHead[i].M3 = "";
                                    lstHead[i].IsCheckM3 = IsCheck;
                                    break;
                                case 4:
                                    lstHead[i].M4 = "";
                                    lstHead[i].IsCheckM4 = IsCheck;
                                    break;
                                case 5:
                                    lstHead[i].M5 = "";
                                    lstHead[i].IsCheckM5 = IsCheck;
                                    break;
                                case 6:
                                    lstHead[i].M6 = "";
                                    lstHead[i].IsCheckM6 = IsCheck;
                                    break;
                                case 7:
                                    lstHead[i].M7 = "";
                                    lstHead[i].IsCheckM7 = IsCheck;
                                    break;
                                case 8:
                                    lstHead[i].M8 = "";
                                    lstHead[i].IsCheckM8 = IsCheck;
                                    break;
                                case 9:
                                    lstHead[i].M9 = "";
                                    lstHead[i].IsCheckM9 = IsCheck;
                                    break;
                                case 10:
                                    lstHead[i].M10 = "";
                                    lstHead[i].IsCheckM10 = IsCheck;
                                    break;
                                case 11:
                                    lstHead[i].M11 = "";
                                    lstHead[i].IsCheckM11 = IsCheck;
                                    break;
                                case 12:
                                    lstHead[i].M12 = "";
                                    lstHead[i].IsCheckM12 = IsCheck;
                                    break;
                            }
                        }
                        for (var g = 0; g < arrPoint.length; g++) {
                            switch (nMonth) {
                                case 1:
                                    arrPoint[g].M1 = "";
                                    arrPoint[g].IsCheckM1 = IsCheck;
                                    break;
                                case 2:
                                    arrPoint[g].M2 = "";
                                    arrPoint[g].IsCheckM2 = IsCheck;
                                    break;
                                case 3:
                                    arrPoint[g].M3 = "";
                                    arrPoint[g].IsCheckM3 = IsCheck;
                                    break;
                                case 4:
                                    arrPoint[g].M4 = "";
                                    arrPoint[g].IsCheckM4 = IsCheck;
                                    break;
                                case 5:
                                    arrPoint[g].M5 = "";
                                    arrPoint[g].IsCheckM5 = IsCheck;
                                    break;
                                case 6:
                                    arrPoint[g].M6 = "";
                                    arrPoint[g].IsCheckM6 = IsCheck;
                                    break;
                                case 7:
                                    arrPoint[g].M7 = "";
                                    arrPoint[g].IsCheckM7 = IsCheck;
                                    break;
                                case 8:
                                    arrPoint[g].M8 = "";
                                    arrPoint[g].IsCheckM8 = IsCheck;
                                    break;
                                case 9:
                                    arrPoint[g].M9 = "";
                                    arrPoint[g].IsCheckM9 = IsCheck;
                                    break;
                                case 10:
                                    arrPoint[g].M10 = "";
                                    arrPoint[g].IsCheckM10 = IsCheck;
                                    break;
                                case 11:
                                    arrPoint[g].M11 = "";
                                    arrPoint[g].IsCheckM11 = IsCheck;
                                    break;
                                case 12:
                                    arrPoint[g].M12 = "";
                                    arrPoint[g].IsCheckM12 = IsCheck;
                                    break;
                            }
                            // setDataInArr(arrPoint[g], nMonth, "", false);
                        }
                        for (var i = 1; i <= 12; i++) {
                            if (!$("input[id$=Chk_" + i + "]").is(':checked')) {
                                var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
                                for (var x = 0; x < lst.length; x++) {
                                    $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").val('');
                                }
                            }
                        }
                        CalPointToHead();
                        if ($hidOpenPoint.val() != "") $divEditPoint.hide(); $hidOpenPoint.val('');
                    }

                }
            });
            $txtNumPoint.change(function () {
                var val = $txtNumPoint.val();
                val = CheckTextNum(val);
                $txtNumPoint.val(val);
            });
            $txtWater.change(function () {
                var val = $txtWater.val();
                val = CheckTextInput(val);
                $txtWater.val(val);
            });
            $ddlTreatment.change(function () {
                if ($(this).val() == "999") {
                    $("div#divTreatmentOther").show();
                }
                else {
                    $("input[id$=txtOther]").val('');
                    $("div#divTreatmentOther").hide();
                }
            });
            $tbOperting.delegate("input:not(input.str,input.target)", "change", function () {
                var sVal = $(this).val();
                var Month = $(this).attr("nmonth");
                var ProductID = $(this).attr("productid");
                var sValIN = CheckTextInput(sVal);
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
            });
            $tbOperting.delegate("input.target", "change", function () {
                $(this).val(CheckTextInput($(this).val().replace(/,/g, '')));
            });
        }
        function LoadData() {
            var param = {
                sIndID: Select("ddlIndicator").val(),
                sOprtID: Select("ddlOperationType").val(),
                sFacID: Select("ddlFacility").val(),
                sYear: Select("Year").val()
            }
            IsFullMonth = true;
            LoaddinProcess();
            AjaxCallWebMethod("LoadData", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                } else {
                    $txtNumPoint.prop('disabled', true);
                    $txtWater.prop('disabled', true);
                    $btnEditPointCal.show();
                    $btnConfirmPointCal.hide();
                    lstStatus.length = 0;
                    lstStatus = response.d.lstStatus;
                    lstProduct = response.d.lstProduct;
                    nStatus = response.d.nStatus;
                    $hdfPRMS.val(response.d.hdfPRMS);
                    SetValueTextArea("txtRemarkEff", response.d.sRemarkTotal)
                    lstIndicatorOther = response.d.lstIndicatorOther;
                    lstUnitOther = response.d.lstUnitOther;
                    arrOther = response.d.lstOther;
                    lstPointInput = response.d.lstPointInput;
                    arrPoint = response.d.lstPoint;
                    bindDataHead();
                    bindAddPoint(response.d);
                    bindDataPoint("M");
                    bindDataPoint("C");
                    $.each(lstStatus, function (i, el) {
                        if (el.nStatusID == 0) {
                            IsFullMonth = false;
                        }
                    });
                    if (IsFullMonth) {
                        if (($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") || $hdfIsAdmin.val() == "Y") {
                            // $btnAddNon_Complaint.show();
                            $btnAddCalculated.show();
                            $btnAddManual.show();
                        } else {
                            /// 30-01-2020 เพิ่มเช็ค L2 แก้ไขได้ทุกอย่างเหมือน superAdmin
                            if ($hdfIsAdmin.val() != "Y" && $hdfRole.val() != "4") {
                                $btnAddCalculated.hide();
                                $btnAddManual.hide();
                            }
                        }
                    } else {
                        $btnAddCalculated.show();
                        $btnAddManual.show();
                    }
                    $txtNumPoint.val(response.d.sNumPoint);
                    $txtWater.val(response.d.sPercentWater);
                    $hidNumPoint.val(response.d.sNumPoint);
                    $hidWaterPoint.val(response.d.sPercentWater);
                }
            }, function () {
                // setPRMS();
                $divRemark.show();
                $divContent.show();
                $divOther.show();
                CheckboxQuarterChanged();
                SetDisableMonth();
                $divEditPoint.hide();
                //SetTootip();
                HideLoadding();
            }, { param: param });
        }
        function SetDisableMonth() {
            $("textarea[id$=txtRemarkEff]").prop("disabled", false);
            $("textarea[id$=txtRemarkPoint]").prop("disabled", false);
            $txtPointName.prop("disabled", false);
            $ddlArea.prop("disabled", false);
            $ddlDischarge.prop("disabled", false);
            $ddlTreatment.prop("disabled", false);
            $btnSavePoint.show();
            $btnEditPointCal.show();
            $("button[id$=btnAddOther]").show();

            var IsFullMonth = true;
            $.each(lstStatus, function (i, el) {
                if (el.nStatusID == 0) {
                    IsFullMonth = false;
                    ///สั่งปิดที่ไม่ได้อีดิทคอนเทนมา
                    if (el.nStatusID == 0 && $hdfsStatus.val() != "") {
                        $("input[nmonth=" + el.nMonth + "]").prop('disabled', true);
                    }
                }
                else if (el.nStatusID > 0) {
                    $("input[nmonth=" + el.nMonth + "]").prop('disabled', true);

                    ////เข้ามาappove eidth eidt content
                    if ($hdfsStatus.val() != "" && el.nStatusID == 2) {
                        $("input[nmonth=" + el.nMonth + "]").prop('disabled', false);
                    }

                    ///สั่งปิดที่ไม่ได้อีดิทคอนเทนมา
                    if (el.nStatusID == 0 && $hdfsStatus.val() != "") {
                        $("input[nmonth=" + el.nMonth + "]").prop('disabled', true);
                    }
                }
            });

            if (nStatus == 1) {
                $("button.btn-danger").prop("disabled", true);
                $("select[id*=ddlInd_]").prop('disabled', true);
                $("select[id*=ddlUni_]").prop('disabled', true);
            }
            else {
                $("button.btn-danger").prop("disabled", false);
                $("select[id*=ddlInd_]").prop('disabled', false);
                $("select[id*=ddlUni_]").prop('disabled', false);
            }

            if (IsFullMonth || $hdfPRMS.val() == 1) {
                /// 30-01-2020 เพิ่มเช็ค L2 แก้ไขได้ทุกอย่างเหมือน superAdmin
                if ($hdfIsAdmin.val() != "Y" && $hdfRole.val() != "4") {
                    $("input.target").prop("disabled", true);
                    $("textarea[id$=txtRemarkEff]").prop("disabled", true);
                    $("textarea[id$=txtRemarkPoint]").prop("disabled", true);
                    // $("input[id*=_Name_]").prop('disabled', true);   
                    $("button[id$=btnAddOther]").hide();
                    $btnSavePoint.hide();
                    $btnEditPointCal.hide();
                    $txtPointName.prop("disabled", true);
                    $ddlArea.prop("disabled", true);
                    $ddlDischarge.prop("disabled", true);
                    $ddlTreatment.prop("disabled", true);
                    $("button[btnID=124]").hide();
                    // เพิ่มวันที่ 26/04/2562
                    for (var i = 1; i <= 12; i++) {
                        $("input[nmonth=" + i + "]").prop('disabled', true);
                    }
                    $btnAddManual.hide();
                    $btnAddCalculated.hide();
                }
            }

            if (lstStatus.length > 0) {
                if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") {
                    $("textarea[id$=txtRemarkEff]").prop("disabled", false);
                    $("textarea[id$=txtRemarkPoint]").prop("disabled", false);
                    $("input.target").prop("disabled", false);
                    $txtPointName.prop("disabled", false);
                    $ddlArea.prop("disabled", false);
                    $ddlDischarge.prop("disabled", false);
                    $ddlTreatment.prop("disabled", false);
                    $btnSavePoint.show();
                    $("button[id$=btnAddOther]").show();
                    $btnEditPointCal.show();
                    for (var i = 1; i <= 12; i++) {
                        //// แก้วันที่ 28/01/2563 ตามwaste สามารถแก้ไขได้หมด
                        $("input[nmonth=" + i + "]").prop('disabled', false);
                    }
                    // $("input[id*=_A_M]").prop("disabled", true);                    
                }
                if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() != "") {
                    $.each(lstStatus, function (i2, el2) {
                        if (el2.nStatusID == 2) {
                            $("input[nmonth=" + el2.nMonth + "]").prop('disabled', false);
                        }
                    });
                }
            }

            CheckEventButton();
        }
        function bindDataHead() {
            var sHtml = "";
            $tbData.empty();
            sHtml += "<thead>";
            sHtml += "  <tr>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthIndicator + "px;'><label>Indicator</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'><label>Unit</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;'><label>Target</label></th>";

            var lstHead = Enumerable.From(lstProduct).Where(function (w) { return w.cTotal == "Y" }).ToArray();
            if (lstHead.length > 0) {
                $.each(Enumerable.From(lstHead).Where(function (w) { return w.sType == 1 }).ToArray(), function (i, el) {
                    sHtml += bindColumnQ(true, null, el);
                    sHtml += "  </tr>";
                    sHtml += "</thead>";
                    sHtml += "<tbody>";

                    sHtml += "<tr class='cTotalYY' id='" + el.ProductID + "'>";
                    sHtml += " <td class='text-left'>" + el.ProductName + "</td>"; //setTooltipProduct(el.sTooltip) +
                    sHtml += " <td class='text-center'>" + el.sUnit + "</td>";
                    sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' ProductID='" + el.ProductID + "' value='" + (el.Target != null ? CheckTextOutput(el.Target) : "") + "' maxlength='20'></td>";
                    sHtml += bindColumHead(false, el);
                    sHtml += "</tr>";
                });
                $.each(Enumerable.From(lstHead).Where(function (w) { return w.sType != 1 && w.sStatus == "Y" }).ToArray(), function (i, el) {
                    sHtml += "<tr class='' id='" + el.ProductID + "'>";
                    sHtml += " <td class='text-left'>" + el.ProductName + "</td>"; //setTooltipProduct(el.sTooltip) +
                    sHtml += " <td class='text-center'>" + el.sUnit + "</td>";
                    sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' ProductID='" + el.ProductID + "' value='" + (el.Target != null ? CheckTextOutput(el.Target) : "") + "' maxlength='20'></td>";
                    sHtml += bindColumHead(false, el);
                    sHtml += "</tr>";
                });
                sHtml += "</tbody>";
            }
            $tbData.html(sHtml);
            var dHead = Enumerable.From(lstHead).FirstOrDefault(null, function (s) { return s.sType == 1 });
            if (dHead.IsCheckM1 == "Y") $("input[id$=Chk_1]").prop('checked', true);
            if (dHead.IsCheckM2 == "Y") $("input[id$=Chk_2]").prop('checked', true);
            if (dHead.IsCheckM3 == "Y") $("input[id$=Chk_3]").prop('checked', true);
            if (dHead.IsCheckM4 == "Y") $("input[id$=Chk_4]").prop('checked', true);
            if (dHead.IsCheckM5 == "Y") $("input[id$=Chk_5]").prop('checked', true);
            if (dHead.IsCheckM6 == "Y") $("input[id$=Chk_6]").prop('checked', true);
            if (dHead.IsCheckM7 == "Y") $("input[id$=Chk_7]").prop('checked', true);
            if (dHead.IsCheckM8 == "Y") $("input[id$=Chk_8]").prop('checked', true);
            if (dHead.IsCheckM9 == "Y") $("input[id$=Chk_9]").prop('checked', true);
            if (dHead.IsCheckM10 == "Y") $("input[id$=Chk_10]").prop('checked', true);
            if (dHead.IsCheckM11 == "Y") $("input[id$=Chk_11]").prop('checked', true);
            if (dHead.IsCheckM12 == "Y") $("input[id$=Chk_12]").prop('checked', true);

            $('.flat-green-custom').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_square-green' //'iradio_flat-green'
            });
            $tbData.tableHeadFixer({ "left": 1 }, { "head": true });
        }
        function bindAddPoint(lst) {
            var sHtml = "";
            $tbDataAddPoint.empty();
            sHtml += "<thead>";
            sHtml += "  <tr>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthIndicator + "px;'><label>Indicator</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'><label>Unit</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;' ><label>Target</label></th>";
            sHtml += bindColumHead(true, [], 2);
            sHtml += "  </tr>";
            //sHtml += "  <tr>";
            //sHtml += bindColumHead(true, [], 2);
            //sHtml += "  </tr>";
            sHtml += "</thead>";
            var lstPoint = Enumerable.From(lst.lstProduct).Where(function (w) { return w.cTotal == "N" }).ToArray();
            if (lstPoint.length > 0) {
                sHtml += "<tbody>";
                //// เพิ่มหัวข้อเอง
                sHtml += "<tr class='cTotalYN'>";
                sHtml += " <td class='text-left'><b>General Information</b></td>";
                sHtml += " <td class='text-center'></td>";
                sHtml += " <td class='cTarget'></td>";
                sHtml += bindDataColum(true, false);
                sHtml += "</tr>";
                $.each(Enumerable.From(lstPoint).Where(function (w) { return w.nGroupCalc == 1 }).ToArray(), function (i, el) {
                    var btnRecal = "";
                    if (el.ProductID == 124) {
                        btnRecal = "<button btnID='124' Type='button' class='btn btn-sm btn-info' onclick='RecalQuter();'>Recalculate</button>";
                    }
                    sHtml += "<tr id='" + el.ProductID + "'>";
                    sHtml += " <td class='text-left'>" + el.ProductName + "&nbsp;" + btnRecal + "</td>"; //setTooltipProduct(el.sTooltip) +
                    sHtml += " <td class='text-center'>" + el.sUnit + "</td>";
                    sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' ProductID='" + el.ProductID + "' value='" + (el.Target != null ? CheckTextInput(el.Target) : "") + "' maxlength='20'></td>";
                    sHtml += bindDataColum(false, false, el, false);
                    sHtml += "</tr>";
                });
                //// วนหัวข้อปกติ
                $.each(Enumerable.From(lstPoint).Where(function (w) { return w.sType == "2H" }).ToArray(), function (i, el) {
                    sHtml += "<tr class='cTotalYN'>";
                    sHtml += " <td class='text-left'><b>" + el.ProductName + "</b></td>";
                    sHtml += " <td class='text-center'></td>";
                    sHtml += " <td class='cTarget'></td>";
                    sHtml += bindDataColum(true, false);
                    sHtml += "</tr>";
                    //// COD
                    if (el.ProductID == 126) {
                        sHtml += "<tr>";
                        sHtml += "<td class='text-left' colspan='6'>";
                        sHtml += "<input type='radio' name='COD" + el.ProductID + "' id='chk_" + el.ProductID + "_1" + "'  class='flat-green radio radio-inline' value='1'>";
                        sHtml += "&nbsp; Option 1: COD Load data available from online device or Periodic Sampling (figure from online device is preferred)<br/><br/>";
                        sHtml += "<input type='radio' name='COD" + el.ProductID + "' id='chk_" + el.ProductID + "_2" + "' checked  class='flat-green radio radio-inline' value='2'>";
                        sHtml += "&nbsp;Option 2: COD Concentration data available from online device or Periodic Sampling (figure from online device is preferred)";
                        sHtml += "</td>";
                        sHtml += "</tr>";
                    }
                    $.each(Enumerable.From(lstPoint).Where(function (w) { return w.sType == "2" && w.nGroupCalc == el.nGroupCalc }).ToArray(), function (i, el2) {
                        sHtml += "<tr class='' id='" + el2.ProductID + "'>";
                        sHtml += " <td class='text-left'>" + el2.ProductName + "</td>"; //setTooltipProduct(el.sTooltip) +
                        sHtml += " <td class='text-center'>" + el2.sUnit + "</td>";
                        sHtml += " <td class='cTarget'><input id='' class='form-control input-sm target' ProductID='" + el2.ProductID + "' value='" + (el2.Target != null ? CheckTextInput(el2.Target) : "") + "' maxlength='20'></td>";
                        sHtml += bindDataColum(false, false, el2, false);
                        sHtml += "</tr>";
                    });
                });

                sHtml += "</tbody>";
            }
            $tbDataAddPoint.html(sHtml);

            //// checked RADIO
            if ($("input[name$=COD126]:checked").val() == "1") { ///$("input[name$=COD126]:checked").val() == "U")
                $("tr[id$=127]").show();
                $("tr[id$=128]").hide();
            }
            else {
                $("tr[id$=127]").hide();
                $("tr[id$=128]").show();
            }
            $('input[id$=chk_126_2].flat-green').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_square-green'//'iradio_flat-green'
            });
            $("input[id$=chk_126_2]").on("ifChanged", function () {
                $("tr[id$=127]").hide();
                $("tr[id$=128]").show();
            });
            $('input[id$=chk_126_1].flat-green').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_square-green'//'iradio_flat-green'
            });
            $("input[id$=chk_126_1]").on("ifChanged", function () {
                $("tr[id$=127]").show();
                $("tr[id$=128]").hide();
            });
            $tbDataAddPoint.tableHeadFixer({ "left": 1 }, { "head": true });
        }
        function SaveData(Status) {
            if ($hidOpenPoint.val() != "" && (Status == "1" || Status == "0")) {
                DialogWarning(DialogHeader.Warning, "Please save or cancel action point.");
            }
            else {
                var isPass = true;
                var sMsg = "";
                var sMsgComfirmAlert = DialogMsg.ConfirmSave;
                var sMsgComplete = DialogMsg.SaveComplete;
                switch (+Status) {
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

                var sRemark = GetValTextArea("txtRemarkEff");
                var dataTotal = Enumerable.From(lstProduct).FirstOrDefault(null, function (s) { return s.ProductID == 115 });
                var arrTotalData = [dataTotal.M1, dataTotal.M2, dataTotal.M3, dataTotal.M4, dataTotal.M5, dataTotal.M6, dataTotal.M7, dataTotal.M8, dataTotal.M9, dataTotal.M10, dataTotal.M11, dataTotal.M12]
                var arrTotalCheck = [dataTotal.IsCheckM1, dataTotal.IsCheckM2, dataTotal.IsCheckM3, dataTotal.IsCheckM4, dataTotal.IsCheckM5, dataTotal.IsCheckM6, dataTotal.IsCheckM7, dataTotal.IsCheckM8, dataTotal.IsCheckM9, dataTotal.IsCheckM10, dataTotal.IsCheckM11, dataTotal.IsCheckM12]

                if (Status == 0 || Status == 9999) {
                    for (var i = 0; i < 12 ; i++) {
                        if (arrTotalData[i] != "") { // arrTotalCheck[i] == "Y" ||
                            isPass = true;
                        }
                    }
                    if (!isPass) {
                        DialogWarning(DialogHeader.Warning, "Please specify data");
                        return false;
                    }
                }
                else if (Status == 24 || Status == 2) {
                    isPass = true;
                }
                else {
                    for (var i = 0; i < 12 ; i++) {
                        var IsThisMonth = arrMonth.indexOf((i + 1) + "") > -1;
                        if (IsThisMonth) {
                            if (arrTotalData[i] == "") { //arrTotalCheck[i] != "Y" &&
                                isPass = false
                                sMsg += "<br/>&nbsp;- Please specify " + arrFullMonth[i];
                            }
                        }
                    }
                    if (sRemark == "") {
                        isPass = false
                        sMsg += "<br/>&nbsp;- Please specify Remark Effluent";
                    }
                }

                if (isPass) {
                    var arrData = {
                        sRemarkTotal: GetValTextArea("txtRemarkEff"),
                        nIndicatorID: $ddlIndicator.val(),
                        nOperationID: $ddlOperationType.val(),
                        nFacilityID: $ddlFacility.val(),
                        sYear: $ddlYear.val(),
                        lstProduct: lstProduct,
                        lstMonthSubmit: arrMonth,
                        nStatus: +Status,
                        lstPointInput: lstPointInput,
                        lstPoitn: arrPoint,
                        lstProductOther: arrOther,
                        sNumPoint: $txtNumPoint.val(),
                        sPercentWater: $txtWater.val(),
                    }
                    if (Status != "1") {
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
                            DialogConfirm(DialogHeader.Confirm, sMsgComfirmAlert, function () {
                                LoaddinProcess();
                                arrData.nStatus = 0;
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
                                    lstPrdDeviate.push(dataTotal);
                                    Deviate(lstPrdDeviate, Status);
                                }, { arrData: arrData })
                            }, function () { HideLoadding(); })
                        } else {
                            LoaddinProcess();
                            AjaxCallWebMethod("saveToDB", function (response) {
                                if (response.d.Status == SysProcess.SessionExpired) {
                                    PopupLogin();
                                } else if (response.d.Status == SysProcess.Success) {
                                    DialogSuccess(DialogHeader.Success, sMsgComplete);
                                    LoadDataCheckddl();
                                } else {
                                    DialogWarning(DialogHeader.Warning, response.d.Msg);
                                }
                            }, function () { HideLoadding(); }, { arrData: arrData })
                        }
                    }
                } else {
                    DialogWarning(DialogHeader.Warning, "&bull; " + dataTotal.ProductName + sMsg);
                }
            }
        }
        function RecalQuter() {
            DialogConfirm(DialogHeader.Confirm, "Do you want to recalculate ?", function () {
                var param = {
                    sIndID: Select("ddlIndicator").val(),
                    sOprtID: Select("ddlOperationType").val(),
                    sFacID: Select("ddlFacility").val(),
                    sYear: Select("Year").val()
                }

                AjaxCallWebMethod("SetDataWater", function (response) {
                    if (response.d.Status == SysProcess.SessionExpired) {
                        PopupLogin();
                    } else {
                        for (var i = 1; i <= 12; i++) {
                            var d = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == i });
                            if (d.nStatusID == 0) {
                                var sVal = getDataInArr(response.d, i);
                                $("input[productid=124][nmonth=" + i + "]").val(sVal);
                            }
                        }
                    }
                }, function () {
                    HideLoadding();
                }, { param: param, sPercent: $txtWater.val() });
            }, function () { HideLoadding(); });
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
                    sHtml += "<th class='text-center M_" + i + " QHead_" + getQrt(i) + "'><input id='Chk_" + i + "' type='checkbox' nMonth ='" + i + "' class='flat-green-custom " + (strForDisabled != "" ? "submited" : "") + "' " + IsCheck + " " + strForDisabled + " />&nbsp;<label>Q" + getQrt(i) + " : " + arrShortMonth[i - 1] + "</label></th>";
                }
            } else {
                for (var i = 1 ; i <= 12; i++) {
                    sHtml += "<td class='text-center M_" + i + " QHead_" + getQrt(i) + "'><input class='form-control' nMonth ='" + i + "' value ='" + CheckTextInput(dataMonth[i - 1]) + "' disabled/></td>";
                }
            }
            return sHtml;
        }
        function bindColumHead(isHead, data, nRow) {
            var sHtml = "";
            if (isHead) {
                if (nRow == 1) {
                    //var isQ1 = isQ2 = isQ3 = isQ4 = true;
                    //var n1 = 0;
                    //for (var i = 1; i <= 3; i++) {
                    //    var d = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == i });
                    //    if (d.nStatusID != 0) {
                    //        n1 = n1 + 1;
                    //    }
                    //}
                    //if (n1 == 3) isQ1 = false;
                    //var n2 = 0;
                    //for (var y = 4; y <= 6; y++) {
                    //    var d = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == y });
                    //    if (d.nStatusID != 0) {
                    //        n2 = n2 + 1;
                    //    }
                    //}
                    //if (n2 == 3) isQ2 = false;
                    //var n3 = 0;
                    //for (var o = 7; o <= 9; o++) {
                    //    var d = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == o });
                    //    if (d.nStatusID != 0) {
                    //        n3 = n3 + 1;
                    //    }
                    //}
                    //if (n3 == 3) isQ3 = false;
                    //var n4 = 0;
                    //for (var j = 10; j <= 12; j++) {
                    //    var d = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == j });
                    //    if (d.nStatusID != 0) {
                    //        n4 = n4 + 1;
                    //    }
                    //}
                    //if (n4 == 3) isQ4 = false;

                    //sHtml += "<th class='text-center QHead_" + getQrt(1) + "' colspan='3'>&nbsp;<label>Q" + getQrt(1) + "</label>" + (isQ1 ? "&nbsp;<button btnID='1' Type='button' class='btn btn-sm btn-info' onclick='RecalQuter(1);'>Recalculate</button>" : "") + "</th>";
                    //sHtml += "<th class='text-center QHead_" + getQrt(4) + "' colspan='3'>&nbsp;<label>Q" + getQrt(4) + "</label>" + (isQ2 ? "<button btnID='2' Type='button' class='btn btn-sm btn-info' onclick='RecalQuter(2);'>Recalculate</button>" : "") + "</th>";
                    //sHtml += "<th class='text-center QHead_" + getQrt(7) + "' colspan='3'>&nbsp;<label>Q" + getQrt(7) + "</label>" + (isQ3 ? "<button btnID='3' Type='button' class='btn btn-sm btn-info' onclick='RecalQuter(3);'>Recalculate</button>" : "") + "</th>";
                    //sHtml += "<th class='text-center QHead_" + getQrt(10) + "' colspan='3'>&nbsp;<label>Q" + getQrt(10) + "</label>" + (isQ4 ? "<button btnID='4' Type='button' class='btn btn-sm btn-info' onclick='RecalQuter(4);'>Recalculate</button>" : "") + "</th>";
                }
                else {
                    for (var i = 1 ; i <= 12; i++) {
                        sHtml += "<th class='text-center M_" + i + " QHead_" + getQrt(i) + "'>&nbsp;<label>Q" + getQrt(i) + " : " + arrShortMonth[i - 1] + "</label></th>";
                    }
                }
            }
            else {
                sHtml += bindDataColum(false, true, data, false);
            }
            return sHtml;
        }
        function bindDataColum(isNoINput, isHead, data, isOther) {
            var sHtml = "";
            ///// BODY HEAD
            if (isNoINput) {
                for (var i = 1; i <= 12; i++) {
                    sHtml += "<td class='text-center M_1 QHead_" + getQrt(i) + "'></td>";
                }
            }
            else {
                if (isHead) {
                    var sDisabled = "";
                    if (isHead) sDisabled = "disabled";
                    sHtml += "<td class='text-center M_1 QHead_" + getQrt(1) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='1' value ='" + CheckTextOutput(data.M1) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_2 QHead_" + getQrt(2) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='2' value ='" + CheckTextOutput(data.M2) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_3 QHead_" + getQrt(3) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='3' value ='" + CheckTextOutput(data.M3) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_4 QHead_" + getQrt(4) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='4' value ='" + CheckTextOutput(data.M4) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_5 QHead_" + getQrt(5) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='5' value ='" + CheckTextOutput(data.M5) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_6 QHead_" + getQrt(6) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='6' value ='" + CheckTextOutput(data.M6) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_7 QHead_" + getQrt(7) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='7' value ='" + CheckTextOutput(data.M7) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_8 QHead_" + getQrt(8) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='8' value ='" + CheckTextOutput(data.M8) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_9 QHead_" + getQrt(9) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='9' value ='" + CheckTextOutput(data.M9) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_10 QHead_" + getQrt(10) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='10' value ='" + CheckTextOutput(data.M10) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_11 QHead_" + getQrt(11) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='11' value ='" + CheckTextOutput(data.M11) + "' " + sDisabled + "/></td>";
                    sHtml += "<td class='text-center M_12 QHead_" + getQrt(12) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='12' value ='" + CheckTextOutput(data.M12) + "' " + sDisabled + "/></td>";
                }
                else {
                    if (!isOther) {
                        var sDisabled = "";
                        if (isHead) sDisabled = "disabled";
                        sHtml += "<td class='text-center M_1 QHead_" + getQrt(1) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='1' value ='" + CheckTextInput(data.M1) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_2 QHead_" + getQrt(2) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='2' value ='" + CheckTextInput(data.M2) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_3 QHead_" + getQrt(3) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='3' value ='" + CheckTextInput(data.M3) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_4 QHead_" + getQrt(4) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='4' value ='" + CheckTextInput(data.M4) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_5 QHead_" + getQrt(5) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='5' value ='" + CheckTextInput(data.M5) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_6 QHead_" + getQrt(6) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='6' value ='" + CheckTextInput(data.M6) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_7 QHead_" + getQrt(7) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='7' value ='" + CheckTextInput(data.M7) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_8 QHead_" + getQrt(8) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='8' value ='" + CheckTextInput(data.M8) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_9 QHead_" + getQrt(9) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='9' value ='" + CheckTextInput(data.M9) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_10 QHead_" + getQrt(10) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='10' value ='" + CheckTextInput(data.M10) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_11 QHead_" + getQrt(11) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='11' value ='" + CheckTextInput(data.M11) + "' " + sDisabled + "/></td>";
                        sHtml += "<td class='text-center M_12 QHead_" + getQrt(12) + "'><input ProductID='" + data.ProductID + "' class='form-control' nMonth ='12' value ='" + CheckTextInput(data.M12) + "' " + sDisabled + "/></td>";
                    }
                    else {
                        for (var x = 1; x <= 12; x++) {
                            var strForDisabled = "";
                            if ($hdfsStatus.val() != "" && $hdfsStatus.val() == "27") {
                                if (lstStatus[x - 1].nStatusID != "2") {
                                    strForDisabled = "disabled";
                                }
                            } else {
                                if (lstStatus[x - 1].nStatusID != "0") {
                                    strForDisabled = "disabled";
                                }
                            }
                            if ($hdfIsAdmin.val() == "Y" || ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "")) {
                                strForDisabled = "";
                            }
                            sHtml += "<td class='text-center M_" + x + " QHead_" + getQrt(x) + "'><input class='form-control' nOtherID='" + data.nOtherID + "' nMonth ='" + x + "' value ='" + CheckTextInput(getDataInArr(data, x)) + "' " + strForDisabled + " /></td>";
                        }
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
        function setDataInArr(itemPrd, nMonth, nVal, isCalAll) {
            var Month = nMonth;
            switch (Month + "") {
                case "1":
                    itemPrd.M1 = itemPrd.IsCheckM1 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M1 == "" ? nVal + "" : ((+itemPrd.M1) + (+nVal)) + "";
                    break;
                case "2":
                    itemPrd.M2 = itemPrd.IsCheckM2 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M2 == "" ? nVal + "" : ((+itemPrd.M2) + (+nVal)) + "";
                    break;
                case "3":
                    itemPrd.M3 = itemPrd.IsCheckM3 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M3 == "" ? nVal + "" : ((+itemPrd.M3) + (+nVal)) + "";
                    break;
                case "4":
                    itemPrd.M4 = itemPrd.IsCheckM4 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M4 == "" ? nVal + "" : ((+itemPrd.M4) + (+nVal)) + "";
                    break;
                case "5":
                    itemPrd.M5 = itemPrd.IsCheckM5 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M5 == "" ? nVal + "" : ((+itemPrd.M5) + (+nVal)) + "";
                    break;
                case "6":
                    itemPrd.M6 = itemPrd.IsCheckM6 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M6 == "" ? nVal + "" : ((+itemPrd.M6) + (+nVal)) + "";
                    break;
                case "7":
                    itemPrd.M7 = itemPrd.IsCheckM7 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M7 == "" ? nVal + "" : ((+itemPrd.M7) + (+nVal)) + "";
                    break;
                case "8":
                    itemPrd.M8 = itemPrd.IsCheckM8 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M8 == "" ? nVal + "" : ((+itemPrd.M8) + (+nVal)) + "";
                    break;
                case "9":
                    itemPrd.M9 = itemPrd.IsCheckM9 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M9 == "" ? nVal + "" : ((+itemPrd.M9) + (+nVal)) + "";
                    break;
                case "10":
                    itemPrd.M10 = itemPrd.IsCheckM10 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M10 == "" ? nVal + "" : ((+itemPrd.M10) + (+nVal)) + "";
                    break;
                case "11":
                    itemPrd.M11 = itemPrd.IsCheckM11 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M11 == "" ? nVal + "" : ((+itemPrd.M11) + (+nVal)) + "";
                    break;
                case "12":
                    itemPrd.M12 = itemPrd.IsCheckM12 == "Y" ? "0" : !isCalAll ? nVal + "" : itemPrd.M12 == "" ? nVal + "" : ((+itemPrd.M12) + (+nVal)) + "";
                    break;
            }
            return nVal;
        }
        //// ข้อมูลAdd Point
        function bindDataPoint(sType) {
            var lstData = Enumerable.From(lstPointInput).Where(function (w) { return w.sTypePoint == sType && w.cStatus == "Y" }).ToArray();
            var sTableName = sType == "M" ? "tbDataAddPointManual" : "tbDataAddPointCalculated";
            if (lstData.length > 0) {
                var sHtml = "";
                $("#" + sTableName + " tbody tr").remove();
                for (var i = 0; i < lstData.length; i++) {
                    var sName = lstData[i].sTreatmentID == "999" ? lstData[i].sTreatmentOther : lstData[i].sTreatmentName;
                    var btn = "";
                    var IsFull = true;
                    $.each(lstStatus, function (i, el) {
                        if (el.nStatusID == 0) {
                            IsFull = false;
                        }
                    });
                    if (IsFull && lstData[i].cNew == "N") {
                        btn = '<button Type="button" class="btn btn-sm btn-info btnEdit" onclick="EditPoint(\'' + sType + '\',' + lstData[i].nPointID + ');" title="View"><i class="fas fa-search"></i></button>';
                    }
                    else {
                        btn = '<button Type="button" class="btn btn-sm btn-warning btnEdit" onclick="EditPoint(\'' + sType + '\',' + lstData[i].nPointID + ');" title="Edit"><i class="fas fa-edit"></i></button>';
                    }

                    sHtml += '<tr>';
                    sHtml += '<td class="text-center cMiddle">' + (i + 1) + '</td>';
                    sHtml += '<td class="text-left cMiddle">' + lstData[i].sPointName + '</td>';
                    sHtml += '<td class="text-left cMiddle">' + sName + '</td>';
                    sHtml += '<td class="text-left cMiddle">' + lstData[i].sDisChargeName + '</td>';
                    sHtml += '<td class="text-left cMiddle">' + lstData[i].sAreaName + '</td>';
                    sHtml += '<td>' + btn + '</td>';
                    sHtml += '<td><button Type="button" class="btn btn-sm btn-danger" nPoint="' + lstData[i].nPointID + '" onclick="DeletePoint(\'' + sType + '\',' + lstData[i].nPointID + ');" title="Delete"><i class="fas fa-trash-alt"></i></button></td>';
                    sHtml += '</tr>';
                }
                $("#" + sTableName + " tbody").append(sHtml);
                $.each(lstData, function (i2, el2) {
                    if (el2.cNew == "Y") {
                        $("button[nPoint=" + el2.nPointID + "].btn-danger").prop("disabled", false);
                    }
                    else if (el2.cNew == "N" && nStatus == 0) {
                        $("button[nPoint=" + el2.nPointID + "].btn-danger").prop("disabled", false);
                    }
                    else {
                        $("button[nPoint=" + el2.nPointID + "].btn-danger").hide();
                    }
                });
            }
            else {
                if (sType == "M") SetRowNoData($tbDataAddPointManual.attr("id"), 7);
                else SetRowNoData($tbDataAddPointCalculated.attr("id"), 7);
            }
            SetTootip();
        }
        function SavePoint() { //sClick, sType, nPointID
            var IsPass = CheckValidate("Point");
            var isOther = GetValDropdown("ddlTreatment") == "999" ? (GetValTextBox("txtOther") != "" ? true : false) : true;
            if (IsPass && isOther) {
                if ($hidOrderPoint.val() == "") {
                    var nPointID = 1;
                    if (lstPointInput.length > 0) {
                        //var lst = Enumerable.From(lstPointInput).Where(function (w) { return w.cStatus == "Y" }).ToArray();
                        //if (lst.length > 0) 
                        nPointID = Enumerable.From(lstPointInput).Max(function (x) { return x.nPointID }) + 1;
                    }
                    var sCOD = $("input[name$=COD126]:checked").val();
                    var obj = {
                        nPointID: nPointID,
                        sTypePoint: $hidTypePoint.val(),
                        sPointName: $txtPointName.val(),
                        sDisChargeName: $("select[id$=ddlDischarge] option:selected").text(),
                        sDisChargeID: $ddlDischarge.val(),
                        sTreatmentName: $("select[id$=ddlTreatment] option:selected").text(),
                        sTreatmentID: $ddlTreatment.val(),
                        sAreaName: $("select[id$=ddlArea] option:selected").text(),
                        sAreaID: $ddlArea.val(),
                        cStatus: "Y",
                        cCOD: sCOD,
                        sRemark: $("textarea[id$=txtRemarkPoint]").val(),
                        sTreatmentOther: GetValTextBox("txtOther"),
                        cNew: "Y",
                    };
                    lstPointInput.push(obj);

                    var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
                    for (var i = 0; i < lst.length; i++) {
                        var objPoint = {};
                        var isPass = true;
                        if (sCOD == "1") { //sCOD == "U"
                            if (lst[i].ProductID == 128) isPass = false
                        }
                        if (sCOD == "2") { //sCOD == "C"
                            if (lst[i].ProductID == 127) isPass = false
                        }
                        if (isPass) {
                            objPoint = {
                                nPointID: nPointID,
                                sTypePoint: $hidTypePoint.val(),
                                ProductID: lst[i].ProductID,
                                M1: $("input[id$=Chk_1]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=1]").val().replace(/,/g, ''),
                                M2: $("input[id$=Chk_2]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=2]").val().replace(/,/g, ''),
                                M3: $("input[id$=Chk_3]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=3]").val().replace(/,/g, ''),
                                M4: $("input[id$=Chk_4]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=4]").val().replace(/,/g, ''),
                                M5: $("input[id$=Chk_5]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=5]").val().replace(/,/g, ''),
                                M6: $("input[id$=Chk_6]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=6]").val().replace(/,/g, ''),
                                M7: $("input[id$=Chk_7]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=7]").val().replace(/,/g, ''),
                                M8: $("input[id$=Chk_8]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=8]").val().replace(/,/g, ''),
                                M9: $("input[id$=Chk_9]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=9]").val().replace(/,/g, ''),
                                M10: $("input[id$=Chk_10]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=10]").val().replace(/,/g, ''),
                                M11: $("input[id$=Chk_11]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=11]").val().replace(/,/g, ''),
                                M12: $("input[id$=Chk_12]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=12]").val().replace(/,/g, ''),
                                Target: $("input[productid=" + lst[i].ProductID + "].target").val().replace(/,/g, ''),
                                cStatus: "Y",
                                IsCheckM1: $("input[id$=Chk_1]").is(":checked") ? "Y" : "N",
                                IsCheckM2: $("input[id$=Chk_2]").is(":checked") ? "Y" : "N",
                                IsCheckM3: $("input[id$=Chk_3]").is(":checked") ? "Y" : "N",
                                IsCheckM4: $("input[id$=Chk_4]").is(":checked") ? "Y" : "N",
                                IsCheckM5: $("input[id$=Chk_5]").is(":checked") ? "Y" : "N",
                                IsCheckM6: $("input[id$=Chk_6]").is(":checked") ? "Y" : "N",
                                IsCheckM7: $("input[id$=Chk_7]").is(":checked") ? "Y" : "N",
                                IsCheckM8: $("input[id$=Chk_8]").is(":checked") ? "Y" : "N",
                                IsCheckM9: $("input[id$=Chk_9]").is(":checked") ? "Y" : "N",
                                IsCheckM10: $("input[id$=Chk_10]").is(":checked") ? "Y" : "N",
                                IsCheckM11: $("input[id$=Chk_11]").is(":checked") ? "Y" : "N",
                                IsCheckM12: $("input[id$=Chk_12]").is(":checked") ? "Y" : "N",
                            };
                        }
                        else {
                            objPoint = {
                                nPointID: nPointID,
                                sTypePoint: $hidTypePoint.val(),
                                ProductID: lst[i].ProductID,
                                M1: $("input[id$=Chk_1]").is(":checked") ? "0" : "",
                                M2: $("input[id$=Chk_2]").is(":checked") ? "0" : "",
                                M3: $("input[id$=Chk_3]").is(":checked") ? "0" : "",
                                M4: $("input[id$=Chk_4]").is(":checked") ? "0" : "",
                                M5: $("input[id$=Chk_5]").is(":checked") ? "0" : "",
                                M6: $("input[id$=Chk_6]").is(":checked") ? "0" : "",
                                M7: $("input[id$=Chk_7]").is(":checked") ? "0" : "",
                                M8: $("input[id$=Chk_8]").is(":checked") ? "0" : "",
                                M9: $("input[id$=Chk_9]").is(":checked") ? "0" : "",
                                M10: $("input[id$=Chk_10]").is(":checked") ? "0" : "",
                                M11: $("input[id$=Chk_11]").is(":checked") ? "0" : "",
                                M12: $("input[id$=Chk_12]").is(":checked") ? "0" : "",
                                Target: "",
                                cStatus: "Y",
                                IsCheckM1: $("input[id$=Chk_1]").is(":checked") ? "Y" : "N",
                                IsCheckM2: $("input[id$=Chk_2]").is(":checked") ? "Y" : "N",
                                IsCheckM3: $("input[id$=Chk_3]").is(":checked") ? "Y" : "N",
                                IsCheckM4: $("input[id$=Chk_4]").is(":checked") ? "Y" : "N",
                                IsCheckM5: $("input[id$=Chk_5]").is(":checked") ? "Y" : "N",
                                IsCheckM6: $("input[id$=Chk_6]").is(":checked") ? "Y" : "N",
                                IsCheckM7: $("input[id$=Chk_7]").is(":checked") ? "Y" : "N",
                                IsCheckM8: $("input[id$=Chk_8]").is(":checked") ? "Y" : "N",
                                IsCheckM9: $("input[id$=Chk_9]").is(":checked") ? "Y" : "N",
                                IsCheckM10: $("input[id$=Chk_10]").is(":checked") ? "Y" : "N",
                                IsCheckM11: $("input[id$=Chk_11]").is(":checked") ? "Y" : "N",
                                IsCheckM12: $("input[id$=Chk_12]").is(":checked") ? "Y" : "N",
                            };
                        }

                        arrPoint.push(objPoint);
                        $("input[productid=" + lst[i].ProductID + "]").val('');
                    }

                    var lstOther = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == $hidTypePoint.val() && w.nPointID == "" && w.sStatus == "Y" }).ToArray();
                    for (var k = 0; k < lstOther.length; k++) {
                        lstOther[k].nPointID = nPointID;
                        lstOther[k].sProductID = $("select[id$=ddlInd_" + lstOther[k].nOtherID + "]").val();
                        lstOther[k].sUnitID = $("select[id$=ddlUni_" + lstOther[k].nOtherID + "]").val();
                        lstOther[k].Target = $("input[notherid=" + lstOther[k].nOtherID + "].target").val().replace(/,/g, '');
                        lstOther[k].M1 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=1]").val().replace(/,/g, '');
                        lstOther[k].M2 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=2]").val().replace(/,/g, '');
                        lstOther[k].M3 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=3]").val().replace(/,/g, '');
                        lstOther[k].M4 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=4]").val().replace(/,/g, '');
                        lstOther[k].M5 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=5]").val().replace(/,/g, '');
                        lstOther[k].M6 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=6]").val().replace(/,/g, '');
                        lstOther[k].M7 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=7]").val().replace(/,/g, '');
                        lstOther[k].M8 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=8]").val().replace(/,/g, '');
                        lstOther[k].M9 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=9]").val().replace(/,/g, '');
                        lstOther[k].M10 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=10]").val().replace(/,/g, '');
                        lstOther[k].M11 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=11]").val().replace(/,/g, '');
                        lstOther[k].M12 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=12]").val().replace(/,/g, '');
                    }

                    var lstDelNew = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == $hidTypePoint.val() && w.nPointID == "" && w.sStatus == "N" }).ToArray();
                    $.each(Enumerable.From(lstDelNew).ToArray(), function (i2, el2) {
                        el2.nPointID = nPointID;
                    });
                }
                else {
                    var nPointID = +$hidOrderPoint.val();
                    var sTypePoint = $hidTypePoint.val();
                    var sCOD = $("input[name$=COD126]:checked").val();
                    var d = Enumerable.From(lstPointInput).FirstOrDefault(null, function (s) { return s.nPointID == nPointID && s.sTypePoint == sTypePoint });
                    d.sPointName = $txtPointName.val();
                    d.sDisChargeName = $("select[id$=ddlDischarge] option:selected").text();
                    d.sDisChargeID = $ddlDischarge.val();
                    d.sTreatmentName = $("select[id$=ddlTreatment] option:selected").text();
                    d.sTreatmentID = $ddlTreatment.val();
                    d.sAreaName = $("select[id$=ddlArea] option:selected").text();
                    d.sAreaID = $ddlArea.val();
                    d.cStatus = "Y";
                    d.cCOD = sCOD;
                    d.sRemark = $("textarea[id$=txtRemarkPoint]").val();
                    d.sTreatmentOther = GetValTextBox("txtOther");
                    var lst = Enumerable.From(arrPoint).Where(function (w) { return w.sTypePoint == sTypePoint && w.nPointID == nPointID }).ToArray();
                    for (var i = 0; i < lst.length; i++) {
                        var isPass = true;
                        if (sCOD == "1") { // sCOD == "U"
                            if (lst[i].ProductID == 128) isPass = false
                        }
                        if (sCOD == "2") { //sCOD == "C"
                            if (lst[i].ProductID == 127) isPass = false
                        }
                        if (isPass) {
                            lst[i].M1 = $("input[id$=Chk_1]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=1]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=1]").val().replace(/,/g, '') : "";
                            lst[i].M2 = $("input[id$=Chk_2]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=2]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=2]").val().replace(/,/g, '') : "";
                            lst[i].M3 = $("input[id$=Chk_3]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=3]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=3]").val().replace(/,/g, '') : "";
                            lst[i].M4 = $("input[id$=Chk_4]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=4]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=4]").val().replace(/,/g, '') : "";
                            lst[i].M5 = $("input[id$=Chk_5]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=5]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=5]").val().replace(/,/g, '') : "";
                            lst[i].M6 = $("input[id$=Chk_6]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=6]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=6]").val().replace(/,/g, '') : "";
                            lst[i].M7 = $("input[id$=Chk_7]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=7]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=7]").val().replace(/,/g, '') : "";
                            lst[i].M8 = $("input[id$=Chk_8]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=8]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=8]").val().replace(/,/g, '') : "";
                            lst[i].M9 = $("input[id$=Chk_9]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=9]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=9]").val().replace(/,/g, '') : "";
                            lst[i].M10 = $("input[id$=Chk_10]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=10]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=10]").val().replace(/,/g, '') : "";
                            lst[i].M11 = $("input[id$=Chk_11]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=11]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=11]").val().replace(/,/g, '') : "";
                            lst[i].M12 = $("input[id$=Chk_12]").is(":checked") ? "0" : $("input[productid=" + lst[i].ProductID + "][nmonth=12]").val() != "" ? $("input[productid=" + lst[i].ProductID + "][nmonth=12]").val().replace(/,/g, '') : "";
                            lst[i].Target = $("input[productid=" + lst[i].ProductID + "].target").val() != "" ? $("input[productid=" + lst[i].ProductID + "].target").val().replace(/,/g, '') : "";
                            $("input[productid=" + lst[i].ProductID + "]").val('');
                        }
                        else {
                            lst[i].M1 = $("input[id$=Chk_1]").is(":checked") ? "0" : "";
                            lst[i].M2 = $("input[id$=Chk_2]").is(":checked") ? "0" : "";
                            lst[i].M3 = $("input[id$=Chk_3]").is(":checked") ? "0" : "";
                            lst[i].M4 = $("input[id$=Chk_4]").is(":checked") ? "0" : "";
                            lst[i].M5 = $("input[id$=Chk_5]").is(":checked") ? "0" : "";
                            lst[i].M6 = $("input[id$=Chk_6]").is(":checked") ? "0" : "";
                            lst[i].M7 = $("input[id$=Chk_7]").is(":checked") ? "0" : "";
                            lst[i].M8 = $("input[id$=Chk_8]").is(":checked") ? "0" : "";
                            lst[i].M9 = $("input[id$=Chk_9]").is(":checked") ? "0" : "";
                            lst[i].M10 = $("input[id$=Chk_10]").is(":checked") ? "0" : "";
                            lst[i].M11 = $("input[id$=Chk_11]").is(":checked") ? "0" : "";
                            lst[i].M12 = $("input[id$=Chk_12]").is(":checked") ? "0" : "";
                            lst[i].Target = "";
                            $("input[productid=" + lst[i].ProductID + "]").val('');
                        }
                    }

                    var lstOther = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sTypePoint && w.nPointID == nPointID && w.sStatus == "Y" }).ToArray();
                    for (var k = 0; k < lstOther.length; k++) {

                        lstOther[k].nPointID = nPointID;
                        lstOther[k].sProductID = $("select[id$=ddlInd_" + lstOther[k].nOtherID + "]").val();
                        lstOther[k].sUnitID = $("select[id$=ddlUni_" + lstOther[k].nOtherID + "]").val();
                        lstOther[k].Target = $("input[notherid=" + lstOther[k].nOtherID + "].target").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "].target").val().replace(/,/g, '') : "";
                        lstOther[k].M1 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=1]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=1]").val().replace(/,/g, '') : "";
                        lstOther[k].M2 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=2]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=2]").val().replace(/,/g, '') : "";
                        lstOther[k].M3 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=3]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=3]").val().replace(/,/g, '') : "";
                        lstOther[k].M4 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=4]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=4]").val().replace(/,/g, '') : "";
                        lstOther[k].M5 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=5]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=5]").val().replace(/,/g, '') : "";
                        lstOther[k].M6 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=6]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=6]").val().replace(/,/g, '') : "";
                        lstOther[k].M7 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=7]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=7]").val().replace(/,/g, '') : "";
                        lstOther[k].M8 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=8]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=8]").val().replace(/,/g, '') : "";
                        lstOther[k].M9 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=9]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=9]").val().replace(/,/g, '') : "";
                        lstOther[k].M10 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=10]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=10]").val().replace(/,/g, '') : "";
                        lstOther[k].M11 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=11]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=11]").val().replace(/,/g, '') : "";
                        lstOther[k].M12 = $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=12]").val() != "" ? $("input[notherid=" + lstOther[k].nOtherID + "][nmonth=12]").val().replace(/,/g, '') : "";
                    }

                    var lstOtherNopoint = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == $hidTypePoint.val() && w.nPointID == "" && w.sStatus == "Y" }).ToArray();
                    for (var y = 0; y < lstOtherNopoint.length; y++) {

                        lstOtherNopoint[y].nPointID = nPointID;
                        lstOtherNopoint[y].sProductID = $("select[id$=ddlInd_" + lstOtherNopoint[y].nOtherID + "]").val();
                        lstOtherNopoint[y].sUnitID = $("select[id$=ddlUni_" + lstOtherNopoint[y].nOtherID + "]").val();
                        lstOtherNopoint[y].Target = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "].target").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "].target").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M1 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=1]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=1]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M2 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=2]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=2]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M3 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=3]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=3]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M4 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=4]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=4]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M5 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=5]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=5]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M6 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=6]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=6]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M7 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=7]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=7]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M8 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=8]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=8]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M9 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=9]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=9]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M10 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=10]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=10]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M11 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=11]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=11]").val().replace(/,/g, '') : "";
                        lstOtherNopoint[y].M12 = $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=12]").val() != "" ? $("input[notherid=" + lstOtherNopoint[y].nOtherID + "][nmonth=12]").val().replace(/,/g, '') : "";
                    }
                    var lstDelNew = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == $hidTypePoint.val() && w.nPointID == "" && w.sStatus == "N" }).ToArray();
                    $.each(Enumerable.From(lstDelNew).ToArray(), function (i2, el2) {
                        el2.nPointID = nPointID;
                    });
                }

                UpdateStatusValidateControl("Point", $txtPointName, 'NOT_VALIDATED');
                UpdateStatusValidateControl("Point", $ddlDischarge, 'NOT_VALIDATED');
                UpdateStatusValidateControl("Point", $ddlArea, 'NOT_VALIDATED');
                UpdateStatusValidateControl("Point", $ddlTreatment, 'NOT_VALIDATED');
                bindDataPoint($hidTypePoint.val());
                ClearPoint();
                CalPointToHead();
                $divEditPoint.hide();
                $hidOpenPoint.val('');
            }
            else if (!isOther && IsPass) {
                DialogWarning(DialogHeader.Warning, "Please specify other Treatment !");
                ScrollTopToElementsTo("divEditPoint", 80);
            }
        }
        function DeletePoint(sType, nPointID) {
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, function () {
                var nPointOld = 0;
                var d = Enumerable.From(lstPointInput).FirstOrDefault(null, function (s) { return s.sTypePoint == sType && s.nPointID == nPointID && s.cStatus == "Y" });
                d.cStatus = "N";
                nPointOld = d.nPointID;
                var lstPointDel = Enumerable.From(arrPoint).Where(function (w) { return w.cStatus == "Y" && w.nPointID == d.nPointID && w.sTypePoint == sType }).ToArray();
                $.each(Enumerable.From(lstPointDel).ToArray(), function (i2, el2) {
                    el2.cStatus = "N";
                });
                var lstOtherDel = Enumerable.From(arrOther).Where(function (w) { return w.sStatus == "Y" && w.nPointID == d.nPointID && w.sTypePoint == sType }).ToArray();
                $.each(Enumerable.From(lstOtherDel).ToArray(), function (i3, el3) {
                    el3.sStatus = "N";
                });
                //$.each(Enumerable.From(lstPointInput).Where(function (w) { return w.cStatus == "Y" }).ToArray(), function (i, el) {
                //    if (el.nPointID > nPointOld) {
                //        el.nPointID = el.nPointID - 1;
                //    }
                //});
                //$.each(Enumerable.From(arrPoint).Where(function (w) { return w.cStatus == "Y" }).ToArray(), function (i4, el4) {
                //    if (el4.nPointID > nPointOld) {
                //        el4.nPointID = el4.nPointID - 1;
                //    }
                //});
                //$.each(Enumerable.From(arrOther).Where(function (w) { return w.sStatus == "Y" }).ToArray(), function (i5, el5) {
                //    if (el5.nPointID > nPointOld) {
                //        el5.nPointID = el5.nPointID - 1;
                //    }
                //});
                bindDataPoint("M"); bindDataPoint("C");
                CalPointToHead();
                ClearPoint(); $divEditPoint.hide();
                HideLoadding();
            }, function () { HideLoadding(); });
        }
        function EditPoint(sType, nPointID) {
            $hidOpenPoint.val('Y');
            var objValidate = {};
            objValidate[GetElementName("txtPointName", objControl.txtbox)] = addValidate_notEmpty(DialogMsg.Specify + " Point Name");
            objValidate[GetElementName("ddlDischarge", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Discharge to");
            objValidate[GetElementName("ddlTreatment", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Treatment method");
            objValidate[GetElementName("ddlArea", objControl.dropdown)] = addValidate_notEmpty(DialogMsg.Specify + " Area");
            BindValidate("Point", objValidate);
            $hidOrderPoint.val('');
            $hidOrderPoint.val(nPointID);
            $hidTypePoint.val(sType);
            var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
            for (var x = 0; x < lst.length; x++) {
                var d = Enumerable.From(arrPoint).FirstOrDefault(null, function (s) { return s.ProductID == lst[x].ProductID && s.nPointID == nPointID && s.sTypePoint == sType });
                $("input[productid=" + lst[x].ProductID + "].target").val(CheckTextInput(d.Target));
                for (var i = 1; i <= 12; i++) {
                    var sVal = getDataInArr(d, i);
                    $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").val(CheckTextInput(sVal));
                    var data = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == i });
                    if (data.nStatusID == 0) {
                        if ($("input[id$=Chk_" + i + "]").is(':checked')) {
                            $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").val("0");
                            $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', true);
                        }
                        else {
                            $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', false);
                        }
                    }
                    else {
                        $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', true);
                    }
                    ///เพิ่มวันที่ 26/04/2562
                    if (lstStatus.length > 0) {
                        if ($hdfPRMS.val() == "2" && $hdfRole.val() == "4" && $hdfsStatus.val() == "") {
                            $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', false);
                        }
                        $.each(lstStatus, function (i2, el2) {
                            if (el2.nStatusID == 2) {
                                $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', false);
                            }
                        });
                    }
                }
            }
            if (sType == "C") {
                $("button[btnID = 124]").show();
            }
            else {
                $("button[btnID = 124]").hide();
            }
            var arr = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPointID }).ToArray()
            bindDataOther(arr);
            $.each(Enumerable.From(lstPointInput).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPointID }).ToArray(), function (i, el) {
                $txtPointName.val(el.sPointName);
                $ddlArea.val(el.sAreaID);
                $ddlDischarge.val(el.sDisChargeID);
                $ddlTreatment.val(el.sTreatmentID);
                $txtRemarkPoint.val(el.sRemark);
                $("input[id$=txtOther]").val(el.sTreatmentOther);
                if (el.sTreatmentID == "999") $("div#divTreatmentOther").show();
                else $("div#divTreatmentOther").hide();

                if (el.sDisChargeID == "3" && el.sAreaID == "14") {
                    $ddlArea.prop('disabled', true);
                }

                if (el.cCOD == "1") { //el.cCOD == "U"
                    $("input[id$=chk_126_2]").removeAttr('checked');
                    $("input[id$=chk_126_1]").prop('checked', true);
                    $("input[id$=chk_126_1]").iCheck('update');
                    $("input[id$=chk_126_2]").iCheck('update');
                    $("tr[id$=127]").show();
                    $("tr[id$=128]").hide();
                }
                else {
                    $("input[id$=chk_126_1]").removeAttr('checked');
                    $("input[id$=chk_126_2]").prop('checked', true);
                    $("input[id$=chk_126_2]").iCheck('update');
                    $("input[id$=chk_126_1]").iCheck('update');
                    $("tr[id$=127]").hide();
                    $("tr[id$=128]").show();
                }
            });
            $divEditPoint.show();
            ScrollTopToElementsTo("divEditPoint", 80);

            //// เพิ่มวันที่26/04/62 เช็ค L1
            if (IsFullMonth || $hdfPRMS.val() == 1) {
                /// 30-01-2020 เพิ่มเช็ค L2 แก้ไขได้ทุกอย่างเหมือน superAdmin
                if ($hdfIsAdmin.val() != "Y" && $hdfRole.val() != "4") {
                    for (var i = 1; i <= 12; i++) {
                        $("input[nmonth=" + i + "]").prop('disabled', true);
                    }
                }
            }
        }
        function ClearPoint() {
            $ddlArea.prop('disabled', false);
            $("div#divTreatmentOther").hide();
            $txtPointName.val('');
            $ddlArea.val('');
            $ddlDischarge.val('');
            $ddlTreatment.val('');
            $txtRemarkPoint.val('');
            $hidTypePoint.val('');
            var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
            for (var x = 0; x < lst.length; x++) {
                $("input[productid=" + lst[x].ProductID + "]").val('');
            }
            $divEditPoint.hide();
            //DelArry();          
        }
        function DelArry() {
            if ($hidOrderPoint.val() != "") {
                var nPoint = +$hidOrderPoint.val();
                var lstback = Enumerable.From(arrOther).Where(function (w) { return w.nPointID == nPoint }).ToArray();
                $.each(lstback, function (i2, el) {
                    el.sStatus = "Y";
                });
            }
            for (var d = 0; d < arrOther.length; d++) {
                if (arrOther[d].nPointID == "") {
                    arrOther.splice(d, 1);
                }
            }
            var lst = Enumerable.From(arrOther).Where(function (w) { return w.nPointID == "" }).ToArray();
            if (lst.length > 0) DelArry();
        }
        function CalPointToHead() {
            var lst = Enumerable.From(lstProduct).Where(function (w) { return w.cTotal == "Y" }).ToArray();
            for (var h = 0; h < lst.length; h++) {
                $("input[productid=" + lst[h].ProductID + "]").val('');
                for (var y = 1; y <= 12; y++) {
                    setDataInArr(lst[h], y, "", false);
                }
            }

            var lstHead = Enumerable.From(lstProduct).Where(function (w) { return w.cTotal == "Y" && w.sType == "SP" }).ToArray();
            for (var i = 0; i < lstHead.length; i++) {
                var isData = false;
                var sDisChargeID = "";
                if (lstHead[i].ProductID == 117) {
                    sDisChargeID = "1";
                    // Surface water = 1                  
                }
                else if (lstHead[i].ProductID == 118) {
                    //  Municipal treatment plan = 2
                    sDisChargeID = "2";
                }
                else if (lstHead[i].ProductID == 119) {
                    //Discharge to Saltwate = 3
                    sDisChargeID = "3";
                }
                else if (lstHead[i].ProductID == 120) {
                    // Injection for production/disposa = 4
                    sDisChargeID = "4";
                }
                else if (lstHead[i].ProductID == 121) {
                    //"Discharge to Aquifer recharge" = 5
                    sDisChargeID = "5";
                }
                else if (lstHead[i].ProductID == 122) {
                    //"Discharge to Storage/Waste lagoon" = 6
                    sDisChargeID = "6";
                }
                else if (lstHead[i].ProductID == 214) {
                    //"Discharge to Central treatment plant" = 76
                    sDisChargeID = "76";
                }

                var lstPoint = Enumerable.From(lstPointInput).Where(function (w) { return w.sDisChargeID == sDisChargeID && w.cStatus == "Y" }).ToArray();
                $.each(lstPoint, function (i2, el) {
                    var dataFlow = Enumerable.From(arrPoint).FirstOrDefault(null, function (s) { return s.nPointID == el.nPointID && s.sTypePoint == el.sTypePoint && s.ProductID == 124 });
                    var dataHouse = Enumerable.From(arrPoint).FirstOrDefault(null, function (s) { return s.nPointID == el.nPointID && s.sTypePoint == el.sTypePoint && s.ProductID == 125 });
                    for (var x = 1; x <= 12; x++) {
                        var nTotal = "";
                        var nF = getDataInArr(dataFlow, x);
                        var nH = getDataInArr(dataHouse, x);
                        if (nF != "" && nH != "" && nF != "N/A" && nH != "N/A") {
                            nF = nF.replace(/,/g, '');
                            nF != "" ? +nF : "";

                            nH = nH.replace(/,/g, '');
                            nH != "" ? +(nH) : "";
                            nTotal = nF * nH;
                        }
                        setDataInArr(lstHead[i], x, nTotal, true);
                    }
                    isData = true;
                });

                if (lstPoint.length > 0 || isData) {
                    lstHead[i].sStatus = "Y";
                }
                else {
                    lstHead[i].sStatus = "N";
                }
            }
            CalTotal();
        }
        function CalTotal() {
            var dataTotal = Enumerable.From(lstProduct).FirstOrDefault(null, function (w) { return w.cTotal == "Y" && w.sType == "1" });
            $.each(Enumerable.From(lstProduct).Where(function (w) { return w.cTotal == "Y" && w.sType == "SP" && w.sStatus == "Y" }).ToArray(), function (i2, el) {
                for (var i = 1; i <= 12; i++) {
                    var nTotal = 0;
                    nTotal = getDataInArr(el, i);
                    setDataInArr(dataTotal, i, nTotal, true);
                }
            });
            bindDataHead();
        }
        function SetWater() {
            var param = {
                sIndID: Select("ddlIndicator").val(),
                sOprtID: Select("ddlOperationType").val(),
                sFacID: Select("ddlFacility").val(),
                sYear: Select("Year").val()
            }
            LoaddinProcess();
            AjaxCallWebMethod("SetDataWater", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                } else {
                    var n = 1 / (+$txtNumPoint.val());
                    $hidOperhours.val(n);
                    var IsFull = true;
                    $.each(lstStatus, function (i, el) {
                        if (el.nStatusID == 0) {
                            IsFull = false;
                        }
                    });
                    for (var i = 1; i <= 12; i++) {
                        var data = Enumerable.From(lstStatus).FirstOrDefault(null, function (s) { return s.nMonth == i });
                        if (data.nStatusID == 0) {
                            if ($("input[id$=Chk_" + i + "]").is(':checked')) {
                                var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
                                for (var x = 0; x < lst.length; x++) {
                                    if (IsFull) {

                                    }
                                    $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").val('0');
                                    $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', true);
                                }
                            }
                            else {
                                var sVal = getDataInArr(response.d, i);
                                $("input[productid=124][nmonth=" + i + "]").val(sVal);
                                $("input[productid=125][nmonth=" + i + "]").val(n);
                                var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
                                for (var x = 0; x < lst.length; x++) {
                                    $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', false);
                                }
                            }
                        }
                        else {
                            var lst = Enumerable.From(lstProduct).Where(function (w) { return w.sType == "2" }).ToArray();
                            for (var x = 0; x < lst.length; x++) {
                                $("input[productid=" + lst[x].ProductID + "][nmonth=" + i + "]").prop('disabled', true);
                            }
                        }
                    }
                }
            }, function () {
                HideLoadding();
            }, { param: param, sPercent: $txtWater.val() });
        }
        function SaveOperating() {
            DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSave, function () {
                var lstData = Enumerable.From(lstPointInput).Where(function (w) { return w.sTypePoint == "C" && w.cStatus == "Y" }).OrderBy(function (x) { return x.nPointID }).ToArray();
                if (+$txtNumPoint.val() < lstData.length) {
                    var n = 1;
                    for (var h = 0; h < lstData.length; h++) {
                        if (n > +$txtNumPoint.val()) {
                            lstData[h].cStatus = "N";
                            var lstPointDel = Enumerable.From(arrPoint).Where(function (w) { return w.cStatus == "Y" && w.nPointID == lstData[h].nPointID && w.sTypePoint == "C" }).ToArray();
                            $.each(Enumerable.From(lstPointDel).ToArray(), function (i2, el2) {
                                el2.cStatus = "N";
                            });
                            var lstOtherDel = Enumerable.From(arrOther).Where(function (w) { return w.sStatus == "Y" && w.nPointID == lstData[h].nPointID && w.sTypePoint == "C" }).ToArray();
                            $.each(Enumerable.From(lstOtherDel).ToArray(), function (i3, el3) {
                                el3.sStatus = "N";
                            });
                        }
                        n++;
                    }
                }

                $.each(Enumerable.From(lstPointInput).Where(function (w) { return w.cStatus == "Y" && w.sTypePoint == "C" }).ToArray(), function (i, el) {
                    var data = Enumerable.From(arrPoint).FirstOrDefault(null, function (s) { return s.nPointID == el.nPointID && s.sTypePoint == "C" && s.ProductID == 125 });
                    data.M1 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =1]").val();
                    data.M2 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =2]").val();
                    data.M3 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =3]").val();
                    data.M4 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =4]").val();
                    data.M5 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =5]").val();
                    data.M6 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =6]").val();
                    data.M7 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =7]").val();
                    data.M8 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =8]").val();
                    data.M9 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =9]").val();
                    data.M10 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =10]").val();
                    data.M11 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =11]").val();
                    data.M12 = $("input[pointID=" + el.nPointID + "][productid=" + data.ProductID + "][nMonth =12]").val();
                    data.Target = $("input[pointID=" + el.nPointID + "].target").val();
                });
                var val = $txtNumPoint.val();
                var water = $txtWater.val();
                var n = 1 / (+val);
                $hidOperhours.val(n);
                $hidNumPoint.val(val);
                $hidWaterPoint.val(water);
                $txtNumPoint.prop('disabled', true);
                $txtWater.prop('disabled', true);
                $btnEditPointCal.show();
                $btnConfirmPointCal.hide();
                bindDataPoint("C");
                CalPointToHead();
                if ($hidOpenPoint.val() != "") $divEditPoint.hide(); $hidOpenPoint.val('');
                $("#popOperting").modal("hide");
                HideLoadding();

            }, function () { HideLoadding(); });
        }
        function CancelModal() {
            var val = $hidNumPoint.val();
            var water = $hidWaterPoint.val();
            $txtWater.val(water);
            $txtNumPoint.val(val);
            $txtNumPoint.prop('disabled', true);
            $txtWater.prop('disabled', true);
            $btnEditPointCal.show();
            $btnConfirmPointCal.hide();
            $("#popOperting").modal("hide");
        }
        function CheckTextNum(nVal) {

            if (nVal != "") {
                nVal = nVal.replace(/,/g, '');
                if (IsNumberic(nVal)) {
                    var nCheck = parseFloat(nVal);
                    if (nCheck > 0) {
                        nVal = addCommas(nCheck);
                    }
                    else {

                        nVal = nCheck;
                    }

                }
                else {
                    nVal = "";
                }
            }
            nVal = nVal + "";
            return nVal + "";
        }

        //// ข้อมูลเพิ่มเติม ไม่คำนวณ
        function bindDataOther(arrData) {
            var sHtml = "";
            $tbDataAddOther.empty();

            sHtml += "<thead>";
            sHtml += "  <tr id='Head'>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthIndicator + "px;'><label>Indicator</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle; width:" + nWidthTD + "px;'><label>Unit</label></th>";
            sHtml += "      <th class='text-center' style='vertical-align: middle;width:" + nWidthTD + "px;'><label>Target</label></th>";
            sHtml += bindColumHead(true, []);
            sHtml += "  </tr>";
            sHtml += "</thead>";

            var arr = Enumerable.From(arrData).Where(function (w) { return w.sStatus == "Y" }).OrderBy(function (x) { return x.nOtherID }).ToArray();
            if (arr.length > 0) {
                sHtml += "<tbody>";
                $.each(arr, function (i, el) {
                    sHtml += "  <tr id='tr_" + el.nOtherID + "'>";
                    var isNew = false;
                    if (el.cNew == "Y") isNew = true;
                    else if (el.cNew == "N" && nStatus == 0) isNew = true;

                    sHtml += "      <td style='vertical-align: middle;'>";
                    if (isNew) {
                        sHtml += "<div class='input-group'>";
                        sHtml += '<div class="input-group-addon cStyleDivTrash" onclick="DeletOther(\'' + el.sTypePoint + '\',\'' + el.nPointID + '\',' + el.nOtherID + ');"><i class="fas fa-trash-alt" style="color: red;"></i></div>';
                        sHtml += "   <select id='ddlInd_" + el.nOtherID + "' nOtherID='" + el.nOtherID + "' class='form-control input-sm' id='' onchange=''></select></div></td>";
                    }
                    else {
                        sHtml += "   <select id='ddlInd_" + el.nOtherID + "' nOtherID='" + el.nOtherID + "' class='form-control input-sm' id='' onchange=''></select></td>";
                    }
                    sHtml += "      <td style='vertical-align: middle;'><select id='ddlUni_" + el.nOtherID + "' nOtherID='" + el.nOtherID + "' class='form-control input-sm' id='' onchange=''></select></td>";
                    sHtml += "      <td class='text-center cTarget' style='vertical-align: middle;'><input class='form-control text-right target' nOtherID='" + el.nOtherID + "'  value ='" + CheckTextInput(el.Target) + "' maxlength='20'/></td>";
                    sHtml += bindDataColum(false, false, el, true);
                    sHtml += "  </tr>";
                });
                sHtml += "</tbody>";
                $tbDataAddOther.append(sHtml);
                SetDisableMonth();

                $.each(arr, function (i2, el2) {
                    var InID = "ddlInd_" + el2.nOtherID + "";
                    var UnitID = "ddlUni_" + el2.nOtherID + "";
                    Gen_ddl_TM_DATA(InID, el2.sProductID, "I");
                    Gen_ddl_TM_DATA(UnitID, el2.sUnitID, "U");
                    if (el2.cNew == "Y" && nStatus == 1) {
                        $("select[id$=" + InID + "]").prop('disabled', false);
                        $("select[id$=" + UnitID + "]").prop('disabled', false);
                    }
                });
            }
            else {
                sHtml += "<tbody></tbody>";
                $tbDataAddOther.append(sHtml);
                SetRowNoData($tbDataAddOther.attr("id"), 15);
            }
            $tbDataAddOther.tableHeadFixer({ "left": 1 }, { "head": true });
        }
        function AddOther() {
            var nPoint = $hidOrderPoint.val() == "" ? "" : +$hidOrderPoint.val();
            var sType = $hidTypePoint.val();
            var nOtherID = 1;
            var nMax = 0;
            if (arrOther.length > 0) {
                //// เช็ค พ้อยนั้น มีไหม
                var lst = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint }).ToArray();
                if (nPoint == "") {
                    //// NEW
                    if (lst.length > 0) {
                        nOtherID = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint }).Max(function (x) { return x.nOtherID }) + 1;
                        var lstActive = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint && w.sStatus == "Y" }).ToArray();
                        if (lstActive.length > 0) nMax = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint && w.sStatus == "Y" }).Max(function (x) { return x.nOtherID });
                    }
                }
                else {
                    //// มี Point แล้ว
                    if (lst.length > 0) {
                        ///เช็คก่อนมีข้อมูลที่พึ่งแอดใหม่ไหม
                        var lst2 = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == "" }).ToArray();
                        if (lst2.length > 0) {
                            nOtherID = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == "" }).Max(function (x) { return x.nOtherID }) + 1;
                            if (Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == "" && w.sStatus == "Y" }).ToArray().length > 0)
                                nMax = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == "" && w.sStatus == "Y" }).Max(function (x) { return x.nOtherID });
                            else if (Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint && w.sStatus == "Y" }).ToArray().length > 0) {
                                nMax = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint && w.sStatus == "Y" }).Max(function (x) { return x.nOtherID });
                            }
                        }
                        else {
                            /// ไม่มีข้อมูลที่พึ่งเพิ่มมาใหม่
                            nOtherID = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint }).Max(function (x) { return x.nOtherID }) + 1;
                            var lstActive = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint && w.sStatus == "Y" }).ToArray();
                            if (lstActive.length > 0) nMax = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == nPoint && w.sStatus == "Y" }).Max(function (x) { return x.nOtherID });
                        }
                    }
                    else {
                        ///เช็คก่อนมีข้อมูลที่พึ่งแอดใหม่ไหม เพิ่มวันที่ 26/04/2562
                        var lst2 = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == "" }).ToArray();
                        if (lst2.length > 0) {
                            nOtherID = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == "" }).Max(function (x) { return x.nOtherID }) + 1;
                            if (Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == "" && w.sStatus == "Y" }).ToArray().length > 0)
                                nMax = Enumerable.From(arrOther).Where(function (w) { return w.sTypePoint == sType && w.nPointID == "" && w.sStatus == "Y" }).Max(function (x) { return x.nOtherID });
                        }
                    }
                }
            }
            var obj = {
                nPointID: "",
                FromID: 0,
                nOtherID: nOtherID,
                sTypePoint: sType,
                sProductID: "",
                sUnitID: "",
                Target: "",
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
                cNew: "Y",
            };
            arrOther.push(obj);

            var d = Enumerable.From(arrOther).FirstOrDefault(null, function (s) { return s.sStatus == "Y" && s.sTypePoint == sType && s.nPointID == "" && s.nOtherID == nOtherID });
            var sHtml = "";
            sHtml += "  <tr id='tr_" + d.nOtherID + "'>";
            sHtml += "      <td style='vertical-align: middle;'>";
            sHtml += "<div class='input-group'>";
            sHtml += '<div class="input-group-addon cStyleDivTrash" onclick="DeletOther(\'' + d.sTypePoint + '\',\'' + d.nPointID + '\',' + d.nOtherID + ');"><i class="fas fa-trash-alt" style="color: red;"></i></div>';
            sHtml += "   <select id='ddlInd_" + d.nOtherID + "' nOtherID='" + d.nOtherID + "' class='form-control input-sm' id='' onchange=''></select></div></td>";
            sHtml += "      <td style='vertical-align: middle;'><select id='ddlUni_" + d.nOtherID + "' nOtherID='" + d.nOtherID + "' class='form-control input-sm' id='' onchange=''></select></td>";
            sHtml += "      <td class='text-center cTarget' style='vertical-align: middle;'><input class='form-control text-right target' nOtherID='" + d.nOtherID + "'  value ='' maxlength='20'/></td>";
            sHtml += bindDataColum(false, false, d, true);
            sHtml += "  </tr>";

            //var n = d.nOtherID - 1;
            if (nMax == 0) {
                $("#tbDataAddOther tbody tr").remove();
                $("#tbDataAddOther tbody").append(sHtml);
            }
            else $("#tbDataAddOther").find("#tr_" + nMax + "").after(sHtml);

            var InID = "ddlInd_" + d.nOtherID + "";
            var UnitID = "ddlUni_" + d.nOtherID + "";
            Gen_ddl_TM_DATA(InID, d.sProductID, "I");
            Gen_ddl_TM_DATA(UnitID, d.sUnitID, "U");
            //}

        }
        function DeletOther(sTypePoint, nPoint, nOtherID) {
            nPoint = nPoint != "" ? +nPoint : nPoint;
            var d = Enumerable.From(arrOther).FirstOrDefault(null, function (s) { return s.sStatus == "Y" && s.sTypePoint == sTypePoint && s.nPointID == nPoint && s.nOtherID == nOtherID });
            d.sStatus = "N";
            $("#tbDataAddOther tbody").find("#tr_" + nOtherID + "").remove();
            //var lst = Enumerable.From(arrOther).Where(function (w) { return w.sStatus == "Y" && w.sTypePoint == sTypePoint && w.nPointID == nPoint }).ToArray();
            //$.each(Enumerable.From(lst).ToArray(), function (i2, el2) {
            //    if (el2.nOtherID > nOtheID) {
            //        el2.nOtherID = el2.nOtherID - 1;
            //    }
            //});
            // bindDataOther(lst);
        }
        function Gen_ddl_TM_DATA(sName, sValue, sType) {
            var sTypeName = sType == "I" ? "Indicator" : "Unit";
            var lst = [];
            if (sType == "I") {
                lst = Enumerable.From(lstIndicatorOther).Where(function (w) { return w.cActive == "Y" || (sValue == "" ? "" : w.Value == sValue) }).OrderBy(function (x) { return x.Value }).ToArray();
            }
            else {
                lst = Enumerable.From(lstUnitOther).Where(function (w) { return w.cActive == "Y" || (sValue == "" ? "" : w.Value == sValue) }).OrderBy(function (x) { return x.Value }).ToArray();
            }
            //// var lst = sType == "I" ? lstIndicatorOther : lstUnitOther;
            $('select[id$=' + sName + ']').append("<option value=''>- select " + sTypeName + "-</option>");
            for (var i = 0; i < lst.length; i++) {
                $('select[id$=' + sName + ']').append("<option value=" + lst[i].Value + ">" + lst[i].sText + "</option>");
            }
            $('select[id$=' + sName + ']').val(sValue).trigger("chosen:updated");
        }
    </script>
</asp:Content>

