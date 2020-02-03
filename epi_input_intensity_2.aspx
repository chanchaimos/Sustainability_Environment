<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Intensity.master" AutoEventWireup="true" CodeFile="epi_input_intensity_2.aspx.cs" Inherits="epi_input_intensity_2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        function CreateTable() {
            var dvcon = "", dvFile = "", dvRemark = "";
            var i = 1;
            if (arrCt.lstIn != null) {

                CheckEventButton();
                $("button#btnDraft").show();
                $.each(arrCt.lstIn, function () {
                    var sCC = "cTotalYY";
                    if (this.ProductID == 83) {
                        dvcon += '<div id="divPanel_' + i + '">';
                        dvcon += '<div class="panel">';
                        dvcon += '<div class="panel panel-primary">'
                        dvcon += '<div class="panel-heading" href="#dvThroughput" data-toggle="collapse" style="cursor: pointer;">Product and By-product</div>'
                        dvcon += '<div id="dvThroughput" class="panel-body pad-no collapse in">'
                        dvcon += '<div class="form-group">'
                        dvcon += '<div class="col-xs-12">'
                    }
                    if (this.cTotalAll != "Y") {
                        dvcon += '<div class="form-group form-inline" style="margin-top: 1rem;">';
                        dvcon += '<label for="txtByProductAdd" style="margin-right: 1rem;">Number of ' + this.ProductName + '</label>';
                        dvcon += '<input type="text" class="form-control input-sm NoDis" id="txtContextAdd_' + i + '" style="margin-right: 1rem;width: 100px;" disabled/>';
                        dvcon += '<button type="button" class="btn btn-primary NoPRMS" onclick="AddDetail(' + this.ProductID + ',\'tbCon_' + i + '\',' + i + ')" id="btnAddRow_' + i + '">Add</button>';
                        dvcon += '</div>';
                        sCC = "cTotalYN"
                    }
                    else {
                        nRowAll = i;
                        nProductAll = this.ProductID;
                    }
                    dvcon += '<div class="table-responsive" id="dvTB_' + i + '" style="width:100%;"><table id="tbCon_' + i + '" class="table dataTable table-bordered table-hover" style="min-width:100%;">';
                    dvcon += CreateHead();
                    dvcon += "<tbody>" + CreateRowTotal(this.ProductName, i, i, this.ProductID, this.ProductID, sCC, this.sUnit) + "</tbody>";
                    dvcon += '</table></div>';
                    if (this.ProductID == 84 || this.ProductID == 85) {
                        dvRemark += '<div class="col-lg-6 col-sm-12"><div class="well"><div class="form-group" id="dvRemark_' + i + '"><label class="control-label col-xs-12">Remark (' + this.ProductName + ')<span class="text-red">*</span></label><br/><textarea class="form-control" id="txtRemark_' + i + '" style="max-width:100%;width:100%;min-width:100%;" rows="5"></textarea></div></div></div>';
                    }
                    if (this.cTotalAll == "Y") {
                        nRowAll = i;
                        nIDAll = this.ProductID;
                    }
                    if (this.ProductID == 85) {
                        dvcon += "<div class='row' style='margin-top: 20px;'>" + dvRemark + "</div>";
                        dvcon += '</div></div>';
                        dvcon += '</div> </div> </div></div>';
                    }
                    ArrInputFromTableID.push("tbCon_" + i);                 
                    i++;
                });
                CreateHead();
                //$dvRemark.append("<div class='row' style='margin-top: 20px;'>" + dvRemark + "</div>");
                $dvContent.append(dvcon);
                CheckboxQuarterChanged();
                i = 1;
                Enumerable.From(arrCt.lstIn).ForEach(function (f) {
                    var nUnderProduct = f.ProductID;
                    //#region Set Value Detail
                    var sUnit = f.sUnit;
                    var nRow = 1;
                    Enumerable.From(f.lstarrDetail).Where(function (w) { return w.IsActive == true }).ForEach(function (f2) {
                        var tr = CreateRowDetail(i, nRow, f2.nProductID, nUnderProduct, false, false, sUnit)

                        $("table#tbCon_" + i + " tr:last").after(tr);                        
                        $("input#txtIndicatorDetail_" + i + "_" + nRow).val(f2.sIndicator);
                        $("input#txtTargetDetail_" + i + "_" + nRow).val(CheckTextInput(f2.sTarget));
                        var nStatusWF = 0;
                        for (var k = 1; k <= 12 ; k++) {
                            var nVal = "";
                            switch (k) {
                                case 1: nVal = f2.M1 + "";
                                    break;
                                case 2: nVal = f2.M2 + "";
                                    break;
                                case 3: nVal = f2.M3 + "";
                                    break;
                                case 4: nVal = f2.M4 + "";
                                    break
                                case 5: nVal = f2.M5 + "";
                                    break;
                                case 6: nVal = f2.M6 + "";
                                    break;
                                case 7: nVal = f2.M7 + "";
                                    break;
                                case 8: nVal = f2.M8 + "";
                                    break;
                                case 9: nVal = f2.M9 + "";
                                    break;
                                case 10: nVal = f2.M10 + "";
                                    break;
                                case 11: nVal = f2.M11 + "";
                                    break;
                                case 12: nVal = f2.M12 + "";
                                    break;
                            }
                            var wf = Enumerable.From(arrCt.lstwf).Where(function (w) { return w.nMonth == k }).FirstOrDefault();
                            if (wf != null) {
                                nStatusWF = wf.nStatusID;
                            }
                            if (nStatusWF > 0) {
                                $("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', true);

                            }
                            if ($hdfsStatus.val() != "" && nStatusWF == 2) {
                                $("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', false);
                            }
                            if (nStatusWF == 0 && $hdfsStatus.val() != "") {
                                $("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', true);
                            }
                            if ($hdfPRMS.val() == "1" && ($hdfsRoleID.val() != "3" && $hdfsRoleID.val() != "4")) {
                                $("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', true);
                                $("input#txtIndicatorDetail_" + i + "_" + nRow).prop('disabled', true);
                                $("input#txtTargetDetail_" + i + "_" + nRow).prop('disabled', true);
                            }
                            $("input#txtDetail_" + i + "_" + nRow + "_M" + k).val(CheckTextInput(nVal));
                        }
                        var wfAll = Enumerable.From(arrCt.lstwf).Where(function (w) { return w.nStatusID > 0 }).Count();
                        if (wfAll == 12) {
                            $btnSaveDraft.hide();
                            $btnSubmit.hide();

                            $("button[id^=btnAddRow_]").hide();
                        }
                        nRow++;
                        return f2;
                    });
                    $("input#txtTargetDetail_" + i).val(CheckTextInput(f.Target));
                    if ($hdfPRMS.val() == "1" && ($hdfsRoleID.val() != "3" && $hdfsRoleID.val() != "4")) {
                        $("input#txtTargetDetail_" + i).prop('disabled', true);
                    }
                    if ($hdfPRMS.val() == "1" && ($hdfsRoleID.val() != "3" && $hdfsRoleID.val() != "4")) {
                        $("input#txtDetail_" + i + "_" + nRow + "_M" + k).prop('disabled', true);
                        $("input#txtIndicatorDetail_" + i + "_" + nRow).prop('disabled', true);
                        $("input#txtTargetDetail_" + i + "_" + nRow).prop('disabled', true);
                    }
                    $("textarea#txtRemark_" + i).val(f.sRemark);
                    for (var k = 1; k <= 12 ; k++) {
                        var nValTotal = "";
                        switch (k) {
                            case 1: nValTotal = f.M1 + "";
                                break;
                            case 2: nValTotal = f.M2 + "";
                                break;
                            case 3: nValTotal = f.M3 + "";
                                break;
                            case 4: nValTotal = f.M4 + "";
                                break
                            case 5: nValTotal = f.M5 + "";
                                break;
                            case 6: nValTotal = f.M6 + "";
                                break;
                            case 7: nValTotal = f.M7 + "";
                                break;
                            case 8: nValTotal = f.M8 + "";
                                break;
                            case 9: nValTotal = f.M9 + "";
                                break;
                            case 10: nValTotal = f.M10 + "";
                                break;
                            case 11: nValTotal = f.M11 + "";
                                break;
                            case 12: nValTotal = f.M12 + "";
                                break;
                        }

                        $("input#txtTotal_" + i + "_M" + k).val(CheckTextOutput(nValTotal));
                    }
                    var nAll = "";
                    if (Enumerable.From(f.lstarrDetail).Count() > 0) {

                        nAll = Enumerable.From(f.lstarrDetail).Where(function (w) { return w.IsActive == true }).Count();
                    }
                    $("input#txtContextAdd_" + i).val(nAll);
                    $("table[id$=tbCon_" + i + "]").tableHeadFixer({ "left": 1 }, { "head": true });
                    i++;
                    //#endregion

                });
            }
            else {
                nStatus = -1;
                CheckEventButton();
            }

        }
        function CheckDataSave(sMode) {
            IsPass = false;
            var nValtotal = "", nTotal
            All = "";
            var k = 1;
            var IsValidate = true, sDiv = "", sDetail = "";
            var arrValidate = [], arrSub = [], arrDraft = [];
            var nValAllDraft = "";
            Enumerable.From(arrCt.lstIn).ForEach(function (f) {
                arrSub = [];
                var ProductName = f.ProductName;
                if (k == 1) {
                    $.each(arrMonth, function (e, k) {
                        var nValAll = "", sMonth = "";
                        var kMonthAll = parseInt(k);
                        var wfAll = Enumerable.From(arrCt.lstwf).Where(function (w) { return w.nMonth == kMonthAll && w.nStatusID > 0 }).FirstOrDefault();
                        if (wfAll == null) {
                            switch (kMonthAll) {
                                case 1: nValAll = f.M1;
                                    break;
                                case 2: nValAll = f.M2;
                                    break;
                                case 3: nValAll = f.M3;
                                    break;
                                case 4: nValAll = f.M4;
                                    break
                                case 5: nValAll = f.M5;
                                    break;
                                case 6: nValAll = f.M6;
                                    break;
                                case 7: nValAll = f.M7;
                                    break;
                                case 8: nValAll = f.M8;
                                    break;
                                case 9: nValAll = f.M9;
                                    break;
                                case 10: nValAll = f.M10;
                                    break;
                                case 11: nValAll = f.M11;
                                    break;
                                case 12: nValAll = f.M12;
                                    break;
                            }
                            if (nValAll == "") {
                                var sName = arrMonthName[k - 1];
                                var wf = Enumerable.From(arrSub).Where(function (w) { return w.sType == sName }).FirstOrDefault();
                                if (wf == null) {
                                    arrSub.push({
                                        sDetail: "- Please specify " + sName,
                                        sType: sName
                                    })
                                }
                            }
                        }


                    });
                    for (var i = 1; i <= 12 ; i++) {
                        switch (i) {
                            case 1: nValAllDraft = f.M1;
                                break;
                            case 2: nValAllDraft = f.M2;
                                break;
                            case 3: nValAllDraft = f.M3;
                                break;
                            case 4: nValAllDraft = f.M4;
                                break
                            case 5: nValAllDraft = f.M5;
                                break;
                            case 6: nValAllDraft = f.M6;
                                break;
                            case 7: nValAllDraft = f.M7;
                                break;
                            case 8: nValAllDraft = f.M8;
                                break;
                            case 9: nValAllDraft = f.M9;
                                break;
                            case 10: nValAllDraft = f.M10;
                                break;
                            case 11: nValAllDraft = f.M11;
                                break;
                            case 12: nValAllDraft = f.M12;
                                break;
                        }

                        if (nValAllDraft == "" && arrDraft.length == 0) {
                            var sName = arrMonthName[i - 1];
                            arrDraft.push({
                                sDetail: "- Please specify data",
                                ProductName: ProductName
                            })
                        }

                        if (nValAllDraft == "" && arrDraft.length == 0) {

                            var sName = arrMonthName[i - 1];
                            arrDraft.push({
                                sDetail: "- Please specify data",
                                ProductName: ProductName
                            })
                        }
                        if (nValAllDraft != "") {
                            arrDraft = [];
                            debugger
                            break;
                        }
                    }

                }
                if (Enumerable.From(f.lstarrDetail).Where(function (w) { return w.IsActive == true }).Count() > 0) {
                    $.each(arrMonth, function (e, k) {
                        var nValTotal = "", sMonth = "";
                        var kMonthTotal = parseInt(k);
                        var wfTotal = Enumerable.From(arrCt.lstwf).Where(function (w) { return w.nMonth == kMonthTotal && w.nStatusID > 0 }).FirstOrDefault();
                        if (wfTotal == null || wfTotal.nStatusID == 2) {
                            switch (kMonthTotal) {
                                case 1: nValTotal = f.M1;
                                    break;
                                case 2: nValTotal = f.M2;
                                    break;
                                case 3: nValTotal = f.M3;
                                    break;
                                case 4: nValTotal = f.M4;
                                    break
                                case 5: nValTotal = f.M5;
                                    break;
                                case 6: nValTotal = f.M6;
                                    break;
                                case 7: nValTotal = f.M7;
                                    break;
                                case 8: nValTotal = f.M8;
                                    break;
                                case 9: nValTotal = f.M9;
                                    break;
                                case 10: nValTotal = f.M10;
                                    break;
                                case 11: nValTotal = f.M11;
                                    break;
                                case 12: nValTotal = f.M12;
                                    break;
                            }
                            if (nValTotal == "") {
                                var sName = arrMonthName[k - 1];
                                var wf = Enumerable.From(arrSub).Where(function (w) { return w.sType == sName }).FirstOrDefault();
                                if (wf == null) {
                                    arrSub.push({
                                        sDetail: "- Please specify " + sName,
                                        sType: sName
                                    })
                                }
                            }
                        }

                    });
                    Enumerable.From(f.lstarrDetail).Where(function (w) { return w.IsActive == true }).ForEach(function (f2) {
                        if (f2.sIndicator == "") {
                            var wf = Enumerable.From(arrSub).Where(function (w) { return w.sType == "Indicator" }).FirstOrDefault();
                            if (wf == null) {
                                arrSub.push({
                                    sDetail: " - Please specify Indicator ",
                                    sType: "Indicator"
                                })
                            }

                        }
                        $.each(arrMonth, function (e, k) {
                            var nVal = "", sMonth = "";
                            var kMonth = parseInt(k);
                            var wfDT = Enumerable.From(arrCt.lstwf).Where(function (w) { return w.nMonth == kMonth && w.nStatusID > 0 }).FirstOrDefault();
                            if (wfDT == null) {
                                switch (kMonth) {
                                    case 1: nVal = f2.M1;
                                        break;
                                    case 2: nVal = f2.M2;
                                        break;
                                    case 3: nVal = f2.M3;
                                        break;
                                    case 4: nVal = f2.M4;
                                        break
                                    case 5: nVal = f2.M5;
                                        break;
                                    case 6: nVal = f2.M6;
                                        break;
                                    case 7: nVal = f2.M7;
                                        break;
                                    case 8: nVal = f2.M8;
                                        break;
                                    case 9: nVal = f2.M9;
                                        break;
                                    case 10: nVal = f2.M10;
                                        break;
                                    case 11: nVal = f2.M11;
                                        break;
                                    case 12: nVal = f2.M12;
                                        break;
                                }
                                if (nVal == "") {
                                    var sName = arrMonthName[k - 1];
                                    var wf = Enumerable.From(arrSub).Where(function (w) { return w.sType == sName }).FirstOrDefault();
                                    if (wf == null) {
                                        arrSub.push({
                                            sDetail: "- Please specify " + sName,
                                            sType: sName
                                        })
                                    }
                                }
                            }

                        });
                        return f2;
                    });


                    if (f.ProductID == 84) {
                        if ($("textarea#txtRemark_" + k).val() == "") {

                            arrSub.push({
                                sDetail: " - Please specify Remark",
                                sType: "Remark"
                            })
                        }
                        else {
                            f.sRemark = $("textarea#txtRemark_" + k).val();
                        }

                    }
                    else if (f.ProductID == 85) {
                        if ($("textarea#txtRemark_" + k).val() == "") {
                            arrSub.push({
                                sDetail: " - Please specify Remark ",
                                sType: "Remark"
                            })
                        }
                        else {
                            f.sRemark = $("textarea#txtRemark_" + k).val();
                        }
                    }

                }

                if (arrSub.length > 0) {
                    arrValidate.push({
                        ProductName: ProductName,
                        arrSub: arrSub
                    })
                }


                k++;
                return f;
            });

            $.each($dataFileOther, function (e, n) {
                var ID = n.ID;
                var sVal = $("input[id$=txtFile_" + ID + "]").val();
                if (IsValidate == true && sVal == "") {
                    IsValidate = false;
                    sDiv = "dvFile";
                    sDetail = "Please specify File Description";
                    arrSub = [];
                    arrSub.push({
                        sDetail: " - Please specify  File Description",
                        sType: "File"
                    })
                    arrValidate.push({
                        ProductName: "File",
                        arrSub: arrSub
                    })
                }
                n.sDescription = sVal;
            });

            if ((sMode != "0" && arrValidate.length == 0) || (sMode == "0" && arrDraft.length == 0) || sMode == "2") {
                IsPass = true;
            }
            else {
                var sDiv = "<div>"
                $.each(arrValidate, function () {
                    sDiv += "<br/>&bull;" + this.ProductName + "<br/>";
                    $.each(this.arrSub, function () {
                        sDiv += "" + this.sDetail + "<br/>";
                    });
                });
                sDiv += "</div>";
                if (sMode == "0") {
                    sDiv = "<div>"
                    $.each(arrDraft, function () {
                        sDiv += "<br/>&bull;" + this.ProductName + "<br/>";
                        sDiv += "" + this.sDetail + "<br/>";
                    });
                    sDiv += "</div>";


                }
                $("div#divPopContentValidate").empty();
                $("div#divPopContentValidate").append(sDiv);
                $("#popValidate").modal('toggle');

            }
            return IsPass;
        }
        function EventFrom() {
            $("body").delegate("table[id^='tbCon_'] input[id^='txtDetail_']", 'blur', function () {
                var nProductID = $(this).attr('data-nProductID');
                var nUnder = $(this).attr('data-nUnder');
                var nM = parseInt($(this).attr('data-month'));
                var nVal = $(this).val();
                $(this).val(CheckTextInput(nVal))
                nVal = $(this).val();
                nVal = nVal != "" ? nVal.replace(/,/g, "") : "";
                var nUnderProduct = 0;
                var nValtotal = "";
                var k = 1, x = 1;
                var nTotalAll = "";
                Enumerable.From(arrCt.lstIn).ForEach(function (f) {
                    var nUnderProduct = f.ProductID;
                    //#region เก็บ Value Detail
                    Enumerable.From(f.lstarrDetail).Where(function (w) { return w.IsActive == true }).ForEach(function (f2) {
                        if (nUnderProduct == nUnder) {
                            if (nProductID == f2.nProductID) {
                                switch (nM) {
                                    case 1: f2.M1 = nVal + "";
                                        break;
                                    case 2: f2.M2 = nVal + "";
                                        break;
                                    case 3: f2.M3 = nVal + "";
                                        break;
                                    case 4: f2.M4 = nVal + "";
                                        break
                                    case 5: f2.M5 = nVal + "";
                                        break;
                                    case 6: f2.M6 = nVal + "";
                                        break;
                                    case 7: f2.M7 = nVal + "";
                                        break;
                                    case 8: f2.M8 = nVal + "";
                                        break;
                                    case 9: f2.M9 = nVal + "";
                                        break;
                                    case 10: f2.M10 = nVal + "";
                                        break;
                                    case 11: f2.M11 = nVal + "";
                                        break;
                                    case 12: f2.M12 = nVal + "";
                                        break;
                                }

                            }
                            switch (nM) {
                                case 1:
                                    nValtotal = SumValue(nValtotal, f2.M1);
                                    break;
                                case 2:
                                    nValtotal = SumValue(nValtotal, f2.M2);
                                    break;
                                case 3:
                                    nValtotal = SumValue(nValtotal, f2.M3);
                                    break;
                                case 4:
                                    nValtotal = SumValue(nValtotal, f2.M4);
                                    break
                                case 5:
                                    nValtotal = SumValue(nValtotal, f2.M5);
                                    break;
                                case 6:
                                    nValtotal = SumValue(nValtotal, f2.M6);
                                    break;
                                case 7:
                                    nValtotal = SumValue(nValtotal, f2.M7);
                                    break;
                                case 8:
                                    nValtotal = SumValue(nValtotal, f2.M8);
                                    break;
                                case 9:
                                    nValtotal = SumValue(nValtotal, f2.M9);
                                    break;
                                case 10:
                                    nValtotal = SumValue(nValtotal, f2.M10);
                                    break;
                                case 11:
                                    nValtotal = SumValue(nValtotal, f2.M11);
                                    break;
                                case 12:
                                    nValtotal = SumValue(nValtotal, f2.M12);
                                    break;
                            }
                            if (nValtotal != "") {
                                nValtotal = parseFloat(nValtotal);
                            }
                        }
                        switch (nM) {
                            case 1:
                                nTotalAll = SumValue(nTotalAll, f2.M1);
                                break;
                            case 2:
                                nTotalAll = SumValue(nTotalAll, f2.M2);
                                break;
                            case 3:
                                nTotalAll = SumValue(nTotalAll, f2.M3);
                                break;
                            case 4:
                                nTotalAll = SumValue(nTotalAll, f2.M4);
                                break
                            case 5:
                                nTotalAll = SumValue(nTotalAll, f2.M5);
                                break;
                            case 6:
                                nTotalAll = SumValue(nTotalAll, f2.M6);
                                break;
                            case 7:
                                nTotalAll = SumValue(nTotalAll, f2.M7);
                                break;
                            case 8:
                                nTotalAll = SumValue(nTotalAll, f2.M8);
                                break;
                            case 9:
                                nTotalAll = SumValue(nTotalAll, f2.M9);
                                break;
                            case 10:
                                nTotalAll = SumValue(nTotalAll, f2.M10);
                                break;
                            case 11:
                                nTotalAll = SumValue(nTotalAll, f2.M11);
                                break;
                            case 12:
                                nTotalAll = SumValue(nTotalAll, f2.M12);
                                break;
                        }
                        return f2;
                    });

                    //#endregion

                    //#region เก็บ Value Total
                    if (f.ProductID == nUnder) {
                        switch (nM) {
                            case 1: f.M1 = nValtotal + "";
                                break;
                            case 2: f.M2 = nValtotal + "";
                                break;
                            case 3: f.M3 = nValtotal + "";
                                break;
                            case 4: f.M4 = nValtotal + "";
                                break
                            case 5: f.M5 = nValtotal + "";
                                break;
                            case 6: f.M6 = nValtotal + "";
                                break;
                            case 7: f.M7 = nValtotal + "";
                                break;
                            case 8: f.M8 = nValtotal + "";
                                break;
                            case 9: f.M9 = nValtotal + "";
                                break;
                            case 10: f.M10 = nValtotal + "";
                                break;
                            case 11: f.M11 = nValtotal + "";
                                break;
                            case 12: f.M12 = nValtotal + "";
                                break;
                        }
                        $("input#txtTotal_" + k + "_M" + nM).val(CheckTextOutput(nValtotal + ""));

                    }

                    $("input#txtTotal_" + nRowAll + "_M" + nM).val(CheckTextOutput(nTotalAll + ""));
                    k++;
                    return f;
                    //#endregion

                });
                Enumerable.From(arrCt.lstIn).Where(function (w) { return w.ProductID == nProductAll }).ForEach(function (f) {
                    switch (nM) {
                        case 1: f.M1 = nTotalAll;
                            break;
                        case 2: f.M2 = nTotalAll;
                            break;
                        case 3: f.M3 = nTotalAll;
                            break;
                        case 4: f.M4 = nTotalAll;
                            break
                        case 5: f.M5 = nTotalAll;
                            break;
                        case 6: f.M6 = nTotalAll;
                            break;
                        case 7: f.M7 = nTotalAll;
                            break;
                        case 8: f.M8 = nTotalAll;
                            break;
                        case 9: f.M9 = nTotalAll;
                            break;
                        case 10: f.M10 = nTotalAll;
                            break;
                        case 11: f.M11 = nTotalAll;
                            break;
                        case 12: f.M12 = nTotalAll;
                            break;
                    }
                    return f;
                    //#endregion

                });
            });
            $("body").delegate("table[id^='tbCon_'] input[id^='txtIndicatorDetail_']", 'blur', function () {
                var nProductID = $(this).attr('data-nProductID');
                var nUnder = $(this).attr('data-nUnder');
                var nVal = $(this).val();
                var nValtotal = "";
                var k = 1, x = 1;
                var nTotalAll = "";
                Enumerable.From(arrCt.lstIn).ForEach(function (f) {
                    if (f.ProductID == nUnder) {
                        Enumerable.From(f.lstarrDetail).ForEach(function (f2) {
                            if (nProductID == f2.nProductID) {
                                f2.sIndicator = nVal + "";
                            }
                            return f2;
                        });
                    }
                    return f;
                    k++;
                });
            });
            $("body").delegate("table[id^='tbCon_'] input[id^='txtTargetDetail_']", 'blur', function () {
                var nProductID = $(this).attr('data-nProductID');
                var nUnder = $(this).attr('data-nUnder');
                var nVal = $(this).val();
                var nValtotal = "";
                var k = 1, x = 1;
                var nTotalAll = "";
                $(this).val(CheckTextInput(nVal))
                nVal = $(this).val();
                nVal = nVal != "" ? nVal.replace(/,/g, "") : "";
                Enumerable.From(arrCt.lstIn).ForEach(function (f) {
                    if (f.ProductID == nUnder) {
                        if (f.ProductID == nProductID) {
                            f.Target = nVal != "" ? nVal.replace(/,/g, '') : "";
                        }
                        else {
                            Enumerable.From(f.lstarrDetail).ForEach(function (f2) {
                                if (nProductID == f2.nProductID) {
                                    f2.sTarget = nVal != "" ? nVal.replace(/,/g, '') : "";
                                }
                                return f2;
                            });
                        }

                    }
                    return f;
                    k++;
                });
            });
        }
    </script>
</asp:Content>
