using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsShop.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public const string SuccessStatus = "Success";
        public const string FailedStatus = "Failed";

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            if (orderId > 0)
            {
                //Get Order info by OrderId
                var dbOrder = dbContext.TblOrders.Find(orderId);

                //Get Customer info by CustomerId
                var dbCustomer = dbContext.TblCustomers.Find(dbOrder.CustomerId);

                //Get Ordered Items associated with OrderId
                var dbOrderedItems = dbContext.TblOrderedItems.Where(p => p.OrderId == orderId).ToList();
               
                //Get Complete Order Info
                OrderDetails orderDeatails = new OrderDetails();
                List<ProductDetails> itemsDetails = new List<ProductDetails>();
                
                orderDeatails.OrderId = dbOrder.OrderId;
                orderDeatails.CustomerId = dbCustomer.CustomerId;
                orderDeatails.CustomerName = dbCustomer.CustomerName;

                foreach (var item in dbOrderedItems)
                {
                    //Get Product detail by ProductId
                    var dbProduct = dbContext.TblProducts.Find(item.ProductId);
                    
                    ProductDetails product = new ProductDetails();
                    product.ProductId = dbProduct.ProductId;
                    product.ProductName = dbProduct.ProductName;
                    product.ProductPrice = dbProduct.ProductPrice;

                    product.ProductColor = dbProduct.ProductColor;
                    product.ProductSize = dbProduct.ProductSize;
                    
                    //Add Product to OrderedProducts
                    itemsDetails.Add(product);
                }
                
                //Assign products
                orderDeatails.OrderedProducts = itemsDetails;
                
                apiRes.Result = orderDeatails;
                apiRes.IsValid = true;
                return Ok(apiRes);
            }
            apiRes.ErrorMessage = $"{orderId}:Record Not Found";
            apiRes.IsValid = false;
            return Ok(apiRes);
        }

        [HttpPost]
        public IActionResult Post(SelectedItems items)
        {
            ShopDBContext dbContext = new ShopDBContext();
            TblOrder tblOrder = new TblOrder();
            var customer = dbContext.TblCustomers.Find(items.CustomerId);
            tblOrder.CustomerId = customer.CustomerId;
            tblOrder.OrderAddress = items.OrderedAddress;
            dbContext.Add(tblOrder);
            dbContext.SaveChanges();
            //Save Items
            foreach (var prodId in items.SelectedProducts)
            {
                var product = dbContext.TblProducts.Find(prodId);
                TblOrderedItem tblOrderedItem = new TblOrderedItem();
                tblOrderedItem.OrderId = tblOrder.OrderId;
                tblOrderedItem.ProductId = product.ProductId;

                dbContext.Add(tblOrderedItem);
            }
            //Save all items
            var dbState = dbContext.SaveChanges();
            return Ok(dbState);
        }

        [HttpDelete]
        public IActionResult Delete(int orderId)
        {
            ApiResponse apiResp = new ApiResponse();
            
            ShopDBContext dbContext = new ShopDBContext();

            //Get Child Record and Delete them first
            var dbOrderedItems = dbContext.TblOrderedItems
                .Where(p => p.OrderId == orderId).ToList();
            if (dbOrderedItems != null && dbOrderedItems.Count > 0)
            {
                dbContext.RemoveRange(dbOrderedItems);
                var dbStatus = dbContext.SaveChanges();
                if (dbStatus > 0)
                {
                    //Get Master Record and Delete it now
                    var dbOrder = dbContext.TblOrders.Find(orderId);
                    dbContext.Remove(dbOrder);
                    var dbState = dbContext.SaveChanges();
                    apiResp.IsValid = true;
                    apiResp.StatusMessage = "Success";

                    return Ok(apiResp);
                }
            }
            apiResp.IsValid = false;
            apiResp.StatusMessage = "Failed";
            return Ok(apiResp);

        }

        [HttpPut]
        public IActionResult Put(TblOrder tblOrder)
        {
            ShopDBContext dbContext = new ShopDBContext();
            var dbOrder = dbContext.TblOrders.Find(tblOrder.OrderId);
            dbOrder.CustomerId = tblOrder.CustomerId;
            dbOrder.OrderAddress = tblOrder.OrderAddress;
            dbContext.Update(dbOrder);
            var dbState = dbContext.SaveChanges();
            return Ok(dbState);

        }

    }


}