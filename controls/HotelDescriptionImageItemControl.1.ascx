<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HotelDescriptionImageItemControl.1.ascx.cs"
    Inherits="HotelDescriptionImageItemControl" %>
<asp:Panel CssClass="hotel_image_popup_body_item xngr_wbs_popup" runat="server">
    <asp:Panel CssClass="hotel_image_popup_body_image_area" runat="server">
        <xnc:XImage ID="imgHotelImage" CssClass="image" Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
    </asp:Panel>
    <asp:Panel CssClass="hotel_image_popup_body_text_area" runat="server">
        <h3>
            <asp:Label ID="lblTitle" runat="server" Text="" /></h3>
        <p>
            <asp:Label ID="lblDescription" runat="server" Text="" /></p>
        <p>
            <asp:Label ID="lblCopyright" runat="server" Text="" /></p>
    </asp:Panel>
</asp:Panel>
