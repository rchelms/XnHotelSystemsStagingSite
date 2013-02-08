<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RateGridHeaderItemControl.1.ascx.cs"
    Inherits="RateGridHeaderItemControl" %>
<asp:Panel ID="panRateGridHeaderItem" CssClass="rate_grid_header_item" runat="server" meta:resourcekey="panRateGridHeaderItem" >
    <xnc:XImageButton ID="btnItemDate" runat="server" OnClick="btnItemDate_Click" Location="ImageCDNPath" ImageUrl="~/images/space.gif" />
    <asp:Panel ID="panRateGridHeaderWeekday" CssClass="rate_grid_header_item_weekday"
        runat="server">
        <asp:Panel ID="Panel1" CssClass="rate_grid_header_item_legend" runat="server">
            <asp:Label ID="lblWeekdayLegendText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="Panel2" CssClass="rate_grid_header_item_dow" runat="server">
            <asp:Label ID="lblWeekdayDOWText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="Panel3" CssClass="rate_grid_header_item_date" runat="server">
            <asp:Label ID="lblWeekdayDateText" runat="server" Text="" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panRateGridHeaderWeekdaySelected" CssClass="rate_grid_header_item_weekday_selected"
        runat="server">
        <asp:Panel ID="Panel4" CssClass="rate_grid_header_item_legend" runat="server">
            <asp:Label ID="lblWeekdayLegendTextSelected" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="Panel5" CssClass="rate_grid_header_item_dow" runat="server">
            <asp:Label ID="lblWeekdayDOWTextSelected" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="Panel6" CssClass="rate_grid_header_item_date" runat="server">
            <asp:Label ID="lblWeekdayDateTextSelected" runat="server" Text="" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panRateGridHeaderWeekend" CssClass="rate_grid_header_item_weekend"
        runat="server">
        <asp:Panel ID="Panel7" CssClass="rate_grid_header_item_legend" runat="server">
            <asp:Label ID="lblWeekendLegendText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="Panel8" CssClass="rate_grid_header_item_dow" runat="server">
            <asp:Label ID="lblWeekendDOWText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="Panel9" CssClass="rate_grid_header_item_date" runat="server">
            <asp:Label ID="lblWeekendDateText" runat="server" Text="" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panRateGridHeaderWeekendSelected" CssClass="rate_grid_header_item_weekend_selected"
        runat="server">
        <asp:Panel ID="Panel10" CssClass="rate_grid_header_item_legend" runat="server">
            <asp:Label ID="lblWeekendLegendTextSelected" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="Panel11" CssClass="rate_grid_header_item_dow" runat="server">
            <asp:Label ID="lblWeekendDOWTextSelected" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="Panel12" CssClass="rate_grid_header_item_date" runat="server">
            <asp:Label ID="lblWeekendDateTextSelected" runat="server" Text="" />
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
