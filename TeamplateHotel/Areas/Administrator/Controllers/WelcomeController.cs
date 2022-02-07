using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class WelcomeController : Controller
    {
        //
        // GET: /Administrator/Welcome/

        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang welcome";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<Welcome> list = db.Welcomes.Where(x => x.LanguageID == Request.Cookies["lang_client"].Value).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Welcome model = db.Welcomes.FirstOrDefault(m => m.Id == id);

                if (model != null)
                {
                    return View(model);
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Update(Welcome model, string[] content)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Welcome edit = db.Welcomes.FirstOrDefault(b => b.Id == model.Id);
                        if (edit != null)
                        {
                            edit.BannerAbout = model.BannerAbout;
                            edit.BannerGallery = model.BannerGallery;
                            edit.BannerRoom = model.BannerRoom;
                            edit.BannerRoomPage = model.BannerRoomPage;
                            edit.BannerService = model.BannerService;
                            edit.DescRoom = model.DescRoom;
                            string str = "";
                            if (content != null)
                            {
                                str = string.Join("|", content);
                            }
                            edit.DescTag = str;
                            edit.DescUtilities = model.DescUtilities;
                            edit.TitleTag = model.TitleTag;
                            db.SubmitChanges();

                            string message = "Sửa welcome thành công";
                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                ViewBag.Messages = "Dữ liệu đầu vào không đúng định dạng";
                return View(model);
            }
        }

    }
}
