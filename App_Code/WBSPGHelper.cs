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
using XHS.Logging;
using XHS.WBSUIBizObjects;

/// <summary>
/// Helper class for WBS Payment Gateways
/// </summary>

public class WBSPGHelper
{
    public const int CBA_MERCHANT_ID = 0;
    public const int CBA_MERCHANT_ACCESS_CODE = 1;
    public const int CBA_SECURE_HASH_SECRET = 2;
    public const int CBA_API_USER_NAME = 3;
    public const int CBA_API_USER_PASSWORD = 4;

    public const int OGONE_MERCHANT_ID = 0;
    public const int OGONE_USER_ID = 1;
    public const int OGONE_USER_PASSWORD = 2;
    public const int OGONE_SECURE_SECRET = 3;

    public const int BNZ_MERCHANT_ID = 0;
    public const int BNZ_CLIENT_CERT_PATH = 1;
    public const int BNZ_CLIENT_CERT_PASSWORD = 2;

    public const int DIBS_MERCHANT_ID = 0;
    public const int DIBS_SECURE_SECRET_1 = 1;
    public const int DIBS_SECURE_SECRET_2 = 2;
    public const int DIBS_CURRENCY_CODE_NUMBER = 3;
    public const int DIBS_SYSTEM_MODE = 4;

    public const int MIGS3P_MERCHANT_ID = 0;
    public const int MIGS3P_MERCHANT_ACCESS_CODE = 1;
    public const int MIGS3P_SECURE_HASH_SECRET = 2;
    public const int MIGS3P_API_USER_NAME = 3;
    public const int MIGS3P_API_USER_PASSWORD = 4;
    public const int MIGS3P_AVS_ACTIVE = 5;
    public const int MIGS3P_AVS_RETURN_CODES = 6;

    public const int HDFC_TID_ALIAS = 0;
    public const int HDFC_TID_RESOURCE_PATH = 1;

    private System.Web.SessionState.HttpSessionState Session;
    private System.Web.UI.Page Page;
    private FileLog objEventLog;
    private ExceptionLog objExceptionEventLog;
    private bool bIsProduction;

    public WBSPGHelper(System.Web.SessionState.HttpSessionState session, System.Web.UI.Page page, FileLog eventLog, ExceptionLog exceptionEventLog, bool isProduction)
    {
        Session = session;
        Page = page;
        objEventLog = eventLog;
        objExceptionEventLog = exceptionEventLog;
        bIsProduction = isProduction;

        return;
    }

