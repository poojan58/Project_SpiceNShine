using System.ComponentModel.DataAnnotations;

namespace SpiceAndShine.Areas.Manager.Models
{
    public class ProductCategoryListModel
    {
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public bool IsAvailable { get; set; }
    }
    public class GetProductCategoryDetailsById
    {
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }       
        public int DisplayOrder { get; set; }
        public bool IsAvailable { get; set; }
    }
    public class AddEditProductCategoryViewModel
    {
        public int ProductCategoryID { get; set; }.l

        [Required(ErrorMessage = "Please enter name")]
        [StringLength(200, ErrorMessage = "Name length can't be more than 200.")]
        [Display(Name = "Name")]
        public string ProductCategoryName { get; set; }

        [Required(ErrorMessage = "Please enetr display order.")]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        public bool IsAvailable { get; set; }
       
    }
    public class AddEditProductCategoryResponse
    {
        public int status { get; set; }
        public int ProductCategoryID { get; set; }
    }
    public class DeleteProductCategoryResponse
    {
        public bool AllowToDelete { get; set; }
    }
}
