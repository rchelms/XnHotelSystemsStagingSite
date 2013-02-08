<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AlternateHotelSelectorItemControl.1.ascx.cs"
    Inherits="AlternateHotelSelectorItemControl" %>
<%@ Reference Control="~/controls/AlternateHotelRatePlanItemControl.1.ascx" %>
<%@ Reference Control="~/controls/HotelDescriptionControl.1.ascx" %>
<%@ Reference Control="~/controls/HotelRatingControl.1.ascx" %>
<asp:Panel ID="panAlternateHotelSelectorItem" CssClass="content_section" runat="server">
    <asp:Panel CssClass="pane_left" runat="server">
        <asp:Panel CssClass="pane_right" runat="server">
            <asp:Panel CssClass="pane_center" runat="server">
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="pane_body" runat="server">
        <asp:Panel ID="panAlternateHotelItemInfo" CssClass="alt_hotel_info" runat="server">
            <xnc:XImage ID="imgHotel" CssClass="alt_hotel_info_image" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
            <asp:Panel CssClass="alt_hotel_info_desc_rates" runat="server">
                <asp:Panel CssClass="alt_hotel_rating" runat="server">
                    <asp:PlaceHolder ID="phHotelRating" runat="server" />
                </asp:Panel>
                <asp:Panel CssClass="alt_hotel_info_desc_rates_items" runat="server">
                    <h4>
                        <asp:Label ID="lblHotelNameText" runat="server" Text="" />
                    </h4>
                    <p>
                        <asp:Label ID="lblHotelAddressInfo" runat="server" Text="" />
                    </p>
                    <p>
                        <asp:Label ID="lblTelephone" CssClass="alt_hotel_info_label" runat="server" Text=""
                            meta:resourcekey="lblTelephone" />
                        <asp:Label ID="lblTelephoneNumber" runat="server" Text="" />
                    </p>
                    <p>
                        <asp:Label ID="lblFax" CssClass="alt_hotel_info_label" runat="server" Text="" meta:resourcekey="lblFax" />
                        <asp:Label ID="lblFaxNumber" runat="server" Text="" /></p>
                    <p>
                        <asp:Label ID="lblEmail" CssClass="alt_hotel_info_label" runat="server" Text="" meta:resourcekey="lblEmail" />
                        <asp:Label ID="lblEmailAddress" runat="server" Text="" />
                    </p>
                    <asp:Panel ID="panCompanyRegNumber" runat="server">
                        <p>
                            <asp:Label ID="lblCompanyRegNumber" CssClass="stay_hotel_info_label" runat="server"
                                Text="" meta:resourcekey="lblCompanyRegNumber" />
                            <asp:Label ID="lblCompanyRegNumberInfo" runat="server" Text="" />
                        </p>
                    </asp:Panel>
                    <asp:Panel CssClass="alt_hotel_description" runat="server">
                        <asp:PlaceHolder ID="phHotelDescription" runat="server" />
                    </asp:Panel>
                    <asp:Panel ID="panRatePlans" CssClass="alt_hotel_rate_plans" runat="server">
                        <asp:PlaceHolder ID="phRatePlans" runat="server" />
                    </asp:Panel>
                    <asp:Panel CssClass="field_set_button" runat="server">
                        <asp:Panel ID="panSelectAlternateHotel" CssClass="std_button" runat="server">
                            <asp:Panel CssClass="std_button_left" runat="server">
                                <asp:Panel CssClass="std_button_right" runat="server">
                                    <asp:Panel CssClass="std_button_center" runat="server">
                                        <asp:Button ID="btnSelectAlternateHotel" CssClass="std_button_control" runat="server"
                                            Text="" meta:resourcekey="btnSelectAlternateHotel" OnClick="btnSelectAlternateHotel_Click" />
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
