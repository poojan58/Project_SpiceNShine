using System.ComponentModel.DataAnnotations;

namespace SpiceAndShine.Areas.Manager.Models
{
    public class ProductListModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsAvailable { get; set; }
    }
    public class AddEditProductViewModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [StringLength(200, ErrorMessage = "Name length can't be more than 200.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter price of product")]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Display(Name = "Is Best Seller")]
        public bool IsBestSeller { get; set; }

        [Required(ErrorMessage = "Please add image of product")]
        [Display(Name = "Image")]
        public List<byte[]> ImageName { get; set; }

        [Required(ErrorMessage = "Please enetr display order.")]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        public bool IsAvailable { get; set; }
        public string ProductImageName { get; set; }
        public string imgX1 { get; set; } = "1";
        public string imgY1 { get; set; } = "1";
        public string imgWidth { get; set; } = "1";
        public string imgHeight { get; set; } = "1";
    }
    public class GetProductDetailsById
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public bool IsBestSeller { get; set; } 
        public string ProductImageName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsAvailable { get; set; } 

    }
    public class AddEditProductResponse
    {
        public int status { get; set; }
        public int ProductID { get; set; }
    }
    public class DeleteProductResponse
    {
        public string ImageName { get; set; }
        public bool AllowToDelete { get; set; }
    }
}
