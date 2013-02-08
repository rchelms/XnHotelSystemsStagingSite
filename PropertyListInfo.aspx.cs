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
using XHS.WBS.PropertyList;

// Returns XML Property List Information in the Http Response

public partial class PropertyListInfo : XnGR_WBS_Page
{
    private bool bAsyncGetHotelSearchPropertyList;

    protected override void Page_Init(object sender, EventArgs e)
    {
        this.IsNewSessionOverride = true;

        base.Page_Init(sender, e);

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        bAsyncGetHotelSearchPropertyList = true;

        PageAsyncTask task = new PageAsyncTask(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation), new EndEventHandler(TimeoutAsyncOperation), null);
        RegisterAsyncTask(task);

        return;
    }

    public IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        wbsIISAsyncResult = new WBSAsyncResult(cb, state);

        if (bAsyncGetHotelSearchPropertyList)
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
        if (bAsyncGetHotelSearchPropertyList)
        {
            string strAreaID = "";

            if (Request.QueryString.Get("area") != null)
            {
                strAreaID = Request.QueryString.Get("area");
            }

            string[] objAreaIDs = new string[0];

            if (strAreaID != "")
            {
                objAreaIDs = new string[] { strAreaID };
            }

            wbsAPIRouterData = new WBSAPIRouterData();
            this.WbsApiRouterHelper.InitHotelSearchPropertyListRQ(ref wbsAPIRouterData, objAreaIDs);
            this.WbsApiRouter.QueueNewRequest(wbsAPIRouterData, new AsyncCallback(HotelSearchPropertyListComplete), null, false);
        }

        else
        {
            // End async page operation

            if (!wbsIISAsyncResult.IsCompleted)
                wbsIISAsyncResult.SetComplete();
        }

        return;
    }

    private void HotelSearchPropertyListComplete(IAsyncResult ar)
    {
        if (this.WbsApiRouterHelper.ProcessHotelSearchPropertyListRS(ref wbsAPIRouterData))
        {
            bAsyncGetHotelSearchPropertyList = false;
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
        HotelSearchRS objHotelSearchRS = (HotelSearchRS)Session["PropertyListHotelSearchRS"];

        PropertyList objPropertyList = new PropertyList();
        objPropertyList.Property = new WBS_Property[0];

        if (objHotelSearchRS.ResponseHeader.Success)
        {
            List<WBS_Property> lProperties = new List<WBS_Property>();

            if (objHotelSearchRS.HotelListItems != null)
            {
                List<string> lBrandCodes = new List<string>();

                for (int i = 0; i < objHotelSearchRS.HotelListItems.Length; i++)
                {
                    if (!lBrandCodes.Contains(objHotelSearchRS.HotelListItems[i].BrandCode))
                        lBrandCodes.Add(objHotelSearchRS.HotelListItems[i].BrandCode);
                }

                lBrandCodes.Sort();

                for (int i = 0; i < lBrandCodes.Count; i++)
                {
                    for (int j = 0; j < objHotelSearchRS.HotelListItems.Length; j++)
                    {
                        if (objHotelSearchRS.HotelListItems[j].BrandCode == lBrandCodes[i])
                        {
                            WBS_Property objProperty = new WBS_Property();
                            lProperties.Add(objProperty);

                            objProperty.HotelCode = objHotelSearchRS.HotelListItems[j].HotelCode;
                            objProperty.HotelName = objHotelSearchRS.HotelListItems[j].HotelName;
                            objProperty.BrandCode = objHotelSearchRS.HotelListItems[j].BrandCode;
                            objProperty.BrandName = objHotelSearchRS.HotelListItems[j].BrandName;

                            if (objHotelSearchRS.HotelListItems[j].AreaIDs != null && objHotelSearchRS.HotelListItems[j].AreaIDs.Length != 0)
                            {
                                List<WBS_Area> lAreas = new List<WBS_Area>();

                                for (int k = 0; k < objHotelSearchRS.HotelListItems[j].AreaIDs.Length; k++)
                                {
                                    WBS_Area objArea = new WBS_Area();
                                    lAreas.Add(objArea);

                                    objArea.AreaID = objHotelSearchRS.HotelListItems[j].AreaIDs[k];
                                }

                                objProperty.Areas = lAreas.ToArray();
                            }

                        }

                    }

                }

            }

            objPropertyList.Property = lProperties.ToArray();
        }

        XmlSerializer ser = new XmlSerializer(objPropertyList.GetType());
        MemoryStream ms = new MemoryStream();
        ser.Serialize(ms, objPropertyList);

        UTF8Encoding utf8 = new UTF8Encoding();
        string strPropertyListXML = utf8.GetString(ms.ToArray());
        ms.Close();

        try
        {
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(strPropertyListXML);
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
