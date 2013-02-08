<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoomTypeSelectorItemControl.1.ascx.cs"
    Inherits="RoomTypeSelectorItemControl" %>
<%@ Reference Control="~/controls/RatePlanSelectorItemControl.1.ascx" %>
<asp:Panel ID="panRoomTypeSelectorItem" CssClass="mm_content_section" runat="server">

    <asp:Panel ID="panRoomTypeSelected" runat="server"></asp:Panel>
    <asp:Panel ID="panRoomTypeEdit" runat="server">
        <asp:Panel ID="panRoomRates" runat="server">
            <asp:PlaceHolder ID="phRatePlans" runat="server" />
        </asp:Panel>
    </asp:Panel>

    <%--<asp:Panel ID="panRoomType" CssClass="room_type_info" runat="server">
        <asp:Panel CssClass="room_type_image" runat="server">
            <xnc:XImage ID="imgRoomType" CssClass="room_type_image_image" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
        </asp:Panel>
        <asp:Panel CssClass="room_type_desc" runat="server">
            <h4>
                <asp:Label ID="lblRoomTypeNameText" runat="server" Text="" />
            </h4>
            <p>
                <asp:Label ID="lblRoomTypeDescriptionText" runat="server" Text="" />
            </p>
            <asp:Panel CssClass="room_type_desc_link" runat="server">
                <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-info-icon.gif" />
                <asp:LinkButton ID="lbRoomDescription" runat="server" Text="" ToolTip="" meta:resourcekey="lbRoomDescription" />
            </asp:Panel>
            <asp:Panel ID="panShowMoreRatesLink" CssClass="room_type_more_rates_link" runat="server">
                <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-add-icon.gif" />
                <asp:LinkButton ID="lbShowMoreRates" runat="server" Text="" ToolTip="" meta:resourcekey="lbShowMoreRates"
                    OnClick="btnShowMoreLessRates_Click" />
            </asp:Panel>
            <asp:Panel ID="panShowLessRatesLink" CssClass="room_type_less_rates_link" runat="server">
                <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-minus-icon.gif" />
                <asp:LinkButton ID="lbShowLessRates" runat="server" Text="" ToolTip="" meta:resourcekey="lbShowLessRates"
                    OnClick="btnShowMoreLessRates_Click" />
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panRatePlanInfo" CssClass="rate_plans_content_section" runat="server">
        <asp:Panel runat="server">
            <asp:Panel CssClass="rate_plan_desc_header rate_plan_header" runat="server">
                <asp:Label ID="lblRatePlanDescription" runat="server" Text="" meta:resourcekey="lblRatePlanDescription" />
            </asp:Panel>
            <asp:Panel CssClass="rate_plan_price_header rate_plan_header" runat="server">
                <asp:Label ID="lblRatePlanPricePerNight" runat="server" Text="" meta:resourcekey="lblRatePlanPricePerNight" />
            </asp:Panel>
            <asp:Panel CssClass="rate_plan_total_header rate_plan_header" runat="server">
                <asp:Label ID="lblRatePlanPriceTotal" runat="server" Text="" meta:resourcekey="lblRatePlanPriceTotal" />
            </asp:Panel>
            <asp:Panel CssClass="rate_plan_select_header rate_plan_header" runat="server">
                <asp:Label ID="lblRatePlanSelect" runat="server" Text="" meta:resourcekey="lblRatePlanSelect" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panRatePlans" runat="server">
            <asp:PlaceHolder ID="phRatePlans" runat="server" />
        </asp:Panel>
    </asp:Panel>--%>
</asp:Panel>
<!-- Room Description pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_RoomTypeDescPopup" style="width: 600px;
    display: none; overflow: hidden;">
    <div class="highslide-header">
        <ul>
            <li class="highslide-close">
                <xnc:XImageButton ID="btnClose1" runat="server" Location="ImageCDNPath" ImageUrl="~/images/close.gif" OnClientClick="return hs.close(this);"
                    meta:resourcekey="iblClosePopup" />
            </li>
        </ul>
    </div>
    <div class="highslide-body">
        <asp:Panel CssClass="room_type_popup xngr_wbs_popup" runat="server">
            <asp:Panel CssClass="room_type_popup_header" runat="server">
                <asp:Panel CssClass="room_type_popup_header_title" runat="server">
                    <asp:Label ID="lblRoomTypePopupTitle" runat="server" Text="" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="room_type_popup_body" runat="server">
                <xnc:XImage ID="imgRoomTypePopupImage" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
                <asp:Label ID="lblRoomTypePopupDescription" runat="server" Text="" />
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
