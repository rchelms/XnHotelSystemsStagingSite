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
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class HotelDescriptionItemControl : System.Web.UI.UserControl
{
    private string _Title;
    private string _Description;

    public string Title
    {
        set
        {
            _Title = value;
        }

        get
        {
            return _Title;
        }

    }

    public string Description
    {
        set
        {
            _Description = value;
        }

        get
        {
            return _Description;
        }

    }

    public void RenderUserControl()
    {
        lblTitle.Text = _Title;
        lblDescription.Text = _Description;

        return;
    }

}
