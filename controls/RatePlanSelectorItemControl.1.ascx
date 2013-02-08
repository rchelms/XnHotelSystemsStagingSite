<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RatePlanSelectorItemControl.1.ascx.cs"
    Inherits="RatePlanSelectorItemControl" %>
<asp:Panel ID="panRatePlanSelectorItem" CssClass="" runat="server">
    <asp:UpdatePanel ID="udpRatePlanSelectorControl" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panRoomRatePlan" runat="server" CssClass="">
                <asp:Label ID="lblRoomTypeText" runat="server" CssClass="mm_text_bold"></asp:Label>
                <asp:Label ID="lblRatePlanNameText" runat="server" CssClass="mm_text_x_strong"></asp:Label>
                <asp:Panel runat="server">
                    <asp:Label runat="server" ID="btnDescription" CssClass="mm_room_detail_button" meta:resourcekey="lblDescriptionButton"></asp:Label>
                    <span class="mm_sperator">| </span>
                    <asp:Label ID="lblRateInfoButton" runat="server" CssClass="mm_room_detail_button"
                        meta:resourcekey="lbViewRates"></asp:Label>
                    <span class="mm_sperator">| </span>
                    <asp:Label ID="lblPolicyButton" runat="server" CssClass="mm_room_detail_button" meta:resourcekey="lbViewPolicies"></asp:Label>
                    <span class="mm_sperator" runat="server" id="spanPhotoSeperator">| </span>
                    <asp:Label ID="lblPhotosButton" runat="server" CssClass="mm_room_detail_button" meta:resourcekey="lblPhotosButton"></asp:Label>
                </asp:Panel>
                <%-- Room Description --%>
                <asp:Panel ID="panRoomTypeDescription" runat="server" CssClass="mm_room_type_description mm_hidden">
                    <p>
                        <asp:Label ID="lblRoomTypeDescription" runat="server" CssClass="mm_detail_text"></asp:Label></p>
                    <p>
                        <asp:Label ID="lblRatePlanDescriptionText" runat="server" CssClass="mm_detail_text"></asp:Label></p>
                </asp:Panel>
                <%-- Rate Info --%>
                <asp:Panel ID="panRoomTypeRateInfo" runat="server" CssClass="mm_room_type_description mm_hidden">
                    <asp:PlaceHolder ID="phRoomTypeRates" runat="server"></asp:PlaceHolder>
                </asp:Panel>
                <%-- View Policy --%>
                <asp:Panel ID="panRoomTypePolicies" runat="server" CssClass="mm_room_type_description mm_hidden">
                    <asp:Panel ID="Panel2" CssClass="mm_rate_policies_popup_policy" runat="server">
                        <asp:Label ID="lblGuaranteePolicyTitle" runat="server" Text="" meta:resourcekey="lblGuaranteePolicyTitle"
                            CssClass="mm_rate_policy_title" /><br />
                        <asp:Label ID="lblGuaranteePolicyText" runat="server" Text="" CssClass="mm_detail_text" />
                    </asp:Panel>
                    <asp:Panel ID="Panel3" CssClass="mm_rate_policies_popup_policy" runat="server">
                        <asp:Label ID="lblCancelPolicyTitle" runat="server" Text="" meta:resourcekey="lblCancelPolicyTitle"
                            CssClass="mm_rate_policy_title" /><br />
                        <asp:Label ID="lblCancelPolicyText" runat="server" Text="" CssClass="mm_detail_text" />
                    </asp:Panel>
                    <asp:Panel ID="Panel4" CssClass="mm_rate_policies_popup_policy" runat="server">
                        <asp:Label ID="lblDepositPolicyTitle" runat="server" Text="" meta:resourcekey="lblDepositPolicyTitle"
                            CssClass="mm_rate_policy_title" /><br />
                        <asp:Label ID="lblDepositPolicyText" runat="server" Text="" CssClass="mm_detail_text" />
                    </asp:Panel>
                    <asp:Panel ID="panPaymentPolicy" CssClass="mm_rate_policies_popup_policy" runat="server">
                        <asp:Label ID="lblPaymentPolicyTitle" runat="server" Text="" meta:resourcekey="lblPaymentPolicyTitle"
                            CssClass="mm_rate_policy_title" /><br />
                        <asp:Label ID="lblPaymentPolicyText" runat="server" Text="" CssClass="mm_detail_text" />
                    </asp:Panel>
                    <asp:Panel ID="Panel5" CssClass="mm_rate_policies_popup_policy" runat="server">
                        <asp:Label ID="lblAcceptedPaymentCardsTitle" runat="server" Text="" meta:resourcekey="lblAcceptedPaymentCardsTitle"
                            CssClass="mm_rate_policy_title" /><br />
                        <asp:Label ID="lblAcceptedPaymentCardsText" runat="server" Text="" CssClass="mm_detail_text" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Label ID="lblTotalStayRateText" runat="server" CssClass="mm_roomrate_price"></asp:Label>
                <asp:Panel ID="panButtonSelect" runat="server" CssClass="mm_wrapper_button_selectOrAdd mm_roomrate_wrapper_button_select">
                    <asp:Button ID="btnSelect" runat="server" CssClass="mm_button mm_text_button_select"
                        OnClick="btnSelect_Click" meta:resourcekey="btnSelect" />
                </asp:Panel>
                <asp:Panel ID="panButtonEdit" runat="server" CssClass="mm_roomrate_wrapper_button_editOrRemoveStyle mm_roomrate_wrapper_button_edit">
                    <asp:Button ID="btnEdit" runat="server" CssClass="mm_button mm_text_button_edit"
                        OnClick="btnEdit_Click" Text="<%$ Resources:SiteResources, Edit %>" />
                </asp:Panel>
                <asp:Button ID="btnShowPhoto" runat="server" CssClass="mm_hidden" OnClick="btnShowPhoto_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
