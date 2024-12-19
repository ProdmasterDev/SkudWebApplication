namespace SkudWebApplication.Requests
{
    public class AccessGroupWorker
    {
        public int Id { get; set; }
        public int AccessGroupId { get; set; }
        public string AccessGroupName { set; get; } = string.Empty;
        public bool isActive { get; set; }
    }
}
