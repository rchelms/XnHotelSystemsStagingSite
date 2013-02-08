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
/// Hotel Pricing Helper Class
/// </summary>

public static class HotelPricingHelper
{
    public static HotelPricing[] GetHotelPricing(StayCriteriaSelection objStayCriteriaSelection, RoomRateSelection[] objRoomRateSelections, AddOnPackageSelection[] objAllAddOnPackageSelections, HotelRoomAvailInfo[] objHotelRoomAvailInfos, string strCurrenyCode)
    {
        HotelPricing[] objHotelPricing = new HotelPricing[objStayCriteriaSelection.RoomOccupantSelections.Length];

        int intNumberStayNights = ((TimeSpan)objStayCriteriaSelection.DepartureDate.Subtract(objStayCriteriaSelection.ArrivalDate)).Days;

        for (int i = 0; i < objStayCriteriaSelection.RoomOccupantSelections.Length; i++)
        {
            RoomOccupantSelection objRoomOccupantSelection = objStayCriteriaSelection.RoomOccupantSelections[i];
            RoomRateSelection objRoomRateSelection = HotelPricingHelper.GetRoomRateSelection(objRoomOccupantSelection, objRoomRateSelections);
            HotelAvailRatePlan objHotelAvailRatePlan = HotelPricingHelper.GetHotelAvailRatePlan(objRoomRateSelection, objHotelRoomAvailInfos);
            HotelAvailRoomRate objHotelAvailRoomRate = HotelPricingHelper.GetHotelAvailRoomRate(objRoomRateSelection, objHotelRoomAvailInfos);
            AddOnPackageSelection[] objAddOnPackageSelections = HotelPricingHelper.GetAddOnPackageSelections(objRoomRateSelection, objAllAddOnPackageSelections);

            decimal decTotalRoomAmount = 0;
            decimal decTotalRoomTaxes = 0; // Exclusive taxes only
            decimal decTotalRoomFees = 0; // Exclusive fees only

            for (int j = 0; j < objHotelAvailRoomRate.Rates.Length; j++)
            {
                decTotalRoomAmount += objRoomOccupantSelection.NumberRooms * objHotelAvailRoomRate.Rates[j].Amount * objHotelAvailRoomRate.Rates[j].NumNights;

                for (int k = 0; k < objHotelAvailRoomRate.Rates[j].PerNightTaxesFees.Length; k++)
                {
                    if (objHotelAvailRoomRate.Rates[j].PerNightTaxesFees[k].CategoryType == TaxFeeCategoryType.Tax && objHotelAvailRoomRate.Rates[j].PerNightTaxesFees[k].Type == TaxFeeType.Exclusive)
                    {
                        decTotalRoomTaxes += objHotelAvailRoomRate.Rates[j].PerNightTaxesFees[k].Amount;
                    }

                    else if (objHotelAvailRoomRate.Rates[j].PerNightTaxesFees[k].CategoryType == TaxFeeCategoryType.Fee && objHotelAvailRoomRate.Rates[j].PerNightTaxesFees[k].Type == TaxFeeType.Exclusive)
                    {
                        decTotalRoomFees += objHotelAvailRoomRate.Rates[j].PerNightTaxesFees[k].Amount;
                    }

                }

            }

            decTotalRoomAmount += decTotalRoomTaxes + decTotalRoomFees;

            decimal decTotalPackageAmount = 0;
            decimal decTotalPackageDeposit = 0;
            decimal decTotalPackageTaxes = 0; // Exclusive taxes only
            decimal decTotalPackageFees = 0; // Exclusive fees only

            for (int j = 0; j < objAddOnPackageSelections.Length; j++)
            {
                HotelAvailPackage objHotelAvailPackage = HotelPricingHelper.GetHotelAvailPackage(objAddOnPackageSelections[j], objRoomRateSelection, objHotelAvailRatePlan);

                if (objHotelAvailPackage.PriceType == PackagePriceType.PerStayPerNight || objHotelAvailPackage.PriceType == PackagePriceType.PerPersonPerNight)
                {
                    decTotalPackageAmount += objAddOnPackageSelections[j].Quantity * objHotelAvailPackage.Price * intNumberStayNights;
                    decTotalPackageDeposit += objAddOnPackageSelections[j].Quantity * objHotelAvailPackage.Deposit * intNumberStayNights;
                }

                else
                {
                    decTotalPackageAmount += objAddOnPackageSelections[j].Quantity * objHotelAvailPackage.Price;
                    decTotalPackageDeposit += objAddOnPackageSelections[j].Quantity * objHotelAvailPackage.Deposit;
                }

            }

            decimal decDepositTaxFactor = 0;

            if (decTotalPackageAmount != 0)
                decDepositTaxFactor = decTotalPackageDeposit / decTotalPackageAmount;

            decTotalPackageAmount += decTotalPackageTaxes + decTotalPackageFees;
            decTotalPackageDeposit += decimal.Round(((decTotalPackageTaxes + decTotalPackageFees) * decDepositTaxFactor), 2);

            decimal decTotalAdditionalTaxes = 0; // Exclusive taxes only
            decimal decTotalAdditionalFees = 0; // Exclusive fees only

            for (int j = 0; j < objHotelAvailRoomRate.PerStayTaxesFees.Length; j++)
            {
                if (objHotelAvailRoomRate.PerStayTaxesFees[j].CategoryType == TaxFeeCategoryType.Tax && objHotelAvailRoomRate.PerStayTaxesFees[j].Type == TaxFeeType.Exclusive)
                {
                    decTotalAdditionalTaxes += objHotelAvailRoomRate.PerStayTaxesFees[j].Amount;
                }

                else if (objHotelAvailRoomRate.PerStayTaxesFees[j].CategoryType == TaxFeeCategoryType.Fee && objHotelAvailRoomRate.PerStayTaxesFees[j].Type == TaxFeeType.Exclusive)
                {
                    decTotalAdditionalFees += objHotelAvailRoomRate.PerStayTaxesFees[j].Amount;
                }

            }

            objHotelPricing[i] = new HotelPricing();
            objHotelPricing[i].SegmentRefID = objRoomOccupantSelection.RoomRefID;
            objHotelPricing[i].CurrencyCode = strCurrenyCode;

            objHotelPricing[i].TotalRoomTaxes = decTotalRoomTaxes;
            objHotelPricing[i].TotalRoomFees = decTotalRoomFees;
            objHotelPricing[i].TotalRoomAmount = decTotalRoomAmount;

            objHotelPricing[i].TotalPackageTaxes = decTotalPackageTaxes;
            objHotelPricing[i].TotalPackageFees = decTotalPackageFees;
            objHotelPricing[i].TotalPackageAmount = decTotalPackageAmount;

            objHotelPricing[i].TotalAdditionalTaxes = decTotalAdditionalTaxes;
            objHotelPricing[i].TotalAdditionalFees = decTotalAdditionalFees;

            if (ConfigurationManager.AppSettings["HotelPricing.IncludeAddOnPackageAmountsInTotal"] == "1")
                objHotelPricing[i].TotalAmount = decTotalRoomAmount + decTotalPackageAmount + decTotalAdditionalTaxes + decTotalAdditionalFees;
            else
                objHotelPricing[i].TotalAmount = decTotalRoomAmount + decTotalAdditionalTaxes + decTotalAdditionalFees;

            //objHotelPricing[i].TotalDeposit = HotelPricingHelper.GetDepositAmount(objHotelAvailRatePlan, decTotalRoomAmount + decTotalPackageDeposit + decTotalAdditionalTaxes + decTotalAdditionalFees, intNumberStayNights);
            objHotelPricing[i].TotalDeposit = HotelPricingHelper.GetDepositAmount(objHotelAvailRatePlan, objHotelPricing[i].TotalAmount, intNumberStayNights);
        }

        return objHotelPricing;
    }

