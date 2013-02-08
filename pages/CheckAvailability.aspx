<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="CheckAvailability.aspx.cs" Inherits="CheckAvailability"
    Title="Check Availability" %>

<asp:Content ID="cpgCheckAvailability" ContentPlaceHolderID="cphBody" runat="Server">
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
                    <asp:Label ID="lblCheckAvailability" runat="server" Text="" meta:resourcekey="lblCheckAvailability" /></h3>
                <p>
                    <asp:Label ID="lblCheckAvailabilityComments" runat="server" Text="" meta:resourcekey="lblCheckAvailabilityComments" /></p>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
