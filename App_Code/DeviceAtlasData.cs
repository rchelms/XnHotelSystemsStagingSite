using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;
using ChaosSoftware;
using Mobi.Mtld.DA;

/// <summary>
/// DeviceAtlasData class for WBS Mobile Detection and Redirection
/// </summary>

public class DeviceAtlasData
{
    public Hashtable Tree;
    public static Object CacheLock = new Object();

    public DeviceAtlasData(FileLog objEventLog, ExceptionLog objExceptionEventLog)
    {
        Tree = null;

        try
        {
            Tree = Api.GetTreeFromFile((string)ConfigurationManager.AppSettings["DeviceAtlasDataPath"]);
        }

        catch (Exception e)
        {
            objExceptionEventLog.Write("Exception while reading DA hash tree from file: ", e);
        }

        return;
    }

    public static DeviceAtlasData GetDeviceAtlasData(Cache objCache, FileLog objEventLog, ExceptionLog objExceptionEventLog)
    {
        DeviceAtlasData objDeviceAtlasData = null;

        lock (DeviceAtlasData.CacheLock)
        {
            if (objCache["DeviceAtlasData"] != null)
            {
                objDeviceAtlasData = (DeviceAtlasData)objCache["DeviceAtlasData"];
            }

            else
            {
                objDeviceAtlasData = new DeviceAtlasData(objEventLog, objExceptionEventLog);
                objCache.Add("DeviceAtlasData", objDeviceAtlasData, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, null);
            }

        }

        return objDeviceAtlasData;
    }

}

