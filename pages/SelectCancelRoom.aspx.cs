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
using MamaShelter;
using System.Threading;

public partial class SelectCancelRoom : XnGR_WBS_Page
{
   private const string SessionStayCriteriaSelection = "StayCriteriaSelection";

   private static string strConfirmationControlPath = ConfigurationManager.AppSettings["ConfirmationControl.ascx"];
   private static string strImageHoldingControlPath = ConfigurationManager.AppSettings["ImageHoldingControl.ascx"];
   private static string strStayCriteriaSelectorControlPath = ConfigurationManager.AppSettings["StayCriteriaSelectorControl.MM.ascx"];
   private static string strAvailCalSelectorControlPath = ConfigurationManager.AppSettings["AvailCalSelectorControl.ascx"];
   private static string strRoomQuantitySelectorControlPath = ConfigurationManager.AppSettings["RoomQuantitySelectorControl.ascx"];

   private LanguageSelectorControl ucLanguageSelectorControl;
   private ErrorDisplayControl ucErrorDisplayControl;
   private ConfirmationControl ucConfirmationControl;
   private StayCriteriaSelectorControl_MM ucStayCriteriaSelectorControl;
   private AvailCalSelectorControl ucAvailCalSelectorControl;
   private RoomQuantitySelectorControl ucRoomQuantitySelectorControl;
   private CancelRoomSelectorControl ucCancelRoomSelectorControl;
   private TrackingCodeControl ucBodyTrackingCodeControl;
   private RemoteContentContainer ucRemoteContentContainerControl;

   private StayCriteriaSelection _stayCriteriaSelection;
   public StayCriteriaSelection StayCriteriaSelection
   {
      get
      {
         if (_stayCriteriaSelection == null)
            _stayCriteriaSelection = (StayCriteriaSelection)Session[SessionStayCriteriaSelection];

         return _stayCriteriaSelection;
      }
      set
      {
         Session[SessionStayCriteriaSelection] = _stayCriteriaSelection = value;
      }
   }

   private CancelDetailsEntryInfo _CancelDetailsEntryInfo;
   private CancelDetailsEntryInfo CancelDetailsEntryInfo
   {
      get
      {
         if (_CancelDetailsEntryInfo == null)
            _CancelDetailsEntryInfo = (CancelDetailsEntryInfo)Session["CancelDetailsEntryInfo"];
         return _CancelDetailsEntryInfo;
      }

   }

   private HotelBookingReadSegment[] _HotelBookingReadSegments;
   public HotelBookingReadSegment[] HotelBookingReadSegments
   {
      get
      {
         if (_HotelBookingReadSegments == null)
         {
            _HotelBookingReadSegments = WbsUiHelper.GetValidatedBookings(CancelDetailsEntryInfo);
         }
         return _HotelBookingReadSegments;
      }
   }

   bool bAsyncCancelBooking = false;
   bool bIsCancelled = false;

   #region PAGE EVENTS
   protected override void Page_Init(object sender, EventArgs e)
   {
      base.Page_Init(sender, e);

      //this.LoadLanguageSelectorControl();
      this.LoadRemoteContentContainerControl();
      this.LoadConfirmationControl();
      this.LoadErrorDisplayControl();
      this.LoadStaySelectorControl();
      this.LoadAvailCalSelectorControl();
      this.LoadRoomQuantitySelectorControl();
      this.LoadCancelRoomSelectorControl();

      panBookingCancelled.Visible = false;
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
         this.ConfigureRemoteContentContainerControl();
         this.ConfigureConfirmationControl();
         //this.ConfigureLanguageSelectorControl();
         this.ConfigureErrorDisplayControl();
         this.ConfigureStaySelectorControl();
         this.ConfigureAvailCalSelectorControl();
         this.ConfigureRoomQuantitySelectorControl();
         this.ConfigureCancelRoomSelectorControl();
      }

      PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
      RegisterAsyncTask(task);

