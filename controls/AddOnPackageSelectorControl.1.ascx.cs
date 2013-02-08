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

public partial class AddOnPackageSelectorControl : System.Web.UI.UserControl
{
    public delegate void RoomSelectedEvent(object sender, string selectedRoomRefID);
    public event RoomSelectedEvent RoomSelected;

    public delegate void AddOnPackageCompletedEvent(object sender, EventArgs e);
    public event AddOnPackageCompletedEvent AddOnPackageCompleted;

    private string _RoomRefID;

    private RoomOccupantSelection _RoomOccupantSelection;
    private AddOnPackageSelection[] _AddOnPackageSelections;

    private List<RoomSelectorItemControl> lRoomSelectorItemControls;
    private List<AddOnPackageSelectorItemControl> lAddOnPackageSelectorItemControls;

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

    public AddOnPackageSelection[] AddOnPackageSelections
    {
        get
        {
            if (_AddOnPackageSelections != null)
                return _AddOnPackageSelections;
            else
                return new AddOnPackageSelection[0];
        }

        set
        {
            _AddOnPackageSelections = value;
        }

    }

    public AddOnPackageSelectorItemControl[] AddOnPackageSelectorItems
    {
        get
        {
            if (lAddOnPackageSelectorItemControls != null)
                return lAddOnPackageSelectorItemControls.ToArray();
            else
                return new AddOnPackageSelectorItemControl[0];
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

    public void Clear()
    {
        _AddOnPackageSelections = null;

        lAddOnPackageSelectorItemControls = null;
        lRoomSelectorItemControls = null;

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

    public void AddAddOnPackageSelectorItem(AddOnPackageSelectorItemControl ucNewAddOnPackageSelectorItem)
    {
        if (lAddOnPackageSelectorItemControls == null)
        {
            lAddOnPackageSelectorItemControls = new List<AddOnPackageSelectorItemControl>();
        }

        lAddOnPackageSelectorItemControls.Add(ucNewAddOnPackageSelectorItem);

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

        panIsPackageInfo.Visible = false;
        panNoPackageInfo.Visible = false;

        if (lAddOnPackageSelectorItemControls != null && lAddOnPackageSelectorItemControls.Count != 0)
            panIsPackageInfo.Visible = true;
        else
            panNoPackageInfo.Visible = true;

        for (int i = 0; i < lRoomSelectorItemControls.Count; i++)
            lRoomSelectorItemControls[i].RenderUserControl();

        for (int i = 0; i < lAddOnPackageSelectorItemControls.Count; i++)
            lAddOnPackageSelectorItemControls[i].RenderUserControl();

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

        this.GetAddOnPackageSelection();
        RoomSelected(this, ucRoomSelectorItemControl.RoomRefID);
        return;
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        this.GetAddOnPackageSelection();
        AddOnPackageCompleted(this, new EventArgs());
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

        if (lAddOnPackageSelectorItemControls == null)
        {
            lAddOnPackageSelectorItemControls = new List<AddOnPackageSelectorItemControl>();
        }

        phAddonPackages.Controls.Clear();

        for (int i = 0; i < lAddOnPackageSelectorItemControls.Count; i++)
        {
            phAddonPackages.Controls.Add(lAddOnPackageSelectorItemControls[i]);
        }

        return;
    }

    private void GetAddOnPackageSelection()
    {
        List<AddOnPackageSelection> lAddOnPackageSelections = new List<AddOnPackageSelection>();

        for (int i = 0; i < lAddOnPackageSelectorItemControls.Count; i++)
        {
            if (lAddOnPackageSelectorItemControls[i].Selected)
            {
                AddOnPackageSelection objAddOnPackageSelection = new AddOnPackageSelection();
                lAddOnPackageSelections.Add(objAddOnPackageSelection);

                objAddOnPackageSelection.RoomRefID = lAddOnPackageSelectorItemControls[i].RoomRefID;
                objAddOnPackageSelection.PackageCode = lAddOnPackageSelectorItemControls[i].PackageDescription.Code;
                objAddOnPackageSelection.Quantity = lAddOnPackageSelectorItemControls[i].PackageQuantity;
            }

            _AddOnPackageSelections = lAddOnPackageSelections.ToArray();
        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
