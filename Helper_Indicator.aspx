<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="Helper_Indicator.aspx.cs" Inherits="Helper_Indicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style>
        td {
            vertical-align: top !important;
        }

        .cHead {
            font-weight: 700;
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:HiddenField ID="hdfDivMenuID" runat="server" />
    <div class="row">
        <div class="col-xs-12">
            <%--#region Material--%>
            <div id="dvMaterial">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#pnMaterial" data-toggle="collapse" style="cursor: pointer;">Material</div>
                        <div id="pnMaterial" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive">

                                        <table id="TBM1" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Material consumption</th>
                                                </tr>
                                            </thead>

                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Quantity of each material in mass – Tonne (wet or dry basis)</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Reflect the utilization of materials consumed by operation and trends in changing material consumption of both quantity and type.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>• Material: Any solid, liquid and gaseous material including materials imported or purchased from external sources (External party of PTT Group ) or from exploration & production business i.e. material purchased from PTTEP and those obtained from internal sources (captive production and extraction activities).
                                                                                                    <br />
                                                        • Direct material: Materials that are present in a final a product.<br />
                                                        • Non-renewable material: Resources that do not renew in short time period (i.e. mineral, metals, oil, gas, etc.).
                                                                                                    <br />
                                                        • Associated process materials: Materials that are needed for the manufacturing process but are not part of the final product.
                                                                                                    <br />
                                                        • Renewable materials: Materials that are composed of biomass from a living source and that can be continually replenished.  The sources include, but not limited to wood, grass, fiber, plant-based plastic and bio-based fuel. 
                                                                                                    (Reference: ISO 14021:2011 draft and the BIFMA sustainability standard)</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following specifications are included:
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	Total material used including imported or purchased materials from external sources (External party of PTT Group ) or from exploration & production business i.e. material purchased from PTTEP and domestic extractions (i.e. fossil fuels, coal, oil, etc.) those obtained from internal source (captive production and extraction activities).
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	Boundary covers consumption of a whole Reporting Unit of PTT Group.
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	Chemical used in hydraulic fracturing (i.e. acids, biocides, breakers, clay stabilizers, corrosion inhibitors, crosslinkers, friction reducers, gelling agents, iron controllers, scale inhibitors, surfactants, etc.).
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	For material that is purchased from the external sources , the name of facility that sells the material should be indicated.<br />
                                                        &nbsp;
                                                                                                    •	For “Associated Process Material”, Reporting Unit may adopt data reported in EIA Monitoring report.
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	Catalyst is included in the reporting scope upon replacement.
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	Define de minimus threshold not to exceed 5% of total material consumed by the facility.
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	For Exploration & Production; associated process materials that are used in their activities should be included in reporting.
                                                                                                    <br />
                                                        &nbsp; •	For Utility; water consumption is required to separately reported as follows:
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -	Report water used for steam production in material consumption indicator;
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -	Report water used for other activities in water withdrawal indicator.
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	The operation types indicated in Table 4.2 are required to report this indicator as these operation types require material input into the production process. Therefore these operations should monitor the efficiency to reduce material consumption. </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Reflect the utilization of materials consumed by operation and trends in changing material consumption of both quantity and type.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">&nbsp;</td>
                                                    <td>&nbsp;&nbsp;&nbsp;
                                                                                                    Table 4.2&nbsp; Preliminary Materials for Each Operation Type<br />
                                                        <table class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                                            <thead>
                                                                <tr>
                                                                    <td style="width: 14%; text-align: center;" rowspan="3">Material
                                                                    </td>
                                                                    <td style="width: 14%; text-align: center;" rowspan="3">Definition
                                                                    </td>
                                                                    <td style="width: 15%; text-align: center;" colspan="5">Operation type
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 15%; text-align: center;" colspan="5">(Unit: Tonne, in case of volume, Please input density)</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 15%; text-align: center;">GSP
                                                                    </td>
                                                                    <td style="width: 15%; text-align: center;">Refinery
                                                                    </td>
                                                                    <td style="width: 15%; text-align: center;">Petrochemical
                                                                    </td>
                                                                    <td style="width: 15%; text-align: center;">Lube
                                                                    </td>
                                                                    <td style="width: 15%; text-align: center;">Other
                                                                    </td>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr>
                                                                    <td style="width: 14%; text-align: left;">Direct Material (Raw material, Semi-manufactured goods or parts, Packaging)
                                                                    </td>

                                                                    <td style="width: 14%; text-align: left;">Tonne gas processed
                                                                    </td>
                                                                    <td style="width: 14%; text-align: left;">-	Refinery Throughput Additive
                                                                    </td>
                                                                    <td style="width: 14%; text-align: left;">Material that are present in final product;
                                                                    </td>
                                                                    <td style="width: 14%; text-align: left;">-	Additive
                                                                                                                <br />
                                                                        -	Packaging 
                                                                                                                <br />
                                                                        -	Petrochemical product 
                                                                    </td>
                                                                    <td style="width: 14%; text-align: left;">-	Base Oil
                                                                                                                <br />
                                                                        -	Additive
                                                                                                                <br />
                                                                        -	Packaging
                                                                    </td>

                                                                    <td style="width: 14%; text-align: left;">-	Ethanol (Oil)<br />
                                                                        -	Biodiesel (Oil)
                                                                                                                <br />
                                                                        -	Additive (e.g. Oil, LPG)
                                                                                                                <br />
                                                                        -	CO¬2 (GTM)
                                                                                                                <br />
                                                                        -	Mercaptan (GTM)
                                                                                                                <br />
                                                                        -	CO2 (NGV)
                                                                                                                <br />
                                                                        -	Packaging
                                                                    </td>
                                                                </tr>
                                                                <tr>

                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 14%; text-align: left;">Associated process materials
                                                                    </td>
                                                                    <td style="width: 14%; text-align: left;">Materials that are needed for the manufacturing process but are not part of the final product
                                                                    </td>
                                                                    <td style="width: 14%; text-align: left;" colspan="5">-	Lubricant
                                                                                                                <br />
                                                                        -	Chemical
                                                                                                                <br />
                                                                        -	Solvent
                                                                                                                <br />
                                                                        -	Liquid Nitrogen (for operation type: Utility)
                                                                                                                <br />
                                                                        <br />
                                                                        All operation type is to be used.
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 14%; text-align: left;">Catalyst
                                                                    </td>
                                                                    <td style="width: 14%; text-align: left;">Catalyst replacement
                                                                    </td>
                                                                    <td style="width: 14%; text-align: left;" colspan="5">All operation type is to be used.

                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead"></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead"></td>
                                                    <td>The following items are excluded from reporting:
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    •	All associated water and air consumption with an exception of water content in materials
                                                                                                    <br />
                                                        &nbsp; •	Any material that is used for reporting "Direct Energy Consumption" and “Water withdrawal” indicator
                                                                                                    <br />
                                                        &nbsp; •	Wastes from its own Reporting Unit that go through any process which can be reused as raw material (e.g. regenerated-catalyst)
                                                                                                    <br />
                                                        &nbsp; •	Input recycled materials (by–products) enter to process material consumption in office.
                                                                                                    <br />
                                                        &nbsp; •	Materials which are not relevant to production process i.e. oily rag.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method
                                                    </td>
                                                    <td>•	Identify the raw material used in the Reporting unit's boundary.<br />
                                                        •	Obtain the weight or volume of material consumed by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1)	Direct measurement e.g. counting, weighing, etc.; or
                                                                                                    <br />
                                                        &nbsp; (2)  	Estimation e.g. mass balance (referred to Basic Principles and Calculations in Chemical Engineering 2nd edition (1967)), using default factor.  
                                                                                                    <br />
                                                        Note: If method (2) is applied, Reporting Unit is required to state the methodology used to define material consumed and the reason for using that method.
                                                                                                    <br />
                                                        •	Calculate of 'as is' rather than by 'dry substance/ weight'.
                                                                                                    <br />
                                                        •	Potential data sources include billing and accounting system, and the procurement or supply management department. 
                                                                                                    <br />
                                                        •	Calculating total materials by using this formula:<br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    Total materials = Directed Material + Associated Material 
                                                             = Renewable Material Used + Non-Renewable Material used<br />
                                                        <br />
                                                        <div style="width: 100%;" class="text-center">
                                                            <img src="Images/helpindicator/image010.png" class="img-rounded" />
                                                        </div>


                                                        <br />
                                                        •	Report the total volume or weight of material consumption, broken down by non-renewable direct materials used, non-renewable associated materials used, renewable direct materials used and renewable associated materials used.
                                                                                                    <br />
                                                        •	In case that quantity is reported in volume, density of material should be indicated.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	Quantity of material consumed shall be reported on a monthly basis.</td>
                                                </tr>

                                            </tbody>

                                        </table>
                                        <br />
                                        <table id="TBM2" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Percentage of recycled input material consumed</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Recycled input material consumed - Percentage (%)</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Indicate ability to reduce the demand of virgin material.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>• Recycled input material: Materials that have been recycled by recycled process and used to replace virgin materials (listed in Table 4.2).  Therefore, recycled by-products or non-product outputs or waste that are not listed in Table 4.2, are not considered as recycled input material consumed.	</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following items are included in reporting:
                                                                                                    <br />
                                                        •	All recycled by-product, non-product outputs or wastes can be considered as recycled input material if recycled non-product are listed in Table 4.2 . 
                                                                                                    <br />
                                                        •	Percentage of recycled input material consumed.<br />
                                                        <div style="width: 100%;" class="text-center">
                                                            <img src="images/helpindicator/image011.png" class="img-rounded" />
                                                        </div>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify the total weight or volume of recycled input materials by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1) 	Direct measurement e.g. counting, weighing, etc.; or 
                                                                                                    <br />
                                                        &nbsp; (2) 	Estimation e.g. mass balance, using default factor. 
                                    Note: If estimation method is applied, the Reporting Unit is required to state the methodology used to define recycled input material and the reason for using that method. 
                                                                                                    <br />
                                                        •	Data sources include billing and accounting system, the procurement or supply chain management, and internal production and waste disposal records.
                                                                                                    <br />
                                                        •	Calculate the percentage by using the formula:
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        % of Recycle input material =       <span class="auto-style4">Total recycled input material x 100</span><br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total input materials used*
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    (Reference: Sustainability Reporting Guidelines & Oil and Gas Sector Supplement)<br />
                                                        Remark:<br />
                                                        * - Total input materials used = Direct Materials + Associated process materials + Total Recycled input material<br />
                                                        •	In case that quantity is reported in volume, density of material should be indicated.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	Percentage of recycled input materials shall be reported on a monthly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>

                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>
            <%--#region Water --%>
            <div id="dvWater">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#pnWater" data-toggle="collapse" style="cursor: pointer;">Water withdrawal</div>
                        <div id="pnWater" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive">
                                        <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;" id="divWaterWithdrawal">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Water withdrawal</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Water withdrawal by source - Cubic meter (m<sup>3</sup>))</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Indicate the level of risk posed by disruption to water supply or increases in the cost of water.
                                                        <br />
                                                        •	Provide the baseline figure for other calculations relating to the efficient use of water.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>• <strong>Water withdrawal:</strong> The total volume of water withdrawn either directly by the Reporting Unit or through intermediaries into the boundary of the Reporting Unit from all sources.
                                                        <br />
                                                        • <strong>Water supply:</strong> The provision of water by public utilities, usually via a system of pumps and pipes such as tap water.
                                                        <br />
                                                        • <strong>Surface water:</strong> Water collecting in a stream, river or lake. 
                                                        <br />
                                                        • <strong>Seawater:</strong> Water from sea or ocean.
                                                        <br />
                                                        • <strong>Groundwater:</strong> Water located beneath the earth’s surface in soil pore space and the fractures of rock formations.
                                                        <br />
                                                        •<strong> Imported wastewater:</strong>  Wastewater imported from external source for reuse or recycling before use within the reporting unit.
                                                        <br />
                                                        • <strong>Rainwater:</strong> Water that has fallen as rain.
                                                        <br />
                                                        • <strong>Fresh water:</strong> The total volume of groundwater, surface water, water supply and rainwater.	</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following items are included:<br />
                                                        &nbsp;•	The scope of reporting includes surface water (including seawater), groundwater and municipal supply/water supply/tap water/de-mineralized water for any use during the reporting period.
                                                                                                   <br />
                                                        &nbsp; • The use of seawater in once through cooling process and heat exchanger process is included in water withdrawal reporting.  
                                                                                                    <br />
                                                        &nbsp; • Wastewater from other organization withdrawn to the Reporting Unit for further use is included for reporting.<br />
                                                        <br />
                                                        <div style="width: 100%;" class="text-center">
                                                            <img src="Images/helpindicator/image015.png" class="img-rounded" />
                                                        </div>
                                                        <br />
                                                        &nbsp;
                                                                                                    •	Identify water withdrawal from water bodies that is particularly sensitive due to their status as a rare, threatened, or endangered system.
                                                                                                    <br />
                                                        &nbsp; •	Water withdrawal used in office buildings, dormitories or employee housing estates are included in case that those buildings are under the operational control of the reporting unit.
                                                                                                    <br />
                                                        &nbsp; •	Rainwater is included in case that the reporting unit purposely collects rainwater for consumption as shown below
                                                                                                    <br />
                                                        <br />
                                                        <div style="width: 100%;" class="text-center">
                                                            <img src="Images/helpindicator/image016.png" class="img-rounded" />
                                                        </div>
                                                        <br />
                                                        &nbsp;
                                                                                                    •	Fresh water withdrawal is included in the scope of reporting as fresh water resources are constrained due to limited supplies or extensive use.<br />
                                                        The following items are excluded from reporting:<br />
                                                        &nbsp;
                                                                                                    •	Exemption is made for imported wastewater to be treated onsite.  Steam condensate obtained from other organization is excluded for reporting.
                                                                                                    <br />
                                                        &nbsp; •	Rainwater collected without intention for being consumed within the reporting unit.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify all sources of water withdrawn into the Reporting Unit.
                                                                                                    <br />
                                                        •	Identify total volume of water withdrawn from each source by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred): 
                                                                                                    <br />
                                                        &nbsp; (1) 	Direct measurement e.g. reading from calibrated flow meter;
                                                                                                    <br />
                                                        &nbsp; (2)	Information from invoices from water supplier;
                                                                                                    <br />
                                                        &nbsp; (3) 	Calculation based on pumping capacity multiplied by the time of pumping;<br />
                                                        &nbsp; 
                                    (4) 	Measurement of the height that water is withdrawn and calculation based on pond dimension as the following formula; or<br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    Volume of water withdrawal (m<sup>3</sup>) = Height that water is withdrawn (m) x Width of pond (m) 
                                                                                                 x Length of pond (m)<br />
                                                        <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    Calculate the volume of rainwater withdrawal on monthly basis by using the formula:
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    Volume of water withdrawal (m3) = [Average annual height of rainwater in 
                                                                                                        that area (mm)/1000/12] x Width of pond (m) 
                                                                                                         x Length of pond (m)
                                                                                                    <br />
                                                        <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    The height of rainwater in that area shall refer from the rainwater level indicated in Thai Meteorological Department (reference: <a href="http://www.tmd.go.th/climate/climate.php">http://www.tmd.go.th/climate/climate.php</a>).<br />
                                                        &nbsp;
                                    (5) 	Estimation from process requirements.<br />
                                                        Note: If method (2) – (5) is applied, the Reporting Unit is required to state the methodology used to define water withdrawal and the reason for using that method.
                                                                                                    <br />
                                                        •	Report total volume of water withdrawn by sources.
                                                                                                    <br />
                                                        •	Separately report total volume of seawater used in once through cooling process and heat exchanger process from total volume of seawater used for other purposes.
                                                                                                    <br />
                                                        •	Report normalized water withdrawal for each type of water intensity operation, by million barrel oil equivalent (MBOE) produced.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	Total volume of water withdrawal shall be reported on a monthly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <br />
                                        <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;" id="divWaterReuse">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Water recycled and reused</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Water recycled - Cubic meter (m<sup>3</sup>)
                                                                                                    <br />
                                                        •	Water reused - Cubic meter (m<sup>3</sup>)
                                                                                                    <br />
                                                        •	Water recycled and reused - Percentage (%)</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Demonstrate the efficiency and success of the Reporting Unit in reducing total water withdrawals and discharge.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>• <strong>Water recycle:</strong> Water recovered from used water/wastewater that has been treated to suitable standards for beneficial purposes 
                                                                                                    <br />
                                                        • <strong>Water reuse:</strong> The utilization of appropriately wastewater for some further beneficial purpose
                                    (Reference: Virginia Cooperative Extension, Virginia Polytechnic Institute and State University; and United States Environment Protection Agency)</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead"></td>
                                                    <td>In general, there are three types of water recycling/re-use:<br />
                                                        &nbsp;
                                                                                                    o	Wastewater recycled back in the same process or higher use of recycled water in the process cycle;
                                                                                                    <br />
                                                        &nbsp; o	Wastewater recycled/re-used in a different process, but within the same facility; and
                                                                                                    <br />
                                                        &nbsp; o	Wastewater re-used at another of the reporting unit’s facilities.<br />
                                                        <br />
                                                        <table style="min-width: 100%; background-color: #fff !important;">
                                                            <thead>
                                                                <tr style="background-color: #fff !important;">
                                                                    <td class="cHead">Water Recycled</td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr style="background-color: #fff !important;">
                                                                    <td style="width: 20%;">Case 1:</td>
                                                                    <td>
                                                                        <div style="width: 100%; text-align: center">
                                                                            <img src="Images/helpindicator/image019_2.png" class="img-rounded" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr style="background-color: #fff !important;">
                                                                    <td style="width: 20%;">Case 2:</td>
                                                                    <td>
                                                                        <div style="width: 100%; text-align: center">
                                                                            <img src="Images/helpindicator/image020_2.png" class="img-rounded" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr style="background-color: #fff !important;">
                                                                    <td style="width: 20%;">Case 3:</td>
                                                                    <td>
                                                                        <div style="width: 100%; text-align: center">
                                                                            <img src="Images/helpindicator/image021_2.png" class="img-rounded" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <table style="min-width: 100%; background-color: #fff !important;">
                                                            <thead>
                                                                <tr style="background-color: #fff !important;">
                                                                    <td class="cHead">Water Reused</td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr style="background-color: #fff !important;">
                                                                    <td style="width: 20%;">Case 1:</td>
                                                                    <td>
                                                                        <div style="width: 100%; text-align: center">
                                                                            <img src="Images/helpindicator/image023_2.png" class="img-rounded" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr style="background-color: #fff !important;">
                                                                    <td style="width: 20%;">Case 2:</td>
                                                                    <td>
                                                                        <div style="width: 100%; text-align: center">
                                                                            <img src="Images/helpindicator/image022_2.png" class="img-rounded" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr style="background-color: #fff !important;">
                                                                    <td style="width: 20%;">Case 3:</td>
                                                                    <td>
                                                                        <div style="width: 100%; text-align: center">
                                                                            <img src="Images/helpindicator/image024_2.png" class="img-rounded" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following items are included:
                                                                                                    <br />
                                                        &nbsp; •	The scope of reporting includes total volume of water recycled/reuse supply for any use during the reporting period.<br />
                                                        The following items are excluded from reporting:
                                                                                                    <br />
                                                        &nbsp; •	Imported wastewater to be treated onsite, steam condensate returned into the process, and backwash water returned into the process are excluded from reporting.

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify the process or activity on recycling/reuse water within the Reporting Unit. 
                                                                                                    <br />
                                                        •	Identify the volume of recycled/reused water based on the volume of water demand satisfied by recycled/reused water rather than further withdrawals by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1) 	Direct measurement e.g. reading of water meter;
                                                                                                    <br />
                                                        &nbsp; (2) 	Calculation based on pumping capacity multiplied by the time of pumping; or
                                                                                                    <br />
                                                        &nbsp; (3) 	Estimation from process requirements or water balance.
                                                                                                    <br />
                                                        Note: If method (2) – (3) is applied, please state the methodology used to define water recycled/reused and the reason for using that method.
                                    •	Calculate the percentage of water recycled/reused based on the total amount of water demanded by using the formula:
                                                                                                    <br />
                                                        <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    %Water recycled/reused =                        <span class="auto-style4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total water recycled/reused x 100&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                                                        <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (Total water withdrawal +Total water recycled +Total water reused)
                                                                                                    <br />
                                                        <br />
                                                        &nbsp;&nbsp;&nbsp; (Reference: Sustainability Reporting Guidelines & Oil and Gas Sector Supplement)<br />
                                                        &nbsp;
                                                                                                    •	Report water recycled/reused in term of both total volume and percentage.  In case the water is reused in the same process for multi cycles, total amount of water reused (water reused at each cycle multiplied by number of cycle) shall be reported.<br />
                                                        &nbsp;&nbsp;&nbsp;
                                    Example: If the reporting unit has a production cycle that requires 20 m3 of water, the organization withdraws 20 m3 of water for one production process and reuses it for an additional 3 cycles. The total quantity of water reused for that process is 60 m3.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	Total volume of water withdrawal shall be reported on a monthly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>
            <%--#region Waste --%>
            <div id="dvWaste">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#pnWaste" data-toggle="collapse" style="cursor: pointer;">Waste</div>
                        <div id="pnWaste" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive">
                                        <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Total hazardous waste disposed</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Total hazardous waste disposed - Tonne</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Indicate the level of progress the Reporting Unit has made toward waste reduction efforts. 
                                                                                                    <br />
                                                        •	Indicate potential improvement in process efficiency and productivity.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>The definition of hazardous waste is according to national legislation/regulations.<br />
                                                        • The definition of hazardous waste is according to national legislation/regulations.<br />
                                                        • <strong>Disposal Method:</strong> The method by which waste is treated or disposed including composting, reuse, recycling, recovery (e.g. used as alternative fuel for energy recovery), incineration (i.e. combustion without further use), landfill, and deep-well injection.
                                                                                                    <br />
                                                        • <strong>Routine waste:</strong> Waste produced from any type of normal operations or recurring work that is considered ongoing in nature including waste generated from planned maintenance (i.e. T/A, operation shutdown, etc.).
                                                                                                    <br />
                                                        • <strong>Non-routine waste:</strong>  Waste generated from specialist activities, emergency case, large construction and spill clean-up.
                                                                                                    <br />
                                                        • <strong>Large construction: </strong>Construction of new facilities that requires Environmental Impact Assessment (EIA) and construction of all new power plants and all new terminals.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following specifications are included in reporting:<br />
                                                        &nbsp;
                                                                                                    •	The scope of reporting covers hazardous waste managed as per the methods approved by related government authorities including the followings:
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp; -	All hazardous waste removed from the Reporting Unit for disposal and/or treatment;
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp; -	All hazardous waste disposed of on site, e.g. by landfill and deep well disposal;
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp; -	All hazardous residues resulting from treatment on own site/another site, and are classified as waste;
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp; -	Hazardous waste that are re-used, recycled or sold as raw material by other organizations; 
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp; -	Hazardous waste from office;
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp; -	Hazardous waste generated from non-routine operation is included (i.e. construction, spill cleanup, remediation activities, maintenance, emergency shutdown etc.); and 
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp; -	Hazardous waste from dormitories or employee housing estates is included in case that those buildings are under the operational control of the reporting unit.
                                                                                                    <br />
                                                        &nbsp; •	Identify waste disposal that account for an average of more than 5% of the annual average mass of reporting unit’s total hazardous waste<br />
                                                        &nbsp;
                                                                                                    •	<strong>For Gas Pipeline</strong>; waste generated by contractors during the project phase is included.
                                                                                                    <br />
                                                        &nbsp; •	<strong>For Exploration & Production</strong>; waste generated from construction phase is defined as waste generated from non-routine waste.
                                    •	The mapping of disposal method identified in guideline with the disposal method defined in the Notification of Ministry of Industry, Re: Industrial Waste Disposal, B.E.2548 (2005) is illustrated in the following Table 4.3  For E&P, disposal method is required to refer the Notification of the Department of Mineral Resources, B.E. 2556 (2013), Re: Prescribing Waste Management Measure for Petroleum Facility.<br />
                                                        <br />
                                                        &nbsp;&nbsp;
                                                                                                    Table 4.3  Disposal Method defined in Thai Regulations
                                                                                                    <br />
                                                        <table class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                                            <thead>
                                                                <tr>
                                                                    <th class="cHead" colspan="2" style="width: 35%;">Disposal Method</th>
                                                                    <th class="cHead" colspan="2">Treatment and Disposal Codes</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr>
                                                                    <td rowspan="3" colspan="2">Reuse
                                                                    </td>
                                                                    <td>033 - Return to original producer for reuse or refill
                                                                    </td>
                                                                </tr>
                                                                <tr>

                                                                    <td>039 - Other reuse methods</td>
                                                                </tr>
                                                                <tr>

                                                                    <td>031 - Use as raw material substitution</td>
                                                                </tr>
                                                                <tr>
                                                                    <td rowspan="2" colspan="2">Recycling;</td>
                                                                    <td>044 - Use as co-material in cement kiln on rotary kiln</td>
                                                                </tr>
                                                                <tr>

                                                                    <td>049 - other recycle methods</td>
                                                                </tr>
                                                                <tr>

                                                                    <td rowspan="8" colspan="2">Recovery, including energy recovery</td>
                                                                    <td>041 - Use as fuel substitution or burn for energy recovery</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>042 - fuel blending</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>043 - Burn for energy recovery</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>051 - Solvent reclamation/regeneration</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>052 - Reclamation/regeneration of metal and metal compounds</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>053 - Acid/base regeneration</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>054 - Catalyst regeneration</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>059 - Other recovery unlisted materials</td>
                                                                </tr>
                                                                <tr>
                                                                    <td rowspan="3" colspan="2">Landfill
                                                                    </td>
                                                                    <td>071 - Sanitary landfill (for non-hazardous waste only)
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>072 - Secure landfill</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>073 - Secure landfill of stabilized and/or solidified waste</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="10%" rowspan="9">Other Disposal
                                                                    </td>
                                                                    <td width="25%">Compositing
                                                                    </td>
                                                                    <td>083 - Compositing or soil conditioner (for non-hazardous waste only)
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="25%" rowspan="3">Incineration (mass burn</td>
                                                                    <td>074 - Burn for destruction in solid waste incinerator (for non-hazardous waste only)</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>075 - Burn for destruction in hazardous waste incinerator</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="auto-style13">076 - Co-incineration in cement kiln</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Deep well injection</td>
                                                                    <td>077 - Deepwell or underground injection; sea-bed insertion
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="25%">Land reclamation</td>
                                                                    <td>082 - Land reclamation (for non-hazardous waste only)</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="25%">On-site storage</td>
                                                                    <td>021 - Storage in packing or containers</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="25%">Other disposal methods</td>
                                                                    <td>079 - Other disposal methods</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="25%">&nbsp;</td>
                                                                    <td>084 - Animal feed (for non-hazardous waste only)</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" rowspan="2">For these treatment and disposal codes, 
                                                                    </td>
                                                                    <td>011 - Sorting for resale 
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>032 - Return to original producer for disposal</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" rowspan="10">the final disposal method (i.e. landfill, incineration) should be identified and grouped in the above disposal methods for reporting.
                                                                    </td>
                                                                    <td>061 - Biological treatment
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>062 - Chemical treatment</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>063 - Physical treatment</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>064 - Physico-chemical treatment</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>065 - Physico-chemical treatment of wastewater</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>066 – Direct discharge to central wastewater treatment plant</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>067 - Chemical stabilization</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>068 - Chemical fixation using cementitious and/or pozzolanic material</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>069 - Other detoxification methods</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>081 - Collect and export </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify hazardous waste generated by the Reporting Unit based on hazardous waste lists prescribed by national regulations.
                                                                                                    <br />
                                                        •	Identify total hazardous waste disposed from routine and non-routine operations by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1)	Measure the total weight of hazardous waste directly by using calibrated weight scale, weighbridges or truck scale;
                                                                                                    <br />
                                                        &nbsp; (2)	Information from manifests from license waste disposal contractor; or
                                                                                                    <br />
                                                        &nbsp; (3)	In the absence of calibrated weighbridge, estimation can be used for assessing the total volume. The estimation methods used are stated.  In absence of domestic waste density, average domestic waste density with reference to data available from government authority can be adopted (e.g. 0.37 kg/l – refer to BMA environmental statistics 2009).<br />
                                                        •	Report total weight of hazardous waste by categories.  Separate reporting waste generated from routine operations and non-routine operations.   
                                                                                                    <br />
                                                        •	Report total weight of onsite storage waste at the end of previous year and the end of reporting year (i.e. on December, 31<sup>st</sup>).
                                                                                                    <br />
                                                        •	Report total weight of waste generated by using the following formula: 
                                                                                                    <br />
                                                        <br />
                                                        Weight of waste = | Weight of onsite storage waste at the end of previous year -  generated	Weight of onsite storage waste at the end of reporting year | +  Weight of waste disposed in the reporting year<br />
                                                        <br />
                                                        Remark:
                                                                                                    <br />
                                                        &nbsp;
                                                                                                    -	Weight of onsite storage waste at the end of reporting year: Include all wastes generated in the previous year and the reporting year which are stored on the site at the end of the reporting year.
                                                                                                    <br />
                                                        &nbsp; -	Weight of waste disposed in the reporting year: Include all wastes generated in the previous year and the reporting year which are disposed in the reporting year. 

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	Total hazardous waste disposed shall be reported on a monthly basis.
                                    •	Total onsite storage waste shall be reported at the end of year (i.e. on December, 31<sup>st</sup>).
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <br />
                                        <table id="TBW2" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Total hazardous waste disposed at landfill</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Total hazardous waste landfill - Tonne</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Demonstrate effort of the Reporting Unit in reducing waste disposed at landfill.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>• <strong>Total hazardous waste disposed at landfill:</strong> Total weight of hazardous waste disposed by the method of landfilling in which refuse is buried between layers of dirt so as to fill in or reclaim low-lying ground.	</td>
                                                </tr>

                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>•	See scope of total hazardous waste disposed.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify total hazardous waste disposed at landfill by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1)	Measure the total weight of hazardous waste directly by using calibrated weight scale, weighbridges or truck scale; or
                                                                                                    <br />
                                                        &nbsp; (2)	Data can also be obtained from waste manifest document from providers of disposal service, waste balance sheet, internal billing and accounting system and supply chain management.
                                    If weight data is estimated using waste density, volume collected, or mass balance, the methods used are stated.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	The data shall be reported on a monthly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <br />
                                        <table id="TBW3" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Total non-hazardous waste disposed</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Total non-hazardous waste disposed – Tonne</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Indicate the level of progress the Reporting Unit has made toward waste reduction efforts.
                                                                                                    <br />
                                                        •	Indicate potential improvement in process efficiency and productivity.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>• <strong>Total non-hazardous waste disposal:</strong> As prescribed in national regulation, this includes all other forms of solid or liquid waste. Exemption is made for wastewater.</td>
                                                </tr>

                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following specifications are included in reporting:
                                                                                                    <br />
                                                        &nbsp; •	See scope of total hazardous waste disposed.
                                                                                                    <br />
                                                        &nbsp; •	All business units shall include non-hazardous waste generated from general activities in the office as well as domestic waste. 
                                                                                                    <br />
                                                        &nbsp; •	Identify waste disposal that account for an average of more than 5% of the annual average mass of reporting unit’s total non-hazardous waste.
                                                                                                    <br />
                                                        Based on the workshop conducted amongst PTT group-wide stakeholders the following specification is excluded from reporting:
                                                                                                    <br />
                                                        &nbsp; •	Exemption is made for retail store (e.g. 7-11, amazon, jiffy market) due to technical difficulties on measuring waste from customers.  In case that waste generated from retail store cannot practically be separated from waste generated from PTT Retail, it can be included in the waste reporting.

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify non-hazardous waste generated by the Reporting Unit based on definition prescribed by national regulations.
                                                                                                    <br />
                                                        •	Identify total non-hazardous waste disposed from routine and non-routine operations by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1)	Measure the total weight of non-hazardous waste directly by using calibrated weight scale, weighbridges or truck scale;
                                                                                                    <br />
                                                        &nbsp; (2)	Information from manifests from license waste disposal contractor; or
                                                                                                    <br />
                                                        &nbsp; (3)	In the absence of calibrated weighbridge, estimation can be used for assessing the total volume. The estimation methods used are stated.  In absence of domestic waste density, average domestic waste density with reference to data available from government authority can be adopted (e.g. 0.37 kg/l – refer to BMA environmental statistics 2009).<br />
                                                        •	Report total weight of non-hazardous waste by categories. Separate reporting waste generated from routine operations and non-routine operations.
                                                                                                    <br />
                                                        •	Report accumulation of on-site storage waste on the last month of the year.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	The data shall be reported on a monthly basis.
                                    •	Total onsite storage waste shall be reported at the end of year (i.e. on December, 31<sup>st</sup>).
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <br />
                                        <table id="TBW4" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Total non-hazardous waste reused, recycled or recovered</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Total non-hazardous waste reused – Tonne
                                                                                                    <br />
                                                        •	Total non-hazardous waste recycled - Tonne
                                                                                                    <br />
                                                        •	Total non-hazardous waste recovered - Tonne</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Demonstrate effort of the Reporting Unit in reducing waste disposed at landfill.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>See definitions in total hazardous waste reused, recycled or recovered.</td>
                                                </tr>

                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following specification is included in reporting:<br />
                                                        &nbsp;
                                                                                                    •	Boundary covers non-hazardous wastes from process and office activities.
                                                                                                    <br />
                                                        The following specification is excluded from reporting:
                                                                                                    <br />
                                                        &nbsp; •	Usual reprocessed hazardous waste in daily production is exempted from reporting as it is not considered as waste.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	 Identify non-hazardous waste. 
                                                                                                    <br />
                                                        •	Identify total non-hazardous waste reused, recycled or recovered by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1)	Measure the total weight of non-hazardous waste directly by using calibrated weight scale, weighbridges or truck scale by disposal type (reused/recycled/recovered); or
                                                                                                    <br />
                                                        &nbsp; (2)	Data can also be obtained from waste manifest document from providers of disposal service, waste balance sheet, internal billing and accounting system and supply chain management.
                                                                                                    <br />
                                                        If weight data is estimated using waste density, volume collected, or mass balance, the methods used are stated.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	The data shall be reported on a monthly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <br />
                                        <table id="TBW5" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Total non-hazardous waste disposed at landfill</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Total non-hazardous waste landfill - Tonne</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Demonstrate effort of the Reporting Unit in reducing waste disposed at landfill.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>See definitions in total non-hazardous waste disposed at landfill.</td>
                                                </tr>

                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>•	See scope of total hazardous waste disposed.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify total non-hazardous waste disposed at landfill by (methods are prioritized by level of accuracy of data obtained, hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1)	Measure the total weight of non-hazardous waste directly by using calibrated weight scale, weighbridges or truck scale.
                                                                                                    <br />
                                                        &nbsp; (2)	Data can also be obtained from waste manifest document from providers of disposal service, waste balance sheet, internal billing and accounting system and supply chain management.
                                    If weight data is estimated using waste density, volume collected, or mass balance, the methods used are stated.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	The data shall be reported on a monthly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>
            <%--#region Drilling Waste --%>
            <div id="dvDrillingWaste">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#pnDrillingWaste" data-toggle="collapse" style="cursor: pointer;">Drilling Waste</div>
                        <div id="pnDrillingWaste" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive">
                                        <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Drilling Waste</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Drilling waste - Tonne</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Indicate the handling and manner of drilling waste as it has the potential impact to the surrounding environment and ecology.
                                                                                                    <br />
                                                        •	Demonstrate effort of the Reporting Unit in reducing disposal of drilling waste.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>• <strong>Drilling waste:</strong> Drilling mud and rock cuttings that are occurred during operation to drill boreholes.	</td>
                                                </tr>

                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following specifications are included in reporting:
                                                                                                    <br />
                                                        &nbsp; •	All drilling mud and cutting produced using non-aqueous drilling fluid and aqueous drilling fluid;
                                                                                                    <br />
                                                        &nbsp; •	Reporting of this indicator is applicable only for Exploration & Production as there is drilling activities in the operation.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify drilling mud and cutting produced using non-aqueous drilling fluid and aqueous drilling fluid 
                                                                                                    <br />
                                                        •	Identify total amount of drilling mud and cutting re-injected, recycled, disposed onshore, disposed offshore by (methods are prioritized by level of accuracy of data obtained, 
                                                                                                    <br />
                                                        hence first method is preferred):
                                                                                                    <br />
                                                        &nbsp; (1)	Measure the total amount of drilling mud and cutting directly by using calibrated weight scale, weighbridges or truck scale.
                                                                                                    <br />
                                                        &nbsp; (2)	Data can also be obtained from waste manifest document from providers of disposal service, waste balance sheet, internal billing and accounting system and supply chain management.
                                                                                                    <br />
                                                        •	Report total amount of drilling waste produced using non-aqueous drilling fluid and aqueous drilling fluid by disposal method (i.e. reinjection, recycling, onshore disposal to controlled site and offshore disposal). 
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	The data shall be reported on a monthly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>

            <%--#region Intensity denominator --%>
            <div id="dvIntensity">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#pnIntensity" data-toggle="collapse" style="cursor: pointer;">Intensity denominator</div>
                        <div id="pnIntensity" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive">
                                        <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2">Intensity denominator</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td class="cHead">Unit</td>
                                                    <td>•	Intensity Denominator – million barrel oil equivalent (MBOE), Tonne, MMSCF, MMBTU, Litres, Area, Employee</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Purpose</td>
                                                    <td>•	Normalize quantities to allow comparisons of indicator data between same type of operations of different size.</td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Definition</td>
                                                    <td>Normalized quantities are relative values presented as ratios between two absolute quantities of the same or different kind. Typically, indicator data are the numerators of the ratio, and a suitable normalization factor is selected as the denominator.
                                                                                                    <br />
                                                        • Key Product: The main outputs from production process.
                                                                                                    <br />
                                                        • By-product: Secondary product derived from a manufacturing process or chemical reaction.
                                    <br />
                                                        • Million Barrel Oil Equivalent (MBOE): Unit of energy based on the approximate energy released by burning one barrel of crude oil.</td>
                                                </tr>

                                                <tr>
                                                    <td class="cHead">Scope</td>
                                                    <td>The following specifications are included in reporting:
                                                        <br />
                                                        •	Key products generated from production process;
                                                                                                    <br />
                                                        •	Products and by-products reported in stock market report (Form 56-1);<br />
                                                        •	All valuable products that can be sold to other facilities
                                                                                                    <br />
                                                        •	For those reporting units that are not listed in stock market, identification of product to be included in reporting can be performed by considering the alignment with business decision-making and contribution to the environmental impact.  The assumption used for such identification shall also be indicated; and
                                                                                                    <br />
                                                        •	Recommended list of denominator and unit by each operation types are illustrated in Table 4.1 (refer to PTT GHG Accounting & Reporting Standard).
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead"></td>
                                                    <td>&nbsp;&nbsp;&nbsp;
                                                                                                     Table 4.1&nbsp; Denominators for Each Operation Type
                                                                                                     <table class="table dataTable table-bordered table-hover" style="min-width: 100%;">
                                                                                                         <tr>
                                                                                                             <td align="center" valign="top" width="30%" class="auto-style1">Operation Type
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">Recommended denominator unit
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">Source
                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Gas Processing
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">Tonne product
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">•	Butane
                                                                                                                 <br />
                                                                                                                 •	Ethane
                                                                                                                 <br />
                                                                                                                 •	LPG
                                                                                                                 <br />
                                                                                                                 •	NGL
                                                                                                                 <br />
                                                                                                                 •	Propane

                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Gas Terminal
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">•	Tonne throughput (for LPG)
                                                                                                                 <br />
                                                                                                                 •	mmscf Output (for LNG)

                                                                                                             </td>
                                                                                                             <td style="width: 35%;">•	LPG
                                                                                                                 <br />
                                                                                                                 •	LNG
                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Liquid Transportation & Storage
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">M<sup>3</sup> of Liquid transported/ distributed
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">•	Gasoline
                                                                                                                 <br />
                                                                                                                 •	Diesel
                                                                                                                 <br />
                                                                                                                 •	Aviation fuel
                                                                                                                 <br />
                                                                                                                 •	Fuel oil
                                                                                                                 <br />
                                                                                                                 •	Kerosene
                                                                                                                 <br />
                                                                                                                 •	Gasohol
                                                                                                                 <br />
                                                                                                                 •	Ethanol
                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Gas Transmission
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">mmscf or mmbtu of gas transmitted/ distributed
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">•	Natural gas (dry)
                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Refinery
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">Refining Tonnes Throughput
                                                                                                             </td>
                                                                                                             <td style="width: 35%;">•	Light product i.e. LPG, Gasoline, Mixed Xylene, Light naphtha, Reformate, Isomerate, Propylene
                                                                                                                 <br />
                                                                                                                 •	Middle distillate i.e. Aviation fuel, Diesel, Kerosene
                                                                                                                 <br />
                                                                                                                 •	Heavy Product i.e. Fuel oil, Long residue, Bitumen

                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Petrochemical

                                                                                                             </td>
                                                                                                             <td style="width: 35%;">Tonnes Product

                                                                                                             </td>
                                                                                                             <td style="width: 35%;">•	Feedstock By-Product i.e. Naphtha, Pygas, Condensate Residue
                                                                                                                 <br />
                                                                                                                 •	Olefin i.e. Ethylene, Propylene
                                                                                                                 <br />
                                                                                                                 •	Aromatics i.e. Benzene, Toluene, Mixylene, Orthoxylene, Paraxylene, Cyclo hexane
                                                                                                                 <br />
                                                                                                                 •	Chemicals i.e. MTBE, Acetone, Ethanol, Ammonia
                                                                                                                 <br />
                                                                                                                 •	Polymer
    
                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Chemical Transportation & Storage</td>
                                                                                                             <td style="width: 35%;">Tonnes Throughput</td>
                                                                                                             <td style="width: 35%;">-</td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Retail (Oil and Gas)</td>
                                                                                                             <td style="width: 35%;" rowspan="2">•	M3 of fuel sold (liquid)
                                                                                                                 <br />
                                                                                                                 •	Tonnes of fuel sold (gas)</td>
                                                                                                             <td style="width: 35%;" rowspan="2">•	Gasoline
                                                                                                                 <br />
                                                                                                                 •	Diesel
                                                                                                                 <br />
                                                                                                                 •	Aviation fuel
                                                                                                                 <br />
                                                                                                                 •	Fuel oil
                                                                                                                 <br />
                                                                                                                 •	Kerosene
                                                                                                                 <br />
                                                                                                                 •	LPG
                                                                                                                 <br />
                                                                                                                 •	Gasohol
                                                                                                                 <br />
                                                                                                                 •	Ethanol
                                                                                                                 <br />
                                                                                                                 •	NGV or CNG
                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Natural Gas Mother Station</td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Oil Terminal and Aviation</td>
                                                                                                             <td style="width: 30%;">M<sup>3</sup> of fuel sold</td>
                                                                                                             <td style="width: 30%;">•	Gasoline
                                                                                                                 <br />
                                                                                                                 •	Diesel
                                                                                                                 <br />
                                                                                                                 •	Aviation fuel
                                                                                                                 <br />
                                                                                                                 •	Fuel oil
                                                                                                                 <br />
                                                                                                                 •	Kerosene
                                                                                                                 <br />
                                                                                                                 •	Gasohol
                                                                                                                 <br />
                                                                                                                 •	Ethanol
                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Utility

                                                                                                             </td>
                                                                                                             <td style="width: 30%;">MWh (energy generated)</td>
                                                                                                             <td style="width: 30%;">-</td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">E&P</td>
                                                                                                             <td style="width: 30%;">MBOE of production</td>
                                                                                                             <td style="width: 30%;">•	Condensate
                                                                                                                 <br />
                                                                                                                 •	Crude Oil
                                                                                                                 <br />
                                                                                                                 •	Natural Gas (dry)
                                                                                                                 <br />
                                                                                                                 •	Natural Gas (wet)
                                                                                                             </td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">Lubricant</td>
                                                                                                             <td style="width: 30%;">Litres of Lubricant Produced</td>
                                                                                                             <td style="width: 30%;">-</td>
                                                                                                         </tr>
                                                                                                         <tr>
                                                                                                             <td style="width: 30%;">PTT Group Building</td>
                                                                                                             <td style="width: 30%;">•	Area
                                                                                                                 <br />
                                                                                                                 •	Employees (for reporting waste and water withdrawal)
                                                                                                             </td>
                                                                                                             <td style="width: 30%;">-</td>
                                                                                                         </tr>
                                                                                                     </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead"></td>
                                                    <td>The following items are excluded from reporting:
                                                                                                     <br />
                                                        •	By-products which are not listed in Form 56-1; and
                                                                                                     <br />
                                                        •	Products that are not counted during business decision-making and do not significantly contribute to environmental impacts.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Collection Method</td>
                                                    <td>•	Identify the product generated from the Reporting unit's boundary
                                                                                                    <br />
                                                        •	Obtain the weight or volume of product by (methods are prioritized by level of 
                                                                                                    accuracy of data obtained, hence first method is preferred):

                                                                                                    <br />
                                                        (1)	Direct measurement and report in MBOE unit; or 
                                                                                                    <br />
                                                        (2)	Direct measurement and report in mass (Tonne, MMSCF); or
                                                                                                    <br />
                                                        (3)	Direct measurement in volume (Litres) and convert to mass by multiplying with density of each product as the following formula:
                                                                                                    <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                   Product in Mass (Tonne) = Product in Volume (Litres) x Product Density (Tonne/Litres)
                                                                                                    <br />
                                                        The specific product density of each reporting unit (i.e. product density per batch, weekly product density, annually product density) should be used for calculating product in mass to reduce the risk of reporting inaccurate data.
                                                                                                    <br />
                                                        (4)	Estimation e.g. mass balance, using default factor.
                                                                                                    <br />
                                                        Note: If method (2) – (4) is applied, Reporting Unit is required to state the methodology used to define product quantity and the reason for using that method.
                                                                                                    <br />
                                                        •	Report the total product in MBOE unit (unit conversion factor can be referred to Appendix 4).
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cHead">Reporting Basis</td>
                                                    <td>•	Quantity of product shall be reported on a monthly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>

            <%--#region Emission --%>
            <div id="DvEmission">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#pnEmission" data-toggle="collapse" style="cursor: pointer;">Emission</div>
                        <div id="pnEmission" class="panel-body pad-no collapse in">
                            <%-- Table --%>
                            <div class="form-group">
                                <div class="col-xs-12">
                                    <div class="table-responsive">
                                        <table id="E160" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2"><strong>NO<sub>X</sub> emission</strong></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <strong>Unit</strong>
                                                    </td>
                                                    <td>Ton
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Purpose</strong>
                                                    </td>
                                                    <td>Measure the scale of the Reporting Unit's air emissions and be able to identify emission reduction strategy and target.
                                                                                                    <br>
                                                        <br>
                                                        Demonstrate compliance with environmental regulation.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Definition</strong>
                                                    </td>
                                                    <td>Mass of NO<sub>X</sub>emitted to the atmosphere.  NO<sub>X</sub> is the generic name for Nitric Oxide (NO) and Nitrogen Dioxide (NO<sub>2</sub>).
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Scope</strong>
                                                    </td>
                                                    <td>The scope of reporting includes emissions of NO<sub>X</sub> from combustion processes (e.g. turbine, boiler, compressor and other engines for power and heat generation); Fluid Catalytic Cracking Unit (FCCU); and Residue Catalytic Cracking Unit (RCCU)).
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Collection Method</strong>
                                                    </td>
                                                    <td>Identify all sources of NO<sub>X</sub> emission i.e. stack.
                                                                                                    <br>
                                                        <br>
                                                        Identify the quantity of NO<sub>X</sub> emission from each stack by:
                                                                                                    <br>
                                                        (1) Direct measurement by Continuous Emission Monitoring (CEM) system or other relevant devices;<br>
                                                        (2) Direct measurement by qualified laboratory;<br>
                                                        (3) Calculation based on process specification, fuel input and combustion efficiency; or<br>
                                                        (4) Estimation by use of default factors.<br>
                                                        <br>
                                                        Calculate the total mass of NO<sub>X</sub> emission from each stack by using the following formula:
                                                                                                    <br>
                                                        <br>
                                                        <strong>Total NO<sub>X</sub>emission</strong> = Concentration (actual O<sub>2</sub>) x Flow rate(normal flow) x Operating hours
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Reporting basis</strong>
                                                    </td>
                                                    <td>Emission of NO<sub>X</sub> shall be reported on a quarterly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        </br>
                                        <table id="E162" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2"><strong>SO<sub>2</sub> emission</strong></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <strong>Unit</strong>
                                                    </td>
                                                    <td>Ton
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Purpose</strong>
                                                    </td>
                                                    <td>Measure the scale of the Reporting Unit's air emissions and be able to identify emission reduction strategy and target.
                                                                                                    <br>
                                                        <br>
                                                        Demonstrate compliance with environmental regulation.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Definition</strong>
                                                    </td>
                                                    <td>Oxides of sulfur emission in form of sulfur dioxide (SO<sub>2</sub>).
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Scope</strong>
                                                    </td>
                                                    <td>The scope of reporting includes SO<sub>2</sub> emission from combustion process, sulfur recovery, sulfuric acid regeneration.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Collection Method</strong>
                                                    </td>
                                                    <td>Identify all sources of SO<sub>2</sub> emission i.e. stack.
                                                                                                    <br>
                                                        <br>
                                                        Identify the quantity of SO<sub>2</sub> emission from each stack by:
                                                                                                    <br>
                                                        (1) Direct measurement by Continuous Emission Monitoring (CEM) system or other relevant devices;
                                                                                                    (2) Direct measurement by qualified laboratory;
                                                                                                    (3) Calculation based on process specification, fuel input and combustion efficiency; or
                                                                                                    (4) Estimation by use of default factors.
                                                                                                    <br>
                                                        Calculate the total mass of SO<sub>2</sub> emission from each stack by using the following formula:
                                                                                                    <br>
                                                        <br>
                                                        <strong>Total SO<sub>2</sub>emission</strong> = Concentration (actual O<sub>2</sub>) x Flow rate(normal flow) x Operating hours
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Reporting basis</strong>
                                                    </td>
                                                    <td>Emission of SO<sub>2</sub> shall be reported on a quarterly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <br />
                                        <table id="E164" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2"><strong>Total suspended particulate matters (TSP) emission</strong></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <strong>Unit</strong>
                                                    </td>
                                                    <td>Ton
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Purpose</strong>
                                                    </td>
                                                    <td>Measure the scale of the Reporting Unit's air emissions and be able to identify emission reduction strategy and target.
                                                                                                   <br>
                                                        <br>
                                                        Demonstrate compliance with environmental regulation.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Definition</strong>
                                                    </td>
                                                    <td>Particles of 10 micrometers or less as defined in national regulation.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Collection Method</strong>
                                                    </td>
                                                    <td>Identify all sources of SO<sub>2</sub> emission i.e. stack.
                                                                                                    <br>
                                                        <br>
                                                        Identify the quantity of SO<sub>2</sub> emission from each stack by:
                                                                                                    <br>
                                                        (1) Direct measurement by Continuous Emission Monitoring (CEM) system or other relevant devices;
                                                                                                    (2) Direct measurement by qualified laboratory;
                                                                                                    (3) Calculation based on process specification, fuel input and combustion efficiency; or
                                                                                                    (4) Estimation by use of default factors.
                                                                                                    <br>
                                                        Calculate the total mass of SO<sub>2</sub> emission from each stack by using the following formula:
                                                                                                    <br>
                                                        <br>
                                                        <strong>Total SO<sub>2</sub>emission</strong> = Concentration (actual O<sub>2</sub>) x Flow rate(normal flow) x Operating hours
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Reporting basis</strong>
                                                    </td>
                                                    <td>Emission of SO<sub>2</sub> shall be reported on a quarterly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        </br/>
                                        <table id="E193" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2"><strong>VOC Emission</strong></th>
                                                </tr>
                                            </thead>
                                            <tbody>

                                                <tr>
                                                    <td width="200px;">
                                                        <strong>Unit</strong>
                                                    </td>
                                                    <td>Ton
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Purpose</strong>
                                                    </td>
                                                    <td>Measure the scale of the Reporting Unit's air emissions and be able to identify emission reduction strategy and target.
                                                                                                    <br>
                                                        <br>
                                                        Demonstrate compliance with environmental regulation.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Definition</strong>
                                                    </td>
                                                    <td>Organic chemical compounds that have high enough vapor pressures under normal conditions to significantly vaporize and enter the atmosphere.
                                                                                                    <br>
                                                        <br>
                                                        Lists of VOC are specified either by national regulation or Notification from Pollution Control Department (PCD).
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Scope</strong>
                                                    </td>
                                                    <td>Emission of VOC from all sources within the Reporting Unit includes fugitive emission sources (e.g. open-ended line, sampling connection system, valve, pump, flange, etc.) and stationary emission sources (e.g. tank far, stack, vent, etc.).
                                                                                                    <br>
                                                        <br>
                                                        Exemption for specific equipments (i.e. inaccessible or unsafe condition) are prescribed in accordance to PCD guidelines.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Collection Method</strong>
                                                    </td>
                                                    <td>Calculation of other relevant indirect GHG emissions is referred to calculation method developed by PTT Group VOC Task Force.
                                                                                                    <br>
                                                        <br>
                                                        Report total VOC emissions by categories i.e.:
                                                                                                    <br>
                                                        (1) Fugitive emission from equipment &amp; machines;<br>
                                                        (2) Emission via stack &amp; vent from fuel combustion;<br>
                                                        (3) Emission from tank farm;<br>
                                                        (4) Emission from loading &amp; unloading;<br>
                                                        (5) Emission from flare; and<br>
                                                        (6) Emission from wastewater treatment system.<br>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Reporting basis</strong>
                                                    </td>
                                                    <td>Emission of VOCs shall be reported on a quarterly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        </br>
                                        <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                            <thead>
                                                <tr class="dt-head-center">
                                                    <th colspan="2"><strong>Hydrocarbon flared</strong></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <strong>Unit</strong>
                                                    </td>
                                                    <td>Ton
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Purpose</strong>
                                                    </td>
                                                    <td>Measure the scale of the Reporting Unit's air emissions and be able to identify emission reduction strategy and target.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Definition</strong>
                                                    </td>
                                                    <td>Total mass of hydrocarbons sent to flare for disposal.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Scope</strong>
                                                    </td>
                                                    <td>Scope includes purge gas and gas for flare pilot burners and gas and oil flared from well testing. This includes all hydrocarbons to flare whether for emergency disposal or installation blow-down, continuous disposal of hydrocarbon gas for which there is no commercial or own use, or to keep the flare system alive (pilot or purge).
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Collection Method</strong>
                                                    </td>
                                                    <td>Identify all sources where hydrocarbon is flared.<br>
                                                        Identify composition of hydrocarbon contained in gas being sent to flared (referred to Appendix 4).<br>
                                                        Calculate total mass of hydrocarbon flared based on flow rate, temperature, pressure, operating time and actual field gas composition data and their molecular weight by using the formula:
                                                                                                    <br>
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <div style="float: left;">Total mass of hydrocarbon flared = &nbsp;</div>
                                                                    </td>
                                                                    <td>
                                                                        <div style="float: left">
                                                                            <div style="border-bottom: 1px solid; font-size: small; text-align: center;">Hydrocarbon fraction x Molecular weight x Flow rate x Flaring duration x Pressure</div>
                                                                            <div style="font-size: small; text-align: center;">0.082 x Temperature</div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <br>
                                                        Report the total mass of hydrocarbon flared by type of flare i.e. low pressure flare, high pressure flare.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Reporting basis</strong>
                                                    </td>
                                                    <td>Total mass of hydrocarbon flared shall be reported on a quarterly basis.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>

                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>

            <%--#region Effluent --%>
            <div id="divEffluent">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#spEffluent" data-toggle="collapse" style="cursor: pointer;">Effluent</div>
                    </div>
                    <div id="spEffluent" class="panel-body pad-no collapse in">
                        <%-- Table --%>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                        <thead>
                                            <tr class="dt-head-center">
                                                <th colspan="2">
                                                    <strong>Total water discharge</strong>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <strong>Unit</strong>
                                                </td>
                                                <td>Cubic meter (m<sup>3</sup>)</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Purpose</strong>
                                                </td>
                                                <td v>Reflect level of impact to the surrounding environment and ecology due to the water discharged from the Reporting Unit.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Definition</strong>
                                                </td>
                                                <td>Total planned or unplanned water effluents discharged over the course of the reporting period to subsurface waters, surface waters, sewers that lead to rivers, oceans, lakes, wetlands, treatment facilities, and groundwater either through:
                                                                                                    <br>
                                                    &nbsp;&nbsp;- Defined discharge point(s);<br>
                                                    &nbsp;&nbsp;- Over land in a dispersed or undefined manner; or<br>
                                                    &nbsp;&nbsp;- Wastewater removed from the Reporting Unit via truck (if not defined as waste as per the regulations.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Scope</strong>
                                                </td>
                                                <td>The scope of reporting includes the planned and unplanned water discharge as per the above definition as well as the water discharge to central wastewater treatment plant (e.g. industrial estate's facility) and water discharged from the desalination process.
                                                                                                    <br>
                                                    Discharge of collected rainwater, domestic sewage and steam condensate (sent back to steam production facility) is not regarded as water discharge.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Collection Method</strong>
                                                </td>
                                                <td>Identify all locations of water discharge from the Reporting Unit.<br>
                                                    Identify total volume of water discharge at each location by:<br>
                                                    &nbsp;&nbsp;(1) Direct measurement e.g. reading from calibrated meter;<br>
                                                    &nbsp;&nbsp;(2) Information from invoices from wastewater treatment service provider;<br>
                                                    &nbsp;&nbsp;(3) Calculation based on pumping capacity multiplied by the time of pumping; or<br>
                                                    &nbsp;&nbsp;(4) Estimation from process specifications or use of water balance.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Reporting basis</strong>
                                                </td>
                                                <td>Volume of water discharge shall be reported on a daily basis.
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                    <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                        <thead>
                                            <tr>
                                                <th colspan="2"><strong>Produced water discharge</strong>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <strong>Unit</strong>
                                                </td>
                                                <td>Cubic meter (m<sup>3</sup>)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Purpose</strong>
                                                </td>
                                                <td>Reflect level of impact to the surrounding environment and ecology due to the produced water discharged from the Reporting Unit.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Definition</strong>
                                                </td>
                                                <td>Total volume of water co-produced with hydrocarbon production from the hydrocarbon reservoir.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Scope</strong>
                                                </td>
                                                <td>The reporting of produced water discharge volume is applicable to <b>Exploration and Production</b> only.<br>
                                                    The scope of reporting of the volume of produced water discharged includes:<br>
                                                    &nbsp;&nbsp;- Re-injected for reservoir management purposes
                                                                                                    &nbsp;&nbsp;- Re-injected for disposal
                                                                                                    &nbsp;&nbsp;- Discharged over board
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Collection Method</strong>
                                                </td>
                                                <td>Calculate the total produced water discharge by using the following formula:<br>
                                                    <b>volume of produced water discharge</b> = Discharge pump capacity x Pump operating duration<br>
                                                    OR<br>
                                                    <b>Total volume of produced water discharge</b> = Total volume of reservoir fluids produced (by metered) x  Fraction of produced water in the reservoir fluids (by well test)   
                                                                                                    <br>
                                                    Or estimate from process requirements and specification                                                                        
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Reporting basis</strong>
                                                </td>
                                                <td>Volume of produced water discharge shall be reported on a daily basis.
                                                </td>
                                            </tr>


                                            <tr>
                                                <th colspan="2">
                                                    <strong>Quality of water discharge</strong>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Unit</strong>
                                                </td>
                                                <td>Ton
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Purpose</strong>
                                                </td>
                                                <td>Reflect level of impact to the surrounding environment and ecology due to the water discharged from the Reporting Unit.<br>
                                                    Demonstrate compliance with environmental regulation.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Definition</strong>
                                                </td>
                                                <td>Quality of water generated from activities and processes of the Reporting Unit and discharged (either with or without treatment) in term of volume.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Scope</strong>
                                                </td>
                                                <td>The scope of reporting includes the total mass of the following contaminants out of the Reporting Unit.<br>
                                                    &nbsp;&nbsp;1) COD<br>
                                                    &nbsp;&nbsp;2) BOD<br>
                                                    &nbsp;&nbsp;3) TSS<br>
                                                    &nbsp;&nbsp;4) Hydrocarbon<br>
                                                    &nbsp;&nbsp;5) Oil and grease
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Collection Method</strong>
                                                </td>
                                                <td>Identify all locations of water discharge.<br>
                                                    Identify the quantity of contaminants discharged from each location by:<br>
                                                    &nbsp;&nbsp;(1) Direct measurement e.g. online monitoring system;<br>
                                                    &nbsp;&nbsp;(2) Direct measurement by qualified laboratory;<br>
                                                    Calculate the total load of contaminants discharged by using the following formula:<br>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Contaminants discharge loads</b> = Concentration x Flow rate x Pump operating duration
                                                                                                    <br>
                                                    the total load of contaminants discharged from the Reporting Unit by parameters.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Reporting basis</strong>
                                                </td>
                                                <td>Mass of discharged contaminants shall be reported on a daily basis.
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>

            <%--#region Spill --%>
            <div id="divSpill">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#spSPILL" data-toggle="collapse" style="cursor: pointer;">Spill</div>
                    </div>
                    <div id="spSPILL" class="panel-body pad-no collapse in">
                        <%-- Table --%>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                        <thead>
                                            <tr>
                                                <th colspan="2">
                                                    <strong>Spill</strong>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <strong>Unit</strong>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Purpose</strong>
                                                </td>
                                                <td>Indicate level of impact to the environment.<br>
                                                    Demonstrate compliance with regulations and avoid risks of regulatory action.<br>
                                                    Evaluate the effectiveness of incident reporting and emergency management measures.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Definition</strong>
                                                </td>
                                                <td>Any loss of containment of hazardous substance that reaches the environment, irrespective of quantity recovered.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Scope</strong>
                                                </td>
                                                <td>All releases from operations that are considered under the Reporting Unit's operational control.<br>
                                                    General inclusions comprise:<br>
                                                    &nbsp;&nbsp;- Loss from primary or secondary containment into the environment including land or water<br>
                                                    &nbsp;&nbsp;- Spill caused by sabotage, earthquakes, storm events or any other accidental event<br>
                                                    &nbsp;&nbsp;- Spill from all transport under the Reporting Unit's operational control
                                                                                                    <br>
                                                    <br>
                                                    On-going aboveground or underground leakage over time shall be counted once at the time identified.<br>
                                                    <br>
                                                    Excluded are:<br>
                                                    &nbsp;&nbsp;- Spill or parts of spills to secondary containment or other impermeable surfaces that do not reach the environment is excluded.
                                                                                                    &nbsp;&nbsp;- Earthen bunds are not counted as secondary containment unless they are engineered to be sufficiently impervious to prevent spill from contaminating underlying soil and/or groundwater.
                                                                                                    <br>
                                                    <br>
                                                    <b>Exploration and Production</b>, reporting shall be in accordance with OGP environmental performance data reporting guideline.<br>
                                                    <br>
                                                    <b>For Trading business</b>, (since its business nature  without production process i.e. oil terminal, supply &amp; logistics and retail businesses) is not included in the scope of reporting.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Collection Method</strong>
                                                </td>
                                                <td>Summarize the number of spills reported and volume of these spills by estimation regardless of volume spill recovered.<br>
                                                    Report the spill in term of both number of spills and total volume of spills.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Reporting basis</strong>
                                                </td>
                                                <td>Spill data shall be reported on a monthly basis.
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <%--Table Tier 1--%>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <table id="ST1" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                    <thead>
                                        <tr>
                                            <th class="text-left">Spill Tier 1</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="text-left">
                                                <br />
                                                <strong>ปริมาณการรั่วไหล (Material Release Threshold Quantities)</strong>&nbsp;ซึ่งเป็นการรั่วไหลอย่างเฉียบพลัน (Acute Release)
                                                ภายในระยะเวลา 1 ชั่วโมง ขึ้นอยู่กับคุณสมบัติของสารเคมี สารไวไฟแต่ละชนิด
                                                <br />
                                                <br />
                                                <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                                    <thead>
                                                        <tr>
                                                            <th class="text-center">ประเภทสารเคมี</th>
                                                            <th class="text-center">ประเภทรั่วไหล</th>
                                                            <th class="text-center">ตัวอย่าง</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td class="text-left">ก๊าซพิษ (TIH) Zone A</td>
                                                            <td class="text-center">5 kg</td>
                                                            <td class="text-left">Phosgene</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="text-left">ก๊าซพิษ (TIH) Zone B</td>
                                                            <td class="text-center">25 kg</td>
                                                            <td class="text-left">Chlorine, Hydrogen sulfide</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="text-left">ก๊าซพิษ (TIH) Zone C</td>
                                                            <td class="text-center">100 kg</td>
                                                            <td class="text-left">Sulfur dioxide</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="text-left">ก๊าซพิษ (TIH) Zone D</td>
                                                            <td class="text-center">200 kg</td>
                                                            <td class="text-left">Ammonia, Ethylene oxide</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="text-left">ก๊าซไวไฟ</td>
                                                            <td class="text-center">500 kg</td>
                                                            <td class="text-left">Ethylene</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="text-left">ของเหลวไวไฟ</td>
                                                            <td class="text-center">1,000 kg (7 barrels)</td>
                                                            <td class="text-left">Benzene, Toluene, Xylenes, Hexanes, Reformate, Condensate</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="text-left">ของเหลวติดไฟ</td>
                                                            <td class="text-center">2,000 kg (14 barrels)</td>
                                                            <td class="text-left">Ethylene Glycol, Ethanolamine, Fuel oil, Kerosene</td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <br />
                                                <strong>Consequence ของ LOPC รุนแรง</strong>&nbsp;ได้แก่
                                                <br />
                                                1.&nbsp;ผลกระทบต่อคน (Human Effect)
                                                <br />
                                                &nbsp;&nbsp;-&nbsp;พนักงานบริษัท (Employee) และผู้รับเหมา (Contractor) เกิดการบาดเจ็บ/เจ็บป่วยถึงขั้นหยุดงาน (Loss Time) หรือ เสียชีวิต (Fatality)
                                                <br />
                                                &nbsp;&nbsp;-&nbsp;บุคคลภายนอก (Visitor) เกิดการบาดเจ็บ/เจ็บป่วยถึงขั้นเข้าพักรักษาตัวในโรงพยาบาล (Hospital admission) หรือ เสียชีวิต (Fatality)
                                                <br />
                                                2.&nbsp;ทำให้เกิดไฟไหม้หรือการระเบิด (Fire and Explosion) เกิดความสูญเสียโดยตรง (Direct cost) ต่อทรัพย์สินของบริษัท ตั้งแต่ 3,500,000 บาท ($100,000) ขึ้นไป
                                                <br />
                                                3.&nbsp;มีการสั่งอพยพคนในชุมชนไปในที่ปลอดภัย อย่างเป็นทางการ (Official)
                                                <br />
                                                4.&nbsp;การรั่วไหลออกจาก Pressure Relief Device (PRD) ที่ส่งผลให้ Liquid carryover, ต้องสั่งอพยพคนไปในที่ปลอดภัยอย่างเป็นทางการ หรือรั่วไหลเป็นเวลาอย่างน้อย 1 ชั่วโมงและมีปริมาณการรั่วไหลอยู่ในระดับ Tier 1
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <%--Table Tier 2--%>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <table id="ST2" class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                    <thead>
                                        <tr>
                                            <th class="text-left">Spill Tier 2</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="text-left">
                                                <br />
                                                <strong>ปริมาณการรั่วไหล (Material Release Threshold Quantities)</strong>&nbsp;ซึ่งเป็นการรั่วไหลอย่างเฉียบพลัน (Acute Release) ภายในระยะเวลา 1 ชั่วโมง ปริมาณ 10% Tier1
                                                <br />
                                                <br />
                                                <strong>Consequence ของ LOPC รุนแรง</strong>&nbsp;ได้แก่
                                                <br />
                                                1.&nbsp;ผลกระทบต่อคน (Human Effect) เกิดการบาดเจ็บ/เจ็บป่วยถึงบันทึก (Recordable Injury)
                                                <br />
                                                2.&nbsp;ทำให้เกิดไฟไหม้หรือการระเบิด (Fire and Explosion) เกิดความสูญเสียโดยตรง (Direct cost) ต่อทรัพย์สินของบริษัท ตั้งแต่ 75,000 บาท ($2,500) ขึ้นไป
                                                <br />
                                                3.&nbsp;การรั่วไหลออกจาก Pressure Relief Device (PRD) ที่ส่งผลให้ Liquid carryover, ต้องสั่งอพยพคนไปในที่ปลอดภัยอย่างเป็นทางการหรือรั่วไหลเป็นเวลาอย่างน้อย 1 ชั่วโมงและมีปริมาณการรั่วไหลอยู่ในระดับ Tier 2
                                                กรณีที่สารเคมีเป็นกลุ่มสารไม่มีพิษและสารไม่ติดไฟด้วย เช่น steam, hot condensate, nitrogen, compressed CO2 หรือ compressed air จะไม่พิจารณาปริมาณการรั่วไหล แต่ให้พิจารณาจาก Consequence ของการรั่วไหลนั้น
                                                ทั้งนี้ การรั่วไหลจากอุปกรณ์ที่ออกแบบไว้โดยเฉพาะ เช่น flare, scrubber or relief devices ที่ไม่ส่งผลให้ Liquid carryover, ต้องสั่งอพยพคนไปในที่ปลอดภัยอย่างเป็นทางการ หรือรั่วไหลเป็นเวลาน้อยกว่า 1 ชั่วโมงและมีปริมาณการรั่วไหล/Consequence
                                                ไม่ถึงระดับ Tier 1, 2 ไม่พิจารณาเป็น Process Safety Incident
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>

            <%--#region ENVIRONMENTAL REGULATORY COMPLIANCE --%>
            <div id="divERC">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#spERC" data-toggle="collapse" style="cursor: pointer;">ENVIRONMENTAL REGULATORY COMPLIANCE</div>
                    </div>
                    <div id="spERC" class="panel-body pad-no collapse in">
                        <%-- Table --%>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                        <thead>
                                            <tr class="dt-head-center">
                                                <th colspan="2">
                                                    <strong>Non-compliance case</strong>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <strong>Unit</strong>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Purpose</strong>
                                                </td>
                                                <td>Indicate the ability of management to ensure that operations conform to certain performance parameter.
                                                                                                    <br>
                                                    <br>
                                                    Strength of record can affect its ability to expand operations or gain permits.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Definition</strong>
                                                </td>
                                                <td>Any non-compliance against local and national regulations related to all types of environmental issues (i.e. emissions, effluents, and waste, as well as material use, energy, water, and biodiversity) applicable to the Reporting Unit.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Scope</strong>
                                                </td>
                                                <td>Scope covers all non-compliances against applicable laws and regulations relevant to the Reporting Unit's environmental aspects as well as against permit conditions and regulated standards.
                                                                                                    <br>
                                                    <br>
                                                    Only documented non-compliance case is taken into account.  Exception is made for non-compliance case raised during the audit carried out by either internal or external party.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Collection Method</strong>
                                                </td>
                                                <td>Identify administrative or judicial sanctions for failure to comply with environmental laws and regulations.<br>
                                                    <br>
                                                    Non-compliance raised by different parties on the same issue is counted as one case.<br>
                                                    <br>
                                                    Report details of non-compliance case including countermeasures.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Reporting basis</strong>
                                                </td>
                                                <td>All non-compliance case is required to be reported on a quarterly basis.
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>

            <%--#region ENVIRONMENTAL COMPLAINT--%>
            <div id="divEC">
                <div class="panel">
                    <div class="panel panel-primary">
                        <div class="panel-heading" href="#spEC" data-toggle="collapse" style="cursor: pointer;">ENVIRONMENTAL COMPLAINT</div>
                    </div>
                    <div id="spEC" class="panel-body pad-no collapse in">
                        <%-- Table --%>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <table class="table dataTable table-bordered table-hover" style="width: 100%; min-width: 100%;">
                                        <thead>
                                            <tr class="dt-head-center">
                                                <th colspan="2">
                                                    <strong>Environmental Complaint</strong>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <strong>Unit</strong>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Purpose</strong>
                                                </td>
                                                <td>
                                                    <br>
                                                    <br>
                                                    Understanding of community complaint at early stage helps managers to respond early.
                                                                                                    <br>
                                                    <br>
                                                    Provide the operation views on its negative impacts on local community and understanding of community’s expectations.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Definition</strong>
                                                </td>
                                                <td>Any external information sent (by phone, fax, letter, etc) to the Reporting Unit with regard to impacts on environmental of the Reporting Unit that is already validated by investigation and verification.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Scope</strong>
                                                </td>
                                                <td>Only validated environmental complaints relating to the activities, products and services of the Reporting Unit shall be accounted.  These validated environmental complaints cover both officials and verbals.
                                                                                                    <br>
                                                    <br>
                                                    Boundary of reporting comprises complaints from both internal and external parties.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Collection Method</strong>
                                                </td>
                                                <td>Validate the received complaint whether it is occurred as a result from plant operation or not.<br>
                                                    <br>
                                                    Data sources include complaint record, complaint incident/investigation report, complaint documents from internal and external parties.<br>
                                                    <br>
                                                    If it is proved that the received complaint caused by the Reporting Unit's activities, count as one case.<br>
                                                    <br>
                                                    Complaint raised by different parties on the same issue is counted as one case.<br>
                                                    <br>
                                                    Report details regarding environmental complaint including countermeasures (corrective and preventive actions) 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Reporting basis</strong>
                                                </td>
                                                <td>The reporting is required based upon each occurrence of environmental complaint.
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--#endregion--%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        $(function () {

        });
        function formfocus(sElementID) {
            if (sElementID + "" != "") {
                ScrollTopToElementsTo(sElementID, 110);
                //ScrollTopToElements(sElementID);
            }
        }
    </script>
</asp:Content>

