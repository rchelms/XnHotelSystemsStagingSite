<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoomRateSelectorControl.1.ascx.cs"
    Inherits="RoomRateSelectorControl" %>
<%@ Reference Control="~/controls/RoomSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomTypeSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RatePlanSelectorItemControl.1.ascx" %>
<asp:Panel ID="panRoomRateSelector" CssClass="content_section" runat="server">
    <asp:UpdatePanel ID="formUpdatePanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnContinue1" />
            <asp:PostBackTrigger ControlID="btnContinue2" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel CssClass="pane_left" runat="server">
                <asp:Panel CssClass="pane_right" runat="server">
                    <asp:Panel CssClass="pane_center" runat="server">
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="pane_body" runat="server">
                <asp:Panel ID="panRoomSelectorMenuBar" CssClass="tab_nav_bar" runat="server">
                    <asp:PlaceHolder ID="phRoomSelectors" runat="server" />
                </asp:Panel>
                <asp:Panel ID="panRoomInfo" CssClass="room_info_content_section" runat="server">
                    <asp:Panel CssClass="room_header" runat="server">
                        <p>
                            <asp:Label ID="lblRoomInfoText" runat="server" Text="" /></p>
                        <p>
                            <asp:Label ID="lblRoomInfoInstructions1" runat="server" Text="" meta:resourcekey="lblRoomInfoInstructions1" /></p>
                        <p>
                            <asp:Label ID="lblRoomInfoInstructions2" runat="server" Text="" meta:resourcekey="lblRoomInfoInstructions2" /></p>
                    </asp:Panel>
                    <asp:Panel ID="panContinue1" CssClass="field_set_button" runat="server">
                        <asp:Panel CssClass="std_button" runat="server">
                            <asp:Panel CssClass="std_button_left" runat="server">
                                <asp:Panel CssClass="std_button_right" runat="server">
                                    <asp:Panel CssClass="std_button_center" runat="server">
                                        <asp:Button ID="btnContinue1" CssClass="std_button_control" runat="server" Text=""
                                            meta:resourcekey="btnContinue" OnClick="btnContinue_Click" />
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panRoomTypes" runat="server">
                        <asp:PlaceHolder ID="phRoomTypes" runat="server" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panContinue2" CssClass="field_set_button" runat="server">
                    <asp:Panel CssClass="std_button" runat="server">
                        <asp:Panel CssClass="std_button_left" runat="server">
                            <asp:Panel CssClass="std_button_right" runat="server">
                                <asp:Panel CssClass="std_button_center" runat="server">
                                    <asp:Button ID="btnContinue2" CssClass="std_button_control" runat="server" Text=""
                                        meta:resourcekey="btnContinue" OnClick="btnContinue_Click" />
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
