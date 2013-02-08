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

public partial class SearchHotel : XnGR_WBS_Page
{
    private ProfileLoginControl ucProfileLoginControl;
    private ProfileLoginNameControl ucProfileLoginNameControl;
    private LanguageSelectorControl ucLanguageSelectorControl;
    private BookingStepControl ucBookingStepControl;
    private ErrorDisplayControl ucErrorDisplayControl;
    private RestrictionDateDisplayControl ucRestrictionDateDisplayControl;
    private StayCriteriaSelectorControl ucStayCriteriaControl;
    private TrackingCodeControl ucHeadTrackingCodeControl;
    private TrackingCodeControl ucBodyTrackingCodeControl;

    private bool bAsyncGetHotelSearchAreaList;
    private bool bAsyncGetHotelSearchPropertyList;
    private bool bAsyncGetLoginProfile;
    private bool bAsyncGetLinkedProfile;

    private bool bProcessProfileLogin;

    private string strLinkedProfileIdentifier;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        this.LoadLanguageSelectorControl();
        this.LoadProfileLoginControl();
        this.LoadProfileLoginNameControl();
        this.LoadBookingStepControl();
        this.LoadErrorDisplayControl();
        this.LoadRestrictionDateDisplayControl();
        this.LoadStaySelectorControl();

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        bAsyncGetHotelSearchAreaList = false;
        bAsyncGetHotelSearchPropertyList = false;
        bAsyncGetLoginProfile = false;
        bAsyncGetLinkedProfile = false;

        bProcessProfileLogin = false;

        if (!IsPostBack)
        {
            bAsyncGetHotelSearchAreaList = true;
            bAsyncGetHotelSearchPropertyList = true;
        }

