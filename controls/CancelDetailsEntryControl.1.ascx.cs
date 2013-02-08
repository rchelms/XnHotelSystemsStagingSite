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

public partial class CancelDetailsEntryControl : System.Web.UI.UserControl
{
    public delegate void CancelDetailsCompletedEvent(object sender, EventArgs e);
    public event CancelDetailsCompletedEvent CancelDetailsCompleted;

    private HotelListItem[] _HotelListItems;
    private CancelDetailsEntryInfo _CancelDetailsEntryInfo;

    public HotelListItem[] HotelListItems
    {
        get
        {
            return _HotelListItems;
        }

        set
        {
            _HotelListItems = value;
        }

    }

    public CancelDetailsEntryInfo CancelDetailsEntryInfo
    {
        get
        {
            return _CancelDetailsEntryInfo;
        }

        set
        {
            _CancelDetailsEntryInfo = value;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack && !this.IsParentPreRender())
        {
            if (this.Request.Form.Get(ddlHotelList.ClientID.Replace('_', '$')) != null)
                _CancelDetailsEntryInfo.HotelCode = this.Request.Form.Get(ddlHotelList.ClientID.Replace('_', '$'));

            this.ApplyControlsToPage();
        }

        return;
    }

    public void RenderUserControl()
    {
        this.ApplyControlsToPage();

        for (int i = 0; i < ddlHotelList.Items.Count; i++)
        {
            if (ddlHotelList.Items[i].Value == _CancelDetailsEntryInfo.HotelCode)
                ddlHotelList.Items[i].Selected = true;
        }

        tbConfirmationNumber.Text = _CancelDetailsEntryInfo.ConfirmationNumber;
        tbGuestLastName.Text = _CancelDetailsEntryInfo.GuestLastName;

        return;
    }

    protected void btnLocateBooking_Click(object sender, EventArgs e)
    {
        CancelDetailsEntryInfo objCancelDetailsEntryInfo = new CancelDetailsEntryInfo();

        objCancelDetailsEntryInfo.HotelCode = _CancelDetailsEntryInfo.HotelCode;
        objCancelDetailsEntryInfo.ConfirmationNumber = tbConfirmationNumber.Text.ToUpper().Trim();
        objCancelDetailsEntryInfo.GuestLastName = tbGuestLastName.Text.ToUpper().Trim();
        objCancelDetailsEntryInfo.SelectedConfirmationNumbersToCancel = new string[0];

        _CancelDetailsEntryInfo = objCancelDetailsEntryInfo;

        CancelDetailsCompleted(this, new EventArgs());

        return;
    }

    private void ApplyControlsToPage()
    {
        ddlHotelList.Items.Clear();

        List<ListItem> lHotelListItems = new List<ListItem>();

        for (int i = 0; i < _HotelListItems.Length; i++)
        {
            ListItem liHotel = new ListItem();
            lHotelListItems.Add(liHotel);

            liHotel.Value = _HotelListItems[i].HotelCode;
            liHotel.Text = _HotelListItems[i].HotelName;
        }

        if (lHotelListItems.Count != 1)
        {
            ddlHotelList.Items.Add(new ListItem(GetLocalResourceObject("ddlSelectHotel.Text").ToString(), ""));

            for (int i = 0; i < lHotelListItems.Count; i++)
            {
                ddlHotelList.Items.Add(lHotelListItems[i]);
            }

        }

        else
        {
            ddlHotelList.Items.Add(lHotelListItems[0]);
        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
