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

public partial class RateGridDataItemControl : System.Web.UI.UserControl
{
    private string _ItemText;
    private string _ItemPopupText;
    private RateGridItemStatus _ItemStatus;

    public string ItemText
    {
        set
        {
            _ItemText = value;
        }

        get
        {
            return _ItemText;
        }

    }

    public string ItemPopupText
    {
        set
        {
            _ItemPopupText = value;
        }

        get
        {
            return _ItemPopupText;
        }

    }

    public RateGridItemStatus ItemStatus
    {
        set
        {
            _ItemStatus = value;
        }

        get
        {
            return _ItemStatus;
        }

    }

    public void RenderUserControl()
    {
        lblRateGridDataOpenWeekday.Text = _ItemText;
        lblRateGridDataOpenWeekday.ToolTip = _ItemPopupText;

        lblRateGridDataOpenWeekend.Text = _ItemText;
        lblRateGridDataOpenWeekend.ToolTip = _ItemPopupText;

        lblRateGridDataOpenSelected.Text = _ItemText;
        lblRateGridDataOpenSelected.ToolTip = _ItemPopupText;

        panRateGridDataOpenWeekday.Visible = false;
        panRateGridDataOpenWeekend.Visible = false;
        panRateGridDataOpenSelected.Visible = false;
        panRateGridDataSold.Visible = false;
        panRateGridDataNull.Visible = false;

        if (_ItemStatus == RateGridItemStatus.Weekday)
        {
            panRateGridDataOpenWeekday.Visible = true;
        }

        else if (_ItemStatus == RateGridItemStatus.Weekend)
        {
            panRateGridDataOpenWeekend.Visible = true;
        }

        else if (_ItemStatus == RateGridItemStatus.Selected)
        {
            panRateGridDataOpenSelected.Visible = true;
        }

        else if (_ItemStatus == RateGridItemStatus.Sold)
        {
            panRateGridDataSold.Visible = true;
        }

        else
        {
            panRateGridDataNull.Visible = true;
        }

        return;
    }

}
