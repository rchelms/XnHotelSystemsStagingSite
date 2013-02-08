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

public partial class BookingSummaryAddOnPackageItemControl : System.Web.UI.UserControl
{
    private string _RoomRefID;
    private int _NumberStayNights;
    private int _PackageQuantity;
    private HotelDescPackage _PackageDescription;
    private HotelAvailPackage _PackageRate;

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

    public int NumberStayNights
    {
        set
        {
            _NumberStayNights = value;
        }

        get
        {
            return _NumberStayNights;
        }

    }

    public int PackageQuantity
    {
        set
        {
            _PackageQuantity = value;
        }

        get
        {
            return _PackageQuantity;
        }

    }

    public HotelDescPackage PackageDescription
    {
        set
        {
            _PackageDescription = value;
        }

        get
        {
            return _PackageDescription;
        }

    }

    public HotelAvailPackage PackageRate
    {
        set
        {
            _PackageRate = value;
        }

        get
        {
            return _PackageRate;
        }

    }

    public void RenderUserControl()
    {
        lblPackageNameText.Text = _PackageRate.Name;

        if (_PackageRate.PriceType == PackagePriceType.PerPersonPerNight || _PackageRate.PriceType == PackagePriceType.PerPerson)
        {
            lblPackageQuantityUnits.Text = (String)GetLocalResourceObject("PackageQuantityUnitsAdults");
        }

        else
        {
            lblPackageQuantityUnits.Text = "";
        }

        StringBuilder sbPackagePrice = new StringBuilder();

        sbPackagePrice.Append(_PackageRate.CurrencyCode);
        sbPackagePrice.Append(" ");
        sbPackagePrice.Append(_PackageRate.Price.ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat()));

        lblPackagePrice.Text = sbPackagePrice.ToString();

        lblPackagePriceType.Text = (String)GetLocalResourceObject("PackageType" + _PackageRate.PriceType.ToString());

        if (_PackageRate.PriceType == PackagePriceType.PerStayPerNight || _PackageRate.PriceType == PackagePriceType.PerPersonPerNight)
        {
            lblPackageQuantity.Text = _PackageQuantity.ToString();

            StringBuilder sbPackagePriceNights = new StringBuilder();

            sbPackagePriceNights.Append((String)GetLocalResourceObject("PackagePriceNightsPrefix"));
            sbPackagePriceNights.Append(" ");
            sbPackagePriceNights.Append(_NumberStayNights.ToString());
            sbPackagePriceNights.Append(" ");
            sbPackagePriceNights.Append((String)GetLocalResourceObject("PackagePriceNightsSuffix"));

            lblPackagePriceNights.Text = sbPackagePriceNights.ToString();
        }

        else
        {
            lblPackageQuantity.Text = _PackageQuantity.ToString();
            lblPackagePriceNights.Text = "";
        }

        StringBuilder sbTotalPackagePrice = new StringBuilder();

        sbTotalPackagePrice.Append(_PackageRate.CurrencyCode);
        sbTotalPackagePrice.Append(" ");

        if (_PackageRate.PriceType == PackagePriceType.PerStayPerNight || _PackageRate.PriceType == PackagePriceType.PerPersonPerNight)
            sbTotalPackagePrice.Append(((decimal)(_PackageQuantity * _PackageRate.Price * _NumberStayNights)).ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat()));
        else
            sbTotalPackagePrice.Append(((decimal)(_PackageQuantity * _PackageRate.Price)).ToString(((XnGR_WBS_Page)this.Page).CurrencyFormat()));

        lblTotalPackagePrice.Text = sbTotalPackagePrice.ToString();

        return;
    }

}
