namespace SkudWebApplication.ViewModels
{
    public class Card
    {
        public Card() { }
        public int Id { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string CardNumber16 { get; set; } = string.Empty;
        public int? WorkerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
    }
}
