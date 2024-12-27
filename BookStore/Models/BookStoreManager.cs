using BookStore.Models;

public class BookStoreManager
{
    private BookStoreContext context;

    /// Подключаем базу данных через зависимость
    public BookStoreManager(BookStoreContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Покупка книги
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<string> PurchaseBookAsync(int bookId, User user)
    {
        // Ищем книгу
        var book = await context.Books.FindAsync(bookId);
        if (book == null)
        {
            return "Книга не найдена.";
        }

        // Проверяем, что книга уже куплена
        if (!context.Books.Contains(book))
        {
            return "Книга уже куплена.";
        }

        // Проверяем баланс пользователя
        if (user.Wallet < book.Price)
        {
            return $"Недостаточно средств. Нужно {book.Price}, а у пользователя {user.Wallet}.";
        }

        // Списываем деньги и удаляем книгу
        user.Wallet -= book.Price;
        context.Users.Update(user); // Обновляем данные пользователя
        context.Books.Remove(book); // Удаляем книгу

        // Сохраняем изменения в базе данных
        await context.SaveChangesAsync();

        return $"Покупатель '{user.Name}' купил книгу '{book.TitleBook}' за {book.Price}. Остаток денег: {user.Wallet}.";
    }
}
