using ProjectLibrary.Database;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class ETour
    {
        public int ID { get; set; }

        [DisplayName("Chuyên mục Tour")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn chuyên mục tour")]
        public int MenuID { get; set; }

        [DisplayName("Tên tour")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tên tour")]
        public string Title { get; set; }

        [DisplayName("Alias")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Alias { get; set; }

        [DisplayName("Hình đại diện")]
        [MaxLength(300, ErrorMessage = "Tối đa 300 ký tự")]
        [Required(ErrorMessage = "Vui lòng chọn hình đại diện")]
        public string Image { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }

        public int? Index { get; set; }

        [DisplayName("Tiêu đề trang")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string MetaTitle { get; set; }

        [DisplayName("Thẻ mô tả")]
        [MaxLength(300, ErrorMessage = "Tối đa 300 ký tự")]
        public string MetaDescription { get; set; }

        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }

        [DisplayName("Tour hot")]
        public bool Hot { get; set; }

        [DisplayName("Tour sale")]
        public bool Sale { get; set; }

        //[DisplayName("Ngày khởi hành")]
        //[Required(ErrorMessage = "Vui lòng nhập ngày khởi hành")]
        //public string Departure { get; set; }

        [DisplayName("Độ dài tour")]
        public string NumberDay { get; set; }

        [DisplayName("thành phố")]
        public string Country { get; set; }

        [DisplayName("option package tour")]
        public bool StartDeal { get; set; }

        //[DisplayName("Giá sale")]
        public decimal PriceSale { get; set; }

        //[DisplayName("Giá")]
        public decimal Price { get; set; }

        [DisplayName("Nội dung")]
        public string Content { get; set; }

        public bool VNtop { get; set; }
        public string Address { get; set; }
        public string Link { get; set; }
        public string Highlights { get; set; }
        public string PriceList { get; set; }
        public string Bgr { get; set; }
        public bool TourOther { get; set; }
        public string Vehicle { get; set; }
        public string Schema { get; set; }

        public List<EGalleryITem> EGalleryITems { get; set; }

        public List<TabTour> TabTours { get; set; }
    }

    public class EGalleryITem
    {
        public string Image { get; set; }
    }
}