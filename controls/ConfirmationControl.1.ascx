<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ConfirmationControl.1.ascx.cs" Inherits="ConfirmationControl" %>
<asp:Panel runat="server" CssClass="mm_content_section">
    <asp:UpdatePanel runat="server" CssClass="mm_main_step">
        <ContentTemplate>
            <asp:Panel ID="panConfimationInfo" runat="server" CssClass="mm_step mm_background_info mm_border_info">
                <asp:Label runat="server" ID="lblConfirmationText" meta:resourcekey="lblConfirmationText" CssClass="mm_text_info"></asp:Label>
                <asp:Label runat="server" ID="lblConfirmationNumber" CssClass="mm_roomrate_price mm_text_white"></asp:Label>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>