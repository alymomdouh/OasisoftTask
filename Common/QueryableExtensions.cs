using Microsoft.EntityFrameworkCore;

namespace OasisoftTask.Common
{
    public static class QueryableExtensions
    {
        public static async Task<PageResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int page, int pageSize, CancellationToken cancellationToken = default) where T : class
        {
            page = page < Constants.Page ? Constants.Page : page;
            pageSize = pageSize == 0 ? Constants.PageSize : pageSize;
            int count = await source.CountAsync();
            List<T> items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PageResult<T>(pageSize, page, items, count);
        }
        public static PageResult<T> ToPaginatedList<T>(this IQueryable<T> source, int page, int pageSize, CancellationToken cancellationToken = default) where T : class
        {
            page = page < Constants.Page ? Constants.Page : page;
            pageSize = pageSize == 0 ? Constants.PageSize : pageSize;
            int count = source.Count();
            List<T> items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PageResult<T>(pageSize, page, items, count);
        }
        public static IQueryable<T> Paginate<T>(this IQueryable<T> querable, int page, int pageSize, CancellationToken cancellationToken = default) where T : class
        {
            page = page < Constants.Page ? Constants.Page : page;
            pageSize = pageSize == 0 ? Constants.PageSize : pageSize;
            return querable.Skip((page - 1) * pageSize).Take(pageSize);
        }

    }
}
