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

public partial class BookingConfirmation : XnGR_WBS_Page
{
    private ProfileLoginNameControl ucProfileLoginNameControl;
    private LanguageSelectorControl ucLanguageSelectorControl;
    private BookingStepControl ucBookingStepControl;
    private ErrorDisplayControl ucErrorDisplayControl;
    private BookingSummaryControl ucBookingSummaryControl;
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

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            Session["SelectedRoom"] = "1";
        }

        else
        {
            this.ConfigureLanguageSelectorControl();
            this.ConfigureProfileLoginNameControl();
            this.ConfigureBookingStepControl();
            this.ConfigureErrorDisplayControl();
            this.ConfigureBookingSummaryControl();
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
        this.ConfigureTrackingCodeControl();

        ucLanguageSelectorControl.RenderUserControl();
        ucProfileLoginNameControl.RenderUserControl();
        ucBookingStepControl.RenderUserControl();
        ucErrorDisplayControl.RenderUserControl();
        ucBookingSummaryControl.RenderUserControl();
        ucHeadTrackingCodeControl.RenderUserControl();
        ucBodyTrackingCodeControl.RenderUserControl();

        this.PageComplete();

        return;
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
        WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
        WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect("~/Pages/BookingConfirmation.aspx");

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

        ucBookingStepControl.SelectedStep = "Confirmation";

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
            step2.Clickable = false;

            BookingStepItemControl step3 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step3);
            step3.ID = "ChooseRate";
            step3.StepRefID = "ChooseRate";
            step3.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep3");
            step3.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseRate");
            step3.Selected = false;
            step3.Clickable = false;

            BookingStepItemControl step4 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step4);
            step4.ID = "ChooseYourExtras";
            step4.StepRefID = "ChooseYourExtras";
            step4.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep4");
            step4.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseYourExtras");
            step4.Selected = false;
            step4.Clickable = false;

            BookingStepItemControl step5 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step5);
            step5.ID = "EnterYourDetails";
            step5.StepRefID = "EnterYourDetails";
            step5.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep5");
            step5.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepEnterYourDetails");
            step5.Selected = false;
            step5.Clickable = false;

            BookingStepItemControl step6 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step6);
            step6.ID = "Confirmation";
            step6.StepRefID = "Confirmation";
            step6.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep6");
            step6.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepConfirmation");
            step6.Selected = true;
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
            step2.Clickable = false;

            BookingStepItemControl step3 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step3);
            step3.ID = "ChooseYourExtras";
            step3.StepRefID = "ChooseYourExtras";
            step3.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep3");
            step3.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseYourExtras");
            step3.Selected = false;
            step3.Clickable = false;

            BookingStepItemControl step4 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step4);
            step4.ID = "EnterYourDetails";
            step4.StepRefID = "EnterYourDetails";
            step4.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep4");
            step4.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepEnterYourDetails");
            step4.Selected = false;
            step4.Clickable = false;

            BookingStepItemControl step5 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step5);
            step5.ID = "Confirmation";
            step5.StepRefID = "Confirmation";
            step5.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep5");
            step5.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepConfirmation");
            step5.Selected = true;
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
        HotelBookingRS objHotelBookingRS = (HotelBookingRS)Session["HotelBookingRS"];

        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
        AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];
        GuestDetailsEntryInfo objGuestDetailsEntryInfo = (GuestDetailsEntryInfo)Session["GuestDetailsEntryInfo"];
        PaymentGatewayInfo objPaymentGatewayInfo = (PaymentGatewayInfo)Session[Constants.Sessions.PaymentGatewayInfo];
        HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations = (HotelBookingPaymentAllocation[])Session[Constants.Sessions.HotelBookingPaymentAllocations];
        HotelPaymentRS objHotelPaymentRS = (HotelPaymentRS)Session["HotelPaymentRS"];

        HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        HotelPricing[] objHotelPricings = HotelPricingHelper.GetHotelPricing(objStayCriteriaSelection, objRoomRateSelections, objAddOnPackageSelections, objHotelAvailabilityRS.HotelRoomAvailInfos, objHotelDescriptiveInfo.CurrencyCode);

        OnlinePaymentReceipt objOnlinePaymentReceipt = null;

        decimal decTotalPaymentCardPayment = WBSPGHelper.GetTotalPaymentCardPayment(objHotelBookingPaymentAllocations);

        if (objPaymentGatewayInfo != null && decTotalPaymentCardPayment != 0)
        {
            objOnlinePaymentReceipt = new OnlinePaymentReceipt();

            objOnlinePaymentReceipt.PaymentCard = objHotelPaymentRS.PaymentCard;
            objOnlinePaymentReceipt.PaymentGatewayCardType = objHotelPaymentRS.PaymentGatewayCardType;
            objOnlinePaymentReceipt.PaymentDateTime = TZNet.ToLocal(WbsUiHelper.GetTimeZone(objStayCriteriaSelection.HotelCode), DateTime.UtcNow).Date;
            objOnlinePaymentReceipt.AuthCode = objHotelPaymentRS.PaymentAuthCode;
            objOnlinePaymentReceipt.TransRefID = objHotelPaymentRS.PaymentTransRefID;
            objOnlinePaymentReceipt.Amount = decTotalPaymentCardPayment;
            objOnlinePaymentReceipt.CurrencyCode = objHotelDescriptiveInfo.CurrencyCode;
        }

        string strMasterConfirmationNumber = "";

        if (objHotelBookingRS.Segments.Length != 0)
        {
            strMasterConfirmationNumber = objHotelBookingRS.Segments[0].MasterConfirmationNumber;
        }

        int intNumberStayNights = ((TimeSpan)objStayCriteriaSelection.DepartureDate.Subtract(objStayCriteriaSelection.ArrivalDate)).Days;

        string strBookingSummaryRoomItemControlPath = ConfigurationManager.AppSettings["BookingSummaryRoomItemControl.ascx"];
        string strBookingSummaryAddOnPackageItemControlPath = ConfigurationManager.AppSettings["BookingSummaryAddOnPackageItemControl.ascx"];

        ucBookingSummaryControl.Clear();

        ucBookingSummaryControl.ID = "BookingSummaryControl";
        ucBookingSummaryControl.HotelDescriptiveInfo = objHotelDescriptiveInfo;
        ucBookingSummaryControl.StayCriteriaSelection = objStayCriteriaSelection;
        ucBookingSummaryControl.GuestDetailsEntryInfo = objGuestDetailsEntryInfo;
        ucBookingSummaryControl.HotelPricings = objHotelPricings;
        ucBookingSummaryControl.PaymentReceipt = objOnlinePaymentReceipt;
        ucBookingSummaryControl.MasterConfirmationNumber = strMasterConfirmationNumber;

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

    private void ConfigureTrackingCodeControl()
    {
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
        AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];
        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];
        HotelBookingRS objHotelBookingRS = (HotelBookingRS)Session["HotelBookingRS"];

        TrackingCodeInfo[] objTrackingCodeInfos = WbsUiHelper.GetTrackingCodeInfos(objStayCriteriaSelection.HotelCode);

        if (objTrackingCodeInfos.Length == 0)
            return;

        HotelPricing[] objHotelPricings = HotelPricingHelper.GetHotelPricing(objStayCriteriaSelection, objRoomRateSelections, objAddOnPackageSelections, objHotelAvailabilityRS.HotelRoomAvailInfos, "");

        decimal decTotalBookingAmount = 0;
        string strCurrencyCode = "";

        for (int i = 0; i < objHotelPricings.Length; i++)
            decTotalBookingAmount += objHotelPricings[i].TotalAmount;

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length > 0)
            strCurrencyCode = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0].CurrencyCode;

        decTotalBookingAmount = decTotalBookingAmount * WbsUiHelper.GetCurrencyConversionFactor(strCurrencyCode);

        string strMasterConfirmationNumber = "";

        if (objHotelBookingRS.Segments.Length != 0)
        {
            strMasterConfirmationNumber = objHotelBookingRS.Segments[0].MasterConfirmationNumber;
        }

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
            if (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.AllPages || (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.StartPageOnly && this.IsFirstTrackingPage) || objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.ConfirmPageOnly)
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
                ucTrackingCodeItemControl.PageUrl = ("/book/Confirmation");
                ucTrackingCodeItemControl.ConfirmNumber = strMasterConfirmationNumber;
                ucTrackingCodeItemControl.Amount = decTotalBookingAmount;
            }

        }

        this.IsFirstTrackingPage = false;

        return;
    }

}
