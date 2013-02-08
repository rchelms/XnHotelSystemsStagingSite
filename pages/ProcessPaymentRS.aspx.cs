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

public partial class ProcessPaymentRS : XnGR_WBS_Page
{
   protected override void Page_Init(object sender, EventArgs e)
   {
      base.Page_Init(sender, e);

      return;
   }

   protected override void Page_Load(object sender, EventArgs e)
   {
      base.Page_Load(sender, e);

      this.PageComplete();

      HotelPaymentRQ objHotelPaymentRQ = (HotelPaymentRQ)Session["HotelPaymentRQ"];
      HotelPaymentRS objHotelPaymentRS = (HotelPaymentRS)Session["HotelPaymentRS"];

      if (objHotelPaymentRQ.PaymentGateway.Type != PaymentGatewayType.DIBS || !objHotelPaymentRS.Success)
      {
         this.WbsMonitor.RemoveItem((string)Session["PendingPrepayBookingID"]);
      }

      if (objHotelPaymentRS.Success)
      {
         GuestDetailsEntryInfo objGuestDetailsEntryInfo = (GuestDetailsEntryInfo)Session["GuestDetailsEntryInfo"];

         objGuestDetailsEntryInfo.PaymentCardType = objHotelPaymentRS.PaymentCard.PaymentCardType;
         objGuestDetailsEntryInfo.PaymentCardNumber = objHotelPaymentRS.PaymentCard.PaymentCardNumber;
         objGuestDetailsEntryInfo.PaymentCardHolder = objHotelPaymentRS.PaymentCard.PaymentCardHolder;
         objGuestDetailsEntryInfo.PaymentCardExpireDate = objHotelPaymentRS.PaymentCard.PaymentCardExpireDate;
         objGuestDetailsEntryInfo.PaymentCardEffectiveDate = objHotelPaymentRS.PaymentCard.PaymentCardEffectiveDate;
         objGuestDetailsEntryInfo.PaymentCardIssueNumber = objHotelPaymentRS.PaymentCard.PaymentCardIssueNumber;
         objGuestDetailsEntryInfo.PaymentCardSecurityCode = objHotelPaymentRS.PaymentCard.PaymentCardSecurityCode;

         Session["GuestDetailsEntryInfo"] = objGuestDetailsEntryInfo;

         Session[Constants.Sessions.CurrentBookingStep] = MamaShelter.BookingSteps.Confirmed;
         Server.Transfer("~/Pages/BookRoom.aspx");
      }

      else
      {
         for (int i = 0; i < objHotelPaymentRS.Errors.Length; i++)
            this.AddPageError(PageErrorType.WbsPgError, objHotelPaymentRS.Errors[i].Code + "|" + objHotelPaymentRS.Errors[i].Description);

         this.SaveCrossPageErrors();
         Response.Redirect("~/Pages/Default.aspx?CrossPageErrors=1");
      }

      return;
   }

}
