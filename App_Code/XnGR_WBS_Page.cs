using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;
using ChaosSoftware;

/// <summary>
/// Base Page for all XnGR WBS UI Application Pages
/// </summary>

public class XnGR_WBS_Page : System.Web.UI.Page
{
    private FileLog objEventLog;
    private ExceptionLog objExceptionEventLog;

    private WBSPerfCounters wbsPerfCounters;
    private WBSServiceTimesUpdater wbsServiceTimesUpdater;

    private WBSAPIRouter wbsAPIRouter;

    protected WBSAPIRouterData wbsAPIRouterData;
    protected WBSAsyncResult wbsIISAsyncResult;

    private WBSMonitor wbsMonitor;

    private WBSAPIRouterHelper wbsAPI;
    private WBSPGHelper wbsPG;
    private WBSUIHelper wbsUI;

    private bool bIsParentPreRender;
    private bool bIsProduction;
    private bool bIsBookThrough;
    private bool bIsGuestDetailsTestPrefill;
    private bool bIsDeepLinkNav;

    private DateTime dtPageStartTime;

    private List<string> lPageErrors_WbsApi;
    private List<string> lPageErrors_WbsPg;
    private List<string> lPageErrors_Validation;

    public XnGR_WBS_Page()
    {

    }

    // General support properties and methods

    public bool IsParentPreRender
    {
        get
        {
            return bIsParentPreRender;
        }

        set
        {
            bIsParentPreRender = value;
        }

    }

    public bool IsProduction
    {
        get
        {
            return bIsProduction;
        }

    }

    public bool IsBookThrough
    {
        get
        {
            return bIsBookThrough;
        }

    }

    public bool IsGuestDetailsTestPrefill
    {
        get
        {
            return bIsGuestDetailsTestPrefill;
        }

    }

    public bool IsDeepLinkNav
    {
        get
        {
            return bIsDeepLinkNav;
        }

    }

    public bool IsFirstTrackingPage
    {
        get
        {
            return (bool)Session["IsFirstTrackingPage"];
        }

        set
        {
            if (value)
                Session["IsFirstTrackingPage"] = true;
            else
                Session["IsFirstTrackingPage"] = false;
        }

    }

    public int NumberDaysInAvailCalendar
    {
        get
        {
            return 28;
        }

    }

    public int NumberDaysInRateGrid
    {
        get
        {
            return 14;
        }

    }

    public bool IsNewSessionOverride
    {
        set
        {
            if (value)
            {
                Context.Items.Add("IsNewSessionOverride", true);
            }

        }

    }

    public bool IsServiceTimeThresholdExceeded
    {
        get
        {
            if (wbsAPIRouter.GetAverageServiceTime() > (int)Application["WBSAPIRouter.RequestServiceTimeThresholdSeconds"])
                return true;

            return false;
        }

    }

    public FileLog EventLog
    {
        get
        {
            return objEventLog;
        }

    }

    public ExceptionLog ExceptionEventLog
    {
        get
        {
            return objExceptionEventLog;
        }

    }

    public WBSPerfCounters WbsPerfCounters
    {
        get
        {
            return wbsPerfCounters;
        }

    }

    public WBSAPIRouter WbsApiRouter
    {
        get
        {
            return wbsAPIRouter;
        }

    }

    public WBSMonitor WbsMonitor
    {
        get
        {
            return wbsMonitor;
        }

    }

    public WBSAPIRouterHelper WbsApiRouterHelper
    {
        get
        {
            return wbsAPI;
        }

    }

    public WBSPGHelper WbsPgHelper
    {
        get
        {
            return wbsPG;
        }

    }

    public WBSUIHelper WbsUiHelper
    {
        get
        {
            return wbsUI;
        }

    }

    public static bool IsProductionMode()
    {
        if (ConfigurationManager.AppSettings["WBSSystemMode"] == "Production")
            return true;

        return false;
    }

