using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class TrackingCodeItemControl : System.Web.UI.UserControl
{
   private TrackingCodeType _Type = TrackingCodeType.Unknown;
   private string[] _TrackingCodeParameters = new string[0];
   private string _HotelCode = "";
   private string _PageUrl = "";
   private string _ConfirmNumber = "";
   private decimal _Amount = 0;
   private int _StayNights = 0;

   public TrackingCodeType Type
   {
      get
      {
         return _Type;
      }

      set
      {
         _Type = value;
      }

   }

   public string[] TrackingCodeParameters
   {
      get
      {
         return _TrackingCodeParameters;
      }

      set
      {
         _TrackingCodeParameters = value;
      }

   }

   public string HotelCode
   {
      get
      {
         return _HotelCode;
      }

      set
      {
         _HotelCode = value;
      }

   }

   public string PageUrl
   {
      get
      {
         return _PageUrl;
      }

      set
      {
         _PageUrl = value;
      }

   }

   public string ConfirmNumber
   {
      get
      {
         return _ConfirmNumber;
      }

      set
      {
         _ConfirmNumber = value;
      }

   }

   public decimal Amount
   {
      get
      {
         return _Amount;
      }

      set
      {
         _Amount = value;
      }

   }

   public int StayNight { get { return _StayNights; } set { _StayNights = value; } }

   public void RenderUserControl()
   {
      string script = string.Empty;
      string strPageUrl = _PageUrl;

      switch (_Type)
      {
         case TrackingCodeType.GoogleAnalytics_async:
            if (_TrackingCodeParameters[1] != null && _TrackingCodeParameters[1] != "") // Optional page URL override
               strPageUrl = _TrackingCodeParameters[1];
            if (_TrackingCodeParameters[2] != null && _TrackingCodeParameters[2] != "") // Optional page URL prefix
               strPageUrl = _TrackingCodeParameters[2] + strPageUrl;
            var bCDTrack = (_TrackingCodeParameters[3] != null && _TrackingCodeParameters[3].Length > 0);

            var async = new GoogleAnalyticsAsync(_TrackingCodeParameters[0], strPageUrl, _ConfirmNumber, _Amount, bCDTrack, StayNight);
            script = async.GenerateTrackingCode();

            break;
         case TrackingCodeType.GoogleAnalytics_ga:
            if (_TrackingCodeParameters[1] != null && _TrackingCodeParameters[1] != "") // Optional page URL override
               strPageUrl = _TrackingCodeParameters[1];

            if (_TrackingCodeParameters[2] != null && _TrackingCodeParameters[2] != "") // Optional page URL prefix
               strPageUrl = _TrackingCodeParameters[2] + strPageUrl;

            var ga = new GoogleAnalyticsGa(_TrackingCodeParameters[0], strPageUrl);
            script = ga.GenerateTrackingCode();
            break;
         case TrackingCodeType.GoogleAnalytics_urchin:
            if (_TrackingCodeParameters[1] != null && _TrackingCodeParameters[1] != "") // Optional page URL override
               strPageUrl = _TrackingCodeParameters[1];

            if (_TrackingCodeParameters[2] != null && _TrackingCodeParameters[2] != "") // Optional page URL prefix
               strPageUrl = _TrackingCodeParameters[2] + strPageUrl;

            var urchin = new GoogleAnalyticsUrchin(_TrackingCodeParameters[0], strPageUrl);
            script = urchin.GenerateTrackingCode();
            break;
         case TrackingCodeType.GoogleAdWordsConversion:
            var adwconv = new GoogleAdWordsConversion(_TrackingCodeParameters[0], _TrackingCodeParameters[1], _TrackingCodeParameters[2], _TrackingCodeParameters[3], _TrackingCodeParameters[4], _PageUrl, _Amount);
            script = adwconv.GenerateTrackingCode2(ctnAdword.ClientID);
            break;
         case TrackingCodeType.DoubleClickFloodlight_start:
            var dfas = new DoubleClickFloodlightStart(_TrackingCodeParameters[0], _TrackingCodeParameters[1], _TrackingCodeParameters[2], _TrackingCodeParameters[3]);
            script = dfas.GenerateTrackingCode();
            break;
         case TrackingCodeType.DoubleClickFloodlight_confirm:
            var dfac = new DoubleClickFloodlightConfirm(_TrackingCodeParameters[0], _TrackingCodeParameters[1], _TrackingCodeParameters[2], _TrackingCodeParameters[3], _HotelCode);
            script = dfac.GenerateTrackingCode();
            break;
         case TrackingCodeType.YahooSearchMarketing:
            var ysm = new YahooSearchMarketing(_TrackingCodeParameters[0], _Amount);
            script = ysm.GenerateTrackingCode();
            break;
      }

      if (!string.IsNullOrWhiteSpace(script))
         ScriptManager.RegisterStartupScript(this, GetType(), ClientID, script, false);

      return;
   }

}

