using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestFeatureShared
{
    public class MetaData
    {
        [Range(minimum:1, maximum:int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int CurrentPage { get; set; }

        [Range(minimum:1, maximum:int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int TotalPages { get; set; }

        [Range(minimum:1, maximum:int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int PageSize { get; set; }

        [Range(minimum:1, maximum:int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
