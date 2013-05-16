<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CancelAddOnPackageItemControl.1.ascx.cs"
    Inherits="CancelAddOnPackageItemControl" %>
<asp:Panel ID="panCancelAddOnPackageItem" CssClass="mm_room_rate_content mm_text_info mm_cancel_text_info"
    runat="server">
    <asp:Label ID="lblPackageNameText" runat="server" Text="" CssClass="mm_text_x_strong" />
    <asp:Label ID="lblTotalPackagePrice" runat="server" Text="" CssClass="mm_room_rate_price" />
    <asp:Panel runat="server">
            <asp:Label ID="lblPackageQuantityUnits" runat="server" Text="" CssClass="mm_room_detail_button"/>
            <asp:Label ID="lblQuantityMultiplierSymbol" runat="server" Text="" meta:resourcekey="lblQuantityMultiplierSymbol" CssClass="mm_hidden"/>
            <asp:Label ID="lblPackagePrice" runat="server" Text="" CssClass=""/>
            <asp:Label ID="lblPackagePriceType" runat="server" Text="" CssClass=""/>
            <asp:Label ID="lblPackagePriceNights" runat="server" Text="" CssClass=""/>
            (<asp:Label ID="lblPackageQuantity" runat="server" Text="" CssClass=""/>)
    </asp:Panel>
</asp:Panel>
