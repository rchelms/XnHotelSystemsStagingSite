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

public partial class GuestDetailsEntryControl : System.Web.UI.UserControl
{
    public delegate void PaymentGatewayPreSelectCompletedEvent(object sender, EventArgs e);
    public event PaymentGatewayPreSelectCompletedEvent PaymentGatewayPreSelectCompleted;

    public delegate void GuestDetailsCompletedEvent(object sender, EventArgs e);
    public event GuestDetailsCompletedEvent GuestDetailsCompleted;

    private HotelDescriptiveInfo _HotelDescriptiveInfo;
    private GuestDetailsEntryInfo _GuestDetailsEntryInfo;
    private MembershipProgram[] _MembershipPrograms;
    private string[] _PaymentCardCodes;
    private bool _TermsConditionsAccepted;
    private bool _DisplayProfileGuarantee;
    private PaymentGatewayInfo[] _PaymentGatewayInfos;
    private bool _PaymentGatewayPreSelectRequired;
    private PaymentGatewayInfo _SelectedPaymentGateway;
    private PaymentCardApplication _PaymentCardApplicationStatus;
    private decimal _PaymentCardDepositAmount;
    private string _PaymentCardDepositCurrencyCode;

    public bool IsControlConfigured = false;

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

    public GuestDetailsEntryInfo GuestDetailsEntryInfo
    {
        get
        {
            return _GuestDetailsEntryInfo;
        }

        set
        {
            _GuestDetailsEntryInfo = value;
        }

    }

    public MembershipProgram[] MembershipPrograms
    {
        get
        {
            return _MembershipPrograms;
        }

        set
        {
            _MembershipPrograms = value;
        }

    }

    public string[] PaymentCardCodes
    {
        get
        {
            return _PaymentCardCodes;
        }

        set
        {
            _PaymentCardCodes = value;
        }

    }

    public bool TermsConditionsAccepted
    {
        get
        {
            return _TermsConditionsAccepted;
        }

        set
        {
            _TermsConditionsAccepted = value;
        }

    }

    public bool DisplayProfileGuarantee
    {
        get
        {
            return _DisplayProfileGuarantee;
        }

        set
        {
            _DisplayProfileGuarantee = value;
        }

    }

    public PaymentGatewayInfo[] PaymentGatewayInfos
    {
        get
        {
            return _PaymentGatewayInfos;
        }

        set
        {
            _PaymentGatewayInfos = value;
        }

    }

    public bool PaymentGatewayPreSelectRequired
    {
        get
        {
            return _PaymentGatewayPreSelectRequired;
        }

        set
        {
            _PaymentGatewayPreSelectRequired = value;
        }

    }

    public PaymentGatewayInfo SelectedPaymentGateway
    {
        get
        {
            return _SelectedPaymentGateway;
        }

        set
        {
            _SelectedPaymentGateway = value;
        }

    }

    public PaymentCardApplication PaymentCardApplicationStatus
    {
        get
        {
            return _PaymentCardApplicationStatus;
        }

        set
        {
            _PaymentCardApplicationStatus = value;
        }

    }

    public decimal PaymentCardDepositAmount
    {
        get
        {
            return _PaymentCardDepositAmount;
        }

        set
        {
            _PaymentCardDepositAmount = value;
        }

    }

