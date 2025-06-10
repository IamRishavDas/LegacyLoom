using System.ComponentModel.DataAnnotations;

namespace RequestFeatureShared
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;

        [Range(minimum:1, maximum:int.MaxValue, ErrorMessage = "Page Number must be a positive integer value")]
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        [Range(minimum: 1, maximum: 10, ErrorMessage = "Page size must be between 1 to 10")]
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }

        public string? OrderBy { get; set; }
    }
}
