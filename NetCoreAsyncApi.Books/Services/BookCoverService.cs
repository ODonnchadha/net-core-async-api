namespace NetCoreAsyncApi.Books.Services
{
    using Microsoft.Extensions.Logging;
    using NetCoreAsyncApi.Books.ExternalModels;
    using NetCoreAsyncApi.Books.Interfaces.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public class BookCoverService : IBookCoverService, IDisposable
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<BookCoverService> logger;
        private CancellationTokenSource cancellationTokenSource;
        public BookCoverService(IHttpClientFactory httpClientFactory, ILogger<BookCoverService> logger)
        {
            this.clientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        private async Task<BookCover> DownloadBookCoverAsync(HttpClient client, string url, CancellationToken token)
        {
            var response = await client.GetAsync(url, token);

            if (response.IsSuccessStatusCode)
            {
                var bookCover = JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return bookCover;
            }

            // Let the listeners know that a request for a cancellation has occured.
            cancellationTokenSource.Cancel();

            return null;
        }

        //// Each task downloads and then adds to the list. This is not thread safe.
        //// Each task runs on a different thread and can manipulate the list at the exact same time.
        //public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        //{
        //    var client = clientFactory.CreateClient();
        //    var urls = new[]
        //    {
        //        $"https://localhost:44347/api/bookcovers?name={bookId}-V",
        //        $"https://localhost:44347/api/bookcovers?name={bookId}-W"
        //    };

        //    var bookCovers = new List<BookCover> { };

        //    var task1 = DownloadBookCoverAsync(client, urls[0], cancellationTokenSource.Token);
        //    var task2 = DownloadBookCoverAsync(client, urls[1], cancellationTokenSource.Token);

        //    await Task.WhenAll(task1, task2);

        //    return bookCovers;
        //}

        /// <summary>
        /// This will allow us, or the framework, to monitor the status.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        {
            cancellationTokenSource = new CancellationTokenSource { };

            var client = clientFactory.CreateClient();
            var bookCovers = new List<BookCover> { };
            var urls = new[]
            {
                $"https://localhost:44347/api/bookcovers?name={bookId}-V",
                $"https://localhost:44347/api/bookcovers?name={bookId}-W",
                $"https://localhost:44347/api/bookcovers?name={bookId}-X",
                $"https://localhost:44347/api/bookcovers?name={bookId}-Y",
                $"https://localhost:44347/api/bookcovers?name={bookId}-Z"
            };

            // Create the Tasks.
            var downloadBookCoverTasksQuery =
                from url
                in urls
                select DownloadBookCoverAsync(client, url, cancellationTokenSource.Token);

            // Start the Tasks.
            var downloadBookCoverTasks = downloadBookCoverTasksQuery.ToList();

            try
            {
                // await throws or rethrows. It does not wrapper within an aggregate.
                return await Task.WhenAll(downloadBookCoverTasks);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                // e.g.: exception.CancellationToken
                logger.LogInformation($"{operationCanceledException.Message}");

                downloadBookCoverTasks.ForEach(t =>
                {
                    logger.LogInformation($"Task {t.Id} has status {t.Status}");
                });

               return new List<BookCover> { };
            }
            catch (Exception exception)
            {
                logger.LogInformation($"{exception.Message}");
                throw;
            }

            //foreach(var url in urls)
            //{
            //    var response = await client.GetAsync(url);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        bookCovers.Add(JsonSerializer.Deserialize<BookCover>(
            //            await response.Content.ReadAsStringAsync(),
            //            new JsonSerializerOptions
            //            {
            //                PropertyNameCaseInsensitive = true
            //            })
            //        );
            //    }
            //}

            // return bookCovers;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                }
            }
        }
    }
}
