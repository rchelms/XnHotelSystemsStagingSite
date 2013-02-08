<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="SelectRoom.aspx.cs" Inherits="SelectRoom" Title="Select Room" %>

<%@ Reference Control="~/controls/ProfileLoginNameControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/ErrorDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/RestrictionDateDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingStepControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingStepItemControl.1.ascx" %>
<%@ Reference Control="~/controls/StayCriteriaSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/AvailCalSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/AvailCalSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomRateSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomTypeSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RatePlanSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomRateSelectorGridControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomTypeSelectorGridItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RatePlanSelectorGridItemControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeItemControl.1.ascx" %>
<asp:Content ID="cpgSelectRoom" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:Panel runat="server">
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phBookingStepControl" runat="server" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server">
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phProfileLoginNameControl" runat="server" />
        </asp:Panel>
        <asp:Panel ID="panLanguageSelectorControl" runat="server">
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
                    <asp:Label ID="lblSelectRoom" runat="server" Text="" meta:resourcekey="lblSelectRoom" /></h3>
                <p>
                    <asp:Label ID="lblSelectRoomInstructions" runat="server" Text="" meta:resourcekey="lblSelectRoomInstructions" /></p>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phErrorDisplayControl" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phRestrictionDateDisplayControl" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phStayCriteriaControl" runat="server" />
        </asp:Panel>
        <asp:Panel ID="panAvailCalSelectorControl" runat="server">
            <asp:PlaceHolder ID="phAvailCalSelectorControl" runat="server" />
        </asp:Panel>
        <asp:Panel ID="panRoomRateSelectorControl" runat="server">
            <asp:PlaceHolder ID="phRoomRateSelectorControl" runat="server" />
        </asp:Panel>
    </asp:Panel>
    <asp:PlaceHolder ID="phTrackingCodeControl" runat="server" />
</asp:Content>
