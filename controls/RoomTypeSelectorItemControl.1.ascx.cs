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
using MamaShelter;

public partial class RoomTypeSelectorItemControl : System.Web.UI.UserControl
{
    public delegate void ShowMoreLessRatesEvent(object sender, EventArgs e);
    public event ShowMoreLessRatesEvent ShowMoreLessRates;

    private string _RoomRefID;
    private HotelDescRoomType _RoomType;
    private bool _ShowMoreRates;
    public SelectionMode Mode { get; set; }

    private List<RatePlanSelectorItemControl> lRatePlanSelectorItems;

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

    public HotelDescRoomType RoomType
    {
        get
        {
            return _RoomType;
        }

        set
        {
            _RoomType = value;
        }

    }

    public bool ShowMoreRates
    {
        get
        {
            return _ShowMoreRates;
        }

        set
        {
            _ShowMoreRates = value;
        }

    }

    public RatePlanSelectorItemControl[] RatePlanSelectorItems
    {
        get
        {
            if (lRatePlanSelectorItems != null)
                return lRatePlanSelectorItems.ToArray();
            else
                return new RatePlanSelectorItemControl[0];
        }

    }

    public void Clear()
    {
        lRatePlanSelectorItems = null;
        return;
    }

    public void AddRatePlanSelectorItem(RatePlanSelectorItemControl ucNewRatePlanSelectorItem)
    {
        if (lRatePlanSelectorItems == null)
        {
            lRatePlanSelectorItems = new List<RatePlanSelectorItemControl>();
        }

        lRatePlanSelectorItems.Add(ucNewRatePlanSelectorItem);
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack & !this.IsParentPreRender())
        {
            this.ApplyControlsToPage();
        }

        return;
    }

    public void RenderUserControl()
    {
        this.ApplyControlsToPage();

        //for (int i = 0; i < _RoomType.Images.Length; i++)
        //{
        //    if (_RoomType.Images[i].CategoryCode == HotelImageCategoryCode.GuestRoom && _RoomType.Images[i].ImageSize == HotelImageSize.Thumbnail)
        //    {
        //        imgRoomType.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] +_RoomType.Images[i].ImageURL;
        //    }

        //    if (_RoomType.Images[i].CategoryCode == HotelImageCategoryCode.GuestRoom && _RoomType.Images[i].ImageSize == HotelImageSize.FullSize)
        //    {
        //        imgRoomTypePopupImage.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _RoomType.Images[i].ImageURL;
        //    }

        //}

        //lblRoomTypePopupTitle.Text = _RoomType.LongDescription.ContentTitle.ToString();
        //lblRoomTypePopupDescription.Text = this.AddLineBreaks(_RoomType.LongDescription.ContentText.ToString());

        //lblRoomTypeNameText.Text = _RoomType.Name;
        //lblRoomTypeDescriptionText.Text = this.AddLineBreaks(_RoomType.ShortDescription);

        //lbRoomDescription.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_RoomTypeDescPopup'} )");

        //panShowMoreRatesLink.Visible = false;
        //panShowLessRatesLink.Visible = false;

        //if (ConfigurationManager.AppSettings["RoomTypeSelectorItemControl.EnableShowMoreRates"] == "1" && !_ShowMoreRates && lRatePlanSelectorItems.Count > 1)
        //{
        //    panShowMoreRatesLink.Visible = true;
        //}

        //else if (ConfigurationManager.AppSettings["RoomTypeSelectorItemControl.EnableShowMoreRates"] == "1" && _ShowMoreRates && lRatePlanSelectorItems.Count > 1)
        //{
        //    panShowLessRatesLink.Visible = true;
        //}

        int intNumRatePlanSelectorItems = this.GetNumRatePlanSelectorItems();

        for (int i = 0; i < intNumRatePlanSelectorItems; i++)
            lRatePlanSelectorItems[i].RenderUserControl();

        return;
    }

    protected void btnShowMoreLessRates_Click(object sender, EventArgs e)
    {
        _ShowMoreRates = !_ShowMoreRates;

        ShowMoreLessRates(this, new EventArgs());
        return;
    }

    private void ApplyControlsToPage()
    {
        if (lRatePlanSelectorItems == null)
        {
            lRatePlanSelectorItems = new List<RatePlanSelectorItemControl>();
        }

        phRatePlans.Controls.Clear();

        int intNumRatePlanSelectorItems = this.GetNumRatePlanSelectorItems();

        for (int i = 0; i < intNumRatePlanSelectorItems; i++)
            phRatePlans.Controls.Add(lRatePlanSelectorItems[i]);

        return;
    }

    private int GetNumRatePlanSelectorItems()
    {
        int intNumRatePlanSelectorItems = 0;

        if (ConfigurationManager.AppSettings["RoomTypeSelectorItemControl.EnableShowMoreRates"] != "1" || _ShowMoreRates)
        {
            intNumRatePlanSelectorItems = lRatePlanSelectorItems.Count;
        }

        else
        {
            if (lRatePlanSelectorItems.Count > 0)
                intNumRatePlanSelectorItems = 1;
        }

        return intNumRatePlanSelectorItems;
    }

    private string AddLineBreaks(string strString)
    {
        string strReturn;

        strReturn = strString.Replace("\r", "");
        strReturn = strReturn.Replace("\n", "<br />");

        return strReturn;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
