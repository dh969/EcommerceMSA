using Business_Layer;
using cartandorderService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cartandorderService
{
    public class CartToReturnDto
    {
        public Cart cart=new();
        public ProductToReturn prod=new();
    }
}
