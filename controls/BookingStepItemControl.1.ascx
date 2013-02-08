<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookingStepItemControl.1.ascx.cs"
    Inherits="BookingStepItemControl" %>
<asp:Panel ID="panBookingStepItem" CssClass="booking_step" runat="server">
    <asp:Panel ID="panPastBookingStepItem" CssClass="step_done" runat="server">
        <asp:Panel runat="server" CssClass="step_number">
            <asp:Label ID="lblPastStepNumberText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel runat="server" CssClass="step_description">
            <asp:Label ID="lblPastStepDescriptionText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel runat="server" CssClass="step_image">
            <xnc:XImageButton ID="btnPastStep" Location="ImageCDNPath" runat="server" OnClick="btnBookingStepItem_Click"
                ImageUrl="~/images/space.gif" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panPresentBookingStepItem" CssClass="step_inprogress" runat="server">
        <asp:Panel runat="server" CssClass="step_number">
            <asp:Label ID="lblPresentStepNumberText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel runat="server" CssClass="step_description">
            <asp:Label ID="lblPresentStepDescriptionText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel runat="server" CssClass="step_image">
            <xnc:XImageButton ID="btnPresentStep" Location="ImageCDNPath" runat="server" OnClick="btnBookingStepItem_Click"
                ImageUrl="~/images/space.gif" Enabled="false" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panFutureBookingStepItem" CssClass="step_future" runat="server">
        <asp:Panel runat="server" CssClass="step_number">
            <asp:Label ID="lblFutureStepNumberText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel runat="server" CssClass="step_description">
            <asp:Label ID="lblFutureStepDescriptionText" runat="server" Text="" />
        </asp:Panel>
        <asp:Panel runat="server" CssClass="step_image">
            <xnc:XImageButton ID="btnFutureStep" Location="ImageCDNPath" runat="server" OnClick="btnBookingStepItem_Click"
                ImageUrl="~/images/space.gif" Enabled="false" />
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
