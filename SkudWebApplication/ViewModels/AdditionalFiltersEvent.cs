namespace SkudWebApplication.ViewModels
{
    public class AdditionalFiltersEvent
    {
        public ControllerLocationsFilter ControllerLocationsFilter { get; set; } = new ControllerLocationsFilter();
        public WorkerGroupsFilter WorkerGroupsFilter { get; set; } = new WorkerGroupsFilter();
    }

    public class ControllerLocationsFilter
    {
        public IEnumerable<ControllerLocationSelect> ControllerLocationSelect { get; set; } = new List<ControllerLocationSelect>();
    }
    public class WorkerGroupsFilter
    {
        public string StringFilter { get; set; } = string.Empty;
        public IEnumerable<WorkerGroupSelect> WorkerGroupSelects { get; set; } = new List<WorkerGroupSelect>();
        public IEnumerable<WorkerGroupSelect> WorkerGroupSelectsFiltered { get { return WorkerGroupSelects.Where(x => x.WorkerGroup.Name.Contains(StringFilter)); } }
    }
    public class ControllerLocationSelect
    {
        public bool Selected { get; set; }
        public Location Location { get; set; } = new Location();
    }
    public class WorkerGroupSelect
    {
        public bool Selected { get; set; }
        public WorkerGroup WorkerGroup { get; set; } = new WorkerGroup();
    }
}
