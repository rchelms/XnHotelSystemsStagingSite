using System;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using XHS.WBSUIBizObjects;
using MamaShelter;
using System.Web.Script.Serialization;
using System.Web.UI;

public partial class AvailCalSelectorControl : System.Web.UI.UserControl
{
   private const string SessionIsKeepUserData = "DontRemoveUserData";

   public delegate void AvailCalRequestedEvent(object sender, bool bViewCalendar);
   public event AvailCalRequestedEvent AvailCalRequested;

   public event EventHandler NextMonthRequested;
   public event EventHandler PrevMonthRequested;

   public delegate void AvailCalCompletedEvent(object sender, EventArgs e);
   public event AvailCalCompletedEvent AvailCalCompleted;

   public delegate void StayDateSelectedEvent(
       object sender, DateTime selectedCheckinDate, DateTime selectedCheckoutDate);

   public event StayDateSelectedEvent StayDateSelected;

   private AvailabilityCalendarInfo[] _AvailabilityCalendarInfo; // Assumes all instances are for same calendar start date and number days
   private DateTime[] _SelectedDates;
   private StayCriteriaSelection _StayCriteriaSelection;
   private DateTime _Today;

   private List<AvailCalSelectorItemControl> lAvailCalSelectorItemControls;

   private AvailCalendarSelectorMode _AvailCalendarSelectorMode;
   public AvailCalendarSelectorMode AvailCalendarSelectorMode
   {
      get
      {
         return _AvailCalendarSelectorMode;
      }

      set
      {
         _AvailCalendarSelectorMode = value;
      }

   }

   public SelectionMode _Mode = SelectionMode.Hidden;
   public SelectionMode Mode { get { return _Mode; } set { _Mode = value; } }

   public AvailabilityCalendarInfo[] AvailabilityCalendarInfo
   {
      get
      {
         return _AvailabilityCalendarInfo;
      }

      set
      {
         _AvailabilityCalendarInfo = value;
      }

   }

   public DateTime[] SelectedDates
   {
      get
      {
         return _SelectedDates;
      }

      set
      {
         _SelectedDates = value;
      }

   }

   public StayCriteriaSelection StayCriteriaSelection
   {
      get
      {
         return _StayCriteriaSelection;
      }

      set
      {
         _StayCriteriaSelection = value;
      }

   }

   public DateTime Today
   {
      get
      {
         return _Today;
      }

      set
      {
         _Today = value;
      }

   }

   protected void Page_Load(object sender, EventArgs e)
   {
      ScriptManager.RegisterClientScriptInclude(Page, Page.GetType(), "AvailCalSelectorControlScript", this.ResolveUrl(Constants.ScriptPath.AvailCalSelectorControl));

      if (IsPostBack && !this.IsParentPreRender())
      {
         this.ApplyControlsToPage();
      }
      
      return;
   }

   private bool bRemoveUserData
   {
      get
      {
         if (Session[SessionIsKeepUserData] == null || (bool) Session[SessionIsKeepUserData] == false)
            return true;
         return false;
      }
   }

   protected override void OnPreRender(EventArgs e)
   {
      base.OnPreRender(e);
      ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AvailCalSelectorControl", "reassignInstructionTextResource();", true);
      ScriptManager.RegisterClientScriptBlock(this, GetType(), "AvailCalSelectorControl",
                                              bRemoveUserData ? "InitGlobalVariables();" : "InitCalendar();",
                                              true);
      Session[SessionIsKeepUserData] = false;
   }

   protected void btnNavigatePrev_Click(object sender, EventArgs e)
   {
      Session[SessionIsKeepUserData] = true;
      if (PrevMonthRequested != null)
         PrevMonthRequested(sender, e);
   }

   protected void btnNavigateNext_Click(object sender, EventArgs e)
   {
      Session[SessionIsKeepUserData] = true;
      if (NextMonthRequested != null)
         NextMonthRequested(sender, e);
   }

   public void RenderUserControl()
   {
      this.ApplyControlsToPage();

      if ((Mode & SelectionMode.Selected) == SelectionMode.Selected)
      {
         panAvailCalViewInfo.Visible = true;
         panAvailCalControl.Visible = false;
      }

      else if (Mode == SelectionMode.Edit)
      {
         panAvailCalViewInfo.Visible = false;
         panAvailCalControl.Visible = true;
      }

      if ((Mode & SelectionMode.NonModifiable) == SelectionMode.NonModifiable)
         panAvailCalViewEditButton.Visible = false;

      return;
   }

   protected void lbViewAvailCal_Click(object sender, EventArgs e)
   {
      AvailCalRequested(this, true);
      return;
   }

   protected void lbCloseAvailCal_Click(object sender, EventArgs e)
   {
      AvailCalRequested(this, false);
      return;
   }