public class GoogleAnalyticsAsync
{
   private string strAccountID;
   private string strPageUrl;
   private string strConfirmNumber;
   private decimal decAmount;
   private bool bCDTrack = false;
   private int intStayNights;

   public GoogleAnalyticsAsync(string accountID, string pageUrl, string confirmNumber, decimal amount, bool bCDT, int stayNights = 0)
   {
      strAccountID = accountID;
      strPageUrl = pageUrl;
      strConfirmNumber = confirmNumber;
      decAmount = amount;
      bCDTrack = bCDT;
      intStayNights = stayNights;
      return;
   }

   public string GenerateTrackingCode()
   {
      StringBuilder sb = new StringBuilder();

      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("var _gaq = _gaq || []; ");
      sb.Append("_gaq.push(['_setAccount', ");
      sb.Append("'");
      sb.Append(strAccountID);
      sb.Append("'");
      sb.Append("]); ");
      sb.Append("_gaq.push(['_setDomainName', 'mamashelter.com']); ");

      if (bCDTrack)
      {
         sb.Append("_gaq.push(['_setAllowLinker', true]); ");
      }

      sb.Append("_gaq.push(['_trackPageview', ");
      sb.Append("'");
      sb.Append(strPageUrl);
      sb.Append("'");
      sb.Append("]); ");

      if (decAmount != 0)
      {
         sb.Append("_gaq.push(['_addTrans', ");
         sb.Append("'");
         sb.Append(strConfirmNumber);            // order ID - required
         sb.Append("', ");
         sb.Append("'', ");                      // affiliation or store name 
         sb.Append("'");
         sb.Append(decAmount.ToString("F2"));    // total - required 
         sb.Append("', ");
         sb.Append("'', ");                      // tax 
         sb.Append("'', ");                      // shipping 
         sb.Append("'', ");                      // city 
         sb.Append("'', ");                      // state or province
         sb.Append("''");                        // country 
         sb.Append("]); ");

         sb.Append("_gaq.push(['_addItem ', ");
         sb.Append("'");
         sb.Append(strConfirmNumber);            // order ID - required
         sb.Append("', ");
         sb.Append("'StayNights', ");                      // SKU/code - required 
         sb.Append("'Stay Nights', ");                      // product name - required 
         sb.Append("'', ");                      // category - optional
         sb.Append("'0', ");                      // unit price - required
         sb.Append("'");
         sb.Append(intStayNights.ToString("F2"));    // quantity - required
         sb.Append("', ");
         sb.Append("]); ");

         sb.Append("_gaq.push(['_trackTrans']); ");
      }

      sb.Append("</script>");
      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("(function() { ");
      sb.Append("var ga = document.createElement('script'); ");
      sb.Append("ga.type = 'text/javascript'; ");
      sb.Append("ga.async = true; ");
      sb.Append("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js'; ");
      sb.Append("var s = document.getElementsByTagName('script')[0]; ");
      sb.Append("s.parentNode.insertBefore(ga, s); ");
      sb.Append("})(); ");
      sb.Append("</script>");
      sb.Append("\n");

      return sb.ToString();
   }

}

