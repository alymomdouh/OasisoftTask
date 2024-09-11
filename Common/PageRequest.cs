namespace OasisoftTask.Common
{
    public class PageRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public PageRequest(int page = 1, int pageSize = Constants.PageSize)
        {
            if (page == 0)
                page = 1;

            if (pageSize == 0)
                pageSize = Constants.PageSize;

            Page = page;
            PageSize = pageSize;
        }
    }
}
