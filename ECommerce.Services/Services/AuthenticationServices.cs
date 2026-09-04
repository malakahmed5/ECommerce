using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModuleEntities;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonResponses;
using ECommerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ECommerce.Services.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthenticationServices(
        UserManager<ApplicationUser> userManager, 
        IConfiguration configuration,IMapper mapper)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user is null)
            return Error.InvalidCredintals("InvalidCredentials");

        var isPasswordExist = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
        if (!isPasswordExist)
            return Error.InvalidCredintals("InvalidCredentials");

        var token = await CreateTokenAsync(user);
        return new UserDTO(user.Email!,user.DisplayName,token);  
    }

    public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
    {
        var user = new ApplicationUser()
        {
            Email = registerDTO.Email,
            DisplayName = registerDTO.DisplayName,
            PhoneNumber = registerDTO.PhoneNumber,
            UserName = registerDTO.UserName,
        };
        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            var token = await CreateTokenAsync(user);
            return new UserDTO(Email: registerDTO.Email!, DisplayName: registerDTO.DisplayName, Token: token);
        }

        return result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
    }

    public async Task<Result<UserDTO>> GetCurrentUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Error.NotFound("User.NotFound", $"User With This Emil '{email}' Is Not Found");

        var token = await CreateTokenAsync(user);
        return new UserDTO(user.Email!, user.DisplayName, token);
    }

    public async Task<bool> IsEmailExistAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user is not null;
    }

    public async Task<Result<UserAddressDTO>> GetUserAddressAsync(string email)
    {
        var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
            return Error.NotFound("User.NotFound", $"User With This Email '{email}' Is Not Found");

        if(user.Address is null)
            return Error.NotFound("UserAddress.NotFound", $"User With This Email '{email}' Has No Address");

        return _mapper.Map<UserAddressDTO>(user.Address);

    }

    public async Task<Result<UserAddressDTO>> UpdateUserAddressAsync(string email, UserAddressDTO updateUserAddress)
    {
        var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
            return Error.NotFound("User.NotFound", $"User With This Email '{email}' Is Not Found");
        if(user.Address is null)
            return Error.NotFound("UserAddress.NotFound", $"User With This Email '{email}' Has No Address");

        user.Address.FristName = updateUserAddress.FristName;
        user.Address.LastName = updateUserAddress.LastName;
        user.Address.Country = updateUserAddress.Country;
        user.Address.City = updateUserAddress.City;
        user.Address.Street = updateUserAddress.Street;

        var result = await _userManager.UpdateAsync(user);

        if(!result.Succeeded)
            return result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

        return _mapper.Map<UserAddressDTO>(user.Address);
    }



    #region Helper Method

    private async Task<string> CreateTokenAsync(ApplicationUser user)
    {
        //Token  : [Issuer - Audience - expire Date - claims - SiginCredentials]
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email , user.Email!),
            new Claim(JwtRegisteredClaimNames.Name , user.DisplayName!),
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));
        var secretKey = _configuration["JWTOptions:SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWTOptions:Issuer"],
            audience: _configuration["JWTOptions:Audience"],
            expires: DateTime.UtcNow.AddHours(1),
            claims: claims,
            signingCredentials: credentials
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    #endregion

}
