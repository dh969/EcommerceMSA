using System.ComponentModel.DataAnnotations;

namespace ProductsService.Entity
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int? ProductBrandId { get; set; }
        [Required]
        public int AvailableQuantity { get; set; }
        public decimal Discount { get; set; }
        public string Specification { get; set; }

    }
}
