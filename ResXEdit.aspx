<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResXEdit.aspx.cs" Inherits="resxedit" %>

<%@ Register Src="ResXEditor.ascx" TagName="ResXEditor" TagPrefix="rxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Language Resource Editor</title>
    <style type="text/css">
        body {font-size: 62.5%; color: #222; background: #fff; font-family: Arial, sans-serif;}
        h1, h2, h3, h4, h5, h6 {font-weight: normal; color: #605d5a;}
        h1 {font-size: 2.0em; line-height: 1.0; margin-bottom: 10px;}
        h2 {font-size: 1.5em; line-height: 1.0; margin-bottom: 5px;}
        .rxe_button {float: left; clear: right; background: steelblue; border: #fff none; cursor: pointer; cursor: hand; color: #fff; margin-right: 15px;}
        .rxe_button:hover {float: left; clear: right; background: steelblue; border: #fff none; cursor: pointer; cursor: hand; color: #d6d3d1; margin-right: 15px;}
        .rxe_header {float: left; clear: both; margin-bottom: 15px;}
        .rxe_body {float: left; clear: both}
        .rxe_resource_select {float: left; clear: both; margin-right: 50px; margin-bottom: 15px;}
        .rxe_resource_select span {float: left; clear: right; font-size: 1.5em; margin-bottom: 5px;}
        .rxe_resource_select select {float: left; clear: both; margin-bottom: 15px;}
        .rxe_add_lang {float: left; clear: right;}
        .rxe_add_lang span {float: left; clear: both; font-size: 1.5em; margin-bottom: 5px;}
        .rxe_add_lang_control {float: left; clear: both;}
        .rxe_add_lang_control select {float: left; clear: both; margin-right: 15px;}
        .rxe_add_lang_control input[type=submit] {float: left; clear: right; margin-right: 10px;}
        .rxe_add_lang_control input[type=checkbox] {float: left; clear: right; margin-right: 5px;}
        .rxe_grid {float: left; clear: both;}
        .rxe_grid input[type=submit] {float: left; clear: right; margin-top: 15px; margin-right: 15px; margin-bottom: 15px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <rxe:ResXEditor ID="ResXEditor" runat="server" />
        </div>
    </form>
</body>
</html>
