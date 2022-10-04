using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsShop.API.Controllers;
using SportsShop.API.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Results;

namespace SportsShop.UnitTest
{

    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void Test_Get_Request_Is_Returning_Data()
        {
            CustomerController controller = new CustomerController();
            var controllerResponse =
                controller.Get(null);
            var data = (OkObjectResult)controllerResponse;
            var api = data.Value as ApiResponse;
            var res = api.Result as List<CustomerViewModel>;
            Assert.IsNotNull(res);
           

        }


        [TestMethod]
        public void Test_Get_Customer()
        {
            //Setup
            CustomerController controller = new CustomerController();
            var controllerResponse =
                controller.Get(1);

            //Act
            var data = (OkObjectResult)controllerResponse;
            var api = data.Value as ApiResponse;
            var res = api.Result as CustomerViewModel;

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual("Ramya", res.CustomerName);

        }


        [TestMethod]
        public void Test_Invalid_Customer()
        {
            //Setup (prepare data/request/input)
            int customerId = 135;

            //Act (processing your rquest/input)
            CustomerController controller = new CustomerController();
            var controllerResponse =
                controller.Get(customerId);

            var data = (OkObjectResult)controllerResponse;
            var api = data.Value as ApiResponse;
            var res = api.Result as CustomerViewModel;

            //Assert (validating your data)
            Assert.IsNull(res);
            Assert.AreEqual(false, api.IsValid);
            Assert.AreEqual("invalid Customer Id", api.ErrorMessage);

        }
    }


}
