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
public partial class TestAvail : XnGR_WBS_Page
{
    private bool bAsyncGetHotelAvailInfo;

    protected override void Page_Init(object sender, EventArgs e)
    {
        this.IsNewSessionOverride = true;

        base.Page_Init(sender, e);

        this.EventLog.Write(Page.Session.SessionID + ", enter/exit Page_Init, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter Page_Load, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        base.Page_Load(sender, e);

        bAsyncGetHotelAvailInfo = false;

        if (!IsPostBack)
        {
            bAsyncGetHotelAvailInfo = true;
        }

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        this.EventLog.Write(Page.Session.SessionID + ", exit Page_Load, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter BeginAsyncOperation, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetHotelAvailInfo)
        {
            this.BeginResumeAsyncDataCapture();
        }

        else
        {
            while (!ThreadPool.QueueUserWorkItem(new WaitCallback(TerminateAsyncOperation), wbsIISAsyncResult))
                Thread.Sleep(100);
        }

        this.EventLog.Write(Page.Session.SessionID + ", exit BeginAsyncOperation, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return wbsIISAsyncResult;
    }

    private void BeginResumeAsyncDataCapture()
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter BeginResumeAsyncDataCapture, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        if (bAsyncGetHotelAvailInfo)
        {
            StayCriteriaSelection objStayCriteriaSelection = new StayCriteriaSelection();

            objStayCriteriaSelection.HotelCode = "XN00031";
            objStayCriteriaSelection.AreaID = "1";
            objStayCriteriaSelection.CountryCode = "AU";
            objStayCriteriaSelection.ArrivalDate = new DateTime(2009, 12, 24);
            objStayCriteriaSelection.DepartureDate = objStayCriteriaSelection.ArrivalDate.AddDays(1);
            objStayCriteriaSelection.PromotionCode = "";
            objStayCriteriaSelection.RoomOccupantSelections = new RoomOccupantSelection[1];
            objStayCriteriaSelection.RoomOccupantSelections[0] = new RoomOccupantSelection();
            objStayCriteriaSelection.RoomOccupantSelections[0].RoomRefID = "1";
            objStayCriteriaSelection.RoomOccupantSelections[0].NumberAdults = 2;
            objStayCriteriaSelection.RoomOccupantSelections[0].NumberChildren = 0;
            objStayCriteriaSelection.RoomOccupantSelections[0].NumberRooms = 1;

            int intAvailCalNumDays = 0;

            if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] == "1")
            {
                intAvailCalNumDays = this.NumberDaysInRateGrid;
            }

            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitHotelAvailInfoRQ(ref wbsAPIRouterData, objStayCriteriaSelection, objStayCriteriaSelection.ArrivalDate, intAvailCalNumDays, this.WbsUiHelper.GetLoginLinkedProfileIdentifier());
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelAvailInfoComplete), null, false);
        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        this.EventLog.Write(Page.Session.SessionID + ", exit BeginResumeAsyncDataCapture, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

    private void HotelAvailInfoComplete(IAsyncResult ar)
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter HotelAvailInfoComplete, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        if (this.WbsApiRouterHelper.ProcessHotelAvailInfoRS(ref wbsAPIRouterData))
        {
            bAsyncGetHotelAvailInfo = false;
            this.BeginResumeAsyncDataCapture();
        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        this.EventLog.Write(Page.Session.SessionID + ", exit HotelAvailInfoComplete, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

    public void EndAsyncOperation(IAsyncResult ar)
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter/exit EndAsyncOperation, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

    public void TimeoutAsyncOperation(IAsyncResult ar)
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter/exit TimeoutAsyncOperation, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

    private void TerminateAsyncOperation(Object StateInfo)
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter TerminateAsyncOperation, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        WBSAsyncResult wbsIISAsyncResult = (WBSAsyncResult)StateInfo;

        if (!wbsIISAsyncResult.IsCompleted)
            wbsIISAsyncResult.SetComplete();

        this.EventLog.Write(Page.Session.SessionID + ", exit TerminateAsyncOperation, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter Page_PreRenderComplete, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        this.PageComplete();

        this.EventLog.Write(Page.Session.SessionID + ", exit Page_PreRenderComplete, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        this.EventLog.Write(Page.Session.SessionID + ", enter Page_Unload, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        this.PageComplete();

        this.EventLog.Write(Page.Session.SessionID + ", exit Page_Unload, " + DateTime.Now.ToString("MM dd yyyy HH:mm:ss.ffffff"));

        return;
    }

}
