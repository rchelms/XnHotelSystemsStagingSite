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

public partial class SelectAddOn : XnGR_WBS_Page
{
    private ProfileLoginNameControl ucProfileLoginNameControl;
    private LanguageSelectorControl ucLanguageSelectorControl;
    private BookingStepControl ucBookingStepControl;
    private ErrorDisplayControl ucErrorDisplayControl;
    private AddOnPackageSelectorControl ucAddOnPackageSelectorControl;
    private TrackingCodeControl ucHeadTrackingCodeControl;
    private TrackingCodeControl ucBodyTrackingCodeControl;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        this.LoadLanguageSelectorControl();
        this.LoadProfileLoginNameControl();
        this.LoadBookingStepControl();
        this.LoadErrorDisplayControl();
        this.LoadAddOnPackageSelectorControl();

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!IsPostBack)
        {
            if (this.IsPackageAvailable())
            {
                Session["SelectedRoom"] = "1";
            }

            else
            {
                WbsUiHelper.InitGuestDetailsEntryInfo();

                if (this.IsGuestDetailsTestPrefill)
                    WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

                Response.Redirect("~/Pages/EnterGuestDetails.aspx");
            }

        }

        else
        {
            this.ConfigureLanguageSelectorControl();
            this.ConfigureProfileLoginNameControl();
            this.ConfigureBookingStepControl();
            this.ConfigureErrorDisplayControl();
            this.ConfigureAddOnPackageSelectorControl();
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
        this.ConfigureAddOnPackageSelectorControl();
        this.ConfigureTrackingCodeControl();

        ucLanguageSelectorControl.RenderUserControl();
        ucProfileLoginNameControl.RenderUserControl();
        ucBookingStepControl.RenderUserControl();
        ucErrorDisplayControl.RenderUserControl();
        ucAddOnPackageSelectorControl.RenderUserControl();
        ucHeadTrackingCodeControl.RenderUserControl();
        ucBodyTrackingCodeControl.RenderUserControl();

        this.PageComplete();

        return;
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
        WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
        WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect("~/Pages/SelectAddOn.aspx");

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

    protected void RoomSelected(object sender, string selectedRoomRefID)
    {
        Session["SelectedRoom"] = selectedRoomRefID;

        List<AddOnPackageSelection> lAddOnPackageSelections = new List<AddOnPackageSelection>();

        for (int i = 0; i < ucAddOnPackageSelectorControl.AddOnPackageSelections.Length; i++)
            lAddOnPackageSelections.Add(ucAddOnPackageSelectorControl.AddOnPackageSelections[i]);

        AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];

        for (int i = 0; i < objAddOnPackageSelections.Length; i++)
        {
            if (objAddOnPackageSelections[i].RoomRefID != ucAddOnPackageSelectorControl.RoomRefID)
                lAddOnPackageSelections.Add(objAddOnPackageSelections[i]);
        }

        Session["AddOnPackageSelections"] = lAddOnPackageSelections.ToArray(); ;

        return;
    }

    protected void AddOnPackageCompleted(object sender, EventArgs e)
    {
        List<AddOnPackageSelection> lAddOnPackageSelections = new List<AddOnPackageSelection>();

        for (int i = 0; i < ucAddOnPackageSelectorControl.AddOnPackageSelections.Length; i++)
            lAddOnPackageSelections.Add(ucAddOnPackageSelectorControl.AddOnPackageSelections[i]);

        AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];

        for (int i = 0; i < objAddOnPackageSelections.Length; i++)
        {
            if (objAddOnPackageSelections[i].RoomRefID != ucAddOnPackageSelectorControl.RoomRefID)
                lAddOnPackageSelections.Add(objAddOnPackageSelections[i]);
        }

        Session["AddOnPackageSelections"] = lAddOnPackageSelections.ToArray(); ;

        WbsUiHelper.InitGuestDetailsEntryInfo();

        if (this.IsGuestDetailsTestPrefill)
            WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

        Response.Redirect("~/Pages/EnterGuestDetails.aspx");

        return;
    }

    private bool IsPackageAvailable()
    {
        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];

        bool bPackageAvailable = false;

        for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
        {
            for (int j = 0; j < objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans.Length; j++)
            {
                for (int k = 0; k < objRoomRateSelections.Length; k++)
                {
                    if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].Code == objRoomRateSelections[k].RatePlanCode)
                    {
                        for (int l = 0; l < objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].Packages.Length; l++)
                        {
                            if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].Packages[l].RoomTypeCode == "" || objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].Packages[l].RoomTypeCode == objRoomRateSelections[k].RoomTypeCode)
                            {
                                bPackageAvailable = true;
                                break;
                            }

                        }

                        if (bPackageAvailable)
                            break;
                    }

                }

                if (bPackageAvailable)
                    break;
            }

            if (bPackageAvailable)
                break;
        }

        return bPackageAvailable;
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

        ucBookingStepControl.SelectedStep = "ChooseYourExtras";

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
            step4.Selected = true;
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
            step2.Selected = false;
            step2.Clickable = true;

            BookingStepItemControl step3 = (BookingStepItemControl)LoadControl(strBookingStepItemControlPath);
            this.ucBookingStepControl.Add(step3);
            step3.ID = "ChooseYourExtras";
            step3.StepRefID = "ChooseYourExtras";
            step3.StepNumberText = (String)GetGlobalResourceObject("SiteResources", "BookingStep3");
            step3.StepDescriptionText = (String)GetGlobalResourceObject("SiteResources", "BookingStepChooseYourExtras");
            step3.Selected = true;
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

    private void LoadAddOnPackageSelectorControl()
    {
        string strAddOnPackageSelectorPath = ConfigurationManager.AppSettings["AddOnPackageSelectorControl.ascx"];
        ucAddOnPackageSelectorControl = (AddOnPackageSelectorControl)LoadControl(strAddOnPackageSelectorPath);

        phAddOnPackageControl.Controls.Clear();
        phAddOnPackageControl.Controls.Add(ucAddOnPackageSelectorControl);

        ucAddOnPackageSelectorControl.RoomSelected += new AddOnPackageSelectorControl.RoomSelectedEvent(this.RoomSelected);
        ucAddOnPackageSelectorControl.AddOnPackageCompleted += new AddOnPackageSelectorControl.AddOnPackageCompletedEvent(this.AddOnPackageCompleted);

        return;
    }

    private void ConfigureAddOnPackageSelectorControl()
    {
        string strSelectedRoom = (string)Session["SelectedRoom"];

        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
        AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];

        RoomOccupantSelection objRoomOccupantSelection = new RoomOccupantSelection();

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

        List<AddOnPackageSelection> lAddOnPackageSelections = new List<AddOnPackageSelection>();

        for (int i = 0; i < objAddOnPackageSelections.Length; i++)
        {
            if (objAddOnPackageSelections[i].RoomRefID == strSelectedRoom)
            {
                lAddOnPackageSelections.Add(objAddOnPackageSelections[i]);
            }

        }

        HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        int intNumberStayNights = ((TimeSpan)objStayCriteriaSelection.DepartureDate.Subtract(objStayCriteriaSelection.ArrivalDate)).Days;

        string strRoomSelectorItemControlPath = ConfigurationManager.AppSettings["RoomSelectorItemControl.ascx"];
        string strAddOnPackageSelectorItemControlPath = ConfigurationManager.AppSettings["AddOnPackageSelectorItemControl.ascx"];

        ucAddOnPackageSelectorControl.Clear();

        ucAddOnPackageSelectorControl.ID = "AddOnPackageSelector";
        ucAddOnPackageSelectorControl.RoomRefID = strSelectedRoom;
        ucAddOnPackageSelectorControl.RoomOccupantSelection = objRoomOccupantSelection;

        for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
        {
            RoomSelectorItemControl ucRoomSelectorItemControl = (RoomSelectorItemControl)LoadControl(strRoomSelectorItemControlPath);
            ucAddOnPackageSelectorControl.AddRoomSelectorItem(ucRoomSelectorItemControl);

            ucRoomSelectorItemControl.ID = "RoomSelectorItem" + objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
            ucRoomSelectorItemControl.RoomRefID = objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
            ucRoomSelectorItemControl.RoomRefIDMenuText = (String)GetGlobalResourceObject("SiteResources", "RoomSelectorMenuText") + " " + objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;

            if (objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID == strSelectedRoom)
                ucRoomSelectorItemControl.Selected = true;
            else
                ucRoomSelectorItemControl.Selected = false;
        }

        for (int i = 0; i < objHotelRoomAvailInfo.RatePlans.Length; i++)
        {
            if (objHotelRoomAvailInfo.RatePlans[i].Code == objRoomRateSelection.RatePlanCode)
            {
                for (int j = 0; j < objHotelRoomAvailInfo.RatePlans[i].Packages.Length; j++)
                {
                    if (objHotelRoomAvailInfo.RatePlans[i].Packages[j].RoomTypeCode == "" || objHotelRoomAvailInfo.RatePlans[i].Packages[j].RoomTypeCode == objRoomRateSelection.RoomTypeCode)
                    {
                        AddOnPackageSelectorItemControl ucAddOnPackageSelectorItemControl = (AddOnPackageSelectorItemControl)LoadControl(strAddOnPackageSelectorItemControlPath);
                        ucAddOnPackageSelectorControl.AddAddOnPackageSelectorItem(ucAddOnPackageSelectorItemControl);

                        ucAddOnPackageSelectorItemControl.ID = "AddOnPackageSelectorItemControl" + ((int)(j + 1)).ToString();
                        ucAddOnPackageSelectorItemControl.RoomRefID = strSelectedRoom;
                        ucAddOnPackageSelectorItemControl.NumberAdults = objRoomOccupantSelection.NumberAdults;
                        ucAddOnPackageSelectorItemControl.NumberStayNights = intNumberStayNights;

                        for (int k = 0; k < objHotelDescriptiveInfo.Packages.Length; k++)
                        {
                            if (objHotelDescriptiveInfo.Packages[k].Code == objHotelRoomAvailInfo.RatePlans[i].Packages[j].Code)
                            {
                                ucAddOnPackageSelectorItemControl.PackageDescription = objHotelDescriptiveInfo.Packages[k];
                                break;
                            }

                        }

                        ucAddOnPackageSelectorItemControl.PackageRate = objHotelRoomAvailInfo.RatePlans[i].Packages[j];

                        ucAddOnPackageSelectorItemControl.PackageQuantity = 0;
                        ucAddOnPackageSelectorItemControl.Selected = false;

                        for (int k = 0; k < objAddOnPackageSelections.Length; k++)
                        {
                            if (objAddOnPackageSelections[k].RoomRefID == strSelectedRoom && objAddOnPackageSelections[k].PackageCode == objHotelRoomAvailInfo.RatePlans[i].Packages[j].Code)
                            {
                                ucAddOnPackageSelectorItemControl.PackageQuantity = objAddOnPackageSelections[k].Quantity;
                                ucAddOnPackageSelectorItemControl.Selected = true;
                                break;
                            }

                        }

                    }

                }

                break;
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
                ucTrackingCodeItemControl.PageUrl = ("/book/SelectExtras");
                ucTrackingCodeItemControl.Amount = 0;
            }

        }

        this.IsFirstTrackingPage = false;

        return;
    }

}
