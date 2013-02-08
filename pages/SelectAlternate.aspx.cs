using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class SelectAlternate : XnGR_WBS_Page
{
    private ProfileLoginNameControl ucProfileLoginNameControl;
    private LanguageSelectorControl ucLanguageSelectorControl;
    private BookingStepControl ucBookingStepControl;
    private ErrorDisplayControl ucErrorDisplayControl;
    private StayCriteriaSelectorControl ucStayCriteriaControl;
    private AvailCalSelectorControl ucAvailCalSelectorControl;
    private AlternateHotelSelectorControl ucAlternateHotelSelectorControl;
    private TrackingCodeControl ucHeadTrackingCodeControl;
    private TrackingCodeControl ucBodyTrackingCodeControl;

    private bool bAsyncGetHotelAvailCalendarInfo;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        this.LoadLanguageSelectorControl();
        this.LoadProfileLoginNameControl();
        this.LoadBookingStepControl();
        this.LoadErrorDisplayControl();
        this.LoadStaySelectorControl();
        this.LoadAvailCalSelectorControl();
        this.LoadAlternateHotelSelectorControl();

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        bAsyncGetHotelAvailCalendarInfo = false;

        if (!IsPostBack)
        {
            Session["SelectedRoom"] = "1";

            bAsyncGetHotelAvailCalendarInfo = true;
        }

        else
        {
            this.ConfigureLanguageSelectorControl();
            this.ConfigureProfileLoginNameControl();
            this.ConfigureBookingStepControl();
            this.ConfigureErrorDisplayControl();
            this.ConfigureStaySelectorControl();
            this.ConfigureAvailCalSelectorControl();
            this.ConfigureAlternateHotelSelectorControl();
        }

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetHotelAvailCalendarInfo)
        {
            this.BeginResumeAsyncDataCapture();
        }

        else
        {
            while (!ThreadPool.QueueUserWorkItem(new WaitCallback(TerminateAsyncOperation), wbsIISAsyncResult))
                Thread.Sleep(100);
        }

        return wbsIISAsyncResult;
    }

    private void BeginResumeAsyncDataCapture()
    {
        if (bAsyncGetHotelAvailCalendarInfo)
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitHotelAvailCalendarInfoRQ(ref wbsAPIRouterData, objStayCriteriaSelection, objStayCriteriaSelection.ArrivalDate, this.NumberDaysInAvailCalendar, this.WbsUiHelper.GetLoginLinkedProfileIdentifier());
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelAvailCalendarInfoComplete), null, false);
        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    private void HotelAvailCalendarInfoComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessHotelAvailCalendarInfoRS(ref wbsAPIRouterData))
        {
            this.InitAvailCalDateSelections();

            bAsyncGetHotelAvailCalendarInfo = false;
            this.BeginResumeAsyncDataCapture();
        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    public void EndAsyncOperation(IAsyncResult ar)
    {
        return;
    }

    public void TimeoutAsyncOperation(IAsyncResult ar)
    {
        return;
    }

    private void TerminateAsyncOperation(Object StateInfo)
    {
        WBSAsyncResult wbsIISAsyncResult = (WBSAsyncResult)StateInfo;

        if (!wbsIISAsyncResult.IsCompleted)
            wbsIISAsyncResult.SetComplete();

        return;
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (this.IsPageWbsApiError)
        {
            this.PageComplete();

            if (ConfigurationManager.AppSettings["EnableHotelSearch"] == "1")
            {
                this.SaveCrossPageErrors();
                Response.Redirect("~/Pages/SearchHotel.aspx?CrossPageErrors=1");
            }

            else
            {
                this.SaveCrossPageErrors();
                Response.Redirect("~/Pages/SelectHotel.aspx?CrossPageErrors=1");
            }

        }

        else
        {
            this.IsParentPreRender = true;

            this.ConfigureLanguageSelectorControl();
            this.ConfigureProfileLoginNameControl();
            this.ConfigureBookingStepControl();
            this.ConfigureErrorDisplayControl();
            this.ConfigureStaySelectorControl();
            this.ConfigureAvailCalSelectorControl();
            this.ConfigureAlternateHotelSelectorControl();
            this.ConfigureTrackingCodeControl();

            ucLanguageSelectorControl.RenderUserControl();
            ucProfileLoginNameControl.RenderUserControl();
            ucBookingStepControl.RenderUserControl();
            ucErrorDisplayControl.RenderUserControl();
            ucStayCriteriaControl.RenderUserControl();
            ucAvailCalSelectorControl.RenderUserControl();
            ucAlternateHotelSelectorControl.RenderUserControl();
            ucHeadTrackingCodeControl.RenderUserControl();
            ucBodyTrackingCodeControl.RenderUserControl();

            this.PageComplete();
        }

        return;
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
        WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
        WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect("~/Pages/SelectAlternate.aspx");

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

    protected void StayCriteriaCompleted(object sender, EventArgs e)
    {
        this.ValidateStayCriteria(ucStayCriteriaControl.StayCriteriaSelection);

        Session[Constants.Sessions.StayCriteriaSelection] = ucStayCriteriaControl.StayCriteriaSelection;

        if (!this.IsPageError)
        {
            WbsUiHelper.InitRoomRateSelections();
            WbsUiHelper.InitAddOnPackageSelections();
            WbsUiHelper.InitGuestDetailsEntryInfo();

            if (this.IsGuestDetailsTestPrefill)
                WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

            Response.Redirect("~/Pages/CheckAvailability.aspx");
        }

        else
        {
            
        }

        return;
    }

    protected void SelectNewCountry(object sender, EventArgs e)
    {
        return; // Not used in StaySelectorControl "non-search" mode
    }

    protected void SelectNewArea(object sender, EventArgs e)
    {
        return; // Not used in StaySelectorControl "non-search" mode
    }

    protected void SelectNewHotel(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["EnableHotelSearch"] == "1")
        {
            Response.Redirect("~/Pages/SearchHotel.aspx");
        }

        else
        {
            Response.Redirect("~/Pages/SelectHotel.aspx");
        }

        return;
    }

    protected void AvailCalCompleted(object sender, EventArgs e)
    {
        this.ValidateAvailCalDateSelections(ucAvailCalSelectorControl.SelectedDates);

        Session["AvailCalDateSelections"] = ucAvailCalSelectorControl.SelectedDates;

        if (!this.IsPageError)
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

            objStayCriteriaSelection.ArrivalDate = ucAvailCalSelectorControl.SelectedDates[0].Date;
            objStayCriteriaSelection.DepartureDate = ucAvailCalSelectorControl.SelectedDates[ucAvailCalSelectorControl.SelectedDates.Length - 1].Date.AddDays(1);

            Session[Constants.Sessions.StayCriteriaSelection] = objStayCriteriaSelection;

            WbsUiHelper.InitRoomRateSelections();
            WbsUiHelper.InitAddOnPackageSelections();
            WbsUiHelper.InitGuestDetailsEntryInfo();

            if (this.IsGuestDetailsTestPrefill)
                WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

            Response.Redirect("~/Pages/CheckAvailability.aspx");
        }

        else
        {

        }

        return;
    }

    protected void AltHotelSelected(object sender, EventArgs e)
    {
        if (!this.IsPageError)
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

            objStayCriteriaSelection.HotelCode = ucAlternateHotelSelectorControl.SelectedHotelCode;
            Session[Constants.Sessions.StayCriteriaSelection] = objStayCriteriaSelection;

            WbsUiHelper.InitRoomRateSelections();
            WbsUiHelper.InitAddOnPackageSelections();
            WbsUiHelper.InitGuestDetailsEntryInfo();

            if (this.IsGuestDetailsTestPrefill)
                WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

            Response.Redirect("~/Pages/CheckAvailability.aspx");
        }

        else
        {

        }

        return;
    }

    private void InitAvailCalDateSelections()
    {
        Session["AvailCalDateSelections"] = new DateTime[0];

        return;
    }

    private void ValidateStayCriteria(StayCriteriaSelection objStayCriteriaSelection)
    {
        if (objStayCriteriaSelection.ArrivalDate.Date < TZNet.ToLocal(WbsUiHelper.GetTimeZone(objStayCriteriaSelection.HotelCode), DateTime.UtcNow).Date)
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "ArrivalDateInPast"));

        if (objStayCriteriaSelection.DepartureDate.Date <= objStayCriteriaSelection.ArrivalDate.Date)
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "DepartureDateBeforeArrivalDate"));

        int intBookingDaysRequested = ((TimeSpan)(objStayCriteriaSelection.DepartureDate.Subtract(objStayCriteriaSelection.ArrivalDate))).Days;

        int intMaxBookingDays;

        if (!Int32.TryParse(ConfigurationManager.AppSettings["MaxBookingDays"], out intMaxBookingDays))
            intMaxBookingDays = 30;

        if (intBookingDaysRequested > intMaxBookingDays)
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "TooManyBookingDaysRequested"));

        for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
        {
            if (objStayCriteriaSelection.RoomOccupantSelections[i].NumberAdults == 0 && objStayCriteriaSelection.RoomOccupantSelections[i].NumberChildren == 0)
            {
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoRoomOccupantSelection"));
                break;
            }

        }

        return;
    }

    private void ValidateAvailCalDateSelections(DateTime[] objAvailCalDateSelections)
    {
        if (objAvailCalDateSelections == null || objAvailCalDateSelections.Length == 0)
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoAvailCalDateSelections"));

        if (objAvailCalDateSelections.Length > 1)
        {
            DateTime dtPrevious = objAvailCalDateSelections[0].Date;

            for (int i = 1; i < objAvailCalDateSelections.Length; i++)
            {
                if (objAvailCalDateSelections[i].Date != dtPrevious.Date.AddDays(1))
                {
                    this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "AvailCalDateSelectionsNotSequential"));
                    break;
                }

                dtPrevious = objAvailCalDateSelections[i].Date;
            }

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

        ucBookingStepControl.SelectedStep = "ChooseRate";

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
            step3.Selected = true;
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
            step2.Selected = true;
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

    private void LoadStaySelectorControl()
    {
        string strStayCriteriaSelectorControlPath = ConfigurationManager.AppSettings["StayCriteriaSelectorControl.ascx"];
        ucStayCriteriaControl = (StayCriteriaSelectorControl)LoadControl(strStayCriteriaSelectorControlPath);

        phStayCriteriaControl.Controls.Clear();
        phStayCriteriaControl.Controls.Add(ucStayCriteriaControl);

        ucStayCriteriaControl.StayCriteriaCompleted += new StayCriteriaSelectorControl.StayCriteriaCompletedEvent(this.StayCriteriaCompleted);
        ucStayCriteriaControl.SelectNewCountry += new StayCriteriaSelectorControl.SelectNewCountryEvent(this.SelectNewCountry);
        ucStayCriteriaControl.SelectNewArea += new StayCriteriaSelectorControl.SelectNewAreaEvent(this.SelectNewArea);
        ucStayCriteriaControl.SelectNewHotel += new StayCriteriaSelectorControl.SelectNewHotelEvent(this.SelectNewHotel);

        return;
    }

    private void ConfigureStaySelectorControl()
    {
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        HotelSearchRS objAreaListHotelSearchRS = (HotelSearchRS)Session["AreaListHotelSearchRS"];
        HotelSearchRS objPropertyListHotelSearchRS = (HotelSearchRS)Session["PropertyListHotelSearchRS"];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

        CountryListItem[] objCountryListItems = new CountryListItem[0];
        AreaListItem[] objAreaListItems = new AreaListItem[0];
        HotelListItem[] objHotelListItems = new HotelListItem[0];

        if (ConfigurationManager.AppSettings["EnableHotelSearch"] == "1")
        {
            objCountryListItems = this.GetCountryList(objAreaListHotelSearchRS.AreaListItems);
            objAreaListItems = this.GetCountryAreaList(objStayCriteriaSelection.CountryCode, objAreaListHotelSearchRS.AreaListItems);
            objHotelListItems = this.GetAreaHotelList(objStayCriteriaSelection.AreaID, objPropertyListHotelSearchRS.HotelListItems);
        }

        else
        {
            objHotelListItems = objPropertyListHotelSearchRS.HotelListItems;
        }

        HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        ucStayCriteriaControl.Clear();

        ucStayCriteriaControl.ID = "StayCriteriaSelector";

        if (ConfigurationManager.AppSettings["EnableHotelSearch"] == "1")
            ucStayCriteriaControl.StayCriteriaSelectorType = StayCriteriaSelectorType.HotelSearch;
        else
            ucStayCriteriaControl.StayCriteriaSelectorType = StayCriteriaSelectorType.HotelList;

        ucStayCriteriaControl.CountryListItems = objCountryListItems;
        ucStayCriteriaControl.AreaListItems = objAreaListItems;
        ucStayCriteriaControl.HotelListItems = objHotelListItems;
        ucStayCriteriaControl.StayCriteriaSelectorMode = StayCriteriaSelectorMode.Change;
        ucStayCriteriaControl.StayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        ucStayCriteriaControl.HotelDescriptiveInfo = objHotelDescriptiveInfo;

        return;
    }

    private void LoadAvailCalSelectorControl()
    {
        string strAvailCalSelectorControlPath = ConfigurationManager.AppSettings["AvailCalSelectorControl.ascx"];
        ucAvailCalSelectorControl = (AvailCalSelectorControl)LoadControl(strAvailCalSelectorControlPath);

        phAvailCalSelectorControl.Controls.Clear();
        phAvailCalSelectorControl.Controls.Add(ucAvailCalSelectorControl);

        ucAvailCalSelectorControl.AvailCalCompleted += new AvailCalSelectorControl.AvailCalCompletedEvent(this.AvailCalCompleted);

        return;
    }

    private void ConfigureAvailCalSelectorControl()
    {
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityCalendarRS"];
        DateTime[] objAvailCalDateSelections = (DateTime[])Session["AvailCalDateSelections"];

        DateTime dtToday = TZNet.ToLocal(WbsUiHelper.GetTimeZone(objStayCriteriaSelection.HotelCode), DateTime.UtcNow).Date;

        AvailabilityCalendarInfo[] objAvailabilityCalendarInfos = new AvailabilityCalendarInfo[objHotelAvailabilityRS.HotelRoomAvailInfos.Length];

        for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
        {
            objAvailabilityCalendarInfos[i] = new AvailabilityCalendarInfo();
            objAvailabilityCalendarInfos[i].SegmentRefID = objHotelAvailabilityRS.HotelRoomAvailInfos[i].SegmentRefID;
            objAvailabilityCalendarInfos[i].AvailabilityCalendar = objHotelAvailabilityRS.HotelRoomAvailInfos[i].AvailabilityCalendar;
        }

        ucAvailCalSelectorControl.AvailCalendarSelectorMode = AvailCalendarSelectorMode.ViewCalendar;
        ucAvailCalSelectorControl.AvailabilityCalendarInfo = objAvailabilityCalendarInfos;
        ucAvailCalSelectorControl.SelectedDates = objAvailCalDateSelections;
        ucAvailCalSelectorControl.StayCriteriaSelection = objStayCriteriaSelection;
        ucAvailCalSelectorControl.Today = dtToday.Date;

        return;
    }

    private void LoadAlternateHotelSelectorControl()
    {
        string strAlternateHotelSelectorControlPath = ConfigurationManager.AppSettings["AlternateHotelSelectorControl.ascx"];
        ucAlternateHotelSelectorControl = (AlternateHotelSelectorControl)LoadControl(strAlternateHotelSelectorControlPath);

        phAlternateHotelSelectorControl.Controls.Clear();
        phAlternateHotelSelectorControl.Controls.Add(ucAlternateHotelSelectorControl);

        ucAlternateHotelSelectorControl.AlternateHotelSelected +=new AlternateHotelSelectorControl.AlternateHotelSelectedEvent(this.AltHotelSelected);

        return;
    }

    private void ConfigureAlternateHotelSelectorControl()
    {
        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["AlternateHotelDescriptiveInfoRS"];

        List<AlternateHotel> lAlternateHotels = new List<AlternateHotel>();

        for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
        {
            if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].IsAlternateAvailability)
            {
                bool bHotelIncluded = false;

                for (int j = 0; j < lAlternateHotels.Count; j++)
                {
                    if (lAlternateHotels[j].HotelCode == objHotelAvailabilityRS.HotelRoomAvailInfos[i].HotelCode)
                    {
                        bHotelIncluded = true;
                        break;
                    }

                }

                if (!bHotelIncluded)
                {
                    AlternateHotel objAlternateHotel = new AlternateHotel();
                    lAlternateHotels.Add(objAlternateHotel);

                    objAlternateHotel.HotelCode = objHotelAvailabilityRS.HotelRoomAvailInfos[i].HotelCode;
                    objAlternateHotel.AvailInfo = objHotelAvailabilityRS.HotelRoomAvailInfos[i];
                    objAlternateHotel.DescriptiveInfo = null;

                    for (int j = 0; j < objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length; j++)
                    {
                        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos[j].HotelCode == objHotelAvailabilityRS.HotelRoomAvailInfos[i].HotelCode)
                        {
                            objAlternateHotel.DescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[j];
                            break;
                        }

                    }

                }

            }

        }

        if (lAlternateHotels.Count != 0)
        {
            panAlternateHotelSelectorControl.Visible = true;

            string strAlternateHotelSelectorItemControlPath = ConfigurationManager.AppSettings["AlternateHotelSelectorItemControl.ascx"];
            string strAlternateHotelRatePlanItemControlPath = ConfigurationManager.AppSettings["AlternateHotelRatePlanItemControl.ascx"];

            ucAlternateHotelSelectorControl.Clear();

            ucAlternateHotelSelectorControl.ID = "AlternateHotelSelectorControl";

            for (int hi = 0; hi < lAlternateHotels.Count; hi++)
            {
                AlternateHotelSelectorItemControl ucAlternateHotelSelectorItemControl = (AlternateHotelSelectorItemControl)LoadControl(strAlternateHotelSelectorItemControlPath);
                ucAlternateHotelSelectorControl.Add(ucAlternateHotelSelectorItemControl);

                ucAlternateHotelSelectorItemControl.Clear();

                ucAlternateHotelSelectorItemControl.ID = "AlternateHotelSelectorItem" + ((int)(hi + 1)).ToString();
                ucAlternateHotelSelectorItemControl.HotelDescriptiveInfo = lAlternateHotels[hi].DescriptiveInfo;

                for (int ri = 0; ri < lAlternateHotels[hi].AvailInfo.RatePlans.Length; ri++)
                {
                    List<HotelAvailRoomRate> lHotelAvailRoomRates = new List<HotelAvailRoomRate>();

                    for (int i = 0; i < lAlternateHotels[hi].AvailInfo.RoomRates.Length; i++)
                    {
                        if (lAlternateHotels[hi].AvailInfo.RoomRates[i].RatePlanCode == lAlternateHotels[hi].AvailInfo.RatePlans[ri].Code)
                        {
                            lHotelAvailRoomRates.Add(lAlternateHotels[hi].AvailInfo.RoomRates[i]);
                            break;
                        }

                    }

                    if (lHotelAvailRoomRates.Count != 0)
                    {
                        AlternateHotelRatePlanItemControl ucAlternateHotelRatePlanItemControl = (AlternateHotelRatePlanItemControl)LoadControl(strAlternateHotelRatePlanItemControlPath);
                        ucAlternateHotelSelectorItemControl.Add(ucAlternateHotelRatePlanItemControl);

                        ucAlternateHotelRatePlanItemControl.ID = "AlternateHotelRatePlanItem" + ((int)(ri + 1)).ToString();
                        ucAlternateHotelRatePlanItemControl.RatePlan = lAlternateHotels[hi].AvailInfo.RatePlans[ri];
                        ucAlternateHotelRatePlanItemControl.RoomRates = lHotelAvailRoomRates.ToArray();
                    }

                }

            }

        }

        else
        {
            panAlternateHotelSelectorControl.Visible = false;
        }

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
                ucTrackingCodeItemControl.PageUrl = ("/book/SelectAlternateAvailability");
                ucTrackingCodeItemControl.Amount = 0;
            }

        }

        this.IsFirstTrackingPage = false;

        return;
    }

}

public class AlternateHotel
{
    public string HotelCode;
    public HotelRoomAvailInfo AvailInfo;
    public HotelDescriptiveInfo DescriptiveInfo;
}
