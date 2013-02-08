<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="ProcessPaymentRS.aspx.cs" Inherits="ProcessPaymentRS"
    Title="Process Online Payment Response" %>

<asp:Content ID="cpgProcessPaymentRS" ContentPlaceHolderID="cphBody" runat="Server">
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
                    <asp:Label ID="lblProcessPaymentRS" runat="server" Text="" meta:resourcekey="lblProcessPaymentRS" /></h3>
                <p>
                    <asp:Label ID="lblProcessPaymentRSComments" runat="server" Text="" meta:resourcekey="lblProcessPaymentRSComments" /></p>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
