using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;
using XCDN;

public partial class ImageGallery : XnGR_WBS_Page
{
    private HotelImageGallery objHotelImageGallery;

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);

        return;
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        string strHotelCode = "";

        if (!IsPostBack)
        {
            if (Request.QueryString.Get("hotel") != null && Request.QueryString.Get("hotel") != "")
            {
                strHotelCode = Request.QueryString.Get("hotel");
            }

            hfHotelCode.Value = strHotelCode;
        }

        else
        {
            strHotelCode = hfHotelCode.Value;
        }

        objHotelImageGallery = null;

        if (strHotelCode != "")
        {
            objHotelImageGallery = this.GetImageGallery(strHotelCode);

            if (objHotelImageGallery == null)
            {
                HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];
                HotelDescriptiveInfoRS objSearchHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["SearchHotelDescriptiveInfoRS"];
                HotelDescriptiveInfoRS objAlternateHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["AlternateHotelDescriptiveInfoRS"];

                HotelDescriptiveInfo objHotelDescriptiveInfo = this.LocateHotelDescriptiveInfo(strHotelCode, objHotelDescriptiveInfoRS);

                if (objHotelDescriptiveInfo == null)
                    objHotelDescriptiveInfo = this.LocateHotelDescriptiveInfo(strHotelCode, objSearchHotelDescriptiveInfoRS);

                if (objHotelDescriptiveInfo == null)
                    objHotelDescriptiveInfo = this.LocateHotelDescriptiveInfo(strHotelCode, objAlternateHotelDescriptiveInfoRS);

                if (objHotelDescriptiveInfo != null)
                {
                    objHotelImageGallery = this.MakeImageGallery(objHotelDescriptiveInfo);
                    this.PutImageGallery(objHotelImageGallery);
                }

            }

        }

        return;
    }

    protected void btnStripLeft_Click(object sender, EventArgs e)
    {
        if (objHotelImageGallery != null)
        {
            objHotelImageGallery.CurrentHomeThumbNumber--;

            if (objHotelImageGallery.CurrentHomeThumbNumber < 0)
                objHotelImageGallery.CurrentHomeThumbNumber = 0;

            this.PutImageGallery(objHotelImageGallery);
        }

        return;
    }

    protected void btnStripRight_Click(object sender, EventArgs e)
    {
        if (objHotelImageGallery != null)
        {
            if ((objHotelImageGallery.Images.Length - objHotelImageGallery.CurrentHomeThumbNumber - 1) >= 4)
            {
                objHotelImageGallery.CurrentHomeThumbNumber++;
                this.PutImageGallery(objHotelImageGallery);
            }

        }

        return;
    }

    protected void btnThumb_Click(object sender, EventArgs e)
    {
        if (objHotelImageGallery != null)
        {
            int intSelectedThumbPosition = 0;

            if (!Int32.TryParse(((ImageButton)sender).CommandName, out intSelectedThumbPosition))
                return;

            if ((objHotelImageGallery.CurrentHomeThumbNumber + intSelectedThumbPosition - 1) >= objHotelImageGallery.Images.Length)
                return;

            objHotelImageGallery.CurrentImageNumber = objHotelImageGallery.CurrentHomeThumbNumber + intSelectedThumbPosition - 1;
            this.PutImageGallery(objHotelImageGallery);
        }

        return;
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        if (objHotelImageGallery != null)
        {
            objHotelImageGallery.CurrentImageNumber--;

            if (objHotelImageGallery.CurrentImageNumber < 0)
                objHotelImageGallery.CurrentImageNumber = objHotelImageGallery.Images.Length - 1;

            this.PutImageGallery(objHotelImageGallery);
        }

        return;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (objHotelImageGallery != null)
        {
            objHotelImageGallery.CurrentImageNumber++;

            if (objHotelImageGallery.CurrentImageNumber >= objHotelImageGallery.Images.Length)
                objHotelImageGallery.CurrentImageNumber = 0;

            this.PutImageGallery(objHotelImageGallery);
        }

        return;
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (objHotelImageGallery != null && objHotelImageGallery.Images.Length != 0)
        {
            // Update full size image display

            HotelImageGalleryItem objHotelImageGalleryItem = objHotelImageGallery.Images[objHotelImageGallery.CurrentImageNumber];

            lblHotelName.Text = objHotelImageGallery.HotelName;
            lblTitle.Text = objHotelImageGalleryItem.FullSizeImage.ContentTitle;
            lblDescription.Text = objHotelImageGalleryItem.FullSizeImage.ContentText;
            imgFullSize.ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + objHotelImageGalleryItem.FullSizeImage.ImageURL;
            imgFullSize.ToolTip = objHotelImageGalleryItem.FullSizeImage.ContentTitle;

            // Update thumbnal stip

            for (int i = 0; i < 4; i++)
            {
                Control objControl = this.FindControl("btnThumb" + ((int)(i + 1)).ToString());

                if (objControl != null && objHotelImageGallery.CurrentHomeThumbNumber + i < objHotelImageGallery.Images.Length)
                {
                    HotelImageGalleryItem _objHotelImageGalleryItem = objHotelImageGallery.Images[objHotelImageGallery.CurrentHomeThumbNumber + i];

                    ((XImageButton)objControl).ImageUrl = ConfigurationManager.AppSettings["ImageUrlPrefix"] + _objHotelImageGalleryItem.ThumbnailImage.ImageURL;
                    ((XImageButton)objControl).ToolTip = _objHotelImageGalleryItem.ThumbnailImage.ContentTitle;
                    ((XImageButton)objControl).Location = LocationType.HotelMediaCDNPath;

                }

            }

            for (int i = 0; i < 4; i++)
            {
                Control objControl = this.FindControl("panThumb" + ((int)(i + 1)).ToString());

                if (objControl != null)
                {
                    ((Panel)objControl).CssClass = "image_gallery_thumbstrip_thumb";

                    if (objHotelImageGallery.CurrentImageNumber >= objHotelImageGallery.CurrentHomeThumbNumber && objHotelImageGallery.CurrentImageNumber <= (objHotelImageGallery.CurrentHomeThumbNumber + 3))
                    {
                        if ((objHotelImageGallery.CurrentImageNumber - objHotelImageGallery.CurrentHomeThumbNumber) == i)
                        {
                            ((Panel)objControl).CssClass = "image_gallery_thumbstrip_thumb_selected";
                        }

                    }

                }

            }

            // Update footer area

            string strImageNumber = ((int)(objHotelImageGallery.CurrentImageNumber + 1)).ToString();
            string strImageCount = objHotelImageGallery.Images.Length.ToString();

            string strImageIdentifier = (string)GetLocalResourceObject("ImageIdentifier");

            strImageIdentifier = strImageIdentifier.Replace("{image_number}", strImageNumber);
            strImageIdentifier = strImageIdentifier.Replace("{image_count}", strImageCount);

            lblImageIdentifier.Text = strImageIdentifier;
        }

        else
        {
            lblTitle.Text = (string)GetLocalResourceObject("ImageNotAvailable");
            lblDescription.Text = "";
            imgFullSize.ImageUrl = "";
            lblImageIdentifier.Text = "";
        }

        this.PageComplete();

        return;
    }

    private HotelDescriptiveInfo LocateHotelDescriptiveInfo(string strHotelCode, HotelDescriptiveInfoRS objHotelDescriptiveInfoRS)
    {
        if (objHotelDescriptiveInfoRS != null && objHotelDescriptiveInfoRS.HotelDescriptiveInfos != null)
        {
            for (int i = 0; i < objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length; i++)
            {
                if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos[i].HotelCode == strHotelCode)
                {
                    return objHotelDescriptiveInfoRS.HotelDescriptiveInfos[i];
                }

            }

        }

        return null;
    }

    private HotelImageGallery MakeImageGallery(HotelDescriptiveInfo objHotelDescriptiveInfo)
    {
        string[] objImageTitles = this.GetImageGalleryImageTitles(objHotelDescriptiveInfo);

        List<HotelImageGalleryItem> lHotelImageGalleryItems = new List<HotelImageGalleryItem>();

        for (int i = 0; i < objImageTitles.Length; i++)
        {
            HotelImageGalleryItem objHotelImageGalleryItem = new HotelImageGalleryItem();

            objHotelImageGalleryItem.ThumbnailImage = this.GetImageGalleryThumbnailImage(objImageTitles[i], objHotelDescriptiveInfo);
            objHotelImageGalleryItem.FullSizeImage = this.GetImageGalleryFullSizeImage(objImageTitles[i], objHotelDescriptiveInfo);

            if (objHotelImageGalleryItem.ThumbnailImage != null && objHotelImageGalleryItem.FullSizeImage != null)
            {
                lHotelImageGalleryItems.Add(objHotelImageGalleryItem);
            }

        }

        HotelImageGallery objHotelImageGallery = new HotelImageGallery();

        objHotelImageGallery.HotelCode = objHotelDescriptiveInfo.HotelCode;
        objHotelImageGallery.HotelName = objHotelDescriptiveInfo.HotelName;
        objHotelImageGallery.CurrentImageNumber = 0;
        objHotelImageGallery.CurrentHomeThumbNumber = 0;
        objHotelImageGallery.Images = lHotelImageGalleryItems.ToArray();

        return objHotelImageGallery;
    }

    private void PutImageGallery(HotelImageGallery objHotelImageGallery)
    {
        List<HotelImageGallery> lHotelImageGalleries = (List<HotelImageGallery>)Session["ImageGalleries"];

        bool bLocated = false;

        for (int i = 0; i < lHotelImageGalleries.Count; i++)
        {
            if (lHotelImageGalleries[i].HotelCode == objHotelImageGallery.HotelCode)
            {
                lHotelImageGalleries[i] = objHotelImageGallery;

                bLocated = true;
                break;
            }

        }

        if (!bLocated)
        {
            lHotelImageGalleries.Add(objHotelImageGallery);
        }

        Session["ImageGalleries"] = lHotelImageGalleries;

        return;
    }

    private HotelImageGallery GetImageGallery(string strHotelCode)
    {
        List<HotelImageGallery> lHotelImageGalleries = (List<HotelImageGallery>)Session["ImageGalleries"];

        for (int i = 0; i < lHotelImageGalleries.Count; i++)
        {
            if (lHotelImageGalleries[i].HotelCode == strHotelCode)
            {
                return lHotelImageGalleries[i];
            }

        }

        return null;
    }

    private string[] GetImageGalleryImageTitles(HotelDescriptiveInfo objHotelDescriptiveInfo)
    {
        List<string> lTitles = new List<string>();

        if (objHotelDescriptiveInfo != null)
        {
            for (int i = 0; i < objHotelDescriptiveInfo.Images.Length; i++)
            {
                if (objHotelDescriptiveInfo.Images[i].CategoryCode == HotelImageCategoryCode.PhotoGallery && !lTitles.Contains(objHotelDescriptiveInfo.Images[i].ContentTitle))
                    lTitles.Add(objHotelDescriptiveInfo.Images[i].ContentTitle);
            }

        }

        return lTitles.ToArray();
    }

    private HotelImage GetImageGalleryThumbnailImage(string strImageTitle, HotelDescriptiveInfo objHotelDescriptiveInfo)
    {
        for (int i = 0; i < objHotelDescriptiveInfo.Images.Length; i++)
        {
            if (objHotelDescriptiveInfo.Images[i].CategoryCode == HotelImageCategoryCode.PhotoGallery && objHotelDescriptiveInfo.Images[i].ContentTitle == strImageTitle && objHotelDescriptiveInfo.Images[i].ImageSize == HotelImageSize.Thumbnail)
            {
                return objHotelDescriptiveInfo.Images[i];
            }

        }

        return null;
    }

    private HotelImage GetImageGalleryFullSizeImage(string strImageTitle, HotelDescriptiveInfo objHotelDescriptiveInfo)
    {
        for (int i = 0; i < objHotelDescriptiveInfo.Images.Length; i++)
        {
            if (objHotelDescriptiveInfo.Images[i].CategoryCode == HotelImageCategoryCode.PhotoGallery && objHotelDescriptiveInfo.Images[i].ContentTitle == strImageTitle && objHotelDescriptiveInfo.Images[i].ImageSize == HotelImageSize.FullSize)
            {
                return objHotelDescriptiveInfo.Images[i];
            }

        }

        return null;
    }

}
