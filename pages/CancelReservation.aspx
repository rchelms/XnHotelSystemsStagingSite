<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
   CodeFile="CancelReservation.aspx.cs" Inherits="CancelReservation" Title="Cancel Reservation" %>

<%@ Reference Control="~/controls/ErrorDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/CancelDetailsEntryControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RemoteContentContainer.1.ascx" %>
<asp:Content ID="cpgCancelReservation" ContentPlaceHolderID="cphBody" runat="Server">
   <asp:Panel ID="Panel1" runat="server">
      <asp:UpdatePanel ID="udpContentPage" runat="server" UpdateMode="Always">
         <ContentTemplate>
            <div class="mm_colLeft">
               <asp:Panel runat="server">
                  <asp:PlaceHolder ID="phCancelDetailsEntryControl" runat="server" />
               </asp:Panel>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="mm_colRight">
               <asp:Panel ID="Panel3" runat="server">
                  <asp:PlaceHolder ID="phErrorDisplayControl" runat="server" />
               </asp:Panel>
               <asp:PlaceHolder ID="phRemoteContentContainerControl" runat="server" />
            </asp:Panel>
            <asp:PlaceHolder ID="phTrackingCodeControl" runat="server" />
         </ContentTemplate>
      </asp:UpdatePanel>
   </asp:Panel>
   
</asp:Content>
