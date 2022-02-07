using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EAbout
    {
        public int ID { get; set; }
        [DisplayName("Tiêu đề")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }
        [DisplayName("Mô tả")]
        [MaxLength(1000, ErrorMessage = "Tối đa 1000 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; }
        [DisplayName("Ảnh Banner")]
        public string Image { get; set; }
        [DisplayName("Ảnh")]
        public string Image1 { get; set; }
        [DisplayName("Ảnh vị trí 2")]
        public string Image2 { get; set; }
        public string LanguageID { get; set; }
        [DisplayName("Đường dẫn menu")]
        public int menuId { get; set; }
        public bool External { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề nút bấm")]
        public string TitleButton { get; set; }
        public string ExternalPath { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả banner")]
        public string DescriptionBanner { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề banner")]
        public string TitleBanner { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập mô tả đánh giá")]
        public string DescriptionReview { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập tiêu đề đánh giá")]
        public string TitleReview { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập mô tả cơ sở")]
        public string DescriptionBase { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập tiêu đề cơ sở")]
        public string TitleBase { get; set; }
        public string SignImage { get; set; }

        public string menuAlias { get; set; }
    }
}