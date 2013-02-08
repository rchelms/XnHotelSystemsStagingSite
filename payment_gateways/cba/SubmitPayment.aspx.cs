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
using XHS.CommWebVPCHelper;
using XHS.WBSUIBizObjects;

public partial class CBA_SubmitPayment : System.Web.UI.Page
{
    public string strAction = "";

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

            PayRqData objPayRqData = new PayRqData();

            objPayRqData.strMerchantID = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.CBA_MERCHANT_ID];
            objPayRqData.strMerchantAccessCode = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.CBA_MERCHANT_ACCESS_CODE];
            objPayRqData.strSecureHashSecret = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.CBA_SECURE_HASH_SECRET];

            objPayRqData.litPayData = litPayData;

            objPayRqData.strTrnRefNumber = objHotelPaymentRQ.PaymentTransRefID;
            objPayRqData.strTrnOrderInfo = objHotelPaymentRQ.PaymentTransInfo;

            objPayRqData.decAmount = WBSPGHelper.GetTotalPaymentCardPayment(objHotelPaymentRQ.PaymentAmounts);

            DateTime dtPaymentCardExpirationDate = WBSPGHelper.ExpirationDate(objHotelPaymentRQ.PaymentCard.PaymentCardExpireDate);

            objPayRqData.enumCardType = WBSPGHelper.CBACardType(objHotelPaymentRQ.PaymentCard.PaymentCardType);
            objPayRqData.strCardNumber = objHotelPaymentRQ.PaymentCard.PaymentCardNumber;
            objPayRqData.intCardExpirationMonth = dtPaymentCardExpirationDate.Month;
            objPayRqData.intCardExpirationYear = dtPaymentCardExpirationDate.Year;
            objPayRqData.strCardSecurityCode = objHotelPaymentRQ.PaymentCard.PaymentCardSecurityCode;

            objPayRqData.strDisplayLocale = objHotelPaymentRQ.UICultureCode;

            objPayRqData.strTicketNo = "";

            StringBuilder sbReturnURL = new StringBuilder();

            if (Request.IsSecureConnection)
                sbReturnURL.Append("https://");
            else
                sbReturnURL.Append("http://");

            sbReturnURL.Append(Request.ServerVariables["HTTP_HOST"]);

            Uri baseURL = new Uri(sbReturnURL.ToString());

            objPayRqData.strReturnURL = (new Uri(baseURL, Response.ApplyAppPathModifier("PayReceipt.aspx"))).ToString();

            VPCDataProcessor objProcessor = new VPCDataProcessor(objEventLog, objExceptionEventLog, true);

            if (objProcessor.PreparePayRequest(objPayRqData, out strAction))
            {
                Session["PaymentGatewayResponseActive"] = true;
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
