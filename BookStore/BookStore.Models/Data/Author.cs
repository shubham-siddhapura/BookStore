using System;
using System.Collections.Generic;

#nullable disable

namespace BookStore.Models.Data
{
    public partial class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
