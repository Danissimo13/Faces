﻿namespace FacesStorage.Data.Abstractions.SearchOptions
{
    public class RequestsSearchOptions
    {
        public int From { get; set; }

        public int Count { get; set; }

        public bool WithUser { get; set; }

        public bool WithImages { get; set; }

        public bool WithResponse { get; set; }

        public bool WithResponseImages { get; set; }
    }
}
