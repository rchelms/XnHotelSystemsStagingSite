<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RatePlanSelectorGridItemControl.1.ascx.cs"
    Inherits="RatePlanSelectorGridItemControl" %>
<%@ Reference Control="~/controls/RateGridDataItemControl.1.ascx" %>
<asp:Panel ID="panRatePlanSelectorGridItem" CssClass="rate_grid_rate_plan" runat="server">
    <asp:Panel CssClass="rate_grid_rate_plan_left" runat="server">
        <asp:Panel CssClass="rate_grid_rate_plan_info" runat="server">
            <asp:Panel CssClass="rate_grid_rate_plan_info_1" runat="server">
                <asp:Panel CssClass="rate_grid_rate_plan_name" runat="server">
                    <asp:Label ID="lblRatePlanNameText" runat="server" Text="" ToolTip="" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="rate_grid_rate_plan_info_2" runat="server">
                <asp:Panel CssClass="rate_grid_rate_plan_info_links" runat="server">
                    <xnc:XImage CssClass="image" runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-info-icon.gif" />
                    <asp:LinkButton ID="lbViewDescription" runat="server" Text="" ToolTip="" meta:resourcekey="lbViewDescription" />
                    <asp:LinkButton ID="lbViewRates" runat="server" Text="" ToolTip="" meta:resourcekey="lbViewRates" />
                    <asp:LinkButton ID="lbViewPolicies" CssClass="last" runat="server" Text="" ToolTip=""
                        meta:resourcekey="lbViewPolicies" />
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="rate_grid_rate_plan_full_rate" runat="server">
            <asp:Panel CssClass="rate_grid_rate_plan_full_rate_info" runat="server">
                <asp:Label ID="lblFullRateInfoText" runat="server" Text="" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="rate_grid_rate_plan_select" runat="server">
            <asp:Panel ID="panRatePlanAvailable" runat="server" meta:resourcekey="divSelectRate">
                <asp:Panel CssClass="rate_grid_rate_plan_select_price" runat="server">
                    <asp:Label ID="lblRoomRatePriceText" runat="server" Text="" />
                </asp:Panel>
                <asp:Panel CssClass="rate_grid_rate_plan_select_control" runat="server">
                    <asp:Literal ID="litRatePlanSelector" Text="" runat="server" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panRatePlanNotAvailable" runat="server" meta:resourcekey="divRateNotAvailable">
                <asp:Panel CssClass="rate_grid_rate_plan_not_available" runat="server">
                    <asp:Label ID="lblRoomRateNotAvailableText" runat="server" Text="" meta:resourcekey="lblRoomRateNotAvailableText" />
                </asp:Panel>
                <asp:Panel ID="panRatePlanRestricted" CssClass="rate_grid_rate_plan_restricted" runat="server">
                    <xnc:XImage ID="imgRatePlanRestricted" runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-help-icon.gif"
                        ToolTip="" meta:resourcekey="imgRatePlanRestricted" />
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="rate_grid_rate_plan_spacer" runat="server">
    </asp:Panel>
    <asp:Panel CssClass="rate_grid_rate_plan_rates" runat="server">
        <asp:Panel CssClass="rate_grid_data_row" runat="server">
            <asp:PlaceHolder ID="phRateGridDataItems" runat="server" />
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
<!-- "View Rate Plan Description" pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_ViewDescriptionPopup"
    style="width: 450px; display: none; overflow: hidden;">
    <div class="highslide-header">
        <ul>
            <li class="highslide-close">
                <xnc:XImageButton ID="btnClose1" runat="server" Location="ImageCDNPath" ImageUrl="~/images/close.gif" OnClientClick="return hs.close(this);"
                    meta:resourcekey="iblClosePopup" />
            </li>
        </ul>
    </div>
    <div class="highslide-body">
        <asp:Panel CssClass="rate_description_popup xngr_wbs_popup" runat="server">
            <asp:Panel CssClass="rate_description_popup_header" runat="server">
                <asp:Panel CssClass="rate_description_popup_header_title" runat="server">
                    <asp:Label ID="lblViewDescriptionTitle" runat="server" Text="" meta:resourcekey="lblViewDescriptionTitle" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="rate_description_popup_body" runat="server">
                <p>
                    <asp:Label ID="lblRatePlanDescriptionText" runat="server" Text="" />
                </p>
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
<!-- "View Rates" pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_ViewRatesPopup" style="width: 570px;
    display: none; overflow: hidden;">
    <div class="highslide-header">
        <ul>
            <li class="highslide-close">
                <xnc:XImageButton ID="btnClose2" runat="server" Location="ImageCDNPath" ImageUrl="~/images/close.gif" OnClientClick="return hs.close(this);"
                    meta:resourcekey="iblClosePopup" />
            </li>
        </ul>
    </div>
    <div class="highslide-body">
        <asp:Panel CssClass="room_rates_popup xngr_wbs_popup" runat="server">
            <asp:Panel CssClass="room_rates_popup_header" runat="server">
                <asp:Panel CssClass="room_rates_popup_header_title" runat="server">
                    <asp:Label ID="lblViewRatesTitle" runat="server" Text="" meta:resourcekey="lblViewRatesTitle" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="room_rates_popup_body" runat="server">
                <asp:Panel runat="server">
                    <asp:Panel CssClass="room_rates_popup_detail_date_header room_rates_popup_detail_header"
                        runat="server">
                        <asp:Label ID="lblViewRatesDate" runat="server" Text="" meta:resourcekey="lblViewRatesDate" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rates_popup_detail_rate_header room_rates_popup_detail_header"
                        runat="server">
                        <asp:Label ID="lblViewRatesRate" runat="server" Text="" meta:resourcekey="lblViewRatesRate" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rates_popup_detail_inclusions_header room_rates_popup_detail_header"
                        runat="server">
                        <asp:Label ID="lblViewRatesInclusions" runat="server" Text="" meta:resourcekey="lblViewRatesInclusions" />
                    </asp:Panel>
                </asp:Panel>
                <asp:PlaceHolder ID="phPopupRoomRateDetails" runat="server" />
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
<!-- "View Rate Policies" pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_ViewPoliciesPopup" style="width: 450px;
    display: none; overflow: hidden;">
    <div class="highslide-header">
        <ul>
            <li class="highslide-close">
                <xnc:XImageButton ID="btnClose3" runat="server" Location="ImageCDNPath" ImageUrl="~/images/close.gif" OnClientClick="return hs.close(this);"
                    meta:resourcekey="iblClosePopup" />
            </li>
        </ul>
    </div>
    <div class="highslide-body">
        <asp:Panel CssClass="rate_policies_popup xngr_wbs_popup" runat="server">
            <asp:Panel CssClass="rate_policies_popup_header" runat="server">
                <asp:Panel CssClass="rate_policies_popup_header_title" runat="server">
                    <asp:Label ID="lblViewPoliciesTitle" runat="server" Text="" meta:resourcekey="lblViewPoliciesTitle" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="rate_policies_popup_body" runat="server">
                <asp:Panel CssClass="rate_policies_popup_policy" runat="server">
                    <h4>
                        <asp:Label ID="lblGuaranteePolicyTitle" runat="server" Text="" meta:resourcekey="lblGuaranteePolicyTitle" /></h4>
                    <p>
                        <asp:Label ID="lblGuaranteePolicyText" runat="server" Text="" /></p>
                </asp:Panel>
                <asp:Panel CssClass="rate_policies_popup_policy" runat="server">
                    <h4>
                        <asp:Label ID="lblCancelPolicyTitle" runat="server" Text="" meta:resourcekey="lblCancelPolicyTitle" /></h4>
                    <p>
                        <asp:Label ID="lblCancelPolicyText" runat="server" Text="" /></p>
                </asp:Panel>
                <asp:Panel CssClass="rate_policies_popup_policy" runat="server">
                    <h4>
                        <asp:Label ID="lblDepositPolicyTitle" runat="server" Text="" meta:resourcekey="lblDepositPolicyTitle" /></h4>
                    <p>
                        <asp:Label ID="lblDepositPolicyText" runat="server" Text="" /></p>
                </asp:Panel>
                <asp:Panel ID="panPaymentPolicy" CssClass="rate_policies_popup_policy" runat="server">
                    <h4>
                        <asp:Label ID="lblPaymentPolicyTitle" runat="server" Text="" meta:resourcekey="lblPaymentPolicyTitle" /></h4>
                    <p>
                        <asp:Label ID="lblPaymentPolicyText" runat="server" Text="" /></p>
                </asp:Panel>
                <asp:Panel CssClass="rate_policies_popup_policy" runat="server">
                    <h4>
                        <asp:Label ID="lblAcceptedPaymentCardsTitle" runat="server" Text="" meta:resourcekey="lblAcceptedPaymentCardsTitle" /></h4>
                    <p>
                        <asp:Label ID="lblAcceptedPaymentCardsText" runat="server" Text="" /></p>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
<!-- Rate Plan Restriction Notice pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_RatePlanRestrictionNoticePopup"
    style="width: 400px; display: none; overflow: hidden;">
    <div class="highslide-header">
        <ul>
            <li class="highslide-close">
                <xnc:XImageButton ID="btnClose4" runat="server" Location="ImageCDNPath" ImageUrl="~/images/close.gif" OnClientClick="return hs.close(this);"
                    meta:resourcekey="ibClosePopup" />
            </li>
        </ul>
    </div>
    <div class="highslide-body">
        <asp:Panel CssClass="rate_plan_restriction_notice_popup xngr_wbs_popup" runat="server">
            <asp:Panel CssClass="rate_plan_restriction_notice_popup_body" runat="server">
                <asp:Label ID="lblRatePlanRestrictionNoticeMessage" runat="server" Text="" />
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
