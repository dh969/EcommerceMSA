
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProductsService.Dtos;
using ProductsService.Entity;

namespace ProductsService.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        public ProductUrlResolver(IConfiguration config)
        {
            Config = config;
        }

        public IConfiguration Config { get; }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return Config["ApiUrl"] + source.PictureUrl;

            }
            return null;
        }
    }
}
