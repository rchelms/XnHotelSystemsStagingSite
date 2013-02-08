using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;

/// <summary>
/// Helper class for WBS API (router) Gateway Messages
/// </summary>

public class WBSAPIRouterHelper
{
    private System.Web.SessionState.HttpSessionState Session;
    private System.Web.UI.Page Page;
    private FileLog objEventLog;
    private ExceptionLog objExceptionEventLog;
    private bool bIsProduction;

    public WBSAPIRouterHelper(System.Web.SessionState.HttpSessionState session, System.Web.UI.Page page, FileLog eventLog, ExceptionLog exceptionEventLog, bool isProduction)
    {
        Session = session;
        Page = page;
        objEventLog = eventLog;
        objExceptionEventLog = exceptionEventLog;
        bIsProduction = isProduction;

        return;
    }

    public void InitLoginProfileRQ(ref WBSAPIRouterData objWBSAPIRouterData, string strProfileLogonName)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        ProfileReadRQ objProfileReadRQ = new ProfileReadRQ();

        objProfileReadRQ.RequestHeader = this.GetRequestHeader();

        objProfileReadRQ.ReadContext = ProfileReadContext.ProfileLogonName;
        objProfileReadRQ.ReadIdentifier = strProfileLogonName;

        gwWBS.InitProfileReadRQ(ref objWBSAPIRouterData, objProfileReadRQ);

