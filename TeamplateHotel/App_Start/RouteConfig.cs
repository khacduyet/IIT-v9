using System.Web.Mvc;
using System.Web.Routing;

namespace TeamplateHotel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // booking
            routes.MapRoute("Booking2", "Booking/SendBooking", new
            {
                controller = "Booking",
                action = "SendBooking",
            });
            routes.MapRoute("Booking3", "Booking/Messages", new
            {
                controller = "Booking",
                action = "Messages",
            });
            //booking room
            routes.MapRoute("Booking1", "Booking/{id}/{alias}", new
            {
                controller = "Booking",
                action = "MakeReservation",
            });

            //contact
            routes.MapRoute("Contact", "Contact/SubmitContact", new
            {
                controller = "SendContact",
                action = "SubmitContact",
            });
            //Email marketing
            routes.MapRoute("EmailMarketing", "Contact/submitLetter", new
            {
                controller = "SendContact",
                action = "submitLetter",
            });
            routes.MapRoute("search", "tim-kiem", new
            {
                controller = "Search",
                action = "Index",
            });
            routes.MapRoute("search-tag", "tim-kiem/{tag}", new
            {
                controller = "Search",
                action = "searchTag",
            });
            routes.MapRoute("search-categories", "tim-kiem/category/{menuId}", new
            {
                controller = "Search",
                action = "searchCategories",
            });
            //contact
            routes.MapRoute("Contact Messages", "Contact/Messages", new
            {
                controller = "SendContact",
                action = "Messages",
            });
            // comment
            routes.MapRoute("Comments", "Article/postComment", new
            {
                controller = "SendContact",
                action = "postComment",
            });
            // review
            routes.MapRoute("Reviews", "Room/postReview", new
            {
                controller = "SendContact",
                action = "postReview",
            });

            routes.MapRoute("Default", "{aliasMenuSub}/{idSub}/{aliasSub}", new
            {
                controller = "Home",
                action = "Index",
                aliasMenuSub = UrlParameter.Optional,
                idSub = UrlParameter.Optional,
                aliasSub = UrlParameter.Optional
            }
                );
        }
    }
}