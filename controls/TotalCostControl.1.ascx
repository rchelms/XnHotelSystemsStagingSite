<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TotalCostControl.1.ascx.cs"
    Inherits="TotalCostControl" %>
<asp:Panel ID="Panel1" runat="server" CssClass="mm_content_section">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnHold" EventName="Click" />--%>
            <asp:AsyncPostBackTrigger ControlID="btnPayNow" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel runat="server" ID="panTotalCostInfo" CssClass="mm_main_step mm_step mm_background_info mm_border_info">
                <asp:Label ID="lblTotalText" runat="server" meta:resourcekey="lblTotalText" CssClass="mm_text_info"></asp:Label>
                <asp:Label ID="lblTotalCost" runat="server" CssClass="mm_roomrate_price"></asp:Label>
            </asp:Panel>
            <asp:Panel runat="server" ID="panTempTotalCostContent" CssClass="mm_main_step mm_step mm_background_info mm_border_info">
                <asp:Label runat="server" ID="lblTempTotalText" meta:resourcekey="lblTempTotalText"
                    CssClass="mm_text_info"></asp:Label>
                <asp:Label runat="server" ID="lblTempTotalCost" CssClass="mm_roomrate_price"></asp:Label>
            </asp:Panel>
            <asp:Panel runat="server" ID="panSummaryBooking" CssClass="mm_content_section">
                <%--<div class="mm_background_edit mm_border_edit mm_wrapper_final">
                    <div class="mm_position_panel_title">
                        <asp:Label runat="server" ID="lblHoldBookingTitle" Text="Need some time?" CssClass="mm_text_title mm_text_title_hold_booking"></asp:Label></div>
                    <div class="mm_position_panel_button">
                        <asp:Panel runat="server" ID="panHoldButtonWrapper" CssClass="mm_wrapper_payment_button mm_wrapper_hold_button">
                            <asp:Label runat="server" ID="lblHoldButtonText" Text="Hold now" CssClass="mm_text_postion_button"></asp:Label>
                        </asp:Panel>
                    </div>
                    <div class="mm_position_panel_info">
                        <asp:Label runat="server" ID="lblHoldMessage" CssClass="mm_text_payment_info" Text="Hold until"></asp:Label>
                        <asp:Label runat="server" ID="lblHoldDate" CssClass="mm_text_payment_info_extra_info"
                            Text="5pm Sunday 6 November 2011"></asp:Label>
                        <asp:Label runat="server" ID="lblHoldPrice" CssClass="mm_text_price_hold" Text="5 EUR"></asp:Label></div>
                    <asp:Button runat="server" ID="btnHold" CssClass="mm_proceed_payment_button"/>
                </div>--%>
                <div class="mm_background_info mm_border_info mm_wrapper_final mm_text_info">
                    <div class="mm_position_panel_title">
                        <asp:Label runat="server" ID="lblPayNowTitle" meta:resourcekey="lblTotalText" CssClass="mm_text_title mm_text_title_paynow"></asp:Label></div>
                 
                        <div class="mm_position_panel_button">
                        <asp:Panel runat="server" ID="panPayButtonWrapper" CssClass="mm_wrapper_payment_button mm_wrapper_paynow_button">
                            <asp:Label runat="server" ID="lblPayNow" meta:resourcekey="lblPaynowText" CssClass="mm_text_postion_button"></asp:Label><br />
                             <asp:Label runat="server" ID="lblPayNow1" meta:resourcekey="lblPaynowText1" CssClass="mm_text_postion_button1"></asp:Label>
                        </asp:Panel>
                    </div>
                    <div class="mm_position_panel_info">
                        <asp:Label runat="server" ID="lblGrandTotal" CssClass="mm_text_price_pay_now"></asp:Label></div>
                    <asp:Button runat="server" ID="btnPayNow" CssClass="mm_proceed_payment_button" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
