using Shop.Domain.Models;

public class UsersService
{
    private readonly BookStoreContext context;
    private readonly ILogger<UsersService> logger;

    /// <summary>
    /// Подключаем базу данных через зависимость (Dependency Injection)
    /// </summary>
    /// <param name="context"></param>
    public UsersService(BookStoreContext context, ILogger<UsersService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    /// <summary>
    /// Метод добавления покупателя
    /// </summary>
    public async Task AddUsersAsync(User user)
    {
        if (user == null)
        {
            logger.LogWarning("Попытка добавления пользователя с неккоректными данными");
            throw new ArgumentNullException(nameof(user));
        }
        logger.LogInformation("Пользователь <{Name}> добавлен", user.Name);
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Метод получения пользователя по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User?> SearchByIdAsync(int id)
    {
        logger.LogInformation("Попытка поиска покупателя по id = '{Id}'", id);
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            logger.LogWarning("Покупатель с id = '{ID}' не найден", id);
            return null;
        }

        logger.LogInformation("Покупатель с id = '{ID}' найден: <{Name}>", id, user.Name);
        return user;
    }


    /// <summary>
    /// Удаление данных о покупателе
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User?> DeleteUser(int id)
    {
        logger.LogInformation("Попытка удаления данных покупателя с id = '{Id}'", id);
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            logger.LogWarning("Покупатель с id = '{ID}' не найден", id);
            return null;
        }
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        logger.LogInformation("Данные покупателя с id = '{ID}' удалены: <{Name}>", id, user.Name);

        return user;
    }

    /// <summary>
    /// Метод покупки книги
    /// </summary>
    /// <param name="bookId">ID книги</param>
    /// <param name="userId">ID пользователя</param>
    /// <returns>Сообщение о результате покупки</returns>
    public async Task<string> PurchaseBookAsync(int bookId, int userId)
    {
        // Поиск пользователя
        logger.LogInformation("Поиск пользователя с id = '{UserId}'", userId);
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            logger.LogWarning("Пользователь с id = '{UserId}' не найден.", userId);
            return $"Пользователь с ID = '{userId}' не найден.";
        }

        // Поиск книги
        logger.LogInformation("Поиск книги с id = '{BookId}'", bookId);
        var book = await context.Books.FindAsync(bookId);
        if (book == null)
        {
            logger.LogWarning("Книга с id = '{BookId}' не найдена.", bookId);
            return $"Книга с ID = '{bookId}' не найден.";
        }

        // Проверка баланса пользователя
        if (user.Wallet < book.Price)
        {
            logger.LogWarning("Недостаточно средств у пользователя '{UserName}' для покупки книги c id = '{BookId}'.", user.Name, bookId);
            return $"Недостаточно средств на счете пользователя '{user.Name}'. Нужно '{book.Price}', а у него только '{user.Wallet}'.";
        }

        // Списание средств
        user.WithdrawFunds(book.Price);

        // Обновление данных в базе
        context.Users.Update(user);
        await context.SaveChangesAsync();

        logger.LogInformation("Пользователь {UserId} успешно купил книгу c id = {BookId}.", userId, bookId);
        return $"Книга '{book.TitleBook}' успешно куплена за '{book.Price}'. Остаток на счете пользователя '{user.Name}': '{user.Wallet}'.";
    }
}
