<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="AppError.aspx.cs" Inherits="AppError" Title="Application Error" %>

<asp:Content ID="cpgAppError" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:Panel runat="server">
        <asp:Panel CssClass="content_section" runat="server">
            <asp:Panel CssClass="pane_left" runat="server">
                <asp:Panel CssClass="pane_right" runat="server">
                    <asp:Panel CssClass="pane_center" runat="server">
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="pane_body" runat="server">
                <asp:Panel CssClass="app_error" runat="server">
                    <asp:Panel CssClass="app_error_img" runat="server">
                        <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/symbol_error.gif" />
                    </asp:Panel>
                    <asp:Panel CssClass="app_error_instructions" runat="server">
                        <h3>
                            <asp:Label ID="lblAppErrorTitle" runat="server" Text="" meta:resourcekey="lblAppErrorTitle" /></h3>
                        <p>
                            <asp:Label ID="lblAppError1" runat="server" Text="" meta:resourcekey="lblAppError1" /></p>
                        <p>
                            <asp:Label ID="lblAppError2" runat="server" Text="" meta:resourcekey="lblAppError2" /></p>
                        <p>
                            <asp:Label ID="lblAppError3" runat="server" Text="" meta:resourcekey="lblAppError3" /></p>
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
