<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
   CodeFile="SelectCancelRoom.aspx.cs" Inherits="SelectCancelRoom" Title="Select Cancel Room" %>

<%@ Reference Control="~/controls/ErrorDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/LanguageSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/ConfirmationControl.1.ascx" %>
<%@ Reference Control="~/controls/StayCriteriaSelectorControl.MM.ascx" %>
<%@ Reference Control="~/controls/ImageHoldingControl.1.ascx" %>
<%@ Reference Control="~/controls/AvailCalSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomQuantitySelectorControl.ascx" %>
<%@ Reference Control="~/controls/CancelRoomSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/CancelRoomSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/CancelAddOnPackageItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RemoteContentContainer.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeItemControl.1.ascx" %>
<asp:Content ID="cpgSelectCancelRoom" ContentPlaceHolderID="cphBody" runat="Server">
   <asp:UpdatePanel runat="server">
      <ContentTemplate>
         <asp:Panel runat="server" class="mm_colLeft">
            <asp:PlaceHolder ID="phLanguageSelectorControl" runat="server" />
            <asp:PlaceHolder ID="phConfirmationControl" runat="server" />
            <asp:PlaceHolder ID="phStayCriteriaSelectorControl" runat="server" />
            <asp:PlaceHolder ID="phAvailCalSelectorControl" runat="server" />
            <asp:PlaceHolder ID="phRoomQuantitySelectorControl" runat="server" />
            <asp:PlaceHolder ID="phCancelRoomSelectorControl" runat="server" />
         </asp:Panel>
         <asp:Panel runat="server" CssClass="mm_colRight">
            <asp:PlaceHolder ID="phErrorDisplayControl" runat="server" />
            <asp:PlaceHolder ID="phRemoteContentContainerControl" runat="server" />
            <asp:Panel runat="server" ID="panBookingCancelled">
               <asp:Label runat="server" meta:resourcekey="lblConfirmCancelMessage1" CssClass="mm_text_cancellation mm_text_bold"></asp:Label>
               <asp:Label runat="server" ID="lblCancelConfirmNumber" CssClass="mm_text_cancellation mm_text_x_strong"></asp:Label>
               <asp:Label runat="server" meta:resourcekey="lblConfirmCancelMessage2" CssClass="mm_text_cancellation mm_text_bold"></asp:Label>
            </asp:Panel>
         </asp:Panel>
         <asp:PlaceHolder ID="phTrackingCodeControl" runat="server" />
      </ContentTemplate>
   </asp:UpdatePanel>
</asp:Content>
