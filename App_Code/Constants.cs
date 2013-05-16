/// <summary>
/// Constants that are used over application
/// </summary>
using System.Collections.Generic;
public static class Constants
{
    /// <summary>
    /// Session constants
    /// </summary>
    public static class Sessions
    {
        public const string CurrentBookingStep = "CurrentBookingStep";
        public const string CurrentRoomDetailStep = "SessionCurrentRoomDetailStep";
        public const string AvailCalStartDate = "SessionAvailCalStartDate";
        public const string AvailCalDateSelections = "AvailCalDateSelections";
        public const string CurrentRoomRefID = "SessionCurrentRoomRefID";
        public const string StayCriteriaSelection = "StayCriteriaSelection";
        public const string HotelAvailabilityRS = "HotelAvailabilityRS";
        public const string HotelDescriptiveInfoRS = "HotelDescriptiveInfoRS";
        public const string RoomRateSelections = "RoomRateSelections";
        public const string AddOnPackageSelections = "AddOnPackageSelections";
        public const string ColumnImages = "ColumnImages";

        public const string PaymentGatewayInfos = "PaymentGatewayInfos";
        public const string PaymentGatewayInfo = "PaymentGatewayInfo";
        public const string HotelBookingPaymentAllocations = "HotelBookingPaymentAllocations";

        public const string GuestDetailsEntryInfo = "GuestDetailsEntryInfo";

        public const string HotelBookingRS = "HotelBookingRS";
        public const string HotelPaymentRS = "HotelPaymentRS";

        public const string HotelAvailabilityCalendarRS = "HotelAvailabilityCalendarRS";

        public const string CancelDetailsEntryInfo = "CancelDetailsEntryInfo";
        public const string PropertyListHotelSearchRS = "PropertyListHotelSearchRS";
        public const string LoginProfileReadRS = "LoginProfileReadRS";
        public const string LinkedProfileReadRS = "LinkedProfileReadRS";
        public const string LoginProfiles = "LoginProfiles";
        public const string IsLoggedIn = "IsLoggedIn";
    }
    public static class QueryString
    {
        public const string CrossPageErrors = "CrossPageErrors";
        public const string StartOver = "res";
    }
    /// <summary>
    /// Parameters used in query string when using Deep linking.
    /// </summary>
    public static class DeeplinkParams
    {
        public const string Account = "account";
        public const string Language = "language";
        public const string Command = "cmd";
        public const string CmdStayCriteria = "stayCriteria";
        public const string CmdCancelReservation = "cancelCriteria";
        public const string HotelCode = "hotel";
        public const string AreaCode = "area";
        public const string ArrivalDate = "arrDate";
        public const string DepartureDate = "depDate";
        public const string NumberOfRoom = "numRooms";
        public const string PromotionCode = "promo";
        /// <summary>
        /// Use string.Format to add room number to end of string
        /// from 1-8. Without 0 at the begining
        /// </summary>
        public const string NumberOfAdult = "adult{0}";
        /// <summary>
        /// Use string.Format to add room number to end of string
        /// from 1-8. Without 0 at the begining
        /// </summary>
        public const string NumberOfChild = "child{0}";

        public const string DefaultStayCriteria = "defaultStayCriteria";
    }

    public static class WebConfigKeys
    {
        public const string DefaultNumberAdult = "StayCriteriaSelectorControl.DefaultNumberAdult";
        public const string DefaultNumberChildren = "StayCriteriaSelectorControl.DefaultNumberChildren";
        public const string MaxRooms = "StayCriteriaSelectorControl.MaxRooms";
        public const string MaxAdults = "StayCriteriaSelectorControl.MaxAdults";
        public const string MaxChildren = "StayCriteriaSelectorControl.MaxChildren";
        public const string EnableChildren = "StayCriteriaSelectorControl.EnableChildren";

        public const string MaxChildrenHotelCode = "StayCriteriaSelectorControl.MaxChildren_{0}";
        public const string MaxAdultHotelCode = "StayCriteriaSelectorControl.MaxAdults_{0}";
        public const string EnableChildrenHotelCode = "StayCriteriaSelectorControl.EnableChildren_{0}";

        public const string MaxBookingDays = "MaxBookingDays";
        public const string DefaultTimeZone = "DefaultTimeZoneCode";
        public const string EnableHotelSearch = "EnableHotelSearch";

        public static Dictionary<string, string> CurrencySymbol = new Dictionary<string, string>
                                                                   {
                                                                       {"EUR", "CurrencySymbol.EUR"},
                                                                       {"USD", "CurrencySymbol.USD"},
                                                                       {"GBP", "CurrencySymbol.GBP"}
                                                                   };

        public const string CustomCDNPath = "CustomCDNPath";

    }

    public static class ScriptPath
    {
        public const string AvailCalSelectorControl = "~/scripts/availcalcontrol.js";
        public const string LanguageSelectorControl = "~/scripts/languageselectorcontrol.js";
    }
}
