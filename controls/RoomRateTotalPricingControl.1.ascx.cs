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

public partial class RoomRateTotalPricingControl : System.Web.UI.UserControl
{
    private List<RoomRateTotalPricingItemControl> lRoomRateTotalPricingItems;

    public RoomRateTotalPricingItemControl[] RoomRateTotalPricingItems
    {
        get
        {
            if (lRoomRateTotalPricingItems != null)
                return lRoomRateTotalPricingItems.ToArray();
            else
                return new RoomRateTotalPricingItemControl[0];
        }

    }

    public void Clear()
    {
        lRoomRateTotalPricingItems = null;
        return;
    }

    public void AddRoomRateTotalPricingItem(RoomRateTotalPricingItemControl ucNewRoomRateTotalPricingItem)
    {
        if (lRoomRateTotalPricingItems == null)
        {
            lRoomRateTotalPricingItems = new List<RoomRateTotalPricingItemControl>();
        }

        lRoomRateTotalPricingItems.Add(ucNewRoomRateTotalPricingItem);
        return;
    }

    public void RenderUserControl()
    {
        this.ApplyControlsToPage();

        lbRoomRateTotalPricing.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_RoomRateTotalPricingPopup'} )");

        for (int i = 0; i < lRoomRateTotalPricingItems.Count; i++)
            lRoomRateTotalPricingItems[i].RenderUserControl();

        return;
    }

    private void ApplyControlsToPage()
    {
        if (lRoomRateTotalPricingItems == null)
        {
            lRoomRateTotalPricingItems = new List<RoomRateTotalPricingItemControl>();
        }

        phRoomRatesTotalPricingPopupSectionPerNightItems.Controls.Clear();
        phRoomRatesTotalPricingPopupSectionPerStayItems.Controls.Clear();
        phRoomRatesTotalPricingPopupSectionTotalItems.Controls.Clear();

        for (int i = 0; i < lRoomRateTotalPricingItems.Count; i++)
        {
            if (lRoomRateTotalPricingItems[i].PriceType == TotalPricingPriceType.PerNight)
                phRoomRatesTotalPricingPopupSectionPerNightItems.Controls.Add(lRoomRateTotalPricingItems[i]);

            else if (lRoomRateTotalPricingItems[i].PriceType == TotalPricingPriceType.PerStay)
                phRoomRatesTotalPricingPopupSectionPerStayItems.Controls.Add(lRoomRateTotalPricingItems[i]);

            else if (lRoomRateTotalPricingItems[i].PriceType == TotalPricingPriceType.Total)
                phRoomRatesTotalPricingPopupSectionTotalItems.Controls.Add(lRoomRateTotalPricingItems[i]);
        }

        panPerNightSection.Visible = false;
        panPerStaySection.Visible = false;
        panTotalSection.Visible = false;

        if (phRoomRatesTotalPricingPopupSectionPerNightItems.Controls.Count != 0)
            panPerNightSection.Visible = true;

        if (phRoomRatesTotalPricingPopupSectionPerStayItems.Controls.Count != 0)
            panPerStaySection.Visible = true;

        if (phRoomRatesTotalPricingPopupSectionTotalItems.Controls.Count != 0)
            panTotalSection.Visible = true;

        return;
    }

}
