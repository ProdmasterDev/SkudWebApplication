using ControllerDomain.Entities;

namespace SkudWebApplication.ViewModels
{
    public class AccessGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AccessGroupAccesses
    {
        public long Id { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public bool Enterance { get; set; }
        public bool Exit { get; set; }
        public virtual ControllerLocation? ControllerLocation { get; set; }
        public virtual AccessGroup? AccessGroup { get; set; }
    }
}
