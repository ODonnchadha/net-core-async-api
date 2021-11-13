namespace NetCoreAsyncApi.Books.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using NetCoreAsyncApi.Books.Entities;
    using System;

    public class BooksContext : DbContext
    {
        /// <summary>
        /// We start from a book, in our case, and not from an author.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        public BooksContext(DbContextOptions<BooksContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.EnableSensitiveDataLogging();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Author>().HasData(
                new Author
                {
                    Id = Guid.Parse("14cdd303-a816-4890-8885-f37814a4f611"),
                    FirstName = "FLann",
                    LastName = "O'Brien"
                },
                new Author
                {
                    Id = Guid.Parse("38e43b21-abae-431c-bb16-58258701c4f8"),
                    FirstName = "James",
                    LastName = "Joyce"
                },
                new Author
                {
                    Id = Guid.Parse("cdabb18a-03fd-48f0-a5cb-065a66b4698a"),
                    FirstName = "C. K.",
                    LastName = "Chesterton"
                }
            );
            builder.Entity<Book>().HasData(
                new Book
                {
                    Id = Guid.Parse("69772d90-348e-43a2-8965-bc35434081c2"),
                    AuthorId = Guid.Parse("14cdd303-a816-4890-8885-f37814a4f611"),
                    Title = "At Swim-Two-Birds",
                    Description = "At Swim-Two-Birds presents itself as a first-person story by an unnamed Irish student of literature. The student believes that 'one beginning and one ending for a book was a thing I did not agree with', as he accordingly sets three apparently quite separate stories in motion."
                },
                new Book
                {
                    Id = Guid.Parse("10dc9f82-1f19-4ca0-b5db-a6e1511e24e8"),
                    AuthorId = Guid.Parse("14cdd303-a816-4890-8885-f37814a4f611"),
                    Title = "The Third Policeman",
                    Description = "The Third Policeman is set in rural Ireland and is narrated by a dedicated amateur scholar of de Selby, a scientist and philosopher. The narrator, whose name we never learn, is orphaned at a young age. At boarding school, he discovers the work of de Selby and becomes a fanatically dedicated student of it."
                },
                new Book
                {
                    Id = Guid.Parse("c2ec68cf-5401-42e1-9f83-5f90f1c23f90"),
                    AuthorId = Guid.Parse("38e43b21-abae-431c-bb16-58258701c4f8"),
                    Title = "A Portrait Of The Artist As A Young Man",
                    Description = "Born into a middle-class family in Dublin, Ireland, James Joyce excelled as a student, graduating from University College, Dublin, in 1902. He moved to Paris to study medicine, but soon gave it up."
                },
                new Book
                {
                    Id = Guid.Parse("40f7d9fe-9377-4f2c-9578-11d66d50a375"),
                    AuthorId = Guid.Parse("38e43b21-abae-431c-bb16-58258701c4f8"),
                    Title = "Ulysses",
                    Description = "At 8 a.m., Malachi 'Buck' Mulligan, a boisterous medical student, calls aspiring writer Stephen Dedalus up to the roof of the Sandycove Martello tower, where they both live."
                },
                new Book
                {
                    Id = Guid.Parse("9a971366-e2e3-4865-a84b-c4e2135851ee"),
                    AuthorId = Guid.Parse("cdabb18a-03fd-48f0-a5cb-065a66b4698a"),
                    Title = "The Napoleon Of Notting Hill",
                    Description = "The dreary succession of randomly selected Kings of England is broken up when Auberon Quin, who cares for nothing but a good joke, is chosen. To amuse himself, he institutes elaborate costumes for the provosts of the districts of London."
                }
            );
        }
    }
}
