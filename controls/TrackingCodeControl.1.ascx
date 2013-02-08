<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TrackingCodeControl.1.ascx.cs"
   Inherits="TrackingCodeControl" %>
<%@ Reference Control="~/controls/TrackingCodeItemControl.1.ascx" %>
<asp:UpdatePanel runat="server">
   <ContentTemplate>
      <asp:PlaceHolder ID="phTrackingCodeItems" runat="server" />
   </ContentTemplate>
</asp:UpdatePanel>
