
using Microsoft.EntityFrameworkCore;
using ProductsService.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductsService.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../ProductsService/Data/SeedData/Brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                context.ProductBrands.AddRange(brands);
            }
            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../ProductsService/Data/SeedData/Types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }
            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../ProductsService/Data/SeedData/Products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
          


        }
    }
}
