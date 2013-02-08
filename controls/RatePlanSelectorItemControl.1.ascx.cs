using System;
using System.Linq;
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
using System.Globalization;
using MamaShelter;

public partial class RatePlanSelectorItemControl : System.Web.UI.UserControl
{
    public delegate void ShowRoomPhotoEventHandler(string roomTypeCode);
    public event ShowRoomPhotoEventHandler ShowRoomPhotoSelected;

    public delegate void RoomRateSelectedHandler(string roomRefID, string roomTypeCode, string ratePlanCode, string promotionCode);
    public event RoomRateSelectedHandler RoomRateSelected;

    public delegate void EditModeSelectedHandler(string roomRefID, RoomDetailSelectionStep roomDetailStep);
    public event EditModeSelectedHandler EditModeSelected;

    private string _RoomRefID;
    private HotelAvailRatePlan _RatePlan;
    private HotelAvailRoomRate _RoomRate;
    private string[] _CreditCardCodes;
    private bool _Selected;

    public bool IsShowPhotoLink { get; set; }

    public SelectionMode Mode { get; set; }

    public HotelDescRoomType RoomTypeDescription { get; set; }

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

    public decimal TotalRoomRate
    {
        get
        {
            decimal decTotalStayRate = 0;
            foreach (var rate in _RoomRate.Rates)
            {
                decimal decCurStayRate = rate.Amount;
                decimal decCurStayRateTaxesFees = TaxesPerNight(rate.PerNightTaxesFees, rate.NumNights, TaxFeeType.Exclusive) + FeesPerNight(rate.PerNightTaxesFees, rate.NumNights, TaxFeeType.Exclusive);
                decTotalStayRate += ((decCurStayRate + decCurStayRateTaxesFees) * rate.NumNights);
            }
            return decTotalStayRate;
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

            if (strRoomRateCodeSelection == _RoomRate.RoomTypeCode + "_" + _RoomRate.RatePlanCode)
                _Selected = true;
            else
                _Selected = false;
        }

        btnDescription.Attributes.Add("onclick", string.Format("toggleDetail('{0}', this);", panRoomTypeDescription.ClientID));
        lblRateInfoButton.Attributes.Add("onclick", string.Format("toggleDetail('{0}', this);", panRoomTypeRateInfo.ClientID));
        lblPolicyButton.Attributes.Add("onclick", string.Format("toggleDetail('{0}', this);", panRoomTypePolicies.ClientID));
        lblPhotosButton.Attributes.Add("onclick", string.Format("showphoto('{0}', this);", btnShowPhoto.ClientID));

        return;
    }

