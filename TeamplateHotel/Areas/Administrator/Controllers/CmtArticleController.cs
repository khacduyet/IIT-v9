using ProjectLibrary.Database;
using System;
using System.Linq;
using System.Web.Mvc;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class CmtArticleController : Controller
    {
        //
        // GET: /Administrator/CmtArticle/

        public ActionResult Index(int? id)
        {
            using (var db = new MyDbDataContext())
            {
                ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
                ViewBag.Title = "Trang bình luận";
                ViewBag.ArtId = id;
                return View();
            }
        }

        [HttpPost]
        public JsonResult List(int id, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var listcmt = db.Comments.Where(x => x.BlogId == id).ToList();
                    var records = listcmt.Select(a => new
                    {
                        Id = a.Id,
                        BlogId = a.BlogId,
                        Comment1 = a.Comment1,
                        Email = a.Email,
                        FullName = a.FullName,
                        Status = a.Status,
                        DateCreated = a.DateCreated
                    }).OrderBy(a => a.Id).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listcmt.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Comment del = db.Comments.FirstOrDefault(c => c.Id == id);
                    if (del != null)
                    {
                        db.Comments.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa bình luận thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Bình luận không tồn tại" });
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
                Comment cmt = db.Comments.FirstOrDefault(b => b.Id == id);
                if (cmt != null)
                {
                    cmt.Status = !val;
                    db.SubmitChanges();
                    return Json(new { Result = "The Comment status update was successfull" }, JsonRequestBehavior.AllowGet);
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
