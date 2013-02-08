using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using XHS.Logging;
using XHS.WBSUIBizObjects;
using Mobi.Mtld.DA;
using MamaShelter;

public partial class DeepLink : XnGR_WBS_Page
{
   private bool bAsyncGetLoginProfile;
   private bool bAsyncGetLinkedProfile;
   private bool bAsyncGetHotelSearchPropertyList;

   bool bAreaIDSpecified;
   bool bHotelCodeSpecified;
   bool bArrivalDateSpecified;
   bool bDepartureDateSpecified;
   bool bNumberRoomsSpecified;
   bool[] bAdultsSpecified = { false, false, false, false, false, false, false, false };
   bool[] bChildrenSpecified = { false, false, false, false, false, false, false, false };

   string strAreaID = "";
   string strHotelCode = "";
   DateTime dtArrivalDate;
   DateTime dtDepartureDate;
   string strPromoCode = "";
   int intNumberRooms = 0;
   int[] intAdults = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
   int[] intChildren = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

   private string strLinkedProfileIdentifier;

   protected override void Page_Init(object sender, EventArgs e)
   {
      if (Session[Constants.Sessions.CurrentBookingStep] != null && ((BookingSteps) Session[Constants.Sessions.CurrentBookingStep]) > BookingSteps.SelectHotel 
         && (string.IsNullOrWhiteSpace(Request.QueryString["is"]) || Request.QueryString["is"] == "0")
         && (string.IsNullOrWhiteSpace(Request.QueryString[Constants.DeeplinkParams.Command])
               || (!string.IsNullOrWhiteSpace(Request.QueryString[Constants.DeeplinkParams.Command]) && Request.QueryString[Constants.DeeplinkParams.Command] != Constants.DeeplinkParams.CmdCancelReservation)))
      {
         string requestUrl = Request.Url.PathAndQuery;
         string queryString = requestUrl.Substring(requestUrl.IndexOf("?", System.StringComparison.Ordinal));
         Response.Redirect("DeepLinkPreProcess.aspx" + queryString);
      }

      ((FileLog) Application.Get("EventLog")).Write(Session.SessionID);

      DeviceAtlasData objDeviceAtlasData = DeviceAtlasData.GetDeviceAtlasData(Context.Cache, (FileLog)Application["EventLog"], (ExceptionLog)Application["ExceptionEventLog"]);

      if (objDeviceAtlasData != null && objDeviceAtlasData.Tree != null)
      {
         Hashtable objDAProperties = Api.GetProperties(objDeviceAtlasData.Tree, Request.Headers["User-Agent"]);

         if (objDAProperties.ContainsKey("mobileDevice"))
         {
            if ((string)objDAProperties["mobileDevice"] == "1")
            {
               // Mobile device detected -- redirect to mobile site with query string parameters if mobile site available

               string strMobileSiteDeepLinkUrl = ConfigurationManager.AppSettings["MobileSiteDeepLinkUrl"];

               if (!string.IsNullOrWhiteSpace(strMobileSiteDeepLinkUrl))
               {
                  strMobileSiteDeepLinkUrl = strMobileSiteDeepLinkUrl + "?" + Request.QueryString;

                  Session.Abandon();
                  Response.Redirect(strMobileSiteDeepLinkUrl);
                  return;
               }

            }

         }

      }

      this.IsNewSessionOverride = true;

      base.Page_Init(sender, e);

      if (this.IsServiceTimeThresholdExceeded)
      {
         this.WbsPerfCounters.IncPerfCounter(WBSPerfCounters.REQUEST_TURNDOWNS);
         this.WbsPerfCounters.IncPerfCounter(WBSPerfCounters.TOTAL_REQUEST_TURNDOWNS);

         Session.Abandon();
         Response.Redirect("~/SystemBusy.htm");
      }

      return;
   }

