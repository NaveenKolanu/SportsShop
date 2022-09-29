using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsShop.API.Models;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]

        public IActionResult Get(int? productId)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                List<ProductViewModel> vmProducts = new List<ProductViewModel>();
                if (productId > 0)
                {
                    var dbProduct = dbContext.TblProducts.Find(productId);
                    if (dbProduct == null)
                    {
                        apiRes.IsValid = false;
                        apiRes.ErrorMessage = "invalid Product Id";
                        return Ok(apiRes);
                    }
                    ProductViewModel productView = new ProductViewModel();
                    productView.ProductId = dbProduct.ProductId;
                    productView.ProductName = dbProduct.ProductName;
                    productView.ProductPrice = dbProduct.ProductPrice;
                    productView.ProductColor = dbProduct.ProductColor;
                    productView.ProductSize = dbProduct.ProductSize;

                    apiRes.IsValid = true;
                    apiRes.Result = productView;
                    return Ok(apiRes);
                }
                var dbProducts = dbContext.TblProducts.ToList();
                foreach (var dbProduct in dbProducts)
                {
                    ProductViewModel productView = new ProductViewModel();
                    productView.ProductId = dbProduct.ProductId;
                    productView.ProductName = dbProduct.ProductName;
                    productView.ProductPrice = dbProduct.ProductPrice;
                    productView.ProductColor = dbProduct.ProductColor;
                    productView.ProductSize = dbProduct.ProductSize;
                    vmProducts.Add(productView);
                }
                apiRes.IsValid = true;
                apiRes.Result = vmProducts;
            }
            catch (Exception ex)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = ex.Message;
            }
            return Ok(apiRes);

        }

        [HttpPost]

        public IActionResult Post(ProductViewModel vmProduct)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                var price = vmProduct.ProductPrice.ToString();
                if (string.IsNullOrEmpty(vmProduct.ProductName) &&
                   (string.IsNullOrEmpty(price)) &&
                   string.IsNullOrEmpty(vmProduct.ProductColor) &&
                   string.IsNullOrEmpty(vmProduct.ProductSize))
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "product name, price, color," +
                        " size should not be empty";
                    return Ok(apiRes);
                }

                TblProduct tblProduct = new TblProduct();
                tblProduct.ProductName = vmProduct.ProductName;
                tblProduct.ProductPrice = vmProduct.ProductPrice;
                tblProduct.ProductColor = vmProduct.ProductColor;
                tblProduct.ProductSize = vmProduct.ProductSize;
                dbContext.Add(tblProduct);
                var dbState = dbContext.SaveChanges();
                if (dbState == 0)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "enter valid details";
                    return Ok(apiRes);
                }
                apiRes.IsValid = true;
                apiRes.Result = dbState;
            }
            catch (Exception ex)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = ex.Message;
            }
            return Ok(apiRes);
        }


        [HttpDelete]

        public IActionResult Delete(int productId)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                ProductViewModel productView = new ProductViewModel();
                if (productId > 0)
                {
                    var dbProduct = dbContext.TblProducts.Find(productId);
                    if (dbProduct == null)
                    {
                        apiRes.IsValid = false;
                        apiRes.ErrorMessage = $"{productId}:Product Id doesn't exist";
                        return Ok(apiRes);
                    }
                    productView.ProductId = dbProduct.ProductId;
                    productView.ProductName = dbProduct.ProductName;
                    productView.ProductPrice = dbProduct.ProductPrice;
                    productView.ProductColor = dbProduct.ProductColor;
                    productView.ProductSize = dbProduct.ProductSize;
                    dbContext.Remove(productView);
                    apiRes.Result = dbContext.SaveChanges();
                    apiRes.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = ex.Message;
            }
            return Ok(apiRes);

        }

        [HttpPut]

        public IActionResult Put(ProductViewModel vmProduct)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                var dbProduct = dbContext.TblProducts.Find(vmProduct.ProductId);
                if (dbProduct == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "Product doesn't exist";
                    return Ok(apiRes);
                }
                var price = vmProduct.ProductPrice.ToString();
                if (string.IsNullOrEmpty(vmProduct.ProductName) &&
                    (string.IsNullOrEmpty(price)) &&
                    string.IsNullOrEmpty(vmProduct.ProductColor) &&
                    string.IsNullOrEmpty(vmProduct.ProductSize))
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "product name, price, color," +
                        " size should not be empty";
                    return Ok(apiRes);
                }

                dbProduct.ProductName = vmProduct.ProductName;
                dbProduct.ProductPrice = vmProduct.ProductPrice;
                dbProduct.ProductColor = vmProduct.ProductColor;
                dbProduct.ProductSize = vmProduct.ProductSize;

                dbContext.Update(dbProduct);
                apiRes.IsValid = true;
                apiRes.Result = dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = ex.Message;
            }
            return Ok(apiRes);

        }


    }


}
