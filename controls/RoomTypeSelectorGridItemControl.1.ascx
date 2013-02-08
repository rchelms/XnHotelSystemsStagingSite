<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoomTypeSelectorGridItemControl.1.ascx.cs"
    Inherits="RoomTypeSelectorGridItemControl" %>
<%@ Reference Control="~/controls/RateGridHeaderItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RatePlanSelectorGridItemControl.1.ascx" %>
<asp:Panel ID="panRoomTypeSelectorGridItem" CssClass="rate_grid_room_type_content_section"
    runat="server">
    <asp:Panel ID="panRoomType" CssClass="rate_grid_room_type_info" runat="server">
        <asp:Panel CssClass="rate_grid_room_type_info_col_1" runat="server">
            <asp:Panel CssClass="rate_grid_room_type_image" runat="server">
                <xnc:XImage ID="imgRoomType" CssClass="rate_grid_room_type_image_image" Location="HotelMediaCDNPath" runat="server"
                    ImageUrl="" />
            </asp:Panel>
            <asp:Panel CssClass="rate_grid_room_type_desc" runat="server">
                <h4>
                    <asp:Label ID="lblRoomTypeNameText" runat="server" Text="" />
                </h4>
                <p>
                    <asp:Label ID="lblRoomTypeDescriptionText" runat="server" Text="" />
                </p>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="rate_grid_room_type_info_col_2" runat="server">
            <asp:Panel CssClass="rate_grid_room_type_desc_link" runat="server">
                <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-info-icon.gif" />
                <asp:LinkButton ID="lbRoomDescription" runat="server" Text="" ToolTip="" meta:resourcekey="lbRoomDescription" />
            </asp:Panel>
            <asp:Panel ID="panShowMoreRatesLink" CssClass="rate_grid_room_type_more_rates_link"
                runat="server">
                <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-add-icon.gif" />
                <asp:LinkButton ID="lbShowMoreRates" runat="server" Text="" ToolTip="" meta:resourcekey="lbShowMoreRates"
                    OnClick="btnShowMoreLessRates_Click" />
            </asp:Panel>
            <asp:Panel ID="panShowLessRatesLink" CssClass="rate_grid_room_type_less_rates_link"
                runat="server">
                <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-minus-icon.gif" />
                <asp:LinkButton ID="lbShowLessRates" runat="server" Text="" ToolTip="" meta:resourcekey="lbShowLessRates"
                    OnClick="btnShowMoreLessRates_Click" />
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panRatePlanInfo" CssClass="rate_grid_rate_plans_content_section" runat="server">
        <asp:Panel CssClass="rate_grid_nav" runat="server">
            <asp:Panel ID="panRateGridPrevious" CssClass="rate_grid_nav_previous" runat="server">
                <asp:LinkButton ID="btnRateGridPrevious" runat="server" Text="" OnClick="btnRateGridPrevious_Click"
                    meta:resourcekey="btnRateGridPrevious" />
            </asp:Panel>
            <asp:Panel ID="panRateGridNext" CssClass="rate_grid_nav_next" runat="server">
                <asp:LinkButton ID="btnRateGridNext" runat="server" Text="" OnClick="btnRateGridNext_Click"
                    meta:resourcekey="btnRateGridNext" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="rate_grid_header" runat="server">
            <asp:Panel CssClass="rate_grid_header_info" runat="server">
                <asp:Panel CssClass="rate_grid_header_info_rate_plan" runat="server">
                    <asp:Label ID="lblRateGridHeaderRatePlanText" runat="server" Text="" meta:resourcekey="lblRateGridHeaderRatePlanText" />
                </asp:Panel>
                <asp:Panel CssClass="rate_grid_header_info_full_rate" runat="server">
                    <asp:Label ID="lblRateGridHeaderFullRateText" runat="server" Text="" meta:resourcekey="lblRateGridHeaderFullRateText" />
                </asp:Panel>
                <asp:Panel CssClass="rate_grid_header_info_stay_price" runat="server">
                    <asp:Label ID="lblRateGridHeaderStayPriceText" runat="server" Text="" meta:resourcekey="lblRateGridHeaderStayPriceText" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="rate_grid_header_spacer" runat="server">
            </asp:Panel>
            <asp:Panel CssClass="rate_grid_header_dates" runat="server">
                <asp:PlaceHolder ID="phRateGridHeaderItems" runat="server" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panRatePlans" runat="server">
            <asp:PlaceHolder ID="phRatePlans" runat="server" />
        </asp:Panel>
    </asp:Panel>
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
