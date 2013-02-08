using System;
using System.Text;
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
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class AlternateHotelSelectorControl : System.Web.UI.UserControl
{
    public delegate void AlternateHotelSelectedEvent(object sender, EventArgs e);
    public event AlternateHotelSelectedEvent AlternateHotelSelected;

    private string _SelectedHotelCode;

    private List<AlternateHotelSelectorItemControl> lAlternateHotelSelectorItemControls;

    public string SelectedHotelCode
    {
        get
        {
            return _SelectedHotelCode;
        }

    }

    public AlternateHotelSelectorItemControl[] AlternateHotelSelectorItems
    {
        get
        {
            if (lAlternateHotelSelectorItemControls != null)
                return lAlternateHotelSelectorItemControls.ToArray();
            else
                return new AlternateHotelSelectorItemControl[0];
        }

    }

    public void Clear()
    {
        _SelectedHotelCode = "";
        lAlternateHotelSelectorItemControls = null;
        return;
    }

    public void Add(AlternateHotelSelectorItemControl ucAlternateHotelSelectorItemControl)
    {
        if (lAlternateHotelSelectorItemControls == null)
        {
            lAlternateHotelSelectorItemControls = new List<AlternateHotelSelectorItemControl>();
        }

        lAlternateHotelSelectorItemControls.Add(ucAlternateHotelSelectorItemControl);
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack && !this.IsParentPreRender())
        {
            this.ApplyControlsToPage();
        }

        return;
    }

    public void RenderUserControl()
    {
        ApplyControlsToPage();

        for (int i = 0; i < lAlternateHotelSelectorItemControls.Count; i++)
            lAlternateHotelSelectorItemControls[i].RenderUserControl();

        return;
    }

    public void AlternateHotelItemSelected(object sender, string HotelCode)
    {
        _SelectedHotelCode = HotelCode;
        AlternateHotelSelected(sender, new EventArgs());
        return;

    }

    private void ApplyControlsToPage()
    {
        if (lAlternateHotelSelectorItemControls == null)
        {
            lAlternateHotelSelectorItemControls = new List<AlternateHotelSelectorItemControl>();
        }

        phAlternateHotelItems.Controls.Clear();

        for (int i = 0; i < lAlternateHotelSelectorItemControls.Count; i++)
        {
            lAlternateHotelSelectorItemControls[i].AlternateHotelSelected += new AlternateHotelSelectorItemControl.AlternateHotelSelectedEvent(this.AlternateHotelItemSelected);
            phAlternateHotelItems.Controls.Add(lAlternateHotelSelectorItemControls[i]);

            lAlternateHotelSelectorItemControls[i].RenderUserControl();
        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
