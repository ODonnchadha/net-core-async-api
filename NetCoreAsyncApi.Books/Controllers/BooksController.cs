namespace NetCoreAsyncApi.Books.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAsyncApi.Books.Filters;
    using NetCoreAsyncApi.Books.Interfaces.Repositories;
    using System;
    using System.Threading.Tasks;

    [ApiController, Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository repository;
        public BooksController(IBookRepository repository) => this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

        [HttpGet(), Route("{id}"), BookResultFilter()]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var book = await repository.GetBookAsync(id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpGet(), BooksResultFilter()]
        public async Task<IActionResult> GetBooks()
        {
            var books = await repository.GetBooksAsync();

            return Ok(books);
        }
    }
}
