<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CancelRoomSelectorItemControl.1.ascx.cs"
    Inherits="CancelRoomSelectorItemControl" %>
<%@ Reference Control="~/controls/CancelAddOnPackageItemControl.1.ascx" %>
<asp:Panel ID="panCancelRoomSelectorItem" CssClass="mm_room_rate_info mm_background_info" runat="server">
    <asp:Panel runat="server" CssClass="mm_main_step">
        <asp:Panel runat="server" CssClass="mm_step mm_border_info">
            <asp:Label ID="lblRoomIdentifier" runat="server" Text="" CssClass="mm_text_info" />
            <asp:CheckBox ID="cbRoomSelected" runat="server" meta:resourcekey="lblSelectCancelRoom"
                CssClass="mm_checkbox_cancel_room" />
            <asp:Label ID="lblNonCancelableRoom" runat="server" meta:resourcekey="lblNonCancelableRoom" CssClass="mm_text_non_cancellable"></asp:Label>
        </asp:Panel>
        
        <asp:Panel runat="server" CssClass="mm_room_rate_content_cancellation mm_text_info_cancellation">
            <asp:Label ID="lblRoomTypeNameText" runat="server" Text="" CssClass="mm_text_bold" />
            <asp:Label ID="lblRatePlanNameText" runat="server" Text="" CssClass="mm_text_x_strong" />
            <asp:Panel ID="Panel4" runat="server">
                <asp:Label ID="lblPolicyButton" runat="server" CssClass="mm_room_detail_button" meta:resourcekey="lbViewPolicies"></asp:Label>
            </asp:Panel>

            <asp:Panel runat="server" ID="panCancellationPolicy" CssClass="mm_room_type_description mm_hidden">
                <asp:Label ID="lblCancelPolicyText" runat="server" CssClass="mm_detail_text" />
            </asp:Panel>

            <asp:Label ID="lblTotalRoomPrice" runat="server" Text="" CssClass="mm_room_rate_price" />
        </asp:Panel>

        <asp:Panel ID="Panel5" runat="server" CssClass="">
            <asp:PlaceHolder ID="phAddOnPackages" runat="server" />
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
