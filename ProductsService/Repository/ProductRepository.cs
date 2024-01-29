using Microsoft.EntityFrameworkCore;
using ProductsService.Entity;
using ProductsService.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.ProductType).Include(p => p.ProductBrand).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }
        public async Task EditQuan(int prodId,int quan)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(x => x.Id == prodId);
            prod.AvailableQuantity = quan;
            _context.Products.Update(prod);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<Product>> GetProductsListAsync(ProductSpecParams proParams)
        {

            return await _context.Products.Where(x =>
        (string.IsNullOrEmpty(proParams.Search) || x.Name.ToLower().Contains(proParams.Search) || x.Description.ToLower().Contains(proParams.Search)) &&
        (!proParams.BrandId.HasValue || x.ProductBrandId == proParams.BrandId) &&
        (!proParams.TypeId.HasValue || x.ProductTypeId == proParams.TypeId)).Include(p => p.ProductType).Include(p => p.ProductBrand).OrderBy(x => x.Name).
        Skip(proParams.PageSize * (proParams.PageIndex - 1)).Take(proParams.PageSize).ToListAsync();
        }
        public async Task<int> GetProductsCountAsync(ProductSpecParams proParams)
        {
            return await _context.Products.Where(x =>
     (string.IsNullOrEmpty(proParams.Search) || x.Name.ToLower().Contains(proParams.Search)) &&
        (!proParams.BrandId.HasValue || x.ProductBrandId == proParams.BrandId) && (!proParams.TypeId.HasValue || x.ProductTypeId == proParams.TypeId)).CountAsync();
        }
        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
        public async Task<bool> AddNewProduct(Product newProd)
        {
            await _context.Products.AddAsync(newProd);
            return await _context.SaveChangesAsync() > 1;
        }
        public async Task<Product> EditProduct(int id, Product newProd)
        {
            var prod = await _context.Products.FindAsync(id);
            prod.Description = newProd.Description;
            prod.AvailableQuantity = newProd.AvailableQuantity;
            prod.Discount = newProd.Discount;
            prod.PictureUrl = newProd.PictureUrl;
            prod.Price = newProd.Price;
            prod.ProductBrandId = newProd.ProductBrandId;
            prod.ProductTypeId = newProd.ProductTypeId;
            prod.Specification = newProd.Specification;
            _context.Update(prod);
            await _context.SaveChangesAsync();
            return prod;
        }
        public bool IsValidImageFile(string fileName)
        {
            string[] allowedExtensions = { ".jpeg", ".jpg", ".png" };
            string fileExtension = Path.GetExtension(fileName);

            return allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
        }

        public async Task Delete(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            _context.Remove(prod);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductType> AddType(ProductType pt)
        {
            await _context.ProductTypes.AddAsync(pt);
            var type = await _context.ProductTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == pt.Name.ToLower());
            if (type != null) return null;
            await _context.SaveChangesAsync();
            return pt;
        }
        public async Task<ProductBrand> AddBrand(ProductBrand pb)
        {
            await _context.ProductBrands.AddAsync(pb);
            var type = await _context.ProductBrands.FirstOrDefaultAsync(x => x.Name.ToLower() == pb.Name.ToLower());
            if (type != null) return null;
            await _context.SaveChangesAsync();
            return pb;

        }

    }
}
