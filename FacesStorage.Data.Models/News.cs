using System;

namespace FacesStorage.Data.Models
{
    public class News
    {
        public int NewsId { get; set; }
        
        public string Topic { get; set; }

        public string Body { get; set; }

        public string ImageName { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
