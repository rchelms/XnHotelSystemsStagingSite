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

public partial class StayCriteriaSelectorControl : System.Web.UI.UserControl
{
    public delegate void StayCriteriaCompletedEvent(object sender, EventArgs e);
    public event StayCriteriaCompletedEvent StayCriteriaCompleted;

    public delegate void SelectNewCountryEvent(object sender, EventArgs e);
    public event SelectNewCountryEvent SelectNewCountry;

    public delegate void SelectNewAreaEvent(object sender, EventArgs e);
    public event SelectNewAreaEvent SelectNewArea;

    public delegate void SelectNewHotelEvent(object sender, EventArgs e);
    public event SelectNewHotelEvent SelectNewHotel;

    public string CalZaptecTheme;

    private bool _IsCustomUIMode;
    private StayCriteriaSelectorType _StayCriteriaSelectorType;
    private StayCriteriaSelectorMode _StayCriteriaSelectorMode;
    private CountryListItem[] _CountryListItems;
    private AreaListItem[] _AreaListItems;
    private HotelListItem[] _HotelListItems;
    private StayCriteriaSelection _StayCriteriaSelection;
    private HotelDescriptiveInfo _HotelDescriptiveInfo;

    public bool IsCustomUIMode
    {
        get
        {
            return _IsCustomUIMode;
        }
        set
        {
            _IsCustomUIMode = value;
        }
    }
    public StayCriteriaSelectorType StayCriteriaSelectorType
    {
        get
        {
            return _StayCriteriaSelectorType;
        }

        set
        {
            _StayCriteriaSelectorType = value;
        }

    }

    public StayCriteriaSelectorMode StayCriteriaSelectorMode
    {
        get
        {
            return _StayCriteriaSelectorMode;
        }

        set
        {
            _StayCriteriaSelectorMode = value;
        }

    }

    public CountryListItem[] CountryListItems
    {
        get
        {
            return _CountryListItems;
        }

        set
        {
            _CountryListItems = value;
        }

    }

    public AreaListItem[] AreaListItems
    {
        get
        {
            return _AreaListItems;
        }

        set
        {
            _AreaListItems = value;
        }

    }

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

    public HotelDescriptiveInfo HotelDescriptiveInfo
    {
        get
        {
            return _HotelDescriptiveInfo;
        }

        set
        {
            _HotelDescriptiveInfo = value;
        }

    }

    public void Clear()
    {
        _StayCriteriaSelectorType = StayCriteriaSelectorType.HotelList;
        _StayCriteriaSelectorMode = StayCriteriaSelectorMode.New;

        _StayCriteriaSelection = new StayCriteriaSelection();
        _StayCriteriaSelection.RoomOccupantSelections = new RoomOccupantSelection[0];

        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            panStayCriteriaInfo.Visible = false;
            panStayCriteriaEdit.Visible = false;
        }

        if (IsPostBack && !this.IsParentPreRender() && !_IsCustomUIMode)
        {
            this.ApplyControlsToPage();
        }

