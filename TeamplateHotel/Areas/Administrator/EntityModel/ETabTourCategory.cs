using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class ETabTourCategory
    {
        public int ID { get; set; }

        [DisplayName("Chuyên mục")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn chuyên mục")]
        public int MenuID { get; set; }

        [DisplayName("Tiêu đề")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; }

        [DisplayName("Nội dung")]
        public string Content { get; set; }
    }
}