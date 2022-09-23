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
        [HttpGet]
        public IActionResult Get(int orderId)
        {
            ShopDBContext dbContext = new ShopDBContext();
            if (orderId > 0)
            {
                var record = dbContext.TblOrders.Find(orderId);
                return Ok(record);
            }
            var res = dbContext.TblOrders.Include(p => p.Customer).Include(p => p.TblOrderedItems).ToList();
            return Ok(res);
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
            ShopDBContext dbContext = new ShopDBContext();
            var dbOrder = dbContext.TblOrders.Find(orderId);
            dbContext.Remove(orderId);
            var dbState = dbContext.SaveChanges();
            return Ok(dbState);
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

        public class SelectedItems
        {
            public int CustomerId { get; set; }
            public string OrderedAddress { get; set; }
            public List<int> SelectedProducts { get; set; }
        }
    }
}