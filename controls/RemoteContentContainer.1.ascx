<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RemoteContentContainer.1.ascx.cs" Inherits="RemoteContentContainer" %>
<asp:Panel runat="server"  CssClass="mm_content_section">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <iframe runat="server" id="ifrRemoteContentContainer" scrolling="no" class="mm_wrapper_image_holder" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>