    public string PaymentCardDepositCurrencyCode
    {
        get
        {
            return _PaymentCardDepositCurrencyCode;
        }

        set
        {
            _PaymentCardDepositCurrencyCode = value;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsControlConfigured)
        {
            this.ApplyControlsToPage();

            string strSelectedPaymentGatewayValue = this.Request.Form.Get(ddlPaymentGatewaySelect.ClientID.Replace('_', '$')); // form uses "name" not "id" property

            for (int i = 0; i < ddlPaymentGatewaySelect.Items.Count; i++)
            {
                if (ddlPaymentGatewaySelect.Items[i].Value == strSelectedPaymentGatewayValue)
                {
                    ddlPaymentGatewaySelect.Items[i].Selected = true;
                    break;
                }

            }

            string strSelectedAirlineProgramValue = this.Request.Form.Get(ddlAirlineProgram.ClientID.Replace('_', '$')); // form uses "name" not "id" property

            for (int i = 0; i < ddlAirlineProgram.Items.Count; i++)
            {
                if (ddlAirlineProgram.Items[i].Value == strSelectedAirlineProgramValue)
                {
                    ddlAirlineProgram.Items[i].Selected = true;
                    break;
                }

            }

            string strSelectedHotelProgramValue = this.Request.Form.Get(ddlHotelProgram.ClientID.Replace('_', '$')); // form uses "name" not "id" property

            for (int i = 0; i < ddlHotelProgram.Items.Count; i++)
            {
                if (ddlHotelProgram.Items[i].Value == strSelectedHotelProgramValue)
                {
                    ddlHotelProgram.Items[i].Selected = true;
                    break;
                }

            }

            string strSelectedCardTypeValue = this.Request.Form.Get(ddlCardType.ClientID.Replace('_', '$')); // form uses "name" not "id" property

            for (int i = 0; i < ddlCardType.Items.Count; i++)
            {
                if (ddlCardType.Items[i].Value == strSelectedCardTypeValue)
                {
                    ddlCardType.Items[i].Selected = true;
                    break;
                }

            }

            string strSelectedCardExpiryYearValue = this.Request.Form.Get(ddlCardExpiryYear.ClientID.Replace('_', '$')); // form uses "name" not "id" property

            for (int i = 0; i < ddlCardExpiryYear.Items.Count; i++)
            {
                if (ddlCardExpiryYear.Items[i].Value == strSelectedCardExpiryYearValue)
                {
                    ddlCardExpiryYear.Items[i].Selected = true;
                    break;
                }

            }

            string strSelectedCardStartYearValue = this.Request.Form.Get(ddlCardStartYear.ClientID.Replace('_', '$')); // form uses "name" not "id" property

            for (int i = 0; i < ddlCardStartYear.Items.Count; i++)
            {
                if (ddlCardStartYear.Items[i].Value == strSelectedCardStartYearValue)
                {
                    ddlCardStartYear.Items[i].Selected = true;
                    break;
                }
            }
        }

        btnConfirmBooking.OnClientClick = "showWaitingPage()";

        return;
    }

