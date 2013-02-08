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

public partial class CancelRoomSelectorControl : System.Web.UI.UserControl
{
    public delegate void CancelRoomCompletedEvent(object sender, EventArgs e);
    public event CancelRoomCompletedEvent CancelRoomCompleted;

    private HotelDescriptiveInfo _HotelDescriptiveInfo;
    private HotelBookingReadSegment[] _BookingReadSegments;

    List<CancelRoomSelectorItemControl> lCancelRoomSelectorItemControls;
    List<string> lConfirmationNumberSelections;
    public SelectionMode Mode { get; set; }

    public HotelDescriptiveInfo HotelDescriptiveInfo
    {
        get
        {
            return _HotelDescriptiveInfo;
        }

        set
        {
            _HotelDescriptiveInfo = value;
        }

    }

    public HotelBookingReadSegment[] BookingReadSegments
    {
        get
        {
            return _BookingReadSegments;
        }

        set
        {
            _BookingReadSegments = value;
        }

    }

    public string[] ConfirmationNumberSelections
    {
        get
        {
            if (lConfirmationNumberSelections != null)
                return lConfirmationNumberSelections.ToArray();
            else
                return new string[0];
        }

    }

    public CancelRoomSelectorItemControl[] CancelRoomSelectorItems
    {
        get
        {
            if (lCancelRoomSelectorItemControls != null)
                return lCancelRoomSelectorItemControls.ToArray();
            else
                return new CancelRoomSelectorItemControl[0];
        }

    }

    public void Clear()
    {
        _HotelDescriptiveInfo = new HotelDescriptiveInfo();
        _BookingReadSegments = new HotelBookingReadSegment[0];

        lCancelRoomSelectorItemControls = null;
        lConfirmationNumberSelections = null;

        return;
    }

    public void AddCancelRoomSelectorItem(CancelRoomSelectorItemControl ucNewCancelRoomSelectorItem)
    {
        if (lCancelRoomSelectorItemControls == null)
        {
            lCancelRoomSelectorItemControls = new List<CancelRoomSelectorItemControl>();
        }

        lCancelRoomSelectorItemControls.Add(ucNewCancelRoomSelectorItem);

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

        for (int i = 0; i < _HotelDescriptiveInfo.Images.Length; i++)
        {
            if (_HotelDescriptiveInfo.Images[i].CategoryCode.ToString() == "ExteriorView" && _HotelDescriptiveInfo.Images[i].ImageSize.ToString() == "Thumbnail")
            {
                imgHotel.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _HotelDescriptiveInfo.Images[i].ImageURL.ToString();
            }

        }

        lblHotelNameText.Text = _HotelDescriptiveInfo.HotelName;

        StringBuilder sbAddressAddressInfo = new StringBuilder();

        if (_HotelDescriptiveInfo.Address1.Trim() != "")
        {
            sbAddressAddressInfo.Append(_HotelDescriptiveInfo.Address1);
        }

        if (_HotelDescriptiveInfo.Address2.Trim() != "")
        {
            if (sbAddressAddressInfo.ToString().Trim() != "")
            {
                sbAddressAddressInfo.Append(", ");
            }

            sbAddressAddressInfo.Append(_HotelDescriptiveInfo.Address1);
        }

        if (_HotelDescriptiveInfo.City.Trim() != "")
        {
            if (sbAddressAddressInfo.ToString().Trim() != "")
            {
                sbAddressAddressInfo.Append(", ");
            }

            sbAddressAddressInfo.Append(_HotelDescriptiveInfo.City);
        }

        if (_HotelDescriptiveInfo.PostalCode.Trim() != "" || _HotelDescriptiveInfo.Country.Trim() != "")
        {
            if (sbAddressAddressInfo.ToString().Trim() != "")
            {
                sbAddressAddressInfo.Append(", ");
            }

            sbAddressAddressInfo.Append(_HotelDescriptiveInfo.PostalCode.Trim() + " " + _HotelDescriptiveInfo.Country.Trim());
        }

        lblHotelAddressInfo.Text = sbAddressAddressInfo.ToString();
        lblTelephoneNumber.Text = _HotelDescriptiveInfo.Phone;
        lblFaxNumber.Text = _HotelDescriptiveInfo.Fax;
        lblEmailAddress.Text = _HotelDescriptiveInfo.Email;

        if (_BookingReadSegments != null && _BookingReadSegments.Length != 0)
        {
            lblArrivalDateText.Text = _BookingReadSegments[0].ArrivalDate.ToString("dddd, dd MMMM yyyy");
            lblDepartureDateText.Text = _BookingReadSegments[0].DepartureDate.ToString("dddd, dd MMMM yyyy");
        }

        for (int i = 0; i < lCancelRoomSelectorItemControls.Count; i++)
            lCancelRoomSelectorItemControls[i].RenderUserControl();

        if (Mode == SelectionMode.NonModifiable)
        {
            panCancelBooking.Visible = false;
        }

        return;
    }

    protected void btnCancelBooking_Click(object sender, EventArgs e)
    {
        this.GetConfirmationNumberSelections();
        CancelRoomCompleted(this, new EventArgs());
        return;
    }

    private void ApplyControlsToPage()
    {
        if (lCancelRoomSelectorItemControls == null)
        {
            lCancelRoomSelectorItemControls = new List<CancelRoomSelectorItemControl>();
        }

        phRooms.Controls.Clear();

        for (int i = 0; i < lCancelRoomSelectorItemControls.Count; i++)
            phRooms.Controls.Add(lCancelRoomSelectorItemControls[i]);

        return;
    }

    private void GetConfirmationNumberSelections()
    {
        if (lConfirmationNumberSelections == null)
        {
            lConfirmationNumberSelections = new List<string>();
        }

        lConfirmationNumberSelections.Clear();

        for (int i = 0; i < lCancelRoomSelectorItemControls.Count; i++)
        {
            if (lCancelRoomSelectorItemControls[i].Selected)
            {
                lConfirmationNumberSelections.Add(lCancelRoomSelectorItemControls[i].ConfirmationNumber);
            }

        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
