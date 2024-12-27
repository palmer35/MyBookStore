using Microsoft.EntityFrameworkCore;
using BookStore.Models;
public class BookStoreContext : DbContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<User>Users { get; set; }

    public BookStoreContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host = localhost; Port = 5432; Database = BookStoreDB; Username = postgres; Password = 12345");
    }
}