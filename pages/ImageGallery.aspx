<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageGallery.aspx.cs" Inherits="ImageGallery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Image Gallery</title>
    <link href="../css/Special/image_gallery.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <div id="xngr_wbs_image_gallery">
        <form id="main" runat="server">
            <asp:HiddenField ID="hfHotelCode" runat="server" />
            <asp:Panel CssClass="image_gallery_container" runat="server">
                <asp:Panel CssClass="image_gallery_content" runat="server">
                    <asp:Panel CssClass="image_gallery_header" runat="server">
                        <asp:Label ID="lblTitle" runat="server" Text="" />
                    </asp:Panel>
                    <asp:Panel CssClass="image_gallery_image" runat="server">
                        <xnc:XImage ID="imgFullSize" runat="server" Location="HotelMediaCDNPath" />
                    </asp:Panel>
                    <asp:Panel CssClass="image_gallery_description" runat="server">
                        <asp:Label ID="lblDescription" runat="server" Text="" />
                    </asp:Panel>
                    <asp:Panel CssClass="image_gallery_thumbstrip" runat="server">
                        <asp:Panel CssClass="image_gallery_thumbstrip_nav" runat="server">
                            <xnc:XImageButton ID="btnStripLeft" runat="server" ImageUrl="~/images/nav-left.gif"
                                OnClick="btnStripLeft_Click" Width="16" Height="16" Location="ImageCDNPath" />
                        </asp:Panel>
                        <asp:Panel ID="panThumb1" CssClass="image_gallery_thumbstrip_thumb" runat="server">
                            <xnc:XImageButton ID="btnThumb1" runat="server" ImageUrl="~/images/image_blank_95x71.gif" OnClick="btnThumb_Click"
                                CommandName="1" Width="95" Height="71" Location="ImageCDNPath" />
                        </asp:Panel>
                        <asp:Panel ID="panThumb2" CssClass="image_gallery_thumbstrip_thumb" runat="server">
                            <xnc:XImageButton ID="btnThumb2" runat="server" ImageUrl="~/images/image_blank_95x71.gif" OnClick="btnThumb_Click"
                                CommandName="2" Width="95" Height="71" Location="ImageCDNPath" />
                        </asp:Panel>
                        <asp:Panel ID="panThumb3" CssClass="image_gallery_thumbstrip_thumb" runat="server">
                            <xnc:XImageButton ID="btnThumb3" runat="server" ImageUrl="~/images/image_blank_95x71.gif" OnClick="btnThumb_Click"
                                CommandName="3" Width="95" Height="71" Location="ImageCDNPath" />
                        </asp:Panel>
                        <asp:Panel ID="panThumb4" CssClass="image_gallery_thumbstrip_thumb" runat="server">
                            <xnc:XImageButton ID="btnThumb4" runat="server" ImageUrl="~/images/image_blank_95x71.gif" OnClick="btnThumb_Click"
                                CommandName="4" Width="95" Height="71" Location="ImageCDNPath" />
                        </asp:Panel>
                        <asp:Panel CssClass="image_gallery_thumbstrip_nav" runat="server">
                            <xnc:XImageButton ID="btnStripRight" runat="server" ImageUrl="~/images/nav-right.gif"
                                OnClick="btnStripRight_Click" Width="16" Height="16" Location="ImageCDNPath" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel CssClass="image_gallery_hotel" runat="server">
                        <asp:Label ID="lblHotelName" runat="server" Text="" />
                    </asp:Panel>
                    <asp:Panel CssClass="image_gallery_nav" runat="server">
                        <asp:Panel CssClass="image_gallery_nav_col1" runat="server">
                            <asp:Label ID="lblImageIdentifier" runat="server" Text="" />
                        </asp:Panel>
                        <asp:Panel CssClass="image_gallery_nav_col2" runat="server">
                            <asp:Panel CssClass="std_button" runat="server">
                                <asp:Panel CssClass="std_button_left" runat="server">
                                    <asp:Panel CssClass="std_button_right" runat="server">
                                        <asp:Panel CssClass="std_button_center" runat="server">
                                            <asp:Button ID="btnNext" CssClass="std_button_control" runat="server" Text="" meta:resourcekey="btnNext"
                                                OnClick="btnNext_Click" />
                                        </asp:Panel>
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel CssClass="std_button" runat="server">
                                <asp:Panel CssClass="std_button_left" runat="server">
                                    <asp:Panel CssClass="std_button_right" runat="server">
                                        <asp:Panel CssClass="std_button_center" runat="server">
                                            <asp:Button ID="btnPrevious" CssClass="std_button_control" runat="server" Text=""
                                                meta:resourcekey="btnPrevious" OnClick="btnPrevious_Click" />
                                        </asp:Panel>
                                    </asp:Panel>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </form>
    </div>
</body>
</html>
