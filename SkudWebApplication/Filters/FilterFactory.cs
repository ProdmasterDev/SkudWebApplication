using SkudWebApplication.Filters.Event;
using SkudWebApplication.Services.Classes;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using SkudWebApplication.Filters.WorkerGroup;
using SkudWebApplication.Filters.AccessGroup;
using SkudWebApplication.Filters.Worker;
using SkudWebApplication.Filters.Card;

namespace SkudWebApplication.Filters
{
    public static class FilterFactory
    {
        public static IFilter<VM.Event>? GetFiltrator(this IQueryable<VM.Event> events, string propertyName)
        {
            if (propertyName == nameof(VM.Event.CardNumber16))
            {
                return new EventFilterCard();
            }
            if (propertyName == nameof(VM.Event.WorkerId))
            {
                return new EventFilterWorkerId();
            }
            if (propertyName == nameof(VM.Event.FullName))
            {
                return new EventFilterFullName();
            }
            if (propertyName == nameof(VM.Event.LastName))
            {
                return new EventFilterLastName();
            }
            if (propertyName == nameof(VM.Event.FirstName))
            {
                return new EventFilterFirstName();
            }
            if (propertyName == nameof(VM.Event.FatherName))
            {
                return new EventFilterFatherName();
            }
            if (propertyName == nameof(VM.Event.Create))
            {
                return new EventFilterCreate();
            }
            if (propertyName == nameof(VM.Event.Flags))
            {
                return new EventFilterFlags();
            }
            return null;
        }
        public static IFilter<DB.WorkerGroup>? GetFiltrator(this IEnumerable<DB.WorkerGroup>? workerGroups, string propertyName)
        {
            if (propertyName == nameof(DB.WorkerGroup.Name))
            {
                return new WorkerGroupFilterName();
            }
            return null;
        }
        public static IFilter<DB.AccessGroup>? GetFiltrator(this IEnumerable<DB.AccessGroup>? accessGroups, string propertyName)
        {
            if (propertyName == nameof(DB.AccessGroup.Name))
            {
                return new AccessGroupFilterName();
            }
            return null;
        }
        public static IFilter<VM.Worker>? GetFiltrator(this IQueryable<VM.Worker> Workers, string propertyName)
        {
            if (propertyName == nameof(VM.Worker.AccessMethodId))
            {
                return new WorkerFilterAccessMethod();
            }
            if (propertyName == nameof(VM.Worker.Comment))
            {
                return new WorkerFilterComment();
            }
            if (propertyName == nameof(VM.Worker.FatherName))
            {
                return new WorkerFilterFatherName();
            }
            if (propertyName == nameof(VM.Worker.FirstName))
            {
                return new WorkerFilterFirstName();
            }
            if (propertyName == nameof(VM.Worker.Id))
            {
                return new WorkerFilterId();
            }
            if (propertyName == nameof(VM.Worker.LastName))
            {
                return new WorkerFilterLastName();
            }
            if (propertyName == nameof(VM.Worker.Position))
            {
                return new WorkerFilterPosition();
            }
            if (propertyName == nameof(VM.Worker.Group))
            {
                return new WorkerFilterWorkerGroupName();
            }
            return null;
        }
        public static IFilter<VM.Card>? GetFiltrator(this IQueryable<VM.Card> Cards, string propertyName)
        {
            if (propertyName == nameof(VM.Card.FatherName))
            {
                return new CardFilterFatherName();
            }
            if (propertyName == nameof(VM.Card.FirstName))
            {
                return new CardFilterFirstName();
            }
            if (propertyName == nameof(VM.Card.FullName))
            {
                return new CardFilterFullName();
            }
            if (propertyName == nameof(VM.Card.Id))
            {
                return new CardFilterId();
            }
            if (propertyName == nameof(VM.Card.LastName))
            {
                return new CardFilterLastName();
            }
            if (propertyName == nameof(VM.Card.CardNumber))
            {
                return new CardFilterNumber();
            }
            if (propertyName == nameof(VM.Card.CardNumber16))
            {
                return new CardFilterNumber16();
            }
            if (propertyName == nameof(VM.Card.WorkerId))
            {
                return new CardFilterWorkerId();
            }
            return null;
        }
    }
}
