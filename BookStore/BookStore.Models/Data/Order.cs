using System;
using System.Collections.Generic;

#nullable disable

namespace BookStore.Models.Data
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int OrderBy { get; set; }
        public int BookId { get; set; }
        public int Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DispatchDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Book Book { get; set; }
        public virtual User OrderByNavigation { get; set; }
        public virtual Status StatusNavigation { get; set; }
    }
}
