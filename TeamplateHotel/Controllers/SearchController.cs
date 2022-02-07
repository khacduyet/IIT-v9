using PagedList;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeamplateHotel.Handler;

namespace TeamplateHotel.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        public ActionResult Index(string keySearch, string lang, int? number)
        {
            using (var db = new MyDbDataContext())
            {
                lang = Request.Cookies["LanguageID"].Value;
                List<Room> rooms = db.Rooms.Where(x=>x.LanguageID == lang && x.MaxPeople < number.Value).ToList();
                Menu menu = db.Menus.Where(x => x.LanguageID == lang && x.Level == 0 && x.Type == SystemMenuType.RoomRate).Take(1).FirstOrDefault();
                foreach (var room in rooms)
                {
                    room.MenuAlias = menu.Alias;
                    room.roomGalleries = db.RoomGalleries.Where(a => a.RoomId == room.ID).ToList();
                    if (menu.LanguageID.Equals("vi"))
                    {
                        room.Price = room.Price * GetPriceUSD.USDToVND();
                    }
                }
                foreach (var r in rooms)
                {
                    r.MenuAlias = menu.Alias;
                }
                return View(rooms);
            }
        }
        [HttpPost]
        public PartialViewResult searchTag(int menuId, int? tag, int? page)
        {
            using (var db = new MyDbDataContext())
            {
                ViewBag.Menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                page = page ?? 1;
                int pageSize = 3;
                var ACom = db.Comments.Where(x => x.Status).ToList();
                List<Article> articles = new List<Article>();
                var artTag = db.ArticleTags.ToList();
                foreach (var item in artTag)
                {
                    string[] arr = item.Tags.Split(',').ToArray();
                    foreach (var t in arr)
                    {
                        if (t.Equals(tag.ToString()))
                        {
                            articles.Add(db.Articles.Where(x => x.ID == item.ArticleId).FirstOrDefault());
                            break;
                        }
                    }
                }
                ViewBag.ACom = ACom;
                return PartialView("PartialView/ListArticles", articles.ToPagedList(page.Value, pageSize));
            }
        }

        [HttpPost]
        public PartialViewResult searchCategories(int menuId, int? page)
        {
            using (var db = new MyDbDataContext())
            {
                ViewBag.Menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                page = page ?? 1;
                int pageSize = 3;
                var ACom = db.Comments.Where(x => x.Status).ToList();
                List<Article> articles = db.Articles.Where(x => x.Status && x.MenuID == menuId).ToList();
                ViewBag.ACom = ACom;
                return PartialView("PartialView/ListArticles", articles.ToPagedList(page.Value, pageSize));
            }
        }
    }
}