    public static HotelPaymentRQ GetHotelPaymentRQ(PaymentGatewayInfo objPaymentGatewayInfo, HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations, HotelDescriptiveInfo objHotelDescriptiveInfo, GuestDetailsEntryInfo objGuestDetailsEntryInfo, string strCultureCode, string strUICultureCode)
    {
        HotelPaymentRQ objHotelPaymentRQ = new HotelPaymentRQ();

        objHotelPaymentRQ.RequestTransID = Guid.NewGuid().ToString();
        objHotelPaymentRQ.PaymentGateway = objPaymentGatewayInfo;
        objHotelPaymentRQ.PaymentType = PaymentType.Payment;
        objHotelPaymentRQ.PaymentMethod = PaymentMethod.PaymentCard;
        objHotelPaymentRQ.PaymentAmounts = objHotelBookingPaymentAllocations;
        objHotelPaymentRQ.PaymentTransRefID = DateTime.UtcNow.Ticks.ToString("00000000000000000000");
        objHotelPaymentRQ.PaymentTransInfo = "Hotel Accommodation";

        objHotelPaymentRQ.PaymentCard = new HotelBookingPaymentCard();

        if (objPaymentGatewayInfo.Mode == PaymentGatewayMode.MerchantSiteCapturesCardDetails)
        {
            objHotelPaymentRQ.PaymentCard.PaymentCardType = objGuestDetailsEntryInfo.PaymentCardType;
            objHotelPaymentRQ.PaymentCard.PaymentCardNumber = objGuestDetailsEntryInfo.PaymentCardNumber;
            objHotelPaymentRQ.PaymentCard.PaymentCardHolder = objGuestDetailsEntryInfo.PaymentCardHolder;
            objHotelPaymentRQ.PaymentCard.PaymentCardEffectiveDate = objGuestDetailsEntryInfo.PaymentCardEffectiveDate;
            objHotelPaymentRQ.PaymentCard.PaymentCardExpireDate = objGuestDetailsEntryInfo.PaymentCardExpireDate;
            objHotelPaymentRQ.PaymentCard.PaymentCardIssueNumber = objGuestDetailsEntryInfo.PaymentCardIssueNumber;
            objHotelPaymentRQ.PaymentCard.PaymentCardSecurityCode = objGuestDetailsEntryInfo.PaymentCardSecurityCode;
        }

        else if (objPaymentGatewayInfo.Mode == PaymentGatewayMode.PaymentGatewayCapturesCardDetails)
        {
            objHotelPaymentRQ.PaymentCard.PaymentCardType = "";
            objHotelPaymentRQ.PaymentCard.PaymentCardNumber = "";
            objHotelPaymentRQ.PaymentCard.PaymentCardHolder = "";
            objHotelPaymentRQ.PaymentCard.PaymentCardEffectiveDate = "";
            objHotelPaymentRQ.PaymentCard.PaymentCardExpireDate = "";
            objHotelPaymentRQ.PaymentCard.PaymentCardIssueNumber = "";
            objHotelPaymentRQ.PaymentCard.PaymentCardSecurityCode = "";
        }

        objHotelPaymentRQ.HotelCode = objHotelDescriptiveInfo.HotelCode;
        objHotelPaymentRQ.CurrencyCode = objHotelDescriptiveInfo.CurrencyCode;
        objHotelPaymentRQ.CultureCode = strCultureCode;
        objHotelPaymentRQ.UICultureCode = strUICultureCode;

        return objHotelPaymentRQ;
    }

    public static HotelBookingPaymentAllocation[] GetPaymentAllocations(HotelPricing[] objHotelPricing)
    {
        HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocation = new HotelBookingPaymentAllocation[objHotelPricing.Length];

        for (int i = 0; i < objHotelPricing.Length; i++)
        {
            objHotelBookingPaymentAllocation[i] = new HotelBookingPaymentAllocation();
            objHotelBookingPaymentAllocation[i].SegmentRefID = objHotelPricing[i].SegmentRefID;
            objHotelBookingPaymentAllocation[i].Amount = objHotelPricing[i].TotalDeposit;
        }

        return objHotelBookingPaymentAllocation;
    }

    public static decimal GetBookingSegmentPayment(HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations, string strSegmentRefID)
    {
        decimal decBookingSegmentPayment = 0;

        for (int i = 0; i < objHotelBookingPaymentAllocations.Length; i++)
        {
            if (objHotelBookingPaymentAllocations[i].SegmentRefID == strSegmentRefID)
            {
                decBookingSegmentPayment = objHotelBookingPaymentAllocations[i].Amount;
                break;
            }

        }

        return decBookingSegmentPayment;
    }

    public static decimal GetTotalPaymentCardPayment(HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations)
    {
        decimal decPaymentTotal = 0;

        for (int i = 0; i < objHotelBookingPaymentAllocations.Length; i++)
            decPaymentTotal += objHotelBookingPaymentAllocations[i].Amount;

        return decPaymentTotal;
    }

    public static PaymentCardApplication GetPaymentCardApplicationStatus(HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations)
    {
        if (WBSPGHelper.BookingIsGuaranteeOnly(objHotelBookingPaymentAllocations))
            return PaymentCardApplication.GuaranteeOnly;

        if (WBSPGHelper.BookingIsDepositOnly(objHotelBookingPaymentAllocations))
            return PaymentCardApplication.DepositOnly;

        return PaymentCardApplication.GuaranteeAndDeposit;
    }

    public static bool IsPaymentGatewayPreSelectRequired(PaymentGatewayInfo[] objPaymentGatewayInfos, HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations)
    {
        if (WBSPGHelper.BookingIsGuaranteeOnly(objHotelBookingPaymentAllocations))
            return false;

        if (objPaymentGatewayInfos == null || objPaymentGatewayInfos.Length <= 1)
            return false;

        bool bPaymentGatewayCapturesCardDetails = false;

        for (int i = 0; i < objPaymentGatewayInfos.Length; i++)
        {
            if (objPaymentGatewayInfos[i].Mode == PaymentGatewayMode.PaymentGatewayCapturesCardDetails)
            {
                bPaymentGatewayCapturesCardDetails = true;
                break;
            }

        }

        if (!bPaymentGatewayCapturesCardDetails)
            return false;

        return true;
    }

