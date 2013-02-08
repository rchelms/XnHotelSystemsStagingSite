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

public partial class RatePlanSelectorGridItemControl : System.Web.UI.UserControl
{
    private string _RoomRefID;
    private DateTime _GridStartDate;
    private int _GridNumberDays;
    private HotelAvailRatePlan _RatePlan;
    private HotelAvailRoomRate _RoomRate;
    private AvCalRatePlan _GridRatePlan;
    private AvCalRoomRate _GridRoomRate;
    private DateTime _GridRoomRateStartDate;
    private DateTime _ArrivalDate;
    private int _TotalStayNights;
    private string[] _CreditCardCodes;
    private bool _Available;
    private bool _Selected;

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

    public HotelAvailRatePlan RatePlan
    {
        get
        {
            return _RatePlan;
        }

        set
        {
            _RatePlan = value;
        }

    }

    public HotelAvailRoomRate RoomRate
    {
        get
        {
            return _RoomRate;
        }

        set
        {
            _RoomRate = value;
        }

    }

    public AvCalRatePlan GridRatePlan
    {
        get
        {
            return _GridRatePlan;
        }

        set
        {
            _GridRatePlan = value;
        }

    }

    public AvCalRoomRate GridRoomRate
    {
        get
        {
            return _GridRoomRate;
        }

        set
        {
            _GridRoomRate = value;
        }

    }

    public DateTime GridRoomRateStartDate
    {
        get
        {
            return _GridRoomRateStartDate;
        }

        set
        {
            _GridRoomRateStartDate = value;
        }

    }

    public DateTime ArrivalDate
    {
        get
        {
            return _ArrivalDate;
        }

        set
        {
            _ArrivalDate = value;
        }

    }

    public int TotalStayNights
    {
        get
        {
            return _TotalStayNights;
        }

        set
        {
            _TotalStayNights = value;
        }

    }

    public string[] CreditCardCodes
    {
        get
        {
            return _CreditCardCodes;
        }

        set
        {
            _CreditCardCodes = value;
        }

    }

    public bool Available
    {
        get
        {
            return _Available;
        }

        set
        {
            _Available = value;
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
            string strRoomRateCodeSelection = this.Request.Form.Get("room_rate_code");

            if (strRoomRateCodeSelection == _GridRoomRate.RoomTypeCode + "_" + _GridRoomRate.RatePlanCode)
                _Selected = true;
            else
                _Selected = false;
        }

        return;
    }

    public void RenderUserControl()
    {
        if (_Available)
        {
            panRatePlanAvailable.Visible = true;
            panRatePlanNotAvailable.Visible = false;
            panRatePlanRestricted.Visible = false;
        }

        else
        {
            panRatePlanAvailable.Visible = false;
            panRatePlanNotAvailable.Visible = true;

            panRatePlanRestricted.Visible = true;

            lblRatePlanRestrictionNoticeMessage.Text = (string)GetLocalResourceObject("RatePlanRestrictionNotice");
            imgRatePlanRestricted.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_RatePlanRestrictionNoticePopup'} )");

            for (int di = 0; di < _TotalStayNights; di++)
            {
                AvailStatus enumRateStatus = AvailStatus.Closed;

                DateTime dtStayDate = _ArrivalDate.AddDays(di);

                for (int ri = 0; ri < _GridRoomRate.Rates.Length; ri++)
                {
                    DateTime dtGridRoomRateDate = _GridRoomRateStartDate.AddDays(_GridRoomRate.Rates[ri].DayNum - 1);

                    if (dtGridRoomRateDate.Date == dtStayDate.Date)
                    {
                        enumRateStatus = _GridRoomRate.Rates[ri].Status;
                        break;
                    }

                }

                if (enumRateStatus != AvailStatus.Open && enumRateStatus != AvailStatus.ClosedToArrival)
                {
                    panRatePlanRestricted.Visible = false;
                    break;
                }

            }

        }

        if (ConfigurationManager.AppSettings["RatePlanSelectorGridItemControl.UseRoomRateDescription"] != "1" || _RatePlan.Type == RatePlanType.Negotiated || _RatePlan.Type == RatePlanType.Consortia)
        {
            lblRatePlanNameText.Text = _RatePlan.Name;
            lblRatePlanDescriptionText.Text = this.AddLineBreaks(_RatePlan.ShortDescription);
        }

        else
        {
            lblRatePlanNameText.Text = _GridRoomRate.Name;
            lblRatePlanDescriptionText.Text = this.AddLineBreaks(_GridRoomRate.Description);
        }

        if (_GridRoomRate.FullPriceReference != 0)
            lblFullRateInfoText.Text = ((decimal)(_GridRoomRate.FullPriceReference * _TotalStayNights)).ToString("F0") + "<br />" + _GridRatePlan.CurrencyCode;
        else
            lblFullRateInfoText.Text = "";

        phPopupRoomRateDetails.Controls.Clear();

        lblGuaranteePolicyText.Text = ((XnGR_WBS_Page)this.Page).GuaranteePolicy(_RatePlan);
        lblCancelPolicyText.Text = ((XnGR_WBS_Page)this.Page).CancellationPolicy(_RatePlan);
        lblDepositPolicyText.Text = ((XnGR_WBS_Page)this.Page).DepositPolicy(_RatePlan);
        lblPaymentPolicyText.Text = _RatePlan.GeneralPaymentPolicy;
        lblAcceptedPaymentCardsText.Text = ((XnGR_WBS_Page)this.Page).AcceptedPaymentCards(_CreditCardCodes);

        if (lblPaymentPolicyText.Text != null && lblPaymentPolicyText.Text != "")
            panPaymentPolicy.Visible = true;
        else
            panPaymentPolicy.Visible = false;

        lbViewDescription.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_ViewDescriptionPopup'} )");
        lbViewRates.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_ViewRatesPopup'} )");
        lbViewPolicies.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_ViewPoliciesPopup'} )");

        if (_Available)
        {
            int intDayIndex = 0;
            decimal decTotalStayRate = 0;

            for (int i = 0; i < _RoomRate.Rates.Length; i++)
            {
                for (int j = 0; j < _RoomRate.Rates[i].NumNights; j++)
                {
                    decTotalStayRate += _RoomRate.Rates[i].Amount;

                    Panel panRateDetail = new Panel();
                    panRateDetail.CssClass = "room_rates_popup_detail_block";

                    Panel panDate = new Panel();
                    panDate.CssClass = "room_rates_popup_detail_date room_rates_popup_detail";

                    Panel panRate = new Panel();
                    panRate.CssClass = "room_rates_popup_detail_rate room_rates_popup_detail";

                    Panel panInclusions = new Panel();
                    panInclusions.CssClass = "room_rates_popup_detail_inclusions room_rates_popup_detail";

                    Label lblDate = new Label();
                    Label lblRate = new Label();
                    Label lblInclusions = new Label();

                    lblDate.Text = _ArrivalDate.AddDays(intDayIndex).Date.ToString("ddd dd\"-\"MMM\"-\"yyyy");
                    lblRate.Text = _RoomRate.Rates[i].Amount.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat()) + " " + _RoomRate.Rates[i].CurrencyCode;
                    lblInclusions.Text = _RoomRate.Rates[i].Inclusions;

                    panDate.Controls.Add(lblDate);
                    panRate.Controls.Add(lblRate);
                    panInclusions.Controls.Add(lblInclusions);

                    panRateDetail.Controls.Add(panDate);
                    panRateDetail.Controls.Add(panRate);
                    panRateDetail.Controls.Add(panInclusions);

                    phPopupRoomRateDetails.Controls.Add(panRateDetail);

                    intDayIndex++;
                }

            }

            lblRoomRatePriceText.Text = decTotalStayRate.ToString("F0");

            StringBuilder rb = new StringBuilder();

            rb.Append("<input type=\"radio\" name=\"room_rate_code\" value=\"");
            rb.Append(_GridRoomRate.RoomTypeCode + "_" + _GridRoomRate.RatePlanCode);
            rb.Append("\" ");

            if (_Selected == true)
                rb.Append("checked");

            rb.Append(" />");

            litRatePlanSelector.Text = rb.ToString();
        }

