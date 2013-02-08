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

public partial class RoomSelectorItemControl : System.Web.UI.UserControl
{
    public delegate void RoomSelectedEvent(object sender, EventArgs e);
    public event RoomSelectedEvent RoomSelected;

    private string _RoomRefID;
    private string _RoomRefIDMenuText;
    private bool _Selected;

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

    public string RoomRefIDMenuText
    {
        get
        {
            return _RoomRefIDMenuText;
        }

        set
        {
            _RoomRefIDMenuText = value;
        }

    }

    public bool Selected
    {
        get
        {
            return _Selected;
        }

        set
        {
            _Selected = value;
        }

    }

    public void RenderUserControl()
    {
        panRoomSelectorItemSelected.Visible = false;
        panRoomSelectorItemNotSelected.Visible = false;

        panRoomSelectorItemNotSelected.Attributes.Remove("onclick");

        lblRoomSelectorItemSelected.Text = _RoomRefIDMenuText;
        lblRoomSelectorItemNotSelected.Text = _RoomRefIDMenuText;

        if (_Selected)
        {
            panRoomSelectorItemSelected.Visible = true;
        }
        else
        {
            panRoomSelectorItemNotSelected.Visible = true;
            panRoomSelectorItemNotSelected.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(btnRoomSelectorItemNotSelected, ""));
        }

        return;
    }

    protected void btnRoomSelectorItem_Click(object sender, EventArgs e)
    {
        _Selected = true;
        RoomSelected(this, new EventArgs());
        return;
    }

}