    public static string[] GetPaymentGatewayAcceptedCardTypes(PaymentGatewayInfo[] objPaymentGatewayInfos)
    {
        List<string> lPaymentGatewayAcceptedCardTypes = new List<string>();

        if (objPaymentGatewayInfos != null)
        {
            for (int i = 0; i < objPaymentGatewayInfos.Length; i++)
            {
                if (objPaymentGatewayInfos[i].Mode == PaymentGatewayMode.MerchantSiteCapturesCardDetails)
                    lPaymentGatewayAcceptedCardTypes.AddRange(objPaymentGatewayInfos[i].AcceptedCardTypes);
            }

        }

        return lPaymentGatewayAcceptedCardTypes.ToArray();
    }

    public static PaymentGatewayInfo GetPaymentGatewayByCardType(PaymentGatewayInfo[] objPaymentGatewayInfos, string strPaymentCardType)
    {
        PaymentGatewayInfo objPaymentGatewayInfo = null;

        if (objPaymentGatewayInfos != null)
        {
            for (int i = 0; i < objPaymentGatewayInfos.Length; i++)
            {
                for (int j = 0; j < objPaymentGatewayInfos[i].AcceptedCardTypes.Length; j++)
                {
                    if (objPaymentGatewayInfos[i].AcceptedCardTypes[j] == strPaymentCardType)
                    {
                        objPaymentGatewayInfo = objPaymentGatewayInfos[i];
                        break;
                    }

                }

            }

        }

        return objPaymentGatewayInfo;
    }

    public static bool IsOnlinePayment(PaymentGatewayInfo[] objPaymentGatewayInfos, HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations, string strPaymentCardType)
    {
        if (WBSPGHelper.BookingIsGuaranteeOnly(objHotelBookingPaymentAllocations))
            return false;

        if (!WBSPGHelper.IsOnlineCardType(objPaymentGatewayInfos, strPaymentCardType))
            return false;

        return true;
    }

    public static bool IsOnlineCardType(PaymentGatewayInfo[] objPaymentGatewayInfos, string strPaymentCardType)
    {
        if (strPaymentCardType == "XX")
            return true;

        PaymentGatewayInfo objPaymentGatewayInfo = GetPaymentGatewayByCardType(objPaymentGatewayInfos, strPaymentCardType);

        if (objPaymentGatewayInfo != null)
            return true;

        return false;
    }

    public static XHS.CommWebVPCHelper.CardType CBACardType(string strCardType)
    {
        if (strCardType == "VI")
            return XHS.CommWebVPCHelper.CardType.Visa;

        if (strCardType == "MC" || strCardType == "CA")
            return XHS.CommWebVPCHelper.CardType.MasterCard;

        if (strCardType == "AX")
            return XHS.CommWebVPCHelper.CardType.AMEX;

        if (strCardType == "DC")
            return XHS.CommWebVPCHelper.CardType.Diners;

        return XHS.CommWebVPCHelper.CardType.Undefined;
    }

    public static XHS.OgoneHelper.CardType OgoneCardType(string strCardType)
    {
        if (strCardType == "VI")
            return XHS.OgoneHelper.CardType.Visa;

        if (strCardType == "MC" || strCardType == "CA")
            return XHS.OgoneHelper.CardType.MasterCard;

        if (strCardType == "AX")
            return XHS.OgoneHelper.CardType.AMEX;

        if (strCardType == "DC")
            return XHS.OgoneHelper.CardType.Diners;

        return XHS.OgoneHelper.CardType.Undefined;
    }

    public static XHS.BNZHelper.CardType BNZCardType(string strCardType)
    {
        if (strCardType == "VI")
            return XHS.BNZHelper.CardType.Visa;

        if (strCardType == "MC" || strCardType == "CA")
            return XHS.BNZHelper.CardType.MasterCard;

        if (strCardType == "AX")
            return XHS.BNZHelper.CardType.AMEX;

        if (strCardType == "DC")
            return XHS.BNZHelper.CardType.Diners;

        return XHS.BNZHelper.CardType.Undefined;
    }

