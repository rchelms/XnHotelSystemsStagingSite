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

public partial class SelectRoom : XnGR_WBS_Page
{
    private ProfileLoginNameControl ucProfileLoginNameControl;
    private LanguageSelectorControl ucLanguageSelectorControl;
    private BookingStepControl ucBookingStepControl;
    private ErrorDisplayControl ucErrorDisplayControl;
    private RestrictionDateDisplayControl ucRestrictionDateDisplayControl;
    private StayCriteriaSelectorControl ucStayCriteriaControl;
    private AvailCalSelectorControl ucAvailCalSelectorControl;
    private RoomRateSelectorControl ucRoomRateSelectorControl;
    private RoomRateSelectorGridControl ucRoomRateSelectorGridControl;
    private TrackingCodeControl ucHeadTrackingCodeControl;
    private TrackingCodeControl ucBodyTrackingCodeControl;

    private bool bAsyncGetHotelAvailInfo;
    private bool bAsyncGetHotelAvailCalendarInfo;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        this.LoadLanguageSelectorControl();
        this.LoadProfileLoginNameControl();
        this.LoadBookingStepControl();
        this.LoadErrorDisplayControl();
        this.LoadRestrictionDateDisplayControl();
        this.LoadStaySelectorControl();
        this.LoadAvailCalSelectorControl();
        this.LoadRoomRateSelectControl();

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        bAsyncGetHotelAvailInfo = false;
        bAsyncGetHotelAvailCalendarInfo = false;

        if (!IsPostBack)
        {
            Session["SelectedRoom"] = "1";
            Session["AvailCalRequested"] = false;

            WbsUiHelper.UpdateRatePlanPaymentPolicies();
        }

