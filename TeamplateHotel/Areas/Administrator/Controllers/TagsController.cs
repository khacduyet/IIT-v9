using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class TagsController : Controller
    {
        //
        // GET: /Administrator/Tags/

        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang nhãn";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<Tag> tags = db.Tags.Where(x => x.IdLanguage == Request.Cookies["lang_client"].Value).ToList();
                    var records = tags.Select(a => new
                    {
                        a.Id,
                        a.IdLanguage,
                        a.Status,
                        a.TagName,
                        a.DateCreated
                    }).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = tags.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(ETag model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var chk = model.TagName.ToLower();
                        foreach (var item in db.Tags)
                        {
                            if (chk.Equals(item.TagName.ToLower()))
                            {
                                return Json(new { Result = "Error", Message = "Error: Đã tồn tại thẻ này!" });
                            }
                        }
                        var insert = new Tag
                        {
                            IdLanguage = Request.Cookies["lang_client"].Value,
                            DateCreated = DateTime.Now,
                            Status = model.Status,
                            TagName = model.TagName
                        };

                        db.Tags.InsertOnSubmit(insert);
                        db.SubmitChanges();
                        string message = "Thêm tag thành công";
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
        public JsonResult Edit(ETag model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Tag edit = db.Tags.FirstOrDefault(b => b.Id == model.ID);
                        if (edit != null)
                        {
                            edit.TagName = model.TagName;
                            edit.Status = model.Status;
                            db.SubmitChanges();

                            string message = "Sửa tag thành công";
                            return Json(new { Result = "OK", Message = message, Record = model });
                        }
                        return Json(new { Result = "ERROR", Message = "Tag không tồn tại" });
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
                    Tag del = db.Tags.FirstOrDefault(c => c.Id == id);
                    if (del != null)
                    {
                        db.Tags.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa tag thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Tag không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = "Error: " + ex.Message });
            }
        }

    }
}
