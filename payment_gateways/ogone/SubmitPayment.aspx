<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubmitPayment.aspx.cs" Inherits="Ogone_SubmitPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ogone Submit Payment</title>
    <xnc:xlink href="~/css/Special/payment_gateway.css" type="text/css" rel="stylesheet" Location="CSSCDNPath" runat="server" />
</head>
<body onload="document.forms[0].submit()">
    <asp:Panel CssClass="submit_payment_container" runat="server">
        <asp:Panel CssClass="submit_payment_content" runat="server">
            <asp:Label ID="lblSubmitPaymentProcessing" runat="server" Text="" meta:resourcekey="lblSubmitPaymentProcessing" />
            <%--<xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/Processing_Dots.gif" />--%>
        </asp:Panel>
        <asp:Panel CssClass="submit_payment_form" runat="server">
            <asp:Literal ID="lit3DSecureData" runat="server"></asp:Literal>
        </asp:Panel>
    </asp:Panel>
</body>
</html>
