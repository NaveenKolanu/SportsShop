using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsShop.API.Models;
using Microsoft.Extensions.Logging;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> logger;

        public ProductController(ILogger<ProductController> _logger)
        {
            logger = _logger;
        }

        [HttpGet]
        public IActionResult Get(int? productId)
        {
            logger.LogInformation("ProductController get method initiated at {DT}",DateTime.UtcNow.ToString());
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            if (productId > 0)
            {
                var record = dbContext.TblProducts.Find(productId);
                if (record == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "invalid Product Id";
                    logger.LogError("ProductId {productId}, not found", productId.ToString());
                    return Ok(apiRes);
                }
                apiRes.IsValid = true;
                apiRes.Result = record;
                logger.LogInformation("record found and succesfully sent the results");
                return Ok(apiRes);
            }
            return Ok(dbContext.TblProducts.ToList());
        }

        [HttpPost]
        public IActionResult Post(TblProduct tblProduct)
        {
            logger.LogInformation("ProductsController post method initiated at {DT}",DateTime.UtcNow.ToString());
            ShopDBContext dbContext = new ShopDBContext();
            dbContext.Add(tblProduct);
            var dbState = dbContext.SaveChanges();
            logger.LogInformation("Product posted successfully with post method at {DT}",DateTime.UtcNow.ToString());
            return Ok(dbState);
        }

        [HttpPut]
        public IActionResult Put(TblProduct tblProduct)
        {
            logger.LogInformation("productsController put method initiated at {DT}",DateTime.UtcNow.ToString());
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            var dbProduct = dbContext.TblProducts.Find(tblProduct.ProductId);
            if (dbProduct==null)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = "Product not found";
                logger.LogError("Product put method execution failed at {DT}",DateTime.UtcNow.ToString());
                return Ok(apiRes);
            }
            dbProduct.ProductName = tblProduct.ProductName;
            dbProduct.ProductPrice = tblProduct.ProductPrice;
            dbProduct.ProductSize = tblProduct.ProductSize;
            dbProduct.ProductColor = tblProduct.ProductColor;

            dbContext.Update(dbProduct);
            apiRes.IsValid = true;
            apiRes.Result = dbContext.SaveChanges();
            logger.LogInformation("productcontroller put method " +
                "succesfully executed at {DT}",DateTime.UtcNow.ToString());
            return Ok(apiRes);
            
        }

        [HttpDelete]
        public IActionResult Delete(int productId)
        {
            logger.LogInformation("PostController delete method executed at {DT}", DateTime.UtcNow.ToString());
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            if (productId > 0)
            {
                var dbProduct = dbContext.TblProducts.Find(productId);
                if (dbProduct == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = $"{productId}:Product Id doesn't exist";
                    logger.LogError("ProductId {productId} was not found, " +
                        "delete not completed at {DT}", productId.ToString(), DateTime.UtcNow.ToString());
                    return Ok(apiRes);
                }
                dbContext.Remove(dbProduct);
                apiRes.Result = dbContext.SaveChanges();
                apiRes.IsValid = true;
                logger.LogInformation("PostController delete method " +
                    "executed succesfully with productid {productId} at {DT}", 
                    productId.ToString(),DateTime.UtcNow.ToString());
                return Ok(apiRes);
            }
            return Ok();
        }

    }


}
