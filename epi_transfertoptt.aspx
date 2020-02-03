<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="epi_transfertoptt.aspx.cs" Inherits="epi_transfertoptt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        /*.flat-green label {
            padding-left: 10%;
        }*/

        .cHeadIndicatorTable {
            vertical-align: middle;
            width: 350px;
        }

        .cHeadAllTable {
            vertical-align: middle;
            width: 120px;
        }

        .cOrange {
            background-color: #fabd4f !important;
        }

        .cGreen {
            background-color: #dbea97 !important;
        }

        .cSubTotal3 {
            background-color: rgb(255, 237, 196) !important;
        }

        .cBGWhite {
            background-color: #fff !important;
        }

        .cExStyle {
            text-align: right;
            margin-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;Transfer to PTT</div>
        <div class="panel-body" id="divContent">
            <%-- Info --%>
            <div class="panel panel-info">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-12 col-md-6">
                            <div class="row">
                                <label class="control-label col-xs-12 col-md-5 col-lg-4">Group Indicator :</label>
                                <div class="col-xs-12 col-md-7 col-lg-8">
                                    <asp:Label ID="lblGroupIndicator" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-6">
                            <div class="row">
                                <label class="control-label col-xs-12 col-md-4">Operation type :</label>
                                <div class="col-xs-12 col-md-8">
                                    <asp:Label ID="lblOperationtype" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-6">
                            <div class="row">
                                <label class="control-label col-xs-12 col-md-4">Facility :</label>
                                <div class="col-xs-12 col-md-8">
                                    <asp:Label ID="lblFacility" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-6">
                            <div class="row">
                                <label class="control-label col-xs-12 col-md-4">Year :</label>
                                <div class="col-xs-12 col-md-8">
                                    <asp:Label ID="lblYear" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Select Month & Detail --%>
            <div class="panel panel-info">
                <div class="panel-heading">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-xs-12 col-md-3">
                                <i class="glyphicon glyphicon-check"></i>&nbsp;Select Quarter :
                            </div>
                            <div class="col-xs-12 col-md-9">
                                <asp:CheckBoxList ID="cklQuarter" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-body no-padding">
                    <div id="divModify1"></div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div id="divContentInten" class="table-responsive">
                                <table id="tblDataInten" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                    <thead>
                                        <tr>
                                            <th class="text-center cHeadIndicatorTable">Indicator</th>
                                            <th class="text-center cHeadAllTable">Unit</th>
                                            <th class="text-center cHeadAllTable">YTD</th>
                                            <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                            <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                            <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                            <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                            <th class="text-center cHeadAllTable QHead_2">May</th>
                                            <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                            <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                            <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                            <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                            <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                            <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                            <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                            <div id="divContentWaste" class="table-responsive">
                                <table id="tblWasteHZD" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                    <thead>
                                        <tr>
                                            <th class="text-center cHeadIndicatorTable">Indicator</th>
                                            <th class="text-center cHeadAllTable">Unit</th>
                                            <th class="text-center cHeadAllTable">YTD</th>
                                            <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                            <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                            <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                            <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                            <th class="text-center cHeadAllTable QHead_2">May</th>
                                            <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                            <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                            <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                            <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                            <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                            <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                            <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                    <tfoot style="font-size: 13px;"></tfoot>
                                </table>
                                <table id="tblWasteNHZD" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                    <thead>
                                        <tr>
                                            <th class="text-center cHeadIndicatorTable">Indicator</th>
                                            <th class="text-center cHeadAllTable">Unit</th>
                                            <th class="text-center cHeadAllTable">YTD</th>
                                            <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                            <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                            <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                            <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                            <th class="text-center cHeadAllTable QHead_2">May</th>
                                            <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                            <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                            <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                            <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                            <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                            <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                            <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                    <tfoot style="font-size: 13px;"></tfoot>
                                </table>
                            </div>
                            <div id="divContentEmission" class="table-responsive">
                                <table id="tblDataEmisionSummary" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                    <thead>
                                        <tr>
                                            <th class="text-center cHeadIndicatorTable">Indicator</th>
                                            <th class="text-center cHeadAllTable">Unit</th>
                                            <th class="text-center cHeadAllTable">YTD</th>
                                            <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                            <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                            <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                            <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                            <th class="text-center cHeadAllTable QHead_2">May</th>
                                            <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                            <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                            <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                            <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                            <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                            <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                            <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                                <table id="tblDataEmisionVOC" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                    <thead>
                                        <tr>
                                            <th class="text-center cHeadIndicatorTable">Indicator</th>
                                            <th class="text-center cHeadAllTable">Unit</th>
                                            <th class="text-center cHeadAllTable">YTD</th>
                                            <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                            <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                            <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                            <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                            <th class="text-center cHeadAllTable QHead_2">May</th>
                                            <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                            <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                            <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                            <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                            <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                            <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                            <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                            <div id="divContentSpill" class="table-responsive">
                                <table id="tblDataSpillSummary" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                    <thead>
                                        <tr>
                                            <th class="text-center cHeadIndicatorTable">Indicator</th>
                                            <th class="text-center cHeadAllTable">Unit</th>
                                            <th class="text-center cHeadAllTable">YTD</th>
                                            <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                            <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                            <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                            <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                            <th class="text-center cHeadAllTable QHead_2">May</th>
                                            <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                            <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                            <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                            <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                            <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                            <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                            <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                                <table id="tblDataSpill" class="table dataTable table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th class="dt-head-center dissort" style="width: 5%;">No</th>
                                            <th class="dt-head-center dissort" style="width: 20%;">Primary reason for loss of containment</th>
                                            <th class="dt-head-center dissort" style="width: 15%;">Spill Type</th>
                                            <th class="dt-head-center dissort" style="width: 15%;">Spill of</th>
                                            <th class="dt-head-center dissort" style="width: 15%;">Spill to</th>
                                            <th class="dt-head-center dissort" style="width: 15%;">Volume</th>
                                            <th class="dt-head-center dissort" style="width: 15%;">Spill Date</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                            <div id="divContentCompliance" class="table-responsive">
                                <table id="tblDataComplianceSummary" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                    <thead>
                                        <tr>
                                            <th class="text-center cHeadIndicatorTable">Indicator</th>
                                            <th class="text-center cHeadAllTable">Unit</th>
                                            <th class="text-center cHeadAllTable">YTD</th>
                                            <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                            <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                            <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                            <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                            <th class="text-center cHeadAllTable QHead_2">May</th>
                                            <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                            <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                            <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                            <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                            <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                            <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                            <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                                <table id="tblDataCompliance" class="table dataTable table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th class="dt-head-center dissort" style="width: 5%;">No</th>
                                            <th class="dt-head-center dissort" style="width: 20%;">Issue Date</th>
                                            <th class="dt-head-center dissort" style="width: 20%;">Document Number</th>
                                            <th class="dt-head-center dissort" style="width: 20%;">Issued by</th>
                                            <th class="dt-head-center dissort" style="width: 35%;">Subject</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                            <div id="divContentComplaint" class="table-responsive">
                                <table id="tblDataComplaintSummary" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                    <thead>
                                        <tr>
                                            <th class="text-center cHeadIndicatorTable">Indicator</th>
                                            <th class="text-center cHeadAllTable">Unit</th>
                                            <th class="text-center cHeadAllTable">YTD</th>
                                            <th class="text-center cHeadAllTable QHead_1">Jan</th>
                                            <th class="text-center cHeadAllTable QHead_1">Feb</th>
                                            <th class="text-center cHeadAllTable QHead_1">Mar</th>
                                            <th class="text-center cHeadAllTable QHead_2">Apr</th>
                                            <th class="text-center cHeadAllTable QHead_2">May</th>
                                            <th class="text-center cHeadAllTable QHead_2">Jun</th>
                                            <th class="text-center cHeadAllTable QHead_3">Jul</th>
                                            <th class="text-center cHeadAllTable QHead_3">Aug</th>
                                            <th class="text-center cHeadAllTable QHead_3">Sep</th>
                                            <th class="text-center cHeadAllTable QHead_4">Oct</th>
                                            <th class="text-center cHeadAllTable QHead_4">Nov</th>
                                            <th class="text-center cHeadAllTable QHead_4">Dec</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                                <table id="tblDataComplaint" class="table dataTable table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th class="dt-head-center dissort" style="width: 5%;">No</th>
                                            <th class="dt-head-center dissort" style="width: 20%;">Issue Date</th>
                                            <th class="dt-head-center dissort" style="width: 20%;">Issued by</th>
                                            <th class="dt-head-center dissort" style="width: 55%;">Subject</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Comment Request Edit --%>
            <div id="divCommentReqEdit" class="panel panel-warning">
                <div class="panel-heading">Request Edit Comment</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-xs-12 col-md-3">Comment <span class="text-red">*</span></label>
                            <div class="col-xs-12 col-md-9">
                                <asp:TextBox ID="txtCommentReqEdit" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Comment Request Edit --%>
            <div id="divCommentRecalcuate" class="panel panel-warning">
                <div class="panel-heading">Recalculate Comment</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-xs-12 col-md-3">Comment <span class="text-red">*</span></label>
                            <div class="col-xs-12 col-md-9">
                                <asp:TextBox ID="txtRecalculateComment" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <div class="text-center">
                <button type="button" id="btnSubmit" runat="server" class="btn btn-primary" onclick="SubmitData()">Submit</button>
                <button type="button" id="btnRequestEdit" runat="server" class="btn btn-warning" onclick="RequestEditData()">Request Edit</button>
                <button type="button" id="btnRecalcuate" runat="server" class="btn btn-danger" onclick="RecalculateData()" title="Reset workflow to L0">Recalculate</button>
                <a href="epi_transfertoptt_lst.aspx" class="btn btn-default">Back</a>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdfYearEncrypt" runat="server" />
    <asp:HiddenField ID="hdfFacIDEncrypt" runat="server" />
    <asp:HiddenField ID="hdfIndIDEncrypt" runat="server" />
    <asp:HiddenField ID="hdfQuareterEncrypt" runat="server" />
    <asp:HiddenField ID="hdfModeWF" runat="server" />
    <asp:HiddenField ID="hdfAllowRecalcualte" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        var $resposneMS = {};
        $(document).ready(function () {
            $("#divContentInten").hide();
            $("#divContentWaste").hide();
            $("#divContentEmission").hide();
            $("#divContentSpill").hide();
            $("#divContentCompliance").hide();
            $("#divContentComplaint").hide();

            if (Input("hdfModeWF").val() != "R") {
                $("#divCommentReqEdit").hide();
            } else {
                var objValidate = {};
                objValidate[GetElementName("txtCommentReqEdit", objControl.txtarea)] = addValidate_notEmpty(DialogMsg.Specify + " Comment");
                BindValidate("divCommentReqEdit", objValidate);
            }

            if (Input("hdfAllowRecalcualte").val() == "Y") {
                var objValidate = {};
                objValidate[GetElementName("txtRecalculateComment", objControl.txtarea)] = addValidate_notEmpty(DialogMsg.Specify + " Comment");
                BindValidate("divCommentRecalcuate", objValidate);
            } else {
                $("#divCommentRecalcuate").hide();
            }

            LoaddinProcess();
            AjaxCallWebMethod("GetDataOnPageLoad", function (response) {
                if (response.d.Status == SysProcess.Success) {
                    if (response.d.nIndID == 6) {
                        $("#divContentInten").show();
                        $resposneMS = response.d;
                        BindTableIntensity(response.d.lstDataInten, response.d.nOperationtypeID);
                    } else if (response.d.nIndID == 11) {
                        $("#divContentInten").show();
                        $resposneMS = response.d;
                        BindTableWater(response.d.lstDataWater);
                    }
                    else if (response.d.nIndID == 8) {
                        $("#divContentInten").show();
                        $resposneMS = response.d;
                        BindTableMaterial(response.d.lstDataMaterial);
                    } else if (response.d.nIndID == 10) {
                        $("#divContentWaste").show();
                        $resposneMS = response.d;
                        BindTableWaste(response.d.lstDataWaste);
                    } else if (response.d.nIndID == 4) {
                        $("#divContentEmission").show();
                        $resposneMS = response.d;
                        BindTableEmission(response.d.lstDataEmissionCombusion, response.d.lstDataEmissionVOC);
                    } else if (response.d.nIndID == 3) {
                        $("#divContentEmission").show();
                        $resposneMS = response.d;
                        BindTableEffluent(response.d.lstDataEffluentSumamry, response.d.lstDataEffluentOutput);
                    } else if (response.d.nIndID == 9) {
                        $("#divContentSpill").show();
                        $resposneMS = response.d;
                        BindTableSpill(response.d.lstDataSpillProduct, response.d.lstDataSpill);
                    } else if (response.d.nIndID == 2) {
                        $("#divContentCompliance").show();
                        $resposneMS = response.d;
                        BindTableCompliance(response.d.lstDataComplianceProduct, response.d.lstDataCompliance);
                    } else if (response.d.nIndID == 1) {
                        $("#divContentComplaint").show();
                        $resposneMS = response.d;
                        BindTableComplaint(response.d.lstDataComplaintProduct, response.d.lstDataComplaint);
                    } else {
                        HideLoadding();
                    }
                } else {
                    HideLoadding();
                }
            }, "", { sYear: Input("hdfYearEncrypt").val(), sFacID: Input("hdfFacIDEncrypt").val(), sIndID: Input("hdfIndIDEncrypt").val() });
        });

        function BindTableIntensity(lstData, OperaID) {
            var sTableID = "tblDataInten";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var htmlTD = "";
            if (lstData != null && lstData.length > 0) {
                var arrData = [];
                if (OperaID == 14) {//Building
                    arrData = Enumerable.From(lstData).Where(function (w) { return w.nProductID == 87; }).ToArray();
                    var qTotalArea = Enumerable.From(lstData).FirstOrDefault(null, function (w) { return w.nProductID == 86; });
                    if (qTotalArea != null)
                        $("#divModify1").html('<div class="form-group"><div class="col-xs-12"><label class="control-label col-xs-6 col-md-10 text-right">Total Area :</label><span class="col-xs-6 col-md-2" id="spTotalArea" style="text-align: left;">' + qTotalArea.sShowTotal + '&nbsp;m<sup>2</sup></span></div></div>');
                } else {
                    arrData = lstData;
                }
                $.each(arrData, function (indx, item) {
                    var sClass = item.cTotalAll == "Y" ? "cGreen" : item.cTotal == "Y" ? "cOrange" : item.nHeaderID;
                    var btn = item.cTotalAll == "N" && item.cTotal == "Y" ? '' + (item.cTotal == "Y" ? '<a id="a_' + item.nProductID + '" class="btn btn-default"'
                                    + 'onclick="DetailSub(' + item.nProductID + ');"><i id="i_' + item.nProductID + '" class="fas fa-chevron-down"></i></a>&nbsp;' : '') + '' : '';

                    htmlTD += '<tr class="' + sClass + '">';
                    htmlTD += '<td class="dt-body-left">' + btn + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                HideLoadding();
            } else {
                SetRowNoData(sTableID, 15);
                HideLoadding();
            }
        }

        function BindTableWater(lstData) {
            var sTableID = "tblDataInten";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var htmlTD = "";
            if (lstData != null && lstData.length > 0) {
                $.each(lstData, function (indx, item) {
                    var sClass = item.cTotalAll == "Y" ? "cGreen" : item.cTotal == "Y" ? "cOrange" : item.nHeaderID;
                    //var btn = item.cTotalAll == "N" && item.cTotal == "Y" ? '' + (item.cTotal == "Y" ? '<a id="a_' + item.nProductID + '" class="btn btn-default"'
                    // + 'onclick="DetailSub(' + item.nProductID + ');"><i id="i_' + item.nProductID + '" class="fas fa-chevron-down"></i></a>&nbsp;' : '') + '' : '';

                    htmlTD += '<tr class="' + sClass + '">';
                    htmlTD += '<td class="dt-body-left">' + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                HideLoadding();
            } else {
                SetRowNoData(sTableID, 15);
                HideLoadding();
            }
        }

        function BindTableMaterial(lstData) {
            var sTableID = "tblDataInten";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var htmlTD = "";
            if (lstData != null && lstData.length > 0) {
                $.each(lstData, function (indx, item) {
                    var sClass = item.sSetHeader == "Y" ? "cSubTotal3" : item.cTotalAll == "Y" ? "cGreen" : item.cTotal == "Y" ? "cOrange" : item.nHeaderID;
                    var btn = item.sSetHeader == "Y" && item.cTotalAll == "N" && item.cTotal == "Y" ? '' + (item.cTotal == "Y" ? '<a id="a_' + item.nProductID + '" class="btn btn-default"'
                                    + 'onclick="DetailSub(' + item.nProductID + ');"><i id="i_' + item.nProductID + '" class="fas fa-chevron-down"></i></a>&nbsp;' : '') + '' : '';

                    htmlTD += '<tr class="' + sClass + '">';
                    htmlTD += '<td class="dt-body-left">' + btn + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                HideLoadding();
            } else {
                SetRowNoData(sTableID, 15);
                HideLoadding();
            }
        }

        function BindTableWaste(lstData) {
            var htmlTD = "";

            //Hazardous Waste
            var dataHZD = Enumerable.From(lstData).Where(function (w) { return w.sType == "HZD" && w.nGroupCal != 99 && w.nGroupCal != 12; }).OrderBy("$.nOrder").ToArray();
            $.each(dataHZD, function (indx, item) {
                var sClass = item.cTotalAll == "Y" ? "cGreen" : item.cTotal == "Y" ? "cOrange" : item.sType + item.nGroupCal;
                var btn = item.cTotalAll == "N" && item.cTotal == "Y" ? '' + (item.cTotal == "Y" ? '<a id="a_' + item.sType + item.nGroupCal + '" class="btn btn-default"' + 'onclick="DetailSub(\'' + item.sType + item.nGroupCal + '\');"><i id="i_' + item.sType + item.nGroupCal + '" class="fas fa-chevron-down"></i></a>&nbsp;' : '') + '' : '';
                htmlTD += '<tr class="' + sClass + '">';
                htmlTD += '<td class="dt-body-left">' + btn + item.sProductName + '</td>';
                htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                htmlTD += '</tr>';
            });
            $("table[id$=tblWasteHZD] tbody").append(htmlTD);

            htmlTD = "";
            //On-Site Storage Hazardous Waste
            var qOnsiteHZD = Enumerable.From(lstData).FirstOrDefault(null, function (w) { return w.nProductID == 14; });
            if (qOnsiteHZD != null) {
                htmlTD += '<tr class="cOrange">';
                htmlTD += '<td class="text-left" rowspan="2">' + qOnsiteHZD.sProductName + '</td>';
                htmlTD += '<td class="text-center" rowspan="2">' + qOnsiteHZD.sUnit + '</td>';
                htmlTD += '<td class="text-center">Previous year</td>';
                htmlTD += '<td class="text-center">Reporting year</td>';
                htmlTD += '</tr>';
                htmlTD += '<tr class="cOrange">';
                htmlTD += '<td class="text-right">' + qOnsiteHZD.sShowPreviousYear + '</td>';
                htmlTD += '<td class="text-right">' + qOnsiteHZD.sShowReportingYear + '</td>';
                htmlTD += '</tr>';
            }

            //Hazardous Waste Generated
            var qWasteGenerateHZD = Enumerable.From(lstData).FirstOrDefault(null, function (w) { return w.nProductID == 15; });
            if (qWasteGenerateHZD) {
                htmlTD += '<tr class="cOrange">';
                htmlTD += '<td class="text-left">' + qWasteGenerateHZD.sProductName + '</td>';
                htmlTD += '<td class="text-center">' + qWasteGenerateHZD.sUnit + '</td>';
                htmlTD += '<td class="text-right">' + qWasteGenerateHZD.sShowTotal + '</td>';
                htmlTD += '</tr>';
            }
            $("table[id$=tblWasteHZD] tfoot").append(htmlTD);

            //Non-hazardous Waste
            htmlTD = "";
            var dataNHZD = Enumerable.From(lstData).Where(function (w) { return w.sType == "NHZD" && w.nGroupCal != 99 && w.nGroupCal != 12; }).ToArray();
            $.each(dataNHZD, function (indx, item) {
                var sClass = item.cTotalAll == "Y" ? "cGreen" : item.cTotal == "Y" ? "cOrange" : item.sType + item.nGroupCal;
                var btn = item.cTotalAll == "N" && item.cTotal == "Y" ? '' + (item.cTotal == "Y" ? '<a id="a_' + item.sType + item.nGroupCal + '" class="btn btn-default"' + 'onclick="DetailSub(\'' + item.sType + item.nGroupCal + '\');"><i id="i_' + item.sType + item.nGroupCal + '" class="fas fa-chevron-down"></i></a>&nbsp;' : '') + '' : '';
                htmlTD += '<tr class="' + sClass + '">';
                htmlTD += '<td class="dt-body-left">' + btn + item.sProductName + '</td>';
                htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                htmlTD += '</tr>';
            });
            $("table[id$=tblWasteNHZD] tbody").append(htmlTD);

            htmlTD = "";
            //On-Site Storage Non-hazardous Waste
            var qOnsiteNHZD = Enumerable.From(lstData).FirstOrDefault(null, function (w) { return w.nProductID == 31; });
            if (qOnsiteNHZD != null) {
                htmlTD += '<tr class="cOrange">';
                htmlTD += '<td class="text-left" rowspan="2">' + qOnsiteNHZD.sProductName + '</td>';
                htmlTD += '<td class="text-center" rowspan="2">' + qOnsiteNHZD.sUnit + '</td>';
                htmlTD += '<td class="text-center">Previous year</td>';
                htmlTD += '<td class="text-center">Reporting year</td>';
                htmlTD += '</tr>';
                htmlTD += '<tr class="cOrange">';
                htmlTD += '<td class="text-right">' + qOnsiteNHZD.sShowPreviousYear + '</td>';
                htmlTD += '<td class="text-right">' + qOnsiteNHZD.sShowReportingYear + '</td>';
                htmlTD += '</tr>';
            }

            //Non-hazardous Waste Generated
            var qWasteGenerateNHZD = Enumerable.From(lstData).FirstOrDefault(null, function (w) { return w.nProductID == 32; });
            if (qWasteGenerateNHZD) {
                htmlTD += '<tr class="cOrange">';
                htmlTD += '<td class="text-left">' + qWasteGenerateNHZD.sProductName + '</td>';
                htmlTD += '<td class="text-center">' + qWasteGenerateNHZD.sUnit + '</td>';
                htmlTD += '<td class="text-right">' + qWasteGenerateNHZD.sShowTotal + '</td>';
                htmlTD += '</tr>';
            }
            $("table[id$=tblWasteNHZD] tfoot").append(htmlTD);

            HideLoadding();
        }

        function BindTableEmission(lstDataCombussion, lstDataVOC) {
            var sTableID = "tblDataEmisionSummary";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var htmlTD = "";
            if (lstDataCombussion != null && lstDataCombussion.length > 0) {//บาง Operation type ไม่มี Stack
                $.each(lstDataCombussion, function (indx, item) {
                    var sClass = item.sType == "SUM" ? "cGreen" : "cBGWhite";
                    htmlTD += '<tr class="' + sClass + '">';
                    if (item.sType == "SUM") {
                        htmlTD += '<td class="dt-body-left">' + item.sProductName + '</td>';
                    }
                    else {
                        htmlTD += '<td class="dt-body-left"></td>';
                    }
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                $("table[id$=" + sTableID + "]").hide();
            }

            sTableID = "tblDataEmisionVOC";
            htmlTD = "";
            if (lstDataVOC != null && lstDataVOC.length > 0) {
                $.each(lstDataVOC, function (indx, item) {
                    var sClass = item.cTotal == "Y" ? "cGreen" : item.ProductID;
                    htmlTD += '<tr class="' + sClass + '">';
                    htmlTD += '<td class="dt-body-left">' + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                SetRowNoData(sTableID, 15);
            }
            HideLoadding();
        }

        function BindTableEffluent(lstDataSummary, lstDataOutput) {
            var sTableID = "tblDataEmisionSummary";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var htmlTD = "";
            if (lstDataSummary != null && lstDataSummary.length > 0) {
                $.each(lstDataSummary, function (indx, item) {
                    var sClass = item.sType == "1" ? "cGreen" : "1";
                    htmlTD += '<tr class="' + sClass + '">';
                    htmlTD += '<td class="dt-body-left">' + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                SetRowNoData(sTableID, 15);
            }

            sTableID = "tblDataEmisionVOC";
            $("#tblDataEmisionVOC th.cHeadIndicatorTable").width("370px");
            htmlTD = "";
            if (lstDataOutput != null && lstDataOutput.length > 0) {
                $.each(lstDataOutput, function (indx, item) {
                    htmlTD += '<tr>';
                    htmlTD += '<td class="dt-body-left">' + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                SetRowNoData(sTableID, 15);
            }
            HideLoadding();
        }

        function BindTableSpill(lstDataSummary, lstDataSpill) {
            var sTableID = "tblDataSpillSummary";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var htmlTD = "";
            if (lstDataSummary != null && lstDataSummary.length > 0) {
                $.each(lstDataSummary, function (indx, item) {
                    htmlTD += '<tr>';
                    htmlTD += '<td class="dt-body-left">' + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                SetRowNoData(sTableID, 15);
            }

            sTableID = "tblDataSpill";
            if (lstDataSpill != null && lstDataSpill.length > 0) {
                var htmlTD = '<tr><td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-left"></td>';
                htmlTD += '<td class="dt-body-left"></td>';
                htmlTD += '<td class="dt-body-left"></td>';
                htmlTD += '<td class="dt-body-left"></td>';
                htmlTD += '<td class="dt-body-right"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '</tr>';

                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                $("table[id$=" + sTableID + "] tbody tr").remove();
                var nNo = 1;
                $.each(lstDataSpill, function (indx, item) {
                    $("td", row).eq(0).html(nNo);

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
                    if (item.sSpillIcon == "1") {//Spill
                        sClassIcon = sClassIconSpill;
                        sTitle = sTitleSpill;
                        sColor = sColorSpill;
                    } else if (item.sSpillIcon == "2") {//Sign. Spill
                        sClassIcon = sClassIconSignificantSpill;
                        sTitle = sTitleSignificantSpill;
                        sColor = sColorSignificantSpill;
                    } else {//None
                        sClassIcon = sClassIconNoneSpill;
                        sTitle = sTitleNoneSpill;
                    }
                    var HtmlIcon = "&nbsp;<i style='font-size:20px;" + sColor + "' class='" + sClassIcon + "' title='" + sTitle + "'></i>";

                    $("td", row).eq(1).html(item.sPrimaryReasonName + HtmlIcon);
                    $("td", row).eq(2).html(item.sSpillTypeName);
                    $("td", row).eq(3).html(item.sSpillOfName);
                    $("td", row).eq(4).html(item.sSpillToName);

                    var htmlSensitiveArea = '';
                    if (item.sIsSensitiveArea == "Y") {
                        htmlSensitiveArea = '<span class="text-red" title="Spill to sensitive area">*</span>';
                    }

                    $("td", row).eq(5).html(item.sVolume + " " + item.sUnitName + " " + htmlSensitiveArea);
                    $("td", row).eq(6).html(item.sSpillDate);

                    $("table[id$=" + sTableID + "] tbody").append(row);
                    row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                    nNo++
                });
                SetTootip();
            } else {
                SetRowNoData(sTableID, 7);
            }
            HideLoadding();
        }

        function BindTableCompliance(lstDataSummary, lstDataCompliance) {
            var sTableID = "tblDataComplianceSummary";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var htmlTD = "";
            if (lstDataSummary != null && lstDataSummary.length > 0) {
                $.each(lstDataSummary, function (indx, item) {
                    htmlTD += '<tr>';
                    htmlTD += '<td class="dt-body-left">' + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                SetRowNoData(sTableID, 15);
            }

            sTableID = "tblDataCompliance";
            if (lstDataCompliance != null && lstDataCompliance.length > 0) {
                var htmlTD = '<tr><td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-left"></td>';
                htmlTD += '</tr>';

                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                $("table[id$=" + sTableID + "] tbody tr").remove();
                var nNo = 1;
                $.each(lstDataCompliance, function (indx, item) {
                    $("td", row).eq(0).html(nNo);
                    $("td", row).eq(1).html(item.sComplianceDate);
                    $("td", row).eq(2).html(item.sDocNumber);
                    $("td", row).eq(3).html(item.sIssueBy);
                    $("td", row).eq(4).html(item.sSubject);

                    $("table[id$=" + sTableID + "] tbody").append(row);
                    row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                    nNo++
                });
                SetTootip();
            } else {
                SetRowNoData(sTableID, 5);
            }
            HideLoadding();
        }

        function BindTableComplaint(lstDataSummary, lstDataComplaint) {
            var sTableID = "tblDataComplaintSummary";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            var htmlTD = "";
            if (lstDataSummary != null && lstDataSummary.length > 0) {
                $.each(lstDataSummary, function (indx, item) {
                    htmlTD += '<tr>';
                    htmlTD += '<td class="dt-body-left">' + item.sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    htmlTD += '<td class="dt-body-right">' + item.sShowTotal + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM1 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM2 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_1">' + item.sShowM3 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM4 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM5 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_2">' + item.sShowM6 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM7 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM8 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_3">' + item.sShowM9 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM10 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM11 + '</td>';
                    htmlTD += '<td class="dt-body-right QHead_4">' + item.sShowM12 + '</td>';
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                SetRowNoData(sTableID, 15);
            }

            sTableID = "tblDataComplaint";
            if (lstDataComplaint != null && lstDataComplaint.length > 0) {
                var htmlTD = '<tr><td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-center"></td>';
                htmlTD += '<td class="dt-body-left"></td>';
                htmlTD += '</tr>';

                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
                var row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                $("table[id$=" + sTableID + "] tbody tr").remove();
                var nNo = 1;
                $.each(lstDataComplaint, function (indx, item) {
                    $("td", row).eq(0).html(nNo);
                    $("td", row).eq(1).html(item.sComplaintDate);
                    $("td", row).eq(2).html(item.sIssueBy);
                    $("td", row).eq(3).html(item.sSubject);

                    $("table[id$=" + sTableID + "] tbody").append(row);
                    row = $("table[id$=" + sTableID + "] tbody").find("tr").last().clone(true);
                    nNo++
                });
                SetTootip();
            } else {
                SetRowNoData(sTableID, 4);
            }
            HideLoadding();
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

        function GetSelectQuarter() {
            var arr = [];
            $.each($("input[id*=cklQuarter]:checked"), function () { arr.push($(this).val()); });
            return arr;
        }

        function SubmitData() {
            var arrQuarter = GetSelectQuarter();
            if (arrQuarter.length > 0) {
                DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmSubmit, function () {
                    var dataValue = {
                        sYear: Input("hdfYearEncrypt").val(),
                        sFacID: Input("hdfFacIDEncrypt").val(),
                        sIndID: Input("hdfIndIDEncrypt").val(),
                        arrQuater: arrQuarter
                    }
                    AjaxCallWebMethod("SubmitData", function (response) {
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupLogin();
                        } else if (response.d.Status == SysProcess.Success) {
                            var html = '';
                            if (response.d.lstResultWSV != null && response.d.lstResultWSV.length > 0) {
                                html = '<table class="table dataTable table-bordered table-hover">';
                                html += '<thead><tr><th class="dt-head-center dissort" style="width:10%">Quarter</th><th class="dt-head-center dissort" style="width:15%">Status</th><th class="dt-head-center dissort" style="width:75%">Message</th></tr></thead>';
                                html += '<tbody>';
                                $.each(response.d.lstResultWSV, function (indx, item) {
                                    html += '<tr>';
                                    html += '<td class="dt-body-center">' + item.nQuarter + '</td>';
                                    html += '<td class="dt-body-center">' + (item.IsPass == true ? "Passed" : "Failed") + '</td>';
                                    html += '<td class="dt-body-left">' + item.sMsg + '</td>';
                                    html += '</tr>';
                                });
                                html += '</tbody>';
                                html += '</table>';
                            }
                            DialogSuccessRedirect(DialogHeader.Info, html, "epi_transfertoptt_lst.aspx");
                        } else {
                            var html = '';
                            if (response.d.lstResultWSV != null && response.d.lstResultWSV.length > 0) {
                                html = '<br/><table class="table dataTable table-bordered table-hover">';
                                html += '<thead><tr><th class="dt-head-center dissort" style="width:10%">Quarter</th><th class="dt-head-center dissort" style="width:15%">Status</th><th class="dt-head-center dissort" style="width:75%">Message</th></tr></thead>';
                                html += '<tbody>';
                                $.each(response.d.lstResultWSV, function (indx, item) {
                                    html += '<tr>';
                                    html += '<td class="dt-body-center">' + item.nQuarter + '</td>';
                                    html += '<td class="dt-body-center">' + (item.IsPass == true ? "Passed" : "Failed") + '</td>';
                                    html += '<td class="dt-body-left">' + item.sMsg + '</td>';
                                    html += '</tr>';
                                });
                                html += '</tbody>';
                                html += '</table>';
                            }
                            DialogWarning(DialogHeader.Warning, response.d.Msg + html);
                        }
                        HideLoadding();
                    }, "", { objData: $resposneMS, data: dataValue });
                }, "");
            } else {
                DialogWarning(DialogHeader.Warning, "Please select quarter !");
            }
        }

        function RequestEditData() {
            if (CheckValidate("divCommentReqEdit")) {
                var arrQuarter = GetSelectQuarter();
                if (arrQuarter.length > 0) {
                    DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmRequest, function () {
                        var dataValue = {
                            sYear: Input("hdfYearEncrypt").val(),
                            sFacID: Input("hdfFacIDEncrypt").val(),
                            sIndID: Input("hdfIndIDEncrypt").val(),
                            arrQuater: arrQuarter,
                            sComment: GetValTextArea("txtCommentReqEdit")
                        }
                        AjaxCallWebMethod("RequestEditData", function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupLogin();
                            } else if (response.d.Status == SysProcess.Success) {
                                var html = '';
                                if (response.d.lstResultWSV != null && response.d.lstResultWSV.length > 0) {
                                    html = '<table class="table dataTable table-bordered table-hover">';
                                    html += '<thead><tr><th class="dt-head-center dissort" style="width:10%">Quarter</th><th class="dt-head-center dissort" style="width:15%">Status</th><th class="dt-head-center dissort" style="width:75%">Message</th></tr></thead>';
                                    html += '<tbody>';
                                    $.each(response.d.lstResultWSV, function (indx, item) {
                                        html += '<tr>';
                                        html += '<td class="dt-body-center">' + item.nQuarter + '</td>';
                                        html += '<td class="dt-body-center">' + (item.IsPass == true ? "Passed" : "Failed") + '</td>';
                                        html += '<td class="dt-body-left">' + item.sMsg + '</td>';
                                    });
                                    html += '</tbody>';
                                    html += '</table>';
                                }
                                DialogSuccessRedirect(DialogHeader.Info, html, "epi_transfertoptt_lst.aspx");
                            } else {
                                var html = '';
                                if (response.d.lstResultWSV != null && response.d.lstResultWSV.length > 0) {
                                    html = '<br/><table class="table dataTable table-bordered table-hover">';
                                    html += '<thead><tr><th class="dt-head-center dissort" style="width:10%">Quarter</th><th class="dt-head-center dissort" style="width:15%">Status</th><th class="dt-head-center dissort" style="width:75%">Message</th></tr></thead>';
                                    html += '<tbody>';
                                    $.each(response.d.lstResultWSV, function (indx, item) {
                                        html += '<tr>';
                                        html += '<td class="dt-body-center">' + item.nQuarter + '</td>';
                                        html += '<td class="dt-body-center">' + (item.IsPass == true ? "Passed" : "Failed") + '</td>';
                                        html += '<td class="dt-body-left">' + item.sMsg + '</td>';
                                    });
                                    html += '</tbody>';
                                    html += '</table>';
                                }
                                DialogWarning(DialogHeader.Warning, response.d.Msg + html);
                            }
                            HideLoadding();
                        }, "", { data: dataValue });
                    }, "");
                } else {
                    DialogWarning(DialogHeader.Warning, "Please select quarter !");
                }
            }
        }

        function RecalculateData() {
            var arrQuarter = GetSelectQuarter();
            if (arrQuarter.length > 0) {
                if (CheckValidate("divCommentRecalcuate")) {
                    DialogConfirm(DialogHeader.Confirm, "Do you want to recalculate ?", function () {
                        var dataValue = {
                            sYear: Input("hdfYearEncrypt").val(),
                            sFacID: Input("hdfFacIDEncrypt").val(),
                            sIndID: Input("hdfIndIDEncrypt").val(),
                            arrQuater: arrQuarter,
                            sComment: GetValTextArea("txtRecalculateComment")
                        }
                        AjaxCallWebMethod("RecalculateToL0", function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupLogin();
                            } else if (response.d.Status == SysProcess.Success) {
                                DialogSuccessRedirect(DialogHeader.Info, "Already recalculated data.", "epi_transfertoptt_lst.aspx");
                            } else {
                                DialogWarning(DialogHeader.Warning, response.d.Msg);
                            }
                            HideLoadding();
                        }, "", { data: dataValue });
                    }, "");
                }
            } else {
                DialogWarning(DialogHeader.Warning, "Please select quarter !");
            }
        }
    </script>
</asp:Content>
