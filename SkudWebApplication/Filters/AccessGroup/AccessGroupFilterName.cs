using Microsoft.EntityFrameworkCore;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Filters.AccessGroup
{
    public class AccessGroupFilterName : IFilter<DB.AccessGroup>
    {
        public IQueryable<DB.AccessGroup> Filter(IQueryable<DB.AccessGroup> accessGroups, string op, object? value)
        {
            var valueStr = (string)value;
            if (op == "contains")
            {
                return accessGroups.Where(x => EF.Functions.ILike(x.Name, $"%{valueStr}%"));
            }
            if (op == "not contains")
            {
                return accessGroups.Where(x => !EF.Functions.ILike(x.Name, $"%{valueStr}%"));
            }
            if (op == "equals")
            {
                return accessGroups.Where(x => EF.Functions.ILike(x.Name, $"{valueStr}"));
            }
            if (op == "not equals")
            {
                return accessGroups.Where(x => !EF.Functions.ILike(x.Name, $"{valueStr}"));
            }
            if (op == "is empty")
            {
                return accessGroups.Where(x => EF.Functions.ILike(x.Name, string.Empty));
            }
            if (op == "is not empty")
            {
                return accessGroups.Where(x => !EF.Functions.ILike(x.Name, string.Empty));
            }
            if (op == "starts with")
            {
                return accessGroups.Where(x => EF.Functions.ILike(x.Name, $"{valueStr}%"));
            }
            if (op == "ends with")
            {
                return accessGroups.Where(x => EF.Functions.ILike(x.Name, $"%{valueStr}"));
            }
            return accessGroups;
        }
    }
}
