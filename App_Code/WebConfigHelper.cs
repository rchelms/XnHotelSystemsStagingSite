using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

/// <summary>
/// Summary description for ConfigurationHelper
/// </summary>
public static class WebconfigHelper
{
    public static bool EnabledHotelSearch { get { return GetBoolValue(Constants.WebConfigKeys.EnableHotelSearch); } }

    public static string DefaultTimeZone { get { return ConfigurationManager.AppSettings[Constants.WebConfigKeys.DefaultTimeZone]; } }

    public static int DefaultNumberAdult { get { return GetIntValue(Constants.WebConfigKeys.DefaultNumberAdult, 2); } }

    public static int DefaultNumberChildren { get { return GetIntValue(Constants.WebConfigKeys.DefaultNumberChildren, 0); } }

    public static int MaxRooms
    {
        get { return GetIntValue(Constants.WebConfigKeys.MaxRooms, 5); }
    }

    public static int MaxAdult
    {
        get
        {
            return GetIntValue(Constants.WebConfigKeys.MaxAdults, 0);
        }
    }

    public static int MaxChildren
    {
        get
        {
            return GetIntValue(Constants.WebConfigKeys.MaxChildren, 0);
        }
    }

    public static bool EnableChildren { get { return GetBoolValue(Constants.WebConfigKeys.EnableChildren); } }

    public static int GetMaxAdultByHotel(string hotelCode)
    {
        return GetIntValue(string.Format(Constants.WebConfigKeys.MaxAdultHotelCode, hotelCode), MaxAdult);
    }

    public static int GetMaxChildrenByHotel(string hotelCode)
    {
        return GetIntValue(string.Format(Constants.WebConfigKeys.MaxChildrenHotelCode, hotelCode), MaxChildren);
    }

    public static bool GetEnableChildrenByHotelCode(string hotelCode)
    {
        string EnableChildrenByHotelCodeKey = string.Format(Constants.WebConfigKeys.EnableChildrenHotelCode, hotelCode);
        if (ConfigurationManager.AppSettings.Get(EnableChildrenByHotelCodeKey) == null)
            return GetBoolValue(Constants.WebConfigKeys.EnableChildren);
        else
            return GetBoolValue(EnableChildrenByHotelCodeKey);
    }

    public static string GetCurrencyCodeString(string currencyCode)
    {
        if (!Constants.WebConfigKeys.CurrencySymbol.ContainsKey(currencyCode))
            return currencyCode;

        if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[Constants.WebConfigKeys.CurrencySymbol[currencyCode]]))
            return ConfigurationManager.AppSettings[Constants.WebConfigKeys.CurrencySymbol[currencyCode]];
        else
            return currencyCode;
    }

    public static int MaxBookingDays
    {
        get { return GetIntValue(Constants.WebConfigKeys.MaxBookingDays, 30); }
    }

    #region Private Methods

    private static int GetIntValue(string key, int defaultFailureValue)
    {
        int result;

        if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get(key)))
            return defaultFailureValue;

        var convertingResult = int.TryParse(ConfigurationManager.AppSettings[key], out result);

        return convertingResult ? result : defaultFailureValue;
    }

    private static bool GetBoolValue(string key)
    {
        if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get(key)))
            return false;
        if (ConfigurationManager.AppSettings[key].Equals("1")
           || ConfigurationManager.AppSettings[key].Equals("true", StringComparison.InvariantCultureIgnoreCase))
            return true;
        return false;
    }

    #endregion
}