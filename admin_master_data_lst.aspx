<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_master_data_lst.aspx.cs" Inherits="admin_master_data_lst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading"><i class="fa fa-table"></i>&nbsp<asp:Literal runat="server" ID="ltrHeader"></asp:Literal>Master Data</div>
        <div class="panel-body" id="divContent">
            <div class="form-horizontal">
                <table class="table dataTable table-bordered" cellspacing="0" border="1" id="gvwData" style="border-collapse: collapse;">
                    <tbody>
                        <tr>
                            <td class="dt-body-left" valign="top" style="width: 33%;"><a href="admin_master_waste_lst.aspx">Waste Disposal Method</a></td>
                            <td class="dt-body-left" valign="top" style="width: 33%;"><a href="admin_master_effluent_otherproduct_lst.aspx">Effluent Other Indicator</a></td>
                            <td class="dt-body-left" valign="top" style="width: 33%;"><a href="admin_master_effluent_otherunit_lst.aspx">Effluent Other Unit</a></td>
                        </tr>
                        <tr>
                            <td class="dt-body-left" valign="top" style="width: 33%;"><a href="admin_master_emission_otherproduct_lst.aspx">Emission Other Indicator</a></td>
                            <td class="dt-body-left" valign="top" style="width: 33%;"><a href="#"></a></td>
                            <td class="dt-body-left" valign="top" style="width: 33%;"><a href="#"></a></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
</asp:Content>