        CalZaptecTheme = ConfigurationManager.AppSettings["CalZaptecThemes"];
        xsZTheme.href = CalZaptecTheme;
        xsZLang.src = GetLocalResourceObject("hdCalLangPath.Value").ToString();
        return;
    }

    public void RenderUserControl()
    {
        StringBuilder sb;

        this.ApplyControlsToPage();

        calDateHiddenFieldArr.Value = _StayCriteriaSelection.ArrivalDate.ToString("yyyy-MM-dd");
        calDateHiddenFieldDep.Value = _StayCriteriaSelection.DepartureDate.ToString("yyyy-MM-dd");

        ddlDayArr.SelectedIndex = _StayCriteriaSelection.ArrivalDate.Day - 1;
        ddlMonthArr.SelectedIndex = _StayCriteriaSelection.ArrivalDate.Month - 1;

        for (int i = 0; i < ddlYearArr.Items.Count; i++)
        {
            if (ddlYearArr.Items[i].Value == _StayCriteriaSelection.ArrivalDate.Year.ToString())
                ddlYearArr.Items[i].Selected = true;
        }

        ddlDayDep.SelectedIndex = _StayCriteriaSelection.DepartureDate.Day - 1;
        ddlMonthDep.SelectedIndex = _StayCriteriaSelection.DepartureDate.Month - 1;

        for (int i = 0; i < ddlYearDep.Items.Count; i++)
        {
            if (ddlYearDep.Items[i].Value == _StayCriteriaSelection.DepartureDate.Year.ToString())
                ddlYearDep.Items[i].Selected = true;
        }


        if (ConfigurationManager.AppSettings["StayCriteriaSelectorControl.UseStayNights"] == "1")
        {
            ddlStayNights.SelectedIndex = ((TimeSpan)_StayCriteriaSelection.DepartureDate.Subtract(_StayCriteriaSelection.ArrivalDate)).Days - 1;

            panDepDate.Visible = false;
            panStayNights.Visible = true;
        }

        else
        {
            panDepDate.Visible = true;
            panStayNights.Visible = false;
        }

        tbPromotionCode.Text = _StayCriteriaSelection.PromotionCode;

        ddlRoom.SelectedIndex = _StayCriteriaSelection.RoomOccupantSelections.Length - 1;

        int intRoom;
        int intMaxChildren = Convert.ToInt32(ConfigurationManager.AppSettings["StayCriteriaSelectorControl.MaxChildren"]);

        for (int i = 0; i < _StayCriteriaSelection.RoomOccupantSelections.Length; i++)
        {
            intRoom = i + 1;

            Panel panRoom = (Panel)this.FindControl("panRoom" + intRoom.ToString());
            panRoom.Visible = true;

            DropDownList ddlAdults = ((DropDownList)this.FindControl("ddlAdults" + intRoom.ToString()));
            ddlAdults.SelectedIndex = _StayCriteriaSelection.RoomOccupantSelections[i].NumberAdults;

            DropDownList ddlChildren = (DropDownList)this.FindControl("ddlChildren" + intRoom.ToString());
            ddlChildren.SelectedIndex = _StayCriteriaSelection.RoomOccupantSelections[i].NumberChildren;
        }

        if (_HotelDescriptiveInfo != null)
        {
            for (int i = 0; i < _HotelDescriptiveInfo.Images.Length; i++)
            {
                if (_HotelDescriptiveInfo.Images[i].CategoryCode == HotelImageCategoryCode.ExteriorView && _HotelDescriptiveInfo.Images[i].ImageSize == HotelImageSize.Thumbnail)
                {
                    imgHotel.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _HotelDescriptiveInfo.Images[i].ImageURL;
                    break;
                }

            }

            lblHotelNameText.Text = _HotelDescriptiveInfo.HotelName;

            sb = new StringBuilder();

            if (_HotelDescriptiveInfo.Address1.Trim() != "")
            {
                sb.Append(_HotelDescriptiveInfo.Address1);
            }

            if (_HotelDescriptiveInfo.Address2.Trim() != "")
            {
                if (sb.ToString().Trim() != "")
                {
                    sb.Append(", ");
                }

                sb.Append(_HotelDescriptiveInfo.Address2);
            }

            if (_HotelDescriptiveInfo.City.Trim() != "")
            {
                if (sb.ToString().Trim() != "")
                {
                    sb.Append(", ");
                }

                sb.Append(_HotelDescriptiveInfo.City);
            }

            if (_HotelDescriptiveInfo.PostalCode.Trim() != "" || _HotelDescriptiveInfo.Country.Trim() != "")
            {
                if (sb.ToString().Trim() != "")
                {
                    sb.Append(", ");
                }

                sb.Append(_HotelDescriptiveInfo.PostalCode.Trim() + " " + _HotelDescriptiveInfo.Country.Trim());
            }

            lblHotelAddressInfo.Text = sb.ToString();
            lblTelephoneNumber.Text = _HotelDescriptiveInfo.Phone;
            lblFaxNumber.Text = _HotelDescriptiveInfo.Fax;
            lblEmailAddress.Text = _HotelDescriptiveInfo.Email;
            lblCompanyRegNumberInfo.Text = _HotelDescriptiveInfo.CompanyRegNumber;

            if (_HotelDescriptiveInfo.CompanyRegNumber != null && _HotelDescriptiveInfo.CompanyRegNumber != "")
                panCompanyRegNumber.Visible = true;
            else
                panCompanyRegNumber.Visible = false;

            phHotelDescription.Controls.Clear();

            string strHotelDescriptionControlPath = ConfigurationManager.AppSettings["HotelDescriptionControl.ascx"];
            HotelDescriptionControl ucHotelDescriptionControl = (HotelDescriptionControl)LoadControl(strHotelDescriptionControlPath);
            phHotelDescription.Controls.Add(ucHotelDescriptionControl);

            ucHotelDescriptionControl.HotelDescriptiveInfo = _HotelDescriptiveInfo;
            ucHotelDescriptionControl.HotelDescriptionOnly = false;
            ucHotelDescriptionControl.RenderUserControl();

            phHotelRating.Controls.Clear();

            string strHotelRatingControlPath = ConfigurationManager.AppSettings["HotelRatingControl.ascx"];
            HotelRatingControl ucHotelRatingControl = (HotelRatingControl)LoadControl(strHotelRatingControlPath);
            phHotelRating.Controls.Add(ucHotelRatingControl);

            ucHotelRatingControl.RatingProvider = _HotelDescriptiveInfo.RatingProvider;
            ucHotelRatingControl.Rating = _HotelDescriptiveInfo.Rating;
            ucHotelRatingControl.RatingSymbol = _HotelDescriptiveInfo.RatingSymbol;
            ucHotelRatingControl.RenderUserControl();
        }

        lblAreaInfoText.Text = this.GetAreaName(_StayCriteriaSelection.AreaID);
        lblHotelInfoText.Text = this.GetHotelName(_StayCriteriaSelection.HotelCode);
        lblArrDateInfoText.Text = _StayCriteriaSelection.ArrivalDate.ToLongDateString();
        lblDepDateInfoText.Text = _StayCriteriaSelection.DepartureDate.ToLongDateString();
        lblPromotionCodeInfoText.Text = _StayCriteriaSelection.PromotionCode;

        for (int i = 0; i < _StayCriteriaSelection.RoomOccupantSelections.Length; i++)
        {
            intRoom = i + 1;

            Panel panRoomInfo = (Panel)this.FindControl("panRoomInfo" + intRoom.ToString());
            panRoomInfo.Visible = true;

            Label lblRoomInfoText = (Label)this.FindControl("lblRoomInfoText" + intRoom.ToString());

            sb = new StringBuilder();

            sb.Append(_StayCriteriaSelection.RoomOccupantSelections[i].NumberAdults.ToString());
            sb.Append(" ");
            sb.Append((String)GetLocalResourceObject("AdultsInfo"));

            if (_StayCriteriaSelection.RoomOccupantSelections[i].NumberChildren != 0)
            {
                sb.Append(", ");
                sb.Append(_StayCriteriaSelection.RoomOccupantSelections[i].NumberChildren.ToString());
                sb.Append(" ");
                sb.Append((String)GetLocalResourceObject("ChildrenInfo"));
            }

            lblRoomInfoText.Text = sb.ToString();
        }

        for (int i = _StayCriteriaSelection.RoomOccupantSelections.Length; i < 8; i++)
        {
            intRoom = i + 1;
            Panel panRoomInfo = (Panel)this.FindControl("panRoomInfo" + intRoom.ToString());
            panRoomInfo.Visible = false;
        }

        panStayCriteriaInfo.Visible = false;

        panCountryList.Visible = false;
        panAreaList.Visible = false;
        panHotelList.Visible = false;

        panAreaInfo.Visible = false;

        panFindRates.Visible = false;
        panFindHotel.Visible = false;
        panFindAnotherHotel.Visible = false;

        panCloseEditStayCriteria.Visible = false;

        if (!IsCustomUIMode)
        {
            if (_StayCriteriaSelectorType == StayCriteriaSelectorType.HotelList)
            {
                if (_StayCriteriaSelectorMode == StayCriteriaSelectorMode.New)
                {
                    panStayCriteriaEdit.Visible = true;
                    panHotelList.Visible = true;
                    panFindRates.Visible = true;
                }

                else if (_StayCriteriaSelectorMode == StayCriteriaSelectorMode.Change)
                {
                    if (ConfigurationManager.AppSettings["StayCriteriaSelectorControl.ChangeModeOpen"] != "1")
                    {
                        panStayCriteriaInfo.Visible = true;
                        panCloseEditStayCriteria.Visible = true;
                        panFindRates.Visible = true;

                        if (_HotelListItems.Length > 1)
                            panFindAnotherHotel.Visible = true;
                    }

                    else
                    {
                        panStayCriteriaEdit.Visible = true;
                        panFindRates.Visible = true;

                        if (_HotelListItems.Length > 1)
                            panFindAnotherHotel.Visible = true;
                    }

                }

            }

            else if (_StayCriteriaSelectorType == StayCriteriaSelectorType.HotelSearch)
            {
                if (_StayCriteriaSelectorMode == StayCriteriaSelectorMode.New)
                {
                    panStayCriteriaEdit.Visible = true;
                    panCountryList.Visible = true;
                    panAreaList.Visible = true;
                    panHotelList.Visible = true;
                    panFindHotel.Visible = true;
                }

                else if (_StayCriteriaSelectorMode == StayCriteriaSelectorMode.Change)
                {
                    if (ConfigurationManager.AppSettings["StayCriteriaSelectorControl.ChangeModeOpen"] != "1")
                    {
                        panStayCriteriaInfo.Visible = true;
                        panCloseEditStayCriteria.Visible = true;
                        panAreaInfo.Visible = true;
                        panFindRates.Visible = true;
                        panFindAnotherHotel.Visible = true;
                    }

                    else
                    {
                        panStayCriteriaInfo.Visible = true;
                        panStayCriteriaEdit.Visible = true;
                        panAreaInfo.Visible = true;
                        panFindRates.Visible = true;
                        panFindAnotherHotel.Visible = true;
                    }

                }

                else if (_StayCriteriaSelectorMode == StayCriteriaSelectorMode.Search)
                {
                    panStayCriteriaEdit.Visible = true;
                    panFindRates.Visible = true;
                }

            }
            panCustomStayCriteriaInfo.Visible = false;
            panCustomStayCriteriaEdit.Visible = false;
        }
        else
        {
            panStayCriteriaInfo.Visible = false;
            panStayCriteriaEdit.Visible = false;

            if (_StayCriteriaSelectorMode == XHS.WBSUIBizObjects.StayCriteriaSelectorMode.New)
            {
                panCustomStayCriteriaEdit.Visible = true;
                panCustomStayCriteriaInfo.Visible = false;
            }
            else
            {
                panCustomStayCriteriaEdit.Visible = true;
                panCustomStayCriteriaInfo.Visible = false;
            }
        }

        return;
    }

    protected void ddlCountryList_Change(object sender, EventArgs e)
    {
        this.UpdateStayCriteriaSelection();

        _StayCriteriaSelection.AreaID = "";
        _StayCriteriaSelection.HotelCode = "";

        SelectNewCountry(this, new EventArgs());
        return;
    }

    protected void ddlAreaList_Change(object sender, EventArgs e)
    {
        this.UpdateStayCriteriaSelection();

        _StayCriteriaSelection.HotelCode = "";

        SelectNewArea(this, new EventArgs());
        return;
    }

    protected void btnStayCriteriaCompleted_Click(object sender, EventArgs e)
    {
        this.UpdateStayCriteriaSelection();

        StayCriteriaCompleted(this, new EventArgs());
        return;
    }

    protected void btnFindAnotherHotel_Click(object sender, EventArgs e)
    {
        SelectNewHotel(this, new EventArgs());
        return;
    }

    protected void btnEditStayCriteria_Click(object sender, EventArgs e)
    {
        panStayCriteriaEdit.Visible = true;
        return;
    }

    protected void btnCloseEditStayCriteria_Click(object sender, EventArgs e)
    {
        panStayCriteriaEdit.Visible = false;
        return;
    }

    private void ApplyControlsToPage()
    {
        ddlCountryList.Items.Clear();
        ddlAreaList.Items.Clear();
        ddlHotelList.Items.Clear();

        ddlYearArr.Items.Clear();
        ddlYearDep.Items.Clear();

        ddlStayNights.Items.Clear();

        ddlRoom.Items.Clear();

        ddlAdults1.Items.Clear();
        ddlAdults2.Items.Clear();
        ddlAdults3.Items.Clear();
        ddlAdults4.Items.Clear();
        ddlAdults5.Items.Clear();
        ddlAdults6.Items.Clear();
        ddlAdults7.Items.Clear();
        ddlAdults8.Items.Clear();

        ddlChildren1.Items.Clear();
        ddlChildren2.Items.Clear();
        ddlChildren3.Items.Clear();
        ddlChildren4.Items.Clear();
        ddlChildren5.Items.Clear();
        ddlChildren6.Items.Clear();
        ddlChildren7.Items.Clear();
        ddlChildren8.Items.Clear();

        List<ListItem> lCountryListItems = new List<ListItem>();

        for (int i = 0; i < _CountryListItems.Length; i++)
        {
            ListItem liCountry = new ListItem();
            lCountryListItems.Add(liCountry);

            liCountry.Value = _CountryListItems[i].CountryCode;
            liCountry.Text = _CountryListItems[i].CountryName;
        }

        ddlCountryList.Items.Add(new ListItem(GetLocalResourceObject("ddlSelectCountry.Text").ToString(), ""));

        for (int i = 0; i < lCountryListItems.Count; i++)
            ddlCountryList.Items.Add(lCountryListItems[i]);

        List<ListItem> lAreaListItems = new List<ListItem>();

        for (int i = 0; i < _AreaListItems.Length; i++)
        {
            ListItem liArea = new ListItem();
            lAreaListItems.Add(liArea);

            liArea.Value = _AreaListItems[i].AreaID;
            liArea.Text = _AreaListItems[i].AreaName;
        }

        ddlAreaList.Items.Add(new ListItem(GetLocalResourceObject("ddlSelectDestination.Text").ToString(), ""));

        for (int i = 0; i < lAreaListItems.Count; i++)
            ddlAreaList.Items.Add(lAreaListItems[i]);

        // Hotel
        List<ListItem> lHotelListItems = new List<ListItem>();

        for (int i = 0; i < _HotelListItems.Length; i++)
        {
            ListItem liHotel = new ListItem();
            lHotelListItems.Add(liHotel);

            liHotel.Value = _HotelListItems[i].HotelCode;
            liHotel.Text = _HotelListItems[i].HotelName;
        }

        ddlHotelList.Items.Add(new ListItem(GetLocalResourceObject("ddlSelectHotel.Text").ToString(), ""));
        ddlHotelList.Items.Add(new ListItem("Mama Istanbul", "istanbun"));

        for (int i = 0; i < lHotelListItems.Count; i++)
            ddlHotelList.Items.Add(lHotelListItems[i]);

        AddHotelButtonToCustomUI();

        // Country
        string strSelectedValue = "";

        if (_StayCriteriaSelection.CountryCode != null && _StayCriteriaSelection.CountryCode != "")
            strSelectedValue = _StayCriteriaSelection.CountryCode;
        else
            strSelectedValue = "";

        for (int i = 0; i < ddlCountryList.Items.Count; i++)
        {
            if (ddlCountryList.Items[i].Value == strSelectedValue)
                ddlCountryList.Items[i].Selected = true;
            else
                ddlCountryList.Items[i].Selected = false;
        }

        if (_StayCriteriaSelection.AreaID != null && _StayCriteriaSelection.AreaID != "")
            strSelectedValue = _StayCriteriaSelection.AreaID;
        else
            strSelectedValue = "";

        for (int i = 0; i < ddlAreaList.Items.Count; i++)
        {
            if (ddlAreaList.Items[i].Value == strSelectedValue)
                ddlAreaList.Items[i].Selected = true;
            else
                ddlAreaList.Items[i].Selected = false;
        }

        if (_StayCriteriaSelection.HotelCode != null && _StayCriteriaSelection.HotelCode != "")
            strSelectedValue = _StayCriteriaSelection.HotelCode;
        else
            strSelectedValue = "";

        for (int i = 0; i < ddlHotelList.Items.Count; i++)
        {
            if (ddlHotelList.Items[i].Value == strSelectedValue)
                ddlHotelList.Items[i].Selected = true;
            else
                ddlHotelList.Items[i].Selected = false;
        }

        DateTime dtNow = DateTime.Now;
        int intMaxYear = dtNow.Year + 5;

        for (int i = dtNow.Year; i <= intMaxYear; i++)
        {
            ddlYearArr.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlYearDep.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        ddlDayArr.Attributes.Add("onchange", "UpdateCalHiddenField();");
        ddlMonthArr.Attributes.Add("onchange", "UpdateCalHiddenField();");
        ddlYearArr.Attributes.Add("onchange", "UpdateCalHiddenField();");

        ddlDayDep.Attributes.Add("onchange", "UpdateCalHiddenFieldDep();");
        ddlMonthDep.Attributes.Add("onchange", "UpdateCalHiddenFieldDep();");
        ddlYearDep.Attributes.Add("onchange", "UpdateCalHiddenFieldDep();");

        ddlRoom.Attributes.Add("onchange", "HideRoomPan()");

        int intMaxBookingDays = Convert.ToInt32(ConfigurationManager.AppSettings["MaxBookingDays"]);
        int intMaxRooms = Convert.ToInt32(ConfigurationManager.AppSettings["StayCriteriaSelectorControl.MaxRooms"]);
        int intMaxAdults = Convert.ToInt32(ConfigurationManager.AppSettings["StayCriteriaSelectorControl.MaxAdults"]);
        int intMaxChildren = Convert.ToInt32(ConfigurationManager.AppSettings["StayCriteriaSelectorControl.MaxChildren"]);

        for (int i = 1; i <= intMaxBookingDays; i++)
            ddlStayNights.Items.Add(new ListItem(i.ToString(), i.ToString()));

        for (int i = 1; i <= intMaxRooms; i++)
            ddlRoom.Items.Add(new ListItem(i.ToString(), i.ToString()));

        for (int i = 0; i <= intMaxAdults; i++)
        {
            ddlAdults1.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlAdults2.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlAdults3.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlAdults4.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlAdults5.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlAdults6.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlAdults7.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlAdults8.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        for (int i = 0; i <= intMaxChildren; i++)
        {
            ddlChildren1.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlChildren2.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlChildren3.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlChildren4.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlChildren5.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlChildren6.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlChildren7.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlChildren8.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        if (intMaxChildren == 0)
        {
            lbChildren1.Visible = false;
            lbChildren2.Visible = false;
            lbChildren3.Visible = false;
            lbChildren4.Visible = false;
            lbChildren5.Visible = false;
            lbChildren6.Visible = false;
            lbChildren7.Visible = false;
            lbChildren8.Visible = false;

            ddlChildren1.Visible = false;
            ddlChildren2.Visible = false;
            ddlChildren3.Visible = false;
            ddlChildren4.Visible = false;
            ddlChildren5.Visible = false;
            ddlChildren6.Visible = false;
            ddlChildren7.Visible = false;
            ddlChildren8.Visible = false;
        }

        return;
    }

    void hotelButton_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string hotelCode = button.Attributes["HotelCode"];

        for (int i = 0; i < ddlHotelList.Items.Count; i++)
        {
            if (ddlHotelList.Items[i].Value == hotelCode)
                ddlHotelList.Items[i].Selected = true;
            else
                ddlHotelList.Items[i].Selected = false;
        }

        lblInfo2.Text = hotelCode;
        StayCriteriaSelection.HotelCode = hotelCode;
    }

    private string GetAreaName(string AreaID)
    {
        if (_AreaListItems != null)
        {
            for (int i = 0; i < _AreaListItems.Length; i++)
            {
                if (_AreaListItems[i].AreaID == AreaID)
                {
                    return _AreaListItems[i].AreaName + ", " + ((XnGR_WBS_Page)this.Page).GetCountryName(_AreaListItems[i].CountryCode);
                }

            }

        }

        return "";
    }

    private string GetHotelName(string HotelCode)
    {
        if (_HotelListItems != null)
        {
            for (int i = 0; i < _HotelListItems.Length; i++)
            {
                if (_HotelListItems[i].HotelCode == HotelCode)
                {
                    return _HotelListItems[i].HotelName;
                }

            }

        }

        return "";
    }

    private void UpdateStayCriteriaSelection()
    {
        StayCriteriaSelection objStayCriteriaSelection = new StayCriteriaSelection();

        objStayCriteriaSelection.ArrivalDate = DateTime.Parse(calDateHiddenFieldArr.Value);
        objStayCriteriaSelection.DepartureDate = DateTime.Parse(calDateHiddenFieldDep.Value);

        if (ConfigurationManager.AppSettings["StayCriteriaSelectorControl.UseStayNights"] == "1")
        {
            objStayCriteriaSelection.DepartureDate = objStayCriteriaSelection.ArrivalDate.AddDays(Convert.ToInt32(ddlStayNights.SelectedValue));
        }

        if (_StayCriteriaSelectorMode == StayCriteriaSelectorMode.New)
        {
            objStayCriteriaSelection.CountryCode = ddlCountryList.SelectedValue;
            objStayCriteriaSelection.AreaID = ddlAreaList.SelectedValue;
            objStayCriteriaSelection.HotelCode = ddlHotelList.SelectedValue;
        }

        else if (_StayCriteriaSelectorMode == StayCriteriaSelectorMode.Change)
        {
            objStayCriteriaSelection.CountryCode = _StayCriteriaSelection.CountryCode;
            objStayCriteriaSelection.AreaID = _StayCriteriaSelection.AreaID;
            objStayCriteriaSelection.HotelCode = _StayCriteriaSelection.HotelCode;
        }

        objStayCriteriaSelection.PromotionCode = tbPromotionCode.Text;

        List<RoomOccupantSelection> lRoomOccupantSelections = new List<RoomOccupantSelection>();

        for (int i = 1; i <= Convert.ToInt32(ddlRoom.SelectedValue); i++)
        {
            RoomOccupantSelection objRoomOccupantSelection = new RoomOccupantSelection();
            lRoomOccupantSelections.Add(objRoomOccupantSelection);

            objRoomOccupantSelection.RoomRefID = i.ToString();
            objRoomOccupantSelection.NumberRooms = 1;

            Panel objRoomPanel = (Panel)panRoom.FindControl("panRoom" + i.ToString());

            DropDownList objAdultsDDL = (DropDownList)objRoomPanel.FindControl("ddlAdults" + i.ToString());
            objRoomOccupantSelection.NumberAdults = Convert.ToInt32(objAdultsDDL.SelectedValue);

            DropDownList objChildrenDDL = (DropDownList)objRoomPanel.FindControl("ddlChildren" + i.ToString());
            objRoomOccupantSelection.NumberChildren = Convert.ToInt32(objChildrenDDL.SelectedValue);
        }

        objStayCriteriaSelection.RoomOccupantSelections = lRoomOccupantSelections.ToArray();

        _StayCriteriaSelection = objStayCriteriaSelection;

        return;
    }

    private void AddHotelButtonToCustomUI()
    {
        //Custom UI
        foreach (HotelListItem hotelItem in HotelListItems)
        {
            string hotelButtonID = "btnHotelButton" + hotelItem.HotelCode;

            if (phdStayCriteriaOptions.FindControl(hotelButtonID) != null)
                continue;


            Panel hotelButtonWrapper = new Panel();
            hotelButtonWrapper.CssClass = "MM_StayCriteria_HotelButtonWrapper";
            Button hotelButton = new Button();
            hotelButton.ID = hotelButtonID;
            hotelButton.Text = hotelItem.HotelName;
            hotelButton.CssClass = "MM_StayCriteria_HotelButton";
            hotelButton.Attributes.Add("HotelCode", hotelItem.HotelCode);
            hotelButton.Click += new EventHandler(hotelButton_Click);
           
            hotelButtonWrapper.Controls.Add(hotelButton);
            phdStayCriteriaOptions.Controls.Add(hotelButtonWrapper);
            formUpdatePanelStayCriteria.Triggers.Add(new AsyncPostBackTrigger { ControlID = hotelButton.ID, EventName = "Click" });
        }

        string hotelButton2Id = "btnHotelButtonIstanbun";
        if (phdStayCriteriaOptions.FindControl(hotelButton2Id) != null)
            return;

        // Test dummy data
        Panel hotelButtonWrapper2 = new Panel();
        hotelButtonWrapper2.CssClass = "MM_StayCriteria_HotelButtonWrapper";
        Button hotelButton2 = new Button();
        hotelButton2.ID = "btnHotelButtonIstanbun";
        hotelButton2.Text = "Mama Istanbul";
        hotelButton2.CssClass = "MM_StayCriteria_HotelButton";
        hotelButton2.Attributes.Add("HotelCode", "Istanbun");
        hotelButton2.Click += new EventHandler(hotelButton_Click);

        hotelButtonWrapper2.Controls.Add(hotelButton2);
        phdStayCriteriaOptions.Controls.Add(hotelButtonWrapper2);
        formUpdatePanelStayCriteria.Triggers.Add(new AsyncPostBackTrigger { ControlID = hotelButton2.ID, EventName = "Click" });

    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
