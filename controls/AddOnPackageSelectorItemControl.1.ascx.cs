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

public partial class AddOnPackageSelectorItemControl : System.Web.UI.UserControl
{
   protected bool IsTrackingLoaded
   {
      get { return ViewState["xxx"] != null; }
   }

   public delegate void ToggleAddOnPackageEventHandler(string roomRefID, string packageCode, int quantity, bool isSelected);
   public event ToggleAddOnPackageEventHandler AddOnToggled;

   private string _RoomRefID;
   private int _NumberStayNights;
   private int _NumberAdults;

   private HotelDescPackage _PackageDescription;
   private HotelAvailPackage _PackageRate;
   public decimal TotalCost
   {
      get
      {
         int pkgQty = 1;
         if (PackageQuantity == null || PackageQuantity == 0)
            pkgQty = int.Parse(ddlPackageQuantity.SelectedValue);
         else
            pkgQty = PackageQuantity;

         if (_PackageRate.PriceType == PackagePriceType.PerPersonPerNight)
            return _NumberStayNights * _PackageRate.Price * pkgQty;
         if (_PackageRate.PriceType == PackagePriceType.PerStayPerNight)
            return _NumberStayNights * _PackageRate.Price;

         return pkgQty * _PackageRate.Price;
      }
   }

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

   public int NumberAdults
   {
      get
      {
         return _NumberAdults;
      }

      set
      {
         _NumberAdults = value;
      }

   }

   public int NumberStayNights
   {
      get
      {
         return _NumberStayNights;
      }

      set
      {
         _NumberStayNights = value;
      }

   }


   public HotelDescPackage PackageDescription
   {
      get
      {
         return _PackageDescription;
      }

      set
      {
         _PackageDescription = value;
      }

   }

   public HotelAvailPackage PackageRate
   {
      get
      {
         return _PackageRate;
      }

      set
      {
         _PackageRate = value;
      }

   }

