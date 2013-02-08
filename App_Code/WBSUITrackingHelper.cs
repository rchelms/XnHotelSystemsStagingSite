using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

/// <summary>
/// Helper class for Tracking Codes
/// </summary>
/// 

public class WBSUITrackingHelper
{
	public WBSUITrackingHelper()
	{

    }

    public static TrackingCodeInfo[] GetTrackingCodeInfos(string strHotelCode)
    {
        int intNumTrackingCodeInfos = 0;

        if (ConfigurationManager.AppSettings["TrackingCodeCount"] != "")
        {
            try
            {
                intNumTrackingCodeInfos = Convert.ToInt32(ConfigurationManager.AppSettings["TrackingCodeCount"]);
            }

            catch
            {
                intNumTrackingCodeInfos = 0;
            }

        }

        List<TrackingCodeInfo> lTrackingCodeInfos = new List<TrackingCodeInfo>();

        for (int i = 0; i < intNumTrackingCodeInfos; i++)
        {
            string strTrackingCodeInfo = ConfigurationManager.AppSettings["TrackingCode" + ((int)(i + 1)).ToString()];

            if (strTrackingCodeInfo != "")
            {
                string[] saTrackingCodeInfo = strTrackingCodeInfo.Split(new char[] { ';' });

                if (saTrackingCodeInfo.Length < 4)
                    continue;

                TrackingCodeInfo objTrackingCodeInfo = new TrackingCodeInfo();

                if (saTrackingCodeInfo[0] != "default")
                {
                    if (strHotelCode == null || strHotelCode == "")
                        continue;

                    if (saTrackingCodeInfo[0] != strHotelCode)
                        continue;
                }

                TrackingTagLocation enumTagLocation = TrackingTagLocation.Unknown;

                if (saTrackingCodeInfo[1] == "head")
                    enumTagLocation = TrackingTagLocation.HeadElement;
                else if (saTrackingCodeInfo[1] == "body")
                    enumTagLocation = TrackingTagLocation.BodyElement;

                if (enumTagLocation == TrackingTagLocation.Unknown)
                    continue;

                TrackingPageSelection enumPageSelection = TrackingPageSelection.Unknown;

                if (saTrackingCodeInfo[2] == "start-only")
                    enumPageSelection = TrackingPageSelection.StartPageOnly;
                else if (saTrackingCodeInfo[2] == "confirm-only")
                    enumPageSelection = TrackingPageSelection.ConfirmPageOnly;
                else if (saTrackingCodeInfo[2] == "all-pages")
                    enumPageSelection = TrackingPageSelection.AllPages;

                if (enumPageSelection == TrackingPageSelection.Unknown)
                    continue;

                objTrackingCodeInfo.HotelCode = saTrackingCodeInfo[0];
                objTrackingCodeInfo.TagLocation = enumTagLocation;
                objTrackingCodeInfo.PageSelection = enumPageSelection;

                if (saTrackingCodeInfo[3] == "async")
                {
                    if (saTrackingCodeInfo.Length != 5 && saTrackingCodeInfo.Length != 6 && saTrackingCodeInfo.Length != 7 && saTrackingCodeInfo.Length != 8)
                        continue;

                    objTrackingCodeInfo.Type = TrackingCodeType.GoogleAnalytics_async;

                    objTrackingCodeInfo.TrackingCodeParameters = new string[4];
                    objTrackingCodeInfo.TrackingCodeParameters[0] = saTrackingCodeInfo[4]; // Account ID
                    objTrackingCodeInfo.TrackingCodeParameters[1] = ""; // Page URL (optional)
                    objTrackingCodeInfo.TrackingCodeParameters[2] = ""; // Page URL Prefix (optional)
                    objTrackingCodeInfo.TrackingCodeParameters[3] = ""; // GA Cross-Domain Tracking (optional)


                    if (saTrackingCodeInfo.Length >= 6)
                        objTrackingCodeInfo.TrackingCodeParameters[1] = saTrackingCodeInfo[5]; // Page URL (optional)

                    if (saTrackingCodeInfo.Length >= 7)
                        objTrackingCodeInfo.TrackingCodeParameters[2] = saTrackingCodeInfo[6]; // Page URL Prefix (optional)

                    if (saTrackingCodeInfo.Length >= 8)
                        objTrackingCodeInfo.TrackingCodeParameters[3] = saTrackingCodeInfo[7]; // GA Cross-Domain Tracking (optional)
                }

                else if (saTrackingCodeInfo[3] == "ga")
                {
                    if (saTrackingCodeInfo.Length != 5 && saTrackingCodeInfo.Length != 6 && saTrackingCodeInfo.Length != 7)
                        continue;

                    objTrackingCodeInfo.Type = TrackingCodeType.GoogleAnalytics_ga;

                    objTrackingCodeInfo.TrackingCodeParameters = new string[3];
                    objTrackingCodeInfo.TrackingCodeParameters[0] = saTrackingCodeInfo[4]; // Account ID
                    objTrackingCodeInfo.TrackingCodeParameters[1] = ""; // Page URL (optional)
                    objTrackingCodeInfo.TrackingCodeParameters[2] = ""; // Page URL Prefix (optional)

                    if (saTrackingCodeInfo.Length >= 6)
                        objTrackingCodeInfo.TrackingCodeParameters[1] = saTrackingCodeInfo[5]; // Page URL (optional)

                    if (saTrackingCodeInfo.Length >= 7)
                        objTrackingCodeInfo.TrackingCodeParameters[2] = saTrackingCodeInfo[6]; // Page URL Prefix (optional)
                }

                else if (saTrackingCodeInfo[3] == "urchin")
                {
                    if (saTrackingCodeInfo.Length != 5 && saTrackingCodeInfo.Length != 6 && saTrackingCodeInfo.Length != 7)
                        continue;

                    objTrackingCodeInfo.Type = TrackingCodeType.GoogleAnalytics_urchin;

                    objTrackingCodeInfo.TrackingCodeParameters = new string[3];
                    objTrackingCodeInfo.TrackingCodeParameters[0] = saTrackingCodeInfo[4]; // Account ID
                    objTrackingCodeInfo.TrackingCodeParameters[1] = ""; // Page URL (optional)
                    objTrackingCodeInfo.TrackingCodeParameters[2] = ""; // Page URL Prefix (optional)

                    if (saTrackingCodeInfo.Length >= 6)
                        objTrackingCodeInfo.TrackingCodeParameters[1] = saTrackingCodeInfo[5]; // Page URL (optional)

                    if (saTrackingCodeInfo.Length >= 7)
                        objTrackingCodeInfo.TrackingCodeParameters[2] = saTrackingCodeInfo[6]; // Page URL Prefix (optional)
                }

                else if (saTrackingCodeInfo[3] == "adwconv")
                {
                    if (saTrackingCodeInfo.Length != 9)
                        continue;

                    objTrackingCodeInfo.Type = TrackingCodeType.GoogleAdWordsConversion;

                    objTrackingCodeInfo.TrackingCodeParameters = new string[5];
                    objTrackingCodeInfo.TrackingCodeParameters[0] = saTrackingCodeInfo[4]; // ID
                    objTrackingCodeInfo.TrackingCodeParameters[1] = saTrackingCodeInfo[5]; // Language
                    objTrackingCodeInfo.TrackingCodeParameters[2] = saTrackingCodeInfo[6]; // Format
                    objTrackingCodeInfo.TrackingCodeParameters[3] = saTrackingCodeInfo[7]; // Color
                    objTrackingCodeInfo.TrackingCodeParameters[4] = saTrackingCodeInfo[8]; // Label
                }

                else if (saTrackingCodeInfo[3] == "dfa_start")
                {
                    if (saTrackingCodeInfo.Length != 8)
                        continue;

                    objTrackingCodeInfo.Type = TrackingCodeType.DoubleClickFloodlight_start;

                    objTrackingCodeInfo.TrackingCodeParameters = new string[4];
                    objTrackingCodeInfo.TrackingCodeParameters[0] = saTrackingCodeInfo[4]; // Src
                    objTrackingCodeInfo.TrackingCodeParameters[1] = saTrackingCodeInfo[5]; // Type
                    objTrackingCodeInfo.TrackingCodeParameters[2] = saTrackingCodeInfo[6]; // Cat
                    objTrackingCodeInfo.TrackingCodeParameters[3] = saTrackingCodeInfo[7]; // Ord
                }

                else if (saTrackingCodeInfo[3] == "dfa_confirm")
                {
                    if (saTrackingCodeInfo.Length != 8)
                        continue;

                    objTrackingCodeInfo.Type = TrackingCodeType.DoubleClickFloodlight_confirm;

                    objTrackingCodeInfo.TrackingCodeParameters = new string[4];
                    objTrackingCodeInfo.TrackingCodeParameters[0] = saTrackingCodeInfo[4]; // Src
                    objTrackingCodeInfo.TrackingCodeParameters[1] = saTrackingCodeInfo[5]; // Type
                    objTrackingCodeInfo.TrackingCodeParameters[2] = saTrackingCodeInfo[6]; // Cat
                    objTrackingCodeInfo.TrackingCodeParameters[3] = saTrackingCodeInfo[7]; // Ord
                }

                else if (saTrackingCodeInfo[3] == "ysm")
                {
                    if (saTrackingCodeInfo.Length != 5)
                        continue;

                    objTrackingCodeInfo.Type = TrackingCodeType.YahooSearchMarketing;

                    objTrackingCodeInfo.TrackingCodeParameters = new string[1];
                    objTrackingCodeInfo.TrackingCodeParameters[0] = saTrackingCodeInfo[4]; // Account ID
                }

                else
                    continue;

                lTrackingCodeInfos.Add(objTrackingCodeInfo);
            }

        }

        return lTrackingCodeInfos.ToArray();
    }
}