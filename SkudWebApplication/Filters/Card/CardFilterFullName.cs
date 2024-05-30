﻿using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Card
{
    public class CardFilterFullName : IFilter<VM.Card>
    {
        public IQueryable<VM.Card> Filter(IQueryable<VM.Card> cards, string op, object? value)
        {
            var valueStr = (string)value;
            if (op == "contains")
            {
                return cards.Where(x => EF.Functions.ILike(x.FullName, $"%{valueStr}%"));
            }
            if (op == "not contains")
            {
                return cards.Where(x => !EF.Functions.ILike(x.FullName, $"%{valueStr}%"));
            }
            if (op == "equals")
            {
                return cards.Where(x => EF.Functions.ILike(x.FullName, $"{valueStr}"));
            }
            if (op == "not equals")
            {
                return cards.Where(x => !EF.Functions.ILike(x.FullName, $"{valueStr}"));
            }
            if (op == "is empty")
            {
                return cards.Where(x => EF.Functions.ILike(x.FullName, string.Empty));
            }
            if (op == "is not empty")
            {
                return cards.Where(x => !EF.Functions.ILike(x.FullName, string.Empty));
            }
            if (op == "starts with")
            {
                return cards.Where(x => EF.Functions.ILike(x.FullName, $"{valueStr}%"));
            }
            if (op == "ends with")
            {
                return cards.Where(x => EF.Functions.ILike(x.FullName, $"%{valueStr}"));
            }
            return cards;
        }
    }
}