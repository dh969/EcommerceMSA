using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Dtos;
using ProductsService.Entity;
using ProductsService.Helpers;
using ProductsService.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository product;

        public ProductController(StoreContext context,
            IMapper mapper, IProductRepository product)
        {
            

            _mapper = mapper;
            this.product = product;
        }
        [HttpGet]
       
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {

            var totalItems = await product.GetProductsCountAsync(productParams);
            var products = await product.GetProductsListAsync(productParams);
            var data = _mapper.Map<List<Product>, List<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {

            var prod = await product.GetProductByIdAsync(id);
            return _mapper.Map<Product, ProductToReturnDto>(prod);
       
        }
        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetBrands()
        {
            return Ok(await product.GetProductsBrandsAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetTypes()
        {
            return Ok(await product.GetProductTypesAsync());
        }
        [HttpPost("AddNewProduct")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ProductToReturnDto>> AddNewProduct(Product proDto)
        {
            var succ = await product.AddNewProduct(proDto);
            var prod = _mapper.Map<Product, ProductToReturnDto>(proDto);
            return prod;
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ProductToReturnDto>> EditProduct(int id, Product newPro)
        {
            var prod = await product.EditProduct(id, newPro);
            var newProd = _mapper.Map<Product, ProductToReturnDto>(prod);
            return Ok();

        }
        [HttpPut("editquan/{id}")]
        public async Task<ActionResult> Edit(int id,ProductToReturnDto prod)
        {
            await product.EditQuan(id, prod.AvailableQuantity);
            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task DeleteProduct(int id)
        {
            await product.Delete(id);
        }
        [HttpPost("upload")]
        public IActionResult UploadImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Perform validation on the uploaded file
                if (product.IsValidImageFile(file.FileName))
                {

                    // Save the file to a desired location
                    var filePath = "wwwroot/images/products/" + file.FileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }


                    return Ok(new { imageUrl = "images/products/" + file.FileName });
                }
                else
                {
                    return BadRequest("this extension is not allowed");
                }

            }


            return BadRequest("No image file provided");
        }
        [HttpPost("addtype")]

        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ProductType>> proType(ProductType pt)
        {
            var type = await product.AddType(pt);
            if (type == null) return BadRequest(StatusCode(409));
            return type;
        }
        [HttpPost("addbrand")]

       [Authorize(Roles ="admin")]
        public async Task<ActionResult<ProductBrand>> proBrand(ProductBrand pb)
        {
            var type = await product.AddBrand(pb);
            if (type == null) return BadRequest(StatusCode(409));
            return type;
        }
    }
}
