using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpiceAndShine.Areas.Manager.Models;
using SpiceAndShine.Models;

namespace SpiceAndShine.Areas.Manager.Controllers
{
    [Area("manager")]
    public class ProductCategoryController : BaseController
    {
        DatabaseManager objDatabaseManager = new DatabaseManager();
        public void GetActiveInActive()
        {
            List<SelectListItem> IsAvailable = new List<SelectListItem>();
            IsAvailable.Add(new SelectListItem { Value = "-1", Text = "-- All --" });
            IsAvailable.Add(new SelectListItem { Value = "1", Text = "Available" });
            IsAvailable.Add(new SelectListItem { Value = "0", Text = "Unavilable" });
            ViewBag.IsAvailable = new SelectList(IsAvailable, "Value", "Text");
        }
        [Route("manager/product-category")]
        public ActionResult ProductCategoryList()
        {
            GetActiveInActive();
            return View("_ProductCategoryList");
        }
        [HttpGet]
        public ActionResult GetProductCategoryList(JQueryDataTableParamModel param, string Name, string DiscountInPercentage, string IsAvailable, string IsVegetarian, string IsBestSeller)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<ProductCategoryListModel> list = objDatabaseManager.GetProductCategoryList(param, Name, IsAvailable, out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.ProductCategoryID),
                        c.ProductCategoryName,
                        c.IsAvailable==true ?"Yes":"No"
                      };

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = noOfRecords,
                    iTotalDisplayRecords = noOfRecords,
                    aaData = obj
                });
            }
            catch (Exception) { throw; }
        }
        [Route("manager/add-edit-productcategory")]
        [HttpGet]
        public ActionResult AddEditProductCategory(int ProductCategoryId)
        {
            AddEditProductCategoryViewModel addEditProductCategoryModel = new AddEditProductCategoryViewModel();

            GetProductCategoryDetailsById getProductCategoryDetailsById = objDatabaseManager.GetProductCategoryDetailsById(ProductCategoryId);
            if (getProductCategoryDetailsById != null)
            {
                addEditProductCategoryModel.ProductCategoryID = ProductCategoryId;
                addEditProductCategoryModel.ProductCategoryName = getProductCategoryDetailsById.ProductCategoryName;
                addEditProductCategoryModel.IsAvailable = getProductCategoryDetailsById.IsAvailable;
                addEditProductCategoryModel.DisplayOrder = getProductCategoryDetailsById.DisplayOrder;
                
            }
            else
            {
                ViewBag.IsReadOnlyClass = "";
            }
            return View("_AddEditProductCategory", addEditProductCategoryModel);
        }

        [HttpPost]
        [Route("manager/add-edit-productcategory")]
        public IActionResult AddEditProductCategory(AddEditProductCategoryViewModel addEditProductCategoryViewModel)
        {
            try
            {
                if (addEditProductCategoryViewModel.ProductCategoryID > 0)
                {
                    AddEditProductCategoryResponse addEditProductCategoryResponse = new AddEditProductCategoryResponse();
                    addEditProductCategoryResponse = objDatabaseManager.AddEditProductCategory(addEditProductCategoryViewModel);
                    if (addEditProductCategoryResponse.status == 201)
                    {
                        return RedirectToAction("ProductCategoryList", new { Status = addEditProductCategoryResponse.status });
                    }
                    else
                    {
                        return View("_AddEditProductCategory", addEditProductCategoryViewModel);
                    }
                }
                else
                {


                    AddEditProductCategoryResponse addEditProductCategoryResponse = new AddEditProductCategoryResponse();
                    addEditProductCategoryResponse = objDatabaseManager.AddEditProductCategory(addEditProductCategoryViewModel);
                    if (addEditProductCategoryResponse.status == 200)
                    {
                         
                        return RedirectToAction("ProductCategoryList", new { Status = addEditProductCategoryResponse.status });
                    }
                    else
                    {
                        return View("_AddEditProductCategory", addEditProductCategoryViewModel);
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteProductCategory(int ProductCategoryId)
        {
            try
            {
                DeleteProductCategoryResponse deleteProductCategoryResponse = new DeleteProductCategoryResponse();
                deleteProductCategoryResponse = objDatabaseManager.DeleteProductCategory(ProductCategoryId);
                return Json(new { result = deleteProductCategoryResponse });
            }
            catch
            {
                throw;
            }
        }

    }
}
