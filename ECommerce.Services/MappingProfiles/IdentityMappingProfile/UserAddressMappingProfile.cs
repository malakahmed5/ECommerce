using AutoMapper;
using ECommerce.Domain.Entities.IdentityModuleEntities;
using ECommerce.Shared.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles.IdentityMappingProfile
{
    public class UserAddressMappingProfile:Profile
    {
        public UserAddressMappingProfile()
        {
            CreateMap<Address, UserAddressDTO>();
            CreateMap<UpdateUserAddressDTO, Address>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