    public static string DIBSPostingCardType(string strDIBSCardType)
    {
        if (strDIBSCardType == "DK")
            return "DK";

        if (strDIBSCardType == "V-DK")
            return "VI";

        if (strDIBSCardType == "VISA(SE)")
            return "VI";

        if (strDIBSCardType == "VISA")
            return "VI";

        if (strDIBSCardType == "MC(DK)")
            return "MC";

        if (strDIBSCardType == "MC(SE)")
            return "MC";

        if (strDIBSCardType == "MC")
            return "MC";

        if (strDIBSCardType == "DIN(DK)")
            return "DC";

        if (strDIBSCardType == "DIN")
            return "DC";

        if (strDIBSCardType == "AMEX(DK)")
            return "AX";

        if (strDIBSCardType == "AMEX")
            return "AX";

        if (strDIBSCardType == "MTRO(DK)")
            return "MA";

        if (strDIBSCardType == "MTRO")
            return "MA";

        if (strDIBSCardType == "ELEC")
            return "VE";

        if (strDIBSCardType == "JCB")
            return "JC";

        return "XX";
    }

    public static string MIGS3PPostingCardType(string strMIGS3PCardType)
    {
        if (strMIGS3PCardType == "VC")
            return "VI";

        if (strMIGS3PCardType == "MC")
            return "MC";

        if (strMIGS3PCardType == "AE")
            return "AX";

        return "XX";
    }

    public static string HDFCPostingCardType()
    {
        return "XX";
    }

    public static DateTime ExpirationDate(string strExpirationDateMMYY)
    {
        int ccMonth = 0;
        int ccYear = 0;

        try
        {
            ccMonth = Convert.ToInt32(strExpirationDateMMYY.Substring(0, 2));
            ccYear = Convert.ToInt32(strExpirationDateMMYY.Substring(2, 2)) + 2000;
        }

        catch
        {
            ccMonth = 1;
            ccYear = 2001;
        }

        return new DateTime(ccYear, ccMonth, 1);
    }