public class GoogleAnalyticsGa
{
   private string strAccountID;
   private string strPageUrl;

   public GoogleAnalyticsGa(string accountID, string pageUrl)
   {
      strAccountID = accountID;
      strPageUrl = pageUrl;

      return;
   }

   public string GenerateTrackingCode()
   {
      StringBuilder sb = new StringBuilder();

      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("var gaJsHost = ((\"https:\" == document.location.protocol) ? \"https://ssl.\" : \"http://www.\");");
      sb.Append("document.write(unescape(\"%3Cscript src='\" + gaJsHost + \"google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E\"));");
      sb.Append("</script>");
      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("try{var pageTracker = _gat._getTracker(\"");
      sb.Append(strAccountID);
      sb.Append("\");");
      sb.Append("pageTracker._trackPageview(\"");
      sb.Append(strPageUrl);
      sb.Append("\");}");
      sb.Append("catch(err) {}");
      sb.Append("</script>");
      sb.Append("\n");

      return sb.ToString();
   }

}

public class GoogleAnalyticsUrchin
{
   private string strAccountID;
   private string strPageUrl;

   public GoogleAnalyticsUrchin(string accountID, string pageUrl)
   {
      strAccountID = accountID;
      strPageUrl = pageUrl;

      return;
   }

   public string GenerateTrackingCode()
   {
      StringBuilder sb = new StringBuilder();

      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\" src=\"https://ssl.google-analytics.com/urchin.js\"></script>");
      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("_uacct = \"");
      sb.Append(strAccountID);
      sb.Append("\";");
      sb.Append("urchinTracker(\"");
      sb.Append(strPageUrl);
      sb.Append("\");");
      sb.Append("</script>");
      sb.Append("\n");

      return sb.ToString();
   }

}

public class GoogleAdWordsConversion
{
   private string strID;
   private string strLanguage;
   private string strFormat;
   private string strColor;
   private string strLabel;
   private string strPagrUrl;
   private decimal decAmount;

   public GoogleAdWordsConversion(string ID, string language, string format, string color, string label, string pageUrl, decimal amount)
   {
      strID = ID;
      strLanguage = language;
      strFormat = format;
      strColor = color;
      strLabel = label;
      strPagrUrl = pageUrl;
      decAmount = amount;

      return;
   }
   public string GenerateTrackingCode()
   {
      StringBuilder sb = new StringBuilder();

      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("\n");
      sb.Append("<!--");
      sb.Append("\n");
      sb.Append("var google_conversion_id = ");
      sb.Append(strID);
      sb.Append(";");
      sb.Append("\n");
      sb.Append("var google_conversion_language = \"");
      sb.Append(strLanguage);
      sb.Append("\";");
      sb.Append("\n");
      sb.Append("var google_conversion_format = \"");
      sb.Append(strFormat);
      sb.Append("\";");
      sb.Append("\n");
      sb.Append("var google_conversion_color = \"");
      sb.Append(strColor);
      sb.Append("\";");
      sb.Append("\n");
      sb.Append("var google_conversion_label = \"");
      sb.Append(strLabel);
      sb.Append("\";");
      sb.Append("\n");

      if (decAmount != 0)
      {
         sb.Append("var google_conversion_value = \"");
         sb.Append(decAmount.ToString("F2"));
         sb.Append("\";");
         sb.Append("\n");
      }

      sb.Append("-->");
      sb.Append("\n");
      sb.Append("</script>");
      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\"");
      sb.Append("\n");
      sb.Append("src=\"https://www.googleadservices.com/pagead/conversion.js\">");
      sb.Append("\n");
      sb.Append("</script>");
      sb.Append("\n");

