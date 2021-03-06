﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAppFoodOrder.Services
{
    public interface IRepository<T>
    {
        Task<T> GetById(string id);

        Task<IEnumerable<T>> GetAll(string filter);

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate);

        Task Add(T item);

        Task Update(T item);

        Task Delete(string id);
    }
}