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
    public class TabTourCategoryController : BaseController
    {
        //
        // GET: /Administrator/TabTourCategory/

        public ActionResult Index()
        {
            LoadData();
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang Tab Tour Category";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDbDataContext())
            {
                List<TabTourCategory> records = db.TabTourCategories.ToList();
                foreach (TabTourCategory record in records)
                {
                    string itemTour = Request.Params[string.Format("Sort[{0}].Index", record.ID)];
                    int index;
                    int.TryParse(itemTour, out index);
                    db.SubmitChanges();
                }
                TempData["Messages"] = "Sắp xếp thành công";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult List(int menuId = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var db = new MyDbDataContext();
                var listQS = menuId == 0
                    ? db.TabTourCategories.Join(db.Menus.Where(m => m.LanguageID == Request.Cookies["lang_client"].Value),
                        a => a.MenuID, b => b.ID, (a, b) => new { a, b }).ToList()
                    : db.TabTourCategories.Join(
                        db.Menus.Where(m => m.ID == menuId && m.LanguageID == Request.Cookies["lang_client"].Value),
                        a => a.MenuID, b => b.ID, (a, b) => new { a, b }).ToList();

                var records = listQS.Select(a => new
                {
                    a.a.ID,
                    a.a.Title,
                    titleMenu = a.b.Title,
                }).Skip(jtStartIndex).Take(jtPageSize).OrderBy(a => a.ID).ToList();
                //Return result to jTable
                return Json(new { Result = "OK", Records = records, TotalRecordCount = listQS.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Thêm mới Tab Tour Category";
            LoadData();
            var tabtour = new ETabTourCategory();
            return View(tabtour);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ETabTourCategory model)
        {
            using (var db = new MyDbDataContext())
            {
                //Kiểm tra xem đã chọn đến chuyên mục con cuối cùng chưa
                //if (db.Menus.Any(a => a.ParentID == model.MenuID))
                //{
                //    ModelState.AddModelError("MenuId", "Phải chọn đến chuyên mục con cuối cùng");
                //}

                if (ModelState.IsValid)
                {
                    try
                    {
                        var tab = new TabTourCategory
                        {
                            MenuID = model.MenuID,
                            Title = model.Title,
                            Content = model.Content
                        };
                        db.TabTourCategories.InsertOnSubmit(tab);
                        db.SubmitChanges();

                        TempData["Messages"] = "Thêm mới Tab Tour Category thành công";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        LoadData();
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                LoadData();
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.Title = "Cập nhật Tab Tour Category";
            var db = new MyDbDataContext();
            TabTourCategory DetailTabTourCategory = db.TabTourCategories.FirstOrDefault(a => a.ID == id);
            if (DetailTabTourCategory == null)
            {
                TempData["Messages"] = "Tab Tour Category không tồn tại";
                return RedirectToAction("Index");
            }
            LoadData();
            var tab = new ETabTourCategory
            {
                ID = DetailTabTourCategory.ID,
                MenuID = DetailTabTourCategory.MenuID,
                Title = DetailTabTourCategory.Title,
                Content = DetailTabTourCategory.Content,
            };
            return View(tab);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ETabTourCategory model)
        {
            //Kiểm tra xem alias thuộc tour này đã tồn tại chưa
            var db = new MyDbDataContext();

            //Kiểm tra xem đã chọn đến chuyên mục con cuối cùng chưa
            //if (db.Menus.Any(a => a.ParentID == model.ID))
            //{
            //    ModelState.AddModelError("MenuId", "Vui lòng chọn đến chuyên mục con cuối cùng");
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    TabTourCategory tab = db.TabTourCategories.FirstOrDefault(b => b.ID == model.ID);
                    if (tab != null)
                    {
                        tab.MenuID = model.MenuID;
                        tab.Title = model.Title;
                        tab.Content = model.Content;
                        db.SubmitChanges();
                        TempData["Messages"] = "Sửa Tab Tour Category thành công";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception exception)
                {
                    LoadData();
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View();
                }
            }
            LoadData();
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    TabTourCategory del = db.TabTourCategories.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.TabTourCategories.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa Tab Tour Category thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Tab Tour Category này không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public void LoadData()
        {
            var listMenu = new List<SelectListItem>();
            listMenu.Add(new SelectListItem { Value = "0", Text = "---Select a menu---" });
            List<Menu> getListMenu =
                MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId,
                    Request.Cookies["lang_client"].Value).ToList();

            foreach (Menu menu in getListMenu)
            {
                string subTitle = "";
                for (int i = 1; i <= menu.Level; i++)
                {
                    subTitle += "|--";
                }
                menu.Title = subTitle + menu.Title;
            }
            listMenu.AddRange(getListMenu.Select(a => new SelectListItem
            {
                Value =
                    a.ID.ToString(
                        CultureInfo.InvariantCulture),
                Text = a.Title
            }).ToList());
            ViewBag.ListMenu = listMenu;
        }
    }
}