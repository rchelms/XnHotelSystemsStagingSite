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
using MamaShelter;

public partial class CheckAvailability : XnGR_WBS_Page
{
   private bool bAsyncGetHotelSearchAreaList;
   private bool bAsyncGetHotelSearchPropertyList;
   private bool bAsyncGetHotelDescriptiveInfo;
   private bool bAsyncGetHotelAvailInfo;
   private bool bAsyncGetAlernateHotelDescriptiveInfo;

   protected override void Page_Init(object sender, EventArgs e)
   {
      base.Page_Init(sender, e);

      return;
   }

   protected override void Page_Load(object sender, EventArgs e)
   {
      base.Page_Load(sender, e);

      bAsyncGetHotelSearchAreaList = false;
      bAsyncGetHotelSearchPropertyList = false;
      bAsyncGetHotelDescriptiveInfo = false;
      bAsyncGetHotelAvailInfo = false;
      bAsyncGetAlernateHotelDescriptiveInfo = false;

      if (!IsPostBack)
      {
         if (this.IsDeepLinkNav) // if here from DeepLink.aspx, then SearchHotel.aspx or SelectHotel.aspx was by-passed
         {
            WbsUiHelper.InitRoomRateSelections();
            WbsUiHelper.InitAddOnPackageSelections();
            WbsUiHelper.InitGuestDetailsEntryInfo();

            if (this.IsGuestDetailsTestPrefill)
               WbsUiHelper.PrefillGuestDetailsEntryInfoForTesting();

            if (ConfigurationManager.AppSettings["EnableHotelSearch"] == "1")
            {
               bAsyncGetHotelSearchAreaList = true;
               bAsyncGetHotelSearchPropertyList = true;
            }

            else
            {
               bAsyncGetHotelSearchPropertyList = true;
            }

         }

         bAsyncGetHotelDescriptiveInfo = true;
         bAsyncGetHotelAvailInfo = true;

         Session.Add("AlternateHotelDescriptiveInfoRS", new HotelDescriptiveInfoRS()); // init in case not requested
      }

      PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
      RegisterAsyncTask(task);

      return;
   }

