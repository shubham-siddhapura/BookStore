using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels
{
    public class CancelOrder
    {
        public int OrderId { get; set; }

        public string Password { get; set; }
    }
}
