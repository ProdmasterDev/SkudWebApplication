using MudBlazor;
using SkudWebApplication.Services.Classes;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Filters
{
    public static class Filtrator
    {
        public static IQueryable<VM.Event>? Filter(this IQueryable<VM.Event>? events, IFilterDefinition<VM.Event>? filter)
        {
            if (events == null || filter == null || filter.Column == null || filter.Column.PropertyName == string.Empty || filter.Operator == null || filter.Operator == string.Empty)
            {
                return events;
            }
            var filtrator = events.GetFiltrator(filter.Column.PropertyName);
            if (filtrator == null)
            {
                return events;
            }
            return filtrator.Filter(events, filter.Operator, filter.Value);
        }
        public static IQueryable<DB.WorkerGroup>? Filter(this IQueryable<DB.WorkerGroup>? workerGroups, IFilterDefinition<VM.WorkerGroup>? filter)
        {
            if (workerGroups == null || filter == null || filter.Column == null || filter.Column.PropertyName == string.Empty || filter.Operator == null || filter.Operator == string.Empty)
            {
                return workerGroups;
            }
            var filtrator = workerGroups.GetFiltrator(filter.Column.PropertyName);
            if (filtrator == null)
            {
                return workerGroups;
            }
            return filtrator.Filter(workerGroups, filter.Operator, filter.Value);
        }
        public static IQueryable<DB.AccessGroup>? Filter(this IQueryable<DB.AccessGroup> accessGroups, IFilterDefinition<VM.AccessGroup>? filter)
        {
            if (accessGroups == null || filter == null || filter.Column == null || filter.Column.PropertyName == string.Empty || filter.Operator == null || filter.Operator == string.Empty)
            {
                return accessGroups;
            }
            var filtrator = accessGroups.GetFiltrator(filter.Column.PropertyName);
            if (filtrator == null)
            {
                return accessGroups;
            }
            return filtrator.Filter(accessGroups, filter.Operator, filter.Value);
        }
        public static IQueryable<VM.Worker>? Filter(this IQueryable<VM.Worker>? Workers, IFilterDefinition<VM.Worker>? filter)
        {
            if (Workers == null || filter == null || filter.Column == null || filter.Column.PropertyName == string.Empty || filter.Operator == null || filter.Operator == string.Empty)
            {
                return Workers;
            }
            var filtrator = Workers.GetFiltrator(filter.Column.PropertyName);
            if (filtrator == null)
            {
                return Workers;
            }
            return filtrator.Filter(Workers, filter.Operator, filter.Value);
        }
        public static IQueryable<VM.Card>? Filter(this IQueryable<VM.Card>? Cards, IFilterDefinition<VM.Card>? filter)
        {
            if (Cards == null || filter == null || filter.Column == null || filter.Column.PropertyName == string.Empty || filter.Operator == null || filter.Operator == string.Empty)
            {
                return Cards;
            }
            var filtrator = Cards.GetFiltrator(filter.Column.PropertyName);
            if (filtrator == null)
            {
                return Cards;
            }
            return filtrator.Filter(Cards, filter.Operator, filter.Value);
        }
    }
}
