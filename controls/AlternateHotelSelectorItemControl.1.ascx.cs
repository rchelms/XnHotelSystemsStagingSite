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

public partial class AlternateHotelSelectorItemControl : System.Web.UI.UserControl
{
    public delegate void AlternateHotelSelectedEvent(object sender, string HotelCode);
    public event AlternateHotelSelectedEvent AlternateHotelSelected;

    private HotelDescriptiveInfo _HotelDescriptiveInfo;

    List<AlternateHotelRatePlanItemControl> lAlternateHotelRatePlanItemControls;

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

    public AlternateHotelRatePlanItemControl[] AlternateHotelRatePlanItems
    {
        get
        {
            if (lAlternateHotelRatePlanItemControls != null)
                return lAlternateHotelRatePlanItemControls.ToArray();
            else
                return new AlternateHotelRatePlanItemControl[0];
        }

    }
    
    public void  Clear()
    {
        lAlternateHotelRatePlanItemControls = null;
        return;
    }

    public void Add(AlternateHotelRatePlanItemControl ucAlternateHotelRatePlanItemControl)
    {
        if (lAlternateHotelRatePlanItemControls == null)
        {
            lAlternateHotelRatePlanItemControls = new List<AlternateHotelRatePlanItemControl>();
        }

        lAlternateHotelRatePlanItemControls.Add(ucAlternateHotelRatePlanItemControl);
        return;
    }

    protected void btnSelectAlternateHotel_Click(object sender, EventArgs e)
    {
        AlternateHotelSelected(sender, _HotelDescriptiveInfo.HotelCode);
        return;
    }

    public void RenderUserControl()
    {
        StringBuilder sb;

        ApplyControlsToPage();

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

            sb.Append(_HotelDescriptiveInfo.Address1);
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
        ucHotelDescriptionControl.RenderUserControl();

        phHotelRating.Controls.Clear();

        string strHotelRatingControlPath = ConfigurationManager.AppSettings["HotelRatingControl.ascx"];
        HotelRatingControl ucHotelRatingControl = (HotelRatingControl)LoadControl(strHotelRatingControlPath);
        phHotelRating.Controls.Add(ucHotelRatingControl);

        ucHotelRatingControl.RatingProvider = _HotelDescriptiveInfo.RatingProvider;
        ucHotelRatingControl.Rating = _HotelDescriptiveInfo.Rating;
        ucHotelRatingControl.RatingSymbol = _HotelDescriptiveInfo.RatingSymbol;
        ucHotelRatingControl.RenderUserControl();

        for (int i = 0; i < lAlternateHotelRatePlanItemControls.Count; i++)
            lAlternateHotelRatePlanItemControls[i].RenderUserControl();

        return;
    }

    private void ApplyControlsToPage()
    {
        if (lAlternateHotelRatePlanItemControls == null)
        {
            lAlternateHotelRatePlanItemControls = new List<AlternateHotelRatePlanItemControl>();
        }

        phRatePlans.Controls.Clear();

        for (int i = 0; i < lAlternateHotelRatePlanItemControls.Count; i++)
        {
            phRatePlans.Controls.Add(lAlternateHotelRatePlanItemControls[i]);

            lAlternateHotelRatePlanItemControls[i].RenderUserControl();
        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
