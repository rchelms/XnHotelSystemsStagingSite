using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using MamaShelter;
using XHS.WBSUIBizObjects;

public partial class RoomDetailSelectorControl : System.Web.UI.UserControl
{
    public delegate void RoomRateCompletedHandler(string roomRefID);
    public event RoomRateCompletedHandler RoomRateCompleted;

    public event PeopleQuantitySelectorControl.PeopleQuantityCompletedHandler AdultQuantityCompleted;

    public event PeopleQuantitySelectorControl.PeopleQuantityCompletedHandler ChildrenQuantityCompleted;
    
    public event RatePlanSelectorItemControl.ShowRoomPhotoEventHandler ShowRoomPhotoSelected;

    public event RatePlanSelectorItemControl.RoomRateSelectedHandler RatePlanSelected;

    public event AddOnPackageSelectorItemControl.ToggleAddOnPackageEventHandler AddOnToggled;

    public delegate void EditModeSelectedHandler(string roomRefID, RoomDetailSelectionStep step);
    public event EditModeSelectedHandler EditModeSelected;

    public bool ShowTempTotal { get; set; }
    public double TempTotal { get; set; }
    public SelectionMode Mode { get; set; }
    public string RoomRefID { get; set; }
    public RoomDetailSelectionStep Step { get; set; }
    
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void Clear()
    {
        phAdultQuantityControl.Controls.Clear();
        phChildrenQuantityControl.Controls.Clear();
        phRoomTypeSelectorControl.Controls.Clear();
        phRoomExtraSelectorControl.Controls.Clear();
        phSelectedRoomExtras.Controls.Clear();
        phSelectedRoomType.Controls.Clear();
        //phTempTotalCostControl.Controls.Clear();
    }

    public void AddAdultQuantitySelectorControl(PeopleQuantitySelectorControl control)
    {
        phAdultQuantityControl.Controls.Add(control);
        control.PeopleQuantityCompleted += PeopleQuantitySelectorControl_AdultQuantityCompleted;
        control.EditModeSelected += control_EditModeSelected;
    }

    public void AddChildrenQuantitySelectorContorl(PeopleQuantitySelectorControl control)
    {
        phChildrenQuantityControl.Controls.Add(control);
        control.PeopleQuantityCompleted += PeopleQuantitySelectorControl_ChildrenQuantityCompleted;
        control.EditModeSelected += control_EditModeSelected;
    }

    public void AddSelectedRoomRate(RatePlanSelectorItemControl control)
    {
        phSelectedRoomType.Controls.Add(control);
        control.EditModeSelected += control_EditModeSelected;
        control.ShowRoomPhotoSelected += new RatePlanSelectorItemControl.ShowRoomPhotoEventHandler(control_ShowRoomPhotoSelected);
    }

    public void AddRatePlanSelectorItem(RatePlanSelectorItemControl control)
    {
        phRoomTypeSelectorControl.Controls.Add(control);
        control.RoomRateSelected += RoomTypeControl_RoomRateSelected;
        control.EditModeSelected += control_EditModeSelected;
        control.ShowRoomPhotoSelected += new RatePlanSelectorItemControl.ShowRoomPhotoEventHandler(control_ShowRoomPhotoSelected);
    }

    public void AddTempTotalCostControl(TotalCostControl control)
    {
        phTempTotalCostControl.Controls.Add(control);
    }

    public void AddRoomExtraItemControl(AddOnPackageSelectorItemControl control)
    {
        control.AddOnToggled += control_AddOnSelected;
        phRoomExtraSelectorControl.Controls.Add(control);
    }

    public void AddSelectedRoomExtraItemControl(AddOnPackageSelectorItemControl control)
    {
        control.AddOnToggled += control_AddOnSelected;
        phSelectedRoomExtras.Controls.Add(control);
    }

    #region Control Events
    void PeopleQuantitySelectorControl_AdultQuantityCompleted(string roomRefID, int quantity)
    {
        if (AdultQuantityCompleted != null)
            AdultQuantityCompleted(RoomRefID, quantity);
    }

    void PeopleQuantitySelectorControl_ChildrenQuantityCompleted(string roomRefID, int quantity)
    {
        if (ChildrenQuantityCompleted != null)
            ChildrenQuantityCompleted(RoomRefID, quantity);
    }

    void control_ShowRoomPhotoSelected(string roomTypeCode)
    {
        if (ShowRoomPhotoSelected != null)
            ShowRoomPhotoSelected(roomTypeCode);
    }

    void control_EditModeSelected(string roomRefID, RoomDetailSelectionStep step)
    {
        if (EditModeSelected != null)
            EditModeSelected(roomRefID, step);
    }

    void RoomTypeControl_RoomRateSelected(string roomRefID, string roomTypeCode, string roomRateCode, string promotionCode)
    {
        if (RatePlanSelected != null)
            RatePlanSelected(roomRefID, roomTypeCode, roomRateCode, promotionCode);
    }

    void control_AddOnSelected(string roomRefID, string packageCode, int quantity, bool isSelected)
    {
        if (AddOnToggled != null)
            AddOnToggled(roomRefID, packageCode, quantity, isSelected);
    }

    protected void btnDone_Click(object sender, EventArgs e)
    {
        if (RoomRateCompleted != null)
            RoomRateCompleted(RoomRefID);
    }

