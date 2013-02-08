<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CancelRoomSelectorControl.1.ascx.cs"
    Inherits="CancelRoomSelectorControl" %>
<%@ Reference Control="~/controls/CancelRoomSelectorItemControl.1.ascx" %>
<asp:Panel ID="panCancelRoomSelector" CssClass="mm_content_section" runat="server">
    <asp:Panel CssClass="" runat="server">
        <asp:Panel ID="panHotelInfo" CssClass="mm_hidden" runat="server">
            <xnc:XImage ID="imgHotel" CssClass="hotel_info_image" Location="HotelMediaCDNPath"
                runat="server" ImageUrl="" />
            <asp:Panel CssClass="hotel_info_desc" runat="server">
                <asp:Panel CssClass="hotel_info_desc_items" runat="server">
                    <h4>
                        <asp:Label ID="lblHotelNameText" runat="server" Text="" /></h4>
                    <p>
                        <asp:Label ID="lblHotelAddressInfo" runat="server" Text="" /></p>
                    <p>
                        <asp:Label ID="lblTelephone" CssClass="hotel_info_label" runat="server" Text="" meta:resourcekey="lblTelephone" />
                        <asp:Label ID="lblTelephoneNumber" runat="server" Text="" /></p>
                    <p>
                        <asp:Label ID="lblFax" CssClass="hotel_info_label" runat="server" Text="" meta:resourcekey="lblFax" />
                        <asp:Label ID="lblFaxNumber" runat="server" Text="" /></p>
                    <p>
                        <asp:Label ID="lblEmail" CssClass="hotel_info_label" runat="server" Text="" meta:resourcekey="lblEmail" />
                        <asp:Label ID="lblEmailAddress" runat="server" Text="" /></p>
                    <asp:Panel CssClass="hotel_info_stay_details" runat="server">
                        <p>
                            <asp:Label ID="lblArrivalDate" CssClass="hotel_info_label" runat="server" Text=""
                                meta:resourcekey="lblArrivalDate" />
                            <asp:Label ID="lblArrivalDateText" runat="server" Text="" /></p>
                        <p>
                            <asp:Label ID="lblDepartureDate" CssClass="hotel_info_label" runat="server" Text=""
                                meta:resourcekey="lblDepartureDate" />
                            <asp:Label ID="lblDepartureDateText" runat="server" Text="" /></p>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panRooms" runat="server" >
            <asp:PlaceHolder ID="phRooms" runat="server" />
        </asp:Panel>
        <asp:Panel ID="panCancelBooking" CssClass="mm_step mm_background_edit mm_border_edit" runat="server">
            <asp:Panel  CssClass="mm_wrapper_confirm_cancel" runat="server">
                <asp:Button ID="btnCancelBooking" CssClass="mm_button_locate_reservation" runat="server" Text=""
                    meta:resourcekey="btnCancelBooking" OnClick="btnCancelBooking_Click" />
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