    private static decimal GetDepositAmount(HotelAvailRatePlan RatePlan, decimal decRoomCost, int intNumStayNights)
    {
        decimal decDepositAmount = 0;

        if (RatePlan.GuaranteeType == GuaranteeType.Deposit || RatePlan.GuaranteeType == GuaranteeType.PrePay)
        {
            if (RatePlan.DepositRequiredAmount != 0)
            {
                decDepositAmount = RatePlan.DepositRequiredAmount;
            }

            else if (RatePlan.DepositRequiredNumberNights != 0)
            {
                int intNumRequiredNights = RatePlan.DepositRequiredNumberNights;

                if (RatePlan.DepositRequiredNumberNights > intNumStayNights)
                    intNumRequiredNights = intNumStayNights;

                decDepositAmount = decRoomCost / intNumStayNights;
                decDepositAmount = decDepositAmount * intNumRequiredNights;
                decDepositAmount = decimal.Round(decDepositAmount, 2);
            }

            else if (RatePlan.DepositRequiredPercent != 0)
            {
                decDepositAmount = (decRoomCost * RatePlan.DepositRequiredPercent) / 100;
                decDepositAmount = decimal.Round(decDepositAmount, 2);
            }

        }

        return decDepositAmount;
    }

    private static RoomRateSelection GetRoomRateSelection(RoomOccupantSelection objRoomOccupantSelection, RoomRateSelection[] objRoomRateSelections)
    {
        for (int i = 0; i < objRoomRateSelections.Length; i++)
        {
            if (objRoomRateSelections[i].RoomRefID == objRoomOccupantSelection.RoomRefID)
                return objRoomRateSelections[i];
        }

        return null;
    }