    public void RenderUserControl()
    {
        string strExpiryYear = "";
        string strStartYear = "";

        this.ApplyControlsToPage();

        if (_PaymentGatewayPreSelectRequired)
        {
            panPaymentGatewayPreSelect.Visible = true;
            panGuestDetails.Visible = false;

            lblPaymentGatewaySelectInstructions.Text = ((XnGR_WBS_Page)this.Page).PaymentGatewaySelectionNotice(_PaymentCardDepositAmount, _PaymentCardDepositCurrencyCode);
        }

        else
        {
            panPaymentGatewayPreSelect.Visible = false;
            panGuestDetails.Visible = true;

            ddlNamePrefix.Text = _GuestDetailsEntryInfo.NamePrefix;
            tbFirstName.Text = _GuestDetailsEntryInfo.FirstName;
            tbLastName.Text = _GuestDetailsEntryInfo.LastName;

            tbConfirmEmail.Text = _GuestDetailsEntryInfo.EmailConfirmEntry;
            tbEmail.Text = _GuestDetailsEntryInfo.Email;

            tbTelephone.Text = _GuestDetailsEntryInfo.Phone;
            tbAddress1.Text = _GuestDetailsEntryInfo.Address1;
            tbAddress2.Text = _GuestDetailsEntryInfo.Address2;
            tbCity.Text = _GuestDetailsEntryInfo.City;
            tbStateRegion.Text = _GuestDetailsEntryInfo.StateRegion;
            tbPostalCode.Text = _GuestDetailsEntryInfo.PostalCode;

            for (int i = 0; i < ddlCountry.Items.Count; i++)
            {
                if (ddlCountry.Items[i].Value == _GuestDetailsEntryInfo.Country)
                {
                    ddlCountry.SelectedIndex = i;
                    break;
                }

            }

            tbCompanyName.Text = _GuestDetailsEntryInfo.CompanyName;
            tbTravelAgencyIATA.Text = _GuestDetailsEntryInfo.TravelAgencyIATA;

            for (int i = 0; i < ddlTravelPurpose.Items.Count; i++)
            {
                if ((TravelPurpose)Convert.ToInt32(ddlTravelPurpose.Items[i].Value) == _GuestDetailsEntryInfo.TravelPurpose)
                {
                    ddlTravelPurpose.SelectedIndex = i;
                    break;
                }

            }

            for (int i = 0; i < ddlAirlineProgram.Items.Count; i++)
            {
                if (ddlAirlineProgram.Items[i].Value == _GuestDetailsEntryInfo.AirlineProgramCode)
                {
                    ddlAirlineProgram.SelectedIndex = i;
                    break;
                }

            }

            for (int i = 0; i < ddlHotelProgram.Items.Count; i++)
            {
                if (ddlHotelProgram.Items[i].Value == _GuestDetailsEntryInfo.HotelProgramCode)
                {
                    ddlHotelProgram.SelectedIndex = i;
                    break;
                }

            }

            tbFltNumber.Text = _GuestDetailsEntryInfo.FlightNumber;

            if (_GuestDetailsEntryInfo.ArrivalTime != null && _GuestDetailsEntryInfo.ArrivalTime != "")
            {
                DateTime dtArrivalTime;

                if (DateTime.TryParse(_GuestDetailsEntryInfo.ArrivalTime, out dtArrivalTime))
                {
                    if (dtArrivalTime.Hour == 0)
                    {
                        tbArrivalTimeHours.Text = "12";
                        tbArrivalTimeMinutes.Text = dtArrivalTime.Minute.ToString("0#");
                        rbArrivalTimeAM.Checked = true;
                        rbArrivalTimePM.Checked = false;
                    }

                    else if (dtArrivalTime.Hour < 12)
                    {
                        tbArrivalTimeHours.Text = dtArrivalTime.Hour.ToString("0#");
                        tbArrivalTimeMinutes.Text = dtArrivalTime.Minute.ToString("0#");
                        rbArrivalTimeAM.Checked = true;
                        rbArrivalTimePM.Checked = false;
                    }

                    else if (dtArrivalTime.Hour == 12)
                    {
                        tbArrivalTimeHours.Text = "12";
                        tbArrivalTimeMinutes.Text = dtArrivalTime.Minute.ToString("0#");
                        rbArrivalTimeAM.Checked = false;
                        rbArrivalTimePM.Checked = true;
                    }

                    else
                    {
                        tbArrivalTimeHours.Text = ((int)(dtArrivalTime.Hour - 12)).ToString("0#");
                        tbArrivalTimeMinutes.Text = dtArrivalTime.Minute.ToString("0#");
                        rbArrivalTimeAM.Checked = false;
                        rbArrivalTimePM.Checked = true;
                    }

                }

            }

            tbCardholderName.Text = _GuestDetailsEntryInfo.PaymentCardHolder;
            tbCardIssueNumber.Text = _GuestDetailsEntryInfo.PaymentCardIssueNumber;
            tbCardNumber.Text = _GuestDetailsEntryInfo.PaymentCardNumber;
            tbCardSecurityCode.Text = _GuestDetailsEntryInfo.PaymentCardSecurityCode;

            cbProfileGuarantee.Checked = _GuestDetailsEntryInfo.ProfileGuaranteeRequested;

            tbSpecialInstructions.Text = _GuestDetailsEntryInfo.SpecialInstructions;
            cbSubscribeToNewsletter.Checked = _GuestDetailsEntryInfo.SubscribeToNewsletter;

            ddlAdultRollaway.SelectedIndex = _GuestDetailsEntryInfo.NumberRollawaysAdult;
            ddlChildRollaway.SelectedIndex = _GuestDetailsEntryInfo.NumberRollawaysChild;
            ddlCrib.SelectedIndex = _GuestDetailsEntryInfo.NumberCribs;

            PaymentGatewayMode enumPaymentGatewayMode = PaymentGatewayMode.MerchantSiteCapturesCardDetails;

            if (_SelectedPaymentGateway != null)
                enumPaymentGatewayMode = _SelectedPaymentGateway.Mode;

            lblPaymentCardApplication.Text = ((XnGR_WBS_Page)this.Page).PaymentCardApplicationNotice(_PaymentCardApplicationStatus
                , enumPaymentGatewayMode
                , _PaymentCardDepositAmount
                , string.Format("<span class=\"mm_rate_plan_currency_symbol\">{0}</span>", WebconfigHelper.GetCurrencyCodeString(_PaymentCardDepositCurrencyCode)));

            GeneralPaymentPolicy objGeneralPaymentPolicy = ((XnGR_WBS_Page)this.Page).WbsUiHelper.GetGeneralPaymentPolicyInfo();

            if (_PaymentCardApplicationStatus == PaymentCardApplication.GuaranteeOnly)
            {
                lblPaymentPolicyText.Text = objGeneralPaymentPolicy.Guarantee;
            }

            else if (_PaymentCardApplicationStatus == PaymentCardApplication.DepositOnly)
            {
                lblPaymentPolicyText.Text = objGeneralPaymentPolicy.Prepay;
            }

            else if (_PaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit)
            {
                lblPaymentPolicyText.Text = objGeneralPaymentPolicy.Mixed;
            }

            if (lblPaymentPolicyText.Text != null && lblPaymentPolicyText.Text != "")
                panPaymentPolicy.Visible = true;
            else
                panPaymentPolicy.Visible = false;

            if (_SelectedPaymentGateway != null && _SelectedPaymentGateway.Mode == PaymentGatewayMode.PaymentGatewayCapturesCardDetails && (_PaymentCardApplicationStatus == PaymentCardApplication.DepositOnly || _PaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit))
            {
                lblPaymentGatewayDepositNotice.Text = ((XnGR_WBS_Page)this.Page).PaymentCardApplicationNotice(_PaymentCardApplicationStatus, PaymentGatewayMode.PaymentGatewayCapturesCardDetails, _PaymentCardDepositAmount, _PaymentCardDepositCurrencyCode);
                panPaymentGatewayDepositNotice.Visible = true;
            }

            else
            {
                panPaymentGatewayDepositNotice.Visible = false;
            }

            for (int i = 0; i < _PaymentCardCodes.Length; i++)
            {
                if (_PaymentCardCodes[i] == _GuestDetailsEntryInfo.PaymentCardType)
                {
                    ddlCardType.SelectedIndex = i;
                }

            }

            if (_GuestDetailsEntryInfo.PaymentCardExpireDate != "")
            {
                ddlCardExpiryMonth.SelectedIndex = Convert.ToInt32(_GuestDetailsEntryInfo.PaymentCardExpireDate.Substring(0, 2)) - 1;
                strExpiryYear = _GuestDetailsEntryInfo.PaymentCardExpireDate.Substring(2);
            }

            for (int i = DateTime.Now.Year; i <= DateTime.Now.Year + 9; i++)
            {
                if (i.ToString().Substring(2) == strExpiryYear)
                {
                    ddlCardExpiryYear.SelectedIndex = i - DateTime.Now.Year;
                    break;
                }

            }

            if (_GuestDetailsEntryInfo.PaymentCardEffectiveDate != "")
            {
                ddlCardStartMonth.SelectedIndex = Convert.ToInt32(_GuestDetailsEntryInfo.PaymentCardEffectiveDate.Substring(0, 2)) - 1;
                strStartYear = _GuestDetailsEntryInfo.PaymentCardEffectiveDate.Substring(2);
            }

            for (int i = DateTime.Now.Year - 9; i <= DateTime.Now.Year; i++)
            {
                if (i.ToString().Substring(2) == strStartYear)
                {
                    ddlCardStartYear.SelectedIndex = i - (DateTime.Now.Year - 9);
                    break;
                }

            }

            phPaymentCardLogos.Controls.Clear();

            string strPaymentCardLogoControlPath = ConfigurationManager.AppSettings["PaymentCardLogoControl.ascx"];
            PaymentCardLogoControl ucPaymentCardLogoControl = (PaymentCardLogoControl)LoadControl(strPaymentCardLogoControlPath);
            phPaymentCardLogos.Controls.Add(ucPaymentCardLogoControl);

            ucPaymentCardLogoControl.PaymentCardCodes = _PaymentCardCodes;
            ucPaymentCardLogoControl.RenderUserControl();

            if (_SelectedPaymentGateway != null && _SelectedPaymentGateway.Mode == PaymentGatewayMode.PaymentGatewayCapturesCardDetails && (_PaymentCardApplicationStatus == PaymentCardApplication.DepositOnly || _PaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit))
                panCardDetails.Visible = false;
            else
                panCardDetails.Visible = true;

            if (_PaymentCardCodes.Length != 0)
                panPaymentCardLogos.Visible = true;
            else
                panPaymentCardLogos.Visible = false;

            if (_DisplayProfileGuarantee)
                panProfileGuarantee.Visible = true;
            else
                panProfileGuarantee.Visible = false;

            if (_PaymentCardApplicationStatus == PaymentCardApplication.DepositOnly || _PaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit)
            {
                if (_SelectedPaymentGateway != null && _SelectedPaymentGateway.Mode == PaymentGatewayMode.MerchantSiteCapturesCardDetails)
                    panPaymentGatewayNotice.Visible = true;
                else if (_SelectedPaymentGateway == null && _PaymentGatewayInfos != null && _PaymentGatewayInfos.Length > 1)
                    panPaymentGatewayNotice.Visible = true;
                else
                    panPaymentGatewayNotice.Visible = false;
            }

            else
            {
                panPaymentGatewayNotice.Visible = false;
            }

            cbTermsAgreement.Checked = _TermsConditionsAccepted;
        }

        return;
    }

