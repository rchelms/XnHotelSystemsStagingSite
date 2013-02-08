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

public partial class AlternateHotelRatePlanItemControl : System.Web.UI.UserControl
{
    private HotelAvailRatePlan _RatePlan;
    private HotelAvailRoomRate[] _RoomRates;

    public HotelAvailRatePlan RatePlan
    {
        get
        {
            return _RatePlan;
        }

        set
        {
            _RatePlan = value;
        }

    }

    public HotelAvailRoomRate[] RoomRates
    {
        get
        {
            return _RoomRates;
        }

        set
        {
            _RoomRates = value;
        }

    }

    public void RenderUserControl()
    {
        StringBuilder sb;

        lblRatePlanNameText.Text = _RatePlan.Name;
        lblRatePlanDescriptionText.Text = _RatePlan.ShortDescription;

        decimal decMinRate = 0;
        string strCurrencyCode = "";

        if (_RoomRates.Length > 0)
        {
            decMinRate = this.GetMinRate(_RoomRates[0].Rates);
            strCurrencyCode = _RoomRates[0].Rates[0].CurrencyCode;

            for (int i = 1; i < _RoomRates.Length; i++)
            {
                decimal decRate = this.GetMinRate(_RoomRates[i].Rates);

                if (decRate < decMinRate)
                    decMinRate = decRate;
            }

        }

        sb = new StringBuilder();

        if (_RoomRates.Length > 0)
        {
            sb.Append(decMinRate.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat()));
            sb.Append(" ");
            sb.Append(strCurrencyCode);
        }

        lblLowestRateText.Text = sb.ToString();

        return;
    }

    protected decimal GetMinRate(HotelAvailRate[] objHotelAvailRates)
    {
        decimal decMinRate = 0;

        if (objHotelAvailRates.Length > 0)
        {
            decMinRate = objHotelAvailRates[0].Amount;

            for (int i = 1; i < objHotelAvailRates.Length; i++)
            {
                if (objHotelAvailRates[i].Amount < decMinRate)
                    decMinRate = objHotelAvailRates[i].Amount;
            }

        }

        return decMinRate;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
