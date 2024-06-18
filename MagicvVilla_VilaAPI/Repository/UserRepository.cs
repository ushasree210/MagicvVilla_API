using AutoMapper;
using MagicvVilla_VilaAPI.Data;
using MagicvVilla_VilaAPI.Models;
using MagicvVilla_VilaAPI.Models.Dto;
using MagicvVilla_VilaAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicvVilla_VilaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IMapper mapper,IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");

        }
        public bool IsUniqueUser(string username)
        {
            if (_db.LocalUsers.FirstOrDefault(u => u.UserName == username) == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestaDTO)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestaDTO.Username.ToLower()
            && u.Password == loginRequestaDTO.Password);
            if (user == null)
            {
              return new LoginResponseDTO()
                {
                    Token ="",
                    User = null,
                };
            }
            //if user was found generate JWT Token

            var tokenHandler = new JwtSecurityTokenHandler();
            //access secret key which is in strnng and encode it to byte array
            var key = Encoding.ASCII.GetBytes(secretKey);
            //configure tokenDescriptor ==> it contains  claims , sigin credentilas etc
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,user.Id.ToString()),
                        new Claim(ClaimTypes.Role,user.Role)
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //generating token

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user,
            };

            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var user=_mapper.Map<LocalUser>(registrationRequestDTO);
            _db.LocalUsers.Add(user);
           await  _db.SaveChangesAsync();
            user.Password = "";
            //after saving the user, you clear the password field in the object, though this change won't affect the database unless you explicitly save changes again.
            return user;
        }

    }
}

