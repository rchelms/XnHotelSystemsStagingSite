<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LanguageSelectorControl.1.ascx.cs"
    Inherits="LanguageSelectorControl" %>
<%@ Reference Control="~/controls/LanguageSelectorItemControl.1.ascx" %>
<asp:Panel ID="panLanguageSelector" CssClass="mm_language_bar" runat="server">
    <asp:Panel runat="server" ID="panSelectedLanguage">
        <asp:PlaceHolder ID="phSelectedLanguage" runat="server" />
    </asp:Panel>
    <asp:Panel runat="server" ID="panLanguageSelectorEdit" style="display:none">
        <asp:PlaceHolder ID="phLanguageSelector" runat="server" />
    </asp:Panel>
</asp:Panel>
