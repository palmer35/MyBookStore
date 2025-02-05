namespace Shop.Domain.Models
{
    public class Book
    {
        public int IdBook { get; set; } 
        public string TitleBook { get; set; } 
        public string Author { get; set; } 
        public int PublishedYear { get; set; } 
        public string GenreBook { get; set; } 
        public decimal Price { get; set; } 

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
