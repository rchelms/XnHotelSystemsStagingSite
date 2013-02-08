<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="CancelRoom.aspx.cs" Inherits="CancelRoom" Title="Cancel Room" %>

<asp:Content ID="cpgCancelRoom" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:Panel runat="server">
        <asp:Panel CssClass="content_section" runat="server">
            <asp:Panel CssClass="pane_left" runat="server">
                <asp:Panel CssClass="pane_right" runat="server">
                    <asp:Panel CssClass="pane_center" runat="server">
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="pane_body" runat="server">
                <h3>
                    <asp:Label ID="lblCancelRoom" runat="server" Text="" meta:resourcekey="lblCancelRoom" /></h3>
                <p>
                    <asp:Label ID="lblCancelRoomComments" runat="server" Text="" meta:resourcekey="lblCancelRoomComments" /></p>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