    public void RenderUserControl()
    {
        lblRoomTypeText.Text = RoomTypeDescription.Name + ": ";
        lblRoomTypeDescription.Text = RoomTypeDescription.ShortDescription;
        lblRatePlanDescriptionText.Text = _RatePlan.ShortDescription;

        if (ConfigurationManager.AppSettings["RatePlanSelectorItemControl.UseRoomRateDescription"] != "1" || _RatePlan.Type == RatePlanType.Negotiated || _RatePlan.Type == RatePlanType.Consortia)
            lblRatePlanNameText.Text = _RatePlan.Name;

        else
            lblRatePlanNameText.Text = _RoomRate.Name;

        if (_RoomRate.Rates.Length <= 0)
            return;

        var lowestRate = (from item in _RoomRate.Rates where item.Amount == _RoomRate.Rates.Min(rate => rate.Amount) select item).FirstOrDefault();
        decimal decLowestStayRate = lowestRate.Amount;
        decimal decLowestStayRateTaxesFees = this.TaxesPerNight(lowestRate.PerNightTaxesFees, lowestRate.NumNights, TaxFeeType.Exclusive) + this.FeesPerNight(lowestRate.PerNightTaxesFees, lowestRate.NumNights, TaxFeeType.Exclusive);

        string strRateCurrencyCode = lowestRate.CurrencyCode;
        string currencyFormat = ((XnGR_WBS_Page)this.Page).CurrencyFormat();
        string strLowestStayRate = string.Format("{0} {1}", decLowestStayRate.ToString(currencyFormat), strRateCurrencyCode);
        string strLowestStayRateTaxesFees = FormatCurrencyString(decLowestStayRateTaxesFees, strRateCurrencyCode);

        decimal decTotalStayRate = 0;

        // Rate Info
        foreach (var rate in _RoomRate.Rates)
        {
            decimal decCurStayRate = rate.Amount;
            decimal decCurStayRateTaxesFees = TaxesPerNight(rate.PerNightTaxesFees, rate.NumNights, TaxFeeType.Exclusive) + FeesPerNight(rate.PerNightTaxesFees, rate.NumNights, TaxFeeType.Exclusive);
            decTotalStayRate += ((decCurStayRate + decCurStayRateTaxesFees) * rate.NumNights);

            string strCurStayRate = FormatCurrencyString(decCurStayRate, strRateCurrencyCode);
            string strCurStayRateTaxesFees = decCurStayRateTaxesFees.ToString(currencyFormat) + " " + strRateCurrencyCode;

            Panel panRateDetail = new Panel();
            panRateDetail.CssClass = "mm_room_type_description";

            Label lblDate = new Label() { Text = string.Format("{0:ddd d-MMM-yyyy}", rate.StartDate), CssClass = "mm_detail_text" };
            Label lblRate = new Label() { Text = strCurStayRate, CssClass = "mm_detail_text" };
            Label lblTaxes = new Label() { Text = strCurStayRateTaxesFees, CssClass = "mm_detail_text" };

            panRateDetail.Controls.Add(lblDate);
            panRateDetail.Controls.Add(new Label() { Text = " : " });
            panRateDetail.Controls.Add(lblRate);
            panRateDetail.Controls.Add(new Label() { Text = " + " });
            panRateDetail.Controls.Add(lblTaxes);
            panRateDetail.Controls.Add(new Label() { Text = string.Format(" {0} ", GetLocalResourceObject("TaxesType")), CssClass = "mm_detail_text" });

            if(rate.NumNights > 1)
            {
                panRateDetail.Controls.Add(new Label
                                               {
                                                   Text = string.Format(" {0} {1} {2}", GetLocalResourceObject("RateNightsPrefix"), rate.NumNights, GetLocalResourceObject("RateNightsSuffix")),
                                                   CssClass = "mm_detail_text"
                                               });
            }

            phRoomTypeRates.Controls.Add(panRateDetail);
        }

        // Policies
        lblGuaranteePolicyText.Text = ((XnGR_WBS_Page)this.Page).GuaranteePolicy(_RatePlan);
        lblCancelPolicyText.Text = ((XnGR_WBS_Page)this.Page).CancellationPolicy(_RatePlan);
        lblDepositPolicyText.Text = ((XnGR_WBS_Page)this.Page).DepositPolicy(_RatePlan);
        lblPaymentPolicyText.Text = _RatePlan.GeneralPaymentPolicy;
        lblAcceptedPaymentCardsText.Text = ((XnGR_WBS_Page)this.Page).AcceptedPaymentCards(_CreditCardCodes);

        if (lblPaymentPolicyText.Text != null && lblPaymentPolicyText.Text != "")
            panPaymentPolicy.Visible = true;
        else
            panPaymentPolicy.Visible = false;

        lblTotalStayRateText.Text = FormatCurrencyString(TotalRoomRate, strRateCurrencyCode);

        // MamaShelter
        if (Mode == SelectionMode.Edit)
        {
            panRatePlanSelectorItem.CssClass = "mm_roomrate_edit mm_background_edit";
            panRoomRatePlan.CssClass = "mm_roomrate_content mm_text_edit";
            lblTotalStayRateText.CssClass = lblTotalStayRateText.CssClass + " mm_roomrate_price_edit";

            panButtonEdit.Visible = false;
            string scriptToApplySpecialStyleForTouchDevice = string.Format("applyStyleForTouchDevice(\"{0}\");", "mm_roomrate_wrapper_button_select");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StyleForTouchDevice_RoomRate_Select", scriptToApplySpecialStyleForTouchDevice, true);
        }
        else
        {
            panRatePlanSelectorItem.CssClass = "mm_roomrate_info mm_background_info";
            panRoomRatePlan.CssClass = "mm_roomrate_content mm_text_info";

            panButtonSelect.Visible = false;
            string scriptToApplySpecialStyleForTouchDevice = string.Format("applyStyleForTouchDevice(\"{0}\");", "mm_roomrate_wrapper_button_edit");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StyleForTouchDevice_RoomRate_Edit", scriptToApplySpecialStyleForTouchDevice, true);
        }
        if ((Mode & SelectionMode.NonModifiable) == SelectionMode.NonModifiable) { 
            panButtonEdit.Visible = false;
        }

        if (!IsShowPhotoLink) {
            spanPhotoSeperator.Visible = false;
            lblPhotosButton.Visible = false;
        }
            

        return;
    }

