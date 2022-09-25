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
            ShopDBContext dbContext = new ShopDBContext();
            if (productId > 0)
            {
                var record = dbContext.TblProducts.Find(productId);
                if (record == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "invalid Product Id";
                    return Ok(apiRes);
                }
                apiRes.IsValid = true;
                apiRes.Result = record;
                return Ok(apiRes);
            }
            return Ok(dbContext.TblProducts.ToList());
        }

        [HttpPost]
        public IActionResult Post(TblProduct tblProduct)
        {

            ShopDBContext dbContext = new ShopDBContext();
            dbContext.Add(tblProduct);
            var dbState = dbContext.SaveChanges();

            return Ok(dbState);
        }

        [HttpPut]
        public IActionResult Put(TblProduct tblProduct)
        {
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            var dbProduct = dbContext.TblProducts.Find(tblProduct.ProductId);
            if (dbProduct==null)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = "Product not found";
                return Ok(apiRes);
            }
            dbProduct.ProductName = tblProduct.ProductName;
            dbProduct.ProductPrice = tblProduct.ProductPrice;
            dbProduct.ProductSize = tblProduct.ProductSize;
            dbProduct.ProductColor = tblProduct.ProductColor;

            dbContext.Update(dbProduct);
            apiRes.IsValid = true;
            apiRes.Result = dbContext.SaveChanges();
            return Ok(apiRes);
            
        }

        [HttpDelete]
        public IActionResult Delete(int productId)
        {
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            if (productId > 0)
            {
                var dbProduct = dbContext.TblProducts.Find(productId);
                if (dbProduct == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = $"{productId}:Product Id doesn't exist";
                    return Ok(apiRes);
                }
                dbContext.Remove(dbProduct);
                apiRes.Result = dbContext.SaveChanges();
                apiRes.IsValid = true;
               
                return Ok(apiRes);
            }
            return Ok();
        }

    }


}
