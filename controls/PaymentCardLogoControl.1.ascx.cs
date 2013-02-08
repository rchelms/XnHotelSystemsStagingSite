using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.WBSUIBizObjects;

public partial class PaymentCardLogoControl : System.Web.UI.UserControl
{
    private string[] _PaymentCardCodes;

    public string[] PaymentCardCodes
    {
        get
        {
            return _PaymentCardCodes;
        }

        set
        {
            _PaymentCardCodes = value;
        }

    }

    public void RenderUserControl()
    {
        phPaymentCardLogoItems.Controls.Clear();

        if (_PaymentCardCodes.Length != 0)
        {
            panPaymentCardLogos.Visible = true;

            for (int i = 0; i < _PaymentCardCodes.Length; i++)
            {
                string strPaymentCardLogoItemControlPath = ConfigurationManager.AppSettings["PaymentCardLogoItemControl.ascx"];
                PaymentCardLogoItemControl ucPaymentCardLogoItemControl = (PaymentCardLogoItemControl)LoadControl(strPaymentCardLogoItemControlPath);
                phPaymentCardLogoItems.Controls.Add(ucPaymentCardLogoItemControl);

                ucPaymentCardLogoItemControl.CardType = _PaymentCardCodes[i];
                ucPaymentCardLogoItemControl.RenderUserControl();
            }

        }

        else
        {
            panPaymentCardLogos.Visible = false;
        }

        return;
    }

}
