using Bogus;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        public ActionResult Index()
        {
            ViewBag.Title = "Shop Online";
            return View();
        }
        public ActionResult Partial_Sub()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe(Subscribe req)
        {
            if (ModelState.IsValid)
            {
                db.Subscribes.Add(new Subscribe { Email = req.Email, CreatedDate = DateTime.Now });
                db.SaveChanges();
                return Json(new { Success = true });
            }
            return View("Partial_Sub");
        }
        //cach 2 dung ajax
        //[HttpPost]
        //public ActionResult Subscribe(string email)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Subscribes.Add(new Subscribe { Email = email, CreatedDate = DateTime.Now });
        //        db.SaveChanges();
        //        return Json(new { Success = true });
        //    }
        //    return View("Partial_Sub");
        //}
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Refresh()
        {
            var item = new ThongKeModel();
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
            item.HomNay =HttpContext.Application["HomNay"].ToString();
            item.HomQua = HttpContext.Application["HomQua"].ToString();
            item.TuanNay = HttpContext.Application["TuanNay"].ToString();
            item.TuanTruoc = HttpContext.Application["TuanTruoc"].ToString();
            item.ThangNay = HttpContext.Application["ThangNay"].ToString();
            item.ThangTruoc = HttpContext.Application["ThangTruoc"].ToString();
            item.TatCa = HttpContext.Application["TatCa"].ToString();
            return PartialView(item);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult GenerateFakeData(int? page)
        {

            var faker = new Bogus.Faker("vi");
            // Tạo dữ liệu giả mạo cho một số thuộc tính cần thiết
            var fakeDataList = new List<FakeData>();
            for (int i = 0; i < 10; i++)
            {
                var product = new Faker<FakeData>()

                .RuleFor(p => p.FullName, f => f.Commerce.ProductName())
                 .RuleFor(p => p.Email, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Address, f => f.Commerce.ProductName())

                .RuleFor(p => p.ImageUrl, f => $"https://picsum.photos/seed/{f.Random.Guid()}/200/300")
                .Generate();


                fakeDataList.Add(product);
            }
            //ViewBag.FakeDataList = fakeDataList;
            //// Truyền dữ liệu giả mạo đến view để hiển thị
            //return View(fakeDataList);
            var pageSize = 100;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<FakeData> items = fakeDataList.ToList();
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
    }
}