    protected override PageStatePersister PageStatePersister
    {
        // Save view state in user session object and not page html

        get
        {
            return new SessionPageStatePersister(Page);
        }

    }

    protected virtual void Page_Init(object sender, EventArgs e)
    {
        dtPageStartTime = DateTime.Now;

        if (Context.Session != null)
        {
            bool bIsNewSessionOverride = false;

            if (Context.Items["IsNewSessionOverride"] != null)
                bIsNewSessionOverride = (bool)Context.Items["IsNewSessionOverride"];

            if (Session.IsNewSession && !bIsNewSessionOverride)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

        }

        bIsProduction = XnGR_WBS_Page.IsProductionMode();

        if (ConfigurationManager.AppSettings["WBSBookingMode"] == "Production")
            bIsBookThrough = true;
        else
            bIsBookThrough = false;

        if (ConfigurationManager.AppSettings["GuestDetailsTestPrefill"] == "1")
            bIsGuestDetailsTestPrefill = true;
        else
            bIsGuestDetailsTestPrefill = false;

        bIsParentPreRender = false;
        bIsDeepLinkNav = false;

        objEventLog = (FileLog)Application["EventLog"];
        objExceptionEventLog = (ExceptionLog)Application["ExceptionEventLog"];

        wbsAPIRouter = WBSAPIRouter.GetWbsApiRouter(Context.Cache, objEventLog, objExceptionEventLog, bIsProduction, (int)Application["WBSAPIRouter.RequestExpirationSeconds"], (int)Application["WBSAPIRouter.WindowUnitSeconds"], (int)Application["WBSAPIRouter.WindowUnits"]);
        wbsAPIRouterData = null;
        wbsIISAsyncResult = null;

        wbsAPI = new WBSAPIRouterHelper(Session, Page, objEventLog, objExceptionEventLog, bIsProduction);
        wbsPG = new WBSPGHelper(Session, Page, objEventLog, objExceptionEventLog, bIsProduction);
        wbsUI = new WBSUIHelper(Session, Page);

        wbsPerfCounters = WBSPerfCounters.GetWbsPerfCounters(Context.Cache, objEventLog, objExceptionEventLog, (string)Application["WBSPerfCounters.PerformanceMonitorGroupName"]);
        wbsServiceTimesUpdater = WBSServiceTimesUpdater.GetWbsServiceTimesUpdater(Context.Cache, objEventLog, objExceptionEventLog, bIsProduction, (int)Application["WBSAPIRouter.RequestExpirationSeconds"], (int)Application["WBSAPIRouter.WindowUnitSeconds"], (int)Application["WBSAPIRouter.WindowUnits"], (string)Application["WBSPerfCounters.PerformanceMonitorGroupName"]); // retrieved only as a "keep-alive" mechanism

        wbsMonitor = WBSMonitor.GetWbsMonitor(Context.Cache, objEventLog, objExceptionEventLog, bIsProduction, (int)Application["WBSMonitor.ExpirationSeconds"]); // Used for pending prepay bookings logging

        this.InitPageErrors();

        if (!IsPostBack)
        {
            if (Request.QueryString.Get("CrossPageErrors") == "1")
                this.RestoreCrossPageErrors();

            if (Request.QueryString.Get("DeepLinkNav") == "1") // used wtih Response.Redirect
                bIsDeepLinkNav = true;

            if ((string)Context.Items["DeepLinkNav"] == "1") // used with Server.Transfer
                bIsDeepLinkNav = true;
        }

        return;
    }

    protected virtual void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");

