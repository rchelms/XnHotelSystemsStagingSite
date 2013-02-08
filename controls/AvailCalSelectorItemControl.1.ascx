<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AvailCalSelectorItemControl.1.ascx.cs"
    Inherits="AvailCalSelectorItemControl" %>
<%@ Reference Control="~/controls/SpecialRatesIndicatorControl.1.ascx" %>
<asp:Panel ID="panAvailCalItem" CssClass="mm_avail_cal_day" runat="server" onclick="me=this;setTimeout(function(){availcal_click(me);}, 100);" ontouchend="availcal_touch(this);"
    onmouseover="availcal_mouseover(this);">
    <asp:Panel ID="panActiveAvailCalItem" runat="server">
        <asp:Panel CssClass="special_rates_indicator" runat="server">
            <asp:PlaceHolder ID="phSpecialRatesIndicator" runat="server" />
        </asp:Panel>
        <asp:Label CssClass="mama_avail_cal_day_rate" ID="lbViewCalRates" runat="server"
            meta:resourcekey="lbViewCalRates"></asp:Label>
    </asp:Panel>

    <asp:Label ID="lblAvailCalItemDay" CssClass="mama_avail_cal_day_label" runat="server"></asp:Label>
    <asp:HiddenField ID="hdfAvailCalDayDate" runat="server" />
    <asp:CheckBox ID="cbAvailCalItemDaySelected" runat="server" CssClass="mm_hidden"/>
    <asp:Label CssClass="mama_avail_cal_day_label_checkin" ID="lblCheckIn" runat="server"
        meta:resourcekey="lblCheckIn"></asp:Label>
    <asp:Label CssClass="mama_avail_cal_day_label_checkout" ID="lblChecOut" runat="server"
        meta:resourcekey="lblCheckOut"></asp:Label>
    <asp:Label CssClass="mama_avail_cal_day_label_soldout" ID="lblSoldOut" runat="server" meta:resourcekey="lblSoldOut"></asp:Label>
</asp:Panel>
<!-- "View Calendar Rates" pop-up markup -->
<div class="highslide-html-content" id="<%= this.ClientID %>_ViewCalRatesPopup" style="width: 575px;
    display: none; overflow: hidden;">
    <div class="highslide-header">
        <ul>
            <li class="highslide-close">
                <xnc:XImageButton ID="btnClose1" runat="server" Location="ImageCDNPath" ImageUrl="~/images/close.gif"
                    OnClientClick="return hs.close(this);" meta:resourcekey="iblClosePopup" />
            </li>
        </ul>
    </div>
    <div class="highslide-body">
        <asp:Panel CssClass="avail_cal_rates_popup xngr_wbs_popup" runat="server">
            <asp:Panel CssClass="avail_cal_rates_popup_header" runat="server">
                <asp:Panel CssClass="avail_cal_rates_popup_header_title" runat="server">
                    <asp:Label ID="lblViewRatesTitle" runat="server" Text="" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="avail_cal_rates_popup_body" runat="server">
                <asp:PlaceHolder ID="phViewCalRatesDetails" runat="server" />
            </asp:Panel>
        </asp:Panel>
    </div>
</div>
