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
using XHS.MIGS3VPCHelper;
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class MIGS3P_PayReceipt : System.Web.UI.Page
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
            PayRqData objPayRqData = (PayRqData)Session["XHS.MIGS3VPCHelper.PayRqData"];

            VPCDataProcessor objProcessor = new VPCDataProcessor(objEventLog, objExceptionEventLog, true);

            PayRsData objPayRsData = null;

            if (objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.MIGS3P_AVS_ACTIVE] == "1")
                objPayRsData = objProcessor.ProcessPayResponseAndCapture(Page.Request, objPayRqData, objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.MIGS3P_AVS_RETURN_CODES]);
            else
                objPayRsData = objProcessor.ProcessPayResponse(Page.Request, objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.MIGS3P_SECURE_HASH_SECRET]);

            HotelPaymentRS objHotelPaymentRS = new HotelPaymentRS();

            objHotelPaymentRS.RequestTransID = objHotelPaymentRQ.RequestTransID;

            if (objPayRsData.enumStatus == PayRSStatus.Authorized)
            {
                objHotelPaymentRS.PaymentAuthCode = objPayRsData.strAuthCode;
                objHotelPaymentRS.PaymentTransRefID = objHotelPaymentRQ.PaymentTransRefID;
                objHotelPaymentRS.PaymentGatewayCardType = objPayRsData.strCardType;

                objHotelPaymentRS.PaymentCard = new HotelBookingPaymentCard();
                objHotelPaymentRS.PaymentCard.PaymentCardType = WBSPGHelper.MIGS3PPostingCardType(objPayRsData.strCardType);

                if (objPayRsData.strCarNumMasked != null && objPayRsData.strCarNumMasked != "")
                    objHotelPaymentRS.PaymentCard.PaymentCardNumber = objPayRsData.strCarNumMasked;
                else
                    objHotelPaymentRS.PaymentCard.PaymentCardNumber = "xxxxxxxxxxxxxxxx";

                objHotelPaymentRS.PaymentCard.PaymentCardHolder = objPayRsData.strCardType + ", " + objHotelPaymentRQ.PaymentTransRefID;
                objHotelPaymentRS.PaymentCard.PaymentCardEffectiveDate = "";
                objHotelPaymentRS.PaymentCard.PaymentCardExpireDate = "";
                objHotelPaymentRS.PaymentCard.PaymentCardIssueNumber = "";
                objHotelPaymentRS.PaymentCard.PaymentCardSecurityCode = "";

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
                objHotelPaymentRS.Errors[0].Description = objPayRsData.strMessage;

                objHotelPaymentRS.Warnings = new Warning[0];
            }

            Session["HotelPaymentRS"] = objHotelPaymentRS;

            Server.Transfer("~/Pages/ProcessPaymentRS.aspx");
        }

        return;
    }

}
