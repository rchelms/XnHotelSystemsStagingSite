<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AvailCalSelectorControl.1.ascx.cs"
    Inherits="AvailCalSelectorControl" %>
<%@ Reference Control="~/controls/AvailCalSelectorItemControl.1.ascx" %>
<asp:Panel ID="panAvailCalSelector" runat="server" CssClass="mm_content_section">
    <asp:UpdatePanel ID="formUpdatePanel" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnViewRates" />
            <asp:AsyncPostBackTrigger ControlID = "btnEdit" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="panAvailCalViewInfo" CssClass="mm_main_step" runat="server">
                <asp:Panel ID="panAvailCalViewInfoCheckIn" CssClass="mm_step mm_background_info mm_border_info" runat="server">
                    <asp:Label ID="lblCheckinInfo" runat="server" meta:resourcekey="lblCheckinInfo" CssClass="mm_text_info"></asp:Label>
                    <asp:Panel ID="panAvailCalViewEditButton" CssClass="mm_wrapper_button_edit" runat="server">
                        <asp:Button ID="btnEdit" CssClass="mm_button mm_text_button_edit" runat="server" OnClick="lbViewAvailCal_Click" OnClientClick="return btnEdit_click();" Text="<%$ Resources:SiteResources, Edit %>"/>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panAvailCalViewInfoChekOut" CssClass="mm_step mm_background_info mm_border_info" runat="server">
                    <asp:Label runat="server" ID="lblCheckoutInfo" meta:resourcekey="lblCheckoutInfo" CssClass="mm_text_info"></asp:Label>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panAvailCalControl" CssClass="mm_main_step" runat="server">
                <asp:Panel CssClass="mm_step mm_background_edit mm_border_edit" runat="server">
                    <asp:Label runat="server" ID="lblAvailCalInstructionMessage" meta:resourcekey="lblAvailCalSelectorEditInfo" CssClass="mm_text_edit"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="panAvailCal" CssClass="mm_avail_cal" runat="server">
                    <asp:HiddenField ID="hdfSelectedCheckinDate" runat="server" />
                    <asp:HiddenField ID="hdfSelectedCheckoutDate" runat="server" />
                    <asp:Table ID="tblAvalCal" runat="server" CellSpacing="0">
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" ColumnSpan="7">
                                <asp:Panel CssClass="mm_wrapper_month_header" runat="server">
                                    <asp:Panel ID="panAvailCalNavigatePrev" CssClass="mm_wrapper_navigate mm_wrapper_navigate_left"
                                        runat="server">
                                        <asp:Button ID="btnNavigatePrev" runat="server" CssClass="mm_button_navigate" OnClick="btnNavigatePrev_Click" OnClientClick="showWaitingPage();"/>
                                    </asp:Panel>
                                    <asp:Panel ID="panAvailCalNavigateNext" CssClass="mm_wrapper_navigate mm_wrapper_navigate_right"
                                        runat="server">
                                        <asp:Button ID="btnNavigateNext" runat="server" CssClass="mm_button_navigate" OnClick="btnNavigateNext_Click" OnClientClick="showWaitingPage();"/>
                                    </asp:Panel>
                                    <asp:Label CssClass="mm_month_header_label" ID="lblAvailCalMonth" runat="server"
                                        Text="" />
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableHeaderCell runat="server">
                                <asp:Panel CssClass="mm_day_of_week_header" runat="server">
                                    <asp:Label ID="lblDayOfWeek1" runat="server" Text="" meta:resourcekey="lblDayOfWeek1" />
                                </asp:Panel>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server">
                                <asp:Panel CssClass="mm_day_of_week_header" runat="server">
                                    <asp:Label ID="lblDayOfWeek2" runat="server" Text="" meta:resourcekey="lblDayOfWeek2" />
                                </asp:Panel>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server">
                                <asp:Panel CssClass="mm_day_of_week_header" runat="server">
                                    <asp:Label ID="lblDayOfWeek3" runat="server" Text="" meta:resourcekey="lblDayOfWeek3" />
                                </asp:Panel>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server">
                                <asp:Panel CssClass="mm_day_of_week_header" runat="server">
                                    <asp:Label ID="lblDayOfWeek4" runat="server" Text="" meta:resourcekey="lblDayOfWeek4" />
                                </asp:Panel>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server">
                                <asp:Panel CssClass="mm_day_of_week_header" runat="server">
                                    <asp:Label ID="lblDayOfWeek5" runat="server" Text="" meta:resourcekey="lblDayOfWeek5" />
                                </asp:Panel>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server">
                                <asp:Panel CssClass="mm_day_of_week_header" runat="server">
                                    <asp:Label ID="lblDayOfWeek6" runat="server" Text="" meta:resourcekey="lblDayOfWeek6" />
                                </asp:Panel>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server">
                                <asp:Panel CssClass="mm_day_of_week_header" runat="server">
                                    <asp:Label ID="lblDayOfWeek7" runat="server" Text="" meta:resourcekey="lblDayOfWeek7" />
                                </asp:Panel>
                            </asp:TableHeaderCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell ID="tdDay1" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay2" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay3" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay4" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay5" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay6" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay7" runat="server">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell ID="tdDay8" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay9" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay10" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay11" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay12" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay13" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay14" runat="server">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell ID="tdDay15" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay16" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay17" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay18" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay19" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay20" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay21" runat="server">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell ID="tdDay22" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay23" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay24" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay25" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay26" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay27" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay28" runat="server">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell ID="tdDay29" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay30" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay31" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay32" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay33" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay34" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay35" runat="server">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell ID="tdDay36" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay37" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay38" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay39" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay40" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay41" runat="server">&nbsp;</asp:TableCell>
                            <asp:TableCell ID="tdDay42" runat="server">&nbsp;</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
                <asp:Panel ID="panViewRates" CssClass="mm_avail_cal_price_information" runat="server">
                    <asp:Label ID="lblLegendInfo1" runat="server" CssClass="mm_avail_cal_legend_text"
                        meta:resourcekey="lblLegendInfo1"></asp:Label>
                    <asp:Label ID="lblLegendInfo2" runat="server" CssClass="mm_avail_cal_legend_text"
                        meta:resourcekey="lblLegendInfo2"></asp:Label>
                    <asp:Button ID="btnViewRates" CssClass="mm_avail_cal_submit_button" runat="server"
                        OnClick="btnViewRates_Click" />
                </asp:Panel>
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hdfAvailCalDayCheckInDate" />
            <asp:HiddenField runat="server" ID="hdfAvailCalDayCheckOutDate" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
