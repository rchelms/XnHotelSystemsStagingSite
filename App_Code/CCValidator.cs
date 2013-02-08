using System;

/// <summary>
/// CCValidator class
/// </summary>
/// 

public class CCValidator
{
    private CCValidator()
    {

    }

    public static bool ValidateType(CardType cardType, string cardNumber)
    {
        byte[] number = new byte[19]; // number to validate

        // Remove non-digits

        int len = 0;

        for (int i = 0; i < cardNumber.Length; i++)
        {
            if (char.IsDigit(cardNumber, i))
            {
                if (len == 19)
                    return false; // number has too many digits

                number[len++] = byte.Parse(cardNumber[i].ToString());
            }

        }

        // Validate based on card type, first "if" tests length, second "if" tests prefix

        switch (cardType)
        {
            case CardType.AmericanExpress:
                if (len != 15)
                    return false;
                if (number[0] != 3 || (number[1] != 4 && number[1] != 7))
                    return false;
                break;

            case CardType.BankCard:
                if (len != 16)
                    return false;
                if (number[0] != 5 || number[1] != 6 || number[2] > 1)
                    return false;
                break;

            case CardType.DinersClub:
                if (len != 14)
                    return false;
                if (number[0] != 3 || (number[1] != 0 && number[1] != 6 && number[1] != 8)
                   || number[1] == 0 && number[2] > 5)
                    return false;
                break;

            case CardType.Discover:
                if (len != 16)
                    return false;
                if (number[0] != 6 || number[1] != 0 || number[2] != 1 || number[3] != 1)
                    return false;
                break;

            case CardType.JCB:
                if (len != 16 && len != 15)
                    return false;
                if (number[0] != 3 || number[1] != 5)
                    return false;
                break;

            case CardType.MaestroSwitch:
                break;

            case CardType.MasterCard:
                if (len != 16)
                    return false;
                if (number[0] != 5 || number[1] == 0 || number[1] > 5)
                    return false;
                break;

            case CardType.Visa:
                if (len != 16 && len != 13)
                    return false;
                if (number[0] != 4)
                    return false;
                break;

            case CardType.Unknown:
                break;

            default:
                return false;
        }

        return true;
    }

    public static bool ValidateNumber(string cardNumber)
    {
        byte[] number = new byte[19]; // number to validate

        // Remove non-digits

        int len = 0;

        for (int i = 0; i < cardNumber.Length; i++)
        {
            if (char.IsDigit(cardNumber, i))
            {
                if (len == 19)
                    return false; // number has too many digits

                number[len++] = byte.Parse(cardNumber[i].ToString());
            }

        }

        // Use Luhn Algorithm to validate

        int sum = 0;

        for (int i = len - 1; i >= 0; i--)
        {
            if (i % 2 == len % 2)
            {
                int n = number[i] * 2;
                sum += (n / 10) + (n % 10);
            }

            else
                sum += number[i];
        }

        return (sum % 10 == 0);
    }

    public static CardType CreditCardType(string strCreditCardCode)
    {
        switch (strCreditCardCode)
        {
            case "AX":
                return CardType.AmericanExpress;

            case "DC":
                return CardType.DinersClub;

            case "DS":
                return CardType.Discover;

            case "JC":
                return CardType.JCB;

            case "CA":
                return CardType.MasterCard;

            case "MC":
                return CardType.MasterCard;

            case "VI":
                return CardType.Visa;

            default:
                return CardType.Unknown;
        }

    }

}

public enum CardType
{
    AmericanExpress,
    BankCard,
    DinersClub,
    Discover,
    JCB,
    MaestroSwitch,
    MasterCard,
    Visa,
    Unknown
}

