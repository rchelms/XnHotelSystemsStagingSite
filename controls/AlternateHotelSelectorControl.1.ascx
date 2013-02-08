<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AlternateHotelSelectorControl.1.ascx.cs"
    Inherits="AlternateHotelSelectorControl" %>
<%@ Reference Control="~/controls/AlternateHotelSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/AlternateHotelRatePlanItemControl.1.ascx" %>
<asp:Panel ID="panAlternateHotelSelector" CssClass="content_section" runat="server">
    <asp:Panel CssClass="pane_left" runat="server">
        <asp:Panel CssClass="pane_right" runat="server">
            <asp:Panel CssClass="pane_center" runat="server">
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="pane_body" runat="server">
        <asp:Panel ID="panAlternateHotelInfo" runat="server">
            <h3>
                <asp:Label ID="lblAlternateHotelInfo" runat="server" Text="" meta:resourcekey="lblAlternateHotelInfo" /></h3>
            <p>
                <asp:Label ID="lblAlternateHotelInfoInstructions" runat="server" Text="" meta:resourcekey="lblAlternateHotelInfoInstructions" /></p>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
<asp:Panel ID="panAlternateHotelItems" runat="server">
    <asp:PlaceHolder ID="phAlternateHotelItems" runat="server" />
</asp:Panel>
