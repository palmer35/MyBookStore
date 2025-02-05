using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private UsersService userFinder;
    private BookStoreContext context;

    public UsersController(UsersService userFinder, BookStoreContext context)
    {
        this.userFinder = userFinder;
        this.context = context;
    }

    /// <summary>
    /// Добавление пользователя
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> AddUser([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest(new { Message = "Некорректные данные пользователя." });
        }

        await userFinder.AddUsersAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Получение пользователя по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await userFinder.SearchByIdAsync(id);

        if (user == null)
        {
            return NotFound(new { Message = "Пользователь не найден." });
        }

        return Ok(user);
    }

    /// <summary>
    /// Удаление пользователя по ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { Message = "Пользователь не найден." });
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return Ok(new { Message = $"Пользователь '{user.Name}' с ID '{user.Id}' успешно удален." });
    }

    /// <summary>
    /// Покупка книги
    /// </summary>
    [HttpPost("{userId}/purchase/{bookId}")]
    public async Task<ActionResult> PurchaseBook(int userId, int bookId)
    {
        // Ищем пользователя
        var user = await userFinder.SearchByIdAsync(userId);
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
        var result = await userFinder.PurchaseBookAsync(bookId, userId);

        return Ok(result);
    }
 
}