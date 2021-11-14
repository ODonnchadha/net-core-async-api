namespace NetCoreAsyncApi.Books.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBookRepository
    {
        Entities.Book GetBook(Guid id);
        IEnumerable<Entities.Book> GetBooks();
        Task<Entities.Book> GetBookAsync(Guid id);
        Task<IEnumerable<Entities.Book>> GetBooksAsync();
    }
}
