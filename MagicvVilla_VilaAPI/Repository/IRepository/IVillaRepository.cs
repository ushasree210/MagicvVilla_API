﻿using MagicvVilla_VilaAPI.Models;
using System.Linq.Expressions;

namespace MagicvVilla_VilaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa,bool>> filter =null);
        Task<Villa> GetAsync(Expression<Func<Villa,bool>> filter = null, bool tracked=true);
        Task CreateAsync(Villa entity);

        Task UpdateAsync(Villa entity);
        Task RemoveAsync(Villa entity);
        Task SaveAsync();

    }
}
