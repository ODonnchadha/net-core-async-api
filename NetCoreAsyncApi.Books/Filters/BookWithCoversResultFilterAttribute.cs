namespace NetCoreAsyncApi.Books.Filters
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BookWithCoversResultFilterAttribute : ResultFilterAttribute
    {
        public BookWithCoversResultFilterAttribute() { }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction?.Value == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            // 1. Deconstructing.
            //var (book, bookCovers) = ((Entities.Book book, 
            //    IEnumerable<ExternalModels.BookCover> bookCovers))resultFromAction.Value;
            // 2. Property names are syntatic sugar.
            //var temp = ((Entities.Book, IEnumerable<ExternalModels.BookCover>))resultFromAction.Value;
            var (book, bookCovers) = ((Entities.Book, 
                IEnumerable<ExternalModels.BookCover>))resultFromAction.Value;

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            var mappedBook = mapper.Map<Models.BookWithCovers>(book);
            resultFromAction.Value = mapper.Map(bookCovers, mappedBook);

            await next();
        }
    }
}
