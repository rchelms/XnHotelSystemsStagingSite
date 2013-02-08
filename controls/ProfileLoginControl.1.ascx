<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProfileLoginControl.1.ascx.cs"
    Inherits="ProfileLoginControl" %>
<asp:Panel ID="panProfileLogin" runat="server">
    <asp:UpdatePanel ID="formUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panProfileViewLoginLink" CssClass="login_content_section" runat="server">
                <asp:Panel CssClass="login_pane_left" runat="server">
                    <asp:Panel CssClass="login_pane_right" runat="server">
                        <asp:Panel CssClass="login_pane_center" runat="server">
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="login_pane_body" runat="server">
                    <asp:Panel CssClass="login_link_view" runat="server">
                        <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/login-icon.gif" />
                        <asp:LinkButton ID="lbViewLoginForm" runat="server" Text="" ToolTip="" OnClick="lbViewLoginForm_Click"
                            meta:resourcekey="lbViewLoginForm" />
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panProfileViewLoginForm" CssClass="login_content_section" runat="server">
                <asp:Panel CssClass="login_pane_left" runat="server">
                    <asp:Panel CssClass="login_pane_right" runat="server">
                        <asp:Panel CssClass="login_pane_center" runat="server">
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="login_pane_body" runat="server">
                    <asp:Panel ID="panProfileLoginForm" CssClass="login_form" runat="server">
                        <asp:Panel CssClass="login_form_header" runat="server">
                            <h3>
                                <asp:Label ID="lblProfileLoginHeader" runat="server" Text="" meta:resourcekey="lblProfileLoginHeader" />
                            </h3>
                            <asp:Panel ID="panProfileLoginFormErrors" CssClass="login_form_errors" runat="server">
                                <p>
                                    <asp:Label ID="lblLoginErrorList" runat="server" Text="" />
                                </p>
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel CssClass="login_form_body" runat="server">
                            <asp:Panel CssClass="field_set_row" runat="server">
                                <asp:Panel CssClass="field_set_left" runat="server">
                                    <asp:Panel CssClass="field_set_label_inline" runat="server">
                                        <asp:Label ID="lblLoginName" runat="server" Text="" meta:resourcekey="lblLoginName" />
                                    </asp:Panel>
                                    <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                        <asp:TextBox ID="tbLoginName" runat="server" />
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_row" runat="server">
                                <asp:Panel CssClass="field_set_left" runat="server">
                                    <asp:Panel CssClass="field_set_label_inline" runat="server">
                                        <asp:Label ID="lblLoginPassword" runat="server" Text="" meta:resourcekey="lblLoginPassword" />
                                    </asp:Panel>
                                    <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                        <asp:TextBox ID="tbLoginPassword" runat="server" TextMode="Password" />
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_button" runat="server">
                                <asp:Panel ID="panLoginButton" CssClass="std_button" runat="server">
                                    <asp:Panel CssClass="std_button_left" runat="server">
                                        <asp:Panel CssClass="std_button_right" runat="server">
                                            <asp:Panel CssClass="std_button_center" runat="server">
                                                <asp:Button ID="btnLogin" CssClass="std_button_control" runat="server" Text="" meta:resourcekey="btnLogin"
                                                    OnClick="btnLogin_Click" />
                                            </asp:Panel>
                                        </asp:Panel>
                                    </asp:Panel>
                                </asp:Panel>
                                <asp:Panel ID="panCancelLoginButton" CssClass="std_button" runat="server">
                                    <asp:Panel CssClass="std_button_left" runat="server">
                                        <asp:Panel CssClass="std_button_right" runat="server">
                                            <asp:Panel CssClass="std_button_center" runat="server">
                                                <asp:Button ID="btnCancelLogin" CssClass="std_button_control" runat="server" Text=""
                                                    meta:resourcekey="btnCancelLogin" OnClick="btnCancelLogin_Click" />
                                            </asp:Panel>
                                        </asp:Panel>
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panProfileViewLogOutLink" CssClass="login_content_section" runat="server">
                <asp:Panel CssClass="login_pane_left" runat="server">
                    <asp:Panel CssClass="login_pane_right" runat="server">
                        <asp:Panel CssClass="login_pane_center" runat="server">
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="login_pane_body" runat="server">
                    <asp:Panel CssClass="login_link_view" runat="server">
                        <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/login-icon.gif" />
                        <asp:LinkButton ID="lbLogout" runat="server" Text="" meta:resourcekey="btnLogout"
                            OnClick="btnLogout_Click" />
                        <asp:Label ID="lblLoginInfo" runat="server" Text="" />
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
