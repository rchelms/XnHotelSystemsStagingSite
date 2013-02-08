<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PaymentReceiptControl.1.ascx.cs"
    Inherits="PaymentReceiptControl" %>
<asp:Panel ID="panPaymentReceipt" CssClass="mm_content_section" runat="server">
    <asp:Panel CssClass="mm_step mm_background_info mm_border_info" runat="server">
        <asp:Label ID="lblPaymentReceipt" runat="server" Text="" meta:resourcekey="lblPaymentReceipt"
            CssClass="mm_text_info" />
    </asp:Panel>
    <asp:Panel CssClass="mm_payment_receipt_summary mm_background_info" runat="server">
        <asp:Panel CssClass="mm_payment_receipt_row" runat="server">
            <asp:Panel CssClass="mm_payment_receipt_left" runat="server">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblCardType" runat="server" Text="" meta:resourcekey="lblCardType" CssClass="mm_text_no_transform " />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="payment_receipt_right" runat="server">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblCardTypeText" runat="server" Text="" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="mm_payment_receipt_row" runat="server">
            <asp:Panel CssClass="mm_payment_receipt_left" runat="server">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblCardNumber" runat="server" Text="" meta:resourcekey="lblCardNumber" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" CssClass="payment_receipt_right">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblCardNumberText" runat="server" Text="" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="mm_payment_receipt_row" runat="server">
            <asp:Panel CssClass="mm_payment_receipt_left" runat="server">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblPaymentAmount" runat="server" Text="" meta:resourcekey="lblPaymentAmount" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" CssClass="payment_receipt_right">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblPaymentAmountText" runat="server" Text="" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="mm_payment_receipt_row" runat="server">
            <asp:Panel CssClass="mm_payment_receipt_left" runat="server">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblPaymentDate" runat="server" Text="" meta:resourcekey="lblPaymentDate" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" CssClass="payment_receipt_right">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblPaymentDateText" runat="server" Text="" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="mm_payment_receipt_row" runat="server">
            <asp:Panel CssClass="mm_payment_receipt_left" runat="server">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblAuthCode" runat="server" Text="" meta:resourcekey="lblAuthCode" CssClass="mm_text_no_transform " />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" CssClass="payment_receipt_right">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblAuthCodeText" runat="server" Text="" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel CssClass="mm_payment_receipt_row" runat="server">
            <asp:Panel CssClass="mm_payment_receipt_left" runat="server">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblPaymentReference" runat="server" Text="" meta:resourcekey="lblPaymentReference" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" CssClass="payment_receipt_right">
                <asp:Panel CssClass="mm_field_set_label" runat="server">
                    <asp:Label ID="lblPaymentReferenceText" runat="server" Text="" CssClass="mm_text_no_transform "/>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
