using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cartandorderService.Entities
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
       
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string OrderOf { get; set; }

    }
}
