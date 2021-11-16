namespace NetCoreAsyncApi.Books.Profiles
{
    using AutoMapper;
    using System.Collections.Generic;

    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Entities.Book, Models.Book>().ForMember(
                model => model.Author, options => options.MapFrom(
                    entity => $"{entity.Author.LastName}, {entity.Author.FirstName}"));

            CreateMap<Models.BookForCreation, Entities.Book>();

            CreateMap<ExternalModels.BookCover, Models.BookCover>();

            CreateMap<Entities.Book, Models.BookWithCovers>().ForMember(
                model => model.Author, options => options.MapFrom(
                    entity => $"{entity.Author.LastName}, {entity.Author.FirstName}"));
            CreateMap<IEnumerable<ExternalModels.BookCover>, Models.BookWithCovers>().ForMember(
                model => model.BookCovers, options => options.MapFrom(externalModel => externalModel));
        }
    }
}
