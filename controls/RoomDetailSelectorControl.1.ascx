<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoomDetailSelectorControl.1.ascx.cs"
    Inherits="RoomDetailSelectorControl" %>
<%@ Reference Control="~/controls/PeopleQuantitySelectorControl.ascx" %>
<%@ Reference Control="~/controls/RoomTypeSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RatePlanSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/AddOnPackageSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/TotalCostControl.1.ascx" %>

<asp:Panel ID="panRoomTypeSelector" runat="server" CssClass="mm_content_section">
    <asp:UpdatePanel ID="upRoomTypeSelector" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panAdultQuantitySelectorControl" runat="server">
                <asp:PlaceHolder ID="phAdultQuantityControl" runat="server"></asp:PlaceHolder>
            </asp:Panel>
            <asp:Panel ID="panChildrenQuantitySelectorControl" runat="server">
                <asp:PlaceHolder ID="phChildrenQuantityControl" runat="server"></asp:PlaceHolder>
            </asp:Panel>

            <asp:Panel ID="panSelectedRoomRate" runat="server" CssClass="mm_main_step">
                <asp:PlaceHolder ID="phSelectedRoomType" runat="server"></asp:PlaceHolder>
            </asp:Panel>

            <asp:Panel ID="panRoomExtraInfo" runat="server" CssClass="mm_background_info">
                <asp:PlaceHolder ID="phSelectedRoomExtras" runat="server"></asp:PlaceHolder>
            </asp:Panel>

            <asp:Panel runat="server" ID="panTempTotalCost">
                <asp:PlaceHolder runat="server" ID="phTempTotalCostControl"></asp:PlaceHolder>
            </asp:Panel>

            <asp:Panel ID="panRoomTypeEditInstruction" runat="server" CssClass="mm_step mm_background_edit mm_border_edit">
                <asp:Label ID="lblRoomTypeEditInstruction" runat="server" CssClass="mm_text_edit"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="panRoomType" runat="server" CssClas="mm_main_step">
                    <asp:PlaceHolder ID="phRoomTypeSelectorControl" runat="server"></asp:PlaceHolder>
            </asp:Panel>

            <asp:Panel ID="panRoomExtraInstruction" runat="server" CssClass="mm_step mm_background_edit mm_border_edit">
                <asp:Label ID="lblRoomExtraInstruction" runat="server" CssClass="mm_text_edit"></asp:Label>
                <asp:Panel ID="panButtonDone" runat="server" CssClass="mm_wrapper_button_finish_select_addon">
                    <asp:Button ID="btnDone" runat="server" CssClass="mm_button mm_text_button_select" OnClick="btnDone_Click"
                        meta:resourcekey="btnDone" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panRoomExtra" runat="server">
                <asp:PlaceHolder ID="phRoomExtraSelectorControl" runat="server"></asp:PlaceHolder>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
