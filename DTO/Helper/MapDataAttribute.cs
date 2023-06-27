using System;

namespace DTO.Kpi
{
    public class MapDataAttribute : Attribute
    {
        public string Type { get; set; }

        public string Parameter { get; set; }
    }
}