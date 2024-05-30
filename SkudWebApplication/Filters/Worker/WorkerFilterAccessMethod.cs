using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Worker
{
    public class WorkerFilterAccessMethod : IFilter<VM.Worker>
    {
        public IQueryable<VM.Worker> Filter(IQueryable<VM.Worker> workers, string op, object? value)
        {
            var valueDouble = (double)value;
            if (op == "=")
            {
                return workers.Where(x => x.AccessMethodId == valueDouble);
            }
            if (op == "!=")
            {
                return workers.Where(x => x.AccessMethodId != valueDouble);
            }
            if (op == ">")
            {
                return workers.Where(x => x.AccessMethodId > valueDouble);
            }
            if (op == ">=")
            {
                return workers.Where(x => x.AccessMethodId <= valueDouble);
            }
            if (op == "<")
            {
                return workers.Where(x => x.AccessMethodId < valueDouble);
            }
            if (op == "<=")
            {
                return workers.Where(x => x.AccessMethodId <= valueDouble);
            }
            if (op == "is empty")
            {
                return workers.Where(x => x.AccessMethodId == null);
            }
            if (op == "is not empty")
            {
                return workers.Where(x => x.AccessMethodId != null);
            }
            return workers;
        }
    }
}
