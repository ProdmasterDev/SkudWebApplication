using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;
namespace SkudWebApplication.Filters.Event
{
    public class EventFilterFlags : IFilter<VM.Event>
    {
        public IQueryable<VM.Event> Filter(IQueryable<VM.Event> events, string op, object? value)
        {
            var valueDouble = (double)value;
            if (op == "=")
            {
                return events.Where(x => x.Flags == valueDouble);
            }
            if (op == "!=")
            {
                return events.Where(x => x.Flags != valueDouble);
            }
            if (op == ">")
            {
                return events.Where(x => x.Flags > valueDouble);
            }
            if (op == ">=")
            {
                return events.Where(x => x.Flags <= valueDouble);
            }
            if (op == "<")
            {
                return events.Where(x => x.Flags < valueDouble);
            }
            if (op == "<=")
            {
                return events.Where(x => x.Flags <= valueDouble);
            }
            if (op == "is empty")
            {
                return events.Where(x => false);
            }
            if (op == "is not empty")
            {
                return events;
            }
            return events;
        }
    }
}