    protected void btnPaymentGatewaySelect_Click(object sender, EventArgs e)
    {
        PaymentGatewayInfo objPaymentGatewayInfo = null;

        for (int i = 0; i < _PaymentGatewayInfos.Length; i++)
        {
            if (_PaymentGatewayInfos[i].Type.ToString() == ddlPaymentGatewaySelect.SelectedValue)
            {
                objPaymentGatewayInfo = _PaymentGatewayInfos[i];
                break;
            }

        }

        if (objPaymentGatewayInfo != null)
        {
            _SelectedPaymentGateway = objPaymentGatewayInfo;
            PaymentGatewayPreSelectCompleted(this, new EventArgs());
        }

        return;
    }

    protected void btnConfirmBooking_Click(object sender, EventArgs e)
    {
        GuestDetailsEntryInfo objGuestDetailsEntryInfo = new GuestDetailsEntryInfo();

        objGuestDetailsEntryInfo.NamePrefix = ddlNamePrefix.Text.Trim();
        objGuestDetailsEntryInfo.FirstName = tbFirstName.Text.Trim();
        objGuestDetailsEntryInfo.LastName = tbLastName.Text.Trim();
        objGuestDetailsEntryInfo.Email = tbEmail.Text.Trim();
        objGuestDetailsEntryInfo.EmailConfirmEntry = tbConfirmEmail.Text.Trim();

        objGuestDetailsEntryInfo.Phone = tbTelephone.Text.Trim();
        objGuestDetailsEntryInfo.Fax = "";
        objGuestDetailsEntryInfo.Address1 = tbAddress1.Text.Trim();
        objGuestDetailsEntryInfo.Address2 = tbAddress2.Text.Trim();
        objGuestDetailsEntryInfo.StateRegion = tbStateRegion.Text.Trim();
        objGuestDetailsEntryInfo.City = tbCity.Text.Trim();
        objGuestDetailsEntryInfo.PostalCode = tbPostalCode.Text.Trim();

        objGuestDetailsEntryInfo.SpecialInstructions = tbSpecialInstructions.Text.Trim();
        objGuestDetailsEntryInfo.CompanyName = tbCompanyName.Text.Trim();
        objGuestDetailsEntryInfo.TravelAgencyIATA = tbTravelAgencyIATA.Text.Trim();
        objGuestDetailsEntryInfo.Country = ddlCountry.SelectedValue.Trim();
        objGuestDetailsEntryInfo.SubscribeToNewsletter = cbSubscribeToNewsletter.Checked;
        objGuestDetailsEntryInfo.TravelPurpose = (TravelPurpose)Convert.ToInt32(ddlTravelPurpose.SelectedValue);
        objGuestDetailsEntryInfo.FlightNumber = tbFltNumber.Text.Trim();

        objGuestDetailsEntryInfo.AirlineProgramCode = ddlAirlineProgram.SelectedValue;
        objGuestDetailsEntryInfo.AirlineProgramIdentifier = tbAirlineProgramNumber.Text.Trim();
        objGuestDetailsEntryInfo.HotelProgramCode = ddlHotelProgram.SelectedValue;
        objGuestDetailsEntryInfo.HotelProgramIdentifier = tbHotelProgramNumber.Text.Trim();

        if ((tbArrivalTimeHours.Text != null && tbArrivalTimeHours.Text != "") && (tbArrivalTimeMinutes.Text != null && tbArrivalTimeMinutes.Text != ""))
        {
            bool bReturnTimeForValidationCheck = true;

            int intHours;

            if (Int32.TryParse(tbArrivalTimeHours.Text, out intHours))
            {
                if (intHours == 0 && rbArrivalTimePM.Checked) // Microsoft DateTime parser permits this entry (00:15pm accepted as 12:15pm)
                {
                    objGuestDetailsEntryInfo.ArrivalTime = "error";
                    bReturnTimeForValidationCheck = false;
                }

            }

            if (bReturnTimeForValidationCheck)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(tbArrivalTimeHours.Text);
                sb.Append(":");
                sb.Append(tbArrivalTimeMinutes.Text);

                if (rbArrivalTimeAM.Checked)
                    sb.Append(" AM");
                else
                    sb.Append(" PM");

                objGuestDetailsEntryInfo.ArrivalTime = sb.ToString();
            }

        }

