using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;

public partial class ProfileLoginControl : System.Web.UI.UserControl
{
    public delegate void ProfileLoginEvent(object sender, EventArgs e);
    public event ProfileLoginEvent ProfileLogin;

    public delegate void ProfileLogoutEvent(object sender, EventArgs e);
    public event ProfileLogoutEvent ProfileLogout;

    private ProfileLoginInfo _ProfileLoginInfo;
    private Profile[] _Profiles;
    private bool _IsLoggedIn;
    private bool _ViewLoginForm;
    private bool _Authenticate;
    private string[] _LoginErrors;

    public ProfileLoginInfo ProfileLoginInfo
    {
        get
        {
            return _ProfileLoginInfo;
        }

        set
        {
            _ProfileLoginInfo = value;
        }

    }

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

    public bool ViewLoginForm
    {
        get
        {
            return _ViewLoginForm;
        }

        set
        {
            _ViewLoginForm = value;
        }

    }

    public bool Authenticate
    {
        get
        {
            return _Authenticate;
        }

    }

    public string[] LoginErrors
    {
        get
        {
            return _LoginErrors;
        }

        set
        {
            _LoginErrors = value;
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

                lblLoginInfo.Text = sb.ToString();
            }

            tbLoginName.Text = "";
            tbLoginPassword.Text = "";

            panProfileViewLoginLink.Visible = false;
            panProfileViewLoginForm.Visible = false;
            panProfileViewLogOutLink.Visible = true;
        }

        else
        {
            if (_ViewLoginForm)
            {
                tbLoginName.Text = _ProfileLoginInfo.LogonName;
                tbLoginPassword.Text = "";

                panProfileLoginFormErrors.Visible = false;

                if (_LoginErrors.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < _LoginErrors.Length; i++)
                    {
                        sb.Append(_LoginErrors[i]);
                        sb.Append(@"<br />");
                    }

                    lblLoginErrorList.Text = sb.ToString();

                    panProfileLoginFormErrors.Visible = true;
                }

                panProfileViewLoginLink.Visible = false;
                panProfileViewLoginForm.Visible = true;
                panProfileViewLogOutLink.Visible = false;
            }

            else
            {
                tbLoginName.Text = "";
                tbLoginPassword.Text = "";

                panProfileViewLoginLink.Visible = true;
                panProfileViewLoginForm.Visible = false;
                panProfileViewLogOutLink.Visible = false;
            }

        }

        return;
    }

    protected void lbViewLoginForm_Click(object sender, EventArgs e)
    {
        _ViewLoginForm = true;
        _Authenticate = false;

        ProfileLogin(this, new EventArgs());

        return;
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        _ProfileLoginInfo.LogonName = tbLoginName.Text.Trim();
        _ProfileLoginInfo.LogonPassword = tbLoginPassword.Text.Trim();
        _ProfileLoginInfo.SecurityAnswer = "";

        _ViewLoginForm = false;
        _Authenticate = true;

        ProfileLogin(this, new EventArgs());

        return;
    }

    protected void btnCancelLogin_Click(object sender, EventArgs e)
    {
        _ProfileLoginInfo.LogonName = "";
        _ProfileLoginInfo.LogonPassword = "";
        _ProfileLoginInfo.SecurityAnswer = "";

        _ViewLoginForm = false;
        _Authenticate = false;

        ProfileLogin(this, new EventArgs());

        return;
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        _ProfileLoginInfo.LogonName = "";
        _ProfileLoginInfo.LogonPassword = "";
        _ProfileLoginInfo.SecurityAnswer = "";

        ProfileLogout(this, new EventArgs());

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
