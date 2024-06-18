using MagicvVilla_VilaAPI.Models;
using MagicvVilla_VilaAPI.Models.Dto;

namespace MagicvVilla_VilaAPI.Repository.IRepository
{
    public interface IUserRepository
    {
         bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestaDTO);
        Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
