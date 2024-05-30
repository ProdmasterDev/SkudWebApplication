using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Card
{
    public class CardFilterNumber : IFilter<VM.Card>
    {
        public IQueryable<VM.Card> Filter(IQueryable<VM.Card> cards, string op, object? value)
        {
            var valueStr = (string)value;
            if (op == "contains")
            {
                return cards.Where(x => EF.Functions.ILike(x.CardNumber, $"%{valueStr}%"));
            }
            if (op == "not contains")
            {
                return cards.Where(x => !EF.Functions.ILike(x.CardNumber, $"%{valueStr}%"));
            }
            if (op == "equals")
            {
                return cards.Where(x => EF.Functions.ILike(x.CardNumber, $"{valueStr}"));
            }
            if (op == "not equals")
            {
                return cards.Where(x => !EF.Functions.ILike(x.CardNumber, $"{valueStr}"));
            }
            if (op == "is empty")
            {
                return cards.Where(x => EF.Functions.ILike(x.CardNumber, string.Empty));
            }
            if (op == "is not empty")
            {
                return cards.Where(x => !EF.Functions.ILike(x.CardNumber, string.Empty));
            }
            if (op == "starts with")
            {
                return cards.Where(x => EF.Functions.ILike(x.CardNumber, $"{valueStr}%"));
            }
            if (op == "ends with")
            {
                return cards.Where(x => EF.Functions.ILike(x.CardNumber, $"%{valueStr}"));
            }
            return cards;
        }
    }
}
