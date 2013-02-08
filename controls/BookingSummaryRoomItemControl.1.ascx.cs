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

public partial class BookingSummaryRoomItemControl : System.Web.UI.UserControl
{
    private string _RoomRefID;
    private RoomOccupantSelection _RoomOccupantSelection;
    private HotelDescRoomType _RoomType;
    private HotelAvailRatePlan _RatePlan;
    private HotelAvailRoomRate _RoomRate;
    private HotelPricing _HotelPricing;
    private string _ConfirmationNumber;

    private List<BookingSummaryAddOnPackageItemControl> lBookingSummaryAddOnPackageItemControls;

    public string RoomRefID
    {
        set
        {
            _RoomRefID = value;
        }

        get
        {
            return _RoomRefID;
        }

    }

    public RoomOccupantSelection RoomOccupantSelection
    {
        set
        {
            _RoomOccupantSelection = value;
        }

        get
        {
            return _RoomOccupantSelection;
        }

    }

    public HotelDescRoomType RoomType
    {
        set
        {
            _RoomType = value;
        }

        get
        {
            return _RoomType;
        }

    }

    public HotelAvailRatePlan RatePlan
    {
        set
        {
            _RatePlan = value;
        }

        get
        {
            return _RatePlan;
        }

    }

    public HotelAvailRoomRate RoomRate
    {
        set
        {
            _RoomRate = value;
        }

        get
        {
            return _RoomRate;
        }

    }

    public HotelPricing HotelPricing
    {
        set
        {
            _HotelPricing = value;
        }

        get
        {
            return _HotelPricing;
        }

    }

    public string ConfirmationNumber
    {
        set
        {
            _ConfirmationNumber = value;
        }

        get
        {
            return _ConfirmationNumber;
        }

    }

    public BookingSummaryAddOnPackageItemControl[] AddOnPackageSummaryItems
    {
        get
        {
            if (lBookingSummaryAddOnPackageItemControls != null)
                return lBookingSummaryAddOnPackageItemControls.ToArray();
            else
                return new BookingSummaryAddOnPackageItemControl[0];
        }

    }

    public void Clear()
    {
        lBookingSummaryAddOnPackageItemControls = null;
        return;
    }

    public void AddAddOnPackageSummaryItem(BookingSummaryAddOnPackageItemControl ucBookingSummaryAddOnPackageItemControl)
    {
        if (lBookingSummaryAddOnPackageItemControls == null)
        {
            lBookingSummaryAddOnPackageItemControls = new List<BookingSummaryAddOnPackageItemControl>();
        }

        lBookingSummaryAddOnPackageItemControls.Add(ucBookingSummaryAddOnPackageItemControl);
        return;
    }

    public void RenderUserControl()
    {
        this.ApplyControlsToPage();

        lblRoomIdentifier.Text = (String)GetLocalResourceObject("RoomIdentifierText" + _RoomRefID);

        StringBuilder sbRoomOccupantsInfo = new StringBuilder();

        sbRoomOccupantsInfo.Append(_RoomOccupantSelection.NumberAdults.ToString());
        sbRoomOccupantsInfo.Append(" ");
        sbRoomOccupantsInfo.Append((String)GetLocalResourceObject("AdultsInfo"));

        if (_RoomOccupantSelection.NumberChildren != 0)
        {
            sbRoomOccupantsInfo.Append(", ");
            sbRoomOccupantsInfo.Append(_RoomOccupantSelection.NumberChildren.ToString());
            sbRoomOccupantsInfo.Append(" ");
            sbRoomOccupantsInfo.Append((String)GetLocalResourceObject("ChildrenInfo"));
        }

        lblRoomOccupantsInfo.Text = sbRoomOccupantsInfo.ToString();

        lblRoomTypeNameText.Text = _RoomType.Name;
        lblRoomTypeDescriptionText.Text = _RoomType.ShortDescription;

        if (ConfigurationManager.AppSettings["BookingSummaryRoomItemControl.UseRoomRateDescription"] != "1" || _RatePlan.Type == RatePlanType.Negotiated || _RatePlan.Type == RatePlanType.Consortia)
        {
            lblRatePlanNameText.Text = _RatePlan.Name;
            lblRatePlanDescriptionText.Text = _RatePlan.ShortDescription;
        }

        else
        {
            lblRatePlanNameText.Text = _RoomRate.Name;
            lblRatePlanDescriptionText.Text = _RoomRate.Description;
        }

        lblGuaranteePolicyText.Text = ((XnGR_WBS_Page)this.Page).GuaranteePolicy(_RatePlan);
        lblCancelPolicyText.Text = ((XnGR_WBS_Page)this.Page).CancellationPolicy(_RatePlan);
        lblDepositPolicyText.Text = ((XnGR_WBS_Page)this.Page).DepositPolicy(_RatePlan);
        lblPaymentPolicyText.Text = _RatePlan.GeneralPaymentPolicy;

        if (lblPaymentPolicyText.Text != null && lblPaymentPolicyText.Text != "")
            panPaymentPolicy.Visible = true;
        else
            panPaymentPolicy.Visible = false;

        StringBuilder sbTotalRoomPrice = new StringBuilder();

        sbTotalRoomPrice.Append(_HotelPricing.CurrencyCode);
        sbTotalRoomPrice.Append(" ");
        sbTotalRoomPrice.Append(_HotelPricing.TotalRoomAmount.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat()));

