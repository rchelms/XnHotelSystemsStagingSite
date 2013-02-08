<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoomRateTotalPricingControl.1.ascx.cs"
    Inherits="RoomRateTotalPricingControl" %>
<%@ Reference Control="~/controls/RoomRateTotalPricingItemControl.1.ascx" %>
<asp:Panel CssClass="room_rate_total_pricing" runat="server">
    <xnc:XImage CssClass="image" runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-info-icon.gif" />
    <asp:LinkButton ID="lbRoomRateTotalPricing" runat="server" Text="" ToolTip=""
        meta:resourcekey="lbRoomRateTotalPricing" />
</asp:Panel>
<!-- Room Rate Total Pricing pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_RoomRateTotalPricingPopup"
    style="width: 910px; display: none; overflow: hidden;">
    <div class="highslide-header">
        <ul>
            <li class="highslide-close">
                <xnc:XImageButton ID="btnClose1" runat="server" Location="ImageCDNPath" ImageUrl="~/images/close.gif" OnClientClick="return hs.close(this);"
                    meta:resourcekey="iblClosePopup" />
            </li>
        </ul>
    </div>
    <div class="highslide-body">
        <asp:Panel CssClass="room_rate_total_pricing_popup xngr_wbs_popup" runat="server">
            <asp:Panel CssClass="room_rate_total_pricing_popup_header" runat="server">
                <asp:Panel CssClass="room_rate_total_pricing_popup_header_title" runat="server">
                    <asp:Label ID="lblRoomRatesTotalPricingPopupTitle" runat="server" Text="" meta:resourcekey="lblRoomRatesTotalPricingPopupTitle" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="room_rate_total_pricing_popup_body" runat="server">
                <asp:Panel CssClass="room_rate_total_pricing_popup_body_header_items" runat="server">
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_header room_rate_total_pricing_popup_body_header_date" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupHeaderDate" runat="server" Text="" meta:resourcekey="lblRoomRatesTotalPricingPopupHeaderDate" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_header room_rate_total_pricing_popup_body_header_item_desc" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupHeaderItemDesc" runat="server" Text=""
                            meta:resourcekey="lblRoomRatesTotalPricingPopupHeaderItemDesc" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_header room_rate_total_pricing_popup_body_header_item_price" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupHeaderItemPrice" runat="server" Text=""
                            meta:resourcekey="lblRoomRatesTotalPricingPopupHeaderItemPrice" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_header room_rate_total_pricing_popup_body_header_item_tax" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupHeaderItemTax" runat="server" Text=""
                            meta:resourcekey="lblRoomRatesTotalPricingPopupHeaderItemTax" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_header room_rate_total_pricing_popup_body_header_item_fee" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupHeaderItemFee" runat="server" Text=""
                            meta:resourcekey="lblRoomRatesTotalPricingPopupHeaderItemFee" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_header room_rate_total_pricing_popup_body_header_item_subtotal" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupHeaderItemSubtotal" runat="server" Text=""
                            meta:resourcekey="lblRoomRatesTotalPricingPopupHeaderItemSubtotal" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panPerNightSection" CssClass="room_rate_total_pricing_popup_body_section" runat="server">
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_section_title" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupSectionPerNight" runat="server" Text=""
                            meta:resourcekey="lblRoomRatesTotalPricingPopupSectionPerNight" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_section_items" runat="server">
                        <asp:PlaceHolder ID="phRoomRatesTotalPricingPopupSectionPerNightItems" runat="server" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panPerStaySection" CssClass="room_rate_total_pricing_popup_body_section" runat="server">
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_section_title" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupSectionPerStay" runat="server" Text=""
                            meta:resourcekey="lblRoomRatesTotalPricingPopupSectionPerStay" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_section_items" runat="server">
                        <asp:PlaceHolder ID="phRoomRatesTotalPricingPopupSectionPerStayItems" runat="server" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panTotalSection" CssClass="room_rate_total_pricing_popup_body_section" runat="server">
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_section_title" runat="server">
                        <asp:Label ID="lblRoomRatesTotalPricingPopupSectionTotal" runat="server" Text=""
                            meta:resourcekey="lblRoomRatesTotalPricingPopupSectionTotal" />
                    </asp:Panel>
                    <asp:Panel CssClass="room_rate_total_pricing_popup_body_section_items" runat="server">
                        <asp:PlaceHolder ID="phRoomRatesTotalPricingPopupSectionTotalItems" runat="server" />
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
