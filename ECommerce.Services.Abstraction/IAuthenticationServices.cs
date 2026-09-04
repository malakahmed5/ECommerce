using ECommerce.Shared.CommonResponses;
using ECommerce.Shared.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IAuthenticationServices
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO  registerDTO);
        Task<bool> IsEmailExistAsync(string email);
        Task<Result<UserDTO>> GetCurrentUserByEmailAsync(string email);
        Task<Result<UserAddressDTO>> GetUserAddressAsync(string email);
        Task<Result<UserAddressDTO>> UpdateUserAddressAsync(string email,UserAddressDTO updateUserAddress);
    }
}