        else
        {
            this.ConfigureLanguageSelectorControl();
            this.ConfigureProfileLoginNameControl();
            this.ConfigureBookingStepControl();
            this.ConfigureErrorDisplayControl();
            this.ConfigureRestrictionDateDisplayControl();
            this.ConfigureStaySelectorControl();
            this.ConfigureAvailCalSelectorControl();
            this.ConfigureRoomRateSelectControl();
        }

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetHotelAvailInfo || bAsyncGetHotelAvailCalendarInfo)
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
        if (bAsyncGetHotelAvailInfo)
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
            DateTime dtRateGridStartDate = (DateTime)Session["RateGridStartDate"];
            DateTime dtRateGridEndDate = dtRateGridStartDate.AddDays(this.NumberDaysInRateGrid - 1);

            DateTime dtAvailCalStartDate = objStayCriteriaSelection.ArrivalDate;

            if (dtRateGridStartDate.Date < dtAvailCalStartDate.Date)
                dtAvailCalStartDate = dtRateGridStartDate;

            DateTime dtAvailCalEndDate = objStayCriteriaSelection.DepartureDate.AddDays(-1);

            if (dtRateGridEndDate.Date > dtAvailCalEndDate.Date)
                dtAvailCalEndDate = dtRateGridEndDate;

            int intAvailCalNumDays = ((TimeSpan)(dtRateGridEndDate.Date.Subtract(dtAvailCalStartDate.Date))).Days + 1;

            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitHotelAvailInfoRQ(ref wbsAPIRouterData, objStayCriteriaSelection, dtAvailCalStartDate, intAvailCalNumDays, this.WbsUiHelper.GetLoginLinkedProfileIdentifier());
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelAvailInfoComplete), null, false);
        }

        else if (bAsyncGetHotelAvailCalendarInfo)
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

    private void HotelAvailInfoComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessHotelAvailInfoRS(ref wbsAPIRouterData))
        {
            WbsUiHelper.UpdateRatePlanPaymentPolicies();

            bAsyncGetHotelAvailInfo = false;
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

    private void HotelAvailCalendarInfoComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessHotelAvailCalendarInfoRS(ref wbsAPIRouterData))
        {
            this.InitAvailCalDateSelections();
            Session["AvailCalRequested"] = true;

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
            this.ConfigureRestrictionDateDisplayControl();
            this.ConfigureStaySelectorControl();
            this.ConfigureAvailCalSelectorControl();
            this.ConfigureRoomRateSelectControl();
            this.ConfigureTrackingCodeControl();

            ucLanguageSelectorControl.RenderUserControl();
            ucProfileLoginNameControl.RenderUserControl();
            ucBookingStepControl.RenderUserControl();
            ucErrorDisplayControl.RenderUserControl();
            ucRestrictionDateDisplayControl.RenderUserControl();
            ucStayCriteriaControl.RenderUserControl();
            ucHeadTrackingCodeControl.RenderUserControl();
            ucBodyTrackingCodeControl.RenderUserControl();

            if (ucAvailCalSelectorControl != null)
                ucAvailCalSelectorControl.RenderUserControl();

            if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
                ucRoomRateSelectorControl.RenderUserControl();
            else
                ucRoomRateSelectorGridControl.RenderUserControl();
        }

        this.PageComplete();

        return;
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
        WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
        WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect("~/Pages/SelectRoom.aspx");

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
            panAvailCalSelectorControl.Visible = false;
            panRoomRateSelectorControl.Visible = false;
        }

        return;
    }

    protected void SelectNewCountry(object sender, EventArgs e)
    {
        return; // Not used in Select Room screen
    }

    protected void SelectNewArea(object sender, EventArgs e)
    {
        return; // Not used in Select Room screen
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

    protected void RoomSelected(object sender, string selectedRoomRefID)
    {
        Session["SelectedRoom"] = selectedRoomRefID;

        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];

        for (int i = 0; i < objRoomRateSelections.Length; i++)
        {
            if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
            {
                if (objRoomRateSelections[i].RoomRefID == ucRoomRateSelectorControl.RoomRateSelection.RoomRefID)
                {
                    objRoomRateSelections[i] = ucRoomRateSelectorControl.RoomRateSelection;
                    break;
                }

            }

            else
            {
                if (objRoomRateSelections[i].RoomRefID == ucRoomRateSelectorGridControl.RoomRateSelection.RoomRefID)
                {
                    objRoomRateSelections[i] = ucRoomRateSelectorGridControl.RoomRateSelection;
                    break;
                }

            }

        }

        Session["RoomRateSelections"] = objRoomRateSelections;

        return;
    }

    protected void RateGridDateSelected(object sender, RateGridEventArgs objRateGridEventArgs)
    {
        Session["RateGridStartDate"] = objRateGridEventArgs.NewStartDate.Date;

        if (objRateGridEventArgs.Operation == RateGridOperation.DateSelected)
        {
            WbsUiHelper.SyncStayDatesToRateGrid(objRateGridEventArgs.NewStartDate);

            WbsUiHelper.InitRoomRateSelections();
            WbsUiHelper.InitAddOnPackageSelections();
            WbsUiHelper.InitGuestDetailsEntryInfo();

            if (this.IsGuestDetailsTestPrefill)
                WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

            bAsyncGetHotelAvailInfo = true;
        }

        else if (objRateGridEventArgs.Operation == RateGridOperation.MovePrevious || objRateGridEventArgs.Operation == RateGridOperation.MoveNext)
        {
            string strSelectedRoom = (string)Session["SelectedRoom"];
            HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];

            AvailabilityCalendar objAvailabilityCalendar = new AvailabilityCalendar();

            for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
            {
                if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].SegmentRefID == strSelectedRoom)
                {
                    objAvailabilityCalendar = objHotelAvailabilityRS.HotelRoomAvailInfos[i].AvailabilityCalendar;
                    break;
                }

            }

            if (!WbsUiHelper.IsRateGridDataCached(objAvailabilityCalendar, objRateGridEventArgs.NewStartDate, this.NumberDaysInRateGrid))
            {
                bAsyncGetHotelAvailInfo = true;
            }

        }

        return;
    }

    protected void RoomRateCompleted(object sender, EventArgs e)
    {
        string strCurrentSelectedRoomRefID = (string)Session["SelectedRoom"];

        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];

        for (int i = 0; i < objRoomRateSelections.Length; i++)
        {
            if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
            {
                if (objRoomRateSelections[i].RoomRefID == ucRoomRateSelectorControl.RoomRateSelection.RoomRefID)
                {
                    objRoomRateSelections[i] = ucRoomRateSelectorControl.RoomRateSelection;
                    break;
                }

            }

            else
            {
                if (objRoomRateSelections[i].RoomRefID == ucRoomRateSelectorGridControl.RoomRateSelection.RoomRefID)
                {
                    objRoomRateSelections[i] = ucRoomRateSelectorGridControl.RoomRateSelection;
                    break;
                }

            }

        }

        this.ValidateRoomRates(objRoomRateSelections);

        Session["RoomRateSelections"] = objRoomRateSelections;

        if (!this.IsPageError)
        {
            WbsUiHelper.InitAddOnPackageSelections();
            WbsUiHelper.InitGuestDetailsEntryInfo();

            if (this.IsGuestDetailsTestPrefill)
                WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

            Response.Redirect("~/Pages/SelectAddOn.aspx");
        }

        else
        {

        }

        return;
    }

    protected void AvailCalRequested(object sender, bool bViewCalendar)
    {
        Session["AvailCalRequested"] = false;

        if (bViewCalendar)
        {
            bAsyncGetHotelAvailCalendarInfo = true;
        }

        return;
    }

    public void ShowMoreLessRatesRequested(object sender, EventArgs e)
    {
        ShowMoreRatesIndicator[] objShowMoreRatesIndicators = (ShowMoreRatesIndicator[])Session["ShowMoreRatesIndicators"];

        List<ShowMoreRatesIndicator> lNewShowMoreRatesIndicators = new List<ShowMoreRatesIndicator>();

        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
        {
            RoomRateSelectorControl objRoomRateSelectorControl = (RoomRateSelectorControl)sender;

            for (int i = 0; i < objShowMoreRatesIndicators.Length; i++)
            {
                if (objShowMoreRatesIndicators[i].RoomRefID != objRoomRateSelectorControl.RoomRefID)
                    lNewShowMoreRatesIndicators.Add(objShowMoreRatesIndicators[i]);
            }

            for (int i = 0; i < objRoomRateSelectorControl.RoomTypeSelectorItems.Length; i++)
            {
                if (objRoomRateSelectorControl.RoomTypeSelectorItems[i].ShowMoreRates)
                {
                    ShowMoreRatesIndicator objShowMoreRatesIndicator = new ShowMoreRatesIndicator();
                    lNewShowMoreRatesIndicators.Add(objShowMoreRatesIndicator);

                    objShowMoreRatesIndicator.RoomRefID = objRoomRateSelectorControl.RoomRefID;
                    objShowMoreRatesIndicator.RoomTypeCode = objRoomRateSelectorControl.RoomTypeSelectorItems[i].RoomType.Code;
                }

            }

        }

        else
        {
            RoomRateSelectorGridControl objRoomRateSelectorGridControl = (RoomRateSelectorGridControl)sender;

            for (int i = 0; i < objShowMoreRatesIndicators.Length; i++)
            {
                if (objShowMoreRatesIndicators[i].RoomRefID != objRoomRateSelectorGridControl.RoomRefID)
                    lNewShowMoreRatesIndicators.Add(objShowMoreRatesIndicators[i]);
            }

            for (int i = 0; i < objRoomRateSelectorGridControl.RoomTypeSelectorGridItems.Length; i++)
            {
                if (objRoomRateSelectorGridControl.RoomTypeSelectorGridItems[i].ShowMoreRates)
                {
                    ShowMoreRatesIndicator objShowMoreRatesIndicator = new ShowMoreRatesIndicator();
                    lNewShowMoreRatesIndicators.Add(objShowMoreRatesIndicator);

                    objShowMoreRatesIndicator.RoomRefID = objRoomRateSelectorGridControl.RoomRefID;
                    objShowMoreRatesIndicator.RoomTypeCode = objRoomRateSelectorGridControl.RoomTypeSelectorGridItems[i].RoomType.Code;
                }

            }

        }

        Session["ShowMoreRatesIndicators"] = lNewShowMoreRatesIndicators.ToArray();

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

        if (!this.IsPageValidationError)
        {
            if (WbsUiHelper.IsBookingRestrictedDate(objStayCriteriaSelection))
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "RestrictedBookingDateRequested"));
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

    private void ValidateRoomRates(RoomRateSelection[] objRoomRateSelections)
    {
        for (int i = 0; i < objRoomRateSelections.Length; i++)
        {
            if (objRoomRateSelections[i].RoomTypeCode == null || objRoomRateSelections[i].RoomTypeCode == "" || objRoomRateSelections[i].RatePlanCode == null || objRoomRateSelections[i].RatePlanCode == "")
            {
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoRoomRateSelection"));
                break;
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
        ucRestrictionDateDisplayControl.RestrictionDateInfos = this.GetBookingRestrictionNoticeMessages(((StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection]).HotelCode);

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
        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
        {
            string strAvailCalSelectorControlPath = ConfigurationManager.AppSettings["AvailCalSelectorControl.ascx"];
            ucAvailCalSelectorControl = (AvailCalSelectorControl)LoadControl(strAvailCalSelectorControlPath);

            phAvailCalSelectorControl.Controls.Clear();
            phAvailCalSelectorControl.Controls.Add(ucAvailCalSelectorControl);

            ucAvailCalSelectorControl.AvailCalRequested += new AvailCalSelectorControl.AvailCalRequestedEvent(this.AvailCalRequested);
            ucAvailCalSelectorControl.AvailCalCompleted += new AvailCalSelectorControl.AvailCalCompletedEvent(this.AvailCalCompleted);
        }

        else
        {
            panAvailCalSelectorControl.Visible = false;
        }

        return;
    }

    private void ConfigureAvailCalSelectorControl()
    {
        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
            HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityCalendarRS"];
            DateTime[] objAvailCalDateSelections = (DateTime[])Session["AvailCalDateSelections"];

            DateTime dtToday = TZNet.ToLocal(WbsUiHelper.GetTimeZone(objStayCriteriaSelection.HotelCode), DateTime.UtcNow).Date;

            if ((bool)Session["AvailCalRequested"] == false)
            {
                ucAvailCalSelectorControl.AvailCalendarSelectorMode = AvailCalendarSelectorMode.ViewLink;
                ucAvailCalSelectorControl.AvailabilityCalendarInfo = new AvailabilityCalendarInfo[0];
                ucAvailCalSelectorControl.SelectedDates = objAvailCalDateSelections;
                ucAvailCalSelectorControl.StayCriteriaSelection = objStayCriteriaSelection;
                ucAvailCalSelectorControl.Today = dtToday.Date;
            }

            else
            {
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
            }

        }

        return;
    }

    protected void LoadRoomRateSelectControl()
    {
        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
        {
            string strRoomRateSelectorControlPath = ConfigurationManager.AppSettings["RoomRateSelectorControl.ascx"];
            ucRoomRateSelectorControl = (RoomRateSelectorControl)LoadControl(strRoomRateSelectorControlPath);

            phRoomRateSelectorControl.Controls.Clear();
            phRoomRateSelectorControl.Controls.Add(ucRoomRateSelectorControl);

            ucRoomRateSelectorControl.RoomSelected += new RoomRateSelectorControl.RoomSelectedEvent(this.RoomSelected);
            ucRoomRateSelectorControl.RoomRateCompleted += new RoomRateSelectorControl.RoomRateCompletedEvent(this.RoomRateCompleted);
            ucRoomRateSelectorControl.ShowMoreLessRates += new RoomRateSelectorControl.ShowMoreLessRatesEvent(this.ShowMoreLessRatesRequested);
        }

        else
        {
            string strRoomRateSelectorGridControlPath = ConfigurationManager.AppSettings["RoomRateSelectorGridControl.ascx"];
            ucRoomRateSelectorGridControl = (RoomRateSelectorGridControl)LoadControl(strRoomRateSelectorGridControlPath);

            phRoomRateSelectorControl.Controls.Clear();
            phRoomRateSelectorControl.Controls.Add(ucRoomRateSelectorGridControl);

            ucRoomRateSelectorGridControl.RoomSelected += new RoomRateSelectorGridControl.RoomSelectedEvent(this.RoomSelected);
            ucRoomRateSelectorGridControl.RateGridDateSelected += new RoomRateSelectorGridControl.RateGridDateSelectedEvent(this.RateGridDateSelected);
            ucRoomRateSelectorGridControl.RoomRateCompleted += new RoomRateSelectorGridControl.RoomRateCompletedEvent(this.RoomRateCompleted);
            ucRoomRateSelectorGridControl.ShowMoreLessRates += new RoomRateSelectorGridControl.ShowMoreLessRatesEvent(this.ShowMoreLessRatesRequested);
        }

        return;
    }

    protected void ConfigureRoomRateSelectControl()
    {
        string strSelectedRoom = (string)Session["SelectedRoom"];

        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];

        ShowMoreRatesIndicator[] objShowMoreRatesIndicators = (ShowMoreRatesIndicator[])Session["ShowMoreRatesIndicators"];

        RoomOccupantSelection objRoomOccupantSelection = new RoomOccupantSelection();

        bool IsRoomRateDescriptionModel;

        if (ConfigurationManager.AppSettings["EnableRoomRateDescriptionModel"] == "1")
            IsRoomRateDescriptionModel = true;
        else
            IsRoomRateDescriptionModel = false;

        for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
        {
            if (objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID == strSelectedRoom)
            {
                objRoomOccupantSelection = objStayCriteriaSelection.RoomOccupantSelections[i];
                break;
            }

        }

        RoomRateSelection objRoomRateSelection = new RoomRateSelection();

        for (int i = 0; i < objRoomRateSelections.Length; i++)
        {
            if (objRoomRateSelections[i].RoomRefID == strSelectedRoom)
            {
                objRoomRateSelection = objRoomRateSelections[i];
                break;
            }

        } 

        HotelRoomAvailInfo objHotelRoomAvailInfo = new HotelRoomAvailInfo();

        for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
        {
            if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].SegmentRefID == strSelectedRoom)
            {
                objHotelRoomAvailInfo = objHotelAvailabilityRS.HotelRoomAvailInfos[i];
                break;
            }

        }

        HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
        {
            string strRoomSelectorItemControlPath = ConfigurationManager.AppSettings["RoomSelectorItemControl.ascx"];
            string strRoomTypeSelectorItemControlPath = ConfigurationManager.AppSettings["RoomTypeSelectorItemControl.ascx"];
            string strRatePlanSelectorItemControlPath = ConfigurationManager.AppSettings["RatePlanSelectorItemControl.ascx"];

            ucRoomRateSelectorControl.Clear();

            ucRoomRateSelectorControl.ID = "RoomRateSelector";
            ucRoomRateSelectorControl.RoomRefID = strSelectedRoom;
            ucRoomRateSelectorControl.HotelRoomAvailInfo = objHotelRoomAvailInfo;
            ucRoomRateSelectorControl.RoomOccupantSelection = objRoomOccupantSelection;

            for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
            {
                RoomSelectorItemControl ucRoomSelectorItemControl = (RoomSelectorItemControl)LoadControl(strRoomSelectorItemControlPath);
                ucRoomRateSelectorControl.AddRoomSelectorItem(ucRoomSelectorItemControl);

                ucRoomSelectorItemControl.ID = "RoomSelectorItem" + objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
                ucRoomSelectorItemControl.RoomRefID = objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
                ucRoomSelectorItemControl.RoomRefIDMenuText = (String)GetGlobalResourceObject("SiteResources", "RoomSelectorMenuText") + " " + objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;

                if (objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID == strSelectedRoom)
                    ucRoomSelectorItemControl.Selected = true;
                else
                    ucRoomSelectorItemControl.Selected = false;
            }

            if (objHotelRoomAvailInfo.RoomTypes != null)
            {
                for (int i = 0; i < objHotelRoomAvailInfo.RoomTypes.Length; i++)
                {
                    bool bRatesAvailable = false;

                    for (int j = 0; j < objHotelRoomAvailInfo.RoomRates.Length; j++)
                    {
                        if (objHotelRoomAvailInfo.RoomRates[j].RoomTypeCode == objHotelRoomAvailInfo.RoomTypes[i].Code)
                        {
                            if (!IsRoomRateDescriptionModel)
                            {
                                bRatesAvailable = true;
                                break;
                            }

                            else
                            {
                                if (objHotelRoomAvailInfo.RoomRates[j].DescriptionStatus == RoomRateDescriptionStatus.Active)
                                {
                                    bRatesAvailable = true;
                                    break;
                                }

                                for (int k = 0; k < objHotelRoomAvailInfo.RatePlans.Length; k++)
                                {
                                    if (objHotelRoomAvailInfo.RatePlans[k].Code == objHotelRoomAvailInfo.RoomRates[j].RatePlanCode)
                                    {
                                        if (objHotelRoomAvailInfo.RatePlans[k].Type == RatePlanType.Negotiated || objHotelRoomAvailInfo.RatePlans[k].Type == RatePlanType.Consortia)
                                        {
                                            bRatesAvailable = true;
                                        }

                                        break;
                                    }

                                }

                                if (bRatesAvailable)
                                    break;
                            }

                        }

                    }

                    if (!bRatesAvailable)
                        continue;

                    RoomTypeSelectorItemControl ucRoomTypeSelectorItemControl = (RoomTypeSelectorItemControl)LoadControl(strRoomTypeSelectorItemControlPath);
                    ucRoomRateSelectorControl.AddRoomTypeSelectorItem(ucRoomTypeSelectorItemControl);

                    ucRoomTypeSelectorItemControl.ID = "RoomTypeSelectorItem" + objHotelRoomAvailInfo.RoomTypes[i].Code;
                    ucRoomTypeSelectorItemControl.RoomRefID = strSelectedRoom;

                    for (int j = 0; j < objHotelDescriptiveInfo.RoomTypes.Length; j++)
                    {
                        if (objHotelDescriptiveInfo.RoomTypes[j].Code == objHotelRoomAvailInfo.RoomTypes[i].Code)
                        {
                            ucRoomTypeSelectorItemControl.RoomType = objHotelDescriptiveInfo.RoomTypes[j];
                            break;
                        }

                    }

                    ucRoomTypeSelectorItemControl.ShowMoreRates = false;

                    for (int j = 0; j < objShowMoreRatesIndicators.Length; j++)
                    {
                        if (objShowMoreRatesIndicators[j].RoomRefID == ucRoomTypeSelectorItemControl.RoomRefID && objShowMoreRatesIndicators[j].RoomTypeCode == ucRoomTypeSelectorItemControl.RoomType.Code)
                        {
                            ucRoomTypeSelectorItemControl.ShowMoreRates = true;
                            break;
                        }

                    }

                    ucRoomTypeSelectorItemControl.Clear();

                    for (int k = 0; k < objHotelRoomAvailInfo.RoomRates.Length; k++)
                    {
                        if (objHotelRoomAvailInfo.RoomRates[k].RoomTypeCode == ucRoomTypeSelectorItemControl.RoomType.Code)
                        {
                            bool bListRatePlan = true;

                            if (IsRoomRateDescriptionModel && objHotelRoomAvailInfo.RoomRates[k].DescriptionStatus == RoomRateDescriptionStatus.Inactive)
                            {
                                for (int l = 0; l < objHotelRoomAvailInfo.RatePlans.Length; l++)
                                {
                                    if (objHotelRoomAvailInfo.RatePlans[l].Code == objHotelRoomAvailInfo.RoomRates[k].RatePlanCode)
                                    {
                                        if (objHotelRoomAvailInfo.RatePlans[l].Type != RatePlanType.Negotiated && objHotelRoomAvailInfo.RatePlans[l].Type != RatePlanType.Consortia)
                                        {
                                            bListRatePlan = false;
                                        }

                                        break;
                                    }

                                }

                            }

                            if (!bListRatePlan)
                                continue;

                            RatePlanSelectorItemControl ucRatePlanSelectorItemControl = (RatePlanSelectorItemControl)LoadControl(strRatePlanSelectorItemControlPath);
                            ucRoomTypeSelectorItemControl.AddRatePlanSelectorItem(ucRatePlanSelectorItemControl);

                            ucRatePlanSelectorItemControl.ID = "RatePlanSelectorItem" + objHotelRoomAvailInfo.RoomRates[k].RatePlanCode;
                            ucRatePlanSelectorItemControl.RoomRefID = strSelectedRoom;
                            ucRatePlanSelectorItemControl.RoomRate = objHotelRoomAvailInfo.RoomRates[k];
                            ucRatePlanSelectorItemControl.CreditCardCodes = objHotelDescriptiveInfo.CreditCardCodes;
                            ucRatePlanSelectorItemControl.Selected = false;

                            for (int m = 0; m < objHotelRoomAvailInfo.RatePlans.Length; m++)
                            {
                                if (objHotelRoomAvailInfo.RatePlans[m].Code == objHotelRoomAvailInfo.RoomRates[k].RatePlanCode)
                                {
                                    ucRatePlanSelectorItemControl.RatePlan = objHotelRoomAvailInfo.RatePlans[m];
                                    break;
                                }

                            }

                            if (objRoomRateSelection.RoomTypeCode == ucRatePlanSelectorItemControl.RoomRate.RoomTypeCode && objRoomRateSelection.RatePlanCode == ucRatePlanSelectorItemControl.RoomRate.RatePlanCode)
                                ucRatePlanSelectorItemControl.Selected = true;
                        }

                    }

                }

            }

        }

        else
        {
            AvailabilityCalendar objAvailabilityCalendar = new AvailabilityCalendar();

            for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
            {
                if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].SegmentRefID == strSelectedRoom)
                {
                    objAvailabilityCalendar = objHotelAvailabilityRS.HotelRoomAvailInfos[i].AvailabilityCalendar;
                    break;
                }

            }

            DateTime dtRateGridStartDate = (DateTime)Session["RateGridStartDate"];

            string strRoomSelectorItemControlPath = ConfigurationManager.AppSettings["RoomSelectorItemControl.ascx"];
            string strRoomTypeSelectorGridItemControlPath = ConfigurationManager.AppSettings["RoomTypeSelectorGridItemControl.ascx"];
            string strRatePlanSelectorGridItemControlPath = ConfigurationManager.AppSettings["RatePlanSelectorGridItemControl.ascx"];

            ucRoomRateSelectorGridControl.Clear();

            ucRoomRateSelectorGridControl.ID = "RoomRateSelector";
            ucRoomRateSelectorGridControl.RoomRefID = strSelectedRoom;
            ucRoomRateSelectorGridControl.HotelRoomAvailInfo = objHotelRoomAvailInfo;
            ucRoomRateSelectorGridControl.RoomOccupantSelection = objRoomOccupantSelection;
            ucRoomRateSelectorGridControl.GridTodayDate = TZNet.ToLocal(WbsUiHelper.GetTimeZone(objStayCriteriaSelection.HotelCode), DateTime.UtcNow).Date;
            ucRoomRateSelectorGridControl.GridStartDate = dtRateGridStartDate.Date;
            ucRoomRateSelectorGridControl.GridNumberDays = this.NumberDaysInRateGrid;

            for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
            {
                RoomSelectorItemControl ucRoomSelectorItemControl = (RoomSelectorItemControl)LoadControl(strRoomSelectorItemControlPath);
                ucRoomRateSelectorGridControl.AddRoomSelectorItem(ucRoomSelectorItemControl);

                ucRoomSelectorItemControl.ID = "RoomSelectorItem" + objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
                ucRoomSelectorItemControl.RoomRefID = objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
                ucRoomSelectorItemControl.RoomRefIDMenuText = (String)GetGlobalResourceObject("SiteResources", "RoomSelectorMenuText") + " " + objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;

                if (objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID == strSelectedRoom)
                    ucRoomSelectorItemControl.Selected = true;
                else
                    ucRoomSelectorItemControl.Selected = false;
            }

            if (objHotelRoomAvailInfo.RoomTypes != null && objAvailabilityCalendar.RoomRates != null)
            {
                for (int i = 0; i < objHotelRoomAvailInfo.RoomTypes.Length; i++)
                {
                    bool bRatesAvailable = false;

                    for (int j = 0; j < objAvailabilityCalendar.RoomRates.Length; j++)
                    {
                        if (objAvailabilityCalendar.RoomRates[j].RoomTypeCode == objHotelRoomAvailInfo.RoomTypes[i].Code)
                        {
                            if (!IsRoomRateDescriptionModel)
                            {
                                bRatesAvailable = true;
                                break;
                            }

                            else
                            {
                                if (objAvailabilityCalendar.RoomRates[j].DescriptionStatus == RoomRateDescriptionStatus.Active)
                                {
                                    bRatesAvailable = true;
                                    break;
                                }

                                for (int k = 0; k < objHotelRoomAvailInfo.RatePlans.Length; k++)
                                {
                                    if (objHotelRoomAvailInfo.RatePlans[k].Code == objAvailabilityCalendar.RoomRates[j].RatePlanCode)
                                    {
                                        if (objHotelRoomAvailInfo.RatePlans[k].Type == RatePlanType.Negotiated || objHotelRoomAvailInfo.RatePlans[k].Type == RatePlanType.Consortia)
                                        {
                                            bRatesAvailable = true;
                                        }

                                        break;
                                    }

                                }

                                if (bRatesAvailable)
                                    break;
                            }

                        }

                    }

                    if (!bRatesAvailable)
                        continue;

                    RoomTypeSelectorGridItemControl ucRoomTypeSelectorGridItemControl = (RoomTypeSelectorGridItemControl)LoadControl(strRoomTypeSelectorGridItemControlPath);
                    ucRoomRateSelectorGridControl.AddRoomTypeSelectorGridItem(ucRoomTypeSelectorGridItemControl);

                    ucRoomTypeSelectorGridItemControl.ID = "RoomTypeSelectorItem" + objHotelRoomAvailInfo.RoomTypes[i].Code;
                    ucRoomTypeSelectorGridItemControl.RoomRefID = strSelectedRoom;
                    ucRoomTypeSelectorGridItemControl.GridTodayDate = ucRoomRateSelectorGridControl.GridTodayDate;
                    ucRoomTypeSelectorGridItemControl.GridStartDate = ucRoomRateSelectorGridControl.GridStartDate;
                    ucRoomTypeSelectorGridItemControl.GridNumberDays = ucRoomRateSelectorGridControl.GridNumberDays;
                    ucRoomTypeSelectorGridItemControl.GridSelectedStartDate = objStayCriteriaSelection.ArrivalDate;
                    ucRoomTypeSelectorGridItemControl.GridSelectedEndDate = objStayCriteriaSelection.DepartureDate.AddDays(-1);

                    for (int j = 0; j < objHotelDescriptiveInfo.RoomTypes.Length; j++)
                    {
                        if (objHotelDescriptiveInfo.RoomTypes[j].Code == objHotelRoomAvailInfo.RoomTypes[i].Code)
                        {
                            ucRoomTypeSelectorGridItemControl.RoomType = objHotelDescriptiveInfo.RoomTypes[j];
                            break;
                        }

                    }

                    ucRoomTypeSelectorGridItemControl.ShowMoreRates = false;

                    for (int j = 0; j < objShowMoreRatesIndicators.Length; j++)
                    {
                        if (objShowMoreRatesIndicators[j].RoomRefID == ucRoomTypeSelectorGridItemControl.RoomRefID && objShowMoreRatesIndicators[j].RoomTypeCode == ucRoomTypeSelectorGridItemControl.RoomType.Code)
                        {
                            ucRoomTypeSelectorGridItemControl.ShowMoreRates = true;
                            break;
                        }

                    }

                    ucRoomTypeSelectorGridItemControl.Clear();

                    for (int k = 0; k < objAvailabilityCalendar.RoomRates.Length; k++)
                    {
                        if (objAvailabilityCalendar.RoomRates[k].RoomTypeCode == ucRoomTypeSelectorGridItemControl.RoomType.Code)
                        {
                            bool bListRatePlan = true;

                            if (IsRoomRateDescriptionModel && objAvailabilityCalendar.RoomRates[k].DescriptionStatus == RoomRateDescriptionStatus.Inactive)
                            {
                                for (int l = 0; l < objHotelRoomAvailInfo.RatePlans.Length; l++)
                                {
                                    if (objHotelRoomAvailInfo.RatePlans[l].Code == objAvailabilityCalendar.RoomRates[k].RatePlanCode)
                                    {
                                        if (objHotelRoomAvailInfo.RatePlans[l].Type != RatePlanType.Negotiated && objHotelRoomAvailInfo.RatePlans[l].Type != RatePlanType.Consortia)
                                        {
                                            bListRatePlan = false;
                                        }

                                        break;
                                    }

                                }

                            }

                            if (!bListRatePlan)
                                continue;

                            RatePlanSelectorGridItemControl ucRatePlanSelectorGridItemControl = (RatePlanSelectorGridItemControl)LoadControl(strRatePlanSelectorGridItemControlPath);
                            ucRoomTypeSelectorGridItemControl.AddRatePlanSelectorGridItem(ucRatePlanSelectorGridItemControl);

                            ucRatePlanSelectorGridItemControl.ID = "RatePlanSelectorItem" + objAvailabilityCalendar.RoomRates[k].RatePlanCode;
                            ucRatePlanSelectorGridItemControl.RoomRefID = strSelectedRoom;
                            ucRatePlanSelectorGridItemControl.GridStartDate = ucRoomTypeSelectorGridItemControl.GridStartDate;
                            ucRatePlanSelectorGridItemControl.GridNumberDays = ucRoomTypeSelectorGridItemControl.GridNumberDays;
                            ucRatePlanSelectorGridItemControl.GridRoomRate = objAvailabilityCalendar.RoomRates[k];
                            ucRatePlanSelectorGridItemControl.GridRoomRateStartDate = objAvailabilityCalendar.StartDate.Date;
                            ucRatePlanSelectorGridItemControl.ArrivalDate = objStayCriteriaSelection.ArrivalDate.Date;
                            ucRatePlanSelectorGridItemControl.TotalStayNights = ((TimeSpan)(objStayCriteriaSelection.DepartureDate.Date.Subtract(objStayCriteriaSelection.ArrivalDate.Date))).Days;
                            ucRatePlanSelectorGridItemControl.CreditCardCodes = objHotelDescriptiveInfo.CreditCardCodes;
                            ucRatePlanSelectorGridItemControl.Available = false;
                            ucRatePlanSelectorGridItemControl.Selected = false;

                            for (int m = 0; m < objHotelRoomAvailInfo.RatePlans.Length; m++)
                            {
                                if (objHotelRoomAvailInfo.RatePlans[m].Code == objAvailabilityCalendar.RoomRates[k].RatePlanCode)
                                {
                                    ucRatePlanSelectorGridItemControl.RatePlan = objHotelRoomAvailInfo.RatePlans[m];
                                    break;
                                }

                            }

                            for (int m = 0; m < objHotelRoomAvailInfo.RoomRates.Length; m++)
                            {
                                if (objHotelRoomAvailInfo.RoomRates[m].RoomTypeCode == objAvailabilityCalendar.RoomRates[k].RoomTypeCode && objHotelRoomAvailInfo.RoomRates[m].RatePlanCode == objAvailabilityCalendar.RoomRates[k].RatePlanCode)
                                {
                                    ucRatePlanSelectorGridItemControl.Available = true;
                                    ucRatePlanSelectorGridItemControl.RoomRate = objHotelRoomAvailInfo.RoomRates[m];
                                    break;
                                }

                            }

                            for (int m = 0; m < objAvailabilityCalendar.RatePlans.Length; m++)
                            {
                                if (objAvailabilityCalendar.RatePlans[m].RatePlanCode == objAvailabilityCalendar.RoomRates[k].RatePlanCode)
                                {
                                    ucRatePlanSelectorGridItemControl.GridRatePlan = objAvailabilityCalendar.RatePlans[m];
                                    break;
                                }

                            }

                            if (objRoomRateSelection.RoomTypeCode == objAvailabilityCalendar.RoomRates[k].RoomTypeCode && objRoomRateSelection.RatePlanCode == objAvailabilityCalendar.RoomRates[k].RatePlanCode)
                                ucRatePlanSelectorGridItemControl.Selected = true;
                        }

                    }

                }

            }

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
                ucTrackingCodeItemControl.PageUrl = ("/book/SelectRoom");
                ucTrackingCodeItemControl.Amount = 0;
            }

        }

        this.IsFirstTrackingPage = false;

        return;
    }

}
