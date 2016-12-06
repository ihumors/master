namespace Dms.Master.Models
{
    public class PaginationModel
    {
        public decimal id { get; set; }
        public decimal totalCount { get; set; }

        private int __pageIndex = 1;
        public int pageIndex
        {
            get { return __pageIndex; }
            set { __pageIndex = value; }
        }

        private int __pageSize = 10;
        public int pageSize
        {
            get { return __pageSize; }
            set { __pageSize = value; }
        }

        public string sort { get; set; }
        public string sortBy { get; set; }
        public string key { get; set; }
    }
}