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
using XHS.WBSUIBizObjects;

public partial class ProfileLoginNameControl : System.Web.UI.UserControl
{
    private Profile[] _Profiles;
    private bool _IsLoggedIn;

    public Profile[] Profiles
    {
        get
        {
            return _Profiles;
        }

        set
        {
            _Profiles = value;
        }

    }

    public bool IsLoggedIn
    {
        get
        {
            return _IsLoggedIn;
        }

        set
        {
            _IsLoggedIn = value;
        }

    }

    public void RenderUserControl()
    {
        if (_IsLoggedIn)
        {
            Profile objPersonProfile = ProfileHelper.GetProfile(_Profiles, ProfileType.Traveler);
            Profile objCompanyProfile = ProfileHelper.GetProfile(_Profiles, ProfileType.Corporation);
            Profile objAgencyProfile = ProfileHelper.GetProfile(_Profiles, ProfileType.TravelAgent);

            if (objPersonProfile != null)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(objPersonProfile.PersonFirstName);
                sb.Append(" ");
                sb.Append(objPersonProfile.PersonLastName);

                if (objCompanyProfile != null)
                {
                    sb.Append(" [");
                    sb.Append(objCompanyProfile.CompanyName);
                    sb.Append("]");
                }

                else if (objAgencyProfile != null)
                {
                    sb.Append(" [");
                    sb.Append(objAgencyProfile.CompanyName);
                    sb.Append("]");
                }

                lblLoginNameInfo.Text = sb.ToString();
            }

            panProfileLoginNameInfo.Visible = true;
        }

        else
        {
            lblLoginNameInfo.Text = "";
            panProfileLoginNameInfo.Visible = false;
        }

        return;
    }

}
