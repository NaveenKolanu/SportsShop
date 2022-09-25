using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsShop.API.Models
{

    public enum StatusMessages
    {
        Success,
        Failed
    }

    //Models-> ViewModels / DomainModels / BussinessModels

    public class OrderDetailsViewModel
    { 
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<ProductViewModel> OrderedProducts { get; set; }
    }

    public class ProductViewModel
    { 
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
    }


    public class SelectedItems
    {
        public int CustomerId { get; set; }
        public string OrderedAddress { get; set; }
        public List<int> SelectedProducts { get; set; }
    }

    public class ApiResponse
    {
        public bool IsValid { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}
