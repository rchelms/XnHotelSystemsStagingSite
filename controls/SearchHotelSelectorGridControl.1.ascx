<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchHotelSelectorGridControl.1.ascx.cs"
    Inherits="SearchHotelSelectorGridControl" %>
<%@ Reference Control="~/controls/RateGridHeaderItemControl.1.ascx" %>
<%@ Reference Control="~/controls/SearchHotelSelectorGridItemControl.1.ascx" %>
<asp:UpdatePanel ID="formUpdatePanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="panSearchHotelGridItems" CssClass="content_section" runat="server">
            <asp:Panel CssClass="pane_left" runat="server">
                <asp:Panel CssClass="pane_right" runat="server">
                    <asp:Panel CssClass="pane_center" runat="server">
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="pane_body" runat="server">
                <asp:Panel CssClass="rate_grid_nav" runat="server">
                    <asp:Panel ID="panRateGridPrevious" CssClass="rate_grid_nav_previous" runat="server">
                        <asp:LinkButton ID="btnRateGridPrevious" runat="server" Text="" OnClick="btnRateGridPrevious_Click"
                            meta:resourcekey="btnRateGridPrevious" />
                    </asp:Panel>
                    <asp:Panel ID="panRateGridNext" CssClass="rate_grid_nav_next" runat="server">
                        <asp:LinkButton ID="btnRateGridNext" runat="server" Text="" OnClick="btnRateGridNext_Click"
                            meta:resourcekey="btnRateGridNext" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="rate_grid_header" runat="server">
                    <asp:Panel CssClass="rate_grid_header_info" runat="server">
                        <asp:Panel CssClass="rate_grid_header_info_area" runat="server">
                            <asp:Label ID="lblAreaInfoText" runat="server" Text="" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="rate_grid_header_spacer" runat="server">
                    </asp:Panel>
                    <asp:Panel CssClass="rate_grid_header_dates" runat="server">
                        <asp:PlaceHolder ID="phRateGridHeaderItems" runat="server" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="rate_grid_hotels" runat="server">
                    <asp:PlaceHolder ID="phRateGridHotelItems" runat="server" />
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
