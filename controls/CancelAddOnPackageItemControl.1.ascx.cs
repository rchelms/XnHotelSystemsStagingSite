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

public partial class CancelAddOnPackageItemControl : System.Web.UI.UserControl
{
    private string _RoomRefID;
    private int _NumberStayNights;
    private int _PackageQuantity;
    private HotelBookingPackageRate _PackageRate;

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

    public HotelBookingPackageRate PackageRate
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

        //if (_PackageRate.PriceType == PackagePriceType.PerPersonPerNight || _PackageRate.PriceType == PackagePriceType.PerPerson)
        //{
        //    lblPackageQuantityUnits.Text = (String)GetLocalResourceObject("PackageQuantityUnitsAdults");
        //}

        //else
        //{
        //    lblPackageQuantityUnits.Text = "";
        //}

        StringBuilder sbPackagePrice = new StringBuilder();
        sbPackagePrice.Append(MamaShelter.MamaShelterHelper.RateAmmountString(_PackageRate.Price, _PackageRate.CurrencyCode));

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

        decimal totalPackagePrice = 0;

        if (_PackageRate.PriceType == PackagePriceType.PerStayPerNight || _PackageRate.PriceType == PackagePriceType.PerPersonPerNight)
            totalPackagePrice = _PackageQuantity * _PackageRate.Price * _NumberStayNights;
        else
            totalPackagePrice = _PackageQuantity * _PackageRate.Price;

        lblTotalPackagePrice.Text = MamaShelter.MamaShelterHelper.RateAmmountString(totalPackagePrice, _PackageRate.CurrencyCode);

        return;
    }

}
