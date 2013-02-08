using System;
using System.Text;
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

public partial class HotelRatingControl : System.Web.UI.UserControl
{
    private string _RatingProvider;
    private string _Rating;
    private RatingSymbol _RatingSymbol;

    public string RatingProvider
    {
        set
        {
            _RatingProvider = value;
        }

        get
        {
            return _RatingProvider;
        }

    }


    public string Rating
    {
        set
        {
            _Rating = value;
        }

        get
        {
            return _Rating;
        }

    }

    public RatingSymbol RatingSymbol
    {
        set
        {
            _RatingSymbol = value;
        }

        get
        {
            return _RatingSymbol;
        }

    }

    public void RenderUserControl()
    {
        panHotelRating.Visible = false;

        if (_RatingSymbol != RatingSymbol.Unknown)
        {
            panHotelRating.Visible = true;

            if (_RatingProvider == "Self")
            {
                if (_Rating == "1")
                    imgHotelRating.ImageUrl = "~/images/self1.gif";
                else if (_Rating == "1.5")
                    imgHotelRating.ImageUrl = "~/images/self1h.gif";
                else if (_Rating == "2")
                    imgHotelRating.ImageUrl = "~/images/self2.gif";
                else if (_Rating == "2.5")
                    imgHotelRating.ImageUrl = "~/images/self2h.gif";
                else if (_Rating == "3")
                    imgHotelRating.ImageUrl = "~/images/self3.gif";
                else if (_Rating == "3.5")
                    imgHotelRating.ImageUrl = "~/images/self3h.gif";
                else if (_Rating == "4")
                    imgHotelRating.ImageUrl = "~/images/self4.gif";
                else if (_Rating == "4.5")
                    imgHotelRating.ImageUrl = "~/images/self4h.gif";
                else if (_Rating == "5")
                    imgHotelRating.ImageUrl = "~/images/star5.gif";
                else
                    imgHotelRating.ImageUrl = "~/images/space.gif";
            }

            else
            {
                if (_RatingSymbol == RatingSymbol.Star)
                {
                    if (_Rating == "1")
                        imgHotelRating.ImageUrl = "~/images/star1.gif";
                    else if (_Rating == "1.5")
                        imgHotelRating.ImageUrl = "~/images/star1h.gif";
                    else if (_Rating == "2")
                        imgHotelRating.ImageUrl = "~/images/star2.gif";
                    else if (_Rating == "2.5")
                        imgHotelRating.ImageUrl = "~/images/star2h.gif";
                    else if (_Rating == "3")
                        imgHotelRating.ImageUrl = "~/images/star3.gif";
                    else if (_Rating == "3.5")
                        imgHotelRating.ImageUrl = "~/images/star3h.gif";
                    else if (_Rating == "4")
                        imgHotelRating.ImageUrl = "~/images/star4.gif";
                    else if (_Rating == "4.5")
                        imgHotelRating.ImageUrl = "~/images/star4h.gif";
                    else if (_Rating == "5")
                        imgHotelRating.ImageUrl = "~/images/star5.gif";
                    else
                        imgHotelRating.ImageUrl = "~/images/space.gif";
                }

                else if (_RatingSymbol == RatingSymbol.Diamond)
                {
                    imgHotelRating.ImageUrl = "~/images/space.gif";
                }

                else if (_RatingSymbol == RatingSymbol.Self)
                {
                    if (_Rating == "1")
                        imgHotelRating.ImageUrl = "~/images/self1.gif";
                    else if (_Rating == "1.5")
                        imgHotelRating.ImageUrl = "~/images/self1h.gif";
                    else if (_Rating == "2")
                        imgHotelRating.ImageUrl = "~/images/self2.gif";
                    else if (_Rating == "2.5")
                        imgHotelRating.ImageUrl = "~/images/self2h.gif";
                    else if (_Rating == "3")
                        imgHotelRating.ImageUrl = "~/images/self3.gif";
                    else if (_Rating == "3.5")
                        imgHotelRating.ImageUrl = "~/images/self3h.gif";
                    else if (_Rating == "4")
                        imgHotelRating.ImageUrl = "~/images/self4.gif";
                    else if (_Rating == "4.5")
                        imgHotelRating.ImageUrl = "~/images/self4h.gif";
                    else if (_Rating == "5")
                        imgHotelRating.ImageUrl = "~/images/star5.gif";
                    else
                        imgHotelRating.ImageUrl = "~/images/space.gif";
                }

            }

        }

        return;
    }

}
