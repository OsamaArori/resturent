using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApplication1.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public decimal Id { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
