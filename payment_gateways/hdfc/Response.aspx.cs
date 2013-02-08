using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.HDFCHelper;
using XHS.WBSUIBizObjects;

public partial class HDFC_Response : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool bPaymentGatewayResponseActive = (bool)Session["PaymentGatewayResponseActive"];

        if (!bPaymentGatewayResponseActive)
        {
            Response.Redirect("~/Default.aspx");
        }

        else
        {
            Session["PaymentGatewayResponseActive"] = false;

            FileLog objEventLog = (FileLog)Application["EventLog"];
            ExceptionLog objExceptionEventLog = (ExceptionLog)Application["ExceptionEventLog"];

            HotelPaymentRQ objHotelPaymentRQ = (HotelPaymentRQ)Session["HotelPaymentRQ"];

            HDFCDataProcessor objHDFCDataProcessor = new HDFCDataProcessor(objEventLog, objExceptionEventLog);
            PayRsData objPayRsData = objHDFCDataProcessor.ProcessPayResponse(this, this.Context);

            HotelPaymentRS objHotelPaymentRS = new HotelPaymentRS();

            objHotelPaymentRS.RequestTransID = objHotelPaymentRQ.RequestTransID;

            if (objPayRsData.enumStatus == PayRSStatus.Authorized)
            {
                objHotelPaymentRS.PaymentAuthCode = objPayRsData.strAuthCode;
                objHotelPaymentRS.PaymentTransRefID = objPayRsData.strTranID;
                objHotelPaymentRS.PaymentGatewayCardType = "";

                objHotelPaymentRS.PaymentCard = new HotelBookingPaymentCard();
                objHotelPaymentRS.PaymentCard.PaymentCardType = WBSPGHelper.HDFCPostingCardType();
                objHotelPaymentRS.PaymentCard.PaymentCardNumber = "xxxxxxxxxxxxxxxx";
                objHotelPaymentRS.PaymentCard.PaymentCardHolder = objPayRsData.strTranID;
                objHotelPaymentRS.PaymentCard.PaymentCardEffectiveDate = "";
                objHotelPaymentRS.PaymentCard.PaymentCardExpireDate = "";
                objHotelPaymentRS.PaymentCard.PaymentCardIssueNumber = "";
                objHotelPaymentRS.PaymentCard.PaymentCardSecurityCode = "";

                objHotelPaymentRS.Success = true;

                objHotelPaymentRS.Errors = new Error[0];
                objHotelPaymentRS.Warnings = new Warning[0];
            }

            else // should never land here -- should have gone to Error.aspx page
            {
                objHotelPaymentRS.PaymentAuthCode = "";
                objHotelPaymentRS.PaymentTransRefID = "";
                objHotelPaymentRS.PaymentGatewayCardType = "";
                objHotelPaymentRS.PaymentCard = null;

                objHotelPaymentRS.Success = false;

                objHotelPaymentRS.Errors = new Error[1];
                objHotelPaymentRS.Errors[0] = new Error();
                objHotelPaymentRS.Errors[0].Code = objPayRsData.enumStatus.ToString();
                objHotelPaymentRS.Errors[0].Description = objPayRsData.strMessage;

                objHotelPaymentRS.Warnings = new Warning[0];
            }

            Session["HotelPaymentRS"] = objHotelPaymentRS;

            Server.Transfer("~/Pages/ProcessPaymentRS.aspx");
        }

        return;
    }

}
