using System.Collections.Generic;

namespace FacesStorage.Data.Models
{
    public class Response
    {
        public int ResponseId { get; set; }

        public IList<ResponseImage> Images { get; set; }
    }
}