        else
        {
            this.ConfigureLanguageSelectorControl();
            this.ConfigureProfileLoginControl();
            this.ConfigureProfileLoginNameControl();
            this.ConfigureBookingStepControl();
            this.ConfigureErrorDisplayControl();
            this.ConfigureRestrictionDateDisplayControl();
            this.ConfigureStaySelectorControl();
        }

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetHotelSearchAreaList || bAsyncGetHotelSearchPropertyList || bAsyncGetLoginProfile || bAsyncGetLinkedProfile)
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
        if (bAsyncGetHotelSearchAreaList)
        {
            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitHotelSearchAreaListRQ(ref wbsAPIRouterData);
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelSearchAreaListComplete), null, false);
        }

        else if (bAsyncGetHotelSearchPropertyList)
        {
            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitHotelSearchPropertyListRQ(ref wbsAPIRouterData);
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelSearchPropertyListComplete), null, false);
        }

        else if (bAsyncGetLoginProfile)
        {
            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitLoginProfileRQ(ref wbsAPIRouterData, ucProfileLoginControl.ProfileLoginInfo.LogonName);
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(LoginProfileComplete), null, false);
        }

        else if (bAsyncGetLinkedProfile)
        {
            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitLinkedProfileRQ(ref wbsAPIRouterData, strLinkedProfileIdentifier);
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(LinkedProfileComplete), null, false);
        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    private void HotelSearchAreaListComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessHotelSearchAreaListRS(ref wbsAPIRouterData))
        {
            bAsyncGetHotelSearchAreaList = false;
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

    private void HotelSearchPropertyListComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessHotelSearchPropertyListRS(ref wbsAPIRouterData))
        {
            bAsyncGetHotelSearchPropertyList = false;
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

    private void LoginProfileComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessLoginProfileRS(ref wbsAPIRouterData))
        {
            ProfileReadRS objLoginProfileReadRS = (ProfileReadRS)Session["LoginProfileReadRS"];
            ProfileIdentifier objLinkedProfileIdentifier = ProfileHelper.GetProfileIdentifier(objLoginProfileReadRS.Profile, ProfileIdentifierType.LinkedProfileID);

            if (objLinkedProfileIdentifier != null)
            {
                strLinkedProfileIdentifier = objLinkedProfileIdentifier.Identifier;
                bAsyncGetLinkedProfile = true;
            }

            bAsyncGetLoginProfile = false;
            this.BeginResumeAsyncDataCapture();
        }

        else
        {
            Session["LoginErrors"] = this.PageErrors;
            Session["ViewLoginForm"] = true;

            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    private void LinkedProfileComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessLinkedProfileRS(ref wbsAPIRouterData))
        {
            bAsyncGetLinkedProfile = false;
            this.BeginResumeAsyncDataCapture();
        }

        else
        {
            Session["LoginErrors"] = this.PageErrors;
            Session["ViewLoginForm"] = true;

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
        if (bProcessProfileLogin && !this.IsPageError)
        {
            this.ProcessProfileLogin();
        }

        this.IsParentPreRender = true;

        this.ConfigureLanguageSelectorControl();
        this.ConfigureProfileLoginControl();
        this.ConfigureProfileLoginNameControl();
        this.ConfigureBookingStepControl();
        this.ConfigureErrorDisplayControl();
        this.ConfigureRestrictionDateDisplayControl();
        this.ConfigureStaySelectorControl();
        this.ConfigureTrackingCodeControl();

        ucLanguageSelectorControl.RenderUserControl();
        ucProfileLoginControl.RenderUserControl();
        ucProfileLoginNameControl.RenderUserControl();
        ucBookingStepControl.RenderUserControl();
        ucErrorDisplayControl.RenderUserControl();
        ucRestrictionDateDisplayControl.RenderUserControl();
        ucStayCriteriaControl.RenderUserControl();
        ucHeadTrackingCodeControl.RenderUserControl();
        ucBodyTrackingCodeControl.RenderUserControl();

        this.PageComplete();

        return;
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
        WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
        WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect("~/Pages/SearchHotel.aspx");

        return;
    }

    public void ProfileLogin(object sender, EventArgs e)
    {
        if (ucProfileLoginControl.ViewLoginForm)
        {
            Session["ViewLoginForm"] = true;
            Session["LoginProfiles"] = new Profile[0];
            Session["LoginErrors"] = new string[0];
        }

        else
        {
            Session["ViewLoginForm"] = false;
            Session["LoginProfiles"] = new Profile[0];
            Session["LoginErrors"] = new string[0];

            if (ucProfileLoginControl.Authenticate)
            {
                Session["ProfileLoginInfo"] = ucProfileLoginControl.ProfileLoginInfo;

                if (ucProfileLoginControl.ProfileLoginInfo.LogonName.Trim() == "")
                    this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoLoginNameEntry"));

                if (ucProfileLoginControl.ProfileLoginInfo.LogonPassword.Trim() == "")
                    this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoLoginPasswordEntry"));

                if (this.IsPageError)
                {
                    Session["LoginErrors"] = this.PageErrors;
                    Session["ViewLoginForm"] = true;

                    return;
                }

                // Setup to retrieve login / linked profiles and authenticate login

                bProcessProfileLogin = true;
                bAsyncGetLoginProfile = true;
            }

        }

        return;
    }

    public void ProfileLogout(object sender, EventArgs e)
    {
        WbsUiHelper.InitProfileLoginInfo();
        WbsUiHelper.InitStayCriteriaSelection();
        WbsUiHelper.InitRoomRateSelections();
        WbsUiHelper.InitAddOnPackageSelections();
        WbsUiHelper.InitGuestDetailsEntryInfo();

        if (this.IsGuestDetailsTestPrefill)
            WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

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

            if (ucStayCriteriaControl.StayCriteriaSelection.HotelCode == null || ucStayCriteriaControl.StayCriteriaSelection.HotelCode == "")
                Response.Redirect("~/Pages/SearchForHotels.aspx");
            else
                Response.Redirect("~/Pages/CheckAvailability.aspx");
        }

        else
        {

        }

        return;
    }

    protected void SelectNewCountry(object sender, EventArgs e)
    {
        Session[Constants.Sessions.StayCriteriaSelection] = ucStayCriteriaControl.StayCriteriaSelection;

        return;
    }

    protected void SelectNewArea(object sender, EventArgs e)
    {
        Session[Constants.Sessions.StayCriteriaSelection] = ucStayCriteriaControl.StayCriteriaSelection;

        return;
    }

    protected void SelectNewHotel(object sender, EventArgs e)
    {
        return; // Not used in StaySelectorControl "new" mode
    }

    private void ValidateStayCriteria(StayCriteriaSelection objStayCriteriaSelection)
    {
        if (objStayCriteriaSelection.CountryCode == null || objStayCriteriaSelection.CountryCode == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoCountrySelection"));

        if (objStayCriteriaSelection.AreaID == null || objStayCriteriaSelection.AreaID == "")
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoDestinationSelection"));

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

        if (!this.IsPageValidationError)
        {
            if (WbsUiHelper.IsBookingRestrictedDate(objStayCriteriaSelection))
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "RestrictedBookingDateRequested"));
        }

        return;
    }

    private void ProcessProfileLogin()
    {
        List<Profile> lProfiles = new List<Profile>();

        ProfileLoginInfo objProfileLoginInfo = (ProfileLoginInfo)Session["ProfileLoginInfo"];
        ProfileReadRS objLoginProfileReadRS = (ProfileReadRS)Session["LoginProfileReadRS"];

        if (objProfileLoginInfo.LogonPassword.Trim() != objLoginProfileReadRS.Profile.Credentials.LogonPassword)
        {
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "LoginCredentialsNotValid"));

            Session["LoginErrors"] = this.PageErrors;
            Session["ViewLoginForm"] = true;

            return;
        }

        lProfiles.Add(objLoginProfileReadRS.Profile);

        ProfileIdentifier objLinkedProfileIdentifier = ProfileHelper.GetProfileIdentifier(objLoginProfileReadRS.Profile, ProfileIdentifierType.LinkedProfileID);

        if (objLinkedProfileIdentifier != null)
        {
            ProfileReadRS objLinkedProfileReadRS = (ProfileReadRS)Session["LinkedProfileReadRS"];
            lProfiles.Add(objLinkedProfileReadRS.Profile);
        }

        Session["LoginProfiles"] = lProfiles.ToArray();
        Session["IsLoggedIn"] = true;

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

        ucLanguageSelectorControl.Clear();

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

    private void LoadProfileLoginControl()
    {
        string strProfileLoginControlPath = ConfigurationManager.AppSettings["ProfileLoginControl.ascx"];
        ucProfileLoginControl = (ProfileLoginControl)LoadControl(strProfileLoginControlPath);

        phProfileLoginControl.Controls.Clear();
        phProfileLoginControl.Controls.Add(ucProfileLoginControl);

        ucProfileLoginControl.ProfileLogin += new ProfileLoginControl.ProfileLoginEvent(this.ProfileLogin);
        ucProfileLoginControl.ProfileLogout += new ProfileLoginControl.ProfileLogoutEvent(this.ProfileLogout);

        return;
    }

    private void ConfigureProfileLoginControl()
    {
        ProfileLoginInfo objProfileLoginInfo = (ProfileLoginInfo)Session["ProfileLoginInfo"];
        Profile[] objProfiles = (Profile[])Session["LoginProfiles"];
        bool bIsLoggedIn = (bool)Session["IsLoggedIn"];
        bool bViewLoginForm = (bool)Session["ViewLoginForm"];
        string[] strLoginErrors = (string[])Session["LoginErrors"];

        ucProfileLoginControl.ID = "ProfileLogin";
        ucProfileLoginControl.ProfileLoginInfo = objProfileLoginInfo;
        ucProfileLoginControl.Profiles = objProfiles;
        ucProfileLoginControl.IsLoggedIn = bIsLoggedIn;
        ucProfileLoginControl.ViewLoginForm = bViewLoginForm;
        ucProfileLoginControl.LoginErrors = strLoginErrors;

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

        ucBookingStepControl.SelectedStep = "SearchHotel";

        BookingStepItemControl step1 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
        this.ucBookingStepControl.Add(step1);
        step1.ID = "SearchHotel";
        step1.StepRefID = "SearchHotel";
        step1.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep1");
        step1.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepSearchHotel");
        step1.Selected = true;
        step1.Clickable = false;

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
        step6.Selected = false;
        step6.Clickable = false;

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

    private void LoadRestrictionDateDisplayControl()
    {
        string strRestrictionDateDisplayControlPath = ConfigurationManager.AppSettings["RestrictionDateDisplayControl.ascx"];
        ucRestrictionDateDisplayControl = (RestrictionDateDisplayControl)LoadControl(strRestrictionDateDisplayControlPath);

        phRestrictionDateDisplayControl.Controls.Clear();
        phRestrictionDateDisplayControl.Controls.Add(ucRestrictionDateDisplayControl);

        return;
    }

    private void ConfigureRestrictionDateDisplayControl()
    {
        ucRestrictionDateDisplayControl.RestrictionDateInfos = this.GetBookingRestrictionNoticeMessages("");

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

        CountryListItem[] objCountryListItems = this.GetCountryList(objAreaListHotelSearchRS.AreaListItems);
        AreaListItem[] objAreaListItems = new AreaListItem[0];
        HotelListItem[] objHotelListItems = new HotelListItem[0];

        bool bCountrySelected = false;
        bool bAreaSelected = false;
        bool bHotelSelected = false;

        if (objStayCriteriaSelection.CountryCode != null && objStayCriteriaSelection.CountryCode != "")
            bCountrySelected = true;

        if (objStayCriteriaSelection.AreaID != null && objStayCriteriaSelection.AreaID != "")
            bAreaSelected = true;

        if (objStayCriteriaSelection.HotelCode != null && objStayCriteriaSelection.HotelCode != "")
            bHotelSelected = true;

        if (!bCountrySelected && !bAreaSelected && bHotelSelected) // can occurr from deep link page, init country and area selections
        {
            bool bHotelLocated = false;

            for (int i = 0; i < objPropertyListHotelSearchRS.HotelListItems.Length; i++)
            {
                if (objPropertyListHotelSearchRS.HotelListItems[i].HotelCode == objStayCriteriaSelection.HotelCode)
                {
                    if (objPropertyListHotelSearchRS.HotelListItems[i].AreaIDs != null && objPropertyListHotelSearchRS.HotelListItems[i].AreaIDs.Length != 0)
                    {
                        string strCountryCode = this.GetCountryCode(objPropertyListHotelSearchRS.HotelListItems[i].AreaIDs[0], objAreaListHotelSearchRS.AreaListItems);

                        if (strCountryCode != null && strCountryCode != "")
                        {
                            objStayCriteriaSelection.CountryCode = strCountryCode;
                            bCountrySelected = true;

                            objStayCriteriaSelection.AreaID = objPropertyListHotelSearchRS.HotelListItems[i].AreaIDs[0];
                            bAreaSelected = true;

                            Session[Constants.Sessions.StayCriteriaSelection] = objStayCriteriaSelection;

                            bHotelLocated = true;
                            break;
                        }

                        else
                        {
                            break;
                        }

                    }

                    else
                    {
                        break;
                    }

                }

            }

            if (!bHotelLocated)
            {
                objStayCriteriaSelection.HotelCode = "";
                Session[Constants.Sessions.StayCriteriaSelection] = objStayCriteriaSelection;
                bHotelSelected = false;
            }

        }

        if (!bCountrySelected && bAreaSelected) // can occurr from deep link page, init country selection
        {
            string strCountryCode = this.GetCountryCode(objStayCriteriaSelection.AreaID, objAreaListHotelSearchRS.AreaListItems);

            if (strCountryCode != null && strCountryCode != "")
            {
                objStayCriteriaSelection.CountryCode = strCountryCode;
                Session[Constants.Sessions.StayCriteriaSelection] = objStayCriteriaSelection;
                bCountrySelected = true;
            }

        }

        if (bCountrySelected)
        {
            objAreaListItems = this.GetCountryAreaList(objStayCriteriaSelection.CountryCode, objAreaListHotelSearchRS.AreaListItems);
        }

        if (!bCountrySelected && objCountryListItems.Length == 1) // default country selection if only 1 choice
        {
            objStayCriteriaSelection.CountryCode = objCountryListItems[0].CountryCode;
            bCountrySelected = true;

            objAreaListItems = this.GetCountryAreaList(objStayCriteriaSelection.CountryCode, objAreaListHotelSearchRS.AreaListItems);

            objStayCriteriaSelection.AreaID = "";
            bAreaSelected = false;

            objStayCriteriaSelection.HotelCode = "";
            bHotelSelected = false;
        }

        if (bCountrySelected && !bAreaSelected && objAreaListItems.Length == 1)  // default area selection if only 1 choice
        {
            objStayCriteriaSelection.AreaID = objAreaListItems[0].AreaID;
            bAreaSelected = true;

            objStayCriteriaSelection.HotelCode = "";
            bHotelSelected = false;
        }

        if (bCountrySelected && bAreaSelected)
        {
            objHotelListItems = this.GetAreaHotelList(objStayCriteriaSelection.AreaID, objPropertyListHotelSearchRS.HotelListItems);

            if (objHotelListItems.Length == 1) // default hotel selection if only 1 choice
                objStayCriteriaSelection.HotelCode = objHotelListItems[0].HotelCode;
        }

        ucStayCriteriaControl.Clear();

        ucStayCriteriaControl.ID = "StayCriteriaSelector";
        ucStayCriteriaControl.StayCriteriaSelectorType = StayCriteriaSelectorType.HotelSearch;
        ucStayCriteriaControl.StayCriteriaSelectorMode = StayCriteriaSelectorMode.New;
        ucStayCriteriaControl.CountryListItems = objCountryListItems;
        ucStayCriteriaControl.AreaListItems = objAreaListItems;
        ucStayCriteriaControl.HotelListItems = objHotelListItems;
        ucStayCriteriaControl.StayCriteriaSelection = objStayCriteriaSelection;
        ucStayCriteriaControl.HotelDescriptiveInfo = null;

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
                ucTrackingCodeItemControl.PageUrl = ("/book/SearchHotel");
                ucTrackingCodeItemControl.Amount = 0;
            }

        }

        this.IsFirstTrackingPage = false;

        return;
    }

}
