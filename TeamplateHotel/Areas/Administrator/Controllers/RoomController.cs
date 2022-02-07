using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class RoomController : BaseController
    {
        // GET: /Administrator/Room/
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang quảng lý phòng";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDbDataContext())
            {
                List<Room> records = db.Rooms.Where(r => r.LanguageID == Request.Cookies["lang_client"].Value).ToList();
                foreach (Room record in records)
                {
                    string itemRoom = Request.Params[string.Format("Sort[{0}].Index", record.ID)];
                    int index;
                    int.TryParse(itemRoom, out index);
                    record.Index = index;
                    db.SubmitChanges();
                }
                TempData["Messages"] = "Sắp xếp phòng thành công";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<Room> list = db.Rooms.Where(a => a.LanguageID == Request.Cookies["lang_client"].Value).ToList();
                    var records = list.Select(a => new
                    {
                        a.ID,
                        a.Title,
                        a.Index,
                        a.Status,
                        a.Home
                    }).OrderBy(a => a.Index).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = list.Count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Thêm phòng";
            var eRoom = new ERoom();
            return View(eRoom);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ERoom model, string[] icon, string[] util)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Alias))
                    {
                        model.Alias = StringHelper.ConvertToAlias(model.Title);
                    }
                    try
                    {
                        string ultilities = getUtilities(icon, util);
                        var room = new Room
                        {
                            LanguageID = Request.Cookies["lang_client"].Value,
                            Title = model.Title,
                            Alias = model.Alias,
                            Image = model.Image,
                            MaxPeople = model.MaxPeople,
                            Price = model.Price.Value,
                            PriceNet = model.PriceNet,
                            Size = model.Size,
                            Bed = model.Bed,
                            Index = 0,
                            Description = model.Description,
                            Content = model.Content,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle,
                            MetaDescription =
                                string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.MetaDescription,
                            Status = model.Status,
                            Home = model.Home,
                            ContentUtilities = model.ContentUtilities,
                            Utilities = ultilities
                        };

                        db.Rooms.InsertOnSubmit(room);
                        db.SubmitChanges();

                        //Thêm hình ảnh cho phòng
                        if (model.EGalleryITems != null)
                        {
                            foreach (EGalleryITem itemGallery in model.EGalleryITems)
                            {
                                var roomGallery = new RoomGallery
                                {
                                    ImageLarge = itemGallery.Image,
                                    ImageSmall = ReturnSmallImage.GetImageSmall(itemGallery.Image),
                                    RoomId = room.ID,
                                };
                                db.RoomGalleries.InsertOnSubmit(roomGallery);
                            }
                            db.SubmitChanges();
                        }

                        TempData["Messages"] = "Thêm phòng thành công.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Room detailRoom = db.Rooms.FirstOrDefault(a => a.ID == id);
                if (detailRoom == null)
                {
                    TempData["Messages"] = "Phòng không tồn tại";
                    return RedirectToAction("Index");
                }
                ViewBag.Title = "Sửa phòng";
                var room = new ERoom
                {
                    ID = detailRoom.ID,
                    Title = detailRoom.Title,
                    Alias = detailRoom.Alias,
                    Image = detailRoom.Image,
                    MaxPeople = detailRoom.MaxPeople,
                    Price = detailRoom.Price,
                    PriceNet = detailRoom.PriceNet,
                    Bed = detailRoom.Bed,
                    Size = detailRoom.Size,
                    Index = detailRoom.Index,
                    Description = detailRoom.Description,
                    Content = detailRoom.Content,
                    MetaTitle = detailRoom.MetaTitle,
                    MetaDescription = detailRoom.MetaDescription,
                    Status = detailRoom.Status,
                    Home = detailRoom.Home,
                    ContentUtilities = detailRoom.ContentUtilities,
                    Utilities = detailRoom.Utilities
                };
                //lấy danh sách hình ảnh
                room.EGalleryITems = db.RoomGalleries.Where(a => a.RoomId == detailRoom.ID).Select(a => new EGalleryITem
                {
                    Image = a.ImageLarge
                }).ToList();
                return View(room);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ERoom model, string[] icon, string[] util)
        {
            using (var db = new MyDbDataContext())
            {
                model.Utilities = getUtilities(icon, util);
                if (ModelState.IsValid)
                {
                    try
                    {
                        Room room = db.Rooms.FirstOrDefault(b => b.ID == model.ID);
                        if (room != null)
                        {
                            room.Title = model.Title;
                            room.Alias = model.Alias;
                            room.Image = model.Image;
                            room.MaxPeople = model.MaxPeople;
                            room.Bed = model.Bed;
                            room.Price = model.Price.Value;
                            room.PriceNet = model.PriceNet;
                            room.Description = model.Description;
                            room.Content = model.Content;
                            room.Size = model.Size;
                            room.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle;
                            room.MetaDescription = string.IsNullOrEmpty(model.MetaDescription)
                                ? model.Title
                                : model.MetaDescription;
                            room.Status = model.Status;
                            room.Home = model.Home;
                            room.ContentUtilities = model.ContentUtilities;
                            room.Utilities = getUtilities(icon, util);

                            db.SubmitChanges();

                            //xóa gallery cho phòng
                            db.RoomGalleries.DeleteAllOnSubmit(db.RoomGalleries.Where(a => a.RoomId == room.ID).ToList());
                            //Thêm hình ảnh cho phòng
                            if (model.EGalleryITems != null)
                            {
                                foreach (EGalleryITem itemGallery in model.EGalleryITems)
                                {
                                    var roomGallery = new RoomGallery
                                    {
                                        ImageLarge = itemGallery.Image,
                                        ImageSmall = ReturnSmallImage.GetImageSmall(itemGallery.Image),
                                        RoomId = room.ID,
                                    };
                                    db.RoomGalleries.InsertOnSubmit(roomGallery);
                                }
                                db.SubmitChanges();
                            }
                            TempData["Messages"] = "Sửa phòng thành công";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    Room del = db.Rooms.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        //xóa hết hình ảnh của phòng này
                        db.RoomGalleries.DeleteAllOnSubmit(db.RoomGalleries.Where(a => a.RoomId == del.ID).ToList());

                        db.Rooms.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa phòng thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Phòng không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public string getUtilities(string[] icon, string[] util)
        {
            string ultilities;
            List<string> arr = new List<string>();
            if (icon == null || util == null)
            {
                return ultilities = "";
            }
            else
            {
                for (int i = 0; i < icon.Length; i++)
                {
                    string str = icon[i] + "~" + util[i];
                    arr.Add(str);
                }
                return ultilities = string.Join("|", arr.ToArray());
            }
        }
    }
}