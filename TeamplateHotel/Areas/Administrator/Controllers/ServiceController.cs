﻿using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Administrator/Service/

        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang service";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var list =
                        db.Services.Join(db.Menus.Where(a => a.LanguageID == Request.Cookies["lang_client"].Value),
                            a => a.MenuID, b => b.ID, (a, b) => new
                            {
                                a.Id,
                                a.Title,
                                a.Status,
                                a.Content
                            }).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count() });
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
            ViewBag.Title = "Thêm dịch vụ";
            var eService = new EService();
            LoadData();
            return View(eService);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EService model)
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
                        var service = new Service
                        {
                            MenuID = model.MenuID,
                            Title = model.Title,
                            Alias = model.Alias,
                            Image = model.Image,
                            Description = model.Description,
                            Content = model.Content,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle,
                            MetaDescription =
                                string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.MetaDescription,
                            Status = model.Status,
                            Icon = model.Icon
                        };

                        db.Services.InsertOnSubmit(service);
                        db.SubmitChanges();

                        //Thêm hình ảnh cho dich vu
                        if (model.EGalleryITems != null)
                        {
                            foreach (EGalleryITem itemGallery in model.EGalleryITems)
                            {
                                var serviceGallery = new ServiceGallery
                                {
                                    Image = itemGallery.Image,
                                    ServiceID = service.Id,
                                };
                                db.ServiceGalleries.InsertOnSubmit(serviceGallery);
                            }
                            db.SubmitChanges();
                        }

                        TempData["Messages"] = "Thêm dịch vụ thành công.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        LoadData();
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
            using (var db = new MyDbDataContext())
            {
                Service service = db.Services.FirstOrDefault(a => a.Id == id);
                if (service == null)
                {
                    TempData["Messages"] = "Dịch vụ không tồn tại";
                    return RedirectToAction("Index");
                }
                ViewBag.Title = "Sửa dịch vụ";
                var eService = new EService
                {
                    ID = service.Id,
                    MenuID = service.MenuID.Value,
                    Title = service.Title,
                    Alias = service.Alias,
                    Image = service.Image,
                    Description = service.Description,
                    Content = service.Content,
                    MetaTitle = service.MetaTitle,
                    MetaDescription = service.MetaDescription,
                    Status = service.Status.Value,
                    Icon = service.Icon
                };
                //lấy danh sách hình ảnh
                eService.EGalleryITems =
                    db.ServiceGalleries.Where(a => a.ServiceID == service.Id).Select(a => new EGalleryITem
                    {
                        Image = a.Image
                    }).ToList();
                LoadData();
                return View(eService);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EService model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Service service = db.Services.FirstOrDefault(b => b.Id == model.ID);
                        if (service != null)
                        {
                            service.Title = model.Title;
                            service.MenuID = model.MenuID;
                            service.Alias = model.Alias;
                            service.Image = model.Image;
                            service.Description = model.Description;
                            service.Content = model.Content;
                            service.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle;
                            service.MetaDescription = string.IsNullOrEmpty(model.MetaDescription)
                                ? model.Title
                                : model.MetaDescription;
                            service.Status = model.Status;
                            service.Icon = model.Icon;
                            db.SubmitChanges();

                            //xóa gallery cho phòng
                            db.ServiceGalleries.DeleteAllOnSubmit(
                                db.ServiceGalleries.Where(a => a.ServiceID == service.Id).ToList());
                            //Thêm hình ảnh cho phòng
                            if (model.EGalleryITems != null)
                            {
                                foreach (EGalleryITem itemGallery in model.EGalleryITems)
                                {
                                    var serviceGallery = new ServiceGallery
                                    {
                                        Image = itemGallery.Image,
                                        ServiceID = service.Id,
                                    };
                                    db.ServiceGalleries.InsertOnSubmit(serviceGallery);
                                }
                                db.SubmitChanges();
                            }
                            TempData["Messages"] = "Sửa dịch vụ thành công";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        LoadData();
                        return View(model);
                    }
                }
                LoadData();
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
                    Service del = db.Services.FirstOrDefault(c => c.Id == id);
                    if (del != null)
                    {
                        //xóa hết hình ảnh của phòng này
                        db.ServiceGalleries.DeleteAllOnSubmit(
                            db.ServiceGalleries.Where(a => a.ServiceID == del.Id).ToList());

                        db.Services.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa dịch vụ thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "dịch vụ không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public void LoadData()
        {
            var listMenu = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "Lựa chọn chuyên mục"}
            };
            var menus = new List<Menu>();

            menus =
                MenuController.GetListMenu(0, Request.Cookies["lang_client"].Value).Where(
                    a =>
                        a.Type == SystemMenuType.Service).ToList();

            foreach (Menu menu in menus)
            {
                string sub = "";
                for (int i = 0; i < menu.Level; i++)
                {
                    sub += "|--";
                }
                menu.Title = sub + menu.Title;
            }

            listMenu.AddRange(menus.OrderBy(a => a.Location).Select(a => new SelectListItem
            {
                Text = a.Title,
                Value = a.ID.ToString()
            }).ToList());
            ViewBag.ListMenu = listMenu;
        }

    }
}
