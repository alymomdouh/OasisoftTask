namespace OasisoftTask.Applications.IServices
{
    public interface IApiHelperService
    {
        Task<List<TResponseEntity>> GetListRequestAsync<TResponseEntity>(string url, Dictionary<string, string> headers = null, bool handleErrors = false) where TResponseEntity : class;

    }
}
