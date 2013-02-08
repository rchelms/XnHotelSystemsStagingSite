using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XHS.WBSUIBizObjects;
using MamaShelter;

public partial class StayCriteriaSelectorControl_MM : System.Web.UI.UserControl
{
   private const int NumberOfButtonPerRow = 2;
   private const string HotelButtonCssClass = "mm_button_hotel_width";

   public SelectionMode StayCriteriaSelectorMode
   {
      get;
      set;
   }

   private StayCriteriaSelection _StayCriteriaSelection;
   public StayCriteriaSelection StayCriteriaSelection
   {
      get
      {
         //_StayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
         return _StayCriteriaSelection;
      }

      set
      {
         _StayCriteriaSelection = value;
         //Session[Constants.Sessions.StayCriteriaSelection] = value;
      }

   }

   private HotelListItem[] _HotelListItems;
   public HotelListItem[] HotelListItems
   {
      get
      {
         return _HotelListItems;
      }

      set
      {
         _HotelListItems = value;
      }

   }

   public delegate void HotelSelectedEvent(object sender, string hotelCode);
   public event HotelSelectedEvent HotelSelected;

   public event EventHandler EditModeSelected;

   protected override void OnInit(EventArgs e)
   {
      base.OnInit(e);

      panCustomStayCriteriaInfo.Visible = false;
      panCustomStayCriteriaEdit.Visible = false;
   }

   protected void btnEdit_Click(object sender, EventArgs e)
   {
      if (EditModeSelected != null)
         EditModeSelected(this, e);
   }

   protected void Page_Load(object sender, EventArgs e)
   {
      ApplyControlToPage();
   }

   private void ApplyControlToPage()
   {
      if (HotelListItems != null && HotelListItems.Any())
      {
         phdStayCriteriaOptions.Controls.Clear();

         int curCol = 0;
         foreach (HotelListItem hotel in HotelListItems)
         {
            Panel panButtonWrapper = new Panel();
            panButtonWrapper.ID = "panHotel" + hotel.HotelCode;
            panButtonWrapper.CssClass = "mm_background_edit mm_border_edit mm_wrapper_button_hotel " + HotelButtonCssClass;

            Button hotelButton = new Button();
            hotelButton.ID = "btnHotel_" + hotel.HotelCode;
            hotelButton.CssClass = "mm_button mm_button_main_step mm_text_button_hotel";
            hotelButton.Text = (string)GetLocalResourceObject(hotel.HotelCode);
            hotelButton.Click += new EventHandler(quantityButton_Click);
            hotelButton.OnClientClick = "showWaitingPage();";

            panButtonWrapper.Controls.Add(hotelButton);
            phdStayCriteriaOptions.Controls.Add(panButtonWrapper);
         }

         ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString()
         , string.Format("recalculateButtonSize({0},'.{1}');", NumberOfButtonPerRow, HotelButtonCssClass), true);

         if ((StayCriteriaSelectorMode & SelectionMode.NonModifiable) == SelectionMode.NonModifiable)
            panEditButton.Visible = false;
      }

   }

   void quantityButton_Click(object sender, EventArgs e)
   {
      var button = (Button)sender;
      string buttonId = button.ID;
      var hotelCode = buttonId.Substring(buttonId.IndexOf("_") + 1);
      HotelSelected(sender, hotelCode);
   }

   public void RenderUserControl()
   {
      if (StayCriteriaSelection != null && !string.IsNullOrWhiteSpace(StayCriteriaSelection.HotelCode))
      {
         var selectedItem = HotelListItems.FirstOrDefault(item => item.HotelCode.Equals(StayCriteriaSelection.HotelCode, StringComparison.InvariantCultureIgnoreCase));
         if (selectedItem != null)
            lblStayCriteriaInfo.Text = GetLocalResourceObject("lblStayCriteriaInfo.Text") + string.Format(" {0}", selectedItem.HotelName);
      }
      ApplyControlToPage();
      ShowPanelByMode();
   }

   private void ShowPanelByMode()
   {
      if (StayCriteriaSelectorMode == SelectionMode.Edit)
      {
         panCustomStayCriteriaEdit.Visible = true;
         panCustomStayCriteriaInfo.Visible = false;
      }
      else
      {
         panCustomStayCriteriaEdit.Visible = false;
         panCustomStayCriteriaInfo.Visible = true;

         string scriptToApplySpecialStyleForTouchDevice = string.Format("applyStyleForTouchDevice(\"{0}\");", "mm_wrapper_button_edit");
         ScriptManager.RegisterStartupScript(Page, Page.GetType(), "StyleForTouchDevice_StayCriteriaControl", scriptToApplySpecialStyleForTouchDevice, true);
      }
   }
}