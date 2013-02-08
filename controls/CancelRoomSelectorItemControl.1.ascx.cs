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
using MamaShelter;

public partial class CancelRoomSelectorItemControl : System.Web.UI.UserControl
{
    private string _RoomRefID;
    private RoomOccupantSelection _RoomOccupantSelection;
    private HotelBookingRoomType _RoomType;
    private HotelBookingRatePlan _RatePlan;
    private HotelBookingRate[] _Rates;
    private HotelBookingCancelPolicy _CancelPolicy;
    private string _HotelCode;
    private DateTime _ArrivalDate;
    private string _ConfirmationNumber;
    private bool _Selected;
    public SelectionMode SelectionMode { get; set; }

    private List<CancelAddOnPackageItemControl> lCancelAddOnPackageItemControls;

    public string RoomRefID
    {
        set
        {
            _RoomRefID = value;
        }

        get
        {
            return _RoomRefID;
        }

    }

    public RoomOccupantSelection RoomOccupantSelection
    {
        set
        {
            _RoomOccupantSelection = value;
        }

        get
        {
            return _RoomOccupantSelection;
        }

    }

    public HotelBookingRoomType RoomType
    {
        set
        {
            _RoomType = value;
        }

        get
        {
            return _RoomType;
        }

    }

    public HotelBookingRatePlan RatePlan
    {
        set
        {
            _RatePlan = value;
        }

        get
        {
            return _RatePlan;
        }

    }

    public HotelBookingRate[] Rates
    {
        set
        {
            _Rates = value;
        }

        get
        {
            return _Rates;
        }

    }

    public HotelBookingCancelPolicy CancelPolicy
    {
        set
        {
            _CancelPolicy = value;
        }

        get
        {
            return _CancelPolicy;
        }

    }

    public string HotelCode
    {
        set
        {
            _HotelCode = value;
        }

        get
        {
            return _HotelCode;
        }

    }

    public DateTime ArrivalDate
    {
        set
        {
            _ArrivalDate = value;
        }

        get
        {
            return _ArrivalDate;
        }

    }

    public string ConfirmationNumber
    {
        set
        {
            _ConfirmationNumber = value;
        }

        get
        {
            return _ConfirmationNumber;
        }

    }

    public bool Selected
    {
        set
        {
            _Selected = value;
        }

        get
        {
            return _Selected;
        }

    }

    public CancelAddOnPackageItemControl[] CancelAddOnPackageItems
    {
        get
        {
            if (lCancelAddOnPackageItemControls != null)
                return lCancelAddOnPackageItemControls.ToArray();
            else
                return new CancelAddOnPackageItemControl[0];
        }

    }

    public void Clear()
    {
        lCancelAddOnPackageItemControls = null;
        return;
    }

    public void AddCancelAddOnPackageItem(CancelAddOnPackageItemControl ucCancelAddOnPackageItemControl)
    {
        if (lCancelAddOnPackageItemControls == null)
        {
            lCancelAddOnPackageItemControls = new List<CancelAddOnPackageItemControl>();
        }

        lCancelAddOnPackageItemControls.Add(ucCancelAddOnPackageItemControl);
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack & !this.IsParentPreRender())
        {
            if (this.Request.Form.Get(cbRoomSelected.ClientID.Replace('_', '$')) != null) // form uses "name" not "id" property
                _Selected = true;
            else
                _Selected = false;
        }

        lblPolicyButton.Attributes.Add("onclick", string.Format("toggleDetail('{0}');", panCancellationPolicy.ClientID));

        return;
    }

    public void RenderUserControl()
    {
        decimal decTotalRate = 0;
        string strCurrencyCode = "";

        this.ApplyControlsToPage();

        lblRoomIdentifier.Text = (String)GetLocalResourceObject("RoomIdentifierText" + _RoomRefID);
        lblCancelPolicyText.Text = ((XnGR_WBS_Page)this.Page).CancellationPolicy(_CancelPolicy);

        StringBuilder sbRoomOccupantsInfo = new StringBuilder();

        sbRoomOccupantsInfo.Append(_RoomOccupantSelection.NumberAdults.ToString());
        sbRoomOccupantsInfo.Append(" ");
        sbRoomOccupantsInfo.Append((String)GetLocalResourceObject("AdultsInfo"));

        if (_RoomOccupantSelection.NumberChildren != 0)
        {
            sbRoomOccupantsInfo.Append(", ");
            sbRoomOccupantsInfo.Append(_RoomOccupantSelection.NumberChildren.ToString());
            sbRoomOccupantsInfo.Append(" ");
            sbRoomOccupantsInfo.Append((String)GetLocalResourceObject("ChildrenInfo"));
        }

        lblRoomIdentifier.Text += sbRoomOccupantsInfo.ToString();

        lblRoomTypeNameText.Text = _RoomType.Name;
        //lblRoomTypeDescriptionText.Text = _RoomType.ShortDescription;

        lblRatePlanNameText.Text = _RatePlan.Name;
        //lblRatePlanDescriptionText.Text = _RatePlan.ShortDescription;

        //lblCancelPolicyText.Text = ((XnGR_WBS_Page)this.Page).CancellationPolicy(_CancelPolicy);

        for (int i = 0; i < _Rates.Length; i++)
        {
            decTotalRate += _Rates[i].Amount * _Rates[i].NumNights;
            strCurrencyCode = _Rates[i].CurrencyCode;
        }

        //if (((XnGR_WBS_Page)this.Page).IsCancellationPenalty(_CancelPolicy, _HotelCode, _ArrivalDate))
        //    panCancelPenaltyApplies.Visible = true;
        //else
        //    panCancelPenaltyApplies.Visible = false;

        lblTotalRoomPrice.Text = MamaShelter.MamaShelterHelper.RateAmmountString(decTotalRate, _Rates[0].CurrencyCode);

        if (_Selected == true)
            cbRoomSelected.Checked = true;
        else
            cbRoomSelected.Checked = false;

        for (int i = 0; i < lCancelAddOnPackageItemControls.Count; i++)
            lCancelAddOnPackageItemControls[i].RenderUserControl();

        if (SelectionMode == MamaShelter.SelectionMode.NonModifiable)
            cbRoomSelected.Visible = false;

        return;
    }

    private void ApplyControlsToPage()
    {
        if (lCancelAddOnPackageItemControls == null)
        {
            lCancelAddOnPackageItemControls = new List<CancelAddOnPackageItemControl>();
        }

        phAddOnPackages.Controls.Clear();

        for (int i = 0; i < lCancelAddOnPackageItemControls.Count; i++)
            phAddOnPackages.Controls.Add(lCancelAddOnPackageItemControls[i]);

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }
}

