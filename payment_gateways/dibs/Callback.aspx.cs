using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.DIBSHelper;
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class DIBS_Callback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FileLog objEventLog = (FileLog)Application["EventLog"];
        ExceptionLog objExceptionEventLog = (ExceptionLog)Application["ExceptionEventLog"];

        DIBSDataProcessor objProcessor = new DIBSDataProcessor(objEventLog, objExceptionEventLog, true);
        objProcessor.ProcessCallback(Page.Request);

        string strEchoData = "";

        if (Request.QueryString.Get("ed") != null)
            strEchoData = Request.QueryString.Get("ed");

        WBSMonitor wbsMonitor = WBSMonitor.GetWbsMonitor(Context.Cache, objEventLog, objExceptionEventLog, XnGR_WBS_Page.IsProductionMode(), (int)Application["WBSMonitor.ExpirationSeconds"]);
        wbsMonitor.UpdateItem(strEchoData, WBSMonitor.MonitorUpdateType.CallbackReceived);

        return;
    }

}
