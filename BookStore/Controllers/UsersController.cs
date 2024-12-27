using Microsoft.AspNetCore.Mvc;
using BookStore.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private BookStoreManager bookStoreManager;
    private BookStoreContext context;

    // Добавление пользователя
    [HttpPost]
    public async Task<ActionResult> AddUser([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest(new { Message = "Некорректные данные пользователя." });
        }

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    // Получение пользователя по ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { Message = "Пользователь не найден." });
        }

        return Ok(user);
    }

    public UsersController(BookStoreManager bookStoreManager, BookStoreContext context)
    {
        this.bookStoreManager = bookStoreManager;
        this.context = context;
    }

    // Покупка книги
    [HttpPost("{userId}/purchase/{bookId}")]
    public async Task<ActionResult> PurchaseBook(int userId, int bookId)
    {
        // Ищем пользователя
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound("Пользователь не найден.");
        }

        // Ищем книгу
        var book = await context.Books.FindAsync(bookId);
        if (book == null)
        {
            return NotFound("Книга не найдена.");
        }

        // Покупаем книгу
        var result = await bookStoreManager.PurchaseBookAsync(bookId, user);

        return Ok(result);
    }
}