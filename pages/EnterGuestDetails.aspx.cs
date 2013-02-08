using System;
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

public partial class EnterGuestDetails : XnGR_WBS_Page
{
    private ProfileLoginNameControl ucProfileLoginNameControl;
    private LanguageSelectorControl ucLanguageSelectorControl;
    private BookingStepControl ucBookingStepControl;
    private ErrorDisplayControl ucErrorDisplayControl;
    private BookingSummaryControl ucBookingSummaryControl;
    private GuestDetailsEntryControl ucGuestDetailsEntryControl;
    private TrackingCodeControl ucHeadTrackingCodeControl;
    private TrackingCodeControl ucBodyTrackingCodeControl;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        this.LoadLanguageSelectorControl();
        this.LoadProfileLoginNameControl();
        this.LoadBookingStepControl();
        this.LoadErrorDisplayControl();
        this.LoadBookingSummaryControl();
        this.LoadGuestDetailsEntryControl();

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            Session["SelectedRoom"] = "1";

            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
            RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
            AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];
            HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];
            HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];

            string strCurrencyCode = "";

            if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length > 0)
                strCurrencyCode = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0].CurrencyCode;

            PaymentGatewayInfo[] objPaymentGatewayInfos = WBSPGHelper.GetPaymentGatewayInfos(objStayCriteriaSelection.HotelCode);
            Session[Constants.Sessions.PaymentGatewayInfos] = objPaymentGatewayInfos;

            if (objPaymentGatewayInfos.Length == 1)
                Session[Constants.Sessions.PaymentGatewayInfo] = objPaymentGatewayInfos[0];
            else
                Session[Constants.Sessions.PaymentGatewayInfo] = null; // if multiple gateways configured, payment gateway must be selected by method of payment

            Session[Constants.Sessions.HotelBookingPaymentAllocations] = WBSPGHelper.GetPaymentAllocations(HotelPricingHelper.GetHotelPricing(objStayCriteriaSelection, objRoomRateSelections, objAddOnPackageSelections, objHotelAvailabilityRS.HotelRoomAvailInfos, strCurrencyCode));

            Session["HotelPaymentRQ"] = null;
            Session["HotelPaymentRS"] = null;
        }

        else
        {
            this.ConfigureLanguageSelectorControl();
            this.ConfigureProfileLoginNameControl();
            this.ConfigureBookingStepControl();
            this.ConfigureErrorDisplayControl();
            this.ConfigureBookingSummaryControl();
            this.ConfigureGuestDetailsEntryControl();
        }

        return;
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        this.IsParentPreRender = true;

        this.ConfigureLanguageSelectorControl();
        this.ConfigureProfileLoginNameControl();
        this.ConfigureBookingStepControl();
        this.ConfigureErrorDisplayControl();
        this.ConfigureBookingSummaryControl();
        this.ConfigureGuestDetailsEntryControl();
        this.ConfigureTrackingCodeControl();

        ucLanguageSelectorControl.RenderUserControl();
        ucProfileLoginNameControl.RenderUserControl();
        ucBookingStepControl.RenderUserControl();
        ucErrorDisplayControl.RenderUserControl();
        ucBookingSummaryControl.RenderUserControl();
        ucGuestDetailsEntryControl.RenderUserControl();
        ucHeadTrackingCodeControl.RenderUserControl();
        ucBodyTrackingCodeControl.RenderUserControl();

        this.PageComplete();

        return;
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
        WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
        WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect("~/Pages/EnterGuestDetails.aspx");

        return;
    }

    protected void BookingStepSelected(object sender, EventArgs e)
    {
        if (((BookingStepControl)sender).SelectedStep == "SearchHotel")
        {
            Response.Redirect("~/Pages/SearchHotel.aspx");
        }

        if (((BookingStepControl)sender).SelectedStep == "ChooseHotel")
        {
            if (ConfigurationManager.AppSettings["EnableHotelSearch"] == "1")
            {
                Response.Redirect("~/Pages/SelectSearchHotel.aspx");
            }

            else
            {
                Response.Redirect("~/Pages/SelectHotel.aspx");
            }

        }

        if (((BookingStepControl)sender).SelectedStep == "ChooseRate")
        {
            Response.Redirect("~/Pages/SelectRoom.aspx");
        }

        if (((BookingStepControl)sender).SelectedStep == "ChooseYourExtras")
        {
            Response.Redirect("~/Pages/SelectAddOn.aspx");
        }

        if (((BookingStepControl)sender).SelectedStep == "EnterYourDetails")
        {
            Response.Redirect("~/Pages/EnterGuestDetails.aspx");
        }

        return;
    }

    protected void PaymentGatewayPreSelectCompleted(object sender, EventArgs e)
    {
        Session[Constants.Sessions.PaymentGatewayInfo] = ((GuestDetailsEntryControl)sender).SelectedPaymentGateway;
        return;
    }

    protected void GuestDetailsCompleted(object sender, EventArgs e)
    {
        PaymentGatewayInfo[] objPaymentGatewayInfos = (PaymentGatewayInfo[])Session[Constants.Sessions.PaymentGatewayInfos];
        HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations = (HotelBookingPaymentAllocation[])Session[Constants.Sessions.HotelBookingPaymentAllocations];

        this.ValidateGuestDetails(ucGuestDetailsEntryControl.GuestDetailsEntryInfo, ucGuestDetailsEntryControl.TermsConditionsAccepted);

        Session["GuestDetailsEntryInfo"] = ucGuestDetailsEntryControl.GuestDetailsEntryInfo;
        Session["BookingTermsConditionsAccepted"] = ucGuestDetailsEntryControl.TermsConditionsAccepted;
        Session[Constants.Sessions.PaymentGatewayInfo] = ucGuestDetailsEntryControl.SelectedPaymentGateway;

        if (!this.IsPageError)
        {
            if (WBSPGHelper.IsOnlinePayment(objPaymentGatewayInfos, objHotelBookingPaymentAllocations, ucGuestDetailsEntryControl.GuestDetailsEntryInfo.PaymentCardType))
            {
                Server.Transfer("~/Pages/SendPaymentRQ.aspx");
            }

            else
            {
                Server.Transfer("~/Pages/BookRoom.aspx");
            }

        }

        return;
    }

    private void ValidateGuestDetails(GuestDetailsEntryInfo objGuestDetailsEntryInfo, bool bBookingTermsConditionsAccepted)
    {
        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        PaymentGatewayInfo objPaymentGatewayInfo = (PaymentGatewayInfo)Session[Constants.Sessions.PaymentGatewayInfo];

        DateTime dtNow = TZNet.ToLocal(WbsUiHelper.GetTimeZone(objStayCriteriaSelection.HotelCode), DateTime.UtcNow).Date;

        if (objGuestDetailsEntryInfo.FirstName.Trim() == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoFirstNameEntry"));

        if (objGuestDetailsEntryInfo.LastName.Trim() == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoLastNameEntry"));

        if (objGuestDetailsEntryInfo.Email.Trim() == "" || objGuestDetailsEntryInfo.EmailConfirmEntry.Trim() == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoEmailEntry"));
        else if (objGuestDetailsEntryInfo.Email.Trim() != objGuestDetailsEntryInfo.EmailConfirmEntry.Trim())
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "IncorrectEmailEntry"));

        if (objGuestDetailsEntryInfo.Phone.Trim() == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoPhoneEntry"));

        if (objGuestDetailsEntryInfo.Address1.Trim() == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoStreetAddressEntry"));

        if (objGuestDetailsEntryInfo.City.Trim() == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoCityAddressEntry"));

        if (ConfigurationManager.AppSettings["EnterGuestDetailsPage.RequireStateRegion"] == "1")
        {
            if (objGuestDetailsEntryInfo.StateRegion.Trim() == "")
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoStateRegionAddressEntry"));
        }

        if (ConfigurationManager.AppSettings["EnterGuestDetailsPage.RequirePostalCode"] == "1")
        {
            if (objGuestDetailsEntryInfo.PostalCode.Trim() == "")
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoPostalCodeEntry"));
        }

        if (objGuestDetailsEntryInfo.Country.Trim() == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoCountryAddressEntry"));

        if (objGuestDetailsEntryInfo.AirlineProgramCode.Trim() == "" && objGuestDetailsEntryInfo.AirlineProgramIdentifier != "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "AirlineProgramInfoNotComplete"));

        if (objGuestDetailsEntryInfo.AirlineProgramCode.Trim() != "" && objGuestDetailsEntryInfo.AirlineProgramIdentifier == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "AirlineProgramInfoNotComplete"));

        if (objGuestDetailsEntryInfo.HotelProgramCode.Trim() == "" && objGuestDetailsEntryInfo.HotelProgramIdentifier != "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "HotelProgramInfoNotComplete"));

        if (objGuestDetailsEntryInfo.HotelProgramCode.Trim() != "" && objGuestDetailsEntryInfo.HotelProgramIdentifier == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "HotelProgramInfoNotComplete"));

        if (objGuestDetailsEntryInfo.ArrivalTime != null && objGuestDetailsEntryInfo.ArrivalTime != "")
        {
            if (objGuestDetailsEntryInfo.ArrivalTime == "error")
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "IncorrectArrivalTimeEntry"));

            else
            {
                DateTime dtArrivalTime;

                if (!DateTime.TryParse(objGuestDetailsEntryInfo.ArrivalTime, out dtArrivalTime))
                    this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "IncorrectArrivalTimeEntry"));
            }

        }

        if (this.WbsUiHelper.IsCreditCardInfoRequired(objHotelAvailabilityRS, objRoomRateSelections, objPaymentGatewayInfo, objGuestDetailsEntryInfo.ProfileGuaranteeRequested))
        {
            if (objGuestDetailsEntryInfo.PaymentCardHolder.Trim() == "")
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoPaymentCardHolderEntry"));

            if (objGuestDetailsEntryInfo.PaymentCardType.Trim() == "")
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoPaymentCardTypeEntry"));
            else if (objGuestDetailsEntryInfo.PaymentCardNumber.Trim() == "")
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoPaymentCardNumberEntry"));
            else if (!CCValidator.ValidateNumber(objGuestDetailsEntryInfo.PaymentCardNumber.Trim()))
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "PaymentCardNumberEntryInvalid"));
            else if (!CCValidator.ValidateType(CCValidator.CreditCardType(objGuestDetailsEntryInfo.PaymentCardType.Trim()), objGuestDetailsEntryInfo.PaymentCardNumber.Trim()))
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "PaymentCardTypeEntryInvalid"));

            int intExpireMonth = Convert.ToInt32(objGuestDetailsEntryInfo.PaymentCardExpireDate.Substring(0, 2));
            int intExpireYear = Convert.ToInt32(objGuestDetailsEntryInfo.PaymentCardExpireDate.Substring(2)) + 2000;

            if (intExpireYear < dtNow.Year)
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "PaymentCardIsExpired"));
            else if (intExpireYear == dtNow.Year && intExpireMonth < dtNow.Month)
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "PaymentCardIsExpired"));

            if (ConfigurationManager.AppSettings["EnterGuestDetailsPage.RequirePaymentCardSecurityCode"] == "1")
            {
                if (objGuestDetailsEntryInfo.PaymentCardSecurityCode.Trim() == "")
                    this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoPaymentCardSecurityCodeEntry"));
            }

        }

        if (!bBookingTermsConditionsAccepted)
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "BookingTermsConditionsNotAccepted"));

        return;
    }

    private void LoadLanguageSelectorControl()
    {
        string strLanguageSelectorControlPath = ConfigurationManager.AppSettings["LanguageSelectorControl.ascx"];
        ucLanguageSelectorControl = (LanguageSelectorControl)LoadControl(strLanguageSelectorControlPath);

        phLanguageSelectorControl.Controls.Clear();
        phLanguageSelectorControl.Controls.Add(ucLanguageSelectorControl);

        ucLanguageSelectorControl.LanguageSelected += new LanguageSelectorControl.LanguageSelectedEvent(this.LanguageSelected);

        return;
    }

    private void ConfigureLanguageSelectorControl()
    {
        string strLanguageSelectorItemControlPath = ConfigurationManager.AppSettings["LanguageSelectorItemControl.ascx"];

        this.ucLanguageSelectorControl.Clear();

        LanguageSetup[] objLanguageSetups = WbsUiHelper.GetLanguageSetups();

        for (int i = 0; i < objLanguageSetups.Length; i++)
        {
            LanguageSelectorItemControl ucLanguageSelectorItemControl = (LanguageSelectorItemControl)LoadControl(strLanguageSelectorItemControlPath);
            ucLanguageSelectorControl.Add(ucLanguageSelectorItemControl);

            ucLanguageSelectorItemControl.ID = "LanguageSelectorItem" + ((int)(i + 1)).ToString();
            ucLanguageSelectorItemControl.Culture = objLanguageSetups[i].Culture;
            ucLanguageSelectorItemControl.UICulture = objLanguageSetups[i].UICulture;
            ucLanguageSelectorItemControl.LanguageText = objLanguageSetups[i].LanguageText;
            ucLanguageSelectorItemControl.ImageURL = objLanguageSetups[i].ImageURL;
            ucLanguageSelectorItemControl.Selected = false;

            if (WbsUiHelper.SelectedCulture == objLanguageSetups[i].Culture)
                ucLanguageSelectorItemControl.Selected = true;
        }

        return;
    }

    private void LoadProfileLoginNameControl()
    {
        string strProfileLoginNameControlPath = ConfigurationManager.AppSettings["ProfileLoginNameControl.ascx"];
        ucProfileLoginNameControl = (ProfileLoginNameControl)LoadControl(strProfileLoginNameControlPath);

        phProfileLoginNameControl.Controls.Clear();
        phProfileLoginNameControl.Controls.Add(ucProfileLoginNameControl);

        return;
    }

    private void ConfigureProfileLoginNameControl()
    {
        Profile[] objProfiles = (Profile[])Session["LoginProfiles"];
        bool bIsLoggedIn = (bool)Session["IsLoggedIn"];

        ucProfileLoginNameControl.ID = "ProfileLoginName";
        ucProfileLoginNameControl.Profiles = objProfiles;
        ucProfileLoginNameControl.IsLoggedIn = bIsLoggedIn;

        return;
    }

    private void LoadBookingStepControl()
    {
        string strBookingStepControlPath = ConfigurationManager.AppSettings["BookingStepControl.ascx"];
        ucBookingStepControl = (BookingStepControl)LoadControl(strBookingStepControlPath);

        phBookingStepControl.Controls.Clear();
        phBookingStepControl.Controls.Add(ucBookingStepControl);

        ucBookingStepControl.BookingStepSelected += new BookingStepControl.BookingStepSelectedEvent(this.BookingStepSelected);

        return;
    }

    private void ConfigureBookingStepControl()
    {
        string strBookingStepItemControlPath = ConfigurationManager.AppSettings["BookingStepItemControl.ascx"];

        this.ucBookingStepControl.Clear();

        if (ConfigurationManager.AppSettings["BookingStepControl.OrientationMode"] == "Horizontal")
            ucBookingStepControl.OrientationMode = XHS.WBSUIBizObjects.OrientationMode.Horizontal;
        else
            ucBookingStepControl.OrientationMode = XHS.WBSUIBizObjects.OrientationMode.Vertical;

        ucBookingStepControl.SelectedStep = "EnterYourDetails";

        if (ConfigurationManager.AppSettings["EnableHotelSearch"] == "1")
        {
            BookingStepItemControl step1 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step1);
            step1.ID = "SearchHotel";
            step1.StepRefID = "SearchHotel";
            step1.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep1");
            step1.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepSearchHotel");
            step1.Selected = false;
            step1.Clickable = true;

            BookingStepItemControl step2 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step2);
            step2.ID = "ChooseHotel";
            step2.StepRefID = "ChooseHotel";
            step2.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep2");
            step2.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseHotel");
            step2.Selected = false;
            step2.Clickable = true;

            BookingStepItemControl step3 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step3);
            step3.ID = "ChooseRate";
            step3.StepRefID = "ChooseRate";
            step3.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep3");
            step3.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseRate");
            step3.Selected = false;
            step3.Clickable = true;

            BookingStepItemControl step4 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step4);
            step4.ID = "ChooseYourExtras";
            step4.StepRefID = "ChooseYourExtras";
            step4.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep4");
            step4.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseYourExtras");
            step4.Selected = false;
            step4.Clickable = true;

            BookingStepItemControl step5 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step5);
            step5.ID = "EnterYourDetails";
            step5.StepRefID = "EnterYourDetails";
            step5.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep5");
            step5.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepEnterYourDetails");
            step5.Selected = true;
            step5.Clickable = false;

            BookingStepItemControl step6 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step6);
            step6.ID = "Confirmation";
            step6.StepRefID = "Confirmation";
            step6.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep6");
            step6.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepConfirmation");
            step6.Selected = false;
            step6.Clickable = false;
        }

        else
        {
            BookingStepItemControl step1 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step1);
            step1.ID = "ChooseHotel";
            step1.StepRefID = "ChooseHotel";
            step1.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep1");
            step1.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseHotel");
            step1.Selected = false;
            step1.Clickable = true;

            BookingStepItemControl step2 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step2);
            step2.ID = "ChooseRate";
            step2.StepRefID = "ChooseRate";
            step2.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep2");
            step2.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseRate");
            step2.Selected = false;
            step2.Clickable = true;

            BookingStepItemControl step3 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step3);
            step3.ID = "ChooseYourExtras";
            step3.StepRefID = "ChooseYourExtras";
            step3.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep3");
            step3.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseYourExtras");
            step3.Selected = false;
            step3.Clickable = true;

            BookingStepItemControl step4 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step4);
            step4.ID = "EnterYourDetails";
            step4.StepRefID = "EnterYourDetails";
            step4.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep4");
            step4.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepEnterYourDetails");
            step4.Selected = true;
            step4.Clickable = false;

            BookingStepItemControl step5 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step5);
            step5.ID = "Confirmation";
            step5.StepRefID = "Confirmation";
            step5.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep5");
            step5.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepConfirmation");
            step5.Selected = false;
            step5.Clickable = false;
        }

        return;
    }

    private void LoadErrorDisplayControl()
    {
        string strErrorDisplayControlPath = ConfigurationManager.AppSettings["ErrorDisplayControl.ascx"];
        ucErrorDisplayControl = (ErrorDisplayControl)LoadControl(strErrorDisplayControlPath);

        phErrorDisplayControl.Controls.Clear();
        phErrorDisplayControl.Controls.Add(ucErrorDisplayControl);

        return;
    }

    private void ConfigureErrorDisplayControl()
    {
        ucErrorDisplayControl.ErrorInfos = this.PageErrors;

        return;
    }

    private void LoadBookingSummaryControl()
    {
        string strBookingSummaryControlPath = ConfigurationManager.AppSettings["BookingSummaryControl.ascx"];
        ucBookingSummaryControl = (BookingSummaryControl)LoadControl(strBookingSummaryControlPath);

        phBookingSummaryControl.Controls.Clear();
        phBookingSummaryControl.Controls.Add(ucBookingSummaryControl);

        return;
    }

    private void ConfigureBookingSummaryControl()
    {
        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
        AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];

        HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        HotelPricing[] objHotelPricings = HotelPricingHelper.GetHotelPricing(objStayCriteriaSelection, objRoomRateSelections, objAddOnPackageSelections, objHotelAvailabilityRS.HotelRoomAvailInfos, objHotelDescriptiveInfo.CurrencyCode);

        int intNumberStayNights = ((TimeSpan)objStayCriteriaSelection.DepartureDate.Subtract(objStayCriteriaSelection.ArrivalDate)).Days;

        string strBookingSummaryRoomItemControlPath = ConfigurationManager.AppSettings["BookingSummaryRoomItemControl.ascx"];
        string strBookingSummaryAddOnPackageItemControlPath = ConfigurationManager.AppSettings["BookingSummaryAddOnPackageItemControl.ascx"];

        ucBookingSummaryControl.Clear();

        ucBookingSummaryControl.ID = "BookingSummaryControl";
        ucBookingSummaryControl.HotelDescriptiveInfo = objHotelDescriptiveInfo;
        ucBookingSummaryControl.StayCriteriaSelection = objStayCriteriaSelection;
        ucBookingSummaryControl.GuestDetailsEntryInfo = null;
        ucBookingSummaryControl.HotelPricings = objHotelPricings;
        ucBookingSummaryControl.PaymentReceipt = null;
        ucBookingSummaryControl.MasterConfirmationNumber = "";

        for (int ri = 0; ri < objStayCriteriaSelection.RoomOccupantSelections.Length; ri++)
        {
            RoomRateSelection objRoomRateSelection = new RoomRateSelection();

            for (int i = 0; i < objRoomRateSelections.Length; i++)
            {
                if (objRoomRateSelections[i].RoomRefID == objStayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID)
                {
                    objRoomRateSelection = objRoomRateSelections[i];
                    break;
                }

            }

            HotelDescRoomType objHotelDescRoomType = new HotelDescRoomType();

            for (int i = 0; i < objHotelDescriptiveInfo.RoomTypes.Length; i++)
            {
                if (objHotelDescriptiveInfo.RoomTypes[i].Code == objRoomRateSelection.RoomTypeCode)
                {
                    objHotelDescRoomType = objHotelDescriptiveInfo.RoomTypes[i];
                    break;
                }

            }

            HotelRoomAvailInfo objHotelRoomAvailInfo = new HotelRoomAvailInfo();

            for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
            {
                if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].SegmentRefID == objStayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID)
                {
                    objHotelRoomAvailInfo = objHotelAvailabilityRS.HotelRoomAvailInfos[i];
                    break;
                }

            }

            HotelAvailRatePlan objHotelAvailRatePlan = new HotelAvailRatePlan();

            for (int i = 0; i < objHotelRoomAvailInfo.RatePlans.Length; i++)
            {
                if (objHotelRoomAvailInfo.RatePlans[i].Code == objRoomRateSelection.RatePlanCode)
                {
                    objHotelAvailRatePlan = objHotelRoomAvailInfo.RatePlans[i];
                    break;
                }

            }

            HotelAvailRoomRate objHotelAvailRoomRate = new HotelAvailRoomRate();

            for (int i = 0; i < objHotelRoomAvailInfo.RoomRates.Length; i++)
            {
                if (objHotelRoomAvailInfo.RoomRates[i].RoomTypeCode == objRoomRateSelection.RoomTypeCode && objHotelRoomAvailInfo.RoomRates[i].RatePlanCode == objRoomRateSelection.RatePlanCode)
                {
                    objHotelAvailRoomRate = objHotelRoomAvailInfo.RoomRates[i];
                    break;
                }

            }

            HotelPricing objHotelPricing = new HotelPricing();

            for (int i = 0; i < objHotelPricings.Length; i++)
            {
                if (objHotelPricings[i].SegmentRefID == objStayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID)
                {
                    objHotelPricing = objHotelPricings[i];
                    break;
                }

            }

            BookingSummaryRoomItemControl ucBookingSummaryRoomItemControl = (BookingSummaryRoomItemControl)LoadControl(strBookingSummaryRoomItemControlPath);
            ucBookingSummaryControl.AddRoomSummaryItem(ucBookingSummaryRoomItemControl);

            ucBookingSummaryRoomItemControl.Clear();

            ucBookingSummaryRoomItemControl.ID = "BookingSummaryRoomItem" + ((int)(ri + 1)).ToString();
            ucBookingSummaryRoomItemControl.RoomRefID = objStayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID;
            ucBookingSummaryRoomItemControl.RoomOccupantSelection = objStayCriteriaSelection.RoomOccupantSelections[ri];
            ucBookingSummaryRoomItemControl.RoomType = objHotelDescRoomType;
            ucBookingSummaryRoomItemControl.RatePlan = objHotelAvailRatePlan;
            ucBookingSummaryRoomItemControl.RoomRate = objHotelAvailRoomRate;
            ucBookingSummaryRoomItemControl.HotelPricing = objHotelPricing;
            ucBookingSummaryRoomItemControl.ConfirmationNumber = "";

            for (int pi = 0; pi < objAddOnPackageSelections.Length; pi++)
            {
                if (objAddOnPackageSelections[pi].RoomRefID == objStayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID)
                {
                    HotelDescPackage objHotelDescPackage = new HotelDescPackage();

                    for (int i = 0; i < objHotelDescriptiveInfo.Packages.Length; i++)
                    {
                        if (objHotelDescriptiveInfo.Packages[i].Code == objAddOnPackageSelections[pi].PackageCode)
                        {
                            objHotelDescPackage = objHotelDescriptiveInfo.Packages[i];
                            break;
                        }

                    }

                    HotelAvailPackage objHotelAvailPackage = new HotelAvailPackage();

                    for (int i = 0; i < objHotelAvailRatePlan.Packages.Length; i++)
                    {
                        if (objHotelAvailRatePlan.Packages[i].Code == objAddOnPackageSelections[pi].PackageCode)
                        {
                            if (objHotelAvailRatePlan.Packages[i].RoomTypeCode == "" || objHotelAvailRatePlan.Packages[i].RoomTypeCode == objRoomRateSelection.RoomTypeCode)
                            {
                                objHotelAvailPackage = objHotelAvailRatePlan.Packages[i];
                                break;
                            }

                        }

                    }

                    BookingSummaryAddOnPackageItemControl ucBookingSummaryAddOnPackageItemControl = (BookingSummaryAddOnPackageItemControl)LoadControl(strBookingSummaryAddOnPackageItemControlPath);
                    ucBookingSummaryRoomItemControl.AddAddOnPackageSummaryItem(ucBookingSummaryAddOnPackageItemControl);

                    ucBookingSummaryAddOnPackageItemControl.ID = "BookingSummaryAddOnPackageItem" + ((int)(pi + 1)).ToString();
                    ucBookingSummaryAddOnPackageItemControl.RoomRefID = objStayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID;
                    ucBookingSummaryAddOnPackageItemControl.NumberStayNights = intNumberStayNights;
                    ucBookingSummaryAddOnPackageItemControl.PackageQuantity = objAddOnPackageSelections[pi].Quantity;
                    ucBookingSummaryAddOnPackageItemControl.PackageDescription = objHotelDescPackage;
                    ucBookingSummaryAddOnPackageItemControl.PackageRate = objHotelAvailPackage;
                }

            }

        }

        return;
    }

    private void LoadGuestDetailsEntryControl()
    {
        string strGuestDetailsEntryControlPath = ConfigurationManager.AppSettings["GuestDetailsEntryControl.ascx"];
        ucGuestDetailsEntryControl = (GuestDetailsEntryControl)LoadControl(strGuestDetailsEntryControlPath);

        phGuestDetailsEntryControl.Controls.Clear();
        phGuestDetailsEntryControl.Controls.Add(ucGuestDetailsEntryControl);

        ucGuestDetailsEntryControl.PaymentGatewayPreSelectCompleted += new GuestDetailsEntryControl.PaymentGatewayPreSelectCompletedEvent(this.PaymentGatewayPreSelectCompleted);
        ucGuestDetailsEntryControl.GuestDetailsCompleted += new GuestDetailsEntryControl.GuestDetailsCompletedEvent(this.GuestDetailsCompleted);

        return;
    }

    private void ConfigureGuestDetailsEntryControl()
    {
        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
        GuestDetailsEntryInfo objGuestDetailsEntryInfo = (GuestDetailsEntryInfo)Session["GuestDetailsEntryInfo"];
        PaymentGatewayInfo[] objPaymentGatewayInfos = (PaymentGatewayInfo[])Session[Constants.Sessions.PaymentGatewayInfos];
        PaymentGatewayInfo objPaymentGatewayInfo = (PaymentGatewayInfo)Session[Constants.Sessions.PaymentGatewayInfo];
        HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations = (HotelBookingPaymentAllocation[])Session[Constants.Sessions.HotelBookingPaymentAllocations];
        bool bBookingTermsConditionsAccepted = (bool)Session["BookingTermsConditionsAccepted"];

        HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        PaymentCardApplication enumPaymentCardApplicationStatus = WBSPGHelper.GetPaymentCardApplicationStatus(objHotelBookingPaymentAllocations);

        decimal decTotalPaymentCardPayment = WBSPGHelper.GetTotalPaymentCardPayment(objHotelBookingPaymentAllocations);

        List<string> lPaymentCardCodes = new List<string>();

        string[] objPaymentCardCodes = objHotelDescriptiveInfo.CreditCardCodes;

        for (int i = 0; i < objPaymentCardCodes.Length; i++) // mask out cards with no name lookup
        {
            if ((String)GetGlobalResourceObject("SiteResources", "CardType" + objPaymentCardCodes[i]) != null && (String)GetGlobalResourceObject("SiteResources", "CardType" + objPaymentCardCodes[i]) != "")
                lPaymentCardCodes.Add(objPaymentCardCodes[i]);
        }

        objPaymentCardCodes = lPaymentCardCodes.ToArray();

        if (objPaymentGatewayInfos != null && objPaymentGatewayInfos.Length != 0 && decTotalPaymentCardPayment != 0 && ConfigurationManager.AppSettings["EnterGuestDetailsPage.PaymentGatewayAcceptedCardsOnly"] == "1")
            objPaymentCardCodes = WBSPGHelper.GetPaymentGatewayAcceptedCardTypes(objPaymentGatewayInfos);

        bool bPaymentGatewayPreSelectRequired = false;

        if (objPaymentGatewayInfo == null && WBSPGHelper.IsPaymentGatewayPreSelectRequired(objPaymentGatewayInfos, objHotelBookingPaymentAllocations))
            bPaymentGatewayPreSelectRequired = true;

        XHS.WBSUIBizObjects.Profile objProfile = WbsUiHelper.GetLoginLinkedProfile();

        bool bDisplayProfileGuarantee = false;

        if (objProfile != null && objProfile.PermitProfileGuarantee && (enumPaymentCardApplicationStatus == PaymentCardApplication.GuaranteeOnly || enumPaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit))
        {
            bDisplayProfileGuarantee = true;
        }

        ucGuestDetailsEntryControl.HotelDescriptiveInfo = objHotelDescriptiveInfo;
        ucGuestDetailsEntryControl.GuestDetailsEntryInfo = objGuestDetailsEntryInfo;
        ucGuestDetailsEntryControl.MembershipPrograms = WbsUiHelper.GetMembershipPrograms(((StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection]).HotelCode);
        ucGuestDetailsEntryControl.PaymentCardCodes = objPaymentCardCodes;
        ucGuestDetailsEntryControl.TermsConditionsAccepted = bBookingTermsConditionsAccepted;
        ucGuestDetailsEntryControl.PaymentGatewayInfos = objPaymentGatewayInfos;
        ucGuestDetailsEntryControl.PaymentGatewayPreSelectRequired = bPaymentGatewayPreSelectRequired;
        ucGuestDetailsEntryControl.SelectedPaymentGateway = objPaymentGatewayInfo;
        ucGuestDetailsEntryControl.PaymentCardApplicationStatus = enumPaymentCardApplicationStatus;
        ucGuestDetailsEntryControl.PaymentCardDepositAmount = decTotalPaymentCardPayment;
        ucGuestDetailsEntryControl.PaymentCardDepositCurrencyCode = objHotelDescriptiveInfo.CurrencyCode;
        ucGuestDetailsEntryControl.DisplayProfileGuarantee = bDisplayProfileGuarantee;

        return;
    }

    private void ConfigureTrackingCodeControl()
    {
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

        TrackingCodeInfo[] objTrackingCodeInfos = WbsUiHelper.GetTrackingCodeInfos(objStayCriteriaSelection.HotelCode);

        if (objTrackingCodeInfos.Length == 0)
            return;

        string strTrackingCodeControlPath = ConfigurationManager.AppSettings["TrackingCodeControl.ascx"];
        ucHeadTrackingCodeControl = (TrackingCodeControl)LoadControl(strTrackingCodeControlPath);
        ucBodyTrackingCodeControl = (TrackingCodeControl)LoadControl(strTrackingCodeControlPath);

        Control objHeadElement = this.Page.Master.FindControl("head_element");

        if (objHeadElement != null)
            objHeadElement.Controls.Add(ucHeadTrackingCodeControl);

        phTrackingCodeControl.Controls.Clear();
        phTrackingCodeControl.Controls.Add(ucBodyTrackingCodeControl);

        ucHeadTrackingCodeControl.Clear();
        ucBodyTrackingCodeControl.Clear();

        string strTrackingCodeItemControlPath = ConfigurationManager.AppSettings["TrackingCodeItemControl.ascx"];

        for (int i = 0; i < objTrackingCodeInfos.Length; i++)
        {
            if (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.AllPages || (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.StartPageOnly && this.IsFirstTrackingPage))
            {
                TrackingCodeItemControl ucTrackingCodeItemControl = (TrackingCodeItemControl)LoadControl(strTrackingCodeItemControlPath);

                if (objTrackingCodeInfos[i].TagLocation == TrackingTagLocation.HeadElement)
                    ucHeadTrackingCodeControl.Add(ucTrackingCodeItemControl);
                else if (objTrackingCodeInfos[i].TagLocation == TrackingTagLocation.BodyElement)
                    ucBodyTrackingCodeControl.Add(ucTrackingCodeItemControl);
                else
                    continue;

                ucTrackingCodeItemControl.Type = objTrackingCodeInfos[i].Type;
                ucTrackingCodeItemControl.TrackingCodeParameters = objTrackingCodeInfos[i].TrackingCodeParameters;
                ucTrackingCodeItemControl.HotelCode = objStayCriteriaSelection.HotelCode;
                ucTrackingCodeItemControl.PageUrl = ("/book/EnterDetails");
                ucTrackingCodeItemControl.Amount = 0;
            }

        }

        this.IsFirstTrackingPage = false;

        return;
    }

}
