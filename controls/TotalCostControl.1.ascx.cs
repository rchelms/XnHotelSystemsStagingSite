using System;
using System.Web.UI;
using MamaShelter;

public partial class TotalCostControl : System.Web.UI.UserControl
{
    public delegate void ProcessPaymentEventHandler(bool isHold);
    public event ProcessPaymentEventHandler ProceedToPayment;

    public decimal TotalCost;
    public string CurrencyCode;
    public SelectionMode Mode;
    public bool IsBookingConfirmed = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //btnHold.Click += new EventHandler(btnHold_Click);
        btnPayNow.Click += new EventHandler(btnPaynow_Click);
    }

    void btnPaynow_Click(object sender, EventArgs e)
    {
        if (ProceedToPayment != null)
            ProceedToPayment(false);
    }

    void btnHold_Click(object sender, EventArgs e)
    {
        if (ProceedToPayment != null)
            ProceedToPayment(true);
    }

    public void RenderUserControl()
    {
        

        lblTotalCost.Text = lblTempTotalCost.Text = lblGrandTotal.Text = FormatCurrencyString(TotalCost, CurrencyCode);

        if (Mode == SelectionMode.Hidden)
        {
            panTempTotalCostContent.Visible = false;
            panSummaryBooking.Visible = false;
            panTotalCostInfo.Visible = false;
            return;
        }
        else if (Mode == SelectionMode.Edit)
        {
            panTempTotalCostContent.Visible = true;
            panSummaryBooking.Visible = false;
            panTotalCostInfo.Visible = false;
        }
        else if (Mode == SelectionMode.Selected && !IsBookingConfirmed)
        {
            panTempTotalCostContent.Visible = false;
            panSummaryBooking.Visible = true;
            panTotalCostInfo.Visible = false;
        }
        else if (Mode == SelectionMode.Selected && IsBookingConfirmed)
        {
            panTempTotalCostContent.Visible = false;
            panSummaryBooking.Visible = false;
            panTotalCostInfo.Visible = true;
        }

        if (TotalCost == 0)
            panTempTotalCostContent.Visible = false;

    }

    private string FormatCurrencyString(decimal value, string currencyCode)
    {
        string formatString = value.ToString("F2").EndsWith("00") ? "F0" : ((XnGR_WBS_Page)Page).CurrencyFormat();
        return string.Format("<span class=\"mm_rate_plan_currency_symbol\">{0}</span>{1}", WebconfigHelper.GetCurrencyCodeString(currencyCode), value.ToString(formatString));
    }
}
