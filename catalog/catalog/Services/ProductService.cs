using System;
using AutoMapper;

namespace catalog.Services
{
    public interface IProductService
        {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Init();
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

        void IProductService.Init()
        {
            var p1 = new Product();
            p1.Name = "Name 01";
            p1.Description = "Description 01";
            var p2 = new Product();
            p2.Name = "Name 02";
            p2.Description = "Description 02";
            _context.Products.Add(p1);
            _context.Products.Add(p2);
            _context.SaveChanges();
        }

        Product IProductService.GetById(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            return product;
        }
    }
}

