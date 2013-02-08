<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StayCriteriaSelectorControl.MM.ascx.cs"
    Inherits="StayCriteriaSelectorControl_MM" %>
<script language="javascript" type="text/javascript">
    resizeHotelButton = function () {
        var parentWidth = $(".MM_StayCriteria_Options").width();
        var hotels = $(".MM_StayCriteria_HotelButton");
        var equallyWidth = (parentWidth / hotels.length) - 2;
        $(".MM_StayCriteria_HotelButtonWrapper").css("width", equallyWidth + "px");
    }
</script>
<asp:Panel ID="panStayCriteriaSelector" runat="server" CssClass="mm_content_section">
    <asp:UpdatePanel ID="formUpdatePanelStayCriteria" runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="panCustomStayCriteriaInfo" CssClass="mm_main_step mm_step mm_background_info mm_border_info" runat="server">
                    <asp:Label ID="lblStayCriteriaInfo" runat="server" meta:resourcekey="lblStayCriteriaInfo" CssClass="mm_text_info"></asp:Label>
                    <asp:Panel ID="panEditButton" CssClass="mm_wrapper_button_edit mm_wrapper_button_start_over mm_background_button_edit" runat="server">
                        <asp:Button ID="btnEdit" runat="server" meta:resourcekey="btnEdit"
                             CssClass="mm_button mm_text_button_edit" 
                            OnClick="btnEdit_Click" OnClientCLick="return btnEdit_click();"/>
                    </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panCustomStayCriteriaEdit" CssClass="" runat="server">
                <asp:Panel ID="panCustomStayCriteriaEditInfo" CssClass="mm_step mm_background_edit mm_border_edit"
                    runat="server">
                    <asp:Label ID="lblStayCriteriaEditInfo" runat="server" CssClass="mm_text_edit" meta:resourcekey="lblStayCriteriaEditInfo"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="panCustomStatyCriteriaEditSelection" runat="server" CssClass="mm_step">
                    <%--<asp:Panel ID="panHotelParisButton" CssClass="mm_edit_background mm_wrapper_button mm_wrapper_button_hotel"
                        runat="server">
                        <asp:Button ID="btnHotelParis" runat="server" CssClass="mm_button"
                            meta:resourcekey="btnHotelParis" OnClientClick="showWaitingPage()"/>
                    </asp:Panel>
                    <asp:Panel ID="panHotelIstanbulButton" CssClass="mm_edit_background mm_wrapper_button mm_wrapper_button_hotel"
                        runat="server">
                        <asp:Button runat="server" ID="btnHotelIstanbul" CssClass="mm_button"
                            meta:resourcekey="btnHotelIstanbul" OnClientClick="showWaitingPage()"/></asp:Panel>--%>
                    <asp:PlaceHolder ID="phdStayCriteriaOptions" runat="server"></asp:PlaceHolder>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
