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

public partial class ErrorDisplayControl : System.Web.UI.UserControl
{
    private string[] _ErrorInfos;

    public string[] ErrorInfos
    {
        get
        {
            return _ErrorInfos;
        }

        set
        {
            _ErrorInfos = value;
        }

    }

    public void RenderUserControl()
    {
        panErrorInfo.Visible = false;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < _ErrorInfos.Length; i++)
        {
            sb.Append(_ErrorInfos[i]);
            sb.Append(@"<br />");
        }

        lblErrorList.Text = sb.ToString();

        if (_ErrorInfos.Length != 0)
            panErrorInfo.Visible = true;

        return;
    }

}
