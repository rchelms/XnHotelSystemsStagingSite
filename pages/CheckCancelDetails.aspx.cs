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

public partial class CheckCancelDetails : XnGR_WBS_Page
{
    private bool bAsyncGetHotelDescriptiveInfo;
    private bool bAsyncReadHotelBooking;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        bAsyncGetHotelDescriptiveInfo = false;
        bAsyncReadHotelBooking = false;

        if (!IsPostBack)
        {
            Session["SelectedRoom"] = "1";

            bAsyncGetHotelDescriptiveInfo = true;
            bAsyncReadHotelBooking = true;
        }

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetHotelDescriptiveInfo || bAsyncReadHotelBooking)
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
        if (bAsyncGetHotelDescriptiveInfo)
        {
            CancelDetailsEntryInfo objCancelDetailsEntryInfo = (CancelDetailsEntryInfo)Session["CancelDetailsEntryInfo"];

            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitHotelDescriptiveInfoRQ(ref wbsAPIRouterData, objCancelDetailsEntryInfo.HotelCode);
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelDescriptiveInfoComplete), null, false);
        }

        else if (bAsyncReadHotelBooking)
        {
            CancelDetailsEntryInfo objCancelDetailsEntryInfo = (CancelDetailsEntryInfo)Session["CancelDetailsEntryInfo"];

            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitReadHotelBookingRQ(ref wbsAPIRouterData, objCancelDetailsEntryInfo.ConfirmationNumber);
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(ReadHotelBookingComplete), null, false);
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

    private void ReadHotelBookingComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessReadHotelBookingRS(ref wbsAPIRouterData))
        {
            bAsyncReadHotelBooking = false;
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

        if (this.IsPageError)
        {
            this.SaveCrossPageErrors();
            Response.Redirect("~/Pages/CancelReservation.aspx?CrossPageErrors=1");
        }

        else
        {
            CancelDetailsEntryInfo objCancelDetailsEntryInfo = (CancelDetailsEntryInfo)Session["CancelDetailsEntryInfo"];
            this.ValidateCancellationRequest(objCancelDetailsEntryInfo);

            //**** DUMB OBJECT *****
            //var objCancelDetailsEntryInfo = new CancelDetailsEntryInfo { ConfirmationNumber = "TEST12345"
            //    , HotelCode = "XN00049", GuestLastName = "Fisher"
            //    , SelectedConfirmationNumbersToCancel = new string[] { "" } };
            // ***********************

            if (this.IsPageError)
            {
                this.SaveCrossPageErrors();
                Response.Redirect("~/Pages/CancelReservation.aspx?CrossPageErrors=1");
            }

            else
            {
                objCancelDetailsEntryInfo.SelectedConfirmationNumbersToCancel = new string[0];
                Session["CancelDetailsEntryInfo"] = objCancelDetailsEntryInfo;

                Response.Redirect("~/Pages/SelectCancelRoom.aspx");
                //Response.Redirect("~/Pages/default.aspx?cmode=1");
            }

        }

        return;
    }

    private void ValidateCancellationRequest(CancelDetailsEntryInfo objCancelDetailsEntryInfo)
    {
        if (WbsUiHelper.GetValidatedBookings(objCancelDetailsEntryInfo).Length == 0)
            this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "RequestedBookingNotLocated"));

        return;
    }

}
