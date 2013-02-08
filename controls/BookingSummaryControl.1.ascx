<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookingSummaryControl.1.ascx.cs"
    Inherits="BookingSummaryControl" %>
<%@ Reference Control="~/controls/BookingSummaryRoomItemControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingSummaryAddOnPackageItemControl.1.ascx" %>
<%@ Reference Control="~/controls/HotelDescriptionControl.1.ascx" %>
<%@ Reference Control="~/controls/HotelRatingControl.1.ascx" %>
<%@ Reference Control="~/controls/PaymentReceiptControl.1.ascx" %>
<asp:Panel ID="panBookingSummary" CssClass="content_section" runat="server">
    <asp:Panel CssClass="pane_left" runat="server">
        <asp:Panel CssClass="pane_right" runat="server">
            <asp:Panel CssClass="pane_center" runat="server">
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="pane_body" runat="server">
        <asp:Panel ID="panBookingConfirmationNumberLarge" CssClass="hotel_info_confirm_number_large"
            runat="server">
            <asp:Label ID="lblBookingConfirmationNumber1" CssClass="hotel_info_label" runat="server"
                Text="" meta:resourcekey="lblBookingConfirmationNumber" />
            <asp:Label ID="lblBookingConfirmationNumberText1" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel ID="panHotelInfo" CssClass="hotel_info" runat="server">
            <xnc:XImage ID="imgHotel" CssClass="hotel_info_image" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
            <asp:Panel CssClass="hotel_info_desc" runat="server">
                <asp:Panel CssClass="booking_summary_hotel_rating" runat="server">
                    <asp:PlaceHolder ID="phHotelRating" runat="server" />
                </asp:Panel>
                <asp:Panel CssClass="hotel_info_desc_items" runat="server">
                    <h4>
                        <asp:Label ID="lblHotelNameText" runat="server" Text="" />
                    </h4>
                    <p>
                        <asp:Label ID="lblHotelAddressInfo" runat="server" Text="" />
                    </p>
                    <p>
                        <asp:Label ID="lblTelephone" CssClass="hotel_info_label" runat="server" Text="" meta:resourcekey="lblTelephone" />
                        <asp:Label ID="lblTelephoneNumber" runat="server" Text="" />
                    </p>
                    <p>
                        <asp:Label ID="lblFax" CssClass="hotel_info_label" runat="server" Text="" meta:resourcekey="lblFax" />
                        <asp:Label ID="lblFaxNumber" runat="server" Text="" />
                    </p>
                    <p>
                        <asp:Label ID="lblEmail" CssClass="hotel_info_label" runat="server" Text="" meta:resourcekey="lblEmail" />
                        <asp:Label ID="lblEmailAddress" runat="server" Text="" />
                    </p>
                    <asp:Panel ID="panCompanyRegNumber" runat="server">
                        <p>
                            <asp:Label ID="lblCompanyRegNumber" CssClass="stay_hotel_info_label" runat="server"
                                Text="" meta:resourcekey="lblCompanyRegNumber" />
                            <asp:Label ID="lblCompanyRegNumberInfo" runat="server" Text="" />
                        </p>
                    </asp:Panel>
                    <asp:Panel CssClass="booking_summary_hotel_description" runat="server">
                        <asp:PlaceHolder ID="phHotelDescription" runat="server" />
                    </asp:Panel>
                    <asp:Panel CssClass="hotel_info_stay_details" runat="server">
                        <p>
                            <asp:Label ID="lblArrivalDate" CssClass="hotel_info_label" runat="server" Text=""
                                meta:resourcekey="lblArrivalDate" />
                            <asp:Label ID="lblArrivalDateText" runat="server" Text="" />
                        </p>
                        <p>
                            <asp:Label ID="lblDepartureDate" CssClass="hotel_info_label" runat="server" Text=""
                                meta:resourcekey="lblDepartureDate" />
                            <asp:Label ID="lblDepartureDateText" runat="server" Text="" />
                        </p>
                    </asp:Panel>
                    <asp:Panel ID="panBookingConfirmationNumber" CssClass="hotel_info_confirm_number"
                        runat="server">
                        <p>
                            <asp:Label ID="lblBookingConfirmationNumber2" CssClass="hotel_info_label" runat="server"
                                Text="" meta:resourcekey="lblBookingConfirmationNumber" />
                            <asp:Label ID="lblBookingConfirmationNumberText2" runat="server" Text="" />
                        </p>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panRooms" CssClass="booking_summary_rooms_content_section" runat="server">
            <asp:PlaceHolder ID="phRooms" runat="server" />
        </asp:Panel>
        <asp:Panel ID="panTotalRoomCost" CssClass="booking_summary_total_cost" runat="server">
            <asp:Panel CssClass="booking_summary_total_cost_desc" runat="server">
                <asp:Label ID="lblTotalRoomsPrice" runat="server" Text="" meta:resourcekey="lblTotalRoomsPrice" />
            </asp:Panel>
            <asp:Panel CssClass="booking_summary_total_cost_price" runat="server">
                <asp:Label ID="lblTotalRoomsPriceText" runat="server" Text="" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panTotalRoomDeposit" CssClass="booking_summary_total_deposit" runat="server">
            <asp:Panel CssClass="booking_summary_total_deposit_desc" runat="server">
                <asp:Label ID="lblTotalRoomsDeposit" runat="server" Text="" meta:resourcekey="lblTotalRoomsDeposit" />
            </asp:Panel>
            <asp:Panel CssClass="booking_summary_total_deposit_amount" runat="server">
                <asp:Label ID="lblTotalRoomsDepositText" runat="server" Text="" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panPaymentReceipt" CssClass="booking_summary_payment_receipt" runat="server">
            <asp:PlaceHolder ID="phPaymentReceipt" runat="server" />
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
<!-- Hotel Description pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_HotelDescPopup" style="width: 600px;
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
        <asp:Panel CssClass="hotel_desc_popup xngr_wbs_popup" runat="server">
            <asp:Panel CssClass="hotel_desc_popup_header" runat="server">
                <asp:Panel CssClass="hotel_desc_popup_header_title" runat="server">
                    <asp:Label ID="lblHotelDescPopupTitle" runat="server" Text="" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="hotel_desc_popup_body" runat="server">
                <xnc:XImage ID="imgHotelDescPopupImage" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
                <asp:Label ID="lblHotelDescPopupDescription" runat="server" Text="" />
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
