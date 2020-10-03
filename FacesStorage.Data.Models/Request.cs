using System.Collections;
using System.Collections.Generic;

namespace FacesStorage.Data.Models
{
    public class Request
    {
        public int RequestId { get; set; }

        public User User { get; set; }
        public int? UserId { get; set; }

        public Response Response { get; set; }
        public int? ResponseId { get; set; }

        public string Discriminator { get; set; }

        public IList<RequestImage> Images { get; set; }
    }
}
