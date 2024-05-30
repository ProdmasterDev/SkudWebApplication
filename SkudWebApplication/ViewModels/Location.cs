namespace SkudWebApplication.ViewModels
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ControllerId { get; set; }
        public string ControllerSn { get; set; } = string.Empty;
        public string ControllerIp { get; set; } = string.Empty;
    }
}
