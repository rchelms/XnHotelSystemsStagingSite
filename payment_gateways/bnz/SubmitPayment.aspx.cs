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
using XHS.BNZHelper;
using XHS.WBSUIBizObjects;

public partial class BNZ_SubmitPayment : System.Web.UI.Page
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

            objPayRqData.strMerchantID = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.BNZ_MERCHANT_ID];
            objPayRqData.strClientCertificatePath = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.BNZ_CLIENT_CERT_PATH];
            objPayRqData.strClientCertificatePassword = objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.BNZ_CLIENT_CERT_PASSWORD];

            objPayRqData.strTrnRefNumber = objHotelPaymentRQ.PaymentTransRefID;
            objPayRqData.strTrnOrderInfo = objHotelPaymentRQ.PaymentTransInfo;

            objPayRqData.decAmount = WBSPGHelper.GetTotalPaymentCardPayment(objHotelPaymentRQ.PaymentAmounts);

            DateTime dtPaymentCardExpirationDate = WBSPGHelper.ExpirationDate(objHotelPaymentRQ.PaymentCard.PaymentCardExpireDate);

            objPayRqData.enumCardType = WBSPGHelper.BNZCardType(objHotelPaymentRQ.PaymentCard.PaymentCardType);
            objPayRqData.strCardholderName = objHotelPaymentRQ.PaymentCard.PaymentCardHolder;
            objPayRqData.strCardNumber = objHotelPaymentRQ.PaymentCard.PaymentCardNumber;
            objPayRqData.intCardExpirationMonth = dtPaymentCardExpirationDate.Month;
            objPayRqData.intCardExpirationYear = dtPaymentCardExpirationDate.Year;
            objPayRqData.strCardSecurityCode = objHotelPaymentRQ.PaymentCard.PaymentCardSecurityCode;

            BNZDataProcessor objProcessor = new BNZDataProcessor(objEventLog, objExceptionEventLog);

            PayRsData objPayRsData = objProcessor.SubmitPayRequest(objPayRqData);

            HotelPaymentRS objHotelPaymentRS = new HotelPaymentRS();

            objHotelPaymentRS.RequestTransID = objHotelPaymentRQ.RequestTransID;

            if (objPayRsData.enumStatus == PayRSStatus.Authorized)
            {
                objHotelPaymentRS.PaymentAuthCode = objPayRsData.strAuthCode;
                objHotelPaymentRS.PaymentTransRefID = objHotelPaymentRQ.PaymentTransRefID;
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

        return;
    }

}
