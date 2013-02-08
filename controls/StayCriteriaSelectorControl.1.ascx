<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StayCriteriaSelectorControl.1.ascx.cs"
    Inherits="StayCriteriaSelectorControl" %>
<%@ Reference Control="~/controls/HotelDescriptionControl.1.ascx" %>
<%@ Reference Control="~/controls/HotelRatingControl.1.ascx" %>
<xnc:xscript ID="Xscript1" src="~/zapatec/utils/zapatec.js" type="text/javascript"
    Location="SharedCDNPath" runat="server">
</xnc:xscript>
<xnc:xscript ID="Xscript2" src="~/zapatec/zpcal/src/calendar.js" type="text/javascript"
    Location="SharedCDNPath" runat="server">
</xnc:xscript>
<xnc:xscript ID="xsZLang" src="" type="text/javascript" Location="SharedCDNPath"
    runat="server">
</xnc:xscript>
<xnc:xlink ID="xsZTheme" href="" rel="stylesheet" type="text/css" Location="SharedCDNPath"
    runat="server" />
<script language="javascript" type="text/javascript">

    function HideRoomPan() {
        var el = document.getElementById('<% = ddlRoom.ClientID%>');

        var i = 0;

        if (el != null) {
            for (i = 1; i <= 8; i++) {
                var strpanRoomRow = '<% = this.ClientID%>' + "_panRoom" + i;
                var panRoomRow = document.getElementById(strpanRoomRow);
                if (panRoomRow != null)
                    panRoomRow.style.display = "none";
            }

            for (i = 1; i <= parseInt(el.value); i++) {
                var strpanRoomRow = '<% = this.ClientID%>' + "_panRoom" + i;
                var panRoomRow = document.getElementById(strpanRoomRow);
                if (panRoomRow != null)
                    panRoomRow.style.display = "";
            }

        }

        return true;
    }

    function updateSelectArr(cal) {
        var date = cal.date;
        var selectMonth = document.getElementById('<%= ddlMonthArr.ClientID %>');
        selectMonth.selectedIndex = date.getMonth();
        var selectDay = document.getElementById('<%= ddlDayArr.ClientID %>');
        selectDay.selectedIndex = (date.getDate() - 1);
        var selectYear = document.getElementById('<%= ddlYearArr.ClientID %>');
        var curDate = new Date();
        selectYear.selectedIndex = (date.getFullYear() - curDate.getFullYear());

        UpdateCalHiddenField();
    }

    function updateSelectDep(cal) {
        var panDepDate = document.getElementById('<%= panDepDate.ClientID %>');

        if (panDepDate != null) {
            var date = cal.date;
            var selectMonth = document.getElementById('<%= ddlMonthDep.ClientID %>');
            selectMonth.selectedIndex = date.getMonth();
            var selectDay = document.getElementById('<%= ddlDayDep.ClientID %>');
            selectDay.selectedIndex = (date.getDate() - 1);
            var selectYear = document.getElementById('<%= ddlYearDep.ClientID %>');
            var curDate = new Date();
            selectYear.selectedIndex = (date.getFullYear() - curDate.getFullYear());
        }

    }

    function UpdateCalHiddenField() {
        var hidden = document.getElementById('<%= calDateHiddenFieldArr.ClientID %>');
        var format = "%Y-%m-%d";

        var intMonth = parseInt(document.getElementById('<%= ddlMonthArr.ClientID %>').value) + 1;
        var intYear = parseInt(document.getElementById('<%= ddlYearArr.ClientID %>').value);

        var maxDayValue = daysInMonth(intMonth, intYear);

        if (document.getElementById('<%= ddlDayArr.ClientID %>').value > maxDayValue) {
            document.getElementById('<%= ddlDayArr.ClientID %>').selectedIndex = maxDayValue - 1;
        }

        var myDate = new Date(document.getElementById('<%= ddlYearArr.ClientID %>').value, document.getElementById('<%= ddlMonthArr.ClientID %>').value, document.getElementById('<%= ddlDayArr.ClientID %>').value);
        hidden.value = myDate.print(format);

        var panDepDate = document.getElementById('<%= panDepDate.ClientID %>');

        if (panDepDate != null) {
            var hdDepartureDay = document.getElementById('<%= hdDepartureDay.ClientID %>');
            var hiddenDep = document.getElementById('<%= calDateHiddenFieldDep.ClientID %>');

            if (true) // if (hdDepartureDay.value == hiddenDep.value)
            {
                var intDay = parseInt(document.getElementById('<%= ddlDayArr.ClientID %>').value) + 1;

                if (intDay > maxDayValue) {
                    intDay = 1;
                    intMonth = intMonth + 1;

                    if (intMonth > 12) {
                        intMonth = 1;
                        intYear = intYear + 1;
                    }

                }

                myDate = new Date(intYear, intMonth - 1, intDay);

                var selectMonth = document.getElementById('<%= ddlMonthDep.ClientID %>');
                selectMonth.selectedIndex = myDate.getMonth();
                var selectDay = document.getElementById('<%= ddlDayDep.ClientID %>');
                selectDay.selectedIndex = (myDate.getDate() - 1);
                var selectYear = document.getElementById('<%= ddlYearDep.ClientID %>');
                var curDate = new Date();
                selectYear.selectedIndex = (myDate.getFullYear() - curDate.getFullYear());

                UpdateCalHiddenFieldDep();
            }

        }

    }

    function UpdateCalHiddenFieldDep() {
        var panDepDate = document.getElementById('<%= panDepDate.ClientID %>');

        if (panDepDate != null) {
            var hidden = document.getElementById('<%= calDateHiddenFieldDep.ClientID %>');
            var format = "%Y-%m-%d";

            var intMonth = parseInt(document.getElementById('<%= ddlMonthDep.ClientID %>').value) + 1;
            var intYear = parseInt(document.getElementById('<%= ddlYearDep.ClientID %>').value);

            var maxDayValue = daysInMonth(intMonth, intYear);

            if (document.getElementById('<%= ddlDayDep.ClientID %>').value > maxDayValue) {
                document.getElementById('<%= ddlDayDep.ClientID %>').selectedIndex = maxDayValue - 1;
            }

            var myDate = new Date(document.getElementById('<%= ddlYearDep.ClientID %>').value, document.getElementById('<%= ddlMonthDep.ClientID %>').value, document.getElementById('<%= ddlDayDep.ClientID %>').value);
            hidden.value = myDate.print(format);
        }

    }

    function daysInMonth(month, year) {
        var m = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (month != 2) return m[month - 1];
        if (year % 4 != 0) return m[1];
        if (year % 100 == 0 && year % 400 != 0) return m[1];
        return m[1] + 1;
    }

    function SetCalendarArr() {
        var showButton = document.getElementById('<%= showCalendarArr.ClientID %>');

        if (showButton == null)
            return;

        var cal = new Zapatec.Calendar.setup
	    ({
	        ifFormat: "%Y-%m-%d",             // format of the input field
	        cache: true,                   // will recycle calendar object using less memory on IE 
	        button: '<%= showCalendarArr.ClientID %>',         // What will trigger the popup of the calendar
	        inputField: '<%= calDateHiddenFieldArr.ClientID %>',   // load and save hidden field
	        onUpdate: updateSelectArr,          // call function to update dropdowns
	        showsTime: false                  // don't show time, only date
	    });
    }

    function SetCalendarDep() {
        var showButton = document.getElementById('<%= showCalendarDep.ClientID %>');

        if (showButton == null)
            return;

        var cal = new Zapatec.Calendar.setup
	    ({
	        ifFormat: "%Y-%m-%d",             // format of the input field
	        cache: true,                   // will recycle calendar object using less memory on IE 
	        button: '<%= showCalendarDep.ClientID %>',         // What will trigger the popup of the calendar
	        inputField: '<%= calDateHiddenFieldDep.ClientID %>',   // load and save hidden field
	        onUpdate: updateSelectDep,          // call function to update dropdowns
	        showsTime: false                  // don't show time, only date
	    });
	}

	$(document).ready(function () {
	    var parentWidth = $(".MM_StayCriteria_Options").width();
	    var hotels = $(".MM_StayCriteria_HotelButton");
	    var equallyWidth = (parentWidth / hotels.length) - 2;
	    $(".MM_StayCriteria_HotelButtonWrapper").css("width", equallyWidth + "px");
	});

