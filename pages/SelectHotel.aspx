<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="SelectHotel.aspx.cs" Inherits="SelectHotel" Title="Select Hotel" %>

<%@ Reference Control="~/controls/ProfileLoginControl.1.ascx" %>
<%@ Reference Control="~/controls/ProfileLoginNameControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/ErrorDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/RestrictionDateDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingStepControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingStepItemControl.1.ascx" %>
<%@ Reference Control="~/controls/StayCriteriaSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeItemControl.1.ascx" %>
<asp:Content ID="cpgSelectHotel" ContentPlaceHolderID="cphBody" runat="Server">
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
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phProfileLoginControl" runat="server" />
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
                    <asp:Label ID="lblSelectHotel" runat="server" Text="" meta:resourcekey="lblSelectHotel" /></h3>
                <p>
                    <asp:Label ID="lblSelectHotelInstructions" runat="server" Text="" meta:resourcekey="lblSelectHotelInstructions" /></p>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phErrorDisplayControl" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phRestrictionDateDisplayControl" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server">
            <asp:PlaceHolder ID="phStayCriteriaControl" runat="server"/>
        </asp:Panel>
    </asp:Panel>
    <asp:PlaceHolder ID="phTrackingCodeControl" runat="server" />
</asp:Content>
