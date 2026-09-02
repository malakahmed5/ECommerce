using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderModuleEntities;
using ECommerce.Domain.Entities.ProductModuleEntities;
using ECommerce.Persistence.Data.Contetxts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            var hasProduct = await _dbContext.Products.AnyAsync();
            var hasBrands = await _dbContext.ProductBrands.AnyAsync();
            var hasTypes = await _dbContext.ProductTypes.AnyAsync();
            var hasDeliveryMethods = await _dbContext.DeliveryMethods.AnyAsync();

            if (hasProduct && hasBrands && hasTypes && hasDeliveryMethods) return;

            if (!hasBrands)
                await SeedData<int, ProductBrand>(_dbContext.ProductBrands, "brands.Json");
            if(!hasTypes)
                await SeedData<int, ProductType>(_dbContext.ProductTypes, "types.Json");
            await _dbContext.SaveChangesAsync();

            if (!hasProduct)
                await SeedData<int, Product>(_dbContext.Products, "products.Json");

            if(!hasDeliveryMethods)
                await SeedData<int, DeliveryMethod>(_dbContext.DeliveryMethods, "delivery.json");

            await _dbContext.SaveChangesAsync();
        }

        #region Helper Methods 
        private async static Task SeedData<TKey,TEntity>(DbSet<TEntity> entity , string fileName) 
            where TEntity : BaseEntity<TKey>
        {
            var dataLoaded = await LoadDataFromJsonAsync<TEntity>(fileName); 
            await entity.AddRangeAsync(dataLoaded);
        }
        private async static Task<List<TEntity>> LoadDataFromJsonAsync<TEntity>(string fileName)
        {
            var filePath = @"..\ECommerce.Persistence\SystemData\DataSeed\JsonFiles\" + fileName;

            if (!File.Exists(filePath)) return [];

            var dataStream = File.OpenRead(filePath);

            if(dataStream is null || dataStream.Length == 0) return [];

            var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(dataStream 
                , new JsonSerializerOptions() { PropertyNameCaseInsensitive = true} );

            return data ?? [];
        }
        #endregion
    }
}
