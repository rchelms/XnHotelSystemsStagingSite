<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AlternateHotelRatePlanItemControl.1.ascx.cs"
    Inherits="AlternateHotelRatePlanItemControl" %>
<asp:Panel ID="panRatePlanItem" CssClass="alt_hotel_rate_info" runat="server">
    <asp:Panel CssClass="alt_hotel_rate_plan_info" runat="server">
        <h4>
            <asp:Label ID="lblRatePlanNameText" runat="server" Text="" /></h4>
        <p>
            <asp:Label ID="lblRatePlanDescriptionText" runat="server" Text="" /></p>
    </asp:Panel>
    <asp:Panel CssClass="alt_hotel_rate_price_info" runat="server">
        <h4>
            <asp:Label ID="lblLowestRate" runat="server" Text="" meta:resourcekey="lblLowestRate" />
            <asp:Label ID="lblLowestRateText" runat="server" Text="" /></h4>
    </asp:Panel>
</asp:Panel>
