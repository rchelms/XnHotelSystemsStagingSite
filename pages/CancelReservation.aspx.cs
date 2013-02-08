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

public partial class CancelReservation : XnGR_WBS_Page
{
   private ErrorDisplayControl ucErrorDisplayControl;
   private CancelDetailsEntryControl ucCancelDetailsEntryControl;
   private TrackingCodeControl ucTrackingCodeControl;
   private RemoteContentContainer ucRemoteContentContainerControl;

   private bool bAsyncGetHotelSearchPropertyList;

   private bool bProcessProfileLogin;

   private string strLinkedProfileIdentifier;

   protected override void Page_Init(object sender, EventArgs e)
   {
      base.Page_Init(sender, e);

      this.LoadErrorDisplayControl();
      this.LoadCancelDetailsEntryControl();
      this.LoadRemoteContentContainerControl();
      return;
   }

   protected override void Page_Load(object sender, EventArgs e)
   {
      base.Page_Load(sender, e);

      bAsyncGetHotelSearchPropertyList = false;

      bProcessProfileLogin = false;

      if (!IsPostBack)
      {
         Session["SelectedRoom"] = "1";

         bAsyncGetHotelSearchPropertyList = true;
      }

      else
      {
         this.ConfigureErrorDisplayControl();
         this.ConfigureCancelDetailsEntryControl();
         ConfigureRemoteContentContainerControl();
      }

      PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
      RegisterAsyncTask(task);

      return;
   }

   public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
   {
      wbsIISAsyncResult = new WBSAsyncResult(cb, state);

      if (bAsyncGetHotelSearchPropertyList)
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
      this.IsParentPreRender = true;

      this.ConfigureErrorDisplayControl();
      this.ConfigureCancelDetailsEntryControl();
      this.ConfigureTrackingCodeControl();
      ConfigureRemoteContentContainerControl();

      ucErrorDisplayControl.RenderUserControl();
      ucCancelDetailsEntryControl.RenderUserControl();
      ucTrackingCodeControl.RenderUserControl();
      ucRemoteContentContainerControl.RenderUserControl();

      this.PageComplete();

      return;
   }

   protected void CancelDetailsCompleted(object sender, EventArgs e)
   {
      this.ValidateCancelDetails(ucCancelDetailsEntryControl.CancelDetailsEntryInfo);

      Session["CancelDetailsEntryInfo"] = ucCancelDetailsEntryControl.CancelDetailsEntryInfo;

      if (!this.IsPageError)
      {
         Response.Redirect("~/Pages/CheckCancelDetails.aspx");
      }

      else
      {

      }

      return;
   }

   private void ValidateCancelDetails(CancelDetailsEntryInfo objCancelDetailsEntryInfo)
   {
      if (objCancelDetailsEntryInfo.HotelCode == null || objCancelDetailsEntryInfo.HotelCode == "")
         this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoHotelSelection"));

      if (objCancelDetailsEntryInfo.ConfirmationNumber.Trim() == "")
         this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoConfirmationNumberEntry"));

      if (objCancelDetailsEntryInfo.GuestLastName.Trim() == "")
         this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoLastNameEntry"));

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

   private void LoadCancelDetailsEntryControl()
   {
      string strCancelDetailsEntryControlPath = ConfigurationManager.AppSettings["CancelDetailsEntryControl.ascx"];
      ucCancelDetailsEntryControl = (CancelDetailsEntryControl)LoadControl(strCancelDetailsEntryControlPath);

      phCancelDetailsEntryControl.Controls.Clear();
      phCancelDetailsEntryControl.Controls.Add(ucCancelDetailsEntryControl);

      ucCancelDetailsEntryControl.CancelDetailsCompleted += new CancelDetailsEntryControl.CancelDetailsCompletedEvent(this.CancelDetailsCompleted);

      return;
   }

   private void ConfigureCancelDetailsEntryControl()
   {
      ucCancelDetailsEntryControl.HotelListItems = ((HotelSearchRS)Session["PropertyListHotelSearchRS"]).HotelListItems;
      ucCancelDetailsEntryControl.CancelDetailsEntryInfo = (CancelDetailsEntryInfo)Session["CancelDetailsEntryInfo"];

      return;
   }

   private void ConfigureTrackingCodeControl()
   {
      StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

      TrackingCodeInfo[] objTrackingCodeInfos = WbsUiHelper.GetTrackingCodeInfos(objStayCriteriaSelection.HotelCode);

      if (objTrackingCodeInfos.Length == 0)
         return;

      string strTrackingCodeControlPath = ConfigurationManager.AppSettings["TrackingCodeControl.ascx"];
      ucTrackingCodeControl = (TrackingCodeControl)LoadControl(strTrackingCodeControlPath);

      //Control objHeadElement = this.Page.Master.FindControl("head_element");

      //if (objHeadElement != null)
      //    objHeadElement.Controls.Add(ucHeadTrackingCodeControl);

      phTrackingCodeControl.Controls.Clear();
      phTrackingCodeControl.Controls.Add(ucTrackingCodeControl);

      ucTrackingCodeControl.Clear();

      string strTrackingCodeItemControlPath = ConfigurationManager.AppSettings["TrackingCodeItemControl.ascx"];

      for (int i = 0; i < objTrackingCodeInfos.Length; i++)
      {
         if (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.AllPages || (objTrackingCodeInfos[i].PageSelection == TrackingPageSelection.StartPageOnly && this.IsFirstTrackingPage))
         {
            TrackingCodeItemControl ucTrackingCodeItemControl = (TrackingCodeItemControl)LoadControl(strTrackingCodeItemControlPath);

            //if (objTrackingCodeInfos[i].TagLocation == TrackingTagLocation.HeadElement)
            //    ucHeadTrackingCodeControl.Add(ucTrackingCodeItemControl);
            ucTrackingCodeControl.Add(ucTrackingCodeItemControl);

            ucTrackingCodeItemControl.Type = objTrackingCodeInfos[i].Type;
            ucTrackingCodeItemControl.TrackingCodeParameters = objTrackingCodeInfos[i].TrackingCodeParameters;
            ucTrackingCodeItemControl.HotelCode = objStayCriteriaSelection.HotelCode;
            ucTrackingCodeItemControl.PageUrl = "/" + WbsUiHelper.SelectedUICulture + "/cancel/CancelReservation";
            ucTrackingCodeItemControl.Amount = 0;
         }

      }

      this.IsFirstTrackingPage = false;

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

   private void ConfigureRemoteContentContainerControl()
   {
      string customCDNPath = ConfigurationManager.AppSettings[Constants.WebConfigKeys.CustomCDNPath];
      customCDNPath = customCDNPath.EndsWith("/") ? customCDNPath : customCDNPath + "/";
      ucRemoteContentContainerControl.Src = customCDNPath + "booking/cancel-confirmation/";
   }

}
