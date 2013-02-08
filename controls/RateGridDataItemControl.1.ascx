<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RateGridDataItemControl.1.ascx.cs"
    Inherits="RateGridDataItemControl" %>
<asp:Panel CssClass="rate_grid_data_item" runat="server">
    <asp:Panel ID="panRateGridDataOpenWeekday" CssClass="rate_grid_data_item_open_weekday"
        runat="server">
        <asp:Label ID="lblRateGridDataOpenWeekday" runat="server" Text="" ToolTip="" />
    </asp:Panel>
    <asp:Panel ID="panRateGridDataOpenWeekend" CssClass="rate_grid_data_item_open_weekend"
        runat="server">
        <asp:Label ID="lblRateGridDataOpenWeekend" runat="server" Text="" ToolTip="" />
    </asp:Panel>
    <asp:Panel ID="panRateGridDataOpenSelected" CssClass="rate_grid_data_item_open_selected"
        runat="server">
        <asp:Label ID="lblRateGridDataOpenSelected" runat="server" Text="" ToolTip="" />
    </asp:Panel>
    <asp:Panel ID="panRateGridDataSold" CssClass="rate_grid_data_item_sold" runat="server">
        <asp:Label runat="server" Text="" meta:resourcekey="lblRateGridDataItemSold" />
    </asp:Panel>
    <asp:Panel ID="panRateGridDataNull" CssClass="rate_grid_data_item_null" runat="server">
        <asp:Label runat="server" Text="" meta:resourcekey="lblRateGridDataItemNull" />
    </asp:Panel>
</asp:Panel>
