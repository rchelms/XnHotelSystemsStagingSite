<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DeepLinkPreProcess.aspx.cs" Inherits="DeepLinkPreProcess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
   <script type="text/javascript">
      var r = confirm("<%= GetGlobalResourceObject("JSResources", "StartOverConfirmation")%>");
      if (r) {location.replace("Deeplink.aspx" + location.search + "&is=1");} 
      else 
         location.replace("Default.aspx");
   </script>
</asp:Content>

