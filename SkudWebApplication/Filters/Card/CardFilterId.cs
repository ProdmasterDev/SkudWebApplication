using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Card
{
    public class CardFilterId : IFilter<VM.Card>
    {
        public IQueryable<VM.Card> Filter(IQueryable<VM.Card> cards, string op, object? value)
        {
            var valueDouble = (double)value;
            if (op == "=")
            {
                return cards.Where(x => x.Id == valueDouble);
            }
            if (op == "!=")
            {
                return cards.Where(x => x.Id != valueDouble);
            }
            if (op == ">")
            {
                return cards.Where(x => x.Id > valueDouble);
            }
            if (op == ">=")
            {
                return cards.Where(x => x.Id <= valueDouble);
            }
            if (op == "<")
            {
                return cards.Where(x => x.Id < valueDouble);
            }
            if (op == "<=")
            {
                return cards.Where(x => x.Id <= valueDouble);
            }
            if (op == "is empty")
            {
                return cards.Where(x => false);
            }
            if (op == "is not empty")
            {
                return cards;
            }
            return cards;
        }
    }
}
