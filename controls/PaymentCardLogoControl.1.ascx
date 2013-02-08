<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PaymentCardLogoControl.1.ascx.cs"
    Inherits="PaymentCardLogoControl" %>
<%@ Reference Control="~/controls/PaymentCardLogoItemControl.1.ascx" %>
<asp:Panel ID="panPaymentCardLogos" CssClass="payment_card_logo_items" runat="server">
    <asp:PlaceHolder ID="phPaymentCardLogoItems" runat="server" />
</asp:Panel>
