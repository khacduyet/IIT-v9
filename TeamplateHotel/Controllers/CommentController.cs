using PagedList;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamplateHotel.Areas.Administrator.EntityModel;
using TeamplateHotel.Handler;
using TeamplateHotel.Models;

namespace TeamplateHotel.Controllers
{
    public class CommentController : BasicController
    {
        //danh sach ngon ngu
        public static List<Language> GetLanguages()
        {
            using (var db = new MyDbDataContext())
            {
                List<Language> languages = db.Languages.ToList();
                return languages;
            }
        }
        // Welcome
        public static Welcome getWelcome(string lang)
        {
            using (var db = new MyDbDataContext())
            {
                Welcome wc = db.Welcomes.Where(x => x.LanguageID == lang).FirstOrDefault();
                return wc;
            }
        }
        // Counter

        public static ECounter getCounter(string lang)
        {
            using (var db = new MyDbDataContext())
            {
                ECounter ect = db.Counters.Where(x => x.LanguageID == lang).Select(x => new ECounter
                {
                    Counter = x.Counter1,
                    External = x.External.Value,
                    Image = x.Image,
                    menuId = x.menuId.Value,
                    TitleButton = x.TitleButton,
                    ExternalPath = x.ExternalPath,
                    menuAlias = db.Menus.Where(y => y.ID == x.menuId).FirstOrDefault().Alias
                }).FirstOrDefault();
                return ect;
            }
        }
        // FAQs
        public static List<FAQ> getFaqs(string lang)
        {
            using (var db = new MyDbDataContext())
            {
                List<FAQ> faqs = db.FAQs.Where(x => x.LanguageID == lang).ToList();
                return faqs;
            }
        }

