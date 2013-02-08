<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
   CodeFile="CheckCancelDetails.aspx.cs" Inherits="CheckCancelDetails" Title="Check Cancel Details" %>

<asp:Content ID="cpgCheckCancelDetails" ContentPlaceHolderID="cphBody" runat="Server">
   <asp:Panel runat="server">
      <asp:Panel CssClass="content_section" runat="server">
         <asp:Panel CssClass="pane_left" runat="server">
            <asp:Panel CssClass="pane_right" runat="server">
               <asp:Panel CssClass="pane_center" runat="server">
               </asp:Panel>
            </asp:Panel>
         </asp:Panel>
         <asp:Panel CssClass="pane_body" runat="server">
            <h3>
               <asp:Label ID="lblCheckCancelDetails" runat="server" Text="" meta:resourcekey="lblCheckCancelDetails" /></h3>
            <p>
               <asp:Label ID="lblCheckCancelDetailsComments" runat="server" Text="" meta:resourcekey="lblCheckCancelDetailsComments" /></p>
         </asp:Panel>
      </asp:Panel>
   </asp:Panel>
</asp:Content>
