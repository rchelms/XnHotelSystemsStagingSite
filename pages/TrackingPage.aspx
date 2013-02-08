<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrackingPage.aspx.cs" Inherits="pages_TrackingPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tracking Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
    <script type="text/javascript">if (!window.mstag) mstag = { loadTag: function () { }, time: (new Date()).getTime() };</script>
    <script id="mstag_tops" type="text/javascript" src="//flex.atdmt.com/mstag/site/b0aa402d-2306-4165-894c-c6140ddcfb10/mstag.js"></script>
    <script type="text/javascript" language="javascript">
        var querystring = (function (a) {
            if (a == "") return {};
            var b = {};
            for (var i = 0; i < a.length; ++i) {
                var p = a[i].split('=');
                if (p.length != 2) continue;
                b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
            }
            return b;
        })(window.location.search.substr(1).split('&'));

        var stage = querystring["stage"];
        if (stage && stage != "") {
            var actionId = "";
            if (stage == "SelectHotel")
                actionId = "106940";
            else if (stage == "SelectRoom")
                actionId = "106941";
            else if (stage == "SelectExtras")
                actionId = "106942";
            else if (stage == "EnterDetails")
                actionId = "106943";
            else if (stage == "Confirmation")
                actionId = "106944";

            if (actionId != "")
                mstag.loadTag("analytics", { dedup: "1", domainId: "1821264", type: "1", revenue: "", actionid: actionId });
        }
    </script>
</body>
</html>
