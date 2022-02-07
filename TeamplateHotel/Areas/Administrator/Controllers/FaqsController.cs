using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class FaqsController : Controller
    {
        //
        // GET: /Administrator/Faqs/

        public ActionResult Index()
        {
            ViewBag.Title = "Trang FAQs";
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<FAQ> list = db.FAQs.Where(x=>x.LanguageID == Request.Cookies["lang_client"].Value).ToList();
                    var records = list.Select(x => new
                    {
                        Id = x.Id,
                        Content = x.Content,
                        Status = x.Status,
                        Title = x.Title
                    }).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(FAQ model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var insert = new FAQ
                        {
                            Title = model.Title,
                            Content = model.Content,
                            LanguageID = Request.Cookies["lang_client"].Value,
                            Status = true
                        };

                        db.FAQs.InsertOnSubmit(insert);
                        db.SubmitChanges();
                        string message = "Thêm FAQs thành công";
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
        public JsonResult Edit(FAQ model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        FAQ edit = db.FAQs.FirstOrDefault(b => b.Id == model.Id);
                        if (edit != null)
                        {
                            edit.Title = model.Title;
                            edit.Content = model.Content;
                            edit.Status = model.Status;
                            db.SubmitChanges();

                            string message = "Sửa FAQs thành công";
                            return Json(new { Result = "OK", Message = message, Record = model });
                        }
                        return Json(new { Result = "ERROR", Message = "Gallery không tồn tại" });
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
                    FAQ del = db.FAQs.FirstOrDefault(c => c.Id == id);
                    if (del != null)
                    {
                        db.FAQs.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa FAQ thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "FAQ không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = "Error: " + ex.Message });
            }
        }

    }
}