        // Service
        //Danh sách dich vu
        public static List<Service> GetServices(int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                List<Service> ser = new List<Service>();
                List<Service> restaurants = db.Services.Where(a => a.Status.Value && a.MenuID == menuId).ToList();
                foreach (var item in restaurants)
                {
                    ser.Add(item);
                }
                List<Menu> menus = db.Menus.Where(x => x.ParentID == menuId).ToList();
                foreach (var item in menus)
                {
                    var lst = db.Services.Where(x => x.MenuID == item.ID).ToList();
                    foreach (var it in lst)
                    {
                        ser.Add(it);
                    }
                }
                foreach (var restaurant in ser)
                {
                    restaurant.MenuAlias = restaurant.Menu.Alias;
                }
                return ser;
            }
        }
        //Chi tiết dich vu
        public static DetailService Detail_Service(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Service service = db.Services.FirstOrDefault(a => a.Id == id && a.Status.Value) ?? new Service();
                List<ServiceGallery> restaurantGalleries = db.ServiceGalleries.Where(a => a.ServiceID == service.Id).ToList();
                List<Service> restaurants = db.Services.Where(a => a.Status.Value && a.Id != service.Id).ToList();
                foreach (var item in restaurants)
                {
                    item.MenuAlias = service.Menu.Alias;
                }
                DetailService detailRestaurant = new DetailService()
                {
                    Service = service,
                    ServiceGalleries = restaurantGalleries,
                    Services = restaurants
                };
                return detailRestaurant;
            }
        }
        // Dịch vụ khác
        public static List<ServicesOther> ServicesOther(int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                List<ServicesOther> lst = new List<ServicesOther>();
                Menu mnu = db.Menus.FirstOrDefault(x => x.ID == menuId);
                List<Menu> menus = null;
                if (mnu.ParentID == 0)
                {
                    menus = db.Menus.Where(x => x.ParentID == mnu.ID).ToList();
                }
                else
                {
                    menus = db.Menus.Where(x => x.ParentID == mnu.ParentID).ToList();
                }
                foreach (var item in menus)
                {
                    ServicesOther so = new ServicesOther();
                    List<Service> s = new List<Service>();
                    List<Service> sers = db.Services.Where(x => x.MenuID == item.ID && x.Status.Value).ToList();
                    foreach (var it in sers)
                    {
                        it.MenuAlias = item.Alias;
                        s.Add(it);
                    }
                    so.menu = item;
                    so.services = s;
                    lst.Add(so);
                }
                return lst;
            }
        }
        // Services Index
        public static List<ServicesOther> ServicesIndex(string lang)
        {
            using (var db = new MyDbDataContext())
            {
                List<ServicesOther> lst = new List<ServicesOther>();
                List<Menu> menus = db.Menus.Where(x => x.Type == SystemMenuType.Service && x.Status && x.Location == 1 && x.LanguageID == lang).ToList();
                foreach (var item in menus)
                {
                    ServicesOther so = new ServicesOther();
                    List<Service> s = new List<Service>();
                    List<Service> sers = db.Services.Where(x => x.MenuID == item.ID && x.Status.Value).ToList();
                    foreach (var it in sers)
                    {
                        it.MenuAlias = item.Alias;
                        s.Add(it);
                    }
                    so.menu = item;
                    so.services = s;
                    lst.Add(so);
                }
                return lst;
            }
        }

        // ABOUT
        public static EAbout getAbout(string lang)
        {
            using (var db = new MyDbDataContext())
            {
                EAbout ab = (from a in db.Abouts
                             join m in db.Menus on a.menuId equals m.ID
                             where a.LanguageID == lang
                             select new EAbout
                             {
                                 ID = a.Id,
                                 Description = a.Description,
                                 Image = a.Image,
                                 LanguageID = a.LanguageID,
                                 Title = a.Title_,
                                 Image1 = a.Image1,
                                 Image2 = a.Image2,
                                 ExternalPath = a.ExternalPath,
                                 SignImage = a.SignImage,
                                 DescriptionBanner = a.DescriptionBanner,
                                 DescriptionBase = a.DescriptionBase,
                                 DescriptionReview = a.DescriptionReview,
                                 External = a.External.Value,
                                 TitleBanner = a.TitleBanner,
                                 TitleBase = a.TitleBase,
                                 TitleButton = a.TitleButton,
                                 TitleReview = a.TitleReview,
                                 menuAlias = m.Alias
                             }).FirstOrDefault();
                return ab;
            }
        }

        // Overview
        public static List<Overview> getOverview(string lang)
        {
            using (var db = new MyDbDataContext())
            {
                List<Overview> ov = db.Overviews.Where(x => x.LanguageID == lang).ToList();
                return ov;
            }
        }
        //Review
        public static List<Review> getReview(string lang)
        {
            using (var db = new MyDbDataContext())
            {
                List<Review> rv = db.Reviews.Where(x => x.LanguageID == lang && x.Status).ToList();
                return rv;
            }
        }

        //Chi tiết khách sạn
        public static Hotel DetailHotel(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                var hotel =
                    db.Hotels.FirstOrDefault(a => a.LanguageID == languageKey) ??
                    new Hotel();
                return hotel;
            }
        }


        //Danh sách Main menu
        public static List<Menu> GetMainMenus(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> menus = db.Menus.Where(a => a.Status && a.Location == SystemMenuLocation.MainMenu &&
                                                       a.LanguageID == languageKey).OrderBy(a => a.Index).ToList();
                return menus;
            }
        }
        // Get menu child 
        public static List<Menu> getMenuChild(string languageKey, int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> mnu = db.Menus.Where(a => a.Location == SystemMenuLocation.MainMenu &&
                                                       a.LanguageID == languageKey && a.ParentID == menuId).OrderBy(a => a.Index).ToList();
                return mnu;
            }
        }

        //Danh sách Second menu
        public static List<Menu> GetSecondMenus(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> menufooter = db.Menus.Where(a => a.Status && a.Location == SystemMenuLocation.SecondMenu &&
                                                       a.LanguageID == languageKey).ToList();
                return menufooter;
            }
        }

        public static List<ShowSubmenu> GetSecondMenusInArticle(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowSubmenu> obj = new List<ShowSubmenu>();
                List<Menu> menufooter = db.Menus.Where(a => a.Status && a.Location == SystemMenuLocation.SecondMenu &&
                                                       a.LanguageID == languageKey).ToList();
                foreach (var item in menufooter)
                {
                    ShowSubmenu ssm = new ShowSubmenu();
                    List<Article> articles = db.Articles.Where(x => x.Status && x.MenuID == item.ID).ToList();
                    if (articles.Count == 1)
                    {
                        ssm.articles = articles.FirstOrDefault();
                    }
                    ssm.menu = item;
                    obj.Add(ssm);
                }
                return obj;
            }
        }
        // LIST GALLERY
        public static List<Gallery> lstGallery()
        {
            using (var db = new MyDbDataContext())
            {
                List<Gallery> galleries = db.Galleries.OrderBy(a => a.Index).ToList();
                return galleries;
            }
        }
        //Danh sách Second menu
        public static IPagedList<Gallery> Gallery(int? page)
        {
            using (var db = new MyDbDataContext())
            {
                List<Gallery> galleries = db.Galleries.OrderBy(a => a.Index).ToList();
                int pageNumber = (page ?? 1);
                int pageSize = 24;
                return galleries.ToPagedList(pageNumber, pageSize);
            }
        }
        // Get all Gallery (Gallery, Rooms, Services, Tours )
        public static List<AllGallery> GalleryAll()
        {
            using (var db = new MyDbDataContext())
            {
                List<AllGallery> gal = new List<AllGallery>();
                var Galleries = db.Galleries.ToList();
                var Rooms = db.RoomGalleries.ToList();
                var Services = db.ServiceGalleries.ToList();
                var Tours = db.TourGalleries.ToList();
                foreach (var item in Galleries)
                {
                    AllGallery al = new AllGallery();
                    al.Title = "Galleries";
                    al.Image = item.LargeImage;
                    gal.Add(al);
                }
                foreach (var item in Rooms)
                {
                    AllGallery al = new AllGallery();
                    al.Title = "Rooms";
                    al.Image = item.ImageLarge;
                    gal.Add(al);
                }
                foreach (var item in Services)
                {
                    AllGallery al = new AllGallery();
                    al.Title = "Services";
                    al.Image = item.Image;
                    gal.Add(al);
                }
                foreach (var item in Tours)
                {
                    AllGallery al = new AllGallery();
                    al.Title = "Tours";
                    al.Image = item.LargeImage;
                    gal.Add(al);
                }
                return gal.OrderByDescending(x=>Guid.NewGuid()).ToList();
            }
        }
        // Room Gallery
        public static List<RoomGallery> RoomGallery()
        {
            using (var db = new MyDbDataContext())
            {
                List<RoomGallery> roomGalleries = db.RoomGalleries.ToList();
                return roomGalleries;
            }
        }
        //get plugin
        public static Plugin GetPluigPlugin()
        {
            using (var db = new MyDbDataContext())
            {
                return db.Plugins.FirstOrDefault() ?? new Plugin();
            }
        }
        //Danh sach slider
        public static List<Slider> GetListSlider(string languageKey, int menuId = 0, bool index = false)
        {
            using (var db = new MyDbDataContext())
            {
                List<Slider> sliders = db.Sliders.Where(a => a.LanguageID == languageKey && a.Status && a.ViewAll == index).ToList();
                List<Slider> sliderIsChoise = new List<Slider>();

                //lấy banner theo menu
                var menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                if (menu != null)
                {
                    foreach (var slider in sliders)
                    {
                        if (slider.MenuIDs != null && slider.MenuIDs.Length > 0)
                        {
                            int[] menuIds =
                                slider.MenuIDs.Substring(0, slider.MenuIDs.Length - 1).Split(',').Select(
                                    n => Convert.ToInt32(n)).ToArray();

                            if (menuIds.Contains(menu.ID))
                            {
                                sliderIsChoise.Add(slider);
                            }
                        }
                    }
                }
                else
                {
                    //lấy menu theo trang chủ
                    var menuHome = db.Menus.FirstOrDefault(a => a.Type == SystemMenuType.Home);
                    if (menuHome != null)
                    {
                        foreach (var slider in sliders)
                        {
                            if (slider.MenuIDs != null && slider.MenuIDs.Length > 0)
                            {
                                int[] menuIds =
                           slider.MenuIDs.Substring(0, slider.MenuIDs.Length - 1).Split(',').Select(
                               n => Convert.ToInt32(n)).ToArray();

                                if (menuIds.Contains(menuHome.ID))
                                {
                                    sliderIsChoise.Add(slider);
                                }
                            }
                        }
                    }
                }
                if (sliderIsChoise.Count == 0)
                {
                    sliderIsChoise = sliders;
                }
                return sliderIsChoise;
                //lấy menu hiển thị tất cả
            }
        }


        //Danh sach quang cao
        public static List<Advertising> GetAdvertisings()
        {
            using (var db = new MyDbDataContext())
            {
                List<Advertising> advertisings = db.Advertisings.Where(a => a.Status).ToList();
                return advertisings;
            }
        }

        //Danh sách bài viết theo chuyên mục
        public static IPagedList<Article> GetArticles(int menuId, int? page)
        {
            using (var db = new MyDbDataContext())
            {
                List<Article> articles = db.Articles.Where(x => x.MenuID == menuId && x.Status).ToList();

                foreach (var article in articles)
                {
                    article.MenuAlias = article.Menu.Alias;
                }
                int pageNumber = (page ?? 1);
                int pageSize = 3;
                return articles.ToPagedList(pageNumber, pageSize);
            }
        }
        // TOUR

        ////Danh sách tours
        public static IPagedList<Tour> GetTours(string Alias, int? page)
        {
            using (var db = new MyDbDataContext())
            {
                int menuId = db.Menus.FirstOrDefault(x => x.Alias == Alias).ID;
                List<Tour> tours = db.Tours.Where(a => a.Status && a.MenuID == menuId).OrderBy(a => a.Index).ToList();
                foreach (var tour in tours)
                {
                    tour.MenuAlias = tour.Menu.Alias;
                }
                int pageNumber = (page ?? 1);
                int pageSize = 6;
                return tours.ToPagedList(pageNumber, pageSize);
            }
        }

        public static List<EGalleryITem> GetTourGalleries(int id)
        {
            using (var db = new MyDbDataContext())
            {
                List<EGalleryITem> tg = db.TourGalleries.Where(x => x.TourID == id).Select(x => new EGalleryITem
                {
                    Image = x.LargeImage
                }).ToList();
                return tg;
            }
        }


        //Bai viet mới
        public static List<ShowObject> NewArticles(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> articleHots = db.Articles.Where(a => a.New && a.Status)
                        .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                            (a, b) => new ShowObject
                            {
                                ID = a.ID,
                                Alias = a.Alias,
                                MenuAlias = b.Alias,
                                Title = a.Title,
                                DateCreated = a.DateCreated,
                                Index = a.Index,
                                Image = a.Image,
                                Description = a.Description,
                                //Content = a.Content
                            }).OrderByDescending(a => a.ID).Take(10).ToList();
                foreach (var item in articleHots)
                {
                    int count = 0;
                    foreach (var cmt in db.Comments.Where(x => x.Status && x.BlogId == item.ID))
                    {
                        count++;
                    }
                    item.CountComment = count;
                }
                return articleHots;
            }
        }
        //Chi tiết bài viết
        public static DetailArticle Detail_Article(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Article article = db.Articles.FirstOrDefault(a => a.ID == id && a.Status) ?? new Article();
                List<Article> articles = db.Articles.Where(a => a.MenuID == article.MenuID && a.Status && a.ID != article.ID).OrderBy(a => a.Index).ToList();
                foreach (var item in articles)
                {
                    item.MenuAlias = article.Menu.Alias;
                }
                DetailArticle detailArticle = new DetailArticle()
                {
                    Article = article,
                    Articles = articles
                };
                return detailArticle;
            }
        }
        //Danh sách phòng
        public static List<Room> lstRoom(int Type, string lang)
        {
            using (var db = new MyDbDataContext())
            {
                var menu = db.Menus.FirstOrDefault(a => a.Type == Type && a.Level == 0 && a.LanguageID == lang);
                List<Room> rooms = db.Rooms.Where(a => a.Status && a.LanguageID == menu.LanguageID).OrderBy(a => a.Index).ToList();

                foreach (var item in rooms)
                {
                    if (menu.LanguageID.Equals("vi"))
                    {
                        item.Price = item.Price * GetPriceUSD.USDToVND();
                    }
                    item.MenuAlias = menu.Alias;
                }

                return rooms;
            }
        }
        public static List<Room> roomSpecial(int Type, string lang)
        {
            using (var db = new MyDbDataContext())
            {
                var menu = db.Menus.FirstOrDefault(a => a.Type == Type && a.Level == 0 && a.LanguageID == lang);
                List<Room> rooms = db.Rooms.Where(a => a.Status && a.LanguageID == menu.LanguageID && a.Home).OrderBy(a => a.Index).ToList();

                foreach (var item in rooms)
                {
                    if (menu.LanguageID.Equals("vi"))
                    {
                        item.Price = item.Price * GetPriceUSD.USDToVND();
                    }
                    item.MenuAlias = menu.Alias;
                }

                return rooms;
            }
        }
        public static IPagedList<Room> GetRooms(int menuId, int? page)
        {
            using (var db = new MyDbDataContext())
            {
                var menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                List<Room> rooms = db.Rooms.Where(a => a.Status && a.LanguageID == menu.LanguageID).OrderBy(a => a.Index).ToList();

                foreach (var room in rooms)
                {
                    room.MenuAlias = menu.Alias;
                    room.roomGalleries = db.RoomGalleries.Where(a => a.RoomId == room.ID).ToList();
                    if (menu.LanguageID.Equals("vi"))
                    {
                        room.Price = room.Price * GetPriceUSD.USDToVND();
                    }
                }
                page = page ?? 1;
                int pageSize = 6;
                return rooms.ToPagedList(page.Value, pageSize);
            }
        }
        public static IPagedList<Room> GetRooms(int menuId, int? page, int adult, int child)
        {
            using (var db = new MyDbDataContext())
            {
                var menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                List<Room> rooms = db.Rooms.Where(a => a.Status && a.LanguageID == menu.LanguageID && a.MaxPeople >= (adult + child)).OrderBy(a => a.Index).ToList();

                foreach (var room in rooms)
                {
                    room.MenuAlias = menu.Alias;
                    room.roomGalleries = db.RoomGalleries.Where(a => a.RoomId == room.ID).ToList();
                    if (menu.LanguageID.Equals("vi"))
                    {
                        room.Price = room.Price * GetPriceUSD.USDToVND();
                    }
                }
                page = page ?? 1;
                int pageSize = 3;
                return rooms.ToPagedList(page.Value, pageSize);
            }
        }
        //Chi tiết phòng
        public static DetailRoom Detail_Room(int id, int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                var menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                Room room = db.Rooms.FirstOrDefault(a => a.ID == id && a.Status) ?? new Room();
                if (menu.LanguageID.Equals("vi"))
                {
                    room.Price = room.Price * GetPriceUSD.USDToVND();
                }
                List<RoomGallery> roomGalleries = db.RoomGalleries.Where(a => a.RoomId == room.ID).ToList();
                List<Room> rooms = db.Rooms.Where(a => a.Status && a.ID != room.ID && a.LanguageID == menu.LanguageID).OrderBy(a => a.Index).ToList();

                foreach (var item in rooms)
                {
                    item.MenuAlias = menu.Alias;
                    if (menu.LanguageID.Equals("vi"))
                    {
                        item.Price = item.Price * GetPriceUSD.USDToVND();
                    }
                }
                DetailRoom detailRoom = new DetailRoom()
                {
                    Room = room,
                    RoomGalleries = roomGalleries,
                    Rooms = rooms
                };
                return detailRoom;
            }
        }

        ///////////// Trang home ////////////////////////
        //Bai viet welcome
        public static ShowObject WellcomeArticle(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                return
                    db.Articles.Where(a => a.Home && a.Status)
                        .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                            (a, b) => new ShowObject()
                            {
                                ID = a.ID,
                                Alias = a.Alias,
                                MenuAlias = b.Alias,
                                Title = a.Title,
                                Index = a.Index,
                                Image = a.Image,
                                Description = a.Description
                            }).FirstOrDefault();
            }
        }
        //Bai viet hot
        public static List<ShowObject> HotArticles(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> articleHots = db.Articles.Where(a => a.Hot && a.Status)
                        .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                            (a, b) => new ShowObject
                            {
                                ID = a.ID,
                                Alias = a.Alias,
                                MenuAlias = b.Alias,
                                Title = a.Title,
                                DateCreated = a.DateCreated,
                                Index = a.Index,
                                Image = a.Image,
                                Description = a.Description,
                            }).ToList();
                foreach (var item in articleHots)
                {
                    int count = 0;
                    foreach (var cmt in db.Comments.Where(x => x.Status && x.BlogId == item.ID))
                    {
                        count++;
                    }
                    item.CountComment = count;
                }
                return articleHots;
            }
        }
        //Bai viet Customer
        public static List<ShowObject> CustomerArticles(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> CustomerArticle = db.Articles.Where(a => a.Customer && a.Status)
                        .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                            (a, b) => new ShowObject
                            {
                                ID = a.ID,
                                Alias = a.Alias,
                                MenuAlias = b.Alias,
                                Title = a.Title,
                                Index = a.Index,
                                Image = a.Image,
                                Description = a.Description,
                            }).ToList();
                return CustomerArticle;
            }
        }
        // Get contact
        public static Menu getContact(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                Menu m = db.Menus.Where(x => x.LanguageID == languageKey && x.Type == SystemMenuType.Contact && x.Status).FirstOrDefault();
                return m;
            }
        }

        //phòng show home
        public static List<ShowObject> RoomShowHome(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                var memu =
                    db.Menus.FirstOrDefault(a => a.Type == SystemMenuType.RoomRate && a.LanguageID == languageKey) ??
                    new Menu();
                List<ShowObject> roomShowHome = db.Rooms.Where(a => a.Home && a.Status && a.LanguageID == languageKey).Select(a => new ShowObject
                {
                    ID = a.ID,
                    Alias = a.Alias,
                    MenuAlias = memu.Alias,
                    Title = a.Title,
                    Index = a.Index,
                    Image = a.Image,
                    Description = a.Description,
                    Price = (double)a.Price,
                }).ToList();
                foreach (var item in roomShowHome)
                {
                    item.Price = item.Price * (double)GetPriceUSD.USDToVND();
                }
                return roomShowHome;
            }
        }

    }
}