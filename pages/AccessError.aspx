<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="AccessError.aspx.cs" Inherits="AccessError" Title="Page Access Error" %>

<asp:Content ID="cpgPagingError" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:Panel runat="server">
        <asp:Panel CssClass="content_section" runat="server">
            <asp:Panel CssClass="pane_left" runat="server">
                <asp:Panel CssClass="pane_right" runat="server">
                    <asp:Panel CssClass="pane_center" runat="server">
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="pane_body" runat="server">
                <asp:Panel CssClass="access_error" runat="server">
                    <asp:Panel CssClass="access_error_img" runat="server">
                        <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/symbol_error.gif" />
                    </asp:Panel>
                    <asp:Panel CssClass="access_error_instructions" runat="server">
                        <h3>
                            <asp:Label ID="lblAccessErrorTitle" runat="server" Text="" meta:resourcekey="lblAccessErrorTitle" /></h3>
                        <p>
                            <asp:Label ID="lblAccessError1" runat="server" Text="" meta:resourcekey="lblAccessError1" /></p>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="field_set_button" runat="server">
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
    </asp:Panel>
</asp:Content>
