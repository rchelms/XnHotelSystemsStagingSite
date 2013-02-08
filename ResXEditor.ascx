<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResXEditor.ascx.cs" Inherits="ResXEditor" %>
<asp:Panel CssClass="" runat="server">
    <asp:Panel CssClass="rxe_header" runat="server">
        <h1>
            Xn GlobalRES WBS Translation Tool</h1>
    </asp:Panel>
    <asp:Panel CssClass="rxe_body" runat="server">
        <asp:Panel CssClass="rxe_resource_select" runat="server">
            <asp:Label runat="server" Text="Select File to Update" />
            <asp:ListBox ID="lstResX" runat="server" AutoPostBack="True" Height="150px" OnSelectedIndexChanged="lstResX_SelectedIndexChanged"
                Width="350px" />
        </asp:Panel>
        <asp:Panel CssClass="rxe_add_lang" ID="pnlAddLang" runat="server" Visible="False">
            <asp:Label runat="server" Text="Add New Language" />
            <asp:Panel CssClass="rxe_add_lang_control" runat="server">
                <asp:DropDownList ID="ddLanguage" runat="server" />
                <asp:Button ID="btAddLang" CssClass="rxe_button" runat="server" OnClick="btAddLang_Click"
                    OnClientClick="this.disable=true;" Enabled="true" Text="Add" />
                <asp:CheckBox ID="chShowEmpty" runat="server" Text="Show empty strings" Checked="false"
                    AutoPostBack="true" OnCheckedChanged="OnShowEmpty" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="rxe_grid" runat="server">
            <h2>
                <asp:Label ID="lblFileName" runat="server" /></h2>
            <resx:BulkEditGridViewEx ID="gridView" runat="server" GridLines="Both" Width="880px"
                CssClass="GridViewTranslate" AutoGenerateColumns="False" EnableInsert="False"
                OnRowDataBound="GridView_RowDataBound" SaveButtonID="btSave" DataKeyNames="Key"
                OnRowUpdating="gridView_RowUpdating" OnSaved="gridView_Saved" InsertRowCount="1" />
            <asp:Button ID="btSave" CssClass="rxe_button" runat="server" Text="Save" Visible="False" />
            <asp:Panel ID="pnlMsg" runat="server" EnableViewState="false" Visible="false">
                <asp:MultiView ID="MultiViewMsg" runat="server">
                    <asp:View ID="View1" runat="server">
                        <xnc:XImage ID="imgResult" runat="server" Location="ImageCDNPath" ImageUrl="~/images/tick.png" />
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <xnc:XImage ID="Image1" runat="server" Location="ImageCDNPath" ImageUrl="~/images/exclamation.png" />
                    </asp:View>
                </asp:MultiView>
                <asp:Label ID="lblMsg" runat="server" />
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
