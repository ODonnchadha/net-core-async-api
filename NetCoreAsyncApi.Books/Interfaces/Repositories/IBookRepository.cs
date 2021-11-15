namespace NetCoreAsyncApi.Books.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBookRepository
    {
        void AddBook(Entities.Book book);
        Entities.Book GetBook(Guid id);
        IEnumerable<Entities.Book> GetBooks();
        Task<Entities.Book> GetBookAsync(Guid id);
        Task<IEnumerable<Entities.Book>> GetBooksAsync();
        Task<IEnumerable<Entities.Book>> GetBooksAsync(IEnumerable<Guid> ids);
        Task<bool> SaveChangesAsync();
    }
}
