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
            ApiResponse apiRes = new ApiResponse();
            try
            {
                List<CustomerViewModel> vmCustomers = new List<CustomerViewModel>();
                ShopDBContext dbContext = new ShopDBContext();
                if (customerId > 0)
                {

                    var dbCustomer = dbContext.TblCustomers.Find(customerId);

                    if (dbCustomer == null)
                    {
                        apiRes.IsValid = false;
                        apiRes.ErrorMessage = "invalid Customer Id";
                        return Ok(apiRes);
                    }
                    CustomerViewModel customerView = new CustomerViewModel();
                    customerView.CustomerId = dbCustomer.CustomerId;
                    customerView.CustomerName = dbCustomer.CustomerName;
                    customerView.CustomerEmailId = dbCustomer.CustomerEmailId;
                    customerView.ContactNumber = dbCustomer.ContactNumber;
                    customerView.CustomerAddress = dbCustomer.CustomerAddress;

                    apiRes.IsValid = true;
                    apiRes.Result = customerView;
                    return Ok(apiRes);
                }
                var dbCustomers = dbContext.TblCustomers.ToList();
                foreach (var dbCustomer in dbCustomers)
                {
                    CustomerViewModel customerView = new CustomerViewModel();
                    customerView.CustomerId = dbCustomer.CustomerId;
                    customerView.CustomerName = dbCustomer.CustomerName;
                    customerView.CustomerEmailId = dbCustomer.CustomerEmailId;
                    customerView.ContactNumber = dbCustomer.ContactNumber;
                    customerView.CustomerAddress = dbCustomer.CustomerAddress;
                    vmCustomers.Add(customerView);
                }
                apiRes.IsValid = true;
                apiRes.Result = vmCustomers;
            }
            catch (Exception ex)
            {
                apiRes.IsValid = false;
                apiRes.ErrorMessage = ex.Message;
            }
            return Ok(apiRes);
        }

        [HttpPost]

        public IActionResult Post(CustomerViewModel vmCustomer)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbCustomer = new ShopDBContext();
                if (string.IsNullOrEmpty(vmCustomer.CustomerName) &&
                    string.IsNullOrEmpty(vmCustomer.ContactNumber) &&
                    string.IsNullOrEmpty(vmCustomer.CustomerAddress) &&
                    string.IsNullOrEmpty(vmCustomer.CustomerEmailId))
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "customer name, number, address," +
                        " emailid should not be empty";
                    return Ok(apiRes);
                }

                TblCustomer tblCustomer = new TblCustomer();
                tblCustomer.CustomerName = vmCustomer.CustomerName;
                tblCustomer.ContactNumber = vmCustomer.ContactNumber;
                tblCustomer.CustomerAddress = vmCustomer.CustomerAddress;
                tblCustomer.CustomerEmailId = vmCustomer.CustomerEmailId;

                dbCustomer.Add(tblCustomer);
                var dbState = dbCustomer.SaveChanges();
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

        public IActionResult Delete(int customerId)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                CustomerViewModel customerView = new CustomerViewModel();
                if (customerId > 0)
                {
                    var dbCustomer = dbContext.TblCustomers.Find(customerId);
                    if (dbCustomer == null)
                    {
                        apiRes.IsValid = false;
                        apiRes.ErrorMessage = $"{customerId}:Customer Id doesn't exist ";
                        return Ok(apiRes);
                    }

                    customerView.CustomerId = dbCustomer.CustomerId;
                    customerView.CustomerName = dbCustomer.CustomerName;
                    customerView.CustomerEmailId = dbCustomer.CustomerEmailId;
                    customerView.ContactNumber = dbCustomer.ContactNumber;
                    customerView.CustomerAddress = dbCustomer.CustomerAddress;
                    dbContext.Remove(customerView);

                    apiRes.IsValid = true;
                    apiRes.Result = dbContext.SaveChanges();

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

        public IActionResult Put(CustomerViewModel vmCustomer)
        {
            ApiResponse apiRes = new ApiResponse();
            try
            {
                ShopDBContext dbContext = new ShopDBContext();
                var dbCustomer = dbContext.TblCustomers.Find(vmCustomer.CustomerId);

                if (dbCustomer == null)
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "Customer doesn't exist";
                    return Ok(apiRes);
                }
                if (string.IsNullOrEmpty(vmCustomer.CustomerName) &&
                   string.IsNullOrEmpty(vmCustomer.ContactNumber) &&
                   string.IsNullOrEmpty(vmCustomer.CustomerAddress) &&
                   string.IsNullOrEmpty(vmCustomer.CustomerEmailId))
                {
                    apiRes.IsValid = false;
                    apiRes.ErrorMessage = "customer name, number, address," +
                        " emailid should not be empty";
                    return Ok(apiRes);
                }
                dbCustomer.CustomerName = vmCustomer.CustomerName;
                dbCustomer.ContactNumber = vmCustomer.ContactNumber;
                dbCustomer.CustomerEmailId = vmCustomer.CustomerEmailId;
                dbCustomer.CustomerAddress = vmCustomer.CustomerAddress;
                dbContext.Update(dbCustomer);
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
