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

public partial class LanguageSelectorControl : System.Web.UI.UserControl
{
    public delegate void LanguageSelectedEvent(object sender, EventArgs e);
    public event LanguageSelectedEvent LanguageSelected;

    private string _SelectedCulture;
    private string _SelectedUICulture;
    private string _SelectedLanguageText;

    private List<LanguageSelectorItemControl> lLanguageSelectorItems;

    public string SelectedCulture
    {
        get
        {
            return _SelectedCulture;
        }

    }

    public string SelectedUICulture
    {
        get
        {
            return _SelectedUICulture;
        }

    }

    public string SelectedLanguageText
    {
        get
        {
            return _SelectedLanguageText;
        }

    }

    public LanguageSelectorItemControl[] LanguageSelectorItems
    {
        get
        {
            if (lLanguageSelectorItems != null)
                return lLanguageSelectorItems.ToArray();
            else
                return new LanguageSelectorItemControl[0];
        }

    }

    public void Clear()
    {
        _SelectedCulture = "";
        _SelectedUICulture = "";
        _SelectedLanguageText = "";

        lLanguageSelectorItems = null;

        return;
    }

    public void Add(LanguageSelectorItemControl ucLanguageSelectorItem)
    {
        if (lLanguageSelectorItems == null)
        {
            lLanguageSelectorItems = new List<LanguageSelectorItemControl>();
        }
        lLanguageSelectorItems.Add(ucLanguageSelectorItem);

        return;
    }

    public void AddSelectedItem(LanguageSelectorItemControl ucLanguageSelectorItem)
    {
        ucLanguageSelectorItem.LanguageItemSelected += ItemClicked;
        phSelectedLanguage.Controls.Add(ucLanguageSelectorItem);
        return;
    }

     public void ItemClicked(object sender, EventArgs e)
     {
         panSelectedLanguage.Visible = false;
         panLanguageSelectorEdit.Visible = true;
     }

    public void LanguageItemSelected(object sender, EventArgs e)
    {
        LanguageSelectorItemControl ucLanguageSelectorItem = (LanguageSelectorItemControl)sender;

        _SelectedCulture = ucLanguageSelectorItem.Culture;
        _SelectedUICulture = ucLanguageSelectorItem.UICulture;
        _SelectedLanguageText = ucLanguageSelectorItem.LanguageText;

        for (int i = 0; i < lLanguageSelectorItems.Count; i++)
        {
            if (lLanguageSelectorItems[i].Culture != ucLanguageSelectorItem.Culture)
                lLanguageSelectorItems[i].Selected = false;
        }

        LanguageSelected(this, new EventArgs());
        return;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //panLanguageSelectorEdit.Visible = false;
            //panSelectedLanguage.Visible = true;
        }
        if (IsPostBack & !this.IsParentPreRender())
        {
            this.ApplyControlsToPage();
        }
        
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ScriptManager.RegisterClientScriptInclude(Page, typeof(Page), "LanguageSelectorControl", this.ResolveUrl(Constants.ScriptPath.LanguageSelectorControl));
    }

    public void RenderUserControl()
    {
        this.ApplyControlsToPage();

        for (int i = 0; i < lLanguageSelectorItems.Count; i++)
            lLanguageSelectorItems[i].RenderUserControl();
        for (int i = 0; i < phSelectedLanguage.Controls.Count; i++)
            ((LanguageSelectorItemControl)phSelectedLanguage.Controls[i]).RenderUserControl();

        return;
    }

    private void ApplyControlsToPage()
    {
        phLanguageSelector.Controls.Clear();

        if (lLanguageSelectorItems == null)
        {
            lLanguageSelectorItems = new List<LanguageSelectorItemControl>();
        }

        for (int i = 0; i < lLanguageSelectorItems.Count; i++)
        {
            phLanguageSelector.Controls.Add(lLanguageSelectorItems[i]);
            lLanguageSelectorItems[i].LanguageItemSelected += this.LanguageItemSelected;
        }

        return;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
