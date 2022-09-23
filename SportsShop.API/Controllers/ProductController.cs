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
            ShopDBContext dbContext = new ShopDBContext();
            if (productId > 0)
            {
                var record = dbContext.TblProducts.Find(productId);
                return Ok(record);
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
            ShopDBContext dbContext = new ShopDBContext();
            var dbProduct = dbContext.TblProducts.Find(tblProduct.ProductId);
            dbProduct.ProductName = tblProduct.ProductName;
            dbProduct.ProductPrice = tblProduct.ProductPrice;
            dbProduct.ProductSize = tblProduct.ProductSize;
            dbProduct.ProductColor = tblProduct.ProductColor;

            dbContext.Update(dbProduct);
            var dbState = dbContext.SaveChanges();

            return Ok(dbState);
        }

        [HttpDelete]
        public IActionResult Delete(int productId)
        {
            ShopDBContext dbContext = new ShopDBContext();
            var dbProduct = dbContext.TblProducts.Find(productId);

            dbContext.Remove(dbProduct);
            var dbState = dbContext.SaveChanges();

            return Ok(dbState);
        }


        public List<Product> GetDummyProducts()
        {
            Product product1 = new Product();
            product1.ProductId = 101;
            product1.ProductName = "Bat";
            product1.ProductDescription = "Cricket Bat";
            product1.ProductPrice = 2000;

            Product product2 = new Product();
            product2.ProductId = 102;
            product2.ProductName = "Gurd";
            product2.ProductDescription = "Cricket Gurd";
            product2.ProductPrice = 599;

            //List of Products
            List<Product> lstProducts = new List<Product>();

            lstProducts.Add(product1);
            lstProducts.Add(product2);

            return lstProducts;
        }

    }


}
