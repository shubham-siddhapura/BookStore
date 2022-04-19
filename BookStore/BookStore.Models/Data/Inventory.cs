using System;
using System.Collections.Generic;

#nullable disable

namespace BookStore.Models.Data
{
    public partial class Inventory
    {
        public int InventoryId { get; set; }
        public int BookId { get; set; }
        public int Copies { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Book Book { get; set; }
    }
}