   protected void btnViewRates_Click(object sender, EventArgs e)
   {
      //this.GetDateSelections();
      //if (AvailCalCompleted != null)
      //    AvailCalCompleted(this, new EventArgs());

      string[] checkinDateString = hdfSelectedCheckinDate.Value.Split('-');
      string[] checkoutDateString = hdfSelectedCheckoutDate.Value.Split('-');

      DateTime selectedCheckinDate = new DateTime(int.Parse(checkinDateString[0]), int.Parse(checkinDateString[1]), int.Parse(checkinDateString[2]))
          , selectedCheckoutDate = new DateTime(int.Parse(checkoutDateString[0]), int.Parse(checkoutDateString[1]), int.Parse(checkoutDateString[2]));

      StayDate_Selected(sender, selectedCheckinDate, selectedCheckoutDate);
      return;
   }

   private void StayDate_Selected(object sender, DateTime selectedCheckinDate, DateTime selectedCheckoutDate)
   {
      if (StayDateSelected != null)
         StayDateSelected(sender, selectedCheckinDate, selectedCheckoutDate);
   }

   private void ApplyControlsToPage()
   {
      if ((Mode & SelectionMode.Selected) == SelectionMode.Selected && this.IsParentPreRender())
      {
         lblCheckinInfo.Text = string.Concat(GetLocalResourceObject("lblCheckinInfo.Text"), " ", StayCriteriaSelection.ArrivalDate.ToLongDateString());
         lblCheckoutInfo.Text = string.Concat(GetLocalResourceObject("lblCheckoutInfo.Text"), " ", StayCriteriaSelection.DepartureDate.ToLongDateString());
         return;
      }

      if (_AvailabilityCalendarInfo == null || _AvailabilityCalendarInfo.Length == 0)
         return;

      if (_AvailabilityCalendarInfo[0].AvailabilityCalendar.NumDays <= 0)
         return;

      int intCalendarNumDays = _AvailabilityCalendarInfo[0].AvailabilityCalendar.NumDays;
      DateTime dtCalendarStartDate = _AvailabilityCalendarInfo[0].AvailabilityCalendar.StartDate.Date;
      DateTime dtCalendarEndDate = _AvailabilityCalendarInfo[0].AvailabilityCalendar.StartDate.AddDays(intCalendarNumDays - 1).Date;

      StringBuilder sb = new StringBuilder();
      sb.Append(dtCalendarStartDate.ToString("MMMM yyyy"));
      lblAvailCalMonth.Text = sb.ToString();

      int intStartDayOfWeek = (int)dtCalendarStartDate.DayOfWeek;

      if (intStartDayOfWeek == 0)
         intStartDayOfWeek = 7; // Mon = 1; Sun = 7

      string strAvailCalSelectorItemControlPath = ConfigurationManager.AppSettings["AvailCalSelectorItemControl.ascx"];
      lAvailCalSelectorItemControls = new List<AvailCalSelectorItemControl>();

      for (int i = 1; i <= 42; i++)
      {
         if (i >= intStartDayOfWeek)
         {
            int intDayID = i - intStartDayOfWeek + 1;

            if (intDayID > intCalendarNumDays)
               break;

            AvailCalSelectorItemControl ucAvailCalSelectorItem = (AvailCalSelectorItemControl)LoadControl(strAvailCalSelectorItemControlPath);
            lAvailCalSelectorItemControls.Add(ucAvailCalSelectorItem);

            ucAvailCalSelectorItem.ID = "AvailCalDay" + intDayID;
            ucAvailCalSelectorItem.DayID = intDayID;
            ucAvailCalSelectorItem.AvailabilityCalendarInfo = _AvailabilityCalendarInfo;
            ucAvailCalSelectorItem.StayCriteriaSelection = _StayCriteriaSelection;
            ucAvailCalSelectorItem.Today = _Today;
            ucAvailCalSelectorItem.Selected = false;

            for (int j = 0; j < _SelectedDates.Length; j++)
            {
               if (dtCalendarStartDate.AddDays(intDayID - 1).Date == _SelectedDates[j].Date)
               {
                  ucAvailCalSelectorItem.Selected = true;
                  break;
               }

            }

            TableCell td = (TableCell)panAvailCal.FindControl("tdDay" + i);

            td.Controls.Clear();
            td.Controls.Add(ucAvailCalSelectorItem);

            ucAvailCalSelectorItem.RenderUserControl();
         }
      }

      if (dtCalendarStartDate.Month == DateTime.Now.Month)
         btnNavigatePrev.Enabled = false;

      return;
   }

   private void GetDateSelections()
   {
      if (_AvailabilityCalendarInfo == null || _AvailabilityCalendarInfo.Length == 0 || lAvailCalSelectorItemControls == null || lAvailCalSelectorItemControls.Count == 0)
      {
         _SelectedDates = new DateTime[0];
         return;
      }

      DateTime dtCalendarStartDate = _AvailabilityCalendarInfo[0].AvailabilityCalendar.StartDate.Date;

      List<DateTime> lSelectedsDates = new List<DateTime>();

      for (int i = 0; i < lAvailCalSelectorItemControls.Count; i++)
      {
         if (lAvailCalSelectorItemControls[i].Selected)
            lSelectedsDates.Add(dtCalendarStartDate.AddDays(lAvailCalSelectorItemControls[i].DayID - 1).Date);
      }

      _SelectedDates = lSelectedsDates.ToArray();
      return;
   }

   private bool IsParentPreRender()
   {
      return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
   }
}
