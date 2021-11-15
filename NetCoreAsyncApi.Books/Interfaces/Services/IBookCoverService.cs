namespace NetCoreAsyncApi.Books.Interfaces.Services
{
    using NetCoreAsyncApi.Books.ExternalModels;
    using System.Threading.Tasks;

    public interface IBookCoverService
    {
        Task<BookCover> GetBookCoverAsync(string name);
    }
}
