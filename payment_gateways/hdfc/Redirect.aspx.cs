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
using System.Text;
using System.IO;
using XHS.Logging;
using XHS.HDFCHelper;

public partial class HDFC_Redirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FileLog objEventLog = (FileLog)Application["EventLog"];
        ExceptionLog objExceptionEventLog = (ExceptionLog)Application["ExceptionEventLog"];

        StringBuilder sbReturnURL = new StringBuilder();

        if (Request.IsSecureConnection)
            sbReturnURL.Append("https://");
        else
            sbReturnURL.Append("http://");

        sbReturnURL.Append(Request.ServerVariables["HTTP_HOST"]);

        Uri baseURL = new Uri(sbReturnURL.ToString());

        string strResultURL = (new Uri(baseURL, Response.ApplyAppPathModifier("Response.aspx"))).ToString();
        string strErrorURL = (new Uri(baseURL, Response.ApplyAppPathModifier("Error.aspx"))).ToString();

        HDFCDataProcessor objHDFCDataProcessor = new HDFCDataProcessor(objEventLog, objExceptionEventLog);

        string strOutput = objHDFCDataProcessor.ProcessRedirect(this, this.Context, strResultURL, strErrorURL);

        Response.Write(strOutput);
    }

}
