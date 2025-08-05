namespace DotnetAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    
        public ICollection<Product> Products { get; set; }
    }
}
