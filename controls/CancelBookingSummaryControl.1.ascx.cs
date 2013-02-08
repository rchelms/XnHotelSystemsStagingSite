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

public partial class CancelBookingSummaryControl : System.Web.UI.UserControl
{
    private CancelledBookingInfo[] _CancelledBookings;

    public CancelledBookingInfo[] CancelledBookings
    {
        set
        {
            _CancelledBookings = value;
        }

        get
        {
            return _CancelledBookings;
        }

    }

    public void RenderUserControl()
    {
        if (_CancelledBookings == null)
            return;

        StringBuilder sb = new StringBuilder();

        string strSeparator = "";

        for (int i = 0; i < _CancelledBookings.Length; i++)
        {
            sb.Append(strSeparator);
            sb.Append(_CancelledBookings[i].CancellationNumber);

            strSeparator = ", ";
        }

        lblCancellationReferenceInfo.Text = sb.ToString();

        return;
    }

}
