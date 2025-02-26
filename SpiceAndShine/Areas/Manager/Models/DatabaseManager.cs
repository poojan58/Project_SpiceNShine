using Microsoft.Data.SqlClient;
using SpiceAndShine.Models;
using System.Data;

namespace SpiceAndShine.Areas.Manager.Models
{
    public class DatabaseManager
    {
        public ManagerLoginResult ManagerLogin(ManagerLoginModel adminLoginModel)
        {
            try
            {
                ManagerLoginResult adminLoginResult = new ManagerLoginResult();
                LoginStatus objLoginStatus = new LoginStatus();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_ManagerLogin, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmailOrMobileNo", adminLoginModel.EmailOrMobileNo);
                        cmd.Parameters.AddWithValue("@Password", adminLoginModel.Password.Trim());
                        con.Open();
                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            objLoginStatus = UserDefineExtensions.DataReaderMapToEntity<LoginStatus>(dataReader);
                            adminLoginResult.Flag = objLoginStatus.Result;
                            if (adminLoginResult.Flag == Convert.ToInt32(ManagerLoginEnum.Active))
                            {
                                dataReader.NextResult();
                                adminLoginResult.ManagerData = UserDefineExtensions.DataReaderMapToEntity<ManagerSession>(dataReader);
                            }
                        }
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return adminLoginResult;
            }
            catch (Exception ex) { throw; }
        }
        #region Product Category 
        public List<ProductCategoryListModel> GetProductCategoryList(JQueryDataTableParamModel param, string Name, string IsAvailable, out int noOfRecords)
        {
            List<ProductCategoryListModel> productCategoryListModel = new List<ProductCategoryListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_GetProductCategoryGrid, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100)).Value = Name;
                    cmd.Parameters.Add(new SqlParameter("@IsAvailable", SqlDbType.Int)).Value = Convert.ToInt32(IsAvailable);
                    cmd.Parameters.Add(new SqlParameter("@DisplayStart", SqlDbType.Int)).Value = param.iDisplayStart;
                    cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int)).Value = param.iDisplayLength;
                    cmd.Parameters.Add(new SqlParameter("@SortColumnName", SqlDbType.VarChar, 50)).Value = param.iSortCol_0;
                    cmd.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.VarChar, 50)).Value = param.sSortDir_0;
                    con.Open();
                    SqlParameter resultOutPut = new SqlParameter("@noOfRecords", SqlDbType.VarChar);
                    resultOutPut.Size = 50;
                    resultOutPut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultOutPut);
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        productCategoryListModel = UserDefineExtensions.DataReaderMapToList<ProductCategoryListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return productCategoryListModel;
        }
        public GetProductCategoryDetailsById GetProductCategoryDetailsById(int ProductCategoryId)
        {
            GetProductCategoryDetailsById getProductCategoryDetailsById = new GetProductCategoryDetailsById();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_GetProductCategoryDetailsById, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryId);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            getProductCategoryDetailsById = UserDefineExtensions.DataReaderMapToEntity<GetProductCategoryDetailsById>(datareader);
                        }
                        con.Close();
                    }
                }
                return getProductCategoryDetailsById;
            }
            catch
            {
                throw;
            }
        }

        public AddEditProductCategoryResponse AddEditProductCategory(AddEditProductCategoryViewModel addEditProductCategoryViewModel)
        {
            try
            {
                AddEditProductCategoryResponse addEditProductCategoryResponse = new AddEditProductCategoryResponse();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_AddEditProductCategory, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductCategoryID", addEditProductCategoryViewModel.ProductCategoryID);
                        cmd.Parameters.AddWithValue("@ProductCategoryName", addEditProductCategoryViewModel.ProductCategoryName);
                        cmd.Parameters.AddWithValue("@DisplayOrder", addEditProductCategoryViewModel.DisplayOrder);
                        cmd.Parameters.AddWithValue("@IsAvailable", addEditProductCategoryViewModel.IsAvailable);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            addEditProductCategoryResponse = UserDefineExtensions.DataReaderMapToEntity<AddEditProductCategoryResponse>(datareader);
                        }
                        con.Close();
                    }
                    return addEditProductCategoryResponse;
                }
            }
            catch
            {
                throw;
            }
        }

        public DeleteProductCategoryResponse DeleteProductCategory(int ProductCategoryId)
        {
            DeleteProductCategoryResponse deleteProductCategoryResponse = new DeleteProductCategoryResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_DeleteProductCategory, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryId);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            deleteProductCategoryResponse = UserDefineExtensions.DataReaderMapToEntity<DeleteProductCategoryResponse>(datareader);
                        }
                        con.Close();
                    }
                }
                return deleteProductCategoryResponse;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Product 
        public List<ProductListModel> GetProductList(JQueryDataTableParamModel param, string Name, string IsAvailable, string IsBestSeller, out int noOfRecords)
        {
            List<ProductListModel> productListModel = new List<ProductListModel>();
            using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_GetProductGrid, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100)).Value = Name;
                    cmd.Parameters.Add(new SqlParameter("@IsAvailable", SqlDbType.Int)).Value = Convert.ToInt32(IsAvailable);
                    cmd.Parameters.Add(new SqlParameter("@IsBestSeller", SqlDbType.Int)).Value = Convert.ToInt32(IsBestSeller);
                    cmd.Parameters.Add(new SqlParameter("@DisplayStart", SqlDbType.Int)).Value = param.iDisplayStart;
                    cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int)).Value = param.iDisplayLength;
                    cmd.Parameters.Add(new SqlParameter("@SortColumnName", SqlDbType.VarChar, 50)).Value = param.iSortCol_0;
                    cmd.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.VarChar, 50)).Value = param.sSortDir_0;
                    con.Open();
                    SqlParameter resultOutPut = new SqlParameter("@noOfRecords", SqlDbType.VarChar);
                    resultOutPut.Size = 50;
                    resultOutPut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultOutPut);
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        productListModel = UserDefineExtensions.DataReaderMapToList<ProductListModel>(dataReader);
                    }
                    noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                    con.Close();
                }
            }
            return productListModel;
        }
        public GetProductDetailsById GetProductDetailsById(int ProductId)
        {
            GetProductDetailsById getProductDetailsById = new GetProductDetailsById();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_GetProductDetailsById, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", ProductId);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            getProductDetailsById = UserDefineExtensions.DataReaderMapToEntity<GetProductDetailsById>(datareader);
                        }
                        con.Close();
                    }
                }
                return getProductDetailsById;
            }
            catch
            {
                throw;
            }
        }

        public AddEditProductResponse AddEditProduct(AddEditProductViewModel addEditProductViewModel, string ProductImageName)
        {
            try
            {
                AddEditProductResponse addEditProductResponse = new AddEditProductResponse();
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_AddEditProduct, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", addEditProductViewModel.ProductID);
                        cmd.Parameters.AddWithValue("@ProductName", addEditProductViewModel.Name);
                        cmd.Parameters.AddWithValue("@Price", addEditProductViewModel.Price);
                        cmd.Parameters.AddWithValue("@IsBestSeller", addEditProductViewModel.IsBestSeller);
                        cmd.Parameters.AddWithValue("@ImageName", ProductImageName);
                        cmd.Parameters.AddWithValue("@DisplayOrder", addEditProductViewModel.DisplayOrder);
                        cmd.Parameters.AddWithValue("@IsAvailable", addEditProductViewModel.IsAvailable);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            addEditProductResponse = UserDefineExtensions.DataReaderMapToEntity<AddEditProductResponse>(datareader);
                        }
                        con.Close();
                    }
                    return addEditProductResponse;
                }
            }
            catch
            {
                throw;
            }
        }

        public DeleteProductResponse DeleteProduct(int ProductId)
        {
            DeleteProductResponse deleteProductResponse = new DeleteProductResponse();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.DBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Common.StoredProcedureNames.manager_DeleteProduct, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", ProductId);
                        con.Open();
                        using (IDataReader datareader = cmd.ExecuteReader())
                        {
                            deleteProductResponse = UserDefineExtensions.DataReaderMapToEntity<DeleteProductResponse>(datareader);
                        }
                        con.Close();
                    }
                }
                return deleteProductResponse;
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
