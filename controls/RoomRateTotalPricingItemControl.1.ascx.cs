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

public partial class RoomRateTotalPricingItemControl : System.Web.UI.UserControl
{
    private TotalPricingItemType _ItemType;
    private TotalPricingPriceType _PriceType;

    private string _Date;
    private string _Description;
    private string _Price;
    private string _Tax;
    private string _Fee;
    private string _Subtotal;

    public TotalPricingItemType ItemType
    {
        set
        {
            _ItemType = value;
        }

        get
        {
            return _ItemType;
        }

    }

    public TotalPricingPriceType PriceType
    {
        set
        {
            _PriceType = value;
        }

        get
        {
            return _PriceType;
        }

    }

    public string Date
    {
        set
        {
            _Date = value;
        }

        get
        {
            return _Date;
        }

    }

    public string Description
    {
        set
        {
            _Description = value;
        }

        get
        {
            return _Description;
        }

    }

    public string Price
    {
        set
        {
            _Price = value;
        }

        get
        {
            return _Price;
        }

    }

    public string Tax
    {
        set
        {
            _Tax = value;
        }

        get
        {
            return _Tax;
        }

    }

    public string Fee
    {
        set
        {
            _Fee = value;
        }

        get
        {
            return _Fee;
        }

    }

    public string Subtotal
    {
        set
        {
            _Subtotal = value;
        }

        get
        {
            return _Subtotal;
        }

    }

    public void RenderUserControl()
    {
        lblRoomRatesTotalPricingPopupDataDate.Text = _Date;
        lblRoomRatesTotalPricingPopupDataItemDesc.Text = _Description;
        lblRoomRatesTotalPricingPopupDataItemPrice.Text = _Price;
        lblRoomRatesTotalPricingPopupDataItemTax.Text = _Tax;
        lblRoomRatesTotalPricingPopupDataItemFee.Text = _Fee;
        lblRoomRatesTotalPricingPopupDataItemSubtotal.Text = _Subtotal;

        return;
    }

}
