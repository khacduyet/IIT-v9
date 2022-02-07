using System;
using System.ComponentModel;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class ETag
    {
        public int ID { get; set; }

        [DisplayName("Tên thẻ")]
        public string TagName { get; set; }
        public string IdLanguage { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}