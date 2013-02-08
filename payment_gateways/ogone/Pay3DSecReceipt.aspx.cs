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
using XHS.OgoneHelper;
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class Ogone_Pay3DSecReceipt : System.Web.UI.Page
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

            OgoneDataProcessor objDataProcessor = new OgoneDataProcessor(objEventLog, objExceptionEventLog);
            PayRsData objPayRsData = objDataProcessor.ProcessPayResponse(Page.Request, objHotelPaymentRQ.PaymentGateway.ConfigurationParameters[WBSPGHelper.OGONE_SECURE_SECRET]);

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

        return;
    }

}
