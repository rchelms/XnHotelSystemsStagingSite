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

public partial class RoomTypeSelectorGridItemControl : System.Web.UI.UserControl
{
    public delegate void RateGridDateSelectedEvent(object sender, RateGridEventArgs e);
    public event RateGridDateSelectedEvent RateGridDateSelected;

    public delegate void ShowMoreLessRatesEvent(object sender, EventArgs e);
    public event ShowMoreLessRatesEvent ShowMoreLessRates;

    private DateTime _GridTodayDate;
    private DateTime _GridStartDate;
    private int _GridNumberDays;
    private DateTime _GridSelectedStartDate;
    private DateTime _GridSelectedEndDate;
    private string _RoomRefID;
    private HotelDescRoomType _RoomType;
    private bool _ShowMoreRates;

    private List<RatePlanSelectorGridItemControl> lRatePlanSelectorGridItems;

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

    public string RoomRefID
    {
        get
        {
            return _RoomRefID;
        }

        set
        {
            _RoomRefID = value;
        }

    }

    public HotelDescRoomType RoomType
    {
        get
        {
            return _RoomType;
        }

        set
        {
            _RoomType = value;
        }

    }

    public bool ShowMoreRates
    {
        get
        {
            return _ShowMoreRates;
        }

        set
        {
            _ShowMoreRates = value;
        }

    }

    public RatePlanSelectorGridItemControl[] RatePlanSelectorGridItems
    {
        get
        {
            if (lRatePlanSelectorGridItems != null)
                return lRatePlanSelectorGridItems.ToArray();
            else
                return new RatePlanSelectorGridItemControl[0];
        }

    }

    public void Clear()
    {
        lRatePlanSelectorGridItems = null;
        return;
    }

    public void AddRatePlanSelectorGridItem(RatePlanSelectorGridItemControl ucNewRatePlanSelectorGridItem)
    {
        if (lRatePlanSelectorGridItems == null)
        {
            lRatePlanSelectorGridItems = new List<RatePlanSelectorGridItemControl>();
        }

        lRatePlanSelectorGridItems.Add(ucNewRatePlanSelectorGridItem);
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack & !this.IsParentPreRender())
        {
            this.ApplyControlsToPage();
        }

        return;
    }

    public void RenderUserControl()
    {
        this.ApplyControlsToPage();

        for (int i = 0; i < _RoomType.Images.Length; i++)
        {
            if (_RoomType.Images[i].CategoryCode == HotelImageCategoryCode.GuestRoom && _RoomType.Images[i].ImageSize == HotelImageSize.Thumbnail)
            {
                imgRoomType.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] +_RoomType.Images[i].ImageURL;
            }

            if (_RoomType.Images[i].CategoryCode == HotelImageCategoryCode.GuestRoom && _RoomType.Images[i].ImageSize == HotelImageSize.FullSize)
            {
                imgRoomTypePopupImage.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _RoomType.Images[i].ImageURL;
            }

        }

        lblRoomTypePopupTitle.Text = _RoomType.LongDescription.ContentTitle.ToString();
        lblRoomTypePopupDescription.Text = this.AddLineBreaks(_RoomType.LongDescription.ContentText.ToString());

        lblRoomTypeNameText.Text = _RoomType.Name;
        lblRoomTypeDescriptionText.Text = this.AddLineBreaks(_RoomType.ShortDescription);

        lbRoomDescription.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_RoomTypeDescPopup'} )");

        panShowMoreRatesLink.Visible = false;
        panShowLessRatesLink.Visible = false;

        if (ConfigurationManager.AppSettings["RoomTypeSelectorGridItemControl.EnableShowMoreRates"] == "1" && !_ShowMoreRates && lRatePlanSelectorGridItems.Count > 1)
        {
            panShowMoreRatesLink.Visible = true;
        }

        else if (ConfigurationManager.AppSettings["RoomTypeSelectorGridItemControl.EnableShowMoreRates"] == "1" && _ShowMoreRates && lRatePlanSelectorGridItems.Count > 1)
        {
            panShowLessRatesLink.Visible = true;
        }

        if (_GridStartDate.Date > _GridTodayDate.Date)
            panRateGridPrevious.Visible = true;
        else
            panRateGridPrevious.Visible = false;

        int intNumRatePlanSelectorGridItems = this.GetNumRatePlanSelectorGridItems();

        for (int i = 0; i < intNumRatePlanSelectorGridItems; i++)
            lRatePlanSelectorGridItems[i].RenderUserControl();

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

    protected void btnShowMoreLessRates_Click(object sender, EventArgs e)
    {
        _ShowMoreRates = !_ShowMoreRates;

        ShowMoreLessRates(this, new EventArgs());
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

            string strItemLegend = "";

            if (dtGridDay.Date == _GridSelectedStartDate.Date)
                strItemLegend = (string)GetGlobalResourceObject("SiteResources", "RateGridLegendArrival");
            else if (dtGridDay.Date == _GridSelectedEndDate.AddDays(1).Date)
                strItemLegend = (string)GetGlobalResourceObject("SiteResources", "RateGridLegendDeparture");
            else if (dtGridDay.Date > _GridSelectedStartDate.Date && dtGridDay.Date <= _GridSelectedEndDate.Date)
                strItemLegend = (string)GetGlobalResourceObject("SiteResources", "RateGridLegendInStay");
            else
                strItemLegend = (string)GetGlobalResourceObject("SiteResources", "RateGridLegendOutStay");

            ucRateGridHeaderItemControl.ItemLegend = strItemLegend;

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

        if (lRatePlanSelectorGridItems == null)
        {
            lRatePlanSelectorGridItems = new List<RatePlanSelectorGridItemControl>();
        }

        phRatePlans.Controls.Clear();

        int intNumRatePlanSelectorGridItems = this.GetNumRatePlanSelectorGridItems();

        for (int i = 0; i < intNumRatePlanSelectorGridItems; i++)
            phRatePlans.Controls.Add(lRatePlanSelectorGridItems[i]);

        return;
    }

    private int GetNumRatePlanSelectorGridItems()
    {
        int intNumRatePlanSelectorGridItems = 0;

        if (ConfigurationManager.AppSettings["RoomTypeSelectorGridItemControl.EnableShowMoreRates"] != "1" || _ShowMoreRates)
        {
            intNumRatePlanSelectorGridItems = lRatePlanSelectorGridItems.Count;
        }

        else
        {
            if (lRatePlanSelectorGridItems.Count > 0)
                intNumRatePlanSelectorGridItems = 1;
        }

        return intNumRatePlanSelectorGridItems;
    }

    private string AddLineBreaks(string strString)
    {
        string strReturn;

        strReturn = strString.Replace("\r", "");
        strReturn = strReturn.Replace("\n", "<br />");

        return strReturn;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