   public int PackageQuantity
   {
      get;
      set;
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

   public bool IsItemRemovable { get; set; }

   protected void Page_Load(object sender, EventArgs e)
   {
      if (IsPostBack & !this.IsParentPreRender())
      {
         if (this.Request.Form.Get(cbPackageSelected.ClientID.Replace('_', '$')) != null) // form uses "name" not "id" property
            _Selected = true;
         else
            _Selected = false;

         if (this.Request.Form.Get(ddlPackageQuantity.ClientID.Replace('_', '$')) != null)
            PackageQuantity = Convert.ToInt32(this.Request.Form.Get(ddlPackageQuantity.ClientID.Replace('_', '$')));
      }

      lblDescription.Attributes.Add("onclick", string.Format("toggleDetail('{0}',this);", panAddOnPackageDescription.ClientID));
      ddlPackageQuantity.Attributes.Add("onchange", string.Format("item_changed(this, '{0}', '{1}');", lblPackagePrice.ClientID, lblTotal.ClientID));

      return;
   }


   public void RenderUserControl()
   {
      for (int i = 0; i < _PackageDescription.Images.Length; i++)
      {
         if (_PackageDescription.Images[i].CategoryCode == HotelImageCategoryCode.Package && _PackageDescription.Images[i].ImageSize == HotelImageSize.FullSize)
         {
            imgPackage.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _PackageDescription.Images[i].ImageURL;
            break;
         }

      }

      lblPackageNameText.Text = _PackageRate.Name;
      lblPackageDescriptionText.Text = _PackageRate.ShortDescription;

      lblPackagePrice.Text = FormatCurrencyString(_PackageRate.Price, _PackageRate.CurrencyCode);

      lblPackagePriceType.Text = (String)GetLocalResourceObject("PackageType" + _PackageRate.PriceType);
      lblPackageQuantity.Text = string.Format("({0})", PackageQuantity);

      ddlPackageQuantity.Items.Clear();

      if (_PackageRate.PriceType == PackagePriceType.PerStay || _PackageRate.PriceType == PackagePriceType.PerStayPerNight)
      {
         int intMaxPerStayQty = 1;
         Int32.TryParse(ConfigurationManager.AppSettings["AddOnPackageSelectorItemControl.MaxPerStayQty"], out intMaxPerStayQty);

         for (int i = 1; i <= intMaxPerStayQty; i++)
         {
            ddlPackageQuantity.Items.Add(new ListItem(i.ToString(), i.ToString()));
         }

         ddlPackageQuantity.SelectedValue = PackageQuantity.ToString();
         lblPackageQuantityUnits.Text = "";
      }

      else
      {
         if (_PackageRate.PriceType == PackagePriceType.PerPerson)
         {
            for (int i = 1; i <= _NumberAdults; i++)
            {
               ddlPackageQuantity.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ddlPackageQuantity.SelectedValue = PackageQuantity.ToString();

            lblPackageQuantityUnits.Text = (String)GetLocalResourceObject("PackageQuantityUnitsAdults");
         }

         else if (_PackageRate.PriceType == PackagePriceType.PerPersonPerNight)
         {
            if (ConfigurationManager.AppSettings["AddOnPackageSelectorItemControl.UseMaxAdults"] == "1")
            {
               ddlPackageQuantity.Items.Add(new ListItem(_NumberAdults.ToString(), _NumberAdults.ToString()));
            }

            else
            {
               for (int i = 1; i <= _NumberAdults; i++)
               {
                  ddlPackageQuantity.Items.Add(new ListItem(i.ToString(), i.ToString()));
               }

               ddlPackageQuantity.SelectedValue = PackageQuantity.ToString();
            }

            lblPackageQuantityUnits.Text = (String)GetLocalResourceObject("PackageQuantityUnitsAdults");
         }

      }

      if (_PackageRate.PriceType == PackagePriceType.PerStayPerNight || _PackageRate.PriceType == PackagePriceType.PerPersonPerNight)
      {
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
         lblPackagePriceNights.Text = "";
      }

      if (_Selected)
         cbPackageSelected.Checked = true;
      else
         cbPackageSelected.Checked = false;

      lblTotal.Text = FormatCurrencyString(TotalCost, _PackageRate.CurrencyCode);

      ApplyCssClassBasedOnSelectionMode();

      if (!IsItemRemovable)
         panButtonRemove.Visible = false;

      return;
   }

   private void ApplyCssClassBasedOnSelectionMode()
   {
      if (Selected)
      {
         panAddOnPackageSelectorItem.CssClass = "mm_addon_info mm_background_info";
         panAddOnPackageSeletorItemContent.CssClass = "mm_roomrate_content mm_text_info";
         //lblPackageQuantity.CssClass = "";
         ddlPackageQuantity.CssClass = "mm_hidden";
         panTotalPrice.CssClass = "mm_roomrate_price";

         string scriptToApplySpecialStyleForTouchDevice = string.Format("applyStyleForTouchDevice(\"{0}\");", "mm_addon_wrapper_button_remove");
         ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StyleForTouchDevice_Addon_Remove", scriptToApplySpecialStyleForTouchDevice, true);

         panButtonSelect.Visible = false;
      }
      else //Edit
      {
         panAddOnPackageSelectorItem.CssClass = "mm_addon_edit mm_background_edit";
         panAddOnPackageSeletorItemContent.CssClass = "mm_roomrate_content mm_text_edit";
         ddlPackageQuantity.CssClass = "";
         //lblPackageQuantity.CssClass = "mm_hidden";
         panTotalPrice.CssClass = "mm_roomrate_price mm_roomrate_price_edit";
         //panTotalPrice.CssClass = "mm_hidden";

         panButtonRemove.Visible = false;
         string scriptToApplySpecialStyleForTouchDevice = string.Format("applyStyleForTouchDevice(\"{0}\");", "mm_addon_wrapper_button_add");
         ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StyleForTouchDevice_Addon_Add", scriptToApplySpecialStyleForTouchDevice, true);
      }
   }

   protected void btnSelect_Click(object sender, EventArgs e)
   {
      if (AddOnToggled != null)
         AddOnToggled(RoomRefID, _PackageDescription.Code, int.Parse(ddlPackageQuantity.SelectedValue), true);
   }

   protected void btnRemove_Click(object sender, EventArgs e)
   {
      if (AddOnToggled != null)
         AddOnToggled(RoomRefID, _PackageDescription.Code, int.Parse(ddlPackageQuantity.SelectedValue), false);
   }

   private bool IsParentPreRender()
   {
      return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
   }

   private string FormatCurrencyString(decimal value, string currencyCode)
   {
      string formatString = value.ToString("F2").EndsWith("00") ? "F0" : ((XnGR_WBS_Page)Page).CurrencyFormat();
      return string.Format("<span class=\"mm_rate_plan_currency_symbol\">{0}</span><span>{1}</span>", WebconfigHelper.GetCurrencyCodeString(currencyCode), value.ToString(formatString));
   }
}
