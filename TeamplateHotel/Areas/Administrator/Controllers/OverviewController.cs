using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class OverviewController : Controller
    {
        //
        // GET: /Administrator/Overview/

        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang Cơ sở";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<Overview> list = db.Overviews.Where(x => x.LanguageID == Request.Cookies["lang_client"].Value).OrderBy(a => a.Id).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(Overview model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var insert = new Overview
                        {
                            Title = model.Title,
                            Content = model.Content,
                            Link = model.Link,
                            LanguageID = Request.Cookies["lang_client"].Value
                        };

                        db.Overviews.InsertOnSubmit(insert);
                        db.SubmitChanges();
                        string message = "Thêm cơ sở thành công";
                        return Json(new { Result = "OK", Message = message, Record = model });
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                    }
                }
                return
                    Json(
                        new
                        {
                            Result = " Error",
                            Errors = ModelState.Errors(),
                            Message = "Dữ liệu đầu vào không đúng định dang"
                        }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(Overview model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Overview edit = db.Overviews.FirstOrDefault(b => b.Id == model.Id);
                        if (edit != null)
                        {
                            edit.Title = model.Title;
                            edit.Content = model.Content;
                            edit.Link = model.Link;
                            db.SubmitChanges();

                            string message = "Sửa cơ sở  thành công";
                            return Json(new { Result = "OK", Message = message, Record = model });
                        }
                        return Json(new { Result = "ERROR", Message = "Cơ sở không tồn tại" });
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                    }
                }
                return
                    Json(
                        new
                        {
                            Result = " Error",
                            Errors = ModelState.Errors(),
                            Message = "Dữ liệu đầu vào không đúng định dạng"
                        }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Overview del = db.Overviews.FirstOrDefault(c => c.Id == id);
                    if (del != null)
                    {
                        db.Overviews.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa cơ sở  thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Cơ sở  không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = "Error: " + ex.Message });
            }
        }

    }
}
