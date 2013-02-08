using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MamaShelter;
using XHS.WBSUIBizObjects;

public partial class RoomQuantitySelectorControl : System.Web.UI.UserControl
{
    public delegate void RoomQuantityCompletedEvent(object sender, int numberOfRoom, string promoCode);
    public event RoomQuantityCompletedEvent RoomQuantityCompleted;

    public event EventHandler EditModeSelected;

    private const string ButtonQuantityClassName = "mm_wrapper_button_room_quantity";

    public SelectionMode Mode { get; set; }
    public StayCriteriaSelection StayCriteriaSelection { get; set; }
    public int MaxNumberOfRoomAvailable { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ApplyControlToPage();
    }

    public void RenderUserControl()
    {
        string roomUnit = StayCriteriaSelection.RoomOccupantSelections.Length > 1
                              ? (string)GetLocalResourceObject("summaryInfoRoomsUnit")
                              : (string)GetLocalResourceObject("summaryInfoRoomUnit");
        lblInfoSummary.Text = string.Format("{0} {1} {2}", GetLocalResourceObject("summaryInfo"), StayCriteriaSelection.RoomOccupantSelections.Length, roomUnit);

        if (StayCriteriaSelection != null && !string.IsNullOrWhiteSpace(StayCriteriaSelection.PromotionCode))
        {
            lblPromoCodeInfo.Text = string.Format("{0} {1}", (string)GetLocalResourceObject("lblPromotionCodeInfo"), StayCriteriaSelection.PromotionCode);
            panPromotionInfo.Visible = true;
        }
        else
        {
            panPromotionInfo.Visible = false;
        }

        ToogleGUIBasedOnMode();

        if ((Mode & SelectionMode.NonModifiable) == SelectionMode.NonModifiable)
            panEditButton.Visible = false;
        
    }

    private void ApplyControlToPage()
    {
        phdRoomQuantitySelector.Controls.Clear();
        
        if(StayCriteriaSelection != null && !string.IsNullOrWhiteSpace(StayCriteriaSelection.PromotionCode))
            txtPromoCode.Text = StayCriteriaSelection.PromotionCode;

        for (int i = 0; i < MaxNumberOfRoomAvailable; i++)
        {
            Panel panButtonWrapper = new Panel();
            panButtonWrapper.ID = "panButtonWrapper" + i.ToString();
            panButtonWrapper.CssClass = "mm_background_edit mm_border_edit mm_wrapper_button_hotel " + ButtonQuantityClassName;

            Button quantityButton = new Button();
            quantityButton.ID = "btnQuantity" + i.ToString();
            quantityButton.CssClass = "mm_button mm_button_main_step mm_text_button_hotel";
            quantityButton.Text = (i + 1).ToString();
            quantityButton.Click += quantityButton_Click;
            quantityButton.OnClientClick = "showWaitingPage();";

            panButtonWrapper.Controls.Add(quantityButton);
            phdRoomQuantitySelector.Controls.Add(panButtonWrapper);
        }
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString()
            , string.Format("recalculateButtonSize({0},'.{1}');", MaxNumberOfRoomAvailable, ButtonQuantityClassName), true);
    }

    void quantityButton_Click(object sender, EventArgs e)
    {
        Button bt = (Button)sender;
        if (RoomQuantityCompleted != null)
            RoomQuantityCompleted(sender, int.Parse(bt.Text), txtPromoCode.Text);
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (EditModeSelected != null)
            EditModeSelected(sender, e);
    }

    private void ToogleGUIBasedOnMode()
    {
        if (Mode == SelectionMode.Edit)
        {
            panInfo.Visible = false;
            panEdit.Visible = true;
        }
        else
        {
            panInfo.Visible = true;
            panEdit.Visible = false;
        }

        if ((Mode & SelectionMode.NonModifiable) == SelectionMode.NonModifiable)
            btnEdit.Visible = false;
    }
}