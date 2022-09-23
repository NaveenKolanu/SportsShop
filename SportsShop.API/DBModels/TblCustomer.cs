using System;
using System.Collections.Generic;

#nullable disable

namespace SportsShop.API.Models
{
    public partial class TblCustomer
    {
        public TblCustomer()
        {
            TblOrders = new HashSet<TblOrder>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string CustomerEmailId { get; set; }
        public string CustomerAddress { get; set; }

        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
