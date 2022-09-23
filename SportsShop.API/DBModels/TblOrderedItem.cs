using System;
using System.Collections.Generic;

#nullable disable

namespace SportsShop.API.Models
{
    public partial class TblOrderedItem
    {
        public int OrderedItemsId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public virtual TblOrder Order { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
