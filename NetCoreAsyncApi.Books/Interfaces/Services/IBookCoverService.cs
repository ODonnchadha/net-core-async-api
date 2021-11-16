namespace NetCoreAsyncApi.Books.Interfaces.Services
{
    using NetCoreAsyncApi.Books.ExternalModels;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBookCoverService
    {
        Task<BookCover> GetBookCoverAsync(string name);
        Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId);
    }
}
