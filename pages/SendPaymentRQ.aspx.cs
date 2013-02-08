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

public partial class SendPaymentRQ : XnGR_WBS_Page
{
    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        PaymentGatewayInfo objPaymentGatewayInfo = (PaymentGatewayInfo)Session[Constants.Sessions.PaymentGatewayInfo];
        HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations = (HotelBookingPaymentAllocation[])Session[Constants.Sessions.HotelBookingPaymentAllocations];
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
        AddOnPackageSelection[] objAddOnPackageSelections = (AddOnPackageSelection[])Session["AddOnPackageSelections"];
        GuestDetailsEntryInfo objGuestDetailsEntryInfo = (GuestDetailsEntryInfo)Session["GuestDetailsEntryInfo"];

        HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        HotelPaymentRQ objHotelPaymentRQ = WBSPGHelper.GetHotelPaymentRQ(objPaymentGatewayInfo, objHotelBookingPaymentAllocations, objHotelDescriptiveInfo, objGuestDetailsEntryInfo, WbsUiHelper.SelectedCulture, WbsUiHelper.SelectedUICulture);
        Session["HotelPaymentRQ"] = objHotelPaymentRQ;
        Session["HotelPaymentRS"] = null;

        string strPendingPrepayBookingID = Guid.NewGuid().ToString();

        PendingPrepayBookingInfo objPendingPrepayBookingInfo = new PendingPrepayBookingInfo();
        objPendingPrepayBookingInfo.PostingDateTime = DateTime.Now;
        objPendingPrepayBookingInfo.HotelName = objHotelDescriptiveInfo.HotelName;
        objPendingPrepayBookingInfo.StayCriteria = objStayCriteriaSelection;
        objPendingPrepayBookingInfo.RoomRates = objRoomRateSelections;
        objPendingPrepayBookingInfo.AddOnPackages = objAddOnPackageSelections;
        objPendingPrepayBookingInfo.GuestDetails = objGuestDetailsEntryInfo;
        objPendingPrepayBookingInfo.PaymentRequestInfo = objHotelPaymentRQ;

        Session["PendingPrepayBookingID"] = strPendingPrepayBookingID;
        Session["PendingPrepayBookingInfo"] = objPendingPrepayBookingInfo;

        string strPendingPrepayBookingInfo = Serializer.ToString(objPendingPrepayBookingInfo);

        this.WbsMonitor.AddItem(strPendingPrepayBookingID, strPendingPrepayBookingInfo);

        Session["PaymentGatewayRequestActive"] = true;
        Session["PaymentGatewayResponseActive"] = false;

        this.PageComplete();

        Response.Redirect(objPaymentGatewayInfo.SubmitPaymentURL);

        return;
    }

}
