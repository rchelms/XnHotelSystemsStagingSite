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

public partial class SearchHotelSelectorGridItemControl : System.Web.UI.UserControl
{
    public delegate void SearchHotelSelectedEvent(object sender, string HotelCode);
    public event SearchHotelSelectedEvent SearchHotelSelected;

    private DateTime _GridStartDate;
    private int _GridNumberDays;
    private AvCalHiLoRate[] _GridHiLoRates;
    private HotelDescriptiveInfo _HotelDescriptiveInfo;
    private bool _ShowSpecialRatesIndicator;

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

    public AvCalHiLoRate[] GridHiLoRates
    {
        get
        {
            return _GridHiLoRates;
        }

        set
        {
            _GridHiLoRates = value;
        }

    }

    public HotelDescriptiveInfo HotelDescriptiveInfo
    {
        get
        {
            return _HotelDescriptiveInfo;
        }

        set
        {
            _HotelDescriptiveInfo = value;
        }

    }

    public bool ShowSpecialRatesIndicator
    {
        get
        {
            return _ShowSpecialRatesIndicator;
        }

        set
        {
            _ShowSpecialRatesIndicator = value;
        }

    }

    public void RenderUserControl()
    {
        StringBuilder sb;

        for (int i = 0; i < _HotelDescriptiveInfo.Images.Length; i++)
        {
            if (_HotelDescriptiveInfo.Images[i].CategoryCode == HotelImageCategoryCode.ExteriorView && _HotelDescriptiveInfo.Images[i].ImageSize == HotelImageSize.Thumbnail)
            {
                imgHotel.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _HotelDescriptiveInfo.Images[i].ImageURL;
                break;
            }

        }

        lblHotelNameText.Text = _HotelDescriptiveInfo.HotelName;

        sb = new StringBuilder();

        if (_HotelDescriptiveInfo.Address1.Trim() != "")
        {
            sb.Append(_HotelDescriptiveInfo.Address1);
        }

        if (_HotelDescriptiveInfo.Address2.Trim() != "")
        {
            if (sb.ToString().Trim() != "")
            {
                sb.Append(", ");
            }

            sb.Append(_HotelDescriptiveInfo.Address1);
        }

        if (_HotelDescriptiveInfo.City.Trim() != "")
        {
            if (sb.ToString().Trim() != "")
            {
                sb.Append(", ");
            }

            sb.Append(_HotelDescriptiveInfo.City);
        }

        lblHotelAddressInfo.Text = sb.ToString();

        phHotelDescription.Controls.Clear();

        string strHotelDescriptionControlPath = ConfigurationManager.AppSettings["HotelDescriptionControl.ascx"];
        HotelDescriptionControl ucHotelDescriptionControl = (HotelDescriptionControl)LoadControl(strHotelDescriptionControlPath);
        phHotelDescription.Controls.Add(ucHotelDescriptionControl);

        bool bHotelDescriptionOnly = false;

        if (ConfigurationManager.AppSettings["SearchHotelSelectorGridItemControl.HotelDescriptionOnlyInPopUp"] == "1")
            bHotelDescriptionOnly = true;

        ucHotelDescriptionControl.HotelDescriptiveInfo = _HotelDescriptiveInfo;
        ucHotelDescriptionControl.HotelDescriptionOnly = bHotelDescriptionOnly;
        ucHotelDescriptionControl.RenderUserControl();

        phHotelRating.Controls.Clear();

        string strHotelRatingControlPath = ConfigurationManager.AppSettings["HotelRatingControl.ascx"];
        HotelRatingControl ucHotelRatingControl = (HotelRatingControl)LoadControl(strHotelRatingControlPath);
        phHotelRating.Controls.Add(ucHotelRatingControl);

        ucHotelRatingControl.RatingProvider = _HotelDescriptiveInfo.RatingProvider;
        ucHotelRatingControl.Rating = _HotelDescriptiveInfo.Rating;
        ucHotelRatingControl.RatingSymbol = _HotelDescriptiveInfo.RatingSymbol;
        ucHotelRatingControl.RenderUserControl();

        phSpecialRatesIndicator.Controls.Clear();

        string strSpecialRatesIndicatorControlPath = ConfigurationManager.AppSettings["SpecialRatesIndicatorControl.ascx"];
        SpecialRatesIndicatorControl ucSpecialRatesIndicatorControl = (SpecialRatesIndicatorControl)LoadControl(strSpecialRatesIndicatorControlPath);
        phSpecialRatesIndicator.Controls.Add(ucSpecialRatesIndicatorControl);

        ucSpecialRatesIndicatorControl.IsActive = _ShowSpecialRatesIndicator;
        ucSpecialRatesIndicatorControl.RenderUserControl();

        phRateGridDataItems.Controls.Clear();

        for (int di = 0; di < _GridNumberDays; di++)
        {
            DateTime dtGridDay = _GridStartDate.AddDays(di).Date;

            string strRateGridDataItemControlPath = ConfigurationManager.AppSettings["RateGridDataItemControl.ascx"];
            RateGridDataItemControl ucRateGridDataItemControl = (RateGridDataItemControl)LoadControl(strRateGridDataItemControlPath);
            phRateGridDataItems.Controls.Add(ucRateGridDataItemControl);

            decimal decRateAmount = 0;
            string strInclusions = "";
            AvailStatus enumRateStatus = AvailStatus.NotIdentified;

            for (int i = 0; i < _GridHiLoRates.Length; i++)
            {
                if (_GridHiLoRates[i].DayNum == (di + 1))
                {
                    decRateAmount = _GridHiLoRates[i].LowAmount;
                    strInclusions = _GridHiLoRates[i].LowInclusions;
                    enumRateStatus = _GridHiLoRates[i].Status;
                    break;
                }

            }

            ucRateGridDataItemControl.ID = "GridDataItem" + di.ToString();
            ucRateGridDataItemControl.ItemText = decRateAmount.ToString("F0") + "<br />" + _HotelDescriptiveInfo.CurrencyCode;
            ucRateGridDataItemControl.ItemPopupText = strInclusions;

            if (enumRateStatus == AvailStatus.Open && ((int)dtGridDay.DayOfWeek >= 1 && (int)dtGridDay.DayOfWeek <= 5))
            {
                ucRateGridDataItemControl.ItemStatus = RateGridItemStatus.Weekday;
            }

            else if (enumRateStatus == AvailStatus.Open && ((int)dtGridDay.DayOfWeek == 0 || (int)dtGridDay.DayOfWeek == 6))
            {
                ucRateGridDataItemControl.ItemStatus = RateGridItemStatus.Weekend;
            }

            else if (enumRateStatus == AvailStatus.Closed)
            {
                ucRateGridDataItemControl.ItemStatus = RateGridItemStatus.Sold;
            }

            else
            {
                ucRateGridDataItemControl.ItemStatus = RateGridItemStatus.Undefined;
            }

            ucRateGridDataItemControl.RenderUserControl();
        }

        return;
    }

    protected void btnSelectSearchHotel_Click(object sender, EventArgs e)
    {
        SearchHotelSelected(sender, _HotelDescriptiveInfo.HotelCode);
        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
