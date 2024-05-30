namespace SkudWebApplication.Filters
{
    public interface IFilter<T>
    {
        public IQueryable<T> Filter(IQueryable<T> events, string op, object? value);
    }
}
