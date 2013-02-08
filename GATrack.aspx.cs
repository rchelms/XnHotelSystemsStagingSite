using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XHS.WBSUIBizObjects;

public partial class GATrack : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Response.AddHeader("p3p", "CP=\"NOI DSP COR CURa ADMa DEVa TAIa OUR BUS IND UNI COM NAV INT\"");

        ConfigureTrackingCodeControl();
    }

    private void ConfigureTrackingCodeControl()
    {
        TrackingCodeInfo[] objTrackingCodeInfos = WBSUITrackingHelper.GetTrackingCodeInfos("");

        if (objTrackingCodeInfos.Length == 0)
            return;

        for (int i = 0; i < objTrackingCodeInfos.Length; i++)
        {
            if (objTrackingCodeInfos[i].Type == TrackingCodeType.GoogleAnalytics_async)
            {
                TrackingCodeItemControl ucTrackingCodeItemControl = (TrackingCodeItemControl)LoadControl("~/Controls/TrackingCodeItemControl.1.ascx");

                ucTrackingCodeItemControl.Type = objTrackingCodeInfos[i].Type;
                ucTrackingCodeItemControl.TrackingCodeParameters = objTrackingCodeInfos[i].TrackingCodeParameters;
                ucTrackingCodeItemControl.HotelCode = "";
                ucTrackingCodeItemControl.PageUrl = ("/GATrack");
                ucTrackingCodeItemControl.Amount = 0;
                phTrackingCode.Controls.Add(ucTrackingCodeItemControl);
                ucTrackingCodeItemControl.RenderUserControl();
                break;
            }

        }

        return;
    }

}