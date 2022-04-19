using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels
{
    public class AdminBook
    {

        public int BookId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Inventory { get; set; }
    }
}
