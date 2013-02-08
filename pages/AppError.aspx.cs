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
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class AppError : XnGR_WBS_Page
{
    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        return;
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        this.PageComplete();

        return;
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
        return;
    }

}