      sb.Append("<noscript>");
      sb.Append("\n");
      sb.Append("<div style=\"display:inline;\">");
      sb.Append("\n");
      sb.Append("<img height=\"1\" width=\"1\" style=\"border-style:none;\" alt=\"\"");
      sb.Append("\n");
      sb.Append("src=\"https://www.googleadservices.com/pagead/conversion/");
      sb.Append(strID);
      sb.Append("/?label=");
      sb.Append(strLabel);
      sb.Append("&guid=ON&script=0\"/>");
      sb.Append("\n");
      sb.Append("</div>");
      sb.Append("\n");
      sb.Append("</noscript>");
      sb.Append("\n");
      return sb.ToString();
   }

   public string GenerateTrackingCode2(string controlId)
   {
      var sb = new StringBuilder();
      sb.Append("<script>");

      sb.AppendFormat("var google_conversion_id = {0};", strID);
      sb.AppendFormat("var google_conversion_language = \"{0}\";", strLanguage);
      sb.AppendFormat("var google_conversion_format = \"{0}\";", strFormat);
      sb.AppendFormat("var google_conversion_col = \"{0}\";", strLabel);

      if (decAmount != 0)
         sb.AppendFormat("var google_conversion_value = \"{0}\";", decAmount.ToString("F2"));

      if (!string.IsNullOrWhiteSpace(controlId))
      {
         sb.AppendFormat("var controlEle = document.getElementById(\"{0}\");", controlId);
         sb.Append("var divEle = document.createElement(\"div\");");
         sb.Append("divEle.style=\"display:inline;\";");
         sb.Append("var imgEle = document.createElement(\"img\");");
         sb.Append("imgEle.width = \"1\"; imgEle.height=\"1\"; imgEle.style = \"border-style:none\"; imgEle.alt=\"\";");
         sb.AppendFormat("imgEle.src = \"{0}{1}/?label={2}&guid=ON&script=0\";", "https://www.googleadservices.com/pagead/conversion/", strID, strLabel);
         sb.Append("divEle.appendChild(imgEle);");
         sb.Append("controlEle.appendChild(divEle);");
      }

      sb.Append("</script>");
      return sb.ToString();
   }

}

public class DoubleClickFloodlightStart
{
   private string strSrc;
   private string strType;
   private string strCat;
   private string strOrd;

   public DoubleClickFloodlightStart(string src, string type, string cat, string ord)
   {
      strSrc = src;
      strType = type;
      strCat = cat;
      strOrd = ord;

      return;
   }

   public string GenerateTrackingCode()
   {
      StringBuilder sb = new StringBuilder();

      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("\n");
      sb.Append("var axel = Math.random() + \"\";");
      sb.Append("\n");
      sb.Append("var a = axel * 10000000000000;");
      sb.Append("\n");
      sb.Append("document.write('<iframe src=\"https://fls.au.doubleclick.net/activityi;");
      sb.Append("src=");
      sb.Append(strSrc);
      sb.Append(";");
      sb.Append("type=");
      sb.Append(strType);
      sb.Append(";");
      sb.Append("cat=");
      sb.Append(strCat);
      sb.Append(";");
      sb.Append("ord=");
      sb.Append(strOrd);
      sb.Append(";");
      sb.Append("num=' + a + '?\" width=\"1\" height=\"1\" frameborder=\"0\"></iframe>');");
      sb.Append("\n");
      sb.Append("</script>");
      sb.Append("\n");

      sb.Append("<noscript>");
      sb.Append("\n");
      sb.Append("<iframe src=\"https://fls.au.doubleclick.net/activityi;");
      sb.Append("src=");
      sb.Append(strSrc);
      sb.Append(";");
      sb.Append("type=");
      sb.Append(strType);
      sb.Append(";");
      sb.Append("cat=");
      sb.Append(strCat);
      sb.Append(";");
      sb.Append("ord=");
      sb.Append(strOrd);
      sb.Append(";");
      sb.Append("num=1?\" width=\"1\" height=\"1\" frameborder=\"0\"></iframe>");
      sb.Append("\n");
      sb.Append("</noscript>");
      sb.Append("\n");

      return sb.ToString();
   }

}

