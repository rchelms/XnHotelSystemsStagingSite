using System;
using System.Collections.Generic;
using System.Threading;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
using XHS.Logging;
using XHS.WBSUIBizObjects;
using XHS.WBS.AreaList;

// Returns XML Area List Information in the Http Response

public partial class AreaListInfo : XnGR_WBS_Page
{
    private bool bAsyncGetHotelSearchAreaList;

    protected override void Page_Init(object sender, EventArgs e)
    {
        this.IsNewSessionOverride = true;

        base.Page_Init(sender, e);

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        bAsyncGetHotelSearchAreaList = true;

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetHotelSearchAreaList)
        {
            this.BeginResumeAsyncDataCapture();
        }

        else
        {
            while (!ThreadPool.QueueUserWorkItem(new WaitCallback(TerminateAsyncOperation), wbsIISAsyncResult))
                Thread.Sleep(100);
        }

        return wbsIISAsyncResult;
    }

    private void BeginResumeAsyncDataCapture()
    {
        if (bAsyncGetHotelSearchAreaList)
        {
            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitHotelSearchAreaListRQ(ref wbsAPIRouterData);
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelSearchAreaListComplete), null, false);
        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    private void HotelSearchAreaListComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessHotelSearchAreaListRS(ref wbsAPIRouterData))
        {
            bAsyncGetHotelSearchAreaList = false;
            this.BeginResumeAsyncDataCapture();
        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    public void EndAsyncOperation(IAsyncResult ar)
    {
        return;
    }

    public void TimeoutAsyncOperation(IAsyncResult ar)
    {
        return;
    }

    private void TerminateAsyncOperation(Object StateInfo)
    {
        WBSAsyncResult wbsIISAsyncResult = (WBSAsyncResult)StateInfo;

        if (!wbsIISAsyncResult.IsCompleted)
            wbsIISAsyncResult.SetComplete();

        return;
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        HotelSearchRS objHotelSearchRS = (HotelSearchRS)Session["AreaListHotelSearchRS"];

        AreaList objAreaList = new AreaList();
        objAreaList.Area = new WBS_Area[0];

        if (objHotelSearchRS.ResponseHeader.Success)
        {
            List<WBS_Area> lAreas = new List<WBS_Area>();

            if (objHotelSearchRS.HotelListItems != null)
            {
                List<string> lCountryCodes = new List<string>();

                for (int i = 0; i < objHotelSearchRS.AreaListItems.Length; i++)
                {
                    if (!lCountryCodes.Contains(objHotelSearchRS.AreaListItems[i].CountryCode))
                        lCountryCodes.Add(objHotelSearchRS.AreaListItems[i].CountryCode);
                }

                lCountryCodes.Sort();

                for (int i = 0; i < lCountryCodes.Count; i++)
                {
                    for (int j = 0; j < objHotelSearchRS.AreaListItems.Length; j++)
                    {
                        if (objHotelSearchRS.AreaListItems[j].CountryCode == lCountryCodes[i])
                        {
                            WBS_Area objArea = new WBS_Area();
                            lAreas.Add(objArea);

                            objArea.AreaID = objHotelSearchRS.AreaListItems[j].AreaID;
                            objArea.AreaName = objHotelSearchRS.AreaListItems[j].AreaName;
                            objArea.CountryCode = objHotelSearchRS.AreaListItems[j].CountryCode;
                        }

                    }

                }

            }

            objAreaList.Area = lAreas.ToArray();
        }

        XmlSerializer ser = new XmlSerializer(objAreaList.GetType());
        MemoryStream ms = new MemoryStream();
        ser.Serialize(ms, objAreaList);

        UTF8Encoding utf8 = new UTF8Encoding();
        string strAreaListXML = utf8.GetString(ms.ToArray());
        ms.Close();

        try
        {
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(strAreaListXML);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        catch (System.Threading.ThreadAbortException)
        {
            // Ignore -- html output always terminated since the xml
            // response document only is to go out to client.

            return;
        }

        Session.Abandon();

        return;
    }

}
