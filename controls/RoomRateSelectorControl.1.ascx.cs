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

public partial class RoomRateSelectorControl : System.Web.UI.UserControl
{
    public delegate void RoomSelectedEvent(object sender, string selectedRoomRefID);
    public event RoomSelectedEvent RoomSelected;

    public delegate void RoomRateCompletedEvent(object sender, EventArgs e);
    public event RoomRateCompletedEvent RoomRateCompleted;

    public delegate void ShowMoreLessRatesEvent(object sender, EventArgs e);
    public event ShowMoreLessRatesEvent ShowMoreLessRates;

    private string _RoomRefID;
    private HotelRoomAvailInfo _HotelRoomAvailInfo;
    private RoomOccupantSelection _RoomOccupantSelection;
    private RoomRateSelection _RoomRateSelection;

    List<RoomSelectorItemControl> lRoomSelectorItemControls;
    List<RoomTypeSelectorItemControl> lRoomTypeSelectorItemControls;

    public string RoomRefID
    {
        get
        {
            return _RoomRefID;
        }

        set
        {
            _RoomRefID = value;
        }

    }

    public HotelRoomAvailInfo HotelRoomAvailInfo
    {
        get
        {
            return _HotelRoomAvailInfo;
        }

        set
        {
            _HotelRoomAvailInfo = value;
        }

    }

    public RoomOccupantSelection RoomOccupantSelection
    {
        get
        {
            return _RoomOccupantSelection;
        }

        set
        {
            _RoomOccupantSelection = value;
        }

    }

    public RoomRateSelection RoomRateSelection
    {
        get
        {
            return _RoomRateSelection;
        }

    }

    public RoomSelectorItemControl[] RoomSelectorItems
    {
        get
        {
            if (lRoomSelectorItemControls != null)
                return lRoomSelectorItemControls.ToArray();
            else
                return new RoomSelectorItemControl[0];
        }

    }

    public RoomTypeSelectorItemControl[] RoomTypeSelectorItems
    {
        get
        {
            if (lRoomTypeSelectorItemControls != null)
                return lRoomTypeSelectorItemControls.ToArray();
            else
                return new RoomTypeSelectorItemControl[0];
        }

    }

    public void Clear()
    {
        _RoomRateSelection = new RoomRateSelection();
        _RoomRateSelection.RoomRefID = "";
        _RoomRateSelection.RoomTypeCode = "";
        _RoomRateSelection.RatePlanCode = "";
        _RoomRateSelection.PromotionCode = "";

        lRoomSelectorItemControls = null;
        lRoomTypeSelectorItemControls = null;

        return;
    }

    public void AddRoomSelectorItem(RoomSelectorItemControl ucNewRoomSelectorItem)
    {
        if (lRoomSelectorItemControls == null)
        {
            lRoomSelectorItemControls = new List<RoomSelectorItemControl>();
        }

        lRoomSelectorItemControls.Add(ucNewRoomSelectorItem);

        return;
    }

    public void AddRoomTypeSelectorItem(RoomTypeSelectorItemControl ucNewRoomTypeSelectorItem)
    {
        if (lRoomTypeSelectorItemControls == null)
        {
            lRoomTypeSelectorItemControls = new List<RoomTypeSelectorItemControl>();
        }

        lRoomTypeSelectorItemControls.Add(ucNewRoomTypeSelectorItem);

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
        this.ApplyControlsToPage();

        StringBuilder sb = new StringBuilder();

        sb.Append(_RoomOccupantSelection.NumberAdults.ToString());
        sb.Append(" ");
        sb.Append((String)GetLocalResourceObject("AdultsInfo"));

        if (_RoomOccupantSelection.NumberChildren != 0)
        {
            sb.Append(", ");
            sb.Append(_RoomOccupantSelection.NumberChildren.ToString());
            sb.Append(" ");
            sb.Append((String)GetLocalResourceObject("ChildrenInfo"));
        }

        lblRoomInfoText.Text = sb.ToString();

        for (int i = 0; i < lRoomSelectorItemControls.Count; i++)
            lRoomSelectorItemControls[i].RenderUserControl();

        for (int i = 0; i < lRoomTypeSelectorItemControls.Count; i++)
            lRoomTypeSelectorItemControls[i].RenderUserControl();

        return;
    }

    public void RoomSelectorItemSelected(object sender, EventArgs e)
    {
        RoomSelectorItemControl ucRoomSelectorItemControl = (RoomSelectorItemControl)sender;

        for (int i = 0; i < lRoomSelectorItemControls.Count; i++)
        {
            if (lRoomSelectorItemControls[i].RoomRefID != ucRoomSelectorItemControl.RoomRefID)
            {
                lRoomSelectorItemControls[i].Selected = false;
            }

        }

        this.GetRoomRateSelection();
        RoomSelected(this, ucRoomSelectorItemControl.RoomRefID);
        return;
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        this.GetRoomRateSelection();
        RoomRateCompleted(this, new EventArgs());
        return;
    }

    public void ShowMoreLessRatesRequested(object sender, EventArgs e)
    {
        ShowMoreLessRates(this, new EventArgs());
        return;
    }

    private void ApplyControlsToPage()
    {
        if (lRoomSelectorItemControls == null)
        {
            lRoomSelectorItemControls = new List<RoomSelectorItemControl>();
        }

        phRoomSelectors.Controls.Clear();

        for (int i = 0; i < lRoomSelectorItemControls.Count; i++)
        {
            lRoomSelectorItemControls[i].RoomSelected += new RoomSelectorItemControl.RoomSelectedEvent(this.RoomSelectorItemSelected);
            phRoomSelectors.Controls.Add(lRoomSelectorItemControls[i]);
        }

        if (lRoomTypeSelectorItemControls == null)
        {
            lRoomTypeSelectorItemControls = new List<RoomTypeSelectorItemControl>();
        }

        phRoomTypes.Controls.Clear();

        for (int i = 0; i < lRoomTypeSelectorItemControls.Count; i++)
        {
            lRoomTypeSelectorItemControls[i].ShowMoreLessRates += new RoomTypeSelectorItemControl.ShowMoreLessRatesEvent(ShowMoreLessRatesRequested);
            phRoomTypes.Controls.Add(lRoomTypeSelectorItemControls[i]);
        }

        return;
    }

    private void GetRoomRateSelection()
    {
        bool bSelectedItemFound = false;

        _RoomRateSelection.RoomRefID = _RoomRefID;

        _RoomRateSelection.RoomTypeCode = "";
        _RoomRateSelection.RatePlanCode = "";
        _RoomRateSelection.PromotionCode = "";

        for (int i = 0; i < lRoomTypeSelectorItemControls.Count; i++)
        {
            for (int j = 0; j < lRoomTypeSelectorItemControls[i].RatePlanSelectorItems.Length; j++)
            {
                if (lRoomTypeSelectorItemControls[i].RatePlanSelectorItems[j].Selected)
                {
                    _RoomRateSelection.RoomTypeCode = lRoomTypeSelectorItemControls[i].RatePlanSelectorItems[j].RoomRate.RoomTypeCode;
                    _RoomRateSelection.RatePlanCode = lRoomTypeSelectorItemControls[i].RatePlanSelectorItems[j].RoomRate.RatePlanCode;
                    _RoomRateSelection.PromotionCode = lRoomTypeSelectorItemControls[i].RatePlanSelectorItems[j].RoomRate.PromotionCode;

                    bSelectedItemFound = true;
                    break;
                }

            }

            if (bSelectedItemFound)
                break;
        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
