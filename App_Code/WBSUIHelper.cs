using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XHS.Logging;
using XHS.WBSUIBizObjects;
using ChaosSoftware;

/// <summary>
/// Helper class for WBS UI Pages
/// </summary>

public class WBSUIHelper
{
    private System.Web.SessionState.HttpSessionState Session;
    private System.Web.UI.Page Page;

    public WBSUIHelper(System.Web.SessionState.HttpSessionState session, System.Web.UI.Page page)
    {
        Session = session;
        Page = page;

        return;
    }

    // UI object initialization methods

    public void InitLanguage()
    {
        Session["SelectedCulture"] = this.SelectedCulture;
        Session["SelectedUICulture"] = this.SelectedUICulture;

        return;
    }

    public void InitProfileLoginInfo()
    {
        ProfileLoginInfo objProfileLoginInfo = new ProfileLoginInfo();

        objProfileLoginInfo.LogonName = "";
        objProfileLoginInfo.LogonPassword = "";
        objProfileLoginInfo.SecurityQuestion = "";
        objProfileLoginInfo.SecurityAnswer = "";

        Session["ProfileLoginInfo"] = objProfileLoginInfo;
        Session["ViewLoginForm"] = false;
        Session["LoginProfiles"] = new Profile[0];
        Session["IsLoggedIn"] = false;

        return;
    }

    public void InitStayCriteriaSelection()
    {
        StayCriteriaSelection objStayCriteriaSelection = new StayCriteriaSelection();

        objStayCriteriaSelection.CountryCode = "";
        objStayCriteriaSelection.AreaID = "";
        objStayCriteriaSelection.HotelCode = "";
        objStayCriteriaSelection.ArrivalDate = TZNet.ToLocal(this.GetTimeZone(""), DateTime.UtcNow.AddDays(1)).Date;
        objStayCriteriaSelection.DepartureDate = objStayCriteriaSelection.ArrivalDate.AddDays(1).Date;
        objStayCriteriaSelection.PromotionCode = "";
        objStayCriteriaSelection.RoomOccupantSelections = new RoomOccupantSelection[1];

        objStayCriteriaSelection.RoomOccupantSelections[0] = new RoomOccupantSelection();
        objStayCriteriaSelection.RoomOccupantSelections[0].RoomRefID = "1";
        objStayCriteriaSelection.RoomOccupantSelections[0].NumberRooms = 1;
        objStayCriteriaSelection.RoomOccupantSelections[0].NumberAdults = WebconfigHelper.DefaultNumberAdult;
        objStayCriteriaSelection.RoomOccupantSelections[0].NumberChildren = WebconfigHelper.DefaultNumberChildren;

        Session[Constants.Sessions.StayCriteriaSelection] = objStayCriteriaSelection;
        Session["RateGridStartDate"] = objStayCriteriaSelection.ArrivalDate.Date;
        Session["SearchRateGridStartDate"] = objStayCriteriaSelection.ArrivalDate.Date;

        Session["ImageGalleries"] = new List<HotelImageGallery>();

        return;
    }

    public void InitRoomRateSelections()
    {
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];
        RoomRateSelection[] objRoomRateSelections = new RoomRateSelection[objStayCriteriaSelection.RoomOccupantSelections.Length];

        for (int i = 0; i < objRoomRateSelections.Length; i++)
        {
            objRoomRateSelections[i] = new RoomRateSelection();
            objRoomRateSelections[i].RoomRefID = ((int)(i + 1)).ToString();
            objRoomRateSelections[i].RoomTypeCode = "";
            objRoomRateSelections[i].RatePlanCode = "";
            objRoomRateSelections[i].PromotionCode = "";
        }

        Session["RoomRateSelections"] = objRoomRateSelections;

        Session["ShowMoreRatesIndicators"] = new ShowMoreRatesIndicator[0];