    #endregion

    public void RenderUserControls()
    {
        ApplyControlToPage();

        //Render people quantity control
        for (int i = 0; i < phAdultQuantityControl.Controls.Count; i++)
            ((PeopleQuantitySelectorControl)phAdultQuantityControl.Controls[i]).RenderUserControls();

        for (int i = 0; i < phChildrenQuantityControl.Controls.Count; i++)
            ((PeopleQuantitySelectorControl)phChildrenQuantityControl.Controls[i]).RenderUserControls();

        //Render selectec room rate controls
        for (int i = 0; i < phSelectedRoomType.Controls.Count; i++)
            ((RatePlanSelectorItemControl)phSelectedRoomType.Controls[i]).RenderUserControl();

        //Render room rate controls
        for (int i = 0; i < phRoomTypeSelectorControl.Controls.Count; i++)
            ((RatePlanSelectorItemControl)phRoomTypeSelectorControl.Controls[i]).RenderUserControl();

        //Render selected extra controls
        for (int i = 0; i < phSelectedRoomExtras.Controls.Count; i++)
        {
            ((AddOnPackageSelectorItemControl)phSelectedRoomExtras.Controls[i]).RenderUserControl();
            btnDone.Text = (string)GetLocalResourceObject("Done");
        }

        //Render available extra controls
        for (int i = 0; i < phRoomExtraSelectorControl.Controls.Count; i++)
            ((AddOnPackageSelectorItemControl)phRoomExtraSelectorControl.Controls[i]).RenderUserControl();

        //Render Temp Total Controls
        for (int i = 0; i < phTempTotalCostControl.Controls.Count; i++)
            ((TotalCostControl)phTempTotalCostControl.Controls[i]).RenderUserControl();
    }

    private void ApplyControlToPage()
    {
        if (Mode == SelectionMode.Edit)
        {
            if (Step == RoomDetailSelectionStep.SelectAdultQuantity)
            {
                panChildrenQuantitySelectorControl.Visible = false;
                panRoomTypeEditInstruction.Visible = false;
                panRoomType.Visible = false;
                panRoomExtraInfo.Visible = false;
                panRoomExtraInstruction.Visible = false;
                panRoomExtra.Visible = false;
            }
            else if(Step == RoomDetailSelectionStep.SelectChildrenQuantity)
            {
                panRoomTypeEditInstruction.Visible = false;
                panRoomType.Visible = false;
                panRoomExtraInfo.Visible = false;
                panRoomExtraInstruction.Visible = false;
                panRoomExtra.Visible = false;
            }
            else if (Step == RoomDetailSelectionStep.SelectRoomType)
            {
                panRoomTypeEditInstruction.Visible = true;
                panRoomType.Visible = true;
                panSelectedRoomRate.Visible = false;
                if(phRoomTypeSelectorControl.Controls.Count > 0)
                    lblRoomTypeEditInstruction.Text = string.Format("{0} {1}", GetLocalResourceObject("lblRoomTypeEditInstruction"), RoomRefID);
                else
                    lblRoomTypeEditInstruction.Text = (string)GetLocalResourceObject("NotAvailable");

                panRoomExtraInfo.Visible = false;
                panRoomExtraInstruction.Visible = false;
                panRoomExtra.Visible = false;
            }
            else
            {
                panAdultQuantitySelectorControl.Visible = true;
                panChildrenQuantitySelectorControl.Visible = true;
                panRoomType.Visible = false;
                panSelectedRoomRate.Visible = true;
                panRoomExtraInstruction.Visible = true;
                panRoomExtra.Visible = true;
                panRoomExtraInfo.Visible = true;
                lblRoomExtraInstruction.Text = string.Format("{0} {1}", GetLocalResourceObject("lblRoomExtraInstruction"), RoomRefID);

                panRoomTypeEditInstruction.Visible = false;

            }
        }
        else if (Mode == SelectionMode.Selected)
        {
            panAdultQuantitySelectorControl.Visible = true;
            panChildrenQuantitySelectorControl.Visible = true;
            panRoomType.Visible = false;
            panSelectedRoomRate.Visible = true;
            panRoomExtraInfo.Visible = true;
            panRoomExtra.Visible = true;

            panRoomTypeEditInstruction.Visible = false;
            panRoomExtraInstruction.Visible = false;
        }
        else
        {
            panAdultQuantitySelectorControl.Visible = false;
            panChildrenQuantitySelectorControl.Visible = false;
            panSelectedRoomRate.Visible = false;
            panRoomTypeEditInstruction.Visible = false;
            panRoomType.Visible = false;
            panRoomExtraInfo.Visible = false;
            panRoomExtraInstruction.Visible = false;
            panRoomExtra.Visible = false;
        }
    }

    public decimal TotalCost()
    {
        decimal totalRoomCost = 0;
        for (int i = 0; i < phSelectedRoomType.Controls.Count; i++)
            totalRoomCost += ((RatePlanSelectorItemControl)phSelectedRoomType.Controls[i]).TotalRoomRate;
        for (int i = 0; i < phSelectedRoomExtras.Controls.Count; i++)
            totalRoomCost += ((AddOnPackageSelectorItemControl)phSelectedRoomExtras.Controls[i]).TotalCost;

        return totalRoomCost;
    }
}