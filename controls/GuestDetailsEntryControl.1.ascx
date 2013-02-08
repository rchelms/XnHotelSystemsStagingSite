<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GuestDetailsEntryControl.1.ascx.cs"
    Inherits="GuestDetailsEntryControl" %>
<%@ Reference Control="~/controls/PaymentCardLogoControl.1.ascx" %>
<%@ Register TagPrefix="xngr" TagName="HelpInfo" Src="~/controls/HelpDisplayControl.1.ascx" %>
<script language="javascript" type="text/javascript">

    function ShowMSInfos() {

        if (document.getElementById('<%= ddlCardType.ClientID %>') == null)
            return;

        var selectCardType = document.getElementById('<%= ddlCardType.ClientID %>').value;
        var panCardStartDate = document.getElementById('<%= panCardStartDate.ClientID %>');
        var panCardIssueNumber = document.getElementById('<%= panCardIssueNumber.ClientID %>');

        if (selectCardType == 'MS') {
            panCardStartDate.style.display = "";
            panCardIssueNumber.style.display = "";
        }
        else {
            panCardStartDate.style.display = "none";
            panCardIssueNumber.style.display = "none";
        }
        return;
    }

</script>
<asp:Panel ID="panGuestDetailsEntry" CssClass="content_section" runat="server">
    <a id="GuestDetailInfoForm" href="#" runat="server" clientidmode="Static"></a>
    <asp:Panel ID="panPaymentGatewayPreSelect" CssClass="pane_body" runat="server">
        <asp:Panel CssClass="guest_details_info" runat="server">
            <asp:Panel CssClass="guest_details_info_header" runat="server">
                <asp:Label ID="lblPaymentGatewaySelect" runat="server" Text="" meta:resourcekey="lblPaymentGatewaySelect" />
            </asp:Panel>
            <asp:Panel CssClass="guest_details_info_body" runat="server">
                <p>
                    <asp:Label ID="lblPaymentGatewaySelectInstructions" runat="server" Text="" />
                </p>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label_inline" runat="server">
                            <asp:Label ID="lblPaymentGatewaySelectCardType" runat="server" Text="" meta:resourcekey="lblPaymentGatewaySelectCardType" />
                        </asp:Panel>
                        <asp:Panel CssClass="mm_field_set_user_entry_inline" runat="server">
                            <asp:DropDownList ID="ddlPaymentGatewaySelect" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="field_set_button" runat="server">
                    <asp:Panel CssClass="std_button" runat="server">
                        <asp:Panel CssClass="std_button_left" runat="server">
                            <asp:Panel CssClass="std_button_right" runat="server">
                                <asp:Panel CssClass="std_button_center" runat="server">
                                    <asp:Button ID="btnPaymentGatewaySelect" CssClass="std_button_control" runat="server"
                                        Text="" meta:resourcekey="btnPaymentGatewaySelect" OnClick="btnPaymentGatewaySelect_Click" />
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="panGuestDetails" runat="server">
        <%-- <h3>
            <asp:Label ID="lblGuestDetailsEntry" runat="server" Text="" meta:resourcekey="lblGuestDetailsEntry" />
        </h3>
        <p>
            <asp:Label ID="lblGuestDetailsEntryInfo" runat="server" Text="" meta:resourcekey="lblGuestDetailsEntryInfo" />
        </p>--%>
        <asp:Panel ID="panGuestInformation" CssClass="mm_guest_details_info" runat="server">
            <asp:Panel CssClass="mm_guest_details_info_header" runat="server">
                <asp:Label ID="lblGuestInformation" runat="server" Text="" meta:resourcekey="lblGuestInformation" />
            </asp:Panel>
            <asp:Panel CssClass="mm_guest_details_info_body" runat="server">
                <p>
                    <asp:Label ID="lblRequiredEntryText" runat="server" Text="" meta:resourcekey="lblRequiredEntryText" />
                </p>
                <br />
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblNamePrefix" runat="server" Text="" meta:resourcekey="lblNamePrefix" />
                            <asp:Label runat="server" Text="&nbsp;" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <a name="TopOfInfoForm" />
                            <asp:DropDownList ID="ddlNamePrefix" runat="server">
                                <asp:ListItem Value="" Text="" meta:resourcekey="ddlNamePrefixMr" />
                                <asp:ListItem Value="" Text="" meta:resourcekey="ddlNamePrefixMrs" />
                                <asp:ListItem Value="" Text="" meta:resourcekey="ddlNamePrefixMs" />
                                <asp:ListItem Value="" Text="" meta:resourcekey="ddlNamePrefixMiss" />
                                <asp:ListItem Value="" Text="" meta:resourcekey="ddlNamePrefixDr" />
                                <asp:ListItem Value="" Text="" meta:resourcekey="ddlNamePrefixProf" />
                            </asp:DropDownList>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblLastName" runat="server" Text="" meta:resourcekey="lblLastName" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbLastName" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblFirstName" runat="server" Text="" meta:resourcekey="lblFirstName" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel19" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbFirstName" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblEmail" runat="server" Text="" meta:resourcekey="lblEmail" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel3" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbEmail" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="Panel6" CssClass="mm_field_set_row" runat="server">
                    <asp:Panel ID="Panel4" CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblConfirmEmail" runat="server" Text="" meta:resourcekey="lblConfirmEmail" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel5" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbConfirmEmail" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblAddress1" runat="server" Text="" meta:resourcekey="lblAddress1" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel7" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbAddress1" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="Panel8" CssClass="mm_field_set_row" runat="server">
                    <asp:Panel ID="Panel10" CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblAddress2" runat="server" Text="" meta:resourcekey="lblAddress2" />
                            <asp:Label runat="server" Text="&nbsp;" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel9" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbAddress2" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="Panel12" CssClass="mm_field_set_row" runat="server" Visible="false">
                    <asp:Panel ID="Panel13" CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblStateRegion" runat="server" Text="" meta:resourcekey="lblStateRegion" />
                            <asp:Label runat="server" Text="&nbsp;" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel14" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbStateRegion" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row1" runat="server">
                    <asp:Panel CssClass="mm_field_set_left1" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblPostalCode" CssClass="label" runat="server" Text="" meta:resourcekey="lblPostalCode" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right1" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry1" runat="server">
                            <asp:TextBox ID="tbPostalCode" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row2" runat="server">
                    <asp:Panel CssClass="mm_field_set_left2" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCity" runat="server" Text="" meta:resourcekey="lblCity" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right2" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry2" runat="server">
                            <asp:TextBox ID="tbCity" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCountry" runat="server" Text="" meta:resourcekey="lblCountry" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel15" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:DropDownList ID="ddlCountry" runat="server">
                                <asp:ListItem Value="" Text="" meta:resourcekey="ddlCountrySelect" />
                                <asp:ListItem Value="AF" Text="Afghanistan" />
                                <asp:ListItem Value="AL" Text="Albania" />
                                <asp:ListItem Value="DZ" Text="Algeria" />
                                <asp:ListItem Value="AS" Text="American Samoa" />
                                <asp:ListItem Value="AD" Text="Andorra" />
                                <asp:ListItem Value="AO" Text="Angola" />
                                <asp:ListItem Value="AI" Text="Anguilla" />
                                <asp:ListItem Value="AQ" Text="Antarctica" />
                                <asp:ListItem Value="AG" Text="Antigua And Barbuda" />
                                <asp:ListItem Value="AR" Text="Argentina" />
                                <asp:ListItem Value="AM" Text="Armenia" />
                                <asp:ListItem Value="AW" Text="Aruba" />
                                <asp:ListItem Value="AU" Text="Australia" />
                                <asp:ListItem Value="AT" Text="Austria" />
                                <asp:ListItem Value="AZ" Text="Azerbaijan" />
                                <asp:ListItem Value="BS" Text="Bahamas" />
                                <asp:ListItem Value="BH" Text="Bahrain" />
                                <asp:ListItem Value="BD" Text="Bangladesh" />
                                <asp:ListItem Value="BB" Text="Barbados" />
                                <asp:ListItem Value="BY" Text="Belarus" />
                                <asp:ListItem Value="BE" Text="Belgium" />
                                <asp:ListItem Value="BZ" Text="Belize" />
                                <asp:ListItem Value="BJ" Text="Benin" />
                                <asp:ListItem Value="BM" Text="Bermuda" />
                                <asp:ListItem Value="BT" Text="Bhutan" />
                                <asp:ListItem Value="BO" Text="Bolivia" />
                                <asp:ListItem Value="BA" Text="Bosnia And Herzegowina" />
                                <asp:ListItem Value="BW" Text="Botswana" />
                                <asp:ListItem Value="BV" Text="Bouvet Island" />
                                <asp:ListItem Value="BR" Text="Brazil" />
                                <asp:ListItem Value="IO" Text="British Indian Ocean Territory" />
                                <asp:ListItem Value="BN" Text="Brunei Darussalam" />
                                <asp:ListItem Value="BG" Text="Bulgaria" />
                                <asp:ListItem Value="BF" Text="Burkina Faso" />
                                <asp:ListItem Value="BI" Text="Burundi" />
                                <asp:ListItem Value="KH" Text="Cambodia" />
                                <asp:ListItem Value="CM" Text="Cameroon" />
                                <asp:ListItem Value="CA" Text="Canada" />
                                <asp:ListItem Value="CV" Text="Cape Verde" />
                                <asp:ListItem Value="KY" Text="Cayman Islands" />
                                <asp:ListItem Value="CF" Text="Central African Republic" />
                                <asp:ListItem Value="TD" Text="Chad" />
                                <asp:ListItem Value="JG" Text="Channel Islands" />
                                <asp:ListItem Value="CL" Text="Chile" />
                                <asp:ListItem Value="CN" Text="China" />
                                <asp:ListItem Value="CX" Text="Christmas Island" />
                                <asp:ListItem Value="CC" Text="Cocos (Keeling) Islands" />
                                <asp:ListItem Value="CO" Text="Colombia" />
                                <asp:ListItem Value="KM" Text="Comoros" />
                                <asp:ListItem Value="CG" Text="Congo" />
                                <asp:ListItem Value="CK" Text="Cook Islands" />
                                <asp:ListItem Value="CR" Text="Costa Rica" />
                                <asp:ListItem Value="CI" Text="Cote D'ivoire" />
                                <asp:ListItem Value="HR" Text="Croatia (local Name: Hrvatska)" />
                                <asp:ListItem Value="CU" Text="Cuba" />
                                <asp:ListItem Value="CY" Text="Cyprus" />
                                <asp:ListItem Value="CZ" Text="Czech Republic" />
                                <asp:ListItem Value="DK" Text="Denmark" />
                                <asp:ListItem Value="DJ" Text="Djibouti" />
                                <asp:ListItem Value="DM" Text="Dominica" />
                                <asp:ListItem Value="DO" Text="Dominican Republic" />
                                <asp:ListItem Value="TP" Text="East Timor" />
                                <asp:ListItem Value="EC" Text="Ecuador" />
                                <asp:ListItem Value="EG" Text="Egypt" />
                                <asp:ListItem Value="SV" Text="El Salvador" />
                                <asp:ListItem Value="GQ" Text="Equatorial Guinea" />
                                <asp:ListItem Value="ER" Text="Eritrea" />
                                <asp:ListItem Value="EE" Text="Estonia" />
                                <asp:ListItem Value="ET" Text="Ethiopia" />
                                <asp:ListItem Value="FK" Text="Falkland Islands" />
                                <asp:ListItem Value="FO" Text="Faroe Islands" />
                                <asp:ListItem Value="FJ" Text="Fiji" />
                                <asp:ListItem Value="FI" Text="Finland" />
                                <asp:ListItem Value="FR" Text="France" />
                                <asp:ListItem Value="GF" Text="French Guiana" />
                                <asp:ListItem Value="PF" Text="French Polynesia" />
                                <asp:ListItem Value="TF" Text="French Southern Territories" />
                                <asp:ListItem Value="GA" Text="Gabon" />
                                <asp:ListItem Value="GM" Text="Gambia" />
                                <asp:ListItem Value="GE" Text="Georgia" />
                                <asp:ListItem Value="DE" Text="Germany" />
                                <asp:ListItem Value="GH" Text="Ghana" />
                                <asp:ListItem Value="GI" Text="Gibraltar" />
                                <asp:ListItem Value="GR" Text="Greece" />
                                <asp:ListItem Value="GL" Text="Greenland" />
                                <asp:ListItem Value="GD" Text="Grenada" />
                                <asp:ListItem Value="GP" Text="Guadeloupe" />
                                <asp:ListItem Value="GU" Text="Guam" />
                                <asp:ListItem Value="GT" Text="Guatemala" />
                                <asp:ListItem Value="GN" Text="Guinea" />
                                <asp:ListItem Value="GW" Text="Guinea-bissau" />
                                <asp:ListItem Value="GY" Text="Guyana" />
                                <asp:ListItem Value="HT" Text="Haiti" />
                                <asp:ListItem Value="HM" Text="Heard And Mc Donald Islands" />
                                <asp:ListItem Value="HN" Text="Honduras" />
                                <asp:ListItem Value="HK" Text="Hong Kong" />
                                <asp:ListItem Value="HU" Text="Hungary" />
                                <asp:ListItem Value="IS" Text="Iceland" />
                                <asp:ListItem Value="IN" Text="India" />
                                <asp:ListItem Value="IR" Text="Iran (Islamic Republic Of)" />
                                <asp:ListItem Value="IQ" Text="Iraq" />
                                <asp:ListItem Value="IE" Text="Ireland (Republic of)" />
                                <asp:ListItem Value="ID" Text="indonesia" />
                                <asp:ListItem Value="IL" Text="Israel" />
                                <asp:ListItem Value="IT" Text="Italy" />
                                <asp:ListItem Value="JM" Text="Jamaica" />
                                <asp:ListItem Value="JP" Text="Japan" />
                                <asp:ListItem Value="JO" Text="Jordan" />
                                <asp:ListItem Value="KZ" Text="Kazakhstan" />
                                <asp:ListItem Value="KE" Text="Kenya" />
                                <asp:ListItem Value="KI" Text="Kiribati" />
                                <asp:ListItem Value="KP" Text="Korea, Democratic People's Republic Of" />
                                <asp:ListItem Value="KR" Text="Korea, Republic Of" />
                                <asp:ListItem Value="KW" Text="Kuwait" />
                                <asp:ListItem Value="KG" Text="Kyrgyzstan" />
                                <asp:ListItem Value="LA" Text="Lao People's Democratic Republic" />
                                <asp:ListItem Value="LV" Text="Latvia" />
                                <asp:ListItem Value="LB" Text="Lebanon" />
                                <asp:ListItem Value="LS" Text="Lesotho" />
                                <asp:ListItem Value="LR" Text="Liberia" />
                                <asp:ListItem Value="LY" Text="Libyan Arab Jamahiriya" />
                                <asp:ListItem Value="LI" Text="Liechtenstein" />
                                <asp:ListItem Value="LT" Text="Lithuania" />
                                <asp:ListItem Value="LU" Text="Luxembourg" />
                                <asp:ListItem Value="MO" Text="Macau" />
                                <asp:ListItem Value="MK" Text="Macedonia, The Former Yugoslav Republic Of" />
                                <asp:ListItem Value="MG" Text="Madagascar" />
                                <asp:ListItem Value="MW" Text="Malawi" />
                                <asp:ListItem Value="MY" Text="Malaysia" />
                                <asp:ListItem Value="MV" Text="Maldives" />
                                <asp:ListItem Value="ML" Text="Mali" />
                                <asp:ListItem Value="MT" Text="Malta" />
                                <asp:ListItem Value="MH" Text="Marshall Islands" />
                                <asp:ListItem Value="MQ" Text="Martinique" />
                                <asp:ListItem Value="MR" Text="Mauritania" />
                                <asp:ListItem Value="MU" Text="Mauritius" />
                                <asp:ListItem Value="YT" Text="Mayotte" />
                                <asp:ListItem Value="MX" Text="Mexico" />
                                <asp:ListItem Value="FM" Text="Micronesia, Federated States Of" />
                                <asp:ListItem Value="MD" Text="Moldova, Republic Of" />
                                <asp:ListItem Value="MC" Text="Monaco" />
                                <asp:ListItem Value="MN" Text="Mongolia" />
                                <asp:ListItem Value="MS" Text="Montserrat" />
                                <asp:ListItem Value="MA" Text="Morocco" />
                                <asp:ListItem Value="MZ" Text="Mozambique" />
                                <asp:ListItem Value="MM" Text="Myanmar" />
                                <asp:ListItem Value="NA" Text="Namibia" />
                                <asp:ListItem Value="NR" Text="Nauru" />
                                <asp:ListItem Value="NP" Text="Nepal" />
                                <asp:ListItem Value="NL" Text="Netherlands" />
                                <asp:ListItem Value="AN" Text="Netherlands Antilles" />
                                <asp:ListItem Value="NC" Text="New Caledonia" />
                                <asp:ListItem Value="NZ" Text="New Zealand" />
                                <asp:ListItem Value="NI" Text="Nicaragua" />
                                <asp:ListItem Value="NE" Text="Niger" />
                                <asp:ListItem Value="NG" Text="Nigeria" />
                                <asp:ListItem Value="NU" Text="Niue" />
                                <asp:ListItem Value="NF" Text="Norfolk Island" />
                                <asp:ListItem Value="MP" Text="Northern Mariana Islands" />
                                <asp:ListItem Value="NO" Text="Norway" />
                                <asp:ListItem Value="OM" Text="Oman" />
                                <asp:ListItem Value="PK" Text="Pakistan" />
                                <asp:ListItem Value="PW" Text="Palau" />
                                <asp:ListItem Value="PA" Text="Panama" />
                                <asp:ListItem Value="PG" Text="Papua New Guinea" />
                                <asp:ListItem Value="PY" Text="Paraguay" />
                                <asp:ListItem Value="PE" Text="Peru" />
                                <asp:ListItem Value="PH" Text="Philippines" />
                                <asp:ListItem Value="PN" Text="Pitcairn" />
                                <asp:ListItem Value="PL" Text="Poland" />
                                <asp:ListItem Value="PT" Text="Portugal" />
                                <asp:ListItem Value="PR" Text="Puerto Rico" />
                                <asp:ListItem Value="QA" Text="Qatar" />
                                <asp:ListItem Value="RE" Text="Reunion" />
                                <asp:ListItem Value="RO" Text="Romania" />
                                <asp:ListItem Value="RU" Text="Russian Federation" />
                                <asp:ListItem Value="RW" Text="Rwanda" />
                                <asp:ListItem Value="KN" Text="Saint Kitts And Nevis" />
                                <asp:ListItem Value="LC" Text="Saint Lucia" />
                                <asp:ListItem Value="VC" Text="Saint Vincent And The Grenadines" />
                                <asp:ListItem Value="WS" Text="Samoa" />
                                <asp:ListItem Value="SM" Text="San Marino" />
                                <asp:ListItem Value="ST" Text="Sao Tome And Principe" />
                                <asp:ListItem Value="SA" Text="Saudi Arabia" />
                                <asp:ListItem Value="SN" Text="Senegal" />
                                <asp:ListItem Value="SC" Text="Seychelles" />
                                <asp:ListItem Value="SL" Text="Sierra Leone" />
                                <asp:ListItem Value="SG" Text="Singapore" />
                                <asp:ListItem Value="SK" Text="Slovakia (slovak Republic)" />
                                <asp:ListItem Value="SI" Text="Slovenia" />
                                <asp:ListItem Value="SB" Text="Solomon Islands" />
                                <asp:ListItem Value="SO" Text="Somalia" />
                                <asp:ListItem Value="ZA" Text="South Africa" />
                                <asp:ListItem Value="GS" Text="South Georgia And The South Sandwich Islands" />
                                <asp:ListItem Value="ES" Text="Spain" />
                                <asp:ListItem Value="LK" Text="Sri Lanka" />
                                <asp:ListItem Value="SH" Text="St. Helena" />
                                <asp:ListItem Value="PM" Text="St. Pierre And Miquelon" />
                                <asp:ListItem Value="SD" Text="Sudan" />
                                <asp:ListItem Value="SR" Text="Suriname" />
                                <asp:ListItem Value="SJ" Text="Svalbard And Jan Mayen Islands" />
                                <asp:ListItem Value="SZ" Text="Swaziland" />
                                <asp:ListItem Value="SE" Text="Sweden" />
                                <asp:ListItem Value="CH" Text="Switzerland" />
                                <asp:ListItem Value="SY" Text="Syrian Arab Republic" />
                                <asp:ListItem Value="TW" Text="Taiwan, Province Of China" />
                                <asp:ListItem Value="TJ" Text="Tajikistan" />
                                <asp:ListItem Value="TZ" Text="Tanzania, United Republic Of" />
                                <asp:ListItem Value="TH" Text="Thailand" />
                                <asp:ListItem Value="TG" Text="Togo" />
                                <asp:ListItem Value="TK" Text="Tokelau" />
                                <asp:ListItem Value="TO" Text="Tonga" />
                                <asp:ListItem Value="TT" Text="Trinidad And Tobago" />
                                <asp:ListItem Value="TN" Text="Tunisia" />
                                <asp:ListItem Value="TR" Text="Turkey" />
                                <asp:ListItem Value="TM" Text="Turkmenistan" />
                                <asp:ListItem Value="TC" Text="Turks And Caicos Islands" />
                                <asp:ListItem Value="TV" Text="Tuvalu" />
                                <asp:ListItem Value="UG" Text="Uganda" />
                                <asp:ListItem Value="UA" Text="Ukraine" />
                                <asp:ListItem Value="AE" Text="United Arab Emirates" />
                                <asp:ListItem Value="GB" Text="United Kingdom (Mainland and N.Ireland)" />
                                <asp:ListItem Value="US" Text="United States" />
                                <asp:ListItem Value="UM" Text="United States Minor Outlying Islands" />
                                <asp:ListItem Value="UY" Text="Uruguay" />
                                <asp:ListItem Value="UZ" Text="Uzbekistan" />
                                <asp:ListItem Value="VU" Text="Vanuatu" />
                                <asp:ListItem Value="VA" Text="Vatican City State (Holy See)" />
                                <asp:ListItem Value="VE" Text="Venezuela" />
                                <asp:ListItem Value="VN" Text="Viet Nam" />
                                <asp:ListItem Value="VG" Text="Virgin Islands (British)" />
                                <asp:ListItem Value="VI" Text="Virgin Islands (U.S.)" />
                                <asp:ListItem Value="WF" Text="Wallis And Futuna Islands" />
                                <asp:ListItem Value="EH" Text="Western Sahara" />
                                <asp:ListItem Value="YE" Text="Yemen" />
                                <asp:ListItem Value="YU" Text="Yugoslavia" />
                                <asp:ListItem Value="ZR" Text="Zaire" />
                                <asp:ListItem Value="ZM" Text="Zambia" />
                                <asp:ListItem Value="ZW" Text="Zimbabwe" />
                            </asp:DropDownList>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblTelephone" runat="server" Text="" meta:resourcekey="lblTelephone" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel16" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbTelephone" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server" Visible="false">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCompanyName" runat="server" Text="" meta:resourcekey="lblCompanyName" />
                            <asp:Label runat="server" Text="&nbsp;" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbCompanyName" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row mm_hidden" runat="server">
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblTravelAgencyIATA" runat="server" Text="" meta:resourcekey="lblTravelAgencyIATA" />
                            <asp:Label runat="server" Text="&nbsp;" />
                        </asp:Panel>
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbTravelAgencyIATA" CssClass="short" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panAirlineProgram" runat="server" Visible="false">
                    <asp:Panel CssClass="mm_field_set_row" runat="server">
                        <asp:Panel CssClass="mm_field_set_left" runat="server">
                            <asp:Panel CssClass="mm_field_set_label" runat="server">
                                <asp:Label ID="lblAirlineProgramNumber" runat="server" Text="" meta:resourcekey="lblAirlineProgramNumber" />
                                <asp:Label runat="server" Text="&nbsp;" />
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel CssClass="mm_field_set_left" runat="server">
                            <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                                <asp:TextBox ID="tbAirlineProgramNumber" runat="server" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_row" runat="server">
                        <asp:Panel CssClass="mm_field_set_left" runat="server">
                            <asp:Panel CssClass="mm_field_set_label" runat="server">
                                <asp:Label ID="lblAirlineProgramName" runat="server" Text="" meta:resourcekey="lblAirlineProgramName" />
                                <asp:Label runat="server" Text="&nbsp;" />
                                <xngr:HelpInfo runat="server" HelpCode="help_ff" />
                            </asp:Panel>
                            <asp:Panel CssClass="mm_field_set_right" runat="server">
                                <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                                    <asp:DropDownList ID="ddlAirlineProgram" runat="server" />
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panHotelProgram" runat="server">
                    <asp:Panel CssClass="mm_field_set_row" runat="server">
                        <asp:Panel CssClass="mm_field_set_left" runat="server">
                            <asp:Panel CssClass="mm_field_set_label" runat="server">
                                <asp:Label ID="lblHotelProgramNumber" runat="server" Text="" meta:resourcekey="lblHotelProgramNumber" />
                                <asp:Label runat="server" Text="&nbsp;" />
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel CssClass="mm_field_set_right" runat="server">
                            <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                                <asp:TextBox ID="tbHotelProgramNumber" runat="server" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_row" runat="server">
                        <asp:Panel CssClass="mm_field_set_left" runat="server">
                            <asp:Panel CssClass="mm_field_set_label" runat="server">
                                <asp:Label ID="lblHotelProgramName" runat="server" Text="" meta:resourcekey="lblHotelProgramName" />
                                <asp:Label runat="server" Text="&nbsp;" />
                                <xngr:HelpInfo runat="server" HelpCode="help2" />
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel CssClass="mm_field_set_right" runat="server">
                            <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                                <asp:DropDownList ID="ddlHotelProgram" runat="server" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblTravelPurpose" runat="server" Text="" meta:resourcekey="lblTravelPurpose" />
                            <asp:Label runat="server" Text="&nbsp;" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:DropDownList ID="ddlTravelPurpose" runat="server">
                                <asp:ListItem Value="0" Text="" meta:resourcekey="ddlTravelPurposeNotIdentified" />
                                <asp:ListItem Value="1" Text="" meta:resourcekey="ddlTravelPurposeBusiness" />
                                <asp:ListItem Value="2" Text="" meta:resourcekey="ddlTravelPurposeLeisure" />
                            </asp:DropDownList>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server" Visible="false">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblFltNumber" runat="server" Text="" meta:resourcekey="lblFltNumber" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbFltNumber" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="Panel1" CssClass="mm_field_set_row" runat="server" Visible="false">
                    <asp:Panel ID="Panel2" CssClass="mm_field_set_left" runat="server">
                        <asp:Panel ID="Panel17" CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblArrivalTime" runat="server" Text="" meta:resourcekey="lblArrivalTime" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel18" CssClass="mm_field_set_right" runat="server">
                        <asp:Panel ID="Panel20" CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbArrivalTimeHours" CssClass="time_set" runat="server" />
                            <asp:Label ID="Label1" CssClass="time_set" runat="server" Text=":" />
                            <asp:TextBox ID="tbArrivalTimeMinutes" CssClass="time_set" runat="server" />
                            <asp:RadioButton ID="rbArrivalTimeAM" CssClass="time_set" runat="server" Checked="true"
                                GroupName="ArrivalTime" Text="" meta:resourcekey="rbAM" />
                            <asp:RadioButton ID="rbArrivalTimePM" CssClass="time_set" runat="server" GroupName="ArrivalTime"
                                Text="" meta:resourcekey="rbPM" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panGuestInstructions" CssClass="mm_guest_details_info1" runat="server">
                    <asp:Panel ID="Panel22" CssClass="mm_guest_details_info_header" runat="server" Visible="false">
                        <asp:Label ID="lblGuestInstructions" runat="server" Text="" meta:resourcekey="lblGuestInstructions" />
                    </asp:Panel>
                    <asp:Panel ID="Panel23" CssClass="mm_guest_details_info_body1" runat="server">
                        <asp:Panel ID="Panel24" CssClass="mm_field_set_row" runat="server">
                            <asp:Panel ID="Panel25" CssClass="mm_field_set_label" runat="server">
                                <asp:Label ID="lblSpecialInstructions" runat="server" Text="" meta:resourcekey="lblSpecialInstructions" />
                            </asp:Panel>
                            <asp:Panel ID="Panel31" CssClass="mm_field_set_label1" runat="server">
                                <asp:Label ID="lblSpecialInstructions1" runat="server" Text="" meta:resourcekey="lblSpecialInstructions1" />
                            </asp:Panel>
                            <asp:Panel ID="Panel26" CssClass="mm_field_set_user_entry clearboth" runat="server">
                                <asp:TextBox ID="tbSpecialInstructions" runat="server" TextMode="MultiLine" />
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel ID="Panel27" runat="server" Visible="false">
                            <asp:Label ID="lblExtraBedding" runat="server" Text="" meta:resourcekey="lblExtraBedding" />
                            <asp:Panel ID="Panel28" runat="server">
                                <asp:Label ID="lblAdultRollaway" runat="server" Text="" meta:resourcekey="lblAdultRollaway" />
                                <asp:DropDownList ID="ddlAdultRollaway" runat="server">
                                    <asp:ListItem Value="0" Text="0" />
                                    <asp:ListItem Value="1" Text="1" />
                                    <asp:ListItem Value="2" Text="2" />
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="Panel29" runat="server">
                                <asp:Label ID="lblChildRollaway" runat="server" Text="" meta:resourcekey="lblChildRollaway" />
                                <asp:DropDownList ID="ddlChildRollaway" runat="server">
                                    <asp:ListItem Value="0" Text="0" />
                                    <asp:ListItem Value="1" Text="1" />
                                    <asp:ListItem Value="2" Text="2" />
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="Panel30" runat="server">
                                <asp:Label ID="lblCrib" runat="server" Text="" meta:resourcekey="lblCrib" />
                                <asp:DropDownList ID="ddlCrib" runat="server">
                                    <asp:ListItem Value="0" Text="0" />
                                    <asp:ListItem Value="1" Text="1" />
                                    <asp:ListItem Value="2" Text="2" />
                                </asp:DropDownList>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panCardDetails" CssClass="mm_guest_details_info" runat="server">
            <asp:Panel CssClass="mm_guest_details_info_header" runat="server">
                <asp:Label ID="lblCardDetails" runat="server" Text="" meta:resourcekey="lblCardDetails" />
            </asp:Panel>
            <asp:Panel CssClass="mm_guest_details_info_body" runat="server">
                <p>
                    <asp:Label ID="lblPaymentCardApplication" runat="server" Text="" />
                </p>
                <asp:Panel ID="panPaymentPolicy" runat="server">
                    <p>
                        <asp:Label ID="lblPaymentPolicyText" runat="server" Text="" />
                    </p>
                </asp:Panel>
                <br />
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCardholderName" runat="server" Text="" meta:resourcekey="lblCardholderName" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbCardholderName" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCardType" runat="server" Text="" meta:resourcekey="lblCardType" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:DropDownList ID="ddlCardType" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCardNumber" runat="server" Text="" meta:resourcekey="lblCardNumber" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbCardNumber" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCardExpiryDate" runat="server" Text="" meta:resourcekey="lblCardExpiryDate" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:DropDownList ID="ddlCardExpiryMonth" CssClass="short" runat="server">
                                <asp:ListItem Value="1" Text="" meta:resourcekey="ddlMonthJan">Jan</asp:ListItem>
                                <asp:ListItem Value="2" Text="" meta:resourcekey="ddlMonthFeb">Feb</asp:ListItem>
                                <asp:ListItem Value="3" Text="" meta:resourcekey="ddlMonthMar">Mar</asp:ListItem>
                                <asp:ListItem Value="4" Text="" meta:resourcekey="ddlMonthApr">Apr</asp:ListItem>
                                <asp:ListItem Value="5" Text="" meta:resourcekey="ddlMonthMay">May</asp:ListItem>
                                <asp:ListItem Value="6" Text="" meta:resourcekey="ddlMonthJun">Jun</asp:ListItem>
                                <asp:ListItem Value="7" Text="" meta:resourcekey="ddlMonthJul">Jul</asp:ListItem>
                                <asp:ListItem Value="8" Text="" meta:resourcekey="ddlMonthAug">Aug</asp:ListItem>
                                <asp:ListItem Value="9" Text="" meta:resourcekey="ddlMonthSep">Sep</asp:ListItem>
                                <asp:ListItem Value="10" Text="" meta:resourcekey="ddlMonthOct">Oct</asp:ListItem>
                                <asp:ListItem Value="11" Text="" meta:resourcekey="ddlMonthNov">Nov</asp:ListItem>
                                <asp:ListItem Value="12" Text="" meta:resourcekey="ddlMonthDec">Dec</asp:ListItem>
                            </asp:DropDownList>
                        </asp:Panel>
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:DropDownList ID="ddlCardExpiryYear" CssClass="short" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panCardStartDate" CssClass="mm_field_set_row mm_hidden" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCardStartDate" runat="server" Text="" meta:resourcekey="lblCardStartDate" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:DropDownList ID="ddlCardStartMonth" CssClass="short" runat="server">
                                <asp:ListItem Value="1" Text="" meta:resourcekey="ddlMonthJan">Jan</asp:ListItem>
                                <asp:ListItem Value="2" Text="" meta:resourcekey="ddlMonthFeb">Feb</asp:ListItem>
                                <asp:ListItem Value="3" Text="" meta:resourcekey="ddlMonthMar">Mar</asp:ListItem>
                                <asp:ListItem Value="4" Text="" meta:resourcekey="ddlMonthApr">Apr</asp:ListItem>
                                <asp:ListItem Value="5" Text="" meta:resourcekey="ddlMonthMay">May</asp:ListItem>
                                <asp:ListItem Value="6" Text="" meta:resourcekey="ddlMonthJun">Jun</asp:ListItem>
                                <asp:ListItem Value="7" Text="" meta:resourcekey="ddlMonthJul">Jul</asp:ListItem>
                                <asp:ListItem Value="8" Text="" meta:resourcekey="ddlMonthAug">Aug</asp:ListItem>
                                <asp:ListItem Value="9" Text="" meta:resourcekey="ddlMonthSep">Sep</asp:ListItem>
                                <asp:ListItem Value="10" Text="" meta:resourcekey="ddlMonthOct">Oct</asp:ListItem>
                                <asp:ListItem Value="11" Text="" meta:resourcekey="ddlMonthNov">Nov</asp:ListItem>
                                <asp:ListItem Value="12" Text="" meta:resourcekey="ddlMonthDec">Dec</asp:ListItem>
                            </asp:DropDownList>
                        </asp:Panel>
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:DropDownList ID="ddlCardStartYear" CssClass="short" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panCardIssueNumber" CssClass="mm_field_set_row mm_hidden" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCardIssueNumber" runat="server" Text="" meta:resourcekey="lblCardIssueNumber" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbCardIssueNumber" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_label" runat="server">
                            <asp:Label ID="lblCardSecurityCode" runat="server" Text="" meta:resourcekey="lblCardSecurityCode" />
                            <asp:Label runat="server" Text="" meta:resourcekey="lblRequiredEntry" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="mm_field_set_right" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:TextBox ID="tbCardSecurityCode" CssClass="short" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panPaymentCardLogos" CssClass="mm_field_set_row mm_hidden" runat="server">
                    <asp:PlaceHolder ID="phPaymentCardLogos" runat="server" />
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <table width="80" border="0" cellpadding="2" cellspacing="0" title="Click to Verify - This site chose Thawte SSL for secure e-commerce and confidential communications.">
                        <tr>
                            <td width="80" align="center" valign="top">
                                <script type="text/javascript" src="https://seal.thawte.com/getthawteseal?host_name=www.xnglobalres.com&amp;size=S&amp;lang=en"></script>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panProfileGuarantee" CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_left" runat="server">
                        <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                            <asp:CheckBox ID="cbProfileGuarantee" runat="server" Text="" />
                            <asp:Label ID="lblProfileGuarantee" runat="server" Text="" meta:resourcekey="lblProfileGuarantee" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panConfirmBooking" CssClass="mm_guest_details_info" runat="server">
            <asp:Panel CssClass="mm_guest_details_info_header" runat="server">
                <asp:Label ID="lblConfirmBooking" runat="server" Text="" meta:resourcekey="lblConfirmBooking" />
            </asp:Panel>
            <asp:Panel CssClass="mm_guest_details_info_body" runat="server">
                <p>
                    <asp:Label ID="lblConfirmBookingInstructions1" runat="server" Text="" meta:resourcekey="lblConfirmBookingInstructions1" />
                </p>
                <p>
                    <asp:Label ID="lblConfirmBookingInstructions2" runat="server" Text="" meta:resourcekey="lblConfirmBookingInstructions2" />
                </p>
                <asp:Panel ID="panPaymentGatewayNotice" CssClass="mm_guest_details_payment_gateway_notice"
                    runat="server">
                    <p>
                        <asp:Label ID="lblPaymentGatewayNotice" runat="server" Text="" meta:resourcekey="lblPaymentGatewayNotice" />
                    </p>
                </asp:Panel>
                <asp:Panel ID="panPaymentGatewayDepositNotice" CssClass="mm_guest_details_payment_gateway_notice"
                    runat="server">
                    <p>
                        <asp:Label ID="lblPaymentGatewayDepositNotice" runat="server" Text="" />
                    </p>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_user_entry" runat="server">
                        <asp:CheckBox ID="cbTermsAgreement" runat="server" Text="" />
                        <asp:Label ID="lblTermsAgreement" runat="server" Text="" meta:resourcekey="lblTermsAgreement" /><br />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_field_set_usert_entry" runat="server">
                        <asp:HyperLink runat="server" NavigateUrl="" Text="" meta:resourcekey="hlTermsAndConditions" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="Panel11" CssClass="mm_field_set_row" runat="server">
                    <asp:Panel ID="Panel21" CssClass="mm_field_set_user_entry" runat="server">
                        <asp:CheckBox ID="cbSubscribeToNewsletter" runat="server" Text="" />
                        <asp:Label ID="lblSubscribeToNewsletter" runat="server" Text="" meta:resourcekey="lblSubscribeToNewsletter" />
                    </asp:Panel>
                </asp:Panel>
                <br />
                <br />
                <asp:Panel CssClass="mm_field_set_row" runat="server">
                    <asp:Panel CssClass="mm_step mm_background_edit mm_border_edit mm_wrapper_confirm_button"
                        runat="server">
                        <asp:Button ID="btnConfirmBooking" CssClass="mm_button1 mm_button_main_step mm_text_button_hotel1"
                            runat="server" Text="" meta:resourcekey="btnConfirmBooking" OnClick="btnConfirmBooking_Click" /><br />
                        <asp:Button ID="btnConfirmBooking1" CssClass="mm_button1 mm_button1_main_step mm_text_button_hotel1"
                            runat="server" Text="" meta:resourcekey="btnConfirmBooking1" OnClick="btnConfirmBooking_Click" />
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
<script language="javascript" type="text/javascript">    ShowMSInfos(); </script>
