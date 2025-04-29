
using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<BooksDbContext>(options =>
                options.UseSqlite("Data Source=books.db"));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BooksDbContext>();
                db.Database.EnsureCreated();
            }

            app.MapGet("/api/books", async (BooksDbContext db) =>
            (
                await db.Books.ToArrayAsync()
            )
            );

            app.MapGet("/api/books/{id}", async (int id, BooksDbContext db) =>
            {
                var book = await db.Books.FindAsync(id);
                if (book != null)
                {
                    return Results.Ok(book);
                }
                return Results.NotFound($"book (id:{id}) not found");

            });

            app.MapPost("/api/books", async (Book book, BooksDbContext db) =>
            {
                if(book != null)
                {
                    db.Books.Add(book);
                    await db.SaveChangesAsync();

                    return Results.Created($"/api/books/{book.Id}", book);
                }
                return Results.BadRequest();
            });

            app.MapPut("/api/books/{id}", async (int id, Book input, BooksDbContext db) =>
            {
                var book = await db.Books.FindAsync(id);
                if (book == null)
                {
                    return Results.NotFound($"book (id:{id}) not found");
                }

                book.Title = input.Title;
                book.Author = input.Author;
                book.PublishedYear = input.PublishedYear;
                book.IsRead = input.IsRead;

                await db.SaveChangesAsync();

                return Results.Ok($"book (id:{id}) was successfully edited");
            });

            app.MapDelete("/api/books/{id}", async (int id, BooksDbContext db) =>
            {
                var book = await db.Books.FindAsync(id);
                if (book is null)
                {
                    return Results.NotFound($"book (id:{id}) not found");
                }

                db.Books.Remove(book);
                await db.SaveChangesAsync();

                return Results.Ok($"removed book (id:{id})");
            });

            app.Run();
        }
    }
}