        lblTotalRoomPrice.Text = sbTotalRoomPrice.ToString();

        for (int i = 0; i < lBookingSummaryAddOnPackageItemControls.Count; i++)
            lBookingSummaryAddOnPackageItemControls[i].RenderUserControl();

        return;
    }

    private void ApplyControlsToPage()
    {
        if (lBookingSummaryAddOnPackageItemControls == null)
        {
            lBookingSummaryAddOnPackageItemControls = new List<BookingSummaryAddOnPackageItemControl>();
        }

        phAddOnPackages.Controls.Clear();

        for (int i = 0; i < lBookingSummaryAddOnPackageItemControls.Count; i++)
            phAddOnPackages.Controls.Add(lBookingSummaryAddOnPackageItemControls[i]);

        phTotalPricing.Controls.Clear();

        string strRoomRateTotalPricingControlPath = ConfigurationManager.AppSettings["RoomRateTotalPricingControl.ascx"];
        RoomRateTotalPricingControl ucRoomRateTotalPricingControl = (RoomRateTotalPricingControl)LoadControl(strRoomRateTotalPricingControlPath);

        phTotalPricing.Controls.Add(ucRoomRateTotalPricingControl);

        string strRoomRateTotalPricingItemControlPath = ConfigurationManager.AppSettings["RoomRateTotalPricingItemControl.ascx"];

        for (int i = 0; i < _RoomRate.Rates.Length; i++)
        {
            for (int j = 0; j < _RoomRate.Rates[i].NumNights; j++)
            {
                RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_Room = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);

                ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_Room);

                ucRoomRateTotalPricingItemControl_Room.ItemType = TotalPricingItemType.RoomRate;
                ucRoomRateTotalPricingItemControl_Room.PriceType = TotalPricingPriceType.PerNight;

                decimal decRoomPrice = _RoomRate.Rates[i].Amount;
                decimal decRoomTax = this.TaxesPerNight(_RoomRate.Rates[i].PerNightTaxesFees, _RoomRate.Rates[i].NumNights, TaxFeeType.Exclusive);
                decimal decRoomFee = this.FeesPerNight(_RoomRate.Rates[i].PerNightTaxesFees, _RoomRate.Rates[i].NumNights, TaxFeeType.Exclusive);
                decimal decRoomTotal = decRoomPrice + decRoomTax + decRoomFee;

                ucRoomRateTotalPricingItemControl_Room.Date = _RoomRate.Rates[i].StartDate.AddDays(j).ToString("ddd dd\"-\"MMM\"-\"yyyy");
                ucRoomRateTotalPricingItemControl_Room.Description = _RoomType.Name;
                ucRoomRateTotalPricingItemControl_Room.Price = _RoomRate.Rates[i].CurrencyCode + " " + decRoomPrice.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                ucRoomRateTotalPricingItemControl_Room.Tax = _RoomRate.Rates[i].CurrencyCode + " " + decRoomTax.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                ucRoomRateTotalPricingItemControl_Room.Fee = _RoomRate.Rates[i].CurrencyCode + " " + decRoomFee.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                ucRoomRateTotalPricingItemControl_Room.Subtotal = _RoomRate.Rates[i].CurrencyCode + " " + decRoomTotal.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());

                for (int k = 0; k < lBookingSummaryAddOnPackageItemControls.Count; k++)
                {
                    if (lBookingSummaryAddOnPackageItemControls[k].PackageRate.PriceType == PackagePriceType.PerStayPerNight || lBookingSummaryAddOnPackageItemControls[k].PackageRate.PriceType == PackagePriceType.PerPersonPerNight)
                    {
                        RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_Package = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);
                        ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_Package);

                        ucRoomRateTotalPricingItemControl_Package.ItemType = TotalPricingItemType.AddOnPackage;
                        ucRoomRateTotalPricingItemControl_Package.PriceType = TotalPricingPriceType.PerNight;

                        decimal decPackagePrice = (decimal)(lBookingSummaryAddOnPackageItemControls[k].PackageQuantity * lBookingSummaryAddOnPackageItemControls[k].PackageRate.Price);
                        decimal decPackageTax = 0;
                        decimal decPackageFee = 0;
                        decimal decPackageTotal = decPackagePrice + decPackageTax + decPackageFee;

                        ucRoomRateTotalPricingItemControl_Package.Date = "";
                        ucRoomRateTotalPricingItemControl_Package.Description = lBookingSummaryAddOnPackageItemControls[k].PackageRate.Name;
                        ucRoomRateTotalPricingItemControl_Package.Price = lBookingSummaryAddOnPackageItemControls[k].PackageRate.CurrencyCode + " " + decPackagePrice.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                        ucRoomRateTotalPricingItemControl_Package.Tax = lBookingSummaryAddOnPackageItemControls[k].PackageRate.CurrencyCode + " " + decPackageTax.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                        ucRoomRateTotalPricingItemControl_Package.Fee = lBookingSummaryAddOnPackageItemControls[k].PackageRate.CurrencyCode + " " + decPackageFee.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                        ucRoomRateTotalPricingItemControl_Package.Subtotal = lBookingSummaryAddOnPackageItemControls[k].PackageRate.CurrencyCode + " " + decPackageTotal.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                    }

                }

            }

        }

        for (int i = 0; i < lBookingSummaryAddOnPackageItemControls.Count; i++)
        {
            if (lBookingSummaryAddOnPackageItemControls[i].PackageRate.PriceType == PackagePriceType.PerStay || lBookingSummaryAddOnPackageItemControls[i].PackageRate.PriceType == PackagePriceType.PerPerson)
            {
                RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_Package_Stay = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);
                ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_Package_Stay);

                ucRoomRateTotalPricingItemControl_Package_Stay.ItemType = TotalPricingItemType.AddOnPackage;
                ucRoomRateTotalPricingItemControl_Package_Stay.PriceType = TotalPricingPriceType.PerStay;

                decimal decPackagePrice = (decimal)(lBookingSummaryAddOnPackageItemControls[i].PackageQuantity * lBookingSummaryAddOnPackageItemControls[i].PackageRate.Price);
                decimal decPackageTax = 0;
                decimal decPackageFee = 0;
                decimal decPackageTotal = decPackagePrice + decPackageTax + decPackageFee;

                ucRoomRateTotalPricingItemControl_Package_Stay.Date = "";
                ucRoomRateTotalPricingItemControl_Package_Stay.Description = lBookingSummaryAddOnPackageItemControls[i].PackageRate.Name;
                ucRoomRateTotalPricingItemControl_Package_Stay.Price = lBookingSummaryAddOnPackageItemControls[i].PackageRate.CurrencyCode + " " + decPackagePrice.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                ucRoomRateTotalPricingItemControl_Package_Stay.Tax = lBookingSummaryAddOnPackageItemControls[i].PackageRate.CurrencyCode + " " + decPackageTax.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                ucRoomRateTotalPricingItemControl_Package_Stay.Fee = lBookingSummaryAddOnPackageItemControls[i].PackageRate.CurrencyCode + " " + decPackageFee.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
                ucRoomRateTotalPricingItemControl_Package_Stay.Subtotal = lBookingSummaryAddOnPackageItemControls[i].PackageRate.CurrencyCode + " " + decPackageTotal.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());
            }

        }

        RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_TotalRoom = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);
        ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_TotalRoom);

        ucRoomRateTotalPricingItemControl_TotalRoom.ItemType = TotalPricingItemType.Total;
        ucRoomRateTotalPricingItemControl_TotalRoom.PriceType = TotalPricingPriceType.Total;

        ucRoomRateTotalPricingItemControl_TotalRoom.Date = "";
        ucRoomRateTotalPricingItemControl_TotalRoom.Description = (String)GetLocalResourceObject("TotalRoomCost");
        ucRoomRateTotalPricingItemControl_TotalRoom.Subtotal = _HotelPricing.CurrencyCode + " " + _HotelPricing.TotalRoomAmount.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());

        RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_TotalAddOn = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);
        ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_TotalAddOn);

        ucRoomRateTotalPricingItemControl_TotalAddOn.ItemType = TotalPricingItemType.Total;
        ucRoomRateTotalPricingItemControl_TotalAddOn.PriceType = TotalPricingPriceType.Total;

        ucRoomRateTotalPricingItemControl_TotalAddOn.Date = "";
        ucRoomRateTotalPricingItemControl_TotalAddOn.Description = (String)GetLocalResourceObject("TotalAddOnCost");
        ucRoomRateTotalPricingItemControl_TotalAddOn.Subtotal = _HotelPricing.CurrencyCode + " " + _HotelPricing.TotalPackageAmount.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());

        RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_TotalAddTax = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);
        ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_TotalAddTax);

        ucRoomRateTotalPricingItemControl_TotalAddTax.ItemType = TotalPricingItemType.Total;
        ucRoomRateTotalPricingItemControl_TotalAddTax.PriceType = TotalPricingPriceType.Total;

        ucRoomRateTotalPricingItemControl_TotalAddTax.Date = "";
        ucRoomRateTotalPricingItemControl_TotalAddTax.Description = (String)GetLocalResourceObject("TotalAddTaxCost");
        ucRoomRateTotalPricingItemControl_TotalAddTax.Subtotal = _HotelPricing.CurrencyCode + " " + _HotelPricing.TotalAdditionalTaxes.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());

        RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_TotalAddFee = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);
        ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_TotalAddFee);

        ucRoomRateTotalPricingItemControl_TotalAddFee.ItemType = TotalPricingItemType.Total;
        ucRoomRateTotalPricingItemControl_TotalAddFee.PriceType = TotalPricingPriceType.Total;

        ucRoomRateTotalPricingItemControl_TotalAddFee.Date = "";
        ucRoomRateTotalPricingItemControl_TotalAddFee.Description = (String)GetLocalResourceObject("TotalAddFeeCost");
        ucRoomRateTotalPricingItemControl_TotalAddFee.Subtotal = _HotelPricing.CurrencyCode + " " + _HotelPricing.TotalAdditionalFees.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());

        RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_Total = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);
        ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_Total);

        ucRoomRateTotalPricingItemControl_Total.ItemType = TotalPricingItemType.Total;
        ucRoomRateTotalPricingItemControl_Total.PriceType = TotalPricingPriceType.Total;

        ucRoomRateTotalPricingItemControl_Total.Date = "";
        ucRoomRateTotalPricingItemControl_Total.Description = (String)GetLocalResourceObject("TotalCost");
        ucRoomRateTotalPricingItemControl_Total.Subtotal = _HotelPricing.CurrencyCode + " " + _HotelPricing.TotalAmount.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());

        RoomRateTotalPricingItemControl ucRoomRateTotalPricingItemControl_Deposit = (RoomRateTotalPricingItemControl)LoadControl(strRoomRateTotalPricingItemControlPath);
        ucRoomRateTotalPricingControl.AddRoomRateTotalPricingItem(ucRoomRateTotalPricingItemControl_Deposit);

        ucRoomRateTotalPricingItemControl_Deposit.ItemType = TotalPricingItemType.Total;
        ucRoomRateTotalPricingItemControl_Deposit.PriceType = TotalPricingPriceType.Total;

        ucRoomRateTotalPricingItemControl_Deposit.Date = "";
        ucRoomRateTotalPricingItemControl_Deposit.Description = (String)GetLocalResourceObject("TotalDeposit");
        ucRoomRateTotalPricingItemControl_Deposit.Subtotal = _HotelPricing.CurrencyCode + " " + _HotelPricing.TotalDeposit.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat());

        ucRoomRateTotalPricingControl.RenderUserControl();

        return;
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

    private decimal Fees(HotelAvailTaxFee[] objTaxesFees, TaxFeeType enumTaxFeeType)
    {
        decimal decAmount = 0;

        for (int i = 0; i < objTaxesFees.Length; i++)
        {
            if (objTaxesFees[i].CategoryType == TaxFeeCategoryType.Fee && objTaxesFees[i].Type == enumTaxFeeType)
                decAmount += objTaxesFees[i].Amount;
        }

        return decAmount;
    }

    private decimal Taxes(HotelAvailTaxFee[] objTaxesFees, TaxFeeType enumTaxFeeType)
    {
        decimal decAmount = 0;

        for (int i = 0; i < objTaxesFees.Length; i++)
        {
            if (objTaxesFees[i].CategoryType == TaxFeeCategoryType.Tax && objTaxesFees[i].Type == enumTaxFeeType)
                decAmount += objTaxesFees[i].Amount;
        }

        return decAmount;
    }

}

