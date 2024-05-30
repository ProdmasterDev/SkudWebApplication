using System.ComponentModel;

namespace SkudWebApplication.ViewModels
{
    public class WorkerGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<GroupAccess> GroupAccesses { get; set; } = new List<GroupAccess>();
    }

    public class GroupAccess
    {
        public int Id { get; set; }
        public int ControllerLocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public bool Enterance { get; set; }
        public bool Exit { get; set; }
    }
}