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

public partial class HotelDescriptionControl : System.Web.UI.UserControl
{
    private HotelDescriptiveInfo _HotelDescriptiveInfo;
    private bool _HotelDescriptionOnly;

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

    public bool HotelDescriptionOnly
    {
        get
        {
            return _HotelDescriptionOnly;
        }

        set
        {
            _HotelDescriptionOnly = value;
        }

    }

    public void RenderUserControl()
    {
        lblHotelDescPopupTitle.Text = _HotelDescriptiveInfo.HotelName;

        for (int i = 0; i < _HotelDescriptiveInfo.Images.Length; i++)
        {
            if (_HotelDescriptiveInfo.Images[i].CategoryCode == HotelImageCategoryCode.ExteriorView && _HotelDescriptiveInfo.Images[i].ImageSize == HotelImageSize.FullSize)
            {
                imgHotelDescPopupImage.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _HotelDescriptiveInfo.Images[i].ImageURL;
                break;
            }

        }

        phHotelDescriptionItems.Controls.Clear();

        for (int i = 0; i < _HotelDescriptiveInfo.Descriptions.Length; i++)
        {
            if (_HotelDescriptiveInfo.Descriptions[i].CategoryCode != HotelDescriptionCategoryCode.RoomTypeDescription && _HotelDescriptiveInfo.Descriptions[i].CategoryCode != HotelDescriptionCategoryCode.RatePlanDescription && _HotelDescriptiveInfo.Descriptions[i].CategoryCode != HotelDescriptionCategoryCode.PackageDescription)
            {
                if (_HotelDescriptionOnly && HotelDescriptiveInfo.Descriptions[i].CategoryCode != HotelDescriptionCategoryCode.PropertyDescription)
                    continue;

                if (HotelDescriptiveInfo.Descriptions[i].CategoryCode == HotelDescriptionCategoryCode.MiscellaneousInformation && HotelDescriptiveInfo.Descriptions[i].MiscCategoryReferenceCode != "") // filter out special "tagged" misc items
                    continue;

                string strHotelDescriptionItemControlPath = ConfigurationManager.AppSettings["HotelDescriptionItemControl.ascx"];
                HotelDescriptionItemControl ucHotelDescriptionItemControl = (HotelDescriptionItemControl)LoadControl(strHotelDescriptionItemControlPath);

                ucHotelDescriptionItemControl.Title = _HotelDescriptiveInfo.Descriptions[i].ContentTitle;
                ucHotelDescriptionItemControl.Description = _HotelDescriptiveInfo.Descriptions[i].ContentText;

                phHotelDescriptionItems.Controls.Add(ucHotelDescriptionItemControl);

                ucHotelDescriptionItemControl.RenderUserControl();
            }

        }

        lbHotelDescription.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_HotelDescPopup'} )");

        hlHotelBrochure.NavigateUrl = "";
        hlHotelBrochure.Enabled = false;

        hlHotelMap.NavigateUrl = "";
        hlHotelMap.Enabled = false;

        for (int i = 0; i < _HotelDescriptiveInfo.Documents.Length; i++)
        {
            if (_HotelDescriptiveInfo.Documents[i].CategoryCode == HotelDocumentCategoryCode.Brochure)
            {
                hlHotelBrochure.NavigateUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _HotelDescriptiveInfo.Documents[i].DocumentURL;
                hlHotelBrochure.Enabled = true;
            }

            else if (_HotelDescriptiveInfo.Documents[i].CategoryCode == HotelDocumentCategoryCode.Map)
            {
                hlHotelMap.NavigateUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _HotelDescriptiveInfo.Documents[i].DocumentURL;
                hlHotelMap.Enabled = true;
            }

        }

        hlHotelPhotos.NavigateUrl = "../pages/imagegallery.aspx?hotel=" + _HotelDescriptiveInfo.HotelCode;
        hlHotelPhotos.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_ImageGalleryPopup', objectType: 'iframe'} )");

        return;
    }

}
