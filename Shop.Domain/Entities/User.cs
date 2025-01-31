namespace Shop.Domain.Models
{
    public class User
    {
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

        public bool WithdrawFunds(decimal amount)
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
