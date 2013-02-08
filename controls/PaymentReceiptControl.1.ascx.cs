using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.WBSUIBizObjects;

public partial class PaymentReceiptControl : System.Web.UI.UserControl
{
    private OnlinePaymentReceipt _PaymentReceipt;

    public OnlinePaymentReceipt PaymentReceipt
    {
        get
        {
            return _PaymentReceipt;
        }

        set
        {
            _PaymentReceipt = value;
        }

    }

    public void RenderUserControl()
    {
        if (_PaymentReceipt.PaymentCard.PaymentCardType != "XX")
            lblCardTypeText.Text = _PaymentReceipt.PaymentCard.PaymentCardType;
        else
            lblCardTypeText.Text = _PaymentReceipt.PaymentGatewayCardType;

        lblCardNumberText.Text = this.CardNumberMasked(_PaymentReceipt.PaymentCard.PaymentCardNumber);
        lblPaymentAmountText.Text = _PaymentReceipt.CurrencyCode + " " + _PaymentReceipt.Amount.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
        lblPaymentDateText.Text = _PaymentReceipt.PaymentDateTime.ToString("dd-MMMM-yyyy");
        lblAuthCodeText.Text = _PaymentReceipt.AuthCode;
        lblPaymentReferenceText.Text = _PaymentReceipt.TransRefID;

        return;
    }

    private string CardNumberMasked(string strPaymentCardNumber)
    {
        int intMaskCount = strPaymentCardNumber.Length - 4;

        if (intMaskCount < 1)
            return "****";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < strPaymentCardNumber.Length; i++)
        {
            if (i < intMaskCount)
                sb.Append("*");
            else
                sb.Append(strPaymentCardNumber[i]);
        }

        return sb.ToString();
    }

}
