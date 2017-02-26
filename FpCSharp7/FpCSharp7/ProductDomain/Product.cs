using System;

namespace FpCSharp7.ProductDomain
{
    public class Product
    {
        public Product(string name) => Name = name;
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public int Version { get; set; } = 0;
    }
}
