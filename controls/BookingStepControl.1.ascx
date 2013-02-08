<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookingStepControl.1.ascx.cs"
    Inherits="BookingStepControl" %>
<%@ Reference Control="~/controls/BookingStepItemControl.1.ascx" %>
<asp:Panel ID="panBookingStepControl" CssClass="booking_steps" runat="server">
    <asp:PlaceHolder ID="phBookingStepItems" runat="server" />
</asp:Panel>
