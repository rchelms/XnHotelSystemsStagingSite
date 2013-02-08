<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="SearchForHotels.aspx.cs" Inherits="SearchForHotels"
    Title="Search For Hotels" %>

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
                    <asp:Label ID="lblSearchForHotels" runat="server" Text="" meta:resourcekey="lblSearchForHotels" /></h3>
                <p>
                    <asp:Label ID="lblSearchForHotelsComments" runat="server" Text="" meta:resourcekey="lblSearchForHotelsComments" /></p>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
