using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class ECounter
    {
        public int ID { get; set; }
        [DisplayName("Bộ đếm")]
        public string Counter { get; set; }
        [DisplayName("Hình ảnh")]
        public string Image { get; set; }
        [DisplayName("Ảnh")]
        public string LanguageID { get; set; }
        [DisplayName("Đường dẫn menu")]
        public int menuId { get; set; }
        public bool External { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề nút bấm")]
        public string TitleButton { get; set; }
        public string ExternalPath { get; set; }
        public string menuAlias { get; set; }
    }
}