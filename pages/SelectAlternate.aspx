<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
   CodeFile="SelectAlternate.aspx.cs" Inherits="SelectAlternate" Title="Select Alternate" %>

<%@ Reference Control="~/controls/ProfileLoginNameControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/ErrorDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingStepControl.1.ascx" %>
<%@ Reference Control="~/controls/BookingStepItemControl.1.ascx" %>
<%@ Reference Control="~/controls/StayCriteriaSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/AvailCalSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/AvailCalSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/AlternateHotelSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/AlternateHotelSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/AlternateHotelRatePlanItemControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeItemControl.1.ascx" %>
<asp:Content ID="cpgSelectAlternate" ContentPlaceHolderID="cphBody" runat="Server">
   <asp:UpdatePanel runat="server">
      <ContentTemplate>
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
                     <asp:Label ID="lblSelectAlternate" runat="server" Text="" meta:resourcekey="lblSelectAlternate" /></h3>
                  <p>
                     <asp:Label ID="lblSelectAlternateInstructions" runat="server" Text="" meta:resourcekey="lblSelectAlternateInstructions" /></p>
               </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server">
               <asp:PlaceHolder ID="phErrorDisplayControl" runat="server" />
            </asp:Panel>
            <asp:Panel runat="server">
               <asp:PlaceHolder ID="phStayCriteriaControl" runat="server" />
            </asp:Panel>
            <asp:Panel ID="panAvailCalSelectorControl" runat="server">
               <asp:PlaceHolder ID="phAvailCalSelectorControl" runat="server" />
            </asp:Panel>
            <asp:Panel ID="panAlternateHotelSelectorControl" runat="server">
               <asp:PlaceHolder ID="phAlternateHotelSelectorControl" runat="server" />
            </asp:Panel>
         </asp:Panel>
         <asp:PlaceHolder ID="phTrackingCodeControl" runat="server" />
      </ContentTemplate>
   </asp:UpdatePanel>
</asp:Content>
