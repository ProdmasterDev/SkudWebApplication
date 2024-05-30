namespace SkudWebApplication.ViewModels
{
    public class Worker
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public string Position {  get; set; } = string.Empty;
        public string Comment {  get; set; } = string.Empty;
        public string Group {  get; set; } = string.Empty;
        public int? AccessMethodId {  get; set; }
        public DateTime? DateBlock {  get; set; }
        public IEnumerable<WorkerCard> Cards { get; set; } = new List<WorkerCard>();
        public WorkerAccessMethods AccessMethods { get; set; } = new WorkerAccessMethods();
    }

    public class WorkerCard
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Number16 { get; set; } = string.Empty;
    }
    public class WorkerAccessMethods
    {
        public int Selected {  get; set; }
        public IEnumerable<AccessMethod> List { get; set; } = new List<AccessMethod>();
    }
    public class AccessMethod
    {
        public int Id { get; set; } = 2;
        public string Name { get; set; } = string.Empty;
    }
}
