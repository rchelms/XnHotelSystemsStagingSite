using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;
using MamaShelter;

public partial class Default : XnGR_WBS_Page
{
    protected override void Page_Init(object sender, EventArgs e)
    {
        this.IsNewSessionOverride = true;

        base.Page_Init(sender, e);

        if (this.IsServiceTimeThresholdExceeded)
        {
            this.WbsPerfCounters.IncPerfCounter(WBSPerfCounters.REQUEST_TURNDOWNS);
            this.WbsPerfCounters.IncPerfCounter(WBSPerfCounters.TOTAL_REQUEST_TURNDOWNS);

            Session.Abandon();
            Response.Redirect("~/SystemBusy.htm");
        }

       if (Session[Constants.Sessions.CurrentBookingStep] != null &&
           ((BookingSteps) Session[Constants.Sessions.CurrentBookingStep]) == BookingSteps.Unknown)
          Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectHotel;

        Response.Redirect("~/Pages/");

        return;
    }

}
