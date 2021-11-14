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
        public Book GetBook(Guid id) => context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);
        public async Task<Book> GetBookAsync(Guid id) => await context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        public IEnumerable<Book> GetBooks() => context.Books.Include(b => b.Author).ToList();
        public async Task<IEnumerable<Book>> GetBooksAsync() => await context.Books.Include(b => b.Author).ToListAsync();

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
