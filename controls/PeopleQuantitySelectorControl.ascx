<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PeopleQuantitySelectorControl.ascx.cs" Inherits="PeopleQuantitySelectorControl" %>
<asp:Panel ID="panPeopleQuantitySelector" runat="server" CssClass="mm_content_section">
    <asp:UpdatePanel ID="formUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panInfo" CssClass="mm_main_step mm_step mm_background_info mm_border_info">
                    <asp:Label runat="server" ID="lblInfoSummary" CssClass="mm_text_info"></asp:Label>
                    <asp:Panel ID="panEditButton" CssClass="mm_wrapper_button_edit" runat="server">
                        <asp:Button ID="btnEdit" CssClass="mm_button mm_text_button_edit" runat="server" OnClick="btnEdit_Click" Text="<%$ Resources:SiteResources, Edit %>"/>
                    </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" ID="panEdit" CssClass="mm_main_step">
                <asp:Panel runat="server" ID="panWrapperStepInstrucion" CssClass="mm_step mm_background_edit mm_border_edit">
                    <asp:Label runat="server" ID="lblStepInstruction" CssClass="mm_text_edit" meta:resourcekey="lblEditInstruction"></asp:Label>
                </asp:Panel>
                <asp:Panel runat="server" ID="panRoomQuantitySelectorOptions" CssClass="mm_step">
                    <asp:PlaceHolder runat="server" ID="phdPeopleQuantitySelector"></asp:PlaceHolder>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

