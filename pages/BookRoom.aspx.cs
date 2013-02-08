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

public partial class BookRoom : XnGR_WBS_Page
{
    private bool bAsyncBookHotel;
    private bool bPrepayBooking;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        bAsyncBookHotel = false;
        bPrepayBooking = false;

        PaymentGatewayInfo[] objPaymentGatewayInfos = (PaymentGatewayInfo[])Session[Constants.Sessions.PaymentGatewayInfos];
        PaymentGatewayInfo objPaymentGatewayInfo = (PaymentGatewayInfo)Session[Constants.Sessions.PaymentGatewayInfo];
        HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations = (HotelBookingPaymentAllocation[])Session[Constants.Sessions.HotelBookingPaymentAllocations];
        GuestDetailsEntryInfo objGuestDetailsEntryInfo = (GuestDetailsEntryInfo)Session["GuestDetailsEntryInfo"];

        if (WBSPGHelper.IsOnlinePayment(objPaymentGatewayInfos, objHotelBookingPaymentAllocations, objGuestDetailsEntryInfo.PaymentCardType))
        {
            bPrepayBooking = true;

            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
            RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
            AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];

            HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

            HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

            if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
            {
                objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
            }

            HotelPaymentRQ objHotelPaymentRQ = (HotelPaymentRQ)Session["HotelPaymentRQ"];
            HotelPaymentRS objHotelPaymentRS = (HotelPaymentRS)Session["HotelPaymentRS"];

            string strFailedPrepayBookingID = Guid.NewGuid().ToString();

            FailedPrepayBookingInfo objFailedPrepayBookingInfo = new FailedPrepayBookingInfo();
            objFailedPrepayBookingInfo.PostingDateTime = DateTime.Now;
            objFailedPrepayBookingInfo.HotelName = objHotelDescriptiveInfo.HotelName;
            objFailedPrepayBookingInfo.StayCriteria = objStayCriteriaSelection;
            objFailedPrepayBookingInfo.RoomRates = objRoomRateSelections;
            objFailedPrepayBookingInfo.AddOnPackages = objAddOnPackageSelections;
            objFailedPrepayBookingInfo.GuestDetails = objGuestDetailsEntryInfo;
            objFailedPrepayBookingInfo.PaymentRequestInfo = objHotelPaymentRQ;
            objFailedPrepayBookingInfo.PaymentResponseInfo = objHotelPaymentRS;

            Session["FailedPrepayBookingID"] = strFailedPrepayBookingID;
            Session["FailedPrepayBookingInfo"] = objFailedPrepayBookingInfo;

            string strFailedPrepayBookingInfo = XHS.WBSUIBizObjects.Serializer.ToString(objFailedPrepayBookingInfo);

            this.WbsMonitor.AddItem(strFailedPrepayBookingID, strFailedPrepayBookingInfo);
        }

        bAsyncBookHotel = true;

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncBookHotel)
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
        if (bAsyncBookHotel)
        {
            BookingAction enumBookingAction;

            if (this.IsBookThrough)
                enumBookingAction = BookingAction.Sell;
            else
                enumBookingAction = BookingAction.TestSell;

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

        return;
    }

    private void BookHotelComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessBookHotelRS(ref wbsAPIRouterData))
        {
            bAsyncBookHotel = false;
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
            if (bPrepayBooking)
                this.WbsMonitor.RemoveItem((string)Session["FailedPrepayBookingID"]);

            Response.Redirect("~/Pages/Default.aspx");
        }

        else
        {
            this.SaveCrossPageErrors();
            Response.Redirect("~/Pages/Default.aspx?CrossPageErrors=1");
        }

        return;
    }

}
