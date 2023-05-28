using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace catalog
{
    public class Product
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
    }
}

