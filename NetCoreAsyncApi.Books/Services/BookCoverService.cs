namespace NetCoreAsyncApi.Books.Services
{
    using NetCoreAsyncApi.Books.ExternalModels;
    using NetCoreAsyncApi.Books.Interfaces.Services;
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class BookCoverService : IBookCoverService
    {
        private IHttpClientFactory clientFactory;
        public BookCoverService(IHttpClientFactory httpClientFactory)
        {
            this.clientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<BookCover> GetBookCoverAsync(string name)
        {
            var client = clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:44347/api/bookcovers?name={name}");

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }

            return null;
        }
    }
}
