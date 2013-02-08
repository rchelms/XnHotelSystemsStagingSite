<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HotelDescriptionControl.1.ascx.cs"
    Inherits="HotelDescriptionControl" %>
<%@ Reference Control="~/controls/HotelDescriptionItemControl.1.ascx" %>
<%@ Reference Control="~/controls/HotelDescriptionImageItemControl.1.ascx" %>
<asp:Panel CssClass="hotel_description" runat="server">
    <xnc:XImage CssClass="image" runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-info-icon.gif" />
    <asp:LinkButton ID="lbHotelDescription" runat="server" Text="" ToolTip="" meta:resourcekey="lbHotelDescription" />
    <asp:HyperLink ID="hlHotelBrochure" runat="server" Target="_blank" Text="" ToolTip=""
        meta:resourcekey="hlHotelBrochure" />
    <asp:HyperLink ID="hlHotelMap" runat="server" Target="_blank" Text="" ToolTip=""
        meta:resourcekey="hlHotelMap" />
    <asp:HyperLink ID="hlHotelPhotos" CssClass="last" runat="server" Text="" ToolTip=""
        meta:resourcekey="lbHotelPhotos" NavigateUrl="" />
</asp:Panel>
<!-- Hotel Description pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_HotelDescPopup" style="width: 600px;
    height: 450px; display: none; overflow: hidden;">
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
                <asp:Panel CssClass="hotel_desc_popup_body_image" runat="server">
                    <xnc:XImage ID="imgHotelDescPopupImage" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
                </asp:Panel>
                <asp:Panel CssClass="hotel_desc_popup_body_desc_items" runat="server">
                    <asp:PlaceHolder ID="phHotelDescriptionItems" runat="server" />
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
<!-- Image Gallery pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_ImageGalleryPopup" style="width: 600px;
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
    </div>
</div>
