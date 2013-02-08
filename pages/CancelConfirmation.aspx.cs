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

public partial class CancelConfirmation : XnGR_WBS_Page
{
    private ProfileLoginNameControl ucProfileLoginNameControl;
    private LanguageSelectorControl ucLanguageSelectorControl;
    private BookingStepControl ucBookingStepControl;
    private ErrorDisplayControl ucErrorDisplayControl;
    private CancelBookingSummaryControl ucCancelBookingSummaryControl;
    private TrackingCodeControl ucHeadTrackingCodeControl;
    private TrackingCodeControl ucBodyTrackingCodeControl;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        this.LoadLanguageSelectorControl();
        this.LoadProfileLoginNameControl();
        this.LoadBookingStepControl();
        this.LoadErrorDisplayControl();
        this.LoadCancelBookingSummaryControl();

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
            this.ConfigureCancelBookingSummaryControl();
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
        this.ConfigureCancelBookingSummaryControl();
        this.ConfigureTrackingCodeControl();

        ucLanguageSelectorControl.RenderUserControl();
        ucProfileLoginNameControl.RenderUserControl();
        ucBookingStepControl.RenderUserControl();
        ucErrorDisplayControl.RenderUserControl();
        ucCancelBookingSummaryControl.RenderUserControl();
        ucHeadTrackingCodeControl.RenderUserControl();
        ucBodyTrackingCodeControl.RenderUserControl();

        this.PageComplete();

        return;
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
        WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
        WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect("~/Pages/CancelConfirmation.aspx");

        return;
    }

    protected void BookingStepSelected(object sender, EventArgs e)
    {
        if (((BookingStepControl)sender).SelectedStep == "LocateReservation")
        {
            Response.Redirect("~/Pages/CancelReservation.aspx");
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

        BookingStepItemControl step1 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
        this.ucBookingStepControl.Add(step1);
        step1.ID = "LocateReservation";
        step1.StepRefID = "LocateReservation";
        step1.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "CancelStep1");
        step1.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "CancelStepLocateReservation");
        step1.Selected = false;
        step1.Clickable = true;

        BookingStepItemControl step2 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
        this.ucBookingStepControl.Add(step2);
        step2.ID = "IdentifyRooms";
        step2.StepRefID = "IdentifyRooms";
        step2.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "CancelStep2");
        step2.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "CancelStepIdentifyRooms");
        step2.Selected = false;
        step2.Clickable = false;

        BookingStepItemControl step3 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
        this.ucBookingStepControl.Add(step3);
        step3.ID = "Confirmation";
        step3.StepRefID = "Confirmation";
        step3.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "CancelStep3");
        step3.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "CancelStepConfirmation");
        step3.Selected = true;
        step3.Clickable = false;

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

    private void LoadCancelBookingSummaryControl()
    {
        string strCancelBookingSummaryControlPath = ConfigurationManager.AppSettings["CancelBookingSummaryControl.ascx"];
        ucCancelBookingSummaryControl = (CancelBookingSummaryControl)LoadControl(strCancelBookingSummaryControlPath);

        phCancelBookingSummaryControl.Controls.Clear();
        phCancelBookingSummaryControl.Controls.Add(ucCancelBookingSummaryControl);

        return;
    }

    private void ConfigureCancelBookingSummaryControl()
    {
        HotelBookingRS objHotelBookingRS = (HotelBookingRS)Session["HotelBookingRS"];

        List<CancelledBookingInfo> lCancelledBookingInfo = new List<CancelledBookingInfo>();

        for (int i = 0; i < objHotelBookingRS.Segments.Length; i++)
        {
            CancelledBookingInfo objCancelledBookingInfo = new CancelledBookingInfo();
            lCancelledBookingInfo.Add(objCancelledBookingInfo);

            objCancelledBookingInfo.ConfirmationNumber = objHotelBookingRS.Segments[i].ConfirmationNumber;
            objCancelledBookingInfo.CancellationNumber = objHotelBookingRS.Segments[i].CancellationNumber;
        }

        ucCancelBookingSummaryControl.ID = "CancelBookingSummaryControl";
        ucCancelBookingSummaryControl.CancelledBookings = lCancelledBookingInfo.ToArray();

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
                ucTrackingCodeItemControl.PageUrl = ("/cancel/CancelConfirmation");
                ucTrackingCodeItemControl.Amount = 0;
            }

        }

        this.IsFirstTrackingPage = false;

        return;
    }

}
