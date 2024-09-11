using OasisoftTask.Applications.Dtos.LiveToDoDtos;
using OasisoftTask.Applications.IServices;
using OasisoftTask.Common;

namespace OasisoftTask.Applications.Services
{
    public class LiveToDoService : ILiveToDoService
    {
        private readonly IApiHelperService _apiHelperService;
        private readonly IConfiguration _configuration;

        public LiveToDoService(IApiHelperService apiHelperService, IConfiguration configuration)
        {
            _apiHelperService = apiHelperService;
            _configuration = configuration;
        }

        public async Task<PageResult<ListLiveToDo>> GetAllWithPaginationAsync(PageRequest query)
        {
            var dataList = await getDataFromApi();
            return dataList.AsQueryable()
                           .ToPaginatedList(query.Page, query.PageSize);
        }

        public async Task<List<ListLiveToDo>> GetToDoListAsync()
        {
            return await getDataFromApi();
        }

        private async Task<List<ListLiveToDo>> getDataFromApi()
        {
            var url = $"{_configuration["TodosAPIService:Url"]}todos";
            return await _apiHelperService.GetListRequestAsync<ListLiveToDo>(url, null, true);
        }
    }
}
