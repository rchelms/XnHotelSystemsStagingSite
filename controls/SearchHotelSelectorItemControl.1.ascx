<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchHotelSelectorItemControl.1.ascx.cs"
    Inherits="SearchHotelSelectorItemControl" %>
<%@ Reference Control="~/controls/HotelDescriptionControl.1.ascx" %>
<%@ Reference Control="~/controls/HotelRatingControl.1.ascx" %>
<%@ Reference Control="~/controls/SpecialRatesIndicatorControl.1.ascx" %>
<asp:Panel ID="panSearchHotelSelectorItem" CssClass="content_section" runat="server">
    <asp:Panel CssClass="pane_left" runat="server">
        <asp:Panel CssClass="pane_right" runat="server">
            <asp:Panel CssClass="pane_center" runat="server">
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="pane_body" runat="server">
        <asp:Panel ID="panSearchHotelItemInfo" CssClass="search_hotel_info" runat="server">
            <xnc:XImage ID="imgHotel" CssClass="search_hotel_info_image" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
            <asp:Panel CssClass="search_hotel_info_desc" runat="server">
                <asp:Panel CssClass="search_hotel_rating" runat="server">
                    <asp:PlaceHolder ID="phHotelRating" runat="server" />
                </asp:Panel>
                <asp:Panel CssClass="search_hotel_info_desc_items" runat="server">
                    <h4>
                        <asp:Label ID="lblHotelNameText" runat="server" Text="" />
                    </h4>
                    <p>
                        <asp:Label ID="lblHotelAddressInfo" runat="server" Text="" />
                    </p>
                    <p>
                        <asp:Label ID="lblTelephone" CssClass="search_hotel_info_label" runat="server" Text=""
                            meta:resourcekey="lblTelephone" />
                        <asp:Label ID="lblTelephoneNumber" runat="server" Text="" />
                    </p>
                    <p>
                        <asp:Label ID="lblFax" CssClass="search_hotel_info_label" runat="server" Text=""
                            meta:resourcekey="lblFax" />
                        <asp:Label ID="lblFaxNumber" runat="server" Text="" />
                    </p>
                    <p>
                        <asp:Label ID="lblEmail" CssClass="search_hotel_info_label" runat="server" Text=""
                            meta:resourcekey="lblEmail" />
                        <asp:Label ID="lblEmailAddress" runat="server" Text="" />
                    </p>
                    <asp:Panel ID="panCompanyRegNumber" runat="server">
                        <p>
                            <asp:Label ID="lblCompanyRegNumber" CssClass="stay_hotel_info_label" runat="server"
                                Text="" meta:resourcekey="lblCompanyRegNumber" />
                            <asp:Label ID="lblCompanyRegNumberInfo" runat="server" Text="" />
                        </p>
                    </asp:Panel>
                    <asp:Panel CssClass="search_hotel_special_rates_indicator" runat="server">
                        <asp:PlaceHolder ID="phSpecialRatesIndicator" runat="server" />
                    </asp:Panel>
                    <asp:Panel CssClass="search_hotel_description" runat="server">
                        <asp:PlaceHolder ID="phHotelDescription" runat="server" />
                    </asp:Panel>
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
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
