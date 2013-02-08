<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddOnPackageSelectorControl.1.ascx.cs"
    Inherits="AddOnPackageSelectorControl" %>
<%@ Reference Control="~/controls/RoomSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/AddOnPackageSelectorItemControl.1.ascx" %>
<asp:Panel ID="panAddOnPackageSelector" CssClass="content_section" runat="server">
    <asp:UpdatePanel ID="formUpdatePanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnContinue" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel CssClass="pane_left" runat="server">
                <asp:Panel CssClass="pane_right" runat="server">
                    <asp:Panel CssClass="pane_center" runat="server">
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="pane_body" runat="server">
                <asp:Panel ID="panRoomSelectorMenuBar" CssClass="tab_nav_bkgd" runat="server">
                    <asp:Panel CssClass="tab_nav_bar" runat="server">
                        <asp:PlaceHolder ID="phRoomSelectors" runat="server" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panPackageInfo" CssClass="addon_packages_content_section" runat="server">
                    <asp:Panel ID="panNoPackageInfo" runat="server">
                        <h4>
                            <asp:Label ID="lblNoPackageInfo" runat="server" Text="" meta:resourcekey="lblNoPackageInfo" /></h4>
                    </asp:Panel>
                    <asp:Panel ID="panIsPackageInfo" runat="server">
                        <asp:Panel CssClass="addon_package_desc_header addon_package_header" runat="server">
                            <asp:Label ID="lblRoomInfoText" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel CssClass="addon_package_price_header addon_package_header" runat="server">
                            <asp:Label ID="lblPackagePrice" runat="server" Text="" meta:resourcekey="lblPackagePrice" />
                        </asp:Panel>
                        <asp:Panel CssClass="addon_package_quantity_header addon_package_header" runat="server">
                            <asp:Label ID="lblPackageQuantity" runat="server" Text="" meta:resourcekey="lblPackageQuantity" />
                        </asp:Panel>
                        <asp:Panel CssClass="addon_package_select_header addon_package_header" runat="server">
                            <asp:Label ID="lblPackageSelect" runat="server" Text="" meta:resourcekey="lblPackageSelect" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panAddonPackages" runat="server">
                        <asp:PlaceHolder ID="phAddonPackages" runat="server" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panContinue" CssClass="field_set_button" runat="server">
                    <asp:Panel CssClass="std_button" runat="server">
                        <asp:Panel CssClass="std_button_left" runat="server">
                            <asp:Panel CssClass="std_button_right" runat="server">
                                <asp:Panel CssClass="std_button_center" runat="server">
                                    <asp:Button ID="btnContinue" CssClass="std_button_control" runat="server" Text=""
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
