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
