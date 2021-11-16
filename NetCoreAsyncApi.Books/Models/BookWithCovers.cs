namespace NetCoreAsyncApi.Books.Models
{
    using System;
    using System.Collections.Generic;

    public class BookWithCovers
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<BookCover> BookCovers { get; set; } = new List<BookCover> { };
    }
}
