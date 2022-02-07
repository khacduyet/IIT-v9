using ProjectLibrary.Database;
using System.Collections.Generic;

namespace TeamplateHotel.Models
{
    public class DetailArticle
    {
        public Article Article { get; set; }
        public List<Article> Articles { get; set; }
    }
}