<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoomSelectorItemControl.1.ascx.cs"
    Inherits="RoomSelectorItemControl" %>
<asp:Panel ID="panRoomSelectorItem" runat="server">
    <asp:Panel ID="panRoomSelectorItemSelected" CssClass="tab_nav_item_current" runat="server">
        <xnc:XImageButton ID="btnRoomSelectorItemSelected" runat="server" Location="ImageCDNPath" OnClick="btnRoomSelectorItem_Click"
            ImageUrl="~/images/space.gif" />
        <asp:Label ID="lblRoomSelectorItemSelected" runat="server" Text="" />
    </asp:Panel>
    <asp:Panel ID="panRoomSelectorItemNotSelected" CssClass="tab_nav_item" runat="server">
        <xnc:XImageButton ID="btnRoomSelectorItemNotSelected" runat="server" Location="ImageCDNPath" OnClick="btnRoomSelectorItem_Click"
            ImageUrl="~/images/space.gif" />
        <asp:Label ID="lblRoomSelectorItemNotSelected" runat="server" Text="" />
    </asp:Panel>
</asp:Panel>
