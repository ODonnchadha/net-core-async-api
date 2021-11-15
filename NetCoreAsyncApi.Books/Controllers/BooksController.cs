namespace NetCoreAsyncApi.Books.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAsyncApi.Books.Filters;
    using NetCoreAsyncApi.Books.Interfaces.Repositories;
    using NetCoreAsyncApi.Books.Models;
    using System;
    using System.Threading.Tasks;

    [ApiController, Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;
        public BooksController(IBookRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [Route("{id}", Name = "GetBook")]
        [BookResultFilter()]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var book = await repository.GetBookAsync(id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpGet()]
        [BooksResultFilter()]
        public async Task<IActionResult> GetBooks()
        {
            var books = await repository.GetBooksAsync();

            return Ok(books);
        }

        [HttpPost()]
        [BookResultFilter()]
        public async Task<IActionResult> CreateBook(BookForCreation bookForCreation)
        {
            var bookEntity = mapper.Map<Entities.Book>(bookForCreation);

            repository.AddBook(bookEntity);
            await repository.SaveChangesAsync();

            // Feth the book from the repository to include the author.
            await repository.GetBookAsync(bookEntity.Id);

            return CreatedAtRoute("GetBook", new { id = bookEntity.Id }, bookEntity);
        }
    }
}
