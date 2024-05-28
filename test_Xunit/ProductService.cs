using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace test_Xunit
{
    

    public class WebBanHangOnlineTestDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Cấu hình kết nối đến cơ sở dữ liệu
            optionsBuilder.UseSqlServer("Data Source=MSI\\MAYAO;Initial Catalog=WebBanHangOnline;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }

    public class ProductService
    {
        private readonly WebBanHangOnlineTestDbContext _context;

        public ProductService(WebBanHangOnlineTestDbContext context)
        {
            _context = context;
        }

        public List<int> GetProductIds()
        {
            return _context.Products.Select(p => p.Id).ToList();
        }
    }

   
}
