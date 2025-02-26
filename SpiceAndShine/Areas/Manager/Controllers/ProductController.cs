using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpiceAndShine.Areas.Manager.Models;
using SpiceAndShine.Models;

namespace SpiceAndShine.Areas.Manager.Controllers
{
    [Area("manager")]
    public class ProductController : BaseController
    {
        private readonly string _imageFolderPath;
        private readonly string _RestImageFolderPath;

        public ProductController()
        {
            _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Product");
            Directory.CreateDirectory(_imageFolderPath);
        }

        DatabaseManager objDatabaseManager = new DatabaseManager();
        public void GetBestSeller()
        {
            List<SelectListItem> IsBestSeller = new List<SelectListItem>();
            IsBestSeller.Add(new SelectListItem { Value = "-1", Text = "-- All --" });
            IsBestSeller.Add(new SelectListItem { Value = "1", Text = "BestSeller" });
            IsBestSeller.Add(new SelectListItem { Value = "0", Text = "Regular" });
            ViewBag.IsBestSeller = new SelectList(IsBestSeller, "Value", "Text");
        }
        
        public void GetActiveInActive()
        {
            List<SelectListItem> IsAvailable = new List<SelectListItem>();
            IsAvailable.Add(new SelectListItem { Value = "-1", Text = "-- All --" });
            IsAvailable.Add(new SelectListItem { Value = "1", Text = "Available" });
            IsAvailable.Add(new SelectListItem { Value = "0", Text = "Unavilable" });
            ViewBag.IsAvailable = new SelectList(IsAvailable, "Value", "Text");
        }
        #region Product

        [Route("manager/product")]
        public ActionResult ProductList()
        {
            GetBestSeller();
            GetActiveInActive();
            return View("_ProductList");
        }
        [HttpGet]
        public ActionResult GetProductList(JQueryDataTableParamModel param, string Name, string IsAvailable, string IsBestSeller)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<ProductListModel> list = objDatabaseManager.GetProductList(param, Name, IsAvailable, IsBestSeller, out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.ProductID),
                        c.ProductName,
                        Convert.ToString(c.Price),
                        c.IsBestSeller==true ?"Yes":"No",
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
        [Route("manager/add-edit-product")]
        [HttpGet]
        public ActionResult AddEditProduct(int ProductId)
        {
            AddEditProductViewModel addEditProductModel = new AddEditProductViewModel();

            GetProductDetailsById getProductDetailsById = objDatabaseManager.GetProductDetailsById(ProductId);
            if (getProductDetailsById != null)
            {
                addEditProductModel.ProductID = ProductId;
                addEditProductModel.Name = getProductDetailsById.ProductName;
                addEditProductModel.Price = getProductDetailsById.Price;
                addEditProductModel.IsBestSeller = getProductDetailsById.IsBestSeller;
                addEditProductModel.ProductImageName = getProductDetailsById.ProductImageName;
                addEditProductModel.IsAvailable = getProductDetailsById.IsAvailable;
                addEditProductModel.DisplayOrder = getProductDetailsById.DisplayOrder; 
            }
            else
            {
                ViewBag.IsReadOnlyClass = "";
            }
            ViewBag.ImageSRC = _imageFolderPath.Replace("\\", "/");
            return View("_AddEditProduct", addEditProductModel);
        }

        [HttpPost]
        [Route("manager/add-edit-product")]
        public IActionResult AddEditProduct(AddEditProductViewModel addEditProuctViewModel, IFormFile ImageName)
        {
            try
            {
                if (addEditProuctViewModel.ProductID > 0)
                {
                    String ProductImageName1 = "";
                    var Extension = "";
                    var NewFileName = "";
                    if (ImageName != null)
                    {
                        ProductImageName1 = GetTimestamp(DateTime.Now);
                        Extension = Path.GetExtension(ImageName.FileName);
                        NewFileName = ProductImageName1 + Extension;
                    }

                    AddEditProductResponse addEditProductResponse = new AddEditProductResponse();
                    addEditProductResponse = objDatabaseManager.AddEditProduct(addEditProuctViewModel, NewFileName);
                    if (addEditProductResponse.status == 201)
                    {
                        if (ImageName != null)
                        {
                            // var fileName = ProductImageName1 + "_" + addEditProductResponse.ProductID + Path.GetExtension(ImageName.FileName);
                            var fileName = ProductImageName1 + Path.GetExtension(ImageName.FileName);
                            var filePath = Path.Combine(_imageFolderPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                ImageName.CopyTo(stream);
                            }
                        }
                        return RedirectToAction("ProductList", new { Status = addEditProductResponse.status });
                    }
                    else
                    {
                        return View("_AddEditProduct", addEditProuctViewModel);
                    }
                }
                else
                {
                    if ((ImageName == null || ImageName.Length == 0) && addEditProuctViewModel.ProductID <= 0)
                    {
                        return BadRequest("No file uploaded.");
                    }
                    String ProductImageName1 = GetTimestamp(DateTime.Now);
                    var Extension = Path.GetExtension(ImageName.FileName);
                    var NewFileName = ProductImageName1 + Extension;
                    AddEditProductResponse addEditProductResponse = new AddEditProductResponse();
                    addEditProductResponse = objDatabaseManager.AddEditProduct(addEditProuctViewModel, NewFileName);
                    if (addEditProductResponse.status == 200)
                    {
                        // var fileName = ProductImageName1 + "_" + addEditProductResponse.ProductID + Path.GetExtension(ImageName.FileName);
                        var fileName = ProductImageName1 + Path.GetExtension(ImageName.FileName);
                        var filePath = Path.Combine(_imageFolderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageName.CopyTo(stream);
                        }
                        return RedirectToAction("ProductList", new { Status = addEditProductResponse.status });
                    }
                    else
                    {
                        return View("_AddEditProduct", addEditProuctViewModel);
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteProduct(int ProductId)
        {
            try
            {
                DeleteProductResponse DeleteProductResponse = new DeleteProductResponse();
                DeleteProductResponse = objDatabaseManager.DeleteProduct(ProductId);
                return Json(new { result = DeleteProductResponse });
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteProductImage(string ImageName)
        {
            try
            {
                string Path = _imageFolderPath + "\\" + ImageName;
                FileInfo file = new FileInfo(Path);
                if (file.Exists)
                {
                    file.Delete();
                }

                bool IsDeleted = true;
                if (IsDeleted)
                {
                    return Json(new { Deleted = 1 });
                }
                else
                {
                    return Json(new { Deleted = 0 });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Deleted = 0 });
            }
        }
        #endregion

        
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }

}
