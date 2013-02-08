using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class PaymentCardLogoItemControl : System.Web.UI.UserControl
{
    private string _CardType;

    public string CardType
    {
        get
        {
            return _CardType;
        }

        set
        {
            _CardType = value;
        }

    }

    public void RenderUserControl()
    {
        string strPaymentCardLogoUrl = (String)GetGlobalResourceObject("SiteResources", "CardLogo" + _CardType);

        if (strPaymentCardLogoUrl != null && strPaymentCardLogoUrl != "")
        {
            imgPaymentCardLogo.ImageUrl = strPaymentCardLogoUrl;
            panPaymentCardLogo.Visible = true;
        }

        else
        {
            panPaymentCardLogo.Visible = false;
        }

        return;
    }

}
