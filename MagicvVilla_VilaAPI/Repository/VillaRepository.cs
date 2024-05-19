using MagicvVilla_VilaAPI.Data;
using MagicvVilla_VilaAPI.Models;
using MagicvVilla_VilaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace MagicvVilla_VilaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

  
        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
       
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;

        }

    }
}
