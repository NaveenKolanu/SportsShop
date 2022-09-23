using System;
using System.Collections.Generic;

#nullable disable

namespace SportsShop.API.Models
{
    public partial class TblProduct
    {
        public TblProduct()
        {
            TblOrderedItems = new HashSet<TblOrderedItem>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }

        public virtual ICollection<TblOrderedItem> TblOrderedItems { get; set; }
    }
}
