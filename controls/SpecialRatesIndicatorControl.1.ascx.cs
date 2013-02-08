using System;
using System.Text;
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

public partial class SpecialRatesIndicatorControl : System.Web.UI.UserControl
{
    private bool _IsActive;

    public bool IsActive
    {
        get
        {
            return _IsActive;
        }

        set
        {
            _IsActive = value;
        }

    }

    public void RenderUserControl()
    {
        if (IsActive)
        {
            panSpecialRatesIndicator.Visible = true;
        }

        else
        {
            panSpecialRatesIndicator.Visible = false;
        }

        return;
    }

}
