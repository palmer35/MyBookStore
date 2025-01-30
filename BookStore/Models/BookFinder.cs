using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BookFinder
{
    private readonly BookStoreContext context;
    private readonly ILogger <BookFinder> logger;

    /// <summary>
    /// Подключаем базу данных через зависимость (Dependency Injection)
    /// </summary>
    /// <param name="context"></param>
    public BookFinder(BookStoreContext context, ILogger<BookFinder>logger)
    {
        this.logger = logger;
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
            logger.LogWarning("Попытка добавления книги с неккоректными данными");
            throw new ArgumentNullException(nameof(book));
        }
        logger.LogInformation("Книга {Title} добавлена", book.TitleBook);
        context.Books.Add(book);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Метод получения всех книг
    /// </summary>
    /// <returns></returns>
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksAsync()
    {
        var books = await context.Books.ToListAsync();
        logger.LogInformation("Получены все книги в количестве {Count} экземпляров", books.Count);
        return books;
    }

    /// <summary>
    /// Метод поиска книг по автору
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public async Task<List<Book>> SearchByAuthorAsync(string author)
    {
        logger.LogInformation("Поиск книг по автору '{Author}'", author);

        var books = await context.Books
            .Where(b => b.Author.ToLower() == author.ToLower())
            .ToListAsync();

        if (books == null || !books.Any())
        {
            logger.LogWarning("Книги по автору '{Author}' не найдены", author);
            return new List<Book>();
        }

        logger.LogInformation("Книги по автору '{Author}' найдены: '{Books}'", author, string.Join(", ", books.Select(b => b.TitleBook)));
        return books;
    }



    /// <summary>
    /// Метод поиска книг по жанру
    /// </summary>
    /// <param name="genre"></param>
    /// <returns></returns>
    public async Task<List<Book>> SearchByGenreAsync(string genre)
    {
        logger.LogInformation("Поиск книг по жанру '{Genre}'", genre);

        var books = await context.Books
            .Where(b => b.GenreBook.ToLower().Contains(genre.ToLower()))
            .ToListAsync();

        if (books == null || !books.Any())
        {
            logger.LogWarning("Книги по жанру '{Genre}' не найдены", genre);
            return new List<Book>();
        }
        logger.LogInformation("Книги с жанром '{Genre}' найдены: {Books}", genre, string.Join(", ", books.Select(b => b.TitleBook)));
        return books;
    }


    /// <summary>
    /// Метод получения книги по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Book?> SearchByIdAsync(int id)
    {
        logger.LogInformation("Попытка поиска книги по id = '{Id}'", id);
        var book = await context.Books.FindAsync(id);
        if (book == null)
        {
            logger.LogWarning("Книга с ID = '{ID}' не найдена", id);
            return null; 
        }

        logger.LogInformation("Книга с id = '{ID}' найдена: <{book}>", id, book.TitleBook);
        return book;
    }
}