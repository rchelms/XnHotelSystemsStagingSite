<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookingSummaryRoomItemControl.1.ascx.cs"
    Inherits="BookingSummaryRoomItemControl" %>
<%@ Reference Control="~/controls/BookingSummaryAddOnPackageItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomRateTotalPricingControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomRateTotalPricingItemControl.1.ascx" %>
<asp:Panel ID="panRoomSummaryItem" CssClass="booking_summary_room_content_section"
    runat="server">
    <asp:Panel ID="panRoomIdentifier" CssClass="booking_summary_room_header" runat="server">
        <asp:Panel CssClass="booking_summary_room_header_identifier" runat="server">
            <asp:Label ID="lblRoomIdentifier" runat="server" Text="" />
            <asp:Label ID="lblRoomOccupantsInfo" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel CssClass="booking_summary_room_header_total_pricing" runat="server">
            <asp:PlaceHolder ID="phTotalPricing" runat="server" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panRoomSummaryInfo" CssClass="booking_summary_room_info" runat="server">
        <asp:Panel ID="panRoomType" CssClass="booking_summary_room_type_info" runat="server">
            <h4>
                <asp:Label ID="lblRoomTypeNameText" runat="server" Text="" />
            </h4>
            <p>
                <asp:Label ID="lblRoomTypeDescriptionText" runat="server" Text="" />
            </p>
        </asp:Panel>
        <asp:Panel ID="panTotalRoomPrice" CssClass="booking_summary_room_rate_price_info"
            runat="server">
            <h4>
                <asp:Label ID="lblTotalRoomPrice" runat="server" Text="" />
            </h4>
        </asp:Panel>
        <asp:Panel ID="panRatePlan" CssClass="booking_summary_rate_plan_info" runat="server">
            <h4>
                <asp:Label ID="lblRatePlanNameText" runat="server" Text="" />
            </h4>
            <p>
                <asp:Label ID="lblRatePlanDescriptionText" runat="server" Text="" />
            </p>
        </asp:Panel>
        <asp:Panel ID="panPolicies" CssClass="booking_summary_rate_policy_info" runat="server">
            <p>
                <asp:Label ID="lblGuaranteePolicyTitle" CssClass="booking_summary_rate_policy_info_label"
                    runat="server" Text="" meta:resourcekey="lblGuaranteePolicyTitle" />
                <asp:Label ID="lblGuaranteePolicyText" runat="server" Text="" />
            </p>
            <p>
                <asp:Label ID="lblCancelPolicyTitle" CssClass="booking_summary_rate_policy_info_label"
                    runat="server" Text="" meta:resourcekey="lblCancelPolicyTitle" />
                <asp:Label ID="lblCancelPolicyText" runat="server" Text="" />
            </p>
            <p>
                <asp:Label ID="lblDepositPolicyTitle" CssClass="booking_summary_rate_policy_info_label"
                    runat="server" Text="" meta:resourcekey="lblDepositPolicyTitle" />
                <asp:Label ID="lblDepositPolicyText" runat="server" Text="" />
            </p>
            <asp:Panel ID="panPaymentPolicy" runat="server">
                <p>
                    <asp:Label ID="lblPaymentPolicyTitle" CssClass="booking_summary_rate_policy_info_label"
                        runat="server" Text="" meta:resourcekey="lblPaymentPolicyTitle" />
                    <asp:Label ID="lblPaymentPolicyText" runat="server" Text="" />
                </p>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panAddOnPackages" CssClass="booking_summary_addon_packages_info" runat="server">
            <asp:PlaceHolder ID="phAddOnPackages" runat="server" />
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
