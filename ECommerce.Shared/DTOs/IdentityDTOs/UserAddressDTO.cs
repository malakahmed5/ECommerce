using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.IdentityDTOs
{
    public class UserAddressDTO
    {
        public int Id { get; set; }
        public string FristName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
    }
}
