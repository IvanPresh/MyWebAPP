using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPP.Data;
using MyWebAPP.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly MyBookShopContext _context;

        // Constructor that initializes the controller with the database context
        public BooksController(MyBookShopContext context)
        {
            _context = context;
        }

        // GET: api/Books/Get-Books
        // Retrieves all books from the database
        [HttpGet("Get-Books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            // Asynchronously fetches all books from the database and returns them as an ActionResult
            return await _context.books.ToListAsync();
        }

        // POST: api/Books/Add-Book
        // Adds a new book to the database
        [HttpPost("Add-Book")]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            // Adds the new book entity to the database context
            _context.books.Add(book);

            // Saves changes asynchronously to the database
            await _context.SaveChangesAsync();

            // Returns a 201 Created response with the location of the newly created book
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // GET: api/Books/Get-Book/{id}
        // Retrieves a specific book by its ID
        [HttpGet("Get-Book/{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            // Attempts to find the book with the given ID in the database
            var book = await _context.books.FindAsync(id);

            // Returns a 404 Not Found response if the book doesn't exist
            if (book == null)
            {
                return NotFound();
            }

            // Returns the book if found
            return book;
        }

        [HttpPut("Edit-Book/{id}")]
        public async Task<ActionResult<Book>> EditBook(int id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.books.Any(b => b.Id != id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("Delete - Book /{id}")]
        public async Task<IActionResult> DeleteBook(int id) 
        {
            var book = _context.books.Find(id);
            if (book == null)
            { 
                return NotFound(); 
            }
             _context.books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
