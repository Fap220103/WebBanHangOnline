using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
   
    public class KiemthuController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Kiemthu
        public List<int> Get()
        {
            return db.Products.Select(p => p.Id).ToList();
        }
    }
}