        phRateGridDataItems.Controls.Clear();

        for (int di = 0; di < _GridNumberDays; di++)
        {
            DateTime dtGridDate = _GridStartDate.AddDays(di);

            string strRateGridDataItemControlPath = ConfigurationManager.AppSettings["RateGridDataItemControl.ascx"];
            RateGridDataItemControl ucRateGridDataItemControl = (RateGridDataItemControl)LoadControl(strRateGridDataItemControlPath);
            phRateGridDataItems.Controls.Add(ucRateGridDataItemControl);

            decimal decRateAmount = 0;
            string strInclusions = "";
            string strRestrictions = "";
            AvailStatus enumRateStatus = AvailStatus.Closed;

            for (int ri = 0; ri < _GridRoomRate.Rates.Length; ri++)
            {
                DateTime dtGridRoomRateDate = _GridRoomRateStartDate.AddDays(_GridRoomRate.Rates[ri].DayNum - 1);

                if (dtGridRoomRateDate.Date == dtGridDate.Date)
                {
                    decRateAmount = _GridRoomRate.Rates[ri].Amount;
                    strInclusions = _GridRoomRate.Rates[ri].Inclusions;
                    strRestrictions = ((XnGR_WBS_Page)this.Page).RateRestrictions(_GridRoomRate, _GridRoomRate.Rates[ri]);
                    enumRateStatus = _GridRoomRate.Rates[ri].Status;

                    break;
                }

            }

            ucRateGridDataItemControl.ID = "GridDataItem" + di.ToString();
            ucRateGridDataItemControl.ItemText = decRateAmount.ToString("F0") + "<br />" + _GridRatePlan.CurrencyCode;

            if (strRestrictions.Length > 0)
            {
                if (strInclusions.Length > 0)
                {
                    ucRateGridDataItemControl.ItemPopupText = strInclusions + "\n\n" + strRestrictions;
                }

                else
                {
                    ucRateGridDataItemControl.ItemPopupText = strRestrictions;
                }

            }

            else
            {
                ucRateGridDataItemControl.ItemPopupText = strInclusions;
            }

            bool bIsStayDate = false;

            if (dtGridDate.Date >= _ArrivalDate.Date && dtGridDate.Date < _ArrivalDate.AddDays(_TotalStayNights).Date)
                bIsStayDate = true;

            if ((enumRateStatus == AvailStatus.Open || enumRateStatus == AvailStatus.ClosedToArrival) && _Available && bIsStayDate)
            {
                ucRateGridDataItemControl.ItemStatus = RateGridItemStatus.Selected;
            }

            else if ((enumRateStatus == AvailStatus.Open || enumRateStatus == AvailStatus.ClosedToArrival) && ((int)dtGridDate.DayOfWeek >= 1 && (int)dtGridDate.DayOfWeek <= 5))
            {
                ucRateGridDataItemControl.ItemStatus = RateGridItemStatus.Weekday;
            }

            else if ((enumRateStatus == AvailStatus.Open || enumRateStatus == AvailStatus.ClosedToArrival) && ((int)dtGridDate.DayOfWeek == 0 || (int)dtGridDate.DayOfWeek == 6))
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

    private  string AddLineBreaks(string strString)
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
