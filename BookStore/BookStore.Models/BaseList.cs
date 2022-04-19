using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models
{
    public class BaseList<T> where T : class
    {
        public List<T> Records { get; set; }
        public int TotalRecords { get; set; }

    }
}
