<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SqlCommand.aspx.cs" Inherits="SqlCommand" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbCon" runat="server"></asp:Label>
            <asp:TextBox runat="server" ID="txtCommand" TextMode="MultiLine" Rows="5" Width="1024px"></asp:TextBox>
            <asp:Button runat="server" ID="btnExecute" OnClick="btnExecute_Click" Text="GO!!!!" />
            <br />
            <br />
            <asp:DataGrid runat="server" ID="dgd" AutoGenerateColumns="true"></asp:DataGrid>
        </div>
    </form>
</body>
</html>
