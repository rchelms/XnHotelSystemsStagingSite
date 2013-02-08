<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RestrictionDateDisplayControl.1.ascx.cs"
    Inherits="RestrictionDateDisplayControl" %>
<asp:Panel ID="panRestrictionDateInfo" CssClass="content_section" runat="server">
    <asp:Panel CssClass="content_section" runat="server">
        <asp:Panel CssClass="pane_left" runat="server">
            <asp:Panel CssClass="pane_right" runat="server">
                <asp:Panel CssClass="pane_center" runat="server">
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="pane_body" runat="server">
            <asp:Panel CssClass="restriction_date_notice" runat="server">
                <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/symbol_information.gif" />
                <asp:Panel CssClass="restriction_date_notice_instructions" runat="server">
                    <h3>
                        <asp:Label ID="lblRestrictionDateInfo" runat="server" Text="" meta:resourcekey="lblRestrictionDateInfo" /></h3>
                    <p>
                        <asp:PlaceHolder ID="phRestrictionDateInfo" runat="server" />
                    </p>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
