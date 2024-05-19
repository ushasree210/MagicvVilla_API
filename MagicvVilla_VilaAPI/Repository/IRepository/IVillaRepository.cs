using MagicvVilla_VilaAPI.Models;
using System.Linq.Expressions;

namespace MagicvVilla_VilaAPI.Repository.IRepository
{
    public interface IVillaRepository:IRepository<Villa>
    {


        Task<Villa> UpdateAsync(Villa entity);
        

    }
}