        return;
    }

    public bool ProcessLoginProfileRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        ProfileReadRS objProfileReadRS = gwWBS.ProcessProfileReadRS(ref objWBSAPIRouterData);

        Session["LoginProfileReadRS"] = objProfileReadRS;

        return !this.ProcessResponseErrors(objProfileReadRS.ResponseHeader);
    }

    public void InitLinkedProfileRQ(ref WBSAPIRouterData objWBSAPIRouterData, string strProfileRefID)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        ProfileReadRQ objProfileReadRQ = new ProfileReadRQ();

        objProfileReadRQ.RequestHeader = this.GetRequestHeader();

        objProfileReadRQ.ReadContext = ProfileReadContext.ProfileRefID;
        objProfileReadRQ.ReadIdentifier = strProfileRefID;

        gwWBS.InitProfileReadRQ(ref objWBSAPIRouterData, objProfileReadRQ);

        return;
    }

    public bool ProcessLinkedProfileRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        ProfileReadRS objProfileReadRS = gwWBS.ProcessProfileReadRS(ref objWBSAPIRouterData);

        Session["LinkedProfileReadRS"] = objProfileReadRS;

        return !this.ProcessResponseErrors(objProfileReadRS.ResponseHeader);
    }

    public void InitHotelSearchAreaListRQ(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelSearchRQ objHotelSearchRQ = new HotelSearchRQ();

        objHotelSearchRQ.RequestHeader = this.GetRequestHeader();

        objHotelSearchRQ.SearchAction = SearchAction.AreaList;
        objHotelSearchRQ.BrandCodes = this.GetHotelBrandCodes();
        objHotelSearchRQ.CountryCodes = new string[0];
        objHotelSearchRQ.AreaIDs = new string[0];

        gwWBS.InitHotelSearchRQ(ref objWBSAPIRouterData, objHotelSearchRQ);

        return;
    }

    public bool ProcessHotelSearchAreaListRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelSearchRS objHotelSearchRS = gwWBS.ProcessHotelSearchRS(ref objWBSAPIRouterData);

        Session["AreaListHotelSearchRS"] = objHotelSearchRS;

        return !this.ProcessResponseErrors(objHotelSearchRS.ResponseHeader);
    }

    public void InitHotelSearchPropertyListRQ(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelSearchRQ objHotelSearchRQ = new HotelSearchRQ();

        objHotelSearchRQ.RequestHeader = this.GetRequestHeader();

        objHotelSearchRQ.SearchAction = SearchAction.PropertyList;
        objHotelSearchRQ.BrandCodes = this.GetHotelBrandCodes();
        objHotelSearchRQ.CountryCodes = new string[0];
        objHotelSearchRQ.AreaIDs = new string[0];

        gwWBS.InitHotelSearchRQ(ref objWBSAPIRouterData, objHotelSearchRQ);

        return;
    }

    public void InitHotelSearchPropertyListRQ(ref WBSAPIRouterData objWBSAPIRouterData, string[] objAreaIDs)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelSearchRQ objHotelSearchRQ = new HotelSearchRQ();

        objHotelSearchRQ.RequestHeader = this.GetRequestHeader();

        objHotelSearchRQ.SearchAction = SearchAction.PropertyList;
        objHotelSearchRQ.BrandCodes = this.GetHotelBrandCodes();
        objHotelSearchRQ.CountryCodes = new string[0];
        objHotelSearchRQ.AreaIDs = objAreaIDs;

        gwWBS.InitHotelSearchRQ(ref objWBSAPIRouterData, objHotelSearchRQ);

        return;
    }

    public bool ProcessHotelSearchPropertyListRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelSearchRS objHotelSearchRS = gwWBS.ProcessHotelSearchRS(ref objWBSAPIRouterData);

        Session["PropertyListHotelSearchRS"] = objHotelSearchRS;

        return !this.ProcessResponseErrors(objHotelSearchRS.ResponseHeader);
    }

    public void InitHotelAvailInfoRQ(ref WBSAPIRouterData objWBSAPIRouterData, StayCriteriaSelection objStayCriteriaSelection, DateTime dtAvailCalStartDate, int intAvailCalNumDays, string strLinkedProfileID)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelAvailabilityRQ objHotelAvailabilityRQ = new HotelAvailabilityRQ();

        objHotelAvailabilityRQ.RequestHeader = this.GetRequestHeader();

        if (ConfigurationManager.AppSettings["EnableRoomRateGridModel"] != "1")
        {
            bool bAlternateAvailability = false;

            if (ConfigurationManager.AppSettings["EnableAlternateHotelAvailability"] == "1")
                bAlternateAvailability = true;

            objHotelAvailabilityRQ.HotelRoomAvailRequests = new HotelRoomAvailRequest[objStayCriteriaSelection.RoomOccupantSelections.Length];

            for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
            {
                HotelRoomAvailRequest objHotelRoomAvailRequest = new HotelRoomAvailRequest();
                objHotelAvailabilityRQ.HotelRoomAvailRequests[i] = objHotelRoomAvailRequest;

                objHotelRoomAvailRequest.SegmentRefID = objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
                objHotelRoomAvailRequest.ReturnAlternateAvailability = bAlternateAvailability;
                objHotelRoomAvailRequest.IsModify = false;

                objHotelRoomAvailRequest.HotelCode = objStayCriteriaSelection.HotelCode;
                objHotelRoomAvailRequest.ArrivalDate = objStayCriteriaSelection.ArrivalDate;
                objHotelRoomAvailRequest.DepartureDate = objStayCriteriaSelection.DepartureDate;
                objHotelRoomAvailRequest.PromotionCode = objStayCriteriaSelection.PromotionCode;
                objHotelRoomAvailRequest.ProfileIdentifier = strLinkedProfileID;

                objHotelRoomAvailRequest.NumberRooms = objStayCriteriaSelection.RoomOccupantSelections[i].NumberRooms;
                objHotelRoomAvailRequest.NumberAdults = objStayCriteriaSelection.RoomOccupantSelections[i].NumberAdults;
                objHotelRoomAvailRequest.NumberChildren = objStayCriteriaSelection.RoomOccupantSelections[i].NumberChildren;

                if (intAvailCalNumDays != 0)
                {
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendar = true;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarOnly = false;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarAvailRatesOnly = true;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarSummaryOnly = false;
                    objHotelRoomAvailRequest.ApplyAvailabilityCalendarSummaryFilter = false;
                    objHotelRoomAvailRequest.AvailCalendarStartDate = dtAvailCalStartDate;
                    objHotelRoomAvailRequest.AvailCalendarNumDays = intAvailCalNumDays;
                }

                else
                {
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendar = false;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarOnly = false;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarAvailRatesOnly = true;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarSummaryOnly = false;
                    objHotelRoomAvailRequest.ApplyAvailabilityCalendarSummaryFilter = false;
                    objHotelRoomAvailRequest.AvailCalendarStartDate = dtAvailCalStartDate;
                    objHotelRoomAvailRequest.AvailCalendarNumDays = 0;
                }

                objHotelRoomAvailRequest.RequestedPublicRatePlanCategories = new string[0];
                objHotelRoomAvailRequest.RequestedRoomTypeCodes = new string[0];
                objHotelRoomAvailRequest.RequestedSpecialRatePlanCodes = new string[0];
            }

        }

        else
        {
            objHotelAvailabilityRQ.HotelRoomAvailRequests = new HotelRoomAvailRequest[objStayCriteriaSelection.RoomOccupantSelections.Length];

            for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
            {
                HotelRoomAvailRequest objHotelRoomAvailRequest = new HotelRoomAvailRequest();
                objHotelAvailabilityRQ.HotelRoomAvailRequests[i] = objHotelRoomAvailRequest;

                objHotelRoomAvailRequest.SegmentRefID = objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
                objHotelRoomAvailRequest.ReturnAlternateAvailability = false;
                objHotelRoomAvailRequest.IsModify = false;

                objHotelRoomAvailRequest.HotelCode = objStayCriteriaSelection.HotelCode;
                objHotelRoomAvailRequest.ArrivalDate = objStayCriteriaSelection.ArrivalDate;
                objHotelRoomAvailRequest.DepartureDate = objStayCriteriaSelection.DepartureDate;
                objHotelRoomAvailRequest.PromotionCode = objStayCriteriaSelection.PromotionCode;
                objHotelRoomAvailRequest.ProfileIdentifier = strLinkedProfileID;

                objHotelRoomAvailRequest.NumberRooms = objStayCriteriaSelection.RoomOccupantSelections[i].NumberRooms;
                objHotelRoomAvailRequest.NumberAdults = objStayCriteriaSelection.RoomOccupantSelections[i].NumberAdults;
                objHotelRoomAvailRequest.NumberChildren = objStayCriteriaSelection.RoomOccupantSelections[i].NumberChildren;

                if (intAvailCalNumDays != 0)
                {
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendar = true;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarOnly = false;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarAvailRatesOnly = false;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarSummaryOnly = false;
                    objHotelRoomAvailRequest.ApplyAvailabilityCalendarSummaryFilter = false;
                    objHotelRoomAvailRequest.AvailCalendarStartDate = dtAvailCalStartDate;
                    objHotelRoomAvailRequest.AvailCalendarNumDays = intAvailCalNumDays;
                }

                else
                {
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendar = false;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarOnly = false;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarAvailRatesOnly = false;
                    objHotelRoomAvailRequest.ReturnAvailabilityCalendarSummaryOnly = false;
                    objHotelRoomAvailRequest.ApplyAvailabilityCalendarSummaryFilter = false;
                    objHotelRoomAvailRequest.AvailCalendarStartDate = dtAvailCalStartDate;
                    objHotelRoomAvailRequest.AvailCalendarNumDays = 0;
                }

                objHotelRoomAvailRequest.RequestedPublicRatePlanCategories = new string[0];
                objHotelRoomAvailRequest.RequestedRoomTypeCodes = new string[0];
                objHotelRoomAvailRequest.RequestedSpecialRatePlanCodes = new string[0];
            }

        }

        gwWBS.InitHotelAvailabilityRQ(ref objWBSAPIRouterData, objHotelAvailabilityRQ);

        return;
    }

    public bool ProcessHotelAvailInfoRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelAvailabilityRS objHotelAvailabilityRS = gwWBS.ProcessHotelAvailabilityRS(ref objWBSAPIRouterData);

        Session["HotelAvailabilityRS"] = objHotelAvailabilityRS;
        Session["HotelAvailabilityCalendarRS"] = null;

        return !this.ProcessResponseErrors(objHotelAvailabilityRS.ResponseHeader);
    }

    public void InitHotelAvailCalendarInfoRQ(ref WBSAPIRouterData objWBSAPIRouterData, StayCriteriaSelection objStayCriteriaSelection, DateTime dtAvailCalStartDate, int intAvailCalNumDays, string strLinkedProfileID)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelAvailabilityRQ objHotelAvailabilityRQ = new HotelAvailabilityRQ();

        objHotelAvailabilityRQ.RequestHeader = this.GetRequestHeader();

        objHotelAvailabilityRQ.HotelRoomAvailRequests = new HotelRoomAvailRequest[objStayCriteriaSelection.RoomOccupantSelections.Length];

        for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
        {
            HotelRoomAvailRequest objHotelRoomAvailRequest = new HotelRoomAvailRequest();
            objHotelAvailabilityRQ.HotelRoomAvailRequests[i] = objHotelRoomAvailRequest;

            objHotelRoomAvailRequest.SegmentRefID = objStayCriteriaSelection.RoomOccupantSelections[i].RoomRefID;
            objHotelRoomAvailRequest.ReturnAlternateAvailability = false;
            objHotelRoomAvailRequest.IsModify = false;

            objHotelRoomAvailRequest.HotelCode = objStayCriteriaSelection.HotelCode;
            objHotelRoomAvailRequest.ArrivalDate = objStayCriteriaSelection.ArrivalDate;
            objHotelRoomAvailRequest.DepartureDate = objStayCriteriaSelection.DepartureDate;
            objHotelRoomAvailRequest.PromotionCode = objStayCriteriaSelection.PromotionCode;
            objHotelRoomAvailRequest.ProfileIdentifier = strLinkedProfileID;

            objHotelRoomAvailRequest.NumberRooms = objStayCriteriaSelection.RoomOccupantSelections[i].NumberRooms;
            objHotelRoomAvailRequest.NumberAdults = objStayCriteriaSelection.RoomOccupantSelections[i].NumberAdults;
            objHotelRoomAvailRequest.NumberChildren = objStayCriteriaSelection.RoomOccupantSelections[i].NumberChildren;

            objHotelRoomAvailRequest.ReturnAvailabilityCalendar = true;
            objHotelRoomAvailRequest.ReturnAvailabilityCalendarOnly = true;
            objHotelRoomAvailRequest.ReturnAvailabilityCalendarAvailRatesOnly = true;
            objHotelRoomAvailRequest.ReturnAvailabilityCalendarSummaryOnly = false;
            objHotelRoomAvailRequest.ApplyAvailabilityCalendarSummaryFilter = false;
            objHotelRoomAvailRequest.AvailCalendarStartDate = dtAvailCalStartDate;
            objHotelRoomAvailRequest.AvailCalendarNumDays = intAvailCalNumDays;

            objHotelRoomAvailRequest.RequestedPublicRatePlanCategories = new string[0];
            objHotelRoomAvailRequest.RequestedRoomTypeCodes = new string[0];
            objHotelRoomAvailRequest.RequestedSpecialRatePlanCodes = new string[0];
        }

        gwWBS.InitHotelAvailabilityRQ(ref objWBSAPIRouterData, objHotelAvailabilityRQ);

        return;
    }

    public bool ProcessHotelAvailCalendarInfoRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelAvailabilityRS objHotelAvailabilityRS = gwWBS.ProcessHotelAvailabilityRS(ref objWBSAPIRouterData);

        Session["HotelAvailabilityCalendarRS"] = objHotelAvailabilityRS;

        return !this.ProcessResponseErrors(objHotelAvailabilityRS.ResponseHeader);
    }

    public void InitSearchHotelAvailCalendarInfoRQ(ref WBSAPIRouterData objWBSAPIRouterData, string[] objHotelCodes, StayCriteriaSelection objStayCriteriaSelection, DateTime dtAvailCalStartDate, int intAvailCalNumDays, string strLinkedProfileID)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelAvailabilityRQ objHotelAvailabilityRQ = new HotelAvailabilityRQ();

        objHotelAvailabilityRQ.RequestHeader = this.GetRequestHeader();

        objHotelAvailabilityRQ.HotelRoomAvailRequests = new HotelRoomAvailRequest[objHotelCodes.Length];

        for (int i = 0; i < objHotelCodes.Length; i++)
        {
            HotelRoomAvailRequest objHotelRoomAvailRequest = new HotelRoomAvailRequest();
            objHotelAvailabilityRQ.HotelRoomAvailRequests[i] = objHotelRoomAvailRequest;

            objHotelRoomAvailRequest.SegmentRefID = objHotelCodes[i];
            objHotelRoomAvailRequest.ReturnAlternateAvailability = false;
            objHotelRoomAvailRequest.IsModify = false;

            objHotelRoomAvailRequest.HotelCode = objHotelCodes[i];
            objHotelRoomAvailRequest.ArrivalDate = objStayCriteriaSelection.ArrivalDate;
            objHotelRoomAvailRequest.DepartureDate = objStayCriteriaSelection.DepartureDate;
            objHotelRoomAvailRequest.PromotionCode = objStayCriteriaSelection.PromotionCode;
            objHotelRoomAvailRequest.ProfileIdentifier = strLinkedProfileID;

            objHotelRoomAvailRequest.NumberRooms = objStayCriteriaSelection.RoomOccupantSelections[0].NumberRooms;
            objHotelRoomAvailRequest.NumberAdults = objStayCriteriaSelection.RoomOccupantSelections[0].NumberAdults;
            objHotelRoomAvailRequest.NumberChildren = objStayCriteriaSelection.RoomOccupantSelections[0].NumberChildren;

            objHotelRoomAvailRequest.ReturnAvailabilityCalendar = true;
            objHotelRoomAvailRequest.ReturnAvailabilityCalendarOnly = true;
            objHotelRoomAvailRequest.ReturnAvailabilityCalendarAvailRatesOnly = true;
            objHotelRoomAvailRequest.ReturnAvailabilityCalendarSummaryOnly = true;
            objHotelRoomAvailRequest.ApplyAvailabilityCalendarSummaryFilter = true;
            objHotelRoomAvailRequest.AvailCalendarStartDate = dtAvailCalStartDate;
            objHotelRoomAvailRequest.AvailCalendarNumDays = intAvailCalNumDays;

            objHotelRoomAvailRequest.RequestedPublicRatePlanCategories = new string[0];
            objHotelRoomAvailRequest.RequestedRoomTypeCodes = new string[0];
            objHotelRoomAvailRequest.RequestedSpecialRatePlanCodes = new string[0];
        }

        gwWBS.InitHotelAvailabilityRQ(ref objWBSAPIRouterData, objHotelAvailabilityRQ);

        return;
    }

    public bool ProcessSearchHotelAvailCalendarInfoRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelAvailabilityRS objHotelAvailabilityRS = gwWBS.ProcessHotelAvailabilityRS(ref objWBSAPIRouterData);

        Session["SearchHotelAvailabilityCalendarRS"] = objHotelAvailabilityRS;

        return !this.ProcessResponseErrors(objHotelAvailabilityRS.ResponseHeader);
    }

    public void InitHotelDescriptiveInfoRQ(ref WBSAPIRouterData objWBSAPIRouterData, string strHotelCode)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelDescriptiveInfoRQ objHotelDescriptiveInfoRQ = new HotelDescriptiveInfoRQ();

        objHotelDescriptiveInfoRQ.RequestHeader = this.GetRequestHeader();

        objHotelDescriptiveInfoRQ.HotelCodes = new string[1];
        objHotelDescriptiveInfoRQ.HotelCodes[0] = strHotelCode;
        objHotelDescriptiveInfoRQ.SendDescriptionInfo = true;
        objHotelDescriptiveInfoRQ.SendGuestRoomInfo = true;
        objHotelDescriptiveInfoRQ.SendPolicyInfo = true;
        objHotelDescriptiveInfoRQ.AreaIDs = new string[0];
        objHotelDescriptiveInfoRQ.BrandCodes = new string[0];
        objHotelDescriptiveInfoRQ.CountryCodes = new string[0];

        gwWBS.InitHotelDescriptionRQ(ref objWBSAPIRouterData, objHotelDescriptiveInfoRQ);

        return;
    }

    public bool ProcessHotelDescriptiveInfoRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = gwWBS.ProcessHotelDescriptionRS(ref objWBSAPIRouterData);

        Session["HotelDescriptiveInfoRS"] = objHotelDescriptiveInfoRS;

        return !this.ProcessResponseErrors(objHotelDescriptiveInfoRS.ResponseHeader);
    }

    public void InitSearchHotelDescriptiveInfoRQ(ref WBSAPIRouterData objWBSAPIRouterData, string[] objHotelCodes)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelDescriptiveInfoRQ objHotelDescriptiveInfoRQ = new HotelDescriptiveInfoRQ();

        objHotelDescriptiveInfoRQ.RequestHeader = this.GetRequestHeader();

        objHotelDescriptiveInfoRQ.HotelCodes = objHotelCodes;
        objHotelDescriptiveInfoRQ.SendDescriptionInfo = true;
        objHotelDescriptiveInfoRQ.SendGuestRoomInfo = false;
        objHotelDescriptiveInfoRQ.SendPolicyInfo = false;
        objHotelDescriptiveInfoRQ.AreaIDs = new string[0];
        objHotelDescriptiveInfoRQ.BrandCodes = new string[0];
        objHotelDescriptiveInfoRQ.CountryCodes = new string[0];

        gwWBS.InitHotelDescriptionRQ(ref objWBSAPIRouterData, objHotelDescriptiveInfoRQ);

        return;
    }

    public bool ProcessSearchHotelDescriptiveInfoRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = gwWBS.ProcessHotelDescriptionRS(ref objWBSAPIRouterData);

        Session["SearchHotelDescriptiveInfoRS"] = objHotelDescriptiveInfoRS;

        return !this.ProcessResponseErrors(objHotelDescriptiveInfoRS.ResponseHeader);
    }

    public void InitAlernateHotelDescriptiveInfoRQ(ref WBSAPIRouterData objWBSAPIRouterData, string[] objHotelCodes)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelDescriptiveInfoRQ objHotelDescriptiveInfoRQ = new HotelDescriptiveInfoRQ();

        objHotelDescriptiveInfoRQ.RequestHeader = this.GetRequestHeader();

        objHotelDescriptiveInfoRQ.HotelCodes = objHotelCodes;
        objHotelDescriptiveInfoRQ.SendDescriptionInfo = true;
        objHotelDescriptiveInfoRQ.SendGuestRoomInfo = false;
        objHotelDescriptiveInfoRQ.SendPolicyInfo = false;
        objHotelDescriptiveInfoRQ.AreaIDs = new string[0];
        objHotelDescriptiveInfoRQ.BrandCodes = new string[0];
        objHotelDescriptiveInfoRQ.CountryCodes = new string[0];

        gwWBS.InitHotelDescriptionRQ(ref objWBSAPIRouterData, objHotelDescriptiveInfoRQ);

        return;
    }

    public bool ProcessAlernateHotelDescriptiveInfoRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = gwWBS.ProcessHotelDescriptionRS(ref objWBSAPIRouterData);

        Session["AlternateHotelDescriptiveInfoRS"] = objHotelDescriptiveInfoRS;

        return !this.ProcessResponseErrors(objHotelDescriptiveInfoRS.ResponseHeader);
    }

    public void InitBookHotelRQ(ref WBSAPIRouterData objWBSAPIRouterData, BookingAction enumBookingAction)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];

        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
        AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];
        GuestDetailsEntryInfo objGuestDetailsEntryInfo = (GuestDetailsEntryInfo)Session["GuestDetailsEntryInfo"];
        CancelDetailsEntryInfo objCancelDetailsEntryInfo = (CancelDetailsEntryInfo)Session["CancelDetailsEntryInfo"];

        PaymentGatewayInfo[] objPaymentGatewayInfos = (PaymentGatewayInfo[])Session[Constants.Sessions.PaymentGatewayInfos];
        PaymentGatewayInfo objPaymentGatewayInfo = (PaymentGatewayInfo)Session[Constants.Sessions.PaymentGatewayInfo];
        HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations = (HotelBookingPaymentAllocation[])Session[Constants.Sessions.HotelBookingPaymentAllocations];
        HotelPaymentRQ objHotelPaymentRQ = (HotelPaymentRQ)Session["HotelPaymentRQ"];
        HotelPaymentRS objHotelPaymentRS = (HotelPaymentRS)Session["HotelPaymentRS"];

        bool bIsOnlinePayment = WBSPGHelper.IsOnlinePayment(objPaymentGatewayInfos, objHotelBookingPaymentAllocations, objGuestDetailsEntryInfo.PaymentCardType);

        Profile[] objProfiles = (Profile[])Session["LoginProfiles"];
        bool bIsLoggedIn = (bool)Session["IsLoggedIn"];

        Profile objCompanyProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.Corporation);
        Profile objAgencyProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.TravelAgent);

        HotelBookingRQ objHotelBookingRQ = new HotelBookingRQ();

        objHotelBookingRQ.RequestHeader = this.GetRequestHeader();
        objHotelBookingRQ.BookingAction = enumBookingAction;

        List<HotelBookingRequestSegment> lSegments = new List<HotelBookingRequestSegment>();

        if (enumBookingAction == BookingAction.Sell || enumBookingAction == BookingAction.TestSell || enumBookingAction == BookingAction.Modify)
        {
            for (int ri = 0; ri < objStayCriteriaSelection.RoomOccupantSelections.Length; ri++)
            {
                HotelBookingRequestSegment objSegment = new HotelBookingRequestSegment();
                lSegments.Add(objSegment);

                string strRoomRefID = objStayCriteriaSelection.RoomOccupantSelections[ri].RoomRefID;
                HotelRoomAvailInfo objHotelRoomAvailInfo = this.GetHotelRoomAvailInfo(objHotelAvailabilityRS.HotelRoomAvailInfos, strRoomRefID);
                RoomRateSelection objRoomRateSelection = this.GetRoomRateSelection(objRoomRateSelections, strRoomRefID);
                HotelAvailRatePlan objHotelAvailRatePlan = this.GetRatePlanInfo(objHotelRoomAvailInfo.RatePlans, objRoomRateSelection.RatePlanCode);
                AddOnPackageSelection[] objRoomAddOnPackageSelections = this.GetAddOnPackageSelections(objAddOnPackageSelections, strRoomRefID);

                objSegment.SegmentRefID = strRoomRefID;

                objSegment.HotelCode = objStayCriteriaSelection.HotelCode;
                objSegment.ChainCode = ConfigurationManager.AppSettings["HotelChainCode"];

                objSegment.ConfirmationNumber = "";

                objSegment.ArrivalDate = objStayCriteriaSelection.ArrivalDate.Date;
                objSegment.DepartureDate = objStayCriteriaSelection.DepartureDate.Date;

                objSegment.RoomTypeCode = objRoomRateSelection.RoomTypeCode;
                objSegment.RatePlanCode = objRoomRateSelection.RatePlanCode;
                objSegment.PromoCode = objStayCriteriaSelection.PromotionCode;

                objSegment.NumRooms = objStayCriteriaSelection.RoomOccupantSelections[ri].NumberRooms;
                objSegment.NumAdults = objStayCriteriaSelection.RoomOccupantSelections[ri].NumberAdults;
                objSegment.NumChildren = objStayCriteriaSelection.RoomOccupantSelections[ri].NumberChildren;

                // Enhancement: special requests section needs to be "by room" in guest details

                objSegment.NumCribs = objGuestDetailsEntryInfo.NumberCribs;
                objSegment.NumRollawaysAdult = objGuestDetailsEntryInfo.NumberRollawaysAdult;
                objSegment.NumRollawaysChild = objGuestDetailsEntryInfo.NumberRollawaysChild;
                objSegment.SpecialInstructions = objGuestDetailsEntryInfo.SpecialInstructions;

                objSegment.IsBaseOccupancyPricing = false;
                objSegment.NumBaseOccupants = 0;

                // Enhancement: arrival time needs to be "by room" in guest details

                objSegment.FlightNumber = objGuestDetailsEntryInfo.FlightNumber;

                if (objGuestDetailsEntryInfo.ArrivalTime != null && objGuestDetailsEntryInfo.ArrivalTime != "")
                {
                    DateTime dtArrivalTime;

                    if (DateTime.TryParse(objGuestDetailsEntryInfo.ArrivalTime, out dtArrivalTime))
                    {
                        objSegment.ArrivalTime = dtArrivalTime;
                        objSegment.ArrivalTimeSpecified = true;
                    }

                    else
                    {
                        objSegment.ArrivalTime = DateTime.Today.Date;
                        objSegment.ArrivalTimeSpecified = false;
                    }

                }

                else
                {
                    objSegment.ArrivalTime = DateTime.Today.Date;
                    objSegment.ArrivalTimeSpecified = false;
                }

                List<Profile> lProfiles = new List<Profile>();

                Profile objGuest = new Profile();
                lProfiles.Add(objGuest);

                objGuest.Type = ProfileType.Traveler;
                objGuest.PersonNamePrefix = objGuestDetailsEntryInfo.NamePrefix;
                objGuest.PersonFirstName = objGuestDetailsEntryInfo.FirstName;
                objGuest.PersonLastName = objGuestDetailsEntryInfo.LastName;
                objGuest.CompanyName = "";
                objGuest.Address1 = objGuestDetailsEntryInfo.Address1;
                objGuest.Address2 = objGuestDetailsEntryInfo.Address2;
                objGuest.City = objGuestDetailsEntryInfo.City;
                objGuest.StateRegion = objGuestDetailsEntryInfo.StateRegion;
                objGuest.PostalCode = objGuestDetailsEntryInfo.PostalCode;
                objGuest.Country = objGuestDetailsEntryInfo.Country;
                objGuest.Phone = objGuestDetailsEntryInfo.Phone;
                objGuest.Fax = "";
                objGuest.Email = objGuestDetailsEntryInfo.Email;
                objGuest.SubscribeToNewsletter = objGuestDetailsEntryInfo.SubscribeToNewsletter;
                objGuest.TravelPurpose = objGuestDetailsEntryInfo.TravelPurpose;

                List<ProfileIdentifier> lGuestProfileIdentifiers = new List<ProfileIdentifier>();

                if (objGuestDetailsEntryInfo.HotelProgramCode != null && objGuestDetailsEntryInfo.HotelProgramCode != "")
                {
                    ProfileIdentifier objProfileIdentifier = new ProfileIdentifier();
                    lGuestProfileIdentifiers.Add(objProfileIdentifier);

                    objProfileIdentifier.Type = ProfileIdentifierType.HotelLoyaltyProgram;
                    objProfileIdentifier.ProgramCode = objGuestDetailsEntryInfo.HotelProgramCode;
                    objProfileIdentifier.Identifier = objGuestDetailsEntryInfo.HotelProgramIdentifier;
                }

                if (objGuestDetailsEntryInfo.AirlineProgramCode != null && objGuestDetailsEntryInfo.AirlineProgramCode != "")
                {
                    ProfileIdentifier objProfileIdentifier = new ProfileIdentifier();
                    lGuestProfileIdentifiers.Add(objProfileIdentifier);

                    objProfileIdentifier.Type = ProfileIdentifierType.AirlineLoyaltyProgram;
                    objProfileIdentifier.ProgramCode = objGuestDetailsEntryInfo.AirlineProgramCode;
                    objProfileIdentifier.Identifier = objGuestDetailsEntryInfo.AirlineProgramIdentifier;
                }

                objGuest.ProfileIdentifiers = lGuestProfileIdentifiers.ToArray();

                if (objCompanyProfile != null)
                {
                    lProfiles.Add(objCompanyProfile);
                }

                else if (objGuestDetailsEntryInfo.CompanyName != null && objGuestDetailsEntryInfo.CompanyName != "")
                {
                    Profile objCompany = new Profile();
                    lProfiles.Add(objCompany);

                    objCompany.Type = ProfileType.Corporation;
                    objCompany.PersonNamePrefix = "";
                    objCompany.PersonFirstName = "";
                    objCompany.PersonLastName = "";
                    objCompany.CompanyName = objGuestDetailsEntryInfo.CompanyName;
                    objCompany.Address1 = "";
                    objCompany.Address2 = "";
                    objCompany.City = "";
                    objCompany.StateRegion = "";
                    objCompany.PostalCode = "";
                    objCompany.Country = "";
                    objCompany.Phone = "";
                    objCompany.Fax = "";
                    objCompany.Email = "";
                    objCompany.SubscribeToNewsletter = false;
                    objCompany.TravelPurpose = TravelPurpose.NotIdentified;
                    objCompany.ProfileIdentifiers = new ProfileIdentifier[0];
                }

                if (objAgencyProfile != null)
                {
                    lProfiles.Add(objAgencyProfile);
                }

                else if (objGuestDetailsEntryInfo.TravelAgencyIATA != null && objGuestDetailsEntryInfo.TravelAgencyIATA != "")
                {
                    Profile objAgency = new Profile();
                    lProfiles.Add(objAgency);

                    objAgency.Type = ProfileType.TravelAgent;
                    objAgency.PersonNamePrefix = "";
                    objAgency.PersonFirstName = "";
                    objAgency.PersonLastName = "";
                    objAgency.CompanyName = "";
                    objAgency.Address1 = "";
                    objAgency.Address2 = "";
                    objAgency.City = "";
                    objAgency.StateRegion = "";
                    objAgency.PostalCode = "";
                    objAgency.Country = "";
                    objAgency.Phone = "";
                    objAgency.Fax = "";
                    objAgency.Email = "";
                    objAgency.SubscribeToNewsletter = false;
                    objAgency.TravelPurpose = TravelPurpose.NotIdentified;

                    ProfileIdentifier objAgencyIATA = new ProfileIdentifier();
                    objAgencyIATA.Type = ProfileIdentifierType.IATA;
                    objAgencyIATA.Identifier = objGuestDetailsEntryInfo.TravelAgencyIATA;
                    objAgencyIATA.ProgramCode = "";

                    objAgency.ProfileIdentifiers = new ProfileIdentifier[1];
                    objAgency.ProfileIdentifiers[0] = objAgencyIATA;
                }

                objSegment.Profiles = lProfiles.ToArray();

                objSegment.PackageQuantities = new HotelBookingPackageQuantity[objRoomAddOnPackageSelections.Length];

                for (int i = 0; i < objRoomAddOnPackageSelections.Length; i++)
                {
                    HotelBookingPackageQuantity objHotelBookingPackageQuantity = new HotelBookingPackageQuantity();
                    objSegment.PackageQuantities[i] = objHotelBookingPackageQuantity;

                    objHotelBookingPackageQuantity.Code = objRoomAddOnPackageSelections[i].PackageCode;
                    objHotelBookingPackageQuantity.Quantity = objRoomAddOnPackageSelections[i].Quantity;
                }

                objSegment.GuaranteeType = objHotelAvailRatePlan.GuaranteeType;
                objSegment.PaymentCard = null;
                objSegment.DepositPaymentAmount = 0;
                objSegment.PaymentAuthCode = "";
                objSegment.PaymentTransRefID = "";

                if (objHotelAvailRatePlan.GuaranteeType == GuaranteeType.CCDCVoucher && objGuestDetailsEntryInfo.ProfileGuaranteeRequested)
                {
                    objSegment.GuaranteeType = GuaranteeType.Profile;
                }

                else if (objHotelAvailRatePlan.GuaranteeType == GuaranteeType.CCDCVoucher)
                {
                    HotelBookingPaymentCard objPaymentCard = new HotelBookingPaymentCard();
                    objSegment.PaymentCard = objPaymentCard;

                    objPaymentCard.PaymentCardType = objGuestDetailsEntryInfo.PaymentCardType;
                    objPaymentCard.PaymentCardNumber = objGuestDetailsEntryInfo.PaymentCardNumber;
                    objPaymentCard.PaymentCardHolder = objGuestDetailsEntryInfo.PaymentCardHolder;
                    objPaymentCard.PaymentCardEffectiveDate = objGuestDetailsEntryInfo.PaymentCardEffectiveDate;
                    objPaymentCard.PaymentCardExpireDate = objGuestDetailsEntryInfo.PaymentCardExpireDate;
                    objPaymentCard.PaymentCardIssueNumber = objGuestDetailsEntryInfo.PaymentCardIssueNumber;
                    objPaymentCard.PaymentCardSecurityCode = objGuestDetailsEntryInfo.PaymentCardSecurityCode;

                    objSegment.CurrencyCode = objHotelAvailRatePlan.DepositRequiredCurrencyCode;
                }

                else if (objHotelAvailRatePlan.GuaranteeType == GuaranteeType.Deposit || objHotelAvailRatePlan.GuaranteeType == GuaranteeType.PrePay)
                {
                    HotelBookingPaymentCard objPaymentCard = new HotelBookingPaymentCard();
                    objSegment.PaymentCard = objPaymentCard;

                    objPaymentCard.PaymentCardType = objGuestDetailsEntryInfo.PaymentCardType;
                    objPaymentCard.PaymentCardNumber = objGuestDetailsEntryInfo.PaymentCardNumber;
                    objPaymentCard.PaymentCardHolder = objGuestDetailsEntryInfo.PaymentCardHolder;
                    objPaymentCard.PaymentCardEffectiveDate = objGuestDetailsEntryInfo.PaymentCardEffectiveDate;
                    objPaymentCard.PaymentCardExpireDate = objGuestDetailsEntryInfo.PaymentCardExpireDate;
                    objPaymentCard.PaymentCardIssueNumber = objGuestDetailsEntryInfo.PaymentCardIssueNumber;
                    objPaymentCard.PaymentCardSecurityCode = objGuestDetailsEntryInfo.PaymentCardSecurityCode;

                    objSegment.CurrencyCode = objHotelAvailRatePlan.DepositRequiredCurrencyCode;

                    decimal decDepositPaymentAmount = WBSPGHelper.GetBookingSegmentPayment(objHotelBookingPaymentAllocations, strRoomRefID);

                    if (bIsOnlinePayment && decDepositPaymentAmount != 0)
                    {
                        objSegment.PaymentCard = objHotelPaymentRS.PaymentCard;
                        objSegment.DepositPaymentAmount = decDepositPaymentAmount;
                        objSegment.PaymentAuthCode = objHotelPaymentRS.PaymentAuthCode;
                        objSegment.PaymentTransRefID = objHotelPaymentRS.PaymentTransRefID;

                        objSegment.PaymentCard.PaymentCardSecurityCode = ""; // remove security code (no longer needed)
                    }

                }

            }

        }

        if (enumBookingAction == BookingAction.Cancel)
        {
            for (int ci = 0; ci < objCancelDetailsEntryInfo.SelectedConfirmationNumbersToCancel.Length; ci++)
            {
                HotelBookingRequestSegment objSegment = new HotelBookingRequestSegment();
                lSegments.Add(objSegment);

                objSegment.SegmentRefID = ((int)(ci + 1)).ToString();
                objSegment.ConfirmationNumber = objCancelDetailsEntryInfo.SelectedConfirmationNumbersToCancel[ci];
            }

        }

        objHotelBookingRQ.Segments = lSegments.ToArray();

        gwWBS.InitHotelBookingRQ(ref objWBSAPIRouterData, objHotelBookingRQ);

        return;
    }

    public bool ProcessBookHotelRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelBookingRS objHotelBookingRS = gwWBS.ProcessHotelBookingRS(ref objWBSAPIRouterData);

        Session["HotelBookingRS"] = objHotelBookingRS;

        return !this.ProcessResponseErrors(objHotelBookingRS.ResponseHeader);
    }

    public void InitReadHotelBookingRQ(ref WBSAPIRouterData objWBSAPIRouterData, string strConfirmationNumber)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelBookingReadRQ objHotelBookingReadRQ = new HotelBookingReadRQ();

        objHotelBookingReadRQ.RequestHeader = this.GetRequestHeader();

        objHotelBookingReadRQ.BookingReferenceNumberType = BookingReferenceNumberType.ConfirmationNumber;
        objHotelBookingReadRQ.BookingReferenceNumber = strConfirmationNumber;

        gwWBS.InitHotelBookingRetrievalRQ(ref objWBSAPIRouterData, objHotelBookingReadRQ);

        return;
    }

    public bool ProcessReadHotelBookingRS(ref WBSAPIRouterData objWBSAPIRouterData)
    {
        WBSAPIRouterGateway gwWBS = new WBSAPIRouterGateway(objEventLog, objExceptionEventLog, bIsProduction);

        HotelBookingReadRS objHotelBookingReadRS = gwWBS.ProcessHotelBookingRetrievalRS(ref objWBSAPIRouterData);

        Session["HotelBookingReadRS"] = objHotelBookingReadRS;

        return !this.ProcessResponseErrors(objHotelBookingReadRS.ResponseHeader);
    }

    private RequestHeader GetRequestHeader()
    {
        RequestHeader objRequestHeader = new RequestHeader();

        objRequestHeader.ServiceRequesterID = ConfigurationManager.AppSettings["ServiceRequesterID"];
        objRequestHeader.ServiceRequestorPassword = ConfigurationManager.AppSettings["ServiceRequestorPassword"];
        objRequestHeader.DistributionChannelCode = ConfigurationManager.AppSettings["DistributionChannelCode"];
        objRequestHeader.DistributionChannelName = ConfigurationManager.AppSettings["DistributionChannelName"];
        objRequestHeader.DistributionChannelSubCode = ConfigurationManager.AppSettings["DistributionChannelSubCode"];
        objRequestHeader.LanguageID = (string)Session["SelectedCulture"];

        return objRequestHeader;
    }

    private string[] GetHotelBrandCodes()
    {
        if (ConfigurationManager.AppSettings["HotelBrandCodes"] == "")
            return new string[0];

        return ConfigurationManager.AppSettings["HotelBrandCodes"].Split(new char[] { ';' });
    }

    private HotelRoomAvailInfo GetHotelRoomAvailInfo(HotelRoomAvailInfo[] objHotelRoomAvailInfos, string strRoomRefID)
    {
        for (int i = 0; i < objHotelRoomAvailInfos.Length; i++)
        {
            if (objHotelRoomAvailInfos[i].SegmentRefID == strRoomRefID)
                return objHotelRoomAvailInfos[i];
        }

        return null;
    }

    private RoomRateSelection GetRoomRateSelection(RoomRateSelection[] objRoomRateSelections, string strRoomRefID)
    {
        for (int i = 0; i < objRoomRateSelections.Length; i++)
        {
            if (objRoomRateSelections[i].RoomRefID == strRoomRefID)
                return objRoomRateSelections[i];
        }

        return null;
    }

    private HotelAvailRatePlan GetRatePlanInfo(HotelAvailRatePlan[] objHotelAvailRatePlans, string strRatePlanCode)
    {
        for (int i = 0; i < objHotelAvailRatePlans.Length; i++)
        {
            if (objHotelAvailRatePlans[i].Code == strRatePlanCode)
                return objHotelAvailRatePlans[i];
        }

        return null;
    }

    private AddOnPackageSelection[] GetAddOnPackageSelections(AddOnPackageSelection[] objAddOnPackageSelections, string strRoomRefID)
    {
        List<AddOnPackageSelection> lAddOnPackageSelections = new List<AddOnPackageSelection>();

        for (int i = 0; i < objAddOnPackageSelections.Length; i++)
        {
            if (objAddOnPackageSelections[i].RoomRefID == strRoomRefID)
                lAddOnPackageSelections.Add(objAddOnPackageSelections[i]);
        }

        return lAddOnPackageSelections.ToArray();
    }

    private bool ProcessResponseErrors(ResponseHeader objResponseHeader)
    {
        bool bErrors = false;

        if (!objResponseHeader.Success)
        {
            for (int i = 0; i < objResponseHeader.Errors.Length; i++)
            {
                ((XnGR_WBS_Page)Page).AddPageError(XnGR_WBS_Page.PageErrorType.WbsApiError, objResponseHeader.Errors[i].Code);
                bErrors = true;
            }

        }

        return bErrors;
    }

}