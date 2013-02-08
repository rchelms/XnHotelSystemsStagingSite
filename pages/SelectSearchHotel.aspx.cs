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

public partial class SelectSearchHotel : XnGR_WBS_Page
{
    private ProfileLoginNameControl ucProfileLoginNameControl;
    private LanguageSelectorControl ucLanguageSelectorControl;
    private BookingStepControl ucBookingStepControl;
    private ErrorDisplayControl ucErrorDisplayControl;
    private SearchHotelSelectorControl ucSearchHotelSelectorControl;
    private SearchHotelSelectorGridControl ucSearchHotelSelectorGridControl;
    private TrackingCodeControl ucHeadTrackingCodeControl;
    private TrackingCodeControl ucBodyTrackingCodeControl;

    private bool bAsyncGetSearchHotelAvailCalendarInfo;

    private string[] SearchHotelCodes;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        this.LoadLanguageSelectorControl();
        this.LoadProfileLoginNameControl();
        this.LoadBookingStepControl();
        this.LoadErrorDisplayControl();
        this.LoadSearchHotelSelectorControl();

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        bAsyncGetSearchHotelAvailCalendarInfo = false;

        if (!IsPostBack)
        {
            HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["SearchHotelDescriptiveInfoRS"];

            if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos == null) // can occur with step back after deep link
            {
                Response.Redirect("~/Pages/SearchHotel.aspx");
            }

        }

        else
        {
            this.ConfigureLanguageSelectorControl();
            this.ConfigureProfileLoginNameControl();
            this.ConfigureBookingStepControl();
            this.ConfigureErrorDisplayControl();
            this.ConfigureSearchHotelSelectorControl();
        }

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetSearchHotelAvailCalendarInfo)
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
        if (bAsyncGetSearchHotelAvailCalendarInfo)
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
            DateTime dtSearchRateGridStartDate = (DateTime)Session["SearchRateGridStartDate"];
            HotelSearchRS objPropertyListHotelSearchRS = (HotelSearchRS)Session["PropertyListHotelSearchRS"];

            HotelListItem[] objHotelListItems = this.GetAreaHotelList(objStayCriteriaSelection.AreaID, objPropertyListHotelSearchRS.HotelListItems);

            List<string> lHotelCodes = new List<string>();

            for (int i = 0; i < objHotelListItems.Length; i++)
                lHotelCodes.Add(objHotelListItems[i].HotelCode);

            SearchHotelCodes = lHotelCodes.ToArray();

            if (SearchHotelCodes.Length != 0)
            {
                wbsAPIRouterData = new WBSAPIRouterData();
                this.WbsApiRouterHelper.InitSearchHotelAvailCalendarInfoRQ(ref wbsAPIRouterData, SearchHotelCodes, objStayCriteriaSelection, dtSearchRateGridStartDate, this.NumberDaysInRateGrid, this.WbsUiHelper.GetLoginLinkedProfileIdentifier());
                this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(SearchHotelAvailCalendarInfoComplete), null, false);
            }

            else
            {
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoHotelsLocatedInSearch"));

                // End async page operation

                if (!wbsIISAsyncResult.IsCompleted)
                    wbsIISAsyncResult.SetComplete();
            }

        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    private void SearchHotelAvailCalendarInfoComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessSearchHotelAvailCalendarInfoRS(ref wbsAPIRouterData))
        {
            HotelAvailabilityRS objSearchHotelAvailabilityCalendarRS = (HotelAvailabilityRS)Session["SearchHotelAvailabilityCalendarRS"];

            if (objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos != null && objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos.Length != 0)
            {
                bAsyncGetSearchHotelAvailCalendarInfo = false;
                this.BeginResumeAsyncDataCapture();
            }

            else
            {
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoHotelsLocatedInSearch"));

                // End async page operation

                if (!wbsIISAsyncResult.IsCompleted)
                    wbsIISAsyncResult.SetComplete();
            }

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
        this.IsParentPreRender = true;

        this.ConfigureLanguageSelectorControl();
        this.ConfigureProfileLoginNameControl();
        this.ConfigureBookingStepControl();
        this.ConfigureErrorDisplayControl();
        this.ConfigureSearchHotelSelectorControl();
        this.ConfigureTrackingCodeControl();

        ucLanguageSelectorControl.RenderUserControl();
        ucProfileLoginNameControl.RenderUserControl();
        ucBookingStepControl.RenderUserControl();
        ucErrorDisplayControl.RenderUserControl();
        ucHeadTrackingCodeControl.RenderUserControl();
        ucBodyTrackingCodeControl.RenderUserControl();

        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
            ucSearchHotelSelectorControl.RenderUserControl();
        else
            ucSearchHotelSelectorGridControl.RenderUserControl();

        this.PageComplete();

        return;
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
        WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
        WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect("~/Pages/SelectSearchHotel.aspx");

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

    protected void RateGridDateSelected(object sender, RateGridEventArgs objRateGridEventArgs)
    {
        Session["SearchRateGridStartDate"] = objRateGridEventArgs.NewStartDate.Date;

        if (objRateGridEventArgs.Operation == RateGridOperation.DateSelected)
        {
            WbsUiHelper.SyncStayDatesToRateGrid(objRateGridEventArgs.NewStartDate);

            bAsyncGetSearchHotelAvailCalendarInfo = true;
        }

        else if (objRateGridEventArgs.Operation == RateGridOperation.MovePrevious || objRateGridEventArgs.Operation == RateGridOperation.MoveNext)
        {
            bAsyncGetSearchHotelAvailCalendarInfo = true;
        }

        return;
    }

    protected void SearchHotelSelected(object sender, EventArgs e)
    {
        if (!this.IsPageError)
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

            if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
                objStayCriteriaSelection.HotelCode = ucSearchHotelSelectorControl.SelectedHotelCode;
            else
                objStayCriteriaSelection.HotelCode = ucSearchHotelSelectorGridControl.SelectedHotelCode;

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

        ucBookingStepControl.SelectedStep = "ChooseHotel";

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
        step2.Selected = true;
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

    private void LoadSearchHotelSelectorControl()
    {
        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
        {
            string strSearchHotelSelectorControlPath = ConfigurationManager.AppSettings["SearchHotelSelectorControl.ascx"];
            ucSearchHotelSelectorControl = (SearchHotelSelectorControl)LoadControl(strSearchHotelSelectorControlPath);

            phSearchHotelSelectorControl.Controls.Clear();
            phSearchHotelSelectorControl.Controls.Add(ucSearchHotelSelectorControl);

            ucSearchHotelSelectorControl.SearchHotelSelected += new SearchHotelSelectorControl.SearchHotelSelectedEvent(this.SearchHotelSelected);
        }

        else
        {
            string strSearchHotelSelectorGridControlPath = ConfigurationManager.AppSettings["SearchHotelSelectorGridControl.ascx"];
            ucSearchHotelSelectorGridControl = (SearchHotelSelectorGridControl)LoadControl(strSearchHotelSelectorGridControlPath);

            phSearchHotelSelectorControl.Controls.Clear();
            phSearchHotelSelectorControl.Controls.Add(ucSearchHotelSelectorGridControl);

            ucSearchHotelSelectorGridControl.SearchHotelSelected += new SearchHotelSelectorGridControl.SearchHotelSelectedEvent(this.SearchHotelSelected);
            ucSearchHotelSelectorGridControl.RateGridDateSelected += new SearchHotelSelectorGridControl.RateGridDateSelectedEvent(this.RateGridDateSelected);
        }

        return;
    }

    private void ConfigureSearchHotelSelectorControl()
    {
        HotelSearchRS objHotelSearchRS = (HotelSearchRS)Session["AreaListHotelSearchRS"];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["SearchHotelDescriptiveInfoRS"];
        HotelAvailabilityRS objSearchHotelAvailabilityCalendarRS = (HotelAvailabilityRS)Session["SearchHotelAvailabilityCalendarRS"];
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

        AreaListItem objAreaListItem = new AreaListItem();

        for (int i = 0; i < objHotelSearchRS.AreaListItems.Length; i++)
        {
            if (objHotelSearchRS.AreaListItems[i].AreaID == objStayCriteriaSelection.AreaID)
            {
                objAreaListItem = objHotelSearchRS.AreaListItems[i];
                break;
            }

        }

        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
        {
            string strSearchHotelSelectorItemControlPath = ConfigurationManager.AppSettings["SearchHotelSelectorItemControl.ascx"];

            ucSearchHotelSelectorControl.Clear();

            ucSearchHotelSelectorControl.ID = "SearchHotelSelectorControl";

            for (int hi = 0; hi < objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length; hi++)
            {
                AvailabilityCalendar objAvailabilityCalendar = null;

                for (int i = 0; i < objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos.Length; i++)
                {
                    if (objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos[i].HotelCode == objHotelDescriptiveInfoRS.HotelDescriptiveInfos[hi].HotelCode)
                    {
                        objAvailabilityCalendar = objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos[i].AvailabilityCalendar;
                        break;
                    }

                }

                SearchHotelSelectorItemControl ucSearchHotelSelectorItemControl = (SearchHotelSelectorItemControl)LoadControl(strSearchHotelSelectorItemControlPath);
                ucSearchHotelSelectorControl.Add(ucSearchHotelSelectorItemControl);

                ucSearchHotelSelectorItemControl.ID = "SearchHotelSelectorItem" + ((int)(hi + 1)).ToString();
                ucSearchHotelSelectorItemControl.HotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[hi];

                if (objAvailabilityCalendar != null)
                    ucSearchHotelSelectorItemControl.ShowSpecialRatesIndicator = objAvailabilityCalendar.RequestedRatesAvailable;
                else
                    ucSearchHotelSelectorItemControl.ShowSpecialRatesIndicator = false;
            }

        }

        else
        {
            DateTime dtRateGridStartDate = (DateTime)Session["SearchRateGridStartDate"];

            string strSearchHotelSelectorGridItemControlPath = ConfigurationManager.AppSettings["SearchHotelSelectorGridItemControl.ascx"];

            ucSearchHotelSelectorGridControl.Clear();

            ucSearchHotelSelectorGridControl.ID = "SearchHotelSelectorGridControl";
            ucSearchHotelSelectorGridControl.AreaListItem = objAreaListItem;
            ucSearchHotelSelectorGridControl.GridTodayDate = TZNet.ToLocal(WbsUiHelper.GetTimeZone(""), DateTime.UtcNow).Date;
            ucSearchHotelSelectorGridControl.GridStartDate = dtRateGridStartDate.Date;
            ucSearchHotelSelectorGridControl.GridNumberDays = this.NumberDaysInRateGrid;
            ucSearchHotelSelectorGridControl.GridSelectedStartDate = objStayCriteriaSelection.ArrivalDate;
            ucSearchHotelSelectorGridControl.GridSelectedEndDate = objStayCriteriaSelection.DepartureDate.AddDays(-1);

            for (int hi = 0; hi < objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length; hi++)
            {
                AvailabilityCalendar objAvailabilityCalendar = null;

                for (int i = 0; i < objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos.Length; i++)
                {
                    if (objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos[i].HotelCode == objHotelDescriptiveInfoRS.HotelDescriptiveInfos[hi].HotelCode)
                    {
                        objAvailabilityCalendar = objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos[i].AvailabilityCalendar;
                        break;
                    }

                }

                if (objAvailabilityCalendar != null)
                {
                    SearchHotelSelectorGridItemControl ucSearchHotelSelectorGridItemControl = (SearchHotelSelectorGridItemControl)LoadControl(strSearchHotelSelectorGridItemControlPath);
                    ucSearchHotelSelectorGridControl.Add(ucSearchHotelSelectorGridItemControl);

                    ucSearchHotelSelectorGridItemControl.ID = "SearchHotelSelectorGridItem" + ((int)(hi + 1)).ToString();
                    ucSearchHotelSelectorGridItemControl.GridStartDate = ucSearchHotelSelectorGridControl.GridStartDate;
                    ucSearchHotelSelectorGridItemControl.GridNumberDays = ucSearchHotelSelectorGridControl.GridNumberDays;
                    ucSearchHotelSelectorGridItemControl.GridHiLoRates = objAvailabilityCalendar.HiLoRates;
                    ucSearchHotelSelectorGridItemControl.HotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[hi];
                    ucSearchHotelSelectorGridItemControl.ShowSpecialRatesIndicator = objAvailabilityCalendar.RequestedRatesAvailable;
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
                ucTrackingCodeItemControl.PageUrl = ("/book/SelectHotel");
                ucTrackingCodeItemControl.Amount = 0;
            }

        }

        this.IsFirstTrackingPage = false;

        return;
    }

}
