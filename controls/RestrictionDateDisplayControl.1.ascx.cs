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

public partial class RestrictionDateDisplayControl : System.Web.UI.UserControl
{
    private string[] _RestrictionDateInfos;

    public string[] RestrictionDateInfos
    {
        get
        {
            return _RestrictionDateInfos;
        }

        set
        {
            _RestrictionDateInfos = value;
        }

    }

    public void RenderUserControl()
    {
        panRestrictionDateInfo.Visible = false;

        phRestrictionDateInfo.Controls.Clear();

        for (int i = 0; i < _RestrictionDateInfos.Length; i++)
        {
            Label lbl = new Label();
            lbl.Text = _RestrictionDateInfos[i];

            Literal lit = new Literal();
            lit.Text = @"<br />";

            phRestrictionDateInfo.Controls.Add(lbl);
            phRestrictionDateInfo.Controls.Add(lit);
        }

        if (_RestrictionDateInfos.Length != 0)
            panRestrictionDateInfo.Visible = true;

        return;
    }

}
