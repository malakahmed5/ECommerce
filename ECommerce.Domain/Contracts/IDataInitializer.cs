using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface IDataInitializer
    {
        public Task InitializeAsync();
    }
}
