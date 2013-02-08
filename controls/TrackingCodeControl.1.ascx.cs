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
using XHS.WBSUIBizObjects;

public partial class TrackingCodeControl : System.Web.UI.UserControl
{
    private List<TrackingCodeItemControl> lTrackingCodeItems;

    public TrackingCodeItemControl[] TrackingCodeItems
    {
        get
        {
            if (lTrackingCodeItems != null)
                return lTrackingCodeItems.ToArray();
            else
                return new TrackingCodeItemControl[0];
        }

    }

    public void Clear()
    {
        lTrackingCodeItems = null;

        return;
    }

    public void Add(TrackingCodeItemControl ucTrackingCodeItem)
    {
        if (lTrackingCodeItems == null)
        {
            lTrackingCodeItems = new List<TrackingCodeItemControl>();
        }

        lTrackingCodeItems.Add(ucTrackingCodeItem);

        return;
    }

    public void RenderUserControl()
    {
        this.ApplyControlsToPage();

        for (int i = 0; i < lTrackingCodeItems.Count; i++)
            lTrackingCodeItems[i].RenderUserControl();

        return;
    }

    private void ApplyControlsToPage()
    {
        phTrackingCodeItems.Controls.Clear();

        if (lTrackingCodeItems == null)
        {
            lTrackingCodeItems = new List<TrackingCodeItemControl>();
        }

        for (int i = 0; i < lTrackingCodeItems.Count; i++)
        {
            phTrackingCodeItems.Controls.Add(lTrackingCodeItems[i]);
        }

        return;
    }

}