    protected void btnShowPhoto_Click(object sender, EventArgs e)
    {
        if (ShowRoomPhotoSelected != null)
            ShowRoomPhotoSelected(RoomTypeDescription.Code);
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        if (RoomRateSelected != null)
            RoomRateSelected(RoomRefID, _RoomRate.RoomTypeCode, _RoomRate.RatePlanCode, _RoomRate.PromotionCode);
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (EditModeSelected != null)
            EditModeSelected(RoomRefID, RoomDetailSelectionStep.SelectRoomType);
    }

    private bool IsRateChange(HotelAvailRate[] objHotelAvailRates)
    {
        bool bRateChange = false;

        if (objHotelAvailRates.Length > 1)
        {
            decimal decLastRate = objHotelAvailRates[0].Amount;

            for (int i = 1; i < objHotelAvailRates.Length; i++)
            {
                if (objHotelAvailRates[i].Amount != decLastRate)
                {
                    bRateChange = true;
                    break;
                }

            }

        }

        return bRateChange;
    }

    private decimal TotalRoomRatePerNight(HotelAvailRate objRate)
    {
        decimal decRoomPrice = objRate.Amount;
        decimal decRoomTax = this.TaxesPerNight(objRate.PerNightTaxesFees, objRate.NumNights, TaxFeeType.Exclusive);
        decimal decRoomFee = this.FeesPerNight(objRate.PerNightTaxesFees, objRate.NumNights, TaxFeeType.Exclusive);

        return decRoomPrice + decRoomTax + decRoomFee;
    }

    private decimal FeesPerNight(HotelAvailTaxFee[] objTaxesFees, int intNumNights, TaxFeeType enumTaxFeeType)
    {
        decimal decAmount = 0;

        for (int i = 0; i < objTaxesFees.Length; i++)
        {
            if (objTaxesFees[i].CategoryType == TaxFeeCategoryType.Fee && objTaxesFees[i].Type == enumTaxFeeType)
                decAmount += objTaxesFees[i].Amount;
        }

        return decAmount / intNumNights;
    }

    private decimal TaxesPerNight(HotelAvailTaxFee[] objTaxesFees, int intNumNights, TaxFeeType enumTaxFeeType)
    {
        decimal decAmount = 0;

        for (int i = 0; i < objTaxesFees.Length; i++)
        {
            if (objTaxesFees[i].CategoryType == TaxFeeCategoryType.Tax && objTaxesFees[i].Type == enumTaxFeeType)
                decAmount += objTaxesFees[i].Amount;
        }

        return decAmount / intNumNights;
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

    private string FormatCurrencyString(decimal value, string currencyCode)
    {
        return string.Format("<span class=\"mm_rate_plan_currency_symbol\">{0}</span>{1}",WebconfigHelper.GetCurrencyCodeString(currencyCode), value.ToString("F0"));
    }

}
