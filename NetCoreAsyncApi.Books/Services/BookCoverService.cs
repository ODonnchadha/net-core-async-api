namespace NetCoreAsyncApi.Books.Services
{
    using NetCoreAsyncApi.Books.ExternalModels;
    using NetCoreAsyncApi.Books.Interfaces.Services;
    using System;
    using System.Collections.Generic;
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

        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        {
            var client = clientFactory.CreateClient();
            var bookCovers = new List<BookCover> { };
            var urls = new[]
            {
                $"https://localhost:44347/api/bookcovers?name={bookId}V",
                $"https://localhost:44347/api/bookcovers?name={bookId}W",
                $"https://localhost:44347/api/bookcovers?name={bookId}X",
                $"https://localhost:44347/api/bookcovers?name={bookId}Y",
                $"https://localhost:44347/api/bookcovers?name={bookId}Z"
            };

            foreach(var url in urls)
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    bookCovers.Add(JsonSerializer.Deserialize<BookCover>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })
                    );
                }
            }

            return bookCovers;
        }
    }
}
