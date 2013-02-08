<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TrackingCodeItemControl.1.ascx.cs"
   Inherits="TrackingCodeItemControl" %>
<asp:UpdatePanel runat="server">
   <ContentTemplate>
      <asp:Literal ID="litTrackingCodeScript" runat="server" Text="" />
      <asp:Panel runat="server" ID="ctnAdword"></asp:Panel>
   </ContentTemplate>
</asp:UpdatePanel>
