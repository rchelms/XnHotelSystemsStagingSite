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

public partial class SearchForHotels : XnGR_WBS_Page
{
    private bool bAsyncGetHotelSearchAreaList;
    private bool bAsyncGetHotelSearchPropertyList;
    private bool bAsyncGetSearchHotelDescriptiveInfo;
    private bool bAsyncGetSearchHotelAvailCalendarInfo;

    private string[] SearchHotelCodes;

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
        bAsyncGetSearchHotelDescriptiveInfo = false;
        bAsyncGetSearchHotelAvailCalendarInfo = false;

        if (!IsPostBack)
        {
            if (this.IsDeepLinkNav) // Here from DeepLink.aspx, SearchHotel.aspx by-passed
            {
                bAsyncGetHotelSearchAreaList = true;
                bAsyncGetHotelSearchPropertyList = true;
            }

            bAsyncGetSearchHotelDescriptiveInfo = true;
            bAsyncGetSearchHotelAvailCalendarInfo = true;
        }

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetHotelSearchAreaList || bAsyncGetHotelSearchPropertyList || bAsyncGetSearchHotelDescriptiveInfo || bAsyncGetSearchHotelAvailCalendarInfo)
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

        else if (bAsyncGetSearchHotelDescriptiveInfo)
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
            HotelSearchRS objPropertyListHotelSearchRS = (HotelSearchRS)Session["PropertyListHotelSearchRS"];

            HotelListItem[] objHotelListItems = this.GetAreaHotelList(objStayCriteriaSelection.AreaID, objPropertyListHotelSearchRS.HotelListItems);

            List<string> lHotelCodes = new List<string>();

            for (int i = 0; i < objHotelListItems.Length; i++)
                lHotelCodes.Add(objHotelListItems[i].HotelCode);

            SearchHotelCodes = lHotelCodes.ToArray();

            if (SearchHotelCodes.Length != 0)
            {
                wbsAPIRouterData = new WBSAPIRouterData();
                this.WbsApiRouterHelper.InitSearchHotelDescriptiveInfoRQ(ref wbsAPIRouterData, SearchHotelCodes);
                this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(SearchHotelDescriptiveInfoComplete), null, false);
            }

            else
            {
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoHotelsLocatedInSearch"));

                // End async page operation

                if (!wbsIISAsyncResult.IsCompleted)
                    wbsIISAsyncResult.SetComplete();
            }

        }

        else if (bAsyncGetSearchHotelAvailCalendarInfo)
        {
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

            int intNumDays;

            if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
            {
                intNumDays = ((TimeSpan)objStayCriteriaSelection.DepartureDate.Date.Subtract(objStayCriteriaSelection.ArrivalDate.Date)).Days;
            }

            else
            {
                intNumDays = this.NumberDaysInRateGrid;
                Session["SearchRateGridStartDate"] = objStayCriteriaSelection.ArrivalDate.Date;
            }

            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitSearchHotelAvailCalendarInfoRQ(ref wbsAPIRouterData, SearchHotelCodes, objStayCriteriaSelection, objStayCriteriaSelection.ArrivalDate, intNumDays, this.WbsUiHelper.GetLoginLinkedProfileIdentifier());
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(SearchHotelAvailCalendarInfoComplete), null, false);
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

    private void SearchHotelDescriptiveInfoComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessSearchHotelDescriptiveInfoRS(ref wbsAPIRouterData))
        {
            HotelDescriptiveInfoRS objSearchHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["SearchHotelDescriptiveInfoRS"];

            if (objSearchHotelDescriptiveInfoRS.HotelDescriptiveInfos != null && objSearchHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
            {
                bAsyncGetSearchHotelDescriptiveInfo = false;
                this.BeginResumeAsyncDataCapture();
            }

            else
            {
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoHotelsLocatedInSearch"));

                // End async page operation

                if (!wbsIISAsyncResult.IsCompleted)
                    wbsIISAsyncResult.SetComplete();
            }

        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    private void SearchHotelAvailCalendarInfoComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessSearchHotelAvailCalendarInfoRS(ref wbsAPIRouterData))
        {
            HotelAvailabilityRS objSearchHotelAvailabilityCalendarRS = (HotelAvailabilityRS)Session["SearchHotelAvailabilityCalendarRS"];

            if (objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos != null && objSearchHotelAvailabilityCalendarRS.HotelRoomAvailInfos.Length != 0)
            {
                bAsyncGetSearchHotelAvailCalendarInfo = false;
                this.BeginResumeAsyncDataCapture();
            }

            else
            {
                this.AddPageError(PageErrorType.ValidationError, (String)GetGlobalResourceObject("SiteErrorMessages", "NoHotelsLocatedInSearch"));

                // End async page operation

                if (!wbsIISAsyncResult.IsCompleted)
                    wbsIISAsyncResult.SetComplete();
            }

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
            Response.Redirect("~/Pages/SelectSearchHotel.aspx");
        }

        else
        {
            this.SaveCrossPageErrors();
            Response.Redirect("~/Pages/SearchHotel.aspx?CrossPageErrors=1");
        }

        return;
    }

}
