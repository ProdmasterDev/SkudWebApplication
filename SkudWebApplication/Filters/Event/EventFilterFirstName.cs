using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Event
{
    public class EventFilterFirstName : IFilter<VM.Event>
    {
        public IQueryable<VM.Event> Filter(IQueryable<VM.Event> events, string op, object? value)
        {
            var valueStr = (string)value;
            if (op == "contains")
            {
                return events.Where(x => EF.Functions.ILike(x.FirstName, $"%{valueStr}%"));
            }
            if (op == "not contains")
            {
                return events.Where(x => !EF.Functions.ILike(x.FirstName, $"%{valueStr}%"));
            }
            if (op == "equals")
            {
                return events.Where(x => EF.Functions.ILike(x.FirstName, $"{valueStr}"));
            }
            if (op == "not equals")
            {
                return events.Where(x => !EF.Functions.ILike(x.FirstName, $"{valueStr}"));
            }
            if (op == "is empty")
            {
                return events.Where(x => EF.Functions.ILike(x.FirstName, string.Empty));
            }
            if (op == "is not empty")
            {
                return events.Where(x => !EF.Functions.ILike(x.FirstName, string.Empty));
            }
            if (op == "starts with")
            {
                return events.Where(x => EF.Functions.ILike(x.FirstName, $"{valueStr}%"));
            }
            if (op == "ends with")
            {
                return events.Where(x => EF.Functions.ILike(x.FirstName, $"%{valueStr}"));
            }
            return events;
        }
    }
}
