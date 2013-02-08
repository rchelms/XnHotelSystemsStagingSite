using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class AvailCalSelectorItemControl : System.Web.UI.UserControl
{
    private int _DayID;
    private AvailabilityCalendarInfo[] _AvailabilityCalendarInfo;
    private StayCriteriaSelection _StayCriteriaSelection;
    private DateTime _Today;
    private bool _Selected;

    public int DayID
    {
        get
        {
            return _DayID;
        }

        set
        {
            _DayID = value;
        }

    }

    public AvailabilityCalendarInfo[] AvailabilityCalendarInfo
    {
        get
        {
            return _AvailabilityCalendarInfo;
        }

        set
        {
            _AvailabilityCalendarInfo = value;
        }

    }

    public StayCriteriaSelection StayCriteriaSelection
    {
        get
        {
            return _StayCriteriaSelection;
        }

        set
        {
            _StayCriteriaSelection = value;
        }

    }

    public DateTime Today
    {
        get
        {
            return _Today;
        }

        set
        {
            _Today = value;
        }

    }

    public bool Selected
    {
        get
        {
            return _Selected;
        }

        set
        {
            _Selected = value;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack & !this.IsParentPreRender())
        {
            if (this.Request.Form.Get(cbAvailCalItemDaySelected.ClientID.Replace('_', '$')) != null) // form uses "name" not "id" property
                _Selected = true;
            else
                _Selected = false;
        }

        return;
    }

    public void RenderUserControl()
    {
        panActiveAvailCalItem.Visible = false;
        //panInactiveAvailCalItem.Visible = false;
        //panPassedDay.Visible = false;

        lblAvailCalItemDay.Text = this.CalendarDay(_DayID, _AvailabilityCalendarInfo).ToString();
        
        if(_AvailabilityCalendarInfo != null && _AvailabilityCalendarInfo.Length > 0)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            hdfAvailCalDayDate.Value = serializer.Serialize(_AvailabilityCalendarInfo[0].AvailabilityCalendar.StartDate.AddDays(DayID - 1).Date);
        }

        if (this.IsDayAvailable(_DayID, _AvailabilityCalendarInfo, _Today))
        {
            panActiveAvailCalItem.Visible = true;

            lblViewRatesTitle.Text = (String)GetLocalResourceObject("AvailabileRatesDate") + " " + _AvailabilityCalendarInfo[0].AvailabilityCalendar.StartDate.AddDays(_DayID - 1).Date.ToString("d MMM yyyy");

            

            if (_Selected == true)
                cbAvailCalItemDaySelected.Checked = true;
            else
                cbAvailCalItemDaySelected.Checked = false;

            lbViewCalRates.Text = this.LowestAvailableRate(_DayID, _AvailabilityCalendarInfo, _Today);
            lbViewCalRates.Attributes.Add("ondblclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_ViewCalRatesPopup'} )");

            //phSpecialRatesIndicator.Controls.Clear();

            //string strSpecialRatesIndicatorControlPath = ConfigurationManager.AppSettings["SpecialRatesIndicatorControl.ascx"];
            //SpecialRatesIndicatorControl ucSpecialRatesIndicatorControl = (SpecialRatesIndicatorControl)LoadControl(strSpecialRatesIndicatorControlPath);
            //phSpecialRatesIndicator.Controls.Add(ucSpecialRatesIndicatorControl);

            //ucSpecialRatesIndicatorControl.IsActive = this.HasRequestedRates(_DayID, _AvailabilityCalendarInfo, _Today);
            //ucSpecialRatesIndicatorControl.RenderUserControl();

            phViewCalRatesDetails.Controls.Clear();

            for (int ri = 0; ri < _StayCriteriaSelection.RoomOccupantSelections.Length; ri++)
            {
                AvailabilityCalendar objAvCal = new AvailabilityCalendar();

                bool bAvCalLocated = false;

                for (int i = 0; i < _AvailabilityCalendarInfo.Length; i++)
                {
                    if (_AvailabilityCalendarInfo[i].SegmentRefID == _StayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID)
                    {
                        objAvCal = _AvailabilityCalendarInfo[i].AvailabilityCalendar;
                        bAvCalLocated = true;
                        break;
                    }

                }

                if (!bAvCalLocated)
                    continue;

                StringBuilder sbRoomHeader = new StringBuilder();

                sbRoomHeader.Append((String)GetLocalResourceObject("RoomIdentifierText" + _StayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID));
                sbRoomHeader.Append(" ");

                sbRoomHeader.Append(_StayCriteriaSelection.RoomOccupantSelections[ri].NumberAdults.ToString());
                sbRoomHeader.Append(" ");
                sbRoomHeader.Append((String)GetLocalResourceObject("AdultsInfo"));

                if (_StayCriteriaSelection.RoomOccupantSelections[ri].NumberChildren != 0)
                {
                    sbRoomHeader.Append(", ");
                    sbRoomHeader.Append(_StayCriteriaSelection.RoomOccupantSelections[ri].NumberChildren.ToString());
                    sbRoomHeader.Append(" ");
                    sbRoomHeader.Append((String)GetLocalResourceObject("ChildrenInfo"));
                }

                Panel panRoomHeader = new Panel();
                panRoomHeader.CssClass = "avail_cal_rates_popup_detail_header";

                Label lblRoomHeaderText = new Label();
                lblRoomHeaderText.Text = sbRoomHeader.ToString();

                panRoomHeader.Controls.Add(lblRoomHeaderText);
                phViewCalRatesDetails.Controls.Add(panRoomHeader);

                PopUpRate[] objPopUpRates = this.GetPopUpRates(_DayID, objAvCal, _Today);

                for (int i = 0; i < objPopUpRates.Length; i++)
                {
                    Panel panPopUpRateHolder = new Panel();
                    panPopUpRateHolder.CssClass = "avail_cal_rates_popup_detail_block";

                    Panel panPopUpRateName = new Panel();
                    panPopUpRateName.CssClass = "avail_cal_rates_popup_detail_name avail_cal_rates_popup_detail";

                    Panel panPopUpRateDesc = new Panel();
                    panPopUpRateDesc.CssClass = "avail_cal_rates_popup_detail_desc avail_cal_rates_popup_detail";

                    Panel panPopUpRateAmount = new Panel();
                    panPopUpRateAmount.CssClass = "avail_cal_rates_popup_detail_amount avail_cal_rates_popup_detail";

                    Label lblPopUpRateName = new Label();
                    Label lblPopUpRateDescription = new Label();
                    Label lblPopUpRateAmount = new Label();

                    Label lblPopUpRateRestrictions = new Label();
                    lblPopUpRateRestrictions.CssClass = "avail_cal_rates_popup_detail_restrictions";

                    lblPopUpRateName.Text = objPopUpRates[i].RatePlan.Name;
                    lblPopUpRateDescription.Text = objPopUpRates[i].RatePlan.Description;
                    lblPopUpRateRestrictions.Text = ((XnGR_WBS_Page)this.Page).RateRestrictions(objPopUpRates[i].RoomRate, objPopUpRates[i].Rate);
                    lblPopUpRateAmount.Text = (string)GetLocalResourceObject("RatesFrom") + " " + objPopUpRates[i].RatePlan.CurrencyCode + " " + objPopUpRates[i].Rate.Amount.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());

                    if (lblPopUpRateRestrictions.Text == null || lblPopUpRateRestrictions.Text == "")
                        lblPopUpRateRestrictions.Visible = false;

                    panPopUpRateName.Controls.Add(lblPopUpRateName);
                    panPopUpRateDesc.Controls.Add(lblPopUpRateDescription);
                    panPopUpRateDesc.Controls.Add(lblPopUpRateRestrictions);
                    panPopUpRateAmount.Controls.Add(lblPopUpRateAmount);

                    panPopUpRateHolder.Controls.Add(panPopUpRateName);
                    panPopUpRateHolder.Controls.Add(panPopUpRateDesc);
                    panPopUpRateHolder.Controls.Add(panPopUpRateAmount);

                    phViewCalRatesDetails.Controls.Add(panPopUpRateHolder);
                }

            }

            var currentDay = _AvailabilityCalendarInfo[0].AvailabilityCalendar.StartDate.AddDays(_DayID - 1);
            var startDate = _AvailabilityCalendarInfo[0].AvailabilityCalendar.StartDate;
            if ((currentDay.Year.Equals(startDate.Year) && currentDay.Month > startDate.Month) || currentDay.Year > startDate.Year)
                panAvailCalItem.CssClass = string.Format("{0} {1}", panAvailCalItem.CssClass, "mm_available_next_month");
            panAvailCalItem.CssClass = string.Format("{0} {1}", panAvailCalItem.CssClass, "mm_available");
        }

        else if(_DayID < _Today.Day)
        {
            panAvailCalItem.CssClass = string.Format("{0} {1}", panAvailCalItem.CssClass, "mm_passed_day");
        }
        else
        {
            panAvailCalItem.CssClass = string.Format("{0} {1}", panAvailCalItem.CssClass, "mm_not_available");
        }

        return;
    }

    private int CalendarDay(int intDayID, AvailabilityCalendarInfo[] objAvCalInfo)
    {
        if (objAvCalInfo.Length == 0)
            return 0;

        return objAvCalInfo[0].AvailabilityCalendar.StartDate.AddDays(intDayID - 1).Day;
    }

    private bool IsDayAvailable(int intDayID, AvailabilityCalendarInfo[] objAvCalInfo, DateTime dtToday)
    {
        // Returns true if at least one room rate is available in all calendar instances for day identified

        for (int i = 0; i < objAvCalInfo.Length; i++)
        {
            if (!this.IsAvCalDayAvailable(intDayID, objAvCalInfo[i].AvailabilityCalendar, dtToday))
                return false;
        }

        return true;
    }

    private bool IsAvCalDayAvailable(int intDayID, AvailabilityCalendar objAvCal, DateTime dtToday)
    {
        // Returns true if at least one room rate is available in single calendar instance for day identified

        for (int i = 0; i < objAvCal.RoomRates.Length; i++)
        {
            if (IsAvCalRoomRateAvailable(intDayID, objAvCal.RoomRates[i], objAvCal.RatePlans, objAvCal.StartDate, dtToday))
                return true;
        }

        return false;
    }

    private bool IsAvCalRoomRateAvailable(int intDayID, AvCalRoomRate objRoomRate, AvCalRatePlan[] objRatePlans, DateTime dtCalendarBase, DateTime dtToday)
    {
        // Returns true if room rate is available in single calendar instance for room rate and day identified

        DateTime dtCalendarDate = dtCalendarBase.AddDays(intDayID - 1).Date;

        if (dtCalendarDate < dtToday)
            return false;

        if (ConfigurationManager.AppSettings["EnableRoomRateDescriptionModel"] == "1")
        {
            // Must be "active" or negotiated / consortia rate to appear in calendar

            if (objRoomRate.DescriptionStatus != RoomRateDescriptionStatus.Active)
            {
                for (int i = 0; i < objRatePlans.Length; i++)
                {
                    if (objRatePlans[i].RatePlanCode == objRoomRate.RatePlanCode)
                    {
                        if (objRatePlans[i].RatePlanType != RatePlanType.Negotiated && objRatePlans[i].RatePlanType != RatePlanType.Consortia)
                        {
                            return false;
                        }

                        break;
                    }

                }

            }

        }

        if (objRoomRate.MinAdvBook != 0)
        {
            if (((TimeSpan)dtCalendarDate.Subtract(dtToday)).Days < objRoomRate.MinAdvBook)
                return false;
        }

        if (objRoomRate.MaxAdvBook != 9999)
        {
            if (((TimeSpan)dtCalendarDate.Subtract(dtToday)).Days > objRoomRate.MaxAdvBook)
                return false;
        }

        for (int i = 0; i < objRoomRate.Rates.Length; i++)
        {
            if (objRoomRate.Rates[i].DayNum == intDayID)
            {
                if (objRoomRate.Rates[i].Status == AvailStatus.Open || objRoomRate.Rates[i].Status == AvailStatus.ClosedToArrival)
                    return true;
                else
                    return false;
            }

        }

        return false;
    }

    private string LowestAvailableRate(int intDayID, AvailabilityCalendarInfo[] objAvCalInfo, DateTime dtToday)
    {
        // Returns the lowest available room rate across all calendar instances for day identified

        decimal decLowestAvailableRate = 0;
        string strRateCurrencyCode = "";
        bool bRateLoaded = false;

        for (int si = 0; si < objAvCalInfo.Length; si++)
        {
            for (int i = 0; i < objAvCalInfo[si].AvailabilityCalendar.RoomRates.Length; i++)
            {
                for (int j = 0; j < objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].Rates.Length; j++)
                {
                    if (objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].Rates[j].DayNum == intDayID)
                    {
                        if (this.IsAvCalRoomRateAvailable(intDayID, objAvCalInfo[si].AvailabilityCalendar.RoomRates[i], objAvCalInfo[si].AvailabilityCalendar.RatePlans, objAvCalInfo[si].AvailabilityCalendar.StartDate, dtToday))
                        {
                            if (!bRateLoaded)
                            {
                                decLowestAvailableRate = objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].Rates[j].Amount;
                                bRateLoaded = true;
                            }

                            else
                            {
                                if (objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].Rates[j].Amount < decLowestAvailableRate)
                                    decLowestAvailableRate = objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].Rates[j].Amount;
                            }

                            strRateCurrencyCode = this.RateCurrencyCode(objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].RatePlanCode, objAvCalInfo[si].AvailabilityCalendar);
                            ConvertCurrencyCodeToHtmlEntity(ref strRateCurrencyCode);
                        }

                    }

                }

            }

        }

        if (bRateLoaded)
        {
            return strRateCurrencyCode + decLowestAvailableRate.ToString("F0");
        }

        return "";
    }

    private void ConvertCurrencyCodeToHtmlEntity(ref string currency)
    {
        string currencySymbolCode = string.Format("<span class=\"euro\">{0}</span>", WebconfigHelper.GetCurrencyCodeString(currency));
        currency = currencySymbolCode;
    }

    private bool HasRequestedRates(int intDayID, AvailabilityCalendarInfo[] objAvCalInfo, DateTime dtToday)
    {
        // Returns true if a requested (special) rate is available in any calendar instance for day identified

        for (int si = 0; si < objAvCalInfo.Length; si++)
        {
            for (int i = 0; i < objAvCalInfo[si].AvailabilityCalendar.RoomRates.Length; i++)
            {
                for (int j = 0; j < objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].Rates.Length; j++)
                {
                    if (objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].Rates[j].DayNum == intDayID)
                    {
                        for (int k = 0; k < objAvCalInfo[si].AvailabilityCalendar.RatePlans.Length; k++)
                        {
                            if (objAvCalInfo[si].AvailabilityCalendar.RatePlans[k].RatePlanCode == objAvCalInfo[si].AvailabilityCalendar.RoomRates[i].RatePlanCode)
                            {
                                if (objAvCalInfo[si].AvailabilityCalendar.RatePlans[k].RatePlanType == RatePlanType.Negotiated || objAvCalInfo[si].AvailabilityCalendar.RatePlans[k].RatePlanType == RatePlanType.Consortia)
                                    return true;
                            }

                        }

                    }

                }

            }

        }

        return false;
    }

    private string RateCurrencyCode(string strRatePlanCode, AvailabilityCalendar objAvCal)
    {
        for (int i = 0; i < objAvCal.RatePlans.Length; i++)
        {
            if (objAvCal.RatePlans[i].RatePlanCode == strRatePlanCode)
                return objAvCal.RatePlans[i].CurrencyCode;
        }

        return "";
    }

    private PopUpRate[] GetPopUpRates(int intDayID, AvailabilityCalendar objAvCal, DateTime dtToday)
    {
        // Returns a list comprising of the lowest available room rate within a rate plan (across all room types) within a single calendar instance 

        List<PopUpRate> lPopUpRates = new List<PopUpRate>();

        for (int rri = 0; rri < objAvCal.RoomRates.Length; rri++)
        {
            if (this.IsAvCalRoomRateAvailable(intDayID, objAvCal.RoomRates[rri], objAvCal.RatePlans, objAvCal.StartDate, dtToday))
            {
                PopUpRate objCompareToPopUpRate = null;
                bool bRatePlanStored = false;

                for (int i = 0; i < lPopUpRates.Count; i++)
                {
                    if (lPopUpRates[i].RatePlan.RatePlanCode == objAvCal.RoomRates[rri].RatePlanCode)
                    {
                        objCompareToPopUpRate = lPopUpRates[i];
                        bRatePlanStored = true;
                        break;
                    }

                }

                if (!bRatePlanStored)
                {
                    PopUpRate objPopUpRate = new PopUpRate();
                    lPopUpRates.Add(objPopUpRate);

                    objPopUpRate.RoomRate = objAvCal.RoomRates[rri];

                    for (int i = 0; i < objAvCal.RatePlans.Length; i++)
                    {
                        if (objAvCal.RatePlans[i].RatePlanCode == objAvCal.RoomRates[rri].RatePlanCode)
                        {
                            objPopUpRate.RatePlan = objAvCal.RatePlans[i];
                            break;
                        }

                    }

                    for (int i = 0; i < objAvCal.RoomRates[rri].Rates.Length; i++)
                    {
                        if (objAvCal.RoomRates[rri].Rates[i].DayNum == intDayID)
                        {
                            objPopUpRate.Rate = objAvCal.RoomRates[rri].Rates[i];
                        }

                    }

                }

                else
                {
                    for (int i = 0; i < objAvCal.RoomRates[rri].Rates.Length; i++)
                    {
                        if (objAvCal.RoomRates[rri].Rates[i].DayNum == intDayID)
                        {
                            if (objAvCal.RoomRates[rri].Rates[i].Amount < objCompareToPopUpRate.Rate.Amount)
                            {
                                objCompareToPopUpRate.Rate = objAvCal.RoomRates[rri].Rates[i];
                                break;
                            }

                        }

                    }

                }

            }

        }

        return lPopUpRates.ToArray();
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}

public class PopUpRate
{
    public AvCalRatePlan RatePlan;
    public AvCalRoomRate RoomRate;
    public AvCalRate Rate;
}
