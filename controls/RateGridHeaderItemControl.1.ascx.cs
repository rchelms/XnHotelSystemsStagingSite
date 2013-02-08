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

public partial class RateGridHeaderItemControl : System.Web.UI.UserControl
{
    public delegate void ItemDateSelectedEvent(object sender, DateTime selectedItemDate);
    public event ItemDateSelectedEvent ItemDateSelected;

    private string _ItemLegend;
    private DateTime _ItemDate;
    private RateGridItemStatus _ItemStatus;

    public string ItemLegend
    {
        set
        {
            _ItemLegend = value;
        }

        get
        {
            return _ItemLegend;
        }

    }

    public DateTime ItemDate
    {
        set
        {
            _ItemDate = value;
        }

        get
        {
            return _ItemDate;
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
        lblWeekdayLegendText.Text = _ItemLegend;
        lblWeekdayLegendTextSelected.Text = _ItemLegend;
        lblWeekendLegendText.Text = _ItemLegend;
        lblWeekendLegendTextSelected.Text = _ItemLegend;

        lblWeekdayDOWText.Text = _ItemDate.ToString("ddd");
        lblWeekdayDOWTextSelected.Text = _ItemDate.ToString("ddd");
        lblWeekendDOWText.Text = _ItemDate.ToString("ddd");
        lblWeekendDOWTextSelected.Text = _ItemDate.ToString("ddd");

        lblWeekdayDateText.Text = _ItemDate.ToString("dd MMM");
        lblWeekdayDateTextSelected.Text = _ItemDate.ToString("dd MMM");
        lblWeekendDateText.Text = _ItemDate.ToString("dd MMM");
        lblWeekendDateTextSelected.Text = _ItemDate.ToString("dd MMM");

        panRateGridHeaderWeekday.Visible = false;
        panRateGridHeaderWeekdaySelected.Visible = false;
        panRateGridHeaderWeekend.Visible = false;
        panRateGridHeaderWeekendSelected.Visible = false;

        if (_ItemStatus == RateGridItemStatus.Weekday)
        {
            panRateGridHeaderWeekday.Visible = true;
        }

        else if (_ItemStatus == RateGridItemStatus.WeekdaySelected)
        {
            panRateGridHeaderWeekdaySelected.Visible = true;
        }

        else if (_ItemStatus == RateGridItemStatus.Weekend)
        {
            panRateGridHeaderWeekend.Visible = true;
        }

        else if (_ItemStatus == RateGridItemStatus.WeekendSelected)
        {
            panRateGridHeaderWeekendSelected.Visible = true;
        }

        panRateGridHeaderItem.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(btnItemDate, ""));

        return;
    }

    protected void btnItemDate_Click(object sender, EventArgs e)
    {
        ItemDateSelected(this, _ItemDate.Date);
        return;
    }

}
