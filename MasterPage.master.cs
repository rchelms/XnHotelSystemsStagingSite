using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Text;
using System.Web.UI;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private LanguageSelectorControl ucLanguageSelectorControl;

    private string SelectedCulture
    {
        get
        {
            string strSelectedCulture = (string)Session["SelectedCulture"];

            if (!string.IsNullOrEmpty(strSelectedCulture))
                return strSelectedCulture;

            if (ConfigurationManager.AppSettings["DefaultCulture"] != null && ConfigurationManager.AppSettings["DefaultCulture"] != "")
                return ConfigurationManager.AppSettings["DefaultCulture"];

            return "en-US";
        }

        set
        {
            Session["SelectedCulture"] = value;
        }

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        string strLanguageSelectorControlPath = ConfigurationManager.AppSettings["LanguageSelectorControl.ascx"];
        ucLanguageSelectorControl = (LanguageSelectorControl)LoadControl(strLanguageSelectorControlPath);

        phLanguageSelectorControl.Controls.Clear();
        phLanguageSelectorControl.Controls.Add(ucLanguageSelectorControl);

        ucLanguageSelectorControl.LanguageSelected += new LanguageSelectorControl.LanguageSelectedEvent(this.LanguageSelected);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
            ConfigureLanguageSeletorControl();
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ucLanguageSelectorControl.RenderUserControl();

        JavaScriptSerializer serialier = new JavaScriptSerializer();
        var AvailCalDateSelectorResource = new
        {
            CheckInInstructionMessage = Resources.JSResources.AvailCalSelectorControl_CheckInInstruction,
            CheckOutstructionMessage = Resources.JSResources.AvailCalSelectorControl_CheckOutInstruction,
            StepBackwardWarningMessage = Resources.JSResources.StepBackwardWarningMessage
        };

        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendFormat("__MamaShelterResources = {0};", serialier.Serialize(AvailCalDateSelectorResource));
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "MasterPageScript", scriptBuilder.ToString(), true);
    }
    void ConfigureLanguageSeletorControl()
    {
        string strLanguageSelectorItemControlPath = ConfigurationManager.AppSettings["LanguageSelectorItemControl.ascx"];

        this.ucLanguageSelectorControl.Clear();

        LanguageSetup[] objLanguageSetups = GetLanguageSetups();

        for (int i = 0; i < objLanguageSetups.Length; i++)
        {
            LanguageSelectorItemControl ucLanguageSelectorItemControl = (LanguageSelectorItemControl)LoadControl(strLanguageSelectorItemControlPath);
            ucLanguageSelectorControl.Add(ucLanguageSelectorItemControl);
            ucLanguageSelectorItemControl.ID = "LanguageSelectorItem" + (i + 1);
            ucLanguageSelectorItemControl.Culture = objLanguageSetups[i].Culture;
            ucLanguageSelectorItemControl.UICulture = objLanguageSetups[i].UICulture;
            ucLanguageSelectorItemControl.LanguageText = objLanguageSetups[i].LanguageText;
            ucLanguageSelectorItemControl.ImageURL = objLanguageSetups[i].ImageURL;
            ucLanguageSelectorItemControl.Selected = false;

            if (SelectedCulture == objLanguageSetups[i].Culture)
            {
                ucLanguageSelectorItemControl.Selected = true;

                LanguageSelectorItemControl ucSelectedLanguage = (LanguageSelectorItemControl)LoadControl(strLanguageSelectorItemControlPath);
                ucLanguageSelectorControl.AddSelectedItem(ucSelectedLanguage);
                ucSelectedLanguage.ID = "SelectedLanguageSelectorItem" + (i + 1);
                ucSelectedLanguage.Culture = objLanguageSetups[i].Culture;
                ucSelectedLanguage.UICulture = objLanguageSetups[i].UICulture;
                ucSelectedLanguage.LanguageText = objLanguageSetups[i].LanguageText;
                ucSelectedLanguage.ImageURL = objLanguageSetups[i].ImageURL;
                ucSelectedLanguage.DontShowLanguageText = true;
                ucSelectedLanguage.Selected = true;
            }
        }
    }

    private static LanguageSetup[] GetLanguageSetups()
    {
        int intNumLanguages = 0;

        if (ConfigurationManager.AppSettings["LanguageCount"] != "")
        {
            try
            {
                intNumLanguages = Convert.ToInt32(ConfigurationManager.AppSettings["LanguageCount"]);
            }

            catch
            {
                intNumLanguages = 0;
            }

        }

        List<LanguageSetup> lLanguageSetups = new List<LanguageSetup>();

        for (int i = 0; i < intNumLanguages; i++)
        {
            string strLanguageSetup = ConfigurationManager.AppSettings["Language" + (i + 1)];

            if (strLanguageSetup != "")
            {
                string[] saLanguageSetup = strLanguageSetup.Split(new char[] { ';' });

                if (saLanguageSetup.Length == 4)
                {
                    LanguageSetup objLanguageSetup = new LanguageSetup();
                    lLanguageSetups.Add(objLanguageSetup);

                    objLanguageSetup.LanguageText = saLanguageSetup[0];
                    objLanguageSetup.Culture = saLanguageSetup[1];
                    objLanguageSetup.UICulture = saLanguageSetup[2];
                    objLanguageSetup.ImageURL = saLanguageSetup[3];
                }

            }

        }

        return lLanguageSetups.ToArray();
    }

    public void LanguageSelected(object sender, EventArgs e)
    {
         Session["SelectedCulture"] = ((LanguageSelectorControl)sender).SelectedCulture;
         Session["SelectedUICulture"] = ((LanguageSelectorControl)sender).SelectedUICulture;

        Response.Redirect(Request.AppRelativeCurrentExecutionFilePath);

        return;
    }    

}
