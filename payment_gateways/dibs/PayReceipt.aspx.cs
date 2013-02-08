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
using XHS.DIBSHelper;
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class DIBS_PayReceipt : System.Web.UI.Page
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

            DIBSDataProcessor objProcessor = new DIBSDataProcessor(objEventLog, objExceptionEventLog, true);
            PayRsData objPayRsData = objProcessor.ProcessPayResponse(Page.Request, objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.DIBS_SECURE_SECRET_1], objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.DIBS_SECURE_SECRET_2]);

            HotelPaymentRS objHotelPaymentRS = new HotelPaymentRS();

            objHotelPaymentRS.RequestTransID = objHotelPaymentRQ.RequestTransID;

            WBSMonitor wbsMonitor = WBSMonitor.GetWbsMonitor(Context.Cache, objEventLog, objExceptionEventLog, XnGR_WBS_Page.IsProductionMode(), (int)Application["WBSMonitor.ExpirationSeconds"]);
            wbsMonitor.UpdateItem(objPayRsData.strEchoData, WBSMonitor.MonitorUpdateType.AcceptReceived);

            if (objPayRsData.enumStatus == PayRSStatus.Authorized)
            {
                objHotelPaymentRS.PaymentAuthCode = objPayRsData.strAuthCode;
                objHotelPaymentRS.PaymentTransRefID = objHotelPaymentRQ.PaymentTransRefID;
                objHotelPaymentRS.PaymentGatewayCardType = objPayRsData.strPaymentType;

                objHotelPaymentRS.PaymentCard = new HotelBookingPaymentCard();
                objHotelPaymentRS.PaymentCard.PaymentCardType = WBSPGHelper.DIBSPostingCardType(objPayRsData.strPaymentType);
                objHotelPaymentRS.PaymentCard.PaymentCardNumber = objPayRsData.strCardNumberMasked;
                objHotelPaymentRS.PaymentCard.PaymentCardHolder = objPayRsData.strPaymentType + ", " + objPayRsData.strTransact;
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
                objHotelPaymentRS.Errors[0].Description = "";

                objHotelPaymentRS.Warnings = new Warning[0];
            }

            Session["HotelPaymentRS"] = objHotelPaymentRS;

            Server.Transfer("~/Pages/ProcessPaymentRS.aspx");
        }

        return;
    }

}
