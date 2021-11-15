namespace NetCoreAsyncApi.Books.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAsyncApi.Books.Filters;
    using NetCoreAsyncApi.Books.Interfaces.Repositories;
    using NetCoreAsyncApi.Books.ModelBinders;
    using NetCoreAsyncApi.Books.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/bookcollections")]
    [BooksResultFilter()]
    public class BookCollectionsController : ControllerBase
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;
        public BookCollectionsController(IBookRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // URI of the request. The newly-created book collection.
        [HttpGet("({ bookIds })", Name = "GetBookCollection")]
        public async Task<IActionResult> GetBookCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> bookIds)
        {
            var bookEntities = await repository.GetBooksAsync(bookIds);

            if (bookIds.Count() != bookEntities.Count())
            {
                return NotFound();
            }

            return Ok(bookEntities);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateBookCollection(IEnumerable<BookForCreation> booksForCreation)
        {
            var bookEntities = mapper.Map<IEnumerable<Entities.Book>>(booksForCreation);
            
            foreach(var bookEntity in bookEntities)
            {
                repository.AddBook(bookEntity);
            }

            await repository.SaveChangesAsync();

            var booksToReturn = await repository.GetBooksAsync(bookEntities.Select(b => b.Id).ToList());
            var ids = string.Join(",", booksToReturn.Select(b => b.Id));

            return CreatedAtRoute("GetBookCollection", new { ids }, booksToReturn);
        }
    }
}
