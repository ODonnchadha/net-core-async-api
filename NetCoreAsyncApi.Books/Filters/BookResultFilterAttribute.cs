namespace NetCoreAsyncApi.Books.Filters
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;

    /// <summary>
    /// These types of filters cannot have constructor dependencies provided by the dependency injection framework 
    /// because they must have their constructor parameters supplied when they're applied. This is a limitation 
    /// of how attributes work.
    /// </summary>
    public class BookResultFilterAttribute : ResultFilterAttribute
    {
        public BookResultFilterAttribute() { }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;

            if (resultFromAction?.Value == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            //NOTE: This violates the single responsibilitity principle.
            //if (typeof(IEnumerable).IsAssignableFrom(resultFomAction.Value.GetType()))
            //{
            //    resultFomAction.Value = mapper.Map<IEnumerable<Models.Book>>(resultFomAction.Value);
            //}
            //else
            //{
            //    resultFomAction.Value = mapper.Map<Models.Book>(resultFomAction.Value);
            //}
            resultFromAction.Value = mapper.Map<Models.Book>(resultFromAction.Value);

            await next();
        }
    }
}
