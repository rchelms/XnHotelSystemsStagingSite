<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddOnPackageSelectorItemControl.1.ascx.cs"
    Inherits="AddOnPackageSelectorItemControl" %>
<asp:Panel ID="panAddOnPackageSelectorItem" CssClass="" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSelect" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnRemove" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="panAddOnPackageSeletorItemContent" runat="server" CssClass="">
                <xnc:XImage ID="imgPackage" CssClass="addon_package_desc_detail_image mm_hidden"
                    Location="HotelMediaCDNPath" runat="server" ImageUrl="" />
                <asp:Label ID="lblPackageNameText" runat="server" Text="" CssClass="mm_text_x_strong" />
                <asp:Label runat="server" ID="lblDescription" CssClass="mm_room_detail_button mm_text_normal"
                    meta:resourcekey="lblDescription"></asp:Label>
                <asp:Panel ID="panAddOnPackageDescription" runat="server" CssClass="mm_room_type_description mm_hidden">
                    <asp:Label ID="lblPackageDescriptionText" runat="server" Text="" CssClass="mm_detail_text" />
                    
                </asp:Panel>
                <asp:Panel ID="panAddOnPackagePriceDescription" runat="server">
                    <asp:Label ID="lblPackagePrice" runat="server" Text="" CssClass="mm_text_normal" />
                    <asp:Label ID="lblPackagePriceType" runat="server" Text="" CssClass="mm_text_normal" />
                    <asp:Label ID="lblPackagePriceNights" runat="server" Text="" CssClass="mm_text_normal" />
                    <asp:Label ID="lblPackageQuantity" runat="server" CssClass="mm_hidden"></asp:Label>
                    <asp:DropDownList ID="ddlPackageQuantity" runat="server"/>
                <asp:Label ID="lblPackageQuantityUnits" runat="server" Text="" CssClass="mm_hidden" />
                    <asp:CheckBox ID="cbPackageSelected" runat="server" CssClass="mm_hidden" />
                </asp:Panel>
                <asp:Panel runat="server" ID="panTotalPrice">
                    <asp:Label runat="server" ID="lblTotal"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="panButtonSelect" runat="server" CssClass="mm_wrapper_button_selectOrAdd mm_addon_wrapper_button_add">
                    <asp:Button ID="btnSelect" runat="server" CssClass="mm_button mm_text_button_select"
                        meta:resourcekey="btnSelect" OnClick="btnSelect_Click" />
                </asp:Panel>
                <asp:Panel ID="panButtonRemove" runat="server" CssClass="mm_roomrate_wrapper_button_editOrRemoveStyle mm_addon_wrapper_button_remove">
                    <asp:Button ID="btnRemove" runat="server" CssClass="mm_button mm_text_button_edit"
                        meta:resourcekey="btnRemove" OnClick="btnRemove_Click" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
