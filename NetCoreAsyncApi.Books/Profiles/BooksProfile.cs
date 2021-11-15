namespace NetCoreAsyncApi.Books.Profiles
{
    using AutoMapper;
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Entities.Book, Models.Book>().ForMember(
                m => m.Author, options => options.MapFrom(
                    e => $"{e.Author.LastName}, {e.Author.FirstName}"));
            CreateMap<Models.BookForCreation, Entities.Book>();
        }
    }
}
