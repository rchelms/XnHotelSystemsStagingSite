using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class BookingStepItemControl : System.Web.UI.UserControl
{
    public delegate void BookingStepSelectedEvent(object sender, EventArgs e);
    public event BookingStepSelectedEvent BookingStepSelected;

    private string _StepRefID = "";
    private string _StepNumberText = "";
    private string _StepDescriptionText = "";
    private bool _Selected;
    private bool _Clickable;

    public string StepRefID
    {
        get
        {
            return _StepRefID;
        }

        set
        {
            _StepRefID = value;
        }

    }

    public string StepNumberText
    {
        get
        {
            return _StepNumberText;
        }

        set
        {
            _StepNumberText = value;
        }

    }

    public string StepDescriptionText
    {
        get
        {
            return _StepDescriptionText;
        }

        set
        {
            _StepDescriptionText = value;
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

    public bool Clickable
    {
        get
        {
            return _Clickable;
        }

        set
        {
            _Clickable = value;
        }

    }

    public void RenderUserControl()
    {
        panPastBookingStepItem.Visible = false;
        panPresentBookingStepItem.Visible = false;
        panFutureBookingStepItem.Visible = false;

        panPastBookingStepItem.Attributes.Remove("onclick");

        lblPastStepNumberText.Text = _StepNumberText;
        lblPresentStepNumberText.Text = _StepNumberText;
        lblFutureStepNumberText.Text = _StepNumberText;

        lblPastStepDescriptionText.Text = _StepDescriptionText;
        lblPresentStepDescriptionText.Text = _StepDescriptionText;
        lblFutureStepDescriptionText.Text = _StepDescriptionText;

        if (_Clickable)
        {
            panPastBookingStepItem.Visible = true;
            panPastBookingStepItem.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(btnPastStep, ""));
        }

        if (_Selected)
        {
            panPresentBookingStepItem.Visible = true;
        }

        if (!_Selected && !_Clickable)
        {
            panFutureBookingStepItem.Visible = true;
        }

        return;
    }

    protected void btnBookingStepItem_Click(object sender, EventArgs e)
    {
        _Selected = true;
        BookingStepSelected(this, new EventArgs());
        return;
    }

}
