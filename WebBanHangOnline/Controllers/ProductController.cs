using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<int> getListId()
        {
            var list = db.Products.Select(p=>p.Id).ToList();
            return list;
        }
        // GET: Product
        public ActionResult Index(string SearchProduct)
        {
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(SearchProduct))
            {
                items = items.Where(x => x.Alias.Contains(SearchProduct) || x.Title.Contains(SearchProduct));
            }                     
            return View(items);
        }

        public ActionResult Detail(string alias,int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x=>x.ViewCount).IsModified=true;
                db.SaveChanges();
            }
            var countReview = db.reviewProducts.Where(x=>x.ProductId==id).Count();
            ViewBag.Count = countReview;
            ViewBag.CountRate = CountRate(id);
            ViewBag.CountRate2 = CountRate2(id);
            return View(item);
        }
        public double CountRate(int productId)
        {
            var items = db.reviewProducts.Where(x => x.ProductId == productId);
            if (items.Any())
            {             
                var n = items.Count();
                var totalRate = items.Sum(x => x.Rate);
                return (double)totalRate / n;
            }
            return 0;
        }
        public int CountRate2(int productId)
        {
            var items = db.reviewProducts.Where(x => x.ProductId == productId);
            if (items.Any())
            {
                var n = items.Count();
                var totalRate = items.Sum(x => x.Rate);
                return totalRate / n;
            }
            return 0;
        }
        public ActionResult ProductCategory(string alias,int? id)
        {

            var items = db.Products.ToList();
            if (id >0)
            {
                items = items.Where(x => x.ProductCategory.Id == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if(cate!=null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(items);
        }
        
        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.OrderByDescending(x=>x.Id).Where(x=>x.IsActive).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(10).ToList();
            return PartialView(items);
        }
        
    }
}