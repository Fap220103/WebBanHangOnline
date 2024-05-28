using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Contact
        public ActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Contact> items = db.Contacts.OrderByDescending(p => p.Id);
           
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult View(int id)
        {
            var item = db.Contacts.Find(id);
            return View(item);
        }
        [HttpPost]
        public ActionResult UpdateTT(int id, bool trangthai)
        {
            var item = db.Contacts.Find(id);
            if (item != null)
            {
                db.Contacts.Attach(item);
                item.IsRead = trangthai;

                db.Entry(item).Property(x => x.IsRead).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "UnSuccess", Success = false });
        }
    }
}