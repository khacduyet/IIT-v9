using System.ComponentModel.DataAnnotations.Schema;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EArtTag
    {
        public int ID { get; set; }
        public int ArticleId { get; set; }
        public string Tags { get; set; }
        [NotMapped]
        public string[] selectedIdArray { get; set; }
    }
}