        return;
    }

    public void InitAddOnPackageSelections()
    {
        Session["AddOnPackageSelections"] = new AddOnPackageSelection[0];
        return;
    }

    public void InitGuestDetailsEntryInfo()
    {
        Profile[] objProfiles = (Profile[])Session["LoginProfiles"];

        Profile objCompanyProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.Corporation);
        Profile objAgencyProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.TravelAgent);
        ProfileIdentifier objAgencyIATA = ProfileHelper.GetProfileIdentifier(objAgencyProfile, ProfileIdentifierType.IATA);

        GuestDetailsEntryInfo objGuestDetailsEntryInfo = new GuestDetailsEntryInfo();

        objGuestDetailsEntryInfo.NamePrefix = "";
        objGuestDetailsEntryInfo.FirstName = "";
        objGuestDetailsEntryInfo.LastName = "";
        objGuestDetailsEntryInfo.Address1 = "";
        objGuestDetailsEntryInfo.Address2 = "";
        objGuestDetailsEntryInfo.City = "";
        objGuestDetailsEntryInfo.StateRegion = "";
        objGuestDetailsEntryInfo.PostalCode = "";
        objGuestDetailsEntryInfo.Country = "";
        objGuestDetailsEntryInfo.Phone = "";
        objGuestDetailsEntryInfo.Fax = "";
        objGuestDetailsEntryInfo.Email = "";
        objGuestDetailsEntryInfo.EmailConfirmEntry = "";

        if (objCompanyProfile != null)
            objGuestDetailsEntryInfo.CompanyName = objCompanyProfile.CompanyName;
        else
            objGuestDetailsEntryInfo.CompanyName = "";

        if (objAgencyIATA != null)
            objGuestDetailsEntryInfo.TravelAgencyIATA = objAgencyIATA.Identifier;
        else
            objGuestDetailsEntryInfo.TravelAgencyIATA = "";

        objGuestDetailsEntryInfo.SubscribeToNewsletter = false;
        objGuestDetailsEntryInfo.TravelPurpose = TravelPurpose.NotIdentified;
        objGuestDetailsEntryInfo.FlightNumber = "";

        objGuestDetailsEntryInfo.AirlineProgramIdentifier = "";
        objGuestDetailsEntryInfo.AirlineProgramCode = "";

        objGuestDetailsEntryInfo.HotelProgramIdentifier = "";
        objGuestDetailsEntryInfo.HotelProgramCode = "";

        objGuestDetailsEntryInfo.NumberCribs = 0;
        objGuestDetailsEntryInfo.NumberRollawaysAdult = 0;
        objGuestDetailsEntryInfo.NumberRollawaysChild = 0;
        objGuestDetailsEntryInfo.SpecialInstructions = "";

        objGuestDetailsEntryInfo.PaymentCardType = "";
        objGuestDetailsEntryInfo.PaymentCardNumber = "";
        objGuestDetailsEntryInfo.PaymentCardHolder = "";
        objGuestDetailsEntryInfo.PaymentCardEffectiveDate = "";
        objGuestDetailsEntryInfo.PaymentCardExpireDate = "";
        objGuestDetailsEntryInfo.PaymentCardIssueNumber = "";
        objGuestDetailsEntryInfo.PaymentCardSecurityCode = "";

        objGuestDetailsEntryInfo.ProfileGuaranteeRequested = false;

        Session["GuestDetailsEntryInfo"] = objGuestDetailsEntryInfo;

        Session["BookingTermsConditionsAccepted"] = false;

        Session[Constants.Sessions.PaymentGatewayInfos] = new PaymentGatewayInfo[0];
        Session[Constants.Sessions.PaymentGatewayInfo] = null;
        Session[Constants.Sessions.HotelBookingPaymentAllocations] = new HotelBookingPaymentAllocation[0];
        Session["HotelPaymentRQ"] = null;
        Session["HotelPaymentRS"] = null;
        Session["PaymentGatewayRequestActive"] = false;
        Session["PaymentGatewayResponseActive"] = false;

        return;
    }

    public void PrefillGuestDetailsEntryInfoForTesting()
    {
        GuestDetailsEntryInfo objGuestDetailsEntryInfo = (GuestDetailsEntryInfo)Session["GuestDetailsEntryInfo"];

        objGuestDetailsEntryInfo.NamePrefix = "Mr";
        objGuestDetailsEntryInfo.FirstName = "John";
        objGuestDetailsEntryInfo.LastName = "Smith";
        objGuestDetailsEntryInfo.Address1 = "123 Xn Circle";
        objGuestDetailsEntryInfo.Address2 = "Suite 101";
        objGuestDetailsEntryInfo.City = "Davidsonville";
        objGuestDetailsEntryInfo.StateRegion = "Maryland";
        objGuestDetailsEntryInfo.PostalCode = "21035";
        objGuestDetailsEntryInfo.Country = "US";
        objGuestDetailsEntryInfo.Phone = "123 456 7890";
        objGuestDetailsEntryInfo.Fax = "123 654 0987";
        objGuestDetailsEntryInfo.Email = "k.m.fisher@comcast.net";
        objGuestDetailsEntryInfo.EmailConfirmEntry = "k.m.fisher@comcast.net";

        //objGuestDetailsEntryInfo.CompanyName = "Xn Hotel Systems";
        //objGuestDetailsEntryInfo.TravelAgencyIATA = "12345678";

        objGuestDetailsEntryInfo.SubscribeToNewsletter = false;
        objGuestDetailsEntryInfo.TravelPurpose = TravelPurpose.Business;
        objGuestDetailsEntryInfo.FlightNumber = "UA1234";
        objGuestDetailsEntryInfo.ArrivalTime = "15:30";

        objGuestDetailsEntryInfo.AirlineProgramIdentifier = "";
        objGuestDetailsEntryInfo.AirlineProgramCode = "";

        objGuestDetailsEntryInfo.HotelProgramIdentifier = "";
        objGuestDetailsEntryInfo.HotelProgramCode = "";

        objGuestDetailsEntryInfo.NumberCribs = 0;
        objGuestDetailsEntryInfo.NumberRollawaysAdult = 0;
        objGuestDetailsEntryInfo.NumberRollawaysChild = 0;
        objGuestDetailsEntryInfo.SpecialInstructions = "Non-smoking room please";

        objGuestDetailsEntryInfo.PaymentCardType = "VI";
        objGuestDetailsEntryInfo.PaymentCardNumber = "4111111111111111";
        objGuestDetailsEntryInfo.PaymentCardHolder = "John Smith";
        objGuestDetailsEntryInfo.PaymentCardEffectiveDate = "1208";
        objGuestDetailsEntryInfo.PaymentCardExpireDate = "1210";
        objGuestDetailsEntryInfo.PaymentCardIssueNumber = "";
        objGuestDetailsEntryInfo.PaymentCardSecurityCode = "123";

        objGuestDetailsEntryInfo.ProfileGuaranteeRequested = false;

        Session["GuestDetailsEntryInfo"] = objGuestDetailsEntryInfo;

        Session["BookingTermsConditionsAccepted"] = true;

        return;
    }

    public void InitCancelDetailsEntryInfo()
    {
        CancelDetailsEntryInfo objCancelDetailsEntryInfo = new CancelDetailsEntryInfo();

        objCancelDetailsEntryInfo.HotelCode = "";
        objCancelDetailsEntryInfo.ConfirmationNumber = "";
        objCancelDetailsEntryInfo.GuestLastName = "";
        objCancelDetailsEntryInfo.SelectedConfirmationNumbersToCancel = new string[0];

        Session["CancelDetailsEntryInfo"] = objCancelDetailsEntryInfo;

        return;
    }

    public void SyncStayDatesToRateGrid(DateTime dtRateGridDate)
    {
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

        int intBookingDaysRequested = ((TimeSpan)(objStayCriteriaSelection.DepartureDate.Subtract(objStayCriteriaSelection.ArrivalDate))).Days;

        objStayCriteriaSelection.ArrivalDate = dtRateGridDate.Date;
        objStayCriteriaSelection.DepartureDate = dtRateGridDate.AddDays(intBookingDaysRequested).Date;

        Session[Constants.Sessions.StayCriteriaSelection] = objStayCriteriaSelection;

        return;
    }

    public bool IsRateGridDataCached(AvailabilityCalendar objAvailabilityCalendar, DateTime dtGridStartDate, int intGridNumDays)
    {
        if (objAvailabilityCalendar.NumDays == 0)
            return false;

        DateTime dtGridEndDate = dtGridStartDate.AddDays(intGridNumDays - 1);
        DateTime dtAvCalDataEndDate = objAvailabilityCalendar.StartDate.AddDays(objAvailabilityCalendar.NumDays - 1);

        if (dtGridStartDate.Date < objAvailabilityCalendar.StartDate.Date || dtGridStartDate.Date > dtAvCalDataEndDate.Date)
            return false;

        if (dtGridEndDate.Date > dtAvCalDataEndDate.Date)
            return false;

        return true;
    }

    public void UpdateRatePlanPaymentPolicies()
    {
        HotelAvailabilityRS objHotelAvailabilityRS = (HotelAvailabilityRS)Session["HotelAvailabilityRS"];

        GeneralPaymentPolicy objGeneralPaymentPolicy = this.GetGeneralPaymentPolicyInfo();

        for (int i = 0; i < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; i++)
        {
            for (int j = 0; j < objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans.Length; j++)
            {
                if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].GuaranteeType == GuaranteeType.CCDCVoucher || objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].GuaranteeType == GuaranteeType.Profile || objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].GuaranteeType == GuaranteeType.None)
                {
                    objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].GeneralPaymentPolicy = objGeneralPaymentPolicy.Guarantee;
                }

                else if (objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].GuaranteeType == GuaranteeType.Deposit || objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].GuaranteeType == GuaranteeType.PrePay)
                {
                    objHotelAvailabilityRS.HotelRoomAvailInfos[i].RatePlans[j].GeneralPaymentPolicy = objGeneralPaymentPolicy.Prepay;
                }

            }

        }

        return;
    }

    public GeneralPaymentPolicy GetGeneralPaymentPolicyInfo()
    {
        GeneralPaymentPolicy objGeneralPaymentPolicy = new GeneralPaymentPolicy();

        objGeneralPaymentPolicy.Guarantee = "";
        objGeneralPaymentPolicy.Prepay = "";
        objGeneralPaymentPolicy.Mixed = "";

        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];

        HotelDescriptiveInfo objHotelDescriptiveInfo = null;

        if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length != 0)
        {
            objHotelDescriptiveInfo = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[0];
        }

        if (objHotelDescriptiveInfo != null)
        {
            for (int i = 0; i < objHotelDescriptiveInfo.Descriptions.Length; i++)
            {
                if (objHotelDescriptiveInfo.Descriptions[i].CategoryCode == HotelDescriptionCategoryCode.MiscellaneousInformation && objHotelDescriptiveInfo.Descriptions[i].MiscCategoryReferenceCode.ToLower().Trim() == "paypolicy_guarantee")
                {
                    objGeneralPaymentPolicy.Guarantee = objHotelDescriptiveInfo.Descriptions[i].ContentText;
                }

                else if (objHotelDescriptiveInfo.Descriptions[i].CategoryCode == HotelDescriptionCategoryCode.MiscellaneousInformation && objHotelDescriptiveInfo.Descriptions[i].MiscCategoryReferenceCode.ToLower().Trim() == "paypolicy_prepay")
                {
                    objGeneralPaymentPolicy.Prepay = objHotelDescriptiveInfo.Descriptions[i].ContentText;
                }

                else if (objHotelDescriptiveInfo.Descriptions[i].CategoryCode == HotelDescriptionCategoryCode.MiscellaneousInformation && objHotelDescriptiveInfo.Descriptions[i].MiscCategoryReferenceCode.ToLower().Trim() == "paypolicy_mixed")
                {
                    objGeneralPaymentPolicy.Mixed = objHotelDescriptiveInfo.Descriptions[i].ContentText;
                }

            }

        }

        return objGeneralPaymentPolicy;
    }

    // Profile login related methods

    public string GetLoginProfileIdentifier()
    {
        Profile[] objProfiles = (Profile[])Session["LoginProfiles"];
        bool bIsLoggedIn = (bool)Session["IsLoggedIn"];

        string strProfileID = "";

        if (bIsLoggedIn)
        {
            Profile objPersonProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.Traveler);

            if (objPersonProfile != null)
            {
                ProfileIdentifier objProfileIdentifier = ProfileHelper.GetProfileIdentifier(objPersonProfile, ProfileIdentifierType.ProfileID);

                if (objProfileIdentifier != null)
                {
                    strProfileID = objProfileIdentifier.Identifier;
                }

            }

        }

        return strProfileID;
    }

    public Profile GetLoginProfile()
    {
        Profile[] objProfiles = (Profile[])Session["LoginProfiles"];
        bool bIsLoggedIn = (bool)Session["IsLoggedIn"];

        if (bIsLoggedIn)
        {
            Profile objPersonProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.Traveler);

            if (objPersonProfile != null)
            {
                return objPersonProfile;
            }

        }

        return null;
    }

    public string GetLoginLinkedProfileIdentifier()
    {
        Profile[] objProfiles = (Profile[])Session["LoginProfiles"];
        bool bIsLoggedIn = (bool)Session["IsLoggedIn"];

        string strLinkedProfileID = "";

        if (bIsLoggedIn)
        {
            Profile objPersonProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.Traveler);

            if (objPersonProfile != null)
            {
                ProfileIdentifier objLinkedProfileIdentifier = ProfileHelper.GetProfileIdentifier(objPersonProfile, ProfileIdentifierType.LinkedProfileID);

                if (objLinkedProfileIdentifier != null)
                {
                    strLinkedProfileID = objLinkedProfileIdentifier.Identifier;
                }

            }

        }

        return strLinkedProfileID;
    }

    public Profile GetLoginLinkedProfile()
    {
        Profile[] objProfiles = (Profile[])Session["LoginProfiles"];
        bool bIsLoggedIn = (bool)Session["IsLoggedIn"];

        if (bIsLoggedIn)
        {
            Profile objCorporateProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.Corporation);

            if (objCorporateProfile != null)
            {
                return objCorporateProfile;
            }

            Profile objAgencyProfile = ProfileHelper.GetProfile(objProfiles, ProfileType.TravelAgent);

            if (objAgencyProfile != null)
            {
                return objAgencyProfile;
            }

        }

        return null;
    }

    // General support methods

    public HotelBookingReadSegment[] GetValidatedBookings(CancelDetailsEntryInfo objCancelDetailsEntryInfo)
    {
        HotelBookingReadRS objHotelBookingReadRS = (HotelBookingReadRS)Session["HotelBookingReadRS"];

        DateTime dtLocateDate = TZNet.ToLocal(this.GetTimeZone(objCancelDetailsEntryInfo.HotelCode), DateTime.UtcNow).Date;

        if (objHotelBookingReadRS.Segments == null || objHotelBookingReadRS.Segments.Length == 0)
            return new HotelBookingReadSegment[0];

        List<HotelBookingReadSegment> lHotelBookingReadSegments = new List<HotelBookingReadSegment>();

        for (int si = 0; si < objHotelBookingReadRS.Segments.Length; si++)
        {
            if (objHotelBookingReadRS.Segments[si].BookingReadStatus != BookingReadStatus.Active)
                continue;

            if (objHotelBookingReadRS.Segments[si].ArrivalDate.Date < dtLocateDate.Date)
                continue;

            if (objHotelBookingReadRS.Segments[si].HotelCode != objCancelDetailsEntryInfo.HotelCode)
                continue;

            bool bBookingProfileLocated = false;

            for (int pi = 0; pi < objHotelBookingReadRS.Segments[si].Profiles.Length; pi++)
            {
                if (objHotelBookingReadRS.Segments[si].Profiles[pi].Type == ProfileType.Traveler)
                {
                    if (objHotelBookingReadRS.Segments[si].Profiles[pi].PersonLastName.ToUpper() == objCancelDetailsEntryInfo.GuestLastName.ToUpper())
                    {
                        bBookingProfileLocated = true;
                        break;
                    }

                }

            }

            if (!bBookingProfileLocated)
                continue;

            lHotelBookingReadSegments.Add(objHotelBookingReadRS.Segments[si]);
        }

        return lHotelBookingReadSegments.ToArray();
    }

    public bool IsCreditCardInfoRequired(HotelAvailabilityRS objHotelAvailabilityRS, RoomRateSelection[] objRoomRateSelections, PaymentGatewayInfo objPaymentGatewayInfo, bool bProfileGuaranteeRequested)
    {
        bool bRequired = false;

        for (int i = 0; i < objRoomRateSelections.Length; i++)
        {
            HotelRoomAvailInfo objHotelRoomAvailInfo = this.GetHotelRoomAvailInfo(objHotelAvailabilityRS.HotelRoomAvailInfos, objRoomRateSelections[i].RoomRefID);
            HotelAvailRatePlan objHotelAvailRatePlan = this.GetRatePlanInfo(objHotelRoomAvailInfo.RatePlans, objRoomRateSelections[i].RatePlanCode);

            if (objHotelAvailRatePlan.GuaranteeType == GuaranteeType.CCDCVoucher && !bProfileGuaranteeRequested)
            {
                bRequired = true;
                break;
            }

            else if (objHotelAvailRatePlan.GuaranteeType == GuaranteeType.Deposit || objHotelAvailRatePlan.GuaranteeType == GuaranteeType.PrePay)
            {
                if (objPaymentGatewayInfo == null || objPaymentGatewayInfo.Mode == PaymentGatewayMode.MerchantSiteCapturesCardDetails)
                {
                    bRequired = true;
                    break;
                }

            }

        }

        return bRequired;
    }

    public bool IsFullAvailability(HotelAvailabilityRS objHotelAvailabilityRS, StayCriteriaSelection objStayCriteriaSelection)
    {
        // Assumes objHotelAvailabilityRS does not contain alternate availability info.

        if (objHotelAvailabilityRS.HotelRoomAvailInfos.Length == objStayCriteriaSelection.RoomOccupantSelections.Length)
        {
            if (ConfigurationManager.AppSettings["EnableRoomRateDescriptionModel"] != "1")
            {
                return true;
            }

            else
            {
                for (int si = 0; si < objHotelAvailabilityRS.HotelRoomAvailInfos.Length; si++)
                {
                    HotelRoomAvailInfo objHotelRoomAvailInfo = objHotelAvailabilityRS.HotelRoomAvailInfos[si];

                    bool bRatesAvailable = false;

                    for (int i = 0; i < objHotelRoomAvailInfo.RoomRates.Length; i++)
                    {
                        if (objHotelRoomAvailInfo.RoomRates[i].DescriptionStatus == RoomRateDescriptionStatus.Active)
                        {
                            bRatesAvailable = true;
                            break;
                        }

                        for (int j = 0; j < objHotelRoomAvailInfo.RatePlans.Length; j++)
                        {
                            if (objHotelRoomAvailInfo.RatePlans[j].Code == objHotelRoomAvailInfo.RoomRates[i].RatePlanCode)
                            {
                                if (objHotelRoomAvailInfo.RatePlans[j].Type == RatePlanType.Negotiated || objHotelRoomAvailInfo.RatePlans[j].Type == RatePlanType.Consortia)
                                {
                                    bRatesAvailable = true;
                                }

                                break;
                            }

                        }

                        if (bRatesAvailable)
                            break;
                    }

                    if (!bRatesAvailable)
                    {
                        return false;
                    }

                }

            }

        }

        return true;
    }

    private HotelRoomAvailInfo GetHotelRoomAvailInfo(HotelRoomAvailInfo[] objHotelRoomAvailInfos, string strRoomRefID)
    {
        for (int i = 0; i < objHotelRoomAvailInfos.Length; i++)
        {
            if (objHotelRoomAvailInfos[i].SegmentRefID == strRoomRefID)
                return objHotelRoomAvailInfos[i];
        }

        return null;
    }

    private HotelAvailRatePlan GetRatePlanInfo(HotelAvailRatePlan[] objHotelAvailRatePlans, string strRatePlanCode)
    {
        for (int i = 0; i < objHotelAvailRatePlans.Length; i++)
        {
            if (objHotelAvailRatePlans[i].Code == strRatePlanCode)
                return objHotelAvailRatePlans[i];
        }

        return null;
    }

    // Membership program support methods

    public MembershipProgram[] GetMembershipPrograms(string strHotelCode)
    {
        int intNumMembershipPrograms = 0;

        if (ConfigurationManager.AppSettings["MembershipProgramCount"] != "")
        {
            try
            {
                intNumMembershipPrograms = Convert.ToInt32(ConfigurationManager.AppSettings["MembershipProgramCount"]);
            }

            catch
            {
                intNumMembershipPrograms = 0;
            }

        }

        List<MembershipProgram> lMembershipPrograms = new List<MembershipProgram>();

        for (int i = 0; i < intNumMembershipPrograms; i++)
        {
            string strMembershipProgram = ConfigurationManager.AppSettings["MembershipProgram" + ((int)(i + 1)).ToString()];

            if (strMembershipProgram != "")
            {
                string[] saMembershipProgram = strMembershipProgram.Split(new char[] { ';' });

                if (saMembershipProgram.Length == 4)
                {
                    MembershipProgram objMembershipProgram = new MembershipProgram();

                    if (strHotelCode != null && strHotelCode != "")
                    {
                        if (saMembershipProgram[0] != strHotelCode)
                            continue;
                    }

                    objMembershipProgram.ProgramName = saMembershipProgram[1];
                    objMembershipProgram.ProgramCode = saMembershipProgram[2];

                    if (saMembershipProgram[3] == "air")
                        objMembershipProgram.ProgramType = MembershipProgramType.Air;
                    else if (saMembershipProgram[3] == "hotel")
                        objMembershipProgram.ProgramType = MembershipProgramType.Hotel;
                    else
                        objMembershipProgram.ProgramType = MembershipProgramType.Unknown;

                    if (objMembershipProgram.ProgramType == MembershipProgramType.Unknown)
                        continue;

                    lMembershipPrograms.Add(objMembershipProgram);
                }

            }

        }

        return lMembershipPrograms.ToArray();
    }

    // Booking restriction notification support methods

    public BookingRestrictionNotice[] GetBookingRestrictionNotices(string strHotelCode)
    {
        int intNumBookingRestrictionNotices = 0;

        if (ConfigurationManager.AppSettings["BookingRestrictionNoticeCount"] != "")
        {
            try
            {
                intNumBookingRestrictionNotices = Convert.ToInt32(ConfigurationManager.AppSettings["BookingRestrictionNoticeCount"]);
            }

            catch
            {
                intNumBookingRestrictionNotices = 0;
            }

        }

        List<BookingRestrictionNotice> lBookingRestrictionNotices = new List<BookingRestrictionNotice>();

        for (int i = 0; i < intNumBookingRestrictionNotices; i++)
        {
            string strBookingRestrictionNotice = ConfigurationManager.AppSettings["BookingRestrictionNotice" + ((int)(i + 1)).ToString()];

            if (strBookingRestrictionNotice != "")
            {
                string[] saBookingRestrictionNotice = strBookingRestrictionNotice.Split(new char[] { ';' });

                if (saBookingRestrictionNotice.Length == 5)
                {
                    BookingRestrictionNotice objBookingRestrictionNotice = new BookingRestrictionNotice();

                    if (strHotelCode != null && strHotelCode != "")
                    {
                        if (saBookingRestrictionNotice[0] != strHotelCode)
                            continue;
                    }

                    objBookingRestrictionNotice.HotelCode = saBookingRestrictionNotice[0];
                    objBookingRestrictionNotice.HotelName = saBookingRestrictionNotice[1];

                    if (saBookingRestrictionNotice[2] == "opening")
                        objBookingRestrictionNotice.Type = BookingRetrictionType.Opening;
                    else if (saBookingRestrictionNotice[2] == "repair")
                        objBookingRestrictionNotice.Type = BookingRetrictionType.Repairs;
                    else if (saBookingRestrictionNotice[2] == "refurbish")
                        objBookingRestrictionNotice.Type = BookingRetrictionType.Refurbishment;
                    else if (saBookingRestrictionNotice[2] == "closing")
                        objBookingRestrictionNotice.Type = BookingRetrictionType.Closing;
                    else
                        objBookingRestrictionNotice.Type = BookingRetrictionType.Unknown;

                    if (objBookingRestrictionNotice.Type == BookingRetrictionType.Unknown)
                        continue;

                    DateTime dtCurrentDate = DateTime.Today; // ok if not converted to local hotel date

                    objBookingRestrictionNotice.Date1 = dtCurrentDate;
                    objBookingRestrictionNotice.Date2 = dtCurrentDate;

                    if (!DateTime.TryParse(saBookingRestrictionNotice[3], out objBookingRestrictionNotice.Date1))
                        continue;

                    if (objBookingRestrictionNotice.Type == BookingRetrictionType.Repairs || objBookingRestrictionNotice.Type == BookingRetrictionType.Refurbishment)
                    {
                        if (!DateTime.TryParse(saBookingRestrictionNotice[4], out objBookingRestrictionNotice.Date2))
                            continue;
                    }

                    if (objBookingRestrictionNotice.Type == BookingRetrictionType.Opening || objBookingRestrictionNotice.Type == BookingRetrictionType.Closing)
                    {
                        if (objBookingRestrictionNotice.Date1.Date <= dtCurrentDate.Date.Date)
                            continue;
                    }

                    else if (objBookingRestrictionNotice.Type == BookingRetrictionType.Repairs || objBookingRestrictionNotice.Type == BookingRetrictionType.Refurbishment)
                    {
                        if (objBookingRestrictionNotice.Date2.Date <= dtCurrentDate.Date.Date)
                            continue;
                    }

                    lBookingRestrictionNotices.Add(objBookingRestrictionNotice);
                }

            }

        }

        return lBookingRestrictionNotices.ToArray();
    }

    public bool IsBookingRestrictedDate(StayCriteriaSelection objStayCriteriaSelection)
    {
        if (objStayCriteriaSelection.HotelCode == null || objStayCriteriaSelection.HotelCode == "")
            return false;

        BookingRestrictionNotice[] objBookingRestrictionNotices = this.GetBookingRestrictionNotices(objStayCriteriaSelection.HotelCode);

        for (int i = 0; i < objBookingRestrictionNotices.Length; i++)
        {
            if (objBookingRestrictionNotices[i].Type == BookingRetrictionType.Opening)
            {
                if (objStayCriteriaSelection.ArrivalDate.Date < objBookingRestrictionNotices[i].Date1.Date || objStayCriteriaSelection.DepartureDate.Date < objBookingRestrictionNotices[i].Date1.Date)
                    return true;
            }

            if (objBookingRestrictionNotices[i].Type == BookingRetrictionType.Closing)
            {
                if (objStayCriteriaSelection.ArrivalDate.Date >= objBookingRestrictionNotices[i].Date1.Date || objStayCriteriaSelection.DepartureDate.Date >= objBookingRestrictionNotices[i].Date1.Date)
                    return true;
            }

            else if (objBookingRestrictionNotices[i].Type == BookingRetrictionType.Repairs || objBookingRestrictionNotices[i].Type == BookingRetrictionType.Refurbishment)
            {
                if (objStayCriteriaSelection.ArrivalDate.Date < objBookingRestrictionNotices[i].Date1.Date && objStayCriteriaSelection.DepartureDate.Date < objBookingRestrictionNotices[i].Date1.Date)
                    continue;

                if (objStayCriteriaSelection.ArrivalDate.Date > objBookingRestrictionNotices[i].Date2.Date && objStayCriteriaSelection.DepartureDate.Date > objBookingRestrictionNotices[i].Date2.Date)
                    continue;

                return true;
            }

        }

        return false;
    }

    // Tracking code support methods

    public TrackingCodeInfo[] GetTrackingCodeInfos(string strHotelCode)
    {
        return WBSUITrackingHelper.GetTrackingCodeInfos(strHotelCode);
    }

    // Globalization support properties and methods

    public string SelectedCulture
    {
        get
        {
            string strSelectedCulture = (string)Session["SelectedCulture"];

            if (strSelectedCulture != null && strSelectedCulture != "")
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

    public string SelectedUICulture
    {
        get
        {
            string strSelectedUICulture = (string)Session["SelectedUICulture"];

            if (strSelectedUICulture != null && strSelectedUICulture != "")
                return strSelectedUICulture;

            if (ConfigurationManager.AppSettings["DefaultUICulture"] != null && ConfigurationManager.AppSettings["DefaultUICulture"] != "")
                return ConfigurationManager.AppSettings["DefaultUICulture"];

            return "en";
        }

        set
        {
            Session["SelectedUICulture"] = value;
        }

    }

    public LanguageSetup[] GetLanguageSetups()
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
            string strLanguageSetup = ConfigurationManager.AppSettings["Language" + ((int)(i + 1)).ToString()];

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

    public string GetTimeZone(string strHotelCode)
    {
        HotelDescriptiveInfoRS objHotelDescriptiveInfoRS = (HotelDescriptiveInfoRS)Session["HotelDescriptiveInfoRS"];
        StayCriteriaSelection objStayCriteriaSelection = (StayCriteriaSelection)Session[Constants.Sessions.StayCriteriaSelection];

        string strTimeZoneCode = ConfigurationManager.AppSettings["DefaultTimeZoneCode"];

        bool bTimeZoneSet = false;

        if (strHotelCode != null && strHotelCode != "")
        {
            if (objHotelDescriptiveInfoRS != null && objHotelDescriptiveInfoRS.HotelDescriptiveInfos != null)
            {
                for (int i = 0; i < objHotelDescriptiveInfoRS.HotelDescriptiveInfos.Length; i++)
                {
                    if (objHotelDescriptiveInfoRS.HotelDescriptiveInfos[i].HotelCode == strHotelCode)
                    {
                        strTimeZoneCode = objHotelDescriptiveInfoRS.HotelDescriptiveInfos[i].TimeZone;

                        bTimeZoneSet = true;
                        break;
                    }

                }

            }

        }

        if (!bTimeZoneSet)
        {
            if (objStayCriteriaSelection != null && objStayCriteriaSelection.CountryCode != null && objStayCriteriaSelection.CountryCode != "")
            {
                if (ConfigurationManager.AppSettings["DefaultTimeZoneCode_" + objStayCriteriaSelection.CountryCode] != null)
                {
                    strTimeZoneCode = ConfigurationManager.AppSettings["DefaultTimeZoneCode_" + objStayCriteriaSelection.CountryCode];
                }

            }

        }

        return strTimeZoneCode;
    }

    public decimal GetCurrencyConversionFactor(string strCurrencyCode)
    {
        // Used for "coarse" adjustment of hotel currency to base currency for analytics "order total" tracking.

        decimal decConversionFactor = 1;

        if (strCurrencyCode != null && strCurrencyCode != "")
        {
            if (ConfigurationManager.AppSettings["DefaultCurrencyConversion_" + strCurrencyCode] != null)
            {
                Decimal.TryParse(ConfigurationManager.AppSettings["DefaultCurrencyConversion_" + strCurrencyCode], out decConversionFactor);
            }

        }

        return decConversionFactor;
    }

}

[Serializable]
public class TrackingCodeInfo
{
    public string HotelCode;
    public TrackingTagLocation TagLocation;
    public TrackingPageSelection PageSelection;
    public TrackingCodeType Type;
    public string[] TrackingCodeParameters;
}

[Serializable]
public enum TrackingTagLocation
{
    HeadElement,
    BodyElement,
    Unknown
}

[Serializable]
public enum TrackingPageSelection
{
    StartPageOnly,
    ConfirmPageOnly,
    AllPages,
    Unknown
}

[Serializable]
public enum TrackingCodeType
{
    GoogleAnalytics_urchin,
    GoogleAnalytics_ga,
    GoogleAnalytics_async,
    GoogleAdWordsConversion,
    YahooSearchMarketing,
    DoubleClickFloodlight_start,
    DoubleClickFloodlight_confirm,
    Unknown
}

[Serializable]
public class BookingRestrictionNotice
{
    public string HotelCode;
    public string HotelName;
    public BookingRetrictionType Type;
    public DateTime Date1;
    public DateTime Date2;
}

public enum BookingRetrictionType
{
    Opening,
    Repairs,
    Refurbishment,
    Closing,
    Unknown
}

[Serializable]
public class ShowMoreRatesIndicator
{
    public string RoomRefID;
    public string RoomTypeCode;
}

[Serializable]
public class LanguageSetup
{
    public string LanguageText;
    public string Culture;
    public string UICulture;
    public string ImageURL;
}

[Serializable]
public class CrossPageErrors
{
    public List<string> PageErrors_WbsApi;
    public List<string> PageErrors_WbsPg;
    public List<string> PageErrors_Validation;
}

