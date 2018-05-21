using System.Net.Http;
using System.Threading.Tasks;
using VGIS.Crowdsourcing.Settings;

namespace VGIS.Crowdsourcing.DAL
{
    public interface ICrowdsourcingApiDal
    {
        Task<string> Upload(string userId, string gameId, string filename, byte[] data);
    }

    public class CrowdsourcingApiDal : ICrowdsourcingApiDal
    {
        private readonly string _apiEndpoint;

        public CrowdsourcingApiDal(ApiEndpointSettings apiUrlData)
        {
            _apiEndpoint = $"{apiUrlData.EndpointUrl}/api/settings";
        }

        public async Task<string> Upload(string userId, string gameId, string filename, byte[] data)
        {
            var httpClient = new HttpClient();
            var form = new MultipartFormDataContent
            {
                {new StringContent(userId), "UserId"},
                {new StringContent(gameId), "GameId"},
                {new ByteArrayContent(data, 0, data.Length), "File", filename}
            };

            var response = await httpClient.PostAsync(_apiEndpoint, form);

            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}