</script>
<asp:HiddenField ID="hdCalLangPath" runat="server" Value="" meta:resourcekey="hdCalLangPath" />
<asp:HiddenField ID="hdDepartureDay" runat="server" />
<asp:HiddenField ID="calDateHiddenFieldArr" runat="server" />
<asp:HiddenField ID="calDateHiddenFieldDep" runat="server" />
<asp:Panel ID="panStayCriteriaSelector" runat="server">
    <asp:UpdatePanel ID="formUpdatePanelStayCriteria" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnFindHotel" />
            <asp:PostBackTrigger ControlID="btnFindRates" />
            <asp:PostBackTrigger ControlID="btnFindAnotherHotel" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="panStayCriteriaInfo" CssClass="content_section" runat="server">
                <asp:Panel CssClass="pane_left" runat="server">
                    <asp:Panel CssClass="pane_right" runat="server">
                        <asp:Panel CssClass="pane_center" runat="server">
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="pane_body" runat="server">
                    <asp:Panel ID="panStayHotelItemInfo" CssClass="stay_hotel_info" runat="server">
                        <xnc:XImage ID="imgHotel" CssClass="stay_hotel_info_image" Location="HotelMediaCDNPath"
                            runat="server" ImageUrl="" />
                        <asp:Panel CssClass="stay_hotel_info_desc" runat="server">
                            <asp:Panel CssClass="stay_hotel_rating" runat="server">
                                <asp:PlaceHolder ID="phHotelRating" runat="server" />
                            </asp:Panel>
                            <asp:Panel CssClass="stay_hotel_info_desc_items" runat="server">
                                <h4>
                                    <asp:Label ID="lblHotelNameText" runat="server" Text="" />
                                </h4>
                                <p>
                                    <asp:Label ID="lblHotelAddressInfo" runat="server" Text="" />
                                </p>
                                <p>
                                    <asp:Label ID="lblTelephone" CssClass="stay_hotel_info_label" runat="server" Text=""
                                        meta:resourcekey="lblTelephone" />
                                    <asp:Label ID="lblTelephoneNumber" runat="server" Text="" />
                                </p>
                                <p>
                                    <asp:Label ID="lblFax" CssClass="stay_hotel_info_label" runat="server" Text="" meta:resourcekey="lblFax" />
                                    <asp:Label ID="lblFaxNumber" runat="server" Text="" />
                                </p>
                                <p>
                                    <asp:Label ID="lblEmail" CssClass="stay_hotel_info_label" runat="server" Text=""
                                        meta:resourcekey="lblEmail" />
                                    <asp:Label ID="lblEmailAddress" runat="server" Text="" />
                                </p>
                                <asp:Panel ID="panCompanyRegNumber" runat="server">
                                    <p>
                                        <asp:Label ID="lblCompanyRegNumber" CssClass="stay_hotel_info_label" runat="server"
                                            Text="" meta:resourcekey="lblCompanyRegNumber" />
                                        <asp:Label ID="lblCompanyRegNumberInfo" runat="server" Text="" />
                                    </p>
                                </asp:Panel>
                                <asp:Panel CssClass="stay_hotel_description" runat="server">
                                    <asp:PlaceHolder ID="phHotelDescription" runat="server" />
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panStayOptionsItemInfo" CssClass="options_summary" runat="server">
                        <asp:Panel ID="panAreaInfo" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblAreaInfo" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblAreaInfo" />
                            <asp:Label ID="lblAreaInfoText" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panHotelInfo" CssClass="options_summary_set" runat="server" Visible="false">
                            <asp:Label ID="lblHotelInfo" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblHotelInfo" />
                            <asp:Label ID="lblHotelInfoText" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panArrDateInfo" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblArrDateInfo" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblArrDateInfo" />
                            <asp:Label ID="lblArrDateInfoText" CssClass="options_summary_data" runat="server"
                                Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panDepDateInfo" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblDepDateInfo" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblDepDateInfo" />
                            <asp:Label ID="lblDepDateInfoText" CssClass="options_summary_data" runat="server"
                                Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panPromotionCodeInfo" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblPromotionCodeInfo" CssClass="options_summary_label" runat="server"
                                Text="" meta:resourcekey="lblPromotionCodeInfo" />
                            <asp:Label ID="lblPromotionCodeInfoText" CssClass="options_summary_data" runat="server"
                                Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panRoomInfo1" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblRoomInfo1" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblRoomInfo1" />
                            <asp:Label ID="lblRoomInfoText1" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panRoomInfo2" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblRoomInfo2" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblRoomInfo2" />
                            <asp:Label ID="lblRoomInfoText2" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panRoomInfo3" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblRoomInfo3" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblRoomInfo3" />
                            <asp:Label ID="lblRoomInfoText3" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panRoomInfo4" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblRoomInfo4" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblRoomInfo4" />
                            <asp:Label ID="lblRoomInfoText4" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panRoomInfo5" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblRoomInfo5" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblRoomInfo5" />
                            <asp:Label ID="lblRoomInfoText5" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panRoomInfo6" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblRoomInfo6" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblRoomInfo6" />
                            <asp:Label ID="lblRoomInfoText6" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panRoomInfo7" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblRoomInfo7" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblRoomInfo7" />
                            <asp:Label ID="lblRoomInfoText7" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel ID="panRoomInfo8" CssClass="options_summary_set" runat="server">
                            <asp:Label ID="lblRoomInfo8" CssClass="options_summary_label" runat="server" Text=""
                                meta:resourcekey="lblRoomInfo8" />
                            <asp:Label ID="lblRoomInfoText8" CssClass="options_summary_data" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel CssClass="" runat="server">
                            <xnc:XImage runat="server" Location="ImageCDNPath" ImageUrl="~/images/calendar.jpg" />
                            <asp:LinkButton ID="btnEditStayCriteria" runat="server" Text="" meta:resourcekey="btnEditStayCriteria"
                                OnClick="btnEditStayCriteria_Click" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panStayCriteriaEdit" CssClass="content_section" runat="server">
                <asp:Panel CssClass="pane_left" runat="server">
                    <asp:Panel CssClass="pane_right" runat="server">
                        <asp:Panel CssClass="pane_center" runat="server">
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="pane_body" runat="server">
                    <asp:Panel ID="panCloseEditStayCriteria" CssClass="options_summary_edit close_edit_stay_criteria"
                        runat="server">
                        <xnc:XImage ID="Image1" runat="server" Location="ImageCDNPath" ImageUrl="~/images/calendar.jpg" />
                        <asp:LinkButton ID="btnCloseEditStayCriteria" runat="server" Text="" meta:resourcekey="btnCloseEditStayCriteria"
                            OnClick="btnCloseEditStayCriteria_Click" />
                    </asp:Panel>
                    <asp:Panel ID="panCountryList" CssClass="field_set_row" runat="server">
                        <asp:Panel CssClass="field_set_left" runat="server">
                            <asp:Panel CssClass="field_set_label_inline" runat="server">
                                <asp:Label ID="lblCountryList" runat="server" Text="" meta:resourcekey="lblCountryList" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlCountryList" runat="server" OnSelectedIndexChanged="ddlCountryList_Change"
                                    AutoPostBack="true" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panAreaList" CssClass="field_set_row" runat="server">
                        <asp:Panel CssClass="field_set_left" runat="server">
                            <asp:Panel CssClass="field_set_label_inline" runat="server">
                                <asp:Label ID="lblAreaList" runat="server" Text="" meta:resourcekey="lblAreaList" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlAreaList" runat="server" OnSelectedIndexChanged="ddlAreaList_Change"
                                    AutoPostBack="true" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panHotelList" CssClass="field_set_row" runat="server">
                        <asp:Panel CssClass="field_set_left" runat="server">
                            <asp:Panel CssClass="field_set_label_inline" runat="server">
                                <asp:Label ID="lblHotelList" runat="server" Text="" meta:resourcekey="lblHotelList" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlHotelList" runat="server" />
                                <asp:Label ID="lblInfo" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panArrDate" CssClass="field_set_row" runat="server">
                        <asp:Panel CssClass="field_set_left" runat="server">
                            <asp:Panel CssClass="field_set_label_inline" runat="server">
                                <asp:Label ID="lblArrDate" runat="server" Text="" meta:resourcekey="lblArrDate" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlDayArr" runat="server">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                    <asp:ListItem Value="13">13</asp:ListItem>
                                    <asp:ListItem Value="14">14</asp:ListItem>
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="16">16</asp:ListItem>
                                    <asp:ListItem Value="17">17</asp:ListItem>
                                    <asp:ListItem Value="18">18</asp:ListItem>
                                    <asp:ListItem Value="19">19</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="21">21</asp:ListItem>
                                    <asp:ListItem Value="22">22</asp:ListItem>
                                    <asp:ListItem Value="23">23</asp:ListItem>
                                    <asp:ListItem Value="24">24</asp:ListItem>
                                    <asp:ListItem Value="25">25</asp:ListItem>
                                    <asp:ListItem Value="26">26</asp:ListItem>
                                    <asp:ListItem Value="27">27</asp:ListItem>
                                    <asp:ListItem Value="28">28</asp:ListItem>
                                    <asp:ListItem Value="29">29</asp:ListItem>
                                    <asp:ListItem Value="30">30</asp:ListItem>
                                    <asp:ListItem Value="31">31</asp:ListItem>
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlMonthArr" runat="server">
                                    <asp:ListItem Value="0" Text="" meta:resourcekey="ddlMonthJan">Jan</asp:ListItem>
                                    <asp:ListItem Value="1" Text="" meta:resourcekey="ddlMonthFeb">Feb</asp:ListItem>
                                    <asp:ListItem Value="2" Text="" meta:resourcekey="ddlMonthMar">Mar</asp:ListItem>
                                    <asp:ListItem Value="3" Text="" meta:resourcekey="ddlMonthApr">Apr</asp:ListItem>
                                    <asp:ListItem Value="4" Text="" meta:resourcekey="ddlMonthMay">May</asp:ListItem>
                                    <asp:ListItem Value="5" Text="" meta:resourcekey="ddlMonthJun">Jun</asp:ListItem>
                                    <asp:ListItem Value="6" Text="" meta:resourcekey="ddlMonthJul">Jul</asp:ListItem>
                                    <asp:ListItem Value="7" Text="" meta:resourcekey="ddlMonthAug">Aug</asp:ListItem>
                                    <asp:ListItem Value="8" Text="" meta:resourcekey="ddlMonthSep">Sep</asp:ListItem>
                                    <asp:ListItem Value="9" Text="" meta:resourcekey="ddlMonthOct">Oct</asp:ListItem>
                                    <asp:ListItem Value="10" Text="" meta:resourcekey="ddlMonthNov">Nov</asp:ListItem>
                                    <asp:ListItem Value="11" Text="" meta:resourcekey="ddlMonthDec">Dec</asp:ListItem>
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlYearArr" runat="server" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <xnc:XImage ID="showCalendarArr" CssClass="cal" Location="ImageCDNPath" runat="server"
                                    AlternateText="Click to see calendar.." ImageUrl="~/images/calendar.jpg" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panDepDate" CssClass="field_set_row" runat="server">
                        <asp:Panel CssClass="field_set_left" runat="server">
                            <asp:Panel CssClass="field_set_label_inline" runat="server">
                                <asp:Label ID="lblDepDate" runat="server" Text="" meta:resourcekey="lblDepDate" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlDayDep" runat="server">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                    <asp:ListItem Value="13">13</asp:ListItem>
                                    <asp:ListItem Value="14">14</asp:ListItem>
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="16">16</asp:ListItem>
                                    <asp:ListItem Value="17">17</asp:ListItem>
                                    <asp:ListItem Value="18">18</asp:ListItem>
                                    <asp:ListItem Value="19">19</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="21">21</asp:ListItem>
                                    <asp:ListItem Value="22">22</asp:ListItem>
                                    <asp:ListItem Value="23">23</asp:ListItem>
                                    <asp:ListItem Value="24">24</asp:ListItem>
                                    <asp:ListItem Value="25">25</asp:ListItem>
                                    <asp:ListItem Value="26">26</asp:ListItem>
                                    <asp:ListItem Value="27">27</asp:ListItem>
                                    <asp:ListItem Value="28">28</asp:ListItem>
                                    <asp:ListItem Value="29">29</asp:ListItem>
                                    <asp:ListItem Value="30">30</asp:ListItem>
                                    <asp:ListItem Value="31">31</asp:ListItem>
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlMonthDep" runat="server">
                                    <asp:ListItem Value="0" Text="" meta:resourcekey="ddlMonthJan">Jan</asp:ListItem>
                                    <asp:ListItem Value="1" Text="" meta:resourcekey="ddlMonthFeb">Feb</asp:ListItem>
                                    <asp:ListItem Value="2" Text="" meta:resourcekey="ddlMonthMar">Mar</asp:ListItem>
                                    <asp:ListItem Value="3" Text="" meta:resourcekey="ddlMonthApr">Apr</asp:ListItem>
                                    <asp:ListItem Value="4" Text="" meta:resourcekey="ddlMonthMay">May</asp:ListItem>
                                    <asp:ListItem Value="5" Text="" meta:resourcekey="ddlMonthJun">Jun</asp:ListItem>
                                    <asp:ListItem Value="6" Text="" meta:resourcekey="ddlMonthJul">Jul</asp:ListItem>
                                    <asp:ListItem Value="7" Text="" meta:resourcekey="ddlMonthAug">Aug</asp:ListItem>
                                    <asp:ListItem Value="8" Text="" meta:resourcekey="ddlMonthSep">Sep</asp:ListItem>
                                    <asp:ListItem Value="9" Text="" meta:resourcekey="ddlMonthOct">Oct</asp:ListItem>
                                    <asp:ListItem Value="10" Text="" meta:resourcekey="ddlMonthNov">Nov</asp:ListItem>
                                    <asp:ListItem Value="11" Text="" meta:resourcekey="ddlMonthDec">Dec</asp:ListItem>
                                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlYearDep" runat="server" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <xnc:XImage ID="showCalendarDep" CssClass="cal" Location="ImageCDNPath" runat="server"
                                    AlternateText="Click to see calendar.." ImageUrl="~/images/calendar.jpg" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panStayNights" CssClass="field_set_row" runat="server">
                        <asp:Panel CssClass="field_set_left" runat="server">
                            <asp:Panel CssClass="field_set_label_inline" runat="server">
                                <asp:Label ID="lblStayNights" runat="server" Text="" meta:resourcekey="lblStayNights" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlStayNights" runat="server" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:Label ID="lblStayNightsTotal" runat="server" Text="" meta:resourcekey="lblStayNightsTotal" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="field_set_row" runat="server">
                        <asp:Panel CssClass="field_set_left" runat="server">
                            <asp:Panel CssClass="field_set_label_inline" runat="server">
                                <asp:Label ID="lblRoom" runat="server" Text="" meta:resourcekey="lblRoom"></asp:Label>
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:DropDownList ID="ddlRoom" runat="server" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panRoom" CssClass="room_info_holder" runat="server">
                        <asp:Panel ID="panRoom1" CssClass="room_info" runat="server">
                            <asp:Label ID="lbRoom1" CssClass="label" runat="server" Text="" meta:resourcekey="lbRoom1" />
                            <asp:Label ID="lbAdults1" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbAdults1" />
                            <asp:DropDownList ID="ddlAdults1" runat="server" />
                            <asp:Label ID="lbChildren1" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbChildren1" />
                            <asp:DropDownList ID="ddlChildren1" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="panRoom2" CssClass="room_info" runat="server">
                            <asp:Label ID="lbRoom2" CssClass="label" runat="server" Text="" meta:resourcekey="lbRoom2" />
                            <asp:Label ID="lbAdults2" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbAdults2" />
                            <asp:DropDownList ID="ddlAdults2" runat="server" />
                            <asp:Label ID="lbChildren2" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbChildren2" />
                            <asp:DropDownList ID="ddlChildren2" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="panRoom3" CssClass="room_info" runat="server">
                            <asp:Label ID="lbRoom3" CssClass="label" runat="server" Text="" meta:resourcekey="lbRoom3" />
                            <asp:Label ID="lbAdults3" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbAdults3" />
                            <asp:DropDownList ID="ddlAdults3" runat="server" />
                            <asp:Label ID="lbChildren3" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbChildren3" />
                            <asp:DropDownList ID="ddlChildren3" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="panRoom4" CssClass="room_info" runat="server">
                            <asp:Label ID="lbRoom4" CssClass="label" runat="server" Text="" meta:resourcekey="lbRoom4" />
                            <asp:Label ID="lbAdults4" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbAdults4" />
                            <asp:DropDownList ID="ddlAdults4" runat="server" />
                            <asp:Label ID="lbChildren4" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbChildren4" />
                            <asp:DropDownList ID="ddlChildren4" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="panRoom5" CssClass="room_info" runat="server">
                            <asp:Label ID="lbRoom5" CssClass="label" runat="server" Text="" meta:resourcekey="lbRoom5" />
                            <asp:Label ID="lbAdults5" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbAdults5" />
                            <asp:DropDownList ID="ddlAdults5" runat="server" />
                            <asp:Label ID="lbChildren5" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbChildren5" />
                            <asp:DropDownList ID="ddlChildren5" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="panRoom6" CssClass="room_info" runat="server">
                            <asp:Label ID="lbRoom6" CssClass="label" runat="server" Text="" meta:resourcekey="lbRoom6" />
                            <asp:Label ID="lbAdults6" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbAdults6" />
                            <asp:DropDownList ID="ddlAdults6" runat="server" />
                            <asp:Label ID="lbChildren6" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbChildren6" />
                            <asp:DropDownList ID="ddlChildren6" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="panRoom7" CssClass="room_info" runat="server">
                            <asp:Label ID="lbRoom7" CssClass="label" runat="server" Text="" meta:resourcekey="lbRoom7" />
                            <asp:Label ID="lbAdults7" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbAdults7" />
                            <asp:DropDownList ID="ddlAdults7" runat="server" />
                            <asp:Label ID="lbChildren7" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbChildren7" />
                            <asp:DropDownList ID="ddlChildren7" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="panRoom8" CssClass="room_info" runat="server">
                            <asp:Label ID="lbRoom8" CssClass="label" runat="server" Text="" meta:resourcekey="lbRoom8" />
                            <asp:Label ID="lbAdults8" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbAdults8" />
                            <asp:DropDownList ID="ddlAdults8" runat="server" />
                            <asp:Label ID="lbChildren8" CssClass="sublabel" runat="server" Text="" meta:resourcekey="lbChildren8" />
                            <asp:DropDownList ID="ddlChildren8" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panPromotionCode" CssClass="field_set_row" runat="server">
                        <asp:Panel CssClass="field_set_left" runat="server">
                            <asp:Panel CssClass="field_set_label_inline" runat="server">
                                <asp:Label ID="lbPromotionCode" runat="server" Text="" meta:resourcekey="lbPromotionCode" />
                            </asp:Panel>
                            <asp:Panel CssClass="field_set_user_entry_inline" runat="server">
                                <asp:TextBox ID="tbPromotionCode" runat="server" />
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="field_set_button" runat="server">
                        <asp:Panel ID="panFindRates" CssClass="std_button" runat="server">
                            <asp:Panel CssClass="std_button_left" runat="server">
                                <asp:Panel CssClass="std_button_right" runat="server">
                                    <asp:Panel CssClass="std_button_center" runat="server">
                                        <asp:Button ID="btnFindRates" CssClass="std_button_control" runat="server" Text=""
                                            meta:resourcekey="btnFindRates" OnClick="btnStayCriteriaCompleted_Click" />
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel ID="panFindHotel" CssClass="std_button" runat="server">
                            <asp:Panel CssClass="std_button_left" runat="server">
                                <asp:Panel CssClass="std_button_right" runat="server">
                                    <asp:Panel CssClass="std_button_center" runat="server">
                                        <asp:Button ID="btnFindHotel" CssClass="std_button_control" runat="server" Text=""
                                            meta:resourcekey="btnFindHotel" OnClick="btnStayCriteriaCompleted_Click" />
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel ID="panFindAnotherHotel" CssClass="std_button" runat="server">
                            <asp:Panel CssClass="std_button_left" runat="server">
                                <asp:Panel CssClass="std_button_right" runat="server">
                                    <asp:Panel CssClass="std_button_center" runat="server">
                                        <asp:Button ID="btnFindAnotherHotel" CssClass="std_button_control" runat="server"
                                            Text="" meta:resourcekey="btnFindAnotherHotel" OnClick="btnFindAnotherHotel_Click" />
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="panCustomUI" CssClass="MM_StayCriteria" runat="server">
                <asp:Panel ID="panCustomStayCriteriaInfo" CssClass="MM_content_section" runat="server">
                    <asp:Panel ID="panCustomStayCriteriaInfoBody" CssClass="MM_Step_Summary" runat="server">
                        <asp:Label ID="lblStayCriteriaInfo" runat="server" meta:resourcekey="lblStayCriteriaInfo"></asp:Label>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panCustomStayCriteriaEdit" CssClass="MM_content_section" runat="server">
                    <asp:Panel ID="panCustomStayCriteriaEditBody" CssClass="MM_Step_Edit" runat="server">
                        <asp:Panel ID="panCustomStayCriteriaEditInfo" CssClass="MM_Step_Edit_Header" runat="server">
                            <asp:Label ID="lblStayCriteriaEditInfo" runat="server" meta:resourcekey="lblStayCriteriaEditInfo"></asp:Label>
                        </asp:Panel>
                        <asp:Panel ID="panCustomStayCriteriaEditSelection" CssClass="MM_StayCriteria_Options"
                            runat="server">
                            <asp:Label ID="lblInfo2" runat="server"></asp:Label>
                            <asp:PlaceHolder ID="phdStayCriteriaOptions" runat="server"></asp:PlaceHolder>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<script language="javascript" type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoadedHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    function pageLoadedHandler(sender, args)
    { SetCalendarArr(); SetCalendarDep(); HideRoomPan(); }

    function EndRequestHandler(sender, args)
    { SetCalendarArr(); SetCalendarDep(); HideRoomPan(); }
</script>
<script language="javascript" type="text/javascript">
    SetCalendarArr();
    SetCalendarDep();
    HideRoomPan();
</script>
