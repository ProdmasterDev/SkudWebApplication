using SkudWebApplication.Components.Pages;
using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Worker
{
    public class WorkerFilterId : IFilter<VM.Worker>
    {
        public IQueryable<VM.Worker> Filter(IQueryable<VM.Worker> workers, string op, object? value)
        {
            var valueDouble = (double)value;
            if (op == "=")
            {
                return workers.Where(x => x.Id == valueDouble);
            }
            if (op == "!=")
            {
                return workers.Where(x => x.Id != valueDouble);
            }
            if (op == ">")
            {
                return workers.Where(x => x.Id > valueDouble);
            }
            if (op == ">=")
            {
                return workers.Where(x => x.Id <= valueDouble);
            }
            if (op == "<")
            {
                return workers.Where(x => x.Id < valueDouble);
            }
            if (op == "<=")
            {
                return workers.Where(x => x.Id <= valueDouble);
            }
            if (op == "is empty")
            {
                return workers.Where(x => false);
            }
            if (op == "is not empty")
            {
                return workers;
            }
            return workers;
        }
    }
}
