using System.ComponentModel.DataAnnotations;
namespace BookStore.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Wallet { get; set; }

        public User() { }

        public User(int id, string name, string email, string phone, decimal wallet)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phone;
            Wallet = wallet;
        }

        public void PrintUser()
        {
            Console.WriteLine("Информвция о покупателе: ");
            Console.WriteLine($"ID: {Id}, Имя : {Name}, Почта: {Email}, Телефон: {PhoneNumber}, Общая сумма: {Wallet}");
        }

        public bool DeductBalance(decimal amount)
        {
            if (Wallet >= amount)
            {
                Wallet -= amount;
                return true;
            }
            return false;
        }
    }
}
