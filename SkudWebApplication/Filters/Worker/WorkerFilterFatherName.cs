﻿using Microsoft.EntityFrameworkCore;
using VM = SkudWebApplication.ViewModels;

namespace SkudWebApplication.Filters.Worker
{
    public class WorkerFilterFatherName : IFilter<VM.Worker>
    {
        public IQueryable<VM.Worker> Filter(IQueryable<VM.Worker> workers, string op, object? value)
        {
            var valueStr = (string)value;
            if (op == "contains")
            {
                return workers.Where(x => EF.Functions.ILike(x.FatherName, $"%{valueStr}%"));
            }
            if (op == "not contains")
            {
                return workers.Where(x => !EF.Functions.ILike(x.FatherName, $"%{valueStr}%"));
            }
            if (op == "equals")
            {
                return workers.Where(x => EF.Functions.ILike(x.FatherName, $"{valueStr}"));
            }
            if (op == "not equals")
            {
                return workers.Where(x => !EF.Functions.ILike(x.FatherName, $"{valueStr}"));
            }
            if (op == "is empty")
            {
                return workers.Where(x => EF.Functions.ILike(x.FatherName, string.Empty));
            }
            if (op == "is not empty")
            {
                return workers.Where(x => !EF.Functions.ILike(x.FatherName, string.Empty));
            }
            if (op == "starts with")
            {
                return workers.Where(x => EF.Functions.ILike(x.FatherName, $"{valueStr}%"));
            }
            if (op == "ends with")
            {
                return workers.Where(x => EF.Functions.ILike(x.FatherName, $"%{valueStr}"));
            }
            return workers;
        }
    }
}