using System.ComponentModel.DataAnnotations;
namespace BookStore.Models
{
    public class Book
    {
        public int IdBook { get; set; } //id - книги
        public string TitleBook { get; set; } //название книги
        public string Author { get; set; } //Автор книги
        public int PublishedYear { get; set; } //год выпуска
        public string GenreBook { get; set; } //жанр книги
        public decimal Price { get; set; } //цена книги

        public Book() {  }

        public Book(int id, string title, string author, int year, string genre, decimal price)
        {
            IdBook = id;
            TitleBook = title;
            Author = author;
            PublishedYear = year;
            GenreBook = genre;
            Price = price;
        }

    }
}
