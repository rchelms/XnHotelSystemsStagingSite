<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ErrorDisplayControl.1.ascx.cs"
    Inherits="ErrorDisplayControl" %>
<asp:Panel ID="panErrorInfo" CssClass="mm_content_section" runat="server">
    <asp:Panel CssClass="mm_page_error" runat="server">
        <asp:Panel runat="server" CssClass="mm_error_block">
            <xnc:ximage runat="server" imageurl="~/images/symbol_error.gif" location="ImageCDNPath" />
        </asp:Panel>
        <asp:Panel CssClass="mm_error_block mm_page_error_instructions" runat="server">
            <h3>
                <asp:Label ID="lblCorrectErrors" runat="server" Text="" meta:resourcekey="lblCorrectErrors" /></h3>
            <p>
                <asp:Label ID="lblErrorList" runat="server" Text="" />
            </p>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
