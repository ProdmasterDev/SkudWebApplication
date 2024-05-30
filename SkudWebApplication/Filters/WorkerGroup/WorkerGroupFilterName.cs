using Microsoft.EntityFrameworkCore;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Filters.WorkerGroup
{
    public class WorkerGroupFilterName : IFilter<DB.WorkerGroup>
    {
        public IQueryable<DB.WorkerGroup> Filter(IQueryable<DB.WorkerGroup> workerGroups, string op, object? value)
        {
            var valueStr = (string)value;
            if (op == "contains")
            {
                return workerGroups.Where(x => EF.Functions.ILike(x.Name, $"%{valueStr}%"));
            }
            if (op == "not contains")
            {
                return workerGroups.Where(x => !EF.Functions.ILike(x.Name, $"%{valueStr}%"));
            }
            if (op == "equals")
            {
                return workerGroups.Where(x => EF.Functions.ILike(x.Name, $"{valueStr}"));
            }
            if (op == "not equals")
            {
                return workerGroups.Where(x => !EF.Functions.ILike(x.Name, $"{valueStr}"));
            }
            if (op == "is empty")
            {
                return workerGroups.Where(x => EF.Functions.ILike(x.Name, string.Empty));
            }
            if (op == "is not empty")
            {
                return workerGroups.Where(x => !EF.Functions.ILike(x.Name, string.Empty));
            }
            if (op == "starts with")
            {
                return workerGroups.Where(x => EF.Functions.ILike(x.Name, $"{valueStr}%"));
            }
            if (op == "ends with")
            {
                return workerGroups.Where(x => EF.Functions.ILike(x.Name, $"%{valueStr}"));
            }
            return workerGroups;
        }
    }
}