        return;
    }

    public void PageComplete()
    {
        DateTime dtPageEndTime = DateTime.Now;
        wbsAPIRouter.AddPageServiceStats(dtPageStartTime, dtPageEndTime);

        return;
    }

    // Globalization support properties and methods

    protected override void InitializeCulture()
    {
        WBSUIHelper objWBSUIHelper = new WBSUIHelper(Session, Page);

        Culture = objWBSUIHelper.SelectedCulture;
        UICulture = objWBSUIHelper.SelectedUICulture;

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(objWBSUIHelper.SelectedCulture);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(objWBSUIHelper.SelectedUICulture);

        base.InitializeCulture();
    }

    public string CurrencyFormat()
    {
        if (ConfigurationManager.AppSettings["CurrencyDecimalPlacements"] == "0")
            return "F0";

        else if (ConfigurationManager.AppSettings["CurrencyDecimalPlacements"] == "1")
            return "F1";

        else if (ConfigurationManager.AppSettings["CurrencyDecimalPlacements"] == "2")
            return "F2";

        else if (ConfigurationManager.AppSettings["CurrencyDecimalPlacements"] == "3")
            return "F3";

        return "F2";
    }

    // Booking restriction notification support methods

    public string[] GetBookingRestrictionNoticeMessages(string strHotelCode)
    {
        BookingRestrictionNotice[] objBookingRestrictionNotices = wbsUI.GetBookingRestrictionNotices(strHotelCode);

        List<string> lNoticeMessages = new List<string>();

        for (int i = 0; i < objBookingRestrictionNotices.Length; i++)
        {
            string strNoticeMessage = "";

            if (objBookingRestrictionNotices[i].Type == BookingRetrictionType.Opening)
                strNoticeMessage = (String)GetGlobalResourceObject("SiteResources", "BookingRestrictionNoticeOpening");
            else if (objBookingRestrictionNotices[i].Type == BookingRetrictionType.Repairs)
                strNoticeMessage = (String)GetGlobalResourceObject("SiteResources", "BookingRestrictionNoticeRepairs");
            else if (objBookingRestrictionNotices[i].Type == BookingRetrictionType.Refurbishment)
                strNoticeMessage = (String)GetGlobalResourceObject("SiteResources", "BookingRestrictionNoticeRefurbishment");
            else if (objBookingRestrictionNotices[i].Type == BookingRetrictionType.Closing)
                strNoticeMessage = (String)GetGlobalResourceObject("SiteResources", "BookingRestrictionNoticeClosing");

            strNoticeMessage = strNoticeMessage.Replace("{hotel}", objBookingRestrictionNotices[i].HotelName);
            strNoticeMessage = strNoticeMessage.Replace("{date1}", objBookingRestrictionNotices[i].Date1.ToLongDateString());
            strNoticeMessage = strNoticeMessage.Replace("{date2}", objBookingRestrictionNotices[i].Date2.ToLongDateString());

            lNoticeMessages.Add(strNoticeMessage);
        }

        return lNoticeMessages.ToArray();
    }

    // Error display support properties and methods

    public string[] PageErrors
    {
        get
        {
            List<string> lPageErrors = new List<string>();

            for (int i = 0; i < lPageErrors_WbsApi.Count; i++)
            {
                string strWbsApiError = (String)GetGlobalResourceObject("WbsApiErrorMessages", "ERC_" + lPageErrors_WbsApi[i]);

                if (strWbsApiError == null || strWbsApiError == "")
                    strWbsApiError = (String)GetGlobalResourceObject("WbsApiErrorMessages", "Default") + " " + lPageErrors_WbsApi[i];

                lPageErrors.Add(strWbsApiError);
            }

            for (int i = 0; i < lPageErrors_WbsPg.Count; i++)
            {
                string[] saWbsPgError = lPageErrors_WbsPg[i].Split(new char[] { '|' });

                string strWbsPgError = (String)GetGlobalResourceObject("WbsPgErrorMessages", "ERC_" + saWbsPgError[0]);

                if (saWbsPgError.Length > 1 && saWbsPgError[1] != null && saWbsPgError[1] != "")
                    strWbsPgError += " [" + saWbsPgError[1] + "]";

                if (strWbsPgError == null || strWbsPgError == "")
                    strWbsPgError = (String)GetGlobalResourceObject("WbsPgErrorMessages", "Default");

                lPageErrors.Add(strWbsPgError);
            }

            lPageErrors.AddRange(lPageErrors_Validation);

            return lPageErrors.ToArray();
        }

    }

    public bool IsPageError
    {
        get
        {
            if (PageErrors.Length != 0)
                return true;

            return false;
        }

    }

    public bool IsPageWbsApiError
    {
        get
        {
            if (lPageErrors_WbsApi.Count != 0)
                return true;

            return false;
        }

    }

    public bool IsPageWbsPgError
    {
        get
        {
            if (lPageErrors_WbsPg.Count != 0)
                return true;

            return false;
        }

    }

    public bool IsPageValidationError
    {
        get
        {
            if (lPageErrors_Validation.Count != 0)
                return true;

            return false;
        }

    }

    private void InitPageErrors()
    {
        lPageErrors_WbsApi = new List<string>();
        lPageErrors_WbsPg = new List<string>();
        lPageErrors_Validation = new List<string>();

        return;
    }

    public void AddPageError(PageErrorType enumPageErrorType, string strError)
    {
        if (enumPageErrorType == PageErrorType.WbsApiError)
            lPageErrors_WbsApi.Add(strError);

        else if (enumPageErrorType == PageErrorType.WbsPgError)
            lPageErrors_WbsPg.Add(strError);

        else if (enumPageErrorType == PageErrorType.ValidationError)
            lPageErrors_Validation.Add(strError);

        return;
    }

    public void SaveCrossPageErrors()
    {
        CrossPageErrors objCrossPageErrors = new CrossPageErrors();

        objCrossPageErrors.PageErrors_WbsApi = new List<string>();
        objCrossPageErrors.PageErrors_WbsApi.AddRange(lPageErrors_WbsApi);

        objCrossPageErrors.PageErrors_WbsPg = new List<string>();
        objCrossPageErrors.PageErrors_WbsPg.AddRange(lPageErrors_WbsPg);

        objCrossPageErrors.PageErrors_Validation = new List<string>();
        objCrossPageErrors.PageErrors_Validation.AddRange(lPageErrors_Validation);

        Session["CrossPageErrors"] = objCrossPageErrors;

        return;
    }

    private void RestoreCrossPageErrors()
    {
        this.InitPageErrors();

        CrossPageErrors objCrossPageErrors = (CrossPageErrors)Session["CrossPageErrors"];

        if (objCrossPageErrors == null)
            return;

        if (objCrossPageErrors.PageErrors_WbsApi != null)
            lPageErrors_WbsApi.AddRange(objCrossPageErrors.PageErrors_WbsApi);

        if (objCrossPageErrors.PageErrors_WbsPg != null)
            lPageErrors_WbsPg.AddRange(objCrossPageErrors.PageErrors_WbsPg);

        if (objCrossPageErrors.PageErrors_Validation != null)
            lPageErrors_Validation.AddRange(objCrossPageErrors.PageErrors_Validation);

        return;
    }

    public enum PageErrorType
    {
        WbsApiError,
        WbsPgError,
        ValidationError
    }

    // Restrictions, guarantee, cancellation, and deposit policy support properties and methods

    public string RateRestrictions(AvCalRoomRate objRoomRate, AvCalRate objRate)
    {
        StringBuilder sb = new StringBuilder();

        if (objRate.MinLOS > 1)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "MinLengthOfStay"));
            sb.Append(" ");
        }

        if (objRate.MaxLOS != 9999)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "MaxLengthOfStay"));
            sb.Append(" ");
        }

        if (objRoomRate.MinAdvBook != 0)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "MinAPDays"));
            sb.Append(" ");
        }

        if (objRoomRate.MaxAdvBook != 9999)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "MaxAPDays"));
            sb.Append(" ");
        }

        if (objRate.Status == AvailStatus.ClosedToArrival)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "ClosedToArrival"));
            sb.Append(" ");
        }

        string strRestrictions = sb.ToString();

        strRestrictions = strRestrictions.Replace("{min_los_days}", objRate.MinLOS.ToString());
        strRestrictions = strRestrictions.Replace("{max_los_days}", objRate.MaxLOS.ToString());
        strRestrictions = strRestrictions.Replace("{min_ap_days}", objRoomRate.MinAdvBook.ToString());
        strRestrictions = strRestrictions.Replace("{max_ap_days}", objRoomRate.MaxAdvBook.ToString());

        return strRestrictions;
    }

    public string GuaranteePolicy(HotelAvailRatePlan objRatePlan)
    {
        StringBuilder sb = new StringBuilder();

        if (objRatePlan.GuaranteeType == GuaranteeType.None)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "GuaranteePolicyNone"));
        }

        else if (objRatePlan.GuaranteeType == GuaranteeType.CCDCVoucher)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "GuaranteePolicyCreditCard"));
        }

        else if (objRatePlan.GuaranteeType == GuaranteeType.Deposit || objRatePlan.GuaranteeType == GuaranteeType.PrePay)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "GuaranteePolicyDeposit"));
        }

        return sb.ToString();
    }

    public string CancellationPolicy(HotelAvailRatePlan objRatePlan)
    {
        StringBuilder sb = new StringBuilder();

        if (objRatePlan.NonRefundable)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyNonrefundable"));
        }

        else
        {
            if (objRatePlan.CancelDeadlineHours != 0)
            {
                if (objRatePlan.CancelDeadlineHours > 48)
                {
                    sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyNoticeBeforeDayOfArrivalDays"));
                }

                else
                {
                    sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyNoticeBeforeDayOfArrivalHours"));
                }
            }

            else
            {
                sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyNoticeOnDayOfArrival"));
            }

        }

        if (objRatePlan.CancelPenaltyNumberNights != 0)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyFeeNumNights"));
        }

        else if (objRatePlan.CancelPenaltyPercent != 0)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyFeePercentOfStay"));
        }

        else if (objRatePlan.CancelPenaltyAmount != 0)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyFeeFixedAmount"));
        }

        string strPolicy = sb.ToString();

        strPolicy = strPolicy.Replace("{cancel_deadline_hours}", objRatePlan.CancelDeadlineHours.ToString());
        strPolicy = strPolicy.Replace("{cancel_deadline_days}", ((int)(objRatePlan.CancelDeadlineHours / 24)).ToString());
        strPolicy = strPolicy.Replace("{cancel_deadline_time}", objRatePlan.CancelDeadline.ToString("h:mmtt"));
        strPolicy = strPolicy.Replace("{cancel_deadline_time_24h}", objRatePlan.CancelDeadline.ToString("HH:mm"));
        strPolicy = strPolicy.Replace("{cancel_penalty_number_nights}", objRatePlan.CancelPenaltyNumberNights.ToString());
        strPolicy = strPolicy.Replace("{cancel_penalty_percent}", objRatePlan.CancelPenaltyPercent.ToString("F2"));
        strPolicy = strPolicy.Replace("{cancel_penalty_amount}", objRatePlan.CancelPenaltyAmount.ToString(this.CurrencyFormat()));
        strPolicy = strPolicy.Replace("{cancel_penalty_currency}", objRatePlan.CancelPenaltyCurrencyCode);

        return strPolicy;
    }

    public string CancellationPolicy(HotelBookingCancelPolicy objCancelPolicy)
    {
        StringBuilder sb = new StringBuilder();

        if (objCancelPolicy.NonRefundable)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyNonrefundable"));
        }

        else
        {
            if (objCancelPolicy.CancelDeadlineHours != 0)
            {
                if (objCancelPolicy.CancelDeadlineHours > 48)
                {
                    sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyNoticeBeforeDayOfArrivalDays"));
                }

                else
                {
                    sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyNoticeBeforeDayOfArrivalHours"));
                }
            }

            else
            {
                sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyNoticeOnDayOfArrival"));
            }

        }

        if (objCancelPolicy.CancelPenaltyNumberNights != 0)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyFeeNumNights"));
        }

        else if (objCancelPolicy.CancelPenaltyPercent != 0)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyFeePercentOfStay"));
        }

        else if (objCancelPolicy.CancelPenaltyAmount != 0)
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "CancelPolicyFeeFixedAmount"));
        }

        string strPolicy = sb.ToString();

        strPolicy = strPolicy.Replace("{cancel_deadline_hours}", objCancelPolicy.CancelDeadlineHours.ToString());
        strPolicy = strPolicy.Replace("{cancel_deadline_days}", ((int)(objCancelPolicy.CancelDeadlineHours / 24)).ToString());
        strPolicy = strPolicy.Replace("{cancel_deadline_time}", objCancelPolicy.CancelDeadline.ToString("h:mmtt"));
        strPolicy = strPolicy.Replace("{cancel_deadline_time_24h}", objCancelPolicy.CancelDeadline.ToString("HH:mm"));
        strPolicy = strPolicy.Replace("{cancel_penalty_number_nights}", objCancelPolicy.CancelPenaltyNumberNights.ToString());
        strPolicy = strPolicy.Replace("{cancel_penalty_percent}", objCancelPolicy.CancelPenaltyPercent.ToString("F2"));
        strPolicy = strPolicy.Replace("{cancel_penalty_amount}", objCancelPolicy.CancelPenaltyAmount.ToString(this.CurrencyFormat()));
        strPolicy = strPolicy.Replace("{cancel_penalty_currency}", objCancelPolicy.CancelPenaltyCurrencyCode);

        return strPolicy;
    }

    public bool IsCancellationPenalty(HotelBookingCancelPolicy objCancelPolicy, string strHotelCode, DateTime dtArrivalDate)
    {
        if (objCancelPolicy.NonRefundable)
            return true;

        DateTime dtCurrentLocal = TZNet.ToLocal(WbsUiHelper.GetTimeZone(strHotelCode), DateTime.UtcNow).Date;
        DateTime dtCancelDeadline = dtArrivalDate.Date;

        dtCancelDeadline = dtCancelDeadline.AddHours(objCancelPolicy.CancelDeadline.Hour);
        dtCancelDeadline = dtCancelDeadline.AddMinutes(objCancelPolicy.CancelDeadline.Minute);
        dtCancelDeadline = dtCancelDeadline.AddHours(objCancelPolicy.CancelDeadlineHours * -1);

        if (dtCurrentLocal > dtCancelDeadline)
            return true;

        return false;
    }

    public string DepositPolicy(HotelAvailRatePlan objRatePlan)
    {
        StringBuilder sb = new StringBuilder();

        if (objRatePlan.GuaranteeType == GuaranteeType.Deposit || objRatePlan.GuaranteeType == GuaranteeType.PrePay)
        {
            if (objRatePlan.NonRefundable)
            {
                sb.Append((String)GetGlobalResourceObject("SiteResources", "DepositPolicyNonrefundable"));
            }

            else
            {
                sb.Append((String)GetGlobalResourceObject("SiteResources", "DepositPolicyRefundable"));
            }

            if (objRatePlan.DepositRequiredNumberNights != 0)
            {
                sb.Append((String)GetGlobalResourceObject("SiteResources", "DepositPolicyNumNights"));
            }

            else if (objRatePlan.DepositRequiredPercent != 0)
            {
                sb.Append((String)GetGlobalResourceObject("SiteResources", "DepositPolicyPercentOfStay"));
            }

            else if (objRatePlan.DepositRequiredAmount != 0)
            {
                sb.Append((String)GetGlobalResourceObject("SiteResources", "DepositPolicyFixedAmount"));
            }

        }

        else
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "DepositPolicyNone"));
        }

        string strPolicy = sb.ToString();

        strPolicy = strPolicy.Replace("{deposit_number_nights}", objRatePlan.DepositRequiredNumberNights.ToString());
        strPolicy = strPolicy.Replace("{deposit_percent}", objRatePlan.DepositRequiredPercent.ToString("F2"));
        strPolicy = strPolicy.Replace("{deposit_amount}", objRatePlan.DepositRequiredAmount.ToString(this.CurrencyFormat()));
        strPolicy = strPolicy.Replace("{deposit_currency}", objRatePlan.DepositRequiredCurrencyCode);

        return strPolicy;
    }

    public string PaymentGatewaySelectionNotice(decimal decPaymentAmount, string strCurrencyCode)
    {
        string strNotice = "";

        strNotice = (String)GetGlobalResourceObject("SiteResources", "PaymentGatewaySelectInstructions");

        strNotice = strNotice.Replace("{deposit_amount}", decPaymentAmount.ToString(this.CurrencyFormat()));
        strNotice = strNotice.Replace("{deposit_currency}", strCurrencyCode);

        return strNotice;
    }

    public string PaymentCardApplicationNotice(PaymentCardApplication enumPaymentCardApplicationStatus, PaymentGatewayMode enumPaymentGatewayMode, decimal decPaymentAmount, string strCurrencyCode)
    {
        string strNotice = "";

        if (enumPaymentGatewayMode == PaymentGatewayMode.MerchantSiteCapturesCardDetails)
        {
            if (enumPaymentCardApplicationStatus == PaymentCardApplication.GuaranteeOnly)
            {
                strNotice = (String)GetGlobalResourceObject("SiteResources", "PaymentCardGuaranteeOnly");
            }

            else if (enumPaymentCardApplicationStatus == PaymentCardApplication.DepositOnly)
            {
                strNotice = (String)GetGlobalResourceObject("SiteResources", "PaymentCardDepositOnly");
            }

            else if (enumPaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit)
            {
                strNotice = (String)GetGlobalResourceObject("SiteResources", "PaymentCardGuaranteeAndDeposit");
            }

        }

        else if (enumPaymentGatewayMode == PaymentGatewayMode.PaymentGatewayCapturesCardDetails)
        {
            if (enumPaymentCardApplicationStatus == PaymentCardApplication.GuaranteeOnly)
            {
                strNotice = (String)GetGlobalResourceObject("SiteResources", "AltPaymentCardGuaranteeOnly");
            }

            else if (enumPaymentCardApplicationStatus == PaymentCardApplication.DepositOnly)
            {
                strNotice = (String)GetGlobalResourceObject("SiteResources", "AltPaymentCardDepositOnly");
            }

            else if (enumPaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit)
            {
                strNotice = (String)GetGlobalResourceObject("SiteResources", "AltPaymentCardGuaranteeAndDeposit");
            }

        }

        strNotice = strNotice.Replace("{deposit_amount}", decPaymentAmount.ToString(this.CurrencyFormat()));
        strNotice = strNotice.Replace("{deposit_currency}", strCurrencyCode);

        return strNotice;
    }

    public string PaymentCardDepositNotice(PaymentGatewayMode enumPaymentGatewayMode, decimal decPaymentAmount, string strCurrencyCode)
    {
        string strNotice = "";

        if (enumPaymentGatewayMode == PaymentGatewayMode.MerchantSiteCapturesCardDetails)
        {
            strNotice = (String)GetGlobalResourceObject("SiteResources", "PaymentCardDepositOnly");
        }

        else if (enumPaymentGatewayMode == PaymentGatewayMode.PaymentGatewayCapturesCardDetails)
        {
            strNotice = (String)GetGlobalResourceObject("SiteResources", "AltPaymentCardDepositOnly");
        }

        strNotice = strNotice.Replace("{deposit_amount}", decPaymentAmount.ToString(this.CurrencyFormat()));
        strNotice = strNotice.Replace("{deposit_currency}", strCurrencyCode);

        return strNotice;
    }

    public string AcceptedPaymentCards(string[] objCreditCardCodes)
    {
        string[] objCreditCardNames = this.GetCreditCardNames(objCreditCardCodes);

        StringBuilder sb = new StringBuilder();

        if (objCreditCardNames.Length != 0)
        {
            string strSeparator = "";

            for (int i = 0; i < objCreditCardNames.Length; i++)
            {
                sb.Append(strSeparator);
                sb.Append(objCreditCardNames[i]);

                strSeparator = ", ";
            }

        }

        else
        {
            sb.Append((String)GetGlobalResourceObject("SiteResources", "AcceptedPaymentCardsNone"));
        }

        return sb.ToString();
    }

    private string[] GetCreditCardNames(string[] objCreditCardCodes)
    {
        List<string> lCreditCardNames = new List<string>();

        for (int i = 0; i < objCreditCardCodes.Length; i++)
        {
            string strCreditCardName = (String)GetGlobalResourceObject("SiteResources", "CardType" + objCreditCardCodes[i]);

            if (strCreditCardName != null && strCreditCardName != "")
                lCreditCardNames.Add(strCreditCardName);
        }

        return lCreditCardNames.ToArray();
    }

    // Other global resource related support methods

    public CountryListItem[] GetCountryList(AreaListItem[] objAreaListItems)
    {
        List<string> lCountryCodes = new List<string>();

        for (int i = 0; i < objAreaListItems.Length; i++)
        {
            if (!lCountryCodes.Contains(objAreaListItems[i].CountryCode))
                lCountryCodes.Add(objAreaListItems[i].CountryCode);
        }

        lCountryCodes.Sort();

        List<CountryListItem> lCountryListItems = new List<CountryListItem>();

        for (int i = 0; i < lCountryCodes.Count; i++)
        {
            CountryListItem liCountryListItem = new CountryListItem();
            lCountryListItems.Add(liCountryListItem);

            liCountryListItem.CountryCode = lCountryCodes[i];
            liCountryListItem.CountryName = this.GetCountryName(lCountryCodes[i]);
        }

        return lCountryListItems.ToArray();
    }

    public AreaListItem[] GetCountryAreaList(string strCountryCode, AreaListItem[] objAreaListItems)
    {
        List<AreaListItem> lAreaListItems = new List<AreaListItem>();

        for (int i = 0; i < objAreaListItems.Length; i++)
        {
            if (objAreaListItems[i].CountryCode == strCountryCode)
                lAreaListItems.Add(objAreaListItems[i]);
        }

        return lAreaListItems.ToArray();
    }

    public HotelListItem[] GetAreaHotelList(string strAreaID, HotelListItem[] objHotelListItems)
    {
        List<HotelListItem> lHotelListItems = new List<HotelListItem>();

        for (int i = 0; i < objHotelListItems.Length; i++)
        {
            for (int j = 0; j < objHotelListItems[i].AreaIDs.Length; j++)
            {
                if (objHotelListItems[i].AreaIDs[j] == strAreaID)
                {
                    lHotelListItems.Add(objHotelListItems[i]);
                    break;
                }

            }

        }

        return lHotelListItems.ToArray();
    }

    public string GetCountryCode(string strAreaID, AreaListItem[] objAreaListItems)
    {
        for (int i = 0; i < objAreaListItems.Length; i++)
        {
            if (objAreaListItems[i].AreaID == strAreaID)
            {
                return objAreaListItems[i].CountryCode;
            }

        }

        return "";
    }

    public string GetCountryName(string strCountryCode)
    {
        string strCountryName = (String)GetGlobalResourceObject("SiteResources", "Country" + strCountryCode);

        if (strCountryName == null || strCountryName == "")
            strCountryName = strCountryCode;

        return strCountryName;
    }

}