      InitStayCriteriaSeletion();
      return;
   }

   protected void Page_PreRenderComplete(object sender, EventArgs e)
   {
      this.IsParentPreRender = true;

      this.ConfigureRemoteContentContainerControl();
      //this.ConfigureLanguageSelectorControl();
      this.ConfigureErrorDisplayControl();
      this.ConfigureConfirmationControl();
      this.ConfigureStaySelectorControl();
      this.ConfigureAvailCalSelectorControl();
      this.ConfigureRoomQuantitySelectorControl();
      this.ConfigureCancelRoomSelectorControl();
      this.ConfigureTrackingCodeControl();

      ucRemoteContentContainerControl.RenderUserControl();
      //ucLanguageSelectorControl.RenderUserControl();
      ucErrorDisplayControl.RenderUserControl();
      ucConfirmationControl.RenderUserControl();
      ucStayCriteriaSelectorControl.RenderUserControl();
      ucAvailCalSelectorControl.RenderUserControl();
      ucRoomQuantitySelectorControl.RenderUserControl();
      ucCancelRoomSelectorControl.RenderUserControl();
      //ucHeadTrackingCodeControl.RenderUserControl();
      //ucBodyTrackingCodeControl.RenderUserControl();

      this.PageComplete();

      return;
   }
   #endregion

   #region Async task related
   public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
   {
      wbsIISAsyncResult = new WBSAsyncResult(cb, state);

      if (bAsyncCancelBooking)
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
      if (bAsyncCancelBooking)
      {
         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitBookHotelRQ(ref wbsAPIRouterData, BookingAction.Cancel);
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(CancelBookHotelComplete), null, true);
      }

      else
      {
         // End async page operation

         if (!wbsIISAsyncResult.IsCompleted)
            wbsIISAsyncResult.SetComplete();
      }

      return;
   }

   private void CancelBookHotelComplete(IAsyncResult ar)
   {
      if (this.WbsApiRouterHelper.ProcessBookHotelRS(ref wbsAPIRouterData))
      {
         bAsyncCancelBooking = false;
         bIsCancelled = true;

         HotelBookingRS objHotelBookingRS = (HotelBookingRS)Session["HotelBookingRS"];
         string[] cancelNumbers = new string[objHotelBookingRS.Segments.Length];
         for (int i = 0; i < objHotelBookingRS.Segments.Length; i++)
         {
            cancelNumbers[i] = objHotelBookingRS.Segments[i].CancellationNumber;
         }
         panBookingCancelled.Visible = true;
         lblCancelConfirmNumber.Text = string.Join(",", cancelNumbers);
         phRemoteContentContainerControl.Controls.Clear();

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
   #endregion

   private void InitStayCriteriaSeletion()
   {
      StayCriteriaSelection.HotelCode = CancelDetailsEntryInfo.HotelCode;
      StayCriteriaSelection.ArrivalDate = HotelBookingReadSegments[0].ArrivalDate;
      StayCriteriaSelection.DepartureDate = HotelBookingReadSegments[0].DepartureDate;
   }

   public void LanguageSelected(object sender, EventArgs e)
   {
      WbsUiHelper.SelectedCulture = ((LanguageSelectorControl)sender).SelectedCulture;
      WbsUiHelper.SelectedUICulture = ((LanguageSelectorControl)sender).SelectedUICulture;

      Response.Redirect("~/Pages/SelectCancelRoom.aspx");

      return;
   }

   protected void CancelRoomCompleted(object sender, EventArgs e)
   {
      this.ValidateCancelRoomSelections(ucCancelRoomSelectorControl.ConfirmationNumberSelections);

      CancelDetailsEntryInfo.SelectedConfirmationNumbersToCancel = ucCancelRoomSelectorControl.ConfirmationNumberSelections;
      Session["CancelDetailsEntryInfo"] = CancelDetailsEntryInfo;

      if (!this.IsPageError)
      {
         //Response.Redirect("~/Pages/CancelRoom.aspx");
         bAsyncCancelBooking = true;
         PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
         RegisterAsyncTask(task);
      }

      return;

   }

   private void ValidateCancelRoomSelections(string[] strConfirmationNumberSelections)
   {
      if (strConfirmationNumberSelections.Length == 0)
         this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoRoomsSelectedToCancel"));

      return;
   }

   #region LOAD CONTROLS
   private void LoadErrorDisplayControl()
   {
      string strErrorDisplayControlPath = ConfigurationManager.AppSettings["ErrorDisplayControl.ascx"];
      ucErrorDisplayControl = (ErrorDisplayControl)LoadControl(strErrorDisplayControlPath);

      phErrorDisplayControl.Controls.Clear();
      phErrorDisplayControl.Controls.Add(ucErrorDisplayControl);

      return;
   }

   private void LoadRemoteContentContainerControl()
   {
      ucRemoteContentContainerControl = (RemoteContentContainer)LoadControl(ConfigurationManager.AppSettings["RemoteContentContainer.ascx"]);
      ucRemoteContentContainerControl.ID = "RemoteContentContainer";

      phRemoteContentContainerControl.Controls.Clear();
      phRemoteContentContainerControl.Controls.Add(ucRemoteContentContainerControl);
      phRemoteContentContainerControl.Visible = true;
   }

   private void LoadConfirmationControl()
   {
      ucConfirmationControl = (ConfirmationControl)LoadControl(strConfirmationControlPath);
      ucConfirmationControl.ID = "ConfirmationControl";
      phConfirmationControl.Controls.Clear();
      phConfirmationControl.Controls.Add(ucConfirmationControl);
   }

   private void LoadStaySelectorControl()
   {
      ucStayCriteriaSelectorControl = (StayCriteriaSelectorControl_MM)LoadControl(strStayCriteriaSelectorControlPath);
      ucStayCriteriaSelectorControl.ID = "StayCriteriaSelectorControl";

      phStayCriteriaSelectorControl.Controls.Clear();
      phStayCriteriaSelectorControl.Controls.Add(ucStayCriteriaSelectorControl);

      return;
   }

   private void LoadAvailCalSelectorControl()
   {
      ucAvailCalSelectorControl = (AvailCalSelectorControl)LoadControl(strAvailCalSelectorControlPath);
      ucAvailCalSelectorControl.ID = "AvailCalSelectorControl";

      phAvailCalSelectorControl.Controls.Clear();
      phAvailCalSelectorControl.Controls.Add(ucAvailCalSelectorControl);
   }

   private void LoadRoomQuantitySelectorControl()
   {
      ucRoomQuantitySelectorControl = (RoomQuantitySelectorControl)LoadControl(strRoomQuantitySelectorControlPath);
      ucRoomQuantitySelectorControl.ID = "RoomQuantitySelectorControl";
      ucRoomQuantitySelectorControl.MaxNumberOfRoomAvailable = int.Parse(ConfigurationManager.AppSettings["StayCriteriaSelectorControl.MaxRooms"]);
      phRoomQuantitySelectorControl.Controls.Clear();
      phRoomQuantitySelectorControl.Controls.Add(ucRoomQuantitySelectorControl);

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

   private void LoadCancelRoomSelectorControl()
   {
      string strCancelRoomSelectorControlPath = ConfigurationManager.AppSettings["CancelRoomSelectorControl.ascx"];
      ucCancelRoomSelectorControl = (CancelRoomSelectorControl)LoadControl(strCancelRoomSelectorControlPath);

      phCancelRoomSelectorControl.Controls.Clear();
      phCancelRoomSelectorControl.Controls.Add(ucCancelRoomSelectorControl);

      ucCancelRoomSelectorControl.CancelRoomCompleted += new CancelRoomSelectorControl.CancelRoomCompletedEvent(this.CancelRoomCompleted);

      return;
   }
   #endregion

   #region CONFIGURE CONTROLS
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

   private void ConfigureRemoteContentContainerControl()
   {
      string customCDNPath = ConfigurationManager.AppSettings[Constants.WebConfigKeys.CustomCDNPath];
      customCDNPath = customCDNPath.EndsWith("/") ? customCDNPath : customCDNPath + "/";
      ucRemoteContentContainerControl.Src = customCDNPath + "booking/cancel-confirmation/";
   }

   private void ConfigureConfirmationControl()
   {
      ucConfirmationControl.ConfirmationNumber = CancelDetailsEntryInfo.ConfirmationNumber;
   }

   private void ConfigureStaySelectorControl()
   {
      HotelSearchRS objPropertyListHotelSearchRS = (HotelSearchRS)Session["PropertyListHotelSearchRS"];
      HotelListItem[] objHotelListItems = objPropertyListHotelSearchRS.HotelListItems;

      if (objHotelListItems == null)
         return;

      ucStayCriteriaSelectorControl.ID = "StayCriteriaSelectorControl";
      ucStayCriteriaSelectorControl.HotelListItems = objHotelListItems;
      ucStayCriteriaSelectorControl.StayCriteriaSelection = StayCriteriaSelection;
      ucStayCriteriaSelectorControl.StayCriteriaSelectorMode = SelectionMode.Cancellation;

      return;
   }

   private void ConfigureAvailCalSelectorControl()
   {
      ucAvailCalSelectorControl.Mode = SelectionMode.Cancellation;
      //ucAvailCalSelectorControl.AvailabilityCalendarInfo = objAvailabilityCalendarInfos;
      //ucAvailCalSelectorControl.SelectedDates = objAvailCalDateSelections;
      ucAvailCalSelectorControl.StayCriteriaSelection = StayCriteriaSelection;
      ucAvailCalSelectorControl.Today = DateTime.Now.Date;
   }

   private void ConfigureRoomQuantitySelectorControl()
   {
      ucRoomQuantitySelectorControl.StayCriteriaSelection = StayCriteriaSelection;
      ucRoomQuantitySelectorControl.Mode = SelectionMode.Cancellation;
   }

   private void ConfigureErrorDisplayControl()
   {
      ucErrorDisplayControl.ErrorInfos = this.PageErrors;

      return;
   }

   private void ConfigureCancelRoomSelectorControl()
   {
      HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

      HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

      if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
      {
         objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
      }

      string strCancelRoomSelectorItemControlPath = ConfigurationManager.AppSettings["CancelRoomSelectorItemControl.ascx"];
      string strCancelAddOnPackageItemControlPath = ConfigurationManager.AppSettings["CancelAddOnPackageItemControl.ascx"];

      ucCancelRoomSelectorControl.Clear();

      ucCancelRoomSelectorControl.ID = "CancelRoomSelectorControl";
      ucCancelRoomSelectorControl.HotelDescriptiveInfo = objHotelDescriptiveInfo;
      ucCancelRoomSelectorControl.BookingReadSegments = HotelBookingReadSegments;
      ucCancelRoomSelectorControl.Mode = bIsCancelled ? SelectionMode.NonModifiable : SelectionMode.Edit;


      for (int si = 0; si < HotelBookingReadSegments.Length; si++)
      {
         int intNumberStayNights = ((TimeSpan)HotelBookingReadSegments[si].DepartureDate.Subtract(HotelBookingReadSegments[si].ArrivalDate)).Days;

         RoomOccupantSelection objRoomOccupantSelection = new RoomOccupantSelection();

         objRoomOccupantSelection.RoomRefID = ((int)(si + 1)).ToString();
         objRoomOccupantSelection.NumberRooms = HotelBookingReadSegments[si].NumRooms;
         objRoomOccupantSelection.NumberAdults = HotelBookingReadSegments[si].NumAdults;
         objRoomOccupantSelection.NumberChildren = HotelBookingReadSegments[si].NumChildren;

         CancelRoomSelectorItemControl ucCancelRoomSelectorItemControl = (CancelRoomSelectorItemControl)LoadControl(strCancelRoomSelectorItemControlPath);
         ucCancelRoomSelectorControl.AddCancelRoomSelectorItem(ucCancelRoomSelectorItemControl);

         ucCancelRoomSelectorItemControl.Clear();

         ucCancelRoomSelectorItemControl.ID = "CancelRoomSelectorItem" + ((int)(si + 1)).ToString();
         ucCancelRoomSelectorItemControl.RoomRefID = ((int)(si + 1)).ToString();
         ucCancelRoomSelectorItemControl.RoomOccupantSelection = objRoomOccupantSelection;
         ucCancelRoomSelectorItemControl.RoomType = HotelBookingReadSegments[si].RoomType;
         ucCancelRoomSelectorItemControl.RatePlan = HotelBookingReadSegments[si].RatePlan;
         ucCancelRoomSelectorItemControl.Rates = HotelBookingReadSegments[si].Rates;
         ucCancelRoomSelectorItemControl.CancelPolicy = HotelBookingReadSegments[si].CancelPolicy;
         ucCancelRoomSelectorItemControl.HotelCode = HotelBookingReadSegments[si].HotelCode;
         ucCancelRoomSelectorItemControl.ArrivalDate = HotelBookingReadSegments[si].ArrivalDate.Date;
         ucCancelRoomSelectorItemControl.ConfirmationNumber = HotelBookingReadSegments[si].ConfirmationNumber;
         ucCancelRoomSelectorItemControl.Selected = false;
         ucCancelRoomSelectorItemControl.SelectionMode = bIsCancelled ? SelectionMode.NonModifiable : SelectionMode.Edit;

         for (int i = 0; i < CancelDetailsEntryInfo.SelectedConfirmationNumbersToCancel.Length; i++)
         {
            if (HotelBookingReadSegments[si].ConfirmationNumber == CancelDetailsEntryInfo.SelectedConfirmationNumbersToCancel[i])
               ucCancelRoomSelectorItemControl.Selected = true;
         }

         for (int pi = 0; pi < HotelBookingReadSegments[si].PackageRates.Length; pi++)
         {
            CancelAddOnPackageItemControl ucCancelAddOnPackageItemControl = (CancelAddOnPackageItemControl)LoadControl(strCancelAddOnPackageItemControlPath);
            ucCancelRoomSelectorItemControl.AddCancelAddOnPackageItem(ucCancelAddOnPackageItemControl);

            ucCancelAddOnPackageItemControl.ID = "CancelAddOnPackageItem" + ((int)(pi + 1)).ToString();
            ucCancelAddOnPackageItemControl.RoomRefID = ((int)(si + 1)).ToString();
            ucCancelAddOnPackageItemControl.NumberStayNights = intNumberStayNights;
            ucCancelAddOnPackageItemControl.PackageQuantity = HotelBookingReadSegments[si].PackageRates[pi].Quantity;
            ucCancelAddOnPackageItemControl.PackageRate = HotelBookingReadSegments[si].PackageRates[pi];
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
      ucBodyTrackingCodeControl = (TrackingCodeControl)LoadControl(strTrackingCodeControlPath);

      phTrackingCodeControl.Controls.Clear();
      phTrackingCodeControl.Controls.Add(ucBodyTrackingCodeControl);

      ucBodyTrackingCodeControl.Clear();

      string strTrackingCodeItemControlPath = ConfigurationManager.AppSettings["TrackingCodeItemControl.ascx"];

      for (int i = 0; i < objTrackingCodeInfos.Length; i++)
      {
         if (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.AllPages || (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.StartPageOnly && this.IsFirstTrackingPage))
         {
            TrackingCodeItemControl ucTrackingCodeItemControl = (TrackingCodeItemControl)LoadControl(strTrackingCodeItemControlPath);

            ucBodyTrackingCodeControl.Add(ucTrackingCodeItemControl);

            ucTrackingCodeItemControl.Type = objTrackingCodeInfos[i].Type;
            ucTrackingCodeItemControl.TrackingCodeParameters = objTrackingCodeInfos[i].TrackingCodeParameters;
            ucTrackingCodeItemControl.HotelCode = objStayCriteriaSelection.HotelCode;
            ucTrackingCodeItemControl.PageUrl = "/" + WbsUiHelper.SelectedUICulture + ("/cancel/SelectCancelRoom");
            ucTrackingCodeItemControl.Amount = 0;
         }

      }

      this.IsFirstTrackingPage = false;

      return;
   }
   #endregion
}
