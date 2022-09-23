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
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(int? customerId)
        {
            ShopDBContext dbContext = new ShopDBContext();
            if (customerId > 0)
            {
                var record = dbContext.TblCustomers.Find(customerId);
                return Ok(record);
            }

            return Ok(dbContext.TblCustomers.ToList());
        }

        [HttpPost]
        public IActionResult Post(TblCustomer tblCustomer)
        {
            ShopDBContext dbCustomer = new ShopDBContext();
            dbCustomer.Add(tblCustomer);
            var dbState = dbCustomer.SaveChanges();
            return Ok(dbState);
        }

        [HttpDelete]
        public IActionResult Delete(int customerId)
        {
            ShopDBContext dbContext = new ShopDBContext();
            var dbCustomer = dbContext.TblCustomers.Find(customerId);
            dbContext.Remove(dbCustomer);
            var dbState = dbContext.SaveChanges();
            return Ok(dbState);
           
        }

        [HttpPut]
        public IActionResult Put(TblCustomer tblCustomer)
        {
            ShopDBContext dbContext = new ShopDBContext();
            var dbCustomer = dbContext.TblCustomers.Find(tblCustomer.CustomerId);
            dbCustomer.CustomerName = tblCustomer.CustomerName;
            dbCustomer.ContactNumber = tblCustomer.ContactNumber;
            dbCustomer.CustomerEmailId = tblCustomer.CustomerEmailId;
            dbCustomer.CustomerAddress= tblCustomer.CustomerAddress;

            dbContext.Update(dbCustomer);
            var dbState = dbContext.SaveChanges();

            return Ok(dbState);
        }
    }
}
