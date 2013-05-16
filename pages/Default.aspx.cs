using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using XHS.Logging;
using XHS.WBSUIBizObjects;
using System.Configuration;
using System.Threading;
using MamaShelter;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class MamaShelterBooking : XnGR_WBS_Page
{
   #region CONSTANTS

   private static readonly string strErrorDisplayControlPath = ConfigurationManager.AppSettings["ErrorDisplayControl.ascx"];
   private static readonly string strImageHoldingControlPath = ConfigurationManager.AppSettings["ImageHoldingControl.ascx"];
   private static readonly string strRemoteContentContainerControlPath = ConfigurationManager.AppSettings["RemoteContentContainer.ascx"];
   private static readonly string strStayCriteriaSelectorControlPath = ConfigurationManager.AppSettings["StayCriteriaSelectorControl.MM.ascx"];
   private static readonly string strAvailCalSelectorControlPath = ConfigurationManager.AppSettings["AvailCalSelectorControl.ascx"];
   private static readonly string strPeopleQuanitySelectorControlPath = ConfigurationManager.AppSettings["PeopleQuantitySelectorControl.ascx"];
   private static readonly string strRatePlanSelectorItemControlPath = ConfigurationManager.AppSettings["RatePlanSelectorItemControl.ascx"];
   private static readonly string strAddOnPackageSelectorItemControlPath = ConfigurationManager.AppSettings["AddOnPackageSelectorItemControl.ascx"];
   private static readonly string strRoomDetailSelectorControlPath = ConfigurationManager.AppSettings["RoomDetailSelectorControl.ascx"];
   private static readonly string strRoomQuantitySelectorControlPath = ConfigurationManager.AppSettings["RoomQuantitySelectorControl.ascx"];
   private static readonly string strTotalCostControlPath = ConfigurationManager.AppSettings["TotalCostControl.ascx"];
   private static readonly string strGuestDetailsEntryControlPath = ConfigurationManager.AppSettings["GuestDetailsEntryControl.ascx"];
   private static readonly string strConfirmationControlPath = ConfigurationManager.AppSettings["ConfirmationControl.ascx"];
   private static readonly string strTrackingCodeControlPath = ConfigurationManager.AppSettings["TrackingCodeControl.ascx"];

   #endregion

   #region SESSION Properties

   BookingSteps Step
   {
      get
      {
         if (Session[Constants.Sessions.CurrentBookingStep] == null)
            Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectHotel;
         return (BookingSteps)Session[Constants.Sessions.CurrentBookingStep];

      }
      set
      {
         Session[Constants.Sessions.CurrentBookingStep] = value;
      }
   }

   private StayCriteriaSelection StayCriteriaSelection
   {
      get
      {
         return (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
      }
      set
      {
         Session[Constants.Sessions.StayCriteriaSelection] = value;
      }
   }

   private HotelAvailabilityRS HotelAvailabilityRS
   {
      get
      {
         return (HotelAvailabilityRS)Session[Constants.Sessions.HotelAvailabilityRS];
      }
   }

   private HotelDescriptiveInfoRS HotelDescriptiveInfoRS
   {
      get
      {
         if (Session[Constants.Sessions.HotelDescriptiveInfoRS] != null)
            return (HotelDescriptiveInfoRS)Session[Constants.Sessions.HotelDescriptiveInfoRS];
         return null;
      }
   }

   private AddOnPackageSelection[] AddOnPackageSelections
   {
      get
      {
         return (AddOnPackageSelection[])Session[Constants.Sessions.AddOnPackageSelections];
      }
      set
      {
         Session[Constants.Sessions.AddOnPackageSelections] = value;
      }
   }

   private DateTime _AvailCalStartDate = DateTime.MinValue;
   private DateTime AvailCalStartDate
   {
      get
      {
         var arrivalDate = StayCriteriaSelection.ArrivalDate;

         if (_AvailCalStartDate == DateTime.MinValue)
         {
            if (Session[Constants.Sessions.AvailCalStartDate] == null || ((DateTime)Session[Constants.Sessions.AvailCalStartDate]).Equals(DateTime.MinValue))
               Session[Constants.Sessions.AvailCalStartDate] = new DateTime(arrivalDate.Year, arrivalDate.Month, 1);

            _AvailCalStartDate = (DateTime)Session[Constants.Sessions.AvailCalStartDate];
         }
         return _AvailCalStartDate;
      }
      set
      {
         _AvailCalStartDate = value;
         Session[Constants.Sessions.AvailCalStartDate] = value;
      }
   }

   RoomDetailSelectionStep _CurrentRoomDetailStep = RoomDetailSelectionStep.Unknown;
   private RoomDetailSelectionStep CurrentRoomDetailStep
   {
      get
      {
         if (_CurrentRoomDetailStep == RoomDetailSelectionStep.Unknown)
         {
            if (Session[Constants.Sessions.CurrentRoomDetailStep] == null)
               Session[Constants.Sessions.CurrentRoomDetailStep] = RoomDetailSelectionStep.SelectAdultQuantity;
            _CurrentRoomDetailStep = (RoomDetailSelectionStep)Session[Constants.Sessions.CurrentRoomDetailStep];
         }

         return _CurrentRoomDetailStep;
      }
      set
      {
         _CurrentRoomDetailStep = value;
         Session[Constants.Sessions.CurrentRoomDetailStep] = value;
      }
   }

   string _CurrentRoomRefID;
   private string CurrentRoomRefID
   {
      get
      {
         if (string.IsNullOrWhiteSpace(_CurrentRoomRefID))
         {
            if (string.IsNullOrWhiteSpace((string)Session[Constants.Sessions.CurrentRoomRefID]))
               Session[Constants.Sessions.CurrentRoomRefID] = RoomRateSelections[0].RoomRefID;
            _CurrentRoomRefID = (string)Session[Constants.Sessions.CurrentRoomRefID];
         }

         return _CurrentRoomRefID;
      }
      set
      {
         _CurrentRoomRefID = value;
         Session[Constants.Sessions.CurrentRoomRefID] = value;
      }
   }

   private RoomRateSelection[] RoomRateSelections
   {
      get
      {
         return (RoomRateSelection[])Session[Constants.Sessions.RoomRateSelections];
      }
      set
      {
         Session[Constants.Sessions.RoomRateSelections] = value;
      }
   }

   private PaymentGatewayInfo[] PaymentGatewayInfos
   {
      get
      {
         if (Session[Constants.Sessions.PaymentGatewayInfos] == null || ((PaymentGatewayInfo[])Session[Constants.Sessions.PaymentGatewayInfos]).Length <= 0)
            Session[Constants.Sessions.PaymentGatewayInfos] = WBSPGHelper.GetPaymentGatewayInfos(StayCriteriaSelection.HotelCode);

         return (PaymentGatewayInfo[])Session[Constants.Sessions.PaymentGatewayInfos];
      }
      set
      {
         Session[Constants.Sessions.PaymentGatewayInfos] = value;
      }
   }

   private PaymentGatewayInfo PaymentGatewayInfo
   {
      get { return (PaymentGatewayInfo)Session[Constants.Sessions.PaymentGatewayInfo]; }
      set
      {
         Session[Constants.Sessions.PaymentGatewayInfo] = value;
      }
   }

   private HotelBookingPaymentAllocation[] HotelBookingPaymentAllocations
   {
      get
      {
         if (Session[Constants.Sessions.HotelBookingPaymentAllocations] == null || ((HotelBookingPaymentAllocation[])(Session[Constants.Sessions.HotelBookingPaymentAllocations])).Length <= 0)
            Session[Constants.Sessions.HotelBookingPaymentAllocations] = WBSPGHelper.GetPaymentAllocations(HotelPricingHelper.GetHotelPricing(StayCriteriaSelection, RoomRateSelections, AddOnPackageSelections, HotelAvailabilityRS.HotelRoomAvailInfos, CurrencyCode));

         return (HotelBookingPaymentAllocation[])Session[Constants.Sessions.HotelBookingPaymentAllocations];
      }
      set
      {
         Session[Constants.Sessions.HotelBookingPaymentAllocations] = value;
      }
   }

   private GuestDetailsEntryInfo GuestDetailsEntryInfo
   {
      get
      {
         return (GuestDetailsEntryInfo)Session[Constants.Sessions.GuestDetailsEntryInfo];
      }
      set
      {
         Session[Constants.Sessions.GuestDetailsEntryInfo] = value;
      }
   }

   private HotelListItem[] _HotelListItems;
   private HotelListItem[] HotelListItems
   {
      get
      {
         if (_HotelListItems == null)
         {
            HotelSearchRS objPropertyListHotelSearchRS = (HotelSearchRS)Session[Constants.Sessions.PropertyListHotelSearchRS];
            _HotelListItems = objPropertyListHotelSearchRS.HotelListItems;
         }
         return _HotelListItems;
      }
      set
      {
         _HotelListItems = value;
         Session[Constants.Sessions.PropertyListHotelSearchRS] = value;
      }
   }

   HotelBookingRS HotelBookingRS
   {
      get { return (HotelBookingRS)Session[Constants.Sessions.HotelBookingRS]; }
      set
      {
         Session[Constants.Sessions.HotelBookingRS] = value;
      }
   }

   HotelPaymentRS HotelPaymentRS
   {
      get
      {
         return (HotelPaymentRS)Session[Constants.Sessions.HotelPaymentRS];
      }
      set
      {
         Session[Constants.Sessions.HotelPaymentRS] = value;
      }
   }

   private Dictionary<ImagePackageTypes, List<HotelImage>> _ColumnImages;
   Dictionary<ImagePackageTypes, List<HotelImage>> ColumnImages
   {
      get
      {
         if (_ColumnImages == null)
         {
            Session[Constants.Sessions.ColumnImages] = new Dictionary<ImagePackageTypes, List<HotelImage>>();
            _ColumnImages = (Dictionary<ImagePackageTypes, List<HotelImage>>)Session[Constants.Sessions.ColumnImages];
            foreach (var itemName in Enum.GetNames(typeof(ImagePackageTypes)))
            {
               var itemType = (ImagePackageTypes)Enum.Parse(typeof(ImagePackageTypes), itemName);
               _ColumnImages.Add(itemType, new List<HotelImage>());
            }
         }
         return _ColumnImages;
      }
   }

   #endregion

   #region Controls

   private ErrorDisplayControl ucErrorDisplayControl;
   private ImageHoldingControl ucImageHoldingControl;
   private StayCriteriaSelectorControl_MM ucStayCriteriaControl;
   private AvailCalSelectorControl ucAvailCalSelectorControl;
   private RoomQuantitySelectorControl ucRoomQuantitySelectorControl;
   private TotalCostControl ucTotalCostControl;
   private GuestDetailsEntryControl ucGuestDetailsEntryControl;
   private ConfirmationControl ucConfirmationControl;
   private RemoteContentContainer ucRemoteContentContainer;
   private TrackingCodeControl ucBodyTrackingCodeControl;

   #endregion

   #region Properties

   private bool IsRoomRateDescriptionModel = ConfigurationManager.AppSettings["EnableRoomRateDescriptionModel"] == "1";
   bool bAsyncGetHotelSearchPropertyList;
   private bool bAsyncGetHotelAvailCalendarInfo;
   private bool bAsyncGetHotelDescriptiveInfo;
   private bool bAsyncGetHotelAvailInfo;
   private bool bAsyncBookHotel;
   private bool bPrepayBooking;
   private bool bIsConfirmed;
   private bool bShowRoomTypePhoto = false;
   private bool bAddonToggled = false;

   private HotelBookingReadSegment[] _HotelBookingReadSegments;
   private HotelBookingReadSegment[] HotelBookingReadSegments
   {
      get
      {
         if (_HotelBookingReadSegments == null)
         {
            CancelDetailsEntryInfo objCancelDetailsEntryInfo = (CancelDetailsEntryInfo)Session[Constants.Sessions.CancelDetailsEntryInfo];
            _HotelBookingReadSegments = WbsUiHelper.GetValidatedBookings(objCancelDetailsEntryInfo);
         }
         return _HotelBookingReadSegments;
      }
   }

   private bool? _EnabledChildren;
   public bool EnabledChildren
   {
      get
      {
         if (_EnabledChildren == null)
            _EnabledChildren = WebconfigHelper.GetEnableChildrenByHotelCode(StayCriteriaSelection.HotelCode);
         return _EnabledChildren.Value;
      }
   }

   private string ClientProtocol
   {
      get { return Request.IsSecureConnection ? "https://" : "http://"; }
   }

   private string _RemoteContentContainerUrl = string.Empty;
   private string RemoteContentContainerBasedUrl
   {
      get
      {
         if (string.IsNullOrWhiteSpace(_RemoteContentContainerUrl))
         {
            string CDNPath = ConfigurationManager.AppSettings[Constants.WebConfigKeys.CustomCDNPath];
            CDNPath = CDNPath.EndsWith("/") ? CDNPath : CDNPath + "/";
            string currentLanguageCodeString = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            string action = "booking";
            string baseUrl = string.Format("{0}{1}/{2}/", CDNPath, currentLanguageCodeString, action);
            _RemoteContentContainerUrl = baseUrl;
         }

         return _RemoteContentContainerUrl;
      }
   }

   private string CurrencyCode
   {
      get
      {
         string strCurrencyCode = string.Empty;
         if (HotelDescriptiveInfoRS.HotelDescriptiveInfos.Length > 0)
            strCurrencyCode = HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].CurrencyCode;

         return strCurrencyCode;
      }
   }


   private string CDNLocationMappingString { get { return ConfigurationManager.AppSettings["CDNMapping." + StayCriteriaSelection.HotelCode]; } }

   #endregion

   #region PAGE EVENTS

   protected override void OnInit(EventArgs e)
   {
      base.OnInit(e);

      if ((!string.IsNullOrWhiteSpace(Request.QueryString["fres"]) && Request.QueryString["fres"] == "1") || Step.Equals(BookingSteps.ViewConfirmation))
      {
         StartOver();
         Session.Abandon();
         Response.Redirect(ResolveUrl("~/Default.aspx"));
         return;
      }
      if (!string.IsNullOrWhiteSpace(Request.QueryString[Constants.QueryString.CrossPageErrors]) &&
          Request.QueryString[Constants.QueryString.CrossPageErrors] == "1"
          && (BookingSteps)(Session[Constants.Sessions.CurrentBookingStep]) == BookingSteps.ProcessPayment)
         Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.GuestInfo;


      LoadControls();
   }

   protected override void Page_Load(object sender, EventArgs e)
   {
      base.Page_Load(sender, e);

      if (Step > BookingSteps.SelectHotel && Step <= BookingSteps.GuestInfo && !string.IsNullOrWhiteSpace(Request.QueryString[Constants.QueryString.StartOver]))
         ScriptManager.RegisterClientScriptBlock(this, GetType(), "DefaultPage"
            , string.Format("startOverPrompt(\"{0}\");", GetGlobalResourceObject("JSResources", "StartOverConfirmation"))
            , true);

      if (Step >= BookingSteps.SelectHotel
          && (Session[Constants.Sessions.PropertyListHotelSearchRS] == null || ((HotelSearchRS)Session[Constants.Sessions.PropertyListHotelSearchRS]).HotelListItems == null))
         bAsyncGetHotelSearchPropertyList = true;

      if (Step >= BookingSteps.SelectStayDate
         && (HotelDescriptiveInfoRS == null
               || HotelDescriptiveInfoRS.HotelDescriptiveInfos == null
               || ((StayCriteriaSelection != null && !String.IsNullOrWhiteSpace(StayCriteriaSelection.HotelCode)) && HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].HotelCode != StayCriteriaSelection.HotelCode)))
         bAsyncGetHotelDescriptiveInfo = true;

      if (Step == BookingSteps.SelectStayDate
          && (Session[Constants.Sessions.HotelAvailabilityCalendarRS] == null
              || ((HotelAvailabilityRS)Session[Constants.Sessions.HotelAvailabilityCalendarRS]).HotelRoomAvailInfos == null
              || ((HotelAvailabilityRS)Session[Constants.Sessions.HotelAvailabilityCalendarRS]).HotelRoomAvailInfos.Length == 0
              || ((StayCriteriaSelection != null && string.IsNullOrWhiteSpace(StayCriteriaSelection.HotelCode)) && ((HotelAvailabilityRS)Session[Constants.Sessions.HotelAvailabilityCalendarRS]).HotelRoomAvailInfos[0].HotelCode != StayCriteriaSelection.HotelCode))
         )
         bAsyncGetHotelAvailCalendarInfo = true;


      ConfiguresControls();

      PageAsyncTask task = new PageAsyncTask(BeginAsyncOperation, EndAsyncOperation, TimeoutAsyncOperation, null);
      RegisterAsyncTask(task);

      return;
   }

   protected void Page_PreRenderComplete(object sender, EventArgs e)
   {
      this.IsParentPreRender = true;

      if (Step == BookingSteps.Confirmed)
         bIsConfirmed = true;

      ConfiguresControls();
      RenderControls();

      bShowRoomTypePhoto = false;

      ////Reset data from session incase confirmation code has been returned
      if (Step == BookingSteps.Confirmed)
         Step = BookingSteps.ViewConfirmation;

      this.PageComplete();


      RegisterClientVariables();
      return;
   }
   #endregion

   #region Async Task Related

   private IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
   {
      wbsIISAsyncResult = new WBSAsyncResult(cb, state);

      if (bAsyncGetHotelSearchPropertyList || bAsyncGetHotelAvailCalendarInfo || bAsyncGetHotelDescriptiveInfo || bAsyncGetHotelAvailInfo)
      {
         this.BeginResumeAsyncDataCapture();
      }
      else if (bAsyncBookHotel)
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
      if (bAsyncGetHotelSearchPropertyList)
      {
         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitHotelSearchPropertyListRQ(ref wbsAPIRouterData);
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelSearchPropertyListComplete), null, false);
      }

      else if (bAsyncGetHotelAvailCalendarInfo)
      {
         wbsAPIRouterData = new WBSAPIRouterData();
         DateTime availCalStart = AvailCalStartDate;
         DateTime availCalEnd = availCalStart.AddMonths(1).AddDays(12);
         TimeSpan dayInMonth = availCalEnd - availCalStart;
         this.WbsApiRouterHelper.InitHotelAvailCalendarInfoRQ(ref wbsAPIRouterData, StayCriteriaSelection, availCalStart, dayInMonth.Days, this.WbsUiHelper.GetLoginLinkedProfileIdentifier());
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelAvailCalendarInfoComplete), null, false);
      }

      else if (bAsyncGetHotelDescriptiveInfo)
      {
         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitHotelDescriptiveInfoRQ(ref wbsAPIRouterData, StayCriteriaSelection.HotelCode);
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelDescriptiveInfoComplete), null, false);
      }

      else if (bAsyncGetHotelAvailInfo)
      {
         DateTime dtRateGridStartDate = (DateTime)Session["RateGridStartDate"];
         DateTime dtRateGridEndDate = dtRateGridStartDate.AddDays(this.NumberDaysInRateGrid - 1);

         DateTime dtAvailCalStartDate = StayCriteriaSelection.ArrivalDate;

         if (dtRateGridStartDate.Date < dtAvailCalStartDate.Date)
            dtAvailCalStartDate = dtRateGridStartDate;

         int intAvailCalNumDays = dtRateGridEndDate.Date.Subtract(dtAvailCalStartDate.Date).Days + 1;

         if (string.IsNullOrWhiteSpace(StayCriteriaSelection.HotelCode))
            Response.Redirect(ResolveUrl("~/Pages/Default.aspx"));

         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitHotelAvailInfoRQ(ref wbsAPIRouterData, StayCriteriaSelection, dtAvailCalStartDate, intAvailCalNumDays, this.WbsUiHelper.GetLoginLinkedProfileIdentifier());
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, HotelAvailInfoComplete, null, false);
      }
      else if (bAsyncBookHotel)
      {
         BookingAction enumBookingAction = this.IsBookThrough ? BookingAction.Sell : BookingAction.TestSell;

         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitBookHotelRQ(ref wbsAPIRouterData, enumBookingAction);
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(BookHotelComplete), null, true);
      }

      else
      {
         // End async page operation

         if (!wbsIISAsyncResult.IsCompleted)
            wbsIISAsyncResult.SetComplete();
      }
   }

   private void EndAsyncOperation(IAsyncResult ar)
   {
      return;
   }

   private void TimeoutAsyncOperation(IAsyncResult ar)
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

   private void HotelAvailCalendarInfoComplete(IAsyncResult ar)
   {
      if (this.WbsApiRouterHelper.ProcessHotelAvailCalendarInfoRS(ref wbsAPIRouterData))
      {
         Session["AvailCalDateSelections"] = new DateTime[0];
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

   private void HotelDescriptiveInfoComplete(IAsyncResult ar)
   {
      if (this.WbsApiRouterHelper.ProcessHotelDescriptiveInfoRS(ref wbsAPIRouterData))
      {
         bAsyncGetHotelDescriptiveInfo = false;
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

   private void BookHotelComplete(IAsyncResult ar)
   {
      if (this.WbsApiRouterHelper.ProcessBookHotelRS(ref wbsAPIRouterData))
      {
         bAsyncBookHotel = false;
         Step = BookingSteps.Confirmed;
         LoadControls();
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


   #endregion

   #region Load Controls

   private void LoadErrorDisplayControl()
   {
      ucErrorDisplayControl = (ErrorDisplayControl)LoadControl(strErrorDisplayControlPath);

      phErrorDisplayControl.Controls.Clear();
      phErrorDisplayControl.Controls.Add(ucErrorDisplayControl);

      return;
   }
   private void LoadStaySelectorControl()
   {
      ucStayCriteriaControl = (StayCriteriaSelectorControl_MM)LoadControl(strStayCriteriaSelectorControlPath);
      ucStayCriteriaControl.ID = "StayCriteriaSelectorControl";

      phStayCriteriaControl.Controls.Clear();
      phStayCriteriaControl.Controls.Add(ucStayCriteriaControl);

      ucStayCriteriaControl.HotelSelected += ucStayCriteriaControl_HotelSelected;
      ucStayCriteriaControl.EditModeSelected += ucStayCriteriaControl_EditModeSelected;

      return;
   }
   private void LoadImageHoldingControl()
   {
      ucImageHoldingControl = (ImageHoldingControl)LoadControl(strImageHoldingControlPath);
      ucImageHoldingControl.ID = "ImageHoldingControl";

      phImageHoldingControl.Controls.Clear();
      phImageHoldingControl.Controls.Add(ucImageHoldingControl);
      phImageHoldingControl.Visible = true;
   }
   private void LoadRemoteContentContainerControl()
   {
      ucRemoteContentContainer = (RemoteContentContainer)LoadControl(strRemoteContentContainerControlPath);
      ucRemoteContentContainer.ID = "RemoteContentContainer";

      phRemoteContentContainerControl.Controls.Clear();
      phRemoteContentContainerControl.Controls.Add(ucRemoteContentContainer);
      phRemoteContentContainerControl.Visible = true;
   }
   private void LoadAvailCalSelectorControl()
   {
      ucAvailCalSelectorControl = (AvailCalSelectorControl)LoadControl(strAvailCalSelectorControlPath);
      ucAvailCalSelectorControl.ID = "AvailCalSelectorControl";

      phAvailCalSelectorControl.Controls.Clear();
      phAvailCalSelectorControl.Controls.Add(ucAvailCalSelectorControl);

      ucAvailCalSelectorControl.PrevMonthRequested += ucAvailCalSelectorControl_PrevMonthRequested;
      ucAvailCalSelectorControl.NextMonthRequested += ucAvailCalSelectorControl_NextMonthRequested;
      ucAvailCalSelectorControl.AvailCalCompleted += ucAvailCalSelectorControl_AvailCalCompleted;
      ucAvailCalSelectorControl.AvailCalRequested += ucAvailCalSelectorControl_AvailCalRequested;
      ucAvailCalSelectorControl.StayDateSelected += ucAvailCalSelectorControl_StayDateSelected;
   }
   private void LoadRoomQuantitySelectorControl()
   {
      ucRoomQuantitySelectorControl = (RoomQuantitySelectorControl)LoadControl(strRoomQuantitySelectorControlPath);
      ucRoomQuantitySelectorControl.ID = "RoomQuantitySelectorControl";
      ucRoomQuantitySelectorControl.MaxNumberOfRoomAvailable = int.Parse(ConfigurationManager.AppSettings["StayCriteriaSelectorControl.MaxRooms"]);
      phRoomQuantitySelectorControl.Controls.Clear();
      phRoomQuantitySelectorControl.Controls.Add(ucRoomQuantitySelectorControl);

      ucRoomQuantitySelectorControl.RoomQuantityCompleted += ucRoomQuantitySelectorControl_RoomQuantityCompleted;
      ucRoomQuantitySelectorControl.EditModeSelected += ucRoomQuantitySelectorControl_EditModeSelected;

      return;
   }
   private void LoadRoomDetailSelectorControl()
   {
      phRoomDetailSelectorControl.Controls.Clear();

      for (var i = 0; i < StayCriteriaSelection.RoomOccupantSelections.Length; i++)
      {
         var ucRoomDetailSelectorControl = (RoomDetailSelectorControl)LoadControl(strRoomDetailSelectorControlPath);
         ucRoomDetailSelectorControl.ID = "RoomDetailSelectorControl" + i;
         ucRoomDetailSelectorControl.RoomRefID = StayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;

         phRoomDetailSelectorControl.Controls.Add(ucRoomDetailSelectorControl);

         ucRoomDetailSelectorControl.AdultQuantityCompleted += ucRoomDetailSelectorControl_AdultQuantityCompleted;
         if (EnabledChildren)
            ucRoomDetailSelectorControl.ChildrenQuantityCompleted += ucRoomDetailSelectorControl_ChildrenQuantityCompleted;

         ucRoomDetailSelectorControl.ShowRoomPhotoSelected += ucRoomDetailSelectorControl_ShowRoomPhotoSelected;
         ucRoomDetailSelectorControl.RatePlanSelected += ucRoomDetailSelectorControl_RatePlanSelected;
         ucRoomDetailSelectorControl.EditModeSelected += ucRoomDetailSelectorControl_EditModeSelected;
         ucRoomDetailSelectorControl.AddOnToggled += ucRoomDetailSelectorControl_AddOnToggled;
         ucRoomDetailSelectorControl.RoomRateCompleted += ucRoomDetailSelectorControl_RoomRateCompleted;
      }
   }
   private void LoadTotalCostControl()
   {
      phTotalCostControl.Controls.Clear();

      ucTotalCostControl = (TotalCostControl)LoadControl(strTotalCostControlPath);
      ucTotalCostControl.ID = "TotalCostControl";
      ucTotalCostControl.ProceedToPayment += ucTotalCostControl_Completed;

      if (Step.Equals(BookingSteps.SelectRoomDetail) &&
          (CurrentRoomDetailStep.Equals(RoomDetailSelectionStep.SelectAdultQuantity) || CurrentRoomDetailStep.Equals(RoomDetailSelectionStep.SelectChildrenQuantity)))
      {
         for (int i = 0; i < phRoomDetailSelectorControl.Controls.Count; i++)
         {
            if (
                ((RoomDetailSelectorControl)phRoomDetailSelectorControl.Controls[i]).RoomRefID.Equals(
                    CurrentRoomRefID) && i > 0)
            {
               ((RoomDetailSelectorControl)phRoomDetailSelectorControl.Controls[i - 1]).AddTempTotalCostControl(
                   ucTotalCostControl);
               break;
            }

         }
      }

      else if (Step == BookingSteps.SelectRoomDetail && (CurrentRoomDetailStep == RoomDetailSelectionStep.SelectRoomType || CurrentRoomDetailStep == RoomDetailSelectionStep.SelectExtra))
      {
         foreach (RoomDetailSelectorControl roomDetailControl in phRoomDetailSelectorControl.Controls)
         {
            if (roomDetailControl.RoomRefID.Equals(CurrentRoomRefID))
            {
               roomDetailControl.AddTempTotalCostControl(ucTotalCostControl);
               break;
            }
         }
      }
      else if (Step >= BookingSteps.BookingSummary)
         phTotalCostControl.Controls.Add(ucTotalCostControl);
   }
   private void LoadGuestDetailsEntryControl()
   {
      ucGuestDetailsEntryControl = (GuestDetailsEntryControl)LoadControl(strGuestDetailsEntryControlPath);
      ucGuestDetailsEntryControl.ID = "GuestDetailsEntryControl";

      phGuestDetailsEntryControl.Controls.Clear();
      phGuestDetailsEntryControl.Controls.Add(ucGuestDetailsEntryControl);

      ucGuestDetailsEntryControl.GuestDetailsCompleted += ucGuestDetailsEntryControl_GuestDetailsCompleted;

      return;
   }
   private void LoadConfirmationControl()
   {
      ucConfirmationControl = (ConfirmationControl)LoadControl(strConfirmationControlPath);
      ucConfirmationControl.ID = "ConfirmationControl";
      phConfirmationControl.Controls.Clear();
      phConfirmationControl.Controls.Add(ucConfirmationControl);
   }

   #endregion

   #region Configure Controls

   private void ConfigureErrorDisplayControl()
   {
      ucErrorDisplayControl.ErrorInfos = this.PageErrors;

      return;
   }

   private void ConfigureStaySelectorControl()
   {
      if (HotelListItems == null)
         return;

      if (string.IsNullOrWhiteSpace(StayCriteriaSelection.HotelCode) && HotelListItems.Length == 1) // default hotel selection if only 1 choice
         StayCriteriaSelection.HotelCode = HotelListItems[0].HotelCode;

      SelectionMode mode = (Step == BookingSteps.SelectHotel ? SelectionMode.Edit : SelectionMode.Selected);
      mode = (bIsConfirmed ? (mode | SelectionMode.NonModifiable) : mode);

      ucStayCriteriaControl.ID = "StayCriteriaSelector";
      ucStayCriteriaControl.HotelListItems = HotelListItems;
      ucStayCriteriaControl.StayCriteriaSelection = StayCriteriaSelection;
      ucStayCriteriaControl.StayCriteriaSelectorMode = mode;

      return;
   }

   private void ConfigureImageHoldingControl()
   {
      if (ucImageHoldingControl.Images != null && ucImageHoldingControl.Images.Count > 0)
      {
         return;
      }

      GetImageFromHotelDescriptive();

      if (Step == BookingSteps.SelectHotel)
         ucImageHoldingControl.Images = GetImage(ImagePackageTypes.SelectHotel);
      else if (Step == BookingSteps.SelectStayDate)
         ucImageHoldingControl.Images = GetImage(ImagePackageTypes.SelectStayDate);
      else if (Step == BookingSteps.SelectRoomQuantity)
         ucImageHoldingControl.Images = GetImage(ImagePackageTypes.SelectRoomQuantity);
      else if (Step == BookingSteps.SelectRoomDetail && (CurrentRoomDetailStep == RoomDetailSelectionStep.SelectAdultQuantity || CurrentRoomDetailStep == RoomDetailSelectionStep.SelectChildrenQuantity))
         ucImageHoldingControl.Images = GetImage(ImagePackageTypes.SelectPeopleQuantity);
      else if (Step == BookingSteps.SelectRoomDetail && CurrentRoomDetailStep == RoomDetailSelectionStep.SelectRoomType)
         ucImageHoldingControl.Images = GetImage(ImagePackageTypes.SelectRoomRate);
      else if (Step == BookingSteps.SelectRoomDetail && CurrentRoomDetailStep == RoomDetailSelectionStep.SelectExtra)
         ucImageHoldingControl.Images = GetImage(ImagePackageTypes.SelectRoomAddon);
      else if (Step == BookingSteps.BookingSummary)
         ucImageHoldingControl.Images = GetImage(ImagePackageTypes.BookingSummary);
      else if (Step == BookingSteps.Confirmed)
         ucImageHoldingControl.Images = GetImage(ImagePackageTypes.Confirmed);
   }

   private void ConfigureRemoteContentContainerControl()
   {
      if (bShowRoomTypePhoto)
      {
         return;
      }

      string selectedLocationUrl = RemoteContentContainerBasedUrl;
      string finalUrl = selectedLocationUrl;

      if (Step >= BookingSteps.SelectStayDate)
      {
         selectedLocationUrl = RemoteContentContainerBasedUrl + CDNLocationMappingString + "/";
         finalUrl = selectedLocationUrl;
      }

      if (Step >= BookingSteps.SelectRoomQuantity)
         finalUrl = selectedLocationUrl + "rooms/";



      if (Step >= BookingSteps.SelectRoomDetail &&
          (CurrentRoomDetailStep == RoomDetailSelectionStep.SelectAdultQuantity ||
           CurrentRoomDetailStep == RoomDetailSelectionStep.SelectChildrenQuantity))
      {
         finalUrl = selectedLocationUrl + "rooms/";
      }

      // Config Room Rate Plan require further step. List of available rates is retrieved later 
      // in ConfigureRoomRateSelectorControl . after that filter images.
      if (Step >= BookingSteps.SelectRoomDetail &&
          (CurrentRoomDetailStep == RoomDetailSelectionStep.SelectRoomType))
         finalUrl = selectedLocationUrl + "rates/";


      // Config Extras require further step. List of available extras is retrieved later 
      // in ConfigureRoomRateSelectorControl . after that filter images.
      if (Step >= BookingSteps.SelectRoomDetail && CurrentRoomDetailStep == RoomDetailSelectionStep.SelectExtra)
         finalUrl = selectedLocationUrl + "extras/";

      if (Step >= BookingSteps.BookingSummary)
         finalUrl = selectedLocationUrl + "summary/";

      if (Step >= BookingSteps.Confirmed)
         finalUrl = selectedLocationUrl + "confirmation/";

      if (!bShowRoomTypePhoto)
         ucRemoteContentContainer.Src = finalUrl;
      else
         bShowRoomTypePhoto = false;
   }

   private void ConfigureAvailCalSelectorControl()
   {
      HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session[Constants.Sessions.HotelAvailabilityCalendarRS];
      DateTime[] objAvailCalDateSelections = (DateTime[])Session["AvailCalDateSelections"];

      DateTime dtToday = TZNet.ToLocal(WbsUiHelper.GetTimeZone(StayCriteriaSelection.HotelCode), DateTime.UtcNow).Date;

      if (Step != BookingSteps.SelectStayDate)
      {
         ucAvailCalSelectorControl.AvailCalendarSelectorMode = AvailCalendarSelectorMode.ViewLink;
         ucAvailCalSelectorControl.Mode = (bIsConfirmed ? SelectionMode.NonModifiable : SelectionMode.Selected);
         ucAvailCalSelectorControl.AvailabilityCalendarInfo = new AvailabilityCalendarInfo[0];
         ucAvailCalSelectorControl.SelectedDates = objAvailCalDateSelections;
         ucAvailCalSelectorControl.StayCriteriaSelection = StayCriteriaSelection;
         ucAvailCalSelectorControl.Today = dtToday.Date;
      }

      else
      {
         AvailabilityCalendarInfo[] objAvailabilityCalendarInfos = new AvailabilityCalendarInfo[0];
         if (objHotelAvailabilityRS != null && objHotelAvailabilityRS.HotelRoomAvailInfos != null)
         {
            objAvailabilityCalendarInfos =
                new AvailabilityCalendarInfo[objHotelAvailabilityRS.HotelRoomAvailInfos.Length];

            for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
            {
               objAvailabilityCalendarInfos[i] = new AvailabilityCalendarInfo
                                                     {
                                                        SegmentRefID = objHotelAvailabilityRS.HotelRoomAvailInfos[i].SegmentRefID,
                                                        AvailabilityCalendar = objHotelAvailabilityRS.HotelRoomAvailInfos[i].AvailabilityCalendar
                                                     };
            }
         }

         ucAvailCalSelectorControl.AvailCalendarSelectorMode = AvailCalendarSelectorMode.ViewCalendar;
         ucAvailCalSelectorControl.Mode = SelectionMode.Edit;
         ucAvailCalSelectorControl.AvailabilityCalendarInfo = objAvailabilityCalendarInfos;
         ucAvailCalSelectorControl.SelectedDates = objAvailCalDateSelections;
         ucAvailCalSelectorControl.StayCriteriaSelection = StayCriteriaSelection;
         ucAvailCalSelectorControl.Today = dtToday.Date;
      }
   }

   private void ConfigureRoomQuantitySelectorControl()
   {
      ucRoomQuantitySelectorControl.StayCriteriaSelection = StayCriteriaSelection;

      if (Step != BookingSteps.SelectRoomQuantity)
         ucRoomQuantitySelectorControl.Mode = (bIsConfirmed ? SelectionMode.NonModifiable : SelectionMode.Selected);
      else
      {
         StayCriteriaSelection.PromotionCode = string.Empty;
         ucRoomQuantitySelectorControl.Mode = SelectionMode.Edit;
      }
   }

   #region Configure Room Detail Selector Control Methods

   private void ConfigureRoomDetailSelectorControl()
   {
      for (int i = 0; i < phRoomDetailSelectorControl.Controls.Count; i++)
      {
         RoomDetailSelectorControl ucRoomDetailSelectorControl = (RoomDetailSelectorControl)phRoomDetailSelectorControl.Controls[i];

         var numberRoomRefId = int.Parse(ucRoomDetailSelectorControl.RoomRefID);
         var numberCurrentRoomRefId = int.Parse(CurrentRoomRefID);
         if (Step == BookingSteps.SelectRoomDetail && numberRoomRefId > numberCurrentRoomRefId)
         {
            ucRoomDetailSelectorControl.Mode = SelectionMode.Hidden;
            ucRoomDetailSelectorControl.Step = RoomDetailSelectionStep.Done;
            continue;
         }
         else if (Step == BookingSteps.SelectRoomDetail && numberRoomRefId == numberCurrentRoomRefId)
         {
            ucRoomDetailSelectorControl.Mode = SelectionMode.Edit;
            ucRoomDetailSelectorControl.Step = CurrentRoomDetailStep;
         }
         else
         {
            ucRoomDetailSelectorControl.Mode = SelectionMode.Selected;
            ucRoomDetailSelectorControl.Step = RoomDetailSelectionStep.Done;
         }

         ucRoomDetailSelectorControl.Clear();

         // PeopleQuantitySelectorControl
         ConfigureAdultQuantityControl(ucRoomDetailSelectorControl);
         if (EnabledChildren)
            ConfigureChildrenQuantityControl(ucRoomDetailSelectorControl);

         if (HotelAvailabilityRS == null || HotelAvailabilityRS.HotelRoomAvailInfos == null || HotelAvailabilityRS.HotelRoomAvailInfos.Length <= 0)
            return;

         HotelRoomAvailInfo objHotelRoomAvailInfo;
         objHotelRoomAvailInfo = HotelAvailabilityRS.HotelRoomAvailInfos.FirstOrDefault(item => item.SegmentRefID == ucRoomDetailSelectorControl.RoomRefID);
         HotelDescriptiveInfo objHotelDescriptiveInfo = HotelDescriptiveInfoRS.HotelDescriptiveInfos[0];

         // RoomTypeSelectorItemControl : Edit Mode
         ConfigureRoomRateSelectorControl(ucRoomDetailSelectorControl, objHotelRoomAvailInfo, objHotelDescriptiveInfo);

         // Room Extras
         ConfigureRoomExtra(ucRoomDetailSelectorControl, objHotelRoomAvailInfo, objHotelDescriptiveInfo);
      }
   }

   private void ConfigureAdultQuantityControl(RoomDetailSelectorControl ucRoomDetailSelectorControl)
   {
      SelectionMode selectionMode = ((CurrentRoomRefID == ucRoomDetailSelectorControl.RoomRefID && CurrentRoomDetailStep == RoomDetailSelectionStep.SelectAdultQuantity) ?
                                          SelectionMode.Edit : SelectionMode.Selected);
      if (bIsConfirmed)
         selectionMode = SelectionMode.NonModifiable;

      var ucAdultQuantitySelectorControl = (PeopleQuantitySelectorControl)LoadControl(strPeopleQuanitySelectorControlPath);
      ucAdultQuantitySelectorControl.Mode = selectionMode;
      ucAdultQuantitySelectorControl.DetailStep = RoomDetailSelectionStep.SelectAdultQuantity;
      ucAdultQuantitySelectorControl.ID = "AdultQuantitySelectorControl_" + ucRoomDetailSelectorControl.RoomRefID;
      ucAdultQuantitySelectorControl.MinQuantity = 1;
      ucAdultQuantitySelectorControl.MaxQuantity = WebconfigHelper.GetMaxAdultByHotel(StayCriteriaSelection.HotelCode);
      ucAdultQuantitySelectorControl.RoomRefID = ucRoomDetailSelectorControl.RoomRefID;
      ucAdultQuantitySelectorControl.NumberOfPeople = StayCriteriaSelection.RoomOccupantSelections.FirstOrDefault(item => item.RoomRefID == ucRoomDetailSelectorControl.RoomRefID).NumberAdults;
      ucRoomDetailSelectorControl.AddAdultQuantitySelectorControl(ucAdultQuantitySelectorControl);
   }

   private void ConfigureChildrenQuantityControl(RoomDetailSelectorControl ucRoomDetailSelectorControl)
   {
      SelectionMode selectionMode = ((CurrentRoomRefID == ucRoomDetailSelectorControl.RoomRefID && CurrentRoomDetailStep == RoomDetailSelectionStep.SelectChildrenQuantity) ?
                                          SelectionMode.Edit : SelectionMode.Selected);
      if (bIsConfirmed)
         selectionMode = SelectionMode.NonModifiable;

      var ucChildrenQuantitySelectorControl = (PeopleQuantitySelectorControl)LoadControl(strPeopleQuanitySelectorControlPath);
      ucChildrenQuantitySelectorControl.Mode = selectionMode;
      ucChildrenQuantitySelectorControl.DetailStep = RoomDetailSelectionStep.SelectChildrenQuantity;
      ucChildrenQuantitySelectorControl.ID = "ChildrenQuantitySelectorControl_" + ucRoomDetailSelectorControl.RoomRefID;
      ucChildrenQuantitySelectorControl.MinQuantity = 0;
      ucChildrenQuantitySelectorControl.MaxQuantity = WebconfigHelper.GetMaxChildrenByHotel(StayCriteriaSelection.HotelCode);
      ucChildrenQuantitySelectorControl.RoomRefID = ucRoomDetailSelectorControl.RoomRefID;
      ucChildrenQuantitySelectorControl.NumberOfPeople = StayCriteriaSelection.RoomOccupantSelections.FirstOrDefault(item => item.RoomRefID == ucRoomDetailSelectorControl.RoomRefID).NumberChildren;
      ucRoomDetailSelectorControl.AddChildrenQuantitySelectorContorl(ucChildrenQuantitySelectorControl);
   }

   private void ConfigureRoomRateSelectorControl(RoomDetailSelectorControl ucRoomDetailSelectorControl, HotelRoomAvailInfo objHotelRoomAvailInfo, HotelDescriptiveInfo objHotelDescriptiveInfo)
   {
      // Selected Mode
      var selectedRoomRate = (from rrs in RoomRateSelections
                              where rrs.RoomRefID == ucRoomDetailSelectorControl.RoomRefID
                              select rrs).FirstOrDefault();

      // Edit Mode
      if (Step == BookingSteps.SelectRoomDetail
              && CurrentRoomRefID == ucRoomDetailSelectorControl.RoomRefID
              && ucRoomDetailSelectorControl.Step == RoomDetailSelectionStep.SelectRoomType)
      {
         List<string> roomTypesForFilter = new List<string>();
         foreach (var roomType in objHotelRoomAvailInfo.RoomTypes)
         {
            bool bRatesAvailable = false;
            HotelAvailRoomType localRoomType = roomType;
            var roomRates = objHotelRoomAvailInfo.RoomRates.Where(item => item.RoomTypeCode == localRoomType.Code);
            if (roomRates.Count() <= 0)
               continue;


            foreach (HotelAvailRoomRate roomRate in roomRates)
            {
               var localRoomRate = roomRate;
               if (!IsRoomRateDescriptionModel || localRoomRate.DescriptionStatus == RoomRateDescriptionStatus.Active)
               {
                  bRatesAvailable = true;
                  break;
               }

               var ratePlans = objHotelRoomAvailInfo.RatePlans.Where(ratePlan => ratePlan.Code == localRoomRate.RatePlanCode &&
                       (ratePlan.Type == RatePlanType.Negotiated || ratePlan.Type == RatePlanType.Consortia));

               if (ratePlans.Count() > 0)
                  bRatesAvailable = true;
            }

            if (!bRatesAvailable)
               continue;

            HtmlGenericControl rtPanelWrapper = new HtmlGenericControl("div");
            rtPanelWrapper.Attributes.Add("class", "mm_roomrate_info mm_background_info");
            HtmlGenericControl rtPanel = new HtmlGenericControl("div");
            rtPanel.Attributes.Add("class", "mm_roomrate_content mm_text_info");
            HtmlGenericControl rtName = new HtmlGenericControl("span");
            rtName.Attributes.Add("class", "mm_text_x_strong");
            rtName.InnerText = roomType.Name;
            rtPanelWrapper.Controls.Add(rtPanel);
            rtPanel.Controls.Add(rtName);
            ucRoomDetailSelectorControl.AddRoomTypeControl(rtPanelWrapper);

            foreach (HotelAvailRoomRate roomRate in roomRates)
            {
               var localRoomRate = roomRate;
               if (IsRoomRateDescriptionModel && localRoomRate.DescriptionStatus == RoomRateDescriptionStatus.Inactive)
               {
                  HotelAvailRoomRate cpRoomRate = localRoomRate;
                  var ratePlans = from ratePlan in objHotelRoomAvailInfo.RatePlans
                                  where ratePlan.Code == cpRoomRate.RatePlanCode && ratePlan.Type != RatePlanType.Negotiated && ratePlan.Type != RatePlanType.Consortia
                                  select ratePlan;
                  if (ratePlans.Count() > 0)
                     continue;
               }

               RatePlanSelectorItemControl ucRatePlanSelectorItemControl = (RatePlanSelectorItemControl)LoadControl(strRatePlanSelectorItemControlPath);
               ucRatePlanSelectorItemControl.Mode = (CurrentRoomDetailStep == RoomDetailSelectionStep.SelectRoomType ? SelectionMode.Edit : SelectionMode.Selected);
               ucRatePlanSelectorItemControl.ID = string.Format("{0}_{1}_{2}_{3}", "RatePlanSelectorItem"
                   , ucRoomDetailSelectorControl.RoomRefID
                   , localRoomRate.RoomTypeCode
                   , RemoveSpecialCharFromString(localRoomRate.RatePlanCode));
               ucRatePlanSelectorItemControl.RoomRefID = ucRoomDetailSelectorControl.RoomRefID;
               ucRatePlanSelectorItemControl.RoomRate = localRoomRate;
               ucRatePlanSelectorItemControl.RoomTypeDescription = objHotelDescriptiveInfo.RoomTypes.FirstOrDefault(item => item.Code == localRoomType.Code);
               ucRatePlanSelectorItemControl.CreditCardCodes = objHotelDescriptiveInfo.CreditCardCodes;
               ucRatePlanSelectorItemControl.Selected = false;
               ucRatePlanSelectorItemControl.RatePlan = objHotelRoomAvailInfo.RatePlans.FirstOrDefault(item => item.Code == localRoomRate.RatePlanCode);
               ucRatePlanSelectorItemControl.IsShowPhotoLink = !(Step == BookingSteps.Confirmed || Step == BookingSteps.GuestInfo);
               ucRatePlanSelectorItemControl.RoomRateSelected += ucRoomDetailSelectorControl_RatePlanSelected;


               ucRoomDetailSelectorControl.AddRatePlanSelectorItem(ucRatePlanSelectorItemControl);

               if (!roomTypesForFilter.Contains(roomRate.RoomTypeCode) && !string.IsNullOrWhiteSpace(roomRate.RoomTypeCode))
                  roomTypesForFilter.Add(roomRate.RoomTypeCode);

            }
         }

         if (!bShowRoomTypePhoto)
            ucRemoteContentContainer.Src += string.Format("?show={0}", string.Join(",", roomTypesForFilter));
      }



      else if (selectedRoomRate != null && !string.IsNullOrWhiteSpace(selectedRoomRate.RatePlanCode))
      {
         HotelAvailRoomRate roomRate = (from item in objHotelRoomAvailInfo.RoomRates
                                        where item.RoomTypeCode == selectedRoomRate.RoomTypeCode && item.RatePlanCode == selectedRoomRate.RatePlanCode
                                        select item).FirstOrDefault();

         RatePlanSelectorItemControl ucSelectedRatePlanSelectorItemControl = (RatePlanSelectorItemControl)LoadControl(strRatePlanSelectorItemControlPath);
         ucSelectedRatePlanSelectorItemControl.Mode = (this.bIsConfirmed ? SelectionMode.NonModifiable : SelectionMode.Selected);
         ucSelectedRatePlanSelectorItemControl.ID = string.Format("{0}_{1}_{2}_{3}"
             , "SelectedRatePlanSelectorItem"
             , ucRoomDetailSelectorControl.RoomRefID
             , selectedRoomRate.RoomTypeCode
             , RemoveSpecialCharFromString(selectedRoomRate.RatePlanCode));
         ucSelectedRatePlanSelectorItemControl.RoomRefID = selectedRoomRate.RoomRefID;
         ucSelectedRatePlanSelectorItemControl.RoomRate = roomRate;
         ucSelectedRatePlanSelectorItemControl.RoomTypeDescription = objHotelDescriptiveInfo.RoomTypes.FirstOrDefault(item => item.Code == selectedRoomRate.RoomTypeCode);
         ucSelectedRatePlanSelectorItemControl.CreditCardCodes = objHotelDescriptiveInfo.CreditCardCodes;
         ucSelectedRatePlanSelectorItemControl.RatePlan = objHotelRoomAvailInfo.RatePlans.FirstOrDefault(item => item.Code == selectedRoomRate.RatePlanCode);
         ucSelectedRatePlanSelectorItemControl.IsShowPhotoLink = !(Step == BookingSteps.Confirmed || Step == BookingSteps.GuestInfo);

         ucRoomDetailSelectorControl.AddSelectedRoomRate(ucSelectedRatePlanSelectorItemControl);
      }
   }

   private void ConfigureTempTotalCostControl(RoomDetailSelectorControl ucRoomDetailSelectorControl)
   {
      if (Step.Equals(BookingSteps.SelectRoomDetail) && CurrentRoomRefID.Equals(ucRoomDetailSelectorControl.RoomRefID))
      {
         TotalCostControl totalCostControl = (TotalCostControl)LoadControl(strTotalCostControlPath);
         totalCostControl.ID = "TempTotalCostControl";
         ucRoomDetailSelectorControl.AddTempTotalCostControl(totalCostControl);
         totalCostControl.CurrencyCode = "EUR";
         totalCostControl.Mode = SelectionMode.Edit;

         decimal totalCost = 0;
         for (int i = 0; i < phRoomDetailSelectorControl.Controls.Count; i++)
         {
            totalCost += ((RoomDetailSelectorControl)phRoomDetailSelectorControl.Controls[i]).TotalCost();

         }
         totalCostControl.TotalCost = totalCost;
      }
   }

   private void ConfigureRoomExtra(RoomDetailSelectorControl ucRoomDetailSelectorControl, HotelRoomAvailInfo objHotelRoomAvailInfo, HotelDescriptiveInfo objHotelDescriptiveInfo)
   {
      // Selected Room Rate
      var selectedRoomRate = (from rrs in RoomRateSelections
                              where rrs.RoomRefID == ucRoomDetailSelectorControl.RoomRefID
                              select rrs).FirstOrDefault();

      if (selectedRoomRate == null || string.IsNullOrWhiteSpace(selectedRoomRate.RatePlanCode))
         return;

      var roomRateInfo = (from item in objHotelRoomAvailInfo.RatePlans
                          where item.Code == selectedRoomRate.RatePlanCode
                          select item).FirstOrDefault();
      var packages = from package in roomRateInfo.Packages
                     where package.RoomTypeCode.Equals(string.Empty) || package.RoomTypeCode == selectedRoomRate.RoomTypeCode
                     select package;

      var roomOccupantSelection = StayCriteriaSelection.RoomOccupantSelections.FirstOrDefault(room => room.RoomRefID == ucRoomDetailSelectorControl.RoomRefID);
      var selectedExtras = from extra in AddOnPackageSelections where extra.RoomRefID == ucRoomDetailSelectorControl.RoomRefID select extra;

      List<string> packageForShowingInRemoteContent = new List<string>();

      //Render selected Extras
      if (selectedExtras.Count() > 0)
      {
         foreach (var extra in selectedExtras)
         {
            var localExtra = extra;
            var selectedPackage = packages.Where(pkg => pkg.Code == localExtra.PackageCode).FirstOrDefault();
            AddOnPackageSelectorItemControl ucAddOnPackageSelectorItemControl = (AddOnPackageSelectorItemControl)LoadControl(strAddOnPackageSelectorItemControlPath);
            ucAddOnPackageSelectorItemControl.ID = string.Format("{0}_{1}_{2}_selected"
                                                                 , "AddOnPackageSelectorItemControl"
                                                                 , ucRoomDetailSelectorControl.RoomRefID
                                                                 , RemoveSpecialCharFromString(selectedPackage.Code));
            ucAddOnPackageSelectorItemControl.IsItemRemovable = (!bIsConfirmed && Step != BookingSteps.GuestInfo);
            ucAddOnPackageSelectorItemControl.RoomRefID = ucRoomDetailSelectorControl.RoomRefID;
            ucAddOnPackageSelectorItemControl.NumberAdults = roomOccupantSelection.NumberAdults;
            ucAddOnPackageSelectorItemControl.NumberStayNights = (StayCriteriaSelection.DepartureDate - StayCriteriaSelection.ArrivalDate).Days;
            ucAddOnPackageSelectorItemControl.PackageDescription = objHotelDescriptiveInfo.Packages.FirstOrDefault(pkg => pkg.Code == selectedPackage.Code);
            ucAddOnPackageSelectorItemControl.PackageRate = packages.FirstOrDefault(pkg => pkg.Code == selectedPackage.Code);
            ucAddOnPackageSelectorItemControl.PackageQuantity = localExtra.Quantity;
            ucAddOnPackageSelectorItemControl.Selected = true;

            ucRoomDetailSelectorControl.AddSelectedRoomExtraItemControl(ucAddOnPackageSelectorItemControl);

         }
      }
      //Render available extras
      if (CurrentRoomRefID == ucRoomDetailSelectorControl.RoomRefID && ucRoomDetailSelectorControl.Step == RoomDetailSelectionStep.SelectExtra)
      {
         foreach (HotelAvailPackage package in packages)
         {
            var localPackage = package;

            if (!packageForShowingInRemoteContent.Contains(localPackage.Code))
               packageForShowingInRemoteContent.Add(localPackage.Code);

            if (selectedExtras.Count() > 0)
            {
               HotelAvailPackage cpPackage = package;
               var selectedAddOn = selectedExtras.Where(addOn => addOn.RoomRefID == CurrentRoomRefID && addOn.PackageCode == cpPackage.Code);
               if (selectedAddOn.Count() > 0)
                  continue;
            }

            AddOnPackageSelectorItemControl ucAddOnPackageSelectorItemControl = (AddOnPackageSelectorItemControl)LoadControl(strAddOnPackageSelectorItemControlPath);
            ucAddOnPackageSelectorItemControl.ID = string.Format("{0}_{1}_{2}_available"
                                                                 , "AddOnPackageSelectorItemControl"
                                                                 , ucRoomDetailSelectorControl.RoomRefID
                                                                 , RemoveSpecialCharFromString(package.Code));
            ucAddOnPackageSelectorItemControl.RoomRefID = ucRoomDetailSelectorControl.RoomRefID;
            ucAddOnPackageSelectorItemControl.NumberAdults = roomOccupantSelection.NumberAdults;
            ucAddOnPackageSelectorItemControl.NumberStayNights = (StayCriteriaSelection.DepartureDate - StayCriteriaSelection.ArrivalDate).Days;
            ucAddOnPackageSelectorItemControl.PackageDescription = objHotelDescriptiveInfo.Packages.FirstOrDefault(pkg => pkg.Code == localPackage.Code);
            ucAddOnPackageSelectorItemControl.PackageRate = packages.FirstOrDefault(pkg => pkg.Code == localPackage.Code);
            ucAddOnPackageSelectorItemControl.PackageQuantity = 0;
            ucAddOnPackageSelectorItemControl.Selected = false;

            ucRoomDetailSelectorControl.AddRoomExtraItemControl(ucAddOnPackageSelectorItemControl);
         }
      }
      if (Step == BookingSteps.SelectRoomDetail && CurrentRoomDetailStep == RoomDetailSelectionStep.SelectExtra && !bShowRoomTypePhoto)
         ucRemoteContentContainer.Src += string.Format("?show={0}", string.Join(",", packageForShowingInRemoteContent));
   }

   #endregion

   private void ConfigureTotalCostControl()
   {
      if (ucTotalCostControl == null)
         return;

      if (Step == BookingSteps.SelectRoomDetail)
      {
         ucTotalCostControl.Mode = SelectionMode.Edit;
         ucTotalCostControl.IsBookingConfirmed = false;
      }
      else if (Step == BookingSteps.BookingSummary || Step == BookingSteps.GuestInfo)
      {
         ucTotalCostControl.Mode = SelectionMode.Selected;
         ucTotalCostControl.IsBookingConfirmed = false;
      }
      else if (Step >= BookingSteps.Confirmed)
      {
         ucTotalCostControl.Mode = SelectionMode.Selected;
         ucTotalCostControl.IsBookingConfirmed = true;
      }
      else
         ucTotalCostControl.Mode = SelectionMode.Hidden;

      if (Step >= BookingSteps.SelectRoomDetail)
      {
         decimal totalCost = 0;
         for (int i = 0; i < phRoomDetailSelectorControl.Controls.Count; i++)
         {
            totalCost += ((RoomDetailSelectorControl)phRoomDetailSelectorControl.Controls[i]).TotalCost();

         }
         ucTotalCostControl.TotalCost = totalCost;
         ucTotalCostControl.CurrencyCode = "EUR";
      }

   }

   private void ConfigureGuestDetailsEntryControl()
   {
      bool bBookingTermsConditionsAccepted = (bool)Session["BookingTermsConditionsAccepted"];

      HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

      if (HotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
      {
         objHotelDescriptiveInfo = HotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
      }

      PaymentCardApplication enumPaymentCardApplicationStatus = WBSPGHelper.GetPaymentCardApplicationStatus(HotelBookingPaymentAllocations);

      decimal decTotalPaymentCardPayment = WBSPGHelper.GetTotalPaymentCardPayment(HotelBookingPaymentAllocations);

      List<string> lPaymentCardCodes = new List<string>();

      string[] objPaymentCardCodes = objHotelDescriptiveInfo.CreditCardCodes;

      // retrieve supported payment methods.
      PaymentGatewayInfos = WBSPGHelper.GetPaymentGatewayInfos(StayCriteriaSelection.HotelCode);
      // Calculate deposit ammount
      HotelBookingPaymentAllocations = WBSPGHelper.GetPaymentAllocations(HotelPricingHelper.GetHotelPricing(StayCriteriaSelection, RoomRateSelections, AddOnPackageSelections, HotelAvailabilityRS.HotelRoomAvailInfos, CurrencyCode));

      if (PaymentGatewayInfos.Length == 1)
         Session[Constants.Sessions.PaymentGatewayInfo] = PaymentGatewayInfos[0];
      else
         Session[Constants.Sessions.PaymentGatewayInfo] = null; // if multiple gateways configured, payment gateway must be selected by method of payment

      for (int i = 0; i < objPaymentCardCodes.Length; i++) // mask out cards with no name lookup
      {
         if ((String)GetGlobalResourceObject("SiteResources", "CardType" + objPaymentCardCodes[i]) != null && (String)GetGlobalResourceObject("SiteResources", "CardType" + objPaymentCardCodes[i]) != "")
            lPaymentCardCodes.Add(objPaymentCardCodes[i]);
      }

      objPaymentCardCodes = lPaymentCardCodes.ToArray();

      if (PaymentGatewayInfos != null && PaymentGatewayInfos.Length != 0 && decTotalPaymentCardPayment != 0 && ConfigurationManager.AppSettings["EnterGuestDetailsPage.PaymentGatewayAcceptedCardsOnly"] == "1")
         objPaymentCardCodes = WBSPGHelper.GetPaymentGatewayAcceptedCardTypes(PaymentGatewayInfos);

      bool bPaymentGatewayPreSelectRequired = false;

      if (PaymentGatewayInfo == null && WBSPGHelper.IsPaymentGatewayPreSelectRequired(PaymentGatewayInfos, HotelBookingPaymentAllocations))
         bPaymentGatewayPreSelectRequired = true;

      XHS.WBSUIBizObjects.Profile objProfile = WbsUiHelper.GetLoginLinkedProfile();

      bool bDisplayProfileGuarantee = false;

      if (objProfile != null && objProfile.PermitProfileGuarantee && (enumPaymentCardApplicationStatus == PaymentCardApplication.GuaranteeOnly || enumPaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit))
      {
         bDisplayProfileGuarantee = true;
      }

      ucGuestDetailsEntryControl.HotelDescriptiveInfo = objHotelDescriptiveInfo;
      ucGuestDetailsEntryControl.GuestDetailsEntryInfo = GuestDetailsEntryInfo;
      ucGuestDetailsEntryControl.MembershipPrograms = WbsUiHelper.GetMembershipPrograms(StayCriteriaSelection.HotelCode);
      ucGuestDetailsEntryControl.PaymentCardCodes = objPaymentCardCodes;
      ucGuestDetailsEntryControl.TermsConditionsAccepted = bBookingTermsConditionsAccepted;
      ucGuestDetailsEntryControl.PaymentGatewayInfos = PaymentGatewayInfos;
      ucGuestDetailsEntryControl.PaymentGatewayPreSelectRequired = bPaymentGatewayPreSelectRequired;
      ucGuestDetailsEntryControl.SelectedPaymentGateway = PaymentGatewayInfo;
      ucGuestDetailsEntryControl.PaymentCardApplicationStatus = enumPaymentCardApplicationStatus;
      ucGuestDetailsEntryControl.PaymentCardDepositAmount = decTotalPaymentCardPayment;
      ucGuestDetailsEntryControl.PaymentCardDepositCurrencyCode = objHotelDescriptiveInfo.CurrencyCode;
      ucGuestDetailsEntryControl.DisplayProfileGuarantee = bDisplayProfileGuarantee;

      ucGuestDetailsEntryControl.IsControlConfigured = true;

      return;
   }

   private void ConfigureConfirmationControl()
   {
      if (Step < BookingSteps.Confirmed)
         return;

      OnlinePaymentReceipt objOnlinePaymentReceipt = null;
      decimal decTotalPaymentCardPayment = WBSPGHelper.GetTotalPaymentCardPayment(HotelBookingPaymentAllocations);

      if (PaymentGatewayInfo != null && decTotalPaymentCardPayment != 0 && HotelPaymentRS != null)
      {
         objOnlinePaymentReceipt = new OnlinePaymentReceipt
                                       {
                                          PaymentCard = HotelPaymentRS.PaymentCard,
                                          PaymentGatewayCardType = HotelPaymentRS.PaymentGatewayCardType,
                                          PaymentDateTime = TZNet.ToLocal(WbsUiHelper.GetTimeZone(StayCriteriaSelection.HotelCode), DateTime.UtcNow).Date,
                                          AuthCode = HotelPaymentRS.PaymentAuthCode,
                                          TransRefID = HotelPaymentRS.PaymentTransRefID,
                                          Amount = decTotalPaymentCardPayment,
                                          CurrencyCode = HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].CurrencyCode
                                       };
      }


      string strMasterConfirmationNumber = "";
      if (HotelBookingRS != null && HotelBookingRS.Segments != null && HotelBookingRS.Segments.Length != 0)
         strMasterConfirmationNumber = HotelBookingRS.Segments[0].MasterConfirmationNumber;

      ucConfirmationControl.ConfirmationNumber = strMasterConfirmationNumber;
      ucConfirmationControl.PaymentReceipt = objOnlinePaymentReceipt;
   }

   private void ConfigureTrackingCodeControl()
   {
      phTrackingCodeControl.Controls.Clear();

      if (
          (Step.Equals(BookingSteps.SelectHotel) && ucStayCriteriaControl.StayCriteriaSelectorMode == SelectionMode.Edit)
          || (Step.Equals(BookingSteps.SelectStayDate) && ucAvailCalSelectorControl.Mode == SelectionMode.Edit)
          || (Step.Equals(BookingSteps.SelectRoomQuantity) && ucRoomQuantitySelectorControl.Mode == SelectionMode.Edit)
          || (Step.Equals(BookingSteps.SelectRoomDetail) && !bAddonToggled && (CurrentRoomDetailStep.Equals(RoomDetailSelectionStep.SelectRoomType) || CurrentRoomDetailStep.Equals(RoomDetailSelectionStep.SelectExtra)))
          || Step == BookingSteps.GuestInfo
         || Step == BookingSteps.Confirmed)
      {

         var objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

         TrackingCodeInfo[] objTrackingCodeInfos = WbsUiHelper.GetTrackingCodeInfos(objStayCriteriaSelection.HotelCode);

         if (objTrackingCodeInfos.Length == 0)
            return;

         ucBodyTrackingCodeControl = (TrackingCodeControl)LoadControl(strTrackingCodeControlPath);
         ucBodyTrackingCodeControl.ID = "ucBodyTrackingCodeControl";
         phTrackingCodeControl.Controls.Add(ucBodyTrackingCodeControl);

         string strTrackingCodeItemControlPath = ConfigurationManager.AppSettings["TrackingCodeItemControl.ascx"];

         for (int i = 0; i < objTrackingCodeInfos.Length; i++)
         {
            if (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.AllPages
                || (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.StartPageOnly && this.IsFirstTrackingPage)
                || (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.ConfirmPageOnly && Step.Equals(BookingSteps.Confirmed)))
            {
               var ucTrackingCodeItemControl = (TrackingCodeItemControl)LoadControl(strTrackingCodeItemControlPath);

               ucBodyTrackingCodeControl.Add(ucTrackingCodeItemControl);

               ucTrackingCodeItemControl.Type = objTrackingCodeInfos[i].Type;
               ucTrackingCodeItemControl.TrackingCodeParameters = objTrackingCodeInfos[i].TrackingCodeParameters;
               ucTrackingCodeItemControl.HotelCode = objStayCriteriaSelection.HotelCode;
               ucTrackingCodeItemControl.PageUrl = TrackingCodeByStep(Step, CurrentRoomDetailStep);

               if (Step == BookingSteps.Confirmed)
               {
                  string strMasterConfirmationNumber = "";
                  if (HotelBookingRS.Segments != null && HotelBookingRS.Segments.Length != 0)
                  {
                     strMasterConfirmationNumber = HotelBookingRS.Segments[0].MasterConfirmationNumber;
                  }
                  ucTrackingCodeItemControl.ConfirmNumber = strMasterConfirmationNumber;

                  var objHotelPricings = HotelPricingHelper.GetHotelPricing(objStayCriteriaSelection, RoomRateSelections, AddOnPackageSelections,
                                                                                       HotelAvailabilityRS.HotelRoomAvailInfos, "");
                  string strCurrencyCode = "";
                  decimal decTotalBookingAmount = objHotelPricings.Sum(t => t.TotalAmount);
                  if (HotelDescriptiveInfoRS.HotelDescriptiveInfos.Length > 0)
                     strCurrencyCode = HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].CurrencyCode;
                  decTotalBookingAmount = decTotalBookingAmount * WbsUiHelper.GetCurrencyConversionFactor(strCurrencyCode);
                  ucTrackingCodeItemControl.Amount = decTotalBookingAmount;

                  ucTrackingCodeItemControl.StayNight = StayCriteriaSelection.DepartureDate.Subtract(StayCriteriaSelection.ArrivalDate).Days;
               }
               else
                  ucTrackingCodeItemControl.Amount = 0;
            }

         }

         this.IsFirstTrackingPage = false;
      }
      return;
   }

   #endregion

   #region CONTROLS EVENTS

   #region StayCriteriaControl Events

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

   void ucStayCriteriaControl_EditModeSelected(object sender, EventArgs e)
   {
      StartOver();
      RemoveAllControls();
      LoadControls();
   }

   void ucStayCriteriaControl_HotelSelected(object sender, string hotelCode)
   {
      StayCriteriaSelection.HotelCode = hotelCode;
      //StayCriteriaSelection.AreaID = "47";
      //StayCriteriaSelection.CountryCode = "AU";
      bAsyncGetHotelAvailCalendarInfo = true;
      bAsyncGetHotelDescriptiveInfo = true;
      NextStep();
   }

   #endregion

   #region AvailCalSelector Control Events

   void ucAvailCalSelectorControl_AvailCalRequested(object sender, bool bViewCalendar)
   {
      Step = BookingSteps.SelectStayDate;

      // Reset StayCriteriaSelection to default 
      var selectedHotelCode = string.Copy(StayCriteriaSelection.HotelCode);
      WbsUiHelper.InitStayCriteriaSelection();
      StayCriteriaSelection.HotelCode = selectedHotelCode;

      bAsyncGetHotelAvailCalendarInfo = true;
      RemoveAllControls();
      LoadControls();
   }

   void ucAvailCalSelectorControl_NextMonthRequested(object sender, EventArgs e)
   {
      AvailCalStartDate = AvailCalStartDate.AddMonths(1);
      bAsyncGetHotelAvailCalendarInfo = true;
      LoadControls();
   }

   void ucAvailCalSelectorControl_PrevMonthRequested(object sender, EventArgs e)
   {
      AvailCalStartDate = AvailCalStartDate.AddMonths(-1);
      bAsyncGetHotelAvailCalendarInfo = true;
      LoadControls();
   }

   void ucAvailCalSelectorControl_AvailCalCompleted(object sender, EventArgs e)
   {
      ValidateAvailCalDateSelections(ucAvailCalSelectorControl.SelectedDates);

      Session["AvailCalDateSelections"] = ucAvailCalSelectorControl.SelectedDates;

      if (!this.IsPageError)
      {


         StayCriteriaSelection.ArrivalDate = ucAvailCalSelectorControl.SelectedDates[0].Date;
         StayCriteriaSelection.DepartureDate = ucAvailCalSelectorControl.SelectedDates[ucAvailCalSelectorControl.SelectedDates.Length - 1].Date;

         NextStep();
      }

      return;
   }

   void ucAvailCalSelectorControl_StayDateSelected(object sender, DateTime selectedCheckinDate, DateTime selectedCheckoutDate)
   {
      var numNights = (selectedCheckoutDate - selectedCheckinDate).Days;
      if (numNights <= 0)
      {
         this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "EmptyStayNight"));
         return;
      }
      else if (numNights > 30)
      {
         this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "TooManyBookingDaysRequested"));
         return;
      }

      DateTime[] selectedDates = new DateTime[numNights];
      for (int i = 0; i < numNights; i++)
      {
         selectedDates[i] = selectedCheckinDate.AddDays(i);
      }

      ValidateAvailCalDateSelections(selectedDates);
      if (!this.IsPageError)
      {
         Session[Constants.Sessions.AvailCalDateSelections] = selectedDates;
         StayCriteriaSelection.ArrivalDate = selectedCheckinDate;
         StayCriteriaSelection.DepartureDate = selectedCheckoutDate;
         NextStep();
      }
      return;
   }

   #endregion

   #region RoomQuantitySelectorControl Events

   void ucRoomQuantitySelectorControl_RoomQuantityCompleted(object sender, int numberOfRoom, string promoCode)
   {
      StayCriteriaSelection.RoomOccupantSelections = new RoomOccupantSelection[numberOfRoom];
      RoomRateSelection[] roomRateSelections = new RoomRateSelection[numberOfRoom];

      for (int i = 1; i <= StayCriteriaSelection.RoomOccupantSelections.Length; i++)
      {
         StayCriteriaSelection.RoomOccupantSelections[i - 1] = new RoomOccupantSelection { NumberAdults = 2, NumberChildren = 0, NumberRooms = 1, RoomRefID = i.ToString() };
         roomRateSelections[i - 1] = new RoomRateSelection { RoomRefID = i.ToString() };
      }

      StayCriteriaSelection.PromotionCode = promoCode;
      RoomRateSelections = roomRateSelections;

      NextStep();
   }

   void ucRoomQuantitySelectorControl_EditModeSelected(object sender, EventArgs e)
   {
      Step = BookingSteps.SelectRoomQuantity;
      LoadControls();
   }

   #endregion

   #region RoomDetailSelectorControl Events

   void ucRoomDetailSelectorControl_AdultQuantityCompleted(string roomRefID, int quantity)
   {
      foreach (var item in StayCriteriaSelection.RoomOccupantSelections)
      {
         if (item.RoomRefID == roomRefID)
            item.NumberAdults = quantity;
      }

      Step = BookingSteps.SelectRoomDetail;
      CurrentRoomDetailStep = EnabledChildren ? RoomDetailSelectionStep.SelectChildrenQuantity : RoomDetailSelectionStep.SelectRoomType;
      CurrentRoomRefID = roomRefID;
      ClearSelectedAddONByRoomRef(roomRefID);
      ClearSelectedRoomRate(roomRefID);
      bAsyncGetHotelAvailInfo = (CurrentRoomDetailStep == RoomDetailSelectionStep.SelectRoomType ? true : false);
      LoadControls();
   }

   void ucRoomDetailSelectorControl_ChildrenQuantityCompleted(string roomRefID, int quantity)
   {
      foreach (var item in StayCriteriaSelection.RoomOccupantSelections)
      {
         if (item.RoomRefID == roomRefID)
            item.NumberChildren = quantity;
      }

      Step = BookingSteps.SelectRoomDetail;
      CurrentRoomDetailStep = RoomDetailSelectionStep.SelectRoomType;
      CurrentRoomRefID = roomRefID;
      ClearSelectedAddONByRoomRef(roomRefID);
      ClearSelectedRoomRate(roomRefID);
      bAsyncGetHotelAvailInfo = true;
      LoadControls();
   }

   void ucRoomDetailSelectorControl_ShowRoomPhotoSelected(string roomTypeCode)
   {
      //IEnumerable<HotelImage> stepImages = from image in HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].Images
      //                                     where image.CategoryCode == HotelImageCategoryCode.ImageColumn
      //                                           && image.ImageSize == HotelImageSize.FullSize
      //                                           && image.ContentCaption.Contains(StringEnum.GetStringValue(ImagePackageTypes.RoomTypeImage))
      //                                           && image.ContentCaption.Contains(roomTypeCode)
      //                                     select new HotelImage { ImageURL = string.Format("{0}{1}", ClientProtocol, image.ImageURL) };
      //ucImageHoldingControl.Images = stepImages.ToList();


      bShowRoomTypePhoto = true;

      ucRemoteContentContainer.Src = string.Format("{0}{1}/rates/{2}", RemoteContentContainerBasedUrl, CDNLocationMappingString, HttpUtility.UrlEncode(roomTypeCode));
   }

   void ucRoomDetailSelectorControl_RatePlanSelected(string roomRefID, string roomTypeCode, string ratePlanCode, string promotionCode)
   {
      var roomRate = (from item in RoomRateSelections where item.RoomRefID == roomRefID select item).FirstOrDefault();
      if (roomRate != null)
      {
         roomRate.RoomTypeCode = roomTypeCode;
         roomRate.RatePlanCode = ratePlanCode;
         roomRate.PromotionCode = promotionCode;
      }
      else
      {
         List<RoomRateSelection> roomRateSelectionList = RoomRateSelections.ToList();
         roomRateSelectionList.Add(new RoomRateSelection { RoomRefID = roomRefID, RatePlanCode = ratePlanCode, PromotionCode = promotionCode });
         RoomRateSelections = roomRateSelectionList.ToArray();
      }
      ClearSelectedAddONByRoomRef(roomRefID);

      // If There is no addon in the selected room rate, go to next step/next room.
      var objHotelAvailInfo = HotelAvailabilityRS.HotelRoomAvailInfos.FirstOrDefault(item => item.SegmentRefID == roomRefID);
      var ratePlan = (objHotelAvailInfo.RatePlans.Where(rp => rp.Code == ratePlanCode)).FirstOrDefault();
      var packages = ratePlan.Packages.Where(pkg => string.IsNullOrEmpty(pkg.RoomTypeCode) || pkg.RoomTypeCode == roomTypeCode).
              FirstOrDefault();
      if (packages == null)
      {
         ucRoomDetailSelectorControl_RoomRateCompleted(roomRefID);
         return;
      }

      //ucRoomDetailSelectorControl_RoomRateCompleted(null, string.Empty);
      CurrentRoomDetailStep = RoomDetailSelectionStep.SelectExtra;
      LoadControls();
      return;
   }

   void ucRoomDetailSelectorControl_EditModeSelected(string roomRefID, RoomDetailSelectionStep step)
   {
      Step = BookingSteps.SelectRoomDetail;
      CurrentRoomRefID = roomRefID;
      CurrentRoomDetailStep = step;

      if (CurrentRoomDetailStep == RoomDetailSelectionStep.SelectAdultQuantity || CurrentRoomDetailStep == RoomDetailSelectionStep.SelectChildrenQuantity
          || CurrentRoomDetailStep == RoomDetailSelectionStep.SelectRoomType)
      {
         ClearSelectedAddONByRoomRef(roomRefID);
         ClearSelectedRoomRate(roomRefID);
      }

      bAsyncGetHotelDescriptiveInfo = false;
      bAsyncGetHotelAvailInfo = false;
      LoadControls();
   }

   void ucRoomDetailSelectorControl_AddOnToggled(string roomRefID, string packageCode, int quantity, bool IsSelected)
   {
      List<AddOnPackageSelection> selectedAddOns = AddOnPackageSelections.ToList();

      if (IsSelected)
         selectedAddOns.Add(new AddOnPackageSelection { RoomRefID = roomRefID, PackageCode = packageCode, Quantity = quantity });
      else
      {
         AddOnPackageSelection selectedAddOn = (from addOn in selectedAddOns
                                                where addOn.RoomRefID == roomRefID && addOn.PackageCode == packageCode
                                                select addOn).FirstOrDefault();
         selectedAddOns.Remove(selectedAddOn);
      }

      AddOnPackageSelections = selectedAddOns.ToArray();
      //This prevent tracking control is hit when user add/remove package.
      bAddonToggled = true;
      LoadControls();

      //if (IsSelected)
      //{
      //    bShowRoomTypePhoto = true;

      //    ucRemoteContentContainer.Src = string.Format("{0}{1}/extras/?show={2}", RemoteContentContainerBasedUrl, CDNLocationMappingString, HttpUtility.UrlEncode(packageCode));
      //}
      //else
      //    bShowRoomTypePhoto = false;
   }

   void ucRoomDetailSelectorControl_RoomRateCompleted(string roomRefID)
   {
      bool isAllRoomAssigned = true;
      if (RoomRateSelections == null)
         return;

      foreach (var item in RoomRateSelections)
      {
         if (string.IsNullOrWhiteSpace(item.RatePlanCode))
         {
            CurrentRoomRefID = item.RoomRefID;
            CurrentRoomDetailStep = RoomDetailSelectionStep.SelectAdultQuantity;
            isAllRoomAssigned = false;
            break;
         }
      }
      if (isAllRoomAssigned)
         NextStep();
      else
         LoadControls();
   }

   #endregion

   void ucTotalCostControl_Completed(bool isHold)
   {
      Step = BookingSteps.GuestInfo;

      Session["HotelPaymentRQ"] = null;
      Session["HotelPaymentRS"] = null;

      LoadControls();
   }

   void ucGuestDetailsEntryControl_GuestDetailsCompleted(object sender, EventArgs e)
   {
      PaymentGatewayInfo[] objPaymentGatewayInfos = (PaymentGatewayInfo[])Session[Constants.Sessions.PaymentGatewayInfos];
      HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations = (HotelBookingPaymentAllocation[])Session[Constants.Sessions.HotelBookingPaymentAllocations];

      this.ValidateGuestDetails(ucGuestDetailsEntryControl.GuestDetailsEntryInfo, ucGuestDetailsEntryControl.TermsConditionsAccepted);

      Session["GuestDetailsEntryInfo"] = ucGuestDetailsEntryControl.GuestDetailsEntryInfo;
      Session["BookingTermsConditionsAccepted"] = ucGuestDetailsEntryControl.TermsConditionsAccepted;
      Session[Constants.Sessions.PaymentGatewayInfo] = ucGuestDetailsEntryControl.SelectedPaymentGateway;

      if (!this.IsPageError)
      {
         if (WBSPGHelper.IsOnlinePayment(PaymentGatewayInfos, HotelBookingPaymentAllocations, ucGuestDetailsEntryControl.GuestDetailsEntryInfo.PaymentCardType))
         {
            Step = BookingSteps.ProcessPayment;
            Server.Transfer("~/Pages/SendPaymentRQ.aspx");
         }

         else
            BookRoom();
      }

      return;
   }

   #endregion

   #region Validation methods

   private void ValidateAvailCalDateSelections(DateTime[] objAvailCalDateSelections)
   {
      if (objAvailCalDateSelections == null || objAvailCalDateSelections.Length == 0)
      {
         AddPageError(PageErrorType.ValidationError,
                           (String)GetGlobalResourceObject("SiteErrorMessages", "NoAvailCalDateSelections"));
         return;
      }

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

   private void ValidateGuestDetails(GuestDetailsEntryInfo objGuestDetailsEntryInfo, bool bBookingTermsConditionsAccepted)
   {
      PaymentGatewayInfo objPaymentGatewayInfo = (PaymentGatewayInfo)Session[Constants.Sessions.PaymentGatewayInfo];

      DateTime dtNow = TZNet.ToLocal(WbsUiHelper.GetTimeZone(StayCriteriaSelection.HotelCode), DateTime.UtcNow).Date;

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

      if (!string.IsNullOrEmpty(objGuestDetailsEntryInfo.ArrivalTime))
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

      if (this.WbsUiHelper.IsCreditCardInfoRequired(HotelAvailabilityRS, RoomRateSelections, objPaymentGatewayInfo, objGuestDetailsEntryInfo.ProfileGuaranteeRequested))
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

   #endregion

   private string TrackingCodeByStep(BookingSteps step, RoomDetailSelectionStep detailStep)
   {
      string currentLanguageCode = WbsUiHelper.SelectedUICulture;
      string trackingCode = "/" + currentLanguageCode;

      if (step.Equals(BookingSteps.SelectHotel))
         return trackingCode + "/book/SelectHotel";

      if (step.Equals(BookingSteps.SelectStayDate))
         return trackingCode + "/book/SelectCheckinDate";

      if (step.Equals(BookingSteps.SelectRoomQuantity))
         return trackingCode + "/book/SelectRoomQuantity";

      if (step.Equals(BookingSteps.SelectRoomDetail) && detailStep.Equals(RoomDetailSelectionStep.SelectRoomType))
         return trackingCode + "/book/SelectRoom";

      if (step.Equals(BookingSteps.SelectRoomDetail) && detailStep.Equals(RoomDetailSelectionStep.SelectExtra))
         return trackingCode + "/book/SelectExtras";

      if (step.Equals(BookingSteps.GuestInfo))
         return trackingCode + "/book/EnterDetails";

      if (step == BookingSteps.Confirmed)
         return trackingCode + "/book/Confirmation";

      return "";
   }

   private static string RemoveSpecialCharFromString(string roomTypeCode)
   {
      const string regexPattern = "[(!\"#$%&'()*+,./:;?@[\\]^`{|}~)]";
      Regex regex = new Regex(regexPattern);
      MatchCollection matches = regex.Matches(roomTypeCode);
      foreach (Match match in matches)
      {
         roomTypeCode = roomTypeCode.Replace(match.Value, string.Empty);
      }
      roomTypeCode = roomTypeCode.Replace(" ", "");

      return roomTypeCode;
   }

   private void GetImageFromHotelDescriptive()
   {
      ColumnImages[ImagePackageTypes.SelectHotel] = new List<HotelImage> { new HotelImage { ImageURL = "~/css/MamaShelter/images/mama_image_column_default.jpg" } };

      if (HotelDescriptiveInfoRS == null
          || HotelDescriptiveInfoRS.HotelDescriptiveInfos == null
          || HotelDescriptiveInfoRS.HotelDescriptiveInfos.Count() <= 0)
         return;

      var selectStayDateImages = from image in HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].Images
                                 where image.CategoryCode == HotelImageCategoryCode.ImageColumn
                                          && image.ImageSize == HotelImageSize.FullSize
                                          && image.ContentCaption.Contains(StringEnum.GetStringValue(ImagePackageTypes.SelectStayDate))
                                 select new HotelImage { ImageURL = string.Format("{0}{1}", ClientProtocol, image.ImageURL) };
      ColumnImages[ImagePackageTypes.SelectStayDate] = selectStayDateImages.ToList();

      var selectPeopleQuantityImages = from image in HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].Images
                                       where image.CategoryCode == HotelImageCategoryCode.ImageColumn
                                           && image.ImageSize == HotelImageSize.FullSize
                                           && image.ContentCaption.Contains(StringEnum.GetStringValue(ImagePackageTypes.SelectPeopleQuantity))
                                       select new HotelImage { ImageURL = string.Format("{0}{1}", ClientProtocol, image.ImageURL) };
      ColumnImages[ImagePackageTypes.SelectPeopleQuantity] = selectPeopleQuantityImages.ToList();

      var selectRatePlanImages = from image in HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].Images
                                 where image.CategoryCode == HotelImageCategoryCode.ImageColumn
                                     && image.ImageSize == HotelImageSize.FullSize
                                     && image.ContentCaption.Contains(StringEnum.GetStringValue(ImagePackageTypes.SelectRoomRate))
                                 select new HotelImage { ImageURL = string.Format("{0}{1}", ClientProtocol, image.ImageURL) };
      ColumnImages[ImagePackageTypes.SelectRoomRate] = selectRatePlanImages.ToList();

      var selectAddOnImages = from image in HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].Images
                              where image.CategoryCode == HotelImageCategoryCode.ImageColumn
                                  && image.ImageSize == HotelImageSize.FullSize
                                  && image.ContentCaption.Contains(StringEnum.GetStringValue(ImagePackageTypes.SelectRoomAddon))
                              select new HotelImage { ImageURL = string.Format("{0}{1}", ClientProtocol, image.ImageURL) };
      ColumnImages[ImagePackageTypes.SelectRoomAddon] = selectAddOnImages.ToList();

      var summaryImages = from image in HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].Images
                          where image.CategoryCode == HotelImageCategoryCode.ImageColumn
                              && image.ImageSize == HotelImageSize.FullSize
                              && image.ContentCaption.Contains(StringEnum.GetStringValue(ImagePackageTypes.BookingSummary))
                          select new HotelImage { ImageURL = string.Format("{0}{1}", ClientProtocol, image.ImageURL) };
      ColumnImages[ImagePackageTypes.BookingSummary] = summaryImages.ToList();

      var confirmedImages = from image in HotelDescriptiveInfoRS.HotelDescriptiveInfos[0].Images
                            where image.CategoryCode == HotelImageCategoryCode.ImageColumn
                                && image.ImageSize == HotelImageSize.FullSize
                                && image.ContentCaption.Contains(StringEnum.GetStringValue(ImagePackageTypes.Confirmed))
                            select new HotelImage { ImageURL = string.Format("{0}{1}", ClientProtocol, image.ImageURL) };
      ColumnImages[ImagePackageTypes.Confirmed] = confirmedImages.ToList();
   }

   private List<HotelImage> GetImage(ImagePackageTypes type)
   {
      if ((int)type == 5)
         return GetImage((type - 1));
      if ((int)type == 1)
         return ColumnImages[type];

      List<HotelImage> result = ColumnImages[type].Count <= 0 ? GetImage((type - 1)) : ColumnImages[type];
      return result;
   }

   private void BookRoom()
   {
      bAsyncBookHotel = true;
      if (WBSPGHelper.IsOnlinePayment(PaymentGatewayInfos, HotelBookingPaymentAllocations, GuestDetailsEntryInfo.PaymentCardType))
      {
         bPrepayBooking = true;

         HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

         if (HotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
         {
            objHotelDescriptiveInfo = HotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
         }

         HotelPaymentRQ objHotelPaymentRQ = (HotelPaymentRQ)Session["HotelPaymentRQ"];
         HotelPaymentRS objHotelPaymentRS = (HotelPaymentRS)Session["HotelPaymentRS"];

         string strFailedPrepayBookingID = Guid.NewGuid().ToString();

         FailedPrepayBookingInfo objFailedPrepayBookingInfo = new FailedPrepayBookingInfo
                                                                  {
                                                                     PostingDateTime = DateTime.Now,
                                                                     HotelName = objHotelDescriptiveInfo.HotelName,
                                                                     StayCriteria = StayCriteriaSelection,
                                                                     RoomRates = RoomRateSelections,
                                                                     AddOnPackages = AddOnPackageSelections,
                                                                     GuestDetails = GuestDetailsEntryInfo,
                                                                     PaymentRequestInfo = objHotelPaymentRQ,
                                                                     PaymentResponseInfo = objHotelPaymentRS
                                                                  };

         Session["FailedPrepayBookingID"] = strFailedPrepayBookingID;
         Session["FailedPrepayBookingInfo"] = objFailedPrepayBookingInfo;

         string strFailedPrepayBookingInfo = XHS.WBSUIBizObjects.Serializer.ToString(objFailedPrepayBookingInfo);

         this.WbsMonitor.AddItem(strFailedPrepayBookingID, strFailedPrepayBookingInfo);
      }
      PageAsyncTask task = new PageAsyncTask(BeginAsyncOperation, EndAsyncOperation, TimeoutAsyncOperation, null);
      RegisterAsyncTask(task);
   }

   private void NextStep()
   {
      if (Step == BookingSteps.SelectHotel)
      {
         Step = BookingSteps.SelectStayDate;
         StayCriteriaSelection.PromotionCode = string.Empty;
      }
      else if (Step == BookingSteps.SelectStayDate)
         Step = BookingSteps.SelectRoomQuantity;
      else if (Step == BookingSteps.SelectRoomQuantity)
      {
         Step = BookingSteps.SelectRoomDetail;
         CurrentRoomDetailStep = RoomDetailSelectionStep.SelectAdultQuantity;
         CurrentRoomRefID = RoomRateSelections[0].RoomRefID;
         AddOnPackageSelections = new AddOnPackageSelection[0];
      }
      else if (Step == BookingSteps.SelectRoomDetail)
         Step = BookingSteps.BookingSummary;
      else if (Step == BookingSteps.BookingSummary)
         Step = BookingSteps.GuestInfo;
      else if (Step == BookingSteps.GuestInfo)
         Step = BookingSteps.ProcessPayment;
      else if (Step == BookingSteps.ProcessPayment)
         Step = BookingSteps.Confirmed;
      else if (Step == BookingSteps.Confirmed)
         Step = BookingSteps.ViewConfirmation;
      else if (Step == BookingSteps.ViewConfirmation)
         Step = BookingSteps.SelectHotel;

      LoadControls();
   }

   private void RemoveAllControls()
   {
      phAvailCalSelectorControl.Controls.Clear();
      phImageHoldingControl.Controls.Clear();
      phRemoteContentContainerControl.Controls.Clear();
      phStayCriteriaControl.Controls.Clear();
      phRoomQuantitySelectorControl.Controls.Clear();
      phRoomDetailSelectorControl.Controls.Clear();
      phTotalCostControl.Controls.Clear();
      phGuestDetailsEntryControl.Controls.Clear();
   }

   private void StartOver()
   {
      Step = BookingSteps.SelectHotel;
      CurrentRoomDetailStep = RoomDetailSelectionStep.SelectAdultQuantity;
      CurrentRoomRefID = string.Empty;
      Session[Constants.Sessions.HotelDescriptiveInfoRS] = null;
      Session[Constants.Sessions.HotelAvailabilityRS] = null;
      WbsUiHelper.InitStayCriteriaSelection();
      WbsUiHelper.InitRoomRateSelections();
      WbsUiHelper.InitAddOnPackageSelections();
      WbsUiHelper.InitGuestDetailsEntryInfo();
      Session[Constants.Sessions.HotelAvailabilityCalendarRS] = null;
   }

   private void ClearSelectedRoomRate(string roomRefID)
   {
      for (int i = 0; i < RoomRateSelections.Length; i++)
      {
         if (RoomRateSelections[i].RoomRefID != null && RoomRateSelections[i].RoomRefID.Equals(roomRefID))
         {
            RoomRateSelections[i].RatePlanCode = string.Empty;
            RoomRateSelections[i].RoomTypeCode = string.Empty;

            break;
         }
      }
   }

   private void ClearSelectedAddONByRoomRef(string roomRefID)
   {
      List<AddOnPackageSelection> selectedAddOns = AddOnPackageSelections.ToList();
      AddOnPackageSelection selectedAddOn = (selectedAddOns.Where(addOn => addOn.RoomRefID == roomRefID)).FirstOrDefault();
      if (selectedAddOn != null)
      {
         selectedAddOns.Remove(selectedAddOn);
      }
      AddOnPackageSelections = selectedAddOns.ToArray();
   }

   private void LoadControls()
   {
      RemoveAllControls();

      LoadErrorDisplayControl();
      LoadStaySelectorControl();

      //LoadImageHoldingControl();
      if (!Step.Equals(BookingSteps.GuestInfo))
         LoadRemoteContentContainerControl();

      if (Step >= BookingSteps.SelectStayDate)
         LoadAvailCalSelectorControl();
      if (Step >= BookingSteps.SelectRoomQuantity)
         LoadRoomQuantitySelectorControl();
      if (Step >= BookingSteps.SelectRoomDetail)
      {
         LoadRoomDetailSelectorControl();
         LoadTotalCostControl();
      }
      //if (Step >= BookingSteps.BookingSummary)
      //    LoadTotalCostControl();
      if (Step == BookingSteps.GuestInfo)
         LoadGuestDetailsEntryControl();
      if (Step >= BookingSteps.Confirmed)
         LoadConfirmationControl();
   }

   private void ConfiguresControls()
   {
      ConfigureErrorDisplayControl();
      if (Step >= BookingSteps.SelectHotel)
         ConfigureStaySelectorControl();

      //ConfigureImageHoldingControl();

      if (!Step.Equals(BookingSteps.GuestInfo))
         ConfigureRemoteContentContainerControl();

      if (Step >= BookingSteps.SelectStayDate)
         ConfigureAvailCalSelectorControl();

      if (Step >= BookingSteps.SelectRoomQuantity)
         ConfigureRoomQuantitySelectorControl();

      if (Step >= BookingSteps.SelectRoomDetail)
      {
         ConfigureRoomDetailSelectorControl();
         ConfigureTotalCostControl();
      }

      //if (Step >= BookingSteps.BookingSummary)
      //    ConfigureTotalCostControl();
      if (Step == BookingSteps.GuestInfo)
         ConfigureGuestDetailsEntryControl();
      if (Step >= BookingSteps.Confirmed)
         ConfigureConfirmationControl();

      ConfigureTrackingCodeControl();
   }

   private void RenderControls()
   {
      ucErrorDisplayControl.RenderUserControl();
      ucStayCriteriaControl.RenderUserControl();
      //ucImageHoldingControl.RenderUserControl();

      if (ucBodyTrackingCodeControl != null)
         ucBodyTrackingCodeControl.RenderUserControl();

      if (!Step.Equals(BookingSteps.GuestInfo))
         ucRemoteContentContainer.RenderUserControl();

      if (Step >= BookingSteps.SelectStayDate)
         ucAvailCalSelectorControl.RenderUserControl();
      if (Step >= BookingSteps.SelectRoomQuantity)
         ucRoomQuantitySelectorControl.RenderUserControl();
      if (Step >= BookingSteps.SelectRoomDetail)
      {
         foreach (RoomDetailSelectorControl control in phRoomDetailSelectorControl.Controls)
         {
            control.RenderUserControls();
         }
      }
      if (Step >= BookingSteps.BookingSummary)
         ucTotalCostControl.RenderUserControl();
      if (Step == BookingSteps.GuestInfo)
         ucGuestDetailsEntryControl.RenderUserControl();
      if (Step >= BookingSteps.Confirmed)
      {
         ucConfirmationControl.RenderUserControl();
         RenderPaymentReceiptControl();
      }
   }

   private void RenderPaymentReceiptControl()
   {
      phPaymentReceiptControl.Controls.Clear();

      if (ucConfirmationControl == null || ucConfirmationControl.PaymentReceipt == null)
         return;


      string strPaymentReceiptControlPath = ConfigurationManager.AppSettings["PaymentReceiptControl.ascx"];
      PaymentReceiptControl ucPaymentReceiptControl = (PaymentReceiptControl)LoadControl(strPaymentReceiptControlPath);
      ucPaymentReceiptControl.ID = "PaymentReceiptControl";
      phPaymentReceiptControl.Controls.Add(ucPaymentReceiptControl);
      ucPaymentReceiptControl.PaymentReceipt = ucConfirmationControl.PaymentReceipt;
      ucPaymentReceiptControl.RenderUserControl();
   }

   private void RegisterClientVariables()
   {
      string stage = string.Empty;
      var scrollTop = false;

      if (Step == BookingSteps.SelectHotel)
         stage = "SelectHotel";
      else if (Step == BookingSteps.SelectRoomDetail && CurrentRoomDetailStep == RoomDetailSelectionStep.SelectRoomType)
         stage = "SelectRoom";
      else if (Step == BookingSteps.SelectRoomDetail && CurrentRoomDetailStep == RoomDetailSelectionStep.SelectExtra)
         stage = "SelectExtras";
      else if (Step == BookingSteps.GuestInfo)
      {
         stage = "EnterDetails";
         scrollTop = true;
      }
      else if (Step == BookingSteps.Confirmed)
         stage = "Confirmation";

      ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Default", string.Format("__CurrentStage = \"{0}\"; __ScrollTop = {1};", stage, scrollTop.ToString().ToLower()), true);
   }
}