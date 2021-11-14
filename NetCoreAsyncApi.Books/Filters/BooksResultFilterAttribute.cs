namespace NetCoreAsyncApi.Books.Filters
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// These types of filters cannot have constructor dependencies provided by the dependency injection framework 
    /// because they must have their constructor parameters supplied when they're applied. This is a limitation 
    /// of how attributes work.
    /// </summary>
    public class BooksResultFilterAttribute : ResultFilterAttribute
    {
        public BooksResultFilterAttribute() { }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFomAction = context.Result as ObjectResult;

            if (resultFomAction?.Value == null || resultFomAction.StatusCode < 200 || resultFomAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            resultFomAction.Value = mapper.Map<IEnumerable<Models.Book>>(resultFomAction.Value);

            await next();
        }
    }
}
