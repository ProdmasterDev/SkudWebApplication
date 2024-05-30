using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Event
{
    public class EventFilterLastName : IFilter<VM.Event>
    {
        public IQueryable<VM.Event> Filter(IQueryable<VM.Event> events, string op, object? value)
        {
            var valueStr = (string)value;
            if (op == "contains")
            {
                return events.Where(x => EF.Functions.ILike(x.LastName, $"%{valueStr}%"));
            }
            if (op == "not contains")
            {
                return events.Where(x => !EF.Functions.ILike(x.LastName, $"%{valueStr}%"));
            }
            if (op == "equals")
            {
                return events.Where(x => EF.Functions.ILike(x.LastName, $"{valueStr}"));
            }
            if (op == "not equals")
            {
                return events.Where(x => !EF.Functions.ILike(x.LastName, $"{valueStr}"));
            }
            if (op == "is empty")
            {
                return events.Where(x => EF.Functions.ILike(x.LastName, string.Empty));
            }
            if (op == "is not empty")
            {
                return events.Where(x => !EF.Functions.ILike(x.LastName, string.Empty));
            }
            if (op == "starts with")
            {
                return events.Where(x => EF.Functions.ILike(x.LastName, $"{valueStr}%"));
            }
            if (op == "ends with")
            {
                return events.Where(x => EF.Functions.ILike(x.LastName, $"%{valueStr}"));
            }
            return events;
        }
    }
}
