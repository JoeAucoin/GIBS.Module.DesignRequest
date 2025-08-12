using System.Collections.Generic;

namespace GIBS.Module.DesignRequest.Models
{
    public class Paged<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
    }
}