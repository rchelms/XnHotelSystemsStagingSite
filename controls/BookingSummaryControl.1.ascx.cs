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

public partial class BookingSummaryControl : System.Web.UI.UserControl
{
    private HotelDescriptiveInfo _HotelDescriptiveInfo;
    private StayCriteriaSelection _StayCriteriaSelection;
    private GuestDetailsEntryInfo _GuestDetailsEntryInfo;
    private HotelPricing[] _HotelPricings;
    private OnlinePaymentReceipt _PaymentReceipt;
    private string _MasterConfirmationNumber;

    private List<BookingSummaryRoomItemControl> lBookingSummaryRoomItemControls;

    public HotelDescriptiveInfo HotelDescriptiveInfo
    {
        set
        {
            _HotelDescriptiveInfo = value;
        }

        get
        {
            return _HotelDescriptiveInfo;
        }

    }

    public StayCriteriaSelection StayCriteriaSelection
    {
        set
        {
            _StayCriteriaSelection = value;
        }

        get
        {
            return _StayCriteriaSelection;
        }

    }

    public GuestDetailsEntryInfo GuestDetailsEntryInfo
    {
        set
        {
            _GuestDetailsEntryInfo = value;
        }

        get
        {
            return _GuestDetailsEntryInfo;
        }

    }

    public HotelPricing[] HotelPricings
    {
        set
        {
            _HotelPricings = value;
        }

        get
        {
            return _HotelPricings;
        }

    }

    public OnlinePaymentReceipt PaymentReceipt
    {
        set
        {
            _PaymentReceipt = value;
        }

        get
        {
            return _PaymentReceipt;
        }

    }

    public string MasterConfirmationNumber
    {
        set
        {
            _MasterConfirmationNumber = value;
        }

        get
        {
            return _MasterConfirmationNumber;
        }

    }

    public BookingSummaryRoomItemControl[] RoomSummaryItems
    {
        get
        {
            if (lBookingSummaryRoomItemControls != null)
                return lBookingSummaryRoomItemControls.ToArray();
            else
                return new BookingSummaryRoomItemControl[0];
        }

    }

    public void Clear()
    {
        lBookingSummaryRoomItemControls = null;
        return;
    }

    public void AddRoomSummaryItem(BookingSummaryRoomItemControl ucBookingSummaryRoomItemControl)
    {
        if (lBookingSummaryRoomItemControls == null)
        {
            lBookingSummaryRoomItemControls = new List<BookingSummaryRoomItemControl>();
        }

        lBookingSummaryRoomItemControls.Add(ucBookingSummaryRoomItemControl);
        return;
    }

