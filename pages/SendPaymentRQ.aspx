<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="SendPaymentRQ.aspx.cs" Inherits="SendPaymentRQ" Title="Send Online Payment Request" %>

<asp:Content ID="cpgSendPaymentRQ" ContentPlaceHolderID="cphBody" runat="Server">
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
                    <asp:Label ID="lblSendPaymentRQ" runat="server" Text="" meta:resourcekey="lblSendPaymentRQ" /></h3>
                <p>
                    <asp:Label ID="lblSendPaymentRQComments" runat="server" Text="" meta:resourcekey="lblSendPaymentRQComments" /></p>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
