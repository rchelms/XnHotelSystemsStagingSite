<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CancelDetailsEntryControl.1.ascx.cs"
    Inherits="CancelDetailsEntryControl" %>
<asp:Panel ID="panCancelDetailsEntry" CssClass="mm_content_section" runat="server">
    <asp:Panel CssClass="mm_step mm_background_edit mm_border_edit" runat="server">
        <asp:Label ID="lblHotelList" runat="server" Text="" meta:resourcekey="lblHotelList"
            CssClass="mm_text_edit" />
        <%--<asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />--%>
        <asp:DropDownList ID="ddlHotelList" runat="server" CssClass="mm_input_textbox" />
    </asp:Panel>
    <asp:Panel CssClass="mm_step mm_background_edit mm_border_edit" runat="server">
        <asp:Label ID="lblConfirmationNumber" runat="server" Text="" meta:resourcekey="lblConfirmationNumber"
            CssClass="mm_text_edit" />
        <%--<asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />--%>
        <asp:TextBox ID="tbConfirmationNumber" runat="server" CssClass="mm_input_textbox" />
    </asp:Panel>
    <asp:Panel CssClass="mm_step mm_background_edit mm_border_edit" runat="server">
        <asp:Label ID="lblGuestLastName" runat="server" Text="" meta:resourcekey="lblGuestLastName"
            CssClass="mm_text_edit" />
        <%--<asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />--%>
        <asp:TextBox ID="tbGuestLastName" runat="server" CssClass="mm_input_textbox" />
    </asp:Panel>
    <asp:Panel CssClass="mm_step mm_background_edit mm_border_edit" runat="server">
        <asp:Panel runat="server" CssClass="mm_wrapper_button_locate_reservation">
            <asp:Button ID="btnLocateBooking" CssClass="mm_button_locate_reservation" runat="server"
                Text="" meta:resourcekey="btnLocateBooking" OnClick="btnLocateBooking_Click" />
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
