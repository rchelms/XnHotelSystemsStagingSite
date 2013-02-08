using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XHS.WBSUIBizObjects;

public partial class ConfirmationControl : System.Web.UI.UserControl
{
    public string ConfirmationNumber { get; set; }

    public OnlinePaymentReceipt PaymentReceipt { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void RenderUserControl()
    {
        lblConfirmationNumber.Text = ConfirmationNumber;
    }
}