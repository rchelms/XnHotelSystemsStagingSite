using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.HDFCHelper;
using XHS.WBSUIBizObjects;

public partial class HDFC_SubmitPayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");

        bool bPaymentGatewayRequestActive = (bool)Session["PaymentGatewayRequestActive"];

        if (!bPaymentGatewayRequestActive)
        {
            Response.Redirect("~/Default.aspx");
        }

        else
        {
            Session["PaymentGatewayRequestActive"] = false;

            FileLog objEventLog = (FileLog)Application["EventLog"];
            ExceptionLog objExceptionEventLog = (ExceptionLog)Application["ExceptionEventLog"];

            HotelPaymentRQ objHotelPaymentRQ = (HotelPaymentRQ)Session["HotelPaymentRQ"];
            HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];
            StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
            RoomRateSelection[] objRoomRateSelections = (RoomRateSelection[])Session["RoomRateSelections"];
            GuestDetailsEntryInfo objGuestDetailsEntryInfo = (GuestDetailsEntryInfo)Session["GuestDetailsEntryInfo"];

            PayRqData objPayRqData = new PayRqData();

            objPayRqData.strAlias = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.HDFC_TID_ALIAS];
            objPayRqData.strResourcePath = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.HDFC_TID_RESOURCE_PATH];

            objPayRqData.strTrnRefNumber = objHotelPaymentRQ.PaymentTransRefID;

            objPayRqData.decAmount = WBSPGHelper.GetTotalPaymentCardPayment(objHotelPaymentRQ.PaymentAmounts);
            objPayRqData.strCurrency = "356";
            objPayRqData.strLanguage = "USA";

            if (objHotelDescriptiveInfoRS != null && objHotelDescriptiveInfoRS.HotelDescriptiveInfos != null && objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length > 0)
                objPayRqData.strHotelName = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0].HotelName;
            else
                objPayRqData.strHotelName = "Unknown"; // should never happen

            if (objStayCriteriaSelection.RoomOccupantSelections != null && objStayCriteriaSelection.RoomOccupantSelections.Length > 0)
                objPayRqData.intNumberGuests = objStayCriteriaSelection.RoomOccupantSelections[0].NumberAdults;
            else
                objPayRqData.intNumberGuests = 0; // should never happen

            if (objRoomRateSelections != null && objRoomRateSelections.Length > 0)
                objPayRqData.strRoomTypeName = objRoomRateSelections[0].RoomTypeCode;
            else
                objPayRqData.strRoomTypeName = "Unknown"; // should never happen

            objPayRqData.dtCheckinDate = objStayCriteriaSelection.ArrivalDate.Date;
            objPayRqData.dtCheckoutDate = objStayCriteriaSelection.DepartureDate.Date;
            objPayRqData.strGuestEmail = objGuestDetailsEntryInfo.Email;
            objPayRqData.strGuestAddress = objGuestDetailsEntryInfo.Address1 + " " + objGuestDetailsEntryInfo.Address2 + " " + objGuestDetailsEntryInfo.City + " " + objGuestDetailsEntryInfo.StateRegion + " " + objGuestDetailsEntryInfo.PostalCode + " " + objGuestDetailsEntryInfo.Country;
            objPayRqData.strGuestPhoneNumber = objGuestDetailsEntryInfo.Phone;

            StringBuilder sbReturnURL = new StringBuilder();

            if (Request.IsSecureConnection)
                sbReturnURL.Append("https://");
            else
                sbReturnURL.Append("http://");

            sbReturnURL.Append(Request.ServerVariables["HTTP_HOST"]);

            Uri baseURL = new Uri(sbReturnURL.ToString());

            objPayRqData.strReturnURL = (new Uri(baseURL, Response.ApplyAppPathModifier("Redirect.aspx"))).ToString();
            objPayRqData.strErrorURL = (new Uri(baseURL, Response.ApplyAppPathModifier("Error.aspx"))).ToString();

            HDFCDataProcessor objHDFCDataProcessor = new HDFCDataProcessor(objEventLog, objExceptionEventLog);

            HDFCInitResult objHDFCInitResult = objHDFCDataProcessor.PreparePayRequest(objPayRqData, this);

            if (objHDFCInitResult.bSuccess)
            {
                Session["PaymentGatewayResponseActive"] = true;
                Response.Redirect(objHDFCInitResult.strRedirectURL);
            }

            else
            {
                HotelPaymentRS objHotelPaymentRS = new HotelPaymentRS();

                objHotelPaymentRS.RequestTransID = objHotelPaymentRQ.RequestTransID;

                objHotelPaymentRS.PaymentAuthCode = "";
                objHotelPaymentRS.PaymentTransRefID = "";
                objHotelPaymentRS.PaymentGatewayCardType = "";
                objHotelPaymentRS.PaymentCard = null;

                objHotelPaymentRS.Success = false;

                objHotelPaymentRS.Errors = new Error[1];
                objHotelPaymentRS.Errors[0] = new Error();
                objHotelPaymentRS.Errors[0].Code = "FatalError";

                objHotelPaymentRS.Warnings = new Warning[0];

                Session["HotelPaymentRS"] = objHotelPaymentRS;

                Server.Transfer("~/Pages/ProcessPaymentRS.aspx");
            }

        }

        return;
    }

}
