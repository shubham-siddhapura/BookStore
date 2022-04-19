using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels
{
    public class ChangePassword
    {
        public int id { get; set; }
        public string oldPwd { get; set; }
        public string newPwd { get; set; }
    }
}
