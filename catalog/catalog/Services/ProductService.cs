using System;
using AutoMapper;

namespace catalog.Services
{
    public interface IProductService
        {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
    }
    public class ProductService : IProductService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public ProductService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        IEnumerable<Product> IProductService.GetAll()
        {
            return _context.Products;
        }

        Product IProductService.GetById(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            return product;
        }
    }
}

