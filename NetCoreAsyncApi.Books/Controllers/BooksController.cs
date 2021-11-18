namespace NetCoreAsyncApi.Books.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAsyncApi.Books.Filters;
    using NetCoreAsyncApi.Books.Interfaces.Repositories;
    using NetCoreAsyncApi.Books.Interfaces.Services;
    using NetCoreAsyncApi.Books.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController, Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookCoverService service;
        private readonly IBookRepository repository;
        private readonly IMapper mapper;
        public BooksController(IBookCoverService service, IBookRepository repository, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [Route("{id}", Name = "GetBook")]
        // [BookResultFilter()]
        [BookWithCoversResultFilter()]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var book = await repository.GetBookAsync(id);
            if (book == null) return NotFound();

            // var bookCover = await service.GetBookCoverAsync("X");
            var bookCovers = await service.GetBookCoversAsync(id);

            // Tuple e.g.:
            //var propertyBag = new Tuple<
            //    Entities.Book, IEnumerable<ExternalModels.BookCover>>(book, bookCovers);
            //var item1 = propertyBag.Item1;
            // C# 7. Value type representation of the Tuple.
            (Entities.Book book, IEnumerable<ExternalModels.BookCover> bookCovers)propertyBag = (book, bookCovers);

            return Ok((book, bookCovers));
            // return Ok(book);
        }

        [HttpGet()]
        [BooksResultFilter()]
        public async Task<IActionResult> GetBooks()
        {
            var books = await repository.GetBooksAsync();

            return Ok(books);
        }

        //[HttpGet()]
        //[BooksResultFilter()]
        //public IActionResult GetBooks()
        //{
        //    // Ensure that the actual entites are returned after the Task is completed instead of returning the Task.
        //    var books = repository.GetBooksAsync().Result;
        //    return Ok(books);
        //}

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