    public void RenderUserControl()
    {
        this.ApplyControlsToPage();

        for (int i = 0; i < _HotelDescriptiveInfo.Images.Length; i++)
        {
            if (_HotelDescriptiveInfo.Images[i].CategoryCode == HotelImageCategoryCode.ExteriorView && _HotelDescriptiveInfo.Images[i].ImageSize == HotelImageSize.Thumbnail)
            {
                imgHotel.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _HotelDescriptiveInfo.Images[i].ImageURL;
            }

        }

        lblHotelNameText.Text = _HotelDescriptiveInfo.HotelName;

        StringBuilder sbAddressAddressInfo = new StringBuilder();

        if (_HotelDescriptiveInfo.Address1.Trim() != "")
        {
            sbAddressAddressInfo.Append(_HotelDescriptiveInfo.Address1);
        }

        if (_HotelDescriptiveInfo.Address2.Trim() != "")
        {
            if (sbAddressAddressInfo.ToString().Trim() != "")
            {
                sbAddressAddressInfo.Append(", ");
            }

            sbAddressAddressInfo.Append(_HotelDescriptiveInfo.Address2);
        }

        if (_HotelDescriptiveInfo.City.Trim() != "")
        {
            if (sbAddressAddressInfo.ToString().Trim() != "")
            {
                sbAddressAddressInfo.Append(", ");
            }

            sbAddressAddressInfo.Append(_HotelDescriptiveInfo.City);
        }

        if (_HotelDescriptiveInfo.PostalCode.Trim() != "" || _HotelDescriptiveInfo.Country.Trim()!="")
        {
            if (sbAddressAddressInfo.ToString().Trim() != "")
            {
                sbAddressAddressInfo.Append(", ");
            }

            sbAddressAddressInfo.Append(_HotelDescriptiveInfo.PostalCode.Trim() + " " + _HotelDescriptiveInfo.Country.Trim());
        }

        lblHotelAddressInfo.Text = sbAddressAddressInfo.ToString();
        lblTelephoneNumber.Text = _HotelDescriptiveInfo.Phone;
        lblFaxNumber.Text = _HotelDescriptiveInfo.Fax;
        lblEmailAddress.Text = _HotelDescriptiveInfo.Email;
        lblCompanyRegNumberInfo.Text = _HotelDescriptiveInfo.CompanyRegNumber;

        if (_HotelDescriptiveInfo.CompanyRegNumber != null && _HotelDescriptiveInfo.CompanyRegNumber != "")
            panCompanyRegNumber.Visible = true;
        else
            panCompanyRegNumber.Visible = false;

        phHotelDescription.Controls.Clear();

        string strHotelDescriptionControlPath = ConfigurationManager.AppSettings["HotelDescriptionControl.ascx"];
        HotelDescriptionControl ucHotelDescriptionControl = (HotelDescriptionControl)LoadControl(strHotelDescriptionControlPath);
        phHotelDescription.Controls.Add(ucHotelDescriptionControl);

        ucHotelDescriptionControl.HotelDescriptiveInfo = _HotelDescriptiveInfo;
        ucHotelDescriptionControl.HotelDescriptionOnly = false;
        ucHotelDescriptionControl.RenderUserControl();

        phHotelRating.Controls.Clear();

        string strHotelRatingControlPath = ConfigurationManager.AppSettings["HotelRatingControl.ascx"];
        HotelRatingControl ucHotelRatingControl = (HotelRatingControl)LoadControl(strHotelRatingControlPath);
        phHotelRating.Controls.Add(ucHotelRatingControl);

        ucHotelRatingControl.RatingProvider = _HotelDescriptiveInfo.RatingProvider;
        ucHotelRatingControl.Rating = _HotelDescriptiveInfo.Rating;
        ucHotelRatingControl.RatingSymbol = _HotelDescriptiveInfo.RatingSymbol;
        ucHotelRatingControl.RenderUserControl();

        phPaymentReceipt.Controls.Clear();

        string strPaymentReceiptControlPath = ConfigurationManager.AppSettings["PaymentReceiptControl.ascx"];
        PaymentReceiptControl ucPaymentReceiptControl = (PaymentReceiptControl)LoadControl(strPaymentReceiptControlPath);
        phPaymentReceipt.Controls.Add(ucPaymentReceiptControl);

        ucPaymentReceiptControl.PaymentReceipt = _PaymentReceipt;

        if (_PaymentReceipt != null)
        {
            ucPaymentReceiptControl.RenderUserControl();
            panPaymentReceipt.Visible = true;
        }

        else
        {
            panPaymentReceipt.Visible = false;
        }

        lblArrivalDateText.Text = _StayCriteriaSelection.ArrivalDate.ToString("dddd, dd MMMM yyyy");
        lblDepartureDateText.Text = _StayCriteriaSelection.DepartureDate.ToString("dddd, dd MMMM yyyy");

        if (_MasterConfirmationNumber != null && _MasterConfirmationNumber != "")
        {
            lblBookingConfirmationNumberText1.Text = _MasterConfirmationNumber;
            lblBookingConfirmationNumberText2.Text = _MasterConfirmationNumber;
            panBookingConfirmationNumber.Visible = true;
            panBookingConfirmationNumberLarge.Visible = true;
        }

        else
        {
            lblBookingConfirmationNumberText1.Text = "";
            lblBookingConfirmationNumberText2.Text = "";
            panBookingConfirmationNumber.Visible = false;
            panBookingConfirmationNumberLarge.Visible = false;
        }

        decimal decTotalBookingCost = 0;
        decimal decTotalDepositAmount = 0;
        string strCurrencyCode = "";

        for (int i = 0; i < _HotelPricings.Length; i++)
        {
            decTotalBookingCost += _HotelPricings[i].TotalAmount;
            decTotalDepositAmount += _HotelPricings[i].TotalDeposit;
            strCurrencyCode = _HotelPricings[i].CurrencyCode;
        }

        StringBuilder sbTotalBookingCost = new StringBuilder();

        sbTotalBookingCost.Append(strCurrencyCode);
        sbTotalBookingCost.Append(" ");
        sbTotalBookingCost.Append(decTotalBookingCost.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat()));

        lblTotalRoomsPriceText.Text = sbTotalBookingCost.ToString();

        if (decTotalDepositAmount != 0)
        {
            StringBuilder sbTotalDepositAmount = new StringBuilder();

            sbTotalDepositAmount.Append(strCurrencyCode);
            sbTotalDepositAmount.Append(" ");
            sbTotalDepositAmount.Append(decTotalDepositAmount.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat()));

            lblTotalRoomsDepositText.Text = sbTotalDepositAmount.ToString();

            panTotalRoomDeposit.Visible = true;
        }

        else
        {
            panTotalRoomDeposit.Visible = false;
        }

        for (int i = 0; i < lBookingSummaryRoomItemControls.Count; i++)
            lBookingSummaryRoomItemControls[i].RenderUserControl();

        return;
    }

    private void ApplyControlsToPage()
    {
        if (lBookingSummaryRoomItemControls == null)
        {
            lBookingSummaryRoomItemControls = new List<BookingSummaryRoomItemControl>();
        }

        phRooms.Controls.Clear();

        for (int i = 0; i < lBookingSummaryRoomItemControls.Count; i++)
            phRooms.Controls.Add(lBookingSummaryRoomItemControls[i]);

        return;
    }

}
