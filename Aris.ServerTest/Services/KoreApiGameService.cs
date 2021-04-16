using Aris.ServerTest.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aris.ServerTest.Services
{
    public class KoreApiGameService : KoreApiBase, IKoreApiGameService
    {
        private readonly KoreOptions _koreOptions;
        private readonly IHttpClientFactory _httpClientFactory;

        public KoreApiGameService(IOptions<KoreOptions> koreOptions, IHttpClientFactory clientFactory)
        {
            _koreOptions = koreOptions.Value;
            _httpClientFactory = clientFactory;
        }

        private HttpClient GetClient(KoreAuthToken token, string returnUrl)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-Kore-ReturnUrl", returnUrl);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
            return client;
        }

        public async Task<List<KoreGame>> GetGamesAsync(KoreAuthToken token, string returnUrl)
        {
            var client = GetClient(token, returnUrl);

            var fullUrl = new Uri(_koreOptions.BaseUrl, "games");
            var response = await client.GetAsync(fullUrl.ToString());

            await CheckResponseForErrorAsync(response);

            //Task 1 complete. add await to following async code
            var data = await response.Content.ReadAsStringAsync();  
            var games = JsonConvert.DeserializeObject<KoreGames>(data);

            return games.Games;
        }

        public async Task<KoreGame> GetGameAsync(KoreAuthToken token, string gameUrl, string returnUrl)
        {
            var client = GetClient(token, returnUrl);

            var response = await client.GetAsync(gameUrl);

            await CheckResponseForErrorAsync(response);

            var data = await response.Content.ReadAsStringAsync();
            var game = JsonConvert.DeserializeObject<KoreGame>(data);

            return game;
        }
    }
}
