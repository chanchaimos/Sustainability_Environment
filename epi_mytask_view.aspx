<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="epi_mytask_view.aspx.cs" Inherits="epi_mytask_view" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        .modal-primary {
            background-color: #286090;
            color: #FFFFFF;
        }

        .flat-green label {
            padding-left: 10%;
        }
    </style>

    <style>
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

        .cExStyle {
            text-align: right;
            margin-bottom: 10px;
        }

        label.headOther {
            background-color: #fdb813 !important;
            width: 100% !important;
            padding: 8px !important;
            color: #fff !important;
        }

        #divContent table {
            margin-top: -1px !important;
        }

        .table-responsive {
            max-height: 400px !important;
            overflow: auto !important;
            margin-bottom: 10px !important;
        }

        table thead th:not(:nth-child(1)) {
            z-index: 3 !important;
        }

        table thead th:nth-child(1) {
            z-index: 4 !important;
        }

        table tbody td:nth-child(1) {
            z-index: 2 !important;
        }

        table tbody td:not(:nth-child(1)) {
            z-index: 1 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp;Approve / Reject</div>
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
            <%-- HIstory --%>
            <div class="row" style="margin-bottom: 10px;">
                <div class="col-xs-12 text-right">
                    <button type="button" onclick="ShowHistory();" class="btn btn-info" title="Workflow History"><i class="fas fa-comment-alt"></i></button>
                </div>
            </div>

            <%-- Select Month & Detail --%>
            <div class="panel panel-info">
                <div class="panel-heading">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-xs-12">
                                <i class="glyphicon glyphicon-check"></i>Observed deviation on this month :
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-4 col-lg-1 cCKBM1">
                                <asp:CheckBox ID="cbMoth_1" runat="server" CssClass="flat-green" Text="Jan." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM2">
                                <asp:CheckBox ID="cbMoth_2" runat="server" CssClass="flat-green" Text="Feb." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM3">
                                <asp:CheckBox ID="cbMoth_3" runat="server" CssClass="flat-green" Text="Mar." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM4">
                                <asp:CheckBox ID="cbMoth_4" runat="server" CssClass="flat-green" Text="Apr." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM5">
                                <asp:CheckBox ID="cbMoth_5" runat="server" CssClass="flat-green" Text="May" />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM6">
                                <asp:CheckBox ID="cbMoth_6" runat="server" CssClass="flat-green" Text="Jun." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM7">
                                <asp:CheckBox ID="cbMoth_7" runat="server" CssClass="flat-green" Text="Jul." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM8">
                                <asp:CheckBox ID="cbMoth_8" runat="server" CssClass="flat-green" Text="Aug." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM9">
                                <asp:CheckBox ID="cbMoth_9" runat="server" CssClass="flat-green" Text="Sep." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM10">
                                <asp:CheckBox ID="cbMoth_10" runat="server" CssClass="flat-green" Text="Oct." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM11">
                                <asp:CheckBox ID="cbMoth_11" runat="server" CssClass="flat-green" Text="Nov." />
                            </div>
                            <div class="col-xs-4 col-lg-1 cCKBM12">
                                <asp:CheckBox ID="cbMoth_12" runat="server" CssClass="flat-green" Text="Dec." />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body no-padding">
                    <div class="row" id="outPut">
                        <%-- Total Area --%>
                        <div class="col-xs-12 cExStyle" id="divTotalArea">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <label class="control-label col-xs-10 text-right">Total Area</label>
                                    <span class="col-xs-2" id="spTotalArea" style="text-align: left;"></span>
                                </div>
                            </div>
                        </div>

                        <%-- TABLE --%>
                        <div class="col-xs-12" id="divTable" style="margin-top: 10px;">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <label id="lbForCombusion" class="headOther">Combusion</label>
                                    <div class="table-responsive">
                                        <table id="tbData" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
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
                                </div>
                            </div>

                            <div class="form-group" id="divTableSpill">
                                <div class="col-xs-12">
                                    <label class="headOther">Spill</label>
                                    <div class="table-responsive">
                                        <table id="tbDataSpill" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                            <thead>
                                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">Spill</th>
                                </tr>--%>
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
                                </div>
                            </div>

                            <div class="form-group" id="divTableSignificantSpill">
                                <div class="col-xs-12">
                                    <label class="headOther">Significant Spill</label>
                                    <div class="table-responsive">
                                        <table id="tbDataSignificantSpill" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                            <thead>
                                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">Significant Spill</th>
                                </tr>--%>
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
                                </div>
                            </div>

                            <div class="form-group" id="divNonCombustion">
                                <div class="col-xs-12">
                                    <label class="headOther">Non-Combustion</label>
                                    <div class="table-responsive">
                                        <table id="tbDataNonCombution" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                            <thead>
                                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">Non-Combustion</th>
                                </tr>--%>
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
                                </div>
                            </div>

                            <div class="form-group" id="divCEM">
                                <div class="col-xs-12">
                                    <label class="headOther">CEM</label>
                                    <div class="table-responsive">
                                        <table id="tbDataCEM" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                            <thead>
                                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">CEM</th>
                                </tr>--%>
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
                                </div>
                            </div>

                            <div class="form-group" id="divAdditionalCombustion">
                                <div class="col-xs-12">
                                    <label class="headOther">Additional Combustion</label>
                                    <div class="table-responsive">
                                        <table id="tbDataAdditionalCombustion" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                            <thead>
                                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">Additional Combustion</th>
                                </tr>--%>
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
                                </div>
                            </div>

                            <div class="form-group" id="divAdditionalNonCombustion">
                                <div class="col-xs-12">

                                    <label class="headOther">Additional Non-Combustion</label>
                                    <div class="table-responsive">
                                        <table id="tbDataAdditionalNonCombustion" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                            <thead>
                                                <%-- <tr>
                                    <th colspan="15" class="text-left thOther">Additional Non-Combustion</th>
                                </tr>--%>
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
                                </div>
                            </div>

                            <div class="form-group" id="divVOC">
                                <div class="col-xs-12">

                                    <label class="headOther">VOC</label>
                                    <div class="table-responsive">
                                        <table id="tbDataVOC" class="table dataTable table-bordered table-hover" style="width: 2030px; min-width: 100%">
                                            <thead>
                                                <%--<tr>
                                    <th colspan="15" class="text-left thOther">VOC</th>
                                </tr>--%>
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-info col" id="divRemarkDeviate" style="display: none">
                <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divDeviate">
                    Remark on deviation data
                </div>
                <div class="panel-body panel-collapse collapse in" id="divDeviate">
                </div>
            </div>
            <%-- Button --%>
            <div class="row" id="btnSection">
                <div class="col-lg-12 text-center">
                    <button type="button" id="btnAppr" data-mode="AP" class="btn btn-success"><i class="fa fa-check"></i>&nbsp;Approve</button>
                    <button type="button" id="btnReject" class="btn btn-danger" data-toggle="modal" data-target="#Comment"><i class="fa fa-times"></i>&nbsp;Reject</button>
                    <button type="button" id="btnAppr_with" data-mode="APC" class="btn btn-warning"><i class="fa fa-check"></i>&nbsp;Approve With Edit Content</button>
                    <button type="button" id="btnAppr_Re" data-mode="ACE" class="btn btn-primary"><i class="fa fa-check"></i>&nbsp;Accept Rquest Edit</button>
                    <a class="btn btn-default" href="epi_mytask.aspx">Back</a>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidFormID" runat="server" />
    <asp:HiddenField ID="hidLevel" runat="server" />

    <asp:HiddenField ID="hidIndicator" runat="server" />
    <asp:HiddenField ID="hidOperationType" runat="server" />
    <asp:HiddenField ID="hidFacility" runat="server" />
    <asp:HiddenField ID="hidYear" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <%--Modal--%>
    <div class="modal fade" id="Comment" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header modal-primary">
                    <h4 class="modal-title">Comment</h4>
                </div>
                <div class="modal-body">
                    <div id="divPopContentRemark">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <textarea id="txtComment" name="txtComment" class="form-control" rows="5"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="btnCancel" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" id="btnSaveComment" data-mode="RJ" class="btn btn-primary">Save</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="popDetail" class="modal fade col-xs-12" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
        <div class="modal-dialog" role="document" style="width: 60%;">
            <div class="modal-content">
                <div class="modal-header cModal">
                    <h4 class="modal-title">History</h4>
                </div>
                <div class="modal-body">
                    <div id="divPopContentDetail">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table id="tbDetail" class="table dataTable table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th class="dt-head-center dissort" style="width: 10%;">Date</th>
                                                <th class="dt-head-center dissort" style="width: 15%;">Status</th>
                                                <th class="dt-head-center dissort" style="width: 15%;">Month</th>
                                                <th class="dt-head-center dissort" style="width: 30%;">Remark</th>
                                                <th class="dt-head-center dissort" style="width: 15%;">Action By</th>
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
                        <button class="btn" type="button" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            var level = +$('input[id$=hidLevel]').val() || 0
            if (level == 3) {
                $('#btnAppr_with').remove();
                $('#btnAppr_Re').remove();
            }
            else if (level == 4) {

            }
            else {

            }
            //$('input[id*=cbMoth]').iCheck('check');
            $('input[id*=cbMoth]').prop('disabled', true);
            for (var i = 1; i <= 12; i++) {
                $(".cCKBM" + i).hide();
            }

            $('#btnAppr').on('click', function () {
                var sMode = $(this).attr('data-mode');
                SaveAction("", sMode);
            });
            $('#btnAppr_with').on('click', function () {
                ApprWith();
            });
            $('#btnAppr_Re').on('click', function () {
                var sMode = $(this).attr('data-mode');
                SaveAction("", sMode);
            });

            $('#btnSaveComment').on('click', function () {
                if (CheckValidate("divPopContentRemark")) {
                    $('#btnCancel').click();
                    var sMode = $(this).attr('data-mode');
                    SaveAction($('#txtComment').val(), sMode);
                }
            });

            //Popup Remark
            $('#Comment').on('shown.bs.modal', function (e) {
                var objValidate = {};
                objValidate[GetElementName("txtComment", objControl.txtarea)] = addValidate_notEmpty("Please Specifiy Comment.");
                BindValidate("divPopContentRemark", objValidate);

            });

            $('input[id*=cbMoth]').on("ifChanged", function () {
                GetDeviate();
            })

            $("#divTable").hide();
            $("#divTotalArea").hide();
            $("#btnAppr").hide();
            $("#btnReject").hide();
            $("#btnAppr_with").hide();
            $("#btnAppr_Re").hide();
            LoadData();
            GetMonth();
        });

        function GetMonth() {
            AjaxCallWebMethod("GetMonth", function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    //PopupLogin();
                }
                else if (data.d.Status == SysProcess.Success) {
                    var r = data.d.lstMonth;
                    if (r.length > 0) {
                        $.each(r, function (i, e) {
                            $('input[id$=cbMoth_' + e + ']').prop('disabled', false);
                            $(".cCKBM" + e).show();
                        });
                    }

                    $.each(data.d.lstShowButton, function (indx, item) {
                        $("#" + item).show();
                    });
                }

            }, UnblockUI, {
                FormID: +$('input[id$=hidFormID]').val() || 0,
                nLevel: +$('input[id$=hidLevel]').val() || 0
            });
        }

        function ApprWith() {
            AjaxCallWebMethod('ApproveWithEditContent', function (res) {
                if (res.d.Status == "Success") {
                    window.location.href = res.d.Msg;
                }
                else if (res.d.Status == "Failed") {
                    DialogError(DialogHeader.Error, "Invalid URL");
                }
                else {
                    PopupLogin();
                }
            }, function () {
                HideLoadding();
            }, {
                FormID: $('input[id$=hidFormID]').val()
            });
        }

        function SaveAction(sComent, Mode) {
            LoaddinProcess();
            var objMonth = [];
            $('input[id*=cbMoth]').map(function (i, e) {
                if (!$(this).is(':disabled')) {
                    if (GetValueCheckBoxiCheck(e.id)) {
                        var gMonth = e.id.split('_')[3];
                        objMonth.push(gMonth);
                    }
                }
            });

            if (objMonth.length > 0) {
                AjaxCallWebMethod('SaveAction', function (res) {
                    if (res.d.Status == "Success") {
                        DialogSuccessRedirect(DialogHeader.Info, DialogMsg.SaveComplete, "epi_mytask.aspx");
                    }
                    else if (res.d.Status == "Failed") {
                        DialogWarning(DialogHeader.Warning, res.d.Msg);
                    }
                    else {
                        PopupLogin();
                    }
                }, function () {
                    HideLoadding();
                }, {
                    lstMonth: objMonth,
                    FormID: $('input[id$=hidFormID]').val(),
                    sComment: sComent,
                    sMode: Mode
                });
            } else {
                HideLoadding();
                DialogWarning(DialogHeader.Warning, "Please select month !");
            }
        }

        function LoadData() {
            BlockUI();
            var item = {
                nIndicator: +$('input[id$=hidIndicator]').val() || 0,
                nOperationType: +$('input[id$=hidOperationType]').val() || 0,
                nFacility: +$('input[id$=hidFacility]').val() || 0,
                sYear: +$('input[id$=hidYear]').val() || 0,
            };

            AjaxCallWebMethod("LoadData", function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    //PopupLogin();
                }
                else if (data.d.Status == SysProcess.Success) {
                    arrData = data.d.lstData;
                    arrDataNonCombustion = data.d.lstDataNonCombustion;
                    arrDataCEM = data.d.lstDataCEM;
                    arrDataAdditionalCombustion = data.d.lstDataAdditionalCombustion;
                    arrDataAdditionalNonCombustion = data.d.lstDataAdditionalNonCombustion;
                    arrDataVOC = data.d.lstDataVOC;
                    if ($('input[id$=hidIndicator]').val() == "4") {
                        $("label[id$=lbForCombusion]").show();
                        if ($('input[id$=hidOperationType]').val() != 13) {
                            BindTableOther(arrDataNonCombustion, "tbDataNonCombution");
                            BindTableOther(arrDataCEM, "tbDataCEM");
                            BindTableOther(arrDataAdditionalCombustion, "tbDataAdditionalCombustion");
                            BindTableOther(arrDataAdditionalNonCombustion, "tbDataAdditionalNonCombustion");
                            $("div[id$=divNonCombustion]").show();
                            $("div[id$=divCEM]").show();
                            $("div[id$=divAdditionalCombustion]").show();
                            $("div[id$=divAdditionalNonCombustion]").show();
                        } else {
                            $("div[id$=divNonCombustion]").hide();
                            $("div[id$=divCEM]").hide();
                            $("div[id$=divAdditionalCombustion]").hide();
                            $("div[id$=divAdditionalNonCombustion]").hide();
                        }
                        BindTableOther(arrDataVOC, "tbDataVOC");
                        $("div[id$=divVOC]").show();
                    } else {
                        $("label[id$=lbForCombusion]").hide();
                        $("div[id$=divNonCombustion]").hide();
                        $("div[id$=divCEM]").hide();
                        $("div[id$=divAdditionalCombustion]").hide();
                        $("div[id$=divAdditionalNonCombustion]").hide();
                        $("div[id$=divVOC]").hide();
                    }
                    BindTable();
                    $("#divTable").show();
                    if (arrData.length > 0) {
                        $("#divExport").show();
                    }
                }

            }, function () {
                $.each($("div[id$=divTable]").find("table"), function (i, el) {
                    el = $(el);
                    if (el.find("tbody tr").length == 1 && el.find("tbody tr td:eq(0)").has("colspan")) {
                        $("table#" + el.attr("id")).tableHeadFixer({ head: true });
                    } else {
                        $("table#" + el.attr("id")).tableHeadFixer({ "left": 1, head: true });
                    }
                })
                UnblockUI();
            }, { item: item });
        }

        function BindTable() {
            var sTableID = "tbData";
            if (arrData.length > 0) {
                var nOperationType = +$('input[id$=hidOperationType]').val() || 0;
                var nIndicator = +$('input[id$=hidIndicator]').val() || 0;
                $("table[id$=" + sTableID + "] tbody tr").remove();
                var htmlTD = "";
                if (nIndicator == 6 && nOperationType == 14) {
                    $("#divTotalArea").show();
                }
                else {
                    $("#divTotalArea").hide();
                }

                if (Enumerable.From(arrData).Where(function (w) { return w.IDIndicator == 9 }).ToArray().length > 0) {
                    BindTableOther(Enumerable.From(arrData).Where(function (w) { return w.sMakeField2 == "1" || w.IDIndicator == "0" }).ToArray(), "tbDataSpill");
                    BindTableOther(Enumerable.From(arrData).Where(function (w) { return w.sMakeField2 == "2" || w.IDIndicator == "0" }).ToArray(), "tbDataSignificantSpill");
                    $("#divTableSpill").show();
                    $("#divTableSignificantSpill").show();
                    arrData = Enumerable.From(arrData).Where(function (w) { return w.sMakeField2 == "0" }).ToArray();
                } else {
                    $("#divTableSpill").hide();
                    $("#divTableSignificantSpill").hide();
                }
                $.each(arrData, function (indx, item) {
                    if (item.IDIndicator == 6 && item.OperationtypeID == 14 && item.ProductID == 86 && item.sType == "TotalArea") {
                        $("#spTotalArea").html(item.sTotalArea);
                        $("input[id$=hidTotalArea]").val(item.sTotalArea);
                    }
                    else {
                        var sClass = item.sType == "Head" ? "cOrange" : item.sType == "Group" ? "cGreen" : item.sType == "Sub" ? item.nHeadID : "";
                        var btn = item.sType == "Head" ? '' + (item.isSub == true ? '<a id="a_' + item.ProductID + '" class="btn btn-default"'
                            + 'onclick="DetailSub(' + item.ProductID + ');"><i id="i_' + item.ProductID + '" class="fas fa-chevron-down"></i></a>&nbsp;' : '') + '' : '';

                        htmlTD += '<tr class="' + sClass + '">';
                        htmlTD += '<td class="dt-body-left">' + btn + item.ProductName + '</td>';
                        htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                        htmlTD += '<td class="dt-body-center">' + item.sTotal + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_1">' + item.sM1 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_1">' + item.sM2 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_1">' + item.sM3 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_2">' + item.sM4 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_2">' + item.sM5 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_2">' + item.sM6 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_3">' + item.sM7 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_3">' + item.sM8 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_3">' + item.sM9 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_4">' + item.sM10 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_4">' + item.sM11 + '</td>';
                        htmlTD += '<td class="dt-body-center QHead_4">' + item.sM12 + '</td>';
                        htmlTD += '</tr>';
                    }
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);

            }
            else {
                SetRowNoData(sTableID, 15);
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

        function BindTableOther(lstData, sTableID) {
            var htmlTD = "";
            $("table[id$=" + sTableID + "] tbody tr").remove();
            if (lstData.length > 0) {
                $.each(lstData, function (indx, item) {
                    var sStyle = "";
                    var sProductName = "";
                    if (item.sType == "SUM" || item.UnitID == "2" || item.ProductID == "193") {
                        sProductName = item.ProductName;
                        sStyle = "style='background-color:#dbea97;'";
                    } else if (item.sType == "SUM2" || item.UnitID == "68") {
                        sProductName = "";
                        sStyle = "";
                    } else {
                        sProductName = item.ProductName;
                        sStyle = "";
                    }
                    htmlTD += '<tr ' + sStyle + '>';
                    htmlTD += '<td class="dt-body-left">' + sProductName + '</td>';
                    htmlTD += '<td class="dt-body-center">' + item.sUnit + '</td>';
                    if ($('input[id$=hidIndicator]').val() == "4") {
                        htmlTD += '<td class="dt-body-right">' + item.nTotal + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.M1 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.M2 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.M3 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.M4 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.M5 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.M6 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.M7 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.M8 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.M9 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.M10 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.M11 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.M12 + '</td>';
                    } else {
                        htmlTD += '<td class="dt-body-right">' + item.sTotal + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM1 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM2 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_1">' + item.sM3 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM4 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM5 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_2">' + item.sM6 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM7 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM8 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_3">' + item.sM9 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM10 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM11 + '</td>';
                        htmlTD += '<td class="dt-body-right QHead_4">' + item.sM12 + '</td>';
                    }
                    htmlTD += '</tr>';
                });
                $("table[id$=" + sTableID + "] tbody").append(htmlTD);
            } else {
                SetRowNoData(sTableID, 15);
            }
        }

        function GetDeviate() {
            var lstSearch = {
                nIncID: +Input("hidIndicator").val(),
                nOprtID: +Input("hidOperationType").val(),
                nFacID: +Input("hidFacility").val(),
                sYear: Input("hidYear").val(),
                lstMonth: $.map($('input[id*=cbMoth_]:visible:checked'), function (el, i) {
                    return el.id.split('_')[3];
                })
            }
            AjaxCallWebMethod("GetDataDeviate", function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupLogin();
                }
                else if (response.d.Status == SysProcess.Success) {
                    $("div#divDeviate").empty();
                    if (response.d.lstMonth.length > 0) {
                        bindDeviate(response.d.lstMonth);
                        $("div#divRemarkDeviate").show();
                    } else {
                        $("div#divRemarkDeviate").hide();
                    }
                }
            }, function () { }, { lstSearch: lstSearch });
        }

        function bindDeviate(lstMonth) {
            var arrFullMonth = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var sHtml = '';
            sHtml += '<div id="divValidateDeviate">';
            $.each(lstMonth, function (i, el) {
                sHtml += ' <div class="panel panel-info">';
                sHtml += '    <div class="panel-heading" style="cursor: pointer" data-toggle="collapse" href="#divMonth_' + el.nMonth + '">';
                sHtml += arrFullMonth[el.nMonth - 1];
                sHtml += '    </div>';
                sHtml += '    <div id="divMonth_' + el.nMonth + '" class="panel-collapse collapse in">';
                sHtml += '        <div class="panel-body">';
                sHtml += '<div class="form-group">';
                sHtml += '        <span>' + el.sRemark + '</span>';
                sHtml += '</div>';
                sHtml += '        </div>';
                sHtml += '    </div>';
                sHtml += '</div>';
            })
            sHtml += '</div>';
            $("div#divDeviate").append(sHtml);
        }

        /////////************ SHOW HISTORY *************\\\\\\\\\\\\
        function ShowHistory() {
            BlockUI();
            $.ajax({
                dataType: 'html',
                type: 'POST',
                url: './Ashx/ShowDetailMonth.ashx',
                data: {
                    status: status,
                    nIncID: $('input[id$=hidIndicator]').val(),
                    nOprtID: $('input[id$=hidOperationType]').val(),
                    nFacID: $('input[id$=hidFacility]').val(),
                    sYear: $('input[id$=hidYear]').val()
                }, //Variable in function
                beforeSend: function () {
                    //BlockUI();
                },
                success: function (response) {
                    var data = JSON.parse(response);
                    if (data.Status == SysProcess.SessionExpired) {
                        PopupLogin();
                    } else if (data.Status == SysProcess.Success) {
                        $("table[id$=tbDetail] tbody tr").remove();
                        if (data.lstData != null && data.lstData.length > 0) {
                            var htmlTD = '<tr>';
                            htmlTD += '<td class="dt-body-center"></td>';
                            htmlTD += '<td class="dt-body-left"></td>';
                            htmlTD += '<td class="dt-body-center"></td>';
                            htmlTD += '<td class="dt-body-left"></td>';
                            htmlTD += '<td class="dt-body-left"></td>';
                            htmlTD += '</tr>';

                            $("table[id$=tbDetail] tbody").append(htmlTD);
                            var row = $("table[id$=tbDetail] tbody").find("tr").last().clone(true);
                            $("table[id$=tbDetail] tbody tr").remove();
                            var nStartDataIndex = 1;

                            var prms = Input("hdfPrmsMenu").val();
                            $.each(data.lstData, function (indx, item) {
                                $("td", row).eq(0).html(item.sDate);
                                $("td", row).eq(1).html(item.sStatusName);
                                $("td", row).eq(2).html(item.sMonth);
                                $("td", row).eq(3).html(item.sRemark);
                                $("td", row).eq(4).html(item.sActionBy);

                                $("table[id$=tbDetail] tbody").append(row);
                                row = $("table[id$=tbDetail] tbody").find("tr").last().clone(true);
                                nStartDataIndex++;
                            });

                            //SetICheck();
                            SetTootip();
                            SetHoverRowColor("tbDetail");
                            $("#popDetail").modal('toggle');
                        }
                        else {
                            SetRowNoData("tbDetail", 7);
                            $("#popDetail").modal('toggle');
                        }

                        HideLoadding();
                    } else {
                        DialogWarning(DialogHeader.Warning, data.Msg);
                    }
                },
                error: AjaxCall.error,
                complete: function () {
                    UnblockUI();
                }
            });
        }
    </script>
</asp:Content>

