using System;
using System.Collections.Generic;
using System.Text;
using ChaosSoftware;

/// <summary>
/// Time zone wrapper class for WorldTime.dll component
/// </summary>

public class TZNet
{
    public static DateTime ToLocal(string strTimeZoneCode, DateTime dtUTC)
    {
        string strLocationID = null;

        //looking for exact match, single value
        strLocationID = WorldTime.FindLocationIDs("locationid", strTimeZoneCode, false, false);

        if (strLocationID == null)
            throw new ApplicationException("Time zone code is invalid: \"" + strTimeZoneCode + "\"");

        WorldTime.Location objLocation = WorldTime.GetLocationData(strLocationID);
        bool bDST = false;

        return ChaosSoftware.WorldTime.GetLocalTime(dtUTC, objLocation, ref bDST);
    }

    public static DateTime ToUTC(string strTimeZoneCode, DateTime dtLocal)
    {
        string strLocationID = null;

        //looking for exact match, single value
        strLocationID = WorldTime.FindLocationIDs("locationid", strTimeZoneCode, false, false);

        if (strLocationID == null)
            throw new ApplicationException("Time zone code is invalid: \"" + strTimeZoneCode + "\"");

        WorldTime.Location objLocation = WorldTime.GetLocationData(strLocationID);
        bool bDST = false;

        return ChaosSoftware.WorldTime.GetUTC(dtLocal, objLocation, ref bDST);
    }

}
