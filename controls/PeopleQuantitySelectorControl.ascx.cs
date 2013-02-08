using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MamaShelter;

public partial class PeopleQuantitySelectorControl : System.Web.UI.UserControl
{
    private const string ButtonAdultCssClassName = "mm_wrapper_button_adult_quantity";
    private const string ButtonChildrenCssClassName = "mm_wrapper_button_children_quantity";

    public delegate void PeopleQuantityCompletedHandler(string roomRefID, int quantity);
    public event PeopleQuantityCompletedHandler PeopleQuantityCompleted;

    public delegate void EditModeSelectedHandler(string roomRefID, RoomDetailSelectionStep step);
    public event EditModeSelectedHandler EditModeSelected;

    public int MinQuantity { get; set; }
    public int MaxQuantity { get; set; }
    public SelectionMode Mode { get; set; }
    public RoomDetailSelectionStep DetailStep { get; set; }
    public int NumberOfPeople { get; set; }
    public string RoomRefID { get; set; }

    private string ButtonClassName
    {
        get
        {
            return DetailStep == RoomDetailSelectionStep.SelectAdultQuantity
                       ? ButtonAdultCssClassName
                       : ButtonChildrenCssClassName;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ApplyControlToPage();
    }

    public void RenderUserControls()
    {
        ToogleGUIBasedOnMode();
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString()
            , string.Format("recalculateButtonSize({0},'.{1}');", phdPeopleQuantitySelector.Controls.Count, ButtonClassName), true);
    }

    private void ApplyControlToPage()
    {
        string instructionMessage = GetLocalResourceObject("EditInstruction").ToString();
        string adultForQuestion = GetLocalResourceObject("AdultForQuestion").ToString();
        string adults = GetLocalResourceObject("Adults").ToString();
        string adult = GetLocalResourceObject("Adult").ToString();
        string child = GetLocalResourceObject("Child").ToString();
        string children = GetLocalResourceObject("Children").ToString();

        // Summary info Mode
        string adultOrAdults = NumberOfPeople == 1 ? adult : adults;
        string childOrChildren = NumberOfPeople > 1 ? children : child;

        lblInfoSummary.Text = string.Format("{0} {1} : {2} {3}"
            , GetLocalResourceObject("summaryInfo")
            , RoomRefID
            , NumberOfPeople
            , (DetailStep == RoomDetailSelectionStep.SelectAdultQuantity ? adultOrAdults : childOrChildren));


        // Selection Mode
        lblStepInstruction.Text = string.Format(instructionMessage,
            (DetailStep == RoomDetailSelectionStep.SelectAdultQuantity ? adultForQuestion : children)
            , RoomRefID);


        phdPeopleQuantitySelector.Controls.Clear();
        for (int i = 0;; i++)
        {
            Panel panButtonWrapper = new Panel();
            panButtonWrapper.ID = "panButtonWrapper" + i;
            panButtonWrapper.CssClass = "mm_background_edit mm_border_edit mm_wrapper_button_hotel " + ButtonClassName;

            Button quantityButton = new Button();
            quantityButton.ID = "btnQuantity" + i;
            quantityButton.CssClass = "mm_button mm_button_main_step mm_text_button_hotel";
            quantityButton.Text = (MinQuantity + i).ToString();
            quantityButton.Click += quantityButton_Click;
            quantityButton.OnClientClick = "showWaitingPage();";

            panButtonWrapper.Controls.Add(quantityButton);
            phdPeopleQuantitySelector.Controls.Add(panButtonWrapper);
            if((MinQuantity + i) == MaxQuantity)
                break;
        }

        if ((Mode & SelectionMode.NonModifiable) == SelectionMode.NonModifiable)
            panEditButton.Visible = false;
        
    }

    void quantityButton_Click(object sender, EventArgs e)
    {
        Button bt = (Button)sender;
        if (PeopleQuantityCompleted != null)
            PeopleQuantityCompleted(RoomRefID, int.Parse(bt.Text));
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (EditModeSelected != null)
            EditModeSelected(RoomRefID, DetailStep);
    }

    private void ToogleGUIBasedOnMode()
    {
        if ((Mode & SelectionMode.Edit) == SelectionMode.Edit)
        {
            panInfo.Visible = false;
            panEdit.Visible = true;
        }
        else if ((Mode & SelectionMode.Selected) == SelectionMode.Selected)
        {
            panInfo.Visible = true;
            panEdit.Visible = false;
        }
        else if((Mode & SelectionMode.Hidden) == SelectionMode.Hidden)
        {
            panInfo.Visible = false;
            panEdit.Visible = false;
        }
    }
}