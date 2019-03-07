﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAppFoodOrder.Services;

namespace WebAppFoodOrder.Data
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<T> GetById(string id)
        {
            return _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> Get(Func<T, bool> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToAsyncEnumerable().ToList();
        }

        public async Task Add(T item)
        {
            _dbContext.Set<T>().Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(T item)
        {
            _dbContext.Set<T>().Update(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var item = await _dbContext.Set<T>().FindAsync(id);
            if (item != null)
            {
                _dbContext.Set<T>().Remove(item);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
