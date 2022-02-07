using ProjectLibrary.Database;
using System;
using System.Linq;
using System.Web.Mvc;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class ReviewController : Controller
    {
        //
        // GET: /Administrator/Review/

        public ActionResult Index()
        {
            using (var db = new MyDbDataContext())
            {
                ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
                ViewBag.Title = "Trang đánh giá";
                return View();
            }
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var listRv = db.Reviews.ToList();
                    var records = listRv.Where(x => x.LanguageID == Request.Cookies["lang_client"].Value).Select(a => new
                    {
                        Id = a.Id,
                        Comment = a.Comment,
                        FullName = a.FullName,
                        Status = a.Status,
                        DateCreated = a.DateCreated,
                        Image = a.Image,
                        Position = a.Position
                    }).OrderBy(a => a.Id).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listRv.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(Review model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var insert = new Review
                        {
                            FullName = model.FullName,
                            Comment = model.Comment,
                            DateCreated = DateTime.Now,
                            Status = true,
                            Image = model.Image,
                            Position = model.Position,
                            LanguageID = Request.Cookies["lang_client"].Value
                        };

                        db.Reviews.InsertOnSubmit(insert);
                        db.SubmitChanges();
                        string message = "Thêm review thành công";
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
        public JsonResult Edit(Review model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Review edit = db.Reviews.FirstOrDefault(b => b.Id == model.Id);
                        if (edit != null)
                        {
                            edit.FullName = model.FullName;
                            edit.Comment = model.Comment;
                            edit.Image = model.Image;
                            edit.Position = model.Position;
                            edit.Status = model.Status;
                            db.SubmitChanges();

                            string message = "Sửa review thành công";
                            return Json(new { Result = "OK", Message = message, Record = model });
                        }
                        return Json(new { Result = "ERROR", Message = "review không tồn tại" });
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
                    Review del = db.Reviews.FirstOrDefault(c => c.Id == id);
                    if (del != null)
                    {
                        db.Reviews.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa đánh giá thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Đánh giá không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public JsonResult Approved(int id = 0, bool val = true)
        {
            var db = new MyDbDataContext();
            try
            {
                Review cmt = db.Reviews.FirstOrDefault(b => b.Id == id);
                if (cmt != null)
                {
                    cmt.Status = !val;
                    db.SubmitChanges();
                    return Json(new { Result = "The Review status update was successfull" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = "Không tồn tại" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR " + ex.Message });
            }
        }

    }
}
