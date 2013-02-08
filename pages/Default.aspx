<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="MamaShelterBooking" AsyncTimeout="15" EnableEventValidation="false" %>

<%@ Reference Control="~/controls/StayCriteriaSelectorControl.MM.ascx" %>
<%@ Reference Control="~/controls/ImageHoldingControl.1.ascx" %>
<%@ Reference Control="~/controls/AvailCalSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/AvailCalSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RoomQuantitySelectorControl.ascx" %>
<%@ Reference Control="~/controls/RoomDetailSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/PeopleQuantitySelectorControl.ascx" %>
<%@ Reference Control="~/controls/RatePlanSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/AddOnPackageSelectorControl.1.ascx" %>
<%@ Reference Control="~/controls/AddOnPackageSelectorItemControl.1.ascx" %>
<%@ Reference Control="~/controls/TotalCostControl.1.ascx" %>
<%@ Reference Control="~/controls/ErrorDisplayControl.1.ascx" %>
<%@ Reference Control="~/controls/GuestDetailsEntryControl.1.ascx" %>
<%@ Reference Control="~/controls/ConfirmationControl.1.ascx" %>
<%@ Reference Control="~/controls/PaymentReceiptControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeControl.1.ascx" %>
<%@ Reference Control="~/controls/TrackingCodeItemControl.1.ascx" %>
<%@ Reference Control="~/controls/RemoteContentContainer.1.ascx" %>
<asp:Content ID="cpgSMyTest" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:Panel runat="server">
        <asp:UpdatePanel ID="udpContentPage" runat="server">
            <ContentTemplate>
                <div class="mm_colLeft">
                    <asp:PlaceHolder ID="phStayCriteriaControl" runat="server"></asp:PlaceHolder>
                    <asp:PlaceHolder ID="phAvailCalSelectorControl" runat="server" />
                    <asp:PlaceHolder ID="phRoomQuantitySelectorControl" runat="server" />
                    <asp:PlaceHolder ID="phRoomDetailSelectorControl" runat="server" />
                    <asp:PlaceHolder ID="phTotalCostControl" runat="server" />
                    <asp:PlaceHolder ID="phConfirmationControl" runat="server" />
                    <asp:PlaceHolder ID="phPaymentReceiptControl" runat="server" />
                </div>
                <asp:Panel runat="server" CssClass="mm_colRight">
                    <asp:PlaceHolder ID="phErrorDisplayControl" runat="server" />
                    <asp:PlaceHolder ID="phImageHoldingControl" runat="server" />
                    <asp:PlaceHolder ID="phRemoteContentContainerControl" runat="server" />
                    <asp:PlaceHolder ID="phGuestDetailsEntryControl" runat="server" />
                </asp:Panel>
                <asp:PlaceHolder ID="phTrackingCodeControl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div id="waitingPage">
        <img alt="waiting Page" src="../css/MamaShelter/images/loading.gif" /></div>
    <script type="text/javascript" language="javascript">
        $("#waitingPage").dialog({ modal: true }
            , { autoOpen: false }
            , { resizable: false }
            , { draggable: false }
            , { dialogClass: 'mm' }
            , { minWidth: 100 }
            , { minHeight: 50 }
            , { width: 130 }
            , { height: 65 });
    </script>
</asp:Content>
