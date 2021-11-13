namespace NetCoreAsyncApi.Books.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("Books")]
    public class Book
    {
        [Key()]
        public Guid Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [Required, MaxLength(2500)]
        public string Description { get; set; }

        /// <summary>
        /// Foreign Key.
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Navigation Property.
        /// </summary>
        public Author Author { get; set; }
    }
}
