using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookFinder bookFinder;

    public BooksController(BookFinder bookFinder)
    {
        this.bookFinder = bookFinder;
    }

    /// <summary>
    /// Добавление книги
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> AddBook([FromBody] Book book)
    {
        if (book == null)   
        {
            return BadRequest(new { Message = "Некорректные данные книги." });
        }

        await bookFinder.AddBookAsync(book);
        return CreatedAtAction(nameof(GetBookById), new { id = book.IdBook }, book);
    }

    /// <summary>
    /// Получение всех книг
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
    {
        var books = await bookFinder.GetAllBooksAsync();
        return Ok(books);
    }

    /// <summary>
    /// Поиск книг по автору
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    [HttpGet("search/author")]
    public async Task<ActionResult<IEnumerable<Book>>> SearchByAuthor([FromQuery] string author)
    {
        var books = await bookFinder.SearchByAuthorAsync(author);
        if (!books.Any())
        {
            return NotFound(new { Message = "Книги по данному автору не найдены." });
        }
        return Ok(books);
    }

    /// <summary>
    /// Поиск книг по жанру
    /// </summary>
    /// <param name="genre"></param>
    /// <returns></returns>
    [HttpGet("search/genre")]
    public async Task<ActionResult<IEnumerable<Book>>> SearchByGenre([FromQuery] string genre)
    {
        var books = await bookFinder.SearchByGenreAsync(genre);
        if (!books.Any())
        {
            return NotFound(new { Message = "Книги по данному жанру не найдены." });
        }
        return Ok(books);
    }

    /// <summary>
    /// Получение книги по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
        var book = await bookFinder.SearchByIdAsync(id);

        if (book == null)
        {
            return NotFound(new { Message = "Книга не найдена." });
        }

        return Ok(book); 
    }
}
