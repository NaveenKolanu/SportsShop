using System;
using System.Collections.Generic;

#nullable disable

namespace SportsShop.API.Models
{
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblOrderedItems = new HashSet<TblOrderedItem>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string OrderAddress { get; set; }

        public virtual TblCustomer Customer { get; set; }
        public virtual ICollection<TblOrderedItem> TblOrderedItems { get; set; }
    }
}
