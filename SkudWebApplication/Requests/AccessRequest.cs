namespace SkudWebApplication.Requests
{
    public class AccessRequest
    {
        public long? Id { get; set; }
        public int ControllerLocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public bool Enterance { get; set; }
        public bool Exit { get; set; }
        public bool Both { get; set; }
    }
}
