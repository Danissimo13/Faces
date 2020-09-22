using System.Collections.Generic;

namespace FacesStorage.Data.Models
{
    public class CutResponse : Response
    {
        public IList<ResponseImage> Images { get; set; }
    }
}
