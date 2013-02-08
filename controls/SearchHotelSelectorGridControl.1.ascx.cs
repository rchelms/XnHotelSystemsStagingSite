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

public partial class SearchHotelSelectorGridControl : System.Web.UI.UserControl
{
    public delegate void SearchHotelSelectedEvent(object sender, EventArgs e);
    public event SearchHotelSelectedEvent SearchHotelSelected;

    public delegate void RateGridDateSelectedEvent(object sender, RateGridEventArgs e);
    public event RateGridDateSelectedEvent RateGridDateSelected;

    private DateTime _GridTodayDate;
    private DateTime _GridStartDate;
    private int _GridNumberDays;
    private DateTime _GridSelectedStartDate;
    private DateTime _GridSelectedEndDate;
    private AreaListItem _AreaListItem;
    private string _SelectedHotelCode;

    private List<SearchHotelSelectorGridItemControl> lSearchHotelSelectorGridItemControls;

    public DateTime GridTodayDate
    {
        get
        {
            return _GridTodayDate;
        }

        set
        {
            _GridTodayDate = value;
        }

    }

    public DateTime GridStartDate
    {
        get
        {
            return _GridStartDate;
        }

        set
        {
            _GridStartDate = value;
        }

    }

    public int GridNumberDays
    {
        get
        {
            return _GridNumberDays;
        }

        set
        {
            _GridNumberDays = value;
        }

    }

    public DateTime GridSelectedStartDate
    {
        get
        {
            return _GridSelectedStartDate;
        }

        set
        {
            _GridSelectedStartDate = value;
        }

    }

    public DateTime GridSelectedEndDate
    {
        get
        {
            return _GridSelectedEndDate;
        }

        set
        {
            _GridSelectedEndDate = value;
        }

    }

    public AreaListItem AreaListItem
    {
        get
        {
            return _AreaListItem;
        }

        set
        {
            _AreaListItem = value;
        }

    }

    public string SelectedHotelCode
    {
        get
        {
            return _SelectedHotelCode;
        }

    }

    public SearchHotelSelectorGridItemControl[] SearchHotelSelectorItems
    {
        get
        {
            if (lSearchHotelSelectorGridItemControls != null)
                return lSearchHotelSelectorGridItemControls.ToArray();
            else
                return new SearchHotelSelectorGridItemControl[0];
        }

    }

    public void Clear()
    {
        _SelectedHotelCode = "";
        lSearchHotelSelectorGridItemControls = null;
        return;
    }

    public void Add(SearchHotelSelectorGridItemControl ucSearchHotelSelectorGridItemControl)
    {
        if (lSearchHotelSelectorGridItemControls == null)
        {
            lSearchHotelSelectorGridItemControls = new List<SearchHotelSelectorGridItemControl>();
        }

        lSearchHotelSelectorGridItemControls.Add(ucSearchHotelSelectorGridItemControl);
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack && !this.IsParentPreRender())
        {
            this.ApplyControlsToPage();
        }

        return;
    }

    public void RenderUserControl()
    {
        ApplyControlsToPage();

        lblAreaInfoText.Text = _AreaListItem.AreaName;

        if (_GridStartDate.Date > _GridTodayDate.Date)
            panRateGridPrevious.Visible = true;
        else
            panRateGridPrevious.Visible = false;

        for (int i = 0; i < lSearchHotelSelectorGridItemControls.Count; i++)
            lSearchHotelSelectorGridItemControls[i].RenderUserControl();

        return;
    }

    public void SearchHotelGridItemSelected(object sender, string HotelCode)
    {
        _SelectedHotelCode = HotelCode;
        SearchHotelSelected(sender, new EventArgs());
        return;
    }

    protected void btnRateGridPrevious_Click(object sender, EventArgs e)
    {
        DateTime dtNewStartDate = this._GridStartDate.AddDays(this._GridNumberDays * -1);

        if (dtNewStartDate.Date < _GridTodayDate.Date)
            dtNewStartDate = _GridTodayDate.Date;

        RateGridEventArgs objRateGridEventArgs = new RateGridEventArgs();
        objRateGridEventArgs.NewStartDate = dtNewStartDate;
        objRateGridEventArgs.Operation = RateGridOperation.MovePrevious;

        RateGridDateSelected(this, objRateGridEventArgs);
        return;
    }

    protected void btnRateGridNext_Click(object sender, EventArgs e)
    {
        RateGridEventArgs objRateGridEventArgs = new RateGridEventArgs();
        objRateGridEventArgs.NewStartDate = this._GridStartDate.AddDays(this._GridNumberDays);
        objRateGridEventArgs.Operation = RateGridOperation.MoveNext;

        RateGridDateSelected(this, objRateGridEventArgs);
        return;
    }

    protected void RateGridDate_Click(object sender, DateTime dtRateGridDate)
    {
        RateGridEventArgs objRateGridEventArgs = new RateGridEventArgs();
        objRateGridEventArgs.NewStartDate = dtRateGridDate;
        objRateGridEventArgs.Operation = RateGridOperation.DateSelected;

        RateGridDateSelected(this, objRateGridEventArgs);
        return;
    }

    private void ApplyControlsToPage()
    {
        phRateGridHeaderItems.Controls.Clear();

        for (int di = 0; di < _GridNumberDays; di++)
        {
            DateTime dtGridDay = _GridStartDate.AddDays(di).Date;

            string strRateGridHeaderItemControlPath = ConfigurationManager.AppSettings["RateGridHeaderItemControl.ascx"];
            RateGridHeaderItemControl ucRateGridHeaderItemControl = (RateGridHeaderItemControl)LoadControl(strRateGridHeaderItemControlPath);
            phRateGridHeaderItems.Controls.Add(ucRateGridHeaderItemControl);

            ucRateGridHeaderItemControl.ID = "GridHeaderItem" + di.ToString();
            ucRateGridHeaderItemControl.ItemDate = dtGridDay.Date;

            if (dtGridDay.Date >= _GridSelectedStartDate.Date && dtGridDay.Date <= _GridSelectedEndDate.Date)
            {
                if (((int)dtGridDay.DayOfWeek >= 1 && (int)dtGridDay.DayOfWeek <= 5))
                    ucRateGridHeaderItemControl.ItemStatus = RateGridItemStatus.WeekdaySelected;
                else
                    ucRateGridHeaderItemControl.ItemStatus = RateGridItemStatus.WeekendSelected;
            }

            else
            {
                if (((int)dtGridDay.DayOfWeek >= 1 && (int)dtGridDay.DayOfWeek <= 5))
                    ucRateGridHeaderItemControl.ItemStatus = RateGridItemStatus.Weekday;
                else
                    ucRateGridHeaderItemControl.ItemStatus = RateGridItemStatus.Weekend;
            }

            ucRateGridHeaderItemControl.ItemDateSelected += new RateGridHeaderItemControl.ItemDateSelectedEvent(this.RateGridDate_Click);

            ucRateGridHeaderItemControl.RenderUserControl();
        }

        if (lSearchHotelSelectorGridItemControls == null)
        {
            lSearchHotelSelectorGridItemControls = new List<SearchHotelSelectorGridItemControl>();
        }

        phRateGridHotelItems.Controls.Clear();

        for (int i = 0; i < lSearchHotelSelectorGridItemControls.Count; i++)
        {
            lSearchHotelSelectorGridItemControls[i].SearchHotelSelected += new SearchHotelSelectorGridItemControl.SearchHotelSelectedEvent(this.SearchHotelGridItemSelected);
            phRateGridHotelItems.Controls.Add(lSearchHotelSelectorGridItemControls[i]);
        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