        else if ((tbArrivalTimeHours.Text != null && tbArrivalTimeHours.Text != "") || (tbArrivalTimeMinutes.Text != null && tbArrivalTimeMinutes.Text != ""))
        {
            objGuestDetailsEntryInfo.ArrivalTime = "error";
        }

        else
        {
            objGuestDetailsEntryInfo.ArrivalTime = "";
        }

        objGuestDetailsEntryInfo.NumberRollawaysAdult = Convert.ToInt32(ddlAdultRollaway.SelectedValue);
        objGuestDetailsEntryInfo.NumberRollawaysChild = Convert.ToInt32(ddlChildRollaway.SelectedValue);
        objGuestDetailsEntryInfo.NumberCribs = Convert.ToInt32(ddlCrib.SelectedValue);

        objGuestDetailsEntryInfo.PaymentCardHolder = tbCardholderName.Text.Trim();
        objGuestDetailsEntryInfo.PaymentCardIssueNumber = tbCardIssueNumber.Text.Trim();
        objGuestDetailsEntryInfo.PaymentCardNumber = tbCardNumber.Text.Replace(" ", "").Trim();
        objGuestDetailsEntryInfo.PaymentCardSecurityCode = tbCardSecurityCode.Text.Trim();
        objGuestDetailsEntryInfo.PaymentCardType = ddlCardType.SelectedValue.Trim();
        objGuestDetailsEntryInfo.PaymentCardExpireDate = ddlCardExpiryMonth.SelectedValue.PadLeft(2, '0') + ddlCardExpiryYear.SelectedValue.Substring(2);
        objGuestDetailsEntryInfo.PaymentCardEffectiveDate = ddlCardStartMonth.SelectedValue.PadLeft(2, '0') + ddlCardStartYear.SelectedValue.Substring(2);

