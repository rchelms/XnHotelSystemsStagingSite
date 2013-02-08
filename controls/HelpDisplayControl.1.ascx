<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HelpDisplayControl.1.ascx.cs"
    Inherits="HelpDisplayControl" %>
<xnc:XImageButton ID="ibHelpInfo" CssClass="help_info_icon" runat="server" Location="ImageCDNPath" ImageUrl="~/images/booking-help-icon.gif"
    meta:resourcekey="ibHelpInfo" />
  
<!-- Help Info pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_HelpInfoPopup" style="width: 600px;
    display: none; overflow: hidden;">
    <div class="highslide-header">
        <ul>
            <li class="highslide-close">
                <xnc:XImageButton ID="btnClose1" runat="server" Location="ImageCDNPath" ImageUrl="~/images/close.gif" OnClientClick="return hs.close(this);"
                    meta:resourcekey="ibClosePopup" />
            </li>
        </ul>
    </div>
    <div class="highslide-body">
        <asp:Panel CssClass="help_info_popup xngr_wbs_popup" runat="server">
            <asp:Label ID="lblHelpInfoMessage" runat="server" Text="" />
        </asp:Panel>
    </div>
</div>
