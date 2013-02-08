<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchHotelSelectorGridItemControl.1.ascx.cs"
    Inherits="SearchHotelSelectorGridItemControl" %>
<%@ Reference Control="~/controls/HotelDescriptionControl.1.ascx" %>
<%@ Reference Control="~/controls/HotelRatingControl.1.ascx" %>
<%@ Reference Control="~/controls/SpecialRatesIndicatorControl.1.ascx" %>
<%@ Reference Control="~/controls/RateGridDataItemControl.1.ascx" %>
<asp:Panel ID="panSearchHotelSelectorGridItem" CssClass="rate_grid_hotel" runat="server">
    <xnc:XImage ID="imgHotel" CssClass="rate_grid_hotel_info_image" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
    <asp:Panel CssClass="rate_grid_hotel_info" runat="server">
        <asp:Panel CssClass="rate_grid_hotel_info_1" runat="server">
            <asp:Panel CssClass="rate_grid_hotel_info_rating" runat="server">
                <asp:PlaceHolder ID="phHotelRating" runat="server" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="rate_grid_hotel_info_2" runat="server">
            <asp:Panel CssClass="rate_grid_hotel_info_name" runat="server">
                <asp:Label ID="lblHotelNameText" runat="server" Text="" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="rate_grid_hotel_info_3" runat="server">
            <asp:Panel CssClass="rate_grid_hotel_info_address" runat="server">
                <asp:Label ID="lblHotelAddressInfo" runat="server" Text="" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="rate_grid_hotel_info_4" runat="server">
            <asp:Panel CssClass="rate_grid_hotel_special_rates_indicator" runat="server">
                <asp:PlaceHolder ID="phSpecialRatesIndicator" runat="server" />
            </asp:Panel>
            <asp:Panel CssClass="rate_grid_hotel_info_description" runat="server">
                <asp:PlaceHolder ID="phHotelDescription" runat="server" />
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="rate_grid_hotel_select" runat="server">
        <asp:Panel CssClass="rate_grid_hotel_select_button" runat="server">
            <asp:Panel CssClass="field_set_button" runat="server">
                <asp:Panel ID="panSelectSearchHotel" CssClass="std_button" runat="server">
                    <asp:Panel CssClass="std_button_left" runat="server">
                        <asp:Panel CssClass="std_button_right" runat="server">
                            <asp:Panel CssClass="std_button_center" runat="server">
                                <asp:Button ID="btnSelectSearchHotel" CssClass="std_button_control" runat="server"
                                    Text="" meta:resourcekey="btnSelectSearchHotel" OnClick="btnSelectSearchHotel_Click" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="rate_grid_hotel_rates" runat="server">
        <asp:Panel CssClass="rate_grid_data_row" runat="server">
            <asp:PlaceHolder ID="phRateGridDataItems" runat="server" />
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
