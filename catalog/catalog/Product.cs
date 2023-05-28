using System;
using System.Data;

namespace catalog
{
	public class Product
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int price { get; set; }
        public int stock { get; set; }
    }
}

