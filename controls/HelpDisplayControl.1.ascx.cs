using System;
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
using XHS.WBSUIBizObjects;

//
// IMPORTANT NOTE: This control is self-rendering and responds to PreRenderComplete event so it can be applied
// declaritively within any page and not require load / configure coordination from parent page code base and
// and hence does not require synchronization.
//

public partial class HelpDisplayControl : System.Web.UI.UserControl
{
    private string _HelpCode;

    public string HelpCode
    {
        get
        {
            return _HelpCode;
        }

        set
        {
            _HelpCode = value;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.PreRenderComplete += new EventHandler(this.Page_PreRenderComplete);
        return;
    }

    public void Page_PreRenderComplete(object sender, EventArgs e)
    {
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

        HotelDescriptiveInfo objHotelDescriptiveInfo = new HotelDescriptiveInfo();
        objHotelDescriptiveInfo.Descriptions = new HotelDescription[0];

        if (objHotelDescriptiveInfoRS != null && objHotelDescriptiveInfoRS.HotelDescriptiveInfos != null && objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        string strHelpInfoMessage = "";

        for (int i = 0; i < objHotelDescriptiveInfo.Descriptions.Length; i++)
        {
            if (objHotelDescriptiveInfo.Descriptions[i].CategoryCode == HotelDescriptionCategoryCode.MiscellaneousInformation && objHotelDescriptiveInfo.Descriptions[i].MiscCategoryReferenceCode.ToLower().Trim() == _HelpCode.ToLower().Trim())
            {
                strHelpInfoMessage = objHotelDescriptiveInfo.Descriptions[i].ContentText;
                break;
            }

        }

        if (strHelpInfoMessage == "")
        {
            strHelpInfoMessage = (String)GetGlobalResourceObject("HelpResources", _HelpCode);
        }

        lblHelpInfoMessage.Text = strHelpInfoMessage;
        ibHelpInfo.Attributes.Add("Onclick", "return hs.htmlExpand(this, { contentId: '" + this.ClientID + "_HelpInfoPopup'} )");

        return;
    }

}
