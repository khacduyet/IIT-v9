using ProjectLibrary.Config;
using ProjectLibrary.Database;
using System;
using System.Linq;
using System.Web.Mvc;
using TeamplateHotel.Handler;

namespace TeamplateHotel.Controllers
{
    public class BookingController : Controller
    {
        [HttpGet]
        public ActionResult MakeReservation(int id, string alias)
        {
            using (var db = new MyDbDataContext())
            {
                var room = db.Rooms.Where(x => x.ID == id && x.Alias == alias && x.Status).SingleOrDefault();
                if (room.LanguageID.Equals("vi"))
                {
                    room.Price = room.Price * GetPriceUSD.USDToVND();
                }
                return View(room);
            }
        }

        [HttpPost]
        public ActionResult SendBooking(BookRoom model)
        {
            string status = "success";
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MyDbDataContext())
                    {
                        Hotel hotel = CommentController.DetailHotel(Request.Cookies["LanguageID"].Value);
                        string codeBooking = hotel.CodeBooking + "1";
                        if (db.BookRooms.Any())
                        {
                            codeBooking = hotel.CodeBooking +
                                          (db.BookRooms.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1);
                        }

                        model.Code = codeBooking;
                        model.DateBook = DateTime.UtcNow;
                        db.BookRooms.InsertOnSubmit(model);
                        db.SubmitChanges();
                        //Gửi email xác nhận đặt phòng
                        var room = db.Rooms.Where(x => x.ID == model.InfoBooking).FirstOrDefault();
                        SendEmail sendEmail =
                        db.SendEmails.FirstOrDefault(
                            a => a.Type == TypeSendEmail.BookRoom && a.LanguageID == Request.Cookies["LanguageID"].Value);

                        sendEmail.Title = sendEmail.Title.Replace("{HotelName}", hotel.Name);
                        string content = sendEmail.Content;
                        content = content.Replace("{Code}", model.Code);
                        content = content.Replace("{Gender}", model.Gender);
                        content = content.Replace("{FullName}", model.FullName);
                        content = content.Replace("{Email}", model.Email);
                        content = content.Replace("{Tel}", model.Phone);
                        content = content.Replace("{Address}", model.Address);
                        content = content.Replace("{City}", model.City);
                        content = content.Replace("{Country}", model.Country);
                        content = content.Replace("{Request}", model.Request);
                        content = content.Replace("{InfoBooking}", room.Title);
                        content = content.Replace("{CheckIn}", model.CheckIn.ToString("MMMM, dd, yyyy"));
                        content = content.Replace("{Adult}", model.Adult.ToString());
                        content = content.Replace("{Child}", model.Child.ToString());
                        content = content.Replace("{HotelName}", hotel.Name);
                        content = content.Replace("{TotalPrice}", model.TotalMoney.ToString());
                        content = content.Replace("{HotelEmail}", hotel.Email);
                        content = content.Replace("{HotelTel}", hotel.Tel);
                        content = content.Replace("{Website}", model.DateBook.ToString("MMMM, dd, yyyy"));
                        content = content.Replace("{DateBook}", model.DateBook.ToString("MMMM, dd, yyyy"));

                        MailHelper.SendMail(model.Email, sendEmail.Title, content);
                        MailHelper.SendMail(hotel.Email, hotel.Name + " (" + model.Code + ")- Booking room of " + model.FullName, content);
                    }
                }
                catch (Exception ex)
                {
                    status = "error";
                }
            }
            return Redirect("/Booking/Messages/?status=" + status);
        }

        [HttpGet]
        public ActionResult Messages()
        {
            using (var db = new MyDbDataContext())
            {
                SendEmail sendEmail =
                       db.SendEmails.FirstOrDefault(
                           a => a.Type == TypeSendEmail.BookRoom && a.LanguageID == Request.Cookies["LanguageID"].Value);

                string status = Request.Params["status"];
                ViewBag.Messages = "";
                if (string.IsNullOrEmpty(status) == false)
                {
                    if (status.Equals("success"))
                    {
                        ViewBag.Messages = sendEmail.Success;
                    }
                    else
                    {
                        ViewBag.Messages = sendEmail.Error;
                    }
                }
                else
                {
                    ViewBag.Messages = sendEmail.Error;
                }
                return View();
            }
        }
    }
}