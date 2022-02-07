using ProjectLibrary.Config;
using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /Administrator/About/

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            return View();
        }
        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            using (var db = new MyDbDataContext())
            {
                try
                {
                    var listMenuShow = (from a in db.Abouts
                                        where a.LanguageID == Request.Cookies["lang_client"].Value
                                        select new EAbout
                                        {
                                            ID = a.Id,
                                            LanguageID = a.LanguageID,
                                            Description = a.Description,
                                            Image = a.Image,
                                            Title = a.Title_,
                                            Image1 = a.Image1,
                                            Image2 = a.Image2,
                                            DescriptionBanner = a.DescriptionBanner,
                                            DescriptionBase = a.DescriptionBase,
                                            DescriptionReview = a.DescriptionReview,
                                            SignImage = a.SignImage,
                                            TitleBanner = a.TitleBanner,
                                            TitleBase = a.TitleBase,
                                            TitleButton = a.TitleButton,
                                            TitleReview = a.TitleReview
                                        }).ToList();
                    return Json(new { Result = "OK", Records = listMenuShow, TotalRecordCount = listMenuShow.Count });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", message = ex.Message });
                }
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                About ab = db.Abouts.FirstOrDefault(m => m.Id == id);

                if (ab != null)
                {
                    var model = new EAbout
                    {
                        ID = ab.Id,
                        Description = ab.Description,
                        Image = ab.Image,
                        LanguageID = ab.LanguageID,
                        Title = ab.Title_,
                        Image2 = ab.Image2,
                        Image1 = ab.Image1,
                        TitleReview = ab.TitleReview,
                        TitleButton = ab.TitleButton,
                        TitleBase = ab.TitleBase,
                        TitleBanner = ab.TitleBanner,
                        SignImage = ab.SignImage,
                        ExternalPath = ab.ExternalPath,
                        DescriptionBanner = ab.DescriptionBanner,
                        DescriptionBase = ab.DescriptionBase,
                        DescriptionReview = ab.DescriptionReview,
                        External = ab.External.Value
                    };
                    ListData();
                    return View(model);
                }
                ListData();
                TempData["Messages"] = "Chuyên mục không tồn tại";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Update(EAbout model)
        {
            //Kiểm tra xem alias thuộc hotel này đã tồn tại chưa
            var db = new MyDbDataContext();
            if (ModelState.IsValid)
            {
                try
                {
                    About edit = db.Abouts.FirstOrDefault(m => m.Id == model.ID);

                    if (edit != null)
                    {
                        edit.Image = model.Image;
                        edit.LanguageID = Request.Cookies["lang_client"].Value;
                        edit.Title_ = model.Title;
                        edit.Description = model.Description;
                        edit.Image2 = model.Image2;
                        edit.Image1 = model.Image1;
                        edit.TitleReview = model.TitleReview;
                        edit.TitleButton = model.TitleButton;
                        edit.TitleBase = model.TitleBase;
                        edit.TitleBanner = model.TitleBanner;
                        edit.SignImage = model.SignImage;
                        edit.menuId = model.menuId;
                        edit.ExternalPath = model.ExternalPath;
                        edit.DescriptionBanner = model.DescriptionBanner;
                        edit.DescriptionBase = model.DescriptionBase;
                        edit.DescriptionReview = model.DescriptionReview;
                        edit.External = model.External;
                        edit.LanguageID = Request.Cookies["lang_client"].Value;
                        db.SubmitChanges();

                        TempData["Messages"] = "Sửa about thành công";
                        return RedirectToAction("Index");
                    }
                    SystemMenuLocation menulocation2 = GetLocaltion(0);
                    TempData["Messages"] = "About không tồn tại";
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ListData();
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View(model);
                }
            }
            ListData();
            ViewBag.Messages = "Dữ liệu đầu vào không đúng định dạng";
            return View(model);
        }

        public SystemMenuLocation GetLocaltion(int locationId)
        {
            SystemMenuLocation menuLocation =
                SystemMenuLocation.ListLocationMenu().ToList().FirstOrDefault(m => m.LocationId == locationId);
            if (menuLocation != null)
            {
                return menuLocation;
            }
            string aliasMenu = Request.QueryString["location"];
            if (string.IsNullOrEmpty(aliasMenu) == false)
            {
                return SystemMenuLocation.ListLocationMenu().ToList().FirstOrDefault(m => m.AliasMenu == aliasMenu);
            }
            return new SystemMenuLocation { AliasMenu = "MainMenu", TitleMenu = "chuyên mục chính", LocationId = 1 };
        }

        //lấy danh sách menu theo ngôn ngữ, theo hotel, theo vị trí, theo AllHotel
        public static List<Menu> GetListMenu(int locationId, string languageID)
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> listMenuRoot = db.Menus.Where(m => m.LanguageID == languageID).ToList();
                listMenuRoot = listMenuRoot.Where(m => m.ParentID == 0).OrderBy(m => m.Index).ToList();

                listMenuRoot = locationId == 0
                    ? listMenuRoot
                    : listMenuRoot.Where(a => a.Location == locationId).ToList();

                List<Menu> listMenu = listMenuRoot;
                Menu menuMaxLevel = locationId == 0
                    ? db.Menus.OrderByDescending(m => m.Level).FirstOrDefault()
                    : db.Menus.Where(m => m.Location == locationId).OrderByDescending(m => m.Level).FirstOrDefault();

                int level = 0;
                if (menuMaxLevel != null)
                {
                    level = menuMaxLevel.Level;
                }

                if (level > 0)
                {
                    for (int i = 1; i <= level; i++)
                    {
                        var listMenuTemp = new List<Menu>();
                        List<Menu> listMenuByLevel;

                        listMenuByLevel =
                            db.Menus.Where(m => m.Level == i && m.LanguageID == languageID).ToList();
                        listMenuByLevel = locationId == 0
                            ? listMenuByLevel
                            : listMenuByLevel.Where(a => a.Location == locationId).ToList();

                        foreach (Menu menu in listMenu)
                        {
                            listMenuTemp.Add(menu);
                            listMenuTemp.AddRange(listMenuByLevel.Where(m => m.ParentID == menu.ID).ToList());
                        }
                        listMenu = listMenuTemp;
                    }
                }
                else
                {
                    listMenu = listMenuRoot;
                }
                return listMenu;
            }
        }
        //Lấy danh sách typemenu, parentMene
        public void ListData()
        {
            var listTypeMenu = new List<SelectListItem>();
            listTypeMenu.Add(new SelectListItem
            {
                Text = "Chọn kiểu hiển thị",
                Value = "0",
            });
            listTypeMenu.AddRange(SystemMenuType.CategoryType.Select(a => new SelectListItem
            {
                Text = a.Value,
                Value = a.Key.ToString(CultureInfo.InvariantCulture)
            }).ToList());
            ViewBag.ListTypeMenu = listTypeMenu;

            SystemMenuLocation menuLocation = GetLocaltion(0);

            List<Menu> listParentMenu = GetListMenu(menuLocation.LocationId, Request.Cookies["lang_client"].Value);
            foreach (Menu menu in listParentMenu)
            {
                string treeNode = "";
                if (menu.Level > 0)
                {
                    for (int i = 1; i <= menu.Level; i++)
                    {
                        treeNode += "|--";
                    }
                }
                menu.Title = treeNode + menu.Title;
            }
            var selectListMenu = new List<SelectListItem>();

            selectListMenu.AddRange(listParentMenu.Select(a => new SelectListItem
            {
                Text = a.Title,
                Value = a.ID.ToString(CultureInfo.InvariantCulture)
            }).ToList());
            ViewBag.ListParentMenu = selectListMenu;
        }
    }
}

