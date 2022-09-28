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
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> logger;

        public CustomerController(ILogger<CustomerController> _logger)
        {
            logger = _logger;
        }

        [HttpGet]
        public IActionResult Get(int? customerId)
        {
            logger.LogInformation("Get customer by id is commenced " +
                "at {DT} with Id {customerId}:", DateTime.UtcNow.ToString(),customerId.ToString());
                //DateTime.UtcNow.ToLongTimeString());
            List<CustomerViewModel> vmCustomers = new List<CustomerViewModel>();
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            if (customerId > 0)
            {
                logger.LogInformation("Customer Id is given as :{customerId}", customerId);
                var dbCustomer = dbContext.TblCustomers.Find(customerId);

                if(dbCustomer==null)
                {
                    logger.LogError("Customer not found in dbContext with {customerId} : ", customerId.ToString());
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "invalid Customer Id";
                    return Ok(apiRes);
                }
                CustomerViewModel customerView = new CustomerViewModel();
                customerView.CustomerName = dbCustomer.CustomerName;
                customerView.CustomerEmailId = dbCustomer.CustomerEmailId;
                customerView.ContactNumber = dbCustomer.ContactNumber;
                customerView.CustomerAddress = dbCustomer.CustomerAddress;
               
                apiRes.IsValid = true;
                apiRes.Result=customerView;
                return Ok(apiRes);
            }
            var dbCustomers = dbContext.TblCustomers.ToList();
            foreach (var dbCustomer in dbCustomers)
            {
                CustomerViewModel customerView = new CustomerViewModel();
                customerView.CustomerName = dbCustomer.CustomerName;
                customerView.CustomerEmailId = dbCustomer.CustomerEmailId;
                customerView.ContactNumber = dbCustomer.ContactNumber;
                customerView.CustomerAddress = dbCustomer.CustomerAddress;
                vmCustomers.Add(customerView);
            }
            logger.LogInformation("Customer is found and returned with id {customerId}," +
                " at {DT}",customerId.ToString(),
                DateTime.UtcNow.ToString());
            apiRes.IsValid = true;
            apiRes.Result = vmCustomers;
            return Ok(apiRes);
        }

        [HttpPost]
        public IActionResult Post(CustomerViewModel vmCustomer )
        {
            logger.LogInformation("Post Method is initiated with info : {vmCustomer}", vmCustomer.ToString());
            ApiResponse apiRes = new ApiResponse();
            if (string.IsNullOrEmpty(vmCustomer.CustomerName) && 
                string.IsNullOrEmpty(vmCustomer.ContactNumber) &&
                string.IsNullOrEmpty(vmCustomer.CustomerAddress) &&
                string.IsNullOrEmpty(vmCustomer.CustomerEmailId))
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = "customer name, number, address," +
                    " emailid should not be empty";
                logger.LogError("Information is not entered, {ErrorMessage}",apiRes.ErrorMessage.ToString());
                return Ok(apiRes);
            }

            TblCustomer tblCustomer = new TblCustomer();
            tblCustomer.CustomerName = vmCustomer.CustomerName;
            tblCustomer.ContactNumber = vmCustomer.ContactNumber;
            tblCustomer.CustomerAddress = vmCustomer.CustomerAddress;
            tblCustomer.CustomerEmailId = vmCustomer.CustomerEmailId;
            ShopDBContext dbCustomer = new ShopDBContext();
            dbCustomer.Add(tblCustomer);
            var dbState = dbCustomer.SaveChanges();
            logger.LogInformation("Post successfully executed with information : {vmCustomer}", vmCustomer.ToString());
            if (dbState == 0)
            {
                logger.LogError("Entered wrong info, post didnt execute");
                apiRes.IsValid = false;
                apiRes.ErrorMessage = "enter valid details";
                return Ok(apiRes);
            }
            apiRes.IsValid = true;
            apiRes.Result = dbState;
            logger.LogInformation("Post executed succsfully and apiRes is returned");
            return Ok(apiRes);
           
        }

        [HttpDelete]
        public IActionResult Delete(int customerId)
        {
            logger.LogInformation("Deletemethod initiated with customerId : {customerId}",customerId.ToString());
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            if (customerId > 0)
            {
                var dbCustomer = dbContext.TblCustomers.Find(customerId);
                if (dbCustomer == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = $"{customerId}:Customer Id doesn't exist ";
                    logger.LogError("Customer Id doesn't exist,Delete did't execute " +
                        "with Id : {customerId}", customerId.ToString());
                    return Ok(apiRes);
                }
                dbContext.Remove(dbCustomer);
                apiRes.Result = dbContext.SaveChanges();
                apiRes.IsValid = true;
                logger.LogInformation("Deleted the customer succssfully " +
                    "with Id :{customerId}",customerId.ToString());
                return Ok(apiRes);
            }
            return Ok();
           
        }

        [HttpPut]
        public IActionResult Put(TblCustomer tblCustomer)
        {
            logger.LogInformation("Put method is initiated at {DT}", DateTime.UtcNow.ToString());
            ApiResponse apiRes = new ApiResponse();
            ShopDBContext dbContext = new ShopDBContext();
            var dbCustomer = dbContext.TblCustomers.Find(tblCustomer.CustomerId);
            //dbCustomer.CustomerName = tblCustomer.CustomerName;
            //dbCustomer.ContactNumber = tblCustomer.ContactNumber;
            //dbCustomer.CustomerEmailId = tblCustomer.CustomerEmailId;
            //dbCustomer.CustomerAddress= tblCustomer.CustomerAddress;
            if(dbCustomer==null)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = "Customer doesn't exist";
                logger.LogError("Customer dosen't exists with id : {CustomerId}", 
                    tblCustomer.CustomerId.ToString());
                return Ok(apiRes);
            }
            dbCustomer.CustomerName = tblCustomer.CustomerName;
            dbCustomer.ContactNumber = tblCustomer.ContactNumber;
            dbCustomer.CustomerEmailId = tblCustomer.CustomerEmailId;
            dbCustomer.CustomerAddress = tblCustomer.CustomerAddress;
            dbContext.Update(dbCustomer);
            apiRes.IsValid = true;
            apiRes.Result = dbContext.SaveChanges();
            logger.LogInformation("Customer deleted successfully with id : {customerId} and customer " +
                "name {tblCustomer.CustomerName}, at {DT}", 
                tblCustomer.CustomerId.ToString(), tblCustomer.CustomerName.ToString(), 
                DateTime.UtcNow.ToString());
            return Ok(apiRes);
        }
    }
}
