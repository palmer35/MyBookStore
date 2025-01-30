using Microsoft.AspNetCore.Mvc;
using BookStore.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private UserFinder userFinder;
    private BookStoreContext context;

    public UsersController(UserFinder userFinder, BookStoreContext context)
    {
        this.userFinder = userFinder;
        this.context = context;
    }

    /// <summary>
    /// Добавление пользователя
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
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

    /// Получение пользователя по ID
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
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// Покупка книги
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