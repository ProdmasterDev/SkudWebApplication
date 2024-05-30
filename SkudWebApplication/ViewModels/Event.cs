namespace SkudWebApplication.ViewModels
{
    public class Event
    {
        public int Id { get; set; }
        public string EventTypeName { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string CardNumber16 { get; set; } = string.Empty;
        public int? WorkerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public DateTime Create { get; set; }
        public int Flags { get; set; }
    }
}
