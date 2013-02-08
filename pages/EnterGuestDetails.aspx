<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="EnterGuestDetails.aspx.cs" Inherits="EnterGuestDetails"
    Title="Enter Guest Details" %>

<%@ Reference Control="~/controls/ProfileLoginNameControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/ErrorDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingStepControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingStepItemControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingSummaryControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingSummaryRoomItemControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingSummaryAddOnPackageItemControl.1.ascx" %>
<%@ Reference Control="~/controls/GuestDetailsEntryControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeItemControl.1.ascx" %>
<asp:Content ID="cpgEnterGuestDetails" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:Panel runat="server">
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phBookingStepControl" runat="server" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server">
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phProfileLoginNameControl" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phLanguageSelectorControl" runat="server" />
        </asp:Panel>
        <asp:Panel CssClass="content_section" runat="server">
            <asp:Panel CssClass="pane_left" runat="server">
                <asp:Panel CssClass="pane_right" runat="server">
                    <asp:Panel CssClass="pane_center" runat="server">
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="pane_body" runat="server">
                <h3>
                    <asp:Label ID="lblBookingInfo" runat="server" Text="" meta:resourcekey="lblBookingInfo" /></h3>
                <p>
                    <asp:Label ID="lblBookingInfoInstructions" runat="server" Text="" meta:resourcekey="lblBookingInfoInstructions" /></p>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phErrorDisplayControl" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phBookingSummaryControl" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phGuestDetailsEntryControl" runat="server" />
        </asp:Panel>
    </asp:Panel>
    <asp:PlaceHolder ID="phTrackingCodeControl" runat="server" />
</asp:Content>
