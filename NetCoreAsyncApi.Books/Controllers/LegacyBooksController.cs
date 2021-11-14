namespace NetCoreAsyncApi.Books.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAsyncApi.Books.Interfaces.Repositories;
    using System;

    [ApiController, Route("api/legacy/books")]
    public class LegacyBooksController : ControllerBase
    {
        private readonly IBookRepository repository;
        public LegacyBooksController(IBookRepository repository) => this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

        [HttpGet(), Route("{id}")]
        public IActionResult GetBook(Guid id)
        {
            var book = repository.GetBook(id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpGet()]
        public IActionResult GetBooks()
        {
            var books = repository.GetBooks();

            return Ok(books);
        }
    }
}
