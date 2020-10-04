using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesWebApi.ApiModels
{
    public class SearchUserModel
    {
        public bool WithRole { get; set; }

        public bool WithRequests { get; set; }

        public int? FromRequest { get; set; }
        public int? RequestsCount { get; set; }

        public bool WithRequestImages { get; set; }

        public bool WithRequestResponses { get; set; }

        public bool WithResponseImages { get; set; }
    }
}
