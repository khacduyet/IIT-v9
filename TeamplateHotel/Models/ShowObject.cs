using System;

namespace TeamplateHotel.Models
{
    public class ShowObject
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string MenuAlias { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CountComment { get; set; }
        public int? Index { get; set; }
        public double Price { get; set; }
        public string SecondMenu { get; set; }
    }
}