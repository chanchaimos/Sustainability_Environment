<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_EPI_FORMS.master" AutoEventWireup="true" CodeFile="Intensity_Petrochemicals.aspx.cs" Inherits="Intensity_Petrochemicals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <input type="text" id="txtTest" class="form-control" />
    <input type="text" id="txtTest2" class="form-control" />
    <input type="text" id="txtTotal" class="form-control" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script>
        var $txtTest = $("input#txtTest");
        var $txtTest2 = $("input#txtTest2");
        var $txtTotal = $("input#txtTotal");
        $(function () {
            $txtTest.blur(function () {
                var nVal = $(this).val();
                $(this).val(CheckTextInput(nVal))
                var nValtotal = "";
                if(CheckTextInput(nVal) != "" && CheckTextInput(nVal).toLowerCase() != "n/a")
                {
                    nValtotal = parseFloat(CheckTextInput(nVal).replace(/,/g, ''));
                }
                if (CheckTextInput($txtTest2.val()) != "" && CheckTextInput($txtTest2.val()).toLowerCase() != "n/a")
                {
                    if(nValtotal != "")
                    {
                        nValtotal = (nValtotal + parseFloat(CheckTextInput($txtTest2.val()).replace(/,/g, '')));
                    }
                    else
                    {
                        nValtotal =parseFloat(CheckTextInput($txtTest2.val()).replace(/,/g, ''));
                    }
                }
                $txtTotal.val(CheckTextOutput(nValtotal+""));
            })
            $txtTest2.blur(function () {
                var nVal = $(this).val();
                $(this).val(CheckTextInput(nVal))
                var nValtotal = "";
                if (CheckTextInput(nVal) != "" && CheckTextInput(nVal).toLowerCase() != "n/a") {
                    nValtotal = parseFloat(CheckTextInput(nVal).replace(/,/g, ''));
                }
                if (CheckTextInput($txtTest.val()) != "" && CheckTextInput($txtTest.val()).toLowerCase() != "n/a") {
                    if (nValtotal != "") {
                        nValtotal = (nValtotal + parseFloat(CheckTextInput($txtTest.val()).replace(/,/g, '')));
                    }
                    else {
                        nValtotal = parseFloat(CheckTextInput($txtTest.val()).replace(/,/g, ''));
                    }
                }
                $txtTotal.val(CheckTextOutput(nValtotal + ""));
            })
        })
        function CheckTextInput(nVal) {
            nVal = nVal.replace(/,/g, '');
            if (nVal != "") {
                if (IsNumberic(nVal)) {
                    var nCheck = parseFloat(nVal);
                    if (nCheck > 0)
                    {
                        nVal = addCommas(nCheck);
                    }
                    else
                    {
                        nVal = "";
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
            return nVal;
        }
        function CheckTextOutput(nValue) {
            var nVal = nValue+"";
            if (nVal != "") {
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
                            if (arrValue[1].length > 3) {
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
            return nVal;
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
    </script>
</asp:Content>

