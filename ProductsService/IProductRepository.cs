using ProductsService.Entity;
using ProductsService.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService
{
   public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsListAsync(ProductSpecParams proParams);
        Task<int> GetProductsCountAsync(ProductSpecParams proParams);
        Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        Task<bool> AddNewProduct(Product newProd);

        Task<Product> EditProduct(int id, Product newProd);
        Task Delete(int id);
        bool IsValidImageFile(string fileName);
        Task<ProductType> AddType(ProductType pt);
        Task<ProductBrand> AddBrand(ProductBrand pb);
        Task EditQuan(int id, int quan);
    }
}
