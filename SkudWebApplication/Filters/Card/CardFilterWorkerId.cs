using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Card
{
    public class CardFilterWorkerId : IFilter<VM.Card>
    {
        public IQueryable<VM.Card> Filter(IQueryable<VM.Card> cards, string op, object? value)
        {
            var valueDouble = (double)value;
            if (op == "=")
            {
                return cards.Where(x => x.WorkerId == valueDouble);
            }
            if (op == "!=")
            {
                return cards.Where(x => x.WorkerId != valueDouble);
            }
            if (op == ">")
            {
                return cards.Where(x => x.WorkerId > valueDouble);
            }
            if (op == ">=")
            {
                return cards.Where(x => x.WorkerId <= valueDouble);
            }
            if (op == "<")
            {
                return cards.Where(x => x.WorkerId < valueDouble);
            }
            if (op == "<=")
            {
                return cards.Where(x => x.WorkerId <= valueDouble);
            }
            if (op == "is empty")
            {
                return cards.Where(x => x.WorkerId == null);
            }
            if (op == "is not empty")
            {
                return cards.Where(x => x.WorkerId != null);
            }
            return cards;
        }
    }
}
