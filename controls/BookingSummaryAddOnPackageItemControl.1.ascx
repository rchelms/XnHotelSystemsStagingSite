<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookingSummaryAddOnPackageItemControl.1.ascx.cs"
    Inherits="BookingSummaryAddOnPackageItemControl" %>
<asp:Panel ID="panAddOnPackageSummaryItem" CssClass="booking_summary_addon_package_info"
    runat="server">
    <asp:Panel CssClass="booking_summary_addon_package_desc_info" runat="server">
        <h4>
            <asp:Label ID="lblPackageNameText" runat="server" Text="" /></h4>
    </asp:Panel>
    <asp:Panel CssClass="booking_summary_addon_package_price_info" runat="server">
        <h4>
            <asp:Label ID="lblTotalPackagePrice" runat="server" Text="" /></h4>
    </asp:Panel>
    <asp:Panel CssClass="booking_summary_addon_package_units_info" runat="server">
        <p>
            <asp:Label ID="lblPackageQuantity" runat="server" Text="" />
            <asp:Label ID="lblPackageQuantityUnits" runat="server" Text="" />
            <asp:Label ID="lblQuantityMultiplierSymbol" runat="server" Text="" meta:resourcekey="lblQuantityMultiplierSymbol" />
            <asp:Label ID="lblPackagePrice" runat="server" Text="" />
            <asp:Label ID="lblPackagePriceType" runat="server" Text="" />
            <asp:Label ID="lblPackagePriceNights" runat="server" Text="" /></p>
    </asp:Panel>
</asp:Panel>
