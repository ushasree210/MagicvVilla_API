using MagicvVilla_VilaAPI.Models;

namespace MagicvVilla_VilaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository: IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateAsync(VillaNumber entity);

    }
}
