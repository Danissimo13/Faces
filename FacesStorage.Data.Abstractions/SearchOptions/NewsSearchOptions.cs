using System;
using System.Collections.Generic;
using System.Text;

namespace FacesStorage.Data.Abstractions.SearchOptions
{
    public class NewsSearchOptions
    {
        public int From { get; set; }

        public int Count { get; set; }

        public bool WithBody { get; set; }
    }
}
