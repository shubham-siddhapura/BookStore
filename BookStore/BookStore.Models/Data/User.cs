using System;
using System.Collections.Generic;

#nullable disable

namespace BookStore.Models.Data
{
    public partial class User
    {
        public User()
        {
            BookAuthors = new HashSet<Book>();
            BookPublishers = new HashSet<Book>();
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Book> BookAuthors { get; set; }
        public virtual ICollection<Book> BookPublishers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
