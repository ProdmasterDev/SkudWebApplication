using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;
namespace SkudWebApplication.Filters.Event
{
    public class EventFilterCreate : IFilter<VM.Event>
    {
        public IQueryable<VM.Event> Filter(IQueryable<VM.Event> events, string op, object? value)
        {
            var valueDateTime = ((DateTime)value).ToUniversalTime().AddHours(3);
            if (op == "is")
            {
                return events.Where(x => x.Create.ToUniversalTime().AddHours(3).Date == valueDateTime.AddHours(3).Date);
            }
            if (op == "is not")
            {
                return events.Where(x => x.Create.ToUniversalTime().AddHours(3).Date != valueDateTime.AddHours(3).Date);
            }
            if (op == "is after")
            {
                return events.Where(x => x.Create > valueDateTime);
            }
            if (op == "is on or after")
            {
                return events.Where(x => x.Create >= valueDateTime);
            }
            if (op == "is before")
            {
                return events.Where(x => x.Create < valueDateTime);
            }
            if (op == "is on or before")
            {
                return events.Where(x => x.Create <= valueDateTime);
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
