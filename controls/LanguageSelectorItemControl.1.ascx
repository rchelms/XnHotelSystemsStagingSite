<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LanguageSelectorItemControl.1.ascx.cs"
    Inherits="LanguageSelectorItemControl" %>
<asp:Panel ID="panLanguageItem" CssClass="mm_language_bar_item" runat="server">
    <asp:Panel ID="panLanguageItemSelected" CssClass="mm_language_bar_item_selected" runat="server">
        <xnc:XImageButton ID="btnLanguageItemSelected" runat="server" Location="ImageCDNPath"
            OnClick="btnLanguageItem_Click" ImageUrl="" OnClientClick="return languageSelector_Clicked();"/>
        <asp:Label ID="lblLanguageItemSelectedText" runat="server" Text="" Visible="true" />
    </asp:Panel>
    <asp:Panel ID="panLanguageItemNotSelected" runat="server" CssClass="mm_language_bar_item_not_selected">
        <xnc:XImageButton ID="btnLanguageItemNotSelected" runat="server" Location="ImageCDNPath"
            OnClick="btnLanguageItem_Click" ImageUrl="" />
        <asp:Label ID="lblLanguageItemNotSelectedText" runat="server" Text="" Visible="true" />
    </asp:Panel>
</asp:Panel>
