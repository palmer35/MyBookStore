using BookStore.Models;
using Microsoft.EntityFrameworkCore;

public class BookFinder
{
    private readonly BookStoreContext context;

    /// <summary>
    /// Подключаем базу данных через зависимость (Dependency Injection)
    /// </summary>
    /// <param name="context"></param>
    public BookFinder(BookStoreContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Метод добавления книги
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task AddBookAsync(Book book)
    {
        if (book == null)
        {
            throw new ArgumentNullException(nameof(book));
        }

        context.Books.Add(book);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Метод получения всех книг
    /// </summary>
    /// <returns></returns>
    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await context.Books.ToListAsync();
    }

    /// <summary>
    /// Метод поиска книг по автору
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public async Task<List<Book>> SearchByAuthorAsync(string author)
    {
        return await context.Books
            .Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    /// <summary>
    /// Метод поиска книг по жанру
    /// </summary>
    /// <param name="genre"></param>
    /// <returns></returns>
    public async Task<List<Book>> SearchByGenreAsync(string genre)
    {
        return await context.Books
            .Where(b => b.GenreBook.Contains(genre, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    /// <summary>
    /// Метод получения книги по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Book?> SearchByIdAsync(int id)
    {
        return await context.Books.FindAsync(id);
    }
}