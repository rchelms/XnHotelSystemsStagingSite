using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XHS.WBSUIBizObjects;

public partial class ImageHoldingControl : System.Web.UI.UserControl
{

    public List<HotelImage> Images { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RenderUserControl()
    {
        if (Images == null || Images.Count <= 0)
            return;

        Images.ForEach(img =>
            {
                Image image = new Image() { ImageUrl = img.ImageURL };
                phdImageHolder.Controls.Add(image);
            });
    }
}