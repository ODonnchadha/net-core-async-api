using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreAsyncApi.Books.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("14cdd303-a816-4890-8885-f37814a4f611"), "FLann", "O'Brien" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("38e43b21-abae-431c-bb16-58258701c4f8"), "James", "Joyce" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("cdabb18a-03fd-48f0-a5cb-065a66b4698a"), "C. K.", "Chesterton" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("69772d90-348e-43a2-8965-bc35434081c2"), new Guid("14cdd303-a816-4890-8885-f37814a4f611"), "At Swim-Two-Birds presents itself as a first-person story by an unnamed Irish student of literature. The student believes that 'one beginning and one ending for a book was a thing I did not agree with', as he accordingly sets three apparently quite separate stories in motion.", "At Swim-Two-Birds" },
                    { new Guid("10dc9f82-1f19-4ca0-b5db-a6e1511e24e8"), new Guid("14cdd303-a816-4890-8885-f37814a4f611"), "The Third Policeman is set in rural Ireland and is narrated by a dedicated amateur scholar of de Selby, a scientist and philosopher. The narrator, whose name we never learn, is orphaned at a young age. At boarding school, he discovers the work of de Selby and becomes a fanatically dedicated student of it.", "The Third Policeman" },
                    { new Guid("c2ec68cf-5401-42e1-9f83-5f90f1c23f90"), new Guid("38e43b21-abae-431c-bb16-58258701c4f8"), "Born into a middle-class family in Dublin, Ireland, James Joyce excelled as a student, graduating from University College, Dublin, in 1902. He moved to Paris to study medicine, but soon gave it up.", "A Portrait Of The Artist As A Young Man" },
                    { new Guid("40f7d9fe-9377-4f2c-9578-11d66d50a375"), new Guid("38e43b21-abae-431c-bb16-58258701c4f8"), "At 8 a.m., Malachi 'Buck' Mulligan, a boisterous medical student, calls aspiring writer Stephen Dedalus up to the roof of the Sandycove Martello tower, where they both live.", "Ulysses" },
                    { new Guid("9a971366-e2e3-4865-a84b-c4e2135851ee"), new Guid("cdabb18a-03fd-48f0-a5cb-065a66b4698a"), "The dreary succession of randomly selected Kings of England is broken up when Auberon Quin, who cares for nothing but a good joke, is chosen. To amuse himself, he institutes elaborate costumes for the provosts of the districts of London.", "The Napoleon Of Notting Hill" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
