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
    public class CounterController : Controller
    {
        //
        // GET: /Administrator/Counter/

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
                    var listMenuShow = (from a in db.Counters
                                        where a.LanguageID == Request.Cookies["lang_client"].Value
                                        select new ECounter
                                        {
                                            ID = a.Id,
                                            LanguageID = a.LanguageID,
                                            Image = a.Image,
                                            TitleButton = a.TitleButton,
                                            Counter = a.Counter1,
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
                Counter cnt = db.Counters.FirstOrDefault(m => m.Id == id);

                if (cnt != null)
                {
                    var model = new ECounter
                    {
                        ID = cnt.Id,
                        Image = cnt.Image,
                        LanguageID = cnt.LanguageID,
                        TitleButton = cnt.TitleButton,
                        ExternalPath = cnt.ExternalPath,
                        Counter = cnt.Counter1,
                        menuId = cnt.menuId.Value,
                        External = cnt.External.Value
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
        public ActionResult Update(ECounter model, string[] title, string[] quantity, string[] desc)
        {
            //Kiểm tra xem alias thuộc hotel này đã tồn tại chưa
            var db = new MyDbDataContext();
            model.Counter = getCounter(title, quantity, desc);
            if (ModelState.IsValid)
            {
                try
                {
                    for (int i = 0; i < title.Length; i++)
                    {
                        if (title[i] == "" || quantity[i] == "" || desc[i] == "")
                        {
                            ListData();
                            ModelState.AddModelError("Counter", "Mời điền đầy đủ vào bộ đếm");
                            return View(model);
                        }
                    }

                    Counter edit = db.Counters.FirstOrDefault(m => m.Id == model.ID);

                    if (edit != null)
                    {
                        edit.Image = model.Image;
                        edit.LanguageID = Request.Cookies["lang_client"].Value;
                        edit.TitleButton = model.TitleButton;
                        edit.menuId = model.menuId;
                        edit.ExternalPath = model.ExternalPath;
                        edit.External = model.External;
                        edit.Counter1 = model.Counter;
                        db.SubmitChanges();

                        TempData["Messages"] = "Sửa counter thành công";
                        return RedirectToAction("Index");
                    }
                    SystemMenuLocation menulocation2 = GetLocaltion(0);
                    TempData["Messages"] = "Counter không tồn tại";
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
        public string getCounter(string[] title, string[] quantity, string[] desc)
        {
            string counter;
            List<string> arr = new List<string>();
            if (title == null || quantity == null || desc == null)
            {
                return counter = "";
            }
            else
            {
                for (int i = 0; i < title.Length; i++)
                {
                    string str = title[i] + "~" + quantity[i] + "~" + desc[i];
                    arr.Add(str);
                }
                return counter = string.Join("|", arr.ToArray());
            }
        }
    }
}
