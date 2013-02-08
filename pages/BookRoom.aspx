<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="BookRoom.aspx.cs" Inherits="BookRoom" Title="Book Room" %>

<asp:Content ID="cpgBookRoom" ContentPlaceHolderID="cphBody" runat="Server">
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
                    <asp:Label ID="lblBookRoom" runat="server" Text="" meta:resourcekey="lblBookRoom" /></h3>
                <p>
                    <asp:Label ID="lblBookRoomComments" runat="server" Text="" meta:resourcekey="lblBookRoomComments" /></p>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
