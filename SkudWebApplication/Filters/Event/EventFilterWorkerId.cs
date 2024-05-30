using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;
namespace SkudWebApplication.Filters.Event
{
    public class EventFilterWorkerId : IFilter<VM.Event>
    {
        public IQueryable<VM.Event> Filter(IQueryable<VM.Event> events, string op, object? value)
        {
            var valueDouble = (double)value;
            if (op == "=")
            {
                return events.Where(x => x.WorkerId == valueDouble);
            }
            if (op == "!=")
            {
                return events.Where(x => x.WorkerId != valueDouble);
            }
            if (op == ">")
            {
                return events.Where(x => x.WorkerId > valueDouble);
            }
            if (op == ">=")
            {
                return events.Where(x => x.WorkerId <= valueDouble);
            }
            if (op == "<")
            {
                return events.Where(x => x.WorkerId < valueDouble);
            }
            if (op == "<=")
            {
                return events.Where(x => x.WorkerId <= valueDouble);
            }
            if (op == "is empty")
            {
                return events.Where(x => x.WorkerId == null);
            }
            if (op == "is not empty")
            {
                return events.Where(x => x.WorkerId != null);
            }
            return events;
        }
    }
}
