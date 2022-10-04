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
        //public const string SuccessStatus = "Success";
        //public const string FailedStatus = "Failed";

        [HttpGet]
        public IActionResult Get(int? orderId)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                if (orderId > 0)
                {
                    //Get Order info by OrderId
                    var dbOrder = dbContext.TblOrders.Find(orderId);

                    //Get Customer info by CustomerId
                    var dbCustomer = dbContext.TblCustomers.Find(dbOrder.CustomerId);

                    //Get Ordered Items associated with OrderId
                    var dbOrderedItems = dbContext.TblOrderedItems.
                        Where(p => p.OrderId == orderId).ToList();

                    //Get Complete Order Info
                    OrderDetailsViewModel orderDeatails = new OrderDetailsViewModel();
                    List<ProductViewModel> itemsDetails = new List<ProductViewModel>();
                    orderDeatails.OrderedAddress = dbOrder.OrderAddress;
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
                    return Ok(apiRes);
                }
                else
                {
                    var dbOrders = dbContext.TblOrders.Include(p => p.Customer).ToList();
                    List<OrderDetailsViewModel> orders = new List<OrderDetailsViewModel>();
                    foreach (var order in dbOrders)
                    {
                        OrderDetailsViewModel orderDetails = new OrderDetailsViewModel();
                        orderDetails.OrderId = order.OrderId;
                        orderDetails.CustomerId = order.CustomerId;
                        orderDetails.CustomerName = order.Customer.CustomerName;
                        orderDetails.OrderedAddress = order.OrderAddress;
                        orders.Add(orderDetails);
                    }
                    apiRes.Result = orders;
                    apiRes.IsValid = true;
                    return Ok(apiRes);

                }
                apiRes.ErrorMessage = $"{orderId}:Record Not Found";
                apiRes.IsValid = false;

            }

            catch (Exception ex)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = ex.Message;
            }

            return Ok(apiRes);
        }

        [HttpPost]
        public IActionResult Post(SelectedItems items)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                TblOrder tblOrder = new TblOrder();
                var customer = dbContext.TblCustomers.Find(items.CustomerId);
                if (customer == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "Customer not found";
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
                    if (product == null)
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
            }
            catch (Exception ex)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = ex.Message;
            }
            return Ok(apiRes);
        }

        [HttpDelete]
        public IActionResult Delete(int orderId)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
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
                        apiRes.IsValid = true;
                        apiRes.StatusMessage = "Success";

                        return Ok(apiRes);
                    }
                }
                apiRes.IsValid = false;
                apiRes.StatusMessage = "Failed";
            }
            catch (Exception ex)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = ex.Message;
            }
            return Ok(apiRes);

        }

        [HttpPut]
        public IActionResult Put(TblOrder tblOrder)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                var dbOrder = dbContext.TblOrders.Find(tblOrder.OrderId);
                if (dbOrder == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "Order not found";
                    return Ok(apiRes);
                }
                dbOrder.CustomerId = tblOrder.CustomerId;
                dbOrder.OrderAddress = tblOrder.OrderAddress;
                dbContext.Update(dbOrder);
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