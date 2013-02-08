<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoomQuantitySelectorControl.ascx.cs"
    Inherits="RoomQuantitySelectorControl" %>
<asp:Panel ID="panAvailCalSelector" runat="server" CssClass="mm_content_section">
    <asp:UpdatePanel ID="formUpdatePanel" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEdit" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel runat="server" ID="panInfo" CssClass="mm_main_step mm_background_info">
                <asp:Panel runat="server" ID="panRoomQuantity" CssClass="mm_step mm_border_info">
                    <asp:Label runat="server" ID="lblInfoSummary" CssClass="mm_text_info"></asp:Label>
                    <asp:Panel ID="panEditButton" CssClass="mm_wrapper_button_edit" runat="server">
                        <asp:Button CssClass="mm_button mm_text_button_edit" runat="server" ID="btnEdit"
                            Text="<%$ Resources:SiteResources, Edit %>" OnClick="btnEdit_Click" OnClientClick="return btnEdit_click();" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel runat="server" ID="panPromotionInfo" CssClass="mm_step  mm_border_info">
                    <asp:Label runat="server" ID="lblPromoCodeInfo" CssClass="mm_text_info"></asp:Label>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" ID="panEdit" CssClass="mm_main_step">
                <asp:Panel runat="server" ID="panPromoCode" CssClass="mm_step mm_background_edit mm_border_edit">
                    <asp:Label runat="server" ID="lblPromoCodeInstrucstion" CssClass="mm_text_edit" meta:resourcekey="lblPromoCodeInstruction"></asp:Label>
                    <asp:TextBox runat="server" ID="txtPromoCode" CssClass="mm_input_textbox"></asp:TextBox>
                </asp:Panel>
                <asp:Panel runat="server" ID="panWrapperStepInstrucion" CssClass="mm_step mm_background_edit mm_border_edit">
                    <asp:Label runat="server" ID="lblStepInstruction" CssClass="mm_text_edit" meta:resourcekey="lblEditInstruction"></asp:Label>
                </asp:Panel>
                <asp:Panel runat="server" ID="panRoomQuantitySelectorOptions" CssClass="mm_step">
                    <asp:PlaceHolder runat="server" ID="phdRoomQuantitySelector"></asp:PlaceHolder>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
