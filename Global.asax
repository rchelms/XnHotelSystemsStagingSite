<%@ Application Language="C#" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="XHS.Logging" %>
<%@ Import Namespace="XHS.WBSUIBizObjects" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Establish application level log objects
        
        FileLog objFileLog = new FileLog(ConfigurationManager.AppSettings["PrimaryLogPath"], ConfigurationManager.AppSettings["BackupLogPath"]);
        Application.Add("EventLog", objFileLog);
        
        ExceptionLog objExceptionLog = new ExceptionLog(objFileLog, "wscript", ConfigurationManager.AppSettings["AlertScriptArgument"], 10);
        Application.Add("ExceptionEventLog", objExceptionLog);
        
        // Establish application level time zone object
        
        ChaosSoftware.WorldTime.LoadData(ConfigurationManager.AppSettings["WorldTimeDataPath"]);
        
        // Establish WBS API Router runtime parameters

        int intWindowUnits = 5; // default value

        if (ConfigurationManager.AppSettings["WBSAPIRouter.WindowUnits"] != "")
        {
            try
            {
                intWindowUnits = Convert.ToInt32(ConfigurationManager.AppSettings["WBSAPIRouter.WindowUnits"]);
            }

            catch
            {
                intWindowUnits = 5;
            }

        }

        Application.Add("WBSAPIRouter.WindowUnits", intWindowUnits);

        int intWindowUnitSeconds = 60; // default value

        if (ConfigurationManager.AppSettings["WBSAPIRouter.WindowUnitSeconds"] != "")
        {
            try
            {
                intWindowUnitSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["WBSAPIRouter.WindowUnitSeconds"]);
            }

            catch
            {
                intWindowUnitSeconds = 60;
            }

        }

        Application.Add("WBSAPIRouter.WindowUnitSeconds", intWindowUnitSeconds);

        int intRequestExpirationSeconds = 30; // default value

        if (ConfigurationManager.AppSettings["WBSAPIRouter.RequestExpirationSeconds"] != "")
        {
            try
            {
                intRequestExpirationSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["WBSAPIRouter.RequestExpirationSeconds"]);
            }

            catch
            {
                intRequestExpirationSeconds = 30;
            }

        }

        Application.Add("WBSAPIRouter.RequestExpirationSeconds", intRequestExpirationSeconds);

        int intRequestServiceTimeThresholdSeconds = 20; // default value

        if (ConfigurationManager.AppSettings["WBSAPIRouter.RequestServiceTimeThresholdSeconds"] != "")
        {
            try
            {
                intRequestServiceTimeThresholdSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["WBSAPIRouter.RequestServiceTimeThresholdSeconds"]);
            }

            catch
            {
                intRequestServiceTimeThresholdSeconds = 20;
            }

        }

        Application.Add("WBSAPIRouter.RequestServiceTimeThresholdSeconds", intRequestServiceTimeThresholdSeconds);

        // Establish WBS Monitor runtime parameters (used for pending prepay bookings logging)

        int intExpirationSeconds = 1500; //  // default value (25 minutes)

        if (ConfigurationManager.AppSettings["WBSMonitor.ExpirationSeconds"] != "")
        {
            try
            {
                intExpirationSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["WBSMonitor.ExpirationSeconds"]);
            }

            catch
            {
                intExpirationSeconds = 1500;
            }

        }

        Application.Add("WBSMonitor.ExpirationSeconds", intExpirationSeconds);

        // Establish the performance counters runtime parameters

        string strPerfMonGroupName = "WBS.UI.Unknown"; // default value

        if (ConfigurationManager.AppSettings["PerformanceMonitorGroupName"] != null && ConfigurationManager.AppSettings["PerformanceMonitorGroupName"] != "")
            strPerfMonGroupName = ConfigurationManager.AppSettings["PerformanceMonitorGroupName"];

        Application.Add("WBSPerfCounters.PerformanceMonitorGroupName", strPerfMonGroupName);

        return;
    }

    void Application_End(object sender, EventArgs e)
    {

    }

    void Application_Error(object sender, EventArgs e)
    {
        ((ExceptionLog)Application["ExceptionEventLog"]).Write(HttpContext.Current.Session.SessionID, Server.GetLastError());
        Server.ClearError();
        Server.Transfer("~/pages/AppError.aspx");

        return;
    }

    void Session_Start(object sender, EventArgs e)
    {
        Session.Add("SelectedCulture", "");
        Session.Add("SelectedUICulture", "");
        
        Session.Add("CrossPageErrors", new CrossPageErrors());

        Session.Add("LoginProfileReadRS", new ProfileReadRS());
        Session.Add("LinkedProfileReadRS", new ProfileReadRS());
        Session.Add("AreaListHotelSearchRS", new HotelSearchRS());
        Session.Add("PropertyListHotelSearchRS", new HotelSearchRS());
        Session.Add("HotelAvailabilityRS", new HotelAvailabilityRS());
        Session.Add("HotelAvailabilityCalendarRS", null);
        Session.Add("SearchHotelAvailabilityCalendarRS", null);
        Session.Add("HotelDescriptiveInfoRS", new HotelDescriptiveInfoRS());
        Session.Add("SearchHotelDescriptiveInfoRS", new HotelDescriptiveInfoRS());
        Session.Add("AlternateHotelDescriptiveInfoRS", new HotelDescriptiveInfoRS());
        Session.Add("HotelBookingRS", new HotelBookingRS());
        Session.Add("HotelBookingReadRS", new HotelBookingReadRS());

        Session.Add("ProfileLoginInfo", new ProfileLoginInfo());
        Session.Add("LoginProfiles", new Profile[0]);
        Session.Add("IsLoggedIn", false);
        Session.Add("ViewLoginForm", false);
        Session.Add("LoginErrors", new string[0]);

        Session.Add("PaymentGatewayInfos", new PaymentGatewayInfo[0]);
        Session.Add("PaymentGatewayInfo", new PaymentGatewayInfo());
        Session.Add("HotelBookingPaymentAllocations", new HotelBookingPaymentAllocation[0]);
        Session.Add("HotelPaymentRQ", new HotelPaymentRQ());
        Session.Add("HotelPaymentRS", new HotelPaymentRS());
        Session.Add("PaymentGatewayRequestActive", false);
        Session.Add("PaymentGatewayResponseActive", false);
        Session.Add("PendingPrepayBookingID", "");
        Session.Add("PendingPrepayBookingInfo", new PendingPrepayBookingInfo());
        Session.Add("FailedPrepayBookingID", "");
        Session.Add("FailedPrepayBookingInfo", new FailedPrepayBookingInfo());
        Session.Add("XHS.MIGS3VPCHelper.PayRqData", new XHS.MIGS3VPCHelper.PayRqData()); // needed to process migs3p pay response

        Session.Add("ImageGalleries", new List<HotelImageGallery>());

        Session.Add("SelectedRoom", "1");
        Session.Add("StayCriteriaSelection", new StayCriteriaSelection());
        Session.Add("AvailCalRequested", false);
        Session.Add("AvailCalDateSelections", new DateTime[0]);
        Session.Add("RateGridStartDate", new DateTime());
        Session.Add("SearchRateGridStartDate", new DateTime());
        Session.Add("RoomRateSelections", new RoomRateSelection[0]);
        Session.Add("ShowMoreRatesIndicators", new ShowMoreRatesIndicator[0]);
        Session.Add("HotelRoomAvailInfos", new HotelRoomAvailInfo[0]);
        Session.Add("AddOnPackageSelections", new AddOnPackageSelection[0]);
        Session.Add("GuestDetailsEntryInfo", new GuestDetailsEntryInfo());
        Session.Add("BookingTermsConditionsAccepted", false);
        Session.Add("CancelDetailsEntryInfo", new CancelDetailsEntryInfo());

        Session.Add("IsFirstTrackingPage", true);

        WBSUIHelper WbsUiHelper = new WBSUIHelper(Session, null);

        WbsUiHelper.InitLanguage();

        WbsUiHelper.InitProfileLoginInfo();
        
        WbsUiHelper.InitStayCriteriaSelection();
        WbsUiHelper.InitRoomRateSelections();
        WbsUiHelper.InitAddOnPackageSelections();
        WbsUiHelper.InitGuestDetailsEntryInfo();

        WbsUiHelper.InitCancelDetailsEntryInfo();
        
        return;
    }

    void Session_End(object sender, EventArgs e)
    {
        
    }
       
</script>

