using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.IdentityDTOs
{
    public class UpdateUserAddressDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string? FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string? LastName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Country name must be between 2 and 50 characters.")]
        public string? Country { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "City name must be between 2 and 50 characters.")]
        public string? City { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Street address must be between 3 and 100 characters.")]
        public string? Street { get; set; }
    }
}