    public static PaymentGatewayInfo[] GetPaymentGatewayInfos(string strHotelCode)
    {
        List<PaymentGatewayInfo> lPaymentGatewayInfos = new List<PaymentGatewayInfo>();

        int intNumPaymentGatewayInfos = 0;

        if (ConfigurationManager.AppSettings["PaymentGatewayCount"] != "")
        {
            try
            {
                intNumPaymentGatewayInfos = Convert.ToInt32(ConfigurationManager.AppSettings["PaymentGatewayCount"]);
            }

            catch
            {
                intNumPaymentGatewayInfos = 0;
            }

        }

        for (int i = 0; i < intNumPaymentGatewayInfos; i++)
        {
            string strPaymentGatewayInfo = ConfigurationManager.AppSettings["PaymentGateway" + ((int)(i + 1)).ToString()];

            if (strPaymentGatewayInfo != "")
            {
                string[] saPaymentGatewayInfo = strPaymentGatewayInfo.Split(new char[] { ';' });

                for (int j = 0; j < saPaymentGatewayInfo.Length; j++)
                    saPaymentGatewayInfo[j] = saPaymentGatewayInfo[j].Replace("^semicolon^", ";");  // de-escape occurance of ";" (used as data and not delimiter) in info string

                if (saPaymentGatewayInfo.Length < 2)
                    continue;

                if (strHotelCode == null || strHotelCode == "" || saPaymentGatewayInfo[0] != strHotelCode)
                    continue;

                PaymentGatewayInfo objPaymentGatewayInfo = new PaymentGatewayInfo();

                if (saPaymentGatewayInfo[1] == "cba")
                {
                    if (saPaymentGatewayInfo.Length != 7)
                        continue;

                    objPaymentGatewayInfo.Type = PaymentGatewayType.CBA;
                    objPaymentGatewayInfo.Mode = PaymentGatewayMode.MerchantSiteCapturesCardDetails;

                    objPaymentGatewayInfo.ConfigurationParameters = new string[5];
                    objPaymentGatewayInfo.ConfigurationParameters[CBA_MERCHANT_ID] = saPaymentGatewayInfo[2];
                    objPaymentGatewayInfo.ConfigurationParameters[CBA_MERCHANT_ACCESS_CODE] = saPaymentGatewayInfo[3];
                    objPaymentGatewayInfo.ConfigurationParameters[CBA_SECURE_HASH_SECRET] = saPaymentGatewayInfo[4];
                    objPaymentGatewayInfo.ConfigurationParameters[CBA_API_USER_NAME] = saPaymentGatewayInfo[5];
                    objPaymentGatewayInfo.ConfigurationParameters[CBA_API_USER_PASSWORD] = saPaymentGatewayInfo[6];

                    objPaymentGatewayInfo.AcceptedCardTypes = WBSPGHelper.GetAcceptedCardTypes(PaymentGatewayType.CBA, strHotelCode);
                    objPaymentGatewayInfo.SubmitPaymentURL = "~/payment_gateways/cba/SubmitPayment.aspx";

                    lPaymentGatewayInfos.Add(objPaymentGatewayInfo);
                }

                else if (saPaymentGatewayInfo[1] == "ogone")
                {
                    if (saPaymentGatewayInfo.Length != 6)
                        continue;

                    objPaymentGatewayInfo.Type = PaymentGatewayType.Ogone;
                    objPaymentGatewayInfo.Mode = PaymentGatewayMode.MerchantSiteCapturesCardDetails;

                    objPaymentGatewayInfo.ConfigurationParameters = new string[4];
                    objPaymentGatewayInfo.ConfigurationParameters[OGONE_MERCHANT_ID] = saPaymentGatewayInfo[2];
                    objPaymentGatewayInfo.ConfigurationParameters[OGONE_USER_ID] = saPaymentGatewayInfo[3];
                    objPaymentGatewayInfo.ConfigurationParameters[OGONE_USER_PASSWORD] = saPaymentGatewayInfo[4];
                    objPaymentGatewayInfo.ConfigurationParameters[OGONE_SECURE_SECRET] = saPaymentGatewayInfo[5];

                    objPaymentGatewayInfo.AcceptedCardTypes = WBSPGHelper.GetAcceptedCardTypes(PaymentGatewayType.Ogone, strHotelCode);
                    objPaymentGatewayInfo.SubmitPaymentURL = "~/payment_gateways/ogone/SubmitPayment.aspx";

                    lPaymentGatewayInfos.Add(objPaymentGatewayInfo);
                }

                else if (saPaymentGatewayInfo[1] == "bnz")
                {
                    if (saPaymentGatewayInfo.Length != 5)
                        continue;

                    objPaymentGatewayInfo.Type = PaymentGatewayType.BNZ;
                    objPaymentGatewayInfo.Mode = PaymentGatewayMode.MerchantSiteCapturesCardDetails;

                    objPaymentGatewayInfo.ConfigurationParameters = new string[3];
                    objPaymentGatewayInfo.ConfigurationParameters[BNZ_MERCHANT_ID] = saPaymentGatewayInfo[2];
                    objPaymentGatewayInfo.ConfigurationParameters[BNZ_CLIENT_CERT_PATH] = saPaymentGatewayInfo[3];
                    objPaymentGatewayInfo.ConfigurationParameters[BNZ_CLIENT_CERT_PASSWORD] = saPaymentGatewayInfo[4];

                    objPaymentGatewayInfo.AcceptedCardTypes = WBSPGHelper.GetAcceptedCardTypes(PaymentGatewayType.BNZ, strHotelCode);
                    objPaymentGatewayInfo.SubmitPaymentURL = "~/payment_gateways/bnz/SubmitPayment.aspx";

                    lPaymentGatewayInfos.Add(objPaymentGatewayInfo);
                }

                else if (saPaymentGatewayInfo[1] == "dibs")
                {
                    if (saPaymentGatewayInfo.Length != 7)
                        continue;

                    objPaymentGatewayInfo.Type = PaymentGatewayType.DIBS;
                    objPaymentGatewayInfo.Mode = PaymentGatewayMode.PaymentGatewayCapturesCardDetails;

                    objPaymentGatewayInfo.ConfigurationParameters = new string[5];
                    objPaymentGatewayInfo.ConfigurationParameters[DIBS_MERCHANT_ID] = saPaymentGatewayInfo[2];
                    objPaymentGatewayInfo.ConfigurationParameters[DIBS_SECURE_SECRET_1] = saPaymentGatewayInfo[3];
                    objPaymentGatewayInfo.ConfigurationParameters[DIBS_SECURE_SECRET_2] = saPaymentGatewayInfo[4];
                    objPaymentGatewayInfo.ConfigurationParameters[DIBS_CURRENCY_CODE_NUMBER] = saPaymentGatewayInfo[5];
                    objPaymentGatewayInfo.ConfigurationParameters[DIBS_SYSTEM_MODE] = saPaymentGatewayInfo[6];

                    objPaymentGatewayInfo.AcceptedCardTypes = WBSPGHelper.GetAcceptedCardTypes(PaymentGatewayType.DIBS, strHotelCode);
                    objPaymentGatewayInfo.SubmitPaymentURL = "~/payment_gateways/dibs/SubmitPayment.aspx";

                    lPaymentGatewayInfos.Add(objPaymentGatewayInfo);
                }

                else if (saPaymentGatewayInfo[1] == "migs3p")
                {
                    if (saPaymentGatewayInfo.Length != 9)
                        continue;

                    objPaymentGatewayInfo.Type = PaymentGatewayType.MIGS3P;
                    objPaymentGatewayInfo.Mode = PaymentGatewayMode.PaymentGatewayCapturesCardDetails;

                    objPaymentGatewayInfo.ConfigurationParameters = new string[7];
                    objPaymentGatewayInfo.ConfigurationParameters[MIGS3P_MERCHANT_ID] = saPaymentGatewayInfo[2];
                    objPaymentGatewayInfo.ConfigurationParameters[MIGS3P_MERCHANT_ACCESS_CODE] = saPaymentGatewayInfo[3];
                    objPaymentGatewayInfo.ConfigurationParameters[MIGS3P_SECURE_HASH_SECRET] = saPaymentGatewayInfo[4];
                    objPaymentGatewayInfo.ConfigurationParameters[MIGS3P_API_USER_NAME] = saPaymentGatewayInfo[5];
                    objPaymentGatewayInfo.ConfigurationParameters[MIGS3P_API_USER_PASSWORD] = saPaymentGatewayInfo[6];
                    objPaymentGatewayInfo.ConfigurationParameters[MIGS3P_AVS_ACTIVE] = saPaymentGatewayInfo[7];
                    objPaymentGatewayInfo.ConfigurationParameters[MIGS3P_AVS_RETURN_CODES] = saPaymentGatewayInfo[8];

                    objPaymentGatewayInfo.AcceptedCardTypes = WBSPGHelper.GetAcceptedCardTypes(PaymentGatewayType.MIGS3P, strHotelCode);
                    objPaymentGatewayInfo.SubmitPaymentURL = "~/payment_gateways/migs3p/SubmitPayment.aspx";

                    lPaymentGatewayInfos.Add(objPaymentGatewayInfo);
                }

                else if (saPaymentGatewayInfo[1] == "hdfc")
                {
                    if (saPaymentGatewayInfo.Length != 4)
                        continue;

                    objPaymentGatewayInfo.Type = PaymentGatewayType.HDFC;
                    objPaymentGatewayInfo.Mode = PaymentGatewayMode.PaymentGatewayCapturesCardDetails;

                    objPaymentGatewayInfo.ConfigurationParameters = new string[2];
                    objPaymentGatewayInfo.ConfigurationParameters[HDFC_TID_ALIAS] = saPaymentGatewayInfo[2];
                    objPaymentGatewayInfo.ConfigurationParameters[HDFC_TID_RESOURCE_PATH] = saPaymentGatewayInfo[3];

                    objPaymentGatewayInfo.AcceptedCardTypes = WBSPGHelper.GetAcceptedCardTypes(PaymentGatewayType.HDFC, strHotelCode);
                    objPaymentGatewayInfo.SubmitPaymentURL = "~/payment_gateways/hdfc/SubmitPayment.aspx";

                    lPaymentGatewayInfos.Add(objPaymentGatewayInfo);
                }

                else
                    continue;
            }

        }

        return lPaymentGatewayInfos.ToArray();
    }

