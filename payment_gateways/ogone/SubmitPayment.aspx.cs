using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.OgoneHelper;
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class Ogone_SubmitPayment : System.Web.UI.Page 
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

            PayRqData objPayRqData = new PayRqData();

            objPayRqData.strMerchantID = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.OGONE_MERCHANT_ID];
            objPayRqData.strUserID = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.OGONE_USER_ID];
            objPayRqData.strPassword = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.OGONE_USER_PASSWORD];
            objPayRqData.strSecureSecret = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.OGONE_SECURE_SECRET];

            objPayRqData.strTrnRefNumber = objHotelPaymentRQ.PaymentTransRefID;
            objPayRqData.strTrnOrderInfo = objHotelPaymentRQ.PaymentTransInfo;

            objPayRqData.decAmount = WBSPGHelper.GetTotalPaymentCardPayment(objHotelPaymentRQ.PaymentAmounts);
            objPayRqData.strCurrency = objHotelPaymentRQ.CurrencyCode;

            DateTime dtPaymentCardExpirationDate = WBSPGHelper.ExpirationDate(objHotelPaymentRQ.PaymentCard.PaymentCardExpireDate);

            objPayRqData.enumCardType = WBSPGHelper.OgoneCardType(objHotelPaymentRQ.PaymentCard.PaymentCardType);
            objPayRqData.strCardNumber = objHotelPaymentRQ.PaymentCard.PaymentCardNumber;
            objPayRqData.intCardExpirationMonth = dtPaymentCardExpirationDate.Month;
            objPayRqData.intCardExpirationYear = dtPaymentCardExpirationDate.Year;
            objPayRqData.strCardSecurityCode = objHotelPaymentRQ.PaymentCard.PaymentCardSecurityCode;

            objPayRqData.strDisplayLocale = objHotelPaymentRQ.CultureCode.Replace('-', '_');

            StringBuilder sbReturnURL = new StringBuilder();

            if (Request.IsSecureConnection)
                sbReturnURL.Append("https://");
            else
                sbReturnURL.Append("http://");

            sbReturnURL.Append(Request.ServerVariables["HTTP_HOST"]);

            Uri baseURL = new Uri(sbReturnURL.ToString());

            objPayRqData.strReturnURL = (new Uri(baseURL, Response.ApplyAppPathModifier("Pay3DSecReceipt.aspx"))).ToString();

            OgoneDataProcessor objProcessor = new OgoneDataProcessor(objEventLog, objExceptionEventLog);

            PayRsData objPayRsData = objProcessor.SubmitPayRequest(objPayRqData, Request);

            if (objPayRsData.enumStatus == PayRSStatus.ThreeDSecureRequired)
            {
                this.lit3DSecureData.Text = objPayRsData.str3DSecureRequestData;

                Session["PaymentGatewayResponseActive"] = true;
            }

            else
            {
                HotelPaymentRS objHotelPaymentRS = new HotelPaymentRS();

                objHotelPaymentRS.RequestTransID = objHotelPaymentRQ.RequestTransID;

                if (objPayRsData.enumStatus == PayRSStatus.Authorized)
                {
                    objHotelPaymentRS.PaymentAuthCode = objPayRsData.strAuthCode;
                    objHotelPaymentRS.PaymentTransRefID = objHotelPaymentRQ.PaymentTransRefID;
                    objHotelPaymentRS.PaymentGatewayCardType = "";
                    objHotelPaymentRS.PaymentCard = objHotelPaymentRQ.PaymentCard;

                    objHotelPaymentRS.Success = true;

                    objHotelPaymentRS.Errors = new Error[0];
                    objHotelPaymentRS.Warnings = new Warning[0];
                }

                else
                {
                    objHotelPaymentRS.PaymentAuthCode = "";
                    objHotelPaymentRS.PaymentTransRefID = "";
                    objHotelPaymentRS.PaymentGatewayCardType = "";
                    objHotelPaymentRS.PaymentCard = null;

                    objHotelPaymentRS.Success = false;

                    objHotelPaymentRS.Errors = new Error[1];
                    objHotelPaymentRS.Errors[0] = new Error();
                    objHotelPaymentRS.Errors[0].Code = objPayRsData.enumStatus.ToString();
                    objHotelPaymentRS.Errors[0].Description = objPayRsData.strErrorMessage;

                    objHotelPaymentRS.Warnings = new Warning[0];
                }

                Session["HotelPaymentRS"] = objHotelPaymentRS;

                Server.Transfer("~/Pages/ProcessPaymentRS.aspx");
            }

        }

        return;
    }

}