   public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
   {
      wbsIISAsyncResult = new WBSAsyncResult(cb, state);

      if (bAsyncGetHotelSearchAreaList || bAsyncGetHotelSearchPropertyList || bAsyncGetHotelDescriptiveInfo || bAsyncGetHotelAvailInfo || bAsyncGetAlernateHotelDescriptiveInfo)
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

      else if (bAsyncGetHotelDescriptiveInfo)
      {
         StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitHotelDescriptiveInfoRQ(ref wbsAPIRouterData, objStayCriteriaSelection.HotelCode);
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelDescriptiveInfoComplete), null, false);
      }

      else if (bAsyncGetHotelAvailInfo)
      {
         StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

         int intAvailCalNumDays = 0;

         if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] == "1")
         {
            intAvailCalNumDays = ((TimeSpan)(objStayCriteriaSelection.DepartureDate.Date.Subtract(objStayCriteriaSelection.ArrivalDate.Date))).Days;

            if (intAvailCalNumDays < this.NumberDaysInRateGrid)
               intAvailCalNumDays = this.NumberDaysInRateGrid;

            Session["RateGridStartDate"] = objStayCriteriaSelection.ArrivalDate.Date;
         }

         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitHotelAvailInfoRQ(ref wbsAPIRouterData, objStayCriteriaSelection, objStayCriteriaSelection.ArrivalDate, intAvailCalNumDays, this.WbsUiHelper.GetLoginLinkedProfileIdentifier());
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelAvailInfoComplete), null, false);
      }

      else if (bAsyncGetAlernateHotelDescriptiveInfo)
      {
         HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];

         List<string> lAlternateHotels = new List<string>();

         for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
         {
            if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].IsAlternateAvailability)
            {
               if (!lAlternateHotels.Contains(objHotelAvailabilityRS.HotelRoomAvailInfos[i].HotelCode))
                  lAlternateHotels.Add(objHotelAvailabilityRS.HotelRoomAvailInfos[i].HotelCode);
            }

         }

         wbsAPIRouterData = new WBSAPIRouterData();
         this.WbsApiRouterHelper.InitAlernateHotelDescriptiveInfoRQ(ref wbsAPIRouterData, lAlternateHotels.ToArray());
         this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(AlernateHotelDescriptiveInfoComplete), null, false);
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
         if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
         {
            HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];

            if (objHotelAvailabilityRS.HotelRoomAvailInfos.Length != 0)
            {
               bool bIsAlternateAvailability = false;

               for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
               {
                  if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].IsAlternateAvailability)
                  {
                     bIsAlternateAvailability = true;
                     break;
                  }

               }

               if (bIsAlternateAvailability)
               {
                  bAsyncGetAlernateHotelDescriptiveInfo = true;
               }

            }

         }

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

   private void AlernateHotelDescriptiveInfoComplete(IAsyncResult ar)
   {
      if (this.WbsApiRouterHelper.ProcessAlernateHotelDescriptiveInfoRS(ref wbsAPIRouterData))
      {
         bAsyncGetAlernateHotelDescriptiveInfo = false;
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
      this.PageComplete();

      if (!this.IsPageError)
      {
         //if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
         //{
         StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
         HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];

         if (objHotelAvailabilityRS.HotelRoomAvailInfos.Length != 0)
         {
            //bool bIsAlternateAvailability = false;

            //for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
            //{
            //   if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].IsAlternateAvailability)
            //   {
            //      bIsAlternateAvailability = true;
            //      break;
            //   }

            //}

            //if (!bIsAlternateAvailability)
            //{
               if (WbsUiHelper.IsFullAvailability(objHotelAvailabilityRS, objStayCriteriaSelection))
               {
                  // Availability for all room/occupant selections

                  //Response.Redirect("~/Pages/SelectRoom.aspx");
                  //Session[Constants.Sessions.CurrentBookingStep] = MamaShelter.BookingSteps.SelectRoomDetail;
                  //Session[Constants.Sessions.CurrentRoomDetailStep] =
                  //    MamaShelter.RoomDetailSelectionStep.SelectRoomType;
                  Response.Redirect("~/Pages/Default.aspx");
               }

               else
               {
                  // Availability for at least one room/occupant selection (but not all)

                  //Response.Redirect("~/Pages/SelectAlternate.aspx");

                  Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectStayDate;
                  AddPageError(PageErrorType.ValidationError, (string)GetLocalResourceObject("SelectAlternateInstructions"));
                  SaveCrossPageErrors();
                  Response.Redirect("~/Pages/Default.aspx?CrossPageErrors=1");
               }

            //}

            //else
            //{
            //   // Alternate availability for at least one near-by hotel

            //   Response.Redirect("~/Pages/SelectAlternate.aspx");
            //}

         }

         else
         {
            // No availability for any room/occupant selection

            //Response.Redirect("~/Pages/SelectAlternate.aspx");

            Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectStayDate;
            AddPageError(PageErrorType.ValidationError, (string)GetLocalResourceObject("SelectAlternateInstructions"));
            SaveCrossPageErrors();
            Response.Redirect("~/Pages/Default.aspx?CrossPageErrors=1");
         }

         //}

         //else
         //{
         //   // Rate grid mode always shows rates whether available or not available

         //   //Response.Redirect("~/Pages/SelectRoom.aspx");
         //   Session[Constants.Sessions.CurrentBookingStep] = MamaShelter.BookingSteps.SelectRoomDetail;
         //   Session[Constants.Sessions.CurrentRoomDetailStep] =
         //       MamaShelter.RoomDetailSelectionStep.SelectRoomType;
         //   Response.Redirect("~/Pages/Default.aspx");
         //}

      }

      else
      {
         //if (ConfigurationManager.AppSettings["EnableHotelSearch"] == "1")
         //{
         //   this.SaveCrossPageErrors();
         //   //Response.Redirect("~/Pages/SearchHotel.aspx?CrossPageErrors=1");
         //   Response.Redirect("~/Pages/Default.aspx?CrossPageErrors=1");
         //}

         //else
         //{
         //   this.SaveCrossPageErrors();
         //   //Response.Redirect("~/Pages/SelectHotel.aspx?CrossPageErrors=1");
         //   Response.Redirect("~/Pages/Default.aspx?CrossPageErrors=1");
         //}

         Session[Constants.Sessions.CurrentBookingStep] = BookingSteps.SelectHotel;
         this.SaveCrossPageErrors();
         Response.Redirect("~/Pages/Default.aspx?CrossPageErrors=1");
      }

      return;
   }

}