    private static string[] GetAcceptedCardTypes(PaymentGatewayType enumPaymentGatewayType, string strHotelCode)
    {
        string[] AcceptedCardTypes = new string[0];

        string strPaymentGatewayHotelPaymentCardInfo = ConfigurationManager.AppSettings["PaymentGatewayPaymentCard_" + strHotelCode];

        if (strPaymentGatewayHotelPaymentCardInfo != null && strPaymentGatewayHotelPaymentCardInfo != "")
        {
            AcceptedCardTypes = strPaymentGatewayHotelPaymentCardInfo.Split(new char[] { ';' });
        }

        else
        {
            int intNumPaymentGatewayPaymentCardInfos = 0;

            if (ConfigurationManager.AppSettings["PaymentGatewayPaymentCardCount"] != null && ConfigurationManager.AppSettings["PaymentGatewayPaymentCardCount"] != "")
            {
                try
                {
                    intNumPaymentGatewayPaymentCardInfos = Convert.ToInt32(ConfigurationManager.AppSettings["PaymentGatewayPaymentCardCount"]);
                }

                catch
                {
                    intNumPaymentGatewayPaymentCardInfos = 0;
                }

            }

            for (int i = 0; i < intNumPaymentGatewayPaymentCardInfos; i++)
            {
                string strPaymentGatewayPaymentCardInfo = ConfigurationManager.AppSettings["PaymentGatewayPaymentCard" + ((int)(i + 1)).ToString()];

                if (strPaymentGatewayPaymentCardInfo != null && strPaymentGatewayPaymentCardInfo != "")
                {
                    string[] saPaymentGatewayPaymentCardInfo = strPaymentGatewayPaymentCardInfo.Split(new char[] { ';' });

                    if (saPaymentGatewayPaymentCardInfo.Length < 2)
                        continue;

                    if (!(saPaymentGatewayPaymentCardInfo[0] == "cba" && enumPaymentGatewayType == PaymentGatewayType.CBA
                        || saPaymentGatewayPaymentCardInfo[0] == "ogone" && enumPaymentGatewayType == PaymentGatewayType.Ogone
                        || saPaymentGatewayPaymentCardInfo[0] == "bnz" && enumPaymentGatewayType == PaymentGatewayType.BNZ
                        || saPaymentGatewayPaymentCardInfo[0] == "dibs" && enumPaymentGatewayType == PaymentGatewayType.DIBS
                        || saPaymentGatewayPaymentCardInfo[0] == "migs3p" && enumPaymentGatewayType == PaymentGatewayType.MIGS3P
                        || saPaymentGatewayPaymentCardInfo[0] == "hdfc" && enumPaymentGatewayType == PaymentGatewayType.HDFC))
                        continue;

                    AcceptedCardTypes = new string[saPaymentGatewayPaymentCardInfo.Length - 1];

                    for (int j = 0; j < AcceptedCardTypes.Length; j++)
                        AcceptedCardTypes[j] = saPaymentGatewayPaymentCardInfo[j + 1];

                    break;
                }

            }

        }

        return AcceptedCardTypes;
    }

    private static bool BookingIsGuaranteeOnly(HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations)
    {
        for (int i = 0; i < objHotelBookingPaymentAllocations.Length; i++)
        {
            if (objHotelBookingPaymentAllocations[i].Amount != 0)
                return false;
        }

        return true;
    }

    private static bool BookingIsDepositOnly(HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations)
    {
        for (int i = 0; i < objHotelBookingPaymentAllocations.Length; i++)
        {
            if (objHotelBookingPaymentAllocations[i].Amount == 0)
                return false;
        }

        return true;
    }

    private static bool BookingIsGuaranteeAndDeposit(HotelBookingPaymentAllocation[] objHotelBookingPaymentAllocations)
    {
        if (WBSPGHelper.BookingIsGuaranteeOnly(objHotelBookingPaymentAllocations) || WBSPGHelper.BookingIsDepositOnly(objHotelBookingPaymentAllocations))
            return false;

        return true;
    }

}

