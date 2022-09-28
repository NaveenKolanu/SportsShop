using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsShop.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SportsShop.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        //public const string SuccessStatus = "Success";
        //public const string FailedStatus = "Failed";
        private readonly ILogger<OrdersController> logger;

        public OrdersController(ILogger<OrdersController> _logger)
        {
            logger = _logger;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            logger.LogInformation("Get method executed in OrdersController with id : {orderId} at {DT}",
                orderId.ToString(), DateTime.UtcNow.ToString());
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
                OrderDetailsViewModel orderDeatails = new OrderDetailsViewModel();
                List<ProductViewModel> itemsDetails = new List<ProductViewModel>();
                
                orderDeatails.OrderId = dbOrder.OrderId;
                orderDeatails.CustomerId = dbCustomer.CustomerId;
                orderDeatails.CustomerName = dbCustomer.CustomerName;

                foreach (var item in dbOrderedItems)
                {
                    //Get Product detail by ProductId
                    var dbProduct = dbContext.TblProducts.Find(item.ProductId);
                    
                    ProductViewModel product = new ProductViewModel();
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
                logger.LogInformation("Order details executed successfully and returned with ");
                return Ok(apiRes);
            }
            apiRes.ErrorMessage = $"{orderId}:Record Not Found";
            apiRes.IsValid = false;
            logger.LogError("OrderId not found, id : {orderId}", orderId.ToString());
            return Ok(apiRes);
        }

        [HttpPost]
        public IActionResult Post(SelectedItems items)
        {
            logger.LogInformation("OrderController, post method initiated at {DT}",DateTime.UtcNow.ToString());
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            TblOrder tblOrder = new TblOrder();
            var customer = dbContext.TblCustomers.Find(items.CustomerId);
            if (customer==null)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = "Customer not found";
                logger.LogError("Customer with id : {items.CustomerId}, not found", items.CustomerId.ToString());
                return Ok(apiRes);
            }
            tblOrder.CustomerId = customer.CustomerId; 
            tblOrder.OrderAddress = items.OrderedAddress;
            dbContext.Add(tblOrder);
            dbContext.SaveChanges();
            //Save Items
            foreach (var prodId in items.SelectedProducts)
            {
                var product = dbContext.TblProducts.Find(prodId);
                if(product==null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "Product not found";
                    return Ok(apiRes);
                }
                TblOrderedItem tblOrderedItem = new TblOrderedItem();
                tblOrderedItem.OrderId = tblOrder.OrderId;
                tblOrderedItem.ProductId = product.ProductId;

                dbContext.Add(tblOrderedItem);
            }
            //Save all items
            apiRes.IsValid = true;
            apiRes.Result = dbContext.SaveChanges();
            logger.LogInformation("Information posted successfully " +
                "from Ordercontroller post method at {DT}",DateTime.UtcNow.ToString());
            return Ok(apiRes);
        }

        [HttpDelete]
        public IActionResult Delete(int orderId)
        {
            logger.LogInformation(" OrdersController,Delete method initiated");
            ApiResponse apiResp = new ApiResponse();
            
            ShopDBContext dbContext = new ShopDBContext();

            //Get Child Record and Delete them first
            var dbOrderedItems = dbContext.TblOrderedItems
                .Where(p => p.OrderId == orderId).ToList();
            if (dbOrderedItems != null && dbOrderedItems.Count > 0)
            {
                dbContext.RemoveRange(dbOrderedItems);
                var dbStatus = dbContext.SaveChanges();
                logger.LogInformation("Child records with id's {dbOrderedItems} " +
                    "of orderid {orderId} deleted", dbOrderedItems.ToString(), orderId.ToString());
                if (dbStatus > 0)
                {
                    //Get Master Record and Delete it now
                    var dbOrder = dbContext.TblOrders.Find(orderId);
                    dbContext.Remove(dbOrder);
                    var dbState = dbContext.SaveChanges();
                    apiResp.IsValid = true;
                    apiResp.StatusMessage = "Success";
                    logger.LogInformation("OrderId {orderId} successfully deleted at {DT}", orderId.ToString(),
                        DateTime.UtcNow.ToString());

                    return Ok(apiResp);
                }
            }
            apiResp.IsValid = false;
            apiResp.StatusMessage = "Failed";
            logger.LogError("Order id {orderId} to delete, not found", orderId.ToString());
            return Ok(apiResp);

        }

        [HttpPut]
        public IActionResult Put(TblOrder tblOrder)
        {
            logger.LogInformation("OrdersController put method initiated");
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            var dbOrder = dbContext.TblOrders.Find(tblOrder.OrderId);
            if (dbOrder == null) 
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = "Order not found";
                logger.LogError("OrderId with id {OrderId} not found in Ordertable", tblOrder.OrderId.ToString());
                return Ok(apiRes);
            }
            dbOrder.CustomerId = tblOrder.CustomerId;
            dbOrder.OrderAddress = tblOrder.OrderAddress;
            dbContext.Update(dbOrder);
            apiRes.IsValid = true;
            apiRes.Result = dbContext.SaveChanges();
            logger.LogInformation("OrderCOntroller put method successfully executed");
            return Ok(apiRes);
            //var dbState = dbContext.SaveChanges();
            //return Ok(dbState);

        }

    }


}