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

public partial class SearchHotelSelectorControl : System.Web.UI.UserControl
{
    public delegate void SearchHotelSelectedEvent(object sender, EventArgs e);
    public event SearchHotelSelectedEvent SearchHotelSelected;

    private string _SelectedHotelCode;

    private List<SearchHotelSelectorItemControl> lSearchHotelSelectorItemControls;

    public string SelectedHotelCode
    {
        get
        {
            return _SelectedHotelCode;
        }

    }

    public SearchHotelSelectorItemControl[] SearchHotelSelectorItems
    {
        get
        {
            if (lSearchHotelSelectorItemControls != null)
                return lSearchHotelSelectorItemControls.ToArray();
            else
                return new SearchHotelSelectorItemControl[0];
        }

    }

    public void Clear()
    {
        _SelectedHotelCode = "";
        lSearchHotelSelectorItemControls = null;
        return;
    }

    public void Add(SearchHotelSelectorItemControl ucSearchHotelSelectorItemControl)
    {
        if (lSearchHotelSelectorItemControls == null)
        {
            lSearchHotelSelectorItemControls = new List<SearchHotelSelectorItemControl>();
        }

        lSearchHotelSelectorItemControls.Add(ucSearchHotelSelectorItemControl);
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

        for (int i = 0; i < lSearchHotelSelectorItemControls.Count; i++)
            lSearchHotelSelectorItemControls[i].RenderUserControl();

        return;
    }

    public void SearchHotelItemSelected(object sender, string HotelCode)
    {
        _SelectedHotelCode = HotelCode;
        SearchHotelSelected(sender, new EventArgs());
        return;

    }

    private void ApplyControlsToPage()
    {
        if (lSearchHotelSelectorItemControls == null)
        {
            lSearchHotelSelectorItemControls = new List<SearchHotelSelectorItemControl>();
        }

        phSearchHotelItems.Controls.Clear();

        for (int i = 0; i < lSearchHotelSelectorItemControls.Count; i++)
        {
            lSearchHotelSelectorItemControls[i].SearchHotelSelected += new SearchHotelSelectorItemControl.SearchHotelSelectedEvent(this.SearchHotelItemSelected);
            phSearchHotelItems.Controls.Add(lSearchHotelSelectorItemControls[i]);
        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