   protected override void Page_Load(object sender, EventArgs e)
   {
      base.Page_Load(sender, e);

      WbsUiHelper.InitProfileLoginInfo();
      WbsUiHelper.InitStayCriteriaSelection();
      WbsUiHelper.InitRoomRateSelections();
      WbsUiHelper.InitAddOnPackageSelections();
      WbsUiHelper.InitGuestDetailsEntryInfo();
      WbsUiHelper.InitCancelDetailsEntryInfo();
      //Init Session Step
      Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectHotel;

      if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.Account)))
      {
         bAsyncGetLoginProfile = true;
         bAsyncGetLinkedProfile = false;
      }

      if (!string.IsNullOrWhiteSpace(Request.QueryString[Constants.DeeplinkParams.Command]) &&
          Request.QueryString.Get(Constants.DeeplinkParams.Command).Equals(Constants.DeeplinkParams.CmdStayCriteria, StringComparison.InvariantCultureIgnoreCase))
      {
         bAsyncGetHotelSearchPropertyList = true;
      }

      PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
      RegisterAsyncTask(task);

      return;
   }

   public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
   {
      wbsIISAsyncResult = new WBSAsyncResult(cb, state);

      if (bAsyncGetLoginProfile || bAsyncGetLinkedProfile || bAsyncGetHotelSearchPropertyList)
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
      else if (bAsyncGetLoginProfile)
      {
         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitLoginProfileRQ(ref wbsAPIRouterData, Request.QueryString[Constants.DeeplinkParams.Account]);
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

   private void LoginProfileComplete(IAsyncResult ar)
   {
      if (this.WbsApiRouterHelper.ProcessLoginProfileRS(ref wbsAPIRouterData))
      {
         ProfileReadRS objLoginProfileReadRS = (ProfileReadRS)Session[Constants.Sessions.LoginProfileReadRS];
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
      // Language selection

      if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.Language)))
      {
         LanguageSetup[] objLanguages = WbsUiHelper.GetLanguageSetups();

         for (int i = 0; i < objLanguages.Length; i++)
         {
            if (objLanguages[i].UICulture == Request.QueryString.Get(Constants.DeeplinkParams.Language))
            {
               WbsUiHelper.SelectedCulture = objLanguages[i].Culture;
               WbsUiHelper.SelectedUICulture = objLanguages[i].UICulture;
               break;
            }

         }

      }

      // Profile login account selection

      if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.Account)))
      {
         List<Profile> lProfiles = new List<Profile>();

         ProfileReadRS objLoginProfileReadRS = (ProfileReadRS)Session[Constants.Sessions.LoginProfileReadRS];

         if (objLoginProfileReadRS.ResponseHeader.Success)
         {
            lProfiles.Add(objLoginProfileReadRS.Profile);

            ProfileIdentifier objLinkedProfileIdentifier = ProfileHelper.GetProfileIdentifier(objLoginProfileReadRS.Profile, ProfileIdentifierType.LinkedProfileID);

            if (objLinkedProfileIdentifier != null)
            {
               ProfileReadRS objLinkedProfileReadRS = (ProfileReadRS)Session[Constants.Sessions.LinkedProfileReadRS];

               if (objLinkedProfileReadRS.ResponseHeader.Success)
               {
                  lProfiles.Add(objLinkedProfileReadRS.Profile);

                  Session[Constants.Sessions.LoginProfiles] = lProfiles.ToArray();
                  Session[Constants.Sessions.IsLoggedIn] = true;
               }

            }

         }

      }

      // Booking stay criteria command

      if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.Command))
          && Request.QueryString.Get(Constants.DeeplinkParams.Command).Equals(Constants.DeeplinkParams.CmdStayCriteria, StringComparison.InvariantCultureIgnoreCase))
      {
         if (Request.QueryString[Constants.DeeplinkParams.DefaultStayCriteria] == "1")
         {
            DateTime dtCurrentLocal = TZNet.ToLocal(WebconfigHelper.DefaultTimeZone, DateTime.UtcNow).Date;
            dtArrivalDate = dtCurrentLocal.Date;
            dtDepartureDate = dtCurrentLocal.AddDays(1).Date;

            intNumberRooms = 1;

            intAdults[0] = 2;
            intChildren[0] = 0;

            bArrivalDateSpecified = true;
            bDepartureDateSpecified = true;
            bNumberRoomsSpecified = true;
            bAdultsSpecified[0] = true;
            bChildrenSpecified[0] = true;
         }

         if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.AreaCode)))
         {
            strAreaID = Request.QueryString.Get(Constants.DeeplinkParams.AreaCode);
            bAreaIDSpecified = true;
         }

         if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.HotelCode)))
         {
            strHotelCode = Request.QueryString.Get(Constants.DeeplinkParams.HotelCode);
            bHotelCodeSpecified = true;
         }

         if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.ArrivalDate)))
         {
            if (DateTime.TryParse(Request.QueryString.Get(Constants.DeeplinkParams.ArrivalDate), out dtArrivalDate))
               bArrivalDateSpecified = true;
         }

         if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.DepartureDate)))
         {
            if (DateTime.TryParse(Request.QueryString.Get(Constants.DeeplinkParams.DepartureDate), out dtDepartureDate))
               bDepartureDateSpecified = true;
         }

         if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.PromotionCode)))
         {
            strPromoCode = Request.QueryString.Get(Constants.DeeplinkParams.PromotionCode);
         }

         if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.NumberOfRoom)))
         {
            if (Int32.TryParse(Request.QueryString.Get(Constants.DeeplinkParams.NumberOfRoom), out intNumberRooms))
               bNumberRoomsSpecified = true;
         }

         for (int i = 0; i < 8; i++)
         {
            if (!string.IsNullOrWhiteSpace(Request.QueryString[string.Format(Constants.DeeplinkParams.NumberOfAdult, i + 1)]))
            {
               if (Int32.TryParse(Request.QueryString[string.Format(Constants.DeeplinkParams.NumberOfAdult, i + 1)], out intAdults[i]))
                  bAdultsSpecified[i] = true;
            }

            if (!string.IsNullOrWhiteSpace(Request.QueryString[string.Format(Constants.DeeplinkParams.NumberOfChild, i + 1)]))
            {
               if (Int32.TryParse(Request.QueryString[string.Format(Constants.DeeplinkParams.NumberOfChild, i + 1)], out intChildren[i]))
                  bChildrenSpecified[i] = true;
            }

         }

         // Validate query string parameters
         var objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection]; 
         if (!IsValidHotelCode())
         {
            Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectHotel;
            RedirectTo("~/Default.aspx");
            return;
         }
         objStayCriteriaSelection.HotelCode = strHotelCode;
         if (!IsValidStayDate())
         {
            Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectStayDate;
            RedirectTo("~/Default.aspx");
            return;
         }
         objStayCriteriaSelection.ArrivalDate = dtArrivalDate;
         objStayCriteriaSelection.DepartureDate = dtDepartureDate;
         if (!IsValidStayPolicy())
         {
            Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectRoomQuantity;
            RedirectTo("~/Pages/CheckAvailability.aspx");
            return;
         }

         objStayCriteriaSelection.PromotionCode = strPromoCode;
         objStayCriteriaSelection.RoomOccupantSelections = new RoomOccupantSelection[intNumberRooms];
         for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
         {
            objStayCriteriaSelection.RoomOccupantSelections[i] = new RoomOccupantSelection
            {
               RoomRefID = (i + 1).ToString(),
               NumberRooms = 1
            };

            objStayCriteriaSelection.RoomOccupantSelections[i].NumberAdults = intAdults[i];
            objStayCriteriaSelection.RoomOccupantSelections[i].NumberChildren = intChildren[i];

         }

         Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectRoomDetail;
         Session[Constants.Sessions.CurrentRoomDetailStep] = RoomDetailSelectionStep.SelectAdultQuantity;

         Context.Items.Add("DeepLinkNav", "1");
         RedirectTo("~/Pages/CheckAvailability.aspx");
         return;

      }

      // Booking cancellation command

      else if (!string.IsNullOrWhiteSpace(Request.QueryString[Constants.DeeplinkParams.Command]) &&
          Request.QueryString[Constants.DeeplinkParams.Command].Equals(Constants.DeeplinkParams.CmdCancelReservation, StringComparison.InvariantCultureIgnoreCase))
      {
         if (!string.IsNullOrWhiteSpace(Request.QueryString.Get(Constants.DeeplinkParams.HotelCode)))
         {
            strHotelCode = Request.QueryString.Get(Constants.DeeplinkParams.HotelCode);
         }

         CancelDetailsEntryInfo objCancelDetailsEntryInfo = new CancelDetailsEntryInfo
         {
            HotelCode = strHotelCode,
            ConfirmationNumber = "",
            GuestLastName = "",
            SelectedConfirmationNumbersToCancel = new string[0]
         };

         Session[Constants.Sessions.CancelDetailsEntryInfo] = objCancelDetailsEntryInfo;

         RedirectTo("~/Pages/CancelReservation.aspx?DeepLinkNav=1");
         return;
      }

      RedirectTo("~/Default.aspx");
      return;
   }

   //private TrackingCodeControl ucTrackingCodeControl;

   //private void ConfigureTrackingCodeControl()
   //{
   //   string uiCulture = WbsUiHelper.SelectedUICulture;

      

   //   StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

   //   TrackingCodeInfo[] objTrackingCodeInfos = WbsUiHelper.GetTrackingCodeInfos(objStayCriteriaSelection.HotelCode);

   //   if (objTrackingCodeInfos.Length == 0)
   //      return;

   //   string strTrackingCodeControlPath = ConfigurationManager.AppSettings["TrackingCodeControl.ascx"];
   //   ucTrackingCodeControl = (TrackingCodeControl)LoadControl(strTrackingCodeControlPath);
   //   phTrackingCodeControl.Controls.Clear();
   //   phTrackingCodeControl.Controls.Add(ucTrackingCodeControl);
   //   ucTrackingCodeControl.Clear();

   //   string strTrackingCodeItemControlPath = ConfigurationManager.AppSettings["TrackingCodeItemControl.ascx"];

   //   for (int i = 0; i < objTrackingCodeInfos.Length; i++)
   //   {
   //      if (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.AllPages ||
   //          (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.StartPageOnly && this.IsFirstTrackingPage))
   //      {
   //         TrackingCodeItemControl ucTrackingCodeItemControl =
   //            (TrackingCodeItemControl)LoadControl(strTrackingCodeItemControlPath);
   //         ucTrackingCodeControl.Add(ucTrackingCodeItemControl);
   //         ucTrackingCodeItemControl.Type = objTrackingCodeInfos[i].Type;
   //         ucTrackingCodeItemControl.TrackingCodeParameters = objTrackingCodeInfos[i].TrackingCodeParameters;
   //         ucTrackingCodeItemControl.HotelCode = objStayCriteriaSelection.HotelCode;
   //         ucTrackingCodeItemControl.PageUrl = string.Format("/{0}/book/DeepLink", uiCulture);
   //         ucTrackingCodeItemControl.Amount = 0;
   //      }
   //   }
   //}

   private void RedirectTo(string pageToGo)
   {
      //ConfigureTrackingCodeControl();
      //ucTrackingCodeControl.RenderUserControl();

      string pageUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host, ResolveUrl(pageToGo));
      string script = string.Format("location.replace(\"{0}\");", pageUrl);
      ScriptManager.RegisterStartupScript(this, GetType(), "DeepLinkPage", script, true);
   }

   private bool IsValidHotelCode()
   {
      if (bHotelCodeSpecified)
      {
         HotelSearchRS objPropertyListHotelSearchRS = (HotelSearchRS)Session[Constants.Sessions.PropertyListHotelSearchRS];
         HotelListItem[] objHotelListItems = objPropertyListHotelSearchRS.HotelListItems;
         return objHotelListItems.Any(hotel => hotel.HotelCode.Equals(strHotelCode, StringComparison.InvariantCultureIgnoreCase));
      }
      return false;
   }

   private bool IsValidStayDate()
   {
      if (!bArrivalDateSpecified || !bDepartureDateSpecified)
         return false;
      if (dtArrivalDate <= TZNet.ToLocal(WbsUiHelper.GetTimeZone(strHotelCode), DateTime.UtcNow).Date)
         return false;
      if (dtDepartureDate <= dtArrivalDate)
         return false;
      int stayDate = dtDepartureDate.Subtract(dtArrivalDate).Days;
      if (stayDate < 1 || stayDate > WebconfigHelper.MaxBookingDays)
         return false;

      return true;
   }

   private bool IsValidStayPolicy()
   {
      if (!bNumberRoomsSpecified)
         return false;

      if (intNumberRooms < 1 || intNumberRooms > WebconfigHelper.MaxRooms)
         return false;

      for (int i = 0; i < intNumberRooms; i++)
      {
         if (!bAdultsSpecified[i] || !bChildrenSpecified[i])
            return false;
         if ((intAdults[i] == 0 && intChildren[i] == 0) || intAdults[i] > WebconfigHelper.MaxAdult || intChildren[i] > WebconfigHelper.MaxChildren)
            return false;
      }

      return true;
   }
}
