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

public partial class BookingStepControl : System.Web.UI.UserControl
{
    public delegate void BookingStepSelectedEvent(object sender, EventArgs e);
    public event BookingStepSelectedEvent BookingStepSelected;

    private string _SelectedStep;
    private XHS.WBSUIBizObjects.OrientationMode _OrientationMode;

    private List<BookingStepItemControl> lBookingStepItems;
    private Table tblFormat;

    public string SelectedStep
    {
        get
        {
            return _SelectedStep;
        }

        set
        {
            _SelectedStep = value;
        }

    }

    public OrientationMode OrientationMode
    {
        get
        {
            return _OrientationMode;
        }

        set
        {
            _OrientationMode = value;
        }

    }

    public BookingStepItemControl[] BookingStepItems
    {
        get
        {
            if (lBookingStepItems != null)
                return lBookingStepItems.ToArray();
            else
                return new BookingStepItemControl[0];
        }

    }

    public void Clear()
    {
        _SelectedStep = "";
        _OrientationMode = XHS.WBSUIBizObjects.OrientationMode.Horizontal;

        lBookingStepItems = null;

        return;
    }

    public void Add(BookingStepItemControl ucBookingStepItem)
    {
        if (lBookingStepItems == null)
        {
            lBookingStepItems = new List<BookingStepItemControl>();
        }

        lBookingStepItems.Add(ucBookingStepItem);

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

        for (int i = 0; i < lBookingStepItems.Count; i++)
            lBookingStepItems[i].RenderUserControl();

        return;
    }

    public void BookingStepItemSelected(object sender, EventArgs e)
    {
        _SelectedStep = ((BookingStepItemControl)sender).StepRefID;

        for (int i = 0; i < lBookingStepItems.Count; i++)
        {
            if (lBookingStepItems[i].StepRefID != _SelectedStep)
                lBookingStepItems[i].Selected = false;
        }

        BookingStepSelected(this, new EventArgs());

        return;
    }

    private void ApplyControlsToPage()
    {
        phBookingStepItems.Controls.Clear();

        tblFormat = new Table();
        phBookingStepItems.Controls.Add(tblFormat);

        tblFormat.Attributes.Add("cellspacing", "0");
        tblFormat.Attributes.Add("cellpadding", "0");
        tblFormat.Attributes.Add("border", "0");

        if (lBookingStepItems == null)
        {
            lBookingStepItems = new List<BookingStepItemControl>();
        }

        for (int i = 0; i < lBookingStepItems.Count; i++)
        {
            if (_OrientationMode == XHS.WBSUIBizObjects.OrientationMode.Horizontal)
            {
                if (tblFormat.Rows.Count == 0)
                {
                    TableRow tr = new TableRow();
                    tblFormat.Rows.Add(tr);
                }

                TableCell td = new TableCell();
                tblFormat.Rows[0].Cells.Add(td);

                lBookingStepItems[i].BookingStepSelected += new BookingStepItemControl.BookingStepSelectedEvent(this.BookingStepItemSelected);
                td.Controls.Add(lBookingStepItems[i]);
            }

            else if (_OrientationMode == XHS.WBSUIBizObjects.OrientationMode.Vertical)
            {
                TableRow tr = new TableRow();
                tblFormat.Rows.Add(tr);

                TableCell td = new TableCell();
                tr.Cells.Add(td);

                lBookingStepItems[i].BookingStepSelected += new BookingStepItemControl.BookingStepSelectedEvent(this.BookingStepItemSelected);
                td.Controls.Add(lBookingStepItems[i]);
            }

        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}

