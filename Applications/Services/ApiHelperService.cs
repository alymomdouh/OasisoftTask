using Newtonsoft.Json;
using OasisoftTask.Applications.IServices;
using System.Net.Http.Headers;

namespace OasisoftTask.Applications.Services
{
    public class ApiHelperService : IApiHelperService
    {
        private readonly HttpClient Client;
        public ApiHelperService(IHttpContextAccessor httpContextAccessor)
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("accept-language", Thread.CurrentThread.CurrentCulture.Name);

            if (httpContextAccessor != null &&
                (httpContextAccessor?.HttpContext?.Request?.Headers?.ContainsKey("Authorization")).GetValueOrDefault())
            {
                Client.DefaultRequestHeaders.Add("Authorization", httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());
            }
        }
        public async Task<List<TResponseEntity>?> GetListRequestAsync<TResponseEntity>(string url, Dictionary<string, string> headers = null, bool handleErrors = false) where TResponseEntity : class
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(true);
            if (handleErrors && !response.IsSuccessStatusCode)
            {
                // throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
                response.EnsureSuccessStatusCode();
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            if (TryParseListJson<TResponseEntity>(responseBody))
            {
                return JsonConvert.DeserializeObject<List<TResponseEntity>>(responseBody);
            }
            else
            {
                return default;
            }
        }
        private static bool TryParseListJson<TEntity>(string jsonString) where TEntity : class
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Error = (sender, args) => { args.ErrorContext.Handled = true; },
                    MissingMemberHandling = MissingMemberHandling.Error
                };
                JsonConvert.DeserializeObject<List<TEntity>>(jsonString);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
