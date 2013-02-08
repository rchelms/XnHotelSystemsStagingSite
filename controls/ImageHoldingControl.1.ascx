<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImageHoldingControl.1.ascx.cs"
    Inherits="ImageHoldingControl" %>
<asp:Panel runat="server" CssClass="mm_content_section mm_wrapper_image_holder">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:PlaceHolder runat="server" ID="phdImageHolder"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