    private static AddOnPackageSelection[] GetAddOnPackageSelections(RoomRateSelection objRoomRateSelection, AddOnPackageSelection[] objAddOnPackageSelections)
    {
        List<AddOnPackageSelection> lAddOnPackageSelections = new List<AddOnPackageSelection>();

        for (int i = 0; i < objAddOnPackageSelections.Length; i++)
        {
            if (objAddOnPackageSelections[i].RoomRefID == objRoomRateSelection.RoomRefID)
                lAddOnPackageSelections.Add(objAddOnPackageSelections[i]);
        }

        return lAddOnPackageSelections.ToArray();
    }

    private static HotelAvailRoomRate GetHotelAvailRoomRate(RoomRateSelection objRoomRateSelection, HotelRoomAvailInfo[] objHotelRoomAvailInfos)
    {
        for (int i = 0; i < objHotelRoomAvailInfos.Length; i++)
        {
            if (objHotelRoomAvailInfos[i].SegmentRefID == objRoomRateSelection.RoomRefID)
            {
                for (int j = 0; j < objHotelRoomAvailInfos[i].RoomRates.Length; j++)
                {
                    if (objHotelRoomAvailInfos[i].RoomRates[j].RoomTypeCode == objRoomRateSelection.RoomTypeCode && objHotelRoomAvailInfos[i].RoomRates[j].RatePlanCode == objRoomRateSelection.RatePlanCode)
                    {
                        return objHotelRoomAvailInfos[i].RoomRates[j];
                    }

                }

            }

        }

        return null;
    }

    private static HotelAvailRatePlan GetHotelAvailRatePlan(RoomRateSelection objRoomRateSelection, HotelRoomAvailInfo[] objHotelRoomAvailInfos)
    {
        for (int i = 0; i < objHotelRoomAvailInfos.Length; i++)
        {
            if (objHotelRoomAvailInfos[i].SegmentRefID == objRoomRateSelection.RoomRefID)
            {
                for (int j = 0; j < objHotelRoomAvailInfos[i].RatePlans.Length; j++)
                {
                    if (objHotelRoomAvailInfos[i].RatePlans[j].Code == objRoomRateSelection.RatePlanCode)
                    {
                        return objHotelRoomAvailInfos[i].RatePlans[j];
                    }

                }

            }

        }

        return null;
    }

    private static HotelAvailPackage GetHotelAvailPackage(AddOnPackageSelection objAddOnPackageSelection, RoomRateSelection objRoomRateSelection, HotelAvailRatePlan objHotelAvailRatePlan)
    {
        for (int i = 0; i < objHotelAvailRatePlan.Packages.Length; i++)
        {
            if (objHotelAvailRatePlan.Packages[i].Code == objAddOnPackageSelection.PackageCode)
            {
                if (objHotelAvailRatePlan.Packages[i].RoomTypeCode == "" || objHotelAvailRatePlan.Packages[i].RoomTypeCode == objRoomRateSelection.RoomTypeCode)
                {
                    return objHotelAvailRatePlan.Packages[i];
                }

            }

        }

        return null;
    }

}