        objGuestDetailsEntryInfo.ProfileGuaranteeRequested = cbProfileGuarantee.Checked;

        _GuestDetailsEntryInfo = objGuestDetailsEntryInfo;

        _TermsConditionsAccepted = cbTermsAgreement.Checked;

        if (_SelectedPaymentGateway == null && (_PaymentCardApplicationStatus == PaymentCardApplication.DepositOnly || PaymentCardApplicationStatus == PaymentCardApplication.GuaranteeAndDeposit))
        {
            _SelectedPaymentGateway = WBSPGHelper.GetPaymentGatewayByCardType(_PaymentGatewayInfos, ddlCardType.SelectedValue.Trim());
        }

        if (_SelectedPaymentGateway != null && _SelectedPaymentGateway.Mode == PaymentGatewayMode.PaymentGatewayCapturesCardDetails)
        {
            objGuestDetailsEntryInfo.PaymentCardType = "XX";
        }

        GuestDetailsCompleted(this, new EventArgs());

        return;
    }

    private void ApplyControlsToPage()
    {
        if (_PaymentGatewayPreSelectRequired)
        {
            ddlPaymentGatewaySelect.Items.Clear();

            if (_PaymentGatewayInfos != null)
            {
                for (int i = 0; i < _PaymentGatewayInfos.Length; i++)
                {
                    StringBuilder sbPaymentGatewayCardTypes = new StringBuilder();

                    bool bInsertSeparator = false;

                    for (int j = 0; j < _PaymentGatewayInfos[i].AcceptedCardTypes.Length; j++)
                    {
                        if (bInsertSeparator)
                            sbPaymentGatewayCardTypes.Append(", ");

                        sbPaymentGatewayCardTypes.Append((String)GetGlobalResourceObject("SiteResources", "CardType" + _PaymentGatewayInfos[i].AcceptedCardTypes[j]));

                        bInsertSeparator = true;
                    }

                    ddlPaymentGatewaySelect.Items.Add(new ListItem(sbPaymentGatewayCardTypes.ToString(), _PaymentGatewayInfos[i].Type.ToString()));
                }

            }

        }

        else
        {
            ddlAirlineProgram.Items.Clear();
            ddlHotelProgram.Items.Clear();

            panAirlineProgram.Visible = false;
            panHotelProgram.Visible = false;

            if (this.IsAirlineProgram())
            {
                List<ListItem> lAirlineProgramListItems = new List<ListItem>();

                for (int i = 0; i < _MembershipPrograms.Length; i++)
                {
                    if (_MembershipPrograms[i].ProgramType != MembershipProgramType.Air)
                        continue;

                    ListItem liMembershipProgram = new ListItem();
                    lAirlineProgramListItems.Add(liMembershipProgram);

                    liMembershipProgram.Value = _MembershipPrograms[i].ProgramCode;
                    liMembershipProgram.Text = _MembershipPrograms[i].ProgramName;
                }

                ddlAirlineProgram.Items.Add(new ListItem(GetLocalResourceObject("ddlSelectAirlineProgram.Text").ToString(), ""));

                for (int i = 0; i < lAirlineProgramListItems.Count; i++)
                    ddlAirlineProgram.Items.Add(lAirlineProgramListItems[i]);

                panAirlineProgram.Visible = true;
            }

            if (this.IsHotelProgram())
            {
                List<ListItem> lHotelProgramListItems = new List<ListItem>();

                for (int i = 0; i < _MembershipPrograms.Length; i++)
                {
                    if (_MembershipPrograms[i].ProgramType != MembershipProgramType.Hotel)
                        continue;

                    ListItem liMembershipProgram = new ListItem();
                    lHotelProgramListItems.Add(liMembershipProgram);

                    liMembershipProgram.Value = _MembershipPrograms[i].ProgramCode;
                    liMembershipProgram.Text = _MembershipPrograms[i].ProgramName;
                }

                ddlHotelProgram.Items.Add(new ListItem(GetLocalResourceObject("ddlSelectHotelProgram.Text").ToString(), ""));

                for (int i = 0; i < lHotelProgramListItems.Count; i++)
                    ddlHotelProgram.Items.Add(lHotelProgramListItems[i]);

                panHotelProgram.Visible = true;
            }

            ddlCardType.Controls.Clear();
            ddlCardType.Attributes.Add("onchange", "ShowMSInfos();");

            ddlCardType.Items.Clear();

            for (int i = 0; i < _PaymentCardCodes.Length; i++)
            {
                ddlCardType.Items.Add(new ListItem((String)GetGlobalResourceObject("SiteResources", "CardType" + _PaymentCardCodes[i]), _PaymentCardCodes[i]));
            }

            ddlCardExpiryYear.Items.Clear();

            for (int i = DateTime.Now.Year; i <= DateTime.Now.Year + 9; i++)
            {
                ddlCardExpiryYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ddlCardStartYear.Items.Clear();

            for (int i = DateTime.Now.Year - 9; i <= DateTime.Now.Year; i++)
            {
                ddlCardStartYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

        }

        return;
    }

    private bool IsAirlineProgram()
    {
        for (int i = 0; i < _MembershipPrograms.Length; i++)
        {
            if (_MembershipPrograms[i].ProgramType == MembershipProgramType.Air)
                return true;
        }

        return false;
    }

    private bool IsHotelProgram()
    {
        for (int i = 0; i < _MembershipPrograms.Length; i++)
        {
            if (_MembershipPrograms[i].ProgramType == MembershipProgramType.Hotel)
                return true;
        }

        return false;
    }

    private bool IsParentPreRender()
    {
        return ((XnGR_WBS_Page)this.Page).IsParentPreRender;
    }

}
