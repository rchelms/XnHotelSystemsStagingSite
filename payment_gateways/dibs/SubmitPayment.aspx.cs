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
using XHS.DIBSHelper;
using XHS.WBSUIBizObjects;

public partial class DIBS_SubmitPayment : System.Web.UI.Page
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

            objPayRqData.strMerchantID = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.DIBS_MERCHANT_ID];
            objPayRqData.strSecureHashSecret1 = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.DIBS_SECURE_SECRET_1];
            objPayRqData.strSecureHashSecret2 = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.DIBS_SECURE_SECRET_2];

            objPayRqData.litPayData = litPayData;

            objPayRqData.strTrnRefNumber = objHotelPaymentRQ.PaymentTransRefID;
            objPayRqData.strTrnOrderInfo = objHotelPaymentRQ.PaymentTransInfo;

            objPayRqData.decAmount = WBSPGHelper.GetTotalPaymentCardPayment(objHotelPaymentRQ.PaymentAmounts);
            objPayRqData.strCurrency = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.DIBS_CURRENCY_CODE_NUMBER];

            if (objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.DIBS_SYSTEM_MODE] == "prod")
                objPayRqData.bTestTransaction = false;
            else
                objPayRqData.bTestTransaction = true;

            objPayRqData.strDisplayLocale = objHotelPaymentRQ.UICultureCode;

            StringBuilder sbReturnURL = new StringBuilder();

            if (Request.IsSecureConnection)
                sbReturnURL.Append("https://");
            else
                sbReturnURL.Append("http://");

            sbReturnURL.Append(Request.ServerVariables["HTTP_HOST"]);

            Uri baseURL = new Uri(sbReturnURL.ToString());

            objPayRqData.strReturnURL = (new Uri(baseURL, Response.ApplyAppPathModifier("PayReceipt.aspx"))).ToString();
            objPayRqData.strCallbackURL = (new Uri(baseURL, Response.ApplyAppPathModifier("Callback.aspx"))).ToString();

            objPayRqData.strEchoData = (string)Session["PendingPrepayBookingID"];

            DIBSDataProcessor objProcessor = new DIBSDataProcessor(objEventLog, objExceptionEventLog, true);

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
