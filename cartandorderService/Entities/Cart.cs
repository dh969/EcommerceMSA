using Business_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cartandorderService.Entities
{
    public class Cart
    {
      
        public int Id { get; set; }
        public string CreatedBy { get; set; }

        public int Quantity { get; set; }
        //public decimal Discount { get; set; }
        public int ProductId { get; set; }
      
     
    }
}