public class DoubleClickFloodlightConfirm
{
   private string strSrc;
   private string strType;
   private string strCat;
   private string strOrd;
   private string strHotelCode;

   public DoubleClickFloodlightConfirm(string src, string type, string cat, string ord, string hotelcode)
   {
      strSrc = src;
      strType = type;
      strCat = cat;
      strOrd = ord;
      strHotelCode = hotelcode;

      return;
   }

   public string GenerateTrackingCode()
   {
      StringBuilder sb = new StringBuilder();

      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("\n");
      sb.Append("var axel = Math.random() + \"\";");
      sb.Append("\n");
      sb.Append("var a = axel * 10000000000000;");
      sb.Append("\n");
      sb.Append("document.write('<iframe src=\"https://fls.au.doubleclick.net/activityi;");
      sb.Append("src=");
      sb.Append(strSrc);
      sb.Append(";");
      sb.Append("type=");
      sb.Append(strType);
      sb.Append(";");
      sb.Append("cat=");
      sb.Append(strCat);
      sb.Append(";");
      sb.Append("u1=");
      sb.Append(strHotelCode);
      sb.Append(";");
      sb.Append("ord=");
      sb.Append(strOrd);
      sb.Append(";");
      sb.Append("num=' + a + '?\" width=\"1\" height=\"1\" frameborder=\"0\"></iframe>');");
      sb.Append("\n");
      sb.Append("</script>");
      sb.Append("\n");

      sb.Append("<noscript>");
      sb.Append("\n");
      sb.Append("<iframe src=\"https://fls.au.doubleclick.net/activityi;");
      sb.Append("src=");
      sb.Append(strSrc);
      sb.Append(";");
      sb.Append("type=");
      sb.Append(strType);
      sb.Append(";");
      sb.Append("cat=");
      sb.Append(strCat);
      sb.Append(";");
      sb.Append("u1=");
      sb.Append(strHotelCode);
      sb.Append(";");
      sb.Append("ord=");
      sb.Append(strOrd);
      sb.Append(";");
      sb.Append("num=1?\" width=\"1\" height=\"1\" frameborder=\"0\"></iframe>");
      sb.Append("\n");
      sb.Append("</noscript>");
      sb.Append("\n");

      return sb.ToString();
   }

}

public class YahooSearchMarketing
{
   private string strAccountID;
   private decimal decAmount;

   public YahooSearchMarketing(string accountID, decimal amount)
   {
      strAccountID = accountID;
      decAmount = amount;

      return;
   }

   public string GenerateTrackingCode()
   {
      StringBuilder sb = new StringBuilder();

      sb.Append("\n");

      sb.Append("<script type=\"text/javascript\">");
      sb.Append("\n");
      sb.Append("<!--");
      sb.Append("\n");
      sb.Append("window.ysm_customData = new Object();");
      sb.Append("\n");
      sb.Append("window.ysm_customData.conversion = \"transId=,currency=,");
      sb.Append("amount=");
      sb.Append(decAmount.ToString("F2"));
      sb.Append("\";");
      sb.Append("\n");
      sb.Append("var ysm_accountid = \"");
      sb.Append(strAccountID);
      sb.Append("\";");
      sb.Append("\n");
      sb.Append("document.write(\"<SCR\" + \"IPT language='JavaScript' type='text/javascript' \" + \"SRC=//\" + \"srv2.wa.marketingsolutions.yahoo.com\" + \"/script/ScriptServlet\" + \"?aid=\" + ysm_accountid + \"></SCR\" + \"IPT>\");");
      sb.Append("\n");
      sb.Append("// -->");
      sb.Append("\n");
      sb.Append("</script>");
      sb.Append("\n");

      return sb.ToString();
   }

}
