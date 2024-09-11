namespace OasisoftTask.Common
{
    public class PageResult<T> where T : class
    {
        public PageResult(int pageSize, int currentPage, List<T> data, int totalCount)
        {
            if (pageSize == 0)
                PageSize = Constants.PageSize;
            else
                PageSize = pageSize;
            CurrentPage = currentPage;
            Data = data;
            TotalCount = totalCount;

        }
        public int TotalCount { get; set; }
        public int TotalPages
        {
            get
            {
                double totalPages = 0;
                if (TotalCount != 0 && TotalCount < PageSize)
                {
                    totalPages = 1;


                }
                else if (PageSize != 0 && (TotalCount % (double)PageSize) == 0)
                {
                    totalPages = TotalCount / PageSize;
                }
                else
                {
                    totalPages = Math.Ceiling((TotalCount / (double)PageSize) + 1);
                }
                return (int)totalPages;
            }
            set { }
        }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Data { get; set; }

    }
}