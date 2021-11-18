namespace NetCoreAsyncApi.Books.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NetCoreAsyncApi.Books.Contexts;
    using NetCoreAsyncApi.Books.Entities;
    using NetCoreAsyncApi.Books.Interfaces.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BookRepository : IBookRepository, IDisposable
    {
        private BooksContext context;
        public BookRepository(BooksContext context) => this.context = context ?? throw new ArgumentNullException(nameof(context));
        public void AddBook(Entities.Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            context.Add(book);
        }
        public Book GetBook(Guid id) => context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);

        /// <summary>
        /// Offload long-running synchronous process to a different thread.
        /// </summary>
        /// <returns></returns>
        private Task<int> GetBookPages()
        {
            return Task.Run(() =>
            {
                var calc = new NetCoreAsync.Legacy.ComplicatedPageCalculator { };
                var pages = calc.CalculateBookPages();

                return pages;
            });
        }
        public async Task<Book> GetBookAsync(Guid id)
        {
            // Task.Run() on the server decreases scalability.
            // var pages = await GetBookPages();

            return await context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }
        public IEnumerable<Book> GetBooks() => context.Books.Include(b => b.Author).ToList();
        public async Task<IEnumerable<Book>> GetBooksAsync() => await context.Books.Include(b => b.Author).ToListAsync();
        public async Task<IEnumerable<Entities.Book>> GetBooksAsync(IEnumerable<Guid> ids)
        {
            return await context.Books.Where(b => ids.Contains(b.Id)).Include(b => b.Author).ToListAsync();
        }
        public async Task<bool> SaveChangesAsync() =>  await context.SaveChangesAsync() > 0;

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
        #endregion
    }
}
