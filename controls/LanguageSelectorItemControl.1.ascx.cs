using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class LanguageSelectorItemControl : System.Web.UI.UserControl
{
    public delegate void LanguageItemSelectedEvent(object sender, EventArgs e);
    public event LanguageItemSelectedEvent LanguageItemSelected;

    private string _Culture;
    private string _UICulture;
    private string _LanguageText;
    private string _ImageURL;
    private bool _Selected;
    public bool DontShowLanguageText { get; set; }

    public string Culture
    {
        get
        {
            return _Culture;
        }

        set
        {
            _Culture = value;
        }

    }

    public string UICulture
    {
        get
        {
            return _UICulture;
        }

        set
        {
            _UICulture = value;
        }

    }

    public string LanguageText
    {
        get
        {
            return _LanguageText;
        }

        set
        {
            _LanguageText = value;
        }

    }

    public string ImageURL
    {
        get
        {
            return _ImageURL;
        }

        set
        {
            _ImageURL = value;
        }

    }

    public bool Selected
    {
        get
        {
            return _Selected;
        }

        set
        {
            _Selected = value;
        }

    }

    public void RenderUserControl()
    {
        panLanguageItemSelected.Visible = false;
        panLanguageItemNotSelected.Visible = false;

        panLanguageItemNotSelected.Attributes.Remove("onclick");

        btnLanguageItemSelected.ImageUrl = _ImageURL;
        btnLanguageItemNotSelected.ImageUrl = _ImageURL;

        btnLanguageItemSelected.AlternateText = _LanguageText;
        btnLanguageItemNotSelected.AlternateText = _LanguageText;

        lblLanguageItemSelectedText.Text = _LanguageText;
        lblLanguageItemNotSelectedText.Text = _LanguageText;

        lblLanguageItemSelectedText.ToolTip = _LanguageText;
        lblLanguageItemNotSelectedText.ToolTip = _LanguageText;

        if (_Selected)
        {
            panLanguageItemSelected.Visible = true;
        }

        else
        {
            panLanguageItemNotSelected.Visible = true;
            panLanguageItemNotSelected.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(btnLanguageItemNotSelected, ""));
        }

        if(DontShowLanguageText)
        {
            lblLanguageItemSelectedText.Visible = false;
            lblLanguageItemNotSelectedText.Visible = false;
        }

        return;
    }

    protected void btnLanguageItem_Click(object sender, EventArgs e)
    {
        if (LanguageItemSelected != null)
        {
            LanguageItemSelected(this, new EventArgs());
            _Selected = true;
        }

        return;
